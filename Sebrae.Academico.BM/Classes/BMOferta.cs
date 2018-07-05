using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.BM.Mapeamentos.Procedures;
using Sebrae.Academico.Dominio.DTO;

namespace Sebrae.Academico.BM.Classes
{
    public class BMOferta : BusinessManagerBase, IDisposable
    {

        private RepositorioBase<Oferta> repositorio;


        public BMOferta()
        {
            repositorio = new RepositorioBase<Oferta>();

        }

        public void EvictOferta(Oferta pOferta)
        {
            this.repositorio.Evict(pOferta);
        }

        public void ValidarOfertaInformada(Oferta pOferta)
        {
            this.ValidarInstancia(pOferta);

            //if (string.IsNullOrWhiteSpace(pOferta.Nome)) throw new AcademicoException("Nome. Campo Obrigatório.");

            if ((int)pOferta.TipoOferta == 0) throw new AcademicoException("Tipo de Oferta. Campo Obrigatório.");

            if (pOferta.SolucaoEducacional == null) throw new AcademicoException("Solução Educacional. Campo Obrigatório.");

            if (pOferta.InscricaoOnline == null) throw new AcademicoException("Inscrição Online. Campo Obrigatório.");

            if ((int)pOferta.CargaHoraria == 0) throw new AcademicoException("Carga Horária. Campo Obrigatório.");

            if (pOferta.DiasPrazo == null && pOferta.TipoOferta == enumTipoOferta.Continua) throw new AcademicoException("Dias de Prazo: Campo obrigatório.");

            this.AplicarRegraParaTipoOferta(pOferta);
        }

        private void AplicarRegraParaTipoOferta(Oferta pOferta)
        {
            if (pOferta.TipoOferta != enumTipoOferta.Continua)
            {
                //Data Fim das Inscrições
                if (!pOferta.DataFimInscricoes.HasValue)
                    throw new AcademicoException("Data fim das inscrições é obrigatória para esse tipo de oferta.");
                
                ValidarLimiteEntreDatasInicioDeInscricaoEDataFimDeInscricao(pOferta);

            }

            if (!pOferta.TipoOferta.Equals(enumTipoOferta.Exclusiva))
            {
                if (pOferta.QuantidadeMaximaInscricoes.Equals(0))
                {
                    throw new AcademicoException("Quantidade máxima de inscrições. Campo obrigatório.");
                }
            }
        }

        private void ValidarLimiteEntreDatasInicioDeInscricaoEDataFimDeInscricao(Oferta pOferta)
        {
            if (pOferta.DataInicioInscricoes.HasValue && pOferta.DataFimInscricoes.HasValue)
            {
                if (pOferta.DataInicioInscricoes > pOferta.DataFimInscricoes)
                {
                    throw new AcademicoException(string.Format("A Data de Início das Inscrições '{0}' deve ser menor que a Data Fim das Inscrições '{1}'",
                                                                pOferta.DataInicioInscricoes.Value.ToShortDateString(), pOferta.DataFimInscricoes.Value.ToShortDateString()));
                }
            }
        }

        private void VerificarChaveExterna(Oferta pOferta)
        {
            bool idChaveExternaJaExiste = VerificarSeChaveExternaExiste(pOferta);

            if (idChaveExternaJaExiste)
            {
                throw new AcademicoException(string.Format("A Chave Externa '{0}' já está cadastrada para a Solução Educacional '{1}'",
                         pOferta.IDChaveExterna, pOferta.SolucaoEducacional.Nome));
            }
        }

        /// <summary>
        /// Verifica se uma chave externa já está cadastrada para uma mesma Solução Educacional.
        /// </summary>
        /// <param name="pOferta">Dados de uma oferta</param>
        /// <returns>Retorna true se já existir uma chave externa para uma mesma solução educacional.
        /// Retorna false senão existir uma chave externa para uma mesma solução educacional.</returns>
        private bool VerificarSeChaveExternaExiste(Oferta pOferta)
        {
            bool chaveExternaExiste = false;

            var query = repositorio.session.Query<Oferta>();
            int qtd = 0;
            if (pOferta != null && !string.IsNullOrEmpty(pOferta.IDChaveExterna))
            {
                qtd = query.Where(x => x.IDChaveExterna.Trim().ToUpper() == pOferta.IDChaveExterna.Trim().ToUpper()
                                      && pOferta.SolucaoEducacional.ID == x.SolucaoEducacional.ID).Count();
            }
            else
            {
                qtd = query.Where(x => x.IDChaveExterna == null
                                      && pOferta.SolucaoEducacional.ID == x.SolucaoEducacional.ID).Count();
            }
            

            if (qtd > 0)
            {
                chaveExternaExiste = true;
            }
            //&& x.SolucaoEducacional != null
            //&& x.SolucaoEducacional.ID == pOferta.SolucaoEducacional.ID);

            return chaveExternaExiste;
        }

        public void Salvar(Oferta pOferta, bool validar = true)
        {
            if(validar)
                ValidarOfertaInformada(pOferta);

            repositorio.Salvar(pOferta);
        }

        public int? ObterProximoCodigoSequencial(SolucaoEducacional solucao)
        {
            if (solucao == null)
                return null;

            var max = repositorio.session.Query<Oferta>()
                .Where(x => x.SolucaoEducacional.ID == solucao.ID)
                .Max(x => x.Sequencia);

            if (max.HasValue)
                return max.Value + 1;
            else
                return 1;
        }

        public bool AlterouSolucaoEducacional(int idOferta, SolucaoEducacional novaSolucao)
        {
            var oferta = repositorio.session.Query<Oferta>().First(s => s.ID == idOferta);

            if (oferta.SolucaoEducacional == null)
                return novaSolucao != null;
            else
                return novaSolucao == null || oferta.SolucaoEducacional.ID != novaSolucao.ID;
        }

        public void ExcluirOferta(Oferta pOferta)
        {
            if (this.ValidarDependencias(pOferta))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes desta Oferta.");

            repositorio.Excluir(pOferta);
        }


        protected override bool ValidarDependencias(object pObject)
        {
            var pOferta = (Oferta)pObject;
            if (pOferta.ListaMatriculaOferta != null && pOferta.ListaMatriculaOferta.Count > 0) return true;
            return pOferta.ListaTurma != null && pOferta.ListaTurma.Count > 0;
        }

        public IList<Oferta> ObterPorFiltro(string nome, string idChaveExterna, int idSolucaoEducacional)
        {
            return ObterPorFiltro(nome, idChaveExterna, idSolucaoEducacional, 0);
        }

        public IList<Oferta> ObterPorFiltro(string nome, string idChaveExterna, int idSolucaoEducacional, int ufPermitida)
        {
            var query = repositorio.session.Query<Oferta>();


            if (!string.IsNullOrWhiteSpace(nome))
                query = query.Where(x => x.Nome.ToUpper().Contains(nome.ToUpper())).AsQueryable();
                /*
                 * Ao executar o "ToList()" você força o NHibernate a executar a query, neste momento ela deixa de ser deferred. 
                 */
                //query = query.ToList().Where(x => x.Nome.ToUpper().Contains(nome.ToUpper())).AsQueryable();

            if (!string.IsNullOrWhiteSpace(idChaveExterna))
                query = query.Where(x => x.IDChaveExterna == idChaveExterna);
            if (idSolucaoEducacional > 0)
                query = query.Where(x => x.SolucaoEducacional.ID == idSolucaoEducacional);

            if (ufPermitida > 0)
                query = query.Where(x => x.ListaPermissao.Any(p => p.Uf.ID == ufPermitida));

            return query.ToList();
        }

        public IQueryable<Oferta> ObterTodos()
        {
            return repositorio.session.Query<Oferta>().Fetch(x => x.ListaTurma);
        }

        public Oferta ObterPorId(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public IQueryable<Oferta> ObterOfertaPorSolucaoEducacional(SolucaoEducacional solucaoEducacional)
        {
            var query = repositorio.session.Query<Oferta>();

            return query.Where(x => x.SolucaoEducacional.ID == solucaoEducacional.ID);
        }

        public IList<Oferta> ConsultarOfertasPorPeriodo(DateTime dataInicio, DateTime dataFim)
        {
            var query = repositorio.session.Query<Oferta>();

            query =
                query.Where(
                    x =>
                        x.ListaTurma.Any(
                            t =>
                                t.AcessoWifi.HasValue && t.DataInicio >= dataInicio &&
                                t.DataFinal.HasValue && t.DataFinal.Value <= dataFim));

            return query.ToList();
        }

        public IQueryable<Oferta> ObterTodasIQueryable()
        {
            return repositorio.session.Query<Oferta>();
        }

        public IList<Oferta> ObterOfertaPorSolucaoEducacionalQuePossuiQuestionario(int idSolucaoEducacional)
        {
            var query = repositorio.session.Query<Oferta>();

            query = query.Where(x => x.SolucaoEducacional.ID == idSolucaoEducacional && x.ListaTurma.Any(t => t.ListaQuestionarioAssociacao.Any())).Select(x => new Oferta() { ID = x.ID  });
                        
            return query.ToList<Oferta>();
        }

        public Oferta ObterOfertaPorFornecedor(string loginFornecedor, string idChaveExternaOferta)
        {

            var query = repositorio.session.Query<Oferta>();
            if (string.IsNullOrEmpty(idChaveExternaOferta))
                query = query.Where(x => x.IDChaveExterna == null);
            else
                query = query.Where(x => x.IDChaveExterna == idChaveExternaOferta);
            query = query.Where(x => x.SolucaoEducacional.Fornecedor.Login == loginFornecedor);

            return query.FirstOrDefault();
        }

        public Oferta ObterOfertaPorFornecedor(IList<int> idsOfertas, string loginFornecedor, string idChaveExternaOferta, string idChaveExternaSolucaoEducacional)
        {
            var query = repositorio.session.Query<Oferta>();

            query = string.IsNullOrEmpty(idChaveExternaOferta) ? query.Where(x => x.IDChaveExterna == null) : query.Where(x => x.IDChaveExterna == idChaveExternaOferta);

            if (idsOfertas != null){
                query = query.Where(x => !idsOfertas.Contains(x.ID));
            }

            query = query.Where(x => x.SolucaoEducacional.Fornecedor.Login == loginFornecedor);
            query = query.Where(x => x.SolucaoEducacional.IDChaveExterna == idChaveExternaSolucaoEducacional);

            //MANTER O ORDER BY PARA PEGAR A MESMA PENDENCIA (CHAVEEXTERNA NULA) NA CONSULTA E NO MANTER
            return query.OrderBy(x => x.ID).FirstOrDefault();

        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public IList<DTOfertaPermissao> ObterListaDePermissoes(int idUsuario)
        {
            var procOfertaPermissao = new ProcOfertaPermissao();
            return procOfertaPermissao.Executar(idUsuario);
        }

        public Oferta ObterPorTurma(int idTurma)
        {
            var query = repositorio.session.Query<Turma>();
            Turma turma = query.FirstOrDefault(x => x.ID == idTurma);
            Oferta oferta = null;

            if (turma != null)
            {
                oferta = turma.Oferta;
            }

            return oferta;
        }

        public IList<Oferta> ObtertasPorUf(int idUF)
        {
            var query = repositorio.session.Query<Oferta>();

            if (idUF > 0)
                query = query.Where(x=>x.ListaPermissao.Any(p=>p.Uf != null || p.Uf.ID == idUF));

            return query.ToList<Oferta>();
        }

        public IQueryable<Oferta> ObterPorTipoPublicoAlvo(List<int> publicosAlvo)
        {
            return repositorio.session.Query<Oferta>()
                .Fetch(x => x.ListaPublicoAlvo)
                .Where(o => o.ListaPublicoAlvo.Any(p => publicosAlvo.Contains(p.PublicoAlvo.Tipo)));
        }
    }
}

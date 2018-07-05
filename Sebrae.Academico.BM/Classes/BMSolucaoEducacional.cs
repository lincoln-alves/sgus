using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.BM.Mapeamentos.Procedures;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio;
using FluentNHibernate.Conventions;

namespace Sebrae.Academico.BM.Classes
{
    public class BMSolucaoEducacional : BusinessManagerBase, IDisposable
    {
        #region Atributos Privados

        private RepositorioBase<SolucaoEducacional> repositorio;

        #endregion

        #region "Construtor"

        public BMSolucaoEducacional()
        {
            repositorio = new RepositorioBase<SolucaoEducacional>();
        }

        #endregion

        public SolucaoEducacional ObterPorId(int ID)
        {
            return repositorio.ObterPorID(ID);
        }

        public IList<SolucaoEducacional> BuscarporNome(SolucaoEducacional pSolucaoEducacional)
        {
            // return repositorio.GetByProperty("Nome", pSolucaoEducacional.Nome);
            var query = repositorio.session.Query<SolucaoEducacional>();
            return query.Where(x => x.ID == pSolucaoEducacional.ID).ToList<SolucaoEducacional>();
        }

        public void Salvar(SolucaoEducacional pSolucaoEducacional, bool validar = true)
        {
            if (validar)
                ValidarSolucaoEducacionalInformado(pSolucaoEducacional);

            repositorio.Salvar(pSolucaoEducacional);
        }

        public void GerarSequencias()
        {
            foreach (var solucao in repositorio.session.Query<SolucaoEducacional>().Where(s => s.Sequencia == null && s.CategoriaConteudo != null))
            {
                solucao.Sequencia = ObterProximoCodigoSequencial(solucao.CategoriaConteudo);
                repositorio.Salvar(solucao);
            }

            var bmOferta = new BMOferta();

            foreach (var oferta in repositorio.session.Query<Oferta>().Where(o => o.Sequencia == null && o.SolucaoEducacional != null && o.SolucaoEducacional.CategoriaConteudo != null))
            {
                oferta.Sequencia = bmOferta.ObterProximoCodigoSequencial(oferta.SolucaoEducacional);
                bmOferta.Salvar(oferta);
            }

            var bmTurma = new BMTurma();

            foreach (var turma in repositorio.session.Query<Turma>().Where(t => t.Sequencia == null && t.Oferta != null && t.Oferta.SolucaoEducacional != null && t.Oferta.SolucaoEducacional.CategoriaConteudo != null))
            {
                turma.Sequencia = bmTurma.ObterProximoCodigoSequencial(turma.Oferta);
                bmTurma.Salvar(turma);
            }

        }

        public void ValidarPreRequisitosDaMatricula(MatriculaOferta pMatriculaOferta, int idTurma = 0)
        {
            if (pMatriculaOferta.Oferta == null)
                throw new AcademicoException("A matrícula não está vinculada adequadamente a uma Oferta. Tente novamente."); ;

            if (pMatriculaOferta.Oferta.SolucaoEducacional == null)
                pMatriculaOferta.Oferta.SolucaoEducacional = ObterPorId(new BMOferta().ObterPorId(pMatriculaOferta.Oferta.ID).SolucaoEducacional.ID);

            ValidarVagasPorUf(pMatriculaOferta);

            if (idTurma > 0)
            {
                //Quantidade Máxima de Inscrições
                var turma = new BMTurma().ObterPorID(idTurma);

                var maxInscricoesOferta = new BMOferta().ObterPorId(pMatriculaOferta.Oferta.ID).QuantidadeMaximaInscricoes;

                if (turma.QuantidadeMaximaInscricoes > maxInscricoesOferta)
                {
                    throw new AcademicoException("A quantidade de inscrições da turma, excede o máximo permitido para a oferta.");
                }
            }

            if (!pMatriculaOferta.Oferta.SolucaoEducacional.ListaPreRequisito.Any()) return;

            var aprovados = new List<enumStatusMatricula>
            {
                enumStatusMatricula.Aprovado,
                enumStatusMatricula.Concluido
            };

            var oferta = pMatriculaOferta.Oferta;

            foreach (var item in pMatriculaOferta.Oferta.SolucaoEducacional.ListaPreRequisito)
            {
                var matriculaOferta = new BMMatriculaOferta().ObterPorUsuarioESolucaoEducacional(pMatriculaOferta.Usuario.ID, item.PreRequisito.ID).ToList();

                if (matriculaOferta != null && matriculaOferta.Any())
                {
                    var lastOrDefault = matriculaOferta.LastOrDefault();

                    if (lastOrDefault != null &&
                        (oferta.SolucaoEducacional.ID == item.SolucaoEducacional.ID &&
                         !aprovados.Contains(lastOrDefault.StatusMatricula)))
                    {
                        throw new AcademicoException("Erro: Existem soluções como pré-requisito que não estão cursadas");
                    }
                }
            }
        }

        public void ValidarVagasPorUf(MatriculaOferta matriculaOferta)
        {
            var oferta = new BMOferta().ObterPorId(matriculaOferta.Oferta.ID);
            var quantidadeDeMatriculas = oferta.ListaMatriculaOferta.Count(x => x.StatusMatricula.IsNotAny(enumStatusMatricula.CanceladoAdm, enumStatusMatricula.CanceladoGestor, enumStatusMatricula.CanceladoAluno) && x.UF.Sigla == matriculaOferta.UF.Sigla && x.Usuario.ID != matriculaOferta.Usuario.ID);

            if (oferta.ListaPermissao.Count(
                l =>
                    l.Uf != null && (l.Uf.Nome == matriculaOferta.UF.Nome && l.Uf.Regiao.SiglaRegiao == matriculaOferta.UF.Regiao.SiglaRegiao &&
                    l.QuantidadeVagasPorEstado > 0 && quantidadeDeMatriculas >= l.QuantidadeVagasPorEstado)) > 0 && !oferta.FiladeEspera)
            {
                throw new AcademicoException("Erro: Quantidade máxima de inscritos por UF atingida");
            }

        }

        public int? ObterProximoCodigoSequencial(CategoriaConteudo categoria)
        {
            if (categoria == null)
                return null;

            var max = repositorio.session.Query<SolucaoEducacional>()
                .Where(x => x.CategoriaConteudo.ID == categoria.ID)
                .Max(x => x.Sequencia);

            if (max.HasValue)
                return max.Value + 1;
            else
                return 1;
        }

        public bool AlterouCategoria(int idSolucaoEducacional, CategoriaConteudo novaCategoria)
        {
            var solucao = repositorio.session.Query<SolucaoEducacional>().First(s => s.ID == idSolucaoEducacional);

            if (solucao.CategoriaConteudo == null)
                return novaCategoria != null;

            return novaCategoria == null || solucao.CategoriaConteudo.ID != novaCategoria.ID;
        }

        public IQueryable<SolucaoEducacional> ObterTodos(bool somenteAtivos = true, bool ordenarPorNome = true)
        {
            var query = repositorio.session.Query<SolucaoEducacional>();

            if (somenteAtivos)
                query = query.Where(s => !somenteAtivos || s.Ativo);

            if (ordenarPorNome)
                query = query.OrderBy(s => s.Nome);

            return query;
        }

        public IEnumerable<SolucaoEducacional> ObterListTodos(bool somenteAtivos = true, bool ordenarPorNome = true)
        {
            var query = repositorio.session.Query<SolucaoEducacional>();

            if (somenteAtivos)
                query = query.Where(s => !somenteAtivos || s.Ativo);

            if (ordenarPorNome)
                query = query.OrderBy(s => s.Nome);

            return query.ToList();
        }

        public IQueryable<SolucaoEducacional> ObterComQuestionario(int? idQuestionario = null)
        {
            var retorno = repositorio.session.Query<SolucaoEducacional>();

            if (idQuestionario.HasValue && idQuestionario > 0)
                retorno = retorno.Where(x => x.ListaOferta
                    .Any(o => o.ListaTurma
                        .Any(t => t.ListaQuestionarioAssociacao.Any(q => q.Questionario.ID == idQuestionario.Value) ||
                                  t.ListaQuestionarioParticipacao.Any(q => q.Questionario.ID == idQuestionario.Value))));
            else
                retorno = retorno.Where(x => x.ListaOferta
                   .Any(o => o.ListaTurma
                       .Any(t => t.ListaQuestionarioAssociacao.Any())));

            return retorno.OrderBy(x => x.Nome);
        }

        public IList<SolucaoEducacional> ObterPorPermissaoUF(int ufPermitida)
        {
            //return repositorio.ObterTodos().OrderBy(x => x.Nome).ToList<SolucaoEducacional>();
            var query = repositorio.session.Query<SolucaoEducacional>();
            return query.Where(x => x.ListaPermissao.Any(l => l.Uf.ID == ufPermitida)).OrderBy(x => x.Nome).ToList();
            #region Código refatorado acima
            //IList<SolucaoEducacional> listaSolucaoEducacional = query.Where(x=>x.ListaPermissao.Any(l=>l.Uf.ID == ufPermitida)).OrderBy(x => x.Nome).ToList();
            //return listaSolucaoEducacional;
            #endregion
        }

        public void Excluir(SolucaoEducacional pSolucaoEducacional)
        {
            if (ValidarDependencias(pSolucaoEducacional))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes desta Solução Educacional.");

            repositorio.Excluir(pSolucaoEducacional);
        }

        /// <summary>
        /// Verifica se a classe Solução Educacional possui dependências, ou seja, se possui registros filhos de entidades gerenciadas.
        /// </summary>
        /// <param name="pSolucaoEducacional">Objeto da Classe SolucaoEducacional</param>
        /// <observacoes>Este método normamente é codificado antes da exclusão de um objeto, pois por default, não são permitidas
        /// exclusões em cascata de entidades gerenciadas. (delete cascade )</observacoes>
        /// <returns>Retorna True, caso a classe Solução Educacional possua dependências, ou seja, registros filhos.
        /// Retorna False, caso a classe Solução Educacional não possua dependências, ou seja, não possua registros filhos. </returns>
        protected override bool ValidarDependencias(object pSolucaoEducacional)
        {
            SolucaoEducacional solucaoEducacional = (SolucaoEducacional)pSolucaoEducacional;

            return ((solucaoEducacional.ListaItemTrilha != null && solucaoEducacional.ListaItemTrilha.Count > 0) ||
                    (solucaoEducacional.ListaOferta != null && solucaoEducacional.ListaOferta.Any()) ||
                    (solucaoEducacional.ListaProgramaSolucaoEducacional != null && solucaoEducacional.ListaProgramaSolucaoEducacional.Count > 0)
                    );

        }

        public IList<SolucaoEducacional> ObterPorFormaAquisicao(int IdFormaAquisicao)
        {
            //return repositorio.GetByProperty("FormaAquisicao.ID", IdFormaAquisicao);
            var query = repositorio.session.Query<SolucaoEducacional>();
            return query.Where(x => x.FormaAquisicao.ID == IdFormaAquisicao).ToList<SolucaoEducacional>();
        }

        /// <summary>
        /// Retorna as soluções educacionais filtradas pela forma de aquisição e uf, caso não informe o filtro retorna todas.
        /// </summary>
        /// <param name="IdFormaAquisicao"></param>
        /// <param name="uf"></param>
        /// <returns></returns>
        public IQueryable<SolucaoEducacional> ObterPorFormaAquisicaoUf(int IdFormaAquisicao = 0, Uf uf = null)
        {
            var query = repositorio.session.Query<SolucaoEducacional>();
            query = IdFormaAquisicao > 0 ? query.Where(x => x.FormaAquisicao.ID == IdFormaAquisicao) : query;
            query = uf != null ? query.Where(x => x.UFGestor.ID == uf.ID) : query;
            return query;
        }

        private void ValidarSolucaoEducacionalInformado(SolucaoEducacional pSolucaoEducacional)
        {
            ValidarInstancia(pSolucaoEducacional);

            //Nome da Solução Educacional
            if (string.IsNullOrWhiteSpace(pSolucaoEducacional.Nome)) throw new AcademicoException("Nome. Campo Obrigatório");

            //Fornecedor
            if ((pSolucaoEducacional.Fornecedor) == null || (pSolucaoEducacional != null && pSolucaoEducacional.Fornecedor.ID <= 0)) throw new AcademicoException("Fornecedor. Campo Obrigatório");

            //Forma de Aquisição
            if ((pSolucaoEducacional.FormaAquisicao) == null || (pSolucaoEducacional != null && pSolucaoEducacional.FormaAquisicao.ID <= 0)) throw new AcademicoException("Forma de Aquisição. Campo Obrigatório");

            //Categoria
            //if ((pSolucaoEducacional.CategoriaConteudo) == null || (pSolucaoEducacional != null && pSolucaoEducacional.CategoriaConteudo.ID <= 0)) throw new AcademicoException("Categoria. Campo Obrigatório");

            VerificarConsistenciaUk(pSolucaoEducacional);
        }

        public void VerificarConsistenciaUk(SolucaoEducacional pSolucaoEducacional)
        {
            if (string.IsNullOrWhiteSpace(pSolucaoEducacional.Nome))
                throw new AcademicoException("Nome da Solução Educacional não pode ser vazio");

            var usuarioLogado = new BMUsuario().ObterUsuarioLogado();

            if (pSolucaoEducacional.UFGestor == null || !usuarioLogado.IsGestor())
            {
                var solucoes =
                    ObterPorNome(pSolucaoEducacional.Nome)
                        .Where(
                            s =>
                                s.Fornecedor != null && pSolucaoEducacional.Fornecedor != null &&
                                s.Fornecedor.ID == pSolucaoEducacional.Fornecedor.ID &&
                                s.Ativo &&
                                pSolucaoEducacional.ID != s.ID);

                if (solucoes.Any())
                {
                    throw new AcademicoException("Nome da Solução Educacional já cadastrado para este fornecedor");
                }
            }
            else
            {
                var solucoes =
                    ObterPorNome(pSolucaoEducacional.Nome).AsEnumerable()
                        .Where(
                            s =>
                                s.Fornecedor.ID == pSolucaoEducacional.Fornecedor.ID &&
                                s.Fornecedor != null &&
                                pSolucaoEducacional.Fornecedor != null &&
                                s.UFGestor != null && !s.UFGestor.PermiteSesMesmoNome() &&
                                s.Ativo &&
                                pSolucaoEducacional.ID != s.ID).AsQueryable();

                FiltrarPermissaoVisualizacao(ref solucoes, usuarioLogado.ID);

                if (solucoes.Any())
                {
                    throw new AcademicoException("Nome da Solução Educacional já cadastrado para esta UF e este fornecedor");
                }
            }
        }

        public SolucaoEducacional ObterPorIDFornecedorEIdChaveExterna(string loginFornecedor, string idChaveExterna)
        {
            SolucaoEducacional solucaoEducacional = null;
            var query = repositorio.session.Query<SolucaoEducacional>();
            solucaoEducacional = query.FirstOrDefault(x => x.Fornecedor.Login == loginFornecedor &&
                                                           x.IDChaveExterna == idChaveExterna);

            return solucaoEducacional;
        }




        public IList<SolucaoEducacional> ObterListaSolucaoEducacionalPorCategoria(int categoriaConteudo)
        {
            var query = repositorio.session.Query<SolucaoEducacional>();

            return query.Where(s => s.CategoriaConteudo.ID == categoriaConteudo && s.Ativo).ToList();

            #region Código refatorado acima
            //IList<SolucaoEducacional> ListaSolucaoEducacional = query.Where(x => x.CategoriaConteudo.ID == categoriaConteudo
            //    && x.Ativo).ToList();
            #endregion
        }

        public IList<SolucaoEducacional> ObterListaSolucaoEducacionalPorCategoria(IEnumerable<int> categoriasConteudo)
        {
            return
                repositorio.session.Query<SolucaoEducacional>()
                    .Where(x => categoriasConteudo.Contains(x.CategoriaConteudo.ID) && x.Ativo)
                    .ToList();
        }

        public IQueryable<SolucaoEducacional> ObterListaSolucaoEducacionalPorCategoriaGestor(IEnumerable<int> categoriasConteudo)
        {
            var usuario = new BMUsuario().ObterUsuarioLogado();

            var solucoes = ObterTodos().Where(x => categoriasConteudo.Contains(x.CategoriaConteudo.ID));

            if (usuario.IsGestor())
                FiltrarPermissaoVisualizacao(ref solucoes, usuario.UF.ID);

            return solucoes;
        }

        public SolucaoEducacional ObterSolucaoEducacionalPorFornecedor(string loginFornecedor, string idChaveExternaSolucaoEducacional)
        {
            var query = repositorio.session.Query<SolucaoEducacional>();
            query = query.Where(x => x.IDChaveExterna == idChaveExternaSolucaoEducacional);
            query = query.Where(x => x.Fornecedor.Login == loginFornecedor);

            return query.FirstOrDefault();

        }

        public IQueryable<SolucaoEducacional> ObterPorFiltro(SolucaoEducacional pSolucaoEducacional)
        {
            return ObterPorFiltro(pSolucaoEducacional, 0);
        }

        public IQueryable<SolucaoEducacional> ObterPorFiltro(SolucaoEducacional pSolucaoEducacional, int ufPermitida)
        {
            var query = repositorio.session.Query<SolucaoEducacional>();

            if (pSolucaoEducacional != null)
            {
                if (!string.IsNullOrWhiteSpace(pSolucaoEducacional.Nome))
                    query = query.Where(x => x.Nome.Trim().ToUpper().Contains(pSolucaoEducacional.Nome.Trim().ToUpper()));

                if (pSolucaoEducacional.FormaAquisicao != null)
                    query = query.Where(x => x.FormaAquisicao.ID == pSolucaoEducacional.FormaAquisicao.ID);

                if (pSolucaoEducacional.Fornecedor != null)
                    query = query.Where(x => x.Fornecedor.ID == pSolucaoEducacional.Fornecedor.ID);

                if (!string.IsNullOrWhiteSpace(pSolucaoEducacional.IDChaveExterna))
                    query =
                        query.Where(
                            x =>
                                x.IDChaveExterna.Trim()
                                    .ToUpper()
                                    .Contains(pSolucaoEducacional.IDChaveExterna.Trim().ToUpper()));
            }

            // Gestores só podem ver soluções educacionais do seu estado

            var bmUsuario = new BMUsuario();

            var usuario = bmUsuario.ObterUsuarioLogado();

            if (usuario.IsGestor())
            {
                var uf = usuario.UF;

                FiltrarPermissaoVisualizacao(ref query, uf.ID);
            }

            if (ufPermitida > 0)
                query = query.Where(x => x.ListaPermissao.Any(l => l.Uf.ID == ufPermitida));

            return query;
        }

        public IQueryable<SolucaoEducacional> ObterPorCategoria(IList<int> listaIdCategoria, IList<int> pUfResponsavel)
        {
            return
                repositorio.session.Query<SolucaoEducacional>()
                    .Where(x => (!listaIdCategoria.Any() || listaIdCategoria.Contains(x.CategoriaConteudo.ID)) &&
                        (!pUfResponsavel.Any() || pUfResponsavel.Contains(x.UFGestor.ID))
                    );
        }

        public IList<SolucaoEducacional> ObterPorFiltroPesquisa(SolucaoEducacional pSolucaoEducacional, bool? ativo, List<int> usuarioPermissoes)
        {
            var query = repositorio.session.Query<SolucaoEducacional>();

            if (pSolucaoEducacional == null) return query.ToList();

            if (!string.IsNullOrWhiteSpace(pSolucaoEducacional.Nome))
                query = query.Where(x => x.Nome.Trim().ToUpper().Contains(pSolucaoEducacional.Nome.Trim().ToUpper()));

            if (pSolucaoEducacional.FormaAquisicao != null)
                query = query.Where(x => x.FormaAquisicao.ID == pSolucaoEducacional.FormaAquisicao.ID);

            if (pSolucaoEducacional.Fornecedor != null)
                query = query.Where(x => x.Fornecedor.ID == pSolucaoEducacional.Fornecedor.ID);

            if (!string.IsNullOrWhiteSpace(pSolucaoEducacional.IDChaveExterna))
                query = query.Where(x => x.IDChaveExterna.Trim().ToUpper().Contains(pSolucaoEducacional.IDChaveExterna.Trim().ToUpper()));

            if (ativo.HasValue)
            {
                query = query.Where(f => f.Ativo == ativo.Value);
            }

            if (usuarioPermissoes.Any())
            {
                query = query.Where(f => usuarioPermissoes.Contains(f.ID));
            }

            return query.ToList();
        }

        public IList<SolucaoEducacional> ConsultarSolucaoEducacionalWebServices(int pUsuario, int pFornecedor, int pFormaAquisicao)
        {


            IList<SolucaoEducacional> lstResult;


            if (pUsuario != 0)

                lstResult = (from se in repositorio.session.Query<SolucaoEducacional>()
                             join op in repositorio.session.Query<Oferta>() on
                                se.ID equals op.SolucaoEducacional.ID
                             join mo in repositorio.session.Query<MatriculaOferta>() on
                                op.ID equals mo.Oferta.ID
                             join opp in repositorio.session.Query<ViewOfertaPermissao>() on
                                op.ID equals opp.Oferta.ID
                             where mo.Usuario.ID == pUsuario && opp.Usuario.ID == pUsuario
                             && mo.StatusMatricula != enumStatusMatricula.CanceladoAdm &&
                                mo.StatusMatricula != enumStatusMatricula.CanceladoAluno
                             select se).ToList();
            else
                lstResult = (from se in repositorio.session.Query<SolucaoEducacional>()
                             join op in repositorio.session.Query<Oferta>() on
                                se.ID equals op.SolucaoEducacional.ID
                             select se).ToList();



            lstResult = lstResult.Distinct().ToList();

            if (pFornecedor != 0)
                lstResult = lstResult.Where(x => x.Fornecedor.ID == pFornecedor).ToList();

            if (pFormaAquisicao != 0)
                lstResult = lstResult.Where(x => x.FormaAquisicao.ID == pFormaAquisicao).ToList();

            return lstResult;

        }

        public SolucaoEducacional ConsultarStatusMatriculaSolucaoEducacinal(int pUsuario, int pSolucaoEducacional)
        {
            return (from se in repositorio.session.Query<SolucaoEducacional>()
                    join op in repositorio.session.Query<Oferta>() on
                       se.ID equals op.SolucaoEducacional.ID
                    join mo in repositorio.session.Query<MatriculaOferta>() on
                       op.ID equals mo.Oferta.ID
                    where mo.Usuario.ID == pUsuario
                    && se.ID == pSolucaoEducacional
                    select se).FirstOrDefault();
        }

        //public IList<SolucaoEducacional> ObterObrigatorios()
        //{
        //    return ObterObrigatorios(null);
        //}

        public IQueryable<SolucaoEducacionalObrigatoria> ObterObrigatorios()
        {
            var query = repositorio.session.Query<SolucaoEducacionalObrigatoria>();

            return query;
        }

        /// <summary>
        /// Obtém uma solução Educacional por Nome.
        /// </summary>
        /// <param name="nome">Nome da Solução Educacional</param>
        /// <returns>Objeto da classe SolucaoEducacional</returns>
        public IQueryable<SolucaoEducacional> ObterPorNome(string nome)
        {
            return
                repositorio.session.Query<SolucaoEducacional>()
                    .Where(s => string.Equals(s.Nome.Trim().ToLower(), nome.Trim().ToLower()));
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public IList<DTOSolucaoEducacionalPermissao> ObterListaDePermissoes(int idUsuario, int solucaoId)
        {
            return new ProcSolucaoEducacionalPermissao().Executar(idUsuario, solucaoId);
        }

        public bool VerificarSeUsuarioPossuiPermissao(int idUsuario, int idSolucaoEducacional)
        {
            bool possuiPermissao = false;

            if (idUsuario <= 0)
                throw new AcademicoException("Usuário. Campo Obrigatório");

            if (idSolucaoEducacional <= 0)
                throw new AcademicoException("Solução Educacional. Campo Obrigatório");

            var procSolucaoEducacionalPermissao = new ProcSolucaoEducacionalPermissao();

            var listaDePermissoes = procSolucaoEducacionalPermissao.Executar(idUsuario, idSolucaoEducacional);

            if (listaDePermissoes != null && listaDePermissoes.Count > 0)
            {
                if (listaDePermissoes.Any(x => x.SolucaoEducacional.ID == idSolucaoEducacional))
                {
                    possuiPermissao = true;
                }
            }

            return possuiPermissao;
        }

        // Pega todos para gestor ou todos caso não seja
        public IQueryable<SolucaoEducacional> ObterTodosPorGestor(bool? exibirInativos = null)
        {
            // Gestores só podem ver soluções educacionais do seu estado
            var usuario = new BMUsuario().ObterUsuarioLogado();

            bool apenasAtivos = exibirInativos != null && exibirInativos == true ? false : !usuario.IsAdministrador();

            // Caso seja administrador traz todos ativos ou não
            var retorno = ObterTodos(apenasAtivos);

            if (usuario.IsGestor())
                FiltrarPermissaoVisualizacao(ref retorno, usuario.UF.ID);

            return retorno;
        }

        public void FiltrarPermissaoVisualizacao(ref IQueryable<SolucaoEducacional> se, int ufId)
        {
            se = se.Where(s => s.UFGestor != null && s.UFGestor.ID == ufId);
        }
    }
}

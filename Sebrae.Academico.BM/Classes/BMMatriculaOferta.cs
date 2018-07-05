using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMMatriculaOferta : BusinessManagerBase
    {
        #region "Atributos Privados"

        private RepositorioBase<MatriculaOferta> Repositorio { get; set; }

        #endregion

        #region "Construtor"

        public BMMatriculaOferta()
        {
            Repositorio = new RepositorioBase<MatriculaOferta>();
        }

        #endregion


        #region "Métodos Públicos"

        public MatriculaOferta ObterInformacoesDaMatricula(int IdMatriculaOferta)
        {
            MatriculaOferta InformacoesDaMatricula = null;
            var query = Repositorio.session.Query<MatriculaOferta>();
            query = query.Fetch(x => x.Oferta).ThenFetch(x => x.ListaTurma);
            InformacoesDaMatricula = query.FirstOrDefault(x => x.ID == IdMatriculaOferta);

            return InformacoesDaMatricula;
        }

        /// <summary>
        /// Obtém a matricula da oferta filtrando pela Solução Educacional e pela Oferta.
        /// </summary>
        /// <param name="idSolucaoEducacional"></param>
        /// <param name="idOferta"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IQueryable<MatriculaOferta> ObterPorFiltro(int idSolucaoEducacional, int idOferta, int limit, int page)
        {
            var query = Repositorio.session.Query<MatriculaOferta>();
            query = query.Where(x => x.Oferta.ID == idOferta && x.Oferta.SolucaoEducacional.ID == idSolucaoEducacional).OrderBy(x => x.Usuario.Nome);
            
            if (limit > 0)
                query = query.Skip(page * limit).Take(limit);
            
            return query;
        }

        public IQueryable<MatriculaOferta> ObterPorUsuario(int usuarioId)
        {
            var query = Repositorio.session.Query<MatriculaOferta>();
            query = query.Where(x => x.Usuario.ID == usuarioId);
            query = query.Fetch(x => x.Oferta).ThenFetch(x => x.SolucaoEducacional);

            return query;
        }

        public IQueryable<MatriculaOferta> ObterPorUsuarioESolucaoEducacional(int usuarioId, int solucaoId)
        {
            var query = Repositorio.session.Query<MatriculaOferta>();

            query = query.Fetch(x => x.Oferta).ThenFetch(x => x.SolucaoEducacional);

            query = query.Where(x => x.Usuario.ID == usuarioId && x.Oferta.SolucaoEducacional.ID == solucaoId);

            return query;
        }

        /// <summary>
        /// Obtém uma lista de Turmas de uma oferta
        /// </summary>
        /// <param name="IdOferta">Id da Oferta</param>
        /// <returns>Lista de Turmas de uma oferta</returns>
        public IQueryable<Turma> ObterTurmasDaOferta(int IdOferta)
        {
            return Repositorio.session.Query<Turma>().Where(x => x.Oferta.ID == IdOferta).OrderBy(x => x.Nome);
        }

        public MatriculaOferta ObterPorID(int Id)
        {
            MatriculaOferta matriculaOferta = null;

            var query = Repositorio.session.Query<MatriculaOferta>();

            query = query.Fetch(x => x.Oferta).ThenFetch(x => x.SolucaoEducacional);
            query = query.Fetch(x => x.Usuario).ThenFetch(x => x.NivelOcupacional);
            query = query.Fetch(x => x.Usuario).ThenFetch(x => x.UF);
            query = query.Fetch(x => x.Oferta).ThenFetch(x => x.ListaTurma);

            matriculaOferta = query.FirstOrDefault(x => x.ID == Id);

            return matriculaOferta;
        }

        /// <summary>
        /// Obterm A Matrícula Oferta pelo Filtro de Uma Matricula Oferta Informados.
        /// </summary>
        /// <param name="pMatriculaOferta">Dados da Matricula Oferta a Serem Filtrados</param>
        /// <returns></returns>
        public IList<MatriculaOferta> ObterPorFiltro(MatriculaOferta pMatriculaOferta)
        {
            var query = Repositorio.session.Query<MatriculaOferta>();
            query = query.Fetch(x => x.Oferta).ThenFetch(x => x.SolucaoEducacional);
            query = query.Fetch(x => x.Usuario).ThenFetch(x => x.NivelOcupacional);
            query = query.Fetch(x => x.Usuario).ThenFetch(x => x.UF);
            query = query.Fetch(x => x.Oferta).ThenFetch(x => x.ListaTurma);

            if (pMatriculaOferta.Oferta != null && pMatriculaOferta.Oferta.ID > 0)
                query = query.Where(x => x.Oferta.ID == pMatriculaOferta.Oferta.ID);

            if (pMatriculaOferta.Usuario != null && pMatriculaOferta.Usuario.ID > 0)
                query = query.Where(x => x.Usuario.ID == pMatriculaOferta.Usuario.ID);

            if (pMatriculaOferta.StatusMatricula > 0)
                query = query.Where(x => x.StatusMatricula == pMatriculaOferta.StatusMatricula);

            if (pMatriculaOferta.DataSolicitacao != DateTime.MinValue)
                query = query.Where(x => x.DataSolicitacao.Date == pMatriculaOferta.DataSolicitacao.Date);

            return query.ToList();

        }

        public void Salvar(MatriculaOferta pMatriculaOferta)
        {
            //Obtém por Id 
            if (pMatriculaOferta.ID > 0)
            {
                Repositorio.Evict(pMatriculaOferta);

                var matriculaOfertaGravadaNoBanco = ObterPorID(pMatriculaOferta.ID);

                //Se houve troca de status, atualiza a data desta troca de status
                if (!matriculaOfertaGravadaNoBanco.StatusMatricula.Equals(pMatriculaOferta.StatusMatricula))
                {
                    pMatriculaOferta.DataStatusMatricula = DateTime.Now;
                    // Atualiza a matrícula do usuário no Moodle, caso seja necessário
                    AtualizaStatusMatriculaMoodle(pMatriculaOferta, matriculaOfertaGravadaNoBanco.StatusMatricula);
                }
                //this.repositorio.session.Update(pMatriculaOferta);
                Repositorio.FazerMerge(pMatriculaOferta);
            }
            else
            {
                Repositorio.Salvar(pMatriculaOferta);
            }
        }

        public void AtualizaStatusMatriculaMoodle(MatriculaOferta pMatriculaOferta, enumStatusMatricula statusMatriculaAnterior)
        {
            pMatriculaOferta.Oferta = new BMOferta().ObterPorId(pMatriculaOferta.Oferta.ID);

            if (pMatriculaOferta != null && pMatriculaOferta.Oferta != null)
            {
                // Status que provocam suspensão no moodle
                System.Collections.ArrayList failStatus = new System.Collections.ArrayList();
                failStatus.Add(3);
                failStatus.Add(4);
                failStatus.Add(5);
                failStatus.Add(10);

                // Se for uma matricula em uma oferta de uma solução educacional do SEBRAE, e tiver indo de um status válido para um de suspensão
                if (pMatriculaOferta.Oferta.CodigoMoodle != null && pMatriculaOferta.Oferta.SolucaoEducacional.Fornecedor.ID.Equals((int)enumFornecedor.MoodleSebrae))
                {
                    Moodle.BMInscricao bmInscricao = new Moodle.BMInscricao();

                    // Indo de um status de não suspensão para um de suspensão
                    if (failStatus.Contains((int)pMatriculaOferta.StatusMatricula) && !failStatus.Contains((int)statusMatriculaAnterior))
                    {
                        bmInscricao.alterarInscricao(pMatriculaOferta.Oferta.CodigoMoodle.Value, pMatriculaOferta.Usuario.CPF, 1);

                        // Indo de um status de suspensão para Inscrito
                    }
                    else if (!failStatus.Contains((int)pMatriculaOferta.StatusMatricula) && enumStatusMatricula.Inscrito.Equals(pMatriculaOferta.StatusMatricula))
                    {
                        bmInscricao.alterarInscricao(pMatriculaOferta.Oferta.CodigoMoodle.Value, pMatriculaOferta.Usuario.CPF, 0);
                    }
                }
            }
        }

        public void Dispose()
        {
            Repositorio.Dispose();
        }

        public bool AprovacaoPorUsuarioESolucaoEducacional(int idUsuario, int idSolucaoEducacional)
        {
            // Deve ser feita a consulta via turma pois essa possui data de termino

            var query = Repositorio.session.Query<MatriculaTurma>();

            query = query.Fetch(x => x.MatriculaOferta).Where(x => x.MatriculaOferta.Oferta.SolucaoEducacional.ID == idSolucaoEducacional && x.MatriculaOferta.Usuario.ID == idUsuario);

            query = query.Where(x => (
                                x.MatriculaOferta.StatusMatricula == enumStatusMatricula.Aprovado ||
                                x.MatriculaOferta.StatusMatricula == enumStatusMatricula.AprovadoComoGestor ||
                                x.MatriculaOferta.StatusMatricula == enumStatusMatricula.AprovadoComoConsultor ||
                                x.MatriculaOferta.StatusMatricula == enumStatusMatricula.AprovadoComoConsultorComAcompanhamento ||
                                x.MatriculaOferta.StatusMatricula == enumStatusMatricula.AprovadoComoFacilitador ||
                                x.MatriculaOferta.StatusMatricula == enumStatusMatricula.AprovadoComoFacilitadorComAcompanhamento ||
                                x.MatriculaOferta.StatusMatricula == enumStatusMatricula.AprovadoComoModerador ||
                                x.MatriculaOferta.StatusMatricula == enumStatusMatricula.AprovadoComoModeradorComAcompanhamento ||
                                x.MatriculaOferta.StatusMatricula == enumStatusMatricula.AprovadoComoMultiplicador ||
                                x.MatriculaOferta.StatusMatricula == enumStatusMatricula.AprovadoComoMultiplicadorComAcompanhamento ||
                                x.MatriculaOferta.StatusMatricula == enumStatusMatricula.Concluido)
            );

            // TODO: Corrigir data de termino quando aprovado pelo gerenciador de matrículas

            //if (dataTermino != null)
            //{
            //    query = query.Where(x => x.DataTermino.HasValue && x.DataTermino <= dataTermino);
            //}

            var matTurmas = query.ToList();

            var aprovacoes = matTurmas.Count();

            // Caso seja aprovado em mais de uma oferta considera como aprovado
            return aprovacoes > 0;

        }

        public IList<MatriculaOferta> ObterPorUsuarioSemMatriculasCapacitacoes(int idUsuario)
        {
            var queryCapacitacao = Repositorio.session.Query<MatriculaCapacitacao>();
            queryCapacitacao = queryCapacitacao.Where(x => x.Usuario.ID == idUsuario && x.StatusMatricula == enumStatusMatricula.Inscrito);
            //queryCapacitacao = queryCapacitacao.Fetch(x => x.Capacitacao).ThenFetchMany(x => x.ListaModulos).ThenFetchMany(x => x.ListaSolucaoEducacional);
            /*.Where(x => x.Usuario.ID == idUsuario)
            .Fetch(x => x.Capacitacao)
            .ThenFetchMany(x => x.ListaModulos)
            .ThenFetchMany(x => x.ListaSolucaoEducacional);*/
            //<--- esse método não funciona corretamente...
            var matrsCapacitacao = queryCapacitacao.ToList();

            /*var listCapSolucoes =
                (from matrCapacitacao in queryCapacitacao
                 from modulo in matrCapacitacao.Capacitacao.ListaModulos
                 from solucao in modulo.ListaSolucaoEducacional
                 select solucao.SolucaoEducacional.ID).Distinct();*/
            // <--- esse método não funciona corretamente...
            var listCapSolucoes = new List<int>();
            foreach (var matrCapacitacao in matrsCapacitacao)
            {
                foreach (var modulo in matrCapacitacao.Capacitacao.ListaModulos.Distinct())
                {
                    listCapSolucoes.AddRange(modulo.ListaSolucaoEducacional.Distinct().Select(solucao => solucao.SolucaoEducacional.ID));
                }
            }

            // Pega somente os Status que são necessário na listagem de Meus Cursos
            var query = Repositorio.session.Query<MatriculaOferta>();
            query = query.Fetch(x => x.Oferta).ThenFetch(x => x.SolucaoEducacional);
            query = query.Where(x => x.Usuario.ID == idUsuario &&
                            (x.StatusMatricula == enumStatusMatricula.Inscrito ||
                             x.StatusMatricula == enumStatusMatricula.PendenteConfirmacaoAluno));

            return !listCapSolucoes.Any()
                ? query.ToList<MatriculaOferta>()
                : query.Where(x => x.Oferta != null &&
                    !listCapSolucoes.Contains(x.Oferta.SolucaoEducacional.ID)).ToList<MatriculaOferta>();
        }

        public MatriculaOferta ObterPorCodigoCertificado(string codigo, Usuario usuario = null)
        {
            var query = Repositorio.session.Query<MatriculaOferta>();

            if (usuario != null && usuario.ID > 0)
            {
                query = query.Where(x => x.Usuario.ID == usuario.ID);
            }

            var matriculaOferta = query.FirstOrDefault(x => x.CDCertificado == codigo);

            return matriculaOferta;
        }

        #endregion

        public void VerificarPoliticaDeConsequencia(MatriculaOferta matricula)
        {
            try
            {
                VerificarPoliticaDeConsequencia(matricula.Usuario, matricula.Oferta.SolucaoEducacional);
            }
            catch (PoliticaConsequenciaException e)
            {
                e.MatriculaOferta = matricula;
                throw;
            }
        }

        public void VerificarPoliticaDeConsequencia(int idUsuario, int idSe)
        {
            VerificarPoliticaDeConsequencia(new BMUsuario().ObterPorId(idUsuario), new BMSolucaoEducacional().ObterPorId(idSe));
        }

        public void VerificarPoliticaDeConsequencia(Usuario usuario, SolucaoEducacional se)
        {
            // Ignorar regras de inscrição caso esteja setado na categoria.
            if (se.CategoriaConteudo.LiberarInscricao)
                return;

            var matriculas = Repositorio.session.Query<MatriculaOferta>().Where(m => m.Usuario.ID == usuario.ID && m.Oferta.SolucaoEducacional.ID == se.ID);

            foreach (var matricula in matriculas)
            {
                #region Paliativo #1598

                // Redmine #1598, alterações paleativas para solucionar problemas de inscrição do cliente.

                var nome = matricula.Oferta.SolucaoEducacional.CategoriaConteudo.Nome;
                var anoConclusao = matricula.DataConclusao != "" ? int.Parse(matricula.DataConclusao.Split('/')[2]) : 0;

                if (nome.ToLower().Contains("capacitações online") && matricula.IsAbandono())
                {
                    CalcularPolitica(matricula, 1, "abandono", DateTime.Now.Date, enumRespostaPoliticaDeConsequencia.Abandono);
                }

                if (nome.ToLower().Contains("in-company") && matricula.IsAbandono())
                {
                    CalcularPolitica(matricula, 3, "abandono", DateTime.Now.Date, enumRespostaPoliticaDeConsequencia.Abandono);
                }

                #endregion

                int? qntMeses = null;
                string nomePolitica = null;
                var dataReferencia = DateTime.Now.Date;
                var consequencia = enumRespostaPoliticaDeConsequencia.AptoInscricao;

                if (matricula.IsCancelado())
                {
                    if (!(anoConclusao < DateTime.Now.Year))
                    {
                        qntMeses = 1;
                        nomePolitica = "cancelamento";
                        consequencia = enumRespostaPoliticaDeConsequencia.Cancelamento;
                    }
                }

                if (matricula.IsAbandono())
                {
                    if (!(anoConclusao < DateTime.Now.Year))
                    {
                        qntMeses = 2;
                        nomePolitica = "abandono";
                        consequencia = enumRespostaPoliticaDeConsequencia.Abandono;
                    }
                }

                if (matricula.IsAprovado())
                {
                    qntMeses = 12;
                    nomePolitica = "aprovação";
                    consequencia = enumRespostaPoliticaDeConsequencia.Aprovado;
                }

                if (matricula.IsReprovado())
                {
                    qntMeses = 1;
                    nomePolitica = "reprovação";
                    consequencia = enumRespostaPoliticaDeConsequencia.Reprovado;
                }

                if (qntMeses != null & nomePolitica != null)
                    CalcularPolitica(matricula, qntMeses.Value, nomePolitica, dataReferencia, consequencia);
            }
        }

        private static void CalcularPolitica(MatriculaOferta matricula, int qntMeses, string nomePolitica, DateTime dataReferencia, enumRespostaPoliticaDeConsequencia consequencia)
        {
            var data = dataReferencia.AddMonths(-qntMeses);

            if (matricula.DataSolicitacao.Date < data.Date) return;

            var msg = string.Format("O usuário '{0}' possui {1} neste curso e não pode se matricular até {2}",
                matricula.Usuario.Nome,
                nomePolitica,
                matricula.DataSolicitacao.AddMonths(qntMeses).ToString("dd/MM/yyyy"));

            throw new PoliticaConsequenciaException(msg, consequencia);
        }

        public IEnumerable<MatriculaOferta> ObterTodos()
        {
            return Repositorio.session.Query<MatriculaOferta>();
        }

        public IQueryable<MatriculaOferta> ObterTodosIQueryable()
        {
            return Repositorio.session.Query<MatriculaOferta>().AsQueryable();
        }

        public IQueryable<MatriculaOferta> ObterPorPerfil(List<int> perfis)
        {
            return
                Repositorio.session.Query<MatriculaOferta>()
                    .Where(x => x.Usuario.ListaPerfil.Any(p => perfis.Contains(p.Perfil.ID)));
        }

        public IQueryable<MatriculaOferta> ObterPorNivelOcupacional(List<int> niveisOcupacionais)
        {
            return
                Repositorio.session.Query<MatriculaOferta>()
                    .Where(x => niveisOcupacionais.Contains(x.NivelOcupacional.ID));
        }

        public IQueryable<MatriculaOferta> ObterPorOferta(int ofertaId)
        {
            return ObterTodos().Where(x => x.Oferta.ID == ofertaId).AsQueryable();
        }

        public void FazerMerge(MatriculaOferta pMatriculaOferta)
        {
            Repositorio.FazerMerge(pMatriculaOferta);
        }

        public IQueryable<MatriculaOferta> ObterPorUsuarioETurma(int usuarioId, int turmaId)
        {
            var query = Repositorio.session.Query<MatriculaOferta>();

            query = query.Fetch(x => x.Oferta).ThenFetch(x => x.SolucaoEducacional);

            query = query.Where(x => x.Usuario.ID == usuarioId && x.MatriculaTurma.Any(mt => mt.Turma.ID == turmaId));

            return query;
        }

        public MatriculaOferta ObterPorOfertaEUsuario(int idOferta, int idUsuario)
        {
            return Repositorio.session
                .Query<MatriculaOferta>()
                .Where(x => x.Oferta.ID == idOferta && x.Usuario.ID == idUsuario)
                .FirstOrDefault();
        }

        public IQueryable<MatriculaOferta> ObterTodosParaConsultarLinkAcessoFornecedor()
        {
            return from x in Repositorio.session.Query<MatriculaOferta>()
                select new MatriculaOferta
                {
                    ID = x.ID,
                    Usuario = new Usuario
                    {
                        ID = x.Usuario.ID,
                        CPF = x.Usuario.CPF,
                        Senha = x.Usuario.Senha
                    },
                    Oferta = new Oferta
                    {
                        ID = x.Oferta.ID,
                        SolucaoEducacional = new SolucaoEducacional
                        {
                            ID = x.Oferta.SolucaoEducacional.ID
                        }
                    }
                };
        }
    }
}

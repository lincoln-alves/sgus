using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;


namespace Sebrae.Academico.BM.Classes
{
    public class BMQuestionarioParticipacao : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<QuestionarioParticipacao> repositorio;

        public BMQuestionarioParticipacao()
        {
            repositorio = new RepositorioBase<QuestionarioParticipacao>();
        }

        public QuestionarioParticipacao ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public QuestionarioParticipacao ObterPorIDItemTrilha(int pId)
        {
            return repositorio.session.Query<QuestionarioParticipacao>()
                .Where(x => x.IdItemTrilha == pId).FirstOrDefault();
        }

        public void Salvar(QuestionarioParticipacao qp)
        {
            repositorio.Salvar(qp);
        }

        public void Remover(QuestionarioParticipacao qp)
        {
            repositorio.Excluir(qp);
        }

        public IQueryable<QuestionarioParticipacao> ObterPorUsuario(Usuario us)
        {
            return repositorio.session.Query<QuestionarioParticipacao>()
                .Where(x => x.Usuario.ID == us.ID);
        }

        public IList<QuestionarioParticipacao> ObterPorUsuarioTrilhaNivel(Usuario us, TrilhaNivel tn)
        {

            IList<QuestionarioParticipacao> lstResut = repositorio.session.Query<QuestionarioParticipacao>().CacheMode(NHibernate.CacheMode.Refresh)
                   .Where(x => x.Usuario.ID == us.ID &&
                               x.TrilhaNivel.ID == tn.ID).ToList();

            return lstResut;
        }

        public IList<QuestionarioParticipacao> ObterPorUsuarioTurma(Usuario usuario, Turma turma)
        {

            IList<QuestionarioParticipacao> lstResut = repositorio.session.Query<QuestionarioParticipacao>().CacheMode(NHibernate.CacheMode.Refresh)
                   .Where(x => x.Usuario.ID == usuario.ID &&
                               x.Turma.ID == turma.ID).ToList();

            return lstResut;
        }

        public IList<int> ObterTodosUsuarios()
        {
            return repositorio.session.Query<QuestionarioParticipacao>().Select(x => x.Usuario.ID).Distinct().ToList();
        }

        public IQueryable<QuestionarioParticipacao> ObterTodosQuestionariosComParticipacaoQueryble()
        {
            return repositorio.session.Query<QuestionarioParticipacao>();
        }

        public IList<ItemQuestionarioParticipacao> ObterTodosQuestionariosComParticipacao()
        {
            return repositorio.session.Query<ItemQuestionarioParticipacao>().ToList();
        }

        /// <summary>
        /// Obtém as últimas provas do usuário.
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <param name="IdTrilhaNivel"></param>
        /// <returns>Lista de provas do usuário</returns>
        public IList<QuestionarioParticipacao> ObterProvasDaTrilhaDoUsuario(int IdUsuario, int IdTrilhaNivel)
        {
            var query = repositorio.session.Query<QuestionarioParticipacao>();

            IList<QuestionarioParticipacao> ListaProvasDoUsuario = query.Where(x => x.Usuario.ID == IdUsuario
                                                                        && x.TrilhaNivel.ID == IdTrilhaNivel).ToList();

            return ListaProvasDoUsuario;
        }

        /// <summary>
        /// Obtém as últimas provas do usuário.
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <param name="IdTurma"></param>
        /// <returns>Lista de provas do usuário</returns>
        public IList<QuestionarioParticipacao> ObterProvasDaTurmaDoUsuario(int IdUsuario, int IdTurma)
        {
            var query = repositorio.session.Query<QuestionarioParticipacao>();

            IList<QuestionarioParticipacao> ListaProvasDoUsuario = query.Where(x => x.Usuario.ID == IdUsuario
                                                                        && x.Turma.ID == IdTurma).ToList();

            return ListaProvasDoUsuario;
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public void ExcluiParticipacoes(IList<QuestionarioParticipacao> questParticipacoes)
        {
            foreach (QuestionarioParticipacao questParticipacao in questParticipacoes)
            {
                repositorio.Excluir(questParticipacao);
            }
        }


        public IQueryable<QuestionarioParticipacao> ObterPorQuestionarioPorFiltro(int idQuestionario,
            List<int> categorias, int? idSolucao, int? idOferta, int? idTurma, List<int> idsUf,
            List<int> idsNivelOcupacional, List<int> idsStatusMatricula)
        {
            var query =
                (from x in
                    repositorio.session.CreateCriteria(typeof(QuestionarioParticipacao))
                        .List<QuestionarioParticipacao>()
                 where
                     x.Questionario.ID == idQuestionario &&
                     x.DataParticipacao != null

                 // Trazer só o necessário. OOYL!
                 select new QuestionarioParticipacao
                 {
                     ID = x.ID,
                     Questionario = x.Questionario,
                     DataParticipacao = x.DataParticipacao,

                     Turma =
                         x.Turma != null
                             ? new Turma
                             {
                                 ID = x.Turma.ID,
                                 Professor = x.Turma.Professor,
                                 Professores = x.Turma.Professores,
                                 Oferta =
                                     new Oferta
                                     {
                                         ID = x.Turma.Oferta.ID,
                                         Nome = x.Turma.Oferta.Nome,
                                         SolucaoEducacional =
                                             new SolucaoEducacional
                                             {
                                                 ID = x.Turma.Oferta.SolucaoEducacional.ID,
                                                 Nome = x.Turma.Oferta.SolucaoEducacional.Nome,
                                                 CategoriaConteudo = new CategoriaConteudo
                                                 {
                                                     ID = x.Turma.Oferta.SolucaoEducacional.CategoriaConteudo != null ? x.Turma.Oferta.SolucaoEducacional.CategoriaConteudo.ID : 0
                                                 }
                                             }
                                     }
                             }
                             : null,
                     TrilhaNivel =
                         x.TrilhaNivel != null
                            ? new TrilhaNivel
                            {
                                ID = x.TrilhaNivel.ID,
                                Trilha = new Trilha
                                {
                                    ID = x.TrilhaNivel.Trilha.ID,
                                    CategoriaConteudo = new CategoriaConteudo
                                    {
                                        ID = x.TrilhaNivel.Trilha.CategoriaConteudo.ID
                                    }
                                }
                            }
                            : null,

                     Usuario = new Usuario
                     {
                         ID = x.Usuario.ID,
                         Nome = x.Usuario.Nome,
                         UF = new Uf { ID = x.Usuario.UF.ID, Sigla = x.Usuario.UF.Sigla },
                         NivelOcupacional = new NivelOcupacional
                         {
                             ID = x.Usuario.NivelOcupacional.ID,
                             Nome = x.Usuario.NivelOcupacional.Nome
                         }
                     }
                 })

                    // Filtros da consulta.
                    .Where(x =>
                        (categorias == null || !categorias.Any() ||
                         ((x.Turma != null && categorias.Contains(x.Turma.Oferta.SolucaoEducacional.CategoriaConteudo.ID)) ||
                          (x.TrilhaNivel != null && categorias.Contains(x.TrilhaNivel.Trilha.CategoriaConteudo.ID)))) &&

                        // Verifica se tem turma, ou oferta, ou SE e filtra, nessa ordem.
                        (idTurma == null || x.Turma != null && x.Turma.ID == idTurma) &&
                        (idOferta == null || x.Turma != null && x.Turma.Oferta.ID == idOferta) &&
                        (idSolucao == null || (x.Turma != null && x.Turma.Oferta.SolucaoEducacional.ID == idSolucao)) &&

                        (!idsUf.Any() || idsUf.Contains(x.Usuario.UF.ID)) &&
                        (!idsNivelOcupacional.Any() || idsNivelOcupacional.Contains(x.Usuario.NivelOcupacional.ID)))

                    .AsQueryable();

            // Atenção, o ministério da programação adverte: o código abaixo faz sangrar.
            // Buscar o status pela TrilhaNivel ou pela Turma somente quando o campo IdStatusMatricuala for informado.
            // Extremamente pesado, mas a refatoração aqui só seria possível se vinculasse o questionário respondido
            // diretamente à matrícula da trilha ou matrícula da turma, o que não ocorre, então realiza consultas
            // separadas para cada caso.

            if (idsStatusMatricula.Any())
            {
                var bmUsuarioTrilha = new BMUsuarioTrilha();
                var bmMatricula = new BMMatriculaTurma();

                query =
                    query.Where(
                        x =>
                            // Tenta a busca através do status a partir do UsuarioTrilha.
                            (x.TrilhaNivel != null &&
                            bmUsuarioTrilha.MatriculaPertenceStatus(x.Usuario.ID, x.TrilhaNivel.ID, idsStatusMatricula)) ||
                            // Se não achar, busca o status a partir da Turma.
                            (x.Turma != null &&
                            bmMatricula.MatriculaPertenceStatus(x.Usuario.ID, x.Turma.ID, idsStatusMatricula)));
            }

            return query.OrderBy(x => x.Usuario.Nome).ThenBy(x => x.Questionario.Nome);
        }

        public IQueryable<ItemQuestionarioParticipacao> ListaItemQuestionarioParticipacao(int idQuestionarioParticipacao)
        {
            return
                repositorio.session.Query<ItemQuestionarioParticipacao>()
                    .Where(
                        x =>
                            x.QuestionarioParticipacao.ID == idQuestionarioParticipacao)
                    .Select(x => new ItemQuestionarioParticipacao
                    {
                        ID = x.ID,
                        TipoItemQuestionario = new TipoItemQuestionario { ID = x.TipoItemQuestionario.ID },
                        Questao = x.Questao,
                        Resposta = x.Resposta,
                        RespostaObrigatoria = x.RespostaObrigatoria
                    });
        }

        public IQueryable<QuestionarioParticipacao> ObterDisponiveisPorItemTrilhaUsuario(int itemTrilhaId, int usuarioId)
        {
            return
                repositorio.session.Query<QuestionarioParticipacao>()
                    .Where(
                        x =>
                            x.Usuario.ID == usuarioId &&
                            x.ListaItemTrilhaParticipacao.Any(itp => itp.ItemTrilha.ID == itemTrilhaId && itp.Autorizado != true) &&
                            x.DataParticipacao == null);
        }

        public IQueryable<QuestionarioParticipacao> ObterTodosIQueryable()
        {
            return repositorio.session.Query<QuestionarioParticipacao>();
        }
    }
}

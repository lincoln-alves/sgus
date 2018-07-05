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
    public class BMQuestionario : BusinessManagerBase
    {

        #region "Construtor"

        public BMQuestionario()
        {
            repositorio = new RepositorioBase<Questionario>();
        }

        #endregion

        private RepositorioBase<Questionario> repositorio;

        public void ValidarQuestionario(Questionario pQuestionario)
        {

            if (pQuestionario.TipoQuestionario == null
                || (pQuestionario.TipoQuestionario != null && pQuestionario.TipoQuestionario.ID == 0))
            {
                throw new AcademicoException("Tipo de Questionário. Campo Obrigatório");
            }

            if (string.IsNullOrWhiteSpace(pQuestionario.Nome))
                throw new AcademicoException("Nome do Questionário. Campo Obrigatório");

        }

        public void Salvar(Questionario pQuestionario)
        {
            ValidarQuestionario(pQuestionario);
            repositorio.Salvar(pQuestionario);
        }

        public void FazerMergeAoSalvar(Questionario pQuestionario)
        {
            ValidarQuestionario(pQuestionario);
            repositorio.FazerMerge(pQuestionario);
        }


        public void Excluir(Questionario pQuestionario)
        {
            repositorio.Excluir(pQuestionario);
        }

        public IList<Questionario> ObterPorFiltro(Questionario pQuestionario)
        {
            var query = repositorio.session.Query<Questionario>();

            query = query.Fetch(x => x.TipoQuestionario);

            if (!string.IsNullOrWhiteSpace(pQuestionario.Nome))
            {
                query = query.Where(x => x.Nome.ToLower().Trim().Contains(pQuestionario.Nome.ToLower().Trim()));
            }

            query = query.Where(x => x.TipoQuestionario.ID != (int)enumTipoQuestionario.Dinamico);

            return query.ToList();
        }

        public IList<Questionario> ObterTodos()
        {
            var query = repositorio.session.Query<Questionario>();

            return query.Where(x => x.TipoQuestionario.ID != (int)enumTipoQuestionario.Dinamico).ToList();
        }

        public IQueryable<Questionario> ObterTodosIQueryable()
        {
            return repositorio.session.Query<Questionario>();
        }

        public IQueryable<Questionario> ObterQuestionariosCancelamento(CategoriaConteudo categoria = null)
        {
            var query = repositorio.session.Query<Questionario>()
                .Where(x => x.TipoQuestionario.ID == (int) enumTipoQuestionario.Cancelamento);

            if (categoria != null)
                query = query.Where(x => x.ListaCategoriaConteudo.Select(c => c.ID).Contains(categoria.ID));

            return query;
        }

        public IQueryable<Questionario> ObterQuestionariosAbandono(CategoriaConteudo categoria = null)
        {
            var query = repositorio.session.Query<Questionario>()
                .Where(x => x.TipoQuestionario.ID == (int)enumTipoQuestionario.Abandono);

            if (categoria != null)
                query = query.Where(x => x.ListaCategoriaConteudo.Select(c => c.ID).Contains(categoria.ID));

            return query;
        }

        public Questionario ObterPorId(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public QuestionarioAssociacao ObterPorQuestionarioAssociacao(QuestionarioAssociacao questionarioAssociacao, int pIdUsuario)
        {
            var query = repositorio.session.Query<QuestionarioAssociacao>();

            if (questionarioAssociacao.Turma != null)
                query = query.Where(y => y.Turma.ID == questionarioAssociacao.Turma.ID);

            if (questionarioAssociacao.TurmaCapacitacao != null)
                query = query.Where(y => y.TurmaCapacitacao.ID == questionarioAssociacao.TurmaCapacitacao.ID);

            if (questionarioAssociacao.TrilhaNivel != null)
                query = query.Where(y => y.TrilhaNivel.ID == questionarioAssociacao.TrilhaNivel.ID);

            if (questionarioAssociacao.TipoQuestionarioAssociacao != null)
                query = query.Where(y => y.TipoQuestionarioAssociacao.ID == questionarioAssociacao.TipoQuestionarioAssociacao.ID);

            query = query.Where(y => y.Evolutivo == questionarioAssociacao.Evolutivo);

            return query.FirstOrDefault();

        }



        public IList<QuestionarioParticipacao> ObterQuestionarioParticipacaoPorFiltro(QuestionarioParticipacao pQuestionarioParticipacao)
        {
            var query = repositorio.session.Query<QuestionarioParticipacao>();

            if (pQuestionarioParticipacao.Turma != null)
                query = query.Where(x => x.Turma.ID == pQuestionarioParticipacao.Turma.ID);

            if (pQuestionarioParticipacao.TurmaCapacitacao != null)
                query = query.Where(x => x.TurmaCapacitacao.ID == pQuestionarioParticipacao.TurmaCapacitacao.ID);

            if (pQuestionarioParticipacao.TrilhaNivel != null)
                query = query.Where(x => x.TrilhaNivel.ID == pQuestionarioParticipacao.TrilhaNivel.ID);

            if (pQuestionarioParticipacao.TipoQuestionarioAssociacao != null)
                query = query.Where(x => x.TipoQuestionarioAssociacao.ID == pQuestionarioParticipacao.TipoQuestionarioAssociacao.ID);

            if (pQuestionarioParticipacao.Usuario != null)
                query = query.Where(x => x.Usuario.ID == pQuestionarioParticipacao.Usuario.ID);

            if (pQuestionarioParticipacao.Questionario != null)
                query = query.Where(x => x.Questionario.ID == pQuestionarioParticipacao.Questionario.ID);

            if (pQuestionarioParticipacao.DataGeracao != new DateTime(1, 1, 1))
                query = query.Where(x => x.DataGeracao.Date == pQuestionarioParticipacao.DataGeracao.Date);

            query = query.Where(x => x.Evolutivo == pQuestionarioParticipacao.Evolutivo);

            return query.ToList();
        }

        /// <summary>
        /// Obtém questionários pelo tipo informado.
        /// </summary>
        /// <param name="tipoQuestionario"></param>
        /// <param name="idCategoria"></param>
        /// <returns></returns>
        public IList<Questionario> ObterQuestionariosPorFiltro(enumTipoQuestionario tipoQuestionario, int? idCategoria = null)
        {
            var query = repositorio.session.Query<Questionario>();

            //Join com Tipo Questionário
            query = query.Fetch(x => x.TipoQuestionario);

            query = query.Where(x => x.TipoQuestionario.ID != (int)enumTipoQuestionario.Dinamico);

            //Esta comparação só é possível com o uso de implicit operator, na classe de domínio TipoQuestionario.
            query = query.Where(x => x.TipoQuestionario.ID == (int)tipoQuestionario);

            if (idCategoria != null)
            {
                query = query.Where(x => x.ListaCategoriaConteudo.Any(p => p.ID == idCategoria.Value) || !x.ListaCategoriaConteudo.Any());
            }

            return query.ToList();
        }

        /// <summary>
        /// Obtém questionários pelo tipo, UF do gestor e categoria.
        /// </summary>
        /// <param name="tipoQuestionario"></param>
        /// <param name="uf">UF do gestor logado. Caso nula, não interfere na consulta</param>
        /// <param name="idCategoria"></param>
        /// <returns></returns>
        public IList<Questionario> ObterQuestionariosPorCategoria(enumTipoQuestionario tipoQuestionario, int idCategoria = 0)
        {
            var query = repositorio.session.Query<Questionario>().Where(x => x.TipoQuestionario != null && x.TipoQuestionario.ID == (int)tipoQuestionario);

            if (idCategoria != 0)
            {
                query = query.Where(x => x.ListaCategoriaConteudo.Any(p => p.ID == idCategoria) || !x.ListaCategoriaConteudo.Any());
            }

            return query.ToList();
        }

        public IList<Questionario> ObterTodosPorCategoria(int idCategoria = 0)
        {
            var query =
                repositorio.session.Query<Questionario>()
                    .Where(
                        x => x.ListaCategoriaConteudo.Any(p => p.ID == idCategoria) || !x.ListaCategoriaConteudo.Any());

            return query.ToList();
        }

        /// <summary>
        /// Obtém questionários pelo tipo, UF do gestor e categoria.
        /// </summary>
        /// <param name="tipoQuestionario"></param>
        /// <param name="uf">UF do gestor logado. Caso nula, não interfere na consulta</param>
        /// <param name="idCategoria"></param>
        /// <returns></returns>
        public IList<Questionario> ObterQuestionariosPorCategoriaGestor(enumTipoQuestionario? tipoQuestionario, Uf uf = null, int idCategoria = 0)
        {
            var query = repositorio.session.Query<Questionario>();

            if (tipoQuestionario != null)
                query = query.Where(x => x.TipoQuestionario != null && x.TipoQuestionario.ID == (int)tipoQuestionario);

            if (uf != null)
                query = query.Where(x => x.ListaCategoriaConteudo.Any( p => p.ListaCategoriaConteudoUF.Any(y => y.UF.ID == uf.ID) ));

            if (idCategoria != 0)
                query = query.Where(x => x.ListaCategoriaConteudo.Any(p => p.ID == idCategoria) || !x.ListaCategoriaConteudo.Any());

            return query.ToList();
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
    }
}

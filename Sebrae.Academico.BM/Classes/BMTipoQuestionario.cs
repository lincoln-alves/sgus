using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Classes
{
    public class BMTipoQuestionario : BusinessManagerBase
    {

        private RepositorioBase<TipoQuestionario> repositorio;

        public BMTipoQuestionario()
        {
            repositorio = new RepositorioBase<TipoQuestionario>();
        }

        public void ValidarTipoQuestionario(TipoQuestionario pTipoQuestionario)
        {

            ValidarDependencias(pTipoQuestionario);

            if (string.IsNullOrWhiteSpace(pTipoQuestionario.Nome))
                throw new AcademicoException("Nome. Campo Obrigatório");

        }

        public void Salvar(TipoQuestionario pTipoQuestionario)
        {
            ValidarTipoQuestionario(pTipoQuestionario);
            repositorio.Salvar(pTipoQuestionario);
        }

        public TipoQuestionario ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        /// <summary>
        /// Lista Todos os Tipos de Questionário, com exceção do tipo dinâmico.
        /// </summary>
        /// <returns></returns>
        public IList<TipoQuestionario> ObterTodos()
        {
            return ObterTodosIQueryable().ToList();
        }

        /// <summary>
        /// Lista Todos os Tipos de Questionário, com exceção do tipo dinâmico.
        /// </summary>
        /// <returns></returns>
        public IQueryable<TipoQuestionario> ObterTodosIQueryable()
        {
            return
                repositorio.session.Query<TipoQuestionario>()
                    .Where(x => x.ID != (int)enumTipoQuestionario.Dinamico);
        }

        public IList<TipoQuestionario> ObterPorFiltro(TipoQuestionario pTipoQuestionario)
        {
            var query = repositorio.session.Query<TipoOferta>();

            if (pTipoQuestionario != null)
            {
                if (!string.IsNullOrWhiteSpace(pTipoQuestionario.Nome))
                    query = query.Where(x => x.Nome.Contains(pTipoQuestionario.Nome));
            }

            query = query.Where(x => x.ID != (int)enumTipoQuestionario.Dinamico);

            return query.Select(x => new TipoQuestionario() { ID = x.ID, Nome = x.Nome }).ToList<TipoQuestionario>();
        }

        protected override bool ValidarDependencias(object pTipoQuestionario)
        {
            TipoQuestionario tipoQuestionario = (TipoQuestionario)pTipoQuestionario;

            return (tipoQuestionario.ListaQuestionario != null && tipoQuestionario.ListaQuestionario.Count > 0);
        }

        public void ExcluirTipoOferta(TipoQuestionario pTipoQuestionario)
        {
            if (ValidarDependencias(pTipoQuestionario))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes deste Tipo de Questionário.");

            repositorio.Excluir(pTipoQuestionario);
        }
    }
}

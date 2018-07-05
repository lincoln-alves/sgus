using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMTipoItemQuestionario : BusinessManagerBase
    {

        private RepositorioBase<TipoItemQuestionario> repositorio;

        public BMTipoItemQuestionario()
        {
            repositorio = new RepositorioBase<TipoItemQuestionario>();
        }

        public void ValidarTipoItemQuestionario(TipoItemQuestionario pTipoItemQuestionario)
        {

            ValidarDependencias(pTipoItemQuestionario);

            if (string.IsNullOrWhiteSpace(pTipoItemQuestionario.Nome))
                throw new AcademicoException("Nome. Campo Obrigatório");

        }

        public void Salvar(TipoItemQuestionario pTipoItemQuestionario)
        {
            ValidarTipoItemQuestionario(pTipoItemQuestionario);
            repositorio.Salvar(pTipoItemQuestionario);
        }
             
        public TipoItemQuestionario ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public IList<TipoItemQuestionario> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public IList<TipoItemQuestionario> ObterPorFiltro(TipoItemQuestionario pTipoItemQuestionario)
        {
            var query = repositorio.session.Query<TipoOferta>();

            if (pTipoItemQuestionario != null)
            {
                if (!string.IsNullOrWhiteSpace(pTipoItemQuestionario.Nome))
                    query = query.Where(x => x.Nome.Contains(pTipoItemQuestionario.Nome));
            }

            return query.Select(x => new TipoItemQuestionario() { ID = x.ID, Nome = x.Nome }).ToList<TipoItemQuestionario>();
        }

        protected override bool ValidarDependencias(object pTipoItemQuestionario)
        {
            TipoItemQuestionario TipoItemQuestionario = (TipoItemQuestionario)pTipoItemQuestionario;

            return (TipoItemQuestionario.ListaItemQuestionario != null && TipoItemQuestionario.ListaItemQuestionario.Count > 0);
        }

        public void ExcluirTipoOferta(TipoItemQuestionario pTipoItemQuestionario)
        {
            if (ValidarDependencias(pTipoItemQuestionario))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes deste Tipo de Item de Questionário.");

            repositorio.Excluir(pTipoItemQuestionario);
        }
    }
}


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
    public class BMEstiloItemQuestionario : BusinessManagerBase
    {
        private RepositorioBase<EstiloItemQuestionario> repositorio;

        public BMEstiloItemQuestionario()
        {
            repositorio = new RepositorioBase<EstiloItemQuestionario>();
        }

        public void ValidarEstiloItemQuestionario(EstiloItemQuestionario pEstiloItemQuestionario)
        {

            ValidarDependencias(pEstiloItemQuestionario);

            if (string.IsNullOrWhiteSpace(pEstiloItemQuestionario.Nome))
                throw new AcademicoException("Nome. Campo Obrigatório");

        }

        public void Salvar(EstiloItemQuestionario pEstiloItemQuestionario)
        {
            ValidarEstiloItemQuestionario(pEstiloItemQuestionario);
            repositorio.Salvar(pEstiloItemQuestionario);
        }

        public EstiloItemQuestionario ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        /// <summary>
        /// Lista Todos os Estilos de Item de Questionário.
        /// </summary>
        /// <returns></returns>
        public IList<EstiloItemQuestionario> ObterTodos()
        {
            var query = repositorio.session.Query<EstiloItemQuestionario>();
            IList<EstiloItemQuestionario> ListaEstiloItemQuestionario = query.ToList<EstiloItemQuestionario>();
            return ListaEstiloItemQuestionario;
        }

        public IList<EstiloItemQuestionario> ObterPorFiltro(EstiloItemQuestionario pEstiloItemQuestionario)
        {
            var query = repositorio.session.Query<EstiloItemQuestionario>();

            if (pEstiloItemQuestionario != null)
            {
                if (!string.IsNullOrWhiteSpace(pEstiloItemQuestionario.Nome))
                    query = query.Where(x => x.Nome.Contains(pEstiloItemQuestionario.Nome));
            }

            return query.Select(x => new EstiloItemQuestionario() { ID = x.ID, Nome = x.Nome }).ToList<EstiloItemQuestionario>();
        }

        protected override bool ValidarDependencias(object pEstiloItemQuestionario)
        {
            EstiloItemQuestionario estiloItemQuestionario = (EstiloItemQuestionario)pEstiloItemQuestionario;

            return (estiloItemQuestionario.ListaItemQuestionario != null && estiloItemQuestionario.ListaItemQuestionario.Count > 0);
        }

        public void ExcluirEstiloItemQuestionario(EstiloItemQuestionario pEstiloItemQuestionario)
        {
            if (ValidarDependencias(pEstiloItemQuestionario))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes deste Estilo de Item de Questionário.");

            repositorio.Excluir(pEstiloItemQuestionario);
        }
    }
}

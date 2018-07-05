using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMTipoQuestionarioAssociacao : BusinessManagerBase
    {

        private RepositorioBase<TipoQuestionarioAssociacao> repositorio;

        public BMTipoQuestionarioAssociacao()
        {
            repositorio = new RepositorioBase<TipoQuestionarioAssociacao>();
        }

        public void ValidarTipoQuestionarioAssociacao(TipoQuestionarioAssociacao pTipoQuestionarioAssociacao)
        {

            ValidarDependencias(pTipoQuestionarioAssociacao);

            if (string.IsNullOrWhiteSpace(pTipoQuestionarioAssociacao.Nome))
                throw new AcademicoException("Nome. Campo Obrigatório");

        }

        public void Salvar(TipoQuestionarioAssociacao pTipoQuestionarioAssociacao)
        {
            ValidarTipoQuestionarioAssociacao(pTipoQuestionarioAssociacao);
            repositorio.Salvar(pTipoQuestionarioAssociacao);
        }

        public TipoQuestionarioAssociacao ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        /// <summary>
        /// Lista Todos os Tipos de Questionário Associação, com exceção do tipo dinâmico.
        /// </summary>
        /// <returns></returns>
        public IList<TipoQuestionarioAssociacao> ObterTodos()
        {
            var query = repositorio.session.Query<TipoQuestionarioAssociacao>();
            IList<TipoQuestionarioAssociacao> ListaTipoQuestionarioAssociacao = query.ToList<TipoQuestionarioAssociacao>();
            return ListaTipoQuestionarioAssociacao;
        }
        
        protected override bool ValidarDependencias(object pTipoQuestionarioAssociacao)
        {
            TipoQuestionarioAssociacao tipoQuestionarioAssociacao = (TipoQuestionarioAssociacao)pTipoQuestionarioAssociacao;

            return (tipoQuestionarioAssociacao.ListaQuestionarioAssociacao != null && tipoQuestionarioAssociacao.ListaQuestionarioAssociacao.Count > 0);
        }

        public void ExcluirTipoOferta(TipoQuestionarioAssociacao pTipoQuestionarioAssociacao)
        {
            if (ValidarDependencias(pTipoQuestionarioAssociacao))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes deste Tipo de Questionário.");

            repositorio.Excluir(pTipoQuestionarioAssociacao);
        }
    }
}

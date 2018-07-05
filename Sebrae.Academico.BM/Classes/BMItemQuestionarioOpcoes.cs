using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMItemQuestionarioOpcoes : BusinessManagerBase
    {
        #region Atributos

        private RepositorioBase<ItemQuestionarioOpcoes> repositorio = null;

        #endregion

        #region Construtor

        public BMItemQuestionarioOpcoes()
        {
            repositorio = new RepositorioBase<ItemQuestionarioOpcoes>();
        }

        #endregion

        #region "Métodos Privados"

        private void ValidarItemQuestionarioInformado(ItemQuestionarioOpcoes pItemQuestionarioOpcoes)
        {
            ValidarInstancia(pItemQuestionarioOpcoes);

            //Nome
            if (string.IsNullOrWhiteSpace(pItemQuestionarioOpcoes.Nome)) throw new AcademicoException("Nome. Campo Obrigatório");

        }

        #endregion

        #region "Métodos Públicos"

        public void Salvar(ItemQuestionarioOpcoes pItemQuestionarioOpcoes)
        {
            ValidarItemQuestionarioInformado(pItemQuestionarioOpcoes);
            repositorio.Salvar(pItemQuestionarioOpcoes);
        }

        public void Excluir(ItemQuestionarioOpcoes pItemQuestionarioOpcoes)
        {
            if (this.ValidarDependencias(pItemQuestionarioOpcoes))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes deste Item de Questionário.");

            repositorio.Excluir(pItemQuestionarioOpcoes);
        }

        public IList<ItemQuestionarioOpcoes> ObterTodos()
        {
            return repositorio.ObterTodos().OrderBy(x => x.Nome).ToList<ItemQuestionarioOpcoes>();
        }

        public ItemQuestionarioOpcoes ObterPorID(int pId)
        {

            ItemQuestionarioOpcoes itemQuestionarioOpcoes = null;
            var query = repositorio.session.Query<ItemQuestionarioOpcoes>();
            itemQuestionarioOpcoes = query.FirstOrDefault(x => x.ID == pId);
            return itemQuestionarioOpcoes;
        }

        #endregion

    }

}

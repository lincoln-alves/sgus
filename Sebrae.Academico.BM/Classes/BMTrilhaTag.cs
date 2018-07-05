using System.Collections.Generic;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;


namespace Sebrae.Academico.BM.Classes
{
    public class BMTrilhaTag : BusinessManagerBase
    {

        #region Atributos

        private RepositorioBase<TrilhaTag> repositorio;

        #endregion

        #region "Construtor"


        public BMTrilhaTag()
        {
            repositorio = new RepositorioBase<TrilhaTag>();
        }

        #endregion

        public TrilhaTag ObterPorId(int ID)
        {
            return repositorio.ObterPorID(ID);
        }

        public void Salvar(TrilhaTag pTrilhaPermissao)
        {
            repositorio.Salvar(pTrilhaPermissao);
        }

        public IList<TrilhaTag> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public void ExcluirTrilhaPermissao(TrilhaTag pTrilhaTag)
        {
            repositorio.Excluir(pTrilhaTag);
        }

        protected override bool ValidarDependencias(object pTrilhaTag)
        {
            TrilhaTag TrilhaPermissao = (TrilhaTag)pTrilhaTag;

            return true;
            //return ((TrilhaPermissao.ListaItemTrilha != null && TrilhaPermissao.ListaItemTrilha.Count > 0) ||
            //        (TrilhaPermissao.ListaUsuarioTrilha != null && TrilhaPermissao.ListaUsuarioTrilha.Count > 0));
        }

       
    }
}

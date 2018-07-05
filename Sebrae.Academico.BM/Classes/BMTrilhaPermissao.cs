using System.Collections.Generic;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;


namespace Sebrae.Academico.BM.Classes
{
    public class BMTrilhaPermissao : BusinessManagerBase
    {

        #region Atributos

        private RepositorioBase<TrilhaPermissao> repositorio;

        #endregion

        #region "Construtor"


        public BMTrilhaPermissao()
        {
            repositorio = new RepositorioBase<TrilhaPermissao>();
        }

        #endregion

        public TrilhaPermissao ObterPorId(int ID)
        {
            return repositorio.ObterPorID(ID);
        }
                
        public void Salvar(TrilhaPermissao pTrilhaPermissao)
        {
            repositorio.Salvar(pTrilhaPermissao);
        }

        public IList<TrilhaPermissao> ObterTodos()
        {
            return repositorio.ObterTodos();
        }
           
        public void ExcluirTrilhaPermissao(TrilhaPermissao pTrilhaPermissao)
        {
            //if (this.ValidarDependencias(pTrilhaPermissao))
            //    throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes deste Trilha Permissão.");

            repositorio.Excluir(pTrilhaPermissao);
        }

        protected override bool ValidarDependencias(object pTrilhaPermissao)
        {
            TrilhaPermissao TrilhaPermissao = (TrilhaPermissao)pTrilhaPermissao;

            return true;
            //return ((TrilhaPermissao.ListaItemTrilha != null && TrilhaPermissao.ListaItemTrilha.Count > 0) ||
            //        (TrilhaPermissao.ListaUsuarioTrilha != null && TrilhaPermissao.ListaUsuarioTrilha.Count > 0));
        }

       
    }
}


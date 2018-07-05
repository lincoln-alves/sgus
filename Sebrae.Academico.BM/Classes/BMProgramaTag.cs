using System.Collections.Generic;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;


namespace Sebrae.Academico.BM.Classes
{
    public class BMProgramaTag : BusinessManagerBase
    {

        #region Atributos

        private RepositorioBase<ProgramaTag> repositorio;

        #endregion

        #region "Construtor"


        public BMProgramaTag()
        {
            repositorio = new RepositorioBase<ProgramaTag>();
        }

        #endregion

        public ProgramaTag ObterPorId(int ID)
        {
            return repositorio.ObterPorID(ID);
        }

        public void Salvar(ProgramaTag pProgramaTag)
        {
            repositorio.Salvar(pProgramaTag);
        }

        public IList<ProgramaTag> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public void ExcluirProgramaTag(ProgramaTag pProgramaTag)
        {
            repositorio.Excluir(pProgramaTag);
        }

        protected override bool ValidarDependencias(object pProgramaTag)
        {
            ProgramaTag programaPermissao = (ProgramaTag)pProgramaTag;

            return true;
            //return ((programaPermissao.ListaItemTrilha != null && programaPermissao.ListaItemTrilha.Count > 0) ||
            //        (programaPermissao.ListaUsuarioTrilha != null && programaPermissao.ListaUsuarioTrilha.Count > 0));
        }

       
    }
}

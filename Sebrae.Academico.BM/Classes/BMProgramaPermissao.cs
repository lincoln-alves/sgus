using System.Collections.Generic;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;


namespace Sebrae.Academico.BM.Classes
{
    public class BMProgramaPermissao : BusinessManagerBase
    {

        #region Atributos

        private RepositorioBase<ProgramaPermissao> repositorio;

        #endregion

        #region "Construtor"


        public BMProgramaPermissao()
        {
            repositorio = new RepositorioBase<ProgramaPermissao>();
        }

        #endregion

        public ProgramaPermissao ObterPorId(int ID)
        {
            return repositorio.ObterPorID(ID);
        }
                
        public void Salvar(ProgramaPermissao pProgramaPermissao)
        {
            repositorio.Salvar(pProgramaPermissao);
        }

        public IList<ProgramaPermissao> ObterTodos()
        {
            return repositorio.ObterTodos();
        }
           
        public void ExcluirProgramaPermissao(ProgramaPermissao pProgramaPermissao)
        {
            //if (this.ValidarDependencias(pProgramaPermissao))
            //    throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes deste Programa Permissão.");

            repositorio.Excluir(pProgramaPermissao);
        }

        protected override bool ValidarDependencias(object pProgramaPermissao)
        {
            ProgramaPermissao programaPermissao = (ProgramaPermissao)pProgramaPermissao;

            return true;
            //return ((programaPermissao.ListaItemTrilha != null && programaPermissao.ListaItemTrilha.Count > 0) ||
            //        (programaPermissao.ListaUsuarioTrilha != null && programaPermissao.ListaUsuarioTrilha.Count > 0));
        }

       
    }
}

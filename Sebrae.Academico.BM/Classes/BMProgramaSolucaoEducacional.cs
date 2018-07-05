using System.Collections.Generic;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;

namespace Sebrae.Academico.BM.Classes
{
    public class BMProgramaSolucaoEducacional : BusinessManagerBase
    {

        #region Atributos

        private RepositorioBase<ProgramaSolucaoEducacional> repositorio;

        #endregion

        #region "Construtor"


        public BMProgramaSolucaoEducacional()
        {
            repositorio = new RepositorioBase<ProgramaSolucaoEducacional>();
        }

        #endregion

        public ProgramaSolucaoEducacional ObterPorId(int ID)
        {
            return repositorio.ObterPorID(ID);
        }

        public void Salvar(ProgramaSolucaoEducacional pProgramaSolucaoEducacional)
        {
            repositorio.Salvar(pProgramaSolucaoEducacional);
        }

        public IList<ProgramaSolucaoEducacional> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public void ExcluirProgramaSolucaoEducacional(ProgramaSolucaoEducacional pProgramaSolucaoEducacional)
        {
            repositorio.Excluir(pProgramaSolucaoEducacional);
        }

        protected override bool ValidarDependencias(object pProgramaSolucaoEducacional)
        {
            ProgramaSolucaoEducacional programaSolucaoEducacional = (ProgramaSolucaoEducacional)pProgramaSolucaoEducacional;

            return true;
            //return ((programaPermissao.ListaItemTrilha != null && programaPermissao.ListaItemTrilha.Count > 0) ||
            //        (programaPermissao.ListaUsuarioTrilha != null && programaPermissao.ListaUsuarioTrilha.Count > 0));
        }

       
    }
}

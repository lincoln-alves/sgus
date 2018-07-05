using System;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterPerfil : BusinessProcessBase, IDisposable
    {
        private BMPerfil bmPerfil = null;

        public ManterPerfil()
            : base()
        {
            bmPerfil = new BMPerfil();
        }

        public void IncluirPerfil(Perfil pPerfil)
        {
            bmPerfil.Salvar(pPerfil);
        }

        public void AlterarPerfil(Perfil pPerfil)
        {
            bmPerfil.ValidarInstancia(pPerfil);
        }

        public void ExcluirPerfil(int IdPerfil)
        {            
            try
            {
                Perfil perfil = null;

                if (IdPerfil > 0)
                {
                    perfil = bmPerfil.ObterPorId(IdPerfil);
                }

                bmPerfil.Excluir(perfil);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }

        }

        public IList<Perfil> ObterPerfilPorFiltro(Perfil pPerfil)
        {
            return bmPerfil.ObterPorFiltro(pPerfil);
        }

        public IList<Perfil> ObterTodosPerfis()
        {
            return bmPerfil.ObterTodos();
        }

        public Perfil ObterPerfilPorID(int pId)
        {
            return bmPerfil.ObterPorId(pId);
        }

        public void Dispose()
        {
            if (bmPerfil != null)
                bmPerfil.Dispose();
            GC.Collect();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterSistemaExterno : BusinessProcessBase
    {

        private BMSistemaExterno bmSistemaExterno = null;

        public ManterSistemaExterno()
            : base()
        {
            bmSistemaExterno = new BMSistemaExterno();
        }

        public void IncluirSistemaExterno(SistemaExterno pSistemaExterno)
        {
            try
            {
                base.PreencherInformacoesDeAuditoria(pSistemaExterno);
                bmSistemaExterno.Salvar(pSistemaExterno);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        private bool VerificarExistenciaDoPerfiNaLista(IList<SistemaExternoPermissao> ListaSistemaExternoPermissao,
                                                       SistemaExternoPermissao SistemaExternoPermissao)
        {
            return ListaSistemaExternoPermissao.Where(x => x.Perfil.ID == SistemaExternoPermissao.Perfil.ID).Any();
        }

        public void AlterarSistemaExterno(SistemaExterno pSistemaExterno)
        {
            try
            {
                base.PreencherInformacoesDeAuditoria(pSistemaExterno);
                bmSistemaExterno.Salvar(pSistemaExterno);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExcluirSistemaExterno(int IdSistemaExterno)
        {
            try
            {
                SistemaExterno sistemaExterno = null;

                if (IdSistemaExterno > 0)
                {
                    sistemaExterno = bmSistemaExterno.ObterPorId(IdSistemaExterno);
                }

                bmSistemaExterno.Excluir(sistemaExterno);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }
                
        public IList<SistemaExterno> ObterTodosSistemaExternos()
        {
            return bmSistemaExterno.ObterTodos();
        }

        public SistemaExterno ObterSistemaExternoPorID(int pId)
        {
            return bmSistemaExterno.ObterPorId(pId);
        }

        public IList<SistemaExterno> ObterSistemaExternoPorFiltro(SistemaExterno pSistemaExterno)
        {
            return bmSistemaExterno.ObterSistemaExternoPorFiltro(pSistemaExterno);
        }

        public IList<SistemaExterno> ObterTodosPorUsuario(Usuario usuario)
        {
            return bmSistemaExterno.ObterTodosPorUsuario(usuario).ToList();
        } 
    }
}

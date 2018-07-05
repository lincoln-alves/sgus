using System.Collections.Generic;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucDashboardNotificacoes : BaseUserControl
    {

        protected override IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get
            {
                IList<enumPerfil> perfisNecessariosParaAcessarAFuncionalidade = new List<enumPerfil>();
                perfisNecessariosParaAcessarAFuncionalidade.Add(enumPerfil.Administrador);
                perfisNecessariosParaAcessarAFuncionalidade.Add(enumPerfil.GestorUC);
                return perfisNecessariosParaAcessarAFuncionalidade;
            }
        }

        public void CarregarInformacoesSobreNotificacoes()
        {
            try
            {
                IList<DTONotificacao> ListaNotificacoesDoUsuario = new ManterDashBoard().ListaNotificacoesMaisAcessadasPorUsuario();

                if (ListaNotificacoesDoUsuario != null && ListaNotificacoesDoUsuario.Count > 0)
                {
                    WebFormHelper.PreencherGrid(ListaNotificacoesDoUsuario, dgvNotificacoes);
                }
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

    }
}
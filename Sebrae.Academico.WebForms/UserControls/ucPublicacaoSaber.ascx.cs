using System.Collections.Generic;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucPublicacaoSaber : System.Web.UI.UserControl
    {
    
        public int IdUsuario
        {
            get
            {
                if (ViewState["ViewStateIdUsuario"] != null)
                {
                    return (int)ViewState["ViewStateIdUsuario"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateIdUsuario"] = value;
            }

        }

        public void CarregarInformacoesSobrePublicacoesDoSaber()
        {
            try
            {

                if (IdUsuario > 0)
                {

                    Usuario usuario = new ManterUsuario().ObterUsuarioPorID(this.IdUsuario);

                    if (usuario != null)
                    {
                        IList<PublicacaoSaberUsuario> ListaPublicacaoSaberUsuario = new ManterPublicacaoSaberUsuario().ObterPorIdUsuario(this.IdUsuario);
                        WebFormHelper.PreencherGrid(ListaPublicacaoSaberUsuario, this.dgvPublicacaoDoSaber);
                    }
                }

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }
    }
}
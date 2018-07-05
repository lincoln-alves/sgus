using System.Collections.Generic;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Web;

namespace Sebrae.Academico.WebForms.UserControls
{
    /// <summary>
    /// User Control com atividades extra-curriculares.
    /// </summary>
    public partial class ucAtividadeExtraCurricular : System.Web.UI.UserControl
    {
        /// <summary>
        /// Carrega as atividades extra-curriculares
        /// </summary>
        public void CarregarHistoricoExtraCurricularDoUsuario()
        {
            try
            {

                if (this.IdUsuario > 0)
                {
                    IList<HistoricoExtraCurricular> ListaHistoricoExtraCurricularDoUsuario = new ManterHistoricoExtraCurricular().ObterPorUsuario(this.IdUsuario);
                    WebFormHelper.PreencherGrid(ListaHistoricoExtraCurricularDoUsuario, dgvAtividadeExtraCurricular);
                }

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

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

        protected void dgvAtividadeExtraCurricular_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                HistoricoExtraCurricular historicoExtraCurricular = (HistoricoExtraCurricular)e.Row.DataItem;

                if (historicoExtraCurricular != null)
                {
                    HyperLink hlnkDownload = (HyperLink)e.Row.FindControl("hlnkDownload");

                    if (hlnkDownload != null)
                    {
                        if (historicoExtraCurricular.FileServer != null && historicoExtraCurricular.FileServer.ID > 0)
                        {
                            hlnkDownload.NavigateUrl = string.Format("/MediaServer.ashx?Identificador={0}", historicoExtraCurricular.FileServer.ID);
                        }
                    }
                }
            }

        }
    }
}
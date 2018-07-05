using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.BP.DTO.Relatorios;

namespace Sebrae.Academico.WebForms.Relatorios.UsuarioCadastrado
{
    public partial class UsuariosCadastrados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (RelatorioUsuarioCadastrado relUsuarioCadastrado = new RelatorioUsuarioCadastrado())
                {
                    WebFormHelper.PreencherLista(relUsuarioCadastrado.ObterPerfilTodos(), cbxPerfil, true, false);
                    WebFormHelper.PreencherLista(relUsuarioCadastrado.ObterNivelOcupacionalTodos(), cbxNivelOcupacional, true, false);
                    WebFormHelper.PreencherLista(relUsuarioCadastrado.ObterStatusMatriculaTodos(), cbxStatus, true, false);
                    WebFormHelper.PreencherLista(relUsuarioCadastrado.ObterUfTodos(), cbxUF, true, false);
                }
            }
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid<DTOUsuarioCadastrado>((IList<DTOUsuarioCadastrado>)Session["dsRelatorio"], dgRelatorio, e.NewPageIndex);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid<DTOUsuarioCadastrado>((IList<DTOUsuarioCadastrado>)Session["dsRelatorio"], dgRelatorio, e.SortExpression, e.SortDirection, "dsRelatorio");
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {

        }
    }
}
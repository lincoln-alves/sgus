using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.WebForms.UserControls;
using System.Collections;
using Microsoft.Reporting.WebForms;

namespace Sebrae.Academico.WebForms.Relatorios
{
    public partial class RelatorioIndividual : System.Web.UI.Page
    {
        private RelatatorioIndividual relatoriosIndividuais = new RelatatorioIndividual();

        protected void Page_Init(object sender, EventArgs e)
        {
            //ScriptManager1.EnablePartialRendering = false;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void UserSelectedHandler(object sender, CompleteUserSelectionEvent e)
        {
            ExecutaRelatorio();
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            ExecutaRelatorio();
        }

        private void ExecutaRelatorio()
        {

            if (txtUsuarioSelect.SelectedUser == null)
                return;

            IList lstResult = relatoriosIndividuais.ExecutarRelatoriosIndividuais(txtUsuarioSelect.SelectedUser.ID);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsRelatorioIndividual", lstResult));
            ReportViewer1.LocalReport.Refresh();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP.Relatorios;
using System.Collections;
using Microsoft.Reporting.WebForms;

namespace Sebrae.Academico.WebForms.Relatorios
{
    public partial class ParticipacaoTrilha : System.Web.UI.Page
    {
        private RelatorioParticipacaoTrilha relatorioBP = new RelatorioParticipacaoTrilha();
        private ManterTrilha manterTrilha = new ManterTrilha();
        private ManterTrilhaNivel manterTrilhaNivel = new ManterTrilhaNivel();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            WebFormHelper.PreencherLista(manterTrilha.ObterTodasTrilhas(), ddlTrilha);
            WebFormHelper.PreencherLista(manterTrilhaNivel.ObterTodosTrilhaNivel(), ddlTrilhaNivel);

            ExecutaRelatorio();

            
        }

        

        private void ExecutaRelatorio()
        {
            IList lstResult = relatorioBP.ExecutarRelatorioParticipacaoTrilha(int.Parse(ddlTrilha.SelectedItem.Value == "" ? "0" : ddlTrilha.SelectedItem.Value),
                                                                              int.Parse(ddlTrilhaNivel.SelectedItem.Value == "" ? "0" : ddlTrilhaNivel.SelectedItem.Value));

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsParticipacaoTrilha", lstResult));
            ReportViewer1.LocalReport.Refresh();
        }

        protected void ddlTrilhaNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExecutaRelatorio();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Mapeamentos.Procedures;
using Sebrae.Academico.Dominio.DTO;

namespace Sebrae.Academico.WebForms.Graficos
{
    public partial class ParticipantesPorUnidade : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var dadosRelatorio = new ProcParticipantesPorUnidade().PegarDadosRelatorio();

            dadosRelatorio.Add(new Dominio.DTO.DtoParticipantesPorUnidade()
            {
                Unidade = "Total",
                Participantes = dadosRelatorio.Sum(d => d.Participantes)
            });

            rptRelatorio.DataSource = dadosRelatorio;
            rptRelatorio.DataBind();

        }

        protected void rptRelatorio_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal lbl = (Literal)e.Item.FindControl("literal");
                var item = e.Item.DataItem as DtoParticipantesPorUnidade;

                lbl.Text = "['" + item.Unidade + "', " + item.Participantes+ "]";

                if (e.Item.ItemIndex != -1)
                {
                    lbl.Text = lbl.Text + ",";
                }
            }
        }
    }
}
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
    public partial class SolucoesEducacionaisPorCategoria : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new ProcSolucoesEducacionaisPorCategoria().PegarDadosRelatorio();

            resultado.Add(new Dominio.DTO.DtoSolucoesEducacionaisPorCategoria()
            {
                Categoria = "Total Geral",
                Concluintes = resultado.Sum(r => r.Concluintes)
            });

            rptRelatorio.DataSource = resultado;
            rptRelatorio.DataBind();

            //MergeEqualCells(grdRelatorio);
        }

        protected void rptRelatorio_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal lbl = (Literal)e.Item.FindControl("literal");
                var item = e.Item.DataItem as DtoSolucoesEducacionaisPorCategoria;

                lbl.Text = "['" + item.Categoria + "', '" + item.SolucaoEducacional + "', " + item.Concluintes + "]";

                if (e.Item.ItemIndex != -1)
                {
                    lbl.Text = lbl.Text + ",";
                }
            }
        }

        private void MergeEqualCells(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (row.Cells[i].Text == previousRow.Cells[i].Text)
                    {
                        row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                               previousRow.Cells[i].RowSpan + 1;
                        previousRow.Cells[i].Visible = false;
                    }
                }
            }
        }
    }
}
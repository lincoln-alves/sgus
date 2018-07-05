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
    public partial class TotalizacoesCursosOferecidos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new ProcTotalizacoesCursosOferecidos().PegarDadosRelatorio();

            rptRelatorio.DataSource = resultado;
            rptRelatorio.DataBind();
        }

        protected void rptRelatorio_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal lbl = (Literal)e.Item.FindControl("literal");
                var item = e.Item.DataItem as DtoTotalizacoesCursosOferecidos;

                lbl.Text = "[" + item.Quantidade + ", '" + item.Descricao + "']";

                if (e.Item.ItemIndex != -1)
                {
                    lbl.Text = lbl.Text + ",";
                }
            }
        }
    }
}
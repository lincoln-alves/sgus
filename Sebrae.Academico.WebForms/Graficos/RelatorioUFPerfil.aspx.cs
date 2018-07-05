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
    public partial class RelatorioUFPerfil : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var dadosRelatorio = new ProcRelatorioUFPerfil().PegarDadosRelatorio();

            var ufs = dadosRelatorio.Select(r => r.UF).Distinct().ToList();
            var perfis = dadosRelatorio.Select(r => r.NivelOcupacional).Distinct().ToList();


            literalTitulos.Text = "['UF'";

            foreach (var perfil in perfis)
                literalTitulos.Text += ", '" + perfil + "'";

            literalTitulos.Text += ", 'Total']";

            var linhas = new List<string>();

            foreach (var uf in ufs)
            {
                string linha = "['" + uf + "'";

                foreach (var perfil in perfis)
                {
                    var registro = dadosRelatorio.Where(r => r.NivelOcupacional == perfil && r.UF == uf).Select(r => r.Quantidade).FirstOrDefault();

                    linha += ", '" + registro + "'";
                }

                linha += ", '" + dadosRelatorio.Where(r => r.UF == uf).Sum(r => r.Quantidade) + "']";

                linhas.Add(linha);
            }

            rptRelatorio.DataSource = linhas;
            rptRelatorio.DataBind();

        }


        protected void rptRelatorio_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal lbl = (Literal)e.Item.FindControl("literalDados");
                var item = e.Item.DataItem as String;

                lbl.Text = item;

                if (e.Item.ItemIndex != -1)
                {
                    lbl.Text = lbl.Text + ",";
                }
            }
        }



    }
}
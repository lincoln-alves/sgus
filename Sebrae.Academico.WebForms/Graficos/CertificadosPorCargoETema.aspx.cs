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
    public partial class CertificadosPorCargoETema : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var dadosRelatorio = new ProcCertificadosPorCargoETema().PegarDadosRelatorio();

            var solucoesEducacionais = dadosRelatorio.OrderBy(n => n.SolucaoEducacional).Select(n => n.SolucaoEducacional).Distinct().ToList();

            for (int coluna = 1; coluna <= 5; coluna++)
            {
                if (solucoesEducacionais.Count >= coluna)
                    grdRelatorio.Columns[coluna].HeaderText = solucoesEducacionais[coluna - 1];
            }

            var resultado = dadosRelatorio.GroupBy(n => n.NivelOcupacional).Select(n => new ResultadoGridView
            {
                EspacoOcupacional = n.Key,
                SolucaoEducacional1 = solucoesEducacionais.Count >= 1 ? n.Where(g => g.SolucaoEducacional == solucoesEducacionais[0]).Sum(g => g.Total) : 0,
                SolucaoEducacional2 = solucoesEducacionais.Count >= 2 ? n.Where(g => g.SolucaoEducacional == solucoesEducacionais[1]).Sum(g => g.Total) : 0,
                SolucaoEducacional3 = solucoesEducacionais.Count >= 3 ? n.Where(g => g.SolucaoEducacional == solucoesEducacionais[2]).Sum(g => g.Total) : 0,
                SolucaoEducacional4 = solucoesEducacionais.Count >= 4 ? n.Where(g => g.SolucaoEducacional == solucoesEducacionais[3]).Sum(g => g.Total) : 0,
                SolucaoEducacional5 = solucoesEducacionais.Count >= 5 ? n.Where(g => g.SolucaoEducacional == solucoesEducacionais[4]).Sum(g => g.Total) : 0,
                Certificacoes = n.Sum(g => g.Total)
            }).ToList();

            resultado.Add(new ResultadoGridView()
            {
                EspacoOcupacional = "Total Geral",
                SolucaoEducacional1 = resultado.Sum(r => r.SolucaoEducacional1),
                SolucaoEducacional2 = resultado.Sum(r => r.SolucaoEducacional2),
                SolucaoEducacional3 = resultado.Sum(r => r.SolucaoEducacional3),
                SolucaoEducacional4 = resultado.Sum(r => r.SolucaoEducacional4),
                SolucaoEducacional5 = resultado.Sum(r => r.SolucaoEducacional5),
                Certificacoes = resultado.Sum(r => r.Certificacoes)
            });

            grdRelatorio.DataSource = resultado;
            grdRelatorio.DataBind();

            var teste = solucoesEducacionais.Aggregate((s1, s2) => "'" + s1 + ", " + s2 + "'");

            literalTitulos.Text = "['Espaço Ocupacional'";
            
            foreach (var item in solucoesEducacionais)
	        {
                literalTitulos.Text += ", '" + item + "'";
            }

            literalTitulos.Text += "]";
            
            rptRelatorio.DataSource = resultado;
            rptRelatorio.DataBind();
        }

        protected void rptRelatorio_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal lbl = (Literal)e.Item.FindControl("literalDados");
                var item = e.Item.DataItem as ResultadoGridView;

                lbl.Text = "['" + item.EspacoOcupacional + "', '" + item.SolucaoEducacional1 + "', '" + item.SolucaoEducacional2 + 
                     "', '" + item.SolucaoEducacional3 + "', '" + item.SolucaoEducacional4 + "', '" + item.SolucaoEducacional5 + "']";

                if (e.Item.ItemIndex != -1)
                {
                    lbl.Text = lbl.Text + ",";
                }
            }
        }

        protected void grdRelatorio_DataBound(object sender, EventArgs e)
        {
            //foreach (var column in grdRelatorio.Columns)
            //{
            //    if (column == grdRelatorio.Columns[0])
            //    {
            //        ((BoundColumn)column).FooterText = "Total Geral";
            //    }
            //    else
            //    {
            //        ((BoundColumn)column).FooterText = 

            //    }
            //}

        }

        public class ResultadoGridView
        {
            public string EspacoOcupacional { get; set; }
            public int SolucaoEducacional1 { get; set; }
            public int SolucaoEducacional2 { get; set; }
            public int SolucaoEducacional3 { get; set; }
            public int SolucaoEducacional4 { get; set; }
            public int SolucaoEducacional5 { get; set; }
            public int Certificacoes { get; set; }
        }
    }
}
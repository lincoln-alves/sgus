using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;
using Sebrae.Academico.BP.Relatorios.Dashboard;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucGraficoMatriculadosUF : UserControl{
        IList<DTOMatriculasPorUF> ListaMatriculas { get; set; }

        public DateTime StartDate;
        public DateTime EndDate;
        public int? ufQuery;

        protected void Page_Load(object sender, EventArgs e)
        {            
            try
            {
                // Se todos os parâmetros forem os padrões da página incial será trazido o dado cacheado
                if (Dashboard.defaultStartDate == StartDate &&
                    Dashboard.defaultEndDate == EndDate)
                {
                    var cacheData = new CacheData("ucGraficoMatriculadosUF", 24);
                    if (cacheData.HasValidCacheData())
                    {
                        ListaMatriculas = cacheData.GetCacheData();
                    }
                    else
                    {
                        ListaMatriculas = new RelatorioMatriculasPorUF().ObterMatriculas(StartDate, EndDate);
                        // Queremos que expire na virada de dia e não dure exatamente 24 horas
                        cacheData.SetCacheData(ListaMatriculas, typeof(IList<DTOMatriculasPorUF>), DateTime.Today.AddDays(1));
                    }
                }
                else
                {
                    ListaMatriculas = new RelatorioMatriculasPorUF().ObterMatriculas(StartDate, EndDate);
                }

            }
            catch 
            {
                pnlAviso.Visible = true;
            }
        }

        public string PorcentagemEstado(enumUF uf)
        {
            if (ListaMatriculas == null) return "0%";
            var reg = ListaMatriculas.FirstOrDefault(p => p.Uf == uf);
            if (reg == null) return "0%";
            return reg.Porcentagem + "%";
        }

        public string CorEstado(enumUF uf)
        {
            if (ListaMatriculas == null) return "#2469ae";
            var reg = ListaMatriculas.FirstOrDefault(p => p.Uf == uf);
            if (reg == null) return "#b7cadc";
            if (reg.Porcentagem <= 20)
            {
                return "#b7cadc";
            }
            if (reg.Porcentagem <= 40)
            {
                return "#92b7dc";
            }
            if (reg.Porcentagem <= 60)
            {
                return "#6ea5dc";
            }
            if (reg.Porcentagem <= 80)
            {
                return "#4992dc";
            }
            return "#2469ae";
        }
    }
}
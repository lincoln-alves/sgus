using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;
using Sebrae.Academico.BP.Relatorios.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.UserControls.dashboard
{
    public partial class ucGraficoMatriculadosPorMes : System.Web.UI.UserControl
    {
        public DateTime StartDate;
        public DateTime EndDate;
        public int? ufQuery;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //...
            }
        }

        //Função chamada pelo <%= %> na página ascx
        public string PegaResultadoSp() {
            
            var idUf = 0;
            int.TryParse(ufQuery.ToString(), out idUf);

            IList<DTOMatriculasPorMes> listaMatriculas;

            // Se todos os parâmetros forem os padrões da página incial será trazido o dado cacheado
            if (Dashboard.defaultStartDate == StartDate &&
                Dashboard.defaultEndDate == EndDate &&
                idUf == Dashboard.defaultUf)
            {
                var cacheData = new CacheData("ucGraficoMatriculadosPorMes", 24);
                if (cacheData.HasValidCacheData())
                {
                    listaMatriculas = cacheData.GetCacheData();
                }
                else
                {
                    listaMatriculas = new RelatorioMatriculasPorMes().ObterMatriculas(StartDate, EndDate, idUf);
                    // Queremos que expire na virada de dia e não dure exatamente 24 horas
                    cacheData.SetCacheData(listaMatriculas, typeof(IList<DTOMatriculasPorMes>), DateTime.Today.AddDays(1));
                }
            }
            else
            {
                listaMatriculas = new RelatorioMatriculasPorMes().ObterMatriculas(StartDate, EndDate, idUf);
            }

            var resultado = listaMatriculas.Aggregate("", (current, item) => current + ("['" + item.Mes + "', " + item.TotalMatriculas + ", " + item.TotalAprovados + "],"));
            if(resultado.Length > 0) resultado = resultado.Substring(0, resultado.Length - 1);
            return resultado;
        }
    }
}
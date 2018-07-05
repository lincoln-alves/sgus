using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;
using Sebrae.Academico.BP.Relatorios.Dashboard;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.UserControls.dashboard
{
    public partial class ucGraficoConcluintesExternos : System.Web.UI.UserControl
    {
        public DateTime StartDate;
        public DateTime EndDate;
        public int? ufQuery;

        public static List<string> Cores = new List<string>()
        {
            "#E6E68A",
            "#87D187",
            "#94E1F5",
            "#CCFF66",
            "#99C2C2",
            "#B280E6",
            "#E0C266",
            "#80B2CC",
            "#FF8566"
        };

        public string CoresGrafico = new JavaScriptSerializer().Serialize(Cores.ToArray());
        public string Titulos { get; set; }
        public string Valores { get; set; }
        public string Legenda { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState["externosDash"] = 1;

                var idUf = 0;
                int.TryParse(ufQuery.ToString(), out idUf);

                IList<DTOConcluintesEspacoOcupacional> lista;

                var relatorioConcluintesEspacoOcupacional = new RelatorioConcluintesEspacoOcupacional();

                // Se todos os parâmetros forem os padrões da página incial será trazido o dado cacheado
                if (Dashboard.defaultStartDate == StartDate &&
                    Dashboard.defaultEndDate == EndDate &&
                    idUf == Dashboard.defaultUf)
                {
                    var cacheData = new CacheData("ucGraficoConcluintesExternos", 24);
                    if (cacheData.HasValidCacheData())
                    {
                        lista = cacheData.GetCacheData();
                    }
                    else
                    {
                        lista = relatorioConcluintesEspacoOcupacional.ObterConcluintesExternos(StartDate, EndDate, idUf);
                        // Queremos que expire na virada de dia e não dure exatamente 24 horas
                        cacheData.SetCacheData(lista, typeof(IList<DTOConcluintesEspacoOcupacional>), DateTime.Today.AddDays(1));
                    }
                }
                else
                {
                    lista = relatorioConcluintesEspacoOcupacional.ObterConcluintesExternos(StartDate, EndDate, idUf);
                }

                Titulos = ObterTitulos(lista);
                Valores = ObterValores(lista);
                Legenda = ObterLegenda(lista);
            }
        }

        protected string ObterTitulos(IList<DTOConcluintesEspacoOcupacional> listaDtoConcluintesEspacoOcupacionals)
        {
            List<string> titulos = new List<string>();
            foreach (var lista in listaDtoConcluintesEspacoOcupacionals)
            {
                if (lista.NomeNivelOcupacional != null)
                {
                    titulos.Add(lista.NomeNivelOcupacional);
                }
            }
            return new JavaScriptSerializer().Serialize(titulos.ToArray());
        }

        protected string ObterValores(IList<DTOConcluintesEspacoOcupacional> listaDtoConcluintesEspacoOcupacionals)
        {
            List<int> titulos = new List<int>();
            foreach (var lista in listaDtoConcluintesEspacoOcupacionals)
            {
                if (lista.NomeNivelOcupacional != null)
                {
                    titulos.Add(lista.QTDConcluintes);
                }
            }
            return new JavaScriptSerializer().Serialize(titulos.ToArray());
        }

        protected string ObterLegenda(IList<DTOConcluintesEspacoOcupacional> listaDtoConcluintesEspacoOcupacionals)
        {
            string legendas = "<ul>";
            int contador = 0;
            int intDivisor = listaDtoConcluintesEspacoOcupacionals.Count / 2;
            foreach (var lista in listaDtoConcluintesEspacoOcupacionals)
            {
                if (lista.NomeNivelOcupacional != null)
                {
                    legendas += "<li style='border-left: 10px solid " + Cores[contador] + "'><strong>" + lista.NomeNivelOcupacional + "</strong></li>";
                    contador++;

                    if (contador == intDivisor)
                    {
                        legendas += "</ul><ul>";
                    }
                }
            }
            legendas += "</ul>";
            return legendas;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;
using Sebrae.Academico.BP.Relatorios.Dashboard;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucGraficoIndiceSatisfacaoCredenciados : UserControl
    {
        public DateTime StartDate;
        public DateTime EndDate;
        public int? ufQuery;

        DTOIndiceSatisfacao indiceSatisfacao { get; set; }
        protected void Page_Load(object sender, EventArgs e){
            if (!IsPostBack){                
                var idUf = 0;
                int.TryParse(ufQuery.ToString(), out idUf);

                try
                {
                    // Se todos os parâmetros forem os padrões da página incial será trazido o dado cacheado
                    if (Dashboard.defaultStartDate == StartDate &&
                        Dashboard.defaultEndDate == EndDate &&
                        idUf == Dashboard.defaultUf)
                    {
                        var cacheData = new CacheData("ucGraficoIndiceSatisfacaoCredenciados", 24);
                        if (cacheData.HasValidCacheData())
                        {
                            indiceSatisfacao = cacheData.GetCacheData();
                        }
                        else
                        {
                            indiceSatisfacao = new RelatorioIndiceSatisfacao().ObterIndiceSatisfacaoCredenciados(StartDate, EndDate, idUf)
                                .FirstOrDefault();
                            // Queremos que expire na virada de dia e não dure exatamente 24 horas
                            cacheData.SetCacheData(indiceSatisfacao, typeof(DTOIndiceSatisfacao), DateTime.Today.AddDays(1));
                        }
                    }
                    else
                    {
                        indiceSatisfacao = new RelatorioIndiceSatisfacao().ObterIndiceSatisfacaoCredenciados(StartDate, EndDate, idUf)
                            .FirstOrDefault();
                    }

                }
                catch
                {
                    pnlGrafico.Visible = false;
                    pnlAviso.Visible = true;
                }
            }
        }
        public string ObterValor()
        {
            if (indiceSatisfacao == null) return "0";
            return indiceSatisfacao.AvaliacaoMedia.ToString().Replace(",", ".");
            //return (((indiceSatisfacao.AvaliacaoMedia*180)/10)+180).ToString().Replace(",",".")+"deg";
        }

        public string ObterValorFormatado()
        {
            if (indiceSatisfacao == null) return "0";
            return indiceSatisfacao.AvaliacaoMedia.ToString();
        }
    }
}
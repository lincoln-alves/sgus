using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;
using Sebrae.Academico.BP.Relatorios.Dashboard;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucGraficoIndiceSatisfacaoGeral : UserControl
    {
        public DateTime StartDate;
        public DateTime EndDate;
        public int? ufQuery;

        DTOIndiceSatisfacao indiceSatisfacao { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var idUf = 0;
                int.TryParse(ufQuery.ToString(), out idUf);

                try
                {
                    // Se todos os parâmetros forem os padrões da página incial será trazido o dado cacheado
                    if (Dashboard.defaultStartDate == StartDate &&
                        Dashboard.defaultEndDate == EndDate &&
                        idUf == Dashboard.defaultUf)
                    {
                        var cacheData = new CacheData("ucGraficoIndiceSatisfacaoGeral", 24);
                        if (cacheData.HasValidCacheData())
                        {
                            indiceSatisfacao = cacheData.GetCacheData();
                        }
                        else
                        {
                            indiceSatisfacao = ObterIndiceSatisfacao(StartDate, EndDate, idUf);
                            // Queremos que expire na virada de dia e não dure exatamente 24 horas
                            cacheData.SetCacheData(indiceSatisfacao, typeof(DTOIndiceSatisfacao), DateTime.Today.AddDays(1) );
                        }
                    }
                    else
                    {
                        indiceSatisfacao = ObterIndiceSatisfacao(StartDate, EndDate, idUf);
                    }
                }
                catch (Exception ex)
                {
                    pnlGrafico.Visible = false;
                    pnlAviso.Visible = true;

                    ltrAvisoErro.Text = ex.Message + "<br/>" + (ex.InnerException != null
                                            ? ex.InnerException.Message
                                            : "");
                }
            }
        }

        public DTOIndiceSatisfacao ObterIndiceSatisfacao(DateTime dataInicio, DateTime dataFim, int idUf)
        {
            return new RelatorioIndiceSatisfacao().ObterIndiceSatisfacao(dataInicio, dataFim, idUf).FirstOrDefault();            
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
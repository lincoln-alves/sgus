using Sebrae.Academico.BP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucRelatorioTop5Cursos : System.Web.UI.UserControl
    {
        public DateTime StartDate;
        public DateTime EndDate;
        public int? ufQuery;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var idUf = 0;
                int.TryParse(ufQuery.ToString(), out idUf);
                DTOTop5Cursos dto;

                // Se todos os parâmetros forem os padrões da página incial será trazido o dado cacheado
                if (Dashboard.defaultStartDate == StartDate &&
                    Dashboard.defaultEndDate == EndDate &&
                    idUf == Dashboard.defaultUf)
                {
                    var cacheData = new CacheData("ucRelatorioTop5Cursos", 24);
                    if (cacheData.HasValidCacheData())
                    {
                        dto = cacheData.GetCacheData();
                    }
                    else
                    {
                        dto = new ManterDashBoard().ObterTop5Cursos(StartDate, EndDate, idUf);
                        // Queremos que expire na virada de dia e não dure exatamente 24 horas
                        cacheData.SetCacheData(dto, typeof(DTOTop5Cursos), DateTime.Today.AddDays(1));
                    }
                }
                else
                {
                    dto = new ManterDashBoard().ObterTop5Cursos(StartDate, EndDate, idUf);
                }

                rtOnline.DataSource = dto.CursosOnline;
                rtOnline.DataBind();

                rtPresencial.DataSource = dto.CursosPresenciais;
                rtPresencial.DataBind();
            }
        }
    }
}
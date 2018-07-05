using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;
using Sebrae.Academico.BP.Relatorios.Dashboard;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucGraficoValoresSistema : UserControl{
        IList<ValorSistema> lista { get; set; }
        DTOValorSistema valorSistema { get; set; }
        protected void Page_Load(object sender, EventArgs e){
            if (!IsPostBack){
                lista = (new ManterValorSistema()).ObterTodosValoresSistema();

                var inicioArray = Request["i"].Split('/');
                var dataInicio = new DateTime(int.Parse(inicioArray[2]), int.Parse(inicioArray[1]), int.Parse(inicioArray[0]));
                var fimArray = Request["f"].Split('/');
                var dataFim = new DateTime(int.Parse(fimArray[2]), int.Parse(fimArray[1]), int.Parse(fimArray[0]));

                valorSistema = new RelatorioValoresSistema().ObterValorSistema(dataInicio, dataFim).FirstOrDefault();
            }
        }
        public string Hht(){
            return valorSistema.TotalCargaHoraria.ToString();
        }
        public string Ano(){
            var inicioArray = Request["i"].Split('/');
            var dataInicio = new DateTime(int.Parse(inicioArray[2]), int.Parse(inicioArray[1]), int.Parse(inicioArray[0]));
            var fimArray = Request["f"].Split('/');
            var dataFim = new DateTime(int.Parse(fimArray[2]), int.Parse(fimArray[1]), int.Parse(fimArray[0]));

            return (dataInicio.Year == dataFim.Year ? dataInicio.Year.ToString() : dataInicio.Year + " - " + dataFim.Year);
        }
        public string ValorSistema(string registro){
            return lista.Where(p => p.Registro == registro).Select(p => p.Descricao).FirstOrDefault() ?? "";
        }
    }
}
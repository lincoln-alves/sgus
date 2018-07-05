using System;
using System.Web.UI;
using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;
using Sebrae.Academico.BP.Relatorios.Dashboard;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.UserControls.dashboard
{
    public partial class ucGraficoBaseConcluintesMatriculados : UserControl
    {
        public int Tipo { get; set; }

        public DateTime StartDate;
        public DateTime EndDate;
        public int? ufQuery;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var idUf = 0;
                int.TryParse(ufQuery.ToString(), out idUf);

                DTOMatriculas dados;

                // Se todos os parâmetros forem os padrões da página incial será trazido o dado cacheado
                if (Dashboard.defaultStartDate == StartDate &&
                    Dashboard.defaultEndDate == EndDate &&
                    idUf == Dashboard.defaultUf)
                {
                    var cacheData = new CacheData("ucGraficoBaseConcluintesMatriculados"+Tipo, 24);
                    if (cacheData.HasValidCacheData())
                    {
                        dados = cacheData.GetCacheData();
                    }
                    else
                    {
                        dados = Tipo == 1 ? new RelatorioMatriculados().ObterMatriculas(StartDate, EndDate, idUf) : new RelatorioConcluintes().ObterMatriculas(StartDate, EndDate, idUf);
                        // Queremos que expire na virada de dia e não dure exatamente 24 horas
                        cacheData.SetCacheData(dados, typeof(DTOMatriculas), DateTime.Today.AddDays(1));
                    }
                }
                else
                {
                    dados = Tipo == 1 ? new RelatorioMatriculados().ObterMatriculas(StartDate, EndDate, idUf) : new RelatorioConcluintes().ObterMatriculas(StartDate, EndDate, idUf);
                }

                SetDataSource(dados);
            }
        }

        private void SetDataSource(DTOMatriculas dados)
        {
            if (dados != null && (dados.Internos + dados.Externos) != 0)
            {
                decimal internosDec = 0;
                decimal externosDec = 0;

                var porcentagemExterno = (100 * dados.Externos) / (dados.Internos + dados.Externos);

                var porcentagemInterno = (100 * dados.Internos) / (dados.Internos + dados.Externos);

                ltnDataSource.Text = "[\"Externos\", " + porcentagemExterno + "],[\"Internos\", " + porcentagemInterno +
                                     "]";

                ltnPorcentagem.Text = porcentagemInterno.ToString();

                if (dados.Externos == 0)
                {
                    ltnQntExterno.Text = "0";
                }
                else
                {
                    externosDec = (decimal)dados.Externos / 1000;

                    ltnQntExterno.Text = dados.Externos < 1000 ? dados.Externos.ToString() : externosDec.ToString("#.#") + "mil";
                }

                if (dados.Internos == 0)
                {
                    ltnQntInterno.Text = "0";
                }
                else
                {
                    internosDec = (decimal)dados.Internos / 1000;

                    ltnQntInterno.Text = dados.Internos < 1000 ? dados.Internos.ToString() : internosDec.ToString("#.#") + "mil";
                }

                if ((dados.Internos + dados.Externos) == 0)
                {
                    ltnQntInterno.Text = "0";
                }
                else
                {
                    var total = dados.Internos + dados.Externos;

                    var totalDec = internosDec + externosDec;

                    ltnQntTotal.Text = total < 1000 ? total.ToString() : totalDec.ToString("#.#") + "mil";
                }
            }
            else
            {
                ltnDataSource.Text = "[\"Externos\", 1],[\"Internos\", 1]";

                ltnPorcentagem.Text = "0";

                ltnQntExterno.Text = "0";

                ltnQntInterno.Text = "0";

                ltnQntTotal.Text = "0";
            }
        }
    }
}
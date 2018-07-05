using System;
using System.Collections.Generic;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BM.Classes;
using System.Linq;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP.DTO.Relatorios;

namespace Sebrae.Academico.WebForms
{
    /// <summary>
    /// Tela de Dashboard.
    /// </summary>
    public partial class HomeRelatoriosPaginaInicial : System.Web.UI.Page
    {

        private ManterDashBoard manterDashboard = new ManterDashBoard();
        private int numItemsCertificacoes = 0;
        private int numItemsInscritosPorQuadroTotal = 0;
        private List<DTOCertificadosPorNivelOperacional> listaCertificadosPorNivelOperacional;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptInclude(GetType(), "dynamic-form", "/js/dynamic-form.js");
            PreencherListaUf();

            if (!Page.IsPostBack)
            {
                this.CarregarInformacoesDasTabelas();
            }

            if (!new BMUsuario().PerfilAdministrador())
            {
                ucNotificacoes1.Visible =
                    ucDashBoardRelatoriosMaisAcessados1.Visible =
                        ucDashboardFuncionalidadesMaisAcessadas1.Visible = false;
            }

            IniciarGraficoConcluintesPorSolucaoEducacional();
            IniciarGraficoPerfilDoPublicoAtendido();
            IniciarGraficoFaixaEtaria();
            IniciarGraficoConcluintesPorRegiao();
            IniciarGraficoLimitesPorUF();
            IniciarGraficoTempoDeSebrae();
            IniciarGraficoNumeroDeCertificacoesPorColaborador();
            IniciarGraficoInscritosPorQuadroTotal();
            IniciarGraficoCertificadosPorNaoCertificados();
            IniciarGraficoCertificadosPorNivelOperacional();
            IniciarGraficoRodaEmPorcentagem();
            IniciarGraficoSituacaoAlunosCursos();
        }

        private void IniciarGraficoRodaEmPorcentagem()
        {
            List<string> listaLinhas = new List<string>();
            listaLinhas.Add("['Item','Porcentagem'],");
            listaLinhas.Add("['AUTOINDICATIVA',21],");
            listaLinhas.Add("['EDUCADOR CORPORATIVO',13],");
            listaLinhas.Add("['EVENTOS DE LIDERANÇA',30],");
            listaLinhas.Add("['PORFOLIO UCSEBRAE',36]");

            rptRodaEmPorcentagem.DataSource = listaLinhas;
            rptRodaEmPorcentagem.DataBind();
        }

        protected void rptRodaEmPorcentagem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal lbl = (Literal)e.Item.FindControl("Literal");
                lbl.Text = e.Item.DataItem as string;
                ;
            }
        }

        private void IniciarGraficoCertificadosPorNivelOperacional()
        {
            listaCertificadosPorNivelOperacional = manterDashboard.ObterCertificadosPorNivelOperacional().ToList();

            List<string> listNivelOperacional =
                listaCertificadosPorNivelOperacional.Where(
                    d => d.Nome.Contains("ANALISTA") || d.Nome.Contains("ASSISTENTE"))
                    .Select(d => d.Nome)
                    .Distinct()
                    .ToList();
            listNivelOperacional.Sort();
            listNivelOperacional.Add("OUTROS");
            List<string> listCertificacoes =
                listaCertificadosPorNivelOperacional.Select(d => d.Certificados.ToString()).Distinct().ToList();
            listNivelOperacional.Sort();
            List<string> listaLinhas = new List<string>();

            listaLinhas.Add("['NIVEL OCUPACIONAL',");

            for (int i = 0; i < listCertificacoes.Count(); i++)
            {
                listaLinhas[0] += "'" + listCertificacoes[i] + "'" + (listCertificacoes.Count() - 1 != i ? ", " : "],");
            }

            for (int i = 0; i < listNivelOperacional.Count(); i++)
            {
                string itemText = "";
                itemText = "['" + listNivelOperacional[i] + "',";

                if (listNivelOperacional[i] != "OUTROS")
                {
                    for (int ii = 0; ii < listCertificacoes.Count(); ii++)
                    {
                        DTOCertificadosPorNivelOperacional item =
                            listaCertificadosPorNivelOperacional.FirstOrDefault(
                                d =>
                                    d.Nome == listNivelOperacional[i] &&
                                    d.Certificados.ToString() == listCertificacoes[ii]);
                        itemText += (item != null ? item.Quantidade.ToString() : "0") +
                                    (listCertificacoes.Count() - 1 != ii ? ", " : "]");
                    }
                }
                else
                {
                    for (int ii = 0; ii < listCertificacoes.Count(); ii++)
                    {
                        List<DTOCertificadosPorNivelOperacional> qts =
                            listaCertificadosPorNivelOperacional.Where(
                                d =>
                                    !(d.Nome.Contains("ANALISTA") || d.Nome.Contains("ASSISTENTE")) &&
                                    d.Certificados.ToString() == listCertificacoes[ii]).ToList();
                        int qt = 0;
                        if (qts != null && qts.Count > 0)
                        {
                            qt = qts.Sum(d => d.Quantidade);
                        }
                        itemText += qt + (listCertificacoes.Count() - 1 != ii ? ", " : "]");
                    }
                }



                if (listNivelOperacional.Count() - 1 != i)
                    itemText += ",";

                listaLinhas.Add(itemText);
            }

            rptCertificadosPorNivelOperacional.DataSource = listaLinhas;
            rptCertificadosPorNivelOperacional.DataBind();
        }

        protected void rptCertificadosPorNivelOperacional_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal lbl = (Literal)e.Item.FindControl("Literal");
                lbl.Text = e.Item.DataItem as string;
                ;
            }
        }

        private void IniciarGraficoCertificadosPorNaoCertificados()
        {
            List<DTOCertificadosPorNaoCertificados> lista =
                manterDashboard.ObterCertificadosPorNaoCertificados().ToList();
            rptCertificadosPorNaoCertificados.DataSource = lista;
            rptCertificadosPorNaoCertificados.DataBind();
        }

        protected void rptCertificadosPorNaoCertificados_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal lbl = (Literal)e.Item.FindControl("Literal");
                DTOCertificadosPorNaoCertificados item = e.Item.DataItem as DTOCertificadosPorNaoCertificados;

                lbl.Text = "['', " + item.Certificado + ", " + item.NaoCertificado + "]";

                if (e.Item.ItemIndex != 1)
                {
                    lbl.Text = lbl.Text + ",";
                }
            }
        }

        private void IniciarGraficoInscritosPorQuadroTotal()
        {
            List<DTOInscritosPorQuadroTotal> lista = manterDashboard.ObterInscritosPorQuadroTotal().ToList();
            lista = lista.OrderBy(d => d.UF).ToList();
            numItemsInscritosPorQuadroTotal = lista.Count();
            rptInscritosPorQuadroTotal.DataSource = lista;
            rptInscritosPorQuadroTotal.DataBind();
        }

        protected void numItemsInscritosPorQuadroTotal_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal lbl = (Literal)e.Item.FindControl("Literal");
                DTOInscritosPorQuadroTotal item = e.Item.DataItem as DTOInscritosPorQuadroTotal;

                if (item != null && item.UF != null)
                {
                    lbl.Text = "['" + item.UF + "', " + item.Inscritos + ", " + item.QuadroTotal + "]";

                    if (e.Item.ItemIndex != numItemsInscritosPorQuadroTotal - 1)
                    {
                        lbl.Text = lbl.Text + ",";
                    }
                }
            }
        }

        private void IniciarGraficoNumeroDeCertificacoesPorColaborador()
        {
            List<DTONumeroDeCertificacoesPorColaborador> lista =
                manterDashboard.ObterNumeroDeCertificacoesPorColaborador().ToList();
            numItemsCertificacoes = lista.Count();
            rptCertificacoesPorColaborador.DataSource = lista;
            rptCertificacoesPorColaborador.DataBind();
        }

        private void IniciarGraficoSituacaoAlunosCursos()
        {
            var lista = manterDashboard.ObterSituacoesAlunosCursos(null).ToList();
            rptSituacaoAlunos.DataSource = lista;
            rptSituacaoAlunos.DataBind();
        }

        protected void rptSituacaoAlunos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal lbl = (Literal)e.Item.FindControl("LiteralSituacao");
                DTOSituacaoCursos item = e.Item.DataItem as DTOSituacaoCursos;

                lbl.Text = "['" + item.Situacao + "', " + item.Quantidade + "]";

                if (e.Item.ItemIndex != numItemsCertificacoes - 1)
                {
                    lbl.Text = lbl.Text + ",";
                }
            }
        }


        protected void rptCertificacoesPorColaborador_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal lbl = (Literal)e.Item.FindControl("Literal");
                DTONumeroDeCertificacoesPorColaborador item = e.Item.DataItem as DTONumeroDeCertificacoesPorColaborador;

                lbl.Text = "['" + item.Certificacoes + "', " + item.Quantidade + "]";

                if (e.Item.ItemIndex != numItemsCertificacoes - 1)
                {
                    lbl.Text = lbl.Text + ",";
                }
            }
        }

        private void IniciarGraficoTempoDeSebrae()
        {
            List<DTOTempoDeSebrae> lista = manterDashboard.ObterTempoDeSebrae().ToList();

            List<DTOTempoDeSebrae> final = new List<DTOTempoDeSebrae>();

            final.Add(new DTOTempoDeSebrae() { Nome = "Menos de 2 anos" });
            final.Add(new DTOTempoDeSebrae() { Nome = "2 a 5 anos" });
            final.Add(new DTOTempoDeSebrae() { Nome = "6 a 10 anos" });
            final.Add(new DTOTempoDeSebrae() { Nome = "11 a 15 anos" });
            final.Add(new DTOTempoDeSebrae() { Nome = "16 a 20 anos" });
            final.Add(new DTOTempoDeSebrae() { Nome = "21 a 25 anos" });
            final.Add(new DTOTempoDeSebrae() { Nome = "26 a 30 anos" });
            final.Add(new DTOTempoDeSebrae() { Nome = "31 a 35 anos" });
            final.Add(new DTOTempoDeSebrae() { Nome = "36 a 41 anos" });

            int total = 0;

            foreach (var item in lista)
            {
                total += (int)item.Quantidade;
                if (item.Tempo < 2)
                    final[0].Quantidade += item.Quantidade;

                if (item.Tempo >= 2 && item.Tempo <= 5)
                    final[1].Quantidade += item.Quantidade;

                if (item.Tempo >= 6 && item.Tempo <= 10)
                    final[2].Quantidade += item.Quantidade;

                if (item.Tempo >= 11 && item.Tempo <= 15)
                    final[3].Quantidade += item.Quantidade;

                if (item.Tempo >= 16 && item.Tempo <= 20)
                    final[4].Quantidade += item.Quantidade;

                if (item.Tempo >= 21 && item.Tempo <= 25)
                    final[5].Quantidade += item.Quantidade;

                if (item.Tempo >= 26 && item.Tempo <= 30)
                    final[6].Quantidade += item.Quantidade;

                if (item.Tempo >= 31 && item.Tempo <= 35)
                    final[7].Quantidade += item.Quantidade;

                if (item.Tempo >= 36 && item.Tempo <= 41)
                    final[8].Quantidade += item.Quantidade;
            }

            rptTempoDeSebrae.DataSource = final;
            rptTempoDeSebrae.DataBind();
        }

        protected void rptTempoDeSebrae_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal lbl = (Literal)e.Item.FindControl("Literal");
                DTOTempoDeSebrae item = e.Item.DataItem as DTOTempoDeSebrae;

                lbl.Text = "['" + item.Nome + "', " + item.Quantidade + "]";

                if (e.Item.ItemIndex != rptTempoDeSebrae.Items.Count - 1)
                {
                    lbl.Text = lbl.Text + ",";
                }
            }
        }

        private void IniciarGraficoLimitesPorUF()
        {
            List<DTOUsuariosPorUF> lista = manterDashboard.ObterUsuariosPorUF().ToList();

            float total = 0;
            lista.ForEach(d => total += d.Quantidade);
            lista.ForEach(d => d.Quantidade = (d.Quantidade * 100) / total);

            rptLimitesPorUF.DataSource = lista;
            rptLimitesPorUF.DataBind();
        }

        protected void rptLimitesPorUF_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal lbl = (Literal)e.Item.FindControl("Literal");
                DTOUsuariosPorUF item = e.Item.DataItem as DTOUsuariosPorUF;

                lbl.Text = "['" + item.Nome + "', " + item.Quantidade.ToString("0.00").Replace(',', '.') + "]";

                if (e.Item.ItemIndex != rptLimitesPorUF.Items.Count - 1)
                {
                    lbl.Text = lbl.Text + ",";
                }
            }
        }

        private void IniciarGraficoConcluintesPorRegiao()
        {
            List<DTOConcluintesPorRegiao> lista = manterDashboard.ObterConcluintesPorRegiao().ToList();

            float total = 0;
            lista.ForEach(d => total += d.Concluintes);
            lista.ForEach(d => d.Concluintes = (d.Concluintes * 100) / total);

            rptConcluintesPorRegiao.DataSource = lista;
            rptConcluintesPorRegiao.DataBind();
        }

        protected void rptConcluintesPorRegiao_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal lbl = (Literal)e.Item.FindControl("Literal");
                DTOConcluintesPorRegiao item = e.Item.DataItem as DTOConcluintesPorRegiao;

                lbl.Text = "['" + item.Regiao + "', " + item.Concluintes.ToString("0.00").Replace(',', '.') + "]";

                if (e.Item.ItemIndex != rptConcluintesPorRegiao.Items.Count - 1)
                {
                    lbl.Text = lbl.Text + ",";
                }
            }
        }

        private void IniciarGraficoFaixaEtaria()
        {
            List<DTOFaixaEtaria> final = manterDashboard.ObterFaixaEtaria().ToList();

            rptFaixaEtaria.DataSource = final;
            rptFaixaEtaria.DataBind();
        }

        protected void rptFaixaEtaria_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal lbl = (Literal)e.Item.FindControl("Literal");
                DTOFaixaEtaria item = e.Item.DataItem as DTOFaixaEtaria;
                lbl.Text = "['" + item.Nome + "', " + item.Quantidade.ToString("0.00").Replace(',', '.') + "]";

                if (e.Item.ItemIndex != rptFaixaEtaria.Items.Count - 1)
                {
                    lbl.Text = lbl.Text + ",";
                }
            }
        }

        private void IniciarGraficoConcluintesPorSolucaoEducacional(int ano = 0)
        {
            rptConcluintesPorSolucacaoEducacional.DataSource = manterDashboard.ObterConcluintesPorSolucaoEducacional();
            rptConcluintesPorSolucacaoEducacional.DataBind();
        }

        protected void rptConcluintesPorSolucacaoEducacional_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var lbl = (Literal)e.Item.FindControl("Literal");
            var item = e.Item.DataItem as DTOConcluintesPorSolucaoEducacional;
            lbl.Text = "['" + item.SolucaoEducacional + "', " + item.Concluintes + "]";
            if (e.Item.ItemIndex != rptConcluintesPorSolucacaoEducacional.Items.Count - 1)
            {
                lbl.Text = lbl.Text + ",";
            }
        }

        private void IniciarGraficoPerfilDoPublicoAtendido()
        {
            var lista = manterDashboard.ObterPerfilDoPublicoAtendido().ToList();
            rptPerfilDoPUblicoAtendido.DataSource = lista;
            rptPerfilDoPUblicoAtendido.DataBind();
        }

        protected void rptPerfilDoPUblicoAtendido_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var lbl = (Literal)e.Item.FindControl("Literal");
            var item = e.Item.DataItem as DTOPerfilDoPublicoAtendido;
            var grupo1 = new DTOPerfilDoPublicoAtendido();
            lbl.Text = "['" + item.Perfil + "', " + item.Quantidade + "]";
            if (e.Item.ItemIndex != rptConcluintesPorSolucacaoEducacional.Items.Count - 1)
            {
                lbl.Text = lbl.Text + ",";
            }
        }

        private void CarregarInformacoesDasTabelas()
        {
            try
            {
                //ucDashBoardRelatoriosMaisAcessados1.CarregarInformacoesSobreRelatoriosMaisAcessados();
                this.GerarTabelas();
                this.GerarGraficos();
            }
            catch
            {
                //TODO: -> Tratar Exceção
                //throw ex;
            }
        }

        private void GerarTabelas()
        {
            this.ucDashBoardRelatoriosMaisAcessados1.CarregarInformacoesSobreRelatoriosMaisAcessados();
            this.ucDashboardFuncionalidadesMaisAcessadas1.CarregarInformacoesSobreFuncionalidadesMaisAcessadas();
            this.ucNotificacoes1.CarregarInformacoesSobreNotificacoes();
        }

        private void GerarGraficos()
        {
            //Todo: Descomentar para ver o retorno do Parâmetro de output
            //this.GerarGraficoDeTaxaDeAprovacaoNoAno();

            //this.ucDashboardParticipacaoProporcionalAoNumeroDeFuncionarios1.ger
            //this.ucDashboardParticipacaoProporcionalAoNumeroDeFuncionarios1.
            //this.ucDashboardParticipacaoProporcionalAoNumeroDeFuncionarios1 .GerarGraficoDeParticipacaoProporcionalAoNumeroDeFuncionarios();
        }

        public void GerarGraficoDeTaxaDeAprovacaoNoAno()
        {
            try
            {
                var anoAtual = DateTime.Today.Year;
                var listaDadosComTaxaDeAprovacaoNoAno = new ManterDashBoard().ObterTaxaDeAprovacaoNoAno(anoAtual, null);
                if (listaDadosComTaxaDeAprovacaoNoAno != null && listaDadosComTaxaDeAprovacaoNoAno.Count > 0)
                {
                    //WebFormHelper.PreencherGrid(ListaDadosComTaxaDeAprovacaoNoAno, dgvInformacoesDeRelatoriosMaisAcessados);
                    //this.MontarGrafico(this.chartTaxaAprovacaoNoAno, SeriesChartType.Bar, ListaDadosComTaxaDeAprovacaoNoAno);
                }
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        private void PreencherListaUf()
        {
            var listaUf = new ManterUf().ObterTodosUf();
            var bmUsuario = new BMUsuario();
            if (!bmUsuario.PerfilAdministrador())
            {
                if (bmUsuario.ObterUfLogadoSeGestor() > 0)
                {
                    ddlUF.SelectedValue = bmUsuario.ObterUfLogadoSeGestor().ToString();
                    ddlUF.Enabled = false;
                }
            }
            WebFormHelper.PreencherLista(listaUf, ddlUF, false, true);
        }

        #region "GraficoDeParticipacaoProporcionalAoNumeroDeFuncionarios"



        #endregion

        #region "GraficoDeTotalDeMatriculasNoAnoPorUf"


        #endregion


        //private void MontarGrafico(Chart graficoTaxaAprovacaoNoAno, SeriesChartType chartType, IList<DTOMatriculaOferta> dados)
        //{
        //    graficoTaxaAprovacaoNoAno.DataSource = dados;
        //    graficoTaxaAprovacaoNoAno.DataBind();
        //}


        protected void lnkConcluintesPorRegiao_OnClick(object sender, EventArgs e)
        {
            try
            {
                int ano = 0;
                if (int.TryParse(txtFiltrarPorAnoConcluintePorRegiao.Value, out ano))
                {
                    var lista = manterDashboard.ObterConcluintesPorRegiao(ano).ToList();

                    float total = 0;
                    lista.ForEach(d => total += d.Concluintes);
                    lista.ForEach(d => d.Concluintes = (d.Concluintes * 100) / total);

                    hdConcluintesPorRegiao.Value = lista.Count > 0 ? new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(lista).ToString() : "";
                }
                else
                {
                    hdConcluintesPorRegiao.Value = "";
                }
            }
            catch (Exception)
            {
                hdConcluintesPorRegiao.Value = "";
            }
        }

        protected void lnkConcluintesPorSolucaoEducacional_OnClick(object sender, EventArgs e)
        {
            try
            {
                int ano = 0;
                if (int.TryParse(txtConcluintesPorSolucaoEducacional.Value, out ano))
                {
                    var lista = manterDashboard.ObterConcluintesPorSolucaoEducacional(ano);
                    if (lista.Count > 0)
                    {
                        hdConcluintesPorSolucaoEducacional.Value = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(lista).ToString();
                    }
                    else
                    {
                        hdConcluintesPorRegiao.Value = "";
                    }
                }
                else
                {
                    hdConcluintesPorRegiao.Value = "";
                }
            }
            catch (Exception)
            {
                hdConcluintesPorRegiao.Value = "";
            }
        }

        protected void lnkFaixaEtaria_OnClick(object sender, EventArgs e)
        {
            try
            {
                var ano = 0;
                if (int.TryParse(txtFaixaEtaria.Value, out ano))
                {
                    var lista = manterDashboard.ObterFaixaEtaria(ano).ToList();
                    hdFaixaEtaria.Value = lista.Count > 0 ? new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(lista).ToString() : "";
                }
                else
                {
                    hdFaixaEtaria.Value = "";
                }
            }
            catch (Exception)
            {
                hdFaixaEtaria.Value = "";
            }
        }


        protected void lnkPerfilDoPublicoAtendimento_OnClick(object sender, EventArgs e)
        {
            try
            {
                int ano = 0;
                if (int.TryParse(txtPerfilDoPublicoAtendimento.Value, out ano))
                {
                    var lista = manterDashboard.ObterPerfilDoPublicoAtendido(ano).ToList();

                    if (lista.Count > 0)
                    {
                        hdPerfilDoPublicoAtendimento.Value =
                            new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(lista).ToString();
                    }
                    else
                    {
                        hdPerfilDoPublicoAtendimento.Value = "";
                    }
                }
                else
                {
                    hdPerfilDoPublicoAtendimento.Value = "";
                }
            }
            catch (Exception)
            {
                hdPerfilDoPublicoAtendimento.Value = "";
            }
        }

        protected void lnkLimitesPorSebraeDF_OnClick(object sender, EventArgs e)
        {
            try
            {
                int ano = 0;
                if (int.TryParse(txtLimitesPorSebraeDF.Value, out ano))
                {
                    var lista = manterDashboard.ObterUsuariosPorUF(ano).ToList();

                    if (lista.Count > 0)
                    {
                        hdLimitesPorSebraeDF.Value =
                            new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(lista).ToString();
                    }
                    else
                    {
                        hdLimitesPorSebraeDF.Value = "";
                    }
                }
                else
                {
                    hdLimitesPorSebraeDF.Value = "";
                }
            }
            catch (Exception)
            {
                hdLimitesPorSebraeDF.Value = "";
            }
        }
    }
}

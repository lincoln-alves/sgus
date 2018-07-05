using System;
using System.Linq;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.Relatorios.Dashboard;

namespace Sebrae.Academico.WebForms
{
    /// <summary>
    /// Tela de Dashboard.
    /// </summary>
    public partial class Painel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Page.ClientScript.RegisterClientScriptInclude(GetType(), "dynamic-form", "/js/dynamic-form.js");

            /*
            if (Page.Master != null)
            {
                var tituloControl = (Literal)Page.Master.FindControl("ltnTitulo");

                if (tituloControl != null)
                {
                    tituloControl.Text = "Painel de indicadores";
                }

                var btnProximoMenuMaster = (Button) Page.Master.FindControl("btnProximoMenu");

                if (btnProximoMenuMaster != null)
                {
                    btnProximoMenuMaster.OnClientClick += "location.href = '/dashboard.aspx'; return false;";
                    btnProximoMenuMaster.Text = "Monitoramento de indicadores";
                }

                btnProximoMenu.OnClientClick += "location.href = '/dashboard.aspx'; return false;";
                btnProximoMenu.Text = "Monitoramento de indicadores";
            }
            */
            // PREENCHER DADOS
            ObterDadosExistentes();
        }

        private void ObterDadosExistentes()
        {
            //Anos
            var ano3 = DateTime.Now.Year;
            var ano2 = ano3 - 1;
            var ano1 = ano2 - 1;
            var ano0 = ano1 - 1;

            // HORAS DE TREINAMENTO (aprovado)
            //Em Formacao de Multiplicadores
            ltr0100.Text = A(ano0);
            ltr0101.Text = A(ano1);
            ltr0102.Text = A(ano2);
            ltr0103.Text = A(ano3);

            //Capacitação do Sebrae Nacional per capta
            var _0200 = B(ano0);
            var _0201 = B(ano1);
            var _0202 = B(ano2);
            var _0203 = B(ano3);

            ltr0200.Text = _0200;
            ltr0201.Text = _0201;
            ltr0202.Text = _0202;
            ltr0203.Text = _0203;

            //Capacitação do Sistema Sebrae per capta
            var _0300 = C(ano0);
            var _0301 = C(ano1);
            var _0302 = C(ano2);
            var _0303 = C(ano3);

            ltr0300.Text = _0300;
            ltr0301.Text = _0301;
            ltr0302.Text = _0302;
            ltr0303.Text = _0303;

            //Capacitacao do Sistema Sebrae (total)
            ltr0400.Text = (decimal.Parse(_0200) + decimal.Parse(_0300)).ToString();
            ltr0401.Text = (decimal.Parse(_0201) + decimal.Parse(_0301)).ToString();
            ltr0402.Text = (decimal.Parse(_0202) + decimal.Parse(_0302)).ToString();
            ltr0403.Text = (decimal.Parse(_0203) + decimal.Parse(_0303)).ToString();

            //PARTICIPACAO NAS CAPACITACOES
            //Aprovadas nas capacitações per capta
            ltr0500.Text = D(ano0);
            ltr0501.Text = D(ano1);
            ltr0502.Text = D(ano2);
            ltr0503.Text = D(ano3);

            //Aprovacoes em relacao aos inscritos
            ltr0600.Text = E(ano0);
            ltr0601.Text = E(ano1);
            ltr0602.Text = E(ano2);
            ltr0603.Text = E(ano3);

            //Participacao dos colaboradores nas acoes de desenvolvimento (NA)
            ltr0700.Text = F(ano0);
            ltr0701.Text = F(ano1);
            ltr0702.Text = F(ano2);
            ltr0703.Text = F(ano3);

            //Aprovacao em acoes de Formacao de Multiplicadores
            ltr0800.Text = G(ano0);
            ltr0801.Text = G(ano1);
            ltr0802.Text = G(ano2);
            ltr0803.Text = G(ano3);

            //Colaboradores que pactuam meta de desenvolvimento (PADI)
            ltr2100.Text = ObterValorIndicador(ano0, "ColaboradoresPactuamMetaDesenvolvimentoPADI");
            ltr2101.Text = ObterValorIndicador(ano1, "ColaboradoresPactuamMetaDesenvolvimentoPADI");
            ltr2102.Text = ObterValorIndicador(ano2, "ColaboradoresPactuamMetaDesenvolvimentoPADI");
            ltr2103.Text = ObterValorIndicador(ano3, "ColaboradoresPactuamMetaDesenvolvimentoPADI");

            //EFICÁCIA E SATISFACAO
            //Satisfacao em relação aos programas educacionais da UC (Portfolio UC)
            ltr0900.Text = H(ano0);
            ltr0901.Text = H(ano1);
            ltr0902.Text = H(ano2);
            ltr0903.Text = H(ano3);

            //Satisfacao nas acoes de Formacao de Multiplicadores
            ltr1000.Text = I(ano0);
            ltr1001.Text = I(ano1);
            ltr1002.Text = I(ano2);
            ltr1003.Text = I(ano3);

            //Eficacia dos programas educacionais (Portfólio UC)
            ltr1900.Text = ObterValorIndicador(ano0, "EficaciaDosProgramasEducacionaisPortifolioUC");
            ltr1901.Text = ObterValorIndicador(ano1, "EficaciaDosProgramasEducacionaisPortifolioUC");
            ltr1902.Text = ObterValorIndicador(ano2, "EficaciaDosProgramasEducacionaisPortifolioUC");
            ltr1903.Text = ObterValorIndicador(ano3, "EficaciaDosProgramasEducacionaisPortifolioUC");

            //Eficacia dos programas academicos
            ltr2000.Text = ObterValorIndicador(ano0, "EficaciaDosProgramasAcademicos");
            ltr2001.Text = ObterValorIndicador(ano1, "EficaciaDosProgramasAcademicos");
            ltr2002.Text = ObterValorIndicador(ano2, "EficaciaDosProgramasAcademicos");
            ltr2003.Text = ObterValorIndicador(ano3, "EficaciaDosProgramasAcademicos");

            //GESTAO DO CONHECIMENTO
            //Acoes de Gestao do Conhecimento registradas no PADI
            ltr1100.Text = ObterValorIndicador(ano0, "AcoesDeGestaoDoConhecimentoRegistradasNoPADI");
            ltr1101.Text = ObterValorIndicador(ano1, "AcoesDeGestaoDoConhecimentoRegistradasNoPADI");
            ltr1102.Text = ObterValorIndicador(ano2, "AcoesDeGestaoDoConhecimentoRegistradasNoPADI");
            ltr1103.Text = ObterValorIndicador(ano3, "AcoesDeGestaoDoConhecimentoRegistradasNoPADI");

            //Producao de conteudos na Plataforma Saber (crescimento)
            ltr1200.Text = ObterValorIndicador(ano0, "ProducaoDeConteudoNaPlataformaSaberCrescimento");
            ltr1201.Text = ObterValorIndicador(ano1, "ProducaoDeConteudoNaPlataformaSaberCrescimento");
            ltr1202.Text = ObterValorIndicador(ano2, "ProducaoDeConteudoNaPlataformaSaberCrescimento");
            ltr1203.Text = ObterValorIndicador(ano3, "ProducaoDeConteudoNaPlataformaSaberCrescimento");

            //CERTIFICACAO
            //Colaboradores certificados do Sistema Sebrae (em relacao ao universo, exceto SP)
            ltr1300.Text = ObterValorIndicador(ano0, "ColaboradoresCertificadosDoSistemaSebraeUniversoExcetoSP");
            ltr1301.Text = ObterValorIndicador(ano1, "ColaboradoresCertificadosDoSistemaSebraeUniversoExcetoSP");
            ltr1302.Text = ObterValorIndicador(ano2, "ColaboradoresCertificadosDoSistemaSebraeUniversoExcetoSP");
            ltr1303.Text = ObterValorIndicador(ano3, "ColaboradoresCertificadosDoSistemaSebraeUniversoExcetoSP");

            //Colaboradores certificados do Sistema Sebrae (em relação aos inscritos)
            ltr1400.Text = ObterValorIndicador(ano0, "ColaboradoresCertificadosDoSistemaSebraeInscritos");
            ltr1401.Text = ObterValorIndicador(ano1, "ColaboradoresCertificadosDoSistemaSebraeInscritos");
            ltr1402.Text = ObterValorIndicador(ano2, "ColaboradoresCertificadosDoSistemaSebraeInscritos");
            ltr1403.Text = ObterValorIndicador(ano3, "ColaboradoresCertificadosDoSistemaSebraeInscritos");

            //Colaboradores certificados do Sebrae-NA (em relação aos inscritos)
            ltr1500.Text = ObterValorIndicador(ano0, "ColaboradoresCertificadosSebraeNA");
            ltr1501.Text = ObterValorIndicador(ano1, "ColaboradoresCertificadosSebraeNA");
            ltr1502.Text = ObterValorIndicador(ano2, "ColaboradoresCertificadosSebraeNA");
            ltr1503.Text = ObterValorIndicador(ano3, "ColaboradoresCertificadosSebraeNA");

            //INVESTIMENTO
            //Capacitacao Metodologica em acoes de Formacao de Multiplicadores
            ltr1600.Text = ObterValorIndicador(ano0, "CapacitacoesMetodologicasCredenciados");
            ltr1601.Text = ObterValorIndicador(ano1, "CapacitacoesMetodologicasCredenciados");
            ltr1602.Text = ObterValorIndicador(ano2, "CapacitacoesMetodologicasCredenciados");
            ltr1603.Text = ObterValorIndicador(ano3, "CapacitacoesMetodologicasCredenciados");

            //Em relacao a folha de pagamento do Sistema Sebrae
            ltr1700.Text = ObterValorIndicador(ano0, "RelacaoFolhaPagamentoSistemaSebrae");
            ltr1701.Text = ObterValorIndicador(ano1, "RelacaoFolhaPagamentoSistemaSebrae");
            ltr1702.Text = ObterValorIndicador(ano2, "RelacaoFolhaPagamentoSistemaSebrae");
            ltr1703.Text = ObterValorIndicador(ano3, "RelacaoFolhaPagamentoSistemaSebrae");

            //Execucao %
            ltr1800.Text = ObterValorIndicador(ano0, "ExecucaoPorcentagem");
            ltr1801.Text = ObterValorIndicador(ano1, "ExecucaoPorcentagem");
            ltr1802.Text = ObterValorIndicador(ano2, "ExecucaoPorcentagem");
            ltr1803.Text = ObterValorIndicador(ano3, "ExecucaoPorcentagem");

        }

        private static string A(int ano)
        {
            var retorno = new RelatorioMonitoramento().ObterHorasCredenciados(ano);

            if (retorno != null)
            {
                return retorno.Valor.ToString();
            }

            return "0";
        }

        private static string B(int ano)
        {
            var retorno = new RelatorioMonitoramento().ObterCapacitacaoNacionalPerCapta(ano);

            if (retorno != null)
            {
                return retorno.Valor.ToString();
            }

            return "0";
        }

        private static string C(int ano)
        {
            var retorno = new RelatorioMonitoramento().ObterCapacitacaoGeralPerCapta(ano);

            if (retorno != null)
            {
                return retorno.Valor.ToString();
            }

            return "0";
        }

        private static string D(int ano)
        {
            var retorno = new RelatorioMonitoramento().ObterAprovacoesCapacitacoes(ano);

            if (retorno != null)
            {
                return retorno.Valor.ToString();
            }

            return "0";
        }

        private static string E(int ano)
        {
            var retorno = new RelatorioMonitoramento().ObterAprovacoesAosInscricos(ano);

            if (retorno != null)
            {
                return retorno.Valor.ToString();
            }

            return "0";
        }

        private static string F(int ano)
        {
            var retorno = new RelatorioMonitoramento().ObterColaboradoresNA(ano);

            if (retorno != null)
            {
                return retorno.Valor.ToString();
            }

            return "0";
        }

        private static string G(int ano)
        {
            var retorno = new RelatorioMonitoramento().ObterCredenciadosAprovados(ano);

            if (retorno != null)
            {
                return retorno.Valor.ToString();
            }

            return "0";
        }

        private static string H(int ano)
        {
            var retorno = new RelatorioMonitoramento().ObterSatisfacaoGeral(ano);

            if (retorno != null)
            {
                return retorno.Valor.ToString();
            }

            return "0";
        }

        private static string I(int ano)
        {
            var retorno = new RelatorioMonitoramento().ObterSatisfacaoFF(ano);

            if (retorno != null)
            {
                return retorno.Valor.ToString();
            }

            return "0";
        }

        public static string ObterValorIndicador(int ano, string indicador)
        {
            return new ManterMonitoramentoIndicadores().ObterValorIndicador(ano, indicador);
        }
    }
}

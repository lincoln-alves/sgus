using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Collections.Generic;

namespace Sebrae.Academico.WebForms
{
    /// <summary>
    /// Tela de Dashboard.
    /// </summary>
    public partial class Dashboard : System.Web.UI.Page
    {
        // Defaults for non filtered
        public static readonly DateTime defaultStartDate = (new DateTime(DateTime.Now.Year, 1, 1));
        public static readonly DateTime defaultEndDate = DateTime.Now;
        public static readonly int defaultUf = 0;

        public DateTime StartDate;
        public DateTime EndDate;
        public int? ufQuery = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptInclude(GetType(), "dynamic-form", "/js/dynamic-form.js");

            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            if (String.IsNullOrEmpty(Request["i"]))
            {
                StartDate = defaultStartDate;
            }
            else
            {
                DateTime.TryParse(Request["i"], out StartDate);
            }

            if (String.IsNullOrEmpty(Request["f"]))
            {
                EndDate = defaultEndDate;
            }
            else
            {
                DateTime.TryParse(Request["f"], out EndDate);
            }

            if (!String.IsNullOrEmpty(Request["uf"]))
            {
                ufQuery = int.Parse(Request["uf"]);
            }
            else
            {
                ufQuery = defaultUf;
            }

            if (!IsPostBack)
            {
  
                if (Request.Url.AbsolutePath.ToLower() == "/dashboard.aspx")
                {                    
                    if (usuarioLogado != null && ufQuery == 0 &&usuarioLogado.UF.ID != (int)enumUF.NA)
                    {
                        ufQuery = usuarioLogado.UF.ID;
                        if (!usuarioLogado.IsGestor() && !usuarioLogado.IsAdministrador())
                        {
                            cbxUf.Enabled = false;
                        }
                    }

                    if (!ValidarDatas(StartDate, EndDate))
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Data Inicial não pode ser maior que a Data Final.", Request.Url.AbsolutePath + "?i=" + (new DateTime(DateTime.Now.Year, 1, 1)).ToShortDateString() + "&f=" + DateTime.Now.ToShortDateString());
                        return;
                    }


                    txtDataInicial.Text = StartDate.ToShortDateString();
                    txtDataFinal.Text = EndDate.ToShortDateString();

                    var manterUfs = new ManterUf();
                    var listaUfs = manterUfs.ObterTodosUf();
                    WebFormHelper.PreencherLista(listaUfs, cbxUf, true);

                    if (ufQuery != null)
                    {
                        var idUf = 0;
                        var uf = int.TryParse(ufQuery.ToString(), out idUf) ? manterUfs.ObterUfPorID(idUf) : manterUfs.ObterUfPorSigla(ufQuery.ToString());
                        if (uf != null) WebFormHelper.SetarValorNaCombo(uf.ID.ToString(), cbxUf);
                    }
                }
                else
                {
                    txtDataInicial.Visible = false;
                    txtDataFinal.Visible = false;
                    cbxUf.Visible = false;
                    btnFiltrar.Visible = false;
                }
            }

            if (cbxUf != null && usuarioLogado != null && usuarioLogado.UF.ID != (int)enumUF.NA)
            {
                if (!usuarioLogado.IsGestor() && !usuarioLogado.IsAdministrador())
                {
                    cbxUf.Enabled = false;
                }
            }

            // Seta Start Date e EndDate nos UserControls
            SetParametersUserControls();            
        }

        private void SetParametersUserControls()
        {
            List<UserControl> ucs = new List<UserControl>() {
                ucGraficoMatriculadosUF,
                ucGraficoTop5Cursos,
                ucGraficoBaseConcluintesMatriculados,
                ucGraficoBaseConcluintesMatriculados1,
                ucGraficoMatriculadosPorMes,
                ucGraficoConcluintesEspacoOcupacionalEmpregados,
                ucGraficoConcluintesExternos,
                ucGraficoIndiceSatisfacaoGeral,
                ucGraficoIndiceSatisfacaoCredenciados
            };

            foreach (dynamic uc in ucs)
            {                
                uc.StartDate = StartDate;
                uc.EndDate = EndDate;
                uc.ufQuery = ufQuery;
            }

        }


        protected void AtualizarPagina()
        {
            Response.Redirect(Request.RawUrl);
        }

        protected void btnPerfilAdministrador_OnServerClick(object sender, EventArgs e)
        {
            new ManterUsuario().SetarPerfisOriginais();

            AtualizarPagina();
        }

        private static bool ValidarDatas(DateTime inicio, DateTime fim)
        {
           return !(inicio.Date > fim.Date);
        }

        protected void btnFiltrar_OnClick(object sender, EventArgs e)
        {
            DateTime inicio;
            DateTime fim;

            if (!DateTime.TryParse(txtDataInicial.Text, out inicio) || !DateTime.TryParse(txtDataFinal.Text, out fim) || !ValidarDatas(inicio, fim))
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Data Inicial não pode ser maior que a Data Final.", Request.Url.AbsolutePath + "?i=" + (new DateTime(DateTime.Now.Year, 1, 1)).ToShortDateString() + "&f=" + DateTime.Now.ToShortDateString());
                return;
            }

            Response.Redirect(Request.Url.AbsolutePath + "?i=" + inicio.ToShortDateString() + "&f=" + fim.ToShortDateString() + "&uf=" + cbxUf.SelectedValue, false);
        }

    }
}

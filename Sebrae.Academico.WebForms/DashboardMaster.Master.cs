using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms
{
    public partial class DashboardMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                // Exibir mensagem de alerta.
                RecuperarMensageSessao();

                if (Request.Url.AbsolutePath.ToLower() == "/dashboard.aspx")
                {
                    var inicio = Request["i"];

                    if (inicio == null)
                    {
                        Response.Redirect(Request.Url.AbsolutePath + "?i=" + (new DateTime(DateTime.Now.Year, 1, 1)).ToShortDateString());
                        return;
                    }

                    var fim = Request["f"];

                    if (fim == null)
                    {
                        Response.Redirect(Request.Url + "&f=" + DateTime.Now.ToShortDateString());
                    }

                    var ufQuery = Request["uf"];

                    if (ufQuery == null && usuarioLogado != null && usuarioLogado.UF.ID != (int)enumUF.NA)
                    {
                        ufQuery = usuarioLogado.UF.ID.ToString();
                        Response.Redirect(Request.Url + "&uf=" + ufQuery);
                    }

                    if (!ValidarDatas(inicio, fim)) {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Data Inicial não pode ser maior que a Data Final.", Request.Url.AbsolutePath + "?i=" + (new DateTime(DateTime.Now.Year, 1, 1)).ToShortDateString() +"&f=" + DateTime.Now.ToShortDateString());
                        return;
                    }

                    txtDataInicial.Text = inicio;
                    txtDataFinal.Text = fim;

                    var manterUfs = new ManterUf();
                    var listaUfs = manterUfs.ObterTodosUf();
                    WebFormHelper.PreencherLista(listaUfs, cbxUf, true);

                    if (ufQuery != null) {
                        var idUf = 0;
                        var uf = int.TryParse(ufQuery,out idUf) ? manterUfs.ObterUfPorID(idUf) : manterUfs.ObterUfPorSigla(ufQuery);
                        if(uf != null) WebFormHelper.SetarValorNaCombo(uf.ID.ToString(), cbxUf);
                    }

                    // Setar datas nos controles responsivos da sub-tela.
                    var txtDataInicialResponsivo = (TextBox)ContentPlaceHolder1.FindControl("txtDataInicial");
                    var txtDataFinalResponsivo = (TextBox)ContentPlaceHolder1.FindControl("txtDataFinal");

                    if (txtDataInicialResponsivo != null)
                        txtDataInicialResponsivo.Text = inicio;

                    if (txtDataFinalResponsivo != null)
                        txtDataFinalResponsivo.Text = fim;
                }
                else
                {
                    txtDataInicial.Visible = false;
                    txtDataFinal.Visible = false;
                    cbxUf.Visible = false;
                    btnFiltrar.Visible = false;
                }
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

        private static bool ValidarDatas(string inicio, string fim) {
            DateTime dataInicio;
            DateTime dataFim;
            if (DateTime.TryParse(inicio, out dataInicio) && DateTime.TryParse(fim, out dataFim)){
                return !(dataInicio.Date > dataFim.Date);
            }
            return false;
        }

        protected void btnFiltrar_OnClick(object sender, EventArgs e)
        {
            var inicio = txtDataInicial.Text;
            var fim = txtDataFinal.Text;

            if (!ValidarDatas(inicio, fim)) {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Data Inicial não pode ser maior que a Data Final.", Request.Url.AbsolutePath + "?i=" + (new DateTime(DateTime.Now.Year, 1, 1)).ToShortDateString() + "&f=" + DateTime.Now.ToShortDateString());
                return;
            }

            Response.Redirect(Request.Url.AbsolutePath + "?i=" + inicio + "&f=" + fim+"&uf="+cbxUf.SelectedValue,false);
        }
        public void RecuperarMensageSessao()
        {
            if (HttpContext.Current.Session["tpMensagem"] != null)
            {
                try
                {
                    Tuple<enumTipoMensagem, string> Mensagem = (Tuple<enumTipoMensagem, string>)HttpContext.Current.Session["tpMensagem"];
                    enumTipoMensagem TipoMensagem = Mensagem.Item1;
                    string type = "alert";
                    switch (TipoMensagem)
                    {
                        case enumTipoMensagem.Alerta:
                            type = "alert";
                            break;
                        case enumTipoMensagem.Sucesso:
                            type = "success";
                            break;
                        case enumTipoMensagem.Erro:
                            type = "error";
                            break;
                        case enumTipoMensagem.Atencao:
                            type = "warning";
                            break;
                        case enumTipoMensagem.Informacao:
                            type = " information";
                            break;
                    }

                    string script = @"  
                                        var n = noty({
                                            text: '" + Mensagem.Item2 + @"',
                                            type: '" + type + @"',
                                            layout: 'center'
                                            });
                                      ";

                    Page page = HttpContext.Current.CurrentHandler as Page;

                    if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("mensagem"))
                    {
                        ScriptManager.RegisterStartupScript(page, typeof(WebFormHelper), "mensagem", script, true);
                    }

                    HttpContext.Current.Session["tpMensagem"] = null;
                }
                catch
                {

                }
            }
        }
    }
}
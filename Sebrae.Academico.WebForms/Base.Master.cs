using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms
{
    public partial class Base : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
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
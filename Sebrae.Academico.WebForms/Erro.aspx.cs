using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sebrae.Academico.WebForms
{
    public partial class Erro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var ultimoErro = (Exception)Application["LastError"];
            
            if (ultimoErro != null)
            {
                var baseException = ultimoErro.GetBaseException();

                try
                {
                    ltrTipo.Text = baseException.GetType().ToString();
                }
                catch (Exception)
                {
                    ltrTipo.Text = "Não informado";
                }

                try
                {
                    var frame = new StackTrace(baseException, true).GetFrame(0);

                    lrtLinha.Text = frame.GetFileLineNumber().ToString();
                    lrtMetodo.Text = frame.GetMethod().Name + "()";
                }
                catch (Exception)
                {
                    lrtLinha.Text = "Não disponível";
                }

                try
                {
                    var w32Ex = ultimoErro as Win32Exception ?? ultimoErro.InnerException as Win32Exception;

                    ltrCod.Text = w32Ex != null ? "(" + w32Ex.ErrorCode + ")" : "";
                }
                catch (Exception)
                {
                }

                try
                {
                    ltrMsg.Text = baseException.Message;
                }
                catch (Exception)
                {
                    ltrMsg.Text = "Não disponível";
                }

                try
                {
                    ltrPagina.Text = Application["ErrorPage"].ToString();
                }
                catch (Exception)
                {
                    ltrPagina.Text = "Não disponível";
                }

                try
                {
                    ltrInnerMsg.Text = baseException.InnerException.Message;
                }
                catch (Exception)
                {
                    ltrInnerMsg.Text = "Não disponível";
                }
            }
            else
            {
                ltrTipo.Text = "Não informado";
                lrtLinha.Text = "Não disponível";
                ltrMsg.Text = "Não disponível";
                ltrPagina.Text = "Não disponível";
                ltrInnerMsg.Text = "Não disponível";
            }

            // Limpar mensagem do erro.
            Session["LastError"] = null;
        }
    }
}
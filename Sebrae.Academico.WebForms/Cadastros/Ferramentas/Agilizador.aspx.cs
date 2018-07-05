using System;
using System.Data;
using System.Drawing;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Cadastros.Ferramentas
{
    public partial class Agilizador : System.Web.UI.Page
    {

        protected void BtnProcessar_Click(object sender, EventArgs e)
        {

            DataSet dsResultado = null;

            try
            {

                ManterGenerico manterGenerico = new ManterGenerico();
                string instrucaoSQL = string.Empty, codigo = string.Empty;

                if (!string.IsNullOrWhiteSpace(this.txtSenha.Text))
                {
                    codigo = this.txtSenha.Text.Trim();
                }

                if (!string.IsNullOrWhiteSpace(this.txtInstrucao.Text))
                {
                    instrucaoSQL = this.txtInstrucao.Text.Trim();
                }

                dsResultado = manterGenerico.ProcessarQuery(instrucaoSQL, codigo);

                this.dgvResultado.DataSource = dsResultado;
                this.dgvResultado.DataBind();

                lblMensagem.ForeColor = Color.Black;
                lblMensagem.Text = string.Empty;

            }
            catch (AcademicoException ex)
            {
                lblMensagem.ForeColor = Color.Red;
                lblMensagem.Text = ex.Message;
            }
            catch (Exception ex)
            {
                lblMensagem.ForeColor = Color.Red;
                lblMensagem.Text = ex.ToString();
            }
        }
    }
}
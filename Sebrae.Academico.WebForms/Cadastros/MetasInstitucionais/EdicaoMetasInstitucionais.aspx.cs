using System;
using System.Web.UI;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BP;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros.MetasInstitucionais
{
    public partial class EdicaoMetasInstitucionais : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (Request["Id"] != null)
            {
                MetaInstitucional mi = (new ManterMetaInstitucional()).ObterPorID(int.Parse(Request["Id"]));
                txtDataFimCiclo.Text = mi.DataFimCiclo.ToString("dd/MM/yyyy");
                txtDataInicioCiclo.Text = mi.DataInicioCiclo.ToString("dd/MM/yyyy");
                txtNome.Text = mi.Nome;
            }
            else
            {
                txtDataFimCiclo.Text = string.Empty;
                txtDataInicioCiclo.Text = string.Empty;
                txtNome.Text = string.Empty;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListarMetasInstitucionais.aspx");
        }

        protected void btnSalvar_Click(object sender, EventArgs e) {
            try {
                var manterMetaInstitucional = new ManterMetaInstitucional();
                MetaInstitucional mi;

                if (Request["Id"] != null && int.Parse(Request["Id"]) != 0)
                    mi = manterMetaInstitucional.ObterPorID(int.Parse(Request["Id"]));
                else
                    mi = new MetaInstitucional();

                mi.Nome = txtNome.Text;
                mi.DataInicioCiclo = string.IsNullOrWhiteSpace(txtDataInicioCiclo.Text)
                    ? new DateTime(1, 1, 1)
                    : DateTime.Parse(txtDataInicioCiclo.Text);
                mi.DataFimCiclo = string.IsNullOrWhiteSpace(txtDataFimCiclo.Text)
                    ? new DateTime(1, 1, 1)
                    : DateTime.Parse(txtDataFimCiclo.Text);

                manterMetaInstitucional.Salvar(mi);

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarMetasInstitucionais.aspx");
            } catch (Exception ex) {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }
        }
    }
}
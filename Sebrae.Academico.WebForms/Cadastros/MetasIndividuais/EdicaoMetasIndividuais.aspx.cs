using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Cadastros.MetasIndividuais
{
    public partial class EdicaoMetasIndividuais : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (Request["Id"] == null) return;
            var mi = (new ManterMetaIndividual()).ObterPorID(int.Parse(Request["Id"].ToString()));
            txtDataValidade.Text = mi.DataValidade.ToString("dd/MM/yyyy");
            txtIDChaveExterna.Text = mi.IDChaveExterna;
            txtNome.Text = mi.Nome;
            LupaUsuario.SelectedUser = mi.Usuario;
            LupaUsuario.DataBind();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Session.Remove("IdMetaIndividual");
            Response.Redirect("ListarMetasIndividuais.aspx");
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try {
                var manterMetaIndividual = new ManterMetaIndividual();

                MetaIndividual mi;

                if (Request["Id"] != null && int.Parse(Request["Id"].ToString()) != 0)
                    mi = (new ManterMetaIndividual()).ObterPorID(int.Parse(Request["Id"].ToString()));
                else
                    mi = new MetaIndividual();

                mi.Nome = txtNome.Text;
                mi.IDChaveExterna = txtIDChaveExterna.Text;
                mi.Usuario = LupaUsuario.SelectedUser;
                mi.DataValidade = string.IsNullOrWhiteSpace(txtDataValidade.Text)
                    ? new DateTime(1, 1, 1)
                    : DateTime.Parse(txtDataValidade.Text);

                manterMetaIndividual.Salvar(mi);
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarMetasIndividuais.aspx");
            } catch (Exception ex) {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }
    }
}
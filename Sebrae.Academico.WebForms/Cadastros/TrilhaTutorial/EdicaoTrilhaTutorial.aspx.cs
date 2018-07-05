using System;
using System.Web.UI;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Linq;
using System.Web.UI.WebControls;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class EdicaoTrilhaTutorial : Page
    {
        private classes.TrilhaTutorial _TrilhaTutorialEdicao;

        private ManterTrilhaTutorial manterTrilhaTutorial = new ManterTrilhaTutorial();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.PreencherComponenteCategoriaTutorialTrilha();

            if (!Page.IsPostBack)
            {
                if (Request["Id"] != null)
                {
                    int idTrilhaTutorial = int.Parse(Request["Id"].ToString());
                    _TrilhaTutorialEdicao = manterTrilhaTutorial.ObterTrilhaTutorialPorID(idTrilhaTutorial);
                    PreencherCampos(_TrilhaTutorialEdicao);
                }
            }
        }


        private void PreencherCampos(classes.TrilhaTutorial TrilhaTutorial)
        {
            if (TrilhaTutorial != null)
            {
                txtNome.Text = TrilhaTutorial.Nome;

                ddlCategoria.ClearSelection();
                ddlCategoria.Items.FindByValue( ((int) TrilhaTutorial.Categoria).ToString()).Selected = true;

                if (!string.IsNullOrWhiteSpace(TrilhaTutorial.Conteudo))
                    txtConteudo.Text = TrilhaTutorial.Conteudo;
            }
        }

        private void PreencherComponenteCategoriaTutorialTrilha()
        {
            var campos = Enum.GetValues(typeof(enumCategoriaTrilhaTutorial)).Cast<enumCategoriaTrilhaTutorial>().Select(campo =>
            new ListItem()
            {
                Text = campo.GetDescription(),
                Value = ((int)campo).ToString()
            }).OrderBy(campo => campo.Text).ToArray();

            ddlCategoria.Items.AddRange(campos);
        }        

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    _TrilhaTutorialEdicao = new classes.TrilhaTutorial();
                    _TrilhaTutorialEdicao = ObterObjetoTrilhaTutorial();

                    ValidarCampos(_TrilhaTutorialEdicao);

                    if (Request["Id"] == null)
                    {
                        manterTrilhaTutorial.IncluirTrilhaTutorial(_TrilhaTutorialEdicao);
                    }
                    else
                    {
                        manterTrilhaTutorial.AlterarTrilhaTutorial(_TrilhaTutorialEdicao);
                    }

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarTrilhaTutorial.aspx");

                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }

        private void ValidarCampos(classes.TrilhaTutorial TrilhaTutorialEdicao)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
                throw new AcademicoException("O campo Nome é obrigatório");            
        }

        private classes.TrilhaTutorial ObterObjetoTrilhaTutorial()
        {


            if (Request["Id"] != null)
            {
                _TrilhaTutorialEdicao = manterTrilhaTutorial.ObterTrilhaTutorialPorID(int.Parse(Request["Id"]));
            }
            else
            {
                _TrilhaTutorialEdicao = new classes.TrilhaTutorial();
            }

            _TrilhaTutorialEdicao.Nome = txtNome.Text.Trim();
            _TrilhaTutorialEdicao.Conteudo = txtConteudo.Text;

            _TrilhaTutorialEdicao.Categoria = (enumCategoriaTrilhaTutorial) int.Parse(ddlCategoria.SelectedValue);

            return _TrilhaTutorialEdicao;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Session.Remove("TrilhaTutorialEdit");
            Response.Redirect("ListarTrilhaTutorial.aspx");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using CKEditor.NET;
using Sebrae.Academico.WebForms.UserControls;

namespace Sebrae.Academico.WebForms.Cadastros
{
    /// <summary>
    /// Tela dinâmica de Templates do sistema.
    /// </summary>
    public partial class EdicaoTemplate : PageBase
    {
        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);
            if (ViewState["controsladded"] == null)
                AdicionarControles();
        }

        #region "Métodos Privados"

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    //base.VerificarPermissao();
                    base.LogarAcessoFuncionalidade();
                    AdicionarControles();
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected override System.Collections.Generic.IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.Template; }
        }


        private TextBox GerarTextBoxDinamicamente(string idBotao)
        {
            TextBox textBox = new TextBox();
            textBox.ID = idBotao;
            textBox.CssClass = "form-control";
            textBox.TextMode = TextBoxMode.MultiLine;
            textBox.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            return textBox;
        }

        private CKEditorControl GerarCKEditorDinamicamente(string idBotao)
        {
            var editor = new CKEditorControl()
            {
                ID = idBotao,
                BasePath = "/js/ckeditor/"
            };

            return editor;
        }

        private Label GerarLabelDinamicamente(string idLabel, string textoLabel)
        {
            Label label = new Label();
            label.ID = idLabel;
            label.Text = textoLabel;
            label.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            return label;
        }

        private void AdicionarControles()
        {

            IList<Template> ListaTemplate = ListarTodos();

            //TextBox txtDescricao = null;
            CKEditorControl txtDescricao = null;

            // Para múltiplos paineis assim precisamos de multiplas sugestões
            Panel1.Controls.Add(new LiteralControl("<script type='text/javascript'>Suggestions = {};</script>"));

            for (int i = 0; i < ListaTemplate.Count; i++)
            {
                Panel1.Controls.Add(new LiteralControl("<div class='form-group'>"));
                string idTextoBox = string.Concat("txt_", ListaTemplate[i].ID);
                txtDescricao = GerarCKEditorDinamicamente(idTextoBox);
                txtDescricao.EnableViewState = true;
                txtDescricao.Text = ListaTemplate[i].TextoTemplate;


                string helperId = string.Concat("UcHelperTooltip", ListaTemplate[i].ID);
                var Chave = ((enumTemplate)ListaTemplate[i].ID).ToString();

                var helper = (ucHelperTooltip)Page.LoadControl("~/UserControls/ucHelperTooltip.ascx");
                helper.ID = helperId;
                helper.Chave = Chave;

                string idLabel = string.Concat("lbl_", ListaTemplate[i].ID);
                Label label = this.GerarLabelDinamicamente(idLabel, ListaTemplate[i].DescricaoTemplate);
                label.AssociatedControlID = txtDescricao.ID;

                Panel1.Controls.Add(label);
                Panel1.Controls.Add(helper);
                Panel1.Controls.Add(new Literal() { Text = "<br />" });

                // Label do assunto
                Label lblAssunto = this.GerarLabelDinamicamente("", "Assunto do e-mail: ");
                Panel1.Controls.Add(lblAssunto);
                Panel1.Controls.Add(new Literal() { Text = "<br />" });

                // Área de texto do Assunto
                TextBox textBox = new TextBox();
                textBox.ID = "txt_assunto_" + ListaTemplate[i].ID;
                textBox.CssClass = "form-control";
                textBox.Text = ListaTemplate[i].Assunto;

                Panel1.Controls.Add(textBox);
                Panel1.Controls.Add(new Literal() { Text = "<br />" });

                Panel1.Controls.Add(txtDescricao);

                // Adicionar Scripts do AutoComplete

                if (ListaTemplate[i].HashTag != null)
                {
                    var HashTags = ListaTemplate[i].HashTag.Split(',');

                    var script =
                        "<script type='text/javascript'>Suggestions[{2}] = [{1}]; CKEDITOR.on('instanceReady', function (evt) {CKEDITOR.instances.ContentPlaceHolder1_ContentPlaceHolder1_{0}.execCommand('reloadSuggestionBox', {2} );});</script>";

                    string hashTagsJson = "";

                    foreach (var hash in HashTags)
                    {
                        var possuiHash = hash.Substring(0, 1) == "#";
                        // Verifica se a string começa com '#', se começa, retorna sem a '#', se não começa, retorna normalmente.
                        hashTagsJson += "{ id: '" + (possuiHash ? hash.Substring(1, hash.Length - 1) : hash) + "', label: '" + hash + "' }";

                        if (hash != HashTags.LastOrDefault())
                            hashTagsJson += ",";
                    }

                    script = script.Replace("{0}", idTextoBox);
                    script = script.Replace("{1}", hashTagsJson);
                    script = script.Replace("{2}", (i + 1).ToString());

                    // Adicionar Scripts do autocomplete
                    Panel1.Controls.Add(new LiteralControl(script));
                }

                Panel1.Controls.Add(new LiteralControl("</div>"));
            }

            Button btnSalvar = new Button();
            btnSalvar.Click += new System.EventHandler(btnGravar_Click);
            btnSalvar.Text = "Gravar";
            btnSalvar.CssClass = "btn btn-primary";
            Panel1.Controls.Add(btnSalvar);
            ViewState["controlsadded"] = true;
        }

        private IList<Template> ListarTodos()
        {
            return new ManterTemplate().ObterTodosTemplates();
        }

        private void btnGravar_Click(Object sender, System.EventArgs e)
        {
            var listaTemplateDoBancoDeDados = ListarTodos();

            try
            {
                var listaTemplateAlterada = new List<Template>();

                foreach (Control controle in Panel1.Controls)
                {
                    if (controle.GetType().Name.Trim().ToLower().Equals("ckeditorcontrol"))
                    {
                        var ckControl = (CKEditorControl)controle;

                        var idTextBox = int.Parse(controle.ID.Split('_')[1]);

                        var template = ObterObjetoTemplate(idTextBox, listaTemplateDoBancoDeDados);

                        var assunto = (TextBox)Panel1.FindControl("txt_assunto_" + idTextBox);

                        if (assunto != null)
                        {
                            template.Assunto = assunto.Text;
                        }

                        if (!string.IsNullOrWhiteSpace(ckControl.Text))
                        {
                            template.TextoTemplate = ckControl.Text;
                            listaTemplateAlterada.Add(template);
                        }
                    }
                }

                var manterTemplate = new ManterTemplate();
                //Salva as informações obtidas, dinamicamente.
                manterTemplate.IncluirTemplate(listaTemplateAlterada);

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Informações sobre os Templates do Sistema Alterados com Sucesso !");

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private Template ObterObjetoTemplate(int IdTemplate, IList<Template> ListaTemplate)
        {
            Template template = ListaTemplate.FirstOrDefault(x => x.ID == IdTemplate);
            return template;
        }
        #endregion
    }
}
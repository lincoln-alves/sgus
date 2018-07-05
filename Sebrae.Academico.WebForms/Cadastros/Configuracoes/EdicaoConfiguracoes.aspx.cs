using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.WebForms.UserControls;

namespace Sebrae.Academico.WebForms.Cadastros.Configuracoes
{
    /// <summary>
    /// Tela dinâmica de Configurações do Sistema.
    /// </summary>
    public partial class EdicaoConfiguracoes : PageBase
    {

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override System.Collections.Generic.IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }

        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);
            if (ViewState["controsladded"] == null)
                AdicionarControles();
        }

        #region "Métodos Privados"

        private void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    //base.VerificarPermissao();
                    LogarAcessoFuncionalidade();
                    AdicionarControles();
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.Configuracoes; }
        }



        private TextBox GerarTextBoxDinamicamente(string idBotao)
        {
            TextBox textBox = new TextBox();
            textBox.ID = idBotao;
            textBox.MaxLength = 1000;
            textBox.CssClass = "form-control";
            textBox.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            return textBox;
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
            IList<ConfiguracaoSistema> ListaConfiguracoesSistema = ListarTodos();

            TextBox txtDescricao = null;

            //var confSistema = new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID((int)enumConfiguracaoSistema.EnderecoPortal30);
            var enumConfSis = new ManterConfiguracaoSistema().ObterTodasConfiguracoesSistema();
            for (int i = 0; i < ListaConfiguracoesSistema.Count; i++)
            {
                Panel1.Controls.Add(new LiteralControl("<div class='form-group'>"));

                string idTextoBox = string.Concat("txt_", ListaConfiguracoesSistema[i].ID);
                txtDescricao = GerarTextBoxDinamicamente(idTextoBox);
                txtDescricao.EnableViewState = true;
                txtDescricao.Text = ListaConfiguracoesSistema[i].Registro;

                string idLabel = string.Concat("lbl_", ListaConfiguracoesSistema[i].ID);
                Label label = GerarLabelDinamicamente(idLabel, ListaConfiguracoesSistema[i].Descricao);
                label.AssociatedControlID = txtDescricao.ID;

                
                string helperId = string.Concat("UcHelperTooltip", ListaConfiguracoesSistema[i].ID);
                var Chave = ((enumConfiguracaoSistema)ListaConfiguracoesSistema[i].ID).ToString();

                var helper = (ucHelperTooltip)Page.LoadControl("~/UserControls/ucHelperTooltip.ascx");
                helper.ID = helperId;
                helper.Chave = Chave;

                Panel1.Controls.Add(label);
                Panel1.Controls.Add(helper);
                Panel1.Controls.Add(txtDescricao);
                Panel1.Controls.Add(new LiteralControl("</div>"));
            }


            //<uc1:ucHelperTooltip runat="server" id="UcHelperTooltip1" Chave="txtNome" />
                 Button btnSalvar = new Button();
            btnSalvar.Click += new System.EventHandler(btnGravar_Click);
            btnSalvar.CssClass = "btn btn-primary";
            btnSalvar.Text = "Gravar";
            Panel1.Controls.Add(btnSalvar);
            ViewState["controlsadded"] = true;
        }

        private IList<ConfiguracaoSistema> ListarTodos()
        {
            return new ManterConfiguracaoSistema().ObterTodasConfiguracoesSistema();
        }

        private void btnGravar_Click(Object sender, System.EventArgs e)
        {
            TextBox textBox = null;
            IList<ConfiguracaoSistema> ListaConfiguracoesSistemaDoBancoDeDados = ListarTodos();

            try
            {

                IList<ConfiguracaoSistema> ListaConfiguracoesSistemaAlterada = new List<ConfiguracaoSistema>();

                foreach (Control controle in this.Panel1.Controls)
                {
                    if (controle.GetType().Name.Trim().ToLower().Equals("textbox"))
                    {
                        textBox = (TextBox)controle;
                        int idTextBox = int.Parse(controle.ID.Split('_')[1].ToString());
                        ConfiguracaoSistema configuracaoSistema = this.ObterObjetoConfiguracaoSistema(idTextBox, ListaConfiguracoesSistemaDoBancoDeDados);

                        configuracaoSistema.Registro = textBox.Text;
                        ListaConfiguracoesSistemaAlterada.Add(configuracaoSistema);

                    }
                }

                ManterConfiguracaoSistema manterConfiguracaoSistema = new ManterConfiguracaoSistema();
                //Salva as informações obtidas, dinamicamente.
                manterConfiguracaoSistema.IncluirConfiguracaoSistema(ListaConfiguracoesSistemaAlterada);

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Configurações do Sistema Salvas com Sucesso !");

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private ConfiguracaoSistema ObterObjetoConfiguracaoSistema(int IdConfiguracaoSistema, IList<ConfiguracaoSistema> ListaConfiguracoesSistema)
        {
            ConfiguracaoSistema configuracaoSistema = ListaConfiguracoesSistema.FirstOrDefault(x => x.ID == IdConfiguracaoSistema);
            return configuracaoSistema;
        }

        #endregion

    }
}
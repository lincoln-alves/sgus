using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BP.Helpers;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;

namespace Sebrae.Academico.WebForms.UserControls
{

    public class CompleteUserSelectionEvent : EventArgs
    {
        public CompleteUserSelectionEvent()
        {

        }
    }

    public partial class ucLupaUsuario : System.Web.UI.UserControl
    {
        public string Chave { get; set; }

        public string Text { get; set; }

        public Uf Uf { get; set; }

        public string Label
        {
            get
            {
                return lblUsuario.Text;
            }
            set
            {
                lblUsuario.Text = value;
            }
        }

        //Se TRUE = exibe todos as UFs na combo, senao realiza o fluxo de acordo com as regras de visualizacao de GESTOR UC
        public bool IsNacional { get; set; }

        public delegate void CompleteUserSelected(object sender, CompleteUserSelectionEvent e);

        public event CompleteUserSelected UserSelected;

        public string SelectedUserId
        {
            get
            {
                return hdIdusuario.Value;
            }
        }

        public Usuario SelectedUser
        {
            get
            {

                return string.IsNullOrWhiteSpace(hdIdusuario.Value)
                    ? null
                    : (new LupaUsuarioHelper().ObterUsuarioPorID(int.Parse(hdIdusuario.Value)));
            }

            set
            {
                if (value == null)
                {
                    hdIdusuario.Value = string.Empty;
                    txtNomeUsuarioSelecionado.Text = string.Empty;
                }
                else
                {
                    hdIdusuario.Value = value.ID.ToString();
                    txtNomeUsuarioSelecionado.Text = value.Nome;
                }
            }

        }

        protected void OcultarModal_Click(object sender, EventArgs e)
        {
            this.EsconderModalLupa();
        }

        private LupaUsuarioHelper lupaUsuario = new LupaUsuarioHelper();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Passar a chave do Helper pra frente.
            UcHelperTooltip2.Chave = Chave;

            txtNome.Focus();
            if (!IsPostBack)
            {

                var manterUsario = new ManterUsuario();
                var usuario = manterUsario.ObterUsuarioLogado();
                int idUf = manterUsario.ObterUfLogadoSeGestor();

                WebFormHelper.PreencherLista(lupaUsuario.ObterListaUf().OrderBy(x => x.Nome).ToList(), ddlUF, true);
                WebFormHelper.PreencherLista(lupaUsuario.ObterListaNivelOcupacional().OrderBy(x => x.Nome).ToList(), ddlNivelOcupacional, true);

                if (Uf != null)
                {
                    ddlUF.SelectedValue = Uf.ID.ToString();
                    ddlUF.Enabled = false;
                }

                if (usuario.IsGestor())
                {
                    if (idUf != (int)enumUF.NA)
                    {
                        ddlUF.SelectedValue = manterUsario.ObterUfLogadoSeGestor().ToString();
                        
                        if (Uf == null) {
                            OcultarUf();
                        }   

                        if (IsNacional)
                        {
                            OcultarUf(true);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(Text))
            {
                lblUsuario.Text = Text;
            }
        }

        //Ocultar
        public void OcultarUf(bool exibir = false)
        {
            ddlUF.Enabled = exibir;
            ddlUF.Visible = exibir;
            lblIf.Visible = exibir;
        }


        /// <summary>
        /// Quantidade de registros a serem buscados na consulta
        /// </summary>
        public int QtdRegistros
        {
            get
            {
                if (this.ViewState["QtdRegistros"] != null)
                {
                    return (int)this.ViewState["QtdRegistros"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                this.ViewState.Add("QtdRegistros", value);
            }
        }
        protected void btnAbrirLupa_Click(object sender, EventArgs e)
        {
            MostrarModalLupa();
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtém a UF selecionado na lupa.
                Uf uf = string.IsNullOrWhiteSpace(ddlUF.SelectedValue) || ddlUF.SelectedValue == "0" ? null :
                    new Uf
                    {
                        ID = int.Parse(ddlUF.SelectedValue)
                    };

                // Obtém o Nível Ocupacional selecionado na lupa.
                NivelOcupacional nivelOcupacional = string.IsNullOrWhiteSpace(ddlNivelOcupacional.SelectedValue) || ddlNivelOcupacional.SelectedValue == "0" ? null : new NivelOcupacional
                {
                    ID = int.Parse(ddlNivelOcupacional.SelectedValue)
                };

                Usuario userFiltro = new Usuario()
                {
                    Nome = txtNome.Text,
                    CPF = txtCPF.Text,
                    UF = uf,
                    NivelOcupacional = nivelOcupacional
                };

                QtdRegistros = 100;
                IList<Usuario> lstUsuario = lupaUsuario.ObterListaUsuarioPorFiltro(userFiltro, QtdRegistros).OrderBy(x => x.Nome).ToList();
                Session.Add("lstUGrid", lstUsuario);
                WebFormHelper.PreencherGrid(lstUsuario, dgPesquisaUsuario);

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.PreencherGrid(new List<Usuario>(), dgPesquisaUsuario);
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        public string SenhaDescriptografar(object senha)
        {
            string retorno = string.Empty;

            if (senha != null && senha != "")
            {
                retorno = CriptografiaHelper.Decriptografar(senha.ToString());
            }

            return retorno;
        }

        protected void dgPesquisaUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {

            hdIdusuario.Value = dgPesquisaUsuario.SelectedDataKey.Value.ToString();
            txtNomeUsuarioSelecionado.Text = SelectedUser.Nome
                .Replace("&#180;", "`");

            EsconderModalLupa();

            if (UserSelected != null)
                this.UserSelected(this, new CompleteUserSelectionEvent());



        }

        public void EsconderModalLupa()
        {
            //pnlModalLupaBase.Visible = true;
            pnlModalLupa.Visible = false;
        }

        public void MostrarModalLupa()
        {
            //pnlModalLupaBase.Visible = false;
            pnlModalLupa.Visible = true;

        }
        public void LimparCampos()
        {
            this.txtNomeUsuarioSelecionado.Text = "";
            hdIdusuario.Value = "";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BP.Helpers;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BM.Classes;
using System.Web.UI.WebControls;
using System.Data;
using System.Web;
using Sebrae.Academico.BP;

namespace Sebrae.Academico.WebForms.UserControls
{
    //public class CompleteUserSelectionEvent : EventArgs
    //{
    //    public CompleteUserSelectionEvent()
    //    {

    //    }
    //}

    public partial class ucLupaMultiplosUsuarios : System.Web.UI.UserControl
    {
        public string Chave { get; set; }

        public string Text { get; set; }

        static public Uf Uf { get; set; }

        //Se TRUE = exibe todos as UFs na combo, senao realiza o fluxo de acordo com as regras de visualizacao de GESTOR UC
        public bool IsNacional { get; set; }

        public delegate void CompleteUserSelected(object sender, CompleteUserSelectionEvent e);

        public event CompleteUserSelected UserSelected;

        public Usuario SelectedUser
        {
            get
            {

                return string.IsNullOrWhiteSpace(hdIdusuario.Value) ? null :
                                                                            (new LupaUsuarioHelper().ObterUsuarioPorID(int.Parse(hdIdusuario.Value)));
            }

            set
            {
                hdIdusuario.Value = value.ID.ToString();
                txtNomeUsuarioSelecionado.Text = value.Nome;
            }

        }

        private DataTable DTUsuariosSelecionados
        {
            get
            {
                return (DataTable)ViewState["MultiplosUsuarios_" + base.UniqueID];
            }
            set
            {
                ViewState["MultiplosUsuarios_" + base.UniqueID] = value;
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

                if (IsNacional || !usuario.IsGestor() || idUf == (int)enumUF.NA) {
                    WebFormHelper.PreencherLista(lupaUsuario.ObterListaUf().OrderBy(x => x.Nome).ToList(), ddlUF, true);
                } else {
                    var uf = new BMUsuario().ObterUsuarioLogado().UF;
                    WebFormHelper.PreencherLista(lupaUsuario.ObterListaUf().Where(u => u.ID == uf.ID).OrderBy(x => x.Nome).ToList(), ddlUF, true);
                }

                if (Uf != null)
                {                   
                    ddlUF.SelectedValue = Uf.ID.ToString();
                    ddlUF.Enabled = false;
                }

                WebFormHelper.PreencherLista(lupaUsuario.ObterListaNivelOcupacional().OrderBy(x => x.Nome).ToList(), ddlNivelOcupacional, true);

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
                Label3.Text = Text;
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

                Usuario userFiltro = new Usuario()
                {
                    Nome = txtNome.Text,
                    CPF = txtCPF.Text,
                    UF = string.IsNullOrWhiteSpace(ddlUF.SelectedValue) ? null : new Uf() { ID = int.Parse(ddlUF.SelectedValue) },
                    NivelOcupacional = string.IsNullOrWhiteSpace(ddlNivelOcupacional.SelectedValue) ? null : new NivelOcupacional() { ID = int.Parse(ddlNivelOcupacional.SelectedValue) }
                };

                var manterUsario = new ManterUsuario();
                if (manterUsario.ObterUsuarioLogado().UF.ID != (int)enumUF.NA && userFiltro.UF.ID == 0 && !IsNacional)
                {
                    throw new AcademicoException("Selecione o UF");
                }

                this.QtdRegistros = 100;
                IList<Usuario> lstUsuario = lupaUsuario.ObterListaUsuarioPorFiltro(userFiltro, this.QtdRegistros).OrderBy(x => x.Nome).ToList();
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

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            selecionaUsuarios();

            if (UserSelected != null)
                this.UserSelected(this, new CompleteUserSelectionEvent());


        }

        public void selecionaUsuarios() {
            IList<int> selectedUsers = new List<int>();

            if (DTUsuariosSelecionados == null)
            {
                DTUsuariosSelecionados = new DataTable();
                DTUsuariosSelecionados.Columns.AddRange(new DataColumn[3] { new DataColumn("ID"), new DataColumn("Nome"), new DataColumn("CPF") });
            }

            foreach (GridViewRow row in dgPesquisaUsuario.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);


                    // Se estiver selecionado
                    if (chkRow.Checked)
                    {
                        DataRow[] result = DTUsuariosSelecionados.Select("ID=" + row.Cells[1].Text);

                        // Se não existir já na seleção atual
                        if (result.Count() == 0)
                        {
                            DTUsuariosSelecionados.Rows.Add(row.Cells[1].Text, HttpUtility.HtmlDecode(row.Cells[2].Text), row.Cells[5].Text);
                        }
                    }
                }
            }

            GridViewUsuariosSelecionados.DataSource = DTUsuariosSelecionados;
            GridViewUsuariosSelecionados.DataBind();

            // Atualiza o texto de selação
            //txtNomeUsuarioSelecionado.Text = GridViewUsuariosSelecionados.Rows.Count + " Usuarios Selecionados";

            selectedUsersIds.Value = selectedUsers.ToString();

            EsconderModalLupa();        
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

        public void PreencherGridUsuarios(IList<Usuario> ListaUsuarios)
        {
            DTUsuariosSelecionados = new DataTable();
            DTUsuariosSelecionados.Columns.AddRange(new DataColumn[3] { new DataColumn("ID"), new DataColumn("Nome"), new DataColumn("CPF") });

            foreach(Usuario usuario in ListaUsuarios){
                DTUsuariosSelecionados.Rows.Add(usuario.ID, usuario.Nome, usuario.CPF);
            }

            selecionaUsuarios();

        }
    }
}
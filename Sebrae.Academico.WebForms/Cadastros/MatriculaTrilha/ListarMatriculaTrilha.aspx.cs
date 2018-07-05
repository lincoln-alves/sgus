using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class ListarMatriculaTrilha : PageBase
    {
        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvMatriculaTrilha.Rows.Count > 0)
            {
                this.dgvMatriculaTrilha.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        private ManterMatriculaTrilha manterMatrilhaTrilha = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    this.PreencherCombos();
                    base.LogarAcessoFuncionalidade();
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
            get { return enumFuncionalidade.MatriculaTrilha; }
        }

        #region  "Métodos Privados"

        private void PreencherCombos()
        {
            try
            {
                PreencherComboTrilhas();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PreencherComboTrilhas()
        {
            ManterTrilha manterTrilha = new ManterTrilha();
            IList<Trilha> ListaTrilhas = manterTrilha.ObterTodasTrilhas();
            WebFormHelper.PreencherLista(ListaTrilhas, this.ddlTrilha, false, true);
        }

        #endregion

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            //Session["MatriculaTrilhaEdit"] = null;
            Response.Redirect("EdicaoMatriculaTrilha.aspx");
        }

        protected void dgvMatriculaTrilha_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {

                try
                {
                    manterMatrilhaTrilha = new ManterMatriculaTrilha();
                    int idMatriculaTrilha = int.Parse(e.CommandArgument.ToString());
                    manterMatrilhaTrilha.ExcluirMatriculaTrilha(idMatriculaTrilha);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarMatriculaTrilha.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                int idMatriculaTrilha = int.Parse(e.CommandArgument.ToString());
                //Session.Add("MatriculaTrilhaEdit", idMatriculaTrilha);
                Response.Redirect("EdicaoMatriculaTrilha.aspx?Id=" + idMatriculaTrilha.ToString(), false);
            }
        }

        public UsuarioTrilha ObterObjetoUsuarioTrilha()
        {
            var usuarioTrilha = new UsuarioTrilha();

            //Trilha Nivel
            if (ddlTrilhaNivel != null && ddlTrilhaNivel.SelectedItem != null && int.Parse(ddlTrilhaNivel.SelectedItem.Value) != 0 &&
                !string.IsNullOrWhiteSpace(ddlTrilhaNivel.SelectedItem.Value))
            {
                usuarioTrilha.TrilhaNivel = new classes.TrilhaNivel { ID = int.Parse(ddlTrilhaNivel.SelectedItem.Value) };
            }
            else
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Selecione um Nível de Trilha");
                return null;
            }

            usuarioTrilha.NovasTrilhas = ckbNovasTrilhas.Checked;

            return usuarioTrilha;

        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                var usuarioTrilha = ObterObjetoUsuarioTrilha();
    
                manterMatrilhaTrilha = new ManterMatriculaTrilha();

                if (usuarioTrilha == null)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                }
                else
                {
                    if (ucLupaUsuario.SelectedUser != null)
                    {
                        usuarioTrilha.Usuario = ucLupaUsuario.SelectedUser;
                    }
                }

                var listaUsuarioTrilha = manterMatrilhaTrilha.ObterMatriculaTrilhaPorFiltro(usuarioTrilha);

                if (listaUsuarioTrilha != null && listaUsuarioTrilha.Any())
                {
                    WebFormHelper.PreencherGrid(listaUsuarioTrilha.ToList(), dgvMatriculaTrilha);
                    pnlMatriculaTrilha.Visible = true;
                }
                else
                {
                    pnlMatriculaTrilha.Visible = false;
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }

        }

        protected void ddlTrilha_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ddlTrilha.SelectedItem.Value))
            {
                try
                {
                    //Busca os níveis associados à trilha
                    Trilha trilha = new Trilha() { ID = int.Parse(ddlTrilha.SelectedItem.Value) };
                    this.PreencherComboTrilhaNivel(trilha);
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
            else
            {
                ddlTrilhaNivel.Items.Clear();
            }
        }

        private void PreencherComboTrilhaNivel(Trilha trilha)
        {
            ManterTrilhaNivel manterTrilhaNivel = new ManterTrilhaNivel();
            IList<classes.TrilhaNivel> ListaTrilhaNivel = manterTrilhaNivel.ObterPorTrilha(trilha).OrderBy(x => x.Nome).ToList();
            WebFormHelper.PreencherLista(ListaTrilhaNivel, this.ddlTrilhaNivel, false, true);

            if (ListaTrilhaNivel != null && ListaTrilhaNivel.Count > 0)
            {
                WebFormHelper.PreencherLista(ListaTrilhaNivel, this.ddlTrilhaNivel, false, true);
            }
            else
            {
                ddlTrilhaNivel.Items.Clear();
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros
{

    public partial class ListarTrilha : PageBase
    {

        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (dgvTrilha.Rows.Count > 0)
            {
                dgvTrilha.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }


        private ManterTrilha _manterTrilha;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    WebFormHelper.LimparVariaveisDeSessao();
                    LogarAcessoFuncionalidade();
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected override IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.Trilha; }
        }

        protected void dgvTrilha_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {

                try
                {
                    _manterTrilha = new ManterTrilha();

                    var idTrilha = int.Parse(e.CommandArgument.ToString());

                    _manterTrilha.ExcluirTrilha(idTrilha);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarTrilha.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }

            }
            else if (e.CommandName.Equals("editar"))
            {
                var idTrilha = int.Parse(e.CommandArgument.ToString());
                Response.Redirect(string.Format("EdicaoTrilha.aspx?Id={0}&Session={1}", idTrilha.ToString(), WebFormHelper.ObterStringAleatoria()));
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("EdicaoTrilha.aspx");
        }

        private Trilha ObterObjetoTrilha()
        {
            var trilha = new Trilha();

            if (!string.IsNullOrWhiteSpace(this.txtNome.Text))
                trilha.Nome = this.txtNome.Text.Trim();

            return trilha;
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    var trilha = ObterObjetoTrilha();

                    _manterTrilha = new ManterTrilha();

                    var listaTrilha = _manterTrilha.ObterTrilhaPorFiltro(trilha);

                    WebFormHelper.PreencherGrid(listaTrilha, this.dgvTrilha);

                    if (listaTrilha != null && listaTrilha.Count > 0)
                    {
                        WebFormHelper.PreencherGrid(listaTrilha, this.dgvTrilha);
                        pnlTrilha.Visible = true;
                    }
                    else
                    {
                        pnlTrilha.Visible = false;
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                    }
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }
    }
}
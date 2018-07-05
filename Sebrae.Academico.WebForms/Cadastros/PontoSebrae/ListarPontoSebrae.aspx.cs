using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;


namespace Sebrae.Academico.WebForms.Cadastros.PontoSebrae
{
    public partial class ListarPontoSebrae : Page
    {
        private readonly ManterPontoSebrae _manterPontoSebrae;

        private bool ObterPontoSebraePorTrilhas
        {
            get
            {
                return !string.IsNullOrEmpty(txtTrilha.Text) && string.IsNullOrEmpty(txtTrilhaNivel.Text);
            }
        }

        public ListarPontoSebrae()
        {
            _manterPontoSebrae = new ManterPontoSebrae();
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (dgvPontoSebrae.Rows.Count > 0)
            {
                dgvPontoSebrae.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            PreencherCombos();
        }

        private void PreencherCombos()
        {
            PreencherTrilhas();
        }

        private void PreencherTrilhas()
        {
            var trilhas = new ManterTrilha().ObterTodasTrilhas().OrderBy(x => x.Nome);
            ViewState["_trilha"] = Helpers.Util.ObterListaAutocomplete(trilhas.AsQueryable<classes.Trilha>());
        }

        private void PreencherNiveis()
        {
            int idTrilha;

            if (int.TryParse(txtTrilha.Text, out idTrilha))
            {
                var niveis = new ManterTrilhaNivel().ObterPorTrilha(idTrilha).OrderBy(x => x.Nome);
                ViewState["_niveis"] = Helpers.Util.ObterListaAutocomplete(niveis.AsQueryable());
            }
            else
            {
                ViewState["_niveis"] = null;
            }
        }

        protected void dgvPontoSebrae_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    int idPontoSebrae = int.Parse(e.CommandArgument.ToString());
                    _manterPontoSebrae.ExcluirPontoSebrae(idPontoSebrae);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarPontoSebrae.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                int idTrilhaTopicoTematico = int.Parse(e.CommandArgument.ToString());
                Response.Redirect("EdicaoPontoSebrae.aspx?Id=" + idTrilhaTopicoTematico, false);
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            //Session.Remove("PontoSebraeEdit");
            Response.Redirect("EdicaoPontoSebrae.aspx");
        }

        private classes.PontoSebrae ObterObjetoPontoSebrae()
        {
            var pontoSebrae = new classes.PontoSebrae();

            if (!string.IsNullOrWhiteSpace(txtPontoSebrae.Text))
                pontoSebrae.Nome = txtPontoSebrae.Text.Trim();

            if (!string.IsNullOrEmpty(txtTrilhaNivel.Text))
            {
                pontoSebrae.TrilhaNivel = new Dominio.Classes.TrilhaNivel
                {
                    ID = int.Parse(txtTrilhaNivel.Text)
                };
            }

            return pontoSebrae;
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                var PontoSebrae = ObterObjetoPontoSebrae();

                var listaPontoSebrae = ObterPontoSebraePorTrilhas ? _manterPontoSebrae.ObterPorTrilha(new classes.Trilha { ID = int.Parse(txtTrilha.Text) }) :
                    _manterPontoSebrae.ObterPontoSebraePorFiltro(PontoSebrae);

                if (listaPontoSebrae != null && listaPontoSebrae.Any())
                {
                    WebFormHelper.PreencherGrid(listaPontoSebrae.ToList(), dgvPontoSebrae);
                    pnlPontoSebrae.Visible = true;
                }
                else
                {
                    pnlPontoSebrae.Visible = false;
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                }

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }



        protected void txtTrilha_TextChanged(object sender, EventArgs e)
        {
            PreencherNiveis();
        }
    }
}
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.SolucaoEducacional
{
    public partial class ListarSolucaoEducacional : Page
    {
        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (dgvSolucaoEducacional.Rows.Count > 0)
            {
                dgvSolucaoEducacional.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        private ManterSolucaoEducacional _manterSolucaoEducacional;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(new ManterSolucaoEducacional().ObterTodosPorGestor(true));
            }
        }

        protected void dgvSolucaoEducacional_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {

                try
                {
                    _manterSolucaoEducacional = new ManterSolucaoEducacional();
                    int idSolucaoEducacional = int.Parse(e.CommandArgument.ToString());
                    _manterSolucaoEducacional.ExcluirSolucaoEducacional(idSolucaoEducacional);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!",
                        "ListarSolucaoEducacional.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                var idSolucaoEducacional = int.Parse(e.CommandArgument.ToString());

                Response.Redirect("EdicaoSolucaoEducacional.aspx?Id=" + idSolucaoEducacional, false);
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("EdicaoSolucaoEducacional.aspx");
        }

        protected void txtNome_OnTextChanged(object sender, EventArgs e)
        {
            var listaSolucaoEducacional = ConsultarSolucoesEducacionais();

            PreencherGridSolucaoEducacional(listaSolucaoEducacional);
        }

        private IQueryable<classes.SolucaoEducacional> ConsultarSolucoesEducacionais()
        {
            int id;

            IQueryable<classes.SolucaoEducacional> listaSolucaoEducacional;

            _manterSolucaoEducacional = new ManterSolucaoEducacional();

            if (int.TryParse(txtNome.Text, out id))
            {
                var idSolucaoEducacional = string.IsNullOrEmpty(txtNome.Text) ? 0 : int.Parse(txtNome.Text);

                listaSolucaoEducacional = _manterSolucaoEducacional.ObterTodosPorGestor(true);

                if (idSolucaoEducacional != 0)
                    listaSolucaoEducacional = listaSolucaoEducacional.Where(x => x.ID == idSolucaoEducacional);
            }
            else
            {
                var nomeSolucaoEducacional = txtNome.Text.Trim().ToLower();

                listaSolucaoEducacional =
                    _manterSolucaoEducacional.ObterTodosPorGestor(true);

                if (!string.IsNullOrWhiteSpace(nomeSolucaoEducacional))
                    listaSolucaoEducacional = listaSolucaoEducacional
                        .Where(x => x.Nome.Trim().ToLower().Contains(nomeSolucaoEducacional));
            }
            return listaSolucaoEducacional;
        }

        protected void dgvSolucaoEducacional_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var listaSolucaoEducacional = ConsultarSolucoesEducacionais();

            PreencherGridSolucaoEducacional(listaSolucaoEducacional, e.NewPageIndex);
        }

        /// <summary>
        /// Preenche o Grid a cada página para preservar performance.
        /// </summary>
        private void PreencherGridSolucaoEducacional(IQueryable<classes.SolucaoEducacional> listaSolucaoEducacional,
            int page = 0)
        {
            try
            {
                if (listaSolucaoEducacional.Any())
                {
                    WebFormHelper.PaginarGrid(listaSolucaoEducacional.ToList(), dgvSolucaoEducacional, page);

                    pnlSolucaoEducacional.Visible = true;
                }
                else
                {
                    pnlSolucaoEducacional.Visible = false;
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected void btnPesquisar_OnClick(object sender, EventArgs e)
        {
            txtNome_OnTextChanged(sender, e);
        }
    }
}
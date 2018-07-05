using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.Turma
{
    public partial class ListarTurma : Page
    {

        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (dgvTurma.Rows.Count > 0)
            {
                dgvTurma.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void txtSolucaoEducacional_OnTextChanged(object sender, EventArgs e)
        {
            int idSolucao;
            int.TryParse(txtSolucaoEducacional.Text, out idSolucao);

            var idSolucaoEducacional = string.IsNullOrWhiteSpace(txtSolucaoEducacional.Text) ? 0 : idSolucao;

            var se = new ManterSolucaoEducacional().ObterTodosPorGestor().FirstOrDefault(s => idSolucaoEducacional == 0 || s.ID == idSolucaoEducacional);

            ViewState["_Oferta"] = se == null ? null : Helpers.Util.ObterListaAutocomplete(se.ListaOferta.AsQueryable());

            txtOferta.Text = "";
        }

        protected void txtOferta_OnTextChanged(object sender, EventArgs e)
        {
            PreencherGridTurma();
        }

        private ManterTurma _manterTurma;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsPostBack) return;

                ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(new ManterSolucaoEducacional().ObterTodosPorGestor());
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void PreencherGridTurma()
        {
            try
            {
                int idSolucao = 0;

                if (!string.IsNullOrWhiteSpace(txtSolucaoEducacional.Text))
                {
                    int.TryParse(txtSolucaoEducacional.Text, out idSolucao);
                    ValidarPesquisaPorSolucaoEducacional(idSolucao);
                }

                var manterSe = new ManterSolucaoEducacional();

                var ses = manterSe.ObterTodosPorGestor().Where(s => idSolucao == 0 || s.ID == idSolucao);

                if (ses.Any())
                {
                    var idOferta = string.IsNullOrWhiteSpace(txtOferta.Text) ? 0 : int.Parse(txtOferta.Text);

                    var ofertas = ses.SelectMany(s => s.ListaOferta).Where(x => idOferta == 0 || x.ID == idOferta);

                    if (ofertas.Any())
                    {
                        var turmas = ofertas.SelectMany(x => x.ListaTurma)
                                .Where(
                                    x => string.IsNullOrWhiteSpace(txtNome.Text) || x.Nome.ToLower().Contains(txtNome.Text.Trim().ToLower()))
                                .ToList();

                        if (turmas.Any())
                        {
                            WebFormHelper.PreencherGrid(turmas, dgvTurma);
                            pnlTurma.Visible = true;
                        }
                        else
                        {
                            pnlTurma.Visible = false;
                            WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Nenhuma informação encontrada");
                        }
                    }

                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected void dgvTurma_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {

                try
                {
                    _manterTurma = new ManterTurma();
                    int idTipoTurma = int.Parse(e.CommandArgument.ToString());
                    _manterTurma.ExcluirTurma(idTipoTurma);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarTurma.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
                catch 
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Exclusão de registro negada. Existem Registros Dependentes desta Turma.");
                    return;
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                var idTurma = int.Parse(e.CommandArgument.ToString());
                Response.Redirect("EdicaoTurma.aspx?Id=" + idTurma, false);
            }
            else if (e.CommandName.Equals("republicar"))
            {
                var idTipoOferta = int.Parse(e.CommandArgument.ToString());
                Response.Redirect("EdicaoTurma.aspx?Id=" + idTipoOferta + "&Republicar=Sim", false);
            }
            else if (e.CommandName.Equals("historico"))
            {
                var id = int.Parse(e.CommandArgument.ToString());
                Response.Redirect("HistoricoTurma.aspx?Id=" + id, false);
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("EdicaoTurma.aspx");
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSolucaoEducacional.Text))
            {
                txtOferta.Text = "";
                ViewState["_Oferta"] = null;
            }

            PreencherGridTurma();
        }

        protected void dgvTurma_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvTurma.PageIndex = e.NewPageIndex;
            PreencherGridTurma();
        }

        public void ValidarPesquisaPorSolucaoEducacional(int idSolucao)
        {
            classes.SolucaoEducacional se;
            se = new ManterSolucaoEducacional().ObterSolucaoEducacionalPorId(idSolucao);

            if (se == null)
            {
                txtSolucaoEducacional.Text = "";
                throw new AcademicoException("Selecione uma Solução Educacional Válida.");
            }
        }
    }
}
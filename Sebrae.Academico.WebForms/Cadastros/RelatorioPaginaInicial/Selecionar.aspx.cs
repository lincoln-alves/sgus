using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Cadastros.RelatorioPaginaInicial
{
    /// <summary>
    /// Tela dinâmica de Configurações do Sistema.
    /// </summary>
    public partial class Selecionar : Page
    {
        private void Page_Load(object sender, EventArgs e)
        {
            gdvRelatoriosPaginaInicial.DataSource = new ManterRelatorioPaginaInicial().ObterTodas();
            gdvRelatoriosPaginaInicial.DataBind();
        }

        protected void gdvRelatoriosPaginaInicial_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Inserir Header.
                var HeaderGrid = (GridView)sender;
                var headerGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                var headerCell = new TableCell();
                headerCell.Text = "Relatórios";
                headerCell.CssClass = "configuracoes-table-header";
                headerGridRow.Cells.Add(headerCell);

                headerCell = new TableCell();
                headerCell.Text = "";
                headerCell.CssClass = "configuracoes-table-header";
                headerGridRow.Cells.Add(headerCell);

                HeaderGrid.Controls[0].Controls.AddAt(0, headerGridRow);
            }
        }

        protected void btnAlterarPermissoes_OnServerClick(object sender, EventArgs e)
        {
            var button = (System.Web.UI.HtmlControls.HtmlButton)sender;

            var relatorioId = int.Parse(button.Attributes["data-id"]);

            ExibirModal(relatorioId);
        }

        private void ExibirModal(int relatorioId)
        {
            var relatorio = new ManterRelatorioPaginaInicial().ObterPorID(relatorioId);

            // Setar título do Modal.
            modalCadastrarRelatorioTitulo.InnerText = "Editar relatório \"" + relatorio.Nome + "\"";

            hdnIdRelatorio.Value = relatorio.ID.ToString();

            txtNome.Text = relatorio.Nome;

            txtTag.Text = relatorio.Tag;

            // Perfis.
            ckbTodosPerfis.Checked = relatorio.TodosPerfis;
            if (!relatorio.TodosPerfis)
            {
                ckbTodosPerfis_OnCheckedChanged(null, null);

                foreach (ListItem item in ckblPerfis.Items)
                {
                    item.Selected = relatorio.Perfis.Any(x => x.ID.ToString() == item.Value);
                }
            }

            // Ufs.
            ckbTodasUfs.Checked = relatorio.TodasUfs;
            if (!relatorio.TodasUfs)
            {
                ckbTodasUfs_OnCheckedChanged(null, null);

                foreach (ListItem item in ckblUfs.Items)
                {
                    item.Selected = relatorio.Ufs.Any(x => x.ID.ToString() == item.Value);
                }
            }

            ExibirEstruturaModal();
        }

        private void ExibirEstruturaModal()
        {
            // Exibir Backdrop
            var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));
            if (pnlbackdrop != null)
                pnlbackdrop.Visible = true;

            // Exibir Modal.
            pnlCadastrarRelatorio.Visible = true;
        }

        private void EsconderModal()
        {
            // Esconder Modal.
            pnlCadastrarRelatorio.Visible = false;

            // Esconder Backdrop do Modal.
            var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));
            if (pnlbackdrop != null)
                pnlbackdrop.Visible = false;
        }

        private void LimparModal()
        {
            hdnIdRelatorio.ID = "0";

            txtNome.Text = "";
            txtTag.Text = "";

            // Permissões
            ckbTodosPerfis.Checked = true;
            divPerfis.Visible = false;
            ckblPerfis.Items.Clear();
            ckbTodasUfs.Checked = true;
            divUfs.Visible = false;
            ckblUfs.Items.Clear();
        }

        protected void ckbTodosPerfis_OnCheckedChanged(object sender, EventArgs e)
        {
            if (!ckbTodosPerfis.Checked)
            {
                var perfisListItens = new ManterPerfil().ObterTodosPerfis().Select(x => new ListItem(x.Nome, x.ID.ToString())).ToArray();

                ckblPerfis.Items.AddRange(perfisListItens);

                divPerfis.Visible = true;
            }
            else
            {
                divPerfis.Visible = false;
                ckblPerfis.Items.Clear();
            }
        }

        protected void ckbTodasUfs_OnCheckedChanged(object sender, EventArgs e)
        {
            if (!ckbTodasUfs.Checked)
            {
                var ufsListItens = new ManterUf().ObterTodosUf().Select(x => new ListItem(x.Nome, x.ID.ToString())).ToArray();

                ckblUfs.Items.AddRange(ufsListItens);

                divUfs.Visible = true;
            }
            else
            {
                divUfs.Visible = false;
                ckblUfs.Items.Clear();
            }
        }

        protected void btnFecharModal_OnServerClick(object sender, EventArgs e)
        {
            LimparModal();

            EsconderModal();
        }

        protected void btnSalvarModal_OnServerClick(object sender, EventArgs e)
        {
            try
            {
                var relatorio = ObterObjetoRelatorio();

                new ManterRelatorioPaginaInicial().Salvar(relatorio);

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, string.Format("Relatório \"{0}\" da página inicial salvo com sucesso", relatorio.Nome));

                btnFecharModal_OnServerClick(null, null);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, string.Format(ex.Message));
            }
        }

        private void ValidarRelatorio()
        {
            // Nome
            if (string.IsNullOrEmpty(txtNome.Text))
                throw new AcademicoException("Campo \"Título\" é obrigatório.");

            if (txtNome.Text.Length > 255)
                throw new AcademicoException("Campo \"Nome\" deve ter no máximo 255 caracteres.");

            // Perfis
            if (!ckbTodosPerfis.Checked & !ckblPerfis.Items.Cast<ListItem>().Any(n => n.Selected))
                throw new AcademicoException("Pelo menos um perfil precisa ser selecionado.");

            // UFs
            if (!ckbTodasUfs.Checked & !ckblUfs.Items.Cast<ListItem>().Any(n => n.Selected))
                throw new AcademicoException("Pelo menos uma UF precisa ser selecionada.");
        }

        private Dominio.Classes.RelatorioPaginaInicial ObterObjetoRelatorio()
        {
            ValidarRelatorio();

            var relatorio = new ManterRelatorioPaginaInicial().ObterPorID(int.Parse(hdnIdRelatorio.Value)) ??
                            new Dominio.Classes.RelatorioPaginaInicial();

            relatorio.Nome = txtNome.Text;

            // Perfis
            relatorio.RemoverTodosPerfis();

            if (ckbTodosPerfis.Checked)
            {
                relatorio.TodosPerfis = true;
            }
            else
            {
                relatorio.TodosPerfis = false;
                ObterPerfisSelecionados(relatorio);
            }

            // Ufs
            relatorio.RemoverTodasUfs();

            if (ckbTodasUfs.Checked)
            {
                relatorio.TodasUfs = true;
            }
            else
            {
                relatorio.TodasUfs = false;
                ObterUfsSelecionadas(relatorio);
            }

            return relatorio;
        }

        private void ObterPerfisSelecionados(Dominio.Classes.RelatorioPaginaInicial relatorioPaginaInicial)
        {
            var manter = new ManterPerfil();

            foreach (ListItem item in ckblPerfis.Items.Cast<ListItem>().Where(x => x.Selected))
                relatorioPaginaInicial.AdicionarPerfil(manter.ObterPerfilPorID(int.Parse(item.Value)));
        }

        private void ObterUfsSelecionadas(Dominio.Classes.RelatorioPaginaInicial relatorioPaginaInicial)
        {
            var manter = new ManterUf();

            foreach (ListItem item in ckblUfs.Items.Cast<ListItem>().Where(x => x.Selected))
                relatorioPaginaInicial.AdicionarUf(manter.ObterUfPorID(int.Parse(item.Value)));
        }
    }
}
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms
{
    public partial class Paginas : Page
    {
        // Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            // Permite somente usuário Super Administrador.
            if (!new ManterUsuario().ObterUsuarioLogado().SuperAdministrador)
                Response.Redirect("/Default.aspx?logout=0");

            if (Page.IsPostBack) return;

            AtualizarGridView();

            LimparModal();
        }

        protected void btnExcluir_OnClick(object sender, EventArgs e)
        {

            var paginaId = int.Parse(hdnExcluir.Value);

            ExcluirPagina(paginaId);
        }

        protected void btnCriarFilho_OnServerClick(object sender, EventArgs e)
        {
            // Atribui a página pai.
            hdnIdPaginaPai.Value = hdnCriarFilho.Value;
           
            // Busca a profundidade.
            var profundidade = int.Parse(hdnProfundidade.Value) + 1; // Adiciona +1 na profundidade pois está cadastrando filho.

            ExibirModalCadastro((enumTipoPagina)profundidade);
        }

        protected void btnEditar_OnServerClick(object sender, EventArgs e)
        {
            // Obtém o ID da Página editada.
            var paginaId = int.Parse(hdnEdicaoId.Value);

            ExibirModalEdicao(paginaId);
        }

        protected void btnSalvarModal_OnServerClick(object sender, EventArgs e)
        {
            try
            {
                var pagina = ObterObjetoPagina();

                var isCadastro = pagina.ID == 0;

                var paginaPai = ObterPaginaPai();

                new ManterPagina().SalvarPagina(pagina, paginaPai);

               WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, string.Format("Página {0} com sucesso", isCadastro ? "cadastrada" : "editada"));

                Response.Redirect(Request.Url.AbsolutePath + "?id=" + pagina.ID);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, string.Format(ex.Message));
            }
        }

        protected void btnFecharModal_OnServerClick(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.AbsolutePath);
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

        protected void ckbTodosNiveisOcupacionais_OnCheckedChanged(object sender, EventArgs e)
        {

            if (!ckbTodosNiveisOcupacionais.Checked)
            {
                var ufsListItens = new ManterNivelOcupacional().ObterTodosNivelOcupacional().Select(x => new ListItem(x.Nome, x.ID.ToString())).ToArray();

                cbklNiveisOcupacionais.Items.AddRange(ufsListItens);

                divNiveisOcupacionais.Visible = true;
            }
            else
            {
                divNiveisOcupacionais.Visible = false;
                cbklNiveisOcupacionais.Items.Clear();
            }
        }

        protected void btnCadastrarMenu_OnServerClick(object sender, EventArgs e)
        {
            ExibirModalCadastro(enumTipoPagina.Menu);
        }

        protected void ckbPaginaInicial_OnCheckedChanged(object sender, EventArgs e)
        {
            if (ckbPaginaInicial.Checked)
            {
                ckbTodosPerfis.Enabled =
                ckbTodasUfs.Enabled =
                ckbTodosNiveisOcupacionais.Enabled =
                divTitulo.Visible = false;
            }
            else
            {
                ckbTodosPerfis.Enabled =
                ckbTodasUfs.Enabled =
                ckbTodosNiveisOcupacionais.Enabled =

                divTitulo.Visible = true;
            }

            ckbTodosPerfis_OnCheckedChanged(null, null);
            ckbTodasUfs_OnCheckedChanged(null, null);
            ckbTodosNiveisOcupacionais_OnCheckedChanged(null, null);
        }

        protected void ddlFiltroProfundidade_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string nivel;

            switch ((enumTipoPagina)int.Parse(ddlFiltroProfundidade.SelectedValue))
            {
                case enumTipoPagina.Container:
                    nivel = "";
                    break;
                case enumTipoPagina.Menu:
                    nivel = "menu";
                    break;
                case enumTipoPagina.Agrupador:
                    nivel = "agrupador";
                    break;
                case enumTipoPagina.Pagina:
                    nivel = "pagina";
                    break;
                case enumTipoPagina.CadastroEdicao:
                    nivel = "cadastro";
                    break;
                case enumTipoPagina.Subcadastro:
                    nivel = "subcadastro";
                    break;
                default:
                    nivel = "";
                    break;
            }

            if (!string.IsNullOrEmpty(nivel))
                Response.Redirect(Request.Url.AbsolutePath + "?nivel=" + nivel, true);
            else
                Response.Redirect(Request.Url.AbsolutePath, true);
        }

        // Métodos
        private void AtualizarGridView()
        {
            var manterPagina = new ManterPagina();

            var paginas = manterPagina.ObterTodasPaginas(true, false, false);

            if (Request["nivel"] != null)
            {
                int profundidade;

                switch (Request["nivel"])
                {
                    case "menu":
                        profundidade = 1;
                        break;
                    case "agrupador":
                        profundidade = 2;
                        break;
                    case "pagina":
                        profundidade = 3;
                        break;
                    case "cadastro":
                        profundidade = 4;
                        break;
                    case "subcadastro":
                        profundidade = 5;
                        break;
                    default:
                        profundidade = 0;
                        break;
                }

                ddlFiltroProfundidade.SelectedValue = profundidade.ToString();

                if (profundidade != 0)
                    paginas = paginas.Where(p => p.Profundidade <= profundidade);
            }

            gdvPaginas.DataSource = paginas;
            gdvPaginas.DataBind();
        }

        protected void gdvPaginas_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Inserir Header.
                var HeaderGrid = (GridView)sender;
                var headerGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                var headerCell = new TableCell();
                headerCell.Text = "Páginas";
                headerCell.CssClass = "paginas-table-header";
                headerGridRow.Cells.Add(headerCell);

                headerCell = new TableCell();
                headerCell.Text = "Posição";
                headerCell.CssClass = "paginas-table-header";
                headerCell.ColumnSpan = 2;
                headerGridRow.Cells.Add(headerCell);

                headerCell = new TableCell();
                headerCell.Text = "Permissões";
                headerCell.ColumnSpan = 3;
                headerCell.CssClass = "paginas-table-header";
                headerGridRow.Cells.Add(headerCell);

                headerCell = new TableCell();
                headerCell.Text = "Operações";
                headerCell.ColumnSpan = 3;
                headerCell.CssClass = "paginas-table-header";
                headerGridRow.Cells.Add(headerCell);

                HeaderGrid.Controls[0].Controls.AddAt(0, headerGridRow);

                headerGridRow.CssClass = "nodrag nodrop";
            }
        }

        private void ExcluirPagina(int paginaId)
        {
            var manterPagina = new ManterPagina();

            try
            {
                manterPagina.ExcluirPagina(paginaId);

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Página excluída com sucesso");

                AtualizarGridView();
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, string.Format(ex.Message));
            }
        }

        private enumTipoPagina ObterTipoCadastro()
        {
            return (enumTipoPagina)int.Parse(hdnTipoPagina.Value);
        }

        private Dominio.Classes.Pagina ObterPaginaPai()
        {
            var manterPagina = new ManterPagina();

            var paginaPaiId = int.Parse(hdnIdPaginaPai.Value);

            return paginaPaiId == 0 ? null : manterPagina.ObterPaginaPorID(paginaPaiId, true, true);
        }

        private void ExibirModalEdicao(int paginaId)
        {
            var manterPagina = new ManterPagina();

            var pagina = manterPagina.ObterPaginaPorID(paginaId, true);

            // Setar título do Modal.
            modalCadastrarPaginaTitulo.InnerText = "Editar " + pagina.Nome + " (" + pagina._ObterTipoPagina.ToLower() + ")";

            var tipoPagina = pagina.ObterTipoPagina();

            hdnIdPagina.Value = pagina.ID.ToString();
            txtNome.Text = pagina.Nome;
            txtDescricao.Text = pagina.Descricao;

            hdnTipoPagina.Value = pagina.Profundidade.ToString();

            // Perfis.
            ckbTodosPerfis.Checked = pagina.TodosPerfis;

            if (!pagina.TodosPerfis)
            {
                ckbTodosPerfis_OnCheckedChanged(null, null);

                foreach (ListItem item in ckblPerfis.Items)
                {
                    item.Selected = pagina.Perfis.Any(x => x.ID.ToString() == item.Value);
                }
            }

            if (pagina.ConsiderarNacionalizacaoUf.HasValue)
            {
                ckbConsiderarNacionalizacaoUf.Checked = pagina.ConsiderarNacionalizacaoUf.Value;
            }

            ckbTodasUfs.Checked = pagina.TodasUfs;

            if (!pagina.TodasUfs)
            {
                ckbTodasUfs_OnCheckedChanged(null, null);

                foreach (ListItem item in ckblUfs.Items)
                {
                    item.Selected = pagina.Ufs.Any(x => x.ID.ToString() == item.Value);
                }
            }

            // Níveis ocupacionais.
            ckbTodosNiveisOcupacionais.Checked = pagina.TodosNiveisOcupacionais;

            if (!pagina.TodosNiveisOcupacionais)
            {
                ckbTodosNiveisOcupacionais_OnCheckedChanged(null, null);

                foreach (ListItem item in cbklNiveisOcupacionais.Items)
                {
                    item.Selected = pagina.NiveisOcupacionais.Any(x => x.ID.ToString() == item.Value);
                }
            }

            if (tipoPagina != enumTipoPagina.Agrupador)
            {
                txtCaminhoRelativo.Text = pagina.CaminhoRelativo;
                txtTitulo.Text = pagina.Titulo;
            }

            switch (tipoPagina)
            {
                case enumTipoPagina.Container:
                    throw new AcademicoException("Página inválida");
                case enumTipoPagina.Menu:
                    divEstilo.Visible = true;
                    divPaginaInicial.Visible = true;

                    // Atribui valores do campo descricao
                    txtDescricaoAdministrador.Text = pagina.DescricaoAdministrador;
                    txtDescricaoGestor.Text = pagina.DescricaoGestor;

                    // Habilita campos de descricao no modal de edicao
                    divDescricaoAdministrador.Visible = true;
                    divDescricaoGestor.Visible = true;

                    divIconeMenu.Visible = true;

                    if (pagina.IsPaginaInicial())
                        divTitulo.Visible = false;

                    ddlEstilo.SelectedValue = pagina.Estilo;
                    ckbConsiderarNacionalizacaoUf.Enabled =
                        !(pagina.PaginaInicial.HasValue && pagina.PaginaInicial.Value);
                    ckbPaginaInicial.Checked = pagina.PaginaInicial.HasValue && pagina.PaginaInicial.Value;
                    ckbPaginaInicial.Enabled = !(pagina.PaginaInicial.HasValue && pagina.PaginaInicial.Value);
                    hddIcone.Value = pagina.IconeMenu;

                    break;
                case enumTipoPagina.Agrupador:
                    divCaminhoRelativo.Visible = false;
                    divTitulo.Visible = false;
                    divIconePaginas.Visible = true;
                    txtIconePaginas.Text = pagina.IconePaginas;
                    break;
                case enumTipoPagina.Pagina:
                    labelIconeMenu.Text = "Ícone do Menu";
                    hddIcone.Value = pagina.IconeMenu;
                    divIconeMenu.Visible = true;
                    break;
                case enumTipoPagina.CadastroEdicao:
                    divNome.Visible = false;
                    divIconeMenu.Visible = true;
                    hddIcone.Value = pagina.IconeMenu;

                    divChaveVerificadora.Visible = true;

                    labelTitulo.Text = "Nome do objeto*";
                    labelTitulo.Attributes.Add("data-help", Resources.Resource.paginaNomeDoObjeto);

                    txtChaveVerificadora.Text = pagina.ChaveVerificadora;

                    break;
                case enumTipoPagina.Subcadastro:
                    divNome.Visible = false;
                    labelTitulo.Text = "Nome do objeto*";
                    labelTitulo.Attributes.Add("data-help", Resources.Resource.paginaNomeDoObjeto);

                    divChaveVerificadora.Visible = true;

                    txtChaveVerificadora.Text = pagina.ChaveVerificadora;

                    break;
            }

            ExibirEstruturaModal();
        }

        private void ExibirModalCadastro(enumTipoPagina tipoPagina)
        {
            // Bloquear permissões selecionadas no pai.

            // Perfis.
            var pai = ObterPaginaPai();

            ckbTodosPerfis.Checked = pai.TodosPerfis;
            if (!pai.TodosPerfis)
            {
                ckbTodosPerfis_OnCheckedChanged(null, null);

                foreach (ListItem item in ckblPerfis.Items)
                {
                    if (pai.Perfis.Any(x => x.ID.ToString() == item.Value))
                    {
                        item.Selected = true;
                        // #1758 - Alterado pelo cliente, que deseja que as sub-permissões sejam editáveis.
                        //item.Enabled = false;
                    }
                }
            }

            // Ufs.
            ckbTodasUfs.Checked = pai.TodasUfs;
            if (!pai.TodasUfs)
            {
                ckbTodasUfs_OnCheckedChanged(null, null);

                foreach (ListItem item in ckblUfs.Items)
                {
                    if (pai.Ufs.Any(x => x.ID.ToString() == item.Value))
                    {
                        item.Selected = true;
                        // #1758 - Alterado pelo cliente, que deseja que as sub-permissões sejam editáveis.
                        //item.Enabled = false;
                    }
                }
            }

            // Níveis ocupacionais.
            ckbTodosNiveisOcupacionais.Checked = pai.TodosNiveisOcupacionais;
            if (!pai.TodosNiveisOcupacionais)
            {
                ckbTodosNiveisOcupacionais_OnCheckedChanged(null, null);

                foreach (ListItem item in cbklNiveisOcupacionais.Items)
                {
                    if (pai.NiveisOcupacionais.Any(x => x.ID.ToString() == item.Value))
                    {
                        item.Selected = true;
                        // #1758 - Alterado pelo cliente, que deseja que as sub-permissões sejam editáveis.
                        //item.Enabled = false;
                    }
                }
            }


            string titulo;

            hdnTipoPagina.Value = ((int)tipoPagina).ToString();

            switch (tipoPagina)
            {
                case enumTipoPagina.Container:
                    throw new AcademicoException("Página inválida");
                case enumTipoPagina.Menu:
                    titulo = "menu";

                    divEstilo.Visible = true;
                    divPaginaInicial.Visible = true;
                    divIconeMenu.Visible = true;

                    break;
                case enumTipoPagina.Agrupador:
                    titulo = "agrupador";

                    divCaminhoRelativo.Visible = false;
                    divIconePaginas.Visible = true;
                    divTitulo.Visible = false;
                    break;
                case enumTipoPagina.Pagina:
                    titulo = "página";

                    break;
                case enumTipoPagina.CadastroEdicao:
                    titulo = "cadastro/edição";

                    divNome.Visible = false;

                    divIconeMenu.Visible = true;

                    divChaveVerificadora.Visible = true;

                    labelTitulo.Text = "Nome do objeto*";

                    labelTitulo.Attributes.Add("data-help", Resources.Resource.paginaNomeDoObjeto);

                    // Adiciona o ícone padrão "plus" e a chave verificadora padrão Id.
                    hddIcone.Value = "plus";
                    txtChaveVerificadora.Text = "Id";

                    break;
                case enumTipoPagina.Subcadastro:
                    titulo = "subcadastro";

                    divNome.Visible = false;

                    divChaveVerificadora.Visible = true;

                    labelTitulo.Text = "Nome do objeto*";

                    labelTitulo.Attributes.Add("data-help", Resources.Resource.paginaNomeDoObjeto);

                    // Adiciona a chave verificadora padrão Id.
                    txtChaveVerificadora.Text = "Id";

                    break;
                default:
                    titulo = "";
                    break;
                    //throw new ArgumentOutOfRangeException(nameof(tipoPagina), tipoPagina, null);
            }

            // Setar título do Modal.
            modalCadastrarPaginaTitulo.InnerText = "Cadastrar " + titulo;

            ExibirEstruturaModal();
        }

        private void ExibirEstruturaModal()
        {
            // Exibir Backdrop
            var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));
            if (pnlbackdrop != null)
                pnlbackdrop.Visible = true;

            // Exibir Modal.
            pnlCadastrarPagina.Visible = true;
        }

        private void ValidarPagina()
        {
            // Validações gerais para todos os tipos de páginas
            var tipoPagina = ObterTipoCadastro();

            // Nome
            if (tipoPagina != enumTipoPagina.CadastroEdicao && tipoPagina != enumTipoPagina.Subcadastro)
            {
                if (string.IsNullOrEmpty(txtNome.Text))
                    throw new AcademicoException("Campo \"Nome\" é obrigatório.");

                if (txtNome.Text.Length > 50)
                    throw new AcademicoException("Campo \"Nome\" deve ter no máximo 50 caracteres.");
            }

            // Perfis
            if (!ckbTodosPerfis.Checked & !ckblPerfis.Items.Cast<ListItem>().Any(n => n.Selected))
                throw new AcademicoException("Pelo menos um perfil precisa ser selecionado.");

            // UFs
            if (!ckbTodasUfs.Checked & !ckblUfs.Items.Cast<ListItem>().Any(n => n.Selected))
                throw new AcademicoException("Pelo menos uma UF precisa ser selecionada.");

            // Níveis Ocupacionais
            if (!ckbTodosNiveisOcupacionais.Checked & !cbklNiveisOcupacionais.Items.Cast<ListItem>().Any(n => n.Selected))
                throw new AcademicoException("Pelo menos um Nível Ocupacional precisa ser selecionado.");

            if (tipoPagina != enumTipoPagina.Agrupador)
            {
                if (txtTitulo.Visible)
                {
                    // Título
                    if (string.IsNullOrEmpty(txtTitulo.Text))
                        throw new AcademicoException("Campo \"Título\" é obrigatório.");

                    if (txtTitulo.Text.Length > 200)
                        throw new AcademicoException("Campo \"Título\" deve ter no máximo 200 caracteres.");
                }
                // Caminho Relativo
                if (string.IsNullOrEmpty(txtCaminhoRelativo.Text))
                    throw new AcademicoException("Campo \"Caminho relativo\" é obrigatório.");

                if (txtCaminhoRelativo.Text.Length > 255)
                    throw new AcademicoException("Campo \"Caminho relativo\" deve ter no máximo 255 caracteres.");
            }

            switch (tipoPagina)
            {
                case enumTipoPagina.Container:
                    // Não permite cadastro do container master.
                    throw new AcademicoException("Cadastro inválido.");

                case enumTipoPagina.Menu:
                    if (int.Parse(hdnIdPagina.Value) == 0)
                    {
                        // Estilo.
                        if (ddlEstilo.SelectedIndex == 0)
                            throw new AcademicoException("O campo \"Estilo\" é obrigatório.");
                    }
                    break;
                case enumTipoPagina.Agrupador:
                    // Ícone Padrão
                    if (string.IsNullOrEmpty(txtIconePaginas.Text))
                        throw new AcademicoException("Campo \"Ícone padrão\" é obrigatório.");

                    break;
                case enumTipoPagina.Pagina:

                    break;
                case enumTipoPagina.CadastroEdicao:
                    if (string.IsNullOrEmpty(txtChaveVerificadora.Text))
                        throw new AcademicoException("O campo \"Chave Verificadora\" é obrigatório.");

                    break;
                case enumTipoPagina.Subcadastro:

                    if (string.IsNullOrEmpty(txtChaveVerificadora.Text))
                        throw new AcademicoException("O campo \"Chave Verificadora\" é obrigatório.");

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void LimparModal()
        {
            // Visualizações
            divEstilo.Visible = false;
            divPaginaInicial.Visible = false;
            divIconeMenu.Visible = false;
            divIconePaginas.Visible = false;
            divCaminhoRelativo.Visible = true;
            divChaveVerificadora.Visible = false;
            divTitulo.Visible = true;

            // Ocultar descricao de administrador e gestor para um nome menu - funcionalidade disponivel apenas para edicao do tipo menu
            divDescricaoAdministrador.Visible = false;
            divDescricaoGestor.Visible = false;

            // Valores
            hdnIdPaginaPai.Value = new ManterPagina().ObterRoot().ID.ToString();
            hdnIdPagina.Value = "0";
            ddlEstilo.SelectedIndex = 0;
            txtNome.Text = "";
            txtDescricao.Text = "";
            txtCaminhoRelativo.Text = "";
            labelIconeMenu.Text = "Ícone do Menu*";
            hddIcone.Value = "";
            txtIconePaginas.Text = "";
            ckbPaginaInicial.Checked = false;
            ckbPaginaInicial.Enabled = true;
            labelTitulo.Text = "Título*";
            labelTitulo.Attributes.Remove("data-help");
            labelTitulo.Text = "Título*";
            txtTitulo.Text = "";
            divNome.Visible = true;

            // Permissões
            ckbTodosPerfis.Checked = true;
            ckbTodosPerfis.Enabled = true;
            divPerfis.Visible = false;
            ckblPerfis.Items.Clear();

            ckbConsiderarNacionalizacaoUf.Checked = false;
            ckbConsiderarNacionalizacaoUf.Enabled = true;
            ckbTodasUfs.Checked = true;
            ckbTodasUfs.Enabled = true;
            divUfs.Visible = false;
            ckblUfs.Items.Clear();

            ckbTodosNiveisOcupacionais.Checked = true;
            ckbTodosNiveisOcupacionais.Enabled = true;
            divNiveisOcupacionais.Visible = false;
            cbklNiveisOcupacionais.Items.Clear();
        }

        private void ObterPerfisSelecionados(Dominio.Classes.Pagina pagina)
        {
            var manter = new ManterPerfil();

            foreach (ListItem item in ckblPerfis.Items.Cast<ListItem>().Where(x => x.Selected))
                pagina.AdicionarPerfil(manter.ObterPerfilPorID(int.Parse(item.Value)));
        }

        private void ObterUfsSelecionadas(Dominio.Classes.Pagina pagina)
        {
            var manter = new ManterUf();

            foreach (ListItem item in ckblUfs.Items.Cast<ListItem>().Where(x => x.Selected))
                pagina.AdicionarUf(manter.ObterUfPorID(int.Parse(item.Value)));
        }

        private void ObterNiveisOcupacionaisSelecionados(Dominio.Classes.Pagina pagina)
        {
            var manter = new ManterNivelOcupacional();

            foreach (ListItem item in cbklNiveisOcupacionais.Items.Cast<ListItem>().Where(x => x.Selected))
                pagina.AdicionarNivelOcupacional(manter.ObterNivelOcupacionalPorID(int.Parse(item.Value)));
        }

        private Dominio.Classes.Pagina ObterObjetoPagina()
        {
            ValidarPagina();

            var manterPagina = new ManterPagina();

            var paginaId = int.Parse(hdnIdPagina.Value);

            var pagina = paginaId == 0 ? new Dominio.Classes.Pagina() : manterPagina.ObterPaginaPorID(paginaId);

            var tipoPagina = (enumTipoPagina)int.Parse(hdnTipoPagina.Value);

            // Nome
            pagina.Nome = (tipoPagina != enumTipoPagina.CadastroEdicao && tipoPagina != enumTipoPagina.Subcadastro) ? txtNome.Text.Trim() : null;

            // Descrição
            pagina.Descricao = string.IsNullOrWhiteSpace(txtDescricao.Text.Trim()) ? null : txtDescricao.Text.Trim();

            // Limpar permissões.
            pagina.RemoverTodosPerfis();
            pagina.RemoverTodasUfs();
            pagina.RemoverTodosNiveisOcupacionais();

            pagina.ConsiderarNacionalizacaoUf = ckbConsiderarNacionalizacaoUf.Checked;

            // Perfis
            if (ckbTodosPerfis.Checked)
            {
                pagina.TodosPerfis = true;
            }
            else
            {
                pagina.TodosPerfis = false;
                ObterPerfisSelecionados(pagina);
            }

            // Ufs
            if (ckbTodasUfs.Checked)
            {
                pagina.TodasUfs = true;
            }
            else
            {
                pagina.TodasUfs = false;
                ObterUfsSelecionadas(pagina);
            }

            // Níveis ocupacionais
            if (ckbTodosNiveisOcupacionais.Checked)
            {
                pagina.TodosNiveisOcupacionais = true;
            }
            else
            {
                pagina.TodosNiveisOcupacionais = false;
                ObterNiveisOcupacionaisSelecionados(pagina);
            }


            // Caminho Relativo.
            if (tipoPagina != enumTipoPagina.Agrupador)
            {
                pagina.CaminhoRelativo = FormatarCaminhoRelativo();

                if (!ckbPaginaInicial.Checked)
                    pagina.Titulo = txtTitulo.Text.Trim();
            }

            switch (tipoPagina)
            {
                case enumTipoPagina.Container:
                    throw new AcademicoException("Página inválida");

                case enumTipoPagina.Menu:
                    // Estilo.
                    pagina.Estilo = ddlEstilo.SelectedValue;

                    // Ícone.
                    pagina.IconeMenu = hddIcone.Value;

                    // Página inicial.
                    if (ckbPaginaInicial.Checked)
                        pagina.PaginaInicial = true;

                    // Descrição Administrador
                    pagina.DescricaoAdministrador = string.IsNullOrWhiteSpace(txtDescricaoAdministrador.Text.Trim()) ? null : txtDescricaoAdministrador.Text.Trim();

                    // Descrição Gestor
                    pagina.DescricaoGestor = string.IsNullOrWhiteSpace(txtDescricaoGestor.Text.Trim()) ? null : txtDescricaoGestor.Text.Trim();

                    break;
                case enumTipoPagina.Agrupador:
                    // Ícone padrão.
                    pagina.IconePaginas = string.IsNullOrEmpty(txtIconePaginas.Text) ? null : txtIconePaginas.Text;

                    break;
                case enumTipoPagina.Pagina:
                    pagina.IconeMenu = string.IsNullOrEmpty(hddIcone.Value) ? null : hddIcone.Value;
                    break;
                case enumTipoPagina.CadastroEdicao:
                    // Ícone.
                    pagina.IconeMenu = hddIcone.Value;
                    pagina.ChaveVerificadora = txtChaveVerificadora.Text;

                    break;
                case enumTipoPagina.Subcadastro:
                    pagina.ChaveVerificadora = txtChaveVerificadora.Text;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            return pagina;
        }

        private string FormatarCaminhoRelativo()
        {
            var t = txtCaminhoRelativo.Text.Trim();
            return string.Format("{0}{1}", !t.StartsWith("/") ? "/" : "", t);
        }

        protected void gdvPaginas_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            var pagina = (Dominio.Classes.Pagina)e.Row.DataItem;

            if (pagina != null)
            {
                e.Row.ClientIDMode = ClientIDMode.Static;
                e.Row.ID = "pagina-" + pagina.ID;
                e.Row.Attributes["data-profundidade"] = pagina.Profundidade.ToString();
                e.Row.Attributes["data-id"] = pagina.ID.ToString();

                if (pagina.Profundidade > 3)
                    e.Row.Attributes["data-subpagina"] = "true";

                // Desabilita o Drag and Drop para itens que não sejam Página.
                if (pagina.Profundidade != 3)
                    e.Row.CssClass = "nodrag nodrop";
            }
        }

        protected void hdnMoverPagina_OnValueChanged(object sender, EventArgs e)
        {
            try
            {
                // Atenção, o ministério da programação adverte: o código abaixo faz sangrar.
                // Obter a página e o novo índice para qual será movida dentro do seu agrupador.
                // Os valores estarão no campo hdnMoverPagina e deverão ser separados por vírgula.
                // O primeiro é o ID da página sendo movida e o segundo é o índice dentro do seu
                // agrupador.

                var paginaId = int.Parse(hdnMoverPagina.Value.Split(',')[0]);
                var novoIndice = int.Parse(hdnMoverPagina.Value.Split(',')[1]);

                // Mover página!
                new ManterPagina().MoverPagina(paginaId, novoIndice);

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Página movida com sucesso.");
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
            catch (Exception)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao mover página. Tente novamente.");
            }

            // Forçar recarregamento total da página porque ela é toda dinâmica e pira se der postback.
            //Response.Redirect(Request.Url.AbsolutePath);

            // Recarregar página pro DOM não pirar.
            AtualizarGridView();
        }
    }
}
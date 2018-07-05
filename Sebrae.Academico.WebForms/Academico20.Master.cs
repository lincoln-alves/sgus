using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Rdl;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BM.Classes;

namespace Sebrae.Academico.WebForms
{
    public partial class Academico20 : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Forçar a criação do evento de javascript __doPostBack para uso no Autocomplete.
            Page.ClientScript.GetPostBackEventReference(this, string.Empty);

            // Exibir mensagem de alerta.
            RecuperarMensageSessao();

            // Verificar simulação de perfil.
            VerificarSimulacaoPerfil();

            var manterPagina = new ManterPagina();

            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            // Salva o estado do usuário logado na sessão para usar nas HelperTags.
            Session["UsuarioLogadoIsAdmin"] = usuarioLogado.IsAdministrador();

            // Obter objeto da página atual.
            var paginaAtual = ObterPaginaAtual(manterPagina);

            // Salva a página atual para ser usada pelas HelperTags.
            Session["paginaAtual"] = paginaAtual;

            // Setar nome da página.
            SetarTituloPagina(paginaAtual, usuarioLogado);
            //var bmLogAcessoPagina = new BMLogAcessoPagina();
            //bmLogAcessoPagina.PreSave(usuarioLogado, paginaAtual); 
            if(!IsPostBack)
            {
                var queryString = HttpContext.Current.Request.QueryString.ToString();
                var ip = HttpContext.Current.Request.UserHostAddress;

                var log = new LogAcessoPagina
                {
                    IDUsuario = usuarioLogado,
                    Pagina = paginaAtual,
                    QueryString = queryString,
                    Acao = enumAcaoNaPagina.Visualizacao,
                    DTSolicitacao = DateTime.Now,
                    IP = ip
                };

                new BMLogAcessoPagina().Salvar(log);
            }

            if (VerificarPermissaoPagina(usuarioLogado, manterPagina, paginaAtual))
            {
                // Setar estilo atual.
                EstilizarPagina(usuarioLogado, manterPagina, paginaAtual);

                // Preencher sidebar.
                PreencherSidebar(manterPagina, usuarioLogado, paginaAtual);
            }

            // Preencher Menu
            PreencherMenu(manterPagina, usuarioLogado);

            // Por causa do GoogleCharts não ser responsivo, indica ao javscript para recarregar
            // a página no collapse do menu lateral, para recarregar os componentes.
            if (paginaAtual != null && paginaAtual.CaminhoRelativo == "/Dashboard.aspx")
            {
                menuCollapse.Attributes.Add("data-postback", "1");
            }

            (ManterLogSincronia.Instance).IniciarThread();
        }

        private Dominio.Classes.Pagina ObterPaginaAtual(ManterPagina manterPagina = null)
        {
            manterPagina = manterPagina ?? new ManterPagina();

            return manterPagina.ObterPaginaPorCaminhoRelativo(Request.Url.PathAndQuery) ??
                   manterPagina.ObterPaginaPorCaminhoRelativo(Request.Url.AbsolutePath);
        }

        private void SetarTituloPagina(Dominio.Classes.Pagina paginaAtual, Usuario usuarioLogado)
        {
            if (Request.Url.AbsolutePath.ToLower() == "/paginas.aspx" || paginaAtual == null)
            {
                linhaTituloPagina.Visible =
                descricaoPagina.Visible =
                tituloPagina.Visible = false;

                return;
            }

            if (!string.IsNullOrEmpty(Page.Title))
            {
                tituloPagina.InnerText = Page.Title;
                return;
            }

            linhaTituloPagina.Visible =
            descricaoPagina.Visible =
            tituloPagina.Visible = true;

            if (paginaAtual.Profundidade == (int)enumTipoPagina.CadastroEdicao ||
                paginaAtual.Profundidade == (int)enumTipoPagina.Subcadastro)
            {
                // Verifica se a URL contém o parâmetro contido na Chave Verificadora da página.

                if (Request.QueryString[paginaAtual.ChaveVerificadora] == null)
                {
                    tituloPagina.InnerText = "Cadastro" + (paginaAtual.Titulo != null ? " de " + paginaAtual.Titulo : "");
                }
                else
                {
                    tituloPagina.InnerText = "Gerenciamento " + (paginaAtual.Titulo != null ? " de " + paginaAtual.Titulo : "");
                }
            }
            else
            {
                tituloPagina.InnerText = paginaAtual.Titulo;
            }

            if (string.IsNullOrWhiteSpace(paginaAtual.Descricao))
            {
                descricaoPagina.Visible = false;
            }
            else
            {
                descricaoPagina.Visible = true;
                descricaoPagina.InnerHtml = paginaAtual.Descricao;
            }

            // Adiciona Conteudo de Administrador
            divDescricaoAdministrador.Visible = false;
            if (!string.IsNullOrWhiteSpace(paginaAtual.DescricaoAdministrador))
            {
                if (usuarioLogado.IsAdministrador())
                {
                    divDescricaoAdministrador.Visible = true;
                    divDescricaoAdministrador.InnerHtml = paginaAtual.DescricaoAdministrador;
                }
            }

            // Adicionar conteudo de Gestor
            divDescricaoGestor.Visible = false;
            if (!string.IsNullOrWhiteSpace(paginaAtual.DescricaoGestor))
            {
                if (usuarioLogado.IsGestor())
                {
                    divDescricaoGestor.Visible = true;
                    divDescricaoGestor.InnerHtml = paginaAtual.DescricaoGestor;
                }
            }
        }

        private void PreencherSidebar(ManterPagina manterPagina, Usuario usuarioLogado, Dominio.Classes.Pagina paginaAtual)
        {
            if (paginaAtual.CaminhoRelativo.ToLower() == "/paginas.aspx")
            {
                // Esconde o sidebar e coloca a página em largura máxima.
                sidebar.Visible = false;
                contentArea.Attributes.Remove("class");
            }
            else
            {
                // Coloca a página em largura reduzida para caber o sidebar.
                contentArea.Attributes["class"] = "col-sm-9 conteudo_busca content-area";

                // Obtém o menu atual para filtrar somente pelas páginas dentro desse Menu.
                var menuAtual = paginaAtual.Profundidade == (int)enumTipoPagina.Menu ? paginaAtual : manterPagina.ObterMenuPai(paginaAtual);

                // Obtém todas as páginas com filtro.
                var paginas = manterPagina.ObterTodasPaginas(true).Where(p => p.PossuiPermissao(usuarioLogado) && p.IsFilhoDe(menuAtual)).ToList();

                var sidebarRow = new HtmlGenericControl("div");
                sidebarRow.Attributes["class"] = "row";

                var sidebarDiv = new HtmlGenericControl("div");
                sidebarDiv.Attributes["class"] = "col-sm-12";

                // Preencher os agrupadores
                foreach (var agrupador in paginas.Where(p => p.Profundidade == 2 && p.PossuiFilho() && p.PossuiPermissao(usuarioLogado)))
                {
                    var row = new HtmlGenericControl("div");
                    row.Attributes["class"] = "row";

                    var box = new HtmlGenericControl("div");
                    box.Attributes["class"] = "box";

                    var subtitulo = new HtmlGenericControl("div");
                    subtitulo.Attributes["class"] = "subtitulo";

                    var nome = new HtmlGenericControl("h3");
                    nome.InnerHtml = agrupador.Nome;

                    if (agrupador.IsPaiOuIguala(paginaAtual))
                        nome.Attributes.Add("class", "text-bold");

                    var contentBody = new HtmlGenericControl("div");
                    contentBody.Attributes["class"] = "contentBody";

                    var ul = new HtmlGenericControl("ul");
                    ul.Attributes.Add("class", "links");


                    // Preencher as páginas
                    foreach (var pagina in paginas.Where(p => p.Profundidade == 3 && p.IsFilhoDe(agrupador)))
                    {
                        var li = new HtmlGenericControl("li");
                        li.Attributes.Add("class", "menu-item");

                        var linkPagina = new HtmlGenericControl("a");
                        linkPagina.Attributes.Add("class", "menu-nome" + (pagina.IsPaiOuIguala(paginaAtual) ? " text-bold" : ""));
                        linkPagina.Attributes.Add("tittle", "Abrir " + pagina.Nome);
                        linkPagina.Attributes.Add("href", pagina.CaminhoRelativo);
                        linkPagina.InnerHtml = pagina.Nome;

                        var actions = new HtmlGenericControl("span");
                        actions.Attributes.Add("class", "actions visible-md visible-lg");


                        // Preencher cadastro
                        foreach (var cadastro in paginas.Where(p => p.Profundidade == 4 && p.IsFilhoDe(pagina)))
                        {
                            var cadastroIcon = new HtmlGenericControl("a");
                            cadastroIcon.Attributes.Add("href", cadastro.CaminhoRelativo);
                            cadastroIcon.Attributes.Add("class", "glyphicon glyphicon-" + cadastro.IconeMenu
                                + " "
                                + (cadastro.IsPaiOuIguala(paginaAtual) ? "link-no-underline" : " menu-relatar"));

                            actions.Controls.Add(cadastroIcon);
                        }


                        var linkIcon = new HtmlGenericControl("a");
                        linkIcon.Attributes.Add("href", pagina.CaminhoRelativo);
                        // Insere ícone padrão do agrupador ou ícone personalizado caso tenha sido informado.
                        linkIcon.Attributes.Add("class", "glyphicon glyphicon-"
                            + (string.IsNullOrEmpty(pagina.IconeMenu) ? agrupador.IconePaginas : pagina.IconeMenu)
                            + " "
                            + (pagina.ID == paginaAtual.ID ? "link-no-underline" : "menu-relatar"));

                        actions.Controls.Add(linkIcon);

                        li.Controls.Add(linkPagina);
                        li.Controls.Add(actions);

                        ul.Controls.Add(li);
                    }

                    // Insere os controles em hierarquia.
                    contentBody.Controls.Add(ul);

                    subtitulo.Controls.Add(nome);
                    subtitulo.Controls.Add(contentBody);

                    box.Controls.Add(subtitulo);

                    row.Controls.Add(box);

                    sidebarDiv.Controls.Add(box);
                }

                sidebarRow.Controls.Add(sidebarDiv);

                sidebar.Controls.Add(sidebarRow);
            }
        }

        private bool VerificarPermissaoPagina(Usuario usuarioLogado, ManterPagina manterPagina, Dominio.Classes.Pagina paginaAtual)
        {
            if (Request.Url.AbsolutePath.ToLower() == "/paginas.aspx" && usuarioLogado.SuperAdministrador)
            {
                MainBody.Attributes["class"] = "paginas";
                NomeLocalizacao.InnerText = "Páginas";

                sidebar.Visible = false;
                contentArea.Attributes.Remove("class");

                return false;
            }

            // Verificar se a página atual foi cadastrada.
            if (paginaAtual == null)
            {
                RedirecionarPaginaInexistente(usuarioLogado, manterPagina, enumPermissaoPagina.PaginaInexistente);
                return false;
            }

            // Verificar permissão de Perfil.
            if (!paginaAtual.TodosPerfis)
            {
                if (!usuarioLogado.ListaPerfil.Any(lp => paginaAtual.Perfis.Any(p => p.ID == lp.Perfil.ID)))
                {
                    // Usuário não possui perfil para visualizar a página.
                    RedirecionarPaginaInexistente(usuarioLogado, manterPagina, enumPermissaoPagina.PerfilNaoPermitido);
                    return false;
                }
            }

            // Verificar permissão de UF.
            if (!paginaAtual.TodasUfs)
            {
                if (paginaAtual.Ufs.All(u => u.ID != usuarioLogado.UF.ID) && usuarioLogado.UF.IsNacionalizado())
                {
                    // Usuário não está em uma UF com permissão para visualizar a página.
                    RedirecionarPaginaInexistente(usuarioLogado, manterPagina, enumPermissaoPagina.UfNaoAutorizada);
                    return false;
                }
            }

            // Verificar permissão de Nível Ocupacional.
            if (!paginaAtual.TodosNiveisOcupacionais)
            {
                if (paginaAtual.NiveisOcupacionais.All(n => n.ID != usuarioLogado.NivelOcupacional.ID))
                {
                    // Usuário está em um Nível Ocupacional para visualizar a página.
                    RedirecionarPaginaInexistente(usuarioLogado, manterPagina, enumPermissaoPagina.NivelOcupacionalNaoAutorizado);
                    return false;
                }
            }

            return true;
        }

        private void EstilizarPagina(Usuario usuarioLogado, ManterPagina manterPagina, Dominio.Classes.Pagina paginaAtual)
        {
            string estilo;
            string nomePagina;

            if (paginaAtual.Profundidade == 1)
            {
                estilo = paginaAtual.Estilo;
                nomePagina = paginaAtual.Nome;
            }
            else
            {
                var menuAtual = manterPagina.ObterMenuPai(paginaAtual);

                if (menuAtual == null)
                {
                    RedirecionarPaginaInexistente(usuarioLogado, manterPagina, enumPermissaoPagina.PaginaInexistente);
                    return;
                }

                estilo = menuAtual.Estilo;
                nomePagina = menuAtual.Nome;
            }

            MainBody.Attributes["class"] = estilo;
            NomeLocalizacao.InnerText = nomePagina;
        }

        private void PreencherMenu(ManterPagina manterPagina, Usuario usuarioLogado)
        {
            var paginas = manterPagina.ObterTodasPaginas(true).Where(p => p.Profundidade == 1 && p.PossuiPermissao(usuarioLogado));

            foreach (var pagina in paginas)
            {
                var li = new HtmlGenericControl("li");
                li.Attributes.Add("class", "item-menu " + pagina.Estilo);

                var link = new HtmlGenericControl("a");
                link.Attributes.Add("href", pagina.CaminhoRelativo);
                link.InnerHtml = "<span class=\"glyphicon glyphicon-" + pagina.IconeMenu + " menu-icon\"></span><br/>" + pagina.Nome;


                li.Controls.Add(link);

                barraMenu.Controls.Add(li);
            }

            // Inserir o gerenciador de páginas no menu principal.
            if (usuarioLogado.SuperAdministrador)
            {
                var li = new HtmlGenericControl("li");
                li.Attributes.Add("class", "item-menu paginas");

                var link = new HtmlGenericControl("a");
                link.Attributes.Add("href", "/Paginas.aspx");
                link.InnerHtml = "<span class=\"glyphicon glyphicon-list menu-icon\"></span><br/>" + "Páginas";

                li.Controls.Add(link);

                barraMenu.Controls.Add(li);
            }
        }

        /// <summary>
        /// Caso a página sendo acessada não exista, redireciona Super Administrador para a página de gerenciamento de páginas ou usuário comum para a página inicial ou para o Login, caso não haja página inicial.
        /// </summary>
        /// <param name="usuarioLogado"></param>
        /// <param name="manterPagina"></param>
        private void RedirecionarPaginaInexistente(Usuario usuarioLogado, ManterPagina manterPagina, enumPermissaoPagina erroPermissaoEnum)
        {
            string redirect;

            if (usuarioLogado.SuperAdministrador)
            {
                redirect = "/Paginas.aspx";
            }
            else
            {
                var paginaInicial = manterPagina.ObterPaginaInicial();
                redirect = paginaInicial != null ? paginaInicial.CaminhoRelativo : "/Default.aspx?logout=0";
            }

            string mensagem;

            switch (erroPermissaoEnum)
            {
                case enumPermissaoPagina.PaginaInexistente:
                    mensagem = "A página selecionada não existe";
                    break;
                case enumPermissaoPagina.PerfilNaoPermitido:
                    mensagem = "Seu perfil de usuário não está autorizado a visualizar esta página";
                    break;
                case enumPermissaoPagina.UfNaoAutorizada:
                    mensagem = "Sua UF (" + usuarioLogado.UF.Nome + ") não está autorizada a visualizar esta página";
                    break;
                case enumPermissaoPagina.NivelOcupacionalNaoAutorizado:
                    mensagem = "Seu Nível Ocupacional (" + usuarioLogado.NivelOcupacional.Nome + ") não está autorizado a visualizar esta página";
                    break;
                default:
                    mensagem = "Página não autorizada";
                    break;
            }

            mensagem += "<br/>Contate um administrador do sistema";

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, mensagem, redirect);
        }

        public void RecuperarMensageSessao()
        {
            if (HttpContext.Current.Session["tpMensagem"] != null)
            {
                try
                {
                    Tuple<enumTipoMensagem, string> Mensagem = (Tuple<enumTipoMensagem, string>)HttpContext.Current.Session["tpMensagem"];
                    enumTipoMensagem TipoMensagem = Mensagem.Item1;
                    string type = "alert";
                    switch (TipoMensagem)
                    {
                        case enumTipoMensagem.Alerta:
                            type = "alert";
                            break;
                        case enumTipoMensagem.Sucesso:
                            type = "success";
                            break;
                        case enumTipoMensagem.Erro:
                            type = "error";
                            break;
                        case enumTipoMensagem.Atencao:
                            type = "warning";
                            break;
                        case enumTipoMensagem.Informacao:
                            type = " information";
                            break;
                    }

                    string script = @"  
                                        var n = noty({
                                            text: '" + Mensagem.Item2 + @"',
                                            type: '" + type + @"',
                                            layout: 'center'
                                            });
                                      ";

                    Page page = HttpContext.Current.CurrentHandler as Page;

                    if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("mensagem"))
                    {
                        ScriptManager.RegisterStartupScript(page, typeof(WebFormHelper), "mensagem", script, true);
                    }

                    HttpContext.Current.Session["tpMensagem"] = null;
                }
                catch
                {

                }
            }
        }

        public string UsuarioLogado
        {
            get
            {
                var usuarioLogado = string.Empty;

                var manterUsuario = new ManterUsuario();

                var usuario = manterUsuario.ObterUsuarioLogado();

                if (usuario != null)
                {
                    usuarioLogado = usuario.Nome;
                }

                return usuarioLogado;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            var manterUsuario = new ManterUsuario();

            if (!manterUsuario.EstaLogado())
            {
                var urlDeRetorno = Request.Url.ToString();
                var urlResolvida = ResolveUrl("~/Default.aspx");
                var url = string.Format("{0}?ReturnUrl={1}", urlResolvida, urlDeRetorno);
                Response.Redirect(url);
            }
            else
            {
                var usuario = manterUsuario.ObterUsuarioLogado();

                if (usuario != null && usuario.Nome.Length > 0)
                {
                    spanSaudacao.InnerText = string.Format("Olá, {0}", usuario.Nome.Split(' ')[0]);

                    if (manterUsuario.IsSimulandoPerfil())
                    {
                        var perfilSimulado = manterUsuario.ObterPerfilSimulado();

                        spanPerfil.InnerText = string.Format(" ({0}{1})",
                            perfilSimulado.Nome.Split(' ')[0],
                            perfilSimulado.ID == (int)enumPerfil.GestorUC ? " - " + usuario.UF.Sigla : "");
                    }
                    else
                    {
                        spanPerfil.InnerText = "";
                    }
                }
                else
                {
                    spanSaudacao.InnerText = "Olá";
                }
            }

            base.OnInit(e);
        }

        protected void AtualizarPagina()
        {
            Response.Redirect(Request.RawUrl);
        }

        protected void btnPerfilAdministrador_OnServerClick(object sender, EventArgs e)
        {
            var manterUsuario = new ManterUsuario();

            manterUsuario.SetarPerfisOriginais();

            // Atualizar a variável de IsAdmin da sessão.
            Session["UsuarioLogadoIsAdmin"] = manterUsuario.ObterUsuarioLogado().IsAdministrador();

            AtualizarPagina();
        }

        protected void AlterarPerfil_OnServerClick(object sender, EventArgs e)
        {
            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado(true);

            if (usuarioLogado.ListaPerfil.Any(x => x.Perfil.ID == (int)enumPerfil.Administrador))
            {
                var idPerfil = ((HtmlAnchor)sender).Name;

                var manterUsuario = new ManterUsuario();

                manterUsuario.SetarPerfilSimulado(int.Parse(idPerfil));

                // Atualizar a variável de IsAdmin da sessão.
                Session["UsuarioLogadoIsAdmin"] = manterUsuario.ObterUsuarioLogado().IsAdministrador();

                AtualizarPagina();
            }
            else
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Você não tem permissões de Administrador para utilizar a simulação de perfis.");

        }

        protected void VerificarSimulacaoPerfil()
        {
            var manterUsuario = new ManterUsuario();

            var isAdmin = manterUsuario.PerfilAdministrador();

            var isSimulandoPerfil = manterUsuario.IsSimulandoPerfil();

            if (!isAdmin && !isSimulandoPerfil)
            {
                AlterarPerfilModal.Visible = false;
            }

            if (!isSimulandoPerfil)
            {
                A1.Visible = false;
            }
        }

        protected void btnFecharHelper_OnClick(object sender, EventArgs e)
        {
            ModalEdicaoHelperTag.Visible = false;
            pnlbackdrop.Visible = false;
            LimparModalHelperTags();
        }

        protected void btnSalvarHelper_OnClick(object sender, EventArgs e)
        {
            try
            {
                var manterHelperTag = new ManterHelperTag();

                var helper = ObterObjetoHelperTag(manterHelperTag);

                manterHelperTag.Salvar(helper);

                // Atualizar as tags da sessão.
                Session["HelperTags"] = new ManterHelperTag().ObterTodos().ToList();

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Texto de ajuda salvo com sucesso.");

                btnFecharHelper_OnClick(null, null);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private HelperTag ObterObjetoHelperTag(ManterHelperTag manterHelperTag)
        {
            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
                throw new AcademicoException("Campo \"Descrição\" é obrigatório.");

            if (txtDescricao.Text.Length > 4000)
                throw new AcademicoException("Campo \"Descrição\" só pode ter no máximo 4000 caracteres..");

            var chave = hdnHelperTagChave.Value;

            var paginaId = int.Parse(hdnHelperTagPaginaId.Value);

            var helper = chave != "0" && !string.IsNullOrWhiteSpace(chave) && paginaId != 0
                ? manterHelperTag.ObterPorChavePagina(chave, paginaId)
                : new HelperTag { Pagina = ObterPaginaAtual() };

            helper.Descricao = txtDescricao.Text;

            return helper;
        }

        private void LimparModalHelperTags()
        {
            txtDescricao.Text = "";
            hdnHelperTagChave.Value = "0";
            hdnHelperTagPaginaId.Value = "0";
        }
    }
}
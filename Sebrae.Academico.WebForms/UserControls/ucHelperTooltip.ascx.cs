using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CKEditor.NET;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucHelperTooltip : UserControl
    {
        public string Chave { get; set; }

        public string CustomClass { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            // Forçar a busca dos dados da tag somente após o load completo, pois os helpers da sessão
            // são atualizados em um evento na MasterPage, portanto dessa forma já teremos esses helpers
            // da sessão com os dados de um possível helper editado.
            Page.LoadComplete += Page_LoadComplete;
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Chave))
            {
                HelperTag.Visible = true;
                btnEditarHelperTag.Visible = true;

                // Se certifica que as tags estão na sessão para evitar consultar ao banco.
                if (Session["HelperTags"] == null)
                    Session["HelperTags"] = new ManterHelperTag().ObterTodos().ToList();

                // Salva se o usuário logado é admin para evitar consultar para cada Tag.
                var usuarioLogadoIsAdmin = Session["UsuarioLogadoIsAdmin"] != null
                    ? (bool)Session["UsuarioLogadoIsAdmin"]
                    : new ManterUsuario().ObterUsuarioLogado().IsAdministrador();

                // Exibir edição somente para Administradores.
                if (!usuarioLogadoIsAdmin)
                    btnEditarHelperTag.Visible = false;

                var tagAtual = ObterTagAtual();

                // Obtém o texto de ajuda ou informa caso não exista.
                var descricao = tagAtual == null || string.IsNullOrWhiteSpace(tagAtual.Descricao)
                    ? "Texto de ajuda não informado"
                    : tagAtual.Descricao;

                HelperTag.Attributes["data-content"] = descricao;

                // Caso a descrição tenha mais de 100 caracteres, adicionar uma classe no popover para deixá-lo mais largo.
                if (descricao.Length > 100)
                    CustomClass = "popover-lg";
            }
        }
        
        protected void btnEditarHelperTag_OnClick(object sender, EventArgs e)
        {
            var master = ObterMasterPagePrincipal();

            // Atribuir os valores da descrição e do ID do helper no Modal lá da Masterpage.
            if (master != null)
            {
                var modal = master.FindControl("ModalEdicaoHelperTag");

                if (modal != null)
                {
                    var backDrop = master.FindControl("pnlbackdrop");

                    if (backDrop != null)
                        backDrop.Visible = true;

                    modal.Visible = true;

                    var paginaAtual = ObterPaginaAtual();

                    if (paginaAtual != null)
                    {
                        var tag = new ManterHelperTag().ObterPorChavePagina(Chave, paginaAtual.ID);

                        // Caso não exista cria um registro de tag vazia no banco isso evita o cadastro manual de novas tags
                        if(tag == null && !String.IsNullOrEmpty(Chave) && paginaAtual.ID!=0)
                        {
                            tag = new HelperTag();
                            tag.Pagina = paginaAtual;
                            tag.Chave = Chave;
                            new ManterHelperTag().Salvar(tag);
                        }

                        if (tag != null)
                        {
                            ((HiddenField) modal.FindControl("hdnHelperTagChave")).Value = tag.Chave;
                            ((HiddenField) modal.FindControl("hdnHelperTagPaginaId")).Value = tag.Pagina.ID.ToString();
                            ((CKEditorControl) modal.FindControl("txtDescricao")).Text = tag.Descricao;
                        }
                    }
                }
            }
        }

        private MasterPage ObterMasterPagePrincipal()
        {
            // Loop básico pra buscar a MasterPage principal. Desculpa.
            return Page.Master != null ? ObterMasterPagePrincipal(Page.Master) : null;
        }

        private MasterPage ObterMasterPagePrincipal(MasterPage masterPage)
        {
            return masterPage.Master == null ? masterPage : ObterMasterPagePrincipal(masterPage.Master);
        }

        /// <summary>
        /// Buscar a tag atual de acordo com o campo Chave. Em vez de buscar do banco, busca da sessão para evitar sobrecarga.
        /// </summary>
        /// <returns></returns>
        private HelperTag ObterTagAtual()
        {
            List<HelperTag> tags;

            // Obtém a tag atual da sessão, caso exista. Se não existir, obtém do banco e atualiza a sessão para o próximo uso.
            if (Session["HelperTags"] != null)
            {
                tags = Session["HelperTags"] as List<HelperTag>;
            }
            else
            {
                tags = new ManterHelperTag().ObterTodos().ToList();

                Session["HelperTags"] = tags;
            }

            var paginaAtual = ObterPaginaAtual();

            return tags != null && paginaAtual != null
                ? tags.FirstOrDefault(x => x.Chave == Chave && x.Pagina.ID == paginaAtual.ID)
                : null;
        }

        private Dominio.Classes.Pagina ObterPaginaAtual()
        {
            // Certifica que a página atual está salva na sessão.
            if (Session["paginaAtual"] == null)
            {
                var manterPagina = new ManterPagina();

                Session["paginaAtual"] = manterPagina.ObterPaginaPorCaminhoRelativo(Request.Url.PathAndQuery) ??
                                         manterPagina.ObterPaginaPorCaminhoRelativo(Request.Url.AbsolutePath);
            }

            return Session["paginaAtual"] as Dominio.Classes.Pagina;
        }
    }
}
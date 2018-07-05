using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucCategorias : UserControl
    {
        private List<int> _idsCategoriasMarcadas;
        private List<int> _idsCategoriasExistentes;

        protected bool IsRadio
        {
            get
            {
                return hdnIsRadio.Value == "1";
            }
            set
            {
                hdnIsRadio.Value = value ? "1" : "0";
            }
        }

        public List<int> IdsCategoriasMarcadas
        {
            get
            {
                _idsCategoriasMarcadas = new List<int>();

                if (IsRadio)
                {
                    var idSelecionado = int.Parse(hdnIdCategoriaRadio.Value);

                    if (idSelecionado != 0)
                    {
                        _idsCategoriasMarcadas.Add(idSelecionado);
                    }

                    return _idsCategoriasMarcadas;
                }

                foreach (TreeNode node in trvCategorias.Nodes)
                    MarcarSelecionadas(node);

                return _idsCategoriasMarcadas;
            }
        }

        public void CategoriaSelecionada(int id)
        {
            hdnIdCategoriaRadio.Value = id.ToString();
        }

        public IEnumerable<int> IdsCategoriasExistentes
        {
            get
            {
                _idsCategoriasExistentes = new List<int>();

                foreach (TreeNode node in trvCategorias.Nodes)
                    MarcarExistentes(node);

                return _idsCategoriasExistentes;
            }
        }

        public EventHandler TreeNodeCheckChanged;

        public void OnTreeNodeCheckChanged(TreeNodeEventArgs e)
        {
            if (TreeNodeCheckChanged != null)
            {
                TreeNodeCheckChanged(this, e);
            }
        }

        public bool Enabled
        {
            set
            {
                string disabled = value ? "" : "disabled";

                string script = "$(\"input[type = 'checkbox']\").each(function(){" +
                                   "$(this).attr('disabled', '" + disabled + "');" +
                                 "});";

                ScriptManager.RegisterStartupScript(this, typeof(Page), "enableUcCategorias", script, true);
            }
        }

        protected void trvCategorias_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            OnTreeNodeCheckChanged(e);

            if (IsRadio)
            {
                hdnIdCategoriaRadio.Value = IdsCategoriasMarcadas.FirstOrDefault().ToString();
            }
        }

        public void PreencherCategorias()
        {
            PreencherCategorias(false, null);
        }

        /// <summary>
        /// Preenche as Categorias de acordo com os parâmetros informados.
        /// </summary>
        /// <param name="radio">Caso true, os nodes serão radiobuttons, caso false, serão checkboxes.</param>
        /// <param name="selecionados">Lista de categorias selecioadas que serão marcadas nos nodes.</param>
        /// <param name="usuario">Usuário logado, para evitar carga, quando necessário. Caso null, busca o usuário logado do ManterUsuario.</param>
        /// <param name="exibirTodasUfsGestor">Caso true, busca as categorias da UF do gestor logado sem passar por algune filtros.
        /// Caso false, busca todas as categorias do gestor logado, ou todas as categorias caso não seja gestor.</param>
        /// <param name="exibirTodasCategorias">Nem lembro mais o que essa propriedade faz... Desculpe.</param>
        /// <param name="exibirSelecionarTodos">Exibir botão de selecionar/desmarcar todas.</param>
        public void PreencherCategorias(bool radio, IList<int> selecionados, Usuario usuario = null, bool exibirTodasUfsGestor = false, bool exibirTodasCategorias = false, bool? exibirSelecionarTodos = true)
        {
            // Verifica se existe postback na seleção de uma categoria.
            var postback = TreeNodeCheckChanged != null;

            IsRadio = radio;

            trvCategorias.Nodes.Clear();

            if (!radio)
            {
                trvCategorias.Attributes.Add("onclick", trvCategorias.ClientID + "_OnTreeClick(this);" + (postback ? trvCategorias.ClientID + "_postBackByObject()" : ""));
            }
            else if (selecionados != null && selecionados.Any())
            {
                hdnIdCategoriaRadio.Value = selecionados.First().ToString();
            }

            usuario = usuario ?? new ManterUsuario().ObterUsuarioLogado();

            var categorias = ObterCategoriasPorUsuario(usuario, exibirTodasUfsGestor, exibirTodasCategorias);

            foreach (var pai in categorias.Where(c => c.CategoriaConteudoPai == null))
            {
                var nodePai = new TreeNode();

                PreencherHierarquia(pai, nodePai, radio, postback, selecionados);

                trvCategorias.Nodes.Add(nodePai);
            }

            VerificarExibicaoSelecionarTodos(exibirSelecionarTodos);

            ExpandNodes();

            trvCategorias.CollapseAll();
        }

        /// <summary>
        /// Carregar exibição da funcionalidade de selecionar todas as categorias.
        /// Salva o estado sempre que chamado, para os casos de telas que chamam
        /// o preenchimento das categorias várias vezes, o preenchimento poderá
        /// ser chamado apenas na primeira vez e o comportamento será preservado
        /// através dos próximos PostBacks.
        /// </summary>
        /// <param name="exibirSelecionarTodos">Variável para exibir a funcionalidade de seleção de todas as categorias.</param>
        private void VerificarExibicaoSelecionarTodos(bool? exibirSelecionarTodos)
        {
            var viewStateName = "exibirSelecionarTodas_" + ClientID;

            var exibir = ViewState[viewStateName] != null && (bool)ViewState[viewStateName];

            if (exibirSelecionarTodos != null)
                exibir = exibirSelecionarTodos.Value;

            divSelecionarTodos.Visible = exibir;

            ViewState[viewStateName] = exibir;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Caso esteja carregando pela primeira vez, limpa o estado da visualização da funcionalidade
                // de selecionar todas as categorias.
                ViewState["exibirSelecionarTodas_" + ClientID] = null;
            }
        }

        public void PreencherTodasCategorias(bool radio, IList<int> selecionados, Usuario usuario = null, bool postback = false, bool exibirTodasUfsGestor = false)
        {
            PreencherCategorias(radio, selecionados, usuario, exibirTodasUfsGestor, true);
        }

        private static IEnumerable<CategoriaConteudo> ObterCategoriasPorUsuario(Usuario usuario, bool exibirTodasUfsGestor, bool exibirTodasCategorias = false)
        {
            var manterCategoria = new ManterCategoriaConteudo();

            // Caso não deseje filtrar as categorias, exibir todas.
            if (exibirTodasCategorias)
                return manterCategoria.ObterTodasCategoriasConteudo();

            // Atualizar lista de categorias, pois fica salva na sessão.
            var listaCategoriaConteudo = new ManterUsuario().ObterUsuarioPorID(usuario.ID).ListaCategoriaConteudo;

            if (usuario == null)
                throw new AcademicoException("Usuário inválido na busca de categorias");

            // Caso seja gestor, filtrar as categorias.
            if (usuario.IsGestor())
            {
                if (exibirTodasUfsGestor)
                    return manterCategoria.ObterTodasCategoriasConteudoPorUF(usuario.UF);

                // Caso o usuário possua categorias selecionadas, filtra mais ainda.
                if (listaCategoriaConteudo.Any())
                {
                    return manterCategoria.ObterTodasCategoriasConteudoPorUF(usuario.UF)
                        .Where(c => c.ListaUsuario.Select(u => u.ID).Contains(usuario.ID));
                }

                return manterCategoria.ObterTodasCategoriasConteudoPorUF(usuario.UF);
            }

            // Caso seja administrador, não filtrar as categorias.
            return manterCategoria.ObterTodasCategoriasConteudo();
        }

        public void PreencherCategorias(bool radio)
        {
            PreencherCategorias(radio, null);
        }

        /// <summary>
        /// Expandir os nodes, via Javascript injetado na tela, até chegar ao Node selecionado. Parabéns WebForms, te amo.
        /// </summary>
        private void ExpandNodes()
        {
            var allNodes = GetAllNodes().Distinct().ToList();

            foreach (var node in allNodes.Where(x => x.Checked))
            {
                // Obtém os nodes dos pais e o próprio node ven na lista.
                var pais = GetAllParentNodes(node);

                foreach (var pai in pais)
                {
                    int index;

                    // Obtém o índice do pai na lista de nodes global.
                    for (index = 0; index < allNodes.Count(); index++)
                    {
                        if (allNodes[index].Value == pai.Value)
                        {
                            // Insere o Javascript pra expandir o node na tela.
                            SetNodeExpandJavascript(index);
                        }
                    }
                }
            }
        }

        private void SetNodeExpandJavascript(int index)
        {
            var trvCientId = trvCategorias.ClientID;

            var script =
                "$(document).ready(function() {TreeView_ToggleNode(" + trvCientId + "_Data," + index +
                ",document.getElementById('" + trvCientId + "n" +
                index + "'),' ',document.getElementById('" + trvCientId + "n" + index + "Nodes'))});";

            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "toggleNode_" + index, script, true);
        }

        private IEnumerable<TreeNode> GetAllParentNodes(TreeNode node, bool insertThis = false)
        {
            var retorno = new List<TreeNode>();

            if (insertThis)
                retorno.Add(node);

            if (node.Parent != null)
            {
                retorno.AddRange(GetAllParentNodes(node.Parent, true));
            }

            return retorno;
        }

        private IEnumerable<TreeNode> GetAllNodes(TreeNode node = null)
        {
            var retorno = new List<TreeNode>();

            if (node == null)
            {
                foreach (TreeNode trvNode in trvCategorias.Nodes)
                {
                    retorno.AddRange(GetAllNodes(trvNode));
                }
            }
            else
            {
                retorno.Add(node);

                foreach (TreeNode child in node.ChildNodes)
                {
                    retorno.AddRange(GetAllNodes(child));
                }
            }

            return retorno;
        }

        private void PreencherHierarquia(CategoriaConteudo pai, TreeNode nodePai, bool radio, bool postback, IList<int> selecionados, Uf uf = null)
        {
            nodePai.Value = pai.ID.ToString();

            nodePai.Text = pai.Nome;

            if (radio)
                AlterarNodeParaRadio(pai, nodePai, postback, selecionados);

            if (selecionados != null)
                nodePai.Checked = selecionados.Any(x => x == pai.ID);

            nodePai.SelectAction = TreeNodeSelectAction.None;

            if (selecionados != null) nodePai.Checked = selecionados.Any(x => x == pai.ID);

            // Nacionaliza ou não a lista de filhos.
            var listaFilhos = uf == null
                ? pai.ListaCategoriaConteudoFilhos
                : pai.ListaCategoriaConteudoFilhos.Where(
                    c => c.ListaCategoriaConteudoUF.Any(u => u.UF.ID == uf.ID));

            foreach (var filho in listaFilhos)
            {
                var nodeFilho = new TreeNode
                {
                    Value = filho.ID.ToString(),
                    Text = filho.Nome
                };

                if (selecionados != null) nodeFilho.Checked = selecionados.Any(x => x == filho.ID);

                if (radio)
                    AlterarNodeParaRadio(filho, nodeFilho, postback, selecionados);

                if (selecionados != null)
                    nodeFilho.Checked = selecionados.Any(x => x == filho.ID);

                PreencherHierarquia(filho, nodeFilho, radio, postback, selecionados, uf);

                nodeFilho.SelectAction = TreeNodeSelectAction.None;

                nodePai.ChildNodes.Add(nodeFilho);
            }
        }

        private void AlterarNodeParaRadio(CategoriaConteudo categoria, TreeNode node, bool postback, IList<int> selecionados)
        {
            node.ShowCheckBox = false;
            node.Text = "<input " + (postback ? "class='mostrarload'" : "") + " type='radio' name='" + trvCategorias.ClientID + "_rbCategoriaConteudo' value ='" +
                        categoria.ID + "' " +
                        (selecionados != null && selecionados.Any(x => x == categoria.ID) ? "checked='checked'" : "") +
                        "/>" + categoria.Nome;
        }

        private void MarcarSelecionadas(TreeNode nodePai)
        {
            if (nodePai.Checked)
                _idsCategoriasMarcadas.Add(int.Parse(nodePai.Value));

            foreach (TreeNode filho in nodePai.ChildNodes)
            {
                MarcarSelecionadas(filho);
            }
        }

        private void MarcarExistentes(TreeNode nodePai)
        {
            _idsCategoriasExistentes.Add(int.Parse(nodePai.Value));

            foreach (TreeNode filho in nodePai.ChildNodes)
            {
                MarcarExistentes(filho);
            }
        }

        protected void lnkCategoria_OnClick(object sender, EventArgs e)
        {
            var selectedNode = GetAllNodes().FirstOrDefault(node => node.Value == hdnIdCategoriaRadio.Value);

            if (selectedNode != null)
            {
                var args = new TreeNodeEventArgs(selectedNode);

                OnTreeNodeCheckChanged(args);
            }
        }
    }
}
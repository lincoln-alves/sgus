using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucTags : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.lvTags.ItemDataBound += ListView1_ItemDataBound;
        }

        public void PreencherTags()
        {
            IList<Tag> ListaTags = new ManterTag().ObterTodasTagNaoSinonimas();
            foreach (var item in ListaTags.Where(x => x.NumeroNivel == 0))
            {
                TreeNode registro = new TreeNode();
                registro.Value = item.ID.ToString();
                registro.Text = item.Nome;
                registro.SelectAction = TreeNodeSelectAction.None;
                if (item.ListaTagFilhos.Count > 0)
                {
                    RetornaFilhosTags(item, registro);
                }
                //Antes de adicionar, verifica se já existe na lista
                if (!TagJaExisteNaLista(registro))
                {
                    trvTags.Nodes.Add(registro);
                }

            }
            trvTags.CollapseAll();
        }

        private bool TagJaExisteNaLista(TreeNode tag)
        {
            bool tagJaEstaAdicionada = false;

            for (int i = 0; i < trvTags.Nodes.Count; i++)
            {
                if (trvTags.Nodes[i].Value == tag.Value)
                {
                    tagJaEstaAdicionada = true;
                    break;
                }
            }

            return tagJaEstaAdicionada;
        }

        private void RetornaFilhosTags(Tag registro, TreeNode lista)
        {

            foreach (var tagfilho in registro.ListaTagFilhos.Where(x => x.InSinonimo != true))
            {
                TreeNode filho = new TreeNode();
                filho.Value = tagfilho.ID.ToString();
                filho.Text = tagfilho.Nome;
                filho.SelectAction = TreeNodeSelectAction.None;
                if (tagfilho.ListaTagFilhos.Count > 0)
                {
                    RetornaFilhosTags(tagfilho, filho);
                }
                lista.ChildNodes.Add(filho);
            }

        }


        private List<Tag> tagsNaoSelecionadas;

        public List<Tag> TagsNaoSelecionadas
        {
            get
            {
                ObterInformacoesSobreAsTags();
                return tagsNaoSelecionadas;
            }
            set
            {
                tagsNaoSelecionadas = value;
            }
        }

        private List<Tag> tagsSelecionadas;

        public List<Tag> TagsSelecionadas
        {
            get
            {
                ObterInformacoesSobreAsTags();
                return tagsSelecionadas;
            }
            set
            {
                tagsSelecionadas = value;
            }
        }

        public void ObterInformacoesSobreAsTags()
        {

            tagsSelecionadas = new List<Tag>();
            tagsNaoSelecionadas = new List<Tag>();

            foreach (TreeNode node in trvTags.Nodes)
            {
                ProcessaTag(node);
                if (node.ChildNodes != null && node.ChildNodes.Count > 0)
                {
                    ObterTagsSelecionadasFilhos(node);
                }
            }
        }
        private void ObterTagsSelecionadasFilhos(TreeNode lista)
        {

            foreach (TreeNode childNode in lista.ChildNodes)
            {
                ProcessaTag(childNode);
                if (childNode.ChildNodes != null && childNode.ChildNodes.Count > 0)
                {
                    ObterTagsSelecionadasFilhos(childNode);
                }
            }
        }

        private void ProcessaTag(TreeNode no)
        {
            Tag tag = new Tag() { ID = int.Parse(no.Value), Nome = no.Text };
            if (no.Checked)
            {
                tagsSelecionadas.Add(tag);
            }
            else
            {
                tagsNaoSelecionadas.Add(tag);
            }
        }



        internal void PreencherListViewComTagsGravadosNoBanco(IList<Tag> ListaTags)
        {
            if (ListaTags != null && ListaTags.Count > 0)
            {
                foreach (TreeNode node in trvTags.Nodes)
                {
                    //CheckBox checkboxTag = (CheckBox)lvTags.Items[i].FindControl("chkTag");
                    //HiddenField hdfIDTag = (HiddenField)lvTags.Items[i].FindControl("hdfIDTag");

                    bool tagFoiEscolhida = this.VerificarSeATagFoiEscolhida(ListaTags, int.Parse(node.Value));
                    node.Checked = tagFoiEscolhida;
                    if (node.ChildNodes != null && node.ChildNodes.Count > 0)
                    {
                        RecuperaTagsSelecionadasFilhos(node, ListaTags);
                    }
                }
            }
        }

        private void RecuperaTagsSelecionadasFilhos(TreeNode lista, IList<Tag> ListaTags)
        {

            foreach (TreeNode childNode in lista.ChildNodes)
            {
                bool tagFoiEscolhida = this.VerificarSeATagFoiEscolhida(ListaTags, int.Parse(childNode.Value));
                childNode.Checked = tagFoiEscolhida;
                if (childNode.ChildNodes != null && childNode.ChildNodes.Count > 0)
                {
                    RecuperaTagsSelecionadasFilhos(childNode, ListaTags);
                }
            }
        }

        private bool VerificarSeATagFoiEscolhida(IList<Tag> ListaTag, int IDTag)
        {
            return ListaTag.Where(x => x.ID == IDTag).Any();
        }

    }
}
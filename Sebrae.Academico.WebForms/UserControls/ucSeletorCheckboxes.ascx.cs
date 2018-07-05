using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucSeletorCheckboxes : System.Web.UI.UserControl
    {
        public ListItemCollection Items
        {
            get
            {
                return cblItens.Items;
            }
        }

        public string Descricao { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblDescricao.Text = Descricao;
        }

        /// <summary>
        /// Preenche a lista de itens disponíveis. Para preencher a lista de selecionados, 
        /// utilizar o método "MarcarComoSelecionados" após este.
        /// </summary>
        public void PreencherItens(IEnumerable lista, string campoId, string campoDescricao, bool selecionadosPorPadrao)
        {
            cblItens.Items.Clear();
            
            foreach (var item in lista)
            {
                var id = item.GetType().GetProperty(campoId).GetValue(item, null);
                var descricao = item.GetType().GetProperty(campoDescricao).GetValue(item, null);

                var listItem = new ListItem()
                {
                    Text = descricao.ToString(),
                    Value = id.ToString(),
                    Selected = selecionadosPorPadrao
                };

                listItem.Attributes.Add("title", descricao.ToString());

                cblItens.Items.Add(listItem);
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T">Tipo da lista a ser retornada com os values selecionados.</typeparam>
        public IEnumerable<T> RecuperarIdsSelecionados<T>()
        {
            return cblItens.Items.Cast<ListItem>()
                .Where(l => l.Selected)
                .Select(l => l.Value)
                .Select(val => (T)Convert.ChangeType(val, typeof(T))).ToArray();
        }

    }
}
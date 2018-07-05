using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucUF : System.Web.UI.UserControl
    {
        public List<int> IdsUfsMarcados
        {
            get
            {
                var lista = (from i in chkUF.Items.Cast<ListItem>().Where(i => i.Selected).ToList()
                             select int.Parse(i.Value)).ToList();

                return lista;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void PreencherUFs()
        {
            var lista = new BMUf().ObterTodos();
            WebFormHelper.PreencherLista(lista, chkUF);
        }

        public void PreencherUfsCategoria(CategoriaConteudo categoria)
        {
            foreach (ListItem item in chkUF.Items)
            {
                item.Selected = categoria.ListaCategoriaConteudoUF.Any(c => c.UF.ID == int.Parse(item.Value));
            }
        }

        public void PreencherUfsFornecedor(Fornecedor fornecedor)
        {
            foreach (ListItem item in chkUF.Items)
            {
                item.Selected = fornecedor.ListaFornecedorUF.Any(f => f.UF.ID == int.Parse(item.Value));
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucSeletorListBox : System.Web.UI.UserControl
    {
        public string DescricaoDisponiveis { get; set; }
        public string DescricaoSelecionados { get; set; }
        public string MostrarSelecaoTodos { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Recarregando os selecionados quando a página recarrega
            if (IsPostBack)
            {
                var marcados = RecuperarIdsSelecionados();
                MarcarComoSelecionados(marcados);
            }

            lblDisponiveis.Text = DescricaoDisponiveis + "&nbsp;";
            lblSelecionados.Text = DescricaoSelecionados + "&nbsp;";

        }

        /// <summary>
        /// Preenche a lista de itens disponíveis. Para preencher a lista de selecionados, 
        /// utilizar o método "MarcarComoSelecionados" após este.
        /// </summary>
        public void PreencherItens(IEnumerable lista, string campoId, string campoDescricao, bool inserirTodos = false, bool removerSelecionados = true)
        {
            ddlItensDisponiveis.Items.Clear();
            if (removerSelecionados)
            {
                ddlItensSelecionados.Items.Clear();
            }

            inputAddTodos.Visible = MostrarSelecaoTodos == "True";

            if (inserirTodos)
            {
                ddlItensDisponiveis.Items.Add(new ListItem("- Todos -", "0"));
            }

            foreach (var item in lista)
            {
                var id = item.GetType().GetProperty(campoId).GetValue(item, null);
                var descricao = item.GetType().GetProperty(campoDescricao).GetValue(item, null);
                ddlItensDisponiveis.Items.Add(new ListItem(descricao.ToString(), id.ToString()));
            }
        }

        /// <summary>
        /// Recebe um IEnumerable de IDs e os marca como items selecionados. Eles devem ter sido carregados anteriormente na lista de disponíveis.
        /// </summary>
        public void MarcarComoSelecionados(IEnumerable idsSelecionados)
        {
            ddlItensDisponiveis.Items.AddRange(ddlItensSelecionados.Items.Cast<ListItem>().ToArray());
            ddlItensSelecionados.Items.Clear();

            foreach (var id in idsSelecionados)
            {

                if (ddlItensSelecionados.Items.FindByValue(id.ToString()) != null)
                    continue;

                var item = ddlItensDisponiveis.Items.FindByValue(id.ToString());

                if (item != null)
                {
                    ddlItensDisponiveis.Items.Remove(item);

                    ddlItensSelecionados.Items.Add(item);
                }
            }

            var valoresSelecionados = ddlItensSelecionados.Items.Cast<ListItem>().Select(i => i.Value);
            hdnSelecionados.Value = string.Join(",", valoresSelecionados);
        }

        public IEnumerable<string> RecuperarIdsSelecionados()
        {
            return hdnSelecionados.Value.Split(',')
                .Where(val => !string.IsNullOrWhiteSpace(val)).ToArray();
        }

        public IEnumerable<int> RecuperarIdsSelecionadosNumerico()
        {
            return hdnSelecionados.Value.Split(',')
                .Where(val => !string.IsNullOrWhiteSpace(val))
                .Select(x => Convert.ToInt32(x))
                .ToArray();
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T">Tipo da lista a ser retornada com os values selecionados.</typeparam>
        public IEnumerable<T> RecuperarIdsSelecionados<T>()
        {
            return RecuperarIdsSelecionados()
                .Select(val => (T)Convert.ChangeType(val, typeof(T))).ToArray();
        }

        public void LimparSelecao()
        {
            var itensSelecionados = ddlItensSelecionados.Items;

            foreach (var item in itensSelecionados)
            {
                ddlItensDisponiveis.Items.Add((ListItem)item);
            }

            ddlItensSelecionados.Items.Clear();
            hdnSelecionados.Value = "";
        }

        public void RemoverTodos()
        {
            ddlItensDisponiveis.Items.Clear();
            ddlItensSelecionados.Items.Clear();
        }
    }
}
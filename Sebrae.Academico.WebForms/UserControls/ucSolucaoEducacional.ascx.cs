using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucSolucaoEducacional : System.Web.UI.UserControl
    {
        public string Chave { get; set; }

        public ListItemCollection ObterTodasSolucoesEducacionais
        {
            get
            {
                return this.lbSolucoesEscolhidas.Items;
            }
        }
        
        public IList<SolucaoEducacional> ConverterListItemCollectionEmListaTipada(ListItemCollection solucoesEducacionais)
        {
            IList<SolucaoEducacional> ListaSolucaoEducacional = null;

            if (solucoesEducacionais != null && solucoesEducacionais.Count > 0)
            {
                ListaSolucaoEducacional = new List<SolucaoEducacional>();

                for (int i = 0; i < solucoesEducacionais.Count; i++)
                {
                    ListaSolucaoEducacional.Add(new SolucaoEducacional()
                    {
                        ID = int.Parse(solucoesEducacionais[i].Value),
                        Nome = solucoesEducacionais[i].Text
                    });
                }
            }

            return ListaSolucaoEducacional;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UcHelperTooltip6.Chave = Chave;

            if (!IsPostBack)
            {
                PreencherCombos();
            }
        }

        private void PreencherCombos()
        {
            try
            {
                PreencherComboSolucaoEducacional();
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void PreencherComboSolucaoEducacional()
        {
            var manterSolucaoEducacional = new ManterSolucaoEducacional();

            var listaSolucaoEducacional = manterSolucaoEducacional.ObterTodosSolucaoEducacional().ToList();

            WebFormHelper.PreencherLista(listaSolucaoEducacional, ddlSolucaoEducacional, false, true);
        }

        public void PreencherListBoxComSolucoesEducacionaisGravadasNoBanco(IList<SolucaoEducacional> listaSolucaoEducacional)
        {

            if (listaSolucaoEducacional != null && listaSolucaoEducacional.Count > 0)
            {
                WebFormHelper.PreencherLista(listaSolucaoEducacional, this.lbSolucoesEscolhidas);
            }
        }

        private bool VerificarSeASolucaoEducacionalFoiEscolhida(IList<SolucaoEducacional> listaSolucaoEducacional, int idSolucaoEducacional)
        {
            return listaSolucaoEducacional.Any(x => x.ID == idSolucaoEducacional);
        }
                
        protected void btnAdicionar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlSolucaoEducacional.SelectedItem != null &&
                      !string.IsNullOrWhiteSpace(ddlSolucaoEducacional.SelectedItem.Value))
                {

                    SolucaoEducacional solucaoEducacionalEscolhida = new SolucaoEducacional()
                    {
                        ID = int.Parse(this.ddlSolucaoEducacional.SelectedItem.Value),
                        Nome = this.ddlSolucaoEducacional.SelectedItem.Text.Trim()
                    };

                    var ListaSolucaoEducacionaisSelecionadas = this.lbSolucoesEscolhidas.Items;
                    bool JaFoiAdicionado = false;

                    for (int i = 0; i < ListaSolucaoEducacionaisSelecionadas.Count; i++)
                    {
                        if (int.Parse(ListaSolucaoEducacionaisSelecionadas[i].Value).Equals(solucaoEducacionalEscolhida.ID))
                        {
                            JaFoiAdicionado = true;
                            break;
                        }
                    }

                    if (JaFoiAdicionado)
                        throw new AcademicoException("Esta Solução Educacional Já foi Adicionada");
                    else
                    {
                        this.lbSolucoesEscolhidas.Items.Add(new ListItem(solucaoEducacionalEscolhida.Nome, solucaoEducacionalEscolhida.ID.ToString()));
                    }
                }
                else
                {
                    throw new AcademicoException("Selecione a Solução Educacional");
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }
        
        protected void btnRemoverSelecionados_Click(object sender, EventArgs e)
        {
            RemoverItemsSelecionados();
        }

        private bool TemItemSelecionado
        {
            get
            {
                bool temAlgumItemSelecionado = false;

                if (this.lbSolucoesEscolhidas.GetSelectedIndices().Length > 0)
                {
                    temAlgumItemSelecionado = true;
                }

                return temAlgumItemSelecionado;
            }
        }

        private void RemoverItemsSelecionados()
        {
            try
            {
                if (!this.TemItemSelecionado)
                {
                    throw new AcademicoException("Selecione uma Solução Educacional para Remover da Lista");
                }

                var solucoesEducacionais = this.lbSolucoesEscolhidas.Items.Count;

                for (int i = 0; i < solucoesEducacionais; i++)
                {
                    if (this.lbSolucoesEscolhidas.Items[i].Selected)
                    {
                        this.lbSolucoesEscolhidas.Items.Remove(this.lbSolucoesEscolhidas.Items[i]);
                    }
                }

                this.ddlSolucaoEducacional.ClearSelection();
            }
            catch (Exception)
            {
                //WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected void btnRemoverTodos_Click(object sender, EventArgs e)
        {
            this.lbSolucoesEscolhidas.Items.Clear();
            this.ddlSolucaoEducacional.ClearSelection();
        }
        
       

    }
}
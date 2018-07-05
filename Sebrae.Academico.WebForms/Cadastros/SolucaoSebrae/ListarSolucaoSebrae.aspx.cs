using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.SolucaoSebrae
{
    public partial class ListarSolucaoSebrae : Page
    {
        private classes.ItemTrilha _itemtrilhaBusca;

        private ManterItemTrilha _manterItemTrilha = new ManterItemTrilha();

        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (dgvItemTrilha.Rows.Count > 0)
            {
                dgvItemTrilha.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    PreencherComboTrilhas();
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("EdicaoSolucaoSebrae.aspx");
        }

        protected void dgvItemTrilha_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    _manterItemTrilha = new ManterItemTrilha();
                    int idItemTrilha = int.Parse(e.CommandArgument.ToString());
                    _manterItemTrilha.ExcluirItemTrilha(idItemTrilha);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarSolucaoSebrae.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                int idTrilhaTopicoTematico = int.Parse(e.CommandArgument.ToString());
                //Session.Add("ItemTrilhaEdit", idTrilhaTopicoTematico);
                Response.Redirect("EdicaoSolucaoSebrae.aspx?Id=" + idTrilhaTopicoTematico.ToString(), false);
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                var itemTrilha = ObterObjetoItemTrilha();

                var listaItemTrilha = _manterItemTrilha.ObterItemTrilhaPorFiltro(itemTrilha).Where(x => x.Missao != null && x.Usuario == null).ToList();

                WebFormHelper.PreencherGrid(listaItemTrilha, dgvItemTrilha);

                if (listaItemTrilha.Any())
                {
                    pnlItemTrilha.Visible = true;
                }
                else
                {
                    pnlItemTrilha.Visible = false;
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        public classes.ItemTrilha ObterObjetoItemTrilha()
        {
            _itemtrilhaBusca = new classes.ItemTrilha
            {
                Nome = txtNome.Text.Trim(),
                Missao = new classes.Missao
                {
                    ID = ddlMissao.SelectedIndex <= 0 ? 0 : int.Parse(ddlMissao.SelectedValue),
                    PontoSebrae = new classes.PontoSebrae
                    {
                        ID = ddlPontoSebrae.SelectedIndex <= 0 ? 0 : int.Parse(ddlPontoSebrae.SelectedValue),
                        TrilhaNivel = new classes.TrilhaNivel
                        {
                            ID = ddlTrilhaNivel.SelectedIndex <= 0 ? 0 : int.Parse(ddlTrilhaNivel.SelectedValue)
                        }
                    }
                }
            };

            //Ativo
            if (rblAtivo.SelectedItem != null)
            {
                if (rblAtivo.SelectedItem.Value.Equals("S"))
                    _itemtrilhaBusca.Ativo = true;
                else
                    _itemtrilhaBusca.Ativo = false;
            }
            else
            {
                _itemtrilhaBusca.Ativo = null;
            }
            /*
            //Ativo
            if (rblAutoIndicativas.SelectedItem != null)
            {
      
                if (rblAutoIndicativas.SelectedItem.Value.Equals("S"))
                    itemtrilhaBusca.UsuarioAssociado = true;
                else
                    itemtrilhaBusca.UsuarioAssociado = false;
            }
            else
            {
                itemtrilhaBusca.UsuarioAssociado = null;
            }
            */
            return _itemtrilhaBusca;
        }

        protected void ddlTrilha_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ddlTrilha.SelectedItem.Value))
            {
                try
                {
                    //Busca os níveis associados à trilha
                    var trilha = new classes.Trilha { ID = int.Parse(ddlTrilha.SelectedItem.Value) };
                    PreencherComboTrilhaNivel(trilha);
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }

        protected void ddlTrilhaNivel_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ddlTrilhaNivel.SelectedItem.Value))
            {
                try
                {
                    //Busca os níveis associados à trilha
                    var trilhaNivel = new classes.TrilhaNivel { ID = int.Parse(ddlTrilhaNivel.SelectedItem.Value) };
                    PreencherComboPontoSebrae(trilhaNivel);
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }

        protected void ddlPontoSebrae_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ddlPontoSebrae.SelectedItem.Value))
            {
                try
                {
                    //Busca os níveis associados à trilha
                    var pontoSebrae = new classes.PontoSebrae { ID = int.Parse(ddlPontoSebrae.SelectedItem.Value) };
                    PreencherComboMissao(pontoSebrae);
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }

        private void PreencherComboTrilhas()
        {
            WebFormHelper.PreencherLista(new ManterTrilha().ObterTodasTrilhas(), ddlTrilha, false, true);
        }

        private void PreencherComboTrilhaNivel(classes.Trilha trilha)
        {
            var listaTrilhaNivel = new ManterTrilhaNivel().ObterPorTrilha(trilha);

            if (listaTrilhaNivel != null && listaTrilhaNivel.Count > 0)
            {
                WebFormHelper.PreencherLista(listaTrilhaNivel, ddlTrilhaNivel, true);
            }
            else
            {
                ddlTrilhaNivel.Items.Clear();
            }
        }

        private void PreencherComboPontoSebrae(classes.TrilhaNivel trilhaNivel)
        {
            var listaPontoSebrae = new ManterPontoSebrae().ObterPorTrilhaNivel(trilhaNivel);

            if (listaPontoSebrae != null && listaPontoSebrae.Any())
            {
                WebFormHelper.PreencherLista(listaPontoSebrae, ddlPontoSebrae, true);
            }
            else
            {
                ddlPontoSebrae.Items.Clear();
            }
        }

        private void PreencherComboMissao(classes.PontoSebrae pontoSebrae)
        {
            var listaMissoes = new ManterMissao().ObterPorPontoSebrae(pontoSebrae);

            if (listaMissoes != null && listaMissoes.Any())
            {
                WebFormHelper.PreencherLista(listaMissoes, ddlMissao, true);
            }
            else
            {
                ddlPontoSebrae.Items.Clear();
            }
        }
    }
}
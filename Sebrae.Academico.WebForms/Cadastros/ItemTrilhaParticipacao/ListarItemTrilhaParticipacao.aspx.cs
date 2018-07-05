using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

using System.Linq;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class ListarItemTrilhaParticipacao : PageBase
    {
        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvItemTrilhaParticipacao.Rows.Count > 0)
            {
                this.dgvItemTrilhaParticipacao.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        private ManterItemTrilhaParticipacao manterItemTrilhaParticipacao;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    this.PreencherCombos();
                    base.LogarAcessoFuncionalidade();
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected override System.Collections.Generic.IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.ItemTrilhaParticipacao; }
        }

        #region "Métodos Privados"

        private void PreencherCombos()
        {
            try
            {
                PreencherComboTrilhas();
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void PreencherComboTrilhas()
        {
            manterItemTrilhaParticipacao = new ManterItemTrilhaParticipacao();
            IList<Trilha> ListaTrilhas = manterItemTrilhaParticipacao.ObterTrilhas();
            WebFormHelper.PreencherLista(ListaTrilhas, this.ddlTrilha, true, false);
        }

        private void PreencherComboTrilhasNivel(Trilha trilha)
        {
            manterItemTrilhaParticipacao = new ManterItemTrilhaParticipacao();
            IList<classes.TrilhaNivel> ListaTrilhasNivel = manterItemTrilhaParticipacao.ObterTrilhasNivelPorTrilha(trilha);
            WebFormHelper.PreencherLista(ListaTrilhasNivel, this.ddlTrilhaNivel, true, false);
        }

        private void PreencherComboAlunos(int idTrilha, int idTrilhaNivel)
        {
            try
            {
                ManterMatriculaTrilha manterMatriculaTrilha = new ManterMatriculaTrilha();
                IList<Usuario> ListaUsuarios = manterMatriculaTrilha.ObterPorTrilhaTrilhaNivel(idTrilha, idTrilhaNivel);

                var listaNova = new List<Usuario>();

                foreach (var item in ListaUsuarios)
                {
                    if (!listaNova.Any(x => x.ID == item.ID))
                    {
                        listaNova.Add(item);
                    }
                }

                WebFormHelper.PreencherLista(listaNova, this.ddlNomeAluno, true, false);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void PreencherComboTopicoTematico(int idTrilha, int idTrilhaNivel)
        {
            manterItemTrilhaParticipacao = new ManterItemTrilhaParticipacao();
            IList<TrilhaTopicoTematico> ListaTopicoTematico = manterItemTrilhaParticipacao.ObterTopicosTematicosPorTrilhaNivel(idTrilha, idTrilhaNivel);
            WebFormHelper.PreencherLista(ListaTopicoTematico, this.ddlTopicoTematico, true, false);
        }

        private void PreencherComboItemTrilha(ViewTrilha filtro)
        {
            manterItemTrilhaParticipacao = new ManterItemTrilhaParticipacao();
            IList<ItemTrilha> ListaItemTrilha = manterItemTrilhaParticipacao.ObterItemsTrilha(filtro);
            WebFormHelper.PreencherLista(ListaItemTrilha, this.ddlItemTrilha, true, false);
        }

        private ViewTrilha ObterObjetoViewTrilha()
        {
            ViewTrilha viewTrilha = new ViewTrilha();

            //Trilha
            if (!string.IsNullOrWhiteSpace(ddlTrilha.SelectedItem.Value))
            {
                viewTrilha.TrilhaOrigem = new Trilha() { ID = int.Parse(this.ddlTrilha.SelectedItem.Value) };
            }

            //Trilha Nível
            if (!string.IsNullOrWhiteSpace(ddlTrilhaNivel.SelectedItem.Value))
            {
                viewTrilha.TrilhaNivelOrigem = new classes.TrilhaNivel() { ID = int.Parse(this.ddlTrilhaNivel.SelectedItem.Value) };
            }

            //Tópico Temático
            if (!string.IsNullOrWhiteSpace(ddlTopicoTematico.SelectedItem.Value))
            {
                viewTrilha.TopicoTematico = new TrilhaTopicoTematico() { ID = int.Parse(this.ddlTopicoTematico.SelectedItem.Value) };
            }

            //Aluno
            if (!string.IsNullOrWhiteSpace(ddlNomeAluno.SelectedItem.Value))
            {
                viewTrilha.UsuarioOrigem = new Usuario()
                {
                    ID = int.Parse(this.ddlNomeAluno.SelectedItem.Value)
                };
            }

            return viewTrilha;
        }

        private ViewUsuarioItemTrilhaParticipacao ObterObjetoViewUsuarioItemTrilhaParticipacao()
        {
            ViewUsuarioItemTrilhaParticipacao viewUsuarioItemTrilhaParticipacao = new ViewUsuarioItemTrilhaParticipacao();

            //Trilha
            if (ddlTrilha.SelectedItem != null && !string.IsNullOrWhiteSpace(ddlTrilha.SelectedItem.Value))
            {
                viewUsuarioItemTrilhaParticipacao.TrilhaOrigem = new Trilha() { ID = int.Parse(this.ddlTrilha.SelectedItem.Value) };
            }

            //Trilha Nível
            if (ddlTrilhaNivel.SelectedItem != null && !string.IsNullOrWhiteSpace(ddlTrilhaNivel.SelectedItem.Value))
            {
                viewUsuarioItemTrilhaParticipacao.TrilhaNivelOrigem = new classes.TrilhaNivel() { ID = int.Parse(this.ddlTrilhaNivel.SelectedItem.Value) };
            }

            //Tópico Temático
            if (ddlTopicoTematico.SelectedItem != null && !string.IsNullOrWhiteSpace(ddlTopicoTematico.SelectedItem.Value))
            {
                viewUsuarioItemTrilhaParticipacao.TopicoTematico = new TrilhaTopicoTematico() { ID = int.Parse(this.ddlTopicoTematico.SelectedItem.Value) };
            }

            //Aluno
            if (ddlNomeAluno.SelectedItem != null && !string.IsNullOrWhiteSpace(ddlNomeAluno.SelectedItem.Value))
            {
                viewUsuarioItemTrilhaParticipacao.UsuarioOrigem = new Usuario()
                {
                    ID = int.Parse(this.ddlNomeAluno.SelectedItem.Value)
                };
            }

            // Item trilha
            if (ddlItemTrilha.SelectedItem != null && !string.IsNullOrWhiteSpace(ddlItemTrilha.SelectedItem.Value)) {
                viewUsuarioItemTrilhaParticipacao.ItemTrilha = new ItemTrilha()
                {
                    ID = int.Parse(this.ddlItemTrilha.SelectedItem.Value)
                };
            }

            return viewUsuarioItemTrilhaParticipacao;
        }


        #endregion

        protected void ddlTrilha_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ddlTrilha.SelectedItem.Value))
            {
                Trilha trilha = new Trilha() { ID = int.Parse(ddlTrilha.SelectedItem.Value) };
                PreencherComboTrilhasNivel(trilha);
            }
        }

        protected void ddlTrilhaNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((!string.IsNullOrWhiteSpace(ddlTrilha.SelectedItem.Value)) &&
                (!string.IsNullOrWhiteSpace(ddlTrilhaNivel.SelectedItem.Value)))
            {

                int idTrilha = int.Parse(ddlTrilha.SelectedItem.Value);
                int idTrilhaNivel = int.Parse(ddlTrilhaNivel.SelectedItem.Value);

                PreencherComboAlunos(idTrilha, idTrilhaNivel);
                PreencherComboTopicoTematico(idTrilha, idTrilhaNivel);
            }

            else
            {
                ddlNomeAluno.Items.Clear();
            }

        }

        protected void ddlNomeAluno_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ddlNomeAluno.SelectedItem.Value))
            {
                ViewTrilha viewTrilha = ObterObjetoViewTrilha();
                PreencherComboItemTrilha(viewTrilha);
            }
        }

        protected void dgvItemTrilhaParticipacao_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    ManterItemTrilhaParticipacao manterItemTrilhaParticipacao = new ManterItemTrilhaParticipacao();
                    int idItemTrilhaParticipacao = int.Parse(e.CommandArgument.ToString());
                    manterItemTrilhaParticipacao.ExcluirItemTrilhaParticipacao(idItemTrilhaParticipacao);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarItemTrilhaParticipacao.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                int idMatriculaTrilha = int.Parse(e.CommandArgument.ToString());
                //Session.Add("ItemTrilhaParticipacaoEdit", idMatriculaTrilha);
                Response.Redirect("EdicaoItemTrilhaParticipacao.aspx?Id=" + idMatriculaTrilha.ToString(), false);
            }
            else if (e.CommandName.Equals("cadastrar"))
            {
                //int idTrilha = int.Parse(e.CommandArgument.ToString());
                string idsconcatenados = (string)e.CommandArgument;
                //string[] arrayDeIDs = idsconcatenados.Split('&');
                //Session.Add("arrayDeIdsParaItemTrilhaParticipacao", arrayDeIDs);
                Response.Redirect("EdicaoItemTrilhaParticipacao.aspx?IdConcatenado=" + idsconcatenados.Replace('&', '|'), false);
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {

            try
            {
                ViewUsuarioItemTrilhaParticipacao viewUsuarioItemTrilhaParticipacao = ObterObjetoViewUsuarioItemTrilhaParticipacao();
                ManterItemTrilhaParticipacao manterItemTrilhaParticipacao = new ManterItemTrilhaParticipacao();
                IList<ViewUsuarioItemTrilhaParticipacao> ListaViewUsuarioItemTrilhaParticipacao = manterItemTrilhaParticipacao.ObterViewUsuarioItemTrilhaParticipacaoPorFiltro(viewUsuarioItemTrilhaParticipacao);
                

                if (ListaViewUsuarioItemTrilhaParticipacao != null && ListaViewUsuarioItemTrilhaParticipacao.Count > 0)
                {
                    WebFormHelper.PreencherGrid(ListaViewUsuarioItemTrilhaParticipacao, this.dgvItemTrilhaParticipacao);
                    pnlTrilha.Visible = true;
                }
                else
                {
                    pnlTrilha.Visible = false;
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }


        protected void dgvItemTrilhaParticipacao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                ViewUsuarioItemTrilhaParticipacao viewUsuarioItemTrilhaParticipacao = (ViewUsuarioItemTrilhaParticipacao)e.Row.DataItem;

                if (viewUsuarioItemTrilhaParticipacao != null)
                {

                    LinkButton lkbCadastrar = (LinkButton)e.Row.Cells[5].FindControl("lkbCadastrar");
                    LinkButton lkbEditar = (LinkButton)e.Row.Cells[5].FindControl("lkbEditar");
                    LinkButton lkbExcluir = (LinkButton)e.Row.Cells[5].FindControl("lkbExcluir");

                    //Exibe o botão cadastrar, caso o usuário não participe de um item trilha
                    if (viewUsuarioItemTrilhaParticipacao.TemParticipacao.Trim().ToUpper().Equals("N"))
                    {
                        lkbCadastrar.Visible = true;

                        //Esconde o botão Editar quando o usuário não possuir participação
                        lkbEditar.Visible = false;

                        //Esconde o botão Excluir quando o usuário não possuir participação
                        lkbExcluir.Visible = false;
                    }
                    else if (viewUsuarioItemTrilhaParticipacao.TemParticipacao.Trim().ToUpper().Equals("S"))
                    {
                        lkbCadastrar.Visible = false;

                        //Exibe o botão Editar quando o usuário possuir participação
                        lkbEditar.Visible = true;

                        //Exibe o botão Excluir quando o usuário possuir participação
                        lkbExcluir.Visible = true;
                    }
                }
            }
        }


    }
}
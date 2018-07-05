using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros.AtividadeFormativa
{
    public partial class ListarAtividadeFormativaParticipacao : PageBase
    {

        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvAtividadeFormativaParticipacao.Rows.Count > 0)
            {
                this.dgvAtividadeFormativaParticipacao.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        private ManterTrilhaAtividadeFormativaParticipacao manterTrilhaAtividadeFormativaParticipacao;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //if (Session["arrayDeIdsParaItemTrilhaParticipacao"] != null)
                //{
                //    Session["arrayDeIdsParaItemTrilhaParticipacao"] = null;
                //}

                base.LogarAcessoFuncionalidade();
                this.PreencherCombos();
            }
        }

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.AtividadeFormativa; }
        }
        
        protected override System.Collections.Generic.IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
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
            manterTrilhaAtividadeFormativaParticipacao = new ManterTrilhaAtividadeFormativaParticipacao();
            IList<Trilha> ListaTrilhas = manterTrilhaAtividadeFormativaParticipacao.ObterTilhas();
            WebFormHelper.PreencherLista(ListaTrilhas, this.ddlTrilha, true, false);
        }

        private void PreencherComboTrilhasNivel(Trilha trilha)
        {
            manterTrilhaAtividadeFormativaParticipacao = new ManterTrilhaAtividadeFormativaParticipacao();
            IList<classes.TrilhaNivel> ListaTrilhasNivel = manterTrilhaAtividadeFormativaParticipacao.ObterTrilhasNivelPorTrilha(trilha);
            WebFormHelper.PreencherLista(ListaTrilhasNivel, this.ddlTrilhaNivel, true, false);
        }

        private void PreencherComboAlunos(int idTrilha, int idTrilhaNivel)
        {
            ManterMatriculaTrilha manterMatriculaTrilha = new ManterMatriculaTrilha();
            IList<Usuario> ListaUsuarios = manterMatriculaTrilha.ObterPorTrilhaTrilhaNivel(idTrilha, idTrilhaNivel);
            WebFormHelper.PreencherLista(ListaUsuarios, this.ddlNomeAluno, true, false);
        }

        private void PreencherComboTopicoTematico(int idTrilha, int idTrilhaNivel)
        {
            manterTrilhaAtividadeFormativaParticipacao = new ManterTrilhaAtividadeFormativaParticipacao();
            IList<TrilhaTopicoTematico> ListaTopicoTematico = manterTrilhaAtividadeFormativaParticipacao.ObterTopicosTematicosPorTrilhaNivel(idTrilha, idTrilhaNivel);
            WebFormHelper.PreencherLista(ListaTopicoTematico, this.ddlTopicoTematico, true, false);
        }

        private ViewUsuarioTrilhaAtividadeFormativaParticipacao ObterObjetoViewUsuarioTrilhaAtividadeFormativaParticipacao()
        {
            ViewUsuarioTrilhaAtividadeFormativaParticipacao viewUsuarioTrilhaAtividadeFormativaParticipacao = new ViewUsuarioTrilhaAtividadeFormativaParticipacao();

            //Trilha
            if ((ddlTrilha.SelectedItem != null) && (!string.IsNullOrWhiteSpace(ddlTrilha.SelectedItem.Value)))
            {
                var idTrilha = int.Parse(this.ddlTrilha.SelectedItem.Value);
                if(idTrilha != 0)viewUsuarioTrilhaAtividadeFormativaParticipacao.TrilhaOrigem = new Trilha() { ID = idTrilha };
            }

            //Trilha Nível
            if ((ddlTrilhaNivel.SelectedItem != null) && (!string.IsNullOrWhiteSpace(ddlTrilhaNivel.SelectedItem.Value)))
            {
                var idTrilhaNivel = int.Parse(this.ddlTrilhaNivel.SelectedItem.Value);
                if (idTrilhaNivel != 0) viewUsuarioTrilhaAtividadeFormativaParticipacao.TrilhaNivelOrigem = new classes.TrilhaNivel() { ID = idTrilhaNivel };
            }

            //Tópico Temático
            if ((ddlTopicoTematico.SelectedItem != null) && (!string.IsNullOrWhiteSpace(ddlTopicoTematico.SelectedItem.Value)))
            {
                var idTopicoTematico = int.Parse(this.ddlTopicoTematico.SelectedItem.Value);
                if (idTopicoTematico != 0) viewUsuarioTrilhaAtividadeFormativaParticipacao.TopicoTematico = new TrilhaTopicoTematico() { ID = idTopicoTematico };
            }

            //Aluno
            if ((ddlNomeAluno.SelectedItem != null) && (!string.IsNullOrWhiteSpace(ddlNomeAluno.SelectedItem.Value)))
            {
                var idAluno = int.Parse(this.ddlNomeAluno.SelectedItem.Value);
                if (idAluno != 0) viewUsuarioTrilhaAtividadeFormativaParticipacao.UsuarioOrigem = new Usuario{ ID = idAluno };
            }
            return viewUsuarioTrilhaAtividadeFormativaParticipacao;
        }

        private ViewUsuarioItemTrilhaParticipacao ObterObjetoViewUsuarioItemTrilhaParticipacao()
        {
            ViewUsuarioItemTrilhaParticipacao viewUsuarioItemTrilhaParticipacao = new ViewUsuarioItemTrilhaParticipacao();

            //Trilha
            if (!string.IsNullOrWhiteSpace(ddlTrilha.SelectedItem.Value))
            {
                viewUsuarioItemTrilhaParticipacao.TrilhaOrigem = new Trilha() { ID = int.Parse(this.ddlTrilha.SelectedItem.Value) };
            }

            //Trilha Nível
            if ((ddlTrilhaNivel.SelectedItem != null) && (!string.IsNullOrWhiteSpace(ddlTrilhaNivel.SelectedItem.Value)))
            {
                viewUsuarioItemTrilhaParticipacao.TrilhaNivelOrigem = new classes.TrilhaNivel() { ID = int.Parse(this.ddlTrilhaNivel.SelectedItem.Value) };
            }

            //Tópico Temático
            if ((ddlTopicoTematico.SelectedItem != null) && (!string.IsNullOrWhiteSpace(ddlTopicoTematico.SelectedItem.Value)))
            {
                viewUsuarioItemTrilhaParticipacao.TopicoTematico = new TrilhaTopicoTematico() { ID = int.Parse(this.ddlTopicoTematico.SelectedItem.Value) };
            }

            //Aluno
            if ((ddlNomeAluno.SelectedItem != null) && (!string.IsNullOrWhiteSpace(ddlNomeAluno.SelectedItem.Value)))
            {
                viewUsuarioItemTrilhaParticipacao.UsuarioOrigem = new Usuario()
                {
                    ID = int.Parse(this.ddlNomeAluno.SelectedItem.Value)
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
            try
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
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }

        }


        protected void btnPesquisar_Click(object sender, EventArgs e)
        {

            try
            {
                ViewUsuarioTrilhaAtividadeFormativaParticipacao viewUsuarioTrilhaAtividadeFormativaParticipacao = ObterObjetoViewUsuarioTrilhaAtividadeFormativaParticipacao();
                ManterTrilhaAtividadeFormativaParticipacao manterTrilhaAtividadeFormativaParticipacao = new ManterTrilhaAtividadeFormativaParticipacao();
                IList<ViewUsuarioTrilhaAtividadeFormativaParticipacao> ListaViewUsuarioTrilhaAtividadeFormativaParticipacao = manterTrilhaAtividadeFormativaParticipacao.ObterViewUsuarioTrilhaAtividadeFormativaParticipacaoPorFiltro(viewUsuarioTrilhaAtividadeFormativaParticipacao);
                WebFormHelper.PreencherGrid(ListaViewUsuarioTrilhaAtividadeFormativaParticipacao, this.dgvAtividadeFormativaParticipacao);

                if (ListaViewUsuarioTrilhaAtividadeFormativaParticipacao != null && ListaViewUsuarioTrilhaAtividadeFormativaParticipacao.Count > 0)
                {
                    WebFormHelper.PreencherGrid(ListaViewUsuarioTrilhaAtividadeFormativaParticipacao, this.dgvAtividadeFormativaParticipacao);
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

        protected void dgvAtividadeFormativaParticipacao_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                ManterTrilhaAtividadeFormativaParticipacao manterTrilhaAtividadeFormativaParticipacao = new ManterTrilhaAtividadeFormativaParticipacao();
                int idTrilhaAtividadeFormativaParticipacao = int.Parse(e.CommandArgument.ToString());
                manterTrilhaAtividadeFormativaParticipacao.ExcluirAtividadeFormativaParticipacao(idTrilhaAtividadeFormativaParticipacao);
                Response.Redirect("ListarAtividadeFormativaParticipacao.aspx");
            }
            else if (e.CommandName.Equals("editar"))
            {
                int idMatriculaTrilha = int.Parse(e.CommandArgument.ToString());
                //Session.Add("AtividadeFormativaParticipacaoEdit", idMatriculaTrilha);
                Response.Redirect("EdicaoAtividadeFormativaParticipacao.aspx?Id=" + idMatriculaTrilha.ToString(), false);
            }
            else if (e.CommandName.Equals("cadastrar"))
            {
                string idsconcatenados = (string)e.CommandArgument;
                //string[] arrayDeIDs = idsconcatenados.Split('&');
                //Session.Add("arrayDeIds", arrayDeIDs);
                Response.Redirect("EdicaoAtividadeFormativaParticipacao.aspx?IdConcatenado=" + idsconcatenados.Replace('&', '|'), false);
            }
        }
        
        protected void dgvAtividadeFormativaParticipacao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                ViewUsuarioTrilhaAtividadeFormativaParticipacao viewUsuarioTrilhaAtividadeFormativaParticipacao = (ViewUsuarioTrilhaAtividadeFormativaParticipacao)e.Row.DataItem;

                if (viewUsuarioTrilhaAtividadeFormativaParticipacao != null)
                {

                    LinkButton lkbCadastrar = (LinkButton)e.Row.Cells[4].FindControl("lkbCadastrar"); 
                    LinkButton lkbEditar = (LinkButton)e.Row.Cells[4].FindControl("lkbEditar");
                    LinkButton lkbExcluir = (LinkButton)e.Row.Cells[4].FindControl("lkbExcluir"); 
                    
                    //Exibe o botão cadastrar, caso o usuário não participe de um item trilha
                    if (viewUsuarioTrilhaAtividadeFormativaParticipacao.TemParticipacao.Trim().ToUpper().Equals("N"))
                    {
                        lkbCadastrar.Visible = true;

                        //Esconde o botão Editar quando o usuário não possuir participação na atividade formativa
                        lkbEditar.Visible = false;

                        //Esconde o botão Excluir quando o usuário não possuir participação na atividade formativa
                        lkbExcluir.Visible = false;
                    }
                    else if (viewUsuarioTrilhaAtividadeFormativaParticipacao.TemParticipacao.Trim().ToUpper().Equals("S"))
                    {
                        lkbCadastrar.Visible = false;

                        //Exibe o botão Editar quando o usuário possuir participação na atividade formativa
                        lkbEditar.Visible = true;
                        
                        lkbExcluir.Visible = true;
                    }
                }
            }
        }
    }
}
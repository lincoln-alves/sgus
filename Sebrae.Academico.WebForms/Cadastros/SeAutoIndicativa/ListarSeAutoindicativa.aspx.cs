using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BM.Views;
using Sebrae.Academico.BM.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.SeAutoIndicativa
{
    public partial class ListarSeAutoindicativa : PageBase
    {
        protected override enumFuncionalidade Funcionalidade
        {
            get
            {
                return enumFuncionalidade.Trilha;
            }
        }

        protected override IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get
            {
                return new List<enumPerfil>();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack){
                WebFormHelper.PreencherLista(new BMTrilha().ObterTrilhas(), ddlTrilha, false, true);
                pnlSolucoesEducacionaisSugeridasAprovacao.Visible = false;
            }
        }

        protected void ddlTrilha_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTrilha.SelectedIndex > 0)
            {
                var idTrilha = int.Parse(ddlTrilha.SelectedValue);

                ddlTopicoTematico.Items.Clear();

                WebFormHelper.PreencherLista(new ManterTrilhaNivel().ObterPorTrilha(new ManterTrilha().ObterTrilhaPorId(idTrilha)).OrderBy(x => x.Nome).ToList(), ddlTrilhaNivel, true);
            }
        }

        protected void ddlTrilhaNivel_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTrilha.SelectedIndex <= 0 || ddlTrilhaNivel.SelectedIndex <= 0) return;

            int idTrilha = int.Parse(ddlTrilha.SelectedValue), idTrilhaNivel = int.Parse(ddlTrilhaNivel.SelectedValue);

            var viewTrilhaFiltro = new ViewTrilha
            {
                TrilhaOrigem = idTrilha == 0 ? null : (new BMTrilha()).ObterPorId(idTrilha),
                TrilhaNivelOrigem = idTrilhaNivel == 0 ? null : (new BMTrilhaNivel()).ObterPorID(idTrilhaNivel),
            };

            var lstView =
                new BMViewTrilha().ObterViewTrilhaPorFiltro(viewTrilhaFiltro)
                    .OrderBy(x => x.TrilhaOrigem.ID)
                    .ThenBy(x => x.TrilhaNivelOrigem.ID)
                    .ThenBy(x => x.TopicoTematico.ID);

            var lstviewTopicos =
                lstView.Where(
                    x =>
                        x.TrilhaOrigem.ID == idTrilha && x.TrilhaNivelOrigem.ID == idTrilhaNivel &&
                        x.UsuarioOrigem == null).Select(x => x.TopicoTematico).Distinct();

            var listaTopicos =
                lstviewTopicos.Select(x => new TrilhaTopicoTematico { ID = x.ID, Nome = x.NomeExibicao });

            WebFormHelper.PreencherLista(listaTopicos.ToList(), ddlTopicoTematico, true);
        }

        protected void ddlTopicoTematico_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var lista = ObterListaItemTrilha();
        }        

        protected void gvSolucoesEducacionaisSugeridasAprovacao_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            //...
        }

        protected void gvSolucoesEducacionaisSugeridasAprovacao_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ValidarRowCommandPorSolucaoEducacionalAprovacao(sender, e);
        }

        protected void ValidarRowCommandPorSolucaoEducacionalAprovacao(object sender, GridViewCommandEventArgs e){
            if (e.CommandName == "Page") return;
            if (e.CommandName.Equals("excluir")){
                var idItemTrilha = int.Parse(e.CommandArgument.ToString());
                try {
                    var manterItemTrilha = new ManterItemTrilha();
                    var manterUsuario = new ManterUsuario();
                    var usuarioLogado = manterUsuario.ObterUsuarioLogado();
                    manterItemTrilha.ExcluirSolucaoEducacionalAutoIndicativa(idItemTrilha, usuarioLogado.CPF, usuarioLogado);
                    PreencherSolucoesEducacionaisSugeridas();
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!");
                }catch(AcademicoException ex){
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }catch(Exception ex){
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
                return;
            }
            if (e.CommandName.Equals("editar")){
                Response.Redirect("~/Cadastros/SeAutoindicativa/EdicaoSeAutoindicativa.aspx?id=" + e.CommandArgument.ToString());
                return;
            }
        }

        protected void gvSolucoesEducacionaisSugeridasAprovacao_PageIndexChanging(object sender, GridViewPageEventArgs e){
            PreencherSolucoesEducacionaisSugeridas(e.NewPageIndex);
        }

        private void PreencherSolucoesEducacionaisSugeridas(int page = 0){
            var lista = ObterListaItemTrilha();

            WebFormHelper.PaginarGrid(lista.OrderByDescending(x => x.DataCriacao).ToList(), gvSolucoesEducacionaisSugeridasAprovacao, page);
            pnlSolucoesEducacionaisSugeridasAprovacao.Visible = true;
        }

        protected void btnPesquisar_Click(object sender, EventArgs e){
            PreencherSolucoesEducacionaisSugeridas();
        }

        private IEnumerable<ItemTrilha> ObterListaItemTrilha(){
            var itemTrilha = new ItemTrilha { };

            if (!string.IsNullOrEmpty(ddlTrilha.SelectedValue)){
                if (itemTrilha.Missao.PontoSebrae.TrilhaNivel == null) itemTrilha.Missao.PontoSebrae.TrilhaNivel = new classes.TrilhaNivel();
                itemTrilha.Missao.PontoSebrae.TrilhaNivel.Trilha = new Trilha { ID = Convert.ToInt32(ddlTrilha.SelectedValue) };
            }

            if (!string.IsNullOrEmpty(ddlTrilhaNivel.SelectedValue)){
                if (itemTrilha.Missao.PontoSebrae.TrilhaNivel == null) itemTrilha.Missao.PontoSebrae.TrilhaNivel = new classes.TrilhaNivel();
                itemTrilha.Missao.PontoSebrae.TrilhaNivel.ID = Convert.ToInt32(ddlTrilhaNivel.SelectedValue);
            }

            if (!string.IsNullOrEmpty(ddlTopicoTematico.SelectedValue)){
                if (itemTrilha.TrilhaTopicoTematico == null) itemTrilha.TrilhaTopicoTematico = new TrilhaTopicoTematico();
                itemTrilha.TrilhaTopicoTematico.ID = Convert.ToInt32(ddlTopicoTematico.SelectedValue);
            }

            var lista = new ManterItemTrilha().ObterItemTrilhaPorFiltro(itemTrilha);
            return lista;
        }
    }
}
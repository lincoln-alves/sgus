using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.PontoSebrae
{
    public partial class EdicaoPontoSebrae : Page
    {
        private readonly ManterPontoSebrae _manterPontoSebrae = new ManterPontoSebrae();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PreencherCombos();

                if (Request["Id"] != null)
                {
                    WebFormHelper.PreencherLista(new ManterTrilhaNivel().ObterTodosTrilhaNivel(), ddlTrilhaNivel, false,
                        true);

                    var pontoSebrae = _manterPontoSebrae.ObterPorId(int.Parse(Request["Id"]));
                    PreencherCampos(pontoSebrae);
                }
            }
        }

        private void PreencherCombos()
        {
            WebFormHelper.PreencherLista(new ManterTrilha().ObterTodasTrilhas(), ddlTrilha, false, true);
            WebFormHelper.PreencherComponenteComOpcoesSimNao(ddlAtivo, true);
        }

        private void PreencherCampos(classes.PontoSebrae pontoSebrae)
        {
            WebFormHelper.SetarValorNaCombo(pontoSebrae.TrilhaNivel.Trilha.ID.ToString(), ddlTrilha, true);
            WebFormHelper.SetarValorNaCombo(pontoSebrae.TrilhaNivel.ID.ToString(), ddlTrilhaNivel, true);

            txtNome.Text = pontoSebrae.Nome;
            txtNomeExibicao.Text = pontoSebrae.NomeExibicao;
            txtQtMinimaPontos.Text = pontoSebrae.QtMinimaPontos.ToString();

            WebFormHelper.PreencherComponenteComOpcoesSimNao(ddlAtivo, true, pontoSebrae.Ativo);
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    ValidarCampos();

                    var pontoSebrae = ObterObjetoPontoSebrae();

                    if (Request["Id"] == null)
                    {
                        _manterPontoSebrae.IncluirPontoSebrae(pontoSebrae);
                    }
                    else
                    {
                        _manterPontoSebrae.AlterarPontoSebrae(pontoSebrae);
                    }

                    if (!pontoSebrae.Ativo)
                    {
                        AtualizarParticipacoesPontoSebrae();
                    }

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso!",
                        "ListarPontoSebrae.aspx");

                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }

        private void ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
                throw new AcademicoException("O campo Nome é obrigatório");

            if (string.IsNullOrWhiteSpace(txtNomeExibicao.Text))
                throw new AcademicoException("O campo Nome Exibição é obrigatório");

            int qt;
            if (string.IsNullOrWhiteSpace(txtQtMinimaPontos.Text) == false && int.TryParse(txtQtMinimaPontos.Text, out qt) == false)
                throw new AcademicoException("Valor inválido para o campo Qt. Mínima de Pontos");

            if (ddlTrilhaNivel.SelectedIndex <= 0)
                throw new AcademicoException("O campo Nível é obrigatório");
        }

        private classes.PontoSebrae ObterObjetoPontoSebrae()
        {
            var pontoSebrae = Request["Id"] != null
                ? _manterPontoSebrae.ObterPorId(int.Parse(Request["Id"]))
                : new classes.PontoSebrae();

            pontoSebrae.TrilhaNivel = new ManterTrilhaNivel().ObterTrilhaNivelPorID(int.Parse(ddlTrilhaNivel.SelectedValue));

            pontoSebrae.Nome = txtNome.Text.Trim();
            pontoSebrae.NomeExibicao = txtNomeExibicao.Text.Trim();

            if (string.IsNullOrWhiteSpace(txtQtMinimaPontos.Text))
            {
                pontoSebrae.QtMinimaPontos = null;
            }
            else
            {
                int qt;
                if (int.TryParse(txtQtMinimaPontos.Text, out qt))
                    pontoSebrae.QtMinimaPontos = qt;
            }

            pontoSebrae.Ativo = ddlAtivo.SelectedValue == "S";

            return pontoSebrae;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListarPontoSebrae.aspx");
        }

        protected void ddlTrilha_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var trilhaId = int.Parse(ddlTrilha.SelectedValue);

            WebFormHelper.PreencherLista(new ManterTrilhaNivel().ObterPorTrilha(new classes.Trilha { ID = trilhaId }),
                ddlTrilhaNivel, false, true);
        }

        protected void ddlAtivo_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAtivo.SelectedValue == "N" && Request["Id"] != null)
            {
                int id;
                if (int.TryParse(Request["Id"], out id))
                {
                    var pontoSebrae = new ManterPontoSebrae().ObterPorId(id);

                    var itensTrilha = pontoSebrae?.ObterItensTrilha().Where(x => x.Missao != null && x.Usuario == null).ToList();

                    if (itensTrilha != null && itensTrilha.Any())
                    {
                        // Informando total de itens
                        totalItemTrilha.Text = itensTrilha.Count.ToString();
                        rptItemTrilha.DataSource = itensTrilha;
                        rptItemTrilha.DataBind();

                        ExibirModal();
                    }
                }
            }
        }

        private void ExibirModal()
        {
            pnlModal.Visible = true;
        }

        private void OcultarModal()
        {
            pnlModal.Visible = false;
        }

        protected void btnCancelarAlteracao_OnClick(object sender, EventArgs e)
        {
            ddlAtivo.SelectedValue = "S";
            OcultarModal();
        }

        protected void btnAutorizarAlteracao_OnClick(object sender, EventArgs e)
        {
            OcultarModal();
        }

        protected void AtualizarParticipacoesPontoSebrae()
        {
            var ponto = new ManterPontoSebrae().ObterPorId(int.Parse(Request["Id"]));
            var participacoes = new ManterItemTrilhaParticipacao().ObterTodosPorPontoSebrae(ponto.ID);

            var manterItemTrilhaParticipacao = new ManterItemTrilhaParticipacao();

            participacoes = manterItemTrilhaParticipacao.ObterParticipacoesPontoSebraeInativo(participacoes);
            manterItemTrilhaParticipacao.ExcluirTodosAsync(participacoes);
        }
    }
}
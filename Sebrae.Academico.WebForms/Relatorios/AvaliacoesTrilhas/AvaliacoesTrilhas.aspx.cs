using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sebrae.Academico.WebForms.Relatorios.AvaliacoesTrilhas
{
    public partial class AvaliacoesTrilhas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PreencherTrilhas();
                PreencherNiveisTrilha();
                PreencherPontosSebrae();
            }
        }

        private void PreencherPontosSebrae()
        {
            int idNivelTrilha;
            if (int.TryParse(drpNivelTrilha.SelectedValue, out idNivelTrilha))
            {
                if (idNivelTrilha <= 0)
                {
                    WebFormHelper.PreencherLista(new List<object> { new { ID = 0, Nome = "-- Selecione um Nível da Trilha --" } }, drpPontoSebrae);
                    return;
                }

                List<PontoSebrae> pontosSebrae = new ManterPontoSebrae().ObterPorTrilhaNivel(idNivelTrilha).ToList();
                WebFormHelper.PreencherLista(pontosSebrae, drpPontoSebrae, true, false);
            }
        }

        private void PreencherNiveisTrilha()
        {
            int idTrilha;
            if (int.TryParse(drpTrilhas.SelectedValue, out idTrilha))
            {
                if (idTrilha <= 0)
                {
                    WebFormHelper.PreencherLista(new List<object> { new { ID = 0, Nome = "-- Selecione uma Trilha --" } }, drpNivelTrilha);
                    return;
                }

                List<TrilhaNivel> trilhaNivel = new ManterTrilhaNivel().ObterPorTrilha(idTrilha).ToList();
                WebFormHelper.PreencherLista(trilhaNivel, drpNivelTrilha, true, false);
            }
        }

        private void PreencherTrilhas()
        {
            var trilhas = new ManterTrilha().ObterTodasTrilhas();
            WebFormHelper.PreencherLista(trilhas, drpTrilhas, true, false);
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            IQueryable<ItemTrilhaAvaliacao> avaliacoes = new ManterItemTrilhaAvaliacao().ObterTodosIQueryable();

            int idTrilha;
            if (int.TryParse(drpTrilhas.SelectedValue, out idTrilha))
            {
                if (idTrilha > 0)
                {
                    avaliacoes = avaliacoes.Where(x => x.ItemTrilha.Missao.PontoSebrae.TrilhaNivel.Trilha.ID == idTrilha);
                }
            }

            int idNivel;
            if (int.TryParse(drpNivelTrilha.SelectedValue, out idNivel))
            {
                if (idNivel > 0)
                {
                    avaliacoes = avaliacoes.Where(x => x.ItemTrilha.Missao.PontoSebrae.TrilhaNivel.ID == idNivel);
                }
            }

            int idPontoSebrae;
            if (int.TryParse(drpPontoSebrae.SelectedValue, out idPontoSebrae))
            {
                if (idPontoSebrae > 0)
                {
                    avaliacoes = avaliacoes.Where(x => x.ItemTrilha.Missao.PontoSebrae.ID == idPontoSebrae);
                }
            }

            if (luUsuario.SelectedUser != null)
            {
                avaliacoes = avaliacoes.Where(x => x.UsuarioTrilha.Usuario.ID == luUsuario.SelectedUser.ID);
            }

            var dtoAvaliacoes = avaliacoes.Select(x => new
            {
                Trilha = x.ItemTrilha.Missao.PontoSebrae.TrilhaNivel.Trilha.Nome,
                PontoSebrae = x.ItemTrilha.Missao.PontoSebrae.Nome,
                SolucaoEducacional = x.ItemTrilha.Nome,
                Avaliacao = x.Avaliacao + " Estrelas",
                Resenha = x.Resenha,
                CPF = x.UsuarioTrilha.Usuario.CPF,
                NomeUsuario = x.UsuarioTrilha.Usuario.Nome
            }).ToList();

            WebFormHelper.PreencherGrid(dtoAvaliacoes, dgRelatorio);
        }

        protected void drpTrilhas_SelectedIndexChanged(object sender, EventArgs e)
        {
            PreencherNiveisTrilha();
        }

        protected void drpNivelTrilha_SelectedIndexChanged(object sender, EventArgs e)
        {
            PreencherPontosSebrae();
        }

        protected void btnLimparUsuario_Click(object sender, EventArgs e)
        {
            luUsuario.LimparCampos();
        }
    }
}
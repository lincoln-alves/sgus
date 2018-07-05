using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Web.UI.WebControls;
using Sebrae.Academico.Dominio.Classes.Moodle;
using Sebrae.Academico.BM.Classes.Moodle;

namespace Sebrae.Academico.WebForms.Cadastros{
    public partial class EdicaoSeAutoindicativa : Page{
        protected void Page_Load(object sender, EventArgs e){
            if (Page.IsPostBack) return;
            if (Request["Id"] == null){
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Solução Educacional Autoindicativa não encontrada!", "ListarSeAutoindicativa.aspx");
                return;
            }
            int idItemTrilha;
            if (!int.TryParse(Request["Id"], out idItemTrilha)){
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Solução Educacional Autoindicativa não encontrada!", "ListarSeAutoindicativa.aspx");
                return;
            }
            PreencherCampos(idItemTrilha);
        }

        void PreencherCampos(int idItemTrilha){
            var manterItemTrilha = new ManterItemTrilha();
            var itemTrilha = manterItemTrilha.ObterItemTrilhaPorID(idItemTrilha);
            if(itemTrilha == null){
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Solução Educacional Autoindicativa não encontrada!", "ListarSeAutoindicativa.aspx");
                return;
            }

            lblTipoItemTrilha.Text = itemTrilha.FormaAquisicao.Nome;
            txtTituloItemTrilha.Text = itemTrilha.Nome;
            lblObjetivoItemTrilha.Text = itemTrilha.Objetivo.NomeExibicao;
            txtLinkAcessoItemTrilha.Text = itemTrilha.LinkConteudo;
            txtReferenciaBibliograficaItemTrilha.Text = itemTrilha.ReferenciaBibliografica;
            txtLocalItemTrilha.Text = itemTrilha.Local;
        }

        protected void btnAprovar_OnClick(object sender, EventArgs e){
            AtualizarItemTrilhaAprovacao(enumStatusSolucaoEducacionalSugerida.Aprovado);
        }

        protected void btnReprovar_OnClick(object sender, EventArgs e){
            AtualizarItemTrilhaAprovacao(enumStatusSolucaoEducacionalSugerida.NaoAprovado);
        }

        void AtualizarItemTrilhaAprovacao(enumStatusSolucaoEducacionalSugerida status){
            int idItemTrilha;
            if (!int.TryParse(Request["Id"], out idItemTrilha)){
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Solução Educacional Autoindicativa não encontrada!", "ListarSeAutoindicativa.aspx");
                return;
            }
            var manterItemTrilha = (new ManterItemTrilha());
            var itemTrilha = manterItemTrilha.ObterItemTrilhaPorID(idItemTrilha);

            itemTrilha.Aprovado = status;
            itemTrilha.Nome = txtTituloItemTrilha.Text;
            itemTrilha.LinkConteudo = txtLinkAcessoItemTrilha.Text;
            itemTrilha.ReferenciaBibliografica = txtReferenciaBibliograficaItemTrilha.Text;
            itemTrilha.Local = txtLocalItemTrilha.Text;

            manterItemTrilha.AlterarItemTrilha(itemTrilha);
            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarSeAutoindicativa.aspx");
            return;
        }
    }
}
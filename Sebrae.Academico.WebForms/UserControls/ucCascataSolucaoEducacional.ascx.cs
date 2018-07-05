using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucCascataSolucaoEducacional : System.Web.UI.UserControl
    {

        public enum enumAcaoDaTela
        {
            EdicaoDeUmaOferta = 1
        }

        /// <summary>
        /// ID da Categoria. 
        /// </summary>
        public int? IdCategoria
        {
            get
            {
                if (ViewState["ViewStateIdCategoria"] != null)
                {
                    return (int)ViewState["ViewStateIdCategoria"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateIdCategoria"] = value;
            }

        }

        /// <summary>
        /// Id da Solução Educacional.
        /// </summary>
        public int? IdSolucao
        {
            get
            {
                if (ViewState["ViewStateIdSolucao"] != null)
                {
                    return (int)ViewState["ViewStateIdSolucao"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateIdSolucao"] = value;
            }

        }

        /// <summary>
        /// Id da Oferta.
        /// </summary>
        public int IdOferta
        {
            get
            {
                if (ViewState["ViewStateIdOferta"] != null)
                {
                    return (int)ViewState["ViewStateIdOferta"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateIdOferta"] = value;
            }

        }

        public void InformarAcaoDeEdicao()
        {
            this.spanAcao.InnerText = "Edição de Oferta";
            
        }

        public void SelecionarCategoriaOfertaSE(int categoriaId, int ofertaId, int solucaoEducacionalId)
        {
            this.IdCategoria = categoriaId;
            this.IdOferta = ofertaId;
            this.IdSolucao = solucaoEducacionalId;

            WebFormHelper.SetarValorNaCombo(categoriaId.ToString(), this.ddlCategoria, true);
            WebFormHelper.SetarValorNaCombo(ofertaId.ToString(), this.ddlOferta, true);
            WebFormHelper.SetarValorNaCombo(solucaoEducacionalId.ToString(), this.ddlSolucao, true);
        }

        public void BloquearDropDown()
        {
            this.ddlCategoria.Attributes.Add("disabled", "disabled");
            this.ddlSolucao.Attributes.Add("disabled", "disabled");
            this.ddlOferta.Attributes.Add("disabled", "disabled");
            //this.divCategoria.Visible = false;
        }

        public void DesbloquearDropDown()
        {
            this.ddlCategoria.Attributes.Remove("disabled");
            this.ddlSolucao.Attributes.Remove("disabled");
            this.ddlOferta.Attributes.Remove("disabled");
            this.divCategoria.Visible = true;
        }
        public void DesbloquearDropDownOferta()
        {
            this.ddlOferta.Attributes.Remove("disabled");
        }
        public void InformarAcaoDeConsulta()
        {
            Session["OfertaEdit"] = null;
            this.spanAcao.InnerText = "Filtro de Oferta";
        }

        public void PreencherCombos()
        {
            try
            {
                PreencherComboCategoriaSolucaoEducacional();
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        public void PreencherComboCategoriaSolucaoEducacional()
        {
            var manterCategoriaSolucaoEducacional = new ManterCategoriaConteudo();

            var listaCategoriaSolucaoEducacionalo = manterCategoriaSolucaoEducacional.ObterTodasCategoriasConteudo();

            WebFormHelper.PreencherLista(listaCategoriaSolucaoEducacionalo, ddlCategoria, false, true);
        }

        internal void LimparCampos()
        {
            //Tipo de Oferta
            ddlCategoria.ClearSelection();

            //Solução Educacional
            ddlSolucao.ClearSelection();

            //Oferta
            ddlOferta.ClearSelection();

        }

        protected void ddlCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCategoria.SelectedItem != null && !string.IsNullOrWhiteSpace(ddlCategoria.SelectedItem.Value))
            {
                PreencherComboSolucaoEducacional(int.Parse(ddlCategoria.SelectedItem.Value));
            }

        }

        public void PreencherComboSolucaoEducacional(int idSolucaoEducacional)
        {
            this.ObterSolucoesEducacionaisDaCategoria(idSolucaoEducacional);
            this.ddlOferta.Items.Clear();
        }

        internal void PreencherComboSolucaoEducacional(SolucaoEducacional solucaoEducacional, bool desabilitaCombo = false)
        {
            this.ObterSolucoesEducacionaisDaCategoria(solucaoEducacional.CategoriaConteudo.ID);

            //Seta o valor na combo de Solucao Educacional
            WebFormHelper.SetarValorNaCombo(solucaoEducacional.ID.ToString(), this.ddlSolucao, true);
            this.ddlOferta.Items.Clear();
        }

        private void ObterSolucoesEducacionaisDaCategoria(int idCategoriaSolucaoEducacional)
        {
            ManterSolucaoEducacional manterSolucaoEducacional = new ManterSolucaoEducacional();
            IList<SolucaoEducacional> ListaSolucaoEducacional = manterSolucaoEducacional.ObterListaSolucaoEducacionalPorCategoria(idCategoriaSolucaoEducacional);
            ListaSolucaoEducacional = ListaSolucaoEducacional.OrderBy(x => x.Nome).ToList();

            WebFormHelper.PreencherLista(ListaSolucaoEducacional, this.ddlSolucao, false, false);
        }

        /// <summary>
        /// Id da Oferta Selecionada na combo ddlOferta.
        /// </summary>
        public int IdOfertaDaCombo
        {
            get
            {
                int idOferta = 0;

                if (this.ddlOferta.SelectedItem != null && !string.IsNullOrWhiteSpace(this.ddlOferta.SelectedItem.Value))
                {
                    idOferta = int.Parse(this.ddlOferta.SelectedItem.Value);
                }

                return idOferta;
            }
        }

        protected void ddlSolucao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlSolucao.SelectedItem != null
                && !string.IsNullOrWhiteSpace(this.ddlSolucao.SelectedItem.Value))
            {
                int idSolucaoEducacional = int.Parse(this.ddlSolucao.SelectedItem.Value);
                ObterInformacoesDaOfertaPorSolucaoEducacional(idSolucaoEducacional);

            }
        }

        private void ObterInformacoesDaOfertaPorSolucaoEducacional(int idSolucaoEducacional)
        {
            var solucaoEducacional = new SolucaoEducacional() { ID = idSolucaoEducacional };

            var listaOferta = new ManterOferta().ObterOfertaPorSolucaoEducacional(solucaoEducacional);

            WebFormHelper.PreencherLista(listaOferta, ddlOferta, false, true);
        }

        public void PreencherInformacoesDaSolucaoEducacional(Turma turma)
        {
            if (turma != null)
            {
                PreencherCombos();
                LimparCampos();

                try
                {
                    WebFormHelper.SetarValorNaCombo(turma.Oferta.SolucaoEducacional.CategoriaConteudo.ID.ToString(), this.ddlCategoria);
                    this.ObterSolucoesEducacionaisDaCategoria(turma.Oferta.SolucaoEducacional.CategoriaConteudo.ID);
                }
                catch { }
                WebFormHelper.SetarValorNaCombo(turma.Oferta.SolucaoEducacional.ID.ToString(), this.ddlSolucao);

                this.ObterInformacoesDaOfertaPorSolucaoEducacional(turma.Oferta.SolucaoEducacional.ID);
                WebFormHelper.SetarValorNaCombo(turma.Oferta.ID.ToString(), this.ddlOferta);
            }

        }

        internal void PreencherComboCategoriaSolucaoEducacional(SolucaoEducacional solucaoEducacional)
        {
            this.PreencherCombos();
            if (ddlCategoria.SelectedIndex > 0)
                WebFormHelper.SetarValorNaCombo(solucaoEducacional.CategoriaConteudo.ID.ToString(), this.ddlCategoria, true);
        }

        internal void PreencherComboOferta(SolucaoEducacional solucaoEducacional)
        {
            this.ObterInformacoesDaOfertaPorSolucaoEducacional(solucaoEducacional.ID);
            //WebFormHelper.SetarValorNaCombo(turma.Oferta.ID.ToString(), this.ddlOferta);
            WebFormHelper.SetarValorNaCombo(this.IdOferta.ToString(), this.ddlOferta, true);
        }

    }

}
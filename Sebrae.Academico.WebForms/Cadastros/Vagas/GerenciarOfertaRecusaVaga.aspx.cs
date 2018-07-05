using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.Vagas
{
    public partial class GerenciarOfertaRecusaVaga : PageBase
    {
        BMUsuario bmUsuario = new BMUsuario();

        protected override enumFuncionalidade Funcionalidade
        {
            get
            {
                return enumFuncionalidade.GerenciamentVagasGestor;
            }
        }

        protected override IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get
            {
                IList<enumPerfil> perfisNecessariosParaAcessarAFuncionalidade = new List<enumPerfil>();
                perfisNecessariosParaAcessarAFuncionalidade.Add(enumPerfil.Administrador);
                return perfisNecessariosParaAcessarAFuncionalidade;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ObterSolicitacoes(rblExibir.SelectedValue);
            }
        }

        private void ObterSolicitacoes(string solicitacao)
        {
            IList<OfertaGerenciadorVaga> listaOfertaGerenciadorVaga = null;

            if (!bmUsuario.PerfilAdministrador())
            {
                listaOfertaGerenciadorVaga = new BMOfertaGerenciadorVaga().ObterSolicitacoes(bmUsuario.ObterUsuarioLogado(), solicitacao);
            }
            else
            {
                listaOfertaGerenciadorVaga = new BMOfertaGerenciadorVaga().ObterSolicitacoes(null, solicitacao);
            }

            if (listaOfertaGerenciadorVaga != null)
                WebFormHelper.PreencherGrid(listaOfertaGerenciadorVaga, gvOferta);
        }
        
        protected void rblExibir_SelectedIndexChanged(object sender, EventArgs e)
        {
            ObterSolicitacoes(rblExibir.SelectedValue);
        }

        protected void gvOferta_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                
            }
        }

        protected void gvOferta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idGerenciadorVagas = int.Parse(e.CommandArgument.ToString());
            this.ObterObjetoGerenciadorVagas(idGerenciadorVagas);
            base.ExibirBackDrop();
            pnlLupaOfertaHistoricoVagas.Visible = true;
        }

        private void ObterObjetoGerenciadorVagas(int idOferta)
        {
            OfertaGerenciadorVaga recusa = new BMOfertaGerenciadorVaga().ObterPorID(idOferta);
            txtSeModal.Text = recusa.Oferta.SolucaoEducacional.Nome;
            txtOfertaModal.Text = recusa.Oferta.Nome;
            txtUfModal.Text = recusa.UF.Sigla;
            txtQtdeVagasUfModal.Text = recusa.VagasAnteriores.ToString();
            txtQtdeVagasRecusadas.Text = recusa.VagasRecusadas.ToString();
            txtObservacaoRecusa.Text = recusa.Descricao;
            rblAprovar.SelectedValue = recusa.Contemplado.HasValue && recusa.Contemplado.Value ? "Aprovar" : "Recusar";
            txtResposta.Text = recusa.Resposta;

            if (!bmUsuario.PerfilAdministrador())
            {
                rblAprovar.Enabled =
                txtResposta.Enabled = 
                btnEnviarObservacao.Visible = false;
            }

            OfertaGerenciadorVagaEmAlteracao = recusa;
        }

        protected void OcultarModal_Click(object sender, EventArgs e)
        {
            pnlLupaOfertaHistoricoVagas.Visible = false;
            OfertaGerenciadorVagaEmAlteracao = null;
            base.OcultarBackDrop();
        }

        protected void gvOfertaGerenciadorVaga_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvOfertaGerenciadorVaga_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                
            }
        }

        protected void btnEnviarObservacao_OnClick(object sender, EventArgs e)
        {
            bool aprovar = rblAprovar.SelectedValue.Contains("Aprovar");
            BMOfertaGerenciadorVaga bmOfertaGerenciadorVaga = new BMOfertaGerenciadorVaga();
            OfertaGerenciadorVaga dados = null;

            if (aprovar)
            {
                dados = bmOfertaGerenciadorVaga.ObterPorID(OfertaGerenciadorVagaEmAlteracao.ID);
                dados.Contemplado = true;
                dados.Resposta = txtResposta.Text;
            }
            else
            {
                if (string.IsNullOrEmpty(txtResposta.Text))
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Para cancelar a recusa você deve informar um motivo");
                }
                else
                {
                    dados = bmOfertaGerenciadorVaga.ObterPorID(OfertaGerenciadorVagaEmAlteracao.ID);
                    dados.Contemplado = false;
                    dados.Vigente = false;
                    dados.Resposta = txtResposta.Text;

                    var oferta = new BMOferta().ObterPorId(dados.Oferta.ID);
                    var permissaoUF = oferta.ListaPermissao.FirstOrDefault(x => x.Uf != null && x.Uf == dados.UF);
                    if (permissaoUF != null)
                    {
                        permissaoUF.QuantidadeVagasPorEstado = dados.VagasAnteriores;
                        new BMOfertaPermissao().Salvar(permissaoUF);
                    }
                }
            }

            if (dados != null)
            {
                dados.Auditoria = new Auditoria(bmUsuario.ObterUsuarioLogado().CPF);
                bmOfertaGerenciadorVaga.Alterar(dados);
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados alterados com sucesso", "GerenciarOfertaRecusaVaga.aspx");
            }
        }

        protected OfertaGerenciadorVaga OfertaGerenciadorVagaEmAlteracao
        {
            get
            {
                if (Session["_OfertaGerenciadorVaga"] != null)
                {
                    return (OfertaGerenciadorVaga)Session["_OfertaGerenciadorVaga"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Session["_OfertaGerenciadorVaga"] = value;
            }
        }

        protected void cbMarcarComoVisto_OnCheckedChanged(object sender, EventArgs e)
        {
            BMOfertaGerenciadorVaga bmOfertaGerenciadorVaga = new BMOfertaGerenciadorVaga();
            CheckBox cbMarcarComoVisto = (CheckBox)sender;
            var ogv = new BMOfertaGerenciadorVaga().ObterPorID(int.Parse(cbMarcarComoVisto.ToolTip));
            ogv.Contemplado = cbMarcarComoVisto.Checked;
            bmOfertaGerenciadorVaga.Alterar(ogv);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;


namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class ListarConfiguracaoPagamento : PageBase
    {
        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvConfigPagamento.Rows.Count > 0)
            {
                this.dgvConfigPagamento.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    base.LogarAcessoFuncionalidade();

                    IList<TipoPagamento> lstTipoPagamento;

                    using (ManterConfiguracaoPagamento manterCP = new ManterConfiguracaoPagamento())
                    {
                        lstTipoPagamento = manterCP.ObterListaTipoPagamento();
                    }

                    WebFormHelper.PreencherLista(lstTipoPagamento.OrderBy(x => x.Nome).ToList(), cbxTipoPagamento, true, false);

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
            get { return enumFuncionalidade.ConfiguracaoPagamento; }
        }

        protected void btnPesquisa_Click(object sender, EventArgs e)
        {
            IList<Dominio.Classes.ConfiguracaoPagamento> lstConfigPag;

            using (ManterConfiguracaoPagamento manterCP = new ManterConfiguracaoPagamento())
            {
                lstConfigPag = manterCP.PesquisaConfiguracaoPagamento(int.Parse(string.IsNullOrWhiteSpace(cbxTipoPagamento.SelectedValue) ? "0" : cbxTipoPagamento.SelectedValue));
            }

            WebFormHelper.PreencherGrid(lstConfigPag, dgvConfigPagamento);
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            //Session.Remove("IdConfigPagamento");
            Response.Redirect("EdicaoConfiguracaoPagamento.aspx", false);
        }

        protected void dgvConfigPagamento_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "excluir")
            {
                try
                {
                    ManterConfiguracaoPagamento manterConfiguracaoPagamento = new ManterConfiguracaoPagamento();
                    int idConfiguracaoPagamento = int.Parse(e.CommandArgument.ToString());
                    manterConfiguracaoPagamento.ExcluirConfiguracaoPagamento(idConfiguracaoPagamento);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarConfiguracaoPagamento.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
            }
            else if (e.CommandName == "editar")
            {
                Response.Redirect("EdicaoConfiguracaoPagamento.aspx?Id=" + e.CommandArgument.ToString(), false);
            }
        }
    }
}
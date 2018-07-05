using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;


namespace Sebrae.Academico.WebForms.Cadastros.Oferta
{
    public partial class ListarOferta : PageBase
    {

        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvOferta.Rows.Count > 0)
            {
                this.dgvOferta.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        private ManterOferta manterOferta = null;

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.Oferta; }
        }

        protected override System.Collections.Generic.IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    base.LogarAcessoFuncionalidade();
                    this.ObterSolucaoEducacional();
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void ObterSolucaoEducacional()
        {
            var manterSolucaoEducacional = new ManterSolucaoEducacional();
            ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(manterSolucaoEducacional.ObterTodosPorGestor());
        }

        protected void dgvOferta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    manterOferta = new ManterOferta();
                    int idTipoOferta = int.Parse(e.CommandArgument.ToString());
                    manterOferta.ExcluirOferta(idTipoOferta);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!",
                        "ListarOferta.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
                catch (Exception)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Exclusão de registro negada. Existem Registros Dependentes desta Oferta.");
                    return;
                }

            }
            else if (e.CommandName.Equals("editar"))
            {
                int idTipoOferta = int.Parse(e.CommandArgument.ToString());
                //Session.Add("OfertaEdit", idTipoOferta);
                Response.Redirect("EdicaoOferta.aspx?Id=" + idTipoOferta.ToString(), false);
            }
            else if (e.CommandName.Equals("republicar"))
            {
                int idTipoOferta = int.Parse(e.CommandArgument.ToString());
                //Session.Add("OfertaEdit", idTipoOferta);
                Response.Redirect("EdicaoOferta.aspx?Id=" + idTipoOferta.ToString() + "&Republicar=Sim", false);
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            //Session["OfertaEdit"] = null;
            Response.Redirect("EdicaoOferta.aspx");
        }

        protected void txtSolucaoEducacional_OnTextChanged(object sender, EventArgs e)
        {
            btnPesquisar_Click(null, null);
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            PreencherGridOfertas();
        }

        protected void dgvOferta_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvOferta.PageIndex = e.NewPageIndex;
            PreencherGridOfertas();
        }

        private void PreencherGridOfertas()
        {
            try
            {
                var solucaoEducacional = 0;
                if (!string.IsNullOrWhiteSpace(txtSolucaoEducacional.Text))
                {
                    int.TryParse(txtSolucaoEducacional.Text, out solucaoEducacional);
                    ValidarPesquisaPorSolucaoEducacional(idSolucao: solucaoEducacional);
                }

                manterOferta = new ManterOferta();

                // Obtém as ofertas que estão com o nome, e obtém as datas a partir das turmas vigentes.
                var listaOferta =
                    manterOferta.ObterOfertasPorGestor(solucaoEducacional).Where(s => s.Nome.Contains(txtNome.Text.Trim())).ToList();

                if (listaOferta.Any())
                {
                    WebFormHelper.PreencherGrid(listaOferta, dgvOferta);
                    pnlOferta.Visible = true;
                }
                else
                {
                    pnlOferta.Visible = false;
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma informação encontrada");
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        public void ValidarPesquisaPorSolucaoEducacional(int idSolucao)
        {
            classes.SolucaoEducacional se;
            se = new ManterSolucaoEducacional().ObterSolucaoEducacionalPorId(idSolucao);

            if (se == null)
            {
                txtSolucaoEducacional.Text = "";
                throw new AcademicoException("Selecione uma Solução Educacional Válida.");
            }
        }
    }
}
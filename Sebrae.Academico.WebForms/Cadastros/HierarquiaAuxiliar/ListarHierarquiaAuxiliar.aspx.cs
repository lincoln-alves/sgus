using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BM.Classes;
using System.Linq;

namespace Sebrae.Academico.WebForms.Cadastros.HierarquiaAuxiliar
{
    public partial class ListarHierarquiaAuxiliar : PageBase
    {
        protected override void OnPreRenderComplete(EventArgs e)
        {     
            if (this.dgvHierarquiaAux.Rows.Count > 0)
            {
                this.dgvHierarquiaAux.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        private ManterHierarquiaAuxiliar manterHierarquiaAux = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
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
            get { return enumFuncionalidade.Fornecedor; }
        }

        protected void dgvHierarquiaAux_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    manterHierarquiaAux = new ManterHierarquiaAuxiliar();
                    int idHierarquiaAux = int.Parse(e.CommandArgument.ToString());
                    manterHierarquiaAux.ExcluirHierarquiaAuxiliar(idHierarquiaAux);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarHierarquiaAuxiliar.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                int idHierarquiaAux = int.Parse(e.CommandArgument.ToString());                
                Response.Redirect("EdicaoHierarquiaAuxiliar.aspx?Id=" + idHierarquiaAux.ToString(), false);
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("EdicaoHierarquiaAuxiliar.aspx");
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                string nome = "";

                if (!string.IsNullOrWhiteSpace(this.txtNome.Text))
                    nome = this.txtNome.Text.Trim();

                manterHierarquiaAux = new ManterHierarquiaAuxiliar();
                IList<Sebrae.Academico.Dominio.Classes.HierarquiaAuxiliar> ListaHierarquiaAuxiliar = manterHierarquiaAux.ObterPorNomeDeUsuario(nome);

                IList<Dominio.Classes.Hierarquia> listaDiretorias = new BMHierarquia().ObterDiretorias();

                foreach (var hierarquiaAux in ListaHierarquiaAuxiliar)
                {
                    var hierarquia = listaDiretorias.FirstOrDefault(x => x.CodUnidade == hierarquiaAux.CodUnidade);

                    if (hierarquia != null)
                    {
                        hierarquiaAux.NomeDiretoria = hierarquia.Unidade;
                    }
                    else
                    {
                        hierarquiaAux.NomeDiretoria = "-";
                    }
                }

                if (ListaHierarquiaAuxiliar != null && ListaHierarquiaAuxiliar.Count > 0)
                {
                    WebFormHelper.PreencherGrid(ListaHierarquiaAuxiliar, this.dgvHierarquiaAux);
                    pnlFornecedor.Visible = true;
                }
                else
                {
                    pnlFornecedor.Visible = false;
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                }

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

    }
}
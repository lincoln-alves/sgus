using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Cadastros.MetasInstitucionais
{
    public partial class ListarMetasInstitucionais : PageBase
    {
        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvMetasInstitucionais.Rows.Count > 0)
            {
                this.dgvMetasInstitucionais.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    pnlMetaInstitucuional.Visible = false;
                    base.LogarAcessoFuncionalidade();
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.MetasInstitucionais; }
        }

        protected override System.Collections.Generic.IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }


        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            MetaInstitucional mi = new MetaInstitucional()
            {
                Nome = txtNome.Text,
            };

            DateTime DataInicioCiclo;
            DateTime DataFimCiclo;

            if (DateTime.TryParse(txtDataInicioCiclo.Text, out DataInicioCiclo))
                mi.DataInicioCiclo = DataInicioCiclo;
            if (DateTime.TryParse(txtDataFimCiclo.Text, out DataFimCiclo))
                mi.DataFimCiclo = DataFimCiclo;



            IList<MetaInstitucional> lstMI = (new ManterMetaInstitucional()).PesquisarMetasInstitucionais(mi);

            if (lstMI.Count > 0) {
                pnlMetaInstitucuional.Visible = true;
                dgvMetasInstitucionais.DataSource = lstMI;
                dgvMetasInstitucionais.DataBind();
            } else {
                pnlMetaInstitucuional.Visible = false;
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Informacao, "Nenhum Registro Encontrado.");
            }


        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("EdicaoMetasInstitucionais.aspx", false);
        }

        protected void dgvMetasInstitucionais_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    ManterMetaInstitucional manterMetaInstitucional = new ManterMetaInstitucional();
                    int idMetaInstitucional = int.Parse(e.CommandArgument.ToString());
                    manterMetaInstitucional.ExcluirMetaInstitucional(idMetaInstitucional);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarMetasInstitucionais.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                int idMetaInstitucional = int.Parse(e.CommandArgument.ToString());
                Response.Redirect("EdicaoMetasInstitucionais.aspx?Id=" + idMetaInstitucional.ToString(), false);
            }


        }
    }
}
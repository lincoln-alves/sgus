using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.CertificadoTemplate
{
    public partial class ListarCertificadoTemplate : PageBase
    {

        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvCertificadoTemplate.Rows.Count > 0)
            {
                this.dgvCertificadoTemplate.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        private ManterCertificadoTemplate manterCertificadoTemplate = null;
        private Dominio.Classes.Usuario usuarioLogado = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                usuarioLogado = new BMUsuario().ObterUsuarioLogado();
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

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.CertificadoTemplate; }
        }

        protected override System.Collections.Generic.IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }

        protected void dgvCertificadoTemplate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {

                try
                {
                    manterCertificadoTemplate = new ManterCertificadoTemplate();
                    var idCertificadoTemplate = int.Parse(e.CommandArgument.ToString());
                    manterCertificadoTemplate.ExcluirCertificadoTemplate(idCertificadoTemplate);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarCertificadoTemplate.aspx");
                }
                catch (Exception ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                var idCertificado = int.Parse(e.CommandArgument.ToString());
                Response.Redirect("EdicaoCertificadoTemplate.aspx?Id=" + idCertificado.ToString());
            }
            else if (e.CommandName.Equals("visualizar"))
            {
                int idCertificado = int.Parse(e.CommandArgument.ToString());
                //Session.Add("CertificadoTemplateEdit", idTrilha);
                //Response.Redirect("EdicaoCertificadoTemplate.aspx?Id=" + idCertificado.ToString() + "&V=S");
                VisualizarCertificado(idCertificado);
            }
            else if (e.CommandName.Equals("copiar"))
            {
                int idCertificado = int.Parse(e.CommandArgument.ToString());
                //Session.Add("CertificadoTemplateEdit", idTrilha);
                Response.Redirect("EdicaoCertificadoTemplate.aspx?Id=" + idCertificado.ToString() + "&C=S");
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            //Session.Remove("CertificadoTemplateEdit");
            Response.Redirect("EdicaoCertificadoTemplate.aspx");
        }

        private classes.CertificadoTemplate ObterObjetoCertificadoTemplate()
        {
            classes.CertificadoTemplate trilha = new classes.CertificadoTemplate();

            if (!string.IsNullOrWhiteSpace(this.txtNome.Text))
                trilha.Nome = this.txtNome.Text.Trim();

            return trilha;
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    classes.CertificadoTemplate certificadoTemplate = ObterObjetoCertificadoTemplate();
                    manterCertificadoTemplate = new ManterCertificadoTemplate();
                    IList<classes.CertificadoTemplate> ListaCertificadoTemplate = manterCertificadoTemplate.ObterCertificadoTemplatePorFiltro(certificadoTemplate);
                    WebFormHelper.PreencherGrid(ListaCertificadoTemplate, this.dgvCertificadoTemplate);

                    if (ListaCertificadoTemplate != null && ListaCertificadoTemplate.Count > 0)
                    {
                        WebFormHelper.PreencherGrid(ListaCertificadoTemplate, this.dgvCertificadoTemplate);
                        pnlTemplateCertificado.Visible = true;
                    }
                    else
                    {
                        pnlTemplateCertificado.Visible = false;
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                    }
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }

        protected void lkbEditarExcluir_PreRender(object sender, EventArgs e)
        {
            if (usuarioLogado != null)
            {
                LinkButton bntPress = (LinkButton)sender;

                if (usuarioLogado.IsGestor())
                        if (usuarioLogado.UF.ID.ToString() != bntPress.Attributes["id_uf"])
                            bntPress.Attributes.Add("hidden", "hidden");
            }
        }

        private void VisualizarCertificado(int idCertificado)
        {
            ucMostraPreviaRel.Src = "~/Cadastros/CertificadoTemplate/VisualizaCertificadoTemplate.aspx?Id=" + idCertificado;
            ucMostraPreviaRel.Abre(idCertificado);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;
using System.Data;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucTrilhas : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void PrepararTelaParaExibirInformacoesDasTrilhas(int IdUsuario)
        {
            if (IdUsuario > 0)
            {
                this.IdUsuario = IdUsuario;
                this.PreencherGrid();
            }

        }

        public int IdUsuario
        {
            get
            {
                if (ViewState["ViewStateIdUsuario"] != null)
                {
                    return (int)ViewState["ViewStateIdUsuario"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateIdUsuario"] = value;
            }

        }

        public int IdUsuarioTrilha
        {
            get
            {
                if (ViewState["ViewStateIdUsuarioTrilha"] != null)
                {
                    return (int)ViewState["ViewStateIdUsuarioTrilha"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateIdUsuarioTrilha"] = value;
            }

        }

        public IList<UsuarioTrilha> ListaUsuarioTrilha { get; set; }

        private void PreencherGrid()
        {
            ManterMatriculaTrilha manterMatriculaTrilha = new ManterMatriculaTrilha();
            this.ListaUsuarioTrilha = manterMatriculaTrilha.ObterMatriculasDoUsuario(this.IdUsuario);
            //IList<TrilhaNivel> ListaTrilhas = this.ListaUsuarioTrilha.Select(x => x.TrilhaNivel).ToList();
            WebFormHelper.PreencherGrid(ListaUsuarioTrilha, this.dgvTrilhas);
        }

        protected void dgvTrilhas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            this.AplicarRegraParaExibirBotaoCertificado(e);
        }

        private void AplicarRegraParaExibirBotaoCertificado(GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                Button btnCertificado = (Button)e.Row.FindControl("btnCertificado");

                if (btnCertificado != null)
                {
                    UsuarioTrilha usuarioTrilha = (UsuarioTrilha)e.Row.DataItem;

                    if (usuarioTrilha != null && usuarioTrilha.TrilhaNivel != null &&
                        usuarioTrilha.TrilhaNivel.ID > 0)
                    {
                        if (usuarioTrilha.StatusMatricula.Equals(enumStatusMatricula.Aprovado))
                        {
                            //Se existe um certificado template configurado para o nível, exibe o botão
                            if (usuarioTrilha.TrilhaNivel.CertificadoTemplate != null &&
                                usuarioTrilha.TrilhaNivel.CertificadoTemplate.ID > 0)
                            {
                                btnCertificado.Visible = true;
                            }
                        }
                    }
                }
            }
        }

        private UsuarioTrilha matriculaDoUsuarioNatrilha { get; set; }

        private void AplicarRegraParaExibirBotaoEmitirCertificado(GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                LinkButton lkbEmitirCertificado = (LinkButton)e.Row.FindControl("lkbEmitirCertificado");
                lkbEmitirCertificado.Click += new EventHandler(lkbEmitirCertificado_Click);
                if (lkbEmitirCertificado != null)
                {
                    TrilhaNivel trilhaNivelDaLinhaDaGrid = (TrilhaNivel)e.Row.DataItem;

                    if (trilhaNivelDaLinhaDaGrid != null)
                    {

                        if (this.ListaUsuarioTrilha != null &&
                            this.ListaUsuarioTrilha.Any(x => x.TrilhaNivel.ID == trilhaNivelDaLinhaDaGrid.ID))
                        {

                            this.matriculaDoUsuarioNatrilha = this.ListaUsuarioTrilha
                                                                            .Where(x => x.TrilhaNivel.ID == trilhaNivelDaLinhaDaGrid.ID)
                                                                            .OrderByDescending(x => x.DataInicio).FirstOrDefault();

                            this.IdUsuarioTrilha = this.matriculaDoUsuarioNatrilha.ID;

                            if (matriculaDoUsuarioNatrilha != null &&
                                matriculaDoUsuarioNatrilha.StatusMatricula.Equals(enumStatusMatricula.Aprovado) &&
                                trilhaNivelDaLinhaDaGrid.CertificadoTemplate != null)
                            {
                                lkbEmitirCertificado.Visible = true;
                            }


                        }
                        else
                        {
                            lkbEmitirCertificado.Visible = false;
                        }
                    }
                }
            }
        }

        protected void btnCertificado_Click(object sender, EventArgs e)
        {
            string a = "";
        }

        protected void lkbEmitirCertificado_Click(object sender, EventArgs e)
        {
            CertificadoTemplate cf = CertificadoTemplateUtil.ConsultarCertificado(0,0, this.IdUsuarioTrilha);
            //DataTable dt = CertificadoTemplateUtil.GerarDataTableComCertificado(0, this.IdUsuarioTrilha, cf);

            //byte[] resultado = CommonHelper.GerarArrayDeBytesParaORelatorioCertificadoTemplate(cf, dt, "PDF");
            //CommonHelper.GerarArquivoParaDowload(resultado);

        }


        protected void dgvTrilhas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Trim().ToLower().Equals("fazerinscricao"))
            {
                int idTrilhaNivel = int.Parse(e.CommandArgument.ToString());

                try
                {
                    //Matricula o aluno no nivel da trilha
                    ManterMatriculaTrilha manterMatriculaTrilha = new ManterMatriculaTrilha();

                    TrilhaNivel trilhaNivelDaLinhaDaGrid = new ManterTrilhaNivel().ObterTrilhaNivelPorID(idTrilhaNivel);
                    UsuarioTrilha usuarioTrilha = new UsuarioTrilha();
                    usuarioTrilha.TrilhaNivel = trilhaNivelDaLinhaDaGrid;
                    usuarioTrilha.Usuario = new ManterUsuario().ObterUsuarioPorID(this.IdUsuario);
                    manterMatriculaTrilha.IncluirMatriculaTrilha(usuarioTrilha);

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, string.Format("O usuário '{0}' foi matriculado no Nível '{1}' da Trilha '{2}' com Sucesso !",
                                                 usuarioTrilha.Usuario.Nome, usuarioTrilha.TrilhaNivel.Nome, usuarioTrilha.TrilhaNivel.Trilha.Nome));

                    PreencherGrid();
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                try
                {
                    //this.TratarEdicaoDeUmaMatriculaTrilha();
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "myscript", " $('#ModalPublicacoesDoSaber').modal({ backdrop: 'static', keyboard: false, show: true });", true);
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }

            }
        }



    }
}
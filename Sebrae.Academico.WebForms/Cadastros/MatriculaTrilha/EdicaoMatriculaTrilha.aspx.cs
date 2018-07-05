using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Linq;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.DTO;
using Microsoft.Reporting.WebForms;
using System.Reflection;
using System.IO;
using System.Web;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class EdicaoMatriculaTrilha : PageBase
    {
        private UsuarioTrilha usuarioTrilhaEdicao = null;
        private ManterMatriculaTrilha manterMatriculaTrilha = new ManterMatriculaTrilha();

        private UsuarioTrilha usuarioTrilha;
        private IList<DTORelatorioHistoricoAtividadeSolucoesAutoindicativa> lstSolAutoInd;
        private IList<DTORelatorioHistoricoAtividadeSolucoesPortifolio> lstSolPort;
        private IList<DTORelatorioHistoricoAtividadeSprint> lstSprint;
        private IList<DTORelatorioHistoricoAtividadeTopicoTematicoCount> lstTopicoCount;
        private IList<DTOAlunoDaTrilha> listaDTOAlunoDaTrilha;
        public IList<DTORelatorioHistoricoAtividadeDiagnostico> lstDiagnostico;
        public IList<DTORelatorioHistoricoAtividadeNotaProva> lstNotaProva;

        string binPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "bin");

        protected override System.Collections.Generic.IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.MatriculaTrilha; }
        }

        protected void OcultarModal_Click(object sender, EventArgs e)
        {
            this.OcultarModal();
        }

        private void OcultarModal()
        {
            base.OcultarBackDrop();
            pnlModal.Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                this.ucMatrilhaTrilha1.PreencherCombos();

                //Se Veio da Tela de Pesquisa, trata a edição
                if (Request["Id"] != null)
                {
                    int idItemTrilha = int.Parse(Request["Id"].ToString());
                    usuarioTrilhaEdicao = manterMatriculaTrilha.ObterMatriculaTrilhaPorID(idItemTrilha);
                    PreencherCampos(usuarioTrilhaEdicao);

                    this.ExibirPanelDeProvasRealizadas();

                    //Todo -> Criar tela para visualizar as provas do usuário
                    PreencherGridComProvasRealizadas(usuarioTrilhaEdicao);

                    this.HabilitarComboDeStatus();
                    this.ExibirCheckBoxDeAcessoBloqueado();
                    this.ucMatrilhaTrilha1.TratarStatus(this.usuarioTrilhaEdicao);
                    //this.TratarStatus();
                }
                else
                {
                    this.EsconderPanelDeProvasRealizadas();
                    this.ucMatrilhaTrilha1.TratarInformacoesDaMatricula();
                    ////Cadastro
                    //WebFormHelper.SetarValorNaCombo(((int)enumStatusMatricula.Inscrito).ToString(), ddlStatus, true);
                    //this.DesabilitarComboDeStatus();
                    //this.EsconderCheckBoxDeAcessoBloqueado();
                }
            }
        }


        private void EsconderCheckBoxDeAcessoBloqueado()
        {
            //this.divAcessoBloqueado.Visible = false;
            this.ucMatrilhaTrilha1.EsconderCheckBoxDeAcessoBloqueado();
        }

        private void ExibirCheckBoxDeAcessoBloqueado()
        {
            this.ucMatrilhaTrilha1.ExibirCheckBoxDeAcessoBloqueado();
        }

        private void HabilitarComboDeStatus()
        {
            this.ucMatrilhaTrilha1.HabilitarComboDeStatus();
        }

        private void DesabilitarComboDeStatus()
        {
            this.ucMatrilhaTrilha1.DesabilitarComboDeStatus();
        }

        private void ExibirPanelDeProvasRealizadas()
        {
            this.pnlProvasRealizadas.Visible = true;
        }

        private void EsconderPanelDeProvasRealizadas()
        {
            this.pnlProvasRealizadas.Visible = false;
        }

        private void PreencherGridComProvasRealizadas(UsuarioTrilha usuarioTrilha)
        {
            try
            {
                ManterQuestionarioParticipacao manterQuestionarioParticipacao = new ManterQuestionarioParticipacao();
                IList<QuestionarioParticipacao> ListaProvasDoUsuario = manterQuestionarioParticipacao.ObterProvasDaTrilhaDoUsuario(usuarioTrilha.Usuario.ID, usuarioTrilha.TrilhaNivel.ID);
                WebFormHelper.PreencherGrid(ListaProvasDoUsuario, this.dgvProvasRealizadas);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        #region "Eventos do Grid"

        protected void dgvProvasRealizadas_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {

            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {

                QuestionarioParticipacao questionarioParticipacao = (QuestionarioParticipacao)e.Row.DataItem;

                if (questionarioParticipacao != null && questionarioParticipacao.ID > 0)
                {

                    decimal percObtido;

                    ManterUsuarioTrilha manterUt;

                    UsuarioTrilha ut;

                    Sebrae.Academico.BP.Services.SgusWebService.ManterQuestionarioParticipacao manterQuestionarioParticipacao = new Sebrae.Academico.BP.Services.SgusWebService.ManterQuestionarioParticipacao();

                    manterQuestionarioParticipacao.CalcularPercentualDaProva(questionarioParticipacao, out percObtido, out manterUt, out ut);

                    //Calcula as notas somente para as provas
                    if (questionarioParticipacao.TipoQuestionarioAssociacao != null &&
                        questionarioParticipacao.TipoQuestionarioAssociacao.ID.Equals((int)enumTipoQuestionarioAssociacao.Prova))
                    {

                        decimal notaProva = 0;

                        foreach (ItemQuestionarioParticipacao itemQuestionarioParticipacao in questionarioParticipacao.ListaItemQuestionarioParticipacao)
                        {
                            if (itemQuestionarioParticipacao.ValorAtribuido.HasValue)
                            {
                                notaProva += itemQuestionarioParticipacao.ValorAtribuido.Value;
                            }
                        }

                        Label lblNotaProva = (Label)e.Row.FindControl("lblNotaProva");
                        lblNotaProva.Text = notaProva.ToString();
                    }

                    Label lblTipoQuestionarioAssociacao = (Label)e.Row.FindControl("lblTipoQuestionarioAssociacao");
                    switch (questionarioParticipacao.TipoQuestionarioAssociacao.ID)
                    {
                        case (int)enumTipoQuestionarioAssociacao.Prova:
                            lblTipoQuestionarioAssociacao.Text = "Prova";
                            break;

                        case (int)enumTipoQuestionarioAssociacao.Pre:

                            if (questionarioParticipacao.Evolutivo)
                            {
                                lblTipoQuestionarioAssociacao.Text = "Diagnóstico Pré";
                            }
                            else
                            {
                                lblTipoQuestionarioAssociacao.Text = "Questionário Pré";
                            }

                            break;

                        case (int)enumTipoQuestionarioAssociacao.Pos:

                            if (questionarioParticipacao.Evolutivo)
                            {
                                lblTipoQuestionarioAssociacao.Text = "Diagnóstico Pós";
                            }
                            else
                            {
                                lblTipoQuestionarioAssociacao.Text = "Questionário Pós";
                            }

                            break;
                    }

                }

            }

        }

        #endregion

        private void PreencherCampos(UsuarioTrilha usuarioTrilha)
        {
            this.ucMatrilhaTrilha1.PreencherCampos(usuarioTrilha);
        }

        protected void btnSalvar_Click(object sender, EventArgs e){
            try{
                var manterMatriculaTrilha = new ManterMatriculaTrilha();

                if (Request["Id"] == null){
                    usuarioTrilhaEdicao = this.ucMatrilhaTrilha1.ObterObjetoUsuarioTrilha();
                    manterMatriculaTrilha.IncluirMatriculaTrilha(usuarioTrilhaEdicao);
                }else{
                    this.ucMatrilhaTrilha1.IdUsuarioTrilhaEdit = int.Parse(Request["Id"].ToString());
                    usuarioTrilhaEdicao = this.ucMatrilhaTrilha1.ObterObjetoUsuarioTrilha(true);
                    manterMatriculaTrilha.AlterarMatriculaTrilha(usuarioTrilhaEdicao);
                }

                Session.Remove("MatriculaTrilhaEdit");

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarMatriculaTrilha.aspx");

            }catch (AcademicoException ex){
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }


        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Session.Remove("MatriculaTrilhaEdit");
            Response.Redirect("ListarMatriculaTrilha.aspx");
        }

        protected void dgvProvasRealizadas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("verprova"))
            {
                try
                {
                    int idQuestionarioParticipacao = int.Parse(e.CommandArgument.ToString());
                    ExibirModal();
                    this.ExibirInformacoesDetalhasDaProva(idQuestionarioParticipacao);
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }

            }
        }

        private void ExibirInformacoesDetalhasDaProva(int idQuestionarioParticipacao)
        {
            QuestionarioParticipacao questionarioParticipacao = new ManterQuestionarioParticipacao().ObterQuestionarioParticipacaoPorId(idQuestionarioParticipacao);

            if (questionarioParticipacao != null)
            {
                this.CarregarInformacoesDetalhadasDaProvaDoAluno(questionarioParticipacao);
            }
        }

        private void CarregarInformacoesDetalhadasDaProvaDoAluno(QuestionarioParticipacao questionarioParticipacao)
        {
            if (questionarioParticipacao != null)
            {
                this.ucExibirQuestionarioResposta.ExibirInformacoesDetalhadasDaProvaDoAluno(questionarioParticipacao, 0);
            }
        }

        private void ExibirModal()
        {
            base.ExibirBackDrop();
            pnlModal.Visible = true;
        }
        protected void btnHistorico_Click(object sender, EventArgs e)
        {
            this.GerarRelatorioHistoricoAtividade(int.Parse(Request["Id"].ToString()));
        }

        private void GerarRelatorioHistoricoAtividade(int idUsuarioTrilha)
        {
            using (RelatorioHistoricoAtividade relHist = new RelatorioHistoricoAtividade())
            {
                usuarioTrilha = relHist.ObterUsuarioTrilhaPorID(idUsuarioTrilha);
                IList<DTORelatorioHistoricoAtividadeDadosBasicos> relDp = null; // relHist.ConsultarHistoricoAtividadesDadosPessoais(pIdUsuarioTrilha);

                this.listaDTOAlunoDaTrilha = new ManterUsuarioTrilha().ListarRelatorioDoAlunoDaTrilha(usuarioTrilha.ID);

                //Dados Básico do usuário
                relDp = relHist.ObterDTORelatorioHistoricoAtividadeDadosBasicos(this.listaDTOAlunoDaTrilha);

                lstTopicoCount = relHist.ConsultarHistoricoAtividadeTopicoTematico(listaDTOAlunoDaTrilha);

                lstNotaProva = relHist.ConsultaHistoricoAtividadeNotaProva(usuarioTrilha.ID);
                lstDiagnostico = relHist.ConsultarHistoricoAtividadeDiagnostico(usuarioTrilha.ID);

                ReportViewer rv = new ReportViewer();

                Assembly assembly = Assembly.LoadFrom(binPath + "\\Sebrae.Academico.Reports.dll");
                Stream stream = assembly.GetManifestResourceStream("Sebrae.Academico.Reports.HistoricoAtividades.rptHistoricoAtividades.rdlc");
                rv.LocalReport.LoadReportDefinition(stream);

                stream = assembly.GetManifestResourceStream("Sebrae.Academico.Reports.HistoricoAtividades.rptHistoricoAtividadesTopicoTematico.rdlc");
                rv.LocalReport.LoadSubreportDefinition("HistoricoAtividades.rptHistoricoAtividadesTopicoTematico.rdlc", stream);

                rv.LocalReport.DataSources.Clear();
                rv.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", relDp));
                rv.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", lstNotaProva));
                rv.LocalReport.DataSources.Add(new ReportDataSource("DataSet2a", lstDiagnostico));
                rv.LocalReport.DataSources.Add(new ReportDataSource("DataSet3", this.listaDTOAlunoDaTrilha));

                rv.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
                rv.LocalReport.Refresh();

                try
                {

                    byte[] arradeBytes = rv.LocalReport.Render("PDF");

                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachement; filename=\"{0}\"", "HistoricoAtividade.pdf"));
                    HttpContext.Current.Response.AddHeader("Content-Length", arradeBytes.Length.ToString());
                    HttpContext.Current.Response.OutputStream.Write(arradeBytes, 0, arradeBytes.Length);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
                }
                catch (System.Exception ex)
                {
                    Exception inner = ex.InnerException;
                    while (inner != null)
                    {
                        string mensagem = inner.Message;
                        inner = inner.InnerException;
                    }

                }





            }
        }

        private void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            using (RelatorioHistoricoAtividade relHist = new RelatorioHistoricoAtividade())
            {
                ReportDataSource r = new ReportDataSource("dsTopicoTematico", lstTopicoCount.Where(x => x.IDTopicoTematico == int.Parse(e.Parameters[0].Values[0])).ToList());
                e.DataSources.Add(r);

                lstSolAutoInd = relHist.ConsultarHistoricoAtividadeSolucoesAutoIndicativas(this.listaDTOAlunoDaTrilha, int.Parse(e.Parameters[0].Values[0]));
                ReportDataSource ra = new ReportDataSource("dsSolucoesAutoIndicativas", lstSolAutoInd);
                e.DataSources.Add(ra);

                lstSolPort = relHist.ConsultaHistoricoAtividadeSolucoesPortifolio(this.listaDTOAlunoDaTrilha, int.Parse(e.Parameters[0].Values[0]));
                ReportDataSource rp = new ReportDataSource("dsSolucoesPortifolio", lstSolPort);
                e.DataSources.Add(rp);

                lstSprint = relHist.ConsultaHistoricoAtividadeSprint(usuarioTrilha.ID, int.Parse(e.Parameters[0].Values[0]));
                ReportDataSource rs = new ReportDataSource("dsSprint", lstSprint);
                e.DataSources.Add(rs);

            }
        }
    }
}
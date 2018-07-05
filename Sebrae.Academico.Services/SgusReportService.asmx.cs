using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Services;
using System.Web.Services.Protocols;
using Microsoft.Reporting.WebForms;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.BP.Services.SgusWebService;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;
using Sebrae.Academico.Util.Classes;
using WebService = System.Web.Services.WebService;

namespace Sebrae.Academico.Services
{
    /// <summary>
    /// Summary description for SgusReportService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SgusReportService : WebService
    {

        public AuthenticationRequest autenticacao;
        private SegurancaAutenticacao segurancaAutenticacao = new SegurancaAutenticacao();

        private UsuarioTrilha usuarioTrilha;
        private IList<DTORelatorioHistoricoAtividadeSolucoesAutoindicativa> lstSolAutoInd;
        private IList<DTORelatorioHistoricoAtividadeSolucoesPortifolio> lstSolPort;
        private IList<DTORelatorioHistoricoAtividadeSprint> lstSprint;
        private IList<DTORelatorioHistoricoAtividadeTopicoTematicoCount> lstTopicoCount;
        private IList<DTOAlunoDaTrilha> listaDTOAlunoDaTrilha;
        readonly string _binPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");

        [WebMethod]
        [SoapHeader("autenticacao")]
        public byte[] EmitirCertificadoModelo(int pIdMatriculaOferta, int pIdUsuarioTrilha, int pIdTrilha, int pIdTrilhaNivel)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao))) throw new Exception("Usuário não autenticado pelo sistema.");
            TrilhaNivel tn = null;
            if (pIdUsuarioTrilha > 0)
            {
                var ut = new BMUsuarioTrilha().ObterPorId(pIdUsuarioTrilha);
                if (!(ut != null && ut.TrilhaNivel.CertificadoTemplate != null && ut.TrilhaNivel.CertificadoTemplate.ID > 0)) throw new Exception("Erro ao gerar o certificado.");
                tn = ut.TrilhaNivel;
            }
            else
            {
                tn = (new BMTrilhaNivel()).ObterPorFiltro(new TrilhaNivel
                {
                    ID = pIdTrilhaNivel,
                    Trilha = new Trilha
                    {
                        ID = pIdTrilha
                    }
                }).FirstOrDefault();
                if (!(tn != null && tn.CertificadoTemplate != null && tn.CertificadoTemplate.ID > 0)) throw new Exception("Erro ao gerar o certificado.");
            }

            var cf = new BMCertificadoTemplate().ObterPorID(tn.CertificadoTemplate.ID);
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("TX_Certificado"));
            dt.Columns.Add(new DataColumn("OB_Imagem", typeof(byte[])));
            var dr = dt.NewRow();
            // Caso seja do tutor o valor pIdUsuarioTrilha é > 0
            if (pIdUsuarioTrilha == 0)
            {
                dr["TX_Certificado"] = cf.TextoDoCertificado;
            }
            else
            {
                dr["TX_Certificado"] = cf.TextoDoCertificado;
            }
            dr["OB_Imagem"] = CertificadoTemplateUtil.ObterImagemBase64(cf.Imagem);
            if (!string.IsNullOrEmpty(cf.TextoCertificado2) && !string.IsNullOrEmpty(cf.Imagem2))
            {
                dt.Columns.Add(new DataColumn("TX_Certificado2"));
                dt.Columns.Add(new DataColumn("OB_Image2", typeof(byte[])));
                dr["TX_Certificado2"] = cf.TextoCertificado2;
                dr["OB_Image2"] = CertificadoTemplateUtil.ObterImagemBase64(cf.Imagem2);
            }
            dt.Rows.Add(dr);
            return CertificadoTemplateUtil.RetornarCertificado(cf, dt);
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public byte[] EmitirCertificado(int pIdMatriculaPrograma, int pIdMatriculaOferta, int pIdUsuarioTrilha)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            var cf = CertificadoTemplateUtil.ConsultarCertificado(pIdMatriculaPrograma,pIdMatriculaOferta, pIdUsuarioTrilha);
            var dt = CertificadoTemplateUtil.GerarDataTableComCertificado(pIdMatriculaPrograma,pIdMatriculaOferta, pIdUsuarioTrilha, cf);
            return CertificadoTemplateUtil.RetornarCertificado(cf, dt);
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public byte[] EmitirCertificadoTutor(int pIdOferta, int pIdTurma, int pIdUsuario)
        {
            try
            {
                if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                    throw new Exception("Usuário não autenticado pelo sistema.");

                var cf = CertificadoTemplateUtil.ConsultarCertificadoTutor(pIdOferta);

                var dt = CertificadoTemplateUtil.GerarDataTableComCertificadoTutor(pIdOferta, pIdTurma, pIdUsuario, cf);

                string caminhoReport;

                if (!string.IsNullOrWhiteSpace(cf.TextoCertificado2) && !string.IsNullOrEmpty(cf.Imagem2))
                {
                    caminhoReport = "EmitirCertificado.rptCertificadoTemplate2paginas.rdlc";
                }
                else
                {
                    caminhoReport = "EmitirCertificado.rptCertificadoTemplate.rdlc";
                }

                var combine = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
                var assembly = Assembly.LoadFrom(combine + "\\Sebrae.Academico.Reports.dll");
                var stream = assembly.GetManifestResourceStream("Sebrae.Academico.Reports." + caminhoReport);


                var rv = new ReportViewer();
                rv.LocalReport.LoadReportDefinition(stream);
                rv.LocalReport.DataSources.Clear();
                rv.LocalReport.DataSources.Add(new ReportDataSource("dsCertificadoTemplate", dt));

                return rv.LocalReport.Render("PDF");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public byte[] HistoricoAtividadeTrilha(int pIdUsuarioTrilha)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            using (RelatorioHistoricoAtividade relHist = new RelatorioHistoricoAtividade())
            {
                usuarioTrilha = relHist.ObterUsuarioTrilhaPorID(pIdUsuarioTrilha);
                IList<DTORelatorioHistoricoAtividadeDadosBasicos> relDp = null; // relHist.ConsultarHistoricoAtividadesDadosPessoais(pIdUsuarioTrilha);

                this.listaDTOAlunoDaTrilha = new ManterUsuarioTrilha().ListarRelatorioDoAlunoDaTrilha(usuarioTrilha.ID);

                //Dados Básico do usuário
                relDp = relHist.ObterDTORelatorioHistoricoAtividadeDadosBasicos(this.listaDTOAlunoDaTrilha);

                lstTopicoCount = relHist.ConsultarHistoricoAtividadeTopicoTematico(listaDTOAlunoDaTrilha);

                IList<DTORelatorioHistoricoAtividadeNotaProva> lstNotaProva = relHist.ConsultaHistoricoAtividadeNotaProva(usuarioTrilha.ID);
                IList<DTORelatorioHistoricoAtividadeDiagnostico> lstDiagnostico = relHist.ConsultarHistoricoAtividadeDiagnostico(usuarioTrilha.ID);

                ReportViewer rv = new ReportViewer();

                Assembly assembly = Assembly.LoadFrom(_binPath + "\\Sebrae.Academico.Reports.dll");
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


                return rv.LocalReport.Render("PDF");

            }
        }



        private IList<DTORelatorioHistoricoAtividadeDadosBasicos> ObterDTORelatorioHistoricoAtividadeDadosBasicos()
        {
            throw new NotImplementedException();
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

        [WebMethod]
        [SoapHeader("autenticacao")]
        public byte[] HistoricoAcademico(int pIdUsuario)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            using (RelatorioHistoricoAcademico relHisAcad = new RelatorioHistoricoAcademico())
            {

                relHisAcad.RegistrarLog();

                var DadosGerais = relHisAcad.ConsultarHistoricoAcademicoDadosGerais(pIdUsuario);
                var lstCursos = relHisAcad.ConsultaHistoricoAcademicoCursos(pIdUsuario);
                var lstSGTC = relHisAcad.ConsultaHistoricoAcademicoSGTC(pIdUsuario);
                var lstExtracurricular = relHisAcad.ConsultarHistoricoAcademicoExtracurricular(pIdUsuario);
                var lstPrograma = relHisAcad.ConsultarHistoricoAcademicoPrograma(pIdUsuario);
                var lstTrilha = relHisAcad.ConsultarHistoricoAcademicoTrilha(pIdUsuario);

                ReportViewer rv = new ReportViewer();

                Assembly assembly = Assembly.LoadFrom(_binPath + "\\Sebrae.Academico.Reports.dll");
                Stream stream = assembly.GetManifestResourceStream("Sebrae.Academico.Reports.HistoricoAcademico.rptHistoricoAcademico.rdlc");
                rv.LocalReport.LoadReportDefinition(stream);

                rv.LocalReport.DataSources.Clear();
                rv.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", DadosGerais));
                rv.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", lstCursos));
                rv.LocalReport.DataSources.Add(new ReportDataSource("DataSet3", lstTrilha));
                rv.LocalReport.DataSources.Add(new ReportDataSource("DataSet4", lstPrograma));
                rv.LocalReport.DataSources.Add(new ReportDataSource("DataSet5", lstExtracurricular));
                rv.LocalReport.DataSources.Add(new ReportDataSource("DataSet6", lstSGTC));
                rv.LocalReport.Refresh();

                return rv.LocalReport.Render("PDF");

            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public byte[] EmitirCertificadoCertame(int certificadoId)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            return new ManterCertificadoCertameService().ConsultarCertificadoCertame(certificadoId, autenticacao.Login);
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public byte[] EmitirBoletimCertame(int certificadoId)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            return new ManterCertificadoCertameService().ConsultarBoletimCertame(certificadoId, autenticacao.Login);
        }
    }
}

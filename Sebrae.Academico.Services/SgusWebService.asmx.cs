using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.Principal;
using System.Web.Services;
using System.Web.Services.Protocols;
using Sebrae.Academico.BM.Classes.Moodle;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.BP.DTO.Services.ConsultarFormacaoDeMultiplicadores;
using Sebrae.Academico.BP.DTO.Services.ConsultarSolucaoEducacionalAutoIndicativa;
using Sebrae.Academico.BP.DTO.Services.GerarPagamento;
using Sebrae.Academico.BP.DTO.Services.GestorUC;
using Sebrae.Academico.BP.DTO.Services.HistoricoAcademico;
using Sebrae.Academico.BP.DTO.Services.ListaProgramas;
using Sebrae.Academico.BP.DTO.Services.MetasIndividuais;
using Sebrae.Academico.BP.DTO.Services.Processo;
using Sebrae.Academico.BP.DTO.Services.Protocolo;
using Sebrae.Academico.BP.DTO.Services.Questionario;
using Sebrae.Academico.BP.Services.Credenciamento;
using Sebrae.Academico.BP.Services.SgusWebService;
using Sebrae.Academico.BP.Services;
using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.BM.Classes;

namespace Sebrae.Academico.Services
{
    /// <summary>
    /// Summary description for SgusWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public partial class SgusWebService : System.Web.Services.WebService
    {
        public AuthenticationRequest autenticacao;
        public AuthenticationTokenRequest autenticacaoToken;
        private SegurancaAutenticacao segurancaAutenticacao = new SegurancaAutenticacao();
        private AtividadeFormativaServices atividadeFormativaServices = new AtividadeFormativaServices();
        private MatriculaTrilhaServices matriculaTrilhaServices = new MatriculaTrilhaServices();
        private ViewTrilhaServices viewTrilhaServices = new ViewTrilhaServices();
        private HistoricoAcademicoServices historicoAcademicoServices = new HistoricoAcademicoServices();

        private string msgErroPadrao = "Erro ao processar a solicitação. Por favor, tente novamente mais tarde.";

        #region Consultar e Cadastrar Programa

        private ManterMatriculaPrograma manterPrograma;

        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<DTOListaProgramaPrograma> ListarProgramasDisponiveisPorUsuario(int pUsuario)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                manterPrograma = new ManterMatriculaPrograma();
                return manterPrograma.ListarProgramasDisponiveis(pUsuario);
            }
            catch
            {
                return null;
            }
        }


        #endregion

        #region MatriculaPrograma

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService RegistrarMatriculaPrograma(int idPrograma, int idUsuario)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                manterPrograma = new ManterMatriculaPrograma();
                manterPrograma.RegistrarMatricula(idPrograma, idUsuario, autenticacao);
                return new RetornoWebService() { Erro = 0, Mensagem = "" };
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = msgErroPadrao };
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<DTOMatriculaPrograma> ConsultaStatusMatriculaPrograma(int idPrograma, int idUsuario)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                manterPrograma = new ManterMatriculaPrograma();
                return manterPrograma.ConsultaStatusMatricula(idPrograma, idUsuario);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region ManterUF


        [WebMethod]
        public List<DTOUf> ListarUF()
        {
            try
            {
                return (new ConsultarUF()).ListarUF().OrderBy(x => x.Sigla).ToList();
            }
            catch
            {
                return null;
            }
        }


        #endregion

        #region ManterFormaAquisicao


        [WebMethod]
        public List<DTOFormaAquisicao> ListarFormaAquisicao()
        {
            try
            {
                return (new ConsultarFormaAquisicao()).ListarFormaAquisicao().OrderBy(x => x.Nome).ToList();
            }
            catch
            {
                return null;
            }

        }


        #endregion

        #region ManterNivelOcupacional

        [WebMethod]
        public List<DTONivelOcupacional> ListarNivelOcupacional()
        {
            try
            {
                return (new ConsultarNivelOcupacional()).ListarNivelOcupacional().OrderBy(x => x.Nome).ToList();
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region ManterPerfis
        [WebMethod]
        public List<DTOPerfil> ListarPerfil()
        {
            try
            {
                return (new ConsultarPerfil()).ListarPerfil().OrderBy(x => x.Nome).ToList();
            }
            catch
            {
                return null;
            }

        }

        #endregion

        #region CadastrarHistoricoExtraCurricular

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService CadastrarHistoricoExtraCurricular(DTOHistoricoExtraCurricular pHistoricoExtracurricular)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                (new CadastrarHistoricoExtraCurricular()).InserirHistoricoExtraCurricular(pHistoricoExtracurricular, autenticacao);
                return new RetornoWebService() { Erro = 0, Mensagem = "" };
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = msgErroPadrao };
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService RemoverHistoricoExtraCurricular(DTOHistoricoExtraCurricular pHistoricoExtracurricular)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                (new CadastrarHistoricoExtraCurricular()).RemoverHistoricoExtraCurricular(pHistoricoExtracurricular, autenticacao);
                return new RetornoWebService() { Erro = 0, Mensagem = "" };
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = msgErroPadrao };
            }
        }


        #endregion

        #region Metas Individuais e Institucionais

        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<DTOMetaInstitucional> ConsultarMetasInstitucionais()
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return (new ConsultarMetas()).ObterTodos().OrderBy(x => x.Nome).ToList();
            }
            catch
            {
                return null;
            }

        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<DTOListaMetaIndividualMeta> ConsultarMetasIndividuais(int pIdUsuario)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return new ConsultarMetas().ConsultarMetasIndividuais(pIdUsuario).ToList();
            }
            catch
            {
                return null;
            }

        }

        #endregion

        #region ManterUsuario

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOManterUsuario ConsultarUsuario(int pId_Usuario)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return (new ManterUsuarioServices()).ConsultarPorId(pId_Usuario);
            }
            catch
            {
                return null;
            }

        }


        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOManterUsuario ConsultarUsuarioPorCPF(string cpf)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return (new ManterUsuarioServices()).ConsultarUsuario(cpf);
            }
            catch
            {
                return null;
            }

        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService CadastrarImagemUsuario(int ID_Usuario, string ImagemBase64)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                string link = new ManterUsuarioServices().CadastrarImagemUsuario(ID_Usuario, ImagemBase64, autenticacao);
                return new RetornoWebService() { Erro = 0, Mensagem = link };
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = msgErroPadrao };
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService ManterUsuario(DTOManterUsuario pUsuario)
        {

            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                (new ManterUsuarioServices()).ManterUsuario(pUsuario, autenticacao);
                return new RetornoWebService() { Erro = 0, Mensagem = "" };
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = msgErroPadrao };
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService ManterUsuarioPortal(DTOUsuarioPortal pUsuario)
        {

            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                (new ManterUsuarioServices()).ManterUsuarioPortal(pUsuario, autenticacao);
                return new RetornoWebService() { Erro = 0, Mensagem = "" };
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = msgErroPadrao };
            }
        }

        [WebMethod]
        public DTOManterUsuario AutenticarUsuario(string cpf, string Senha, string IPUsuario)
        {
            try
            {
                return (new ManterUsuarioServices()).AutenticaUsuarioInformado(cpf, Senha, IPUsuario, string.Empty);
            }
            catch (Exception ex)
            {
                return new DTOManterUsuario() { MensagemLogin = ex.Message };
            }
        }

        [WebMethod]
        public DTOUsuarioPortal AutenticarUsuarioPortal(string cpf, string Senha, string IPUsuario)
        {
            try
            {
                return (new ManterUsuarioServices()).AutenticaUsuarioPortal(cpf, Senha, IPUsuario, string.Empty);
            }
            catch (Exception ex)
            {
                return new DTOUsuarioPortal() { MensagemLogin = ex.Message };
            }
        }

        [WebMethod]
        public DTOUsuarioPortalConselhos AutenticarUsuarioPortalConselhos(string cpf, string Senha, string IPUsuario)
        {
            try
            {
                return (new ManterUsuarioServices()).AutenticaUsuarioConselhos(cpf, Senha, IPUsuario);
            }
            catch (Exception ex)
            {
                return new DTOUsuarioPortalConselhos() { MensagemLogin = ex.Message };
            }
        }

        // Metodo para verificar pagamento para o portal30
        [WebMethod]
        public DTODadosPagamento VerificarPagamento(int Id_Usuario)
        {
            try
            {
                return new ManterUsuarioServices().VerificarPagamento(Id_Usuario);
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        [WebMethod]
        public DTOManterUsuario AutenticarUsuarioPorSID(string sid)
        {
            try
            {
                return (new ManterUsuarioServices()).AutenticaUsuarioInformadoPorSID(sid);
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        [WebMethod]
        public string ConsultarSSID()
        {
            try
            {
                IntPtr logonToken = Context.Request.LogonUserIdentity.Token;

                WindowsIdentity windowsId = new WindowsIdentity(logonToken);

                return (new ManterUsuarioServices()).ConsultarSIDValido(windowsId.User.ToString());

            }
            catch
            {
                return "";
            }

        }

        [WebMethod]
        public string ConsultarTesteSSID()
        {
            try
            {
                IntPtr logonToken = Context.Request.LogonUserIdentity.Token;

                WindowsIdentity windowsId = new WindowsIdentity(logonToken);

                return windowsId.User.ToString();

            }
            catch
            {
                return "";
            }

        }


        [WebMethod]
        public RetornoWebService GerarSenhaPortal(string cpf, int? enumTemplate)
        {
            try
            {
                if (cpf.Length == 11)
                {
                    if (enumTemplate == null)
                    {
                        new ManterUsuarioServices().GerarSenhaPortal(cpf);
                    }
                    else
                    {
                        new ManterUsuarioServices().GerarSenhaPortal(cpf, (enumTemplate)enumTemplate);
                    }
                }
                else
                {
                    return new RetornoWebService() { Erro = 1, Mensagem = "O CPF deve possuir 11 dígitos" };
                }
                return new RetornoWebService() { Erro = 0, Mensagem = "" };
            }

            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = msgErroPadrao };
            }
        }

        #endregion

        #region Cadastrar e Consultar Tag de Interesse

        //[WebMethod]
        //[SoapHeader("autenticacao")]
        //public List<DTOUsuarioTag> ConsultarTagInteresse(DTOUsuario pUsuario)
        //{
        //    if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
        //        throw new Exception("Usuário não autenticado pelo sistema.");

        //    try
        //    {
        //        return (new CadastrarTagInteresse()).ConsultarTagInteresse(pUsuario).ToList();
        //    }
        //    catch
        //    {
        //        return null;
        //    }

        //}

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService AlternarTagInteresse(int IdUsuario, int IdTag)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                enumOperacao enumOperacao = new CadastrarTagInteresse().AlternarTagInteresse(IdUsuario, IdTag, autenticacao);

                if (enumOperacao.Equals(enumOperacao.Exclusao))
                {
                    return new RetornoWebService() { Erro = 0, Mensagem = "Excluido" };
                }
                else if (enumOperacao.Equals(enumOperacao.Inclusao))
                {
                    return new RetornoWebService() { Erro = 0, Mensagem = "Incluido" };
                }
                return new RetornoWebService() { Erro = 1, Mensagem = "Nenhuma operação executada" };

            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = msgErroPadrao };
            }
        }

        #endregion

        #region Solucao Educacional

        // TODO: Correção da adição do SAS
        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService MatriculaSolucaoEducacional(int idUsuario, int idSolucaoEducacional, int idOferta,
            List<int> pListaIdMetaIndividualAssociada = null, List<int> pListaIdMetaInstitucionalAssociada = null)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return (new ManterSolucaoEducacionalService()).MatricularSolucaoEducacional(idUsuario,
                    idSolucaoEducacional, idOferta, pListaIdMetaIndividualAssociada, pListaIdMetaInstitucionalAssociada,
                    autenticacao.Login);
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService { Erro = 1, Mensagem = ex.Message };
            }
            catch (Exception)
            {
                return new RetornoWebService
                {
                    Erro = 1,
                    Mensagem = "Erro ao processar a solicitação. Favor entrar em contato com o suporte"
                };
            }
        }


        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService MatriculaTurma(int usuarioId, int turmaId,
            List<int> pListaIdMetaIndividualAssociada = null, List<int> pListaIdMetaInstitucionalAssociada = null)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return new ManterSolucaoEducacionalService().MatricularTurma(usuarioId, turmaId,
                    pListaIdMetaIndividualAssociada, pListaIdMetaInstitucionalAssociada, autenticacao.Login);
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService { Erro = 1, Mensagem = ex.Message };
            }
            catch (AlertException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService
                {
                    Erro = 1,
                    Mensagem = "Erro ao processar a solicitação. Favor entrar em contato com o suporte"
                };
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService NotificarAlunosFilaDeEsperaAoFinalDaOferta()
        {

            try
            {
                var alunos = new BP.ManterMatriculaOferta().NotificarAlunosFilaDeEsperaAoFinalDaOferta();

                return new RetornoWebService
                {
                    Erro = 0,
                    Mensagem = string.Format("Foram notificados {0}", alunos.Count())
                };
            }
            catch (Exception ex)
            {
                return new RetornoWebService
                {
                    Erro = 1,
                    Mensagem = ex.Message
                };
            }

        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService RegistrarAcessoSolucaoEducacional(string url, int idTurma = 0)
        {
            url = CriptografiaHelper.Base64Decode(url);

            if (idTurma > 0)
            {
                if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                    throw new Exception("Usuário não autenticado pelo sistema.");

                var turma = new BMTurma().ObterPorID(idTurma);
                var bmLog = new BMLogAcessoSolucaoEducacional();
                var logAcesso = bmLog.ObterLog(turma.ID, turma.Oferta.ID, turma.Oferta.SolucaoEducacional.ID);
                if (logAcesso == null)
                {
                    var logAcessoSolucaoEducacional = new LogAcessoSolucaoEducacional
                    {
                        ID_SolucaoEducacional = turma.Oferta.SolucaoEducacional.ID,
                        //ID_Usuario = new BMUsuario().ObterPorCPF(autenticacao.Login.ToString()).ID,
                        DataAcesso = DateTime.Now,
                        QuantidadeDeAcessos = 1,
                        ID_Oferta = turma.Oferta.ID,
                        ID_Turma = turma.ID
                    };
                    bmLog.Salvar(logAcessoSolucaoEducacional);
                }
                else
                {
                    logAcesso.QuantidadeDeAcessos++;
                    bmLog.Salvar(logAcesso);
                }
                if (logAcesso != null)
                {
                    var bmLogAcessoUsuario = new BMLogAcessoSolucaoEducacionalUsuario();
                    bmLogAcessoUsuario.Salvar(new LogAcessoSolucaoEducacionalUsuario
                    {
                        ID_SolucaoEducacional = turma.Oferta.SolucaoEducacional.ID,
                        DataAcesso = DataUtil.AjustarTimeZoneBR(DateTime.Now),
                        ID_AcessoSolucaoEducacional = logAcesso.ID,
                        ID_Oferta = turma.Oferta.ID,
                        ID_Turma = turma.ID,
                        ID_Usuario = new BMUsuario().ObterPorCPF(autenticacao.Login).ID,
                    });
                }
            }
            return new RetornoWebService() { Erro = 0, Mensagem = url };
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService CancelarMatriculaSolucaoEducacional(int idMatricula)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                (new ManterSolucaoEducacionalService()).CancelarMatriculaSolucaoEducacional(idMatricula, autenticacao);
                return new RetornoWebService() { Erro = 0, Mensagem = "" };
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch (Exception ex)
            {
                return new RetornoWebService
                {
                    Erro = 1,
                    Mensagem = "Erro ao processar a solicitação. Favor entrar em contato com o suporte",
                    Stack = ex.Message + " - " + ex.StackTrace
                };
            }
        }
        #endregion

        #region Consultar Publicacoes Saber

        [WebMethod]
        public List<DTOPublicacaoSaber> ListarUltimasPublicacoesSaber()
        {
            try
            {
                ManterPublicacaoSaber manterPublicacaoSaber = new ManterPublicacaoSaber();
                return manterPublicacaoSaber.ConsultarPublicacaoSaber();
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Consulta Status Matricula Solucao Educacional


        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTODisponibilidadeSolucaoEducacional ConsultarDisponibilidadeMatriculaSolucaoEducacional(int pUsuario,
            int SolucaoEducacional)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                var retorno = new ConsultarStatusMatriculaSolucaoEducacional().ConsultarDisponibilidadeMatriculaSolucaoEducacional
                        (pUsuario, SolucaoEducacional);

                return retorno;


            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTODisponibilidadeSolucaoEducacional ConsultarDisponibilidadeTurma(int usuarioId,
            int turmaId)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                return
                    new ConsultarStatusMatriculaSolucaoEducacional().ConsultarDisponibilidadeTurma
                        (usuarioId, turmaId);
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOSolucoes ConsultarDisponibilidadeMatriculaPorUsuario(int pUsuario, int idSolucaoEducacional, int cargaHoraria)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                return new ConsultarStatusMatriculaSolucaoEducacional().ConsultarDisponibilidadeMatriculaPorUsuario(pUsuario, idSolucaoEducacional, cargaHoraria);
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService EntrarNaFilaDeEsperaMatriculaTurmas(int pUsuario, int idOferta)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                return new ConsultarStatusMatriculaSolucaoEducacional().EntrarNaFilaDeEspera(pUsuario, idOferta);
            }
            catch
            {
                return null;
            }
        }


        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOSolucoes ConsultarTurmaPorSolucaoEducacional(int idSolucaoEducacional, int idOferta)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                return new ConsultarStatusMatriculaSolucaoEducacional().ConsultarTurmaPorSolucaoEducacional(idSolucaoEducacional, idOferta, autenticacao.Login);
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOAgenda ConsultarMinhaAgenda(int pUsuario, int mes, int ano)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao))) throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                return (new ConsultarMinhaPagina()).ObterAgenda(pUsuario, mes, ano);
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOMinhaPagina ConsultarMinhaPagina(int pUsuario)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao))) throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                return (new ConsultarMinhaPagina()).ObterMinhaPagina(pUsuario);
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<DTOItemMeusCursos> ConsultarMeusCursos(int pUsuario, bool? portalNovo = false)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                return new ConsultarMeusCursos().ObterMeusCursos(pUsuario, portalNovo ?? false);
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOCursosUsuarioPorCPF ConsultarCursosUsuarioPorCPF(string cpf)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return (new ManterUsuarioServices()).ConsultarCursosUsuarioPorCPF(cpf);
            }
            catch
            {
                return null;
            }

        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOUsuarioPortalConselhos ConsultarUsuarioConselhosPorCPF(string cpf)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            
            return (new ManterUsuarioServices()).ConsultarUsuarioConselhos(cpf);            
        }


        #endregion

        #region Tag

        [WebMethod]
        public List<DTOTag> ListarTag()
        {
            try
            {
                return (new ConsultaTags()).ConsultarTags().OrderBy(x => x.NumeroNivel).ToList();
            }
            catch
            {
                return null;
            }

        }
        #endregion

        #region Bibliotecas

        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<DTOSistemaExterno> ConsultarSistemasExternos(int idUsuario)
        {
            return (List<DTOSistemaExterno>)new SistemaExternoServices().ObterTodosPorUsuario(idUsuario);
        }

        #endregion


        #region Questionário

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOQuestionarioParticipacao QuestionarioConsultar(DTOCadastroQuestionarioParticipacao pQuestionario)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            return new ManterQuestionarioParticipacao().ListarQuestionarioParticipacao(pQuestionario, autenticacao.Login);
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOQuestionarioParticipacao QuestionarioAvulsoDemandasConsultar(int idQuestionario, int idUsuario,
            int tipo, bool edicao)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            return new ManterQuestionarioParticipacao().ListarQuestionarioAvulsoDemandasParticipacao(idQuestionario,
                idUsuario, tipo, autenticacao, edicao);
        }


        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOQuestionarioParticipacao QuestionarioDemandasVisualizar(int questionarioParticipacao)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return (new BP.Services.SgusWebService.ManterQuestionarioParticipacao()).ListarRespostasQuestionarioParticipacao(questionarioParticipacao, autenticacao);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }


        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOQuestionarioParticipacao QuestionarioInformarRespostas(DTOQuestionarioParticipacao pQuestionario)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                var questionarioParticipacao = new DTOQuestionarioParticipacao();

                Tuple<bool, decimal, string> retornoQuestionario;
                // apenas para provas das trilhas (Aprovado, Nota, Mensagem)

                new ManterQuestionarioParticipacao(autenticacao.Login).InformarRespostas(pQuestionario, autenticacao.Login,
                    out retornoQuestionario, ref questionarioParticipacao);

                //Virá dados apenas quando for do tipo prova para trilha. De SE ainda não está sendo tratado.
                var msg = retornoQuestionario.Item1 + ";" + string.Format("{0:0.00}", retornoQuestionario.Item2) + ";" +
                          retornoQuestionario.Item3;


                questionarioParticipacao.Erro = 0;
                questionarioParticipacao.Mensagem = msg;

                return questionarioParticipacao;
            }
            catch (Exception ex)
            {
                return new DTOQuestionarioParticipacao() { Erro = 1, Mensagem = ex.Message };
            }
        }


        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOQuestionarioParticipacao QuestionarioEvolutivoConsultar(DTOCadastroQuestionarioParticipacao pQuestionario)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return (new ManterQuestionarioParticipacao()).GerarQuestionarioEvolutivo(pQuestionario, autenticacao);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
            catch
            {
                return null;
            }


        }
        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOQuestionarioParticipacao ConsultarResultadoProvaTrilha(int IdUsuario, int IdTrilhaNivel)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return (new ManterQuestionarioParticipacao()).ConsultarResultadoProvaTrilha(IdUsuario, IdTrilhaNivel);
            }
            catch
            {
                return null;
            }


        }
        #endregion

        #region Notificações

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService RegistrarVisualizacaoNotificacao(List<int> listaNotificacao)
        {

            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                (new ManterNotificacao()).RegistrarVisualizacao(listaNotificacao, autenticacao);
                return new RetornoWebService() { Erro = 0, Mensagem = "" };
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = msgErroPadrao };
            }
        }


        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<DTONotificacao> ConsultarNotificacaoPorUsuario(int pIdUsuario, DateTime? DataGeracao, bool ocultarVisualizadas = true)
        {

            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {

                var notificacoes = (new ManterNotificacao()).ConsultarNotificacaoPorUsuario(pIdUsuario, DataGeracao, ocultarVisualizadas).ToList();
                return notificacoes;
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTONotificacoes ConsultarNotificacaoPorUsuarioPaginado(int pIdUsuario, DateTime? DataGeracao, int pagina, int limitePorPagina, bool ocultarVisualizadas = true)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return (new ManterNotificacao()).ConsultarNotificacaoPorUsuarioPaginado(pIdUsuario, DataGeracao, pagina, limitePorPagina, ocultarVisualizadas);
            }
            catch
            {
                return null;
            }
        }



        #endregion

        #region Drupal

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService PublicaConteudo(string Link, string Texto, DateTime? Data, int[] ListaUF, int[] ListaNivelOcupacional, int[] ListaPerfil, int[] ListaTag) // TODO: Remover parâmetro ListaTag e corrigir o envio no portal.
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                new ManterNotificacao().PublicarNotificacao(Link, Texto, Data, ListaUF, ListaNivelOcupacional, ListaPerfil);
                return new RetornoWebService() { Erro = 0, Mensagem = "" };
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = msgErroPadrao };
            }

        }


        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService RegistraBusca(int idUsuario, string Texto)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                new RegistraLogs().RegistraBusca(idUsuario, Texto);
                return new RetornoWebService() { Erro = 0, Mensagem = "" };
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = msgErroPadrao };
            }

        }
        #endregion

        #region Trilhas

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService CadastrarAtividadeFormativaParticipacao(int idUsuarioTrilha, int idTopicoTematico, string textoParticipacao,
                                                                         string nomeOriginalArquivo, string tipoArquivo, string nomeArquivoServidor, int tipoParticipacao)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                atividadeFormativaServices.CadastrarAtividadeFormativa(idUsuarioTrilha, idTopicoTematico, textoParticipacao,
                                                                                        nomeOriginalArquivo, tipoArquivo, nomeArquivoServidor, tipoParticipacao, autenticacao);
                return new RetornoWebService() { Erro = 0, Mensagem = "" };
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = "Erro ao processar a solicitação. Favor entrar em contato com o suporte" };
            }

        }


        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOBuscaAtividadeFormativa ConsultaAtividadeFormativaParticipacao(int pUsuarioTrilha, int pTopicoTematico)
        {
            if (!segurancaAutenticacao.AutenticaUsuario(autenticacao))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                IList<TrilhaAtividadeFormativaParticipacao> lstAtividade =
                atividadeFormativaServices.BuscarListaAtividadeFormativaParticipacao(pUsuarioTrilha, pTopicoTematico);

                List<DTOBuscaAtividadeFormativa> lstResult = new List<DTOBuscaAtividadeFormativa>();
                foreach (TrilhaAtividadeFormativaParticipacao atv in lstAtividade)
                {
                    lstResult.Add(new DTOBuscaAtividadeFormativa
                    {

                        ID = atv.ID,
                        NomeArquivoParticipacaoNoServidor = atv.FileServer == null ? "" : atv.FileServer.NomeDoArquivoNoServidor,
                        NomeOriginalArquivoParticipacao = atv.FileServer == null ? "" : atv.FileServer.NomeDoArquivoOriginal,
                        TipoArquivoParticipacao = atv.FileServer == null ? "" : atv.FileServer.TipoArquivo,
                        TextoParticipacao = (string.IsNullOrWhiteSpace(atv.TextoParticipacao) ? "" : atv.TextoParticipacao),
                        TopicoTematico = atv.TrilhaTopicoTematico.Nome,
                        UsuarioOrigem = atv.UsuarioTrilha.Usuario.Nome

                    });
                }

                return lstResult.FirstOrDefault();
            }
            catch
            {
                return null;
            }

        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        [Obsolete]
        public DTOMatriculaTrilha MatriculaTrilha(int pID_Usuario, int pID_TrilhaNivel)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                var trilha = matriculaTrilhaServices.RegistrarMatriculatrilha(pID_Usuario, pID_TrilhaNivel, autenticacao);
                var retornoMatriculaTrilha = new DTOMatriculaTrilha();
                if (trilha != null)
                {
                    retornoMatriculaTrilha.TrilhaId = trilha.ID;
                }
                return retornoMatriculaTrilha;
            }
            catch (AcademicoException ex)
            {
                return new DTOMatriculaTrilha() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new DTOMatriculaTrilha() { Erro = 1, Mensagem = "Erro ao processar a solicitação. Favor entrar em contato com o suporte" };
            }

        }


        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService CancelarMatriculaTrilha(int idMatricula)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                matriculaTrilhaServices.CancelarMatriculaTrilha(idMatricula, autenticacao);
                return new RetornoWebService() { Erro = 0, Mensagem = "" };
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = "Erro ao processar a solicitação. Favor entrar em contato com o suporte" };
            }

        }


        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService CancelarMatriculaCapacitacao(int idMatricula)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                new ManterMatriculaCapacitacaoService().CancelarMatriculaCapacitacao(idMatricula, autenticacao);
                return new RetornoWebService() { Erro = 0, Mensagem = "" };
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = "Erro ao processar a solicitação. Favor entrar em contato com o suporte" };
            }

        }

        #endregion

        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<ItemTrilhaAutoIndicativaDTO> ConsultarSolucaoEducacionalAutoIndicativa(int Id_usuario, int Id_TrilhaNivel, int Id_TopicoTematico)
        {
            if (!segurancaAutenticacao.AutenticaUsuario(autenticacao))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return viewTrilhaServices.BuscarSolucoesEducacionaisAutoIndicativas(Id_usuario, Id_TrilhaNivel, Id_TopicoTematico).ToList();
            }
            catch
            {
                return null;
            }

        }


        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService SolicitarNovaProvaTrilha(int pUsuarioTrilha)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new AcademicoException("Usuário não autenticado pelo sistema.");
            try
            {
                (new BP.ManterUsuarioTrilha()).SolicitarNovaProvaTrilha(pUsuarioTrilha, autenticacao);

                return new RetornoWebService() { Erro = 0, Mensagem = "" };

            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = "Erro ao processar a solicitação. Favor entrar em contato com o suporte" };
            }
        }

        /*
                [WebMethod]
                [SoapHeader("autenticacao")]
                public RetornoWebService CadastrarItemTrilhaParticipacao(int idUsuarioTrilha, int idItemtrilha,
                    string textoParticipacao,
                    string nomeOriginalArquivo, string tipoArquivo, string nomeArquivoServidor,
                    int tipoParticipacao)
                {
                    if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                        throw new AcademicoException("Usuário não autenticado pelo sistema.");
                    try
                    {
                        (new ManterItemTrilhaParticipacao()).CadastrarItemTrilhaParticipacao(idUsuarioTrilha, idItemtrilha,
                            textoParticipacao, nomeOriginalArquivo, tipoArquivo, nomeArquivoServidor, tipoParticipacao,
                            autenticacao.Login);

                        return new RetornoWebService() { Erro = 0, Mensagem = "" };

                    }
                    catch (AcademicoException ex)
                    {
                        return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
                    }
                    catch (Exception ex)
                    {
                        return new RetornoWebService()
                        {
                            Erro = 1,
                            Mensagem = "Erro ao processar a solicitação. Favor entrar em contato com o suporte"
                        };
                    }
                }*/

        #region "Pagamentos"

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOUsuarioPagamento GerarPagamento(DTOGerarPagamento dadosPagamento)
        {

            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                ManterUsuarioPagamento manterUsuarioPagamento = new ManterUsuarioPagamento();
                return manterUsuarioPagamento.GerarPagamento(dadosPagamento, autenticacao);

            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService RegistrarPagamentoInformado(string cpf, string nossoNumero, string autenticacaoBancaria,
                                                               string dataPagamentoInformado, string valorPagamentoInformado)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                ManterUsuarioPagamento manterUsuarioPagamento = new ManterUsuarioPagamento();
                manterUsuarioPagamento.RegistrarPagamentoInformado(cpf, nossoNumero, autenticacaoBancaria, dataPagamentoInformado, valorPagamentoInformado);

                return new RetornoWebService() { Erro = 0, Mensagem = string.Empty };
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = msgErroPadrao };
            }
        }

        #endregion

        #region Ranking

        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<DTORankingTrilha> ConsultarRankingTrilha(int pIdTrilha, int pIdTrilhaNivel)
        {

            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return new RankingTrilhas().ConsultarRankingTrilha(pIdTrilha, pIdTrilhaNivel).ToList();
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Registro de Logs

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService RegistraAcessoTrilha(int IdUsuarioTrilha)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                new RegistraLogs().RegistraAcessoTrilha(IdUsuarioTrilha);
                return new RetornoWebService() { Erro = 0, Mensagem = string.Empty };
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = "Erro ao processar a solicitação. Favor entrar em contato com o suporte" };
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService RegistraAcessoSolucaoEducacional(int IdMatriculaTurma)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                new RegistraLogs().RegistraAcessoSolucaoEducacional(IdMatriculaTurma);
                return new RetornoWebService() { Erro = 0, Mensagem = string.Empty };
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = "Erro ao processar a solicitação. Favor entrar em contato com o suporte" };
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService RegistraSolicitacaoContato(string CPF, string Nome, string Email, string Assunto, string Mensagem, string Ip)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                new RegistraLogs().RegistraSolicitacaoContato(CPF, Nome, Email, Assunto, Mensagem, Ip);
                return new RetornoWebService() { Erro = 0, Mensagem = string.Empty };
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = "Erro ao processar a solicitação. Favor entrar em contato com o suporte" };
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService RegistraAcessoPagina(int IdUsuario, string URL, string NomePagina, string IP)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                // new RegistraLogs().RegistraAcessoPagina(IdUsuario, URL, NomePagina, IP);
                return new RetornoWebService() { Erro = 0, Mensagem = string.Empty };
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = "Erro ao processar a solicitação. Favor entrar em contato com o suporte" };
            }
        }

        #endregion

        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<DTOItemHistoricoAcademico> ConsultarHistoricoAcademico(int Id_Usuario)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                return historicoAcademicoServices.ConsultarHistorico(Id_Usuario);
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<DTOItemHistoricoTutoria> ConsultarHistoricoTutoria(int Id_Usuario)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                return historicoAcademicoServices.ConsultarHistoricoTutoria(Id_Usuario);
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOHistoricoExtraCurricular ConsultarExtraCurricular(int Id_AtividadeExtraCurricular)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                return (new AtividadeExtraCurricularServices()).ConsultarExtraCurricular(Id_AtividadeExtraCurricular);
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOTemplate ConsultarTemplatePorId(int idTemplate)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                return (new TemplateServices()).ConsultarTemplatePorId(idTemplate);
            }
            catch
            {
                return null;
            }
        }


        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOSistemaExterno ConsultarLinkSistemaExternoPorId(int idSistemaExterno, int idUsuario)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                return (new ManterUsuarioServices()).recuperaSistemaExterno(idSistemaExterno, idUsuario);
            }
            catch
            {
                return null;
            }
        }

        #region Gestor UC

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOGestorUC ConsultarDisponibilidadeGestorUC(int idSolucaoEducacional, string cpf, string nome,
            string idOferta, string idTurma)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                return new ManterMatriculaOfertaService().ConsultarDisponibilidadeGestorUC(idSolucaoEducacional, cpf,
                    nome, string.IsNullOrEmpty(idOferta) ? 0 : int.Parse(idOferta),
                    string.IsNullOrEmpty(idTurma) ? 0 : int.Parse(idTurma), autenticacao.Login);
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<DTOListaProgramaPrograma> ConsultarProgramasGestorUC(int idGestor, string filtroNomePrograma)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                return new ManterProgramaService().ConsultarProgramaGestorUC(idGestor, filtroNomePrograma);
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<DTOListaProgramaPrograma> ConsultarProgramas(string filtroPrograma)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                return new ManterProgramaService().ConsultarProgramas(filtroPrograma);
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<DTOCapacitacao> ConsultarCapacitacoes(int idUsuario, int idCapacitacao = 0, int idPrograma = 0, string nome = "")
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                return new ManterCapacitacaoService().ConsultarCapacitacoes(idUsuario, idCapacitacao, idPrograma, nome);
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService ConsultarVeracidadeCertificado(string codigo, int idUsuario = 0)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                var retorno = new ManterCertificadoService().ConsultarVeracidadeCertificado(codigo, idUsuario);
                if (retorno == true)
                {
                    return new RetornoWebService() { Erro = 0, Mensagem = "Certificado válido" };
                }
                else
                {
                    return new RetornoWebService() { Erro = 1, Mensagem = "Certificado inválido" };
                }


            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = "Erro ao processar a solicitação. Favor entrar em contato com o suporte" };
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService MatriculaCapacitacao(int idUsuario, int idCapacitacao, int idTurma)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                new ManterCapacitacaoService().MatriculaCapacitacao(idUsuario, idCapacitacao, idTurma);
                return new RetornoWebService() { Erro = 0, Mensagem = string.Empty };
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = "Erro ao processar a solicitação. Favor entrar em contato com o suporte" };
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOListaProgramaPrograma ConsultarProgramaMatriculaGestorUC(int idGestor, int idPrograma, string filtroAluno)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                return new ManterProgramaService().ConsultarProgramaMatriculaGestorUC(idGestor, idPrograma, filtroAluno);
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService MatriculaProgramaGestorUC(int idPrograma, string CPF, string DataInicio, string DataFim)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                new ManterProgramaService().MatriculaProgramaGestorUC(idPrograma, CPF, DataInicio, DataFim, autenticacao.Login);
                return new RetornoWebService() { Erro = 0, Mensagem = string.Empty };
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = "Erro ao processar a solicitação. Favor entrar em contato com o suporte" };
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOConsultarStatusMatricula ConsultarStatusMatricula()
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                return new ManterMatriculaOfertaService().ConsultarStatusMatricula();
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService MatriculaSolucaoEducacionalGestorUC(int idOferta, string CFP, int idStatusMatricula, string idTurma)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                new ManterMatriculaOfertaService().MatriculaSolucaoEducacionalGestorUC(idOferta, CFP, idStatusMatricula, (!string.IsNullOrEmpty(idTurma) ? int.Parse(idTurma) : 0), autenticacao.Login);
                return new RetornoWebService() { Erro = 0, Mensagem = string.Empty };
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = "Erro ao processar a solicitação. Favor entrar em contato com o suporte" };
            }
        }


        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService CancelaMatriculaSolucaoEducacionalGestorUC(int idMatriculaOferta)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                new ManterMatriculaOfertaService().CancelaMatriculaSolucaoEducacionalGestorUC(idMatriculaOferta, autenticacao.Login);
                return new RetornoWebService() { Erro = 0, Mensagem = string.Empty };
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch
            {
                return new RetornoWebService() { Erro = 1, Mensagem = "Erro ao processar a solicitação. Favor entrar em contato com o suporte" };
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public string AlterarStatusMatriculaPorGestorUC(int idMatriculaOferta, int idStatusMatricula)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            string retorno = "O Status da Matrícula da Turma foi Atualizado com Sucesso !";

            var manterMatriculaOferta = new BP.ManterMatriculaOferta();
            MatriculaOferta matriculaOferta = manterMatriculaOferta.ObterMatriculaOfertaPorID(idMatriculaOferta);

            bool ofertaPermiteAlteracaoPeloGestor = true;
            if (matriculaOferta.Oferta.AlteraPeloGestorUC.HasValue)
                ofertaPermiteAlteracaoPeloGestor = matriculaOferta.Oferta.AlteraPeloGestorUC.Value;

            bool atualizar = false;

            if (ofertaPermiteAlteracaoPeloGestor)
            {
                if (idStatusMatricula == (int)enumStatusMatricula.CanceladoAdm)
                {
                    atualizar = true;
                    retorno = "Cancelamento efetuado com sucesso!";
                }
                else if (idStatusMatricula == (int)enumStatusMatricula.CanceladoAluno)
                {
                    //O usuário do admin, pode ver o status do Cancelado/Aluno, mas nunca pode setar esse status
                    retorno = "Apenas o aluno pode atribuir o status de cancelado pelo aluno";
                }
                else
                {
                    atualizar = true;
                }
            }
            else
            {
                if (idStatusMatricula == (int)enumStatusMatricula.PendenteConfirmacaoAluno || idStatusMatricula == (int)enumStatusMatricula.Inscrito)
                {
                    atualizar = true;
                }
                else if (idStatusMatricula == (int)enumStatusMatricula.CanceladoAdm)
                {
                    atualizar = true;
                    retorno = "Cancelamento efetuado com sucesso!";
                }
                else
                    retorno = "Alteração não permitida";
            }


            if (atualizar)
            {
                if (matriculaOferta.MatriculaTurma != null && matriculaOferta.MatriculaTurma.Count > 0)
                {
                    //matriculaOferta.MatriculaTurma.Clear();
                }
                matriculaOferta.StatusMatricula = (enumStatusMatricula)idStatusMatricula;
                matriculaOferta.DataStatusMatricula = DateTime.Now;
                manterMatriculaOferta.AtualizarMatriculaOferta(matriculaOferta, false);
            }
            return retorno;
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public string AlterarTurmaPorGestorUC(int idMatriculaOferta, int idTurma)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");


            return new ManterMatriculaOfertaService().ManterStatusMatriculaGestorUC(idMatriculaOferta, idTurma);
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOCursosPorCategoria ConsultarCursosPorCategoria(int idNoCategoriaConteudo, string cpf, string nome, string somenteComInscricoesAbertas)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            return new ConsultarStatusMatriculaSolucaoEducacional().ConsultarCursosPorCategoria(idNoCategoriaConteudo, cpf, nome, !string.IsNullOrEmpty(somenteComInscricoesAbertas));
        }
        #endregion

        #region Processos

        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<DTOProcessoInfo> ConsultarProcessosPermitidos()
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return new ManterProcesso().ConsultarProcessosPermitidosInscricao(autenticacao.Login).ToList();
            }
            catch
            {
                return null;
            }
        }

        //[WebMethod]
        //[SoapHeader("autenticacao")]
        //public List<DTOEtapaAAnalisar> ConsultarEtapasAAnalisar(string filtro, int status = 0, string data = "")
        //{
        //    if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
        //        throw new Exception("Usuário não autenticado pelo sistema.");

        //    try
        //    {
        //        DateTime? date = null;

        //        if (!string.IsNullOrWhiteSpace(data))
        //        {
        //            try
        //            {
        //                var dateSplit = data.Split('/');

        //                date = new DateTime(int.Parse(dateSplit[2]), int.Parse(dateSplit[1]), int.Parse(dateSplit[0]));
        //            }
        //            catch (Exception)
        //            {
        //                // ignored
        //            }
        //        }

        //        var retorno = new ManterProcesso().ConsultarEtapasAAnalisar(autenticacao.Login, filtro, status, date).ToList();

        //        return retorno;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService EncaminharEtapaNucleo(int IdEtapaResposta, int IdEtapaPermissaoNucleo, int idUsuario, string txJustificativa)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            RetornoWebService retorno = new RetornoWebService();
            try
            {
                new ManterProcesso().SalvarEncaminhamentoEtapaUsuario(IdEtapaResposta, IdEtapaPermissaoNucleo, idUsuario, txJustificativa);

                retorno.Mensagem = "A Etapa foi encaminhada com sucesso";
                return retorno;
            }
            catch
            {
                retorno.Mensagem = "Erro ao encaminhar etapa";
                retorno.Erro = 1;

                return retorno;
            }
        }


        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService RetornoEncaminhamentoEtapaNucleo(int idEtapaEncamihamentoUsuario, int statusEncaminhamento, string txJustificativa)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            RetornoWebService retorno = new RetornoWebService();

            try
            {
                var encaminhamentoEtapaUsuario = new ManterProcesso().RetornoEncaminhamentoEtapaUsuario(idEtapaEncamihamentoUsuario, statusEncaminhamento, txJustificativa);

                if (encaminhamentoEtapaUsuario.StatusEncaminhamento == (int)enumStatusEncaminhamentoEtapa.Aprovado)
                {
                    retorno.Mensagem = "O Encaminhamento da Etapa foi Aceita";
                }
                else
                {
                    retorno.Mensagem = "O Encaminhamento da Etapa foi Negada";
                }

                return retorno;
            }
            catch
            {
                retorno.Mensagem = "Erro na confirmação do encaminhamento da etapa";
                retorno.Erro = 0;

                return retorno;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public string CancelaEncaminhamentoEtapaNucleoPendentes()
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            RetornoWebService retorno = new RetornoWebService();

            try
            {
                new ManterProcesso().CancelaEncaminhamentoEtapaUsuarioPendentes();
                return "processado";
            }
            catch
            {
                return "";
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        [Versao("1.3.0")]
        public List<DTOEstatisticaHomePortal> ConsultarUCPortalHomeStats()
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return new BP.ManterEstatistica().ConsultarUCPortalHomeStats().ToList();
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOAnalisarEtapasProcesso AnalisarEtapasProcesso(int idProcessoResposta, int idProcesso = 0)
        {
            DTOAnalisarEtapasProcesso retorno;
            try
            {
                if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                    throw new Exception("Usuário não autenticado pelo sistema.");

                var manterProcesso = new ManterProcesso();

                if (idProcesso == 0)
                {
                    if (!manterProcesso.VerificarPermissaoAnalise(idProcessoResposta, autenticacao.Login))
                        return null;

                    retorno = manterProcesso.AnalisarEtapasProcesso(idProcessoResposta, autenticacao.Login, 1, 1);
                    return retorno;
                }

                if (!manterProcesso.VerificarPermissaoInscricao(idProcesso, autenticacao.Login))
                    return null;

                retorno = manterProcesso.ObterPrimeiraEtapa(idProcesso, autenticacao.Login);
                return retorno;
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public string ObterHistoricoAcademico(int idUsuario)
        {
            var usuario = new BMUsuario().ObterPorId(idUsuario);
            var campo = new Campo() { TipoCampo = (byte)enumTipoCampo.Field, TipoDado = (byte)enumCampoUsuario.HistoricoAcademico };

            //var url = new ManterProcesso().ObterUrlHistoricoAcademicoPorUsuario(usuario);
            return "";
        }

        // Semelhante a análise de etapas mas tem regras diferentes para a validação de quem pode visualizar as etapas
        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOAnalisarEtapasProcesso VisualizarEtapasHistorico(int idProcessoResposta, int idEtapaResposta)
        {
            try
            {
                if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                    throw new Exception("Usuário não autenticado pelo sistema.");

                var manterProcesso = new ManterProcesso();

                if (!manterProcesso.VerificarPermissaoVisualizacaoHistorico(idProcessoResposta, autenticacao.Login, idEtapaResposta))
                    return null;

                return manterProcesso.PegaEtapasHistorico(idProcessoResposta, autenticacao.Login, idEtapaResposta);
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTODetalhamentoProcesso ObterDetalhamentoProcesso(int idProcessoResposta)
        {
            if (autenticacao == null || !segurancaAutenticacao.AutenticaUsuario(autenticacao))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                var manterProcesso = new ManterProcesso();

                var resultado = manterProcesso.ObterDetalhamentoProcesso(idProcessoResposta);

                return resultado;
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        public DTOManterUsuario AutenticarUsuarioPorToken(string token)
        {
            try
            {
                var bmTokenAcesso = new BMTokenAcesso();
                var tokenAcesso = new BMTokenAcesso().ObterTokenValidoMd5TokenAcesso(token);
                var usuario = new ManterUsuarioServices().ConsultarUsuarioPorToken(tokenAcesso.Token);
                bmTokenAcesso.repositorio.Excluir(tokenAcesso);

                return usuario;
            }
            catch
            {
                throw new AcademicoException("Usuário não encontrado");
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<DTOCampo> ConsultarFormularioEtapa(int etapaId)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                Usuario usuario = new BMUsuario().ObterPorCPF(autenticacao.Login);
                return (new ManterProcesso()).ConsultarCamposEtapa(etapaId, usuario);
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService ResponderFormularioEtapa(DTOResponderFormulario dadosFormulario, bool rascunho)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return
                    new ManterProcesso().ResponderFormularioEtapa(dadosFormulario.IdEtapaResposta,
                        dadosFormulario.Respostas, dadosFormulario.Situacao, autenticacao.Login,
                        dadosFormulario.IdAnalista, dadosFormulario.PermissoesNucleo, dadosFormulario.IdCargo, rascunho);

            }
            catch (Exception e)
            {
                return null;
            }
        }


        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOMeusProcessos ConsultarMeusProcessos()
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return (new BP.Services.SgusWebService.ManterProcesso()).ConsultarMeusProcessos(autenticacao.Login);
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOVisualizarProcessos AcompanharMeusProcessos(int? numero, int? status, int? demandanteId, int? processoId, int? etapaId, bool carregarFiltros = false, int paginaAtual = 1)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            var retorno = new DTOVisualizarProcessos
            {
                Processos = new ManterProcesso().AcompanharMeusProcessos(
                    autenticacao.Login, numero, (enumStatusDemanda?)status, demandanteId, processoId, etapaId)
            };

            if (carregarFiltros)
            {
                retorno.ConfigurarFiltros();
            }

            retorno.Paginar(paginaAtual, 10);

            return retorno;
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOVisualizarProcessos ConsultarEtapasHistorico(int? numero, int? status, int? demandanteId, int? processoId, int? etapaId, bool carregarFiltros = false, int paginaAtual = 0)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            var retorno = new DTOVisualizarProcessos
            {
                Processos = new ManterProcesso().ConsultarEtapasHistorico(autenticacao.Login, numero,
                    (enumStatusDemanda?)status, demandanteId, processoId, etapaId).ToList()
            };

            if (carregarFiltros)
            {
                retorno.ConfigurarFiltros();
            }

            retorno.Paginar(paginaAtual, 10);

            return retorno;
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService EfetuarInscricaoProcesso(DTOResponderFormulario dadosFormulario, bool rascunho)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return (new ManterProcesso()).EfetuarInscricao(dadosFormulario.IdEtapa, dadosFormulario.IdCargo, dadosFormulario.Respostas,
                    dadosFormulario.inscricaoCPF, rascunho);
            }
            catch(Exception ex)
            {
                return new RetornoWebService {
                    Erro = 1,
                    Mensagem = ex.Message,
                    Stack = ex.StackTrace
                };
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOProcessoResposta ConsultarStatusProcesso(int processoId)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return (new ManterProcesso()).ConsultarStatusProcesso(autenticacao.Login, processoId);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Acessa usuário logado no moodle
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [WebMethod]
        [SoapHeader("autenticacao")]
        public string AcessarCursoMoodle(string url, string cpf, string senha)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                #region Tratando url

                string urlMoodle = "";
                var bmUsuario = new BMUsuario();
                senha = CriptografiaHelper.Decriptografar(senha);

                Usuario usuario = new Usuario();
                usuario = bmUsuario.ObterPorCPF(cpf);

                if (bmUsuario.AutenticarUsuario(cpf, senha))
                {
                    var consulta = new ConsultarMeusCursos();
                    var fornecedor = new BMFornecedor().ObterPorID((int)enumFornecedor.MoodleSebrae);

                    urlMoodle = consulta.ConsultarLinkAcessoFornecedor(fornecedor, usuario, url);
                }

                #endregion

                #region Matricular Aluno

                int idCodigoCurso = 0;

                if (int.TryParse(url, out idCodigoCurso))
                {
                    Dominio.Classes.Moodle.SgusMoodleOferta oferta = new BMSgusMoodleOferta().ObterUltimaOfertaPorCodigoCurso(idCodigoCurso);
                    if (oferta != null)
                    {
                        int id = oferta.ID;
                        Oferta ofertaSgus = new BMOferta().ObterPorFiltro(null, oferta.ID.ToString(), 0).LastOrDefault();
                        this.MatriculaSolucaoEducacional(usuario.ID, ofertaSgus.SolucaoEducacional.ID, ofertaSgus.ID);
                    }
                    else
                    {
                        throw new Exception("Não existe oferta para este curso");
                    }
                }

                #endregion

                return WebUtility.HtmlDecode(urlMoodle);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        #endregion

        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<DTOProtocolo> ListarArquivados(string cpfUsuario, string protocolo, string discriminacao, string dtaEnvio, string dtaRecebimento, string remetente, string destinatario, string statusId)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            List<DTOProtocolo> retorno;

            try
            {
                var usuario = new BMUsuario().ObterPorCPF(cpfUsuario);

                if (usuario.IsGestor() || usuario.IsGestorDeProtocolo())
                    retorno = (List<DTOProtocolo>)new ManterProtocolo()
                        .ListarHistorico(cpfUsuario, protocolo, discriminacao, dtaEnvio, dtaRecebimento, remetente, destinatario, statusId, 1);
                else
                    retorno = (List<DTOProtocolo>)new ManterProtocolo()
                        .ListarHistorico(cpfUsuario, protocolo, discriminacao, dtaEnvio, dtaRecebimento, remetente, destinatario, statusId);
            }
            catch
            {
                throw;
            }

            return retorno;
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<DTOProtocolo> ListarProtocoloEnviado(string cpfUsuario, string protocolo, string discriminacao, string dtaEnvio, string dtaRecebimento, string remetente, string destinatario)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            List<DTOProtocolo> retorno;

            try
            {
                retorno = (List<DTOProtocolo>)new ManterProtocolo()
                    .ListarProtocoloEnviado(cpfUsuario, protocolo, discriminacao, dtaEnvio, dtaRecebimento, remetente, destinatario);
            }
            catch
            {
                throw;
            }

            return retorno;
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<DTOProtocolo> ListarProtocoloAnalisar(string cpfUsuario, string protocolo, string discriminacao, string dtaEnvio, string dtaRecebimento, string remetente, string destinatario)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            List<DTOProtocolo> retorno;

            try
            {
                retorno = (List<DTOProtocolo>)new ManterProtocolo()
                    .ListarProtocoloAnalisar(cpfUsuario, protocolo, discriminacao, dtaEnvio, dtaRecebimento, remetente, destinatario);
            }
            catch
            {
                throw;
            }

            return retorno;
        }


        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<DTOProtocolo> AcompanharProtocolo(int numeroProtocolo)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                var listaProtocolo = (new ManterProtocolo()).AcompanharProtocolo(numeroProtocolo);
                return listaProtocolo;
            }
            catch (AcademicoException exception)
            {
                throw exception;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoProtocolo NovoProtocolo(string cpf)
        {
            RetornoProtocolo retorno = new RetornoProtocolo();

            try
            {
                var usuario = new BMUsuario().ObterPorCPF(cpf);
                var usuarioRementente = new DTOUsuario() { ID = usuario.ID, Nome = usuario.Nome };
                retorno.UsuarioRemetente = usuarioRementente;

                var usuariosDestinatario = new ManterProtocolo().ObterTodosUsuariosDestinatarios();

                retorno.UsuariosDestinatarios = usuariosDestinatario.Select(x => new DTOUsuario() { Nome = x.Nome, ID = x.ID }).OrderBy(x => x.Nome).ToList();

                return retorno;
            }
            catch (AcademicoException exception)
            {
                retorno.MensagemRetorno = exception.Message;
                retorno.ApresentarErro = true;

                return retorno;
            }
            catch (Exception)
            {
                retorno.MensagemRetorno = "Ocorreu um erro ao pesquisar os protocolos do usuário.";
                retorno.ApresentarErro = true;

                return retorno;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService ManterArquivarProtocolo(int numeroProtocolo)
        {
            VerificarAutenticacao();

            try
            {
                new ManterProtocolo().ArquivarProtocolo(numeroProtocolo);

                return new RetornoWebService
                {
                    Mensagem = "Protocolo Arquivado com Sucesso",
                    Erro = 0
                };
            }
            catch (Exception e)
            {
                return new RetornoWebService
                {
                    Mensagem = e.Message,
                    Erro = 1,
                    Stack = e.StackTrace
                };
            }
        }


        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoProtocolo ManterProtocolo(DtoProtocoloNovo dtoProtocoloNovo)
        {
            VerificarAutenticacao();
            VerificarAutorizacaoSobreProtocolosArquivados(dtoProtocoloNovo);

            var retorno = new RetornoProtocolo();
            var bmUsuario = new BMUsuario();

            var manterProtocolo = new ManterProtocolo();
            long numero = 0;
            retorno.ApresentarErro = false;

            try
            {
                var destinatario = bmUsuario.ObterPorId(dtoProtocoloNovo.IdUsuarioDestinatario);
                var remetente = bmUsuario.ObterPorId(dtoProtocoloNovo.IdUsuarioRemetente);
                var dtEnvio = DateTime.Now;

                Protocolo protocolo = new Protocolo()
                {
                    Remetente = remetente,
                    Destinatario = destinatario,
                    Numero = manterProtocolo.GerarNumeroProtocolo(),
                    DataEnvio = dtEnvio,
                    Descricao = dtoProtocoloNovo.Descricao,
                    Arquivado = dtoProtocoloNovo.Arquivado.HasValue ? dtoProtocoloNovo.Arquivado.Value : false
                };

                manterProtocolo.SalvarProtocolo(protocolo);

                // TODO: Refatorar para dentro do manterProtocoloServer
                var manterProtocoloFileService = new BP.ManterProtocoloFileServer();
                foreach (var arquivo in dtoProtocoloNovo.Anexos)
                {
                    var fileServer = new FileServer()
                    {
                        NomeDoArquivoOriginal = arquivo.NomeDoArquivoOriginal,
                        TipoArquivo = arquivo.TipoArquivo,
                        NomeDoArquivoNoServidor = arquivo.NomeDoArquivoNoServidor,
                        MediaServer = true,
                    };

                    var protocoloFileServer = new ProtocoloFileServer()
                    {
                        Protocolo = protocolo,
                        Usuario = remetente,
                        FileServer = fileServer
                    };

                    manterProtocoloFileService.Salvar(protocoloFileServer);
                }

                numero = protocolo.Numero;
                manterProtocolo.NotificarDestinatario(remetente, destinatario, protocolo);
            }
            catch (AcademicoException)
            {
                retorno.MensagemRetorno = "Não foi possível cadastrar o protocolo. Entre em contato com o suporte.";
                retorno.ApresentarErro = true;

                return retorno;
            }
            catch (SmtpException)
            {
                retorno.MensagemRetorno = string.Format("Não foi possível enviar a notificação para o usuário. Protocolo número {0} cadastrado com sucesso !", numero);
                retorno.ApresentarErro = true;

                return retorno;
            }
            catch (Exception e)
            {
                if (numero > 0)
                {
                    retorno.MensagemRetorno = string.Format("Protocolo número {0} cadastrado com sucesso !", numero);
                    retorno.ApresentarErro = false;
                }
                else
                {
                    retorno.MensagemRetorno = e.Message;
                    retorno.ApresentarErro = true;
                }

                return retorno;
            }

            retorno.MensagemRetorno = string.Format("Protocolo número {0} cadastrado com sucesso !", numero);

            return retorno;
        }

        private static void VerificarAutorizacaoSobreProtocolosArquivados(DtoProtocoloNovo dtoProtocoloNovo)
        {
            if (dtoProtocoloNovo.Arquivado.HasValue && dtoProtocoloNovo.Arquivado.Value)
            {
                var usuario = new BMUsuario().ObterPorId(dtoProtocoloNovo.IdUsuarioRemetente);
                if (!usuario.IsGestorDeProtocolo())
                {
                    throw new Exception("Somente gestor de protocolo pode arquivalos.");
                }
            }
        }

        private void VerificarAutenticacao()
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoProtocolo AssinarProtocolo(string cpfValidador, int numeroProtocolo, string despacho, int idProtocolo, List<DTOFileServer> anexos)
        {

            RetornoProtocolo retorno = new RetornoProtocolo();

            try
            {
                var bmProtocolo = new BMProtocolo();
                var bmUsuario = new BMUsuario();
                var usuario = bmUsuario.ObterPorCPF(cpfValidador);

                var protocolo = bmProtocolo.ObterPorId(idProtocolo);

                if (protocolo.Destinatario.ID != usuario.ID)
                {
                    throw new AcademicoException("O usuário não pode assinar esse protocolo");
                }

                protocolo.UsuarioAssinatura = usuario;
                protocolo.DataRecebimento = DateTime.Now;
                protocolo.Despacho = despacho;
                bmProtocolo.Salvar(protocolo);

                var manterProtocoloFileService = new BP.ManterProtocoloFileServer();
                foreach (var arquivo in anexos)
                {
                    var fileServer = new FileServer()
                    {
                        NomeDoArquivoOriginal = arquivo.NomeDoArquivoOriginal,
                        TipoArquivo = arquivo.TipoArquivo,
                        NomeDoArquivoNoServidor = arquivo.NomeDoArquivoNoServidor,
                        MediaServer = true,
                    };

                    var protocoloFileServer = new ProtocoloFileServer()
                    {
                        Protocolo = protocolo,
                        Usuario = usuario,
                        FileServer = fileServer
                    };

                    manterProtocoloFileService.Salvar(protocoloFileServer);
                }

                try
                {
                    new ManterProtocolo().NotificarRemetente(protocolo.Remetente, protocolo.Destinatario, protocolo.Numero);
                }
                catch (Exception)
                {
                    throw new AcademicoException("Não foi possível notificar o usuário, assinatura realizada com sucesso !");
                }

                retorno.MensagemRetorno = "Assinatura realizada com sucesso !";
                return retorno;
            }
            catch (AcademicoException exception)
            {
                retorno.MensagemRetorno = exception.Message;
                retorno.ApresentarErro = true;
                return retorno;
            }
            catch
            {
                retorno.MensagemRetorno = "Ocorreu um erro assinar o protocolo.";
                retorno.ApresentarErro = true;
                return retorno;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoProtocolo ReencaminharProtocolo(DtoProtocoloNovo dtoProtocolo)
        {
            var retorno = new RetornoProtocolo();
            var bmUsuario = new BMUsuario();

            var manterProtocolo = new ManterProtocolo();
            long numero = 0;
            retorno.ApresentarErro = false;

            VerificarAutorizacaoSobreProtocolosArquivados(dtoProtocolo);

            try
            {
                var destinatario = bmUsuario.ObterPorId(dtoProtocolo.IdUsuarioDestinatario);
                var remetente = bmUsuario.ObterPorId(dtoProtocolo.IdUsuarioRemetente);
                var dtEnvio = DateTime.Now;

                Protocolo protocoloPai = dtoProtocolo.IdProtocoloPai.HasValue ?
                    manterProtocolo.ObterProtocoloPorId(dtoProtocolo.IdProtocoloPai.Value) : null;

                Protocolo protocolo = new Protocolo()
                {
                    Numero = protocoloPai.Numero,
                    Descricao = protocoloPai.Descricao,
                    DataEnvio = dtEnvio,
                    Remetente = remetente,
                    Destinatario = destinatario,
                    DespachoReencaminhamento = dtoProtocolo.DespachoReencaminhamento,
                    ProtocoloPai = protocoloPai
                };

                manterProtocolo.SalvarProtocolo(protocolo);

                var manterProtocoloFileService = new BP.ManterProtocoloFileServer();

                if (dtoProtocolo.Anexos.Any())
                {
                    foreach (var arquivo in dtoProtocolo.Anexos)
                    {
                        var fileServer = new FileServer();

                        if (arquivo.IdFileServer == null || arquivo.IdFileServer <= 0)
                        {
                            fileServer = new FileServer()
                            {
                                NomeDoArquivoOriginal = arquivo.NomeDoArquivoOriginal,
                                TipoArquivo = arquivo.TipoArquivo,
                                NomeDoArquivoNoServidor = arquivo.NomeDoArquivoNoServidor,
                                MediaServer = true,
                            };
                        }
                        else if (arquivo.IdFileServer != null)
                        {
                            fileServer = new BP.ManterFileServer().ObterFileServerPorID(arquivo.IdFileServer.Value);
                        }

                        var protocoloFileServer = new ProtocoloFileServer()
                        {
                            Protocolo = protocolo,
                            Usuario = remetente,
                            FileServer = fileServer
                        };

                        manterProtocoloFileService.Salvar(protocoloFileServer);
                    }

                    manterProtocoloFileService.Commit();
                }

                numero = protocolo.Numero;
                manterProtocolo.NotificarDestinatario(remetente, destinatario, protocolo);
            }
            catch (AcademicoException)
            {
                retorno.MensagemRetorno = "Não foi possível cadastrar o protocolo. Entre em contato com o suporte.";
                retorno.ApresentarErro = true;

                return retorno;
            }
            catch (SmtpException)
            {
                retorno.MensagemRetorno = string.Format("Não foi possível enviar a notificação para o usuário. Protocolo número {0} cadastrado com sucesso !", numero);
                retorno.ApresentarErro = true;

                return retorno;
            }
            catch (Exception e)
            {
                if (numero > 0)
                {
                    retorno.MensagemRetorno = string.Format("Protocolo número {0} cadastrado com sucesso !", numero);
                    retorno.ApresentarErro = false;
                }
                else
                {
                    retorno.MensagemRetorno = e.Message;
                    retorno.ApresentarErro = true;
                }

                return retorno;
            }

            retorno.MensagemRetorno = string.Format("Protocolo número {0} cadastrado com sucesso !", numero);

            return retorno;
        }


        /// <summary>
        ///     Retorna informações sobre a disponbilidade de matrícula no nível da trilha além de seu termo de aceite
        /// </summary>
        /// <param name="idNivelTrilha">ID do nível da trilha</param>
        /// <returns></returns>
        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOConsultaDisponibilidadeMatriculaNivelTrilha ConsultaDisponibilidadeMatriculaNivelTrilha(int idNivelTrilha)
        {
            var retorno = new DTOConsultaDisponibilidadeMatriculaNivelTrilha();

            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                retorno = matriculaTrilhaServices.ConsultaDisponibilidadeMatriculaNivelTrilha(autenticacao, idNivelTrilha);

                if (retorno.TermoDeAceite == "")
                {
                    retorno.Mensagem = "Não foi encontrado o nível de trilha.";
                    retorno.Erro = 1;
                }

                return retorno;
            }
            catch (AcademicoException exception)
            {
                retorno.Mensagem = exception.Message;
                retorno.Erro = 1;
                return retorno;
            }
            catch (Exception)
            {
                retorno.Mensagem = "Ocorreu um ao buscar o termo de referência.";
                retorno.Erro = 1;
                return retorno;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOConsultaFormacaoDeMultiplicadores ConsultarFormacaoDeMultiplicadores()
        {
            return new FormacaoDeFormadoresServices().ConsultarFormacaoDeMultiplicadores();
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<DTOFornecedor> ConsultarFornecedores()
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            var manterFornecedor = new BP.ManterFornecedor();

            return manterFornecedor.ObterTodosFornecedoresParaHistoricoAcademico();
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService AtualizarStatusTurmas()
        {
            return new AtualizarStatusTurmasService().AtualizarStatusTurmas();
        }

        [WebMethod]
        public List<DTOConsultarVersoesTeste> ConsultarVersoesTest()
        {
            List<DTOConsultarVersoesTeste> consultarVersoesTeste = new List<DTOConsultarVersoesTeste>();

            MethodInfo[] methodInfos = this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (var m in methodInfos)
            {
                var data = m.GetCustomAttributesData();

                VersaoAttribute versaoAttribute = (VersaoAttribute)Attribute.GetCustomAttribute(m, typeof(VersaoAttribute));

                var version = "1.0.0";
                if (versaoAttribute != null)
                {
                    version = versaoAttribute.version;
                }

                consultarVersoesTeste.Add(
                    new DTOConsultarVersoesTeste
                    {
                        Name = m.Name,
                        Version = version
                    }
                );

            }

            return consultarVersoesTeste;
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService NotificarAlunosAprovadosTurmaQuestionarioPos()
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            return new NotificarAlunosAprovadosTurmaQuestionarioPosService().NotificarAlunosAprovadosTurmaQuestionarioPos();
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public bool FinalizarProcesso(int idProcessoResposta, int idUsuario, string justificativa)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return new ManterProcesso().FinalizarProcessoComJustificativa(idProcessoResposta, idUsuario, justificativa);
            }
            catch (Exception)
            {
                return false;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService NotificarLiberacaoNovaProvaTrilha()
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            return new NotificarLiberacaoNovaProvaTrilhaService().NotificarLiberacaoNovaProvaTrilha();
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService SincronizarUsuarioSAS(int idOferta)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            try
            {
                return new ManterSolucaoEducacionalService().SincronizarUsuarioSAS(idOferta);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService NotificarLiberacaoQuestionarioEficacia()
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            return new NotificarAlunosQuestionarioEficacia().NotificarQuestionarioEficacia();
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public bool PossuiCertificadosCertamesDisponiveis()
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            return new ManterCertificadoCertameService().PossuiCertificadosCertamesUsuario(autenticacao.Login);
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<DtoCertificadoCertame> ObterCertificadosCertamesDisponiveis()
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            return new ManterCertificadoCertameService().ObterCertamesUsuario(autenticacao.Login);
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public void NotificarComentarioPortal(int usuario, int usuarioComentario, string urlComentario, string comentario)
        {
            try
            {
                if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                    throw new Exception("Usuário não autenticado pelo sistema.");

                new ManterEmailService().NotificarComentarioPortal(usuario, usuarioComentario, urlComentario, comentario);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<DtoCertificadoCertame> ObterCertificadosCertameHistoricoAcademico()
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");

            return new ManterCertificadoCertameService().ObterCertificadosCertameHistoricoAcademico(autenticacao.Login);
        }

        /// <summary>
        /// Atualiza matrículas vinculadas a eventos do sistema de credenciamento
        /// </summary>
        [WebMethod]
        [SoapHeader("autenticacao")]
        public void AtualizarMatriculasCredenciamento()
        {
            try
            {
                if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                    throw new Exception("Usuário não autenticado pelo sistema.");

                new ManterCredenciamento().AtualizarMatriculas();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Envia email para gerência sobre usuários demitidos no dia
        /// </summary>
        [WebMethod]
        [SoapHeader("autenticacao")]
        public void NotificarDemitidosDia()
        {
            try
            {
                if (autenticacao == null || !(segurancaAutenticacao.AutenticaUsuario(autenticacao)))
                    throw new Exception("Usuário não autenticado pelo sistema.");

                new ManterEmailService().NotificarDemitidos();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
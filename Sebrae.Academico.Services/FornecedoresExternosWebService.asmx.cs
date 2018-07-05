using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;
using Sebrae.Academico.BP.Services;
using System.Web.Services.Protocols;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.Services;
using Sebrae.Academico.BP.Services.SgusWebService;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.BP.Services.SistemasExternos;
using Sebrae.Academico.BP.DTO.Services.UsuarioPorSID;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using ManterMetaIndividual = Sebrae.Academico.BP.Services.ExternosWebService.ManterMetaIndividual;
using ManterOferta = Sebrae.Academico.BP.Services.ExternosWebService.ManterOferta;
using ManterTurma = Sebrae.Academico.BP.Services.ExternosWebService.ManterTurma;

namespace Sebrae.Academico.Trihas.Services
{
    /// <summary>
    /// Summary description for FornecedoresExternosWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class FornecedoresExternosWebService : System.Web.Services.WebService
    {
        public AuthenticationProviderRequest autenticacao;
        private SegurancaAutenticacao segurancaAutenticacao = new SegurancaAutenticacao();

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService ManterSolucaoEducacional(Sebrae.Academico.BP.DTO.Services.DTOSolucaoEducacional pSolucaoEducacional)
        {

            if (autenticacao == null || !(segurancaAutenticacao.AutenticaFornecedor(autenticacao)))
                throw new Exception("Fornecedor não autenticado. verifique o Login e a Senha informada");

            try
            {
                (new ManterSolucaoEducacionalService()).ManterExternoSolucaoEducacional(pSolucaoEducacional, autenticacao);
                return new RetornoWebService() { Erro = 0, Mensagem = string.Empty };
            }
            catch(Exception ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }

        }

        [WebMethod, SoapHeader("autenticacao")]
        public DTOTrilhaObjetivos ConsultarObjetivosTrilha(string token)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaFornecedor(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                var manterUsuarioTrilha = new ManterUsuarioTrilha();
                var usuarioTrilha = manterUsuarioTrilha.ObterUsuarioPorToken(token);
                if (usuarioTrilha == null) throw new Exception("Usuário não autenticado pelo sistema.");
                var objetivoTrilhaServices = new ObjetivosTrilhaServices();
                var result = objetivoTrilhaServices.ConsultarObjetivosTrilha(usuarioTrilha, usuarioTrilha.TrilhaNivel.ID);
                if (result == null)
                {
                    result = new DTOTrilhaObjetivos
                    {
                        Status = false,
                        Msg = "Usuário não cadastrado no nível da trilha."
                    };
                }
                else
                {
                    result.Usuario = new DTOUsuarioObjetivo
                    {
                        Nome = usuarioTrilha.Usuario.Nome,
                        Cpf = usuarioTrilha.Usuario.CPF,
                        NomeUF = usuarioTrilha.Usuario.UF.Nome,
                        UF = usuarioTrilha.Usuario.UF.Sigla
                    };
                    result.Status = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                //TODO: Realizar Tratamento de erro para o portal
                return new DTOTrilhaObjetivos
                {
                    Status = false,
                    Msg = ex.Message
                };
            }
        }

        [WebMethod, SoapHeader("autenticacao")]
        public DTOTrilhaObjetivos ConsultarSolucoesPorObjetivo(string token, string chaveExterna)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaFornecedor(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                var manterUsuarioTrilha = new ManterUsuarioTrilha();

                var usuarioTrilha = manterUsuarioTrilha.ObterUsuarioPorToken(token);
                if (usuarioTrilha == null) throw new Exception("Usuário não autenticado pelo sistema.");

                var objetivoTrilhaServices = new ObjetivosTrilhaServices();
                var result = objetivoTrilhaServices.ConsultarObjetivosPorChaveExterna(usuarioTrilha, chaveExterna);
                
                return result;
            }
            catch (Exception ex)
            {
                //TODO: Realizar Tratamento de erro para o portal
                return new DTOTrilhaObjetivos
                {
                    Status = false,
                    Msg = ex.Message
                };
            }
        }


        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService CadastrarAtividadeFormativaParticipacao(string token, string chaveExternaObjetivo, string textoParticipacao,
                                                                         string nomeOriginalArquivo, string tipoArquivo, string arquivoBase64)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaFornecedor(autenticacao)))
                throw new Exception("Usuário não autenticado pelo sistema.");
            try
            {
                
                var usuarioTrilha = (new ManterUsuarioTrilha()).ObterUsuarioPorToken(token);

                var topicoTematico = (new ManterTrilhaTopicoTematico()).ObterTrilhaTopicoTematicoPorObjetivoTrilhaNivel(chaveExternaObjetivo, token);

                if (topicoTematico == null) {
                    throw new Exception("Tópico temático não encontrado.");
                }

                var autenticacaoUsuario = new AuthenticationRequest() { Login = usuarioTrilha.Usuario.CPF };

                (new AtividadeFormativaServices()).CadastrarAtividadeFormativa(usuarioTrilha.ID, topicoTematico.ID, textoParticipacao,
                                                                                        nomeOriginalArquivo, tipoArquivo, arquivoBase64, 1, autenticacaoUsuario);
                return new RetornoWebService() { Erro = 0, Mensagem = "" };
            }
            catch (AcademicoException ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
            catch (Exception)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = "Erro ao processar a solicitação. Favor entrar em contato com o suporte" };
            }

        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public List<Sebrae.Academico.BP.DTO.Services.DTOSolucaoEducacional> ConsultarSolucaoEducacional(string idChaveExterna)
        {

            if (autenticacao == null || !(segurancaAutenticacao.AutenticaFornecedor(autenticacao)))
                throw new Exception("Fornecedor não autenticado. verifique o Login e a Senha informada");

            try
            {
                return (new ManterSolucaoEducacionalService()).ConsultarSolucaoEducacionalPorFornecedor(idChaveExterna, autenticacao);
            }
            catch
            {
                return new List<Sebrae.Academico.BP.DTO.Services.DTOSolucaoEducacional>();
            }

        }
        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService ManterOferta(DTOManterOferta pOferta)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaFornecedor(autenticacao)))
                throw new Exception("Fornecedor não autenticado. verifique o Login e a Senha informada");

            try
            {
                (new ManterOferta()).ManterOfertaFornecedor(pOferta, autenticacao);
                return new RetornoWebService() { Erro = 0, Mensagem = string.Empty };
            }
            catch (Exception ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService AtualizarNotaAlunoMoodle(string chaveExternaSolucao, string chaveExternaTurma,
            string codCurso, string idUsuario, string nota, string dataConclusao)
        {
            try
            {
                //Pega a última turma
                var turma = new BMTurma().ObterUltimaTurmaPorChaveExterna(chaveExternaTurma);

                //Pega o id do pelo cpf(idUsuario) usuário
                var usuario = new BMUsuario().ObterPorCPF(idUsuario);

                if (usuario?.ID > 0)
                {
                    //Pegar a MatriculaOferta
                    var bmMatriculaOrfeta = new BMMatriculaOferta();
                    var matriculaOferta = bmMatriculaOrfeta.ObterPorOfertaEUsuario(turma.Oferta.ID, usuario.ID);

                    //Pegar a MatriculaTurma
                    var matriculaTurma = new BMMatriculaTurma().ObterMatriculaTurma(usuario.ID, turma.Oferta.ID);

                    double notaFinal;

                    if (double.TryParse(nota, out notaFinal))
                    {
                        matriculaTurma.MediaFinal = notaFinal;
                    }

                    matriculaTurma.DataTermino = DateTime.Now;

                    new BMMatriculaTurma().Salvar(matriculaTurma);

                    if (notaFinal >= 7)
                    {
                        matriculaOferta.StatusMatricula = enumStatusMatricula.Aprovado;
                    }
                    else
                    {
                        matriculaOferta.StatusMatricula = enumStatusMatricula.Reprovado;
                    }

                    bmMatriculaOrfeta.Salvar(matriculaOferta);

                    return new RetornoWebService() { Erro = 0, Mensagem = string.Empty };
                }

                return new RetornoWebService() { Erro = 1, Mensagem = "usuario não encontrado" };

            }
            catch (Exception ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOManterOferta ConsultaOfertaPeloMoodle(string idsOfertas, string idChaveExternaOferta, string idChaveExternaSolucaoEducacional)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaFornecedor(autenticacao)))
                throw new Exception("Fornecedor não autenticado. verifique o Login e a Senha informada");

            return (new ManterOferta()).ConsultaOferta(idsOfertas, idChaveExternaOferta, idChaveExternaSolucaoEducacional, autenticacao);

        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOManterOferta ConsultaOferta(string idChaveExternaOferta, string idChaveExternaSolucaoEducacional)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaFornecedor(autenticacao)))
                throw new Exception("Fornecedor não autenticado. verifique o Login e a Senha informada");

            return (new ManterOferta()).ConsultaOferta(null,idChaveExternaOferta, idChaveExternaSolucaoEducacional, autenticacao);

        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService ManterTurma(DTOManterTurma pTurma)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaFornecedor(autenticacao)))
                throw new Exception("Fornecedor não autenticado. verifique o Login e a Senha informada");

            try
            {
                (new ManterTurma()).ManterCadastroTurma(pTurma, autenticacao);
                return new RetornoWebService() { Erro = 0, Mensagem = string.Empty };
            }
            catch (Exception ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }

        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOManterTurma CounsultaTurma(string idChaveExternaTurma, string idChaveExternaOferta)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaFornecedor(autenticacao)))
                throw new Exception("Fornecedor não autenticado. verifique o Login e a Senha informada");

            return (new ManterTurma()).CounsultaTurma(idChaveExternaTurma, idChaveExternaOferta, autenticacao);
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService CadastrarMetaIndividual(int ID_usuario, string ID_ChaveExterna, string Descricao, string Nome, DateTime DataValidade, string[] ListaSolucaoEducacional = null)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaFornecedor(autenticacao)))
                throw new Exception("Fornecedor não autenticado. verifique o Login e a Senha informada");

            try
            {
                (new ManterMetaIndividual()).IncluirMetaIndividual(ID_usuario, ID_ChaveExterna, Descricao, Nome, DataValidade, ListaSolucaoEducacional, autenticacao.Login);
                return new RetornoWebService() { Erro = 0, Mensagem = string.Empty };
            }
            catch (Exception ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }

        }

        [WebMethod]
        [SoapHeader("autenticacao")]        
        public DTOManterUsuario ConsultarUsuarioPorCPF(string cpf)
        {
            //if (autenticacao == null || !(segurancaAutenticacao.AutenticaFornecedor(autenticacao)))
            //    throw new Exception("Fornecedor não autenticado. verifique o Login e a Senha informada");

            try
            {
                return (new Sebrae.Academico.BP.Services.SgusWebService.ManterUsuarioServices()).ConsultarUsuario(cpf);
            }
            catch(Exception)
            {

                return null;
            }

        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOManterUsuarioConexao ConsultarUsuarioPorCPFConexao(string cpf)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaFornecedor(autenticacao)))
                throw new Exception("Fornecedor não autenticado. verifique o Login e a Senha informada");

            try
            {
                return (new Sebrae.Academico.BP.Services.SgusWebService.ManterUsuarioServices()).ConsultarUsuarioConexao(cpf);
            }
            catch
            {
                return null;
            }

        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public bool ManterMatriculaTurma(int idMatriculaOferta, int idTurma, double mediaFinal)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaFornecedor(autenticacao)))
                throw new Exception("Fornecedor não autenticado. verifique o Login e a Senha informada");

            return (new ManterOferta()).ManterMatriculaTurma(idMatriculaOferta, idTurma, mediaFinal, autenticacao);

        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService CadastrarPublicacaoSaber(string Titulo, int ChaveExterna, List<string> CpfAutor,
                                                          bool publicado, string Resenha, DateTime DataPublicacao,
                                                          string UF, string NomeCompleto, string Assunto)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaFornecedor(autenticacao)))
                throw new Exception("Fornecedor não autenticado. verifique o Login e a Senha informada");

            RetornoWebService retornoWebService = new RetornoWebService();

            try
            {
                ManterPublicacaoSaber manterPublicacaoSaber = new ManterPublicacaoSaber();
                manterPublicacaoSaber.AlterarPublicacaoSaber(Titulo, ChaveExterna, CpfAutor, publicado,
                                                              Resenha, DataPublicacao, UF, NomeCompleto, Assunto, autenticacao.Login);

                return new RetornoWebService() { Erro = 0, Mensagem = string.Empty };

            }
            catch (Exception ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
        }


        [WebMethod]
        [SoapHeader("autenticacao")]
        public RetornoWebService CadastrarHistoricoSGTC(int idUsuario, string nomeSolucaoEducacional, int idChaveExterna,
                                                        DateTime dtConclusao, string codCertificado)
        {

            if (autenticacao == null || !(segurancaAutenticacao.AutenticaFornecedor(autenticacao)))
                throw new Exception("Fornecedor não autenticado. verifique o Login e a Senha informada");

            RetornoWebService retornoWebService = new RetornoWebService();

            try
            {
                HistoricoSGTCServices historicoSGTCServices = new HistoricoSGTCServices();
                historicoSGTCServices.CadastrarHistoricoSGTC(idUsuario, nomeSolucaoEducacional,
                                                             idChaveExterna, dtConclusao, codCertificado, autenticacao.Login);

                return new RetornoWebService() { Erro = 0, Mensagem = string.Empty };

            }
            catch (Exception ex)
            {
                return new RetornoWebService() { Erro = 1, Mensagem = ex.Message };
            }
        }

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

        [WebMethod]
        public List<DTOCategoriaConteudo> ListarCategoriaConteudo()
        {
            try
            {
                return (new ConsultarCategoriaConteudo()).ListarCategoriaConteudo().OrderBy(x => x.Nome).ToList();
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        public List<DTOCertificadoTemplate> ListarCertificadoTemplate()
        {
            try
            {
                return (new ConsultarCertificadoTemplate()).ObterTodos().OrderBy(x => x.Nome).ToList();
            }
            catch
            {
                return null;
            }

        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public ConsultaUsuarioPorSID ConsultarUsuarioPorSID(string sid)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaFornecedor(autenticacao)))
                throw new Exception("Fornecedor não autenticado. verifique o Login e a Senha informada");

            return new UsuariosADServices().ConsultarUsuarioPorSID(sid);
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOConsultaUsuarioPorFiltro ConsultaUsuarioPorFiltro(string nome, string email, int ID_UF, int ID_NivelOcupacional, int page = 0, int maxPerPage = 20)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaFornecedor(autenticacao)))
                throw new Exception("Fornecedor não autenticado. verifique o Login e a Senha informada");

            if(ID_UF == 0){ throw new Exception("O código de estado deve ser informado.");}

            if (ID_NivelOcupacional == 0){throw new Exception("O código de nivel ocupacional deve ser informado.");}

            return new UsuariosADServices().ConsultarUsuarioPorFiltro(nome, email, ID_UF, ID_NivelOcupacional, page, maxPerPage);
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public CadastraUsuarioPorSID CadastrarUsuarioPorSID(string sid, string nome, string email, string cpf, string senha)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaFornecedor(autenticacao)))
                throw new Exception("Fornecedor não autenticado. verifique o Login e a Senha informada");

            return new UsuariosADServices().CadastrarUsuarioPorSID(sid, nome, email, cpf, senha);
        }

        [WebMethod]
        public DTOManterUsuarioConexao AutenticarUsuarioConexao(string cpf, string senha, string IPUsuario)
        {
            try
            {
                return (new ManterUsuarioServices()).AutenticaUsuarioInformadoConexao(cpf, senha, IPUsuario, string.Empty);
            }
            catch (AcademicoException ex)
            {
                throw new AcademicoException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        [WebMethod]
        public DTOManterUsuario AutenticarUsuarioGuid(string guid, string login)
        {
            try
            {
                return (new ManterUsuarioServices()).AutenticaUsuarioInformadoPorGuid(guid, login);
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public CadastraUsuarioPorSID ManterSIDPorCPF(string sid, string cpf)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaFornecedor(autenticacao)))
                throw new Exception("Fornecedor não autenticado. verifique o Login e a Senha informada");

            return new UsuariosADServices().AtualizaSIDPorCPF(sid, cpf);
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public DTOConsultarOfertasPorPeriodo ConsultarOfertasPorPeriodo(DateTime dataInicio, DateTime dataFim)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaFornecedor(autenticacao)))
                throw new Exception("Fornecedor não autenticado. verifique o Login e a Senha informada");

            return new ManterOferta().ConsultarOfertasPorPeriodo(dataInicio, dataFim);
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public string GerarTokenPorSID(string sid)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaFornecedor(autenticacao)))
                throw new Exception("Fornecedor não autenticado. verifique o Login e a Senha informada");

            return new ManterUsuarioServices().GerarTokenPorSID(sid, autenticacao);
        }

        [WebMethod]
        [SoapHeader("autenticacao")]
        public string GerarUrlLogginUsuario(string cpf, string senha)
        {
            if (autenticacao == null || !(segurancaAutenticacao.AutenticaFornecedor(autenticacao)))
                throw new Exception("Fornecedor não autenticado. verifique o Login e a Senha informada");

            string url = "";

            if (new BMUsuario().AutenticarUsuario(cpf, senha))
            {
                var tokenAcesso = new TokenAcesso();
                tokenAcesso.Usuario = new BMUsuario().ObterPorCPF(cpf);
                tokenAcesso.Token = Guid.NewGuid();
                tokenAcesso.DataCriacao = DateTime.Now;
                tokenAcesso.Fornecedor = new BMFornecedor().ObterPorID(10);
                tokenAcesso.TokenMD5 = CriptografiaHelper.ObterHashMD5(tokenAcesso.Usuario.CPF + "/" + tokenAcesso.Token);
                new BMTokenAcesso().Salvar(tokenAcesso);

                url += new BMConfiguracaoSistema().ObterPorID((int)enumConfiguracaoSistema.EnderecoPortal).Registro + "token/" + tokenAcesso.TokenMD5;
            }

            return url;
        }

    }
}

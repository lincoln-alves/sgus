using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.BP.DTO.Services.ConsultaSolucaoEducacional;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Classes.Moodle;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.BP.integracaoSAS;
using Sebrae.Academico.BP.DTO.Services.ManterSolucaoEducacionalService;
using AutoMapper;
using Sebrae.Academico.Extensions;
using System.Web;
using System.Text.RegularExpressions;
using System.Net;
using AutoMapper.Execution;
using Sebrae.Academico.BP.Auth;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ManterSolucaoEducacionalService : BusinessProcessServicesBase
    {

        private BMSolucaoEducacional solucaoEducacionalBM;

        public IList<DTOSolucEducFormaAquisicao> ConsultarSolucaoEducacional(int pUsuario, int pFornecedor, int pFormaAquisicao)
        {
            solucaoEducacionalBM = new BMSolucaoEducacional();

            IList<SolucaoEducacional> lstSolEduc = solucaoEducacionalBM.ConsultarSolucaoEducacionalWebServices(pUsuario, pFornecedor, pFormaAquisicao);

            IList<FormaAquisicao> lstFormaAquisicao = (from fa in lstSolEduc
                                                       select fa.FormaAquisicao).ToList();

            IList<DTOSolucEducFormaAquisicao> lstResult = new List<DTOSolucEducFormaAquisicao>();

            foreach (FormaAquisicao fa in lstFormaAquisicao)
            {
                DTOSolucEducFormaAquisicao dtofa = new DTOSolucEducFormaAquisicao()
                {
                    Nome = fa.Nome,
                    CodigoFormaAquisicao = fa.ID,
                    ListaSolucaoEducacional = new List<DTOSolucEducSolucaoEducacional>()
                };
                IList<SolucaoEducacional> lstSolEducFA = (from se in lstSolEduc
                                                          where se.FormaAquisicao.ID == fa.ID
                                                          select se).ToList();

                foreach (SolucaoEducacional se in lstSolEducFA)
                {
                    MatriculaOferta mo = (from of in (new BMOferta()).ObterPorFiltro(null, null, se != null ? se.ID : 0)
                                          select of.ListaMatriculaOferta).FirstOrDefault().Where(x => x.StatusMatricula != enumStatusMatricula.CanceladoAdm &&
                                                                                                      x.StatusMatricula != enumStatusMatricula.CanceladoAluno)
                                                                                                      .OrderByDescending(x => x.DataSolicitacao).FirstOrDefault();
                    DTOSolucEducSolucaoEducacional dtoSE = new DTOSolucEducSolucaoEducacional()
                    {
                        CodigoSolucaoEducacional = se.ID,
                        Nome = se.Nome,
                        SolucaoEducacionalMatricula = mo == null ? null : (new DTOSolucEducSolucaoEducacionalMatricula()
                        {
                            DataSolicitacao = mo.DataSolicitacao,
                            StatusMatricula = mo.StatusMatricula.ToString()
                        })
                    };
                    dtofa.ListaSolucaoEducacional.Add(dtoSE);
                }
                lstResult.Add(dtofa);
            }
            return lstResult;
        }

        public void CancelarMatriculaSolucaoEducacional(int idMatriculaOferta, AuthenticationRequest autenticacao)
        {
            var bmMatriculaOferta = new ManterMatriculaOferta();

            var matriculaOferta = bmMatriculaOferta.ObterMatriculaOfertaPorID(idMatriculaOferta);

            if (matriculaOferta != null)
            {
                var diasMatriculados = Convert.ToDateTime(DateTime.Now) -
                                       Convert.ToDateTime(matriculaOferta.DataSolicitacao);
                if (diasMatriculados.Days >
                    int.Parse(
                        ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.DiasCancelamentoCurso).Registro))
                {
                    //Passou do Limite para cancelamento
                    throw new AcademicoException("Prazo expirado para o cancelamento");
                }

                var matriculaTurma = matriculaOferta.MatriculaTurma.FirstOrDefault();
                matriculaTurma.DataTermino = DateTime.Now;
                matriculaOferta.DataStatusMatricula = DateTime.Now;
                matriculaOferta.StatusMatricula = enumStatusMatricula.CanceladoAluno;
                matriculaOferta.Auditoria = new Auditoria(autenticacao.Login);

                var matricula = bmMatriculaOferta.VerificarFilaEspera(matriculaOferta);

                bmMatriculaOferta.Salvar(matricula);
            }
            else
            {
                throw new AcademicoException("Matrícula não encontrada");
            }
        }

        /// <summary>
        /// Método que limpa matrículas do usuário em cursos do moodle.
        /// </summary>
        /// <param name="solucaoEducacional"></param>
        /// <param name="userSelected"></param>
        /// <param name="oferta"></param>
        private void ValidarMatriculasNoMoodle(SolucaoEducacional solucaoEducacional, Usuario userSelected, Oferta oferta)
        {
            // Se tiver reprovado nesse curso e ele for do Moodle limpa a matrícula do mesmo para que ele comece do zero
            if (solucaoEducacional.Fornecedor.ID == (int)enumFornecedor.MoodleSebrae && userSelected.ListaMatriculaOferta.Any(x => x.Oferta.ID == oferta.ID && x.Oferta.SolucaoEducacional.ID == solucaoEducacional.ID && x.StatusMatricula == enumStatusMatricula.Reprovado))
            {
                var usuarioMoodle = (new Sebrae.Academico.BM.Classes.Moodle.BMUsuarioMoodle()).ObterPorCPF(userSelected.CPF);

                // Exclui a participação não aprovada do Moodle
                if (usuarioMoodle.ID != 0 && oferta.CodigoMoodle != 0 && oferta.CodigoMoodle != null)
                {
                    (new ManterUsuarioMoodleInscricao()).ExcluirPorUsuarioECurso(usuarioMoodle, oferta.CodigoMoodle);
                }
            }
        }

        private bool InscreverOfertaExclusiva(Usuario usuario, Oferta oferta, SolucaoEducacional solucao,
            string cpfAutenticacao, BMMatriculaOferta moBM, ref bool fornecedorNotificado,
            List<int> pListaIdMetaIndividualAssociada, List<int> pListaIdMetaInstitucionalAssociada,
            ItemTrilha itemTrilha = null)
        {
            //VALIDAR SE ELE TEM ALGUMA OFERTA EXCLUSIVA PENDENTE DE CONFIRMACAO
            if (
                usuario.ListaMatriculaOferta.Any(
                    x =>
                        x.Oferta.ID == oferta.ID && x.Oferta.SolucaoEducacional.ID == solucao.ID &&
                        x.StatusMatricula == enumStatusMatricula.PendenteConfirmacaoAluno &&
                        x.Oferta.DataFimInscricoes.HasValue &&
                        x.Oferta.DataFimInscricoes.Value.Date >= DateTime.Now.Date))
            {
                var mo =
                    usuario.ListaMatriculaOferta.OrderByDescending(x => x.ID).FirstOrDefault(
                        x =>
                            x.Oferta.SolucaoEducacional.ID == solucao.ID &&
                            x.StatusMatricula == enumStatusMatricula.PendenteConfirmacaoAluno);

                
                if (mo != null)
                {
                    
                    if (mo.MatriculaTurma.Count() > 0 && mo.MatriculaTurma[0] != null)
                        mo.MatriculaTurma[0].DataMatricula = DateTime.Now;

                    mo.StatusMatricula = enumStatusMatricula.Inscrito;                    
                    mo.Auditoria = new Auditoria(cpfAutenticacao);
                    mo.FornecedorNotificado = false;

                    if (!(mo.MatriculaTurma != null && mo.MatriculaTurma.Count > 0))
                    {
                        try
                        {
                            if (mo.Oferta.TipoOferta.Equals(enumTipoOferta.Continua))
                            {
                                fornecedorNotificado = true;
                                NotificaFornecedor.Instancia.Notificar(mo);
                            }
                        }
                        catch (Exception ex)
                        {
                            ErroUtil.Instancia.TratarErro(ex);
                            throw new AcademicoException(
                                "Erro: Ocorreu um erro ao matricular neste curso. Por favor, entre em contato com o atendimento ou tente novamente mais tarde. Obrigado");
                        }
                    }

                    moBM.Salvar(mo);

                    var itemTrilhaParticipacao = VerificarParticipacaoItemTrilha(itemTrilha, mo);

                    if (itemTrilhaParticipacao != null)
                        (new ManterItemTrilhaParticipacao()).InformarParticipacaoLoja(itemTrilhaParticipacao);

                    ValidarMetaIndividual(usuario.ID, solucao.ID, pListaIdMetaIndividualAssociada, cpfAutenticacao);
                    ValidarMetaInstitucional(usuario.ID, solucao.ID, pListaIdMetaInstitucionalAssociada,
                        cpfAutenticacao);

                    return true;
                }
            }

            return false;
        }

        private static ItemTrilhaParticipacao VerificarParticipacaoItemTrilha(ItemTrilha itemTrilha, MatriculaOferta matriculaOferta)
        {
            if (itemTrilha != null)
            {
                var matricula = itemTrilha.Missao.PontoSebrae.TrilhaNivel.ListaUsuarioTrilha.FirstOrDefault(x => x.Usuario.ID == matriculaOferta.Usuario.ID);

                var manterItemTrilhaParticipacao = new BP.ManterItemTrilhaParticipacao();

                ItemTrilhaParticipacao itemTrilhaParticipacao;

                if (matricula == null)
                    throw new AcademicoException("Usuário não matriculado no nível");

                if (!itemTrilha.ListaItemTrilhaParticipacao.Any(
                    x =>
                        x.UsuarioTrilha.ID == matricula.ID && x.Autorizado != true &&
                        x.TipoParticipacao == enumTipoParticipacaoTrilha.SolucaoEducacional))
                {
                    // Criar uma participação que servirá de "Dummy" para relacionar o aluno com a matrícula.
                    itemTrilhaParticipacao = new ItemTrilhaParticipacao
                    {
                        TipoParticipacao = enumTipoParticipacaoTrilha.SolucaoEducacional,
                        UsuarioTrilha = matricula,
                        ItemTrilha = itemTrilha,
                        DataEnvio = DateTime.Now,
                        MatriculaOferta = matriculaOferta,
                        Auditoria = new Auditoria(matriculaOferta.Usuario.CPF),
                        FileServer = null
                    };
                }
                else
                {
                    itemTrilhaParticipacao = itemTrilha.ListaItemTrilhaParticipacao.Where(x =>
                       x.UsuarioTrilha.ID == matricula.ID && x.Autorizado != true &&
                       x.TipoParticipacao == enumTipoParticipacaoTrilha.SolucaoEducacional).FirstOrDefault();

                    itemTrilhaParticipacao.MatriculaOferta = matriculaOferta;
                }

                manterItemTrilhaParticipacao.Salvar(itemTrilhaParticipacao);

                return itemTrilhaParticipacao;

            }

            return null;
        }

        public string CadastrarSAS(Usuario usuario, TreinamentoSasClient conexao)
        {
            try
            {
                // Declaração do Cliente de Conexão ao SAS
                TreinamentoSasClient sasConn = conexao;

                /*
                 * Criação da string do XML com os dados mapeados do usuário ao XML, 
                 * renomeia-se o objeto da string para Usuario e finalmente remove-se o 
                 * ínicio do xml version já que o mesmo não espera ser utilizado nessa chamada
                 */
                string xml = ObterXMLUsuarioSAS(usuario);

                var returnXML = MatricularSAS(sasConn, xml);

                ValidarResponseSAS(returnXML);

                #region Debug
                //Necessário para debugar o que sai e entra no request do SOAP
                InspectorBehavior requestInterceptor = new InspectorBehavior();

                //Adição do Comportamento de Requisição de debug ao sasConn
                sasConn.Endpoint.Behaviors.Add(requestInterceptor);

                //Saída da ultima requisição XML do sasConn
                string requestXML = requestInterceptor.LastRequestXML;

                //Saída da ultima resposta XML do sasConn
                string responseXML = requestInterceptor.LastResponseXML;
                #endregion Debug

                return returnXML;
            }
            catch (AlertException ex)
            {
                throw new AlertException(ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ValidarResponseSAS(string xml)
        {
            List<DTOOcorrenciaSAS> listaRetorno = new List<DTOOcorrenciaSAS>();

            foreach (Match match in Regex.Matches(xml, @"<Ocorrencia>([\s\S]*?)<\/Ocorrencia>"))
            {
                listaRetorno.Add(match.Value.Replace("<Ocorrencia>", "<DTOOcorrenciaSAS>").Replace("</Ocorrencia>", "</DTOOcorrenciaSAS>").XmlDeserialize<DTOOcorrenciaSAS>());
            }

            if (listaRetorno.Any())
            {// notErrors.Any(y => y != x.Codigo)
                if (!listaRetorno.Any(x => (x.Codigo == "1" || x.Codigo == "-1")))
                {
                    throw new AlertException(String.Format("Erro ao efetuar inscrição, problema na conexão com o SAS, a mensagem de retorno foi \"{0}\"", listaRetorno.First().Mensagem));
                }
            }
        }

        private string ObterXMLUsuarioSAS(Usuario usuario)
        {
            return Mapper.Map<DTOUsuarioSAS>(usuario).XmlSerialize().Replace("DTOUsuarioSAS", "Usuario").Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
        }

        private string MatricularSAS(TreinamentoSasClient conexao, string xml)
        {
            //Efetuando a chamada de CadastrarUsuario da sasConn, nota-se a formatação da string para adicionar o CDATA
            return conexao.CadastrarUsuario(xml, ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.ChaveAcessoSAS).Registro);
        }

        private void ValidarUsuarioSas(Usuario usuario)
        {
            if (usuario.TelefoneExibicao == null)
            {
                throw new AlertException("O campo \"telefone\" é obrigatorio para realizar a matrícula do usuário no sas");
            }
        }

        public RetornoWebService MatricularSolucaoEducacional(int idUsuario, int idSolucaoEducacional, int idOferta,
                                                      List<int> pListaIdMetaIndividualAssociada, List<int> pListaIdMetaInstitucionalAssociada,
                                                      string cpfAutenticacao, int? idTurma = null, ItemTrilha itemTrilha = null)
        {
            var solucaoEducacional = new BMSolucaoEducacional().ObterPorId(idSolucaoEducacional);

            if (solucaoEducacional == null)
                throw new AcademicoException("Solução Educacional não encontrada");

            var retorno = new RetornoWebService { Erro = 0, Mensagem = "" };
            var usuario = new BMUsuario().ObterPorId(idUsuario);

            if (solucaoEducacional.IntegracaoComSAS)
                CadastrarSAS(usuario, new TreinamentoSasClient());

            var moBm = new BMMatriculaOferta();
            var fornecedorNotificado = false;
            var oferta = (new BMOferta()).ObterPorId(idOferta);

            ValidarMatriculasNoMoodle(solucaoEducacional, usuario, oferta);

            try
            {
                new ManterMatriculaOferta().VerificarPoliticaDeConsequencia(idUsuario, idSolucaoEducacional);
            }
            catch (PoliticaConsequenciaException ex)
            {
                retorno.Mensagem = ex.Message;
                retorno.Erro = (int)ex.Consequencia;
                return retorno;
            }

            if (InscreverOfertaExclusiva(usuario, oferta, solucaoEducacional, cpfAutenticacao, moBm,
                                         ref fornecedorNotificado, pListaIdMetaIndividualAssociada, pListaIdMetaInstitucionalAssociada, itemTrilha))
            {
                return retorno;
            }

            ValidarRequisitosMatricula(usuario, solucaoEducacional, oferta);
            RetornoWebService retornoWebService;
            MatriculaTurma matriculaTurma;
            if (InscreverOfertasContinuaOuNormal(pListaIdMetaIndividualAssociada, pListaIdMetaInstitucionalAssociada,
                cpfAutenticacao,
                oferta, usuario, fornecedorNotificado, solucaoEducacional, moBm, retorno, out retornoWebService,
                out matriculaTurma, idTurma, itemTrilha))
            {
                if (matriculaTurma != null)
                    EnviarNotificacaoMatriculaSolucaoEducacional(matriculaTurma, usuario);

                return retornoWebService;
            }

            throw new AcademicoException("Não existem ofertas disponíveis");
        }

        private void EnviarNotificacaoMatriculaSolucaoEducacional(MatriculaTurma matriculaTurma, Usuario userSelected)
        {
            try
            {
                var templateInscricaoSe = TemplateUtil.ObterInformacoes(enumTemplate.InscricaoSESucesso);
                var assuntoDoEmail = templateInscricaoSe.Assunto;

                var registros = new Dictionary<string, string>
                {
                    {"#NOME_CURSO", matriculaTurma.MatriculaOferta.Oferta.SolucaoEducacional.Nome},
                    {"#NOME_ALUNO", userSelected.Nome},
                    {"#DATA_MATRICULA", matriculaTurma.DataMatricula.ToShortDateString()},
                    {"#DATA_TERMINO", matriculaTurma.DataLimite.ToShortDateString()}
                };

                EmailUtil.Instancia.EnviarEmail(userSelected.Email, assuntoDoEmail,
                    FormatarTextoEmail(registros, templateInscricaoSe.TextoTemplate));
            }
            catch (Exception)
            {
                // ignored.
            }
        }

        private bool InscreverOfertasContinuaOuNormal(List<int> pListaIdMetaIndividualAssociada,
            List<int> pListaIdMetaInstitucionalAssociada, string cpfAutenticacao, Oferta oferta, Usuario usuario,
            bool fornecedorNotificado, SolucaoEducacional solucaoEducacional, BMMatriculaOferta moBm, RetornoWebService retorno,
            out RetornoWebService retornoWebService, out MatriculaTurma matriculaTurma, int? idTurma = null, ItemTrilha itemTrilha = null)
        {
            //VALIDADO OFERTA CONTINUA.
            if (oferta.TipoOferta.Equals(enumTipoOferta.Continua))
            {
                var qtdInscritosNaOferta =
                    oferta.ListaMatriculaOferta.Count(x => (x.StatusMatricula != enumStatusMatricula.CanceladoAdm &&
                                                            x.StatusMatricula != enumStatusMatricula.CanceladoAluno) &&
                                                           x.StatusMatricula != enumStatusMatricula.FilaEspera);

                var usuarioLogado = new BMUsuario().ObterPorId(usuario.ID);

                var matriculaOferta = new MatriculaOferta()
                {
                    Oferta = new BMOferta().ObterPorId(oferta.ID),
                    Usuario = new BMUsuario().ObterPorId(usuario.ID),
                    Auditoria = new Auditoria(cpfAutenticacao),
                    DataSolicitacao = DateTime.Now,
                    UF = new BMUf().ObterPorId(usuario.UF.ID),
                    NivelOcupacional = new BMNivelOcupacional().ObterPorID(usuario.NivelOcupacional.ID)
                };

                if (oferta.QuantidadeMaximaInscricoes > 0)
                {
                    if (qtdInscritosNaOferta >= oferta.QuantidadeMaximaInscricoes)
                    {
                        if (oferta.FiladeEspera)
                        {
                            matriculaOferta.StatusMatricula = enumStatusMatricula.FilaEspera;
                        }
                        else
                        {
                            throw new AcademicoException("Erro: A quantidade máxima de alunos foi atingida");
                        }
                    }
                    else
                    {
                        matriculaOferta.StatusMatricula = enumStatusMatricula.Inscrito;
                    }
                }
                else
                {
                    matriculaOferta.StatusMatricula = enumStatusMatricula.Inscrito;
                }

                qtdInscritosNaOferta++;

                var ofertaEstado =
                    oferta.ListaPermissao.FirstOrDefault(f => f.Uf != null && f.Uf.ID == usuarioLogado.UF.ID);
                if (ofertaEstado == null)
                {
                    throw new AcademicoException("Erro: A vaga não é permitida para o seu estado");
                }

                if (ofertaEstado.QuantidadeVagasPorEstado > 0)
                {
                    var qtdMatriculaOfertaPorEstado =
                        oferta.ListaMatriculaOferta.Count(x => !x.IsUtilizado() && x.Usuario.ID == usuarioLogado.UF.ID);

                    if (qtdMatriculaOfertaPorEstado >= ofertaEstado.QuantidadeVagasPorEstado && !oferta.FiladeEspera)
                    {
                        throw new AcademicoException("Erro: As vagas já foram preenchidas para o seu estado");
                    }
                }

                matriculaOferta.FornecedorNotificado = false;
                //moBM.Salvar(matriculaOferta);

                #region Turma

                var incluiuTurma = IncluirTurma(matriculaOferta, cpfAutenticacao, out matriculaTurma, idTurma);

                Turma t = null;

                if (incluiuTurma &&
                    (oferta.SolucaoEducacional.Fornecedor.PermiteGestaoSGUS ||
                     oferta.SolucaoEducacional.Fornecedor.ID == (int)enumFornecedor.FGVOCW))
                    t =
                        oferta.ListaTurma.FirstOrDefault(
                            x =>
                                ((idTurma.HasValue && x.ID == idTurma.Value) || x.DataFinal == null ||
                                 x.DataFinal.Value.Date >= DateTime.Now.Date) && x.InAberta);

                if (t != null)
                {
                    var turma = new BMTurma().ObterPorID(t.ID);

                    if (turma.QuantidadeMaximaInscricoes > 0)
                    {
                        if (turma.QuantidadeAlunosMatriculadosNaTurma >= turma.QuantidadeMaximaInscricoes)
                        {
                            throw new AcademicoException("Erro: As vagas para esta turma já foram preenchidas");
                        }
                    }
                }

                //validando se a turma já está chegando ao limite.
                if (qtdInscritosNaOferta >
                    (oferta.QuantidadeMaximaInscricoes -
                     int.Parse(
                         ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.LimiteAlertaInscricaoOferta)
                             .Registro)))
                {
                    ManterMatricula.EnviarEmailLimiteOferta(oferta, matriculaOferta);
                }

                #endregion

                // Notifica o fornecedor da matrícula, atualmente só funciona para a WebAula
                if (!fornecedorNotificado)
                {
                    try
                    {
                        if (solucaoEducacional.Fornecedor.ID == (int)enumFornecedor.WebAula)
                        {
                            NotificaFornecedor.Instancia.Notificar(matriculaOferta);
                            if (matriculaOferta.ID > 0)
                                new ManterMatriculaOferta().AtualizarMatriculaOferta(matriculaOferta);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErroUtil.Instancia.TratarErro(ex);
                        throw new AcademicoException(
                            "Ocorreu um erro ao matricular neste curso. Não foi possível se comunicar com o fornecedor do curso. Por favor, entre em contato com o atendimento ou tente novamente mais tarde. Obrigado");
                    }
                }

                moBm.Salvar(matriculaOferta);

                var itemTrilhaParticipacao = VerificarParticipacaoItemTrilha(itemTrilha, matriculaOferta);

                if (itemTrilhaParticipacao != null)
                    (new ManterItemTrilhaParticipacao()).InformarParticipacaoLoja(itemTrilhaParticipacao);

                ValidarMetaIndividual(usuario.ID, solucaoEducacional.ID, pListaIdMetaIndividualAssociada,
                    cpfAutenticacao);

                ValidarMetaInstitucional(usuario.ID, solucaoEducacional.ID, pListaIdMetaInstitucionalAssociada,
                    cpfAutenticacao);

                retornoWebService = retorno;
                return true;
            }

            //VALIDANDO A OFERTA NORMAL
            if (oferta.TipoOferta.Equals(enumTipoOferta.Normal))
            {
                int qtdInscritosNaOferta =
                    oferta.ListaMatriculaOferta.Count(x => (x.StatusMatricula != enumStatusMatricula.CanceladoAdm &&
                                                            x.StatusMatricula != enumStatusMatricula.CanceladoAluno) &&
                                                           x.StatusMatricula != enumStatusMatricula.FilaEspera);
                var matriculaOferta = new MatriculaOferta()
                {
                    Oferta = new BMOferta().ObterPorId(oferta.ID),
                    Usuario = new BMUsuario().ObterPorId(usuario.ID),
                    Auditoria = new Auditoria(cpfAutenticacao),
                    DataSolicitacao = DateTime.Now,
                    UF = new BMUf().ObterPorId(usuario.UF.ID),
                    NivelOcupacional = new BMNivelOcupacional().ObterPorID(usuario.NivelOcupacional.ID)
                };

                if (oferta.QuantidadeMaximaInscricoes > 0)
                {
                    if (qtdInscritosNaOferta >= oferta.QuantidadeMaximaInscricoes)
                    {
                        if (oferta.FiladeEspera)
                        {
                            matriculaOferta.StatusMatricula = enumStatusMatricula.FilaEspera;
                        }
                        else
                        {
                            throw new AcademicoException("Erro: A quantidade máxima de alunos foi atingida");
                        }
                    }
                    else
                    {
                        matriculaOferta.StatusMatricula = enumStatusMatricula.Inscrito;
                    }
                }
                else
                {
                    matriculaOferta.StatusMatricula = enumStatusMatricula.Inscrito;
                }

                var bmMatriculaOferta = new BMMatriculaOferta();
                #region Turma

                var incluiuTurma = IncluirTurma(matriculaOferta, cpfAutenticacao, out matriculaTurma, idTurma);
                Turma t = null;

                if (incluiuTurma &&
                    (oferta.SolucaoEducacional.Fornecedor.PermiteGestaoSGUS ||
                     oferta.SolucaoEducacional.Fornecedor.ID == (int)enumFornecedor.FGVOCW))
                    t =
                        oferta.ListaTurma.FirstOrDefault(
                            x =>
                                ((idTurma.HasValue && x.ID == idTurma.Value) || x.DataFinal == null ||
                                 x.DataFinal.Value.Date >= DateTime.Now.Date) && x.InAberta);

                if (t != null)
                {
                    var turma = new BMTurma().ObterPorID(t.ID);
                    if (turma.QuantidadeMaximaInscricoes > 0)
                    {
                        if (turma.QuantidadeAlunosMatriculadosNaTurma >= turma.QuantidadeMaximaInscricoes)
                        {
                            throw new AcademicoException("Erro: As vagas para esta turma já foram preenchidas");
                        }
                    }
                }

                //validando se a turma já está chegando ao limite.
                if (qtdInscritosNaOferta >
                    (oferta.QuantidadeMaximaInscricoes -
                     int.Parse(
                         ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.LimiteAlertaInscricaoOferta).Registro)))
                {
                    ManterMatricula.EnviarEmailLimiteOferta(oferta, matriculaOferta);
                }

                //validando se a turma já está chegando ao limite.
                if (qtdInscritosNaOferta >
                    (oferta.QuantidadeMaximaInscricoes -
                     int.Parse(
                         ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.LimiteAlertaInscricaoOferta).Registro)))
                {
                    ManterMatricula.EnviarEmailLimiteOferta(oferta, matriculaOferta);
                }

                #endregion

                bmMatriculaOferta.Salvar(matriculaOferta);
                ValidarMetaIndividual(usuario.ID, solucaoEducacional.ID, pListaIdMetaIndividualAssociada, cpfAutenticacao);
                ValidarMetaInstitucional(usuario.ID, solucaoEducacional.ID, pListaIdMetaInstitucionalAssociada, cpfAutenticacao);

                retornoWebService = retorno;
                return true;
            }

            matriculaTurma = null;
            retornoWebService = null;
            return false;
        }

        /// <summary>
        /// Valida pre requisitos básicos de matrícula na se.
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="solucaoEducacional"></param>
        /// <param name="oferta"></param>
        private void ValidarRequisitosMatricula(Usuario usuario, SolucaoEducacional solucaoEducacional, Oferta oferta)
        {
            //Verificando se existe alguma matricula na SE.
            var buscaMatricula = new MatriculaOferta { Usuario = new Usuario { ID = usuario.ID } };


            var possiveisMatriculas = new BMMatriculaOferta().ObterPorFiltro(buscaMatricula);
             
            if (possiveisMatriculas.Any() &&
                possiveisMatriculas.All(
                    y =>
                        y.Oferta.SolucaoEducacional.ID == solucaoEducacional.ID &&
                        (y.StatusMatricula == enumStatusMatricula.Inscrito ||
                         y.StatusMatricula == enumStatusMatricula.PendenteConfirmacaoAluno)))
            {
                throw new AcademicoException(
                    "Erro: O usuário já está matriculado em uma oferta desta Solução Educacional");
            }

            //VALIDAR SE O USUARIO POSSUI ACESSO A SE
            var usuarioPossuiPermissao = new BMSolucaoEducacional().VerificarSeUsuarioPossuiPermissao(usuario.ID,
                solucaoEducacional.ID); // .ObterListaUsuariosPermitidos();
            if (!usuarioPossuiPermissao)
            {
                throw new AcademicoException("Erro: O usuário Informado não possui permissão à Solução Educacional");
            }

            //VALIDAR SE O USUARIO ESTA CURSANDO OUTRA SE
            IList<MatriculaOferta> listaMatriculaOferta = usuario.ListaMatriculaOferta;
            int cursosEmAndamento =
                listaMatriculaOferta.Count(x => x.StatusMatricula.Equals(enumStatusMatricula.Inscrito));
            int limteCursosSimultaneos =
                int.Parse(ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.CursosSimultaneos).Registro);
            if (cursosEmAndamento >= limteCursosSimultaneos)
            {
                throw new AcademicoException(string.Format("Você já está matriculado em {0} ou mais soluções",
                    limteCursosSimultaneos.ToString()));
            }

            //VALIDAR SE O USUARIO ESTA COM ALGUM ABANDONO ATIVO
            if (new BMUsuarioAbandono().ValidarAbandonoAtivo(usuario.ID))
            {
                throw new AcademicoException("Erro: Existe um abandono registrado para este usuário!");
            }


            if (oferta == null)
                throw new AcademicoException("Erro: Oferta não encontrada");

            ValidaOPeriodoVigenteDaOferta(oferta);

            //VERIFICA PRÉ-REQUISITO
            if (oferta.SolucaoEducacional.ListaPreRequisito.Any())
            {
                var aprovados = new List<enumStatusMatricula>
                {
                    enumStatusMatricula.Aprovado,
                    enumStatusMatricula.Concluido
                };

                foreach (var item in oferta.SolucaoEducacional.ListaPreRequisito)
                {
                    if (
                        !usuario.ListaMatriculaOferta.Any(
                            x =>
                                aprovados.Contains(x.StatusMatricula) &&
                                x.Oferta.SolucaoEducacional.ID == item.PreRequisito.ID))
                    {
                        throw new AcademicoException("Erro: Existem soluções como pré-requisito que não estão cursadas");
                    }
                }
            }
        }

        private static void ValidaOPeriodoVigenteDaOferta(Oferta oferta)
        {
            var ofertaDentroDoPeirodo = (oferta.DataInicioInscricoes.HasValue && oferta.DataInicioInscricoes.Value >= DateTime.Now ||
                                         oferta.DataFimInscricoes.HasValue && DateTime.Now <= oferta.DataFimInscricoes.Value);

            if (!ofertaDentroDoPeirodo)
                throw new AcademicoException("Erro: A oferta está fora do período de inscrição");
        }

        /// <summary>
        /// Realiza inturmação do aluno na oferta.
        /// </summary>
        /// <param name="matriculaOferta"></param>
        /// <param name="autenticacao"></param>
        /// <param name="matricula">Matrícula Turma que será retornda para ser tratada externamente.</param>
        /// <param name="idTurma">ID da turma caso deseja matricular o aluno em uma turma específica. Se não informado, buscará a turma vigente.</param>
        /// <returns>True se turma for incluida com sucesso.</returns>
        private bool IncluirTurma(MatriculaOferta matriculaOferta, string cpfAutenticacao, out MatriculaTurma matricula, int? idTurma = null)
        {
            var oferta = matriculaOferta.Oferta;
            Turma turma = null;

            if (oferta.SolucaoEducacional.Fornecedor.PermiteGestaoSGUS ||
                oferta.SolucaoEducacional.Fornecedor.ID == (int)enumFornecedor.FGVOCW)
                turma =
                    oferta.ListaTurma.FirstOrDefault(
                        x =>
                            ((idTurma.HasValue ? x.ID == idTurma.Value : (x.DataFinal == null ||
                            x.DataFinal.Value.Date >= DateTime.Now.Date)) && x.InAberta));

            if (turma != null)
            {
                var matriculaTurma = new MatriculaTurma
                {
                    Turma = new BMTurma().ObterPorID(turma.ID),
                    Auditoria = new Auditoria(cpfAutenticacao),
                    DataMatricula = DateTime.Now,
                    MatriculaOferta = matriculaOferta
                };

                matriculaTurma.DataLimite = matriculaTurma.CalcularDataLimite(matriculaOferta.Oferta);

                if (matriculaOferta.MatriculaTurma == null)
                    matriculaOferta.MatriculaTurma = new List<MatriculaTurma>();
                matriculaOferta.MatriculaTurma.Add(matriculaTurma);
                matricula = matriculaTurma;

                return true;
            }

            matricula = null;
            return false;
        }


        private void ValidarMetaIndividual(int pUsuario, int pSolucaoEducacional,
                                           List<int> pListaIdMetaIndividualAssociada, string cpfAutenticacao)
        {
            try
            {

                MetaIndividual metaIndividual = null;
                if (pListaIdMetaIndividualAssociada != null && pListaIdMetaIndividualAssociada.Count > 0)
                {

                    foreach (int IdMetaIndividualAssociada in pListaIdMetaIndividualAssociada)
                    {

                        using (BMMetaIndividual miBM = new BMMetaIndividual())
                            metaIndividual = miBM.ObterPorID(IdMetaIndividualAssociada);


                        if (!metaIndividual.ListaItensMetaIndividual.Any(x => x.SolucaoEducacional.ID == pSolucaoEducacional))
                        {
                            metaIndividual.ListaItensMetaIndividual.Add(new ItemMetaIndividual()
                            {
                                Auditoria = new Auditoria(cpfAutenticacao),
                                MetaIndividual = new BMMetaIndividual().ObterPorID(metaIndividual.ID),
                                SolucaoEducacional = new BMSolucaoEducacional().ObterPorId(pSolucaoEducacional),
                            });

                            using (BMMetaIndividual miBM = new BMMetaIndividual())
                                miBM.Salvar(metaIndividual);
                        }

                    }


                    SolucaoEducacional se = null;
                    using (BMSolucaoEducacional seBM = new BMSolucaoEducacional())
                        se = seBM.ObterPorId(pSolucaoEducacional);

                    Usuario user = null;
                    using (BMUsuario userBM = new BMUsuario())
                    {
                        user = userBM.ObterPorId(pUsuario);
                    }

                    bool listaAlterada = false;
                    foreach (var tagSe in se.ListaTags)
                    {
                        if (!user.ListaTag.Any(x => x.Tag.ID == tagSe.ID))
                        {
                            user.ListaTag.Add(new UsuarioTag()
                            {
                                Usuario = user,
                                Auditoria = new Auditoria(cpfAutenticacao),
                                Tag = new BMTag().ObterPorID(tagSe.Tag.ID),
                                DataValidade = metaIndividual.DataValidade,
                                Adicionado = false
                            });
                            listaAlterada = true;
                        }
                    }
                    if (listaAlterada)
                    {
                        using (BMUsuario userBM = new BMUsuario())
                            userBM.Salvar(user);
                    }

                }
            }
            catch
            {
                //TODO: Verificar se cabe alguma ação
            }
        }

        private void ValidarMetaInstitucional(int pUsuario, int pSolucaoEducacional,
                                              List<int> pListaIdMetaInstitucionalAssociada,
                                              string cpfAutenticacao)
        {
            try
            {
                MetaInstitucional mi = null;
                if (pListaIdMetaInstitucionalAssociada != null && pListaIdMetaInstitucionalAssociada.Count > 0)
                {

                    foreach (int IdMetaIndividualAssociada in pListaIdMetaInstitucionalAssociada)
                    {

                        mi = new BMMetaInstitucional().ObterPorID(IdMetaIndividualAssociada);


                        if (!mi.ListaItensMetaInstitucional.Any(x => x.Usuario.ID == pUsuario && x.SolucaoEducacional.ID == pSolucaoEducacional))
                        {
                            mi.ListaItensMetaInstitucional.Add(new ItemMetaInstitucional()
                            {
                                Auditoria = new Auditoria(cpfAutenticacao),
                                MetaInstitucional = new BMMetaInstitucional().ObterPorID(mi.ID),
                                SolucaoEducacional = new BMSolucaoEducacional().ObterPorId(pSolucaoEducacional),
                                Usuario = new BMUsuario().ObterPorId(mi.ID),
                            });

                            using (BMMetaInstitucional miBM = new BMMetaInstitucional())
                                miBM.Salvar(mi);
                        }

                    }

                    SolucaoEducacional se = null;
                    using (BMSolucaoEducacional seBM = new BMSolucaoEducacional())
                        se = seBM.ObterPorId(pSolucaoEducacional);

                    Usuario user = null;
                    using (BMUsuario userBM = new BMUsuario())
                        user = userBM.ObterPorId(pUsuario);

                    foreach (var tagSe in se.ListaTags)
                    {
                        UsuarioTag ut = user.ListaTag.FirstOrDefault(x => x.Tag.ID == tagSe.ID);
                        if (ut == null)
                        {
                            user.ListaTag.Add(new UsuarioTag()
                            {
                                Usuario = new BMUsuario().ObterPorId(pUsuario),
                                Auditoria = new Auditoria(cpfAutenticacao),
                                Tag = new BMTag().ObterPorID(tagSe.Tag.ID),
                                Adicionado = false
                            });
                        }

                        using (BMUsuario userBM = new BMUsuario())
                            userBM.Salvar(user);
                    }
                }
            }
            catch
            {

            }
        }

        public void ManterExternoSolucaoEducacional(DTOSolucaoEducacional pDTOSolucaoEducacional, AuthenticationProviderRequest pAutenticacao)
        {

            try
            {
                this.RegistrarLogExecucaoFornecedor((new BMFornecedor()).ObterPorFiltro(new Fornecedor() { Login = pAutenticacao.Login, Senha = pAutenticacao.Senha }).FirstOrDefault(),
                                                               "CadastrarSolucaoEducacional");

                SolucaoEducacional solucaoEducacional = new BMSolucaoEducacional().ObterPorIDFornecedorEIdChaveExterna(pAutenticacao.Login, pDTOSolucaoEducacional.IDChaveExterna);

                if (solucaoEducacional == null)
                {
                    //CRIACAO
                    ValidarCamposSolucaoEducacional(pDTOSolucaoEducacional);

                    solucaoEducacional = PreencherObjetoSolucaoEducacional(pDTOSolucaoEducacional, pAutenticacao, solucaoEducacional);
                    solucaoEducacional.DataCadastro = DateTime.Now;
                    if (solucaoEducacional.FormaAquisicao == null)
                        throw new AcademicoException("A forma de aquisição não foi encontrada");
                    if (solucaoEducacional.CategoriaConteudo == null)
                        throw new AcademicoException("A categoria não foi encontrada");

                    new BMSolucaoEducacional().Salvar(solucaoEducacional);

                }
                else
                {
                    ValidarCamposSolucaoEducacional(pDTOSolucaoEducacional);
                    solucaoEducacional = PreencherObjetoSolucaoEducacional(pDTOSolucaoEducacional, pAutenticacao, solucaoEducacional);
                    new BMSolucaoEducacional().Salvar(solucaoEducacional);
                }


            }

            catch (AcademicoException ex)
            {
                throw new AcademicoException(ex.Message);
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
        }

        private static void ValidarCamposSolucaoEducacional(DTOSolucaoEducacional pDTOSolucaoEducacional)
        {
            if (!(pDTOSolucaoEducacional.IdFormaAquisicao > 0))
                throw new AcademicoException("A forma de aquisição é obrigatória");
            if (!(pDTOSolucaoEducacional.IdCategoriaSolucaoEducacional > 0))
                throw new AcademicoException("A categoria é obrigatória");
            if (string.IsNullOrWhiteSpace(pDTOSolucaoEducacional.IDChaveExterna))
                throw new AcademicoException("A chave externa é obrigatória");
            if (string.IsNullOrWhiteSpace(pDTOSolucaoEducacional.Nome))
                throw new AcademicoException("O nome da solução é obrigatório");
        }

        private static SolucaoEducacional PreencherObjetoSolucaoEducacional(DTOSolucaoEducacional pDTOSolucaoEducacional, AuthenticationProviderRequest pAutenticacao, SolucaoEducacional solucaoEducacional)
        {
            solucaoEducacional = new SolucaoEducacional()
            {
                Apresentacao = pDTOSolucaoEducacional.Apresentacao,
                Autor = pDTOSolucaoEducacional.Autor,
                CategoriaConteudo = pDTOSolucaoEducacional.IdCategoriaSolucaoEducacional > 0 ? (new BMCategoriaConteudo()).ObterPorID(2) : null,
                Ementa = pDTOSolucaoEducacional.Ementa,
                FormaAquisicao = (new BMFormaAquisicao()).ObterPorID(pDTOSolucaoEducacional.IdFormaAquisicao),
                Fornecedor = (new BMFornecedor()).ObterPorFiltro(new Fornecedor() { Login = pAutenticacao.Login }).FirstOrDefault(),
                Nome = pDTOSolucaoEducacional.Nome,
                Objetivo = pDTOSolucaoEducacional.Objetivo,
                Ativo = pDTOSolucaoEducacional.Ativo,
                TemMaterial = pDTOSolucaoEducacional.TemMaterial,
                IDChaveExterna = pDTOSolucaoEducacional.IDChaveExterna,
                Auditoria = new Auditoria(pAutenticacao.Login),
            };
            return solucaoEducacional;
        }

        public List<DTOSolucaoEducacional> ConsultarSolucaoEducacionalPorFornecedor(string idChaveExterna, AuthenticationProviderRequest autenticacao)
        {
            solucaoEducacionalBM = new BMSolucaoEducacional();

            SolucaoEducacional solucaoFiltro = new SolucaoEducacional();
            solucaoFiltro.Fornecedor = new BMFornecedor().ObterPorLogin(autenticacao.Login);
            solucaoFiltro.IDChaveExterna = idChaveExterna;

            var listaSolucao = solucaoEducacionalBM.ObterPorFiltro(solucaoFiltro);

            List<DTOSolucaoEducacional> listaRetorno = new List<DTOSolucaoEducacional>();
            foreach (var registro in listaSolucao)
            {
                DTOSolucaoEducacional dtoRegistro = new DTOSolucaoEducacional();
                dtoRegistro.Apresentacao = registro.Apresentacao;
                dtoRegistro.Ativo = registro.Ativo;
                dtoRegistro.Autor = registro.Autor;
                dtoRegistro.Ementa = registro.Ementa;
                dtoRegistro.ID = registro.ID;
                dtoRegistro.IdCategoriaSolucaoEducacional = registro.CategoriaConteudo.ID;
                dtoRegistro.IDChaveExterna = registro.IDChaveExterna;
                dtoRegistro.IdFormaAquisicao = registro.FormaAquisicao.ID;
                dtoRegistro.Nome = registro.Nome;
                dtoRegistro.Objetivo = registro.Objetivo;
                dtoRegistro.TemMaterial = registro.TemMaterial;


                listaRetorno.Add(dtoRegistro);
            }
            return listaRetorno;
        }

        public string FormatarTextoEmail(IDictionary<string, string> registros, string textoEmail)
        {
            return registros.Keys.Aggregate(textoEmail, (current, chave) => current.Replace(chave, registros[chave]));
        }

        public RetornoWebService MatricularTurma(int idUsuario, int turmaId, List<int> pListaIdMetaIndividualAssociada,
            List<int> pListaIdMetaInstitucionalAssociada, string cpfAutenticacao, ItemTrilha itemTrilha = null)
        {
            var turma = new ManterTurma().ObterTurmaPorID(turmaId);

            if (turma == null)
                throw new AcademicoException("Turma não encontrada");

            return MatricularSolucaoEducacional(idUsuario, turma.Oferta.SolucaoEducacional.ID, turma.Oferta.ID,
                pListaIdMetaIndividualAssociada, pListaIdMetaIndividualAssociada, cpfAutenticacao, turma.ID, itemTrilha);
        }

        public RetornoWebService SincronizarUsuarioSAS(int idOferta)
        {
            try
            {
                var oferta = new ManterOferta().ObterOfertaPorID(idOferta);

                if (!oferta.SolucaoEducacional.IntegracaoComSAS)
                    throw new AcademicoException("Solução Educacional não permite Integração com o SAS.");

                var matriculasOferta = new ManterMatriculaOferta().ObterTodosIQueryable()
                    .Where(x => x.Oferta.ID == idOferta && x.StatusMatricula == enumStatusMatricula.Inscrito);

                foreach (var matricula in matriculasOferta)
                {
                    if (oferta.SolucaoEducacional.IntegracaoComSAS)
                        CadastrarSAS(matricula.Usuario, new TreinamentoSasClient());
                }

                return new RetornoWebService { };
            }
            catch
            {

                throw;
            }
        }

        public void MatricularAlunoMoodle(Usuario usuario, Oferta oferta)
        {
            if (oferta.SolucaoEducacional.Fornecedor.ID == (int)enumFornecedor.MoodleSebrae && oferta.CodigoMoodle.HasValue)
            {
                var url = (new ManterConfiguracaoSistema()).ObterConfiguracaoSistemaPorID((int)enumConfiguracaoSistema.MoodleUrlMatricular).Registro.Replace("#CPF", usuario.CPF);

                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                var response = (HttpWebResponse)request.GetResponse();
            }
        }
        public HttpWebResponse AtualizarStatusMatriculaAlunoMoodle(string cpf, string IDChaveExterna, int status)
        {
            var url = (new ManterConfiguracaoSistema()).ObterConfiguracaoSistemaPorID((int)enumConfiguracaoSistema.MoodleUrlAtualizarStatus).Registro
                    .Replace("#CPF", cpf)
                    .Replace("#STATUS_MATRICULA", status.ToString())
                    .Replace("#ID_CHAVE_EXTERNA", IDChaveExterna);


            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            return (HttpWebResponse)request.GetResponse();

        }
    }
}
using System;
using System.Linq;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Web;
using Sebrae.Academico.BP.Services.SgusWebService;
using System.Net;
using System.Threading;

namespace Sebrae.Academico.BP.Services
{
    public class ConsultarMeusCursos : BusinessProcessServicesBase
    {
        public List<DTOItemMeusCursos> ObterMeusCursos(int id_Usuario, bool portalNovo = false)
        {
            var resultado = new List<DTOItemMeusCursos>();

            try
            {

                if (id_Usuario > 0)
                {
                    var listaMatriculaOferta = new BMMatriculaOferta()
                            .ObterPorUsuarioSemMatriculasCapacitacoes(id_Usuario);

                    PreencherDTOComInformacoesDaMatriculaOferta(resultado, listaMatriculaOferta);

                    // Busca matriculas em Trilhas e Programas
                    PreencherDTOComInformacoesDoUsuarioTrilha(id_Usuario, resultado);
                    PreencherDTOComInformacoesDaMatriculaPrograma(id_Usuario, resultado);
                }
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return resultado;
        }

        private void PreencherDTOComInformacoesDaMatriculaPrograma(int id_Usuario, List<DTOItemMeusCursos> resultado)
        {

            DTOItemMeusCursos dtoItemMeusCursos = null;

            try
            {
                IList<MatriculaCapacitacao> ListaMatriculaCapacitacao = new BMMatriculaCapacitacao().ObterPorUsuario(id_Usuario).Where(x => x.StatusMatricula.Equals(enumStatusMatricula.Inscrito)).OrderBy(x => x.Capacitacao.Programa.ID).ToList();

                var manterMatCapacitacao = new ManterMatriculaCapacitacaoService();

                int prevPrograma = 0;
                int cont = 0;
                double totalAprovacoesSol = 0;
                double totalSol = 0;

                foreach (var itemHistorico in ListaMatriculaCapacitacao)
                {
                    BMCapacitacao bmCapacitacao = new BMCapacitacao();

                    // PROGRAMAS - Mudou o programa cria um novo item pai
                    if (prevPrograma != itemHistorico.Capacitacao.Programa.ID)
                    {
                        totalAprovacoesSol = 0;
                        totalSol = 0;

                        dtoItemMeusCursos = new DTOItemMeusCursos();
                        dtoItemMeusCursos.ID = itemHistorico.Capacitacao.Programa.ID;
                        dtoItemMeusCursos.NomeSolucao = itemHistorico.Capacitacao.Programa.Nome;
                        dtoItemMeusCursos.Fornecedor = Constantes.Sebrae;
                        dtoItemMeusCursos.TextoApresentacao = itemHistorico.Capacitacao.Programa.Apresentacao;
                        dtoItemMeusCursos.DataInicio = itemHistorico.Capacitacao.DataInicio.ToString("dd/MM/yyyy");
                        dtoItemMeusCursos.DataLimite = itemHistorico.Capacitacao.DataFim.HasValue ? itemHistorico.Capacitacao.DataFim.Value.ToString("dd/MM/yyyy") : "";
                        dtoItemMeusCursos.Tipo = Constantes.ProgramaUC;
                        dtoItemMeusCursos.CargaHoraria = Constantes.NaoDefinido;
                        dtoItemMeusCursos.IdProgramaPortal = itemHistorico.Capacitacao.Programa.IdNodePortal;
                        dtoItemMeusCursos.LinkAcesso = "";//!string.IsNullOrEmpty(itemHistorico.Capacitacao.Programa.Acesso) ? itemHistorico.Capacitacao.Programa.Acesso : "";                        
                    }

                    DTOCapacitacao dtoCapacitacao = manterMatCapacitacao.AprovacoesSolucoesEducacionais(itemHistorico, id_Usuario);

                    dtoItemMeusCursos.CapacitacoesPrograma.Add(dtoCapacitacao);

                    string[] totais = dtoCapacitacao.TextoConclusaoCapacitacao.Split('/');

                    totalAprovacoesSol += float.Parse(totais[0]);
                    totalSol += float.Parse(totais[1]);

                    //dtoItemMeusCursos.LinkSemAcesso.Add(new DTOLinkSemAcesso { MotivoLinkSemAcesso = string.Format("Este curso será iniciado no dia {0}", exibirData.ToString("dd/MM/yyyy")) });                    

                    // Se o próximo programa for diferente ou for o último insere no DTO Pai os filhos
                    if (ListaMatriculaCapacitacao.Count - 1 == cont || itemHistorico.Capacitacao.Programa.ID != ListaMatriculaCapacitacao[cont + 1].Capacitacao.Programa.ID)
                    {
                        dtoItemMeusCursos.PorcentagemConlusaoPrograma = (int)(Math.Round(totalAprovacoesSol / totalSol, 2) * 100);
                        dtoItemMeusCursos.TextoConclusaoPrograma = totalAprovacoesSol + "/" + totalSol;
                        resultado.Add(dtoItemMeusCursos);
                    }

                    // Atualiza a referência ao passado
                    prevPrograma = itemHistorico.Capacitacao.Programa.ID;

                    cont++;
                }
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
        }

        private void PreencherDTOComInformacoesDoUsuarioTrilha(int idUsuario, List<DTOItemMeusCursos> resultado)
         {
            try
            {
                var listaUsuarioTrilha = new BMUsuarioTrilha()
                        .ObterPorUsuario(idUsuario)
                        .Where(x => x.NovasTrilhas == true)
                        .Where(x => x.DataLimite >= DateTime.Now || x.DataLimite == null);

                foreach (var usuarioTrilha in listaUsuarioTrilha)
                {
                    if (!usuarioTrilha.StatusMatricula.Equals(enumStatusMatricula.Inscrito) && !usuarioTrilha.StatusMatricula.Equals(enumStatusMatricula.Aprovado))
                        continue;

                    var item = new DTOItemMeusCursos
                    {
                        NomeSolucao =
                            string.Concat(usuarioTrilha.TrilhaNivel.Trilha.Nome, " - ", usuarioTrilha.TrilhaNivel.Nome),
                        Fornecedor = Constantes.Sebrae,
                        IdTrilhaNivel = usuarioTrilha.TrilhaNivel.ID,
                        IdTrilha = usuarioTrilha.TrilhaNivel.Trilha.IdNodePortal,
                        DataInicio = usuarioTrilha.DataInicio.ToString("dd/MM/yyyy"),
                        DataLimite = usuarioTrilha.DataLimite != null ? usuarioTrilha.DataLimite.ToString("dd/MM/yyyy") : "",
                        PrazoEmDias = usuarioTrilha.TrilhaNivel.QuantidadeDiasPrazo,
                        IdMatricula = usuarioTrilha.ID,
                        Tipo = Constantes.TrilhaUC
                    };

                    var limiteCancelamentoUsuario = usuarioTrilha.TrilhaNivel.LimiteCancelamento > 0
                    ? usuarioTrilha.DataInicio.AddDays(usuarioTrilha.TrilhaNivel.LimiteCancelamento)
                    : usuarioTrilha.DataInicio.AddDays(usuarioTrilha.TrilhaNivel.QuantidadeDiasPrazo);

                    if (string.IsNullOrEmpty(item.DataLimite))
                    {
                        item.DataLimite = limiteCancelamentoUsuario.ToString("dd/MM/yyyy");
                    }

                    item.CargaHoraria = usuarioTrilha.TrilhaNivel.CargaHoraria.ToString();

                    var urlPortal =
                        ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoPortal).Registro;

                    item.LinkAcesso = urlPortal + (urlPortal.EndsWith("/") ? "" : "/") +
                                      string.Format(
                                          ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.UrlTrilha)
                                              .Registro, usuarioTrilha.TrilhaNivel.ID);

                    // Aprovado ou não aprovado na prova pode cancelar a matrícula caso esteja dentro da data de cancelamento
                    try
                    {
                        if (limiteCancelamentoUsuario >= DateTime.Now)
                        {
                            item.HabilitaCancelamento = true;
                        }
                        else
                        {
                            item.HabilitaCancelamento = false;
                        }
                    }
                    catch
                    {
                        item.HabilitaCancelamento = false;
                    }
                    resultado.Add(item);
                }
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
        }

        public void PreencherDTOComInformacoesDaMatriculaOferta(List<DTOItemMeusCursos> resultado, IList<MatriculaOferta> listaMatriculaOferta)
        {
            try
            {
                foreach (var itemMatriculaOferta in listaMatriculaOferta)
                {
                    if (itemMatriculaOferta.StatusMatricula.Equals(enumStatusMatricula.Inscrito) || itemMatriculaOferta.StatusMatricula.Equals(enumStatusMatricula.PendenteConfirmacaoAluno))
                    {
                        var dtoItemMeusCursos = new DTOItemMeusCursos
                        {
                            ID = itemMatriculaOferta.Oferta.SolucaoEducacional.ID,
                            NomeSolucao = itemMatriculaOferta.Oferta.SolucaoEducacional.Nome,
                            Fornecedor = itemMatriculaOferta.Oferta.SolucaoEducacional.Fornecedor.Nome,
                            SituacaoID = (int)itemMatriculaOferta.StatusMatricula,
                            Situacao = itemMatriculaOferta.StatusMatriculaFormatado,
                            IDChaveExterna = itemMatriculaOferta.Oferta.SolucaoEducacional.IDChaveExterna,
                            IDNode = itemMatriculaOferta.Oferta.SolucaoEducacional.IdNodePortal,
                            PrazoEmDias = itemMatriculaOferta.Oferta.DiasPrazo,

                            // Redmine #1112 - Antes de mudar a condição abaixo verifique o redmine
                            // Somente será enviado o link caso o IDChaveExterna seja nulo
                            Link = itemMatriculaOferta.Oferta.IDChaveExterna == null
                                ? itemMatriculaOferta.Oferta.Link
                                : ""
                        };

                        var matriculaTurma = itemMatriculaOferta.MatriculaTurma.FirstOrDefault();

                        if (matriculaTurma == null)
                            continue;

                        // Pega a última data de início nessa ordem de prioridade Data Solicitação, Data Início Turma e Data Início Oferta
                        var dataInicio = matriculaTurma.Turma.DataInicio;

                        if (itemMatriculaOferta.Oferta.TipoOferta == enumTipoOferta.Continua)
                        {                            
                            if(itemMatriculaOferta.MatriculaTurma.Count() > 0)
                            {
                                var dataMatricula = itemMatriculaOferta.MatriculaTurma[0].DataMatricula;

                                dataInicio = dataMatricula > dataInicio ? dataMatricula : dataInicio;
                            }

                        }

                        dtoItemMeusCursos.DataInicio = dataInicio.ToString("dd/MM/yyyy");
                        dtoItemMeusCursos.DataLimite = matriculaTurma.CalcularDataLimite().ToString("dd/MM/yyyy");

                        if (itemMatriculaOferta.MatriculaTurma.Count > 0)
                        {
                            dtoItemMeusCursos.IdTurma = matriculaTurma.Turma.ID;

                            #region VALIDAR QUESTIONARIOS

                            //QUESTIONARIO PRE
                            dtoItemMeusCursos.QuestionarioPrePendente = VerificarQuestionarioPendente(matriculaTurma, itemMatriculaOferta, enumTipoQuestionarioAssociacao.Pre);

                            if (dtoItemMeusCursos.QuestionarioPrePendente
                                || itemMatriculaOferta.Oferta.ListaTurma.Any(t => t.DataInicio > DateTime.Now)
                                ||
                                (matriculaTurma.Turma.DataInicio > DateTime.Now))
                            {
                                dtoItemMeusCursos.LinkAcesso = string.Empty;

                                if (dtoItemMeusCursos.QuestionarioPrePendente)
                                    dtoItemMeusCursos.LinkSemAcesso.Add(new DTOLinkSemAcesso
                                    {
                                        MotivoLinkSemAcesso = "Responda ao Questionário para acessar o curso"
                                    });

                                var dataOferta = matriculaTurma.Turma.DataInicio;
                                var exibirData = dataOferta;

                                if (matriculaTurma.Turma.DataInicio.Date > DateTime.Today)
                                {

                                    var dataTurma = matriculaTurma.Turma.DataInicio;
                                    if (dataTurma > dataOferta)
                                    {
                                        {
                                            exibirData = dataTurma;
                                        }
                                    }


                                    dtoItemMeusCursos.LinkSemAcesso.Add(new DTOLinkSemAcesso
                                    {
                                        MotivoLinkSemAcesso =
                                            string.Format("Este curso será iniciado no dia {0}", exibirData.ToString("dd/MM/yyyy"))
                                    });
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(itemMatriculaOferta.LinkAcesso))
                                    dtoItemMeusCursos.LinkAcesso =
                                        ConsultarLinkAcessoFornecedor(
                                            itemMatriculaOferta.Oferta.SolucaoEducacional.Fornecedor,
                                            itemMatriculaOferta.Usuario,
                                            itemMatriculaOferta.Oferta.CodigoMoodle.ToString());
                                else
                                    dtoItemMeusCursos.LinkAcesso = itemMatriculaOferta.LinkAcesso;
                            }
                        }

                        //Essa chamada é feita para realizar a sincronia com o moodle, então no momento que carrega a página de "Minhas inscrições" no portal, o sistema acessa a url do moodle, sendo assim, é feita a sincronia do usuário com o moodle.
                        if (dtoItemMeusCursos.Fornecedor == "Moodle")
                        {
                            if (!string.IsNullOrEmpty(dtoItemMeusCursos.LinkAcesso))
                            {
                                var thread = new Thread(() =>
                                {
                                    WebRequest request = HttpWebRequest.Create(dtoItemMeusCursos.LinkAcesso);
                                    //WebResponse response = request.GetResponse();
                                })
                                {
                                    IsBackground = true
                                };
                                thread.Start();
                            }
                        }

                        dtoItemMeusCursos.IdMatricula = itemMatriculaOferta.ID;
                        dtoItemMeusCursos.IdOferta = itemMatriculaOferta.Oferta.ID;
                        dtoItemMeusCursos.Tipo = Constantes.CursoUC;

                        dtoItemMeusCursos.CargaHoraria = itemMatriculaOferta.Oferta.CargaHoraria.ToString();

                        int diasCancelamento = int.Parse(ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.DiasCancelamentoCurso).Registro);

                        dtoItemMeusCursos.HabilitaCancelamento = itemMatriculaOferta.DataSolicitacao.Date.AddDays(diasCancelamento) >= DateTime.Now;

                        // Verificar questionário de cancelamento.
                        if (dtoItemMeusCursos.HabilitaCancelamento)
                        {
                            dtoItemMeusCursos.QuestionarioCancelamentoPendente = false;

                            if (itemMatriculaOferta.MatriculaTurma.Any())
                            {
                                var turma = itemMatriculaOferta.MatriculaTurma[0].Turma;

                                var associacao = turma.ListaQuestionarioAssociacao.FirstOrDefault(
                                    x => x.TipoQuestionarioAssociacao.ID ==
                                         (int)enumTipoQuestionarioAssociacao.Cancelamento);

                                if (associacao != null)
                                {
                                    dtoItemMeusCursos.QuestionarioCancelamentoPendente = true;

                                    dtoItemMeusCursos.QuestionarioRespondido =
                                        associacao.Questionario.IsRespondido(itemMatriculaOferta.Usuario, turma);
                                }
                            }
                        }

                        dtoItemMeusCursos.QuestionarioCancelamentoPendente = VerificarQuestionarioPendente(matriculaTurma, itemMatriculaOferta, enumTipoQuestionarioAssociacao.Cancelamento);

                        if (itemMatriculaOferta.MatriculaTurma.Any())
                        {
                            var turma = itemMatriculaOferta.MatriculaTurma[0].Turma;

                            var associacao = turma.ListaQuestionarioAssociacao.FirstOrDefault(
                                x => x.TipoQuestionarioAssociacao.ID ==
                                     (int)enumTipoQuestionarioAssociacao.Cancelamento);

                            if (associacao != null)
                            {
                                dtoItemMeusCursos.QuestionarioCancelamentoPendente = true;

                                dtoItemMeusCursos.QuestionarioRespondido =
                                    associacao.Questionario.IsRespondido(itemMatriculaOferta.Usuario, turma);
                            }
                        }

                        dtoItemMeusCursos.QuestionarioEficaciaPendente = VerificarQuestionarioPendente(matriculaTurma, itemMatriculaOferta, enumTipoQuestionarioAssociacao.Eficacia);
                        dtoItemMeusCursos.QuantidadeItensQuestionarioAgrupados = ConsultarQuantidadeItensQuestionarioAssociados(matriculaTurma);


                        #endregion

                        resultado.Add(dtoItemMeusCursos);
                    }
                }

            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

        }

        /// <summary>
        /// Verifica se existem pendências de resposta do questionário informado
        /// </summary>
        /// <param name="matricula"></param>
        /// <param name="mtOferta"></param>
        /// <param name="tipoQuestionario"></param>
        /// <returns></returns>
        public bool VerificarQuestionarioPendente(MatriculaTurma matricula, MatriculaOferta mtOferta, enumTipoQuestionarioAssociacao tipoQuestionario)
        {
            if (matricula.Turma.ListaQuestionarioAssociacao.Count > 0)
            {
                var questionarioAssociacao =
                    matricula.Turma.ListaQuestionarioAssociacao.FirstOrDefault(
                        x => x.TipoQuestionarioAssociacao.ID == (int)tipoQuestionario);

                if (questionarioAssociacao != null)
                {
                    if (
                        !(new BMQuestionarioParticipacao().ObterPorUsuario(mtOferta.Usuario)
                            .Any(
                                x =>
                                    x.TipoQuestionarioAssociacao.ID ==
                                    (int)tipoQuestionario &&
                                    x.Turma.ID == mtOferta.MatriculaTurma.FirstOrDefault().Turma.ID &&
                                    x.DataParticipacao != null)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Consulta a quantidade de itens questionário agrupados
        /// </summary>
        /// <param name="matricula"></param>
        /// <returns></returns>
        public int ConsultarQuantidadeItensQuestionarioAssociados(MatriculaTurma matricula)
        {
            if (matricula.Turma.ListaQuestionarioAssociacao.Count > 0)
            {
                var itemQuestionarioAssociacaoEficacia =
                    matricula.Turma.ListaQuestionarioAssociacao.FirstOrDefault(
                        x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Eficacia);
                if (itemQuestionarioAssociacaoEficacia != null)
                {
                    int quantidade = new ManterQuestionario().QuantidadeQuestionariosAgrupados(itemQuestionarioAssociacaoEficacia.Questionario);
                    return quantidade > 0 ? quantidade : 1;
                }
            }

            return 1;
        }

        public string ConsultarLinkAcessoFornecedor(Fornecedor fornecedor, Usuario usuario, string codigoMoodle)
        {
            var retorno = string.Empty;
            try
            {
                switch (fornecedor.ID)
                {
                    case (int)enumFornecedor.WebAula:
                        retorno = fornecedor.LinkAcesso;
                        retorno = retorno.Replace("#TOKEN", HttpUtility.UrlEncode(CriptografiaHelper.Criptografar(usuario.CPF, fornecedor.TextoCriptografia)));
                        break;
                    case (int)enumFornecedor.MoodleSebrae:
                        retorno = fornecedor.LinkAcesso;
                        retorno = retorno.Replace("#CPFAES", HttpUtility.UrlEncode(CriptografiaHelper.Criptografar(usuario.CPF, fornecedor.TextoCriptografia)));
                        retorno = retorno.Replace("#CPF", usuario.CPF);
                        retorno = retorno.Replace("#SENAES", HttpUtility.UrlEncode(CriptografiaHelper.Criptografar(CriptografiaHelper.Decriptografar(usuario.Senha), fornecedor.TextoCriptografia)));
                        retorno = retorno.Replace("#CODIGOMOODLE", codigoMoodle);
                        break;
                }
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
            return retorno;
        }
    }
}

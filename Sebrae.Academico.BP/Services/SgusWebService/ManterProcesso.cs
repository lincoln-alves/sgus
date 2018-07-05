using System;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.Dominio.Classes;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.BP.DTO.Services.Processo;
using Sebrae.Academico.BP.DTO.Services.Questionario;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ManterProcesso : BusinessProcessServicesBase
    {
        BMProcesso bmProcesso = new BMProcesso();
        BMUsuario bmUsuario = new BMUsuario();

        #region Métodos de Usuário

        public IList<DTOProcessoInfo> ConsultarProcessosPermitidosInscricao(string cpf)
        {
            var usuario = bmUsuario.ObterPorCPF(cpf);

            if (usuario == null)
                return new List<DTOProcessoInfo>();

            var parametros = new Dictionary<string, object>()
            {
                { "p_IDUsuario", usuario.ID }
            };

            return SQLUtil.ExecutarProcedure<DTOProcessoInfo>("SP_PROCESSOS_PERMITIDOS_INICIAR", parametros);
        }

        public Processo ObterPorID(int id)
        {
            return bmProcesso.ObterPorId(id);
        }

        public EtapaEncaminhamentoUsuario SalvarEncaminhamentoEtapaUsuario(int idEtapaResposta, int idEtapaPermissaoNucleo, int idUsuario, string txJustificativa)
        {
            try
            {
                //var usuarioAlteracao = new ManterUsuario().ObterPorCPF(cpf);

                EtapaResposta etapaResposta = new ManterEtapaResposta().ObterPorID(idEtapaResposta);

                EtapaEncaminhamentoUsuario etapaEncaminhamentoUsuario = etapaResposta.ListaEtapaEncaminhamentoUsuario.Where(x => x.UsuarioEncaminhamento.ID == idUsuario).FirstOrDefault();

                if (etapaEncaminhamentoUsuario != null)
                {
                    etapaEncaminhamentoUsuario.StatusEncaminhamento = (int)enumStatusEncaminhamentoEtapa.Aguardando;
                    etapaEncaminhamentoUsuario.EtapaPermissaoNucleo = new ManterEtapaPermissaoNucleo().ObterPorID(idEtapaPermissaoNucleo);
                    etapaEncaminhamentoUsuario.Justificativa = txJustificativa;
                    etapaEncaminhamentoUsuario.DataSolicitacaoEncaminhamento = DateTime.Now;
                }
                else
                {
                    etapaEncaminhamentoUsuario = new EtapaEncaminhamentoUsuario()
                    {
                        EtapaResposta = etapaResposta,
                        EtapaPermissaoNucleo = new ManterEtapaPermissaoNucleo().ObterPorID(idEtapaPermissaoNucleo),
                        UsuarioEncaminhamento = new ManterUsuario().ObterPorID(idUsuario),
                        StatusEncaminhamento = 0,
                        DataSolicitacaoEncaminhamento = DateTime.Now,
                        Justificativa = txJustificativa
                    };

                    etapaResposta.ListaEtapaEncaminhamentoUsuario.Add(etapaEncaminhamentoUsuario);
                }

                new ManterEtapaResposta().Salvar(etapaResposta);

                //ENVIA NOTIFICACAO USUARIO DESTINATARIO
                var idUsuarioDestinatario = etapaEncaminhamentoUsuario.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.Usuario.ID;
                var mensagemNotificacao = string.Format("A Analise da Demanda #{0} foi encaminhado pelo usuário {1}",
                                        etapaResposta.ProcessoResposta.ID,
                                        etapaEncaminhamentoUsuario.UsuarioEncaminhamento.Nome);

                var notificacaoEnvio = new ManterNotificacaoEnvio().ObterTodosIQueryable().FirstOrDefault();
                string urlDemanda = string.Format("/demandas/analisar/0/{0}", etapaResposta.ProcessoResposta.ID);

                new BP.ManterNotificacao().PublicarNotificacao(urlDemanda, mensagemNotificacao, idUsuarioDestinatario, notificacaoEnvio);

                //ENVIA NOTIFICACAO PARA O SUPERIOR DA HIERARQUIA
                var superiorHierarquia = new ManterHierarquiaNucleoUsuario().ObterTodosIQueryable()
                    .Where(x => x.HierarquiaNucleo.ID == etapaEncaminhamentoUsuario.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.HierarquiaNucleo.ID && x.IsGestor).FirstOrDefault();

                //SÓ ENVIA NOTIFICAÇÃO CASO EXISTA ALGUM REGISTRO DE SUPERIORHIERARQUIA
                if (superiorHierarquia != null)
                    new BP.ManterNotificacao().PublicarNotificacao(urlDemanda, mensagemNotificacao, superiorHierarquia.Usuario.ID, notificacaoEnvio);

                return etapaEncaminhamentoUsuario;
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        public EtapaEncaminhamentoUsuario RetornoEncaminhamentoEtapaUsuario(int idEtapaEncamihamentoUsuario, int statusEncaminhamento, string txJustificativa)
        {
            try
            {
                EtapaResposta etapaResposta = new ManterEtapaResposta().ObterTodosIQueryable().Where(x => x.ListaEtapaEncaminhamentoUsuario.Select(z => new { z.ID, z.StatusEncaminhamento }).Any(y => y.ID == idEtapaEncamihamentoUsuario && y.StatusEncaminhamento == (int)enumStatusEncaminhamentoEtapa.Aguardando)).FirstOrDefault();

                EtapaEncaminhamentoUsuario etapaEncaminhamentoUsuario = etapaResposta.ListaEtapaEncaminhamentoUsuario
                    .Where(x => x.ID == idEtapaEncamihamentoUsuario)
                    .Where(x => x.StatusEncaminhamento == (int)enumStatusEncaminhamentoEtapa.Aguardando)
                    .FirstOrDefault();

                etapaEncaminhamentoUsuario.StatusEncaminhamento = statusEncaminhamento;
                if (!string.IsNullOrWhiteSpace(txJustificativa))
                {
                    etapaEncaminhamentoUsuario.Justificativa = txJustificativa;
                }

                string mensagemNotificacao = "";
                if (etapaEncaminhamentoUsuario.StatusEncaminhamento == (int)enumStatusEncaminhamentoEtapa.Aprovado)
                {
                    var etapaRespostaPermissao = etapaEncaminhamentoUsuario.EtapaResposta.PermissoesNucleoEtapaResposta
                        .Where(p => p.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.Usuario.ID == etapaEncaminhamentoUsuario.UsuarioEncaminhamento.ID).FirstOrDefault();

                    if (etapaRespostaPermissao != null)
                        etapaRespostaPermissao.EtapaPermissaoNucleo = etapaEncaminhamentoUsuario.EtapaPermissaoNucleo;

                    mensagemNotificacao = string.Format("O Encaminhamento de Analise da Demanda #{0} foi aceito pelo usuário {1}",
                        etapaEncaminhamentoUsuario.EtapaResposta.ProcessoResposta.ID,
                        etapaEncaminhamentoUsuario.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.Usuario.Nome);
                }
                else
                {
                    mensagemNotificacao = string.Format("O Encaminhamento de Analise da Demanda #{0} foi negado pelo usuário {1}",
                        etapaEncaminhamentoUsuario.EtapaResposta.ProcessoResposta.ID,
                        etapaEncaminhamentoUsuario.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.Usuario.Nome);
                }

                var idUsuarioRemetente = etapaEncaminhamentoUsuario.UsuarioEncaminhamento.ID;

                //ENVIAR NOTIFICACAO USUARIO REMENTENTE
                var notificacaoEnvio = new ManterNotificacaoEnvio().ObterTodosIQueryable().FirstOrDefault();
                string urlDemanda = string.Format("/demandas/analisar/0/{0}", etapaEncaminhamentoUsuario.EtapaResposta.ProcessoResposta.ID);

                new BP.ManterNotificacao().PublicarNotificacao(urlDemanda, mensagemNotificacao, idUsuarioRemetente, notificacaoEnvio);

                //ENVIA NOTIFICACAO PARA O SUPERIOR DA HIERARQUIA
                var superiorHierarquia = new ManterHierarquiaNucleoUsuario().ObterTodosIQueryable()
                    .Where(x => x.HierarquiaNucleo.ID == etapaEncaminhamentoUsuario.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.HierarquiaNucleo.ID && x.IsGestor).FirstOrDefault();
                new BP.ManterNotificacao().PublicarNotificacao(urlDemanda, mensagemNotificacao, superiorHierarquia.Usuario.ID, notificacaoEnvio);


                new ManterEtapaResposta().Salvar(etapaResposta);

                return etapaEncaminhamentoUsuario;
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        public void CancelaEncaminhamentoEtapaUsuarioPendentes()
        {
            try
            {
                var etapasEncaminhamentoUsuarioPendentes = new ManterEtapaEncaminhamentoUsuario().ObterTodosIQueryable()
                    .Where(x => x.StatusEncaminhamento == (int)enumStatusEncaminhamentoEtapa.Aguardando && x.DataSolicitacaoEncaminhamento > DateTime.Today.AddDays(-5)).ToList();

                if (etapasEncaminhamentoUsuarioPendentes != null)
                {
                    foreach (EtapaEncaminhamentoUsuario etapaEncaminhamentoUsuario in etapasEncaminhamentoUsuarioPendentes)
                    {
                        etapaEncaminhamentoUsuario.StatusEncaminhamento = (int)enumStatusEncaminhamentoEtapa.Negado;

                        new ManterEtapaEncaminhamentoUsuario().Salvar(etapaEncaminhamentoUsuario);

                        var mensagemNotificacao = string.Format("O Encaminhamento de Analise da Demanda #{0} excedeu o prazo de 5 dias e foi cancelado automaticamente.",
                        etapaEncaminhamentoUsuario.EtapaResposta.ProcessoResposta.ID,
                        etapaEncaminhamentoUsuario.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.Usuario.Nome);

                        //ENVIAR NOTIFICACAO USUARIO REMENTENTE
                        var notificacaoEnvio = new ManterNotificacaoEnvio().ObterTodosIQueryable().FirstOrDefault();
                        string urlDemanda = string.Format("/demandas/analisar/0/{0}", etapaEncaminhamentoUsuario.EtapaResposta.ProcessoResposta.ID);

                        new BP.ManterNotificacao().PublicarNotificacao(urlDemanda, mensagemNotificacao, etapaEncaminhamentoUsuario.UsuarioEncaminhamento.ID, notificacaoEnvio);

                        //ENVIA NOTIFICACAO PARA O SUPERIOR DA HIERARQUIA
                        var superiorHierarquia = new ManterHierarquiaNucleoUsuario().ObterTodosIQueryable()
                            .Where(x => x.HierarquiaNucleo.ID == etapaEncaminhamentoUsuario.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.HierarquiaNucleo.ID && x.IsGestor).FirstOrDefault();
                        new BP.ManterNotificacao().PublicarNotificacao(urlDemanda, mensagemNotificacao, superiorHierarquia.Usuario.ID, notificacaoEnvio);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        public List<DTOProcessoRespostaAcompanhar> AcompanharMeusProcessos(string cpf, int? numero,
            enumStatusDemanda? status, int? demandanteId, int? processoId, int? etapaId)
        {
            var usuario = new ManterUsuario().ObterPorCPF(cpf);

            var retorno = new List<DTOProcessoRespostaAcompanhar>();

            // Etapas a Analisar
            if (status == null || status == enumStatusDemanda.Pendente)
            {
                retorno.AddRange(ConsultarEtapasAAnalisar(usuario, numero, demandanteId, processoId, etapaId).ToList());
            }

            // Etapas em andamento.
            if (status == null || status == enumStatusDemanda.EmAndamento)
            {
                retorno.AddRange(ConsultarEtapasEmAndamento(usuario, numero, usuario.ID, processoId, etapaId));
            }

            return retorno.OrderByDescending(x => x.DataAberturaDemanda).ToList();
        }

        public IList<DTOProcessoRespostaAcompanhar> ConsultarEtapasHistorico(string cpf, int? numero,
            enumStatusDemanda? status, int? demandanteId,
            int? processoId, int? etapaId)
        {
            var retorno = new List<DTOProcessoRespostaAcompanhar>();

            var usuario = new ManterUsuario().ObterPorCPF(cpf);

            if (status == null || status == enumStatusDemanda.Analisada)
            {
                var listaAnalisado = ConsultarEtapasAnalisada(usuario, numero, demandanteId, processoId, etapaId);
                retorno.AddRange(listaAnalisado);
            }

            if (status == null || status == enumStatusDemanda.Finalizada)
            {
                var listaConcluido = ConsultarEtapasConcluidas(cpf, numero, demandanteId, processoId, etapaId)
                    .Select(x => new DTOProcessoRespostaAcompanhar
                    {
                        IdProcessoResposta = x.ID_ProcessoResposta,
                        IdProcesso = x.ID_Processo,
                        NomeProcesso = x.Processo,
                        DataAberturaDemanda = x.DataSolicitacao,
                        IdEtapa = x.ID_Etapa,
                        NomeEtapa = x.Etapa,
                        IdDemandante = x.ID_Usuario,
                        Demandante = x.Usuario,
                        Unidade = x.Unidade,
                        DataUltimoEnvio = x.DataUltimaAtualizacaoEtapaResposta,
                        StatusEncaminhamento = (int?)x.ObterStatusEncaminhamento(x.PrazoEncaminhamento),
                        PrazoEncaminhamento = x.PrazoEncaminhamento,
                        StatusProcesso = (int)enumStatusDemanda.Finalizada,
                        DataInicioCapacitacao = new ManterCampoResposta().ObterRespostaDataCapacitacao(x.ID_ProcessoResposta)?.Resposta
                    });
                retorno.AddRange(listaConcluido);
            }

            return retorno.OrderByDescending(x => x.DataAberturaDemanda).ToList();
        }

        public IList<DTOProcessoRespostaAcompanhar> ConsultarEtapasAAnalisar(Usuario usuario, int? numero, int? demandanteId,
            int? processoId, int? etapaId)
        {
            try
            {
                if (usuario == null)
                    return null;

                var etapasAnalisar = new BMProcesso()
                    .ConsultarEtapasAAnalisar(usuario, numero, demandanteId, processoId, etapaId).AsEnumerable();

                return ConverEtapaRespostaParaDto(etapasAnalisar).Select(x =>
                    new DTOProcessoRespostaAcompanhar
                    {
                        IdProcessoResposta = x.ID_ProcessoResposta,
                        IdProcesso = x.ID_Processo,
                        NomeProcesso = x.Processo,
                        DataAberturaDemanda = x.DataSolicitacao,
                        IdEtapa = x.ID_Etapa,
                        NomeEtapa = x.Etapa,
                        IdDemandante = x.ID_Usuario,
                        Demandante = x.Usuario,
                        Unidade = x.Unidade,
                        DataUltimoEnvio = x.DataUltimaAtualizacaoEtapaResposta,
                        StatusEncaminhamento = (int?)x.ObterStatusEncaminhamento(x.PrazoEncaminhamento),
                        PrazoEncaminhamento = x.PrazoEncaminhamento,
                        StatusProcesso = (int)enumStatusDemanda.Pendente,
                        DataInicioCapacitacao = new ManterCampoResposta().ObterRespostaDataCapacitacao(x.ID_ProcessoResposta)?.Resposta
                    }).ToList();
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        public IList<DTOEtapaAAnalisar> ConsultarEtapasConcluidas(string cpf, int? numero = null,
            int? demandanteId = null, int? processoId = null, int? etapaId = null)
        {
            try
            {
                // Usuário que está visualizando a tela.
                var usuario = new BMUsuario().ObterPorCPF(cpf);

                if (usuario == null)
                    return null;

                var etapasConcluidas =
                    bmProcesso.ConsultarEtapasConcluidas(usuario, numero, demandanteId, processoId, etapaId);

                if (!etapasConcluidas.Any() || etapasConcluidas == null)
                    return new List<DTOEtapaAAnalisar>();

                return ConverEtapaRespostaParaDto(etapasConcluidas);

            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        private static IList<DTOEtapaAAnalisar> ConverEtapaRespostaParaDto(IEnumerable<EtapaResposta> etapasConcluidas)
        {
            // Converter as etapas em DTO.
            return etapasConcluidas.Select(e => new DTOEtapaAAnalisar
            {
                Usuario = e.ProcessoResposta.Usuario.Nome,
                Email = e.ProcessoResposta.Usuario.Email,
                Processo = e.ProcessoResposta.Processo.Nome,
                Etapa = e.Etapa.Nome,
                Unidade = e.ProcessoResposta.Usuario.Unidade,
                DataSolicitacao = e.ProcessoResposta.DataSolicitacao,
                DataAlteracaoProcessoResposta = e.ProcessoResposta.DataAlteracao,
                DataUltimaAtualizacaoEtapaResposta = (e.DataPreenchimento.HasValue) ? e.DataPreenchimento : e.DataAlteracao,
                ID_Processo = e.ProcessoResposta.Processo.ID,
                ID_ProcessoResposta = e.ProcessoResposta.ID,
                ID_Etapa = e.Etapa.ID,
                ID_EtapaResposta = e.ID,
                ID_Usuario = e.ProcessoResposta.Usuario.ID,
                PrazoEncaminhamento = e.PrazoEncaminhamento != null ? e.PrazoEncaminhamento : null,
            }).OrderByDescending(y => y.DataUltimaAtualizacaoEtapaResposta).ToList();
        }

        public List<EtapaResposta> ConsultarEtapasPorUsuarioCargo(Usuario usuario, List<Cargo> cargos, bool somenteEtapasComUmAnalista = false)
        {
            return new BMProcesso().ConsultarEtapasAAnalisar(usuario, null, null, null, null, cargos, somenteEtapasComUmAnalista);
        }

        public Processo ObterPorEtapaId(int id)
        {
            return new BMEtapa().ObterPorId(id).Processo;
        }

        public DTOAnalisarEtapasProcesso AnalisarEtapasProcesso(int idProcessoResposta, string cpf, int etapasAtivas = 0, int analisePortal = 0)
        {
            var usuarioLogado = new ManterUsuario().ObterPorCPF(cpf);

            var retorno = new DTOAnalisarEtapasProcesso();

            var bmProcessoResposta = new ManterProcessoResposta();
            var processoResposta = bmProcessoResposta.ObterPorID(idProcessoResposta);

            var listaEtapaResposta = processoResposta.ListaEtapaResposta.OrderBy(x => x.ID);

            if (etapasAtivas == 1)
            {
                listaEtapaResposta = listaEtapaResposta.Where(x => x.Ativo).OrderBy(x => x.ID);
            }
            var bmEtapaResposta = new ManterEtapaResposta();

            retorno.Processo = PreencherInformacoesProcesso(retorno, processoResposta);
            var listaEtapas = bmProcessoResposta.ObterEtapasPorVersao(processoResposta).ToList();

            if (usuarioLogado.ListaUsuarioCargo.Count() > 1 && processoResposta.Usuario.ID == usuarioLogado.ID &&
                listaEtapaResposta.Count(x => x.Analista.ID == usuarioLogado.ID) > 1)
            {
                var cargoAnalistaEtapaAnterior =
                    listaEtapaResposta.Where(x => x.Analista.ID == usuarioLogado.ID).ToList()
                        [listaEtapaResposta.Count() - 2]?.CargoAnalista;

                if (cargoAnalistaEtapaAnterior == null)
                {
                    throw new AcademicoException("Não foi possível obter o cargo do analista da etapa anterior");
                }

                // Só preenche a lista de cargos caso o demandante não esteja mais no último cargo que esteve
                // quando enviou a última etapa resposta.
                if (usuarioLogado.ListaUsuarioCargo.Any(x => x.Cargo.ID == cargoAnalistaEtapaAnterior.ID) == false)
                {
                    retorno.Processo.Cargos = usuarioLogado.ListaUsuarioCargo.Select(x => new DTOCargosDemandante
                    {
                        ID = x.ID,
                        Nome =
                            x.Cargo.TipoCargo.GetDescription() + " de " + x.Cargo.ObterNome() + " - " + x.Cargo.Uf.Sigla
                    }).ToList();
                }
            }

            foreach (var etapaResposta in listaEtapaResposta)
            {
                int statusResposta = etapaResposta != null ? etapaResposta.Status : 0;

                if (listaEtapas.OrderBy(d => d.Ordem).Last() == etapaResposta.Etapa && processoResposta.Status == enumStatusProcessoResposta.Cancelado && (etapaResposta == null || etapaResposta.Status == (int)enumStatusEtapaResposta.Aguardando))
                    statusResposta = (int)enumStatusEtapaResposta.Cancelado;

                if (analisePortal == 1)
                {
                    //ADICIONA LISTA DE ETAPAS ENCAMINHADAS COM STATUS AGUARDANDO PARA O USUARIO
                    etapaResposta.ListaEtapaEncaminhamentoUsuario = new ManterEtapaEncaminhamentoUsuario().ObterTodosIQueryable()
                        .Where(x => x.EtapaResposta.ID == etapaResposta.ID)
                        .Where(x => x.StatusEncaminhamento == (int)enumStatusEncaminhamentoEtapa.Aguardando)
                        .Where(x => usuarioLogado != null && x.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.Usuario.ID == usuarioLogado.ID)
                        .OrderByDescending(x => x.ID).ToList();
                }
                else
                {
                    etapaResposta.ListaEtapaEncaminhamentoUsuario = new ManterEtapaEncaminhamentoUsuario().ObterTodosIQueryable()
                        .Where(x => x.EtapaResposta.ID == etapaResposta.ID)
                        .OrderByDescending(x => x.ID).ToList();
                }

                var dtoEtapa = new DTOEtapa
                {
                    ID = etapaResposta.Etapa.ID,
                    Nome = etapaResposta.Etapa.Nome,
                    RequerAprovacao = etapaResposta.Etapa.RequerAprovacao,
                    PodeSerReprovada = etapaResposta.Etapa.PodeSerReprovada,
                    IDEtapaRetorno = etapaResposta.Etapa.EtapaRetorno != null ? etapaResposta.Etapa.EtapaRetorno.ID : 0,
                    Status = statusResposta,
                    ID_RespostaEtapa = etapaResposta != null ? etapaResposta.ID : 0,
                    LinkAnexo =
                        etapaResposta != null && etapaResposta.Etapa.FileServer != null
                            ? ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS).Registro +
                              @"ExibirFileServer.ashx?Identificador=" +
                              etapaResposta.Etapa.FileServer.NomeDoArquivoNoServidor
                            : "",
                    NomeAnexo = etapaResposta != null && etapaResposta.Etapa.FileServer != null ? etapaResposta.Etapa.FileServer.NomeDoArquivoOriginal : null,
                    Situacao = new DTOSituacaoProcesso().ObterSituacao(statusResposta, etapaResposta),

                    NomeFinalizacaoBotao = etapaResposta.Etapa.ObterNomeFinalizacaoBotao(),
                    NomeReprovacaoBotao = etapaResposta.Etapa.ObterNomeReprovacaoBotao(),
                    NomeBotaoAjuste = etapaResposta.Etapa.NomeBotaoAjuste,
                    Assessor = etapaResposta != null && etapaResposta.Assessor != null ? new DTOUsuario
                    {
                        ID = etapaResposta.Assessor.ID,
                        Nome = etapaResposta.Assessor.Nome
                    } : null,
                    AnalisavelPorNucleoUC = (etapaResposta.Etapa.Permissoes.Any(x => x.NaoAnalisavelPorNucleoUC()) ? false : true),
                    AnalistasPorNucleo = etapaResposta.Etapa.Permissoes.Any(x => x.NaoAnalisavelPorNucleoUC()) ? null :
                        new ManterEtapaPermissaoNucleoService().ObterEtapaPermissaoNucleo(etapaResposta.Etapa, etapaResposta.ProcessoResposta.Usuario.UF).ToList(),

                };

                var encaminhamentoEtapaUsuario = etapaResposta.ListaEtapaEncaminhamentoUsuario.FirstOrDefault();
                if (encaminhamentoEtapaUsuario != null)
                {

                    dtoEtapa.EtapaEncaminhamentoUsuario = new DTOEtapaEncaminhamentoUsuario
                    {
                        ID_EtapaEncamihamentoUsuario = encaminhamentoEtapaUsuario.ID,
                        ID_EtapaResposta = encaminhamentoEtapaUsuario.EtapaResposta.ID,
                        Justificativa = encaminhamentoEtapaUsuario.Justificativa,
                        StatusEncaminhamento = (enumStatusEncaminhamentoEtapa)encaminhamentoEtapaUsuario.StatusEncaminhamento,
                        Status = ((enumStatusEncaminhamentoEtapa)encaminhamentoEtapaUsuario.StatusEncaminhamento).GetDescription(),
                        UsuarioEncaminhamento = new DTOUsuario
                        {
                            ID = encaminhamentoEtapaUsuario.UsuarioEncaminhamento.ID,
                            Nome = encaminhamentoEtapaUsuario.UsuarioEncaminhamento.Nome
                        }
                    };
                }

                // Parte que pega quem será o responsável por aprovar a etapa atual
                if (statusResposta == (int)enumStatusEtapaResposta.Cancelado && processoResposta.UsuarioCancelamento != null)
                {
                    dtoEtapa.Analistas = new List<DTOUsuario> { new DTOUsuario { ID = processoResposta.UsuarioCancelamento.ID, Nome = processoResposta.UsuarioCancelamento.Nome } };
                }

                if (etapaResposta != null)
                {
                    // Buscar todos os diretores, para a UF do Demandante.
                    var diretores =
                        new BMUsuarioCargo().ObterPorTipoCargo(EnumTipoCargo.Diretoria)
                            .Where(x => x.Cargo.Uf.ID == processoResposta.Usuario.UF.ID)
                            .ToList();

                    var analistas = etapaResposta.ObterAnalistas(diretores, usuarioLogado);

                    dtoEtapa.Responsaveis = analistas.Select(u => u.Nome.ToUpper()).ToList();
                    var etapaDisabled = analistas.All(x => x.CPF != cpf);

                    dtoEtapa.DataPreenchimento = etapaResposta.DataPreenchimento.HasValue ? etapaResposta.DataPreenchimento.Value.ToString("dd/MM/yyyy HH:mm") : "";
                    dtoEtapa.ListaCampos.AddRange(ConsultarCamposEtapaResposta(etapaResposta.ID, processoResposta.Usuario, false, etapaDisabled));
                    dtoEtapa.Analista = ObterAnalistaDaEtapa(etapaResposta.Etapa, etapaResposta, listaEtapas, processoResposta);
                    //dtoEtapa.Assessor = ObterAssessorDaEtapa(etapaResposta);

                    // Adiciona os analistas do núcleo
                    if (etapaResposta.PermissoesNucleoEtapaResposta.Any())
                    {
                        dtoEtapa.Analistas.AddRange(ObterAnalistasDoNucleo(etapaResposta));
                    }

                    //ADICIONAR ETAPARESPOSTAS INATIVAS PARA O USUARIO CONFERIR NA TELA
                    dtoEtapa.ListaEtapasRespostasInativas = new List<DTOEtapa>();

                    var listaEtapaRespostasInativas = bmEtapaResposta.ObterEtapaRespostasInativas(etapaResposta.ProcessoResposta.ID, etapaResposta.Etapa.Ordem);
                    foreach (var etapaRespostaInativa in listaEtapaRespostasInativas)
                    {
                        var etapaInativa = new DTOEtapa()
                        {
                            ID = etapaResposta.Etapa.ID,
                            Nome = etapaResposta.Etapa.Nome,
                            RequerAprovacao = etapaResposta.Etapa.RequerAprovacao,
                            PodeSerReprovada = etapaResposta.Etapa.PodeSerReprovada,
                            IDEtapaRetorno = etapaResposta.Etapa.EtapaRetorno != null ? etapaResposta.Etapa.EtapaRetorno.ID : 0,
                            Status = etapaRespostaInativa != null ? etapaRespostaInativa.Status : 0,
                            ID_RespostaEtapa = etapaRespostaInativa != null ? etapaRespostaInativa.ID : 0,
                            Situacao = new DTOSituacaoProcesso
                            {
                                ID = etapaRespostaInativa != null ? etapaRespostaInativa.Status : 0,
                                Nome = etapaRespostaInativa != null ? Enum.GetName(typeof(enumStatusEtapaResposta), etapaRespostaInativa.Status) : Enum.GetName(typeof(enumStatusEtapaResposta), 0)
                            },
                            EtapaEncaminhamentoUsuario = etapaRespostaInativa.ListaEtapaEncaminhamentoUsuario
                                .OrderByDescending(x => x.ID)
                                .Select(encaminhamentoEtapa => new DTOEtapaEncaminhamentoUsuario
                                {
                                    ID_EtapaEncamihamentoUsuario = encaminhamentoEtapa.ID,
                                    ID_EtapaResposta = encaminhamentoEtapa.EtapaResposta.ID,
                                    Justificativa = encaminhamentoEtapa.Justificativa,
                                    StatusEncaminhamento = (enumStatusEncaminhamentoEtapa)encaminhamentoEtapa.StatusEncaminhamento,
                                    Status = ((enumStatusEncaminhamentoEtapa)encaminhamentoEtapa.StatusEncaminhamento).GetDescription(),
                                    UsuarioEncaminhamento = new DTOUsuario
                                    {
                                        ID = encaminhamentoEtapa.UsuarioEncaminhamento.ID,
                                        Nome = encaminhamentoEtapa.UsuarioEncaminhamento.Nome
                                    }
                                }).FirstOrDefault()
                        };

                        if (etapaRespostaInativa != null)
                        {
                            etapaInativa.DataPreenchimento = etapaResposta.DataPreenchimento.HasValue ? etapaResposta.DataPreenchimento.Value.ToString("dd/MM/yyyy HH:mm") : "";
                        }
                        dtoEtapa.ListaEtapasRespostasInativas.Add(etapaInativa);
                    }
                }

                retorno.ListaEtapas.Add(dtoEtapa);
            }

            AdicionarRespotaAosCamposQuandoExiste(idProcessoResposta, retorno);

            return retorno;
        }

        private static void AdicionarRespotaAosCamposQuandoExiste(int idProcessoResposta, DTOAnalisarEtapasProcesso retorno)
        {
            if (retorno.ListaEtapas.Count == 1)
            {
                var camposResp = new BMEtapaResposta().ObterTodosIQueryable()
                     .Where(x => x.ProcessoResposta.ID == idProcessoResposta)
                     .Where(x => x.DataPreenchimento != null)
                     .Where(x => x.Status == 3)
                     .Select(x => x.ListaCampoResposta)
                     .ToList()
                     .LastOrDefault();

                foreach (var campos in retorno.ListaEtapas.SelectMany(x => x.ListaCampos).ToList())
                {
                    campos.Resposta = camposResp.FirstOrDefault(x => x.Campo.ID == campos.ID)?.Resposta;
                }

            }
        }

        private DTOProcessoInfo PreencherInformacoesProcesso(DTOAnalisarEtapasProcesso retorno, ProcessoResposta processoResposta)
        {
            var bmProcessoResposta = new BMProcessoResposta();

            retorno.Processo.ID = processoResposta.Processo.ID;
            retorno.Processo.Nome = processoResposta.Processo.Nome;
            retorno.Processo.Concluido = processoResposta != null && processoResposta.Concluido;

            var etapaAtual = new BMEtapa().ObterEtapaAtualDeProcesso(processoResposta.ID);
            retorno.Processo.EtapaAtual = etapaAtual.ID;
            retorno.Processo.Situacao = new DTOSituacaoProcesso
            {
                ID = 0,
                Nome = Enum.GetName(typeof(enumStatusEtapaResposta), etapaAtual.ListaEtapaResposta.First(e => e.ProcessoResposta.ID == processoResposta.ID).Status)
            };

            // Obtem próxima etapa para verificar se existem analista do núcleo
            var proximaEtapa = this.ObterProximaEtapaNaoRespondida(processoResposta.Processo.ID, processoResposta.ID);

            // #2029 - Já que não tem como saber se deve ou não ser aplicado o núcleo, caso seja análisavel por qualquer um exceto ele
            // não envia os registro de núcleo para o portal
            if (proximaEtapa != null && !proximaEtapa.Permissoes.Any(x => x.NaoAnalisavelPorNucleoUC()))
            {
                retorno.Processo.ProximaEtapa = proximaEtapa.ID;

                var uf = processoResposta.Usuario.UF;

                // Obtem analistas do núcleo que poderão analisar a etapa
                retorno.Processo.AnalistasPorNucleo = new ManterEtapaPermissaoNucleoService().ObterEtapaPermissaoNucleo(proximaEtapa, uf).ToList();
            }

            return retorno.Processo;
        }


        public DTOAnalisarEtapasProcesso PegaEtapasHistorico(int idProcessoResposta, string cpf, int IdEtapaResposta)
        {

            var bmProcessoResposta = new BMProcessoResposta();

            var processoResposta = bmProcessoResposta.ObterPorId(idProcessoResposta);
            processoResposta.ListaEtapaResposta = processoResposta
                .ListaEtapaResposta
                .Where(x => x.Status != (int)enumStatusEtapaResposta.Aguardando)
                .ToList();

            if (processoResposta.Usuario.UF == null)
            {
                processoResposta.Usuario = new BMUsuario().ObterPorId(processoResposta.Usuario.ID);
            }

            var retorno = new DTOAnalisarEtapasProcesso();

            retorno.Processo.ID = processoResposta.Processo.ID;
            retorno.Processo.Nome = processoResposta.Processo.Nome;
            retorno.Processo.Concluido = processoResposta.Concluido;


            var etapaRespostaAtual = new ManterEtapaResposta().ObterPorID(IdEtapaResposta);
            var etapaAtual = etapaRespostaAtual.Etapa;

            retorno.Processo.EtapaAtual = etapaAtual.ID;

            retorno.Processo.Situacao = new DTOSituacaoProcesso
            {
                ID = 0,
                Nome = Enum.GetName(typeof(enumStatusEtapaResposta), etapaRespostaAtual.Status)
            };

            var etapas = processoResposta.ListaEtapaResposta
                    .Select(x => x.Etapa)
                    .ToList();

            var registro = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS).Registro;

            var dtoEtapas = processoResposta.ListaEtapaResposta
                 .Select(er => new DTOEtapa
                 {
                     ID = er.Etapa.ID,
                     Nome = er.Etapa.Nome,
                     RequerAprovacao = er.Etapa.RequerAprovacao,
                     IDEtapaRetorno = er.Etapa.EtapaRetorno != null ? er.Etapa.EtapaRetorno.ID : 0,
                     Status = er.Status,
                     ID_RespostaEtapa = er.ID,
                     LinkAnexo = er.Etapa.FileServer != null
                                 ? registro + @"/ExibirFileServer.ashx?Identificador=" + er.Etapa.FileServer.NomeDoArquivoNoServidor
                                 : "",
                     Situacao = new DTOSituacaoProcesso().ObterSituacao(er.Status, er),
                     DataPreenchimento = er.DataPreenchimento.HasValue ? er.DataPreenchimento.Value.ToString("dd/MM/yyyy HH:mm") : "",
                     ListaCampos = ConsultarCamposEtapaResposta(er.ID, processoResposta.Usuario),
                     Analista = ObterAnalistaDaEtapa(er.Etapa, er, etapas.ToList(), processoResposta),
                     Analistas = er.PermissoesNucleoEtapaResposta.Any() ? ObterAnalistasDoNucleo(er) : null,
                     Ordem = er.Etapa.Ordem
                 });

            retorno.ListaEtapas.AddRange(dtoEtapas);

            return retorno;
        }

        // Atenção: Caso seja a primeira etapa o método retorna o nome do solicitante
        private DTOUsuario ObterAnalistaDaEtapa(Etapa etapa, EtapaResposta etapaResposta, List<Etapa> listaEtapas, ProcessoResposta processoResposta)
        {
            var analista = new DTOUsuario { ID = 0, Nome = "" };
            if (etapaResposta.Analista != null)
            {
                analista.ID = etapaResposta.Analista.ID;
                analista.Nome = etapaResposta.Analista.Nome;
                analista.Cpf = etapaResposta.Analista.CPF;
                analista.Email = etapaResposta.Analista.Email;
                analista.Unidade = etapaResposta.Analista.Unidade;
                analista.NomeNivel = etapaResposta.Analista.NivelOcupacional.Nome;
                analista.NomeUF = etapaResposta.Analista.UF.Nome;
            }
            // O solicitante que preenche a primeira etapa
            else if (etapa == listaEtapas.OrderBy(d => d.Ordem).First())
            {
                analista.ID = processoResposta.Usuario.ID;
                analista.Nome = processoResposta.Usuario.Nome;
                analista.Cpf = processoResposta.Usuario.CPF;
                analista.Email = processoResposta.Usuario.Email;
                analista.Unidade = processoResposta.Usuario.Unidade;
                if (processoResposta.Usuario.NivelOcupacional == null || processoResposta.Usuario.UF.Nome == null)
                {
                    var usuario = new ManterUsuario().ObterPorID(processoResposta.Usuario.ID);
                    analista.NomeNivel = usuario.NivelOcupacional.Nome;
                    analista.NomeUF = usuario.UF.Nome;
                    analista.UF = new DTOUf
                    {
                        ID = usuario.UF.ID,
                        Nome = usuario.UF.Nome
                    };
                }
            }
            return analista;
        }

        private List<DTOUsuario> ObterAnalistasDoNucleo(EtapaResposta etapaResposta)
        {
            return etapaResposta.PermissoesNucleoEtapaResposta
                .Where(x => x.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.HierarquiaNucleo.Ativo &&
                    x.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.Usuario.UF?.ID == x.EtapaResposta.ProcessoResposta.Processo.Uf?.ID)
                .Select(x => new DTOUsuario
                {
                    ID = x.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.Usuario.ID,
                    Nome = x.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.Usuario.Nome
                })
                .ToList();
        }

        private List<Usuario> ObterUsuariosAnalistasDoNucleo(EtapaResposta etapaResposta)
        {
            return etapaResposta.PermissoesNucleoEtapaResposta.Where(x => x.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.HierarquiaNucleo.Ativo
            && x.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.Usuario.UF.ID == x.EtapaResposta.ProcessoResposta.Processo.Uf.ID).Select(x =>
                new Usuario
                {
                    ID = x.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.Usuario.ID,
                    Nome = x.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.Usuario.Nome
                }).ToList();
        }



        private List<EtapaPermissao> ObterResponsaveisDaEtapa(EtapaResposta etapaResposta)
        {
            return new BMEtapa().ObterPorId(etapaResposta.Etapa.ID).Permissoes.ToList();
        }

        private List<string> ObterNomeResponsaveisDaEtapa(Etapa etapa)
        {
            if (etapa.PrimeiraEtapa == true)
                return null;

            return new BMEtapa().ObterPorId(etapa.ID).Permissoes.Where(p => p.Usuario != null).Select(x => x.Usuario.Nome).ToList();
        }

        public DTODetalhamentoProcesso ObterDetalhamentoProcesso(int idProcessoResposta)
        {
            var retorno = new DTODetalhamentoProcesso();

            var bmProcessoResposta = new BMProcessoResposta();
            var bmEtapa = new BMEtapa();
            var bmEtapaResposta = new ManterEtapaResposta();

            var processoResposta = bmProcessoResposta.ObterPorId(idProcessoResposta);
            var etapaAtual = bmEtapa.ObterEtapaAtualDeProcesso(processoResposta.ID);
            var listaEtapas = bmEtapa.ObterPorProcessoId(processoResposta.Processo.ID).ToList();
            var listaEtapaRespostas = bmEtapaResposta.ObterTodasDisponiveisImpressao(processoResposta.ID).ToList();

            retorno.Processo.ID = processoResposta.Processo.ID;
            retorno.Processo.Nome = processoResposta.Processo.Nome;
            retorno.Processo.Concluido = processoResposta.Concluido;
            retorno.Processo.EtapaAtual = etapaAtual.ID;
            retorno.Processo.Demandante = processoResposta.Usuario.Nome;
            retorno.Processo.DemandanteEmail = processoResposta.Usuario.Email;
            retorno.Processo.DataSolicitacao = processoResposta.DataSolicitacao;
            retorno.Processo.Situacao = new DTOSituacaoProcesso()
            {
                Nome = Enum.GetName(typeof(enumStatusEtapaResposta), etapaAtual != null ?
                    etapaAtual.ListaEtapaResposta.First().Status :
                    listaEtapas.OrderByDescending(d => d.Ordem).First().ListaEtapaResposta.First().Status)
            };

            foreach (var etapaResposta in listaEtapaRespostas)
            {
                var etapa = new DTOEtapaInfo()
                {
                    ID = etapaResposta.Etapa.ID,
                    IDRespostaEtapa = etapaResposta.ID,
                    Nome = etapaResposta.Etapa.Nome,
                    LinkAnexo = etapaResposta.Etapa.FileServer != null ? ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS).Registro + @"/ExibirFileServer.ashx?Identificador=" + etapaResposta.Etapa.FileServer.NomeDoArquivoNoServidor : "",
                    Status = etapaResposta.Status,
                    RequerAprovacao = etapaResposta.Etapa.RequerAprovacao,
                    IDEtapaRetorno = etapaResposta.Etapa.EtapaRetorno != null ? etapaResposta.Etapa.EtapaRetorno.ID : 0,
                    ListaCampos = ConsultarCamposEtapaResposta(etapaResposta.ID, processoResposta.Usuario, true),
                    DataPreenchimento = etapaResposta.DataPreenchimento.HasValue ? etapaResposta.DataPreenchimento.Value.ToString("dd/MM/yyyy HH:mm") : null,
                    NomeAprovador = etapaResposta.Etapa.UsuarioAssinatura != null ? etapaResposta.Etapa.UsuarioAssinatura.Nome : "",
                    Situacao = new DTOSituacaoProcesso
                    {
                        ID = etapaResposta.Status,
                        Nome = etapaResposta.ObterSituacao
                    },
                    // Obter o analista da etapa.
                    Analista = ObterAnalistaDaEtapa(etapaResposta.Etapa, etapaResposta, listaEtapas, processoResposta),
                    // Obter o assessor da etapa.
                    //Assessor = ObterAssessorDaEtapa(etapaResposta)
                };

                retorno.Etapas.Add(etapa);
            }

            return retorno;
        }


        public DTOAnalisarEtapasProcesso ObterPrimeiraEtapa(int idProcesso, string usuarioCPF)
        {
            var usuario = bmUsuario.ObterPorCPF(usuarioCPF);
            var retorno = new DTOAnalisarEtapasProcesso();
            var processo = new BMProcesso().ObterPorId(idProcesso);
            var primeiraEtapa = new BMEtapa().ObterPrimeiraEtapaDeProcesso(idProcesso);

            retorno.Processo.ID = processo.ID;
            retorno.Processo.Nome = processo.Nome;
            retorno.Processo.EtapaAtual = primeiraEtapa.ID;
            retorno.Processo.Situacao = new DTOSituacaoProcesso
            {
                ID = 0,
                Nome = Enum.GetName(typeof(enumStatusEtapaResposta), 0)
            };

            if (!usuario.ListaUsuarioCargo.Any())
                throw new AcademicoException("Usuário não está alocado em nenhum cargo na Hierarquia. Entre em contato com os Administradores.");

            retorno.Processo.Cargos = usuario.ListaUsuarioCargo.Select(x => new DTOCargosDemandante
            {
                ID = x.ID,
                Nome = x.Cargo.TipoCargo.GetDescription() + " de " + x.Cargo.ObterNome() + " - " + x.Cargo.Uf.Sigla
            }).ToList();

            var dtoEtapa = new DTOEtapa
            {
                ID = primeiraEtapa.ID,
                Nome = primeiraEtapa.Nome,
                RequerAprovacao = primeiraEtapa.RequerAprovacao,
                IDEtapaRetorno = primeiraEtapa.EtapaRetorno != null ? primeiraEtapa.EtapaRetorno.ID : 0,
                Status = 0,
                LinkAnexo =
                    primeiraEtapa.FileServer != null
                        ? ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS).Registro +
                          @"/ExibirFileServer.ashx?Identificador=" + primeiraEtapa.FileServer.NomeDoArquivoNoServidor
                        : "",
                NomeAnexo = primeiraEtapa.FileServer != null ? primeiraEtapa.FileServer.NomeDoArquivoOriginal : null,
                Situacao = new DTOSituacaoProcesso
                {
                    ID = 0,
                    Nome = Enum.GetName(typeof(enumStatusEtapaResposta), 0)
                },
                NomeFinalizacaoBotao = primeiraEtapa.ObterNomeFinalizacaoBotao(),
                NomeReprovacaoBotao = primeiraEtapa.ObterNomeReprovacaoBotao(),

                // Apenas para a primeira etapa
                LiberarCancelamento = false
            };

            dtoEtapa.ListaCampos.AddRange(ConsultarCamposPrimeiraEtapa(primeiraEtapa.ID, usuario));
            retorno.ListaEtapas.Add(dtoEtapa);

            return retorno;
        }

        public string PreencherCamposUsuario(Campo campo, Usuario usuario)
        {
            if (campo.TipoCampo == (byte)enumTipoCampo.Field)
            {
                switch (campo.TipoDado)
                {
                    case (byte)enumCampoUsuario.AnoConclusao:
                        return usuario.AnoConclusao;
                    case (byte)enumCampoUsuario.Bairro:
                        return usuario.Bairro;
                    case (byte)enumCampoUsuario.CampoConhecimento:
                        return usuario.CodigoCampoConhecimento.Value.ToString();
                    case (byte)enumCampoUsuario.CEP:
                        return usuario.Cep;
                    case (byte)enumCampoUsuario.Cidade:
                        return usuario.Cidade;
                    case (byte)enumCampoUsuario.Complemento:
                        return usuario.Complemento;
                    case (byte)enumCampoUsuario.CPF:
                        return usuario.CPF;
                    case (byte)enumCampoUsuario.DataAdmissao:
                        return usuario.DataAdmissao.HasValue ? usuario.DataAdmissao.Value.ToString("dd/MM/yyyy") : "Não informado";
                    case (byte)enumCampoUsuario.DataExpedicaoIdentidade:
                        return usuario.DataExpedicaoIdentidade.HasValue ? usuario.DataExpedicaoIdentidade.Value.ToString("dd/MM/yyyy") : "Não informado";
                    case (byte)enumCampoUsuario.DataNascimento:
                        return usuario.DataNascimento.HasValue ? usuario.DataNascimento.Value.ToString("dd/MM/yyyy") : "Não informado";
                    case (byte)enumCampoUsuario.Email:
                        return usuario.Email;
                    case (byte)enumCampoUsuario.Endereco:
                        return usuario.Endereco;
                    case (byte)enumCampoUsuario.Escolaridade:
                        return usuario.Escolaridade;
                    case (byte)enumCampoUsuario.Estado:
                        return usuario.Estado;
                    case (byte)enumCampoUsuario.EstadoCivil:
                        return usuario.EstadoCivil;
                    case (byte)enumCampoUsuario.Instituicao:
                        return usuario.Instituicao;
                    case (byte)enumCampoUsuario.Matricula:
                        return usuario.Matricula;
                    case (byte)enumCampoUsuario.MiniCurriculo:
                        return usuario.MiniCurriculo;
                    case (byte)enumCampoUsuario.Nacionalidade:
                        return usuario.Nacionalidade;
                    case (byte)enumCampoUsuario.Naturalidade:
                        return usuario.Naturalidade;
                    case (byte)enumCampoUsuario.NivelOcupacional:
                        return usuario.NivelOcupacional.Nome;
                    case (byte)enumCampoUsuario.Nome:
                        return usuario.Nome;
                    case (byte)enumCampoUsuario.NomeMae:
                        return usuario.NomeMae;
                    case (byte)enumCampoUsuario.NomePai:
                        return usuario.NomePai;
                    case (byte)enumCampoUsuario.NumIdentidade:
                        return usuario.NumeroIdentidade;
                    case (byte)enumCampoUsuario.OrgaoEmissor:
                        return usuario.OrgaoEmissor;
                    case (byte)enumCampoUsuario.Pais:
                        return usuario.Pais;
                    case (byte)enumCampoUsuario.Ramal:
                        return usuario.RamalExibicao;
                    case (byte)enumCampoUsuario.Sexo:
                        return usuario.Sexo;
                    case (byte)enumCampoUsuario.Situacao:
                        return usuario.Situacao;
                    case (byte)enumCampoUsuario.TelCelular:
                        return usuario.TelCelular;
                    case (byte)enumCampoUsuario.TelResidencial:
                        return usuario.TelResidencial;
                    case (byte)enumCampoUsuario.TipoDocumento:
                        return usuario.TipoDocumento;
                    case (byte)enumCampoUsuario.TipoInstituicao:
                        return usuario.TipoInstituicao;
                    case (byte)enumCampoUsuario.UF:
                        return usuario.UF.Nome;
                    case (byte)enumCampoUsuario.Unidade:
                        return usuario.Unidade;
                    // Lento, mas não consegui pensar em outra forma sem modificar toda a lógica de funcionamento dos campos de usuário
                    case (byte)enumCampoUsuario.HistoricoAcademico:
                        return new ManterUsuario().pegaLinkHistoricoAcademico(usuario.ID);
                }
            }

            return null;
        }


        public List<DTOCampo> ConsultarCamposEtapaResposta(int idEtapaResposta, Usuario usuario, bool exibirSomenteImpressao = false, bool disabled = true)
        {
            EtapaResposta etapaResposta = new ManterEtapaResposta().ObterPorID(idEtapaResposta);
            ManterCampo mCampo = new ManterCampo();

            var lista = new List<DTOCampo>();

            if (etapaResposta.Etapa.ListaCampos.Count() > 0)
            {
                var camposEtapa = etapaResposta.Etapa.ListaCampos.Where(c => (exibirSomenteImpressao ? c.ExibirImpressao : true)).OrderBy(x => x.Ordem);
                foreach (var campo in camposEtapa)
                {
                    DTOCampo dto = new DTOCampo();
                    dto.Titulo = campo.Nome;
                    //dto.Label = campo.Label;
                    dto.Ajuda = !exibirSomenteImpressao ? campo.Ajuda : (campo.ExibirAjudaImpressao ? campo.Ajuda : null);
                    dto.TipoCampo = ObterTipoCampo(campo.TipoCampo);
                    dto.PermiteNulo = campo.PermiteNulo;
                    dto.SomenteLetra = campo.SomenteLetra;
                    dto.SomenteNumero = campo.SomenteNumero;
                    dto.TipoDado = ObterTipoDado(campo.TipoDado);
                    dto.Tamanho = campo.Tamanho;
                    dto.ID = campo.ID;
                    dto.Largura = campo.Largura;
                    dto.ExibirAjudaImpressao = campo.ExibirAjudaImpressao;
                    dto.Disabled = disabled;

                    // Adicionando os meta fields no DTO
                    dto = AddMetaFields(campo, dto);

                    CampoResposta campoResposta = new BMCampoResposta().ObterPorEtapaRespostaId(idEtapaResposta, campo.ID);
                    List<int> listaIdsAlternativa = new List<int>();

                    if (campoResposta == null)
                    {
                        //VERIFICA SE O CAMPO É TIPO "CAMPO DO USUARIO", SE SIM PREENCHE O CAMPO RESPOSTA COM O DADO DO USUARIO
                        if (campo.TipoCampo == (byte)enumTipoCampo.Field)
                        {
                            campoResposta = new CampoResposta()
                            {
                                Campo = campo,
                                EtapaResposta = etapaResposta,
                                Resposta = PreencherCamposUsuario(campo, usuario)
                            };
                        }

                        // Se for do tipo Questionário carrega o questionário com resposta vazia
                        if (campo.TipoCampo == (int)enumTipoCampo.Questionário && campo.Questionario != null)
                        {
                            dto.Questionario = new DTO.Services.Questionario.DTOQuestionario
                            {
                                ID = campo.Questionario.ID,
                                Tipo = campo.Questionario.TipoQuestionario.ID
                            };

                            campoResposta = new CampoResposta()
                            {
                                Campo = campo,
                                Resposta = ""
                            };
                        }
                    }
                    //SENAO PROCURA A RESPOSTA E SE HOUVER POPULA O CAMPORESPOSTA
                    else
                    {
                        listaIdsAlternativa = new BMAlternativaResposta().ObterIdsPorCampoId(campoResposta.ID);
                    }

                    dto.Resposta = campoResposta != null ? campoResposta.Resposta : string.Empty;

                    if (campo.ListaAlternativas.Count() > 0)
                    {
                        foreach (var alternativa in campo.ListaAlternativas.OrderBy(x => x.Ordem))
                        {
                            DTOAlternativa opcao = new DTOAlternativa();
                            opcao.ID = alternativa.ID;
                            opcao.Nome = alternativa.Nome;
                            opcao.Descricao = alternativa.Descricao;
                            opcao.TipoCampo = ObterTipoCampo(alternativa.TipoCampo);
                            opcao.OpcaoRespondida = listaIdsAlternativa.Count(d => d == alternativa.ID) > 0;
                            opcao.ID_CampoVinculado =
                                alternativa.CampoVinculado != null
                                    ? (int?)alternativa.CampoVinculado.ID
                                    : null;

                            dto.ListaAlternativas.Add(opcao);
                        }
                    }

                    // Caso seja um capo do tipo somatório recupera o total da soma
                    if (campo.TipoCampo == (int)enumTipoCampo.Somatório && campo.ListaCamposVinculados.Any())
                    {
                        var culture = CultureInfo.CreateSpecificCulture("pt-BR");
                        dto.Resposta = mCampo.ObterTotalSomatorio(campo, etapaResposta.ProcessoResposta.ID).ToString(culture);
                        dto.CamposVinculados = mCampo.ObterIdsCamposVinculados(campo);
                    }

                    // Caso seja um capo do tipo subtração recupera o total da soma
                    if (campo.TipoCampo == (int)enumTipoCampo.Subtracao && campo.ListaCamposVinculados.Any())
                    {
                        var culture = CultureInfo.CreateSpecificCulture("pt-BR");
                        dto.Resposta = mCampo.ObterTotalSubtracao(campo, etapaResposta.ProcessoResposta.ID).ToString(culture);
                        dto.CamposVinculados = mCampo.ObterIdsCamposVinculados(campo);
                    }

                    // Caso seja um capo do tipo somatório recupera o total da soma
                    if (campo.TipoCampo == (int)enumTipoCampo.Multiplicador && campo.ListaCamposVinculados.Any())
                    {
                        var culture = CultureInfo.CreateSpecificCulture("pt-BR");
                        dto.Resposta = mCampo.ObterTotalMultiplicacao(campo, etapaResposta.ProcessoResposta.ID).ToString(culture);
                        dto.CamposVinculados = mCampo.ObterIdsCamposVinculados(campo);
                    }

                    if (campo.TipoCampo == (int)enumTipoCampo.Percentual)
                    {
                        if (campo.ListaCampoPorcentagem.Any())
                        {
                            foreach (var campoPorcentagem in campo.ListaCampoPorcentagem)
                            {
                                var campoRespostaVinculado = new ManterCampoResposta().ObterRespostas(campoPorcentagem.CampoRelacionado).Where(x => x.EtapaResposta.ProcessoResposta.ID == etapaResposta.ProcessoResposta.ID && x.EtapaResposta.Ativo && x.EtapaResposta.Status != 0).FirstOrDefault();
                                if (campoRespostaVinculado != null)
                                {
                                    dto.ListaCampoPorcentagem.Add(new DTOCampo()
                                    {
                                        ID = campoRespostaVinculado.ID,
                                        Titulo = campoRespostaVinculado.Campo.Nome,
                                        Resposta = campoRespostaVinculado.Resposta,
                                        TipoCampo = null,
                                        TipoDado = null
                                    });
                                }
                            }
                        }
                    }

                    if (campo.TipoCampo == (int)enumTipoCampo.Questionário)
                    {
                        int idQuestParticipacao;
                        if (!string.IsNullOrEmpty(campoResposta.Resposta) && int.TryParse(campoResposta.Resposta, out idQuestParticipacao))
                        {
                            var respostas = campo.Questionario.ListaQuestionarioParticipacao.Where(x => x.ID == idQuestParticipacao);
                            var dtoRespostas = new ManterQuestionarioParticipacao().GetListaDto(respostas.ToList()).ToList();
                            dto.QuestionarioParticipacao = dtoRespostas;
                        }

                    }

                    lista.Add(dto);
                }
            }

            return lista;
        }

        public List<DTOCampo> ConsultarCamposEtapa(int idEtapa, Usuario usuario)
        {
            Etapa etapa = new BMEtapa().ObterPorId(idEtapa);

            var lista = new List<DTOCampo>();

            if (etapa.ListaCampos.Count() > 0)
            {
                foreach (var campo in etapa.ListaCampos.OrderBy(x => x.Ordem))
                {
                    DTOCampo dto = new DTOCampo();
                    dto.Titulo = campo.Nome;
                    //dto.Label = campo.Label;
                    dto.TipoCampo = ObterTipoCampo(campo.TipoCampo);
                    dto.PermiteNulo = campo.PermiteNulo;
                    dto.SomenteLetra = campo.SomenteLetra;
                    dto.SomenteNumero = campo.SomenteNumero;
                    dto.TipoDado = ObterTipoDado(campo.TipoDado);
                    dto.ID = campo.ID;
                    dto.CampoDivisor = campo.CampoDivisor;

                    CampoResposta campoResposta = null;
                    List<int> listaIdsAlternativa = new List<int>();

                    //VERIFICA SE O CAMPO É TIPO "CAMPO DO USUARIO", SE SIM PREENCHE O CAMPO RESPOSTA COM O DADO DO USUARIO
                    if (campo.TipoCampo == (byte)enumTipoCampo.Field)
                    {
                        campoResposta = new CampoResposta()
                        {
                            Campo = campo,
                            Resposta = PreencherCamposUsuario(campo, usuario)
                        };
                    }
                    //SENAO PROCURA A RESPOSTA E SE HOUVER POPULA O CAMPORESPOSTA
                    else
                    {
                        campoResposta = new BMCampoResposta().ObterPorCampoID(campo.ID);
                        if (campoResposta != null)
                        {
                            listaIdsAlternativa = new BMAlternativaResposta().ObterIdsPorCampoId(campoResposta.ID);
                        }
                    }

                    dto.Resposta = campoResposta != null ? campoResposta.Resposta : string.Empty;

                    if (campo.ListaAlternativas.Count() > 0)
                    {
                        foreach (var alternativa in campo.ListaAlternativas.OrderBy(x => x.Ordem))
                        {

                            DTOAlternativa opcao = new DTOAlternativa();
                            opcao.ID = alternativa.ID;
                            opcao.Nome = alternativa.Nome;
                            opcao.Descricao = alternativa.Descricao;
                            opcao.TipoCampo = ObterTipoCampo(alternativa.TipoCampo);
                            opcao.OpcaoRespondida = listaIdsAlternativa.Count(d => d == alternativa.ID) > 0;
                            opcao.ID_CampoVinculado =
                                alternativa.CampoVinculado != null
                                    ? (int?)alternativa.CampoVinculado.ID
                                    : null;

                            dto.ListaAlternativas.Add(opcao);
                        }
                    }

                    lista.Add(dto);
                }
            }

            return lista;
        }

        private DTOCampo AddMetaFields(Campo campo, DTOCampo dto)
        {
            // Adicionando meta fields (criados como mecanismos adicionais de validação)
            foreach (CampoMetaValue item in campo.ListaMetaValues)
            {
                if (item.MetaValue != null && item.CampoMeta.MetaKey != null)
                {
                    dto.ListaMetaValues.Add(new DTOMetaValue() { MetaKey = item.CampoMeta.MetaKey, MetaValue = item.MetaValue });
                }
            }

            return dto;
        }


        public List<DTOCampo> ConsultarCamposPrimeiraEtapa(int idEtapa, Usuario usuario)
        {
            Etapa etapa = new BMEtapa().ObterPorId(idEtapa);
            ManterCampo manterCampo = new ManterCampo();

            var lista = new List<DTOCampo>();

            if (etapa.ListaCampos.Count() > 0)
            {
                foreach (var campo in etapa.ListaCampos.OrderBy(x => x.Ordem))
                {
                    DTOCampo dto = new DTOCampo();
                    dto.Titulo = campo.Nome;
                    //dto.Label = campo.Label;
                    dto.TipoCampo = ObterTipoCampo(campo.TipoCampo);
                    dto.PermiteNulo = campo.PermiteNulo;
                    dto.SomenteLetra = campo.SomenteLetra;
                    dto.SomenteNumero = campo.SomenteNumero;
                    dto.TipoDado = ObterTipoDado(campo.TipoDado);
                    dto.Ajuda = campo.Ajuda;
                    dto.Tamanho = campo.Tamanho;
                    dto.ID = campo.ID;
                    dto.Largura = campo.Largura;

                    // Adiciona meta fields relacionados ao campo
                    dto = AddMetaFields(campo, dto);

                    CampoResposta campoResposta = null;
                    List<int> listaIdsAlternativa = new List<int>();

                    //VERIFICA SE O CAMPO É TIPO "CAMPO DO USUARIO", SE SIM PREENCHE O CAMPO RESPOSTA COM O DADO DO USUARIO
                    if (campo.TipoCampo == (byte)enumTipoCampo.Field)
                    {
                        campoResposta = new CampoResposta()
                        {
                            Campo = campo,
                            Resposta = PreencherCamposUsuario(campo, usuario)
                        };
                    }

                    if (campo.TipoCampo == (int)enumTipoCampo.Questionário && campo.Questionario != null)
                    {
                        dto.Questionario = new DTO.Services.Questionario.DTOQuestionario
                        {
                            ID = campo.Questionario.ID,
                            Tipo = campo.Questionario.TipoQuestionario.ID
                        };

                        campoResposta = new CampoResposta()
                        {
                            Campo = campo,
                            Resposta = campo.Resposta != null ? campo.Resposta.Resposta : ""
                        };
                    }

                    ////SENAO PROCURA A RESPOSTA E SE HOUVER POPULA O CAMPORESPOSTA, caso o usuário já tenha feito essa etapa
                    //else
                    //{
                    //    campoResposta = new BMCampoResposta().ObterPorCampoID(campo.ID);
                    //    if (campoResposta != null)
                    //    {
                    //        listaIdsAlternativa = new BMAlternativaResposta().ObterIdsPorCampoId(campoResposta.ID);
                    //    }
                    //}

                    dto.Resposta = campoResposta != null ? campoResposta.Resposta : string.Empty;

                    if (campo.ListaAlternativas.Count() > 0)
                    {
                        foreach (var alternativa in campo.ListaAlternativas.OrderBy(x => x.Ordem))
                        {
                            DTOAlternativa opcao = new DTOAlternativa();
                            opcao.ID = alternativa.ID;
                            opcao.Nome = alternativa.Nome;
                            opcao.Descricao = alternativa.Descricao;
                            opcao.TipoCampo = ObterTipoCampo(alternativa.TipoCampo);
                            opcao.ID_CampoVinculado =
                                alternativa.CampoVinculado != null
                                    ? (int?)alternativa.CampoVinculado.ID
                                    : null; ;

                            dto.ListaAlternativas.Add(opcao);
                        }
                    }

                    if (campo.TipoCampo == (int)enumTipoCampo.Somatório && campo.ListaCamposVinculados.Any())
                    {
                        var culture = CultureInfo.CreateSpecificCulture("pt-BR");
                        dto.CamposVinculados = manterCampo.ObterIdsCamposVinculados(campo);
                    }

                    if ((campo.TipoCampo == (int)enumTipoCampo.Multiplicador || campo.TipoCampo == (int)enumTipoCampo.Divisor) && campo.ListaCamposVinculados.Any())
                    {
                        campo.ListaCamposVinculados.ToList().ForEach(x => dto.CamposVinculadosDivisao.Add(
                            new DTOCampo()
                            {
                                ID = x.ID,
                                CampoDivisor = x.CampoDivisor
                            }));
                    }

                    lista.Add(dto);
                }
            }

            return lista;
        }

        private string ObterTipoDado(byte p)
        {
            return ((enumTipoDado)p).ToString();
        }

        private Etapa ObterProximaEtapaNaoRespondida(int idProcesso, int IdProcessoResposta = 0)
        {
            return new BMProcesso().ObterProximaEtapaNaoRespondida(idProcesso, IdProcessoResposta);
        }

        public RetornoWebService EfetuarInscricao(int idEtapa, int? cargoId, List<DTOCampo> respostas, string inscritoCPF, bool rascunho = false)
        {
            var bmProcessoResposta = new BMProcessoResposta();
            var bmEtapaResposta = new ManterEtapaResposta();
            var manterEtapaVersao = new ManterEtapaVersao();

            var etapa = new ManterEtapa().ObterPorID(idEtapa);
            var usuario = new BMUsuario().ObterPorCPF(inscritoCPF);

            if (cargoId > 0 && cargoId.HasValue && usuario.ListaUsuarioCargo.All(x => x.ID != cargoId))
                throw new AcademicoException("Usuário não está no cargo selecionado para demandar o processo");

            //INCLUIR PROCESSO RESPOSTA NO BANCO
            var processoResposta = new ProcessoResposta
            {
                Usuario = usuario,
                Processo = etapa.Processo,
                DataSolicitacao = DataUtil.AjustarTimeZoneBR(DateTime.Now),
                Status = enumStatusProcessoResposta.Ativo,
                VersaoEtapa = manterEtapaVersao.ObterVersaoAtualEtapa(etapa)

            };

            bmProcessoResposta.Salvar(processoResposta);

            //CRIAR ETAPA RESPOSTA PARA INSCRIÇÃO
            var etapaResposta = new EtapaResposta
            {
                Status = (int)enumStatusEtapaResposta.Concluido,
                ProcessoResposta = processoResposta,
                CargoAnalista =
                    cargoId.HasValue
                        ? new ManterUsuarioCargo().ObterPorId(cargoId.Value)?.Cargo
                        : usuario.ObterCargo()?.Cargo,
                Etapa = etapa,
                Analista = null,
                Ativo = true,
                DataPreenchimento = DateTime.Now,
                Auditoria = new Auditoria(),
                PrazoEncaminhamento = etapa.ObterPrazoParaEncaminhamentoDaDemanda(DateTime.Now)
            };

            if (rascunho == true)
            {
                etapaResposta.Status = (int)enumStatusEtapaResposta.Aguardando;
                etapaResposta.DataPreenchimento = null;
            }

            bmEtapaResposta.Salvar(etapaResposta);

            var listaAlternativaResposta = new List<AlternativaResposta>();

            var bmCampoResposta = new BMCampoResposta();

            foreach (var campo in respostas)
            {
                try
                {
                    //VERIFICA SE A RESPOSTA NÃO É DO TIPO CAMPO DO USUARIO, POIS NÃO PRECISA SER ADICIONADA AO BANCO
                    if (campo.TipoCampo.ID != (int)enumTipoCampo.FileUpload && campo.TipoCampo.ID != (int)enumTipoCampo.MultipleFileUpload)
                    {
                        var campoResposta = new CampoResposta
                        {
                            Campo = new Campo
                            {
                                ID = campo.ID
                            },
                            EtapaResposta = etapaResposta,
                            Resposta = campo.Resposta
                        };

                        bmCampoResposta.Salvar(campoResposta);

                        //VERIFICA SE A QUESTÃO TEM ALTERNATIVAS
                        if (campo.ListaAlternativas != null && campo.ListaAlternativas.Count > 0 && campo.ListaAlternativas.Any(f => f.OpcaoRespondida))
                        {
                            campoResposta.Resposta = null;

                            //ADICIONA CADA ALTERNATIVA RESPOSTA NA LISTA
                            listaAlternativaResposta.AddRange(
                                campo.ListaAlternativas.Where(d => d.OpcaoRespondida)
                                    .Select(alternativa => new AlternativaResposta
                                    {
                                        Alternativa = new Alternativa
                                        {
                                            ID = alternativa.ID
                                        },
                                        CampoResposta = campoResposta
                                    }));
                        }
                    }
                    else if (campo.TipoCampo.ID == (int)enumTipoCampo.FileUpload)
                    {
                        salvaCampoRespostaArquivo(inscritoCPF, etapaResposta, campo);
                    }
                    else if (campo.TipoCampo.ID == (int)enumTipoCampo.MultipleFileUpload)
                    {
                        salvaCampoRespostaMultiplosArquivos(inscritoCPF, etapaResposta, campo);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            //ADICIONAR ALTERNATIVAS CASO EXISTIREM
            new BMAlternativaResposta().Salvar(listaAlternativaResposta);

            Etapa proximaEtapa = null;

            if (rascunho == false)
            {
                //CRIA A PRÓXIMA ETAPA RESPOSTA COM STATUS DE AGUARDANDO
                proximaEtapa = ObterProximaEtapaNaoRespondida(etapa.Processo.ID, processoResposta.ID);
            }

            //ADICIONAR PROXIMA ETAPA RESPOSTA COM STATUS AGUARDANDO SE HOUVE PROXIMA ETAPA
            if (proximaEtapa != null)
            {
                var proximaEtapaResposta = new EtapaResposta()
                {
                    Status = (int)enumStatusEtapaResposta.Aguardando,
                    ProcessoResposta = processoResposta,
                    Etapa = proximaEtapa,
                    Ativo = true,
                    Auditoria = new Auditoria(),
                    PrazoEncaminhamento = proximaEtapa.ObterPrazoParaEncaminhamentoDaDemanda(DateTime.Now)
                };
                bmEtapaResposta.Salvar(proximaEtapaResposta);

                // Emite Notificações: pessoas a serem analistas
                NotificarEtapaAAnalisar(proximaEtapaResposta);
            }
            
            VerificarProcessoConcluido(processoResposta);

            if (rascunho == false)
            {
                // Emite Notificações: pessoas a serem notificadas
                NotificarConclusao(etapaResposta, proximaEtapa != null, false);
            }

            return new RetornoWebService();
        }

        public void salvaCampoRespostaArquivo(string inscritoCPF, EtapaResposta etapaResposta, DTOCampo campo)
        {
            string resposta = "";
            if (campo.Arquivo != null)
            {
                FileServer fs = new FileServer();

                fs.NomeDoArquivoNoServidor = campo.Arquivo.NomeDoArquivoNoServidor;
                fs.Auditoria = new Auditoria(inscritoCPF);
                fs.TipoArquivo = campo.Arquivo.TipoArquivo;
                fs.NomeDoArquivoOriginal = campo.Arquivo.NomeDoArquivoOriginal;
                using (BMFileServer fsBM = new BMFileServer())
                {
                    fsBM.Salvar(fs);
                    string base_url = new BMConfiguracaoSistema().ObterPorID((int)enumConfiguracaoSistema.EnderecoSGUS).Registro;
                    resposta = base_url + "/ExibirFileServer.ashx?Identificador=" + campo.Arquivo.NomeDoArquivoNoServidor;
                }
            }

            CampoResposta campoResposta = new CampoResposta()
            {
                Campo = new Campo()
                {
                    ID = campo.ID
                },
                EtapaResposta = etapaResposta,
                Resposta = resposta
            };

            new BMCampoResposta().Salvar(campoResposta);
        }

        public void salvaCampoRespostaMultiplosArquivos(string inscritoCPF, EtapaResposta etapaResposta, DTOCampo campo)
        {
            var respostas = new List<string>();
            if (campo.Arquivos.Any())
            {
                foreach (var arquivo in campo.Arquivos)
                {
                    FileServer fs = new FileServer();

                    fs.NomeDoArquivoNoServidor = arquivo.NomeDoArquivoNoServidor;
                    fs.Auditoria = new Auditoria(inscritoCPF);
                    fs.TipoArquivo = arquivo.TipoArquivo;
                    fs.NomeDoArquivoOriginal = arquivo.NomeDoArquivoOriginal;
                    using (BMFileServer fsBM = new BMFileServer())
                    {
                        fsBM.Salvar(fs);
                        string base_url = new BMConfiguracaoSistema().ObterPorID((int)enumConfiguracaoSistema.EnderecoSGUS).Registro;
                        respostas.Add(base_url + "/ExibirFileServer.ashx?Identificador=" + arquivo.NomeDoArquivoNoServidor);
                    }
                }
            }

            CampoResposta campoResposta = new CampoResposta()
            {
                Campo = new Campo()
                {
                    ID = campo.ID
                },
                EtapaResposta = etapaResposta,
                Resposta = string.Join("###", respostas)
            };

            new BMCampoResposta().Salvar(campoResposta);
        }


        protected void VerificarProcessoConcluido(ProcessoResposta processoResposta)
        {
            ManterEtapaResposta bmEtapaResposta = new ManterEtapaResposta();

            //VERIFICA SE EXISTEM ETAPAS PENDENTES
            if (!bmEtapaResposta.ObterPorProcessoRespostaId(processoResposta.ID).Any(d => d.Status == 0))
            {
                processoResposta.Concluido = true;
            }
            else
            {
                processoResposta.Concluido = false;
            }

            new BMProcessoResposta().Salvar(processoResposta);
        }

        public RetornoWebService ResponderFormularioEtapa(int idEtapaResposta, List<DTOCampo> respostas, DTOSituacaoProcesso situacao, string cpf, int idAnalista, List<DTOEtapaPermissaoNucleo> permissoesNucleo, int? cargoId = null, bool rascunho = false)
        {
            var bmEtapaResposta = new ManterEtapaResposta();

            if (respostas.Any())
            {
                //REMOVE CAMPOS ATUAIS SE HOUVER PARA EVITAR DUPLICACAO DE RESPOSTA
                var camposResposta = new ManterCampoResposta().ObterRespostasEtapaResposta(idEtapaResposta);
                if (camposResposta.Any())
                {
                    foreach (var campoResposta in camposResposta)
                    {
                        new BMCampoResposta().Excluir(campoResposta);
                    }
                }
            }

            var etapaResposta = new ManterEtapaResposta().ObterPorID(idEtapaResposta);

            //VERIFICA SE A ETAPA AINDA NÃO FOI RESPONDIDA
            if (etapaResposta.Status == (int)enumStatusEtapaResposta.Aguardando)
            {
                etapaResposta.Status = situacao.ID;

                var analista = new BMUsuario().ObterPorId(idAnalista);
                //TODO: Verificar a necessidade de validar as permissões de analise da demanda.
                // Verifica caso a etapa tenha sido respondida por um assessor em vez do diretor correspondente.
                if (etapaResposta.Etapa.Permissoes.Any(p => p.DiretorCorrespondente == true) && etapaResposta.Etapa.PodeSerAprovadoChefeGabinete)
                {
                    var cargoAnalista = analista.ObterCargo();
                    if (cargoAnalista == null)
                    {
                        throw new AcademicoException("Cargo do Analista não encontrado. Favor entrar em contato com o suporte.");
                    }

                    // Diretor respondeu.
                    if (cargoAnalista.Cargo.TipoCargo == EnumTipoCargo.Diretoria || cargoAnalista.Cargo.TipoCargo == EnumTipoCargo.Gabinete)
                    {
                        etapaResposta.Analista = analista;
                    }
                    else
                    {
                        var chefeGabinete = cargoAnalista.Cargo.ObterChefesGabinete(cargoAnalista.Cargo.Uf).FirstOrDefault();
                        var diretor = cargoAnalista.Cargo.ObterDiretores(cargoAnalista.Cargo.Uf).FirstOrDefault();
                        if (diretor != null || chefeGabinete != null)
                        {
                            var usuarioDiretor = diretor.Usuario;
                            var usuarioChefeGabinete = chefeGabinete.Usuario;

                            if (usuarioDiretor == null && usuarioChefeGabinete == null)
                            {
                                throw new AcademicoException("Diretor ou Chefe de Gabinete não encontrado. Favor entrar em contato com o suporte.");
                            }

                            etapaResposta.Analista = usuarioDiretor != null ? usuarioDiretor : usuarioChefeGabinete;
                            etapaResposta.Assessor = analista;
                        }
                        else
                        {
                            throw new AcademicoException("Analista ou hierarquia inválidos.");
                        }
                    }
                }
                else
                {
                    etapaResposta.Analista = analista;
                }

                // Atualizar o cargo do usuário na etapa respondida.
                AtualizarCargoDaEtapaResposta(cargoId, analista, etapaResposta);

                //CRIA A PRÓXIMA ETAPA RESPOSTA COM STATUS DE AGUARDANDO
                var proximaEtapa = ObterProximaEtapaNaoRespondida(etapaResposta.Etapa.Processo.ID, etapaResposta.ProcessoResposta.ID);

                etapaResposta.DataAlteracao = DateTime.Now;
                etapaResposta.DataPreenchimento = DateTime.Now;

                if (rascunho){
                    etapaResposta.Status = (int)enumStatusEtapaResposta.Aguardando;
                    etapaResposta.DataPreenchimento = null;
                    proximaEtapa = null;
                }
                
                //VERIFICA SE A ETAPA FOI NEGADA OU SE ESTÁ PARA AJUSTE
                if (situacao.ID == (int)enumStatusEtapaResposta.Negado || situacao.ID == (int)enumStatusEtapaResposta.AAjustar)
                {
                    //VERIFICA SE A ETAPA REDIRECIONA PARA OUTRA ETAPA E SE ESTA PARA AJUSTE
                    if (situacao.ID == (int)enumStatusEtapaResposta.AAjustar && etapaResposta.Etapa.RequerAprovacao && etapaResposta.Etapa.EtapaRetorno != null)
                    {
                        //INATIVA TODAS AS ETAPA RESPOSTAS ENTRE A ETAPA ATUAL E A ETAPA DE RETORNO
                        List<EtapaResposta> listaEtapas = bmEtapaResposta.ObterEtapasEntreRangeDeOrdem(etapaResposta.Etapa.EtapaRetorno.Ordem, etapaResposta.Etapa.Ordem, etapaResposta.ProcessoResposta.ID);
                        foreach (var item in listaEtapas)
                        {
                            item.Ativo = false;
                            item.DataAlteracao = DateTime.Now;
                            bmEtapaResposta.Salvar(item);
                        }

                        //ADICIONA ETAPA DE RETORNO
                        EtapaResposta etapaRetorno = new EtapaResposta()
                        {
                            Status = (int)enumStatusEtapaResposta.Aguardando,
                            ProcessoResposta = etapaResposta.ProcessoResposta,
                            Etapa = etapaResposta.Etapa.EtapaRetorno,
                            Ativo = true,
                            DataAlteracao = DateTime.Now,
                            Auditoria = new Auditoria(),
                            PrazoEncaminhamento = etapaResposta.Etapa.ObterPrazoParaEncaminhamentoDaDemanda(etapaResposta.DataPreenchimento.HasValue ? etapaResposta.DataPreenchimento.Value : etapaResposta.DataAlteracao.Value)
                        };
                        bmEtapaResposta.Salvar(etapaRetorno);
                    }
                    else
                    {
                        proximaEtapa = null;
                    }

                }
                else
                {
                    if (proximaEtapa != null)
                    {
                        EtapaResposta proximaEtapaResposta = new EtapaResposta()
                        {
                            Status = (int)enumStatusEtapaResposta.Aguardando,
                            ProcessoResposta = etapaResposta.ProcessoResposta,
                            Etapa = proximaEtapa,
                            Ativo = true,
                            DataAlteracao = DateTime.Now,
                            Auditoria = new Auditoria(),
                            PrazoEncaminhamento = proximaEtapa.ObterPrazoParaEncaminhamentoDaDemanda(etapaResposta.DataPreenchimento.HasValue ? etapaResposta.DataPreenchimento.Value : etapaResposta.DataAlteracao.Value)
                        };

                        //ADICIONAR PROXIMA ETAPA RESPOSTA COM STATUS AGUARDANDO
                        bmEtapaResposta.Salvar(proximaEtapaResposta);

                        // SALVA PERMISSÕES DO NÚCLEO NA PRÓXIMA ETAPA
                        if (proximaEtapaResposta != null && permissoesNucleo != null)
                            new ManterEtapaRespostaPermissao().IncluirPermissoes(proximaEtapaResposta, permissoesNucleo.Select(x => x.ID_EtapaPermissaoNucleo));


                        NotificarEtapaAAnalisar(proximaEtapaResposta, permissoesNucleo);
                    }
                }


                List<CampoResposta> listaCampoResposta = new List<CampoResposta>();
                List<AlternativaResposta> listaAlternativaResposta = new List<AlternativaResposta>();
                //var camposResposta = new ManterCampoResposta().ObterRespostasEtapaResposta(idEtapaResposta);

                foreach (var campo in respostas)
                {
                    if (campo.TipoCampo.ID != (int)enumTipoCampo.FileUpload)
                    {
                        CampoResposta campoResposta = new CampoResposta()
                        {
                            Campo = new Campo()
                            {
                                ID = campo.ID
                            },
                            EtapaResposta = etapaResposta,
                            Resposta = campo.Resposta
                        };

                        listaCampoResposta.Add(campoResposta);

                        //VERIFICA SE A QUESTÃO TEM ALTERNATIVAS
                        if (campo.ListaAlternativas != null && campo.ListaAlternativas.Count > 0 && campo.ListaAlternativas.Any(f => f.OpcaoRespondida))
                        {
                            campoResposta.Resposta = null;

                            //ADICIONA CADA ALTERNATIVA RESPOSTA NA LISTA
                            foreach (var alternativa in campo.ListaAlternativas.Where(d => d.OpcaoRespondida))
                            {
                                listaAlternativaResposta.Add(new AlternativaResposta()
                                {
                                    Alternativa = new Alternativa()
                                    {
                                        ID = alternativa.ID
                                    },
                                    CampoResposta = campoResposta
                                });
                            }
                        }

                    }
                    else if (campo.TipoCampo.ID == (int)enumTipoCampo.FileUpload)
                    {
                        salvaCampoRespostaArquivo(analista.CPF, etapaResposta, campo);
                    }
                    else if (campo.TipoCampo.ID == (int)enumTipoCampo.MultipleFileUpload)
                    {
                        salvaCampoRespostaMultiplosArquivos(analista.CPF, etapaResposta, campo);
                    }
                }

                //SALVAR ETAPA RESPOSTA
                bmEtapaResposta.Salvar(etapaResposta);

                //ADICIONAR CAMPO RESPOSTAS
                new BMCampoResposta().Salvar(listaCampoResposta);

                //ADICIONAR ALTERNATIVAS CASO EXISTIREM
                new BMAlternativaResposta().Salvar(listaAlternativaResposta);

                ProcessoResposta processoResposta = new BMProcessoResposta().ObterPorEtapaRespostaId(etapaResposta.ID);
                VerificarProcessoConcluido(processoResposta);
                
                if (etapaResposta.Status != (int)enumStatusEtapaResposta.Aguardando)
                {
                    //SALVAR E ENVIAR NOTIFICAÇÕES DE CONCLUSÃO DE ETAPA/DEMANDA
                    NotificarConclusao(etapaResposta,
                                   temProximaEtapa: proximaEtapa != null,
                                   etapaNegada: situacao.ID == (int)enumStatusEtapaResposta.Negado);

                    if (etapaResposta.Status == (int)enumStatusEtapaResposta.Aprovado ||
                        etapaResposta.Status == (int)enumStatusEtapaResposta.AAjustar ||
                        etapaResposta.Status == (int)enumStatusEtapaResposta.Negado) {

                        string textoMensagem = "";
                        if (etapaResposta.Status == (int)enumStatusEtapaResposta.Aprovado)
                        {
                            textoMensagem = string.Format("A demanda #{0} foi aprovada na etapa {1} e encaminhada para a etapa {2}.", 
                                etapaResposta.ProcessoResposta.ID, etapaResposta.Etapa.Nome, proximaEtapa?.Nome);
                        }

                        if (etapaResposta.Status == (int)enumStatusEtapaResposta.Negado)
                        {
                            textoMensagem = string.Format("A demanda #{0}, não foi aprovada na etapa {1}, foi encerrada.", 
                                etapaResposta.ProcessoResposta.ID, etapaResposta.Etapa.Nome);
                        }

                        if (etapaResposta.Status == (int)enumStatusEtapaResposta.AAjustar)
                        {
                            textoMensagem = string.Format("Demanda #{0} devolvida para ajuste. Acesse www.uc.sebrae.com.br, menu Demandas -> Acompanhar para ajustar conforme orientação.",
                                etapaResposta.ProcessoResposta.ID);
                        }
                        
                        var template = TemplateUtil.ObterInformacoes(enumTemplate.NotificacaoDemanda);
                        var assuntoDoEmail = template.Assunto + string.Format(" #{0}", etapaResposta.ProcessoResposta.ID);
                        var textoEmail = template.TextoTemplate;

                        textoEmail = textoEmail.Replace("#NOME", etapaResposta.ProcessoResposta.Usuario.Nome).Replace("#MENSAGEM", textoMensagem);

                        EmailUtil.Instancia.EnviarEmail(etapaResposta.ProcessoResposta.Usuario.Email, template.Assunto, textoEmail);
                    }
                }
            }

            return new RetornoWebService();
        }

        /// <summary>
        /// Atualizar o cargo do usuário na etapa respondida.
        /// </summary>
        private static void AtualizarCargoDaEtapaResposta(int? cargoId, Usuario analista, EtapaResposta etapaResposta)
        {
            // Esse método só interessa para etapas em que o analista é o demandante.
            // Se não for o caso, escapa do método.
            if (analista.ID != etapaResposta.ProcessoResposta.Usuario.ID)
                return;

            if (cargoId.HasValue && cargoId > 0)
            {
                var cargo = new BMCargo().ObterPorId(cargoId.Value);

                // Verificar se o cargo está ativo.
                if (cargo.Ativo == false)
                {
                    throw new AcademicoException("O cargo selecionado está inativo");
                }

                // Verificar se o usuário está realmente neste cargo.
                if (analista.ListaUsuarioCargo.Any(x => x.Cargo.ID == cargoId) == false)
                {
                    throw new AcademicoException("O analista não está no cargo selecionado");
                }

                etapaResposta.CargoAnalista = cargo;
            }
            // Continuar utilizando o mesmo cargo da etapa anterior na etapa respondida.
            else
            {
                etapaResposta.CargoAnalista = analista.ObterCargo()?.Cargo;
            }
        }

        #endregion

        #region Métodos Helper

        public DTOTipoFormulario ObterTipoCampo(byte campo)
        {
            return new DTOTipoFormulario { ID = campo, Tipo = ((enumTipoCampo)campo).ToString() };
        }

        #endregion

        public DTOMeusProcessos ConsultarMeusProcessos(string cpf)
        {
            var usuario = new ManterUsuario().ObterPorCPF(cpf);

            DTOMeusProcessos meusProcessos = new DTOMeusProcessos();

            if (usuario != null)
            {
                ProcessoResposta processoResposta = new ProcessoResposta();
                BMProcessoResposta bmProcessoResposta = new BMProcessoResposta();

                processoResposta.Usuario = usuario;

                IList<ProcessoResposta> processos = bmProcessoResposta.ObterPorFiltro(processoResposta)
                    .Where(x => !x.Processo.Mensal.Value || (x.Processo.Mensal.Value && DateTime.Now.Day >= x.Processo.DiaInicio && DateTime.Now.Day <= x.Processo.DiaFim))
                    .OrderByDescending(x => x.ID).ToList();

                if (processos.Count() > 0)
                {

                    foreach (ProcessoResposta processo in processos)
                    {
                        DTOProcessoResposta dtoProcessoResposta = new DTOProcessoResposta();

                        // Variáveis do processo resposta
                        dtoProcessoResposta.ID = processo.ID;
                        dtoProcessoResposta.ID_Processo = processo.Processo.ID;
                        dtoProcessoResposta.NomeProcesso = processo.Processo.Nome;
                        dtoProcessoResposta.DataUltimaAtualizacao = processo.ListaEtapaResposta.Any() ? processo.ListaEtapaResposta.LastOrDefault().DataAlteracao : null;
                        dtoProcessoResposta.DataSolicitacao = processo.DataSolicitacao;
                        processoResposta.Usuario = usuario;

                        dtoProcessoResposta.Situacao = new DTOSituacaoProcesso
                        {
                            ID = (int)processo.Status,
                            Nome = processo.Status.GetDescription()
                        };


                        // Se o usuário precisa reiniciar ou não o processo
                        dtoProcessoResposta.DeveReiniciarProcesso = bmProcessoResposta.demandanteDeveReIniciar(processo, usuario);

                        // Variáveis da Etapa e EtapaResposta combinadas
                        EtapaResposta etapaAtual = bmProcessoResposta.ObterEtapaRespostaAtual(processo.ID);

                        if (etapaAtual.ID != 0)
                        {
                            DTOEtapa dtoEtapa = new DTOEtapa();
                            dtoEtapa.ID = etapaAtual.Etapa.ID;
                            dtoEtapa.Nome = etapaAtual.Etapa.Nome;
                            dtoEtapa.LinkAnexo = etapaAtual.Etapa.FileServer != null ? ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS).Registro + @"/ExibirFileServer.ashx?Identificador=" + etapaAtual.Etapa.FileServer.NomeDoArquivoNoServidor : "";

                            // Dados do analista que fez o preenchimento caso exista
                            if (etapaAtual.Analista != null)
                            {
                                dtoEtapa.Analista = new DTOUsuario() { ID = etapaAtual.Analista.ID, Nome = etapaAtual.Analista.Nome };
                            }

                            // Situação atual da Etapa
                            dtoEtapa.Situacao = new DTOSituacaoProcesso() { ID = etapaAtual.Status, Nome = Enum.GetName(typeof(enumStatusEtapaResposta), etapaAtual.Status) };

                            // Inclui a etapa atual no processo resposta
                            dtoProcessoResposta.EtapaAtual = dtoEtapa;
                        }

                        meusProcessos.Processos.Add(dtoProcessoResposta);

                    }
                }
            }

            return meusProcessos;
        }

        private List<DTOProcessoRespostaAcompanhar> ConsultarEtapasAnalisada(Usuario usuario, int? numero,
            int? demandanteId, int? processoId, int? etapaId)
        {
            var listaAnalisadas = bmProcesso.ConsultarEtapasAnalisada(usuario, numero, demandanteId, processoId);

            EtapaResposta etapaResposta;

            return listaAnalisadas.Select(x => new DTOProcessoRespostaAcompanhar
            {
                IdProcessoResposta = x.ID,
                IdProcesso = x.Processo.ID,
                NomeProcesso = x.Processo.Nome,
                DataAberturaDemanda = x.DataSolicitacao,
                IdEtapa = (etapaResposta = x.ListaEtapaResposta.OrderByDescending(er => er.ID).FirstOrDefault())?.Etapa?.ID,
                NomeEtapa = etapaResposta?.Etapa?.Nome,
                IdDemandante = x.Usuario.ID,
                Demandante = x.Usuario.Nome,
                Unidade = x.Usuario.Unidade,
                DataUltimoEnvio = x.DataAlteracao ?? x.DataSolicitacao,
                StatusProcesso = (int)enumStatusDemanda.Analisada,
                DataInicioCapacitacao = new ManterCampoResposta().ObterRespostaDataCapacitacao(x.ID)?.Resposta
            })
            .Where(x => etapaId == null || x.IdEtapa == etapaId)
            .Distinct().ToList();
        }

        private List<DTOProcessoRespostaAcompanhar> ConsultarEtapasEmAndamento(Usuario usuario, int? numero,
            int? demandanteId, int? processoId, int? etapaId)
        {
            var listaEmAndamento =
                bmProcesso.ConsultarEtapasEmAndamento(usuario, numero, demandanteId, processoId);

            var bmProcessoResposta = new BMProcessoResposta();

            EtapaResposta etapaRespostaAtual;

            return listaEmAndamento.Select(x => new DTOProcessoRespostaAcompanhar
            {
                IdProcessoResposta = x.ProcessoResposta.ID,
                IdProcesso = x.ProcessoResposta.Processo.ID,
                NomeProcesso = x.ProcessoResposta.Processo.Nome,
                DataAberturaDemanda = x.ProcessoResposta.DataSolicitacao,
                IdEtapa = (etapaRespostaAtual = bmProcessoResposta.ObterEtapaRespostaAtual(x.ProcessoResposta.ID))?.Etapa?.ID,
                NomeEtapa = etapaRespostaAtual?.Etapa.Nome,
                IdDemandante = x.ProcessoResposta.Usuario.ID,
                Demandante = x.ProcessoResposta.Usuario.Nome,
                Unidade = x.ProcessoResposta.Usuario.Unidade,
                DataUltimoEnvio = x.DataPreenchimento ?? x.DataAlteracao,
                StatusProcesso = (int)enumStatusDemanda.EmAndamento,
                DataInicioCapacitacao = new ManterCampoResposta().ObterRespostaDataCapacitacao(x.ID)?.Resposta
            })
            .Where(x => etapaId == null || x.IdEtapa == etapaId)
            .ToList();
        }

        public DTOProcessoResposta ConsultarStatusProcesso(string cpf, int p_id)
        {
            var usuario = bmUsuario.ObterPorCPF(cpf);

            var dtoProcessoResposta = new DTOProcessoResposta();

            if (usuario != null)
            {
                var bmProcessoResposta = new BMProcessoResposta();

                var processoResposta = bmProcessoResposta.ObterPorId(p_id);

                // O usuário só pode acompanhar o status de um processo que ele iniciou ou que ele é analista
                if (processoResposta.Usuario.ID == usuario.ID || processoResposta.ListaEtapaResposta.Any(x => x.Analista != null && x.Analista.ID == usuario.ID))
                {
                    dtoProcessoResposta.ID = processoResposta.ID;
                    dtoProcessoResposta.NomeProcesso = processoResposta.Processo.Nome;
                    dtoProcessoResposta.ID_Processo = processoResposta.Processo.ID;
                    dtoProcessoResposta.ID_ProcessoReposta = processoResposta.ID;

                    // Pega todas as etapas reprovadas para compor o histórico
                    var etapasReprovadas = bmProcessoResposta.pegaTodasEtapasReprovadas(processoResposta);

                    if (etapasReprovadas.Any())
                    {
                        foreach (var etapaReprovada in etapasReprovadas)
                        {
                            DTOEtapa dtoEtapa = new DTOEtapa();
                            dtoEtapa.ID = etapaReprovada.Etapa.ID;
                            dtoEtapa.Nome = etapaReprovada.Etapa.Nome;
                            dtoEtapa.ID_RespostaEtapa = etapaReprovada.ID;
                            dtoEtapa.Ordem = etapaReprovada.Etapa.Ordem;
                            dtoEtapa.DataPreenchimentoDateTime = etapaReprovada.DataPreenchimento;
                            dtoEtapa.LinkAnexo = etapaReprovada.Etapa.FileServer != null ? ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS).Registro + @"/ExibirFileServer.ashx?Identificador=" + etapaReprovada.Etapa.FileServer.NomeDoArquivoNoServidor : "";

                            if (etapaReprovada.Analista != null)
                            {
                                dtoEtapa.Analista = new DTOUsuario { ID = etapaReprovada.Analista.ID, Nome = etapaReprovada.Analista.Nome };
                            }

                            dtoEtapa.ListaCampos.AddRange(ConsultarCamposEtapaResposta(etapaReprovada.ID, processoResposta.Usuario));

                            dtoEtapa.Situacao = new DTOSituacaoProcesso().ObterSituacao((int)etapaReprovada.Status, etapaReprovada);

                            dtoProcessoResposta.ListaEtapasReprovadas.Add(dtoEtapa);
                        }
                    }

                    // Se o usuário precisa reiniciar ou não o processo
                    dtoProcessoResposta.DeveReiniciarProcesso = bmProcessoResposta.demandanteDeveReIniciar(processoResposta, usuario);

                    var etapaAtual = bmProcessoResposta.ObterEtapaRespostaAtual(processoResposta.ID);

                    // Bloqueia o botão de cancelamento de processo caso não seja a primeira etapa e o processo não esteja concluído
                    dtoProcessoResposta.LiberarCancelamento = VerificarPermissaoCancelamentoProcesso(usuario, processoResposta, etapaAtual);

                    if (etapaAtual.ID != 0)
                    {
                        var dtoEtapa = new DTOEtapa();
                        dtoEtapa.ID = etapaAtual.Etapa.ID;
                        dtoEtapa.Nome = etapaAtual.Etapa.Nome;
                        dtoEtapa.Ordem = etapaAtual.Etapa.Ordem;

                        // Inclui a etapa atual no processo resposta
                        dtoProcessoResposta.EtapaAtual = dtoEtapa;
                    }

                    var etapasRespostaAtivas = processoResposta.ListaEtapaResposta.ToList();

                    foreach (var etapaResposta in etapasRespostaAtivas)
                    {

                        var dtoEtapa = new DTOEtapa();

                        dtoEtapa.ID = etapaResposta.Etapa.ID;
                        dtoEtapa.ID_RespostaEtapa = etapaResposta.ID;
                        dtoEtapa.Nome = etapaResposta.Etapa.Nome;
                        dtoEtapa.Ordem = etapaResposta.Etapa.Ordem;
                        dtoEtapa.LinkAnexo = etapaResposta.Etapa.FileServer != null ? ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS).Registro + @"/ExibirFileServer.ashx?Identificador=" + etapaResposta.Etapa.FileServer.NomeDoArquivoNoServidor : "";

                        if (etapaResposta.Analista != null)
                        {
                            dtoEtapa.Analista = new DTOUsuario { ID = etapaResposta.Analista.ID, Nome = etapaResposta.Analista.Nome };
                        }

                        if (etapaResposta.Status == (int)enumStatusEtapaResposta.Aguardando)
                        {
                            var cargoUsuario = usuario.ObterCargo();
                            if (cargoUsuario != null)
                            {
                                // Buscar todos os diretores, pela UF do UsuarioCargo.
                                var diretores = new BMUsuarioCargo().ObterPorTipoCargo(EnumTipoCargo.Diretoria).Where(x => x.Cargo.Uf.ID == cargoUsuario.Cargo.Uf.ID).ToList();

                                dtoEtapa.Analistas = etapaAtual.ObterAnalistas(diretores).GroupBy(x => x.ID).Select(x => new DTOUsuario
                                    {
                                        ID = x.Key,
                                        Nome = x.FirstOrDefault()?.Nome
                                    })
                                    .ToList();
                            }
                        }

                        if (etapaResposta.Assessor != null)
                        {
                            dtoEtapa.Assessor = new DTOUsuario { ID = etapaResposta.Assessor.ID, Nome = etapaResposta.Assessor.Nome };
                        }

                        // Situação atual da Etapa
                        dtoEtapa.Situacao = new DTOSituacaoProcesso { ID = etapaResposta.Status, Nome = etapaResposta.ObterSituacao };

                        // Data de preenchimento da etapa
                        dtoEtapa.DataPreenchimento = etapaResposta.DataPreenchimento.HasValue ? etapaResposta.DataPreenchimento.Value.ToString("dd/MM/yyyy HH:mm") : "";

                        // #2004 - Data em tipo DateTime somente para ordenar pelo preenchimento, para manter o fluxo do processo na ordem correta.
                        dtoEtapa.DataPreenchimentoDateTime = etapaResposta.DataPreenchimento;

                        // Inclui a etapa atual no processo resposta
                        dtoProcessoResposta.ListaEtapas.Add(dtoEtapa);
                    }

                    // #2004 - Busca todas as etapas futuras não iniciadas.
                    var etapasNaoIniciadas = processoResposta.Processo.ListaEtapas.Where(x => x.Ordem > etapaAtual.Etapa.Ordem).OrderBy(x => x.Ordem).ToList();

                    foreach (var etapa in etapasNaoIniciadas)
                    {
                        var dtoEtapa = new DTOEtapa
                        {
                            ID = etapa.ID,
                            Nome = etapa.Nome,
                            Ordem = etapa.Ordem,
                            Situacao =
                                new DTOSituacaoProcesso
                                {
                                    ID = (int)enumStatusEtapaResposta.Aguardando,
                                    Nome = Enum.GetName(typeof(enumStatusEtapaResposta), enumStatusEtapaResposta.Aguardando)
                                }
                        };

                        // Inclui a esta etapa não iniciada no processo resposta para visualização das etapas futuras.
                        dtoProcessoResposta.ListaEtapas.Add(dtoEtapa);
                    }
                }
            }

            // #2004 - Reordenar as etapas de acordo com o fluxo do processo.
            dtoProcessoResposta.ListaEtapas = dtoProcessoResposta.ListaEtapas
                .OrderBy(x => x.DataPreenchimentoDateTime == null)
                .ThenBy(x => x.DataPreenchimentoDateTime)
                .ToList();

            return dtoProcessoResposta;
        }

        public bool VerificarPermissaoVisualizacaoHistorico(int idProcessoResposta, string cpfUsuario, int idEtapaResposta)
        {
            var usuario = new BMUsuario().ObterPorCPF(cpfUsuario);
            // Pega as etapas disponíveis no histórico do usuário
            var etapasConcluidas = ConsultarEtapasConcluidas(cpfUsuario);
            var etapasEmAndamento = ConsultarEtapasEmAndamento(usuario, null, null, null, null);
            var etapasAnalisadas = ConsultarEtapasAnalisada(usuario, null, null, null, null);

            return (
                etapasConcluidas.Any(e => e.ID_ProcessoResposta == idProcessoResposta) ||
                etapasEmAndamento.Any(e => e.IdProcessoResposta == idProcessoResposta) ||
                etapasAnalisadas.Any(e => e.IdProcessoResposta == idProcessoResposta)
            );
        }

        public bool VerificarPermissaoAnalise(int idProcessoResposta, string cpfUsuario)
        {
            // Buscar todos os diretores, independente da UF.
            var diretores = new BMUsuarioCargo().ObterPorTipoCargo(EnumTipoCargo.Diretoria).ToList();

            // Se o processo estiver concluído e o usuário estiver acessando o processo solicitado (visualização de histórico)            
            var etapaAtual = new BMEtapa().ObterEtapaAtualDeProcesso(idProcessoResposta);

            var etapResposta =
                etapaAtual.ListaEtapaResposta.FirstOrDefault(er => er.ProcessoResposta.ID == idProcessoResposta && er.Ativo);

            etapResposta.ListaEtapaEncaminhamentoUsuario = new ManterEtapaEncaminhamentoUsuario().ObterTodosIQueryable().Where(ee => ee.EtapaResposta.ID == etapResposta.ID).ToList();

            var usuarioLogado = new ManterUsuario().ObterPorCPF(cpfUsuario);

            return bmProcesso.UsuarioPodeAnalisar(diretores, etapResposta, null, null, null, null, usuarioLogado);
        }

        public bool VerificarPermissaoInscricao(int idProcesso, string cpfUsuario)
        {
            var usuario = new BMUsuario().ObterPorCPF(cpfUsuario);

            var processos = ConsultarProcessosPermitidosInscricao(cpfUsuario);

            return processos.Any(p => p.ID == idProcesso);
        }

        public bool VerificarPermissaoCancelamentoProcesso(Usuario usuario, ProcessoResposta processo, EtapaResposta etapaAtual)
        {
            if (!usuario.IsAdministrador() && usuario.ID != processo.Usuario.ID) return false;

            // Verifica se a etapa atual não é a primeira etapa e se o processo ainda não foi concluído.
            return !processo.Concluido && etapaAtual != null &&
                        processo.ListaEtapaResposta.Any() && etapaAtual.ID != processo.ListaEtapaResposta[0].ID;
        }

        private void NotificarEtapaAAnalisar(EtapaResposta etapaResposta, List<DTOEtapaPermissaoNucleo> permissoesNucleo = null)
        {
            try
            {
                List<Usuario> usuariosAAnalisar = new BMPermissao().ObterUsuariosAAnalisar(etapaResposta.ID,
                    etapaResposta.Etapa.PodeSerAprovadoChefeGabinete).ToList();

                // Adiciona os analistas do núcleo
                if (permissoesNucleo != null && permissoesNucleo.Any())
                {
                    var manter = new ManterEtapaPermissaoNucleo();

                    foreach (var dtoEtapaPermissaoNucleo in permissoesNucleo)
                    {
                        var permissao = manter.ObterPorID(dtoEtapaPermissaoNucleo.ID_EtapaPermissaoNucleo);
                        usuariosAAnalisar.Add(permissao.HierarquiaNucleoUsuario.Usuario);
                    }
                }

                var manterNotificacao = new BP.ManterNotificacao();

                var link = "demandas/analisar/";

                var processo = new ManterProcesso().ObterPorID(etapaResposta.ProcessoResposta.Processo.ID);

                var textoNotificacao = string.Format("Nova demanda a ser analisada: {0}.",
                    processo.Nome.ToUpper());

                var usuarioProcessoResposta = new ManterUsuario().ObterPorID(etapaResposta.ProcessoResposta.Usuario.ID);

                var textoEmail =
                    string.Format(
                        "Há uma nova demanda a ser analisada: {0} aberta por {1}. <br /><br />Acesse o portal <a href=\"{2}\">{2}</a> para mais informações.",
                        processo.Nome.ToUpper(),
                        usuarioProcessoResposta.Nome,
                        new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID(
                            (int)enumConfiguracaoSistema.EnderecoPortal).Registro);

                var notificacaoEnvio = new NotificacaoEnvio
                {
                    Texto = textoEmail,
                    Link = link
                };

                new ManterNotificacaoEnvio().AlterarNotificacaoEnvio(notificacaoEnvio);

                foreach (var item in usuariosAAnalisar)
                {
                    manterNotificacao.PublicarNotificacao(link, textoNotificacao, item.ID, notificacaoEnvio);
                }

                if (usuariosAAnalisar.Any())
                {
                    var emailsPorVirgula =
                        usuariosAAnalisar.Select(u => u.Email).Aggregate((e1, e2) => e1 + ", " + e2).Trim();

                    EmailUtil.Instancia.EnviarEmail(emailsPorVirgula, "Demanda a ser analisada", textoEmail);
                }
            }
            catch
            {
                // Ignored.
            }
        }

        /// <summary>
        /// Envia notificação para os usuários com permissão de Notificação e para o usuário demandante
        /// </summary>
        private void NotificarConclusao(EtapaResposta etapaResposta, bool temProximaEtapa, bool etapaNegada)
        {
            try
            {
                var usuariosANotificar = new BMPermissao().ObterUsuariosANotificar(etapaResposta.ID,
                    etapaResposta.Etapa.PodeSerAprovadoChefeGabinete);

                var proximaEtapaResposta = new ManterEtapaResposta().ObterUltimaEtapaRespostaPorProcessoResposta(etapaResposta.ProcessoResposta.ID);

                if (proximaEtapaResposta != null && proximaEtapaResposta.Etapa.NotificarNucleo && proximaEtapaResposta.PermissoesNucleoEtapaResposta.Any())
                {
                    usuariosANotificar.ToList().AddRange(new ManterEtapaRespostaPermissao().ObterTodosUsuarios(etapaResposta));
                }

                //Notificar também o demandante
                if (usuariosANotificar.All(u => u.ID != etapaResposta.ProcessoResposta.Usuario.ID))
                    usuariosANotificar.Add(etapaResposta.ProcessoResposta.Usuario);

                var nomeProcesso = etapaResposta.ProcessoResposta.Processo.Nome.ToUpper();
                var nomeEtapa = etapaResposta.Etapa.Nome.ToUpper();
                var nomeDemandante = etapaResposta.ProcessoResposta.Usuario.Nome.ToUpper();

                string tituloEmail, textoEmail, textoNotificacao;

                var link = "demandas/acompanhar/" + etapaResposta.ProcessoResposta.ID;

                if (!temProximaEtapa && !etapaNegada)
                {
                    textoNotificacao = string.Format("A demanda {0} foi concluída", nomeProcesso);

                    tituloEmail = string.Format("Demanda {0} Concluída", nomeProcesso);

                    textoEmail = string.Format("A demanda {0} solicitada por {1} foi concluída.", nomeProcesso,
                        nomeDemandante);
                }
                else if (etapaNegada)
                {
                    textoNotificacao = string.Format("A etapa {0} foi negada", nomeEtapa);

                    tituloEmail = string.Format("Etapa {0} negada", nomeEtapa);

                    textoEmail = string.Format("A etapa {0} da demanda {1} foi negada!", nomeEtapa, nomeProcesso);
                }
                else
                {
                    textoNotificacao = string.Format("A demanda {0} mudou de status.", nomeProcesso);

                    tituloEmail = string.Format("Etapa {0} da Demanda {1} Concluída", nomeEtapa, nomeProcesso);

                    textoEmail = string.Format("A etapa {0} da demanda {1} solicitada por {2} foi concluída.", nomeEtapa,
                        nomeProcesso, nomeDemandante);
                }

                textoEmail += string.Format(
                    "<br /><br />Acesse o portal <a href=\"{0}\">{0}</a> para mais informações.",
                    new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID(
                        (int)enumConfiguracaoSistema.EnderecoPortal).Registro);

                var manterNotificacao = new BP.ManterNotificacao();

                var notificacaoEnvio = new NotificacaoEnvio();
                var manter = new ManterNotificacaoEnvio();

                notificacaoEnvio.Texto = textoNotificacao;
                notificacaoEnvio.Link = link;

                foreach (var item in usuariosANotificar)
                {
                    manterNotificacao.PublicarNotificacao(link, textoNotificacao, item.ID, notificacaoEnvio);
                }

                string emailsPorVirgula = usuariosANotificar.Select(u => u.Email).Aggregate((e1, e2) => e1 + ", " + e2).Trim();

                EmailUtil.Instancia.EnviarEmail(emailsPorVirgula, tituloEmail, textoEmail);
            }
            catch
            {
                // Ignored.
            }
        }

        public bool FinalizarProcessoComJustificativa(int idProcessoResposta, int idUsuarioCancelamento, string justificativa)
        {
            try
            {
                var bm = bmProcesso;

                var processo = bm.ObterProcessoResposta(idProcessoResposta);
                var usuario = new BMUsuario().ObterPorId(idUsuarioCancelamento);

                if (processo == null || usuario == null) return false;

                processo.Concluido = true;
                processo.JustificativaCancelamento = justificativa;
                processo.Status = enumStatusProcessoResposta.Cancelado;
                processo.UsuarioCancelamento = usuario;
                processo.Auditoria = new Auditoria();

                bm.SalvarProcessoResposta(processo);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}




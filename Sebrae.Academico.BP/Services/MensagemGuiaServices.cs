using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BP.DTO.Services.Trilhas.Solucao;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BP.DTO.Services.Trilhas.MensagemGuia;

namespace Sebrae.Academico.BP.Services
{
    public partial class TrilhaServices
    {
        public List<DTOMensagemGuia> ObterMensagensGuiaMapa(UsuarioTrilha matricula)
        {
            var retorno = new List<DTOMensagemGuia>();

            // Primeiro acesso ao mapa
            var mensagemPrimeiroAcesso = VerificarPrimeiroAcesso(matricula, enumMomento.PrimeiroAcessoMapa);

            if (mensagemPrimeiroAcesso != null)
                retorno = mensagemPrimeiroAcesso;

            // Possuí o mínimo de moedas para prova final
            var mensagensPossuiMoedasProvaFinal = VerificarMoedasProvaFinal(matricula);
            if (mensagensPossuiMoedasProvaFinal != null)
                retorno.Add(mensagensPossuiMoedasProvaFinal);

            var mensagemEvolucaoPin = VerificarEvolucaoPin(matricula);
            if (mensagemEvolucaoPin != null)
                retorno.Add(mensagemEvolucaoPin);

            return retorno;
        }

        public List<DTOMensagemGuia> ObterMensagensGuiaLoja(UsuarioTrilha matricula, PontoSebrae pontoSebrae)
        {
            var retorno = new List<DTOMensagemGuia>();

            // Primeiro acesso à loja.
            var mensagemPrimeiroAcesso = VerificarPrimeiroAcesso(matricula, enumMomento.PrimeiroAcessoLoja, pontoSebrae);
            if (mensagemPrimeiroAcesso != null)
                retorno = mensagemPrimeiroAcesso;

            // Verificar histórico de líderes.
            var mensagemHistoricoLider = VerificarHistoricoLider(matricula, pontoSebrae);
            if (mensagemHistoricoLider != null)
                retorno.Add(mensagemHistoricoLider);

            // Primeira conclusão.
            var mensagemPrimeiraConclusaoSolucaoSebrae = VerificarPrimeiraConclusaoSolucaoSebrae(matricula);
            if (mensagemPrimeiraConclusaoSolucaoSebrae != null)
                retorno.Add(mensagemPrimeiraConclusaoSolucaoSebrae);

            // Demais conclusões.
            var mensagensDemaisConclusaoSolucaoSebrae = VerificarDemaisConclusoesSolucaoSebrae(matricula);
            if (mensagensDemaisConclusaoSolucaoSebrae.Any())
                retorno.AddRange(mensagensDemaisConclusaoSolucaoSebrae);

            // Metade conclusão.
            var mensagemMetadeSolucoesSebrae = VerificarConclusaoMetadeSolucoesLoja(matricula, pontoSebrae);
            if (mensagemMetadeSolucoesSebrae != null)
                retorno.Add(mensagemMetadeSolucoesSebrae);

            // Concluiu metade das soluções
            var mensagensConcluiuTodasSolucoesSebrae = VerificarConclusaoTodasSolucoesLoja(matricula, pontoSebrae);
            if (mensagensConcluiuTodasSolucoesSebrae != null)
                retorno.Add(mensagensConcluiuTodasSolucoesSebrae);

            // Possuí o mínimo de moedas para prova final
            var mensagensPossuiMoedasProvaFinal = VerificarMoedasProvaFinal(matricula);
            if (mensagensPossuiMoedasProvaFinal != null)
                retorno.Add(mensagensPossuiMoedasProvaFinal);

            var mensagemEvolucaoPin = VerificarEvolucaoPin(matricula);
            if (mensagemEvolucaoPin != null)
                retorno.Add(mensagemEvolucaoPin);

            return retorno;
        }

        public List<DTOMensagemGuia> ObterMensagensGuiaConclusaoSolucaoSebrae(UsuarioTrilha matricula,
            TrilhaTopicoTematico loja)
        {
            var retorno = new List<DTOMensagemGuia>();

            // Primeira conclusão.
            var mensagemPrimeiraConclusaoSolucaoSebrae = VerificarPrimeiraConclusaoSolucaoSebrae(matricula);

            if (mensagemPrimeiraConclusaoSolucaoSebrae != null)
                retorno.Add(mensagemPrimeiraConclusaoSolucaoSebrae);

            // Possuí o mínimo de moedas para prova final
            var mensagensPossuiMoedasProvaFinal = VerificarMoedasProvaFinal(matricula);
            if (mensagensPossuiMoedasProvaFinal != null)
                retorno.Add(mensagensPossuiMoedasProvaFinal);

            var mensagemEvolucaoPin = VerificarEvolucaoPin(matricula);
            if (mensagemEvolucaoPin != null)
                retorno.Add(mensagemEvolucaoPin);

            return retorno;
        }

        public List<DTOMensagemGuia> ObterMensagensGuiaCambio(UsuarioTrilha matricula)
        {
            var retorno = new List<DTOMensagemGuia>();

            // Primeiro câmbio.
            var mensagemPrimeiroCambio = VerificarPrimeiraTentativaCambio(matricula);

            if (mensagemPrimeiroCambio != null)
                retorno.Add(mensagemPrimeiroCambio);

            // Possuí o mínimo de moedas para prova final
            var mensagensPossuiMoedasProvaFinal = VerificarMoedasProvaFinal(matricula);
            if (mensagensPossuiMoedasProvaFinal != null)
                retorno.Add(mensagensPossuiMoedasProvaFinal);

            var mensagemEvolucaoPin = VerificarEvolucaoPin(matricula);
            if (mensagemEvolucaoPin != null)
                retorno.Add(mensagemEvolucaoPin);

            return retorno;
        }

        public List<DTOMensagemGuia> ObterMensagensGuiaMochila(UsuarioTrilha matricula, TrilhaNivel nivel)
        {
            var retorno = new List<DTOMensagemGuia>();

            // Primeiro acesso à mochila.
            var mensagemPrimeiroAcesso = VerificarPrimeiroAcesso(matricula, enumMomento.PrimeiroAcessoMochila);

            if (mensagemPrimeiroAcesso != null)
                retorno.AddRange(mensagemPrimeiroAcesso);

            // Primeira conclusão de missão.
            var mensagemPrimeiraConclusaoMissao = VerificarPrimeiraConclusaoMissao(matricula, nivel);
            if (mensagemPrimeiraConclusaoMissao != null)
                retorno.Add(mensagemPrimeiraConclusaoMissao);

            // Demais conclusões.
            var mensagensDemaisConclusoesMissoes = VerificarDemaisConclusoesMissao(matricula, nivel);
            if (mensagensDemaisConclusoesMissoes.Any())
                retorno.AddRange(mensagensDemaisConclusoesMissoes);

            // Possuí o mínimo de moedas para prova final
            var mensagensPossuiMoedasProvaFinal = VerificarMoedasProvaFinal(matricula);
            if (mensagensPossuiMoedasProvaFinal != null)
                retorno.Add(mensagensPossuiMoedasProvaFinal);

            var mensagemEvolucaoPin = VerificarEvolucaoPin(matricula);
            if (mensagemEvolucaoPin != null)
                retorno.Add(mensagemEvolucaoPin);

            return retorno;
        }

        public List<DTOMensagemGuia> ObterMensagensGuiaSolucaoTrilheiro(UsuarioTrilha matricula)
        {
            var retorno = new List<DTOMensagemGuia>();

            // Primeiro acesso ao mapa
            var mensagemPrimeiraCriacaoSolucaoTrilheiro = VerificarPrimeiroAcesso(matricula,
                enumMomento.PrimeiroAcessoCriacaoSolucaoTrilheiro);

            if (mensagemPrimeiraCriacaoSolucaoTrilheiro != null)
                retorno.AddRange(mensagemPrimeiraCriacaoSolucaoTrilheiro);

            return retorno;
        }

        private DTOMensagemGuia VerificarPrimeiraConclusaoSolucaoSebrae(UsuarioTrilha matricula)
        {

            if (matricula != null && matricula.ID != 0)
            {
                // Refresh básico da matrícula. Foi mal, mas estamos com pressa.
                matricula = new ManterUsuarioTrilha().ObterPorId(matricula.ID);

                var trilha = new ManterTrilha().ObterTrilhaPorId(matricula.TrilhaNivel.Trilha.ID);

                var momento = enumMomento.PrimeiraConclusaoSolucaoSebrae;

                if (matricula.ListaVisualizacoesMensagemGuia.All(x => x.MensagemGuia.ID != momento))
                {
                    var primeiraConclusao =
                        matricula.ListaItemTrilhaParticipacao.OrderBy(x => x.DataEnvio)
                            .FirstOrDefault(x => x.Autorizado == true);

                    if (primeiraConclusao == null)
                        return null;

                    var mensagem = new ManterMensagemGuia().ObterPorId(momento);

                    var visualizacao = RegistrarVisualizacao(matricula, mensagem);

                    // Refresh básico da Solução Sebrae. Foi mal, mas estamos com pressa.
                    var solucaoSebrae = new ManterItemTrilha().ObterItemTrilhaPorID(primeiraConclusao.ItemTrilha.ID);

                    return new DTOMensagemGuia(visualizacao.ID, mensagem.ObterTexto(trilha, matricula, solucaoSebrae: solucaoSebrae));
                }
            }
           

            return null;
        }

        private List<DTOMensagemGuia> VerificarDemaisConclusoesSolucaoSebrae(UsuarioTrilha matricula)
        {
            var retorno = new List<DTOMensagemGuia>();

            if (matricula != null && matricula.ID != 0)
            {

                var trilha = new ManterTrilha().ObterTrilhaPorId(matricula.TrilhaNivel.Trilha.ID);

                // Refresh básico da matrícula. Foi mal, mas estamos com pressa.
                matricula = new ManterUsuarioTrilha().ObterPorId(matricula.ID);

                var momento = enumMomento.DemaisConclusoesSolucaoSebrae;

                var visualizacoes =
                    matricula.ListaVisualizacoesMensagemGuia.Where(
                        x => x.MensagemGuia.ID == momento && x.Visualizacao == null && x.ItemTrilha != null).AsQueryable();

                // Contar a partir da segunda aprovação.
                if (matricula.ListaItemTrilhaParticipacao.Count(x => x.Autorizado == true) > 1 && visualizacoes.Any())
                {
                    foreach (var visualizacao in visualizacoes)
                    {
                        var mensagem = new ManterMensagemGuia().ObterPorId(momento);

                        // Salvar visualização dessa mensagem.
                        visualizacao.Visualizacao = DateTime.Now;
                        new ManterUsuarioTrilhaMensagemGuia().Salvar(visualizacao);

                        // Refresh básico da Solução Sebrae. Foi mal, mas estamos com pressa.
                        var solucaoSebrae = new ManterItemTrilha().ObterItemTrilhaPorID(visualizacao.ItemTrilha.ID);

                        retorno.Add(new DTOMensagemGuia(visualizacao.ID,
                            mensagem.ObterTexto(trilha, matricula, solucaoSebrae: solucaoSebrae)));
                    }
                }
            }

            return retorno;
        }

        private DTOMensagemGuia VerificarPrimeiraConclusaoMissao(UsuarioTrilha matricula, TrilhaNivel nivel)
        {
            // Refresh básico dos objetos. Foi mal, mas estamos com pressa.
            matricula = new ManterUsuarioTrilha().ObterPorId(matricula.ID);

            nivel = new ManterTrilhaNivel().ObterTrilhaNivelPorID(nivel.ID);

            var trilha = new ManterTrilha().ObterTrilhaPorId(matricula.TrilhaNivel.Trilha.ID);

            var momento = enumMomento.PrimeiraConclusaoMissao;

            if (matricula.ListaVisualizacoesMensagemGuia.All(x => x.MensagemGuia.ID != momento))
            {

                var primeiraConclusao =
                    nivel.ObterMissoes()
                        .FirstOrDefault(
                            m =>
                                m.UsuarioConcluiu(matricula)
                        );                

                if (primeiraConclusao == null)
                    return null;

                var mensagem = new ManterMensagemGuia().ObterPorId(momento);

                var visualizacao = RegistrarVisualizacao(matricula, mensagem, missao: primeiraConclusao);

                return new DTOMensagemGuia(visualizacao.ID, mensagem.ObterTexto(trilha, matricula, missao: primeiraConclusao));
            }

            return null;
        }

        private List<DTOMensagemGuia> VerificarDemaisConclusoesMissao(UsuarioTrilha matricula, TrilhaNivel nivel)
        {
            var retorno = new List<DTOMensagemGuia>();

            // Refresh básico da matrícula. Foi mal, mas estamos com pressa.
            matricula = new ManterUsuarioTrilha().ObterPorId(matricula.ID);
            nivel = new ManterTrilhaNivel().ObterTrilhaNivelPorID(nivel.ID);

            var trilha = new ManterTrilha().ObterTrilhaPorId(matricula.TrilhaNivel.Trilha.ID);

            var idsMissoesConcluidasVisualizadas =
                matricula.ListaVisualizacoesMensagemGuia.Where(x => x.Missao != null).Select(x => x.Missao.ID).ToList();

            var missoes = nivel.ObterMissoes();

            // Obter missões concluídas que o usuário ainda não recebeu a mensagem.
            var missoesConcluidas =
                missoes.Where(
                    x =>
                        !idsMissoesConcluidasVisualizadas.Contains(x.ID))
                    .ToList()
                    .Where(x => x.UsuarioConcluiu(matricula));

            foreach (var missao in missoesConcluidas)
            {
                var mensagem = new ManterMensagemGuia().ObterPorId(enumMomento.DemaisConclusoesMissao);

                var visualizacao = RegistrarVisualizacao(matricula, mensagem, missao: missao);

                // Salvar visualização dessa mensagem.
                new ManterUsuarioTrilhaMensagemGuia().Salvar(visualizacao);

                retorno.Add(new DTOMensagemGuia(visualizacao.ID, mensagem.ObterTexto(trilha, matricula, missao: missao)));
            }

            return retorno;
        }

        private DTOMensagemGuia VerificarPrimeiraTentativaCambio(UsuarioTrilha matricula)
        {
            var trilha = new ManterTrilha().ObterTrilhaPorId(matricula.TrilhaNivel.Trilha.ID);

            var momento = enumMomento.PrimeiraTentativaCambio;

            if (matricula.ListaVisualizacoesMensagemGuia.All(x => x.MensagemGuia.ID != momento))
            {
                MensagemGuia mensagemGuia;

                using (var manterMensagemGuia = new ManterMensagemGuia())
                {
                    mensagemGuia = manterMensagemGuia.ObterPorId(momento);
                }

                var visualizacao = RegistrarVisualizacao(matricula, mensagemGuia);

                return new DTOMensagemGuia(visualizacao.ID, mensagemGuia.ObterTexto(trilha, matricula));
            }

            return null;
        }

        private DTOMensagemGuia VerificarHistoricoLider(UsuarioTrilha matricula, PontoSebrae pontoSebrae)
        {
            var manterLogLider = new ManterLogLider();

            var trilha = new ManterTrilha().ObterTrilhaPorId(matricula.TrilhaNivel.Trilha.ID);

            // Só interessa obter os dois últimos logs.
            var ultimosLogs =
                manterLogLider.ObterPorAlunoPontoSebrae(matricula, pontoSebrae)
                    .Where(x => x.PontoSebrae.ID == pontoSebrae.ID)
                    .OrderByDescending(x => x.ID)
                    .Take(2)
                    .ToList();

            // Só interessa se a quantidade de logs for maior que 1. Se houver somente 1 log, não é necessário exibir nenhuma msg.
            if (ultimosLogs.Count() > 1)
            {
                var ultimoLog = ultimosLogs[0];
                var penultimoLog = ultimosLogs[1];

                // Explicando a lógica complicada abaixo:
                // Se não havia líder no último acesso e agora tem, exibe a msg do primeiro líder.
                // Caso existem líderes nos dois últimos logs, mas sejam líderes diferentes, exibe a msg.
                // Se nenhuma condição for satisfeita, retorna null e escapa da mensagem.
                var momento = penultimoLog.Lider == null && ultimoLog.Lider != null
                    ? enumMomento.PrimeiroLiderLojaUltimoAcesso
                    : (penultimoLog.Lider != null && ultimoLog.Lider != null &&
                       penultimoLog.Lider.ID != ultimoLog.Lider.ID
                        ? (enumMomento?)enumMomento.AlteracaoLiderLojaUltimoAcesso
                        : null);

                // Ragequit.
                if (momento == null)
                    return null;

                var mensagem = new ManterMensagemGuia().ObterPorId(momento.Value);

                // Verificar se a última visualização da mensagem foi para o líder atual.
                // Só faz sentido ser executado caso o momento seja AlteracaoLiderLojaUltimoAcesso
                if (momento.Value == enumMomento.AlteracaoLiderLojaUltimoAcesso &&
                    mensagem.ListaUsuarioTrilhaMensagemGuia.LastOrDefault(
                        x =>
                            x.UsuarioTrilha.ID == matricula.ID &&
                            x.LogLider != null &&
                            x.LogLider.Lider.ID == penultimoLog.Lider.ID) != null)
                    return null;

                var visualizacao = RegistrarVisualizacao(matricula, mensagem, ultimoLog);

                return new DTOMensagemGuia(visualizacao.ID,
                    mensagem.ObterTexto(trilha, matricula, ultimoLog, pontoSebrae: pontoSebrae));
            }

            return null;
        }

        private List<DTOTutorial> ObterTutoriaisMensagemGuia(IEnumerable<TrilhaTutorial> tutoriais)
        {
            return tutoriais.Select(x => new DTOTutorial(x.ID, x.Conteudo, x.Ordem, x.Categoria)).ToList();
        }

        private List<DTOMensagemGuia> VerificarPrimeiroAcesso(UsuarioTrilha matricula, enumMomento momento,
            PontoSebrae pontoSebrae = null)
        {
            try
            {
                var mensagens = new List<DTOMensagemGuia>();

                // Refresh básico da matrícula. Foi mal, mas estamos com pressa.
                var trilha = new ManterTrilha().ObterTrilhaPorId(matricula.TrilhaNivel.Trilha.ID);

                if (matricula.ListaVisualizacoesMensagemGuia.All(x => x.MensagemGuia.ID != momento))
                {
                    var mensagem = new ManterMensagemGuia().ObterPorId(momento);

                    var visualizacao = RegistrarVisualizacao(matricula, mensagem);

                    // Caso tenha um tutorial vinculado
                    if (mensagem.Tipo == enumTipoMensagemGuia.Tutorial)
                    {
                        mensagens.Add(new DTOMensagemGuia(visualizacao.ID, ObterTutoriaisMensagemGuia(mensagem.Tutoriais)));
                        return mensagens;
                    }

                    mensagens.Add(new DTOMensagemGuia(visualizacao.ID, mensagem.ObterTexto(trilha, matricula, pontoSebrae: pontoSebrae)));

                    return mensagens;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        private DTOMensagemGuia VerificarConclusaoMetadeSolucoesLoja(UsuarioTrilha matricula, PontoSebrae pontoSebrae)
        {
            try
            {
                var trilha = new ManterTrilha().ObterTrilhaPorId(matricula.TrilhaNivel.Trilha.ID);

                // Refresh básico da loja. Foi mal, mas estamos com pressa.
                pontoSebrae = new ManterPontoSebrae().ObterPorId(pontoSebrae.ID);

                var momento = enumMomento.ConcluirMetadeSolucoesLoja;

                // Refresh básico da matrícula. Foi mal, mas estamos com pressa.
                matricula = new ManterUsuarioTrilha().ObterPorId(matricula.ID);

                var solucoesSebrae = pontoSebrae.ObterItensTrilha();

                if (!new ManterUsuarioTrilhaMensagemGuia().ObterTodos()
                        .Any(
                            x =>
                                x.UsuarioTrilha.ID == matricula.ID &&
                                x.ItemTrilha.Missao.PontoSebrae.ID == pontoSebrae.ID && x.MensagemGuia.ID == momento))
                {
                    var concluidos =
                        solucoesSebrae
                            .Count(
                                x =>
                                    x.Usuario == null && x.Missao.PontoSebrae.TrilhaNivel.ID == matricula.TrilhaNivel.ID && x.PodeExibir() &&
                                    x.ObterStatusParticipacoesItemTrilha(matricula) == enumStatusParticipacaoItemTrilha.Aprovado && x.Ativo.Value);

                    if (concluidos > 0)
                    {
                        var mensagem = new ManterMensagemGuia().ObterPorId(momento);

                        var ids = solucoesSebrae.Select(x => new { x.ID }).Select(x => x.ID).ToList();

                        var solucaoSebraeMetade =
                            solucoesSebrae
                                .FirstOrDefault(x => ids.IndexOf(x.ID) >= concluidos / 2);

                        var visualizacao = RegistrarVisualizacao(matricula, mensagem, null, solucaoSebraeMetade);

                        return new DTOMensagemGuia(visualizacao.ID,
                            mensagem.ObterTexto(trilha, matricula, solucaoSebrae: solucaoSebraeMetade, pontoSebrae: pontoSebrae));
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private DTOMensagemGuia VerificarConclusaoTodasSolucoesLoja(UsuarioTrilha matricula, PontoSebrae pontoSebrae)
        {
            try
            {
                var trilha = new ManterTrilha().ObterTrilhaPorId(matricula.TrilhaNivel.Trilha.ID);

                // Refresh básico da loja. Foi mal, mas estamos com pressa.
                pontoSebrae = new ManterPontoSebrae().ObterPorId(pontoSebrae.ID);

                var momento = enumMomento.ConclusoesTodasSolucoesLoja;

                // Refresh básico da matrícula. Foi mal, mas estamos com pressa.
                matricula = new ManterUsuarioTrilha().ObterPorId(matricula.ID);

                if (
                    !new ManterUsuarioTrilhaMensagemGuia().ObterTodos()
                        .Any(
                            x =>
                                x.UsuarioTrilha.ID == matricula.ID &&
                                x.ItemTrilha.Missao.PontoSebrae.ID == pontoSebrae.ID && x.MensagemGuia.ID == momento))
                {
                    var total =
                        pontoSebrae.ObterItensTrilha()
                            .Count(
                                x =>
                                    x.Usuario == null && x.Missao.PontoSebrae.TrilhaNivel.ID == matricula.TrilhaNivel.ID &&
                                    x.PodeExibir() && x.Ativo.Value);

                    var concluidos =
                        pontoSebrae.ObterItensTrilha()
                            .Count(
                                x =>
                                    x.Usuario == null && x.Missao.PontoSebrae.TrilhaNivel.ID == matricula.TrilhaNivel.ID &&
                                    x.PodeExibir() &&
                                    x.ObterStatusParticipacoesItemTrilha(matricula) == enumStatusParticipacaoItemTrilha.Aprovado && x.Ativo.Value);

                    if (total > 0 && concluidos == total)
                    {
                        var mensagem = new ManterMensagemGuia().ObterPorId(momento);

                        var ultimaSolucaoSebrae =
                            pontoSebrae.ObterItensTrilha()
                                .LastOrDefault(
                                    x =>
                                        x.Missao.PontoSebrae.ID == pontoSebrae.ID &&
                                        x.ListaItemTrilhaParticipacao.Any() &&
                                        x.ListaItemTrilhaParticipacao.FirstOrDefault().UsuarioTrilha.ID == matricula.ID);

                        var visualizacao = RegistrarVisualizacao(matricula, mensagem, null, ultimaSolucaoSebrae);

                        return new DTOMensagemGuia(visualizacao.ID,
                            mensagem.ObterTexto(trilha, matricula, solucaoSebrae: ultimaSolucaoSebrae, pontoSebrae: pontoSebrae));
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        private DTOMensagemGuia VerificarMoedasProvaFinal(UsuarioTrilha matricula)
        {
            try
            {
                var trilha = new ManterTrilha().ObterTrilhaPorId(matricula.TrilhaNivel.Trilha.ID);

                var momento = enumMomento.PossuirMoedasProvaFinal;

                // Refresh básico da matrícula. Foi mal, mas estamos com pressa.
                matricula = new ManterUsuarioTrilha().ObterPorId(matricula.ID);

                var moedas = new ManterUsuarioTrilhaMoedas().Obter(matricula, enumTipoMoeda.Ouro);

                var minimoMoedas = matricula.TrilhaNivel.QuantidadeMoedasProvaFinal;

                var jaVisualizou =
                    new ManterUsuarioTrilhaMensagemGuia().ObterTodos()
                        .Any(x => x.UsuarioTrilha.ID == matricula.ID && x.MensagemGuia.ID == momento);

                if (minimoMoedas > 0 && moedas >= minimoMoedas && !jaVisualizou)
                {
                    var mensagem = new ManterMensagemGuia().ObterPorId(momento);

                    var visualizacao = RegistrarVisualizacao(matricula, mensagem);

                    return new DTOMensagemGuia(visualizacao.ID, mensagem.ObterTexto(trilha, matricula));
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static UsuarioTrilhaMensagemGuia RegistrarVisualizacao(UsuarioTrilha matricula, MensagemGuia mensagem,
            LogLider logLider = null, ItemTrilha itemTrilha = null, Missao missao = null, bool salvarData = true)
        {
            using (var manterTrilhaMensagemGuia = new ManterUsuarioTrilhaMensagemGuia())
            {
                var usuarioTrilhaMensagemGuia = new UsuarioTrilhaMensagemGuia
                {
                    UsuarioTrilha = matricula,
                    MensagemGuia = mensagem,
                    Visualizacao = salvarData ? (DateTime?)DateTime.Now : null,
                    LogLider = logLider,
                    ItemTrilha = itemTrilha,
                    Missao = missao
                };

                return manterTrilhaMensagemGuia.Salvar(usuarioTrilhaMensagemGuia);
            }
        }

        private DTOMensagemGuia VerificarEvolucaoPin(UsuarioTrilha matricula)
        {
            try
            {
                var trilha = new ManterTrilha().ObterTrilhaPorId(matricula.TrilhaNivel.Trilha.ID);

                var manterMatricula = new ManterUsuarioTrilha();
                var momento = enumMomento.EvoluirPin;

                // Refresh básico da matrícula. Foi mal, mas estamos com pressa.
                matricula = manterMatricula.ObterPorId(matricula.ID);

                var moedasOuro = new ManterUsuarioTrilhaMoedas().Obter(matricula, enumTipoMoeda.Ouro);
                var moedasNivel = new ManterTrilhaNivel().ObterTotalMoedasSolucoesSebrae(matricula.TrilhaNivel.ID);

                var tipoTrofeu = matricula.ObterTipoTrofeu(moedasOuro, moedasNivel);

                if (tipoTrofeu > matricula.TipoTrofeu && tipoTrofeu != enumTipoTrofeu.Bronze)
                {
                    matricula.TipoTrofeu = tipoTrofeu;
                    manterMatricula.Salvar(matricula);

                    var mensagem = new ManterMensagemGuia().ObterPorId(momento);

                    var visualizacao = RegistrarVisualizacao(matricula, mensagem);

                    return new DTOMensagemGuia(visualizacao.ID,
                        mensagem.ObterTexto(trilha, matricula, corPin: tipoTrofeu.ToString()));
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterNotificacao : BusinessProcessBase
    {
        private readonly BMNotificacao _notificacaoBm;

        public ManterNotificacao()
        {
            _notificacaoBm = new BMNotificacao();
        }

        public IList<Notificacao> ObterUltimasNotificacoesDoUsuario(int IdUsuario)
        {
            //if (usuario == null) throw new AcademicoException("Usuário. Campo Obrigatório");

            return _notificacaoBm.ObterUltimasNotificacoesDoUsuario(IdUsuario);
        }

        /// <summary>
        /// Obtem as útlimas 100 notificações do usuário trilha
        /// </summary>
        /// <param name="usuarioTrilha"></param>
        /// <returns></returns>
        public IQueryable<Notificacao> ObterNotificacoesNaoVisualizadas(UsuarioTrilha usuarioTrilha)
        {
            return _notificacaoBm.ObterNotificacoesNaoVisualizadas(usuarioTrilha.ID);
        }

        public void PublicarNotificacao(string link, string texto, IQueryable<Usuario> usuarios = null)
        {
            var notificacaoBm = new BMNotificacao();

            var cpfAuditoria = new BMUsuario().ObterUsuarioLogado().CPF;

            // Verificar se está em modo debug para enviar para a thread. Se estiver, dá um output do tempo
            // que leva para enviar cada notificação.
            var debugLocal = ConfigurationManager.AppSettings["debugLocal"];
            var isDebug = !string.IsNullOrEmpty(debugLocal) && !string.IsNullOrEmpty(debugLocal) && debugLocal == "S";

            // Configuração com a URL do SignalR. Tem que ser obtida fora da Thread.
            var confg = new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID((int)enumConfiguracaoSistema.SignalR);

            // Começa as inserções em outra Thread para não congelar a UI do usuário
            var thread =
                new Thread(
                    () => ThreadInsertNotificacoes(usuarios, link, texto, notificacaoBm, cpfAuditoria, isDebug, confg));

            thread.Start();
        }

        public IQueryable<Notificacao> ObterPorFiltro(Usuario usuario, DateTime? dataDeEnvio, int idNotificacaoEnvio)
        {
            var query = new BMNotificacao().ObterTodosIqueryable();

            query = query.Where(x => x.NotificacaoEnvio.ID == idNotificacaoEnvio);

            if (!string.IsNullOrEmpty(usuario.Nome))
                query = query.Where(x => x.Usuario.Nome.Contains(usuario.Nome));

            if (!string.IsNullOrEmpty(usuario.CPF))
                query = query.Where(x => x.Usuario.CPF == usuario.CPF);

            if (!string.IsNullOrEmpty(usuario.Email))
                query = query.Where(x => x.Usuario.Email == usuario.Email);

            if (dataDeEnvio.HasValue)
            {
                query = query.Where(x => x.DataNotificacao.HasValue && x.DataNotificacao.Value.Date == dataDeEnvio.Value.Date);
            }

            return query;
        }

        /// <summary>
        /// Thread de envio de notificações.
        /// </summary>
        /// <param name="usuarios">Usuários que serão notificados.</param>
        /// <param name="link">Link da notificação.</param>
        /// <param name="texto">Texto da notificação.</param>
        /// <param name="notificacaoBm">BM da notificação para operações com o banco dentro da Thread.</param>
        /// <param name="cpfAuditoria">CPF de auditoria.</param>
        /// <param name="isDebug">Caso em modo debug, exibe timers no console pra ajudar no debug.</param>
        /// <param name="config">Configuração com os dados da URL do SignalR.</param>
        private void ThreadInsertNotificacoes(IQueryable<Usuario> usuarios, string link, string texto, BMNotificacao notificacaoBm, string cpfAuditoria, bool isDebug, ConfiguracaoSistema config)
        {
            var count = 0;

            var lstSize = usuarios.Count();

            // Tamanho de inserções por vez do batch.
            const int batchSize = 500;

            var stopWatch = new Stopwatch();

            var batchNotList = new List<Notificacao>();

            foreach (var usuario in usuarios)
            {
                // Zera o cronômetro e começa a marcar novamente
                // Só usa o cronômetro em debug.
                if (isDebug && !stopWatch.IsRunning)
                {
                    stopWatch.Reset();
                    stopWatch.Start();
                }

                var not = new Notificacao
                {
                    DataGeracao = DateTime.Now,
                    Link = link,
                    DataNotificacao = DateTime.Now,
                    TextoNotificacao = texto,
                    Usuario = usuario,
                    Auditoria = new Auditoria(cpfAuditoria)
                };

                batchNotList.Add(not);

                // Faz inserts de x em x (sendo x = batchSize) ou menos na última página ou se só tiver um selecionado
                if (count != 0 && (count % batchSize == 0 || lstSize == count + 1) || lstSize == 1)
                {
                    notificacaoBm.SalvarEmLote(batchNotList, batchSize);

                    batchNotList = new List<Notificacao>();

                    // Mostra o tempo que demorou uma inserção
                    // Só usa o cronômetro em debug.
                    if (isDebug && stopWatch.IsRunning)
                    {
                        stopWatch.Stop();

                        var ts = stopWatch.Elapsed;

                        // Format and display the TimeSpan value. 
                        var elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                            ts.Hours, ts.Minutes, ts.Seconds,
                            ts.Milliseconds / 10);

                        Console.WriteLine("Tempo de " + batchSize + " inserções: " + elapsedTime);
                    }
                }

                count++;

                try
                {
                    // Enviar a notificação em tempo real via SignalR.
                    NotificarUsuario(usuario.CPF, not, config);
                }
                catch (Exception)
                {
                    // ignored.
                    // Um dia pode precisar ter o log dos erros.
                }
            }
        }

        /// <summary>
        /// Adiciona uma notificação para o usuário informado
        /// </summary>
        public void PublicarNotificacao(string link, string texto, int idUsuario, NotificacaoEnvio notificacaoEnvio)
        {
            var usuario = new ManterUsuario().ObterUsuarioPorID(idUsuario);

            var notificacao = new Notificacao
            {
                Link = link,
                TextoNotificacao = texto,
                DataGeracao = DateTime.Now,
                DataNotificacao = DateTime.Now,
                Usuario = usuario,
                NotificacaoEnvio = notificacaoEnvio
            };

            var notificacaoBm = new BMNotificacao();
            notificacaoBm.Salvar(notificacao);

            // Enviar notificação em tempo real.
            NotificarUsuario(usuario.CPF, notificacao);
        }

        /// <summary>
        /// Enviar notificação em tempo real pelo SignalR
        /// </summary>
        /// <param name="cpf">cpf do usuário que será notificado</param>
        /// <param name="notificacao">Objeto de notificação que será enviado</param>
        /// <param name="config">Configuração do sistema que possui a URL do SignalR.</param>
        public static void NotificarUsuario(string cpf, Notificacao notificacao, ConfiguracaoSistema config = null)
        {
            if (notificacao == null || string.IsNullOrEmpty(cpf))
            {
                throw new Exception("Não foi possível enviar a notificação.");
            }

            var confg = config ??
                        new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID(
                            (int)enumConfiguracaoSistema.SignalR);

            if (confg == null)
                throw new Exception("Configuração do servidor de notificação não foi encontrado.");

            // Enviar url e querystring com cpf para conexão
            var hubConnection = new HubConnection(confg.Registro, "cpf=" + cpf);
            var hub = hubConnection.CreateHubProxy("UIHub");

            var dtoNotificacao = ObterDtoNotificacao(notificacao);

            var jsonNotificacao = JsonConvert.SerializeObject(dtoNotificacao);
            hubConnection.Start().Wait();
            hub.Invoke("Enviar", cpf, jsonNotificacao).Wait();
        }

        public static DTONotificacao ObterDtoNotificacao(Notificacao notificacao)
        {
            var dtoNotificacao = new DTONotificacao
            {
                ID = notificacao.ID,
                DataGeracao = notificacao.DataGeracao,
                DataVisualizacao = notificacao.DataVisualizacao,
                Link = notificacao.Link,
                Visualizado = notificacao.Visualizado,
                Usuario = notificacao.Usuario.CPF,
                TextoNotificacao = notificacao.TextoNotificacao
            };

            return dtoNotificacao;
        }

        public void SalvarEmLote(List<Notificacao> notificacoes, int batchSize)
        {
            _notificacaoBm.SalvarEmLote(notificacoes, batchSize);
        }

        public void IncluirNotificacaoTrilha(UsuarioTrilha usuario, string texto, DateTime? data)
        {
            try
            {
                // Só inclui notificação caso exista um usuário vinculado
                if (usuario == null) throw new AcademicoException("Não foi possível incluir a notificação do usuário da trilha");

                _notificacaoBm.IncluirNotificacaoTrilha(usuario, texto, data);
            }
            catch (Exception)
            {
            }
        }
    }
}

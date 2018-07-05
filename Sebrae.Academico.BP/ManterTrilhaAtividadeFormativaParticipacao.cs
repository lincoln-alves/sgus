using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BM.Views;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP
{
    public class ManterTrilhaAtividadeFormativaParticipacao : BusinessProcessBase
    {
        #region "Atributos Privados"

        private BMTrilhaAtividadeFormativaParticipacao bmTrilhaAtividadeFormativaParticipacao;
        private BMViewUsuarioTrilhaAtividadeFormativaParticipacao viewTrilhaAtividadeFormativaParticipacao;

        #endregion

        #region "Construtor"

        public ManterTrilhaAtividadeFormativaParticipacao()
            : base()
        {
            bmTrilhaAtividadeFormativaParticipacao = new BMTrilhaAtividadeFormativaParticipacao();
        }

        #endregion

        #region "Métodos Públicos"

        public void ExcluirAtividadeFormativaParticipacao(int IdTrilhaAtividadeFormativaParticipacao)
        {
            try
            {
                TrilhaAtividadeFormativaParticipacao trilhaAtividadeFormativaParticipacao = null;

                if (IdTrilhaAtividadeFormativaParticipacao > 0)
                {
                    trilhaAtividadeFormativaParticipacao = bmTrilhaAtividadeFormativaParticipacao.ObterPorID(IdTrilhaAtividadeFormativaParticipacao);
                }

                bmTrilhaAtividadeFormativaParticipacao.Excluir(trilhaAtividadeFormativaParticipacao);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }

        }

        public IList<Trilha> ObterTilhas()
        {
            viewTrilhaAtividadeFormativaParticipacao = new BMViewUsuarioTrilhaAtividadeFormativaParticipacao();
            return viewTrilhaAtividadeFormativaParticipacao.ObterTrilhas();
        }

        public IList<TrilhaTopicoTematico> ObterTopicosTematicosPorTrilhaNivel(int idTrilha, int idTrilhaNivel)
        {
            viewTrilhaAtividadeFormativaParticipacao = new BMViewUsuarioTrilhaAtividadeFormativaParticipacao();

            Trilha trilha = new Trilha() { ID = idTrilha };
            TrilhaNivel trilhaNivel = new TrilhaNivel() { ID = idTrilhaNivel };

            return viewTrilhaAtividadeFormativaParticipacao.ObterTopicosTematicos(trilha, trilhaNivel);
        }

        public IList<TrilhaNivel> ObterTrilhasNivelPorTrilha(Trilha trilha)
        {
            viewTrilhaAtividadeFormativaParticipacao = new BMViewUsuarioTrilhaAtividadeFormativaParticipacao();
            return viewTrilhaAtividadeFormativaParticipacao.ObterTrilhasNivelPorTrilha(trilha);
        }

        public TrilhaAtividadeFormativaParticipacao ObterTrilhaAtividadeFormativaParticipacaoPorID(int pId)
        {
            return bmTrilhaAtividadeFormativaParticipacao.ObterPorID(pId);
        }

        public IList<ViewUsuarioTrilhaAtividadeFormativaParticipacao> ObterViewUsuarioTrilhaAtividadeFormativaParticipacaoPorFiltro(ViewUsuarioTrilhaAtividadeFormativaParticipacao pFiltro)
        {
            try
            {
                viewTrilhaAtividadeFormativaParticipacao = new BMViewUsuarioTrilhaAtividadeFormativaParticipacao();
                return viewTrilhaAtividadeFormativaParticipacao.ObterViewUsuarioTrilhaAtividadeFormativaParticipacaoPorFiltro(pFiltro);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void IncluirTrilhaAtividadeFormativaParticipacao(TrilhaAtividadeFormativaParticipacao ptrilhaAtividadeFormativaParticipacao)
        {
            bmTrilhaAtividadeFormativaParticipacao.Incluir(ptrilhaAtividadeFormativaParticipacao);
        }

        public void AlterarTrilhaAtividadeFormativaParticipacao(TrilhaAtividadeFormativaParticipacao pTrilhaAtividadeFormativaParticipacao)
        {
            bmTrilhaAtividadeFormativaParticipacao.Alterar(pTrilhaAtividadeFormativaParticipacao);
        }

        public IList<TrilhaAtividadeFormativaParticipacao> ObterTodos()
        {
            return bmTrilhaAtividadeFormativaParticipacao.ObterTodos();

        }


        public IQueryable<TrilhaAtividadeFormativaParticipacao> ObterSprintsRecentes(bool aprovadas, bool emRevisao,
            bool pendente, bool suspensa)
        {
            return bmTrilhaAtividadeFormativaParticipacao.ObterSprintsRecentes(aprovadas, emRevisao, pendente, suspensa);
        }

        public void CadastrarHistorico(TrilhaAtividadeFormativaParticipacao participacao, bool enviarEmail)
        {
            try
            {
                new BMTrilhaAtividadeFormativaParticipacao().Salvar(participacao);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (enviarEmail)
            {
                try
                {
                    Template template = TemplateUtil.ObterInformacoes(enumTemplate.MensagemMonitorTrilha);
                    string mensagem = template.TextoTemplate;
                    string nomeTrilha = participacao.UsuarioTrilha.TrilhaNivel.Trilha.Nome;
                    string anexo = string.Empty;
                    if (participacao.FileServer != null)
                        anexo = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS).Registro + "/ExibirFileServer.ashx?Identificador=" + participacao.FileServer.NomeDoArquivoNoServidor;
                    mensagem = mensagem.Replace("#NOMETRILHA", nomeTrilha)
                                        .Replace("#TRILHANIVEL", participacao.UsuarioTrilha.TrilhaNivel.Nome)
                                        .Replace("#TEXTOMONITOR", participacao.TextoParticipacao)
                                        .Replace("#TOPICOTEMATICO", participacao.TrilhaTopicoTematico.Nome)
                                        .Replace("#ITEMAVALIADO", participacao.TrilhaTopicoTematico.Nome)
                                        
                                        .Replace("#NOMEMONITOR", participacao.Monitor.Nome)
                                        .Replace("#NOME", participacao.UsuarioTrilha.Usuario.Nome)
                                        .Replace("#EMAILMONITOR", participacao.Monitor.Email)
                                        .Replace("#ANEXO", anexo);

                    string destinatario = participacao.UsuarioTrilha.Usuario.Email;
                    string assunto = "Analisamos seu sprint na trilha " + nomeTrilha;
                    EmailUtil.Instancia.EnviarEmail(destinatario, assunto, mensagem);
                }
                catch
                {
                    throw new EmailException("Erro ao enviar o email");
                }
            }

        }

        public IList<TrilhaAtividadeFormativaParticipacao> ObterParticipacoesForaPrazoMonitor(int idMonitor)
        {
            return bmTrilhaAtividadeFormativaParticipacao.ObterParticipacoesForaPrazoMonitor(idMonitor);
        }


        public void GerarNotificacaoItemTrilha(TrilhaAtividadeFormativaParticipacao participacao)
        {
            string enderecoPortal = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoPortal).Registro;
            Notificacao notificacao = new Notificacao();
            notificacao.Usuario = participacao.UsuarioTrilha.Usuario;
            notificacao.DataGeracao = DateTime.Now;
            notificacao.DataNotificacao = DateTime.Now;
            notificacao.TipoNotificacao = enumTipoNotificacao.Academico;
            notificacao.Visualizado = false;
            notificacao.Link = enderecoPortal + string.Format("trilhas/trilha/{0}/nivel/{1}", participacao.UsuarioTrilha.TrilhaNivel.Trilha.ID.ToString(), participacao.UsuarioTrilha.TrilhaNivel.ID);

            string nomeDoItem = participacao.TrilhaTopicoTematico.Nome;
         

            if (participacao.Autorizado.HasValue && participacao.Autorizado.Value)
            {
                notificacao.TextoNotificacao = string.Format("Parabéns, sua participação no sprint {0} da trilha {1} foi aprovada! Veja aqui o resultado", nomeDoItem, participacao.UsuarioTrilha.TrilhaNivel.Trilha.Nome);
            }
            else
            {
                notificacao.TextoNotificacao = string.Format("Sua participação no sprint {0} da trilha {1} precisa ser ajustada. Por favor, edite sua participação. clique aqu", nomeDoItem, participacao.UsuarioTrilha.TrilhaNivel.Trilha.Nome);
            }
            try
            {
                new BMNotificacao().Salvar(notificacao);
            }
            catch
            {
                throw new EmailException("Erro ao gerar notificação");
            }
        }
        #endregion
    }
}
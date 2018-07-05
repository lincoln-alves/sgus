using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BM.Views;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterItemTrilhaParticipacao : RepositorioBase<ItemTrilhaParticipacao>
    {
        #region "Atributos Privados"

        private readonly BMItemTrilhaParticipacao _bmItemTrilhaParticipacao;
        private BMViewTrilha viewTrilha;

        //private void PreencherInformacoesDeAuditoria(ItemTrilhaParticipacao itemTrilhaParticipacao)
        //{
        //    base.PreencherInformacoesDeAuditoria(itemTrilhaParticipacao);
        //}

        #endregion

        #region "Construtor"

        public ManterItemTrilhaParticipacao()
        {
            _bmItemTrilhaParticipacao = new BMItemTrilhaParticipacao();
        }

        #endregion

        #region "Métodos Públicos"

        public void IncluirItemTrilhaParticipacao(ItemTrilhaParticipacao pItemTrilhaParticipacao)
        {
            if (pItemTrilhaParticipacao != null && pItemTrilhaParticipacao.MatriculaOferta.IsCancelado())
            {
                var currentMatriculaOferta = new ManterMatriculaOferta()
                    .ObterTodosIQueryable()
                    .LastOrDefault(x => x.Usuario.ID == pItemTrilhaParticipacao.MatriculaOferta.Usuario.ID &&
                                        x.Oferta.SolucaoEducacional.ID == pItemTrilhaParticipacao.MatriculaOferta.Oferta.SolucaoEducacional.ID &&
                                        (!x.IsCancelado() && x.StatusMatricula == enumStatusMatricula.Inscrito));

                if (currentMatriculaOferta != null)
                    pItemTrilhaParticipacao.MatriculaOferta = currentMatriculaOferta;
            }

            _bmItemTrilhaParticipacao.ValidarItemTrilhaParticipacaoInformado(pItemTrilhaParticipacao);
            //PreencherInformacoesDeAuditoria(pItemTrilhaParticipacao);
            _bmItemTrilhaParticipacao.Salvar(pItemTrilhaParticipacao);
        }

        public void AlterarItemTrilhaParticipacao(ItemTrilhaParticipacao pItemTrilhaParticipacao)
        {
            _bmItemTrilhaParticipacao.ValidarInstancia(pItemTrilhaParticipacao);
            //PreencherInformacoesDeAuditoria(pItemTrilhaParticipacao);
            _bmItemTrilhaParticipacao.Salvar(pItemTrilhaParticipacao);
        }

        public void ExcluirItemTrilhaParticipacao(int IdItemTrilhaParticipacao)
        {
            try
            {
                ItemTrilhaParticipacao itemTrilhaParticipacao = null;

                if (IdItemTrilhaParticipacao > 0)
                {
                    itemTrilhaParticipacao = _bmItemTrilhaParticipacao.ObterPorId(IdItemTrilhaParticipacao);
                }

                _bmItemTrilhaParticipacao.Excluir(itemTrilhaParticipacao);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public IList<ItemTrilhaParticipacao> ObterTodosItemTrilhaParticipacao()
        {
            return _bmItemTrilhaParticipacao.ObterTodos();
        }

        public IQueryable<ItemTrilhaParticipacao> ObterSolucoesRecentes(bool sugeridas, bool aprovadas, bool emRevisao,
            bool pendente, bool suspensa)
        {
            return _bmItemTrilhaParticipacao.ObterSolucoesRecentes(sugeridas, aprovadas, emRevisao,
                pendente, suspensa);
        }

        public IQueryable<ItemTrilhaParticipacao> ObterSolucoesRecentes2016(bool aprovadas, bool emRevisao,
            bool pendente, bool suspensa)
        {
            return _bmItemTrilhaParticipacao.ObterSolucoesRecentes2016(aprovadas, emRevisao,
                pendente, suspensa);
        }

        public ItemTrilhaParticipacao ObterItemTrilhaParticipacaoPorID(int pId)
        {
            return _bmItemTrilhaParticipacao.ObterItemTrilhaParticipacaoPorID(pId);
        }

        public IList<Trilha> ObterTrilhas()
        {
            viewTrilha = new BMViewTrilha();
            return viewTrilha.ObterTrilhas();
        }

        public IList<TrilhaNivel> ObterTrilhasNivelPorTrilha(Trilha trilha)
        {
            viewTrilha = new BMViewTrilha();
            return viewTrilha.ObterTrilhasNivelPorTrilha(trilha);
        }

        public IList<TrilhaTopicoTematico> ObterTopicosTematicosPorTrilhaNivel(int idTrilha, int idTrilhaNivel)
        {
            viewTrilha = new BMViewTrilha();

            Trilha trilha = new Trilha() { ID = idTrilha };
            TrilhaNivel trilhaNivel = new TrilhaNivel() { ID = idTrilhaNivel };

            return viewTrilha.ObterTopicosTematicos(trilha, trilhaNivel);
        }

        public IList<ItemTrilha> ObterItemsTrilha(ViewTrilha filtro)
        {
            viewTrilha = new BMViewTrilha();
            return viewTrilha.ObterItemsTrilha(filtro);
        }

        public IList<ViewUsuarioItemTrilhaParticipacao> ObterViewUsuarioItemTrilhaParticipacaoPorFiltro(
            ViewUsuarioItemTrilhaParticipacao pFiltro)
        {
            try
            {
                BMViewUsuarioItemTrilhaParticipacao bmViewUsuarioItemTrilhaParticipacao =
                    new BMViewUsuarioItemTrilhaParticipacao();
                return bmViewUsuarioItemTrilhaParticipacao.ObterViewUsuarioItemTrilhaParticipacaoPorFiltro(pFiltro);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UsuarioTrilha ObterUsuarioTrilha(int pUsuarioTrilha)
        {
            return new BMUsuarioTrilha().ObterPorId(pUsuarioTrilha);
        }

        public UsuarioTrilha ObterUsuarioTrilha(int pIdTrilhaNivel, int pIdUsuario)
        {
            return new BMUsuarioTrilha().ObterPorFiltro(new UsuarioTrilha()
            {
                TrilhaNivel = new BMTrilhaNivel().ObterPorID(pIdTrilhaNivel),
                Usuario = new BMUsuario().ObterPorId(pIdUsuario)
            }).Where(x => x.StatusMatricula != enumStatusMatricula.CanceladoAdm &&
                          x.StatusMatricula != enumStatusMatricula.CanceladoAluno)
                .OrderBy(x => x.ID)
                .FirstOrDefault();
        }


        public void CadastrarHistorico(ItemTrilhaParticipacao itemParticipacao, bool enviarEmail)
        {
            try
            {
                new BMItemTrilhaParticipacao().Salvar(itemParticipacao);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (enviarEmail)
            {
                try
                {
                    var template = TemplateUtil.ObterInformacoes(enumTemplate.MensagemMonitorTrilha);
                    var mensagem = template.TextoTemplate;
                    var nomeTrilha = itemParticipacao.UsuarioTrilha.TrilhaNivel.Trilha.Nome;
                    var anexo = string.Empty;
                    if (itemParticipacao.FileServer != null)
                        anexo =
                            ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS).Registro +
                            "/ExibirFileServer.ashx?Identificador=" +
                            itemParticipacao.FileServer.NomeDoArquivoNoServidor;
                    mensagem = mensagem.Replace("#NOMETRILHA", nomeTrilha)
                        .Replace("#TRILHANIVEL", itemParticipacao.UsuarioTrilha.TrilhaNivel.Nome)
                        .Replace("#TEXTOMONITOR", itemParticipacao.TextoParticipacao)
                        .Replace("#TOPICOTEMATICO", itemParticipacao.ItemTrilha.Missao.PontoSebrae.Nome)
                        .Replace("#ITEMAVALIADO", itemParticipacao.ItemTrilha.Nome)

                        .Replace("#NOMEMONITOR", itemParticipacao.Monitor.Nome)
                        .Replace("#EMAILMONITOR", itemParticipacao.Monitor.Email)
                        .Replace("#NOME", itemParticipacao.UsuarioTrilha.Usuario.Nome)
                        .Replace("#ANEXO", anexo);

                    var destinatario = itemParticipacao.UsuarioTrilha.Usuario.Email;
                    var assunto = "Analisamos sua participação na trilha " + nomeTrilha;

                    EmailUtil.Instancia.EnviarEmail(destinatario, assunto, mensagem);
                }
                catch
                {
                    throw new EmailException("Erro ao enviar o email");
                }
            }
        }

        public void GerarNotificacaoItemTrilha(ItemTrilhaParticipacao itemParticipacao)
        {
            var enderecoPortal =
                ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoPortal).Registro;

            var notificacao = new Notificacao
            {
                Usuario = itemParticipacao.UsuarioTrilha.Usuario,
                UsuarioTrilha = itemParticipacao.UsuarioTrilha,
                DataGeracao = DateTime.Now,
                DataNotificacao = DateTime.Now,
                TipoNotificacao = enumTipoNotificacao.Academico,
                Visualizado = false,
                Link = enderecoPortal +
                       string.Format("trilha/mapa/{0}", itemParticipacao.UsuarioTrilha.TrilhaNivel.ID)
            };

            var nomeDoItem = itemParticipacao.ItemTrilha.Nome;

            var nomeTrilha =
                new ManterItemTrilha().ObterItemTrilhaPorID(itemParticipacao.ItemTrilha.ID)
                    .Missao.PontoSebrae.TrilhaNivel.Trilha.Nome;

            if (itemParticipacao.Autorizado.HasValue && itemParticipacao.Autorizado.Value)
            {
                notificacao.TextoNotificacao =
                    string.Format(
                        "Parabéns, sua participação na Solução Sebrae {0} da trilha {1} foi aprovada!",
                        nomeDoItem, nomeTrilha);
            }
            else
            {
                notificacao.TextoNotificacao =
                    string.Format(
                        "Sua participação na Solução Sebrae \"{0}\" na trilha {1} precisa ser ajustada. Por favor, edite sua participação. clique aqui",
                        nomeDoItem, nomeTrilha);
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

        public IList<ItemTrilhaParticipacao> ObterParticipacoesForaPrazoMonitor(int idMonitor)
        {
            return _bmItemTrilhaParticipacao.ObterParticipacoesForaPrazoMonitor(idMonitor);
        }

        public DateTime? ObterDataAvaliacao(ItemTrilhaParticipacao item)
        {
            var participacao = _bmItemTrilhaParticipacao.ObterUltimaParticipacaoMonitor(item);

            return participacao != null && participacao.ID != 0 ? participacao.DataAlteracao : null;
        }

        #endregion


        public void Salvar(ItemTrilhaParticipacao itemTrilhaParticipacao)
        {
            _bmItemTrilhaParticipacao.SomenteSalvar(itemTrilhaParticipacao);
        }

        public void AtualizarStatusParticipacoesTrilhas(MatriculaOferta matriculaOferta)
        {
            matriculaOferta = new ManterMatriculaOferta().ObterMatriculaOfertaPorID(matriculaOferta.ID);

            List<ItemTrilhaParticipacao> participacoes;

            try
            {
                participacoes = matriculaOferta.ListaItemTrilhaParticipacao.ToList();
            }
            catch
            {
                try
                {
                    participacoes = new ManterMatriculaOferta().ObterMatriculaOfertaPorID(matriculaOferta.ID).ListaItemTrilhaParticipacao.ToList();
                }
                catch (Exception)
                {
                    throw new AcademicoException("Erro ao atualizar participação na Solução Sebrae de Trilhas.");
                }
            }


            // Verificar se a matrícula do aluno está vinculada a algum ItemTrilhaParticipacao e altera a participação de acordo.
            if (participacoes.Any())
            {
                var participacao = participacoes.LastOrDefault();

                if (participacao == null)
                    throw new AcademicoException("Item Trilha Participação inválido para a matrícula.");

                var anterior = participacoes.LastOrDefault(x => x.ID != participacao.ID);

                bool? autorizacaoAnterior = (anterior != null) ? anterior.Autorizado : false;

                participacao.Autorizado = IsAutorizado(participacao.MatriculaOferta, participacao.Autorizado);

                if (autorizacaoAnterior == false && participacao.Autorizado == true)
                {
                    participacao.DataAvaliacao = DateTime.Now;
                    participacao.DataEnvio = DateTime.Now;
                }

                Salvar(participacao);

                if (autorizacaoAnterior == false && participacao.Autorizado == true)
                {
                    new ManterTrilhaTopicoTematicoParticipacao().IncluirUltimaParticipacao(participacao.UsuarioTrilha, participacao.ItemTrilha);
                    new ManterUsuarioTrilhaMoedas().Incluir(participacao.UsuarioTrilha, participacao.ItemTrilha, null, 0,
                        participacao.ItemTrilha.Moedas ?? 0);
                }
            }
        }

        public void vinculaMatriculaOferta(MatriculaOferta matriculaOferta)
        {
            matriculaOferta = new ManterMatriculaOferta().ObterMatriculaOfertaPorID(matriculaOferta.ID);

            // VERIFICA SE MATRICULA OFERTA PERTENCE A ALGUM ITEM TRILHA
            if (!matriculaOferta.ListaItemTrilhaParticipacao.Any())
            {
                //BUSCA A LISTA DE TURMAS DA MESMA OFERTA CONTINUA
                var turmasOfertaID = new ManterTurma().ObterTodosIQueryable()
                .Where(x => x.Oferta.ID == matriculaOferta.Oferta.ID)
                .Select(x => x.ID).ToList();

                //BUSCA A LISTA DE MATRICULAS OFERTA DO USUARIO POR TURMA
                var matriculasOfertaID = new ManterMatriculaTurma().ObterTodosIQueryable()
                    .Where(x => turmasOfertaID.Contains(x.Turma.ID) && matriculaOferta.Usuario.ID == x.MatriculaOferta.Usuario.ID)
                    .Select(y => y.MatriculaOferta.ID)
                    .ToList();

                var itemTrilhaParticipacaoUsuario = new ManterItemTrilhaParticipacao().ObterTodosIQueryable()
                    .FirstOrDefault(x => matriculasOfertaID.Contains(x.MatriculaOferta.ID) && x.Autorizado == false);

                if (itemTrilhaParticipacaoUsuario != null)
                {
                    itemTrilhaParticipacaoUsuario.MatriculaOferta = matriculaOferta;
                    Salvar(itemTrilhaParticipacaoUsuario);
                }
            }
        }
        
        /// <summary>
        /// Aprova ou reprova o aluno de acordo com o Status da MatriculaOferta.
        /// </summary>
        /// <param name="matriculaOferta"></param>
        /// <param name="prevAutorizacao"></param>
        /// <returns></returns>
        private static bool? IsAutorizado(MatriculaOferta matriculaOferta, bool? prevAutorizacao)
        {
            if (matriculaOferta.IsAprovado())
                return true;

            if (matriculaOferta.IsCancelado() || matriculaOferta.IsAbandono() ||
                matriculaOferta.IsReprovado() || matriculaOferta.IsDesistencia())
            {
                return false;
            }

            return prevAutorizacao;
        }

        public IEnumerable<ItemTrilhaParticipacao> ObterTodosPorPontoSebrae(int idPontoSebrae)
        {
            return _bmItemTrilhaParticipacao.ObterTodos().Where(x => x.ItemTrilha.Missao.PontoSebrae.ID == idPontoSebrae);
        }

        public IEnumerable<ItemTrilhaParticipacao> ObterParticipacoesPontoSebraeInativo(IEnumerable<ItemTrilhaParticipacao> participacoes)
        {
            participacoes = participacoes.Where(x =>
                x.ItemTrilha.ObterStatusParticipacoesItemTrilha(x.UsuarioTrilha) ==
                enumStatusParticipacaoItemTrilha.Reprovado ||
                x.ItemTrilha.ObterStatusParticipacoesItemTrilha(x.UsuarioTrilha) ==
                enumStatusParticipacaoItemTrilha.EmAndamento ||
                x.ItemTrilha.ObterStatusParticipacoesItemTrilha(x.UsuarioTrilha) ==
                enumStatusParticipacaoItemTrilha.Pendente
            );

            return participacoes;
        }

        public Task ExcluirTodosAsync(IEnumerable<ItemTrilhaParticipacao> participacoes)
        {
            var t = Task.Factory.StartNew(() =>
            {
                foreach (var itemTrilhaParticipacao in participacoes)
                {
                    itemTrilhaParticipacao.ItemTrilha.ListaItemTrilhaParticipacao.Remove(itemTrilhaParticipacao);
                }
             
                ExcluirTodos(participacoes);
            });

            return t;
        }
    }
}
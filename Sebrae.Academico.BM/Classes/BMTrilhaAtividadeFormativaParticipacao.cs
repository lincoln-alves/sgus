using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Dominio.Classes.Views;

namespace Sebrae.Academico.BM.Classes
{
    public class BMTrilhaAtividadeFormativaParticipacao : BusinessManagerBase, IDisposable
    {

        private RepositorioBase<TrilhaAtividadeFormativaParticipacao> repositorio;

        public BMTrilhaAtividadeFormativaParticipacao()
            : base()
        {
            repositorio = new RepositorioBase<TrilhaAtividadeFormativaParticipacao>();
        }

        public void Incluir(TrilhaAtividadeFormativaParticipacao pTrilhaAtividadeFormativaParticipacao)
        {
            ValidarTrilhaAtividadeFormativaParticipacaoInformado(pTrilhaAtividadeFormativaParticipacao);

            //bool existeRegistroCadastrado = VerificarExistenciaAtividadeFormativaParticipacao(pTrilhaAtividadeFormativaParticipacao);

            //if (existeRegistroCadastrado) throw new AcademicoException("Já existe uma participação cadastrada para o usuário informado");

            pTrilhaAtividadeFormativaParticipacao.DataEnvio = DateTime.Now;
            //pTrilhaAtividadeFormativaParticipacao.DataAlteracao = DateTime.Now;

            repositorio.Salvar(pTrilhaAtividadeFormativaParticipacao);
        }

        public void Alterar(TrilhaAtividadeFormativaParticipacao pTrilhaAtividadeFormativaParticipacao)
        {
            ValidarTrilhaAtividadeFormativaParticipacaoInformado(pTrilhaAtividadeFormativaParticipacao);
            repositorio.Salvar(pTrilhaAtividadeFormativaParticipacao);
        }

        public void Salvar(TrilhaAtividadeFormativaParticipacao pTrilhaAtividadeFormativaParticipacao)
        {
            repositorio.Salvar(pTrilhaAtividadeFormativaParticipacao);
        }



        /// <summary>
        /// Validação das informações de uma TrilhaAtividadeFormativaParticipacao
        /// </summary>
        /// <param name="pItemTrilhaParticipacao"></param>
        private void ValidarTrilhaAtividadeFormativaParticipacaoInformado(TrilhaAtividadeFormativaParticipacao ptrilhaAtividadeFormativaParticipacao)
        {
            ValidarInstancia(ptrilhaAtividadeFormativaParticipacao);

            //Usuário Trilha
            if (ptrilhaAtividadeFormativaParticipacao.UsuarioTrilha == null) throw new AcademicoException("Usuário. Campo Obrigatório");

            //Tópico Temático
            if (ptrilhaAtividadeFormativaParticipacao.TrilhaTopicoTematico == null) throw new AcademicoException("Tópico Témático. Campo Obrigatório");

            //Verificar se o Texto de Participação ou o arquivo de uploadfoi informado
            if ((string.IsNullOrWhiteSpace(ptrilhaAtividadeFormativaParticipacao.TextoParticipacao)) &&
                (string.IsNullOrWhiteSpace(ptrilhaAtividadeFormativaParticipacao.FileServer.NomeDoArquivoOriginal)))
            {
                throw new AcademicoException("Informe o texto da Participação ou o Arquivo de Envio. Campo Obrigatório");
            }
        }

        public TrilhaAtividadeFormativaParticipacao ObterPorID(int pId)
        {
            //repositorio.session.Clear();

            var query = repositorio.session.Query<TrilhaAtividadeFormativaParticipacao>();
            query = query.Fetch(x => x.UsuarioTrilha).ThenFetch(x => x.Usuario);

            return query.FirstOrDefault(x => x.ID == pId);
        }


        public List<TrilhaAtividadeFormativaParticipacao> ObterParticipacoesUsuarioTrilha(int idTopicoTematico, int idUsuarioTrilha)
        {
            //repositorio.session.Clear();

            var query = repositorio.session.Query<TrilhaAtividadeFormativaParticipacao>();
            query = query.Fetch(x => x.UsuarioTrilha).ThenFetch(x => x.Usuario);
            query = query.Where(x => x.UsuarioTrilha.ID == idUsuarioTrilha && x.TrilhaTopicoTematico.ID == idTopicoTematico);
            return query.ToList();
        }

        public void Excluir(TrilhaAtividadeFormativaParticipacao pAtividade)
        {
            this.ValidarInstancia(pAtividade);
            repositorio.Excluir(pAtividade);
        }

        public void Inserir(TrilhaAtividadeFormativaParticipacao pAtividade)
        {
            ValidarAtividadeFormativaParticipacaoInformada(pAtividade);
            repositorio.Salvar(pAtividade);
        }

        private void ValidarAtividadeFormativaParticipacaoInformada(TrilhaAtividadeFormativaParticipacao atividade)
        {

            if (atividade.TrilhaTopicoTematico == null)
            {
                throw new AcademicoException("Erro ao gravar a Atividade Formativa. Topico Temático não informado.");
            }

            if (atividade.UsuarioTrilha == null)
            {
                throw new AcademicoException("Erro ao gravar a Atividade Formativa. Usuário da Trilha não informado.");
            }
        }

        public IList<TrilhaAtividadeFormativaParticipacao> ObterTrilhaAtividadeFormativaParticipacaoPorFiltro(TrilhaAtividadeFormativaParticipacao pAtividade)
        {
            IList<TrilhaAtividadeFormativaParticipacao> ListaAtividade = null;
            var query = repositorio.session.Query<TrilhaAtividadeFormativaParticipacao>();
            if (pAtividade != null)
            {

                if (pAtividade.ID != 0)
                {
                    query = query.Where(x => x.ID == pAtividade.ID);
                }

                if (!string.IsNullOrWhiteSpace(pAtividade.TextoParticipacao))
                {
                    query = query.Where(x => x.TextoParticipacao == pAtividade.TextoParticipacao);
                }

                if (pAtividade.TrilhaTopicoTematico != null)
                {
                    query = query.Where(x => x.TrilhaTopicoTematico.ID == pAtividade.TrilhaTopicoTematico.ID);
                }

                if (pAtividade.UsuarioTrilha != null)
                {
                    query = query.Where(x => x.UsuarioTrilha.ID == pAtividade.UsuarioTrilha.ID);
                }

            }

            /* O Fetch faz o inner join / left outer join para buscar os dados do usuario.
               O método CriarSessionFactory da classe NHibernateHelper possui a instrução
               .UseOuterJoin() para indicar ao NHibernate que o left outer join será utilizado 
               nas queries */
            query = query.Fetch(x => x.UsuarioTrilha);
            query = query.Fetch(x => x.TrilhaTopicoTematico);

            ListaAtividade = query.AsParallel().ToList<TrilhaAtividadeFormativaParticipacao>().ToList();

            return ListaAtividade;
        }


        public IList<TrilhaAtividadeFormativaParticipacao> ObterTodos()
        {
            return repositorio.session.Query<TrilhaAtividadeFormativaParticipacao>().ToList();

        }

        public IQueryable<TrilhaAtividadeFormativaParticipacao> ObterSprintsRecentes(bool aprovadas, bool emRevisao,
            bool pendente, bool suspensa)
        {
            // Obtém somente os ids das participações que interessam.
            var parametros = new Dictionary<string, object>
            {
                {"p_SomenteAprovada", aprovadas},
                {"p_SomenteEmRevisao", emRevisao},
                {"p_SomentePendente", pendente},
                {"p_SomenteSuspensas", suspensa}
            };

            var ids =
                ExecutarProcedure<TrilhaAtividadeFormativaParticipacao>(
                    "SP_ID_ATIVIDADE_FORMATIVA_PARTICIPACOES_RECENTES", parametros).Select(x => x.ID).ToList();

            // Busca todas as participações que estão na listagem de ids recebidos.
            return repositorio.ObterTodosIQueryable().Where(x => ids.Contains(x.ID));
        }

        public IList<TrilhaAtividadeFormativaParticipacao> ObterParticipacoesForaPrazoMonitor(int idMonitor)
        {

            return repositorio.session.QueryOver<TrilhaAtividadeFormativaParticipacao>()
                        .Where(x => x.TipoParticipacao == enumTipoParticipacaoTrilha.ParticipacaoTrilheiro
                                    && x.Autorizado == null
                                    && x.DataPrazoAvaliacao != null
                                    && x.DataPrazoAvaliacao <= DateTime.Now)
                        .Inner.JoinQueryOver<UsuarioTrilha>(s => s.UsuarioTrilha)
                        .Inner.JoinQueryOver<TrilhaNivel>(s => s.TrilhaNivel)
                        .Where(x => x.Monitor != null
                            && x.Monitor.ID == idMonitor
                            && x.AvisarMonitor == true)
                        .List();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }
    }
}

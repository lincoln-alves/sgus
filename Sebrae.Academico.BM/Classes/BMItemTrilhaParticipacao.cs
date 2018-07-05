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
    public class BMItemTrilhaParticipacao : BusinessManagerBase, IDisposable
    {
        #region "Atributos Privados"

        private RepositorioBase<ItemTrilhaParticipacao> repositorio = null;

        #endregion

        #region "Construtor"

        public BMItemTrilhaParticipacao()
        {
            repositorio = new RepositorioBase<ItemTrilhaParticipacao>();
        }

        #endregion

        #region "Métodos Públicos"

        public void Salvar(ItemTrilhaParticipacao pItemTrilhaParticipacao)
        {
            ValidarItemTrilhaParticipacaoInformado(pItemTrilhaParticipacao);

            pItemTrilhaParticipacao.DataEnvio = DateTime.Now;
            repositorio.Salvar(pItemTrilhaParticipacao);
        }

        public void SomenteSalvar(ItemTrilhaParticipacao itemTrilhaParticipacao)
        {
            repositorio.Salvar(itemTrilhaParticipacao);
        }

        /// <summary>
        /// Validação das informações de um ItemTrilha Participacao
        /// </summary>
        /// <param name="pItemTrilhaParticipacao"></param>
        public void ValidarItemTrilhaParticipacaoInformado(ItemTrilhaParticipacao pItemTrilhaParticipacao)
        {
            ValidarInstancia(pItemTrilhaParticipacao);

            //Usuário Trilha
            if (pItemTrilhaParticipacao.UsuarioTrilha == null) throw new AcademicoException("Usuário. Campo Obrigatório");

            //Item Trilha
            if (pItemTrilhaParticipacao.ItemTrilha == null) throw new AcademicoException("Item Trilha. Campo Obrigatório");

        }

        ItemTrilha itemTrilha = null;
        FormaAquisicao formaAquisicao = null;
        UsuarioTrilha usuarioTrilha = null;
        Usuario usuario = null;

        public IList<ItemTrilhaParticipacao> ObterParticipacoesForaPrazoMonitor(int idMonitor)
        {

            return repositorio.session.QueryOver<ItemTrilhaParticipacao>()
                        .Where(x => x.TipoParticipacao == enumTipoParticipacaoTrilha.ParticipacaoTrilheiro
                                    && x.Autorizado == null
                                    && x.DataPrazoAvaliacao != null
                                    && x.DataPrazoAvaliacao <= DateTime.Now)
                        .Inner.JoinAlias(s => s.ItemTrilha, () => itemTrilha)
                        .Inner.JoinQueryOver<UsuarioTrilha>(s => s.UsuarioTrilha)
                        .Inner.JoinQueryOver<TrilhaNivel>(s => s.TrilhaNivel)
                        .Where(x => x.Monitor != null
                            && x.Monitor.ID == idMonitor
                            && x.AvisarMonitor == true)
                        .List();
        }

        public IList<ItemTrilhaParticipacao> ObterTodos()
        {
            return repositorio.session.QueryOver<ItemTrilhaParticipacao>()
                                      .Inner.JoinAlias(s => s.ItemTrilha, () => itemTrilha).List();

        }

        public IQueryable<ItemTrilhaParticipacao> ObterSolucoesRecentes(bool sugeridas, bool aprovadas,
            bool emRevisao, bool pendente, bool suspensa)
        {
            // Obtém somente os ids das participações que interessam.
            var parametros = new Dictionary<string, object>
            {
                {"p_SolucaoSugerida", sugeridas},
                {"p_SomenteAprovada", aprovadas},
                {"p_SomenteEmRevisao", emRevisao},
                {"p_SomentePendente", pendente},
                {"p_SomenteSuspensas", suspensa}
            };

            var ids =
                ExecutarProcedure<ItemTrilhaParticipacao>("SP_ID_ITEM_TRILHA_PARTICIPACOES_RECENTES", parametros)
                    .Select(x => x.ID)
                    .ToList();

            // Busca todas as participações que estão na listagem de ids recebidos.
            return repositorio.ObterTodosIQueryable().Where(x => ids.Contains(x.ID));
        }

        public IQueryable<ItemTrilhaParticipacao> ObterSolucoesRecentes2016(bool aprovadas,
            bool emRevisao, bool pendente, bool suspensa)
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
                ExecutarProcedure<ItemTrilhaParticipacao>("SP_ID_ITEM_TRILHA_PARTICIPACOES_RECENTES_2016", parametros)
                    .Select(x => x.ID)
                    .ToList();

            // Busca todas as participações que estão na listagem de ids recebidos.
            return repositorio.ObterTodosIQueryable().Where(x => ids.Contains(x.ID));
        }

        public ItemTrilhaParticipacao ObterPorId(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public void Excluir(ItemTrilhaParticipacao pItemTrilhaParticipacao)
        {
            this.ValidarInstancia(pItemTrilhaParticipacao);
            repositorio.Excluir(pItemTrilhaParticipacao);
        }

        public ItemTrilhaParticipacao ObterItemTrilhaParticipacaoPorID(int pId)
        {
            var query = repositorio.session.Query<ItemTrilhaParticipacao>();
            query = query.Fetch(x => x.UsuarioTrilha).ThenFetch(x => x.Usuario);

            return query.FirstOrDefault(x => x.ID == pId);
        }

        public IList<ItemTrilhaParticipacao> ObterItemTrilhaParticipacaoPorUsuario(int pMatriculaTrilha)
        {
            var query = repositorio.session.Query<ItemTrilhaParticipacao>();
            query = query.Fetch(x => x.UsuarioTrilha).ThenFetch(x => x.Usuario);

            return query.Where(x => x.UsuarioTrilha.ID == pMatriculaTrilha).ToList();
        }

        public List<ItemTrilhaParticipacao> ObterItemTrilhaParticipacaoPorFiltro(ItemTrilhaParticipacao pItem)
        {

            IList<ItemTrilhaParticipacao> ListaItemParticipacao = null;
            var query = repositorio.session.Query<ItemTrilhaParticipacao>();

            if (pItem != null)
            {
                if (pItem.ID != 0)
                {
                    query = query.Where(x => x.ID == pItem.ID);
                }

                if (pItem.ItemTrilha != null)
                {
                    query = query.Where(x => x.ItemTrilha == pItem.ItemTrilha);
                }
                if (pItem.TextoParticipacao != null)
                {
                    query = query.Where(x => x.TextoParticipacao == pItem.TextoParticipacao);
                }

                if (pItem.UsuarioTrilha != null)
                {
                    query = query.Where(x => x.UsuarioTrilha == pItem.UsuarioTrilha);
                }

            }
            query = query.Fetch(x => x.UsuarioTrilha).ThenFetch(x => x.Usuario);

            ListaItemParticipacao = query.AsParallel().ToList<ItemTrilhaParticipacao>().ToList();

            return ListaItemParticipacao.ToList();
        }

        public bool ExisteDependenciaEmItemTrilhaParticipacao(int idItemTrilha)
        {
            if (idItemTrilha <= 0)
                throw new AcademicoException("Erro ao Excluir a Solução Educacional AutoIndicativa. Id inválido");

            bool existeRegistroDependente = false;

            var query = repositorio.session.Query<ItemTrilhaParticipacao>();

            existeRegistroDependente = query.Any(x => x.ItemTrilha.ID == idItemTrilha);

            return existeRegistroDependente;

        }

        public bool ItemAutoIndicativo(int idItemTrilha, string cpfSolicitante)
        {
            if (idItemTrilha <= 0)
                throw new AcademicoException("Erro ao Excluir a Solução Educacional AutoIndicativa. Id inválido");

            bool usuarioAssociado = false;
            var query = repositorio.session.Query<ItemTrilha>();
            usuarioAssociado = query.Any(x => x.Usuario.CPF == cpfSolicitante && x.ID == idItemTrilha);

            return usuarioAssociado;

        }

        public List<ItemTrilhaParticipacao> ObterParticipacoesUsuarioTrilha(int idItemTrilha, int idUsuarioTrilha)
        {
            //repositorio.session.Clear();

            var query = repositorio.session.Query<ItemTrilhaParticipacao>();
            query = query.Fetch(x => x.UsuarioTrilha).ThenFetch(x => x.Usuario);
            query = query.Where(x => x.UsuarioTrilha.ID == idUsuarioTrilha && x.ItemTrilha.ID == idItemTrilha);
            return query.ToList();
        }


        public List<ItemTrilhaParticipacao> ObterParticipacoesUsuarioTrilhaObjetivo(int idItemTrilha, int idObjetivo)
        {
            //repositorio.session.Clear();
            var query = repositorio.session.Query<ItemTrilhaParticipacao>();            
            query = query.Fetch(x => x.UsuarioTrilha).ThenFetch(x => x.Usuario);
            query = query.Where(x => x.ItemTrilha.ID == idItemTrilha && x.ItemTrilha.Objetivo.ID == idObjetivo);
            return query.ToList();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public ItemTrilhaParticipacao ObterUltimaParticipacaoMonitor(ItemTrilhaParticipacao item)
        {
            var query = repositorio.session.Query<ItemTrilhaParticipacao>();

            query =
                query.Where(
                    x =>
                        x.UsuarioTrilha == item.UsuarioTrilha && x.ItemTrilha.ID == item.ItemTrilha.ID &&
                        x.TipoParticipacao == enumTipoParticipacaoTrilha.InteracaoMonitor && x.DataAlteracao.HasValue)
                    .OrderByDescending(x => x.DataAlteracao);

            return query.FirstOrDefault();
        }

        #endregion

        public void LimparSessao()
        {
            repositorio.LimparSessao();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System.Collections;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BM.Classes
{
    public class BMNotificacao : BusinessManagerBase
    {
        private RepositorioBase<Notificacao> repositorio;

        public BMNotificacao()
        {
            repositorio = new RepositorioBase<Notificacao>();
        }

        public Notificacao ObterPorID(int pIdNotificacao)
        {
            return repositorio.ObterPorID(pIdNotificacao);
        }

        public IList<Notificacao> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public IList<Notificacao> ObterPorUsuario(Usuario pUsuario, DateTime? DataGeracao)
        {
            //return repositorio.GetByProperty("Usuario", pUsuario);
            var query = repositorio.session.Query<Notificacao>();
            if (DataGeracao.HasValue)
            {
                query = query.Where(x => x.DataGeracao > DataGeracao);
            }
            return query.Where(x => x.Usuario.ID == pUsuario.ID).ToList<Notificacao>();
        }

        public void Salvar(Notificacao notificacao)
        {
            ValidarNotificacaoInformada(notificacao);
            repositorio.Salvar(notificacao);
        }

        public void SalvarEmLote(List<Notificacao> notificacoes, int batchSize)
        {
            using (var transacao = repositorio.session.BeginTransaction())
            {
                repositorio.session.SetBatchSize(batchSize);

                try
                {
                    foreach (var notificacao in notificacoes)
                    {
                        repositorio.session.Save(notificacao);
                    }
                    transacao.Commit(); //flush to database
                }
                catch (Exception)
                {
                    transacao.Rollback();
                    throw;
                }
            }
        }

        private void ValidarNotificacaoInformada(Notificacao notificacao)
        {
            this.ValidarInstancia(notificacao);

            if (notificacao.Usuario == null && notificacao.UsuarioTrilha == null)
                throw new Exception("Usuário não informado. Campo Obrigatório!");
        }

        public IList<Notificacao> ObterUltimasNotificacoesDoUsuario(int IdUsuario)
        {
            IList<Notificacao> ListaNotificacao = null;
            var query = repositorio.session.Query<Notificacao>();

            ListaNotificacao = query.Where(x => x.Usuario.ID == IdUsuario)
                .Take(100).OrderByDescending(x => x.DataNotificacao).ToList<Notificacao>();

            return ListaNotificacao;
        }

        public IQueryable<Notificacao> ObterTodosIqueryable()
        {
            return repositorio.ObterTodosIQueryable();
        }

        private IList<Notificacao> ObterPorFiltro(int IdUsuario, string URL, bool visualizado)
        {
            var query = repositorio.session.Query<Notificacao>();
            return query.Where(x => x.Usuario.ID == IdUsuario && x.Link != string.Empty &&
                               x.Visualizado == visualizado &&
                               x.Link.Trim().ToUpper() == URL.Trim().ToUpper()).ToList();
        }

        public IList<Notificacao> ObterNotificacoesNaoVisualizadas(int IdUsuario, string URL)
        {
            return this.ObterPorFiltro(IdUsuario, URL, false);
        }

        /// <summary>
        /// Obtem as útlimas 100 notificações do usuário trilha
        /// </summary>
        /// <param name="usuarioTrilha"></param>
        /// <returns></returns>
        public IQueryable<Notificacao> ObterNotificacoesNaoVisualizadas(int usuarioTrilha)
        {
            return repositorio.session.Query<Notificacao>().Where(x => x.UsuarioTrilha.ID == usuarioTrilha && !x.Visualizado).Take(100);
        }

        public void IncluirNotificacaoTrilha(UsuarioTrilha usuario, string texto, DateTime? data)
        {
            var notificacao = new Notificacao
            {
                TextoNotificacao = !string.IsNullOrEmpty(texto) ? texto : null,
                DataGeracao = data != null ? (DateTime) data : DateTime.Now,
                UsuarioTrilha = usuario,
                Visualizado = false
            };

            Salvar(notificacao);
        }
    }
}

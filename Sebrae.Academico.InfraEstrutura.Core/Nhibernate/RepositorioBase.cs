using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using System.Web;
using System.Reflection;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Threading;

namespace Sebrae.Academico.InfraEstrutura.Core.Nhibernate
{
    /// <summary>
    /// A generic repository. Normally this would be a base class of customized entity repositories- not directly exposed.
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <remarks>
    /// All operations are wrapped with <see cref="TransactionRequired"/>. If there is NO open transaction, they open a transaction and commit immediately. If there is an open transaction, nothing is commited, so you should commit at a higher level.
    /// </remarks>
    public class RepositorioBase<V> where V : class
    {
        public ISession session = NHibernateSessionManager.GetCurrentSession();

        private ITransaction _transaction;

        public ITransaction ObterTransacao()
        {
            if (this.session == null) throw new ArgumentNullException("session");

            if (!IsOpenTransaction(_transaction))
            {
                _transaction = session.BeginTransaction();
            }

            return _transaction;
        }

              
        public void RollbackTransaction()
        {
            if (IsOpenTransaction(_transaction))
                _transaction.Rollback();

            _transaction = null;
        }

       

        private static bool IsOpenTransaction(ITransaction transaction)
        {
            return transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack;
        }

        /// <summary>
        /// Gets the Id. Returns null if there is no matching row
        /// </summary>
        public V ObterPorID(object id)
        {
            var retorno = session.Get<V>(id);

            return retorno;
        }


        public void Commit()
        {
            if (session.Transaction != null && session.Transaction.IsActive)
                session.Transaction.Commit();
            else
                session.Flush();
        }

        public void SalvarSemCommit(V entity)
        {
            session.SaveOrUpdate(entity);
        }

        /// <summary>
        /// Saves the specified object within a transaction.
        /// </summary>
        public void Salvar(V entity)
        {
            using (var transacao = session.BeginTransaction())
            {
                if (!IsLogAcessoClass(entity))
                {
                    SalvarLog(entity);
                }

                try
                {
                    session.SaveOrUpdate(entity);
                    transacao.Commit();
                }
                catch (Exception ex)
                {
                    transacao.Rollback();
                    throw ex;
                }
            }

        }
        private bool IsLogAcessoClass (V entity)
        {
            return entity.GetType().Name.Contains("LogAcesso");
        }
        private enumAcaoNaPagina PegarAcao (V entity, enumAcaoNaPagina acao = enumAcaoNaPagina.Criacao)
        {
            var existId = (int)entity
               .GetType()
               .GetProperties()
               .FirstOrDefault(x => x.Name == "ID" || x.Name.EndsWith("ID"))
               .GetValue(entity) > 0;

            var name = entity.GetType().Name;

            if (name == "MatriculaTurma" || name == "MatriculaOferta")
            {
                if (existId)
                {
                    return enumAcaoNaPagina.EdicaoMatricula;
                }

                return enumAcaoNaPagina.IncricaoAluno;
            }

            if (existId && acao != enumAcaoNaPagina.Exclusao)
            {
                return enumAcaoNaPagina.Edicao;
            }

            return acao;
        }

        private void SalvarLog(V entity, enumAcaoNaPagina acao = enumAcaoNaPagina.Criacao)
        {
            if (HttpContext.Current?.Session == null)
            {
                return;
            }

            var paginaAtual = (Pagina)HttpContext.Current.Session["paginaAtual"];
            var queryString = HttpContext.Current.Request.QueryString.ToString();
            var usuarioLogado = (Usuario)HttpContext.Current.Session["usuarioSGUS"];
            var ip = HttpContext.Current.Request.UserHostAddress;

            var log = new LogAcessoPagina
            {
                IDUsuario = usuarioLogado,
                Pagina = paginaAtual,
                QueryString = queryString,
                Acao = PegarAcao(entity, acao),
                DTSolicitacao = DateTime.Now,
                IP = ip
            };

            new RepositorioBase<LogAcessoPagina>().SalvarSemCommit(log);
        }

        public void FazerMerge(object entity)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    session.Merge(entity);
                    transacao.Commit(); //flush to database
                }
                catch (Exception ex)
                {
                    transacao.Rollback();
                    throw ex;
                }
                //finally
                //{
                //    NHibernateSessionManager.CloseSession();
                //}
            }

            if (!IsLogAcessoClass((V)entity))
            {
                SalvarLog((V)entity, enumAcaoNaPagina.Edicao);
            }
        }


        public void Evict(object entity)
        {
            try
            {
                session.Evict(entity);
                session.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Saves the specified object within a transaction.
        /// </summary>
        public void Salvar(IList<V> ListEntity)
        {

            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    foreach (V entity in ListEntity)
                    {
                        SalvarSemCommit(entity); 
                    }

                    transacao.Commit(); //flush to database
                }
                catch (Exception ex)
                {
                    transacao.Rollback();
                    throw ex;
                }
                finally
                {
                    session.Clear();
                }
            }

        }

        /// <summary>
        /// Deletes the specified object within a transaction.
        /// </summary>
        public void Excluir(V entity)
        {

            using (var transacao = session.BeginTransaction())
            {
                if (!IsLogAcessoClass(entity))
                {
                    SalvarLog(entity, enumAcaoNaPagina.Exclusao);
                }

                try
                {
                    session.Delete(entity);
                    session.Flush();
                    transacao.Commit();
                }
                catch (Exception ex)
                {
                    transacao.Rollback();
                    throw ex;
                }
                finally
                {
                    session.Clear();
                }
            }
        }
        //#endregion

        public IQueryable<V> ObterTodosIQueryable()
        {
            var query = session.Query<V>();
            //int qtdReg = query.AsParallel().Count();
            return query;
        }

        public IList<V> ObterTodos()
        {
            var query = session.Query<V>();
            //int qtdReg = query.AsParallel().Count();
            return query.ToList();
        }

        public IList<V> LikeByProperty(string pPropriedade, object pValor)
        {
            ICriteria criteria = session.CreateCriteria(typeof(V));
            return criteria.Add(Expression.Like(pPropriedade, "%" + pValor + "%")).List<V>();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void LimparSessao()
        {
            session.Clear();
        }

        /// <summary>
        /// Deletes the specified object within a transaction.
        /// </summary>
        public void ExcluirTodos(IEnumerable<V> entities)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    foreach (var entity in entities)
                    {
                        session.Delete(entity);
                    }

                    session.Flush();
                    transacao.Commit();
                }
                catch (Exception ex)
                {
                    transacao.Rollback();
                    throw ex;
                }
                finally
                {
                    session.Clear();
                }
            }

        }

        protected void PreencherInformacoesDeAuditoria(EntidadeBasicaPorId entidade, string cpfUsuarioLogado)
        {
            entidade.Auditoria.UsuarioAuditoria = cpfUsuarioLogado;
            entidade.Auditoria.DataAuditoria = DateTime.Now;
        }
    }
}

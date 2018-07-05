using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace Sebrae.Academico.InfraEstrutura.Core.Nhibernate
{
    /// <summary>
    /// A generic repository. Normally this would be a base class of customized entity repositories- not directly exposed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// All operations are wrapped with <see cref="TransactionRequired"/>. If there is NO open transaction, they open a transaction and commit immediately. If there is an open transaction, nothing is commited, so you should commit at a higher level.
    /// </remarks>
    public class RepositorioBaseMdl<T> where T : class
    {
        public ISession session = NHibernateSessionManager.GetCurrentSessionMdl();

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
        public T ObterPorID(object id)
        {
            return session.Get<T>(id);
        }


        public void Commit()
        {
            if (session.Transaction != null && session.Transaction.IsActive)
                session.Transaction.Commit();
            else
                session.Flush();
        }

        public void SalvarSemCommit(T entity)
        {
            session.Save(entity);
        }

        /// <summary>
        /// Saves the specified object within a transaction.
        /// </summary>
        public void Salvar(T entity)
        {

            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    session.SaveOrUpdate(entity);
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
        public void Salvar(IList<T> ListEntity)
        {

            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    foreach (T entity in ListEntity)
                    {
                        session.SaveOrUpdate(entity);
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
        public void Excluir(T entity)
        {

            using (var transacao = session.BeginTransaction())
            {
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


        public IList<T> ObterTodos()
        {
            var query = session.Query<T>();
            //int qtdReg = query.AsParallel().Count();
            return query.ToList();
        }

        public IQueryable<T> ObterTodosQueryble()
        {
            var query = session.Query<T>();
            return query.AsQueryable();
        }

        public IList<T> LikeByProperty(string pPropriedade, object pValor)
        {
            ICriteria criteria = session.CreateCriteria(typeof(T));
            return criteria.Add(Expression.Like(pPropriedade, "%" + pValor + "%")).List<T>();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void LimparSessao()
        {
            session.Clear();
        }

    }
}

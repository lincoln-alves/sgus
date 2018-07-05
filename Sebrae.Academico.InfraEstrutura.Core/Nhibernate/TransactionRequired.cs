using System;
using NHibernate;

namespace Sebrae.Academico.InfraEstrutura.Core.Nhibernate
{
    /// <summary>
    /// Ensure a code block is transactional.
    /// If the session transaction is not open, create a transaction; otherwise there is an existing transaction so don't do anything.
    /// </summary>
    /// <remarks>
    /// Equivalent to <see cref="System.Transactions.TransactionScope"/> with default <see cref="System.Transactions.TransactionScopeOption"/> value <c>Required</c> (enlist in enclosing transaction or create a new one if it doesn't exist).
    /// </remarks>
    public sealed class TransactionRequired : IDisposable
    {
        private const string TRANSACTIONKEY = "NHIBERNATE.TRANSACTION";
        private ITransaction _transaction;
        private bool _shouldCommit;
        private bool _completed;

        #region Constructor
        public TransactionRequired(ISession session)
        {
            if (session == null) throw new ArgumentNullException("session");
            _transaction = session.Transaction; //equal to Transaction.Current
            if (!IsOpenTransaction(_transaction))
            {
                _transaction = session.BeginTransaction();
                ShouldCommit = true;
            }
        }
        #endregion

        #region NHibernate Transactions

        /// <summary>
        /// Gets or sets a value indicating whether this transaction should commit. If there is an open transaction, by default calling Commit will not do anything- it will leave the transaction open.
        /// </summary>
        /// <value><c>true</c> if should commit; otherwise, <c>false</c>.</value>
        public bool ShouldCommit
        {
            get { return _shouldCommit; }
            set { _shouldCommit = value; }
        }

        public void Commit()
        {
            if (!ShouldCommit) return;

            if (_completed)
                throw new InvalidOperationException("The current transaction is already committed. You should dispose the transaction.");

            _completed = true;

            try
            {
                if (IsOpenTransaction(_transaction))
                {
                    _transaction.Commit();
                    _transaction = null;
                }
            }
            catch (HibernateException)
            {
                RollbackTransaction();
                throw;
            }
        }

        public void RollbackTransaction()
        {
            if (!ShouldCommit) return;
            _completed = true;

            if (IsOpenTransaction(_transaction))
                _transaction.Rollback();
            _transaction = null;
        }

        private static bool IsOpenTransaction(ITransaction transaction)
        {
            return transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack;
        }

        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            if (!ShouldCommit) return;
            RollbackTransaction();
        }

        #endregion
    }
}

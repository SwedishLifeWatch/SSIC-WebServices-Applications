using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class used to handle a virtual transaction.
    /// Make sure to always end the transaction.
    /// The transaction can be ended with a call to
    /// Commit(), Rollback() or Dispose().
    /// <example> Example pseudo code:
    /// <code>
    ///     using (ITransaction transaction = userContext.StartTransaction())
    ///     {
    ///         // Update data.
    ///         
    ///         transaction.Commit();
    ///     }
    /// </code>
    /// </example>
    /// </summary>
     [Serializable]
    sealed public class Transaction : ITransaction
    {
        private Boolean _isTransactionActive;
        private DataSourceInformation _dataSourceInformation;
        private DateTime _start;
        private Int32 _timeout;
        private IUserContext _userContext;

        /// <summary>
        /// Create a Transaction instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        public Transaction(IUserContext userContext)
            : this(userContext, Settings.Default.DefaultTransactionTimeout)
        {
        }

        /// <summary>
        /// Create a Transaction instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="timeout">
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </param>
        public Transaction(IUserContext userContext,
                           Int32 timeout)
        {
            if (userContext.Transaction.IsNotNull())
            {
                throw new ApplicationException("Transaction is already started");
            }

            _dataSourceInformation = new DataSourceInformation();
            _start = DateTime.Now;
            _timeout = timeout;
            _userContext = userContext;
            _isTransactionActive = true;
            _userContext.Transaction = this;
            _userContext.Transactions = new TransactionList();
        }

        /// <summary>
        /// Get information about data source.
        /// </summary>
        public IDataSourceInformation DataSourceInformation
        {
            get
            {
                return _dataSourceInformation;
            }
        }

        /// <summary>
        /// Get information about when transaction started.
        /// </summary>
        public DateTime Started
        {
            get { return _start; }
        }

        /// <summary>
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </summary>
        public Int32 Timeout
        {
            get { return _timeout; }
        }

        /// <summary>
        /// Commit the transaction.
        /// </summary>
        public void Commit()
        {
            if (_isTransactionActive)
            {
                this.CheckTransactionTimeout();
                try
                {
                    _isTransactionActive = false;
                    if (_userContext.Transactions.IsNotEmpty())
                    {
                        foreach (ITransaction transaction in _userContext.Transactions)
                        {
                            transaction.Commit();
                        }
                    }
                }
                finally
                {
                    _userContext.Transaction = null;
                    _userContext.Transactions = null;
                }
            }
        }

        /// <summary>
        /// Implementation of the IDisposable interface.
        /// Abort transaction if it has not been commited.
        /// </summary>
        public void Dispose()
        {
            if (_isTransactionActive)
            {
                try
                {
                    _isTransactionActive = false;
                    if (_userContext.Transactions.IsNotEmpty())
                    {
                        foreach (ITransaction transaction in _userContext.Transactions)
                        {
                            transaction.Rollback();
                        }
                    }
                }
                finally
                {
                    _userContext.Transaction = null;
                    _userContext.Transactions = null;
                }
            }
        }

        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        public void Rollback()
        {
            Dispose();
        }
    }
}

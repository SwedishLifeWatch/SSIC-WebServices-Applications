using System;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client
{
    /// <summary>
    /// Class used to handle transaction that works
    /// over the web service interface.
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
    public class WebTransaction : ITransaction
    {
        private Boolean _isTransactionActive;
        private DateTime _start;
        private Int32 _timeout;
        private ITransactionService _service;
        private IUserContext _userContext;

        /// <summary>
        /// Create a WebTransaction instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="service">Web service with transaction handling.</param>
        public WebTransaction(IUserContext userContext,
                              ITransactionService service)
            : this(userContext, service, Settings.Default.DefaultTransactionTimeout)
        {
        }

        /// <summary>
        /// Create a WebTransaction instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="service">Web service with transaction handling.</param>
        /// <param name="timeout">
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </param>
        public WebTransaction(IUserContext userContext,
                              ITransactionService service,
                              Int32 timeout)
        {
            _start = DateTime.Now;
            _service = service;
            _timeout = timeout;
            _userContext = userContext;
            _service.StartTransaction(_userContext, timeout);
            _isTransactionActive = true;
            _userContext.Transaction = this;
        }

        /// <summary>
        /// Get information about data source.
        /// </summary>
        public IDataSourceInformation DataSourceInformation
        {
            get
            {
                return _service.GetDataSourceInformation();
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
                _service.CommitTransaction(_userContext);
                _isTransactionActive = false;
                _userContext.Transaction = null;
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
                _service.RollbackTransaction(_userContext);
                _isTransactionActive = false;
                _userContext.Transaction = null;
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

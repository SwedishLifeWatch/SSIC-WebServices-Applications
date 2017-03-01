using System;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Proxy
{
    /// <summary>
    /// Class used to handle transaction that works
    /// over the web service interface.
    /// Make sure to always end the transaction.
    /// The transaction can be ended with a call to
    /// Commit(), Rollback() or Dispose().
    /// <example> Example pseudo code:
    /// <code>
    ///     using (IWebServiceTransaction transaction = new WebServiceTransaction(clientInformation, proxy))
    ///     {
    ///         // Update data.
    ///         
    ///         transaction.Commit();
    ///     }
    /// </code>
    /// </example>
    /// </summary>
    public sealed class WebServiceTransaction : IWebServiceTransaction
    {
        private Boolean _isTransactionActive;
        private DateTime _start;
        private Int32 _timeout;
        private ITransactionProxy _proxy;
        private WebClientInformation _clientInformation;

        /// <summary>
        /// Create a WebServiceTransaction instance.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="proxy">Web service proxy with transaction handling.</param>
        public WebServiceTransaction(WebClientInformation clientInformation,
                                     ITransactionProxy proxy)
            : this(clientInformation, proxy, Settings.Default.DefaultTransactionTimeout)
        {
        }

        /// <summary>
        /// Create a WebServiceTransaction instance.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="proxy">Web service proxy with transaction handling.</param>
        /// <param name="timeout">
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </param>
        public WebServiceTransaction(WebClientInformation clientInformation,
                                     ITransactionProxy proxy,
                                     Int32 timeout)
        {
            _start = DateTime.Now;
            _proxy = proxy;
            _timeout = timeout;
            _clientInformation = clientInformation;
            _proxy.StartTransaction(_clientInformation, timeout);
            _isTransactionActive = true;
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
                _proxy.CommitTransaction(_clientInformation);
                _isTransactionActive = false;
            }
        }

        /// <summary>
        /// Implementation of the IDisposable interface.
        /// Abort transaction if it has not been committed.
        /// </summary>
        public void Dispose()
        {
            if (_isTransactionActive)
            {
                _proxy.RollbackTransaction(_clientInformation);
                _isTransactionActive = false;
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

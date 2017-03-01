using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// Class used to handle transaction that works
    /// over the web service interface.
    /// Make sure to always end the transaction.
    /// The transaction can be ended with a call to
    /// Commit() or Dispose().
    /// <example> Example pseudo code:
    /// <code>
    ///     using (WebTransaction transaction = new WebTransaction())
    ///     {
    ///         // Update data in web server.
    ///         
    ///         transaction.Commit();
    ///     }
    /// </code>
    /// </example>
    /// </summary>
    sealed public class WebTransaction : IDisposable
    {
        private const Int32 DEFAULT_TRANSACTION_TIMEOUT = 30;

        private Boolean _isTransactionActive;

        /// <summary>
        /// Create a WebTransaction instance.
        /// </summary>
        public WebTransaction()
            : this(DEFAULT_TRANSACTION_TIMEOUT)
        {
        }

        /// <summary>
        /// Create a WebTransaction instance.
        /// </summary>
        /// <param name="timeout">
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </param>
        public WebTransaction(Int32 timeout)
        {
            WebServiceClient.StartTransaction(timeout);
            _isTransactionActive = true;
        }

        /// <summary>
        /// Commit the transaction.
        /// </summary>
        public void Commit()
        {
            if (_isTransactionActive)
            {
                WebServiceClient.CommitTransaction();
                _isTransactionActive = false;
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
                WebServiceClient.RollbackTransaction();
                _isTransactionActive = false;
            }
        }
    }
}

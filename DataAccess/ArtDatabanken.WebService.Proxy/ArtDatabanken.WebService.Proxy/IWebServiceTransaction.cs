using System;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Proxy
{
    /// <summary>
    /// Interface used to handle transaction that works
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
    public interface IWebServiceTransaction : IDisposable
    {
        /// <summary>
        /// Get information about when transaction started.
        /// </summary>
        DateTime Started
        { get; }

        /// <summary>
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </summary>
        Int32 Timeout
        { get; }

        /// <summary>
        /// Commit the transaction.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        void Rollback();
    }
}


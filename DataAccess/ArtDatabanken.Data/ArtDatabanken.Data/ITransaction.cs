using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Interface used to handle a transaction.
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
    public interface ITransaction : IDisposable
    {
        /// <summary>
        /// Get information about data source.
        /// </summary>
        IDataSourceInformation DataSourceInformation
        { get; }

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

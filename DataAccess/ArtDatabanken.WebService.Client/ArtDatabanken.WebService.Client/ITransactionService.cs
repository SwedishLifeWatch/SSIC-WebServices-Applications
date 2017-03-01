using System;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Client
{
    /// <summary>
    /// Interface to generic transaction handling in web services.
    /// </summary>
    public interface ITransactionService
    {
        /// <summary>
        /// Commit a transaction.
        /// </summary>
        /// <param name="userContext">User context.</param>
        void CommitTransaction(IUserContext userContext);

        /// <summary>
        /// Get information about data source.
        /// </summary>
        /// <returns>Information about data source.</returns>
        IDataSourceInformation GetDataSourceInformation();

        /// <summary>
        /// Rollback a transaction.
        /// </summary>
        /// <param name="userContext">User context.</param>
        void RollbackTransaction(IUserContext userContext);

        /// <summary>
        /// Start a transaction.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="timeout">
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </param>
        void StartTransaction(IUserContext userContext, Int32 timeout);
    }
}

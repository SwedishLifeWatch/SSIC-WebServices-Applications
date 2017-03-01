using System;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Proxy
{
    /// <summary>
    /// Interface to generic transation handling in web service proxies.
    /// </summary>
    public interface ITransactionProxy
    {
        /// <summary>
        /// Commit a transaction.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        void CommitTransaction(WebClientInformation clientInformation);

        /// <summary>
        /// Rollback a transaction.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        void RollbackTransaction(WebClientInformation clientInformation);

        /// <summary>
        /// Start a transaction.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="timeout">
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </param>
        void StartTransaction(WebClientInformation clientInformation,
                              Int32 timeout);
    }
}


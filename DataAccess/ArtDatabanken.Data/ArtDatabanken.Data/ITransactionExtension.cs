using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Contains extension methods to the ITransaction interface.
    /// </summary>
    public static class ITransactionExtension
    {
        /// <summary>
        /// Check if transaction has timeout.
        /// </summary>
        /// <param name='transaction'>Transaction.</param>
        /// <exception cref="Exception">Thrown if transaction has timeout.</exception>
        public static void CheckTransactionTimeout(this ITransaction transaction)
        {
            if (transaction.IsTimeout())
            {
                throw new Exception("Transaction is no longer active.");
            }
        }

        /// <summary>
        /// Test if transaction has timeout.
        /// </summary>
        /// <param name='transaction'>Transaction.</param>
        /// <returns>True if transaction has timeout.</returns>
        public static Boolean IsTimeout(this ITransaction transaction)
        {
            return DateTime.Now > (transaction.Started.AddSeconds(transaction.Timeout));
        }
    }
}

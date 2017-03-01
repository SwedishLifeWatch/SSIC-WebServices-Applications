using System;
using System.Collections;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about a user who is actively using the data store.
    /// </summary>
    public interface IUserContext
    {
        /// <summary>
        /// Selected role for current active user
        /// </summary>
        IRole CurrentRole
        { get; set; }

        /// <summary>
        /// Authorities for current active user and specified application in login.
        /// </summary>
        RoleList CurrentRoles
        { get; set; }

        /// <summary>
        /// Currently used locale when this user accesses data.
        /// </summary>
        ILocale Locale
        { get; set; }

        /// <summary>
        /// Get repository of contextual data.
        /// </summary>
        Hashtable Properties
        { get; }

        /// <summary>
        /// Information about the virtual transaction.
        /// </summary>
        ITransaction Transaction
        { get; set; }

        /// <summary>
        /// Information about the real transactions.
        /// </summary>
        TransactionList Transactions
        { get; set; }

        /// <summary>
        /// Currently active user.
        /// </summary>
        IUser User
        { get; set; }

        /// <summary>
        /// Clone this user context.
        /// </summary>
        IUserContext Clone();

        /// <summary>
        /// Start transaction.
        /// </summary>
        ITransaction StartTransaction();

        /// <summary>
        /// Start transaction.
        /// </summary>
        /// <param name="timeout">
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </param>
        ITransaction StartTransaction(Int32 timeout);
    }
}

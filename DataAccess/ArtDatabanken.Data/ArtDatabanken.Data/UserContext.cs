using System;
using System.Collections;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about a user who is actively using the data store.
    /// </summary>
    [Serializable]
    public class UserContext : IUserContext
    {
        private Hashtable _properties;

        /// <summary>
        /// Create a UserContext instance.
        /// </summary>
        public UserContext()
        {
            _properties = new Hashtable();
        }

        /// <summary>
        /// Selected role for current active user
        /// </summary>
        public IRole CurrentRole
        { get; set; }

        /// <summary>
        /// Roles and Authorities for current active user and specified application in login.
        /// </summary>
        public RoleList CurrentRoles
        { get; set; }

        /// <summary>
        /// Currently used locale when this user accesses data.
        /// </summary>
        public ILocale Locale
        { get; set; }

        /// <summary>
        /// Get repository of contextual data.
        /// </summary>
        public Hashtable Properties
        {
            get { return _properties; }
        }

        /// <summary>
        /// Information about the virtual transaction.
        /// </summary>
        public ITransaction Transaction
        { get; set; }

        /// <summary>
        /// Information about the real transactions.
        /// </summary>
        public TransactionList Transactions
        { get; set; }

        /// <summary>
        /// Currently active user.
        /// </summary>
        public IUser User
        { get; set; }

        /// <summary>
        /// Clone this user context.
        /// </summary>
        public IUserContext Clone()
        {
            return (IUserContext)MemberwiseClone();
        }

        /// <summary>
        /// Start transaction.
        /// </summary>
        public virtual ITransaction StartTransaction()
        {
            return StartTransaction(Settings.Default.DefaultTransactionTimeout);
        }

        /// <summary>
        /// Start transaction.
        /// </summary>
        /// <param name="timeout">
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </param>
        public virtual ITransaction StartTransaction(Int32 timeout)
        {
            return new Transaction(this, timeout);;
        }
    }
}

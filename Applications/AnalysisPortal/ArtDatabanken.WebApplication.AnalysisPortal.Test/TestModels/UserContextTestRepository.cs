using System;
using System.Collections;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.TestModels
{
    class UserContextTestReository:IUserContext
    {
        private Hashtable _properties;
        public UserContextTestReository()
        {
            _properties = new Hashtable();
        }
        
        public IRole CurrentRole
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public RoleList CurrentRoles
        {
            get { throw new NotImplementedException(); }
            set { return; }
        }

        public ILocale Locale
        {
            get ;
            set ;
        }

        public Hashtable Properties
        {
            get { return _properties; }
        }

        public ITransaction Transaction
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public TransactionList Transactions
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public IUser User
        {
            get ;
            set ;
        }

        /// <summary>
        /// Clone this user context.
        /// </summary>
        public IUserContext Clone()
        {
            return (IUserContext)MemberwiseClone();
        }

        public ITransaction StartTransaction()
        {
            throw new NotImplementedException();
        }

        public ITransaction StartTransaction(int timeout)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;

namespace Dyntaxa.Test.TestModels
{
    class UserContextTest:IUserContext
    {
        private Hashtable _properties;
        public UserContextTest()
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

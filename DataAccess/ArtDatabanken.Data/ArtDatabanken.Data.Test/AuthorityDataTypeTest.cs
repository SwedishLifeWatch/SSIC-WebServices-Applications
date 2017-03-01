using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.Data.Test
{
    [TestClass]
    public class AuthorityDataTypeTest
    {
        AuthorityDataType _authorityDataType;

        public AuthorityDataTypeTest()
        {
            _authorityDataType = null;
        }

        [TestMethod]
        public void Constructor()
        {
            AuthorityDataType authorityDataType;

            authorityDataType = new AuthorityDataType(0,
                                                      "TestName",
                                                      DataContextTest.GetOneDataContext());
            Assert.IsNotNull(authorityDataType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorDataContextNullError()
        {
            AuthorityDataType authorityDataType;
            IDataContext dataContext;

            dataContext = null;
            authorityDataType = new AuthorityDataType(0,
                                                      "TestName",
                                                      dataContext);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorNameEmptyError()
        {
            AuthorityDataType authorityDataType;

            authorityDataType = new AuthorityDataType(0,
                                                      " ",
                                                      DataContextTest.GetOneDataContext());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorNameNullError()
        {
            AuthorityDataType authorityDataType;

            authorityDataType = new AuthorityDataType(0,
                                                      null,
                                                      DataContextTest.GetOneDataContext());
        }

        [TestMethod]
        public void DataContext()
        {
            Assert.IsNotNull(GetAuthorityDataType(true).DataContext);
        }

        private AuthorityDataType GetAuthorityDataType()
        {
            return GetAuthorityDataType(false);
        }

        private AuthorityDataType GetAuthorityDataType(Boolean refresh)
        {
            if (_authorityDataType.IsNull() || refresh)
            {
                _authorityDataType = new AuthorityDataType(0,
                                                           "TestName",
                                                            DataContextTest.GetOneDataContext());
            }
            return _authorityDataType;
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;

            id = GetAuthorityDataType(true).Id;
            Assert.AreEqual(id, GetAuthorityDataType().Id);
        }

        [TestMethod]
        public void Identifier()
        {
            String identifier;

            identifier = AddressTypeId.Billing.ToString();
            GetAuthorityDataType(true).Identifier = identifier;
            Assert.AreEqual(identifier, GetAuthorityDataType().Identifier);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IdentifierEmptyError()
        {
            String identifier;

            identifier = String.Empty;
            GetAuthorityDataType(true).Identifier = identifier;
            Assert.AreEqual(identifier, GetAuthorityDataType().Identifier);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IdentifierNullError()
        {
            String identifier;

            identifier = null;
            GetAuthorityDataType(true).Identifier = identifier;
            Assert.AreEqual(identifier, GetAuthorityDataType().Identifier);
        }

       
    }
}

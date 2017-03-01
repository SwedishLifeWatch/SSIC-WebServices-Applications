using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Summary description for WebAuthorityTest
    /// </summary>
    [TestClass]
    public class WebAuthorityTest
    {
        private WebAuthority _authority;

        public WebAuthorityTest()
        {
            _authority = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebAuthority authority;

            authority = new WebAuthority();
            Assert.IsNotNull(authority);
        }

        private WebAuthority GetAuthority()
        {
            return GetAuthority(false);
        }

        private WebAuthority GetAuthority(Boolean refresh)
        {
            if (_authority.IsNull() || refresh)
            {
                _authority = new WebAuthority();
            }
            return _authority;
        }

        [TestMethod]
        public void Id()
        {
            Int32 id = 100;
            GetAuthority(true).Id = id;
            Assert.AreEqual(id, GetAuthority().Id);
        }

        [TestMethod]
        public void AuthorityIdentity()
        {
            Assert.IsNull(GetAuthority().Identifier);
            String authorityIdentity = "TestIdentity";
            GetAuthority(true).Identifier = authorityIdentity;
            Assert.AreEqual(authorityIdentity, GetAuthority().Identifier);
        }

        [TestMethod]
        public void Name()
        {
            Assert.IsNull(GetAuthority().Name);
            String name = "TestName";
            GetAuthority(true).Name = name;
            Assert.AreEqual(name, GetAuthority().Name);
        }

        [TestMethod]
        public void GUID()
        {
            Assert.IsNull(GetAuthority().GUID);
            String GUID = "TestGUID:1234:artdatabanken.slu.se";
            GetAuthority(true).GUID = GUID;
            Assert.AreEqual(GUID, GetAuthority().GUID);
        }

        [TestMethod]
        public void AdministrationRoleId()
        {
            Int32 administrationRoleId = 99;
            GetAuthority(true).AdministrationRoleId = administrationRoleId;
            Assert.AreEqual(administrationRoleId, GetAuthority().AdministrationRoleId);
        }

        [TestMethod]
        public void Description()
        {
            Assert.IsNull(GetAuthority().Description);
            String value = "DescriptionTest " +
            "DescriptionTest2";
            GetAuthority(true).Description = value;
            Assert.AreEqual(value, GetAuthority().Description);
        }

        [TestMethod]
        public void ContactUserId()
        {
            Boolean readPermission = true;
            GetAuthority(true).ReadPermission = readPermission;
            Assert.AreEqual(readPermission, GetAuthority().ReadPermission);
        }

        [TestMethod]
        public void AuthorityDataType()
        {
            Assert.IsNull(GetAuthority().AuthorityDataType);
            WebAuthorityDataType authorityDataType = new WebAuthorityDataType();
            authorityDataType.Id = 3;
            authorityDataType.Identifier = "AuthorityDataTypeIdentifier";
            GetAuthority(true).AuthorityDataType = authorityDataType;
            Assert.AreEqual(authorityDataType.Id, GetAuthority().AuthorityDataType.Id);
            Assert.AreEqual(authorityDataType.Identifier, GetAuthority().AuthorityDataType.Identifier);
        }

        [TestMethod]
        public void AuthorityType()
        {
            //Assert.IsNull(GetAuthority().AuthorityType);
            //AuthorityType authorityType = ;
            
            //GetAuthority(true).AuthorityDataType = authorityDataType;
            //Assert.AreEqual(authorityDataType.Id, GetAuthority().AuthorityDataType.Id);
            //Assert.AreEqual(authorityDataType.Identifier, GetAuthority().AuthorityDataType.Identifier);
        }



        #region Additional test attributes
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

      
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


    }
}

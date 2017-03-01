using System;
using ArtDatabanken.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Data;

namespace ArtDatabanken.WebService.UserService.Test.Data
{
    [TestClass]
    public class WebPersonExtensionTest : TestBase
    {
        private WebPerson _person;

        public WebPersonExtensionTest()
        {
            _person = null;
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

        public WebPerson GetPerson()
        {
            return GetPerson(false);
        }

        public WebPerson GetPerson(Boolean refresh)
        {
            if (_person.IsNull() || refresh)
            {
                _person = ArtDatabanken.WebService.UserService.Data.UserManager.GetPerson(GetContext(), Settings.Default.TestPersonId);
            }
            return _person;
        }

        [TestMethod]
        public void LoadData()
        {
            WebPerson person;
            using (DataReader dataReader = GetContext().GetUserDatabase().GetPerson(Settings.Default.TestPersonId, Settings.Default.SwedenLocaleId))
            {
                person = new WebPerson();
                Assert.IsTrue(dataReader.Read());
                person.LoadData(dataReader);
                Assert.AreEqual(Settings.Default.TestPersonId, person.Id);
                Assert.AreEqual(Settings.Default.TestEmailAddress, person.EmailAddress);
                Assert.AreEqual(0, person.TaxonNameTypeId);
                Assert.AreEqual(0, person.AdministrationRoleId);
                Assert.IsTrue(person.IsUserIdSpecified);
                Assert.AreEqual(Settings.Default.TestUserId, person.UserId);
                Assert.IsTrue(person.ShowPersonalInformation);
                Assert.IsFalse(person.IsDeathYearSpecified);
                Assert.IsTrue(person.IsBirthYearSpecified);
            }
        }
    }
}

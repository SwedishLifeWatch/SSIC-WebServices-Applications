using System;
using System.Collections.Generic;
using ArtDatabanken.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.UserService.Test.Data
{
    [TestClass]
    public class WebDataExtensionTest : TestBase
    {
        private WebData _data;

        public WebDataExtensionTest()
        {
            _data = null;
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

        [TestMethod]
        public void CheckData()
        {
            GetData(true).CheckData();
            GetData().DataFields = new List<WebDataField>();
            GetData().DataFields.Add(WebDataFieldExtensionTest.GetOneDataField(WebDataType.String));
            GetData().CheckData();
        }

        private WebData GetData()
        {
            return GetData(false);
        }

        private WebData GetData(Boolean refresh)
        {
            if (_data.IsNull() || refresh)
            {
                _data = new WebData();
            }
            return _data;
        }

        [TestMethod]
        public void LoadData()
        {
            using (DataReader dataReader = GetContext().GetDatabase().GetLog(null, null, 10))
            {
                if (dataReader.Read())
                {
                    GetData(true).LoadData(dataReader);
                }
            }
            Assert.IsTrue(GetData().DataFields.IsNotEmpty());
        }
    }
}

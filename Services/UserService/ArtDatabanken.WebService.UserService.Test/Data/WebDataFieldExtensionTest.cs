using System;
using ArtDatabanken.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.UserService.Test.Data
{
    /// <summary>
    /// Test of the class
    /// ArtDatabanken.WebService.Data.WebDataFieldExtension.
    /// </summary>
    [TestClass]
    public class WebDataFieldExtensionTest : TestBase
    {
        private WebDataField dataField;

        public WebDataFieldExtensionTest()
            : base(true, 60)
        {
            dataField = null;
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
            String testName, testValue;

            testName = "Hej";
            GetDataField(true).Name = testName;
            Assert.AreEqual(testName, GetDataField().Name);
            GetDataField().CheckData();
            Assert.AreEqual(testName, GetDataField().Name);

            testName = "'Hej'";
            GetDataField(true).Name = testName;
            Assert.AreEqual(testName, GetDataField().Name);
            GetDataField().CheckData();
            Assert.AreNotEqual(testName, GetDataField().Name);
            Assert.AreEqual("''Hej''", GetDataField().Name);

            testValue = "Hej";
            GetDataField(true).Value = testValue;
            Assert.AreEqual(testValue, GetDataField().Value);
            GetDataField().CheckData();
            Assert.AreEqual(testValue, GetDataField().Value);

            testValue = "'Hej'";
            GetDataField(true).Value = testValue;
            Assert.AreEqual(testValue, GetDataField().Value);
            GetDataField().CheckData();
            Assert.AreNotEqual(testValue, GetDataField().Value);
            Assert.AreEqual("''Hej''", GetDataField().Value);
        }

        private WebDataField GetDataField()
        {
            return GetDataField(false);
        }

        private WebDataField GetDataField(Boolean refresh)
        {
            if (dataField.IsNull() || refresh)
            {
                dataField = GetOneDataField(WebDataType.Int32);
            }
            return dataField;
        }

        public static WebDataField GetOneDataField()
        {
            return GetOneDataField(WebDataType.DateTime);
        }

        public static WebDataField GetOneDataField(WebDataType dataType)
        {
            WebDataField dataField;

            dataField = new WebDataField();
            dataField.Name = "Test";
            dataField.Type = dataType;
            switch (dataType)
            {
                case WebDataType.Boolean:
                    dataField.Value = true.WebToString();
                    break;

                case WebDataType.DateTime:
                    dataField.Value = DateTime.Now.WebToString();
                    break;

                case WebDataType.Float64:
                    dataField.Value = Math.PI.WebToString();
                    break;

                case WebDataType.Int32:
                    dataField.Value = Int32.MaxValue.WebToString();
                    break;

                case WebDataType.Int64:
                    dataField.Value = Int64.MaxValue.WebToString();
                    break;

                case WebDataType.String:
                    dataField.Value = "Testing";
                    break;
            }
            return dataField;
        }

        [TestMethod]
        public void LoadData()
        {
            WebDataField dataField;

            using (DataReader dataReader = GetContext().GetDatabase().GetLog(null, null, 10))
            {
                if (dataReader.Read() && dataReader.NextUnreadColumn())
                {
                    do
                    {
                        dataField = new WebDataField();
                        dataField.LoadData(dataReader);
                        Assert.IsNotNull(dataField);
                        Assert.IsTrue(dataField.Name.IsNotEmpty());
                    }
                    while (dataReader.NextUnreadColumn());
                }
            }
        }
    }
}

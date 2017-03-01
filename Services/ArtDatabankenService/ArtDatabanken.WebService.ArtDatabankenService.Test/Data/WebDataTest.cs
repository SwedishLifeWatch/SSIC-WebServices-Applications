using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebDataTest : TestBase
    {
        private WebTaxon _data;

        public WebDataTest()
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
        public void Constructor()
        {
            WebData data;

            data = new WebData();
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void DataFields()
        {
            List<WebDataField> dataFields;

            // Test null.
            dataFields = null;
            GetData(true).DataFields = dataFields;
            Assert.IsNull(GetData().DataFields);

            // Test no data fields.
            dataFields = new List<WebDataField>();
            GetData().DataFields = dataFields;
            Assert.IsNotNull(GetData().DataFields);
            Assert.IsTrue(GetData().DataFields.IsEmpty());

            // Test one data field.
            dataFields.Add(WebDataFieldTest.GetDataField(GetContext()));
            GetData().DataFields = dataFields;
            Assert.IsNotNull(GetData().DataFields);
            Assert.IsTrue(GetData().DataFields.IsNotEmpty());
            Assert.AreEqual(GetData().DataFields.Count, 1);

            // Test many data fields.
            foreach (WebDataType dataType in Enum.GetValues(typeof(WebDataType)))
            {
                dataFields.Add(WebDataFieldTest.GetDataField(GetContext(), dataType));
            }
            GetData().DataFields = dataFields;
            Assert.IsNotNull(GetData().DataFields);
            Assert.IsTrue(GetData().DataFields.IsNotEmpty());
            Assert.IsTrue(GetData().DataFields.Count > 1);
        }

        private WebTaxon GetData()
        {
            return GetData(false);
        }

        private WebTaxon GetData(Boolean refresh)
        {
            if (_data.IsNull() || refresh)
            {
                _data = TaxonManagerTest.GetOneTaxon(GetContext());
            }
            return _data;
        }

        [TestMethod]
        public void LoadData()
        {
            GetData(true);
            using (DataReader dataReader = DataServer.GetDatabaseUpdate(GetContext()))
            {
                if (dataReader.Read())
                {
                    GetData().LoadData(dataReader);
                }
            }
            Assert.IsNotNull(GetData().DataFields);
            Assert.IsTrue(GetData().DataFields.IsNotEmpty());
            Assert.IsTrue(GetData().DataFields.Count > 1);
        }
    }
}

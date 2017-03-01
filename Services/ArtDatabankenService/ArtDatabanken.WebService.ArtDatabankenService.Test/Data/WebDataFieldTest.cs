using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebDataFieldTest : TestBase
    {
        private WebDataField dataField;

        public WebDataFieldTest()
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
        public void Constructor()
        {
            WebDataField dataField;

            foreach (WebDataType dataType in Enum.GetValues(typeof(WebDataType)))
            {
                dataField = GetDataField(GetContext(), dataType);
                Assert.IsNotNull(dataField);
                Assert.AreEqual(dataType, dataField.Type);
            }
        }

        private WebDataField GetDataField()
        {
            return GetDataField(false);
        }

        private WebDataField GetDataField(Boolean refresh)
        {
            if (dataField.IsNull() || refresh)
            {
                dataField = GetDataField(GetContext());
            }
            return dataField;
        }

        public static WebDataField GetDataField(WebServiceContext context)
        {
            return GetDataField(context, WebDataType.DateTime);
        }

        public static WebDataField GetDataField(WebServiceContext context,
                                                WebDataType dataType)
        {
            WebDataField dataField = null;

            switch (dataType)
            {
                case WebDataType.Boolean:
                    using (DataReader dataReader = DataServer.GetFactors(context))
                    {
                        if (dataReader.Read())
                        {
                            for (Int32 index = 0; index < 11; index++)
                            {
                                if (dataReader.NextUnreadColumn())
                                {
                                    dataField = new WebDataField(dataReader);
                                }
                            }
                            if (dataReader.NextUnreadColumn())
                            {
                                dataField = new WebDataField(dataReader);
                            }
                        }
                    }
                    break;
                case WebDataType.DateTime:
                    using (DataReader dataReader = DataServer.GetDatabaseUpdate(context))
                    {
                        if (dataReader.Read())
                        {
                            if (dataReader.NextUnreadColumn())
                            {
                                dataField = new WebDataField(dataReader);
                            }
                        }
                    }
                    break;

                case WebDataType.Float:
                    using (DataReader dataReader = DataServer.GetFactors(context))
                    {
                        // No float in database use Int32 instead
                        // and convert to float.
                        if (dataReader.Read())
                        {
                            if (dataReader.NextUnreadColumn())
                            {
                                dataField = new WebDataField(dataReader);
                                dataField.Type = WebDataType.Float;
                            }
                        }
                    }
                    break;

                case WebDataType.Int32:
                    using (DataReader dataReader = DataServer.GetFactors(context))
                    {
                        if (dataReader.Read())
                        {
                            if (dataReader.NextUnreadColumn())
                            {
                                dataField = new WebDataField(dataReader);
                            }
                        }
                    }
                    break;

                case WebDataType.Int64:
                    using (DataReader dataReader = DataServer.GetFactors(context))
                    {
                        // No Int64 in database use Int32 instead
                        // and convert to Int64.
                        if (dataReader.Read())
                        {
                            if (dataReader.NextUnreadColumn())
                            {
                                dataField = new WebDataField(dataReader);
                                dataField.Type = WebDataType.Int64;
                            }
                        }
                    }
                    break;

                case WebDataType.String:
                    using (DataReader dataReader = DataServer.GetFactors(context))
                    {
                        if (dataReader.Read())
                        {
                            if (dataReader.NextUnreadColumn())
                            {
                                dataField = new WebDataField(dataReader);
                            }
                            if (dataReader.NextUnreadColumn())
                            {
                                dataField = new WebDataField(dataReader);
                            }
                            if (dataReader.NextUnreadColumn())
                            {
                                dataField = new WebDataField(dataReader);
                            }
                            if (dataReader.NextUnreadColumn())
                            {
                                dataField = new WebDataField(dataReader);
                            }
                        }
                    }
                    break;
            }
            return dataField;
        }

        [TestMethod]
        public void HasValue()
        {
            String value;

            value = null;
            GetDataField(true).Value = value;
            Assert.IsFalse(GetDataField().IsValueSpecified);
            GetDataField().IsValueSpecified = true;
            Assert.IsFalse(GetDataField().IsValueSpecified);

            value = "Something";
            GetDataField().Value = value;
            Assert.IsTrue(GetDataField().IsValueSpecified);
            GetDataField().IsValueSpecified = false;
            Assert.IsTrue(GetDataField().IsValueSpecified);
        }

        [TestMethod]
        public void Name()
        {
            String name;

            name = null;
            GetDataField(true).Name = name;
            Assert.IsNull(GetDataField().Name);
            name = "";
            GetDataField().Name = name;
            Assert.AreEqual(GetDataField().Name, name);
            name = "Test data field";
            GetDataField().Name = name;
            Assert.AreEqual(GetDataField().Name, name);
        }

        [TestMethod]
        public void Type()
        {
            foreach (WebDataType dataType in Enum.GetValues(typeof(WebDataType)))
            {
                GetDataField().Type = dataType;
                Assert.AreEqual(dataType, GetDataField().Type);
            }
        }

        [TestMethod]
        public void Value()
        {
            String value;

            value = null;
            GetDataField(true).Value = value;
            Assert.IsNull(GetDataField().Value);
            value = "";
            GetDataField().Value = value;
            Assert.AreEqual(GetDataField().Value, value);
            value = "Test data field";
            GetDataField().Value = value;
            Assert.AreEqual(GetDataField().Value, value);
        }
    }
}

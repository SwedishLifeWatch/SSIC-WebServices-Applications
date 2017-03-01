using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Data.Test
{
    [TestClass]
    public class WebDataTest
    {
        private WebData _data;

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

       
        #endregion

        [TestMethod]
        public void Constructor()
        {
            WebData data;

            data = new WebData();
            Assert.IsNotNull(data);
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
                _data.DataFields = WebDataFieldListExtensionTest.GetOneDataFieldList();
            }
            return _data;
        }

        [TestMethod]
        public void DataFields()
        {
            List<WebDataField> dataFields;

            // Test no data fields.
            GetData(true).DataFields = null;
            Assert.IsNull(GetData().DataFields);

            // Test some data fields.
            dataFields = WebDataFieldListExtensionTest.GetOneDataFieldList();
            GetData().DataFields = dataFields;
            Assert.IsTrue(GetData().DataFields.IsNotEmpty());
            Assert.AreEqual(dataFields.Count, GetData().DataFields.Count);
        }
    }
}

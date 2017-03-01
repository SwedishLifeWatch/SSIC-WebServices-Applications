using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class DataFieldListTest : TestBase
    {
        private Data.ArtDatabankenService.DataFieldList _dataFields;

        public DataFieldListTest()
        {
            _dataFields = null;
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
            Data.ArtDatabankenService.DataFieldList dataFields;

            dataFields = GetDataFieldList(true);
            Assert.IsNotNull(dataFields);
        }

        [TestMethod]
        public void GetBoolean()
        {
            Boolean value;
            Data.ArtDatabankenService.DataFieldList dataFields;
            WebDataField webDataField;
            List<WebDataField> webDataFields;

            value = false;
            webDataField = new WebDataField();
            webDataField.Name = "Test";
            webDataField.Type = WebDataType.Boolean;
#if DATA_SPECIFIED_EXISTS
            webDataField.TypeSpecified = true;
#endif
            webDataField.Value = value.ToString();
            webDataFields = new List<WebDataField>();
            webDataFields.Add(webDataField);
            dataFields = new Data.ArtDatabankenService.DataFieldList(webDataFields);
            Assert.AreEqual(dataFields.GetBoolean("Test"), value);

            value = true;
            webDataField = new WebDataField();
            webDataField.Name = "Test";
            webDataField.Type = WebDataType.Boolean;
#if DATA_SPECIFIED_EXISTS
            webDataField.TypeSpecified = true;
#endif
            webDataField.Value = value.ToString();
            webDataFields = new List<WebDataField>();
            webDataFields.Add(webDataField);
            dataFields = new Data.ArtDatabankenService.DataFieldList(webDataFields);
            Assert.AreEqual(dataFields.GetBoolean("Test"), value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetBooleanDataFieldNameError()
        {
            Boolean value;

            value = GetDataFieldList().GetBoolean("No data field");
        }

        private Data.ArtDatabankenService.DataFieldList GetDataFieldList()
        {
            return GetDataFieldList(false);
        }

        private Data.ArtDatabankenService.DataFieldList GetDataFieldList(Boolean refresh)
        {
            if (_dataFields.IsNull() || refresh)
            {
                WebTaxon taxon;

                taxon = WebServiceClient.GetTaxon(BEAR_TAXON_ID, TaxonInformationType.PrintObs);
                _dataFields = new Data.ArtDatabankenService.DataFieldList(taxon.DataFields);
            }
            return _dataFields;
        }

        [TestMethod]
        public void GetInt32()
        {
            Data.ArtDatabankenService.DataFieldList dataFields;
            Int32 value;
            WebDataField webDataField;
            List<WebDataField> webDataFields;

            value = 27343;
            webDataField = new WebDataField();
            webDataField.Name = "Test";
            webDataField.Type = WebDataType.Int32;
#if DATA_SPECIFIED_EXISTS
            webDataField.TypeSpecified = true;
#endif
            webDataField.Value = value.ToString();
            webDataFields = new List<WebDataField>();
            webDataFields.Add(webDataField);
            dataFields = new Data.ArtDatabankenService.DataFieldList(webDataFields);

            Assert.AreEqual(dataFields.GetInt32("Test"), value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetInt32DataFieldNameError()
        {
            Int32 id;

            id = GetDataFieldList().GetInt32("No data field");
        }

        [TestMethod]
        public void GetInt64()
        {
            Data.ArtDatabankenService.DataFieldList dataFields;
            Int64 value;
            WebDataField webDataField;
            List<WebDataField> webDataFields;

            value = 27343654645654645;
            webDataField = new WebDataField();
            webDataField.Name = "Test";
            webDataField.Type = WebDataType.Int64;
#if DATA_SPECIFIED_EXISTS
            webDataField.TypeSpecified = true;
#endif
            webDataField.Value = value.ToString();
            webDataFields = new List<WebDataField>();
            webDataFields.Add(webDataField);
            dataFields = new Data.ArtDatabankenService.DataFieldList(webDataFields);

            Assert.AreEqual(dataFields.GetInt64("Test"), value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetInt64DataFieldNameError()
        {
            Int64 id;

            id = GetDataFieldList().GetInt64("No data field");
        }

        [TestMethod]
        public void GetString()
        {
            String phylum;

            phylum = GetDataFieldList(true).GetString(TaxonPrintObs.PHYLUM_DATA_FIELD);
            Assert.IsNotNull(phylum);
            Assert.IsTrue(phylum.IsNotEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetStringDataFieldNameError()
        {
            String phylum;

            phylum = GetDataFieldList().GetString("No data field");
        }
    }
}

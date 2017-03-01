using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Data.Test
{
    [TestClass]
    public class WebDataFieldListExtensionTest
    {
        private const String BOOLEAN_DATA_FIELD_NAME = "BooleanDataField";
        private const String DATE_TIME_DATA_FIELD_NAME = "DateTimeDataField";
        private const String FLOAT64_DATA_FIELD_NAME = "Float64DataField";
        private const String INT32_DATA_FIELD_NAME = "Int32DataField";
        private const String INT64_DATA_FIELD_NAME = "Int64DataField";
        private const String STRING_DATA_FIELD_NAME = "StringDataField";

        private List<WebDataField> _dataFields;

        public WebDataFieldListExtensionTest()
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
        public void GetBoolean()
        {
            bool value = GetDataFields(true).GetBoolean(BOOLEAN_DATA_FIELD_NAME);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetBooleanDataTypeError()
        {
            Boolean value;

            value = GetDataFields(true).GetBoolean(INT32_DATA_FIELD_NAME);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetBooleanFieldNameError()
        {
            Boolean value;

            value = GetDataFields(true).GetBoolean("No data field name");
        }

        private List<WebDataField> GetDataFields()
        {
            return GetDataFields(false);
        }

        private List<WebDataField> GetDataFields(Boolean refresh)
        {
            if (_dataFields.IsNull() || refresh)
            {
                _dataFields = GetOneDataFieldList();
            }
            return _dataFields;
        }

        [TestMethod]
        public void GetDateTime()
        {
            DateTime value;

            value = GetDataFields(true).GetDateTime(DATE_TIME_DATA_FIELD_NAME);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetDateTimeDataTypeError()
        {
            DateTime value;

            value = GetDataFields(true).GetDateTime(INT32_DATA_FIELD_NAME);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetDateTimeFieldNameError()
        {
            DateTime value;

            value = GetDataFields(true).GetDateTime("No data field name");
        }

        [TestMethod]
        public void GetFloat64()
        {
            Double value;

            value = GetDataFields(true).GetFloat64(FLOAT64_DATA_FIELD_NAME);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFloat64DataTypeError()
        {
            Double value;

            value = GetDataFields(true).GetFloat64(INT32_DATA_FIELD_NAME);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFloat64FieldNameError()
        {
            Double value;

            value = GetDataFields(true).GetFloat64("No data field name");
        }

        [TestMethod]
        public void GetInt32()
        {
            Int32 value;

            value = GetDataFields(true).GetInt32(INT32_DATA_FIELD_NAME);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetInt32DataTypeError()
        {
            Int32 value;

            value = GetDataFields(true).GetInt32(INT64_DATA_FIELD_NAME);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetInt32FieldNameError()
        {
            Int32 value;

            value = GetDataFields(true).GetInt32("No data field name");
        }

        [TestMethod]
        public void GetInt64()
        {
            Int64 value;

            value = GetDataFields(true).GetInt64(INT64_DATA_FIELD_NAME);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetInt64DataTypeError()
        {
            Int64 value;

            value = GetDataFields(true).GetInt64(INT32_DATA_FIELD_NAME);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetInt64FieldNameError()
        {
            Int64 value;

            value = GetDataFields(true).GetInt64("No data field name");
        }

        public static List<WebDataField> GetOneDataFieldList()
        {
            Boolean booleanValue;
            List<WebDataField> dataFields;
            WebDataField dataField;

            dataFields = new List<WebDataField>();

            // Add boolean value.
            dataField = new WebDataField();
            dataField.Name = BOOLEAN_DATA_FIELD_NAME;
            dataField.Type = WebDataType.Boolean;
            booleanValue = true;
            dataField.Value = booleanValue.WebToString();
            dataFields.Add(dataField);

            // Add DateTime value.
            dataField = new WebDataField();
            dataField.Name = DATE_TIME_DATA_FIELD_NAME;
            dataField.Type = WebDataType.DateTime;
            dataField.Value = DateTime.Now.WebToString();
            dataFields.Add(dataField);

            // Add float value.
            dataField = new WebDataField();
            dataField.Name = FLOAT64_DATA_FIELD_NAME;
            dataField.Type = WebDataType.Float64;
            dataField.Value = Math.PI.WebToString();
            dataFields.Add(dataField);

            // Add Int32 value.
            dataField = new WebDataField();
            dataField.Name = INT32_DATA_FIELD_NAME;
            dataField.Type = WebDataType.Int32;
            dataField.Value = Int32.MaxValue.WebToString();
            dataFields.Add(dataField);

            // Add Int64 value.
            dataField = new WebDataField();
            dataField.Name = INT64_DATA_FIELD_NAME;
            dataField.Type = WebDataType.Int64;
            dataField.Value = Int64.MaxValue.WebToString();
            dataFields.Add(dataField);

            // Add String value.
            dataField = new WebDataField();
            dataField.Name = STRING_DATA_FIELD_NAME;
            dataField.Type = WebDataType.String;
            dataField.Value = "Testing data fields";
            dataFields.Add(dataField);

            return dataFields;
        }

        [TestMethod]
        public void GetString()
        {
            String value;

            value = GetDataFields(true).GetString(STRING_DATA_FIELD_NAME);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetStringDataTypeError()
        {
            String value;

            value = GetDataFields(true).GetString(INT32_DATA_FIELD_NAME);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetStringFieldNameError()
        {
            String value;

            value = GetDataFields(true).GetString("No data field name");
        }

        [TestMethod]
        public void IsDataFieldSpecified()
        {
            Assert.IsTrue(GetDataFields(true).IsDataFieldSpecified(INT32_DATA_FIELD_NAME));
            Assert.IsFalse(GetDataFields().IsDataFieldSpecified("No data field name"));
        }

        [TestMethod]
        public void SetBoolean()
        {
            Boolean value;

            value = false;
            GetDataFields(true).SetBoolean(BOOLEAN_DATA_FIELD_NAME, value);
            Assert.AreEqual(value, GetDataFields().GetBoolean(BOOLEAN_DATA_FIELD_NAME));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetBooleanDataTypeError()
        {
            Boolean value;

            value = false;
            GetDataFields(true).SetBoolean(INT32_DATA_FIELD_NAME, value);
        }

        [TestMethod]
        public void SetDateTime()
        {
            DateTime value;

            value = DateTime.Now;
            GetDataFields(true).SetDateTime(DATE_TIME_DATA_FIELD_NAME, value);
            Assert.AreEqual(value, GetDataFields().GetDateTime(DATE_TIME_DATA_FIELD_NAME));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetDateTimeDataTypeError()
        {
            DateTime value;

            value = DateTime.Now;
            GetDataFields(true).SetDateTime(INT32_DATA_FIELD_NAME, value);
        }

        [TestMethod]
        public void SetFloat64()
        {
            Double value;

            value = Math.PI;
            GetDataFields(true).SetFloat64(FLOAT64_DATA_FIELD_NAME, value);
            Assert.AreEqual(value.WebToString(), GetDataFields().GetFloat64(FLOAT64_DATA_FIELD_NAME).WebToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetFloat64DataTypeError()
        {
            Double value;

            value = Math.PI;
            GetDataFields(true).SetFloat64(INT32_DATA_FIELD_NAME, value);
        }

        [TestMethod]
        public void SetInt32()
        {
            Int32 value;

            value = Int32.MaxValue;
            GetDataFields(true).SetInt32(INT32_DATA_FIELD_NAME, value);
            Assert.AreEqual(value, GetDataFields().GetInt32(INT32_DATA_FIELD_NAME));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetInt32DataTypeError()
        {
            Int32 value;

            value = Int32.MaxValue;
            GetDataFields(true).SetInt32(INT64_DATA_FIELD_NAME, value);
        }

        [TestMethod]
        public void SetInt64()
        {
            Int64 value;

            value = Int64.MaxValue;
            GetDataFields(true).SetInt64(INT64_DATA_FIELD_NAME, value);
            Assert.AreEqual(value, GetDataFields().GetInt64(INT64_DATA_FIELD_NAME));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetInt64DataTypeError()
        {
            Int64 value;

            value = Int64.MaxValue;
            GetDataFields(true).SetInt64(INT32_DATA_FIELD_NAME, value);
        }

        [TestMethod]
        public void SetString()
        {
            String value;

            value = "Hej";
            GetDataFields(true).SetString(STRING_DATA_FIELD_NAME, value);
            Assert.AreEqual(value, GetDataFields().GetString(STRING_DATA_FIELD_NAME));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetStringDataTypeError()
        {
            String value;

            value = "Hej";
            GetDataFields(true).SetString(INT32_DATA_FIELD_NAME, value);
        }
    }
}

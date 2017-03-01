using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Test.Data
{
    [TestClass]
    public class ListWebDataFieldExtensionTest
    {
        private List<WebDataField> _dataFields;

        public ListWebDataFieldExtensionTest()
        {
            _dataFields = null;
        }

        public List<WebDataField> GetDataFields(Boolean refresh = false)
        {
            if (_dataFields.IsNull() || refresh)
            {
                _dataFields = new List<WebDataField>();
            }

            return _dataFields;
        }

        [TestMethod]
        public void WebToString()
        {
            String dataFieldsString;
            WebDataField dataField;

            dataFieldsString = GetDataFields(true).WebToString();
            Assert.IsTrue(dataFieldsString.IsEmpty());

            dataField = new WebDataField();
            dataField.Name = "Test boolean";
            dataField.Type = WebDataType.Boolean;
            dataField.Value = Boolean.TrueString;
            GetDataFields().Add(dataField);
            dataFieldsString = GetDataFields().WebToString();
            Assert.IsTrue(dataFieldsString.IsNotEmpty());

            dataField = new WebDataField();
            dataField.Name = "Test int";
            dataField.Information = "Något att fundera på.";
            dataField.Type = WebDataType.Int32;
            dataField.Value = 4234234.WebToString();
            GetDataFields().Add(dataField);
            dataFieldsString = GetDataFields().WebToString();
            Assert.IsTrue(dataFieldsString.IsNotEmpty());

            dataField = new WebDataField();
            dataField.Name = "Test string";
            dataField.Unit = "Area";
            dataField.Type = WebDataType.String;
            dataField.Value = "Stort område";
            GetDataFields().Add(dataField);
            dataFieldsString = GetDataFields().WebToString();
            Assert.IsTrue(dataFieldsString.IsNotEmpty());
        }
    }
}

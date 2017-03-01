using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Test.Data
{
    [TestClass]
    public class WebDataFieldExtensionTest
    {
        private WebDataField _dataField;

        public WebDataFieldExtensionTest()
        {
            _dataField = null;
        }

        public WebDataField GetDataField(Boolean refresh = false)
        {
            if (_dataField.IsNull() || refresh)
            {
                _dataField = new WebDataField();
            }

            return _dataField;
        }

        [TestMethod]
        public void WebToString()
        {
            String dataFieldString;

            GetDataField(true).Name = "Test boolean";
            GetDataField().Type = WebDataType.Boolean;
            GetDataField().Value = Boolean.TrueString;
            dataFieldString = GetDataField().WebToString();
            Assert.IsTrue(dataFieldString.IsNotEmpty());

            GetDataField(true).Name = "Test int";
            GetDataField().Information = "Något att fundera på.";
            GetDataField().Type = WebDataType.Int32;
            GetDataField().Value = 4234234.WebToString();
            dataFieldString = GetDataField().WebToString();
            Assert.IsTrue(dataFieldString.IsNotEmpty());

            GetDataField(true).Name = "Test string";
            GetDataField().Unit = "Area";
            GetDataField().Type = WebDataType.String;
            GetDataField().Value = "Stort område";
            dataFieldString = GetDataField().WebToString();
            Assert.IsTrue(dataFieldString.IsNotEmpty());
        }
    }
}

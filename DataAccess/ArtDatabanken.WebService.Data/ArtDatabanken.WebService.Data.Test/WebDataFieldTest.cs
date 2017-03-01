using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    [TestClass]
    public class WebDataFieldTest
    {
        private WebDataField _dataField;

        public WebDataFieldTest()
        {
            _dataField = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebDataField dataField;

            foreach (WebDataType dataType in Enum.GetValues(typeof(WebDataType)))
            {
                dataField = GetOneDataField(dataType);
                Assert.IsNotNull(dataField);
                Assert.AreEqual(dataType, dataField.Type);
            }
        }

        private WebDataField GetDataField(Boolean refresh = false)
        {
            if (_dataField.IsNull() || refresh)
            {
                _dataField = GetOneDataField(WebDataType.Int32);
            }
            return _dataField;
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
        public void Name()
        {
            String name;

            name = null;
            GetDataField(true).Name = name;
            Assert.IsNull(GetDataField().Name);
            name = String.Empty;
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
            value = String.Empty;
            GetDataField().Value = value;
            Assert.AreEqual(GetDataField().Value, value);
            value = "Test data field";
            GetDataField().Value = value;
            Assert.AreEqual(GetDataField().Value, value);
        }
    }
}

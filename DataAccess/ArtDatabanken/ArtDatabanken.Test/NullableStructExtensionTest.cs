using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.Test
{
    [TestClass]
    public class NullableStructExtensionTest
    {
        [TestMethod]
        public void ToStringWithDefaultValue()
        {
            Int32? intValue;
            String stringValue;

            intValue = 50;
            stringValue = intValue.ToString("Data is missing");
            Assert.AreEqual("50", stringValue);

            intValue = null;
            stringValue = intValue.ToString("Data is missing");
            Assert.AreEqual("Data is missing", stringValue);
        }

        [TestMethod]
        public void ToStringWithFormat()
        {
            DateTime? dateTimeValue;
            String stringValue;

            dateTimeValue = new DateTime(2003, 5, 17);
            stringValue = dateTimeValue.ToString("yyyy-MM-dd", "Data is missing");
            Assert.AreEqual("2003-05-17", stringValue);

            dateTimeValue = null;
            stringValue = dateTimeValue.ToString("yyyy-MM-dd", "Data is missing");
            Assert.AreEqual("Data is missing", stringValue);
        }
    }
}
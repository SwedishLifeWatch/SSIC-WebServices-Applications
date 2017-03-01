using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Test.Data
{
    [TestClass]
    public class ListInt32ExtensionTest
    {
        [TestMethod]
        public void WebToString()
        {
            List<Int32> integers;
            String text;

            integers = null;
            text = integers.WebToString();
            Assert.IsTrue(text.IsEmpty());

            integers = new List<Int32>();
            integers.Add(42);
            text = integers.WebToString();
            Assert.IsTrue(text.IsNotEmpty());

            integers.Add(51);
            text = integers.WebToString();
            Assert.IsTrue(text.IsNotEmpty());
        }
    }
}

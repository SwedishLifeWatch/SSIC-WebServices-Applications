using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Test.Data
{
    [TestClass]
    public class ListStringExtensionTest
    {
        [TestMethod]
        public void WebToString()
        {
            List<String> stringList;
            String text;

            stringList = null;
            text = stringList.WebToString();
            Assert.IsTrue(text.IsEmpty());

            stringList = new List<String>();
            stringList.Add("Hej hopp");
            text = stringList.WebToString();
            Assert.IsTrue(text.IsNotEmpty());

            stringList.Add("i lingon skogen");
            text = stringList.WebToString();
            Assert.IsTrue(text.IsNotEmpty());
        }
    }
}

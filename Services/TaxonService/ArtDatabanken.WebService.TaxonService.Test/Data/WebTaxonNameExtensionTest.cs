using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Data;

namespace ArtDatabanken.WebService.TaxonService.Test.Data
{
    using ArtDatabanken.Data;

    [TestClass]
    public class WebTaxonNameExtensionTest : TestBase 
    {
        [TestMethod]
        public void CheckDataTest()
        {
            WebTaxonName taxonName = new WebTaxonName();
            String theName = "te'st";
            taxonName.Name = theName;
            taxonName.CheckData();
            Assert.AreNotEqual(theName, taxonName.Name);
        }
    }
}

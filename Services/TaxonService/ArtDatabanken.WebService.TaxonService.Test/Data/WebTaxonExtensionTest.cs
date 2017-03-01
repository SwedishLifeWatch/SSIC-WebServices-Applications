using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Data;

namespace ArtDatabanken.WebService.TaxonService.Test.Data
{
    [TestClass]
    public class WebTaxonExtensionTest:TestBase
    {
        [TestMethod]
        public void LoadData()
        {
            WebTaxon taxon;

            using (DataReader dataReader = GetContext().GetTaxonDatabase().GetTaxonById((Int32)(TaxonId.Bear), null, Settings.Default.TestLocaleId))
            {
                taxon = new WebTaxon();
                Assert.IsTrue(dataReader.Read());
                taxon.LoadData(dataReader);
                Assert.AreEqual((Int32)(TaxonId.Bear), taxon.Id);
            }
        }
    }
}


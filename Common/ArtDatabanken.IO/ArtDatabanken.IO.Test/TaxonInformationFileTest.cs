using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.IO.Test
{
    [TestClass]
    public class TaxonInformationFileTest : TestBase
    {
        [TestMethod]
        public void GetFileName()
        {
            String fileName;
            ITaxon taxon;

            taxon = CoreData.TaxonManager.GetTaxon(GetUserContext(), BEAR_TAXON_ID);
            fileName = SpeciesInformationDocumentWriter.GetFileName(taxon);
            Assert.AreEqual("Ursus_Arctos_100145", fileName);
        }
    }
}

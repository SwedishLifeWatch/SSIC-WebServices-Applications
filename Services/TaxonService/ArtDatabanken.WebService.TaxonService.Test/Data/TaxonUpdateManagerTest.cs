using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.TaxonService.Test.Data
{
    [TestClass]
    public class TaxonUpdateManagerTest: TestBase
    {

        public TaxonUpdateManagerTest()
            : base(false, 1000)
        {
        }

        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        [TestMethod]
        public void UpdateTaxonSwedishOccurence()
        {
            ArtDatabanken.WebService.TaxonService.Data.TaxonUpdateManager.UpdateTaxonSwedishOccurence(GetContext());
        }

        [TestMethod]
        public void UpdateTaxonSwedishHistory()
        {
            ArtDatabanken.WebService.TaxonService.Data.TaxonUpdateManager.UpdateTaxonSwedishHistory(GetContext());
        }

        [TestMethod]
        public void UpdateTaxonExcludeFromReportingSystem()
        {
            ArtDatabanken.WebService.TaxonService.Data.TaxonUpdateManager.UpdateTaxonExcludeFromReportingSystem(GetContext());
        }

        [TestMethod]
        public void UpdateTaxonDyntaxaQuality()
        {
            ArtDatabanken.WebService.TaxonService.Data.TaxonUpdateManager.UpdateTaxonDyntaxaQuality(GetContext());
        }

        [TestMethod]
        public void UpdateTaxonNumberOfSpeciesInSweden()
        {
            ArtDatabanken.WebService.TaxonService.Data.TaxonUpdateManager.UpdateTaxonNumberOfSpeciesInSweden(GetContext());
        }

    }
}

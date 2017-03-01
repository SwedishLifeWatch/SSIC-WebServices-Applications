using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Data;

namespace ArtDatabanken.WebService.TaxonService.Test.Data
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using ArtDatabanken.Data;

    [TestClass]
    public class WebTaxonPropertiesExtensionTest : TestBase 
    {
        [TestMethod]
        public void CheckDataTest()
        {
            WebTaxonProperties taxonProperty = new WebTaxonProperties();
            String theConceptDefinition = "te'st";
            taxonProperty.ConceptDefinition = theConceptDefinition;
            taxonProperty.CheckData();
            Assert.AreNotEqual(theConceptDefinition, taxonProperty.ConceptDefinition);
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Proxy.NorwayTaxonService;

namespace ArtDatabanken.WebService.Proxy.Test
{
    [TestClass]
    public class NorwayTaxonServiceProxyTest
    {
        [TestMethod]
        public void Constructor()
        {
            NorwayTaxonServiceProxy taxonService;

            taxonService = new NorwayTaxonServiceProxy();
            Assert.IsNotNull(taxonService);
        }

        [TestMethod]
        public void GetTaxonById()
        {
            ArtsnavnType searchResult;
            Int32 taxonId;

            taxonId = 31163;
            searchResult = WebServiceProxy.NorwayTaxonService.GetTaxonById(taxonId);
            Assert.IsNotNull(searchResult);
            Assert.IsTrue(searchResult.Takson.IsNotEmpty());
            Assert.AreEqual(taxonId, Int32.Parse(searchResult.Takson[0].TaksonID));
        }

        [TestMethod]
        public void GetTaxonNamesBySearchCriteria()
        {
            ArtsnavnType searchResult;
            String nameSearchString;

            nameSearchString = "gau";
            searchResult = WebServiceProxy.NorwayTaxonService.GetTaxonNamesBySearchCriteria(nameSearchString);
            Assert.IsNotNull(searchResult);
            Assert.IsTrue(searchResult.LatinskNavn.IsNotEmpty());
        }

        [TestMethod]
        public void GetWebServiceName()
        {
            String webServiceName;

            webServiceName = WebServiceProxy.NorwayTaxonService.GetWebServiceName();
            Assert.IsTrue(webServiceName.IsNotEmpty());
            Assert.AreEqual("Artsnavnebase", webServiceName);
        }
    }
}

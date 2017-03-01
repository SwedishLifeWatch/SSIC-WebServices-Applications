using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Proxy.KulService;

namespace ArtDatabanken.WebService.Proxy.Test
{
    [TestClass]
    public class KulServiceProxyTest
    {
        [TestMethod]
        public void Constructor()
        {
            KulServiceProxy kulServiceProxy;

            kulServiceProxy = new KulServiceProxy();
            Assert.IsNotNull(kulServiceProxy);
        }

        [TestMethod]
        public void GetAreYouThere()
        {
            bool result;

            result = WebServiceProxy.KulService.AreYouThere();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetServiceVersion()
        {
            String result;

            result = WebServiceProxy.KulService.GetServiceVersion();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetSpeciesObservationChangeAsSpecies()
        {
            WebSpeciesObservationChange searchResult;
            Int64 changeId = 1;

            searchResult = WebServiceProxy.KulService.GetSpeciesObservationChangeAsSpecies(DateTime.Now.AddDays(-2),
                                                                                           false,
                                                                                           DateTime.Now,
                                                                                           false,
                                                                                           changeId,
                                                                                           true,
                                                                                           10);
            foreach (WebSpeciesObservation webSpeciesObservation in searchResult.CreatedSpeciesObservations)
            {
                Debug.WriteLine("");
                Debug.WriteLine("-------------------------------------");
                foreach (WebSpeciesObservationField field in webSpeciesObservation.Fields)
                {
                    //  if ( t.Property.Id == SpeciesObservationPropertyId.CatalogNumber)
                    Debug.WriteLine(field.Property.Id + " : " + field.Value);
                }

                Debug.WriteLine("-------------------------------------");
                Debug.WriteLine("");
            }
            Assert.IsNotNull(searchResult);
        }

        [TestMethod]
        public void GetWebServiceName()
        {
            String webServiceName;

            webServiceName = WebServiceProxy.KulService.GetWebServiceName();
            Assert.IsTrue(webServiceName.IsNotEmpty());
            Assert.AreEqual("SpeciesObservationChangeService", webServiceName);
        }
    }
}

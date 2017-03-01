using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Proxy.NorsService;

namespace ArtDatabanken.WebService.Proxy.Test
{
    [TestClass]
    public class NorsServiceProxyTest
    {
        [TestMethod]
        public void Constructor()
        {
            NorsServiceProxy norsServiceProxy;

            norsServiceProxy = new NorsServiceProxy();
            Assert.IsNotNull(norsServiceProxy);
        }

        [TestMethod]
        public void GetAreYouThere()
        {
            bool result;

            result = WebServiceProxy.NorsService.AreYouThere();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetServiceVersion()
        {
            String result;

            result = WebServiceProxy.NorsService.GetServiceVersion();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetSpeciesObservationChangeAsSpecies()
        {
            WebSpeciesObservationChange searchResult;
            Int64 changeId = 1;

            searchResult = WebServiceProxy.NorsService.GetSpeciesObservationChangeAsSpecies(DateTime.Now.AddDays(-2), 
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

            webServiceName = WebServiceProxy.NorsService.GetWebServiceName();
            Assert.IsTrue(webServiceName.IsNotEmpty());
            Assert.AreEqual("SpeciesObservationChangeService", webServiceName);
        }


    }
}

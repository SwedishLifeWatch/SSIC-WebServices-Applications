using System;
using System.Diagnostics;

using ArtDatabanken.WebService.Proxy.SersService;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Proxy.Test
{
    [TestClass]
    public class SersServiceProxyTest
    {
        [TestMethod]
        public void Constructor()
        {
            SersServiceProxy sersServiceProxy;

            sersServiceProxy = new SersServiceProxy();
            Assert.IsNotNull(sersServiceProxy);
        }

        [TestMethod]
        public void GetAreYouThere()
        {
            bool result;

            result = WebServiceProxy.SersService.AreYouThere();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetServiceVersion()
        {
            String result;

            result = WebServiceProxy.SersService.GetServiceVersion();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetSpeciesObservationChangeAsSpecies()
        {
            WebSpeciesObservationChange searchResult;
            Int64 changeId = 1;

            searchResult = WebServiceProxy.SersService.GetSpeciesObservationChangeAsSpecies(DateTime.Now.AddDays(-2), 
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

            webServiceName = WebServiceProxy.SersService.GetWebServiceName();
            Assert.IsTrue(webServiceName.IsNotEmpty());
            Assert.AreEqual("SpeciesObservationChangeService", webServiceName);
        }


    }
}

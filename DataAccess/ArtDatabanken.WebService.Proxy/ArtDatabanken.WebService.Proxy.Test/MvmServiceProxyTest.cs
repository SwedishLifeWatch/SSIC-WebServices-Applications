using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Proxy.MvmService;

namespace ArtDatabanken.WebService.Proxy.Test
{
    [TestClass]
    public class MvmServiceProxyTest
    {
        [TestMethod]
        public void Constructor()
        {
            MvmServiceProxy mvmServiceProxy;

            mvmServiceProxy = new MvmServiceProxy();
            Assert.IsNotNull(mvmServiceProxy);
        }

        [TestMethod]
        public void GetAreYouThere()
        {
            bool result;

            result = WebServiceProxy.MvmService.AreYouThere();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetServiceVersion()
        {
            String result;

            result = WebServiceProxy.MvmService.GetServiceVersion();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetSpeciesObservationChangeAsSpecies()
        {
            WebSpeciesObservationChange searchResult;
            Int64 changeId = 1;

            searchResult = WebServiceProxy.MvmService.GetSpeciesObservationChangeAsSpecies(DateTime.Now, 
                                                                                           false,
                                                                                           DateTime.Now, 
                                                                                           false, 
                                                                                           changeId, 
                                                                                           true, 
                                                                                           100);
            foreach (WebSpeciesObservation webSpeciesObservation in searchResult.CreatedSpeciesObservations)
            {
                Debug.WriteLine("");
                Debug.WriteLine("-------------------------------------");
                foreach (WebSpeciesObservationField t in webSpeciesObservation.Fields)
                {
                  //  if ( t.Property.Id == SpeciesObservationPropertyId.CatalogNumber)
                        Debug.WriteLine(t.Property.Id + " : " + t.Value);
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

            webServiceName = WebServiceProxy.MvmService.GetWebServiceName();
            Assert.IsTrue(webServiceName.IsNotEmpty());
            Assert.AreEqual("SpeciesObservationChangeService", webServiceName);
        }

        /// <summary>
        /// This test will fail if it is run in the middle of the night.
        /// </summary>
        [TestMethod]
        public void IsReadyToUse()
        {
            Boolean isReadyToUse;

            isReadyToUse = WebServiceProxy.MvmService.IsReadyToUse();

            Assert.IsTrue(isReadyToUse);
        }
    }
}

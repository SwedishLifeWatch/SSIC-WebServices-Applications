using System;
using System.Diagnostics;
using ArtDatabanken.WebService.Proxy.WramService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Proxy.Test
{
    [TestClass]
    public class WramServiceProxyTest
    {
        [TestMethod]
        public void Constructor()
        {
            WramServiceProxy wramServiceProxy;

            wramServiceProxy = new WramServiceProxy();
            Assert.IsNotNull(wramServiceProxy);
        }

        [TestMethod]
        public void GetAreYouThere()
        {
            bool areYouThere;

            areYouThere = WebServiceProxy.WramService.AreYouThere();
            Assert.IsTrue(areYouThere);
        }

        [TestMethod]
        public void GetServiceVersion()
        {
            String serviceVersion;

            serviceVersion = WebServiceProxy.WramService.GetServiceVersion();
            Assert.IsTrue(serviceVersion.IsNotEmpty());
        }

        [TestMethod]
        public void GetSpeciesObservationChangeAsSpecies()
        {
            WebSpeciesObservationChange speciesObservationChange;
            Int64 changeId = 1;

            speciesObservationChange = WebServiceProxy.WramService.GetSpeciesObservationChangeAsSpecies(DateTime.Now.AddDays(-2), 
                                                                                                        false,
                                                                                                        DateTime.Now,
                                                                                                        false, 
                                                                                                        changeId, 
                                                                                                        true, 
                                                                                                        10);
            foreach (WebSpeciesObservation webSpeciesObservation in speciesObservationChange.CreatedSpeciesObservations)
            {
                Debug.WriteLine("");
                Debug.WriteLine("-------------------------------------");
                foreach (WebSpeciesObservationField field in webSpeciesObservation.Fields)
                {
                    Debug.WriteLine(field.Property.Id + " : " + field.Value);
                }
                
                Debug.WriteLine("-------------------------------------");
                Debug.WriteLine("");
            }

            Assert.IsNotNull(speciesObservationChange);
         }
     
        [TestMethod]
        public void GetWebServiceName()
        {
            String webServiceName;

            webServiceName = WebServiceProxy.WramService.GetWebServiceName();
            Assert.IsTrue(webServiceName.IsNotEmpty());
            Assert.AreEqual("WramSpeciesDataChangeService", webServiceName);
        }
    }
}

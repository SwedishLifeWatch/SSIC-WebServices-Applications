using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Proxy.Test
{
    [TestClass]
    public class SpeciesObservationHarvestServiceProxyTest
    {
        private WebClientInformation _clientInformation;

        public SpeciesObservationHarvestServiceProxyTest()
        {
            _clientInformation = null;
        }

        [TestMethod]
        public void ClearCache()
        {
            TestInitialize(Settings.Default.UserAdminApplicationIdentifier);
            WebServiceProxy.SpeciesObservationHarvestService.ClearCache(GetClientInformation());
        }

        [TestMethod]
        public void DeleteTrace()
        {
            TestInitialize(Settings.Default.UserAdminApplicationIdentifier);

            // Create some trace information.
            WebServiceProxy.SpeciesObservationHarvestService.StartTrace(GetClientInformation(), Settings.Default.TestUserName);
            WebServiceProxy.SpeciesObservationHarvestService.GetLog(GetClientInformation(), LogType.None, null, 100);
            WebServiceProxy.SpeciesObservationHarvestService.StopTrace(GetClientInformation());

            // Delete trace information.
            WebServiceProxy.SpeciesObservationHarvestService.DeleteTrace(GetClientInformation());
        }

        protected WebClientInformation GetClientInformation()
        {
            return _clientInformation;
        }

        [TestMethod]
        public void GetStatus()
        {
            List<WebResourceStatus> status;

            status = WebServiceProxy.SpeciesObservationHarvestService.GetStatus(GetClientInformation());
            Assert.IsTrue(status.IsNotEmpty());
            status = WebServiceProxy.SpeciesObservationHarvestService.GetStatus(GetClientInformation());
            Assert.IsTrue(status.IsNotEmpty());
        }

        [TestMethod]
        public void Login()
        {
            WebLoginResponse loginResponse;

            loginResponse = WebServiceProxy.SpeciesObservationHarvestService.Login(Settings.Default.TestUserName,
                                                                                   Settings.Default.TestPassword,
                                                                                   Settings.Default.DyntaxaApplicationIdentifier,
                                                                                   false);
            Assert.IsNotNull(loginResponse);
        }

        [TestMethod]
        public void Logout()
        {
            WebClientInformation clientInformation;
            WebLoginResponse loginResponse;

            loginResponse = WebServiceProxy.SpeciesObservationHarvestService.Login(Settings.Default.TestUserName,
                                                                                   Settings.Default.TestPassword,
                                                                                   Settings.Default.DyntaxaApplicationIdentifier,
                                                                                   false);
            Assert.IsNotNull(loginResponse);
            clientInformation = new WebClientInformation();
            clientInformation.Token = loginResponse.Token;
            WebServiceProxy.SpeciesObservationHarvestService.Logout(clientInformation);
        }

        [TestMethod]
        public void Ping()
        {
            Boolean ping;

            ping = WebServiceProxy.SpeciesObservationHarvestService.Ping();
            Assert.IsTrue(ping);
        }

        [TestMethod]
        public void StartTrace()
        {
            TestInitialize(Settings.Default.UserAdminApplicationIdentifier);
            WebServiceProxy.SpeciesObservationHarvestService.StartTrace(GetClientInformation(), "kalle kula");
            WebServiceProxy.SpeciesObservationHarvestService.StopTrace(GetClientInformation());
        }

        [TestMethod]
        public void StopTrace()
        {
            TestInitialize(Settings.Default.UserAdminApplicationIdentifier);
            WebServiceProxy.SpeciesObservationHarvestService.StartTrace(GetClientInformation(), "kalle kula");
            WebServiceProxy.SpeciesObservationHarvestService.StopTrace(GetClientInformation());
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                if (_clientInformation.IsNotNull())
                {
                    WebServiceProxy.SpeciesObservationHarvestService.Logout(_clientInformation);
                }
            }
            catch
            {
                // Test is done.
                // We are not interested in problems that
                // occures due to test of error handling.
            }
            finally
            {
                _clientInformation = null;
            }
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void TestInitialize()
        {
            TestInitialize("WebAdministration");
        }

        public void TestInitialize(String applicationIdentifier)
        {
            WebLoginResponse loginResponse;

            TestCleanup();

// PRODUKTIONSMILJÖ
            ////Configuration.InstallationType = InstallationType.Production;
            ////WebServiceProxy.SpeciesObservationHarvestService.WebServiceAddress = @"silurus2-1.artdata.slu.se/SpeciesObservationHarvestService/SpeciesObservationHarvestService.svc";
            ////loginResponse = WebServiceProxy.SpeciesObservationHarvestService.Login("username",
            ////                                                                       "password",
            ////                                                                       applicationIdentifier,
            ////                                                                       false);
 
 // TESTMILJÖ
            Configuration.InstallationType = InstallationType.ServerTest;
            loginResponse = WebServiceProxy.SpeciesObservationHarvestService.Login(Settings.Default.TestUserName,
                                                                                  Settings.Default.TestPassword,
                                                                                  applicationIdentifier,
                                                                                  false);

            _clientInformation = new WebClientInformation();
            _clientInformation.Locale = loginResponse.Locale;
            _clientInformation.Token = loginResponse.Token;
        }

        [TestMethod]
        public void UpdateSpeciesObservations()
        {
            DateTime startDate = new DateTime(2013, 1, 11);
            DateTime endDate = new DateTime(2013, 11, 15);
            List<Int32> dataProviderIds;

            dataProviderIds = new List<Int32>();
            //dataProviderIds.Add(1); // Artportalen 2
            //dataProviderIds.Add(2); // Obsdatabasen
            dataProviderIds.Add(3); // DINA
            //dataProviderIds.Add(4); // Artportalen 1
            //dataProviderIds.Add(5); // MVM
            //dataProviderIds.Add(6); // Nors
            //dataProviderIds.Add(7); // Sers
            //dataProviderIds.Add(8); // Wram
            Stopwatch harvesttime = new Stopwatch();
            while (startDate <= endDate)
            {
                while ((((DateTime.Now.TimeOfDay.Hours == 23) &&
                         (DateTime.Now.TimeOfDay.Minutes > 50)) ||
                        ((DateTime.Now.TimeOfDay.Hours == 00))) ||
                       (((DateTime.Now.TimeOfDay.Hours == 2) &&
                         (DateTime.Now.TimeOfDay.Minutes > 50)) ||
                        ((DateTime.Now.TimeOfDay.Hours == 3))))
                {
                    // wait a minute...
                    Thread.Sleep(60000);
                }

                harvesttime.Start();
                WebServiceProxy.SpeciesObservationHarvestService.UpdateSpeciesObservations(GetClientInformation(), startDate, startDate, dataProviderIds);
                Debug.WriteLine("{0}: Ready with {1} in {2}s.", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString(), startDate.ToShortDateString(), harvesttime.ElapsedMilliseconds / 1000);
                
                startDate = startDate.AddDays(1);

                if (harvesttime.ElapsedMilliseconds < 3000)
                {
                    Thread.Sleep(500);
                }
                else
                {
                    Thread.Sleep(3000);
                }

                harvesttime.Reset();
            }
        }

        [TestMethod]
        public void StartSpeciesObservationUpdate()
        {
            DateTime startDate = new DateTime(2013, 1, 11);
            DateTime endDate = new DateTime(2013, 11, 15);
            List<Int32> dataProviderIds;

            dataProviderIds = new List<Int32>();
            dataProviderIds.Add(1); // Artportalen 2
            
            WebServiceProxy.SpeciesObservationHarvestService.StartSpeciesObservationUpdate(GetClientInformation(), startDate, startDate, dataProviderIds, true);
        }
    }
}

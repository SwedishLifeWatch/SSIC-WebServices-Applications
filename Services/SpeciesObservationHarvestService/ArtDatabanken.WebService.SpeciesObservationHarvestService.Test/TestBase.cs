using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using ApplicationManager = ArtDatabanken.WebService.Data.ApplicationManager;
using DatabaseManager = ArtDatabanken.WebService.SpeciesObservationHarvestService.Data.DatabaseManager;
using MetadataManager = ArtDatabanken.WebService.Data.MetadataManager;
using RegionManager = ArtDatabanken.WebService.Data.RegionManager;
using SpeciesObservationManager = ArtDatabanken.WebService.SpeciesObservation.Data.SpeciesObservationManager;
using TaxonManager = ArtDatabanken.WebService.Data.TaxonManager;
using UserManager = ArtDatabanken.WebService.Data.UserManager;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test
{
    [TestClass]
    public class TestBase
    {
        private static WebServiceContext _context;
        private readonly Boolean _useTransaction;
        private readonly Int32 _transactionTimeout;

        public TestBase()
            : this(false, 10)
        {
        }

        public TestBase(Boolean useTransaction,
                        Int32 transactionTimeout)
        {
            _context = null;
            _useTransaction = useTransaction;
            _transactionTimeout = transactionTimeout;
        }

        protected WebClientInformation GetClientInformation()
        {
            WebClientInformation clientInformation;
            WebServiceContext context;

            context = GetContext();
            clientInformation = new WebClientInformation();
            clientInformation.Token = context.ClientToken.Token;
            return clientInformation;
        }

        protected virtual WebServiceContext GetContext()
        {
            return _context;
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public virtual void TestCleanup()
        {
            if (_useTransaction)
            {
                // _context.RollbackTransaction();
                _context.CommitTransaction();
            }

            if (_context.IsNotNull())
            {
                ArtDatabanken.Data.ArtDatabankenService.UserManager.Logout();

                WebServiceData.UserManager.Logout(_context);

                _context.Dispose();
                _context = null;
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
            TestCleanup();

            Configuration.SetInstallationType();
            //// Configuration.InstallationType = InstallationType.Production;
           
            WebServiceData.ApplicationManager = new ApplicationManager();
            WebServiceData.AuthorizationManager = new AuthorizationManager();
            WebServiceData.CoordinateConversionManager = new CoordinateConversionManager();
            WebServiceData.DatabaseManager = new DatabaseManager();
            WebServiceData.FactorManager = new WebService.Data.FactorManager();
            WebServiceData.LogManager = new SpeciesObservationHarvestService.Data.LogManager();
            WebServiceData.MetadataManager = new MetadataManager();
            WebServiceData.TaxonManager = new TaxonManager();
            WebServiceData.RegionManager = new RegionManager();
            WebServiceData.SpeciesFactManager = new WebService.Data.SpeciesFactManager();
            WebServiceData.SpeciesObservationManager = new WebService.Data.SpeciesObservationManager();
            WebServiceData.UserManager = new UserManager();
            WebServiceData.WebServiceManager = new WebServiceManager();

            WebSpeciesObservationServiceData.SpeciesObservationManager = new SpeciesObservationManager();
            WebSpeciesObservationServiceData.TaxonManager = new SpeciesObservation.Data.TaxonManager();
            
            // test
            _context = new WebServiceContextCached(Settings.Default.TestUserName, applicationIdentifier);


            try
            {
                WebServiceData.UserManager.Login(_context, Settings.Default.TestUserName, Settings.Default.TestPassword, applicationIdentifier, false);

            }
            catch (Exception)
            {
                // Try Once more if service is turned off... 
                Thread.Sleep(20000);
                WebServiceData.UserManager.Login(_context, Settings.Default.TestUserName, Settings.Default.TestPassword, applicationIdentifier, false);
            }

            // prod
            //// _context = new WebServiceContextCached("UserName" , applicationIdentifier);
            //// WebServiceData.UserManager.Login(_context, "UserName", "Password", applicationIdentifier, false);
            
            ArtDatabanken.Data.ArtDatabankenService.UserManager.Login(WebServiceData.WebServiceManager.Name, 
                                                                      WebServiceData.WebServiceManager.Password,
                                                                      ApplicationIdentifier.ArtDatabankenSOA.ToString(),
                                                                      false);
            if (_useTransaction)
            {
                _context.StartTransaction(_transactionTimeout);
            }
        }
    }
}

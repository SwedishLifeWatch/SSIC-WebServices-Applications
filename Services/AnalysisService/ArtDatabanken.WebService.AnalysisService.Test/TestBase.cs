using System;
using System.Threading;
using ArtDatabanken.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.AnalysisService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Data;
using DatabaseManager = ArtDatabanken.WebService.AnalysisService.Data.DatabaseManager;
using SpeciesFactManager = ArtDatabanken.WebService.Data.SpeciesFactManager;
using SpeciesObservationManager = ArtDatabanken.WebService.SpeciesObservation.Data.SpeciesObservationManager;
using TaxonManager = ArtDatabanken.WebService.SpeciesObservation.Data.TaxonManager;
using UserManager = ArtDatabanken.WebService.Data.UserManager;


namespace ArtDatabanken.WebService.AnalysisService.Test
{
    [TestClass]
    public class TestBase
    {
        private readonly Boolean _useTransaction;
        private readonly Int32 _transactionTimeout;

        public TestBase()
            : this(false, 10)
        {
        }

        public TestBase(Boolean useTransaction,
                        Int32 transactionTimeout)
        {
            Context = null;
            _useTransaction = useTransaction;
            _transactionTimeout = transactionTimeout;
        }

        protected WebServiceContext Context
        { get; private set; }

        protected WebLoginResponse LoginResponse
        { get; private set; }

        protected WebClientInformation GetClientInformation()
        {
            WebClientInformation clientInformation;
            WebServiceContext context;

            context = Context;
            clientInformation = new WebClientInformation();
            clientInformation.Token = context.ClientToken.Token;
            return clientInformation;
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void TestCleanup()
        {
            if (_useTransaction)
            {
                Context.RollbackTransaction();
            }
            Context.Dispose();
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void TestInitialize()
        {
            Configuration.SetInstallationType();
            WebServiceData.ApplicationManager = new WebService.Data.ApplicationManager();
            WebServiceData.AuthorizationManager = new AuthorizationManager();
            WebServiceData.CoordinateConversionManager = new CoordinateConversionManager();
            WebServiceData.DatabaseManager = new DatabaseManager();
            WebServiceData.GeometryManager = new GeometryManager();
            WebServiceData.LogManager = new LogManager();
            WebServiceData.RegionManager = new WebService.Data.RegionManager();
            WebServiceData.UserManager = new UserManager();
            WebServiceData.WebServiceManager = new WebServiceManager();
            WebServiceData.SpeciesActivityManager = new SpeciesActivityManager();
            WebServiceData.SpeciesFactManager = new SpeciesFactManager();
            WebServiceData.SpeciesObservationManager = new WebService.Data.SpeciesObservationManager();
            WebServiceData.TaxonManager = new WebService.Data.TaxonManager();
            WebServiceData.AnalysisManager = new WebService.Data.AnalysisManager();

            //WebServiceData.GeoReferenceManager = new GeoReferenceManager();


            Context = new WebServiceContextCached(Settings.Default.TestUserName, ApplicationIdentifier.PrintObs.ToString());
            //Finally login application user
            try
            {
                LoginResponse = WebServiceData.UserManager.Login(Context, Settings.Default.TestUserName, Settings.Default.TestPassword, ApplicationIdentifier.PrintObs.ToString(), false);
            
            }
            catch
            {
                // Try Once more if service is turned off... 
                Thread.Sleep(20000);
                LoginResponse = WebServiceData.UserManager.Login(Context, Settings.Default.TestUserName, Settings.Default.TestPassword, ApplicationIdentifier.PrintObs.ToString(), false);
            }
            // Login to Artdatabanken service.
            ArtDatabanken.Data.ArtDatabankenService.UserManager.Login(WebServiceData.WebServiceManager.Name, WebServiceData.WebServiceManager.Password, ApplicationIdentifier.ArtDatabankenSOA.ToString(), false);
           
            if (_useTransaction)
            {
                Context.StartTransaction(_transactionTimeout);
            }
        }
    }
}

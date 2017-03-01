using System;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.ReferenceService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.ReferenceService.Test
{
    [TestClass]
    public class TestBase
    {
        private Boolean _useTransaction;
        private readonly Int32 _transactionTimeout;
        private WebServiceContext _context;

        public TestBase()
            : this(true, 10)
        {
        }

        public TestBase(Boolean useTransaction,
                        Int32 transactionTimeout)
        {
            _context = null;
            _useTransaction = useTransaction;
            _transactionTimeout = transactionTimeout;
        }

        public Boolean UseTransaction
        {
            get
            {
                return _useTransaction;
            }

            set
            {
                if (_context.IsNotNull())
                {
                    if (_context.IsInTransaction() && !value)
                    {
                        _context.RollbackTransaction();
                    }

                    if ((!_context.IsInTransaction()) && value)
                    {
                        _context.StartTransaction(_transactionTimeout);
                    }
                }

                _useTransaction = value;
            }
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

        protected WebServiceContext GetContext()
        {
            return _context;
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void TestCleanup()
        {
            if (_context.IsNotNull())
            {
                if (_useTransaction)
                {
                    _context.RollbackTransaction();
                }

                _context.Dispose();
                _context = null;
            }
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void TestInitialize()
        {
            WebServiceData.ApplicationManager = new ApplicationManager();
            WebServiceData.AuthorizationManager = new AuthorizationManager();
            WebServiceData.CoordinateConversionManager = new CoordinateConversionManager();
            WebServiceData.DatabaseManager = new DatabaseManager();
            WebServiceData.LogManager = new LogManager();
            WebServiceData.RegionManager = new RegionManager();
            WebServiceData.UserManager = new UserManager();
            WebServiceData.WebServiceManager = new WebServiceManager();
            WebServiceData.SpeciesActivityManager = new SpeciesActivityManager();
            WebServiceData.SpeciesObservationManager = new SpeciesObservationManager();
            WebServiceData.TaxonManager = new TaxonManager();
            WebServiceData.AnalysisManager = new AnalysisManager();

            _context = new WebServiceContextCached(Settings.Default.TestUserName, ApplicationIdentifier.EVA.ToString());

            if (_useTransaction)
            {
                _context.StartTransaction(_transactionTimeout);
            }
        }
    }
}

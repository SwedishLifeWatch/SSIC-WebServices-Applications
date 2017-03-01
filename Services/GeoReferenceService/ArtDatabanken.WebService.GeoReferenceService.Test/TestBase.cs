using System;
using System.Text;
using ArtDatabanken.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.GeoReferenceService.Data;

namespace ArtDatabanken.WebService.GeoReferenceService.Test
{
    [TestClass]
    public class TestBase
    {
        private static WebServiceContext _context;

        private Boolean _useTransaction;
        private Int32 _transactionTimeout;

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

        protected WebServiceContext GetContext()
        {
            if (_context.IsNull())
            {
                GetContext(ApplicationIdentifier.UserAdmin);
            }
            return _context;
        }

        protected WebServiceContext GetContext(ApplicationIdentifier applicationIdentifier)
        {
            if (_context.IsNotNull())
            {
                TestCleanup();
            }
            switch (applicationIdentifier)
            {
                case ApplicationIdentifier.ArtDatabankenSOA:
                    _context = new WebServiceContextCached(WebServiceData.WebServiceManager.Name, applicationIdentifier.ToString());
                        WebServiceData.UserManager.Login(_context, WebServiceData.WebServiceManager.Name, WebServiceData.WebServiceManager.Password, applicationIdentifier.ToString(), false);
                    break;
                default:
                        _context = new WebServiceContextCached(Settings.Default.TestUserName, applicationIdentifier.ToString());
                        WebServiceData.UserManager.Login(_context, Settings.Default.TestUserName, Settings.Default.TestPassword, applicationIdentifier.ToString(), false);
                    break;
            }
            if (_useTransaction)
            {
                _context.StartTransaction(_transactionTimeout);
            }
            return _context;
        }

        protected String GetString(Int32 stringLength)
        {
            Int32 stringIndex;
            StringBuilder stringBuilder;

            stringBuilder = new StringBuilder(stringLength);
            for (stringIndex = 0; stringIndex < stringLength; stringIndex++)
            {
                stringBuilder.Append("a");
            }
            return stringBuilder.ToString();
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
            Configuration.SetInstallationType();

            WebServiceData.AuthorizationManager = new AuthorizationManager();
            WebServiceData.CoordinateConversionManager = new CoordinateConversionManager();
            WebServiceData.DatabaseManager = new DatabaseManager();
            WebServiceData.LogManager = new LogManager();
            WebServiceData.UserManager = new UserManager();
            WebServiceData.WebServiceManager = new WebServiceManager();
        }
    }
}

using System;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.Data.ArtDatabankenService;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;
using ArtDatabanken.WebService.TaxonService;
using ArtDatabanken.WebService.TaxonService.Data;

namespace ArtDatabanken.WebService.TaxonService.Test
{
    [TestClass]
    public class TestBase
    {
        // These test assumes that the following information
        // has been stored in the tabel ObsUsers:ClientApplicationInformation:
        // WebServiceTest	1.2.3.4	1

        private static WebServiceContext _context;
        private static WebLoginResponse _webLoginResponse;

        private Boolean _useTransaction;
        private Int32 _transactionTimeout;

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

        protected WebServiceContext GetRevisionContext(int revisionId = 1) 
        {
            WebClientInformation clientInformation = new WebClientInformation();
            String _revisionGUID = Settings.Default.TestRevisionGUIDPrefix + ":" + Convert.ToString(revisionId);

            foreach (WebRole role in _webLoginResponse.Roles.Where(role => role.Identifier.IsNotNull() && role.Identifier.EndsWith(_revisionGUID)))
            {
                clientInformation.Role = role;
            }
            if (clientInformation.Role.IsNull())
            {
                throw new ArgumentException("GetRevisionContext - no role with " + _revisionGUID);
            }
            clientInformation.Token = _webLoginResponse.Token;
            clientInformation.Locale = _webLoginResponse.Locale;
            _context = new WebServiceContext(clientInformation);

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
        [TestCleanup()]
        public void TestCleanup()
        {
            if (_useTransaction)
            {
                _context.RollbackTransaction();
                Thread.Sleep(100);
               // _context.CommitTransaction();
            }
            _context.Dispose(); 
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void TestInitialize()
        {
            // Use the following two lines if Test to Production server. Comment out the Configuration.InstallationType = InstallationType.ServerTest row. Login with your own user.
            //Configuration.Debug = false;
            //Configuration.InstallationType = InstallationType.Production;
            Configuration.SetInstallationType();
            WebServiceData.AuthorizationManager = new ArtDatabanken.WebService.Data.AuthorizationManager();
            WebServiceData.DatabaseManager = new ArtDatabanken.WebService.TaxonService.Data.DatabaseManager();
            WebServiceData.LogManager = new LogManager();
            WebServiceData.ReferenceManager = new ArtDatabanken.WebService.Data.ReferenceManager();
            WebServiceData.UserManager = new ArtDatabanken.WebService.Data.UserManager();
            WebServiceData.WebServiceManager = new WebServiceManager();
            _context = new WebServiceContextCached(Settings.Default.TestUserName, Settings.Default.TestApplicationIdentifier);
            //_context = new WebServiceContext("TestDyntaxaReader", "DyntaxaReader1");
             //_webLoginResponse = WebServiceData.UserManager.Login(_context, "TestDyntaxaReader", "DyntaxaReader1", Settings.Default.TestApplicationIdentifier, false);
            try
            {
                _webLoginResponse = WebServiceData.UserManager.Login(_context, Settings.Default.TestUserName, Settings.Default.TestPassword, Settings.Default.TestApplicationIdentifier, false);                
            }
            catch (Exception e)
            {
                // Try Once more if service is turned off... 
                Thread.Sleep(20000);
                _webLoginResponse = WebServiceData.UserManager.Login(_context, Settings.Default.TestUserName, Settings.Default.TestPassword, Settings.Default.TestApplicationIdentifier, false); 
            }

            ArtDatabanken.Data.ArtDatabankenService.UserManager.Login(WebServiceData.WebServiceManager.Name, WebServiceData.WebServiceManager.Password, "ArtDatabankenSOA", false);
            
            if (_useTransaction)
            {
                _context.StartTransaction(_transactionTimeout);
            }
        }
    }
}

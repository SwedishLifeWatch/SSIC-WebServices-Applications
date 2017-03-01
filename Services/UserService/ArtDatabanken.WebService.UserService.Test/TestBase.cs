using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Data;

namespace ArtDatabanken.WebService.UserService.Test
{
    using System.Threading;

    [TestClass]
    public class TestBase
    {
        private Boolean _useTransaction;
        private readonly Int32 _transactionTimeout;
        private WebServiceContext _context;

        public TestBase()
            : this (true, 10)
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
            if (_useTransaction)
            {
                _context.RollbackTransaction();
            }
            _context.Dispose();
            _context = null;
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void TestInitialize()
        {
            WebServiceData.AuthorizationManager = new WebService.Data.AuthorizationManager();
            WebServiceData.DatabaseManager = new DatabaseManager();
            WebServiceData.LogManager = new LogManager();
            WebServiceData.UserManager = new UserManagerAdapter();
            WebServiceData.WebServiceManager = new WebServiceManager();
            _context = new WebServiceContextCached(Settings.Default.TestUserName, Settings.Default.TestApplicationIdentifier);
            UserService.Data.UserManager.Login(_context, Settings.Default.TestUserName, Settings.Default.TestPassword, Settings.Default.TestApplicationIdentifier, false);
            try
            {
                UserService.Data.UserManager.Login(_context, Settings.Default.TestUserName, Settings.Default.TestPassword, Settings.Default.TestApplicationIdentifier, false);

            }
            catch (Exception)
            {
                // Try Once more if service is turned off... 
                Thread.Sleep(20000);
                UserService.Data.UserManager.Login(_context, Settings.Default.TestUserName, Settings.Default.TestPassword, Settings.Default.TestApplicationIdentifier, false);
            }
            if (_useTransaction)
            {
                _context.StartTransaction(_transactionTimeout);
            }
        }
    }
}

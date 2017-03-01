using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Web.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Security;
using ArtDatabanken.WebService.ArtDatabankenService.Data;
using ArtDatabanken.WebService.ArtDatabankenService.Database;
using ArtDatabanken.WebService.ArtDatabankenService.Test.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test
{
    [TestClass]
    public class WebServiceContextTest : TestBase
    {
        public WebServiceContextTest()
            : base(false, 0)
        {
            ApplicationIdentifier = Settings.Default.WebAdminstrationApplicationIdentifier;
        }

        [TestMethod]
        public void AddCachedObjectAbsoluteTime()
        {
            GetContext().AddCachedObject("No key",
                                         this,
                                         DateTime.Now,
                                         CacheItemPriority.Normal);
        }

        [TestMethod]
        public void AddCachedObjectSlidingTime()
        {
            GetContext().AddCachedObject("No key",
                                         this,
                                         new TimeSpan(1, 1, 1),
                                         CacheItemPriority.Normal);
        }

        [TestMethod]
        public void CheckHasTransaction()
        {
            GetContext().StartTransaction(1);
            GetContext().CheckTransaction();
            GetContext().RollbackTransaction();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void CheckHasTransactionNoTransactionerror()
        {
            GetContext().CheckTransaction();
        }

        [TestMethod]
        public void ClearCache()
        {
            GetContext().ClearCache();
        }

        [TestMethod]
        public void ClientToken()
        {
            Assert.IsNotNull(GetContext().ClientToken);
        }

        [TestMethod]
        public void CommitTransaction()
        {
            GetContext().StartTransaction(1);
            GetContext().CommitTransaction();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void CommitTransactionNoTransactionError()
        {
            GetContext().CommitTransaction();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void CommitTransactionTransactionTimeoutError()
        {
            GetContext().StartTransaction(1);
            Thread.Sleep(2000);
            GetContext().CommitTransaction();
        }

        [TestMethod]
        public void Constructor()
        {
            Assert.IsNotNull(GetContext());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorClientIPAddressError()
        {
            CipherString cipherString;
            String token;

            token = TEST_USER_NAME + Settings.Default.ClientTokenDelimitor +
                    ApplicationIdentifier + Settings.Default.ClientTokenDelimitor +
                    42 + Settings.Default.ClientTokenDelimitor +
                    "127.127.127.127" + Settings.Default.ClientTokenDelimitor +
                    WebServiceData.WebServiceManager.Name;
            cipherString = new CipherString();
            token = cipherString.EncryptText(token);
            new WebServiceContext(token);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullClientTokenError()
        {
            new WebServiceContext((String)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorUserError()
        {
            new WebServiceContext(new WebClientToken("No user name", ApplicationIdentifier, WebServiceData.WebServiceManager.Key).Token);
        }

        [TestMethod]
        public void Dispose()
        {
            GetContext().Dispose();
            GetContext().Dispose();
        }

        [TestMethod]
        public void GetCachedObject()
        {
            Assert.IsNull(GetContext().GetCachedObject("No key 56"));
        }

        [TestMethod]
        public void GetClientIPAddress()
        {
            Assert.IsTrue(WebServiceContext.GetClientIPAddress().IsNotEmpty());
        }

        [TestMethod]
        public void GetDatabase()
        {
            DataServer database;

            foreach (DataServer.DatabaseId databaseId in Enum.GetValues(typeof(DataServer.DatabaseId)))
            {
                database = GetContext().GetDatabase(databaseId);
                Assert.IsNotNull(database);
                Assert.AreEqual(databaseId, database.GetDatabaseId());
            }
        }

        public WebServiceContext GetWebServiceContext()
        {
            return new WebServiceContextCached(new WebClientToken(TEST_USER_NAME, ApplicationIdentifier, WebServiceData.WebServiceManager.Key).Token);
        }

        [TestMethod]
        public void RemoveCachedObject()
        {
            GetContext().RemoveCachedObject("No key");
        }

        [TestMethod]
        public void RequestId()
        {
            Assert.IsTrue(GetContext().RequestId >= 0);
            Assert.AreNotEqual(GetContext().RequestId, GetWebServiceContext().RequestId);
        }

        [TestMethod]
        public void RollbackTransaction()
        {
            // Should be ok to rollback an unexisting transaction.
            GetContext().RollbackTransaction();

            // Normal rollback.
            GetContext().StartTransaction(1);
            GetContext().RollbackTransaction();
            Thread.Sleep(2000);

            // Should be ok to rollback twice.
            GetContext().StartTransaction(1);
            GetContext().RollbackTransaction();
            GetContext().RollbackTransaction();
        }

        [TestMethod]
        public void SessionId()
        {
            Assert.IsTrue(GetContext().SessionId >= 0);
            Assert.AreNotEqual(GetContext().SessionId, GetWebServiceContext().SessionId);
        }

        [TestMethod]
        public void StartTrace()
        {
            List<WebLogRow> logRows;

            LogManager.DeleteTrace(GetContext());

            GetContext().StartTrace(null);
            using (WebServiceContext context = new WebServiceContext(new WebClientToken(TEST_USER_NAME, ApplicationIdentifier, WebServiceData.WebServiceManager.Key).Token, "StartTrace"))
            {
                // Do something.
            }
            GetContext().StopTrace();
            logRows = LogManager.GetLog(GetContext(), LogType.Trace, TEST_USER_NAME, 1);
            Assert.IsTrue(logRows.IsNotEmpty());
            LogManager.DeleteTrace(GetContext());

            GetContext().StartTrace(TEST_USER_NAME);
            using (WebServiceContext context = new WebServiceContext(new WebClientToken(TEST_USER_NAME, ApplicationIdentifier, WebServiceData.WebServiceManager.Key).Token, "StartTrace"))
            {
                // Do something.
            }
            GetContext().StopTrace();
            logRows = LogManager.GetLog(GetContext(), LogType.Trace, TEST_USER_NAME, 1);
            Assert.IsTrue(logRows.IsNotEmpty());
            LogManager.DeleteTrace(GetContext());
        }

        [TestMethod]
        public void StartTransaction()
        {
            WebServiceContext context = null;

            GetContext().StartTransaction(15);
            GetContext().CommitTransaction();

            try
            {
                context = GetWebServiceContext();
                context.StartTransaction(15);
                context.CommitTransaction();
            }
            finally
            {
                context.RollbackTransaction();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void StartTransactionAlreadyStartedException()
        {
            GetContext().StartTransaction(1);
            GetContext().StartTransaction(1);
        }

        [TestMethod]
        public void StopTrace()
        {
            List<WebLogRow> logRows1, logRows2;

            LogManager.DeleteTrace(GetContext());
            GetContext().StartTrace(null);
            using (WebServiceContext context = new WebServiceContext(new WebClientToken(TEST_USER_NAME, ApplicationIdentifier, WebServiceData.WebServiceManager.Key).Token, "StartTrace"))
            {
                // Do something.
            }
            GetContext().StopTrace();
            logRows1 = LogManager.GetLog(GetContext(), LogType.Trace, TEST_USER_NAME, 1);
            Assert.IsTrue(logRows1.IsNotEmpty());

            // Check that trace is stopped.
            using (WebServiceContext context = new WebServiceContext(new WebClientToken(TEST_USER_NAME, ApplicationIdentifier, WebServiceData.WebServiceManager.Key).Token, "StartTrace"))
            {
                // Do something.
            }
            logRows2 = LogManager.GetLog(GetContext(), LogType.Trace, TEST_USER_NAME, 1);
            Assert.IsTrue(logRows2.IsNotEmpty());
            Assert.AreEqual(logRows1.Count, logRows2.Count);

            LogManager.DeleteTrace(GetContext());
        }
    }
}

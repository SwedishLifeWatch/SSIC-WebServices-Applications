using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Web.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Database;

namespace ArtDatabanken.WebService.GeoReferenceService.Test
{
    [TestClass]
    public class WebServiceContextTest : TestBase
    {
        public WebServiceContextTest()
            : base(false, 0)
        {
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
            WebClientInformation clientInformation;
            WebClientToken clientToken;
            WebServiceContext context;

            clientToken = new WebClientToken(Settings.Default.TestUserName,
                                             Settings.Default.UserAdminApplicationIdentifier,
                                             WebServiceData.WebServiceManager.Key);
            clientInformation = new WebClientInformation();
            clientInformation.Locale = new WebLocale();
            clientInformation.Locale.Id = 581;
            clientInformation.Locale.ISOCode = "se-SV";
            clientInformation.Locale.Name = "Swedish (Sweden)";
            clientInformation.Locale.NativeName = "svenska (Sverige)";
            clientInformation.Token = clientToken.Token;
            context = new WebServiceContext(clientInformation);
            Assert.IsNotNull(context);

            context = new WebServiceContext(Settings.Default.TestUserName,
                                             Settings.Default.UserAdminApplicationIdentifier);
            Assert.IsNotNull(context);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorNullClientTokenError()
        {
            WebClientInformation clientInformation;
            WebServiceContext context;

            clientInformation = new WebClientInformation();
            clientInformation.Locale = new WebLocale();
            clientInformation.Locale.Id = 581;
            clientInformation.Locale.ISOCode = "se-SV";
            clientInformation.Locale.Name = "Swedish (Sweden)";
            clientInformation.Locale.NativeName = "svenska (Sverige)";
            clientInformation.Token = null;
            context = new WebServiceContext(clientInformation);
            Assert.IsNotNull(context);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorUserError()
        {
            WebClientInformation clientInformation;
            WebClientToken clientToken;
            WebServiceContext context;

            clientToken = new WebClientToken("NoUser",
                                             Settings.Default.UserAdminApplicationIdentifier,
                                             WebServiceData.WebServiceManager.Key);
            clientInformation = new WebClientInformation();
            clientInformation.Locale = new WebLocale();
            clientInformation.Locale.Id = 581;
            clientInformation.Locale.ISOCode = "se-SV";
            clientInformation.Locale.Name = "Swedish (Sweden)";
            clientInformation.Locale.NativeName = "svenska (Sverige)";
            clientInformation.Token = clientToken.Token;
            context = new WebServiceContext(clientInformation);
            Assert.IsNotNull(context);
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
            Object cachedObject;

            cachedObject = GetContext().GetCachedObject("No key GetCachedObject");
            Assert.IsNull(cachedObject);
        }

        [TestMethod]
        public void GetClientIPAddress()
        {
            Assert.IsTrue(WebServiceContext.GetClientIpAddress().IsNotEmpty());
        }

        [TestMethod]
        public void GetDatabase()
        {
            WebServiceDataServer database;

            database = GetContext().GetDatabase();
            Assert.IsNotNull(database);
        }

        public static WebServiceContext GetOneWebServiceContext()
        {
            WebClientInformation clientInformation;
            WebClientToken clientToken;

            clientToken = new WebClientToken(Settings.Default.TestUserName,
                                             Settings.Default.UserAdminApplicationIdentifier,
                                             WebServiceData.WebServiceManager.Key);
            clientInformation = new WebClientInformation();
            clientInformation.Locale = new WebLocale();
            clientInformation.Locale.Id = 581;
            clientInformation.Locale.ISOCode = "se-SV";
            clientInformation.Locale.Name = "Swedish (Sweden)";
            clientInformation.Locale.NativeName = "svenska (Sverige)";
            clientInformation.Token = clientToken.Token;
            return new WebServiceContext(clientInformation);
        }

        [TestMethod]
        public void GetTransactionDatabase()
        {
            WebServiceDataServer database;

            GetContext().StartTransaction(1);
            database = GetContext().GetTransactionDatabase();
            Assert.IsNotNull(database);
            GetContext().RollbackTransaction();
            database = GetContext().GetTransactionDatabase();
            Assert.IsNull(database);
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
            Assert.AreNotEqual(GetContext().RequestId, GetOneWebServiceContext().RequestId);
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
            Assert.AreNotEqual(GetContext().SessionId, GetOneWebServiceContext().SessionId);
        }

        [TestMethod]
        public void StartTrace()
        {
            List<WebLogRow> logRows;

            WebServiceData.LogManager.DeleteTrace(GetContext());

            GetContext().StartTrace(null);
            using (WebServiceContext context = new WebServiceContext(Settings.Default.TestUserName, Settings.Default.UserAdminApplicationIdentifier))
            {
                // Do something.
            }
            GetContext().StopTrace();
            logRows = WebServiceData.LogManager.GetLog(GetContext(), LogType.Trace, Settings.Default.TestUserName, 1);
            Assert.IsTrue(logRows.IsNotEmpty());
            WebServiceData.LogManager.DeleteTrace(GetContext());

            GetContext().StartTrace(Settings.Default.TestUserName);
            using (WebServiceContext context = new WebServiceContext(Settings.Default.TestUserName, Settings.Default.UserAdminApplicationIdentifier))
            {
                // Do something.
            }
            logRows = WebServiceData.LogManager.GetLog(GetContext(), LogType.Trace, Settings.Default.TestUserName, 1);
            GetContext().StopTrace();
            Assert.IsTrue(logRows.IsNotEmpty());
            WebServiceData.LogManager.DeleteTrace(GetContext());
        }

        [TestMethod]
        public void StartTransaction()
        {
            WebServiceContext context = null;

            GetContext().StartTransaction(15);
            GetContext().CommitTransaction();

            try
            {
                context = GetOneWebServiceContext();
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

            WebServiceData.LogManager.DeleteTrace(GetContext());
            GetContext().StartTrace(null);
            using (WebServiceContext context = new WebServiceContext(Settings.Default.TestUserName, Settings.Default.UserAdminApplicationIdentifier))
            {
                // Do something.
            }
            GetContext().StopTrace();
            logRows1 = WebServiceData.LogManager.GetLog(GetContext(), LogType.Trace, Settings.Default.TestUserName, 1);
            Assert.IsTrue(logRows1.IsNotEmpty());

            // Check that trace is stopped.
            using (WebServiceContext context = new WebServiceContext(Settings.Default.TestUserName, Settings.Default.UserAdminApplicationIdentifier))
            {
                // Do something.
            }
            logRows2 = WebServiceData.LogManager.GetLog(GetContext(), LogType.Trace, Settings.Default.TestUserName, 1);
            Assert.IsTrue(logRows2.IsNotEmpty());
            Assert.AreEqual(logRows1.Count, logRows2.Count);

            WebServiceData.LogManager.DeleteTrace(GetContext());
        }
    }
}

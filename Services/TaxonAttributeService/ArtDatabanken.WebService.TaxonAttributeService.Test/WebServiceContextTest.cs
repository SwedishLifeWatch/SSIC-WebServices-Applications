using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Caching;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.TaxonAttributeService.Test
{
    using ArtDatabanken.Data;

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
        public void CheckTransaction()
        {
            GetContext().StartTransaction(1);
            GetContext().CheckTransaction();
            GetContext().RollbackTransaction();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void CheckTransactionNoTransactionerror()
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
                                             ApplicationIdentifier.EVA.ToString(),
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
                                            ApplicationIdentifier.EVA.ToString());
            Assert.IsNotNull(context);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorCurrentRoleError()
        {
            WebClientInformation clientInformation;
            WebClientToken clientToken;
            WebServiceContext context;

            clientToken = new WebClientToken(Settings.Default.TestUserName,
                                             ApplicationIdentifier.EVA.ToString(),
                                             WebServiceData.WebServiceManager.Key);
            clientInformation = new WebClientInformation();
            clientInformation.Role = new WebRole();
            clientInformation.Role.Id = -42;
            clientInformation.Role.Name = "No role name";
            clientInformation.Locale = new WebLocale();
            clientInformation.Locale.Id = 581;
            clientInformation.Locale.ISOCode = "se-SV";
            clientInformation.Locale.Name = "Swedish (Sweden)";
            clientInformation.Locale.NativeName = "svenska (Sverige)";
            clientInformation.Token = clientToken.Token;
            context = new WebServiceContextCached(clientInformation);
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
                                             ApplicationIdentifier.EVA.ToString(),
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
        public void CurrentRole()
        {
            WebClientInformation clientInformation;
            WebClientToken clientToken;
            WebServiceContext context;

            clientToken = new WebClientToken(Settings.Default.TestUserName,
                                             ApplicationIdentifier.UserAdmin.ToString(),
                                             WebServiceData.WebServiceManager.Key);
            clientInformation = new WebClientInformation();
            clientInformation.Role = new WebRole();
            clientInformation.Role.Id = Settings.Default.TestRoleId;
            clientInformation.Role.Name = "No role name";
            clientInformation.Locale = new WebLocale();
            clientInformation.Locale.Id = 581;
            clientInformation.Locale.ISOCode = "se-SV";
            clientInformation.Locale.Name = "Swedish (Sweden)";
            clientInformation.Locale.NativeName = "svenska (Sverige)";
            clientInformation.Token = clientToken.Token;
            context = new WebServiceContextCached(clientInformation);
            Assert.IsNotNull(context.CurrentRole);
            Assert.AreEqual(Settings.Default.TestRoleId, context.CurrentRole.Id);
        }

        [TestMethod]
        public void CurrentRoles()
        {
            WebClientInformation clientInformation;
            WebClientToken clientToken;
            WebServiceContext context;

            // Test without selected role
            clientToken = new WebClientToken(Settings.Default.TestUserName,
                                             ApplicationIdentifier.UserAdmin.ToString(),
                                             WebServiceData.WebServiceManager.Key);
            clientInformation = new WebClientInformation();
            clientInformation.Locale = new WebLocale();
            clientInformation.Locale.Id = 581;
            clientInformation.Locale.ISOCode = "se-SV";
            clientInformation.Locale.Name = "Swedish (Sweden)";
            clientInformation.Locale.NativeName = "svenska (Sverige)";
            clientInformation.Token = clientToken.Token;
            context = new WebServiceContextCached(clientInformation);
            Assert.IsTrue(context.CurrentRoles.IsNotEmpty());
            Assert.IsTrue(1 < context.CurrentRoles.Count);

            // Test with selected role.
            clientToken = new WebClientToken(Settings.Default.TestUserName,
                                             ApplicationIdentifier.UserAdmin.ToString(),
                                             WebServiceData.WebServiceManager.Key);
            clientInformation = new WebClientInformation();
            clientInformation.Role = new WebRole();
            clientInformation.Role.Id = Settings.Default.TestRoleId;
            clientInformation.Role.Name = "No role name";
            clientInformation.Locale = new WebLocale();
            clientInformation.Locale.Id = 581;
            clientInformation.Locale.ISOCode = "se-SV";
            clientInformation.Locale.Name = "Swedish (Sweden)";
            clientInformation.Locale.NativeName = "svenska (Sverige)";
            clientInformation.Token = clientToken.Token;
            context = new WebServiceContextCached(clientInformation);
            Assert.IsTrue(context.CurrentRoles.IsNotEmpty());
            Assert.AreEqual(1, context.CurrentRoles.Count);
            Assert.AreEqual(Settings.Default.TestRoleId, context.CurrentRoles[0].Id);
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
            Assert.IsNull(GetContext().GetCachedObject("No key GetCachedObject"));
        }

        [TestMethod]
        public void GetClientIpAddress()
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

        private static WebServiceContext GetOneWebServiceContext()
        {
            WebClientInformation clientInformation;
            WebClientToken clientToken;

            clientToken = new WebClientToken(Settings.Default.TestUserName,
                                             ApplicationIdentifier.EVA.ToString(),
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
    }
}

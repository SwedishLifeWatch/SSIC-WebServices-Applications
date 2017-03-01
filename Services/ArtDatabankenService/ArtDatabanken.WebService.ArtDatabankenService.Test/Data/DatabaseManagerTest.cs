using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class DatabaseManagerTest : TestBase
    {
        public DatabaseManagerTest()
        {
        }

        [TestMethod]
        public void CheckDatabaseUpdate()
        {
            DatabaseManager.CheckDatabaseUpdate(GetContext());
        }

        [TestMethod]
        public void GetDatabases()
        {
            List<WebDatabase> databases;

            databases = DatabaseManager.GetDatabases(GetContext());
            Assert.IsTrue(databases.IsNotEmpty());
        }

        [TestMethod]
        public void GetDatabaseUpdate()
        {
            WebDatabaseUpdate databaseUpdate;

            databaseUpdate = DatabaseManager.GetDatabaseUpdate(GetContext());
            Assert.IsNotNull(databaseUpdate);
            Assert.IsTrue(databaseUpdate.UpdateStart.Hour >= 0);
            Assert.IsTrue(databaseUpdate.UpdateStart.Hour <= 23);
            Assert.IsTrue(databaseUpdate.UpdateEnd.Hour >= 0);
            Assert.IsTrue(databaseUpdate.UpdateEnd.Hour <= 23);
        }

        public static WebDatabase GetOneDatabase(WebServiceContext context)
        {
            return DatabaseManager.GetDatabases(context)[0];
        }

        public static WebDatabaseUpdate GetOneDatabaseUpdate(WebServiceContext context)
        {
            return DatabaseManager.GetDatabaseUpdate(context);
        }
    }
}

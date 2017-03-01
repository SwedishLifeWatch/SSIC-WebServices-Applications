using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class DatabaseManagerTest : TestBase
    {
        public DatabaseManagerTest()
        {
        }

        #region Additional test attributes
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #endregion

        public static DatabaseList GetAllDatabases()
        {
            return DatabaseManager.GetDatabases();
        }

        public static Data.ArtDatabankenService.Database GetDatabase()
        {
            return DatabaseManager.GetDatabases()[0];
        }

        [TestMethod]
        public void GetDatabases()
        {
            DatabaseList databases;

            databases = DatabaseManager.GetDatabases();
            Assert.IsTrue(databases.IsNotEmpty());
        }
    }
}

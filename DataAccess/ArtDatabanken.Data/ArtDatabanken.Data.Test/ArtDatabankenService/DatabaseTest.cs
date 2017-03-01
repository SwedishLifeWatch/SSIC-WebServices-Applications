using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class DatabaseTest : TestBase
    {
        private Data.ArtDatabankenService.Database _database;

        public DatabaseTest()
        {
            _database = null;
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

        [TestMethod]
        public void Constructor()
        {
            Data.ArtDatabankenService.Database database;

            database = GetDatabase(true);
            Assert.IsNotNull(database);
        }

        private Data.ArtDatabankenService.Database GetDatabase(Boolean refresh = false)
        {
            if (_database.IsNull() || refresh)
            {
                _database = DatabaseManagerTest.GetDatabase();
            }
            return _database;
        }

        [TestMethod]
        public void LongName()
        {
            Assert.IsTrue(GetDatabase(true).LongName.IsNotEmpty());
        }

        [TestMethod]
        public void ShortName()
        {
            Assert.IsTrue(GetDatabase(true).ShortName.IsNotEmpty());
        }
    }
}

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class DatabaseListTest : TestBase
    {
        private DatabaseList _databases;

        public DatabaseListTest()
        {
            _databases = null;
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
        public void Get()
        {
            foreach (Data.ArtDatabankenService.Database database in GetDatabases(true))
            {
                Assert.AreEqual(database, GetDatabases().Get(database.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 databaseId;

            databaseId = Int32.MinValue;
            GetDatabases(true).Get(databaseId);
        }

        private DatabaseList GetDatabases()
        {
            return GetDatabases(false);
        }

        private DatabaseList GetDatabases(Boolean refresh)
        {
            if (_databases.IsNull() || refresh)
            {
                _databases = DatabaseManagerTest.GetAllDatabases();
            }
            return _databases;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            Int32 databaseIndex;
            DatabaseList newDatabaseList, oldDatabaseList;

            oldDatabaseList = GetDatabases(true);
            newDatabaseList = new DatabaseList();
            for (databaseIndex = 0; databaseIndex < oldDatabaseList.Count; databaseIndex++)
            {
                newDatabaseList.Add(oldDatabaseList[oldDatabaseList.Count - databaseIndex - 1]);
            }
            for (databaseIndex = 0; databaseIndex < oldDatabaseList.Count; databaseIndex++)
            {
                Assert.AreEqual(newDatabaseList[databaseIndex], oldDatabaseList[oldDatabaseList.Count - databaseIndex - 1]);
            }
        }
    }
}

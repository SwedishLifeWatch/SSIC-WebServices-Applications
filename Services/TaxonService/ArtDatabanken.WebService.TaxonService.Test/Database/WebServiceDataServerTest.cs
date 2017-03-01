using System;
using ArtDatabanken.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Database;
using ArtDatabanken.WebService.TaxonService.Database;

namespace ArtDatabanken.WebService.TaxonService.Test.Database
{
    /// <summary>
    /// Test of the abtract class
    /// ArtDatabanken.WebService.Database.WebServiceDataServer.
    /// </summary>
    [TestClass]
    public class WebServiceDataServerTest
    {
        private WebServiceDataServer _database;

        public WebServiceDataServerTest()
        {
            _database = null;
        }

        [TestMethod]
        public void BeginTransaction()
        {
            using (WebServiceDataServer database = new TaxonServer())
            {
                database.BeginTransaction();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void BeginTransactionAlreadyStartedError()
        {
            using (WebServiceDataServer database = new TaxonServer())
            {
                database.BeginTransaction();
                database.BeginTransaction();
            }
        }

        [TestMethod]
        public void ClearCache()
        {
            Int32 columnLength;
            String columnName, tableName;

            WebServiceDataServer.ClearCache();

            tableName = WebServiceLogData.TABLE_NAME;
            columnName = WebServiceLogData.TEXT;
            columnLength = GetDatabase(true).GetColumnLength(tableName, columnName);
            Assert.IsTrue(0 < columnLength);

            WebServiceDataServer.ClearCache();

            tableName = WebServiceLogData.TABLE_NAME;
            columnName = WebServiceLogData.SQL_SERVER_USER;
            columnLength = GetDatabase().GetColumnLength(tableName, columnName);
            Assert.IsTrue(0 < columnLength);
        }

        [TestMethod]
        public void CommandTimeout()
        {
            Int32 commandTimeout;

            commandTimeout = -43234;
            GetDatabase(true).CommandTimeout = commandTimeout;
            Assert.AreEqual(commandTimeout, GetDatabase().CommandTimeout);
            commandTimeout = 0;
            GetDatabase().CommandTimeout = commandTimeout;
            Assert.AreEqual(commandTimeout, GetDatabase().CommandTimeout);
            commandTimeout = 5345345;
            GetDatabase().CommandTimeout = commandTimeout;
            Assert.AreEqual(commandTimeout, GetDatabase().CommandTimeout);
        }

        [TestMethod]
        public void CommitTransaction()
        {
            using (WebServiceDataServer database = new TaxonServer())
            {
                database.BeginTransaction();
                database.CommitTransaction();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void CommitTransactionNoTransactionError()
        {
            using (WebServiceDataServer database = new TaxonServer())
            {
                database.CommitTransaction();
            }
        }

        [TestMethod]
        public void Constructor()
        {
            using (WebServiceDataServer database = new TaxonServer())
            {
                Assert.IsNotNull(database);
            }
        }

        [TestMethod]
        public void DeleteTrace()
        {
            GetDatabase(true).DeleteTrace();
        }

        [TestMethod]
        public void Disconnect()
        {
            using (WebServiceDataServer database = new TaxonServer())
            {
                database.Disconnect();

                // Should be ok to disconnect an already disconnected database.
                database.Disconnect();
            }
        }

        [TestMethod]
        public void Dispose()
        {
            using (WebServiceDataServer database = new TaxonServer())
            {
                database.Dispose();

                // Should be ok to dispose an already disposed database.
                database.Dispose();
            }
        }

        [TestMethod]
        public void GetColumnLength()
        {
            Int32 columnLength;
            String columnName, tableName;

            tableName = WebServiceLogData.TABLE_NAME;
            columnName = WebServiceLogData.TCP_IP;
            columnLength = GetDatabase(true).GetColumnLength(tableName, columnName);
            Assert.IsTrue(0 < columnLength);

            tableName = WebServiceLogData.TABLE_NAME;
            columnName = WebServiceLogData.INFORMATION;
            columnLength = GetDatabase().GetColumnLength(tableName, columnName);
            Assert.IsTrue(0 < columnLength);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void GetColumnLengthWrongNameError()
        {
            Int32 columnLength;
            String columnName, tableName;

            tableName = "NoTable";
            columnName = "NoColumn";
            columnLength = GetDatabase().GetColumnLength(tableName, columnName);
            Assert.AreEqual(0, columnLength);
        }

        private WebServiceDataServer GetDatabase()
        {
            return GetDatabase(false);
        }

        private WebServiceDataServer GetDatabase(Boolean refresh)
        {
            if (_database.IsNull() || refresh)
            {
                if (_database.IsNotNull())
                {
                    _database.Dispose();
                }
                _database = new TaxonServer();
                _database.BeginTransaction();
            }
            return _database;
        }

        [TestMethod]
        public void GetLog()
        {
            using (DataReader dataReader = GetDatabase(true).GetLog(null, null, 10))
            {
                Assert.IsTrue(dataReader.Read());
            }
            using (DataReader dataReader = GetDatabase().GetLog("Error", null, 10))
            {
                Assert.IsTrue(dataReader.Read());
            }
            using (DataReader dataReader = GetDatabase().GetLog(null, Settings.Default.TestUserName, 10))
            {
                Assert.IsTrue(dataReader.Read());
            }
            using (DataReader dataReader = GetDatabase().GetLog("Error", Settings.Default.TestUserName, 10))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void HasPendingTransaction()
        {
            using (WebServiceDataServer database = new TaxonServer())
            {
                Assert.IsFalse(database.HasPendingTransaction());
                database.BeginTransaction();
                Assert.IsTrue(database.HasPendingTransaction());
            }
        }

        [TestMethod]
        public void RollbackTransaction()
        {
            using (WebServiceDataServer database = new TaxonServer())
            {
                database.BeginTransaction();
                database.RollbackTransaction();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void RollbackTransactionNoTransactionError()
        {
            using (WebServiceDataServer database = new TaxonServer())
            {
                database.RollbackTransaction();
            }
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void TestCleanup()
        {
            if (_database.IsNotNull())
            {
                _database.RollbackTransaction();
                _database.Dispose();
                _database = null;
            }
        }

        [TestMethod]
        public void UpdateLog()
        {
            // Check that the entry is not in the log.
            using (DataReader dataReader = GetDatabase().GetLog("AutomaticTest", Settings.Default.TestUserName, 10))
            {
                Assert.IsFalse(dataReader.Read());
            }

            // Add entry to the log.
            GetDatabase(true).UpdateLog("Have a nice day!",
                                        "AutomaticTest",
                                        null,
                                        Settings.Default.TestUserName,
                                        Settings.Default.TestApplicationIdentifier,
                                        "127.0.0.1");

            // Check that the entry is in the log.
            using (DataReader dataReader = GetDatabase().GetLog("AutomaticTest", Settings.Default.TestUserName, 10))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }
    }
}

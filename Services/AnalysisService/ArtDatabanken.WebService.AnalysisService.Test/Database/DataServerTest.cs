using System;
using ArtDatabanken.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.AnalysisService.Database;

namespace ArtDatabanken.WebService.AnalysisService.Test.Database
{
    /// <summary>
    /// Test of the abtract class
    /// ArtDatabanken.WebService.Database.DataServer.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable"), TestClass]
    public class DataServerTest 
    {
        private DataServer _database;

        public DataServerTest()
        {
            _database = null;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void BeginTransaction()
        {
            using (DataServer database = new AnalysisServer())
            {
                database.BeginTransaction();
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ApplicationException))]
        public void BeginTransactionAlreadyStartedError()
        {
            using (DataServer database = new AnalysisServer())
            {
                database.BeginTransaction();
                database.BeginTransaction();
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
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
        [TestCategory("NightlyTest")]
        public void CommitTransaction()
        {
            using (DataServer database = new AnalysisServer())
            {
                database.BeginTransaction();
                database.CommitTransaction();
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ApplicationException))]
        public void CommitTransactionNoTransactionError()
        {
            using (DataServer database = new AnalysisServer())
            {
                database.CommitTransaction();
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void Constructor()
        {
            using (DataServer database = new AnalysisServer())
            {
                Assert.IsNotNull(database);
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void Disconnect()
        {
            using (DataServer database = new AnalysisServer())
            {
                database.Disconnect();

                // Should be ok to disconnect an already disconnected database.
                database.Disconnect();
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void Dispose()
        {
            using (DataServer database = new AnalysisServer())
            {
                database.Dispose();

                // Should be ok to dispose an already disposed database.
                database.Dispose();
            }
        }

        private DataServer GetDatabase()
        {
            return GetDatabase(false);
        }

        private DataServer GetDatabase(Boolean refresh)
        {
            if (_database.IsNull() || refresh)
            {
                if (_database.IsNotNull())
                {
                    _database.Dispose();
                }
                _database = new AnalysisServer();
                _database.BeginTransaction();
            }
            return _database;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void HasPendingTransaction()
        {
            using (DataServer database = new AnalysisServer())
            {
                Assert.IsFalse(database.HasPendingTransaction());
                database.BeginTransaction();
                Assert.IsTrue(database.HasPendingTransaction());
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void RollbackTransaction()
        {
            using (DataServer database = new AnalysisServer())
            {
                database.BeginTransaction();
                database.RollbackTransaction();
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ApplicationException))]
        public void RollbackTransactionNoTransactionError()
        {
            using (DataServer database = new AnalysisServer())
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
    }
}

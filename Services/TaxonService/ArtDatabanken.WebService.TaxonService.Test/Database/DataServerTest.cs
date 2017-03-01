using System;
using ArtDatabanken.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.TaxonService.Database;

namespace ArtDatabanken.WebService.TaxonService.Test.Database
{
    /// <summary>
    /// Test of the abtract class
    /// ArtDatabanken.WebService.Database.DataServer.
    /// </summary>
    [TestClass]
    public class DataServerTest
    {
        private DataServer _database;

        public DataServerTest()
        {
            _database = null;
        }

        [TestMethod]
        public void BeginTransaction()
        {
            using (DataServer database = new TaxonServer())
            {
                database.BeginTransaction();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void BeginTransactionAlreadyStartedError()
        {
            using (DataServer database = new TaxonServer())
            {
                database.BeginTransaction();
                database.BeginTransaction();
            }
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
            using (DataServer database = new TaxonServer())
            {
                database.BeginTransaction();
                database.CommitTransaction();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void CommitTransactionNoTransactionError()
        {
            using (DataServer database = new TaxonServer())
            {
                database.CommitTransaction();
            }
        }

        [TestMethod]
        public void Constructor()
        {
            using (DataServer database = new TaxonServer())
            {
                Assert.IsNotNull(database);
            }
        }

        [TestMethod]
        public void Disconnect()
        {
            using (DataServer database = new TaxonServer())
            {
                database.Disconnect();

                // Should be ok to disconnect an already disconnected database.
                database.Disconnect();
            }
        }

        [TestMethod]
        public void Dispose()
        {
            using (DataServer database = new TaxonServer())
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
                _database = new TaxonServer();
                _database.BeginTransaction();
            }
            return _database;
        }

        [TestMethod]
        public void HasPendingTransaction()
        {
            using (DataServer database = new TaxonServer())
            {
                Assert.IsFalse(database.HasPendingTransaction());
                database.BeginTransaction();
                Assert.IsTrue(database.HasPendingTransaction());
            }
        }

        [TestMethod]
        public void RollbackTransaction()
        {
            using (DataServer database = new TaxonServer())
            {
                database.BeginTransaction();
                database.RollbackTransaction();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void RollbackTransactionNoTransactionError()
        {
            using (DataServer database = new TaxonServer())
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

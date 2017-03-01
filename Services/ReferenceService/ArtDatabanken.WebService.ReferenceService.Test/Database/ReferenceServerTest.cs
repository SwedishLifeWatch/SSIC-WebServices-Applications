using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.ReferenceService.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.ReferenceService.Test.Database
{
    [TestClass]
    public class ReferenceServerTest : TestBase
    {
        private ReferenceServer _database;

        public ReferenceServerTest()
        {
            _database = null;
        }

        [TestMethod]
        public void CreateReference()
        {
            GetDatabase(true).CreateReference("AutoInsertTestName", 1966, "AutoInsertTestText", "AutoTestPerson");
        }

        [TestMethod]
        public void CreateReferenceRelation()
        {
            Int32 referenceRelationId;

            referenceRelationId = GetDatabase(true).CreateReferenceRelation("AutoInsertTestName", 1, 1);
            Assert.IsTrue(100 < referenceRelationId);
        }

        [TestMethod]
        public void DeleteReferenceRelation()
        {
            Int32 referenceRelationId;

            referenceRelationId = GetDatabase(true).CreateReferenceRelation("AutoInsertTestName", 1, 1);
            Assert.IsTrue(100 < referenceRelationId);
            GetDatabase().DeleteReferenceRelation(referenceRelationId);
        }

        [TestMethod]
        public void GetAddress()
        {
            String address;

            address = ReferenceServer.GetAddress();
            Assert.IsTrue(address.IsNotEmpty());
        }

        private ReferenceServer GetDatabase(Boolean refresh = false)
        {
            if (_database.IsNull() || refresh)
            {
                if (_database.IsNotNull())
                {
                    _database.Dispose();
                }

                _database = new ReferenceServer();
                _database.BeginTransaction();
            }

            return _database;
        }

        [TestMethod]
        public void GetReferenceRelationById()
        {
            using (DataReader dataReader = GetDatabase(true).GetReferenceRelationById(1))
            {
                Assert.IsTrue(dataReader.Read());
                Assert.IsFalse(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetReferenceRelationsByGuid()
        {
            using (DataReader dataReader = GetDatabase(true).GetReferenceRelationsByGuid("urn:lsid:dyntaxa.se:LumpSplitEvent:0"))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetReferenceRelationTypes()
        {
            using (DataReader dataReader = GetDatabase(true).GetReferenceRelationTypes((Int32)(LocaleId.en_GB)))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetReferences()
        {
            using (DataReader dataReader = GetDatabase(true).GetReferences())
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetReferencesByIds()
        {
            List<Int32> referenceIds;

            referenceIds = new List<Int32>();
            referenceIds.Add(1);
            referenceIds.Add(2);
            using (DataReader dataReader = GetDatabase(true).GetReferencesByIds(referenceIds))
            {
                Assert.IsTrue(dataReader.Read());
                Assert.IsTrue(dataReader.Read());
                Assert.IsFalse(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetReferencesBySearchCriteria()
        {
            String whereCondition;

            whereCondition = " (" + ReferenceData.YEAR_COLUMN_NAME + " = 2003) ";
            using (DataReader dataReader = GetDatabase(true).GetReferencesBySearchCriteria(whereCondition))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public new void TestCleanup()
        {
            if (_database.IsNotNull())
            {
                _database.RollbackTransaction();
                _database.Dispose();
                _database = null;
            }
        }

        [TestMethod]
        public void UpdateReference()
        {
            GetDatabase(true).UpdateReference(Settings.Default.JanEdelsjoReferenceId,
                                              "AutotestName",
                                              1966,
                                              "AutotestText",
                                              "AutoTestPerson");
        }
    }
}

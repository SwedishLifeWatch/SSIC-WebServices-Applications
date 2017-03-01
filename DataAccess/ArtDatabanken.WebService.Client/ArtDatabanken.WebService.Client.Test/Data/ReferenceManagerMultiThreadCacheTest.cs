using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.ReferenceService;
using ArtDatabanken.WebService.Client.TaxonService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class ReferenceManagerMultiThreadCacheTest : TestBase
    {
        private ReferenceManagerMultiThreadCache _referenceManager;

        public ReferenceManagerMultiThreadCacheTest()
        {
            _referenceManager = null;
        }

        private ReferenceManager GetReferenceManager(Boolean refresh = false)
        {
            if (_referenceManager.IsNull() || refresh)
            {
                _referenceManager = new ReferenceManagerMultiThreadCache();
                _referenceManager.DataSource = new ReferenceDataSource();
            }

            return _referenceManager;
        }

        [TestMethod]
        public void GetReferenceRelationTypes()
        {
            Int64 durationFirst, durationSecond;
            ReferenceRelationTypeList referenceRelationTypes;

            Stopwatch.Start();
            referenceRelationTypes = GetReferenceManager(true).GetReferenceRelationTypes(GetUserContext());
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(referenceRelationTypes.IsNotEmpty());

            Stopwatch.Start();
            referenceRelationTypes = GetReferenceManager().GetReferenceRelationTypes(GetUserContext());
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(referenceRelationTypes.IsNotEmpty());

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }

        [TestMethod]
        public void GetReferencesByIds()
        {
            Int32 index;
            Int64 durationFirst, durationSecond;
            List<Int32> referenceIds;
            ReferenceList references;

            referenceIds = new List<Int32>();
            referenceIds.Add(1);
            referenceIds.Add(2);

            Stopwatch.Start();
            references = GetReferenceManager(true).GetReferences(GetUserContext(), referenceIds);
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(references.IsNotEmpty());
            Assert.AreEqual(referenceIds.Count, references.Count);
            for (index = 0; index < referenceIds.Count; index++)
            {
                Assert.AreEqual(referenceIds[index], references[index].Id);
            }

            Stopwatch.Start();
            references = GetReferenceManager().GetReferences(GetUserContext(), referenceIds);
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(references.IsNotEmpty());
            Assert.AreEqual(referenceIds.Count, references.Count);
            for (index = 0; index < referenceIds.Count; index++)
            {
                Assert.AreEqual(referenceIds[index], references[index].Id);
            }

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }

        override protected String GetTestApplicationName()
        {
            return ApplicationIdentifier.Dyntaxa.ToString();
        }
    }
}

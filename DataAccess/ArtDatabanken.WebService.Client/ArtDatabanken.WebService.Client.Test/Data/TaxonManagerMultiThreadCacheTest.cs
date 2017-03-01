using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Client.TaxonService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class TaxonManagerMultiThreadCacheTest : TestBase
    {
        private TaxonManagerMultiThreadCache _taxonManager;

        public TaxonManagerMultiThreadCacheTest()
        {
            _taxonManager = null;
        }

        [TestMethod]
        public void GetLumpSplitEventTypes()
        {
            Int64 durationFirst, durationSecond;
            LumpSplitEventTypeList lumpSplitEventTypes;

            Stopwatch.Start();
            lumpSplitEventTypes = GetTaxonManager(true).GetLumpSplitEventTypes(GetUserContext());
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(lumpSplitEventTypes.IsNotEmpty());

            Stopwatch.Start();
            lumpSplitEventTypes = GetTaxonManager().GetLumpSplitEventTypes(GetUserContext());
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(lumpSplitEventTypes.IsNotEmpty());

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }

        private TaxonManagerMultiThreadCache GetTaxonManager(Boolean refresh = false)
        {
            if (_taxonManager.IsNull() || refresh)
            {
                _taxonManager = new TaxonManagerMultiThreadCache();
                _taxonManager.DataSource = new TaxonDataSource();
            }
            return _taxonManager;
        }

        [TestMethod]
        public void GetTaxaByIds()
        {
            List<Int32> taxonIds;
            Int64 durationFirst, durationSecond;
            TaxonList taxa;

            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.Bear));
            taxonIds.Add((Int32)(TaxonId.Mammals));
            taxonIds.Add((Int32)(TaxonId.Wolverine));

            Stopwatch.Start();
            taxa = GetTaxonManager(true).GetTaxa(GetUserContext(), taxonIds);
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(taxa.IsNotEmpty());
            Assert.AreEqual(taxonIds.Count, taxa.Count);

            Stopwatch.Start();
            taxa = GetTaxonManager().GetTaxa(GetUserContext(), taxonIds);
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(taxa.IsNotEmpty());
            Assert.AreEqual(taxonIds.Count, taxa.Count);

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }

        [TestMethod]
        public void GetTaxonAlertStatuses()
        {
            Int64 durationFirst, durationSecond;
            TaxonAlertStatusList taxonAlertStatuses;

            Stopwatch.Start();
            taxonAlertStatuses = GetTaxonManager(true).GetTaxonAlertStatuses(GetUserContext());
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(taxonAlertStatuses.IsNotEmpty());

            Stopwatch.Start();
            taxonAlertStatuses = GetTaxonManager().GetTaxonAlertStatuses(GetUserContext());
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(taxonAlertStatuses.IsNotEmpty());

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }

        [TestMethod]
        public void GetTaxonById()
        {
            Int32 taxonId;
            Int64 durationFirst, durationSecond;
            ITaxon taxon;

            taxonId = (Int32)(TaxonId.Bear);

            Stopwatch.Start();
            taxon = GetTaxonManager(true).GetTaxon(GetUserContext(), taxonId);
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsNotNull(taxon);
            Assert.AreEqual(taxonId, taxon.Id);

            Stopwatch.Start();
            taxon = GetTaxonManager().GetTaxon(GetUserContext(), taxonId);
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsNotNull(taxon);
            Assert.AreEqual(taxonId, taxon.Id);

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }

        [TestMethod]
        public void GetTaxonCategories()
        {
            TaxonCategoryList taxonCategories;

            taxonCategories = GetTaxonManager(true).GetTaxonCategories(GetUserContext());
            Assert.IsNotNull(taxonCategories);
            Assert.IsTrue(0 < taxonCategories.Count);
        }

        [TestMethod]
        public void GetTaxonChangeStatuses()
        {
            Int64 durationFirst, durationSecond;
            TaxonChangeStatusList taxonChangeStatuses;

            Stopwatch.Start();
            taxonChangeStatuses = GetTaxonManager(true).GetTaxonChangeStatuses(GetUserContext());
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(taxonChangeStatuses.IsNotEmpty());

            Stopwatch.Start();
            taxonChangeStatuses = GetTaxonManager().GetTaxonChangeStatuses(GetUserContext());
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(taxonChangeStatuses.IsNotEmpty());

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }

        [TestMethod]
        public void GetTaxonNameCategories()
        {
            Int64 durationFirst, durationSecond;
            TaxonNameCategoryList taxonNameCategories;

            Stopwatch.Start();
            taxonNameCategories = GetTaxonManager(true).GetTaxonNameCategories(GetUserContext());
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(taxonNameCategories.IsNotEmpty());

            Stopwatch.Start();
            taxonNameCategories = GetTaxonManager().GetTaxonNameCategories(GetUserContext());
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(taxonNameCategories.IsNotEmpty());

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }

        [TestMethod]
        public void GetTaxonNameCategoryTypes()
        {
            Int64 durationFirst, durationSecond;
            TaxonNameCategoryTypeList taxonNameCategoryTypes;

            Stopwatch.Start();
            taxonNameCategoryTypes = GetTaxonManager(true).GetTaxonNameCategoryTypes(GetUserContext());
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(taxonNameCategoryTypes.IsNotEmpty());

            Stopwatch.Start();
            taxonNameCategoryTypes = GetTaxonManager().GetTaxonNameCategoryTypes(GetUserContext());
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(taxonNameCategoryTypes.IsNotEmpty());

            Assert.IsTrue(durationSecond < (durationFirst / 10));
        }

        [TestMethod]
        public void GetTaxonNames()
        {
            ITaxon taxon;
            TaxonNameList taxonNames;

            taxon = GetTaxonManager(true).GetTaxon(GetUserContext(), TaxonId.Bear);
            taxonNames = GetTaxonManager().GetTaxonNames(GetUserContext(), taxon);
            Assert.IsTrue(taxonNames.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonNameStatus()
        {
            TaxonNameStatusList taxonNameStatus;

            taxonNameStatus = GetTaxonManager(true).GetTaxonNameStatuses(GetUserContext());
            Assert.IsTrue(taxonNameStatus.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonNameUsage()
        {
            TaxonNameUsageList taxonNameUsage;

            taxonNameUsage = GetTaxonManager(true).GetTaxonNameUsages(GetUserContext());
            Assert.IsTrue(taxonNameUsage.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonRevisionEventTypes()
        {
            TaxonRevisionEventTypeList taxonRevisionEventTypes;

            taxonRevisionEventTypes = GetTaxonManager().GetTaxonRevisionEventTypes(GetUserContext());
            Assert.IsTrue(taxonRevisionEventTypes.IsNotEmpty());
            foreach (ITaxonRevisionEventType taxonRevisionEventType in taxonRevisionEventTypes)
            {
                Assert.IsTrue(0 < taxonRevisionEventType.Id);
                Assert.IsTrue(taxonRevisionEventType.Identifier.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetTaxonRevisionStates()
        {
            TaxonRevisionStateList taxonRevisionStates;

            taxonRevisionStates = GetTaxonManager().GetTaxonRevisionStates(GetUserContext());
            Assert.IsTrue(taxonRevisionStates.IsNotEmpty());
            foreach (ITaxonRevisionState taxonRevisionState in taxonRevisionStates)
            {
                Assert.IsTrue(0 < taxonRevisionState.Id);
                Assert.IsTrue(taxonRevisionState.Identifier.IsNotEmpty());
            }
        }
    }
}

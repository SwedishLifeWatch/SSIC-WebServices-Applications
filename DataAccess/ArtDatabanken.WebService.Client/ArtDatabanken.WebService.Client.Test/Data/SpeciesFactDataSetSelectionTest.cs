using System;
using ArtDatabanken.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    using System.Collections.Generic;

    [TestClass]
    public class SpeciesFactDataSetSelectionTest : TestBase
    {
        private SpeciesFactDataSetSelection _selection;

        public SpeciesFactDataSetSelectionTest()
        {
            _selection = null;
        }

        [TestMethod]
        public void Constructor()
        {
            SpeciesFactDataSetSelection selection;

            selection = new SpeciesFactDataSetSelection();
            Assert.IsNotNull(selection);
            Assert.IsNotNull(selection.Factors);
            Assert.IsNotNull(selection.Hosts);
            Assert.IsNotNull(selection.IndividualCategories);
            Assert.IsNotNull(selection.Periods);
            Assert.IsNotNull(selection.Taxa);
            Assert.IsFalse(selection.HasFactors);
            Assert.IsFalse(selection.HasHosts);
            Assert.IsFalse(selection.HasIndividualCategories);
            Assert.IsFalse(selection.HasPeriods);
            Assert.IsFalse(selection.HasTaxa);
        }

        private SpeciesFactDataSetSelection GetSelection(Boolean refresh = false)
        {
            if (_selection.IsNull() || refresh)
            {
                _selection = new SpeciesFactDataSetSelection();
            }

            return _selection;
        }

        [TestMethod]
        public void Factors()
        {
            FactorList factors;

            factors = CoreData.FactorManager.GetFactors(GetUserContext());
            GetSelection(true).Factors = factors;
            Assert.IsTrue(GetSelection().Factors.IsNotEmpty());
            Assert.AreEqual(factors.Count, GetSelection().Factors.Count);
        }

        [TestMethod]
        public void HasFactors()
        {
            FactorList factors;

            Assert.IsFalse(GetSelection(true).HasFactors);
            factors = CoreData.FactorManager.GetFactors(GetUserContext());
            GetSelection().Factors = factors;
            Assert.IsTrue(GetSelection().HasFactors);
        }

        [TestMethod]
        public void HasHosts()
        {
            List<Int32> ids;
            TaxonList hosts;

            ids = new List<Int32>();
            for (Int32 index = 0; index < 10; index++)
            {
                ids.Add(index);
            }

            Assert.IsFalse(GetSelection(true).HasHosts);
            hosts = CoreData.TaxonManager.GetTaxa(GetUserContext(), ids);
            GetSelection().Hosts = hosts;
            Assert.IsTrue(GetSelection().HasHosts);
        }

        [TestMethod]
        public void HasIndividualCategories()
        {
            IndividualCategoryList individualCategories;

            Assert.IsFalse(GetSelection(true).HasIndividualCategories);
            individualCategories = CoreData.FactorManager.GetIndividualCategories(GetUserContext());
            GetSelection().IndividualCategories = individualCategories;
            Assert.IsTrue(GetSelection().HasIndividualCategories);
        }

        [TestMethod]
        public void HasPeriods()
        {
            PeriodList periods;

            Assert.IsFalse(GetSelection(true).HasPeriods);
            periods = CoreData.FactorManager.GetPeriods(GetUserContext());
            GetSelection().Periods = periods;
            Assert.IsTrue(GetSelection().HasPeriods);
        }

        [TestMethod]
        public void HasTaxa()
        {
            List<Int32> ids;
            TaxonList taxa;

            ids = new List<Int32>();
            for (Int32 index = 0; index < 10; index++)
            {
                ids.Add(index);
            }

            Assert.IsFalse(GetSelection(true).HasTaxa);
            taxa = CoreData.TaxonManager.GetTaxa(GetUserContext(), ids);
            GetSelection().Taxa = taxa;
            Assert.IsTrue(GetSelection().HasTaxa);
        }

        [TestMethod]
        public void Hosts()
        {
            List<Int32> ids;
            TaxonList hosts;

            ids = new List<Int32>();
            for (Int32 index = 0; index < 10; index++)
            {
                ids.Add(index);
            }

            hosts = CoreData.TaxonManager.GetTaxa(GetUserContext(), ids);
            GetSelection(true).Hosts = hosts;
            Assert.IsTrue(GetSelection().Hosts.IsNotEmpty());
            Assert.AreEqual(hosts.Count, GetSelection().Hosts.Count);
        }

        [TestMethod]
        public void IndividualCategories()
        {
            IndividualCategoryList individualCategories;

            individualCategories = CoreData.FactorManager.GetIndividualCategories(GetUserContext());
            GetSelection(true).IndividualCategories = individualCategories;
            Assert.IsTrue(GetSelection().IndividualCategories.IsNotEmpty());
            Assert.AreEqual(individualCategories.Count, GetSelection().IndividualCategories.Count);
        }

        [TestMethod]
        public void Periods()
        {
            PeriodList periods;

            periods = CoreData.FactorManager.GetPeriods(GetUserContext());
            GetSelection(true).Periods = periods;
            Assert.IsTrue(GetSelection().Periods.IsNotEmpty());
            Assert.AreEqual(periods.Count, GetSelection().Periods.Count);
        }

        [TestMethod]
        public void Taxa()
        {
            List<Int32> ids;
            TaxonList taxa;

            ids = new List<Int32>();
            for (Int32 index = 0; index < 10; index++)
            {
                ids.Add(index);
            }

            taxa = CoreData.TaxonManager.GetTaxa(GetUserContext(), ids);
            GetSelection(true).Taxa = taxa;
            Assert.IsTrue(GetSelection().Taxa.IsNotEmpty());
            Assert.AreEqual(taxa.Count, GetSelection().Taxa.Count);
        }
    }
}

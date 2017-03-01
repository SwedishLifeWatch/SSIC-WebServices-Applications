using System;
using ArtDatabanken.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class SpeciesFactDataSetTest : TestBase
    {
        private SpeciesFactDataSet _dataSet;

        public SpeciesFactDataSetTest()
        {
            _dataSet = null;
        }

        [TestMethod]
        public void AddSelectionFactor()
        {
            FactorList factors;
            IFactor factor1, factor2;
            ITaxon taxon1, taxon2;
            TaxonList taxa;

            factor1 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishHistory);
            factor2 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishOccurrence);
            taxon1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.DrumGrasshopper);
            taxon2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Wolverine);
            factors = new FactorList();
            factors.Add(factor1);
            factors.Add(factor2);
            taxa = new TaxonList();
            taxa.Add(taxon1);
            taxa.Add(taxon2);

            GetDataSet(true).AddSelectionTaxa(GetUserContext(), taxa);
            Assert.IsTrue(GetDataSet().Factors.IsEmpty());
            Assert.IsTrue(GetDataSet().SpeciesFacts.IsEmpty());
            GetDataSet().AddSelection(GetUserContext(), factor1);
            Assert.IsFalse(GetDataSet().Factors.IsEmpty());
            Assert.IsFalse(GetDataSet().SpeciesFacts.IsEmpty());
            factor1 = null;
            GetDataSet().AddSelection(GetUserContext(), factor1);
        }

        [TestMethod]
        public void AddSelectionFactors()
        {
            FactorList factors;
            IFactor factor1, factor2;
            ITaxon taxon1, taxon2;
            TaxonList taxa;

            factor1 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishHistory);
            factor2 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishOccurrence);
            taxon1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.DrumGrasshopper);
            taxon2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Wolverine);
            factors = new FactorList();
            factors.Add(factor1);
            factors.Add(factor2);
            taxa = new TaxonList();
            taxa.Add(taxon1);
            taxa.Add(taxon2);

            GetDataSet(true).AddSelectionTaxa(GetUserContext(), taxa);
            Assert.IsTrue(GetDataSet().Factors.IsEmpty());
            Assert.IsTrue(GetDataSet().SpeciesFacts.IsEmpty());
            GetDataSet().AddSelection(GetUserContext(), factors);
            Assert.IsFalse(GetDataSet().Factors.IsEmpty());
            Assert.IsFalse(GetDataSet().SpeciesFacts.IsEmpty());
            factors = null;
            GetDataSet().AddSelection(GetUserContext(), factors);
        }

        [TestMethod]
        public void AddSelectionHost()
        {
            FactorList factors;
            IFactor factor1, factor2;
            Int32 speciesFactCount;
            ITaxon host, taxon1, taxon2;
            TaxonList taxa;

            factor1 = CoreData.FactorManager.GetFactor(GetUserContext(), 1137); // Svampdelar (inkl lavar).
            factor2 = CoreData.FactorManager.GetFactor(GetUserContext(), 1138); // Mycel.
            taxon1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), 100090); // Nötkråka.
            taxon2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), 100381); //tvåfläckig barkskinnbagge
            factors = new FactorList();
            factors.Add(factor1);
            factors.Add(factor2);
            taxa = new TaxonList();
            taxa.Add(taxon1);
            taxa.Add(taxon2);

            GetDataSet(true).AddSelection(GetUserContext(), factors);
            GetDataSet().AddSelectionTaxa(GetUserContext(), taxa);

            speciesFactCount = GetDataSet().SpeciesFacts.Count;
            host = CoreData.TaxonManager.GetTaxon(GetUserContext(), 230260); // skägglav.
            GetDataSet().AddSelectionHost(GetUserContext(), host);
            Assert.IsTrue(speciesFactCount > GetDataSet().SpeciesFacts.Count);
            host = null;
            GetDataSet().AddSelectionHost(GetUserContext(), host);
        }

        [TestMethod]
        public void AddSelectionHosts()
        {
            FactorList factors;
            IFactor factor1, factor2;
            Int32 speciesFactCount;
            ITaxon host1, host2, taxon1, taxon2;
            TaxonList hosts, taxa;

            factor1 = CoreData.FactorManager.GetFactor(GetUserContext(), 1137); // Svampdelar (inkl lavar).
            factor2 = CoreData.FactorManager.GetFactor(GetUserContext(), 1138); // Mycel.
            taxon1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), 100090); // Nötkråka.
            taxon2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), 100381); // tvåfläckig barkskinnbagge
            factors = new FactorList();
            factors.Add(factor1);
            factors.Add(factor2);
            taxa = new TaxonList();
            taxa.Add(taxon1);
            taxa.Add(taxon2);

            GetDataSet(true).AddSelection(GetUserContext(), factors);
            GetDataSet().AddSelectionTaxa(GetUserContext(), taxa);

            speciesFactCount = GetDataSet().SpeciesFacts.Count;
            host1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), 6238); // violticka.
            host2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), 230260); // skägglav.
            hosts = new TaxonList();
            hosts.Add(host1);
            hosts.Add(host2);
            GetDataSet().AddSelectionHosts(GetUserContext(), hosts);
            Assert.AreEqual(speciesFactCount, GetDataSet().SpeciesFacts.Count);
            hosts = null;
            GetDataSet().AddSelectionHosts(GetUserContext(), hosts);
        }

        [TestMethod]
        public void AddSelectionIndividualCategories()
        {
            FactorList factors;
            IFactor factor1, factor2;
            IndividualCategoryList individualCategories;
            Int32 speciesFactCount;
            ITaxon taxon1, taxon2;
            TaxonList taxa;

            factor1 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishHistory);
            factor2 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishOccurrence);
            taxon1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.DrumGrasshopper);
            taxon2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Wolverine);
            factors = new FactorList();
            factors.Add(factor1);
            factors.Add(factor2);
            taxa = new TaxonList();
            taxa.Add(taxon1);
            taxa.Add(taxon2);
            GetDataSet(true).AddSelection(GetUserContext(), factors);
            GetDataSet().AddSelectionTaxa(GetUserContext(), taxa);

            speciesFactCount = GetDataSet().SpeciesFacts.Count;
            individualCategories = CoreData.FactorManager.GetIndividualCategories(GetUserContext());
            GetDataSet().AddSelection(GetUserContext(), individualCategories);
            Assert.IsFalse(GetDataSet().Factors.IsEmpty());
            Assert.IsFalse(GetDataSet().SpeciesFacts.IsEmpty());
            Assert.IsTrue(speciesFactCount < GetDataSet().SpeciesFacts.Count);
            individualCategories = null;
            GetDataSet().AddSelection(GetUserContext(), individualCategories);
        }

        [TestMethod]
        public void AddSelectionIndividualCategory()
        {
            FactorList factors;
            IFactor factor1, factor2;
            IIndividualCategory individualCategory;
            Int32 speciesFactCount;
            ITaxon taxon1, taxon2;
            TaxonList taxa;

            factor1 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishHistory);
            factor2 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishOccurrence);
            taxon1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.DrumGrasshopper);
            taxon2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Wolverine);
            factors = new FactorList();
            factors.Add(factor1);
            factors.Add(factor2);
            taxa = new TaxonList();
            taxa.Add(taxon1);
            taxa.Add(taxon2);
            GetDataSet(true).AddSelection(GetUserContext(), factors);
            GetDataSet().AddSelectionTaxa(GetUserContext(), taxa);

            speciesFactCount = GetDataSet().SpeciesFacts.Count;
            individualCategory = CoreData.FactorManager.GetDefaultIndividualCategory(GetUserContext());
            GetDataSet().AddSelection(GetUserContext(), individualCategory);
            Assert.IsFalse(GetDataSet().Factors.IsEmpty());
            Assert.IsFalse(GetDataSet().SpeciesFacts.IsEmpty());
            Assert.AreEqual(speciesFactCount, GetDataSet().SpeciesFacts.Count);
            GetDataSet().AddSelection(GetUserContext(), CoreData.FactorManager.GetIndividualCategories(GetUserContext())[3]);
            Assert.IsTrue(speciesFactCount < GetDataSet().SpeciesFacts.Count);
            individualCategory = null;
            GetDataSet().AddSelection(GetUserContext(), individualCategory);
        }

        [TestMethod]
        public void AddSelectionPeriod()
        {
            FactorList factors;
            IFactor factor1, factor2;
            Int32 speciesFactCount;
            IPeriod period;
            ITaxon taxon1, taxon2;
            TaxonList taxa;

            factor1 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.RedlistCategory);
            factor2 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.Redlist_OrganismLabel1);
            taxon1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.DrumGrasshopper);
            taxon2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Wolverine);
            factors = new FactorList();
            factors.Add(factor1);
            factors.Add(factor2);
            taxa = new TaxonList();
            taxa.Add(taxon1);
            taxa.Add(taxon2);
            GetDataSet(true).AddSelection(GetUserContext(), factors);
            GetDataSet().AddSelectionTaxa(GetUserContext(), taxa);

            speciesFactCount = GetDataSet().SpeciesFacts.Count;
            period = CoreData.FactorManager.GetCurrentPublicPeriod(GetUserContext());
            GetDataSet().AddSelection(GetUserContext(), period);
            Assert.IsFalse(GetDataSet().Factors.IsEmpty());
            Assert.IsFalse(GetDataSet().SpeciesFacts.IsEmpty());
            Assert.IsTrue(speciesFactCount > GetDataSet().SpeciesFacts.Count);
            speciesFactCount = GetDataSet().SpeciesFacts.Count;
            GetDataSet().AddSelection(GetUserContext(), CoreData.FactorManager.GetPeriods(GetUserContext())[4]);
            Assert.IsTrue(speciesFactCount < GetDataSet().SpeciesFacts.Count);
            period = null;
            GetDataSet().AddSelection(GetUserContext(), period);
        }

        [TestMethod]
        public void AddSelectionPeriods()
        {
            FactorList factors;
            IFactor factor1, factor2;
            PeriodList periods;
            Int32 speciesFactCount;
            ITaxon taxon1, taxon2;
            TaxonList taxa;

            factor1 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.RedlistCategory);
            factor2 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.Redlist_OrganismLabel1);
            taxon1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.DrumGrasshopper);
            taxon2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Wolverine);
            factors = new FactorList();
            factors.Add(factor1);
            factors.Add(factor2);
            taxa = new TaxonList();
            taxa.Add(taxon1);
            taxa.Add(taxon2);
            GetDataSet(true).AddSelection(GetUserContext(), factors);
            GetDataSet().AddSelectionTaxa(GetUserContext(), taxa);

            speciesFactCount = GetDataSet().SpeciesFacts.Count;
            periods = CoreData.FactorManager.GetPeriods(GetUserContext());
            GetDataSet().AddSelection(GetUserContext(), periods);
            Assert.IsFalse(GetDataSet().Factors.IsEmpty());
            Assert.IsFalse(GetDataSet().SpeciesFacts.IsEmpty());
            Assert.IsTrue(speciesFactCount < GetDataSet().SpeciesFacts.Count);
            periods = null;
            GetDataSet().AddSelection(GetUserContext(), periods);
        }

        [TestMethod]
        public void AddSelectionSpeciesFactDataSetSelection()
        {
            FactorList factors;
            IFactor factor1, factor2;
            ISpeciesFactDataSetSelection selection;
            ITaxon taxon1, taxon2;
            TaxonList taxa;

            factor1 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.RedlistCategory);
            factor2 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.Redlist_OrganismLabel1);
            taxon1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.DrumGrasshopper);
            taxon2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Wolverine);
            factors = new FactorList();
            factors.Add(factor1);
            factors.Add(factor2);
            taxa = new TaxonList();
            taxa.Add(taxon1);
            taxa.Add(taxon2);
            selection = new SpeciesFactDataSetSelection();
            selection.Factors = factors;
            selection.Taxa = taxa;
            GetDataSet(true).AddSelection(GetUserContext(), selection);
            Assert.IsFalse(GetDataSet().Factors.IsEmpty());
            Assert.IsFalse(GetDataSet().Taxa.IsEmpty());
            Assert.IsFalse(GetDataSet().SpeciesFacts.IsEmpty());
        }

        [TestMethod]
        public void AddSelectionTaxa()
        {
            FactorList factors;
            IFactor factor1, factor2;
            ITaxon taxon1, taxon2;
            TaxonList taxa;

            factor1 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishHistory);
            factor2 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishOccurrence);
            taxon1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.DrumGrasshopper);
            taxon2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Wolverine);
            factors = new FactorList();
            factors.Add(factor1);
            factors.Add(factor2);
            taxa = new TaxonList();
            taxa.Add(taxon1);
            taxa.Add(taxon2);

            GetDataSet(true).AddSelection(GetUserContext(), factors);
            Assert.IsTrue(GetDataSet().Taxa.IsEmpty());
            Assert.IsTrue(GetDataSet().SpeciesFacts.IsEmpty());
            GetDataSet().AddSelectionTaxa(GetUserContext(), taxa);
            Assert.IsFalse(GetDataSet().Taxa.IsEmpty());
            Assert.IsFalse(GetDataSet().SpeciesFacts.IsEmpty());
            taxa = null;
            GetDataSet().AddSelectionTaxa(GetUserContext(), taxa);
        }

        [TestMethod]
        public void AddSelectionTaxon()
        {
            FactorList factors;
            IFactor factor1, factor2;
            ITaxon taxon1, taxon2;
            TaxonList taxa;

            factor1 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishHistory);
            factor2 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishOccurrence);
            taxon1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.DrumGrasshopper);
            taxon2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Wolverine);
            factors = new FactorList();
            factors.Add(factor1);
            factors.Add(factor2);
            taxa = new TaxonList();
            taxa.Add(taxon1);
            taxa.Add(taxon2);

            GetDataSet(true).AddSelection(GetUserContext(), factors);
            Assert.IsTrue(GetDataSet().Taxa.IsEmpty());
            Assert.IsTrue(GetDataSet().SpeciesFacts.IsEmpty());
            GetDataSet().AddSelectionTaxon(GetUserContext(), taxon1);
            Assert.IsFalse(GetDataSet().Taxa.IsEmpty());
            Assert.IsFalse(GetDataSet().SpeciesFacts.IsEmpty());
            taxon1 = null;
            GetDataSet().AddSelectionTaxon(GetUserContext(), taxon1);
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

        private SpeciesFactDataSet GetDataSet(Boolean refresh = false)
        {
            if (_dataSet.IsNull() || refresh)
            {
                _dataSet = new SpeciesFactDataSet();
            }

            return _dataSet;
        }

        [TestMethod]
        public void RemoveSelectionFactor()
        {
            FactorList factors;
            IFactor factor1, factor2;
            Int32 speciesFactCount;
            ITaxon taxon1, taxon2;
            TaxonList taxa;

            factor1 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishHistory);
            factor2 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishOccurrence);
            taxon1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.DrumGrasshopper);
            taxon2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Wolverine);
            factors = new FactorList();
            factors.Add(factor1);
            factors.Add(factor2);
            taxa = new TaxonList();
            taxa.Add(taxon1);
            taxa.Add(taxon2);
            GetDataSet(true).AddSelection(GetUserContext(), factors);
            GetDataSet().AddSelectionTaxa(GetUserContext(), taxa);
            Assert.IsFalse(GetDataSet().Factors.IsEmpty());
            Assert.IsFalse(GetDataSet().Taxa.IsEmpty());
            Assert.IsFalse(GetDataSet().SpeciesFacts.IsEmpty());

            speciesFactCount = GetDataSet().SpeciesFacts.Count;
            GetDataSet().RemoveSelection(GetUserContext(), factor1);
            Assert.IsFalse(GetDataSet().Factors.IsEmpty());
            Assert.IsFalse(GetDataSet().SpeciesFacts.IsEmpty());
            Assert.IsTrue(speciesFactCount > GetDataSet().SpeciesFacts.Count);
            factor1 = null;
            GetDataSet().RemoveSelection(GetUserContext(), factor1);
        }

        [TestMethod]
        public void RemoveSelectionFactors()
        {
            FactorList factors;
            IFactor factor1, factor2;
            Int32 speciesFactCount;
            ITaxon taxon1, taxon2;
            TaxonList taxa;

            factor1 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishHistory);
            factor2 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishOccurrence);
            taxon1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.DrumGrasshopper);
            taxon2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Wolverine);
            factors = new FactorList();
            factors.Add(factor1);
            factors.Add(factor2);
            taxa = new TaxonList();
            taxa.Add(taxon1);
            taxa.Add(taxon2);
            GetDataSet(true).AddSelection(GetUserContext(), factors);
            GetDataSet().AddSelectionTaxa(GetUserContext(), taxa);
            Assert.IsFalse(GetDataSet().Factors.IsEmpty());
            Assert.IsFalse(GetDataSet().Taxa.IsEmpty());
            Assert.IsFalse(GetDataSet().SpeciesFacts.IsEmpty());

            speciesFactCount = GetDataSet().SpeciesFacts.Count;
            factors = new FactorList();
            factors.Add(factor1);
            GetDataSet().RemoveSelection(GetUserContext(), factors);
            Assert.IsFalse(GetDataSet().Factors.IsEmpty());
            Assert.IsFalse(GetDataSet().SpeciesFacts.IsEmpty());
            Assert.IsTrue(speciesFactCount > GetDataSet().SpeciesFacts.Count);
            factors = null;
            GetDataSet().RemoveSelection(GetUserContext(), factors);
        }

        [TestMethod]
        public void RemoveSelectionHost()
        {
            FactorList factors;
            IFactor factor1, factor2;
            Int32 speciesFactCount;
            ITaxon host1, host2, taxon1, taxon2;
            TaxonList hosts, taxa;

            factor1 = CoreData.FactorManager.GetFactor(GetUserContext(), 1137); // Svampdelar (inkl lavar).
            factor2 = CoreData.FactorManager.GetFactor(GetUserContext(), 1138); // Mycel.
            taxon1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), 100090); // Nötkråka.
            taxon2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), 100381); //tvåfläckig barkskinnbagge
            factors = new FactorList();
            factors.Add(factor1);
            factors.Add(factor2);
            taxa = new TaxonList();
            taxa.Add(taxon1);
            taxa.Add(taxon2);

            GetDataSet(true).AddSelection(GetUserContext(), factors);
            GetDataSet().AddSelectionTaxa(GetUserContext(), taxa);

            host1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), 6238); // violticka.
            host2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), 230260); // skägglav.
            hosts = new TaxonList();
            hosts.Add(host1);
            hosts.Add(host2);
            GetDataSet().AddSelectionHosts(GetUserContext(), hosts);
            speciesFactCount = GetDataSet().SpeciesFacts.Count;
            GetDataSet().RemoveSelectionHost(GetUserContext(), host1);
            Assert.IsTrue(speciesFactCount > GetDataSet().SpeciesFacts.Count);
            host1 = null;
            GetDataSet().RemoveSelectionHost(GetUserContext(), host1);
        }

        [TestMethod]
        public void RemoveSelectionHosts()
        {
            FactorList factors;
            IFactor factor1, factor2;
            Int32 speciesFactCount;
            ITaxon host1, host2, taxon1, taxon2;
            TaxonList hosts1, hosts2, taxa;

            factor1 = CoreData.FactorManager.GetFactor(GetUserContext(), 1137); // Svampdelar (inkl lavar).
            factor2 = CoreData.FactorManager.GetFactor(GetUserContext(), 1138); // Mycel.
            taxon1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), 100090); // Nötkråka.
            taxon2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), 100381); //tvåfläckig barkskinnbagge
            factors = new FactorList();
            factors.Add(factor1);
            factors.Add(factor2);
            taxa = new TaxonList();
            taxa.Add(taxon1);
            taxa.Add(taxon2);

            GetDataSet(true).AddSelection(GetUserContext(), factors);
            GetDataSet().AddSelectionTaxa(GetUserContext(), taxa);

            host1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), 6238); // violticka.
            host2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), 230260); // skägglav.
            hosts1 = new TaxonList();
            hosts1.Add(host1);
            hosts1.Add(host2);
            GetDataSet().AddSelectionHosts(GetUserContext(), hosts1);
            speciesFactCount = GetDataSet().SpeciesFacts.Count;
            hosts2 = new TaxonList();
            hosts2.Add(host1);
            GetDataSet().RemoveSelectionHosts(GetUserContext(), hosts2);
            Assert.IsTrue(speciesFactCount > GetDataSet().SpeciesFacts.Count);
            hosts2 = null;
            GetDataSet().RemoveSelectionHosts(GetUserContext(), hosts2);
        }

        [TestMethod]
        public void RemoveSelectionIndividualCategories()
        {
            FactorList factors;
            IFactor factor1, factor2;
            IndividualCategoryList individualCategories1, individualCategories2;
            Int32 speciesFactCount;
            ITaxon taxon1, taxon2;
            TaxonList taxa;

            factor1 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishHistory);
            factor2 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishOccurrence);
            taxon1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.DrumGrasshopper);
            taxon2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Wolverine);
            factors = new FactorList();
            factors.Add(factor1);
            factors.Add(factor2);
            taxa = new TaxonList();
            taxa.Add(taxon1);
            taxa.Add(taxon2);
            GetDataSet(true).AddSelection(GetUserContext(), factors);
            GetDataSet().AddSelectionTaxa(GetUserContext(), taxa);

            individualCategories1 = CoreData.FactorManager.GetIndividualCategories(GetUserContext());
            GetDataSet().AddSelection(GetUserContext(), individualCategories1);
            Assert.IsFalse(GetDataSet().Factors.IsEmpty());
            Assert.IsFalse(GetDataSet().SpeciesFacts.IsEmpty());
            speciesFactCount = GetDataSet().SpeciesFacts.Count;

            individualCategories2 = new IndividualCategoryList();
            individualCategories2.Add(individualCategories1[1]);
            individualCategories2.Add(individualCategories1[2]);
            GetDataSet().RemoveSelection(GetUserContext(), individualCategories2);
            Assert.IsTrue(speciesFactCount > GetDataSet().SpeciesFacts.Count);
            individualCategories2 = null;
            GetDataSet().RemoveSelection(GetUserContext(), individualCategories2);
        }

        [TestMethod]
        public void RemoveSelectionIndividualCategory()
        {
            FactorList factors;
            IFactor factor1, factor2;
            IIndividualCategory individualCategory;
            IndividualCategoryList individualCategories;
            Int32 speciesFactCount;
            ITaxon taxon1, taxon2;
            TaxonList taxa;

            factor1 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishHistory);
            factor2 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishOccurrence);
            taxon1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.DrumGrasshopper);
            taxon2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Wolverine);
            factors = new FactorList();
            factors.Add(factor1);
            factors.Add(factor2);
            taxa = new TaxonList();
            taxa.Add(taxon1);
            taxa.Add(taxon2);
            GetDataSet(true).AddSelection(GetUserContext(), factors);
            GetDataSet().AddSelectionTaxa(GetUserContext(), taxa);

            individualCategories = CoreData.FactorManager.GetIndividualCategories(GetUserContext());
            GetDataSet().AddSelection(GetUserContext(), individualCategories);
            Assert.IsFalse(GetDataSet().Factors.IsEmpty());
            Assert.IsFalse(GetDataSet().SpeciesFacts.IsEmpty());
            speciesFactCount = GetDataSet().SpeciesFacts.Count;
 
            individualCategory = CoreData.FactorManager.GetDefaultIndividualCategory(GetUserContext());
            GetDataSet().RemoveSelection(GetUserContext(), individualCategory);
            Assert.IsTrue(speciesFactCount > GetDataSet().SpeciesFacts.Count);
            individualCategory = null;
            GetDataSet().RemoveSelection(GetUserContext(), individualCategory);
        }

        [TestMethod]
        public void RemoveSelectionPeriod()
        {
            FactorList factors;
            IFactor factor1, factor2;
            Int32 speciesFactCount;
            IPeriod period;
            ITaxon taxon1, taxon2;
            TaxonList taxa;

            factor1 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.RedlistCategory);
            factor2 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.Redlist_OrganismLabel1);
            taxon1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.DrumGrasshopper);
            taxon2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Wolverine);
            factors = new FactorList();
            factors.Add(factor1);
            factors.Add(factor2);
            taxa = new TaxonList();
            taxa.Add(taxon1);
            taxa.Add(taxon2);
            GetDataSet(true).AddSelection(GetUserContext(), factors);
            GetDataSet().AddSelectionTaxa(GetUserContext(), taxa);

            GetDataSet().AddSelection(GetUserContext(), CoreData.FactorManager.GetPeriods(GetUserContext()));
            speciesFactCount = GetDataSet().SpeciesFacts.Count;
            period = CoreData.FactorManager.GetCurrentPublicPeriod(GetUserContext());
            GetDataSet().RemoveSelection(GetUserContext(), period);
            Assert.IsFalse(GetDataSet().Factors.IsEmpty());
            Assert.IsFalse(GetDataSet().SpeciesFacts.IsEmpty());
            Assert.IsTrue(speciesFactCount > GetDataSet().SpeciesFacts.Count);
            period = null;
            GetDataSet().RemoveSelection(GetUserContext(), period);
        }

        [TestMethod]
        public void RemoveSelectionPeriods()
        {
            FactorList factors;
            IFactor factor1, factor2;
            Int32 speciesFactCount;
            ITaxon taxon1, taxon2;
            PeriodList periods;
            TaxonList taxa;

            factor1 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.RedlistCategory);
            factor2 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.Redlist_OrganismLabel1);
            taxon1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.DrumGrasshopper);
            taxon2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Wolverine);
            factors = new FactorList();
            factors.Add(factor1);
            factors.Add(factor2);
            taxa = new TaxonList();
            taxa.Add(taxon1);
            taxa.Add(taxon2);
            GetDataSet(true).AddSelection(GetUserContext(), factors);
            GetDataSet().AddSelectionTaxa(GetUserContext(), taxa);

            GetDataSet().AddSelection(GetUserContext(), CoreData.FactorManager.GetPeriods(GetUserContext()));
            speciesFactCount = GetDataSet().SpeciesFacts.Count;
            periods = new PeriodList();
            periods.Add(GetDataSet().Periods[0]);
            periods.Add(GetDataSet().Periods[1]);
            GetDataSet().RemoveSelection(GetUserContext(), periods);
            Assert.IsFalse(GetDataSet().Factors.IsEmpty());
            Assert.IsFalse(GetDataSet().SpeciesFacts.IsEmpty());
            Assert.IsTrue(speciesFactCount > GetDataSet().SpeciesFacts.Count);
            periods = null;
            GetDataSet().RemoveSelection(GetUserContext(), periods);
        }

        [TestMethod]
        public void RemoveSelectionSpeciesFactDataSetSelection()
        {
            FactorList factors;
            IFactor factor1, factor2;
            Int32 speciesFactCount;
            ISpeciesFactDataSetSelection selection;
            ITaxon taxon1, taxon2;
            TaxonList taxa;

            factor1 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.RedlistCategory);
            factor2 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.Redlist_OrganismLabel1);
            taxon1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.DrumGrasshopper);
            taxon2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Wolverine);
            factors = new FactorList();
            factors.Add(factor1);
            factors.Add(factor2);
            taxa = new TaxonList();
            taxa.Add(taxon1);
            taxa.Add(taxon2);
            selection = new SpeciesFactDataSetSelection();
            selection.Factors = factors;
            selection.Taxa = taxa;
            GetDataSet(true).AddSelection(GetUserContext(), selection);
            Assert.IsFalse(GetDataSet().Factors.IsEmpty());
            Assert.IsFalse(GetDataSet().Taxa.IsEmpty());
            Assert.IsFalse(GetDataSet().SpeciesFacts.IsEmpty());
            speciesFactCount = GetDataSet().SpeciesFacts.Count;
            selection = new SpeciesFactDataSetSelection();
            selection.Factors.Add(factor1);
            selection.Taxa.Add(taxon1);
            GetDataSet().RemoveSelection(GetUserContext(), selection);
            Assert.IsTrue(speciesFactCount > GetDataSet().SpeciesFacts.Count);
        }

        [TestMethod]
        public void RemoveSelectionTaxa()
        {
            FactorList factors;
            IFactor factor1, factor2;
            Int32 speciesFactCount;
            ITaxon taxon1, taxon2;
            TaxonList taxa1, taxa2;

            factor1 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishHistory);
            factor2 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishOccurrence);
            taxon1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.DrumGrasshopper);
            taxon2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Wolverine);
            factors = new FactorList();
            factors.Add(factor1);
            factors.Add(factor2);
            taxa1 = new TaxonList();
            taxa1.Add(taxon1);
            taxa1.Add(taxon2);
            GetDataSet(true).AddSelection(GetUserContext(), factors);
            GetDataSet().AddSelectionTaxa(GetUserContext(), taxa1);

            speciesFactCount = GetDataSet().SpeciesFacts.Count;
            Assert.IsFalse(GetDataSet().Taxa.IsEmpty());
            Assert.IsFalse(GetDataSet().SpeciesFacts.IsEmpty());
            taxa2 = new TaxonList();
            taxa2.Add(taxon1);
            GetDataSet().RemoveSelectionTaxa(GetUserContext(), taxa2);
            Assert.IsFalse(GetDataSet().Taxa.IsEmpty());
            Assert.IsFalse(GetDataSet().SpeciesFacts.IsEmpty());
            Assert.IsTrue(speciesFactCount > GetDataSet().SpeciesFacts.Count);
            taxa2 = null;
            GetDataSet().RemoveSelectionTaxa(GetUserContext(), taxa2);
        }

        [TestMethod]
        public void RemoveSelectionTaxon()
        {
            FactorList factors;
            IFactor factor1, factor2;
            Int32 speciesFactCount;
            ITaxon taxon1, taxon2;
            TaxonList taxa;

            factor1 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishHistory);
            factor2 = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.SwedishOccurrence);
            taxon1 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.DrumGrasshopper);
            taxon2 = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Wolverine);
            factors = new FactorList();
            factors.Add(factor1);
            factors.Add(factor2);
            taxa = new TaxonList();
            taxa.Add(taxon1);
            taxa.Add(taxon2);
            GetDataSet(true).AddSelection(GetUserContext(), factors);
            GetDataSet().AddSelectionTaxa(GetUserContext(), taxa);

            speciesFactCount = GetDataSet().SpeciesFacts.Count;
            Assert.IsFalse(GetDataSet().Taxa.IsEmpty());
            Assert.IsFalse(GetDataSet().SpeciesFacts.IsEmpty());
            GetDataSet().RemoveSelectionTaxon(GetUserContext(), taxon1);
            Assert.IsFalse(GetDataSet().Taxa.IsEmpty());
            Assert.IsFalse(GetDataSet().SpeciesFacts.IsEmpty());
            Assert.IsTrue(speciesFactCount > GetDataSet().SpeciesFacts.Count);
            taxon1 = null;
            GetDataSet().RemoveSelectionTaxon(GetUserContext(), taxon1);
        }

        [TestMethod]
        public void UpdateSelection()
        {
            ISpeciesFactDataSetSelection selection;

            selection = new SpeciesFactDataSetSelection();
            selection.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.RedListCategoryAutomatic));
            selection.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.RedListCriteriaDocumentationAutomatic));
            selection.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.RedListCriteriaAutomatic));
            selection.Periods.Add(CoreData.FactorManager.GetPeriod(GetUserContext(), PeriodId.Year2015));
            selection.Taxa.Add(CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.DrumGrasshopper));
            GetDataSet(true).UpdateSelection(GetUserContext(), selection);

            GetDataSet().AddSelection(GetUserContext(), CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.ProtectionLevel));
        }
    }
}

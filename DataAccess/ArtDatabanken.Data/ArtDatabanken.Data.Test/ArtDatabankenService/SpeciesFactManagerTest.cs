using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    using FactorFieldDataTypeId = ArtDatabanken.Data.ArtDatabankenService.FactorFieldDataTypeId;
    using OrganismGroup = Data.ArtDatabankenService.OrganismGroup;
    using OrganismGroupList = Data.ArtDatabankenService.OrganismGroupList;
    using OrganismGroupType = Data.ArtDatabankenService.OrganismGroupType;
    using ReferenceList = ArtDatabanken.Data.ArtDatabankenService.ReferenceList;

    [TestClass]
    public class SpeciesFactManagerTest : TestBase
    {
        private const Int32 INDIVIDUAL_CATEGORY_ID_IMAGO = 10;

        public SpeciesFactManagerTest()
        {
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

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        public static Data.ArtDatabankenService.SpeciesFactQuality GetFirstSpeciesFactQuality()
        {
            return Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFactQuality(1);
        }

        public static OrganismGroup GetOneOrganismGroup()
        {
            return Data.ArtDatabankenService.SpeciesFactManager.GetOrganismGroups()[0];
        }

        public static TaxonCountyOccurrence GetOneTaxonCountyOccurrence()
        {
            Int32 taxonId;
            ArtDatabanken.Data.ArtDatabankenService.Taxon taxon;
            TaxonCountyOccurrenceList countyOccurrencies;

            taxonId = BEAR_TAXON_ID;
            taxon = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxon(taxonId, TaxonInformationType.Basic);
            countyOccurrencies = Data.ArtDatabankenService.SpeciesFactManager.GetTaxonCountyOccurence(taxon);
            return countyOccurrencies[0];
        }

        [TestMethod]
        public void GetOrganismGroups()
        {
            OrganismGroupList organismGroups;

            organismGroups = Data.ArtDatabankenService.SpeciesFactManager.GetOrganismGroups();
            Assert.IsTrue(organismGroups.IsNotEmpty());
            foreach (OrganismGroupType type in Enum.GetValues(typeof(OrganismGroupType)))
            {
                organismGroups = Data.ArtDatabankenService.SpeciesFactManager.GetOrganismGroups(type);
                Assert.IsTrue(organismGroups.IsNotEmpty());
                foreach (OrganismGroup organismGroup in organismGroups)
                {
                    Assert.AreEqual(type, organismGroup.Type);
                }
            }
        }

        public static Data.ArtDatabankenService.SpeciesFactList GetRedListSpeciesFacts()
        {
            Data.ArtDatabankenService.FactorList redlistRootFactors;
            List<Int32> redlistRootFactorIds;
            Data.ArtDatabankenService.SpeciesFactList speciesFacts;
            Data.ArtDatabankenService.TaxonList taxa;
            UserParameterSelection userParameterSelection;

            // Get red list factors.
            redlistRootFactorIds = new List<Int32>();
            redlistRootFactorIds.Add((Int32)FactorId.RedlistFactorGroup1_GenerealFactors);
            redlistRootFactorIds.Add((Int32)FactorId.RedlistFactorGroup2_PopulationReduction);
            redlistRootFactorIds.Add((Int32)FactorId.RedlistFactorGroup3_PopulationSize);
            redlistRootFactorIds.Add((Int32)FactorId.RedlistFactorGroup4_PopulationDistribution);
            redlistRootFactorIds.Add((Int32)FactorId.RedlistFactorGroup5_QuantitativeAnalysis);
            redlistRootFactorIds.Add((Int32)FactorId.RedlistFactorGroup6_RegionalImplications);
            redlistRootFactorIds.Add((Int32)FactorId.RedlistFactorGroup7_Results);
            redlistRootFactorIds.Add((Int32)FactorId.RedlistFactorGroup8_RedlistChanges);
            redlistRootFactors = Data.ArtDatabankenService.FactorManager.GetFactors(redlistRootFactorIds);

            // Get taxa.
            taxa = new Data.ArtDatabankenService.TaxonList();
            taxa.Add(Data.ArtDatabankenService.TaxonManager.GetTaxon(BEAR_TAXON_ID, TaxonInformationType.Basic));

            // Get species facts.
            userParameterSelection = new UserParameterSelection();
            userParameterSelection.Factors.Merge(redlistRootFactors);
            userParameterSelection.Taxa.Merge(taxa);
            speciesFacts = Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFactsByUserParameterSelection(userParameterSelection);

            return speciesFacts;
        }

        public static OrganismGroupList GetSomeOrganismGroups()
        {
            return Data.ArtDatabankenService.SpeciesFactManager.GetOrganismGroups();
        }

        [TestMethod]
        public void GetSpeciesFactQuality()
        {
            Data.ArtDatabankenService.SpeciesFactQuality speciesFactQuality;
            Int32 speciesFactQualityId;

            speciesFactQualityId = 1;
            speciesFactQuality = Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFactQuality(speciesFactQualityId);
            Assert.IsNotNull(speciesFactQuality);
            Assert.AreEqual(speciesFactQualityId, speciesFactQuality.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetSpeciesFactQualityIdError()
        {
            Int32 speciesFactQualityId;

            speciesFactQualityId = Int32.MinValue;
            Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFactQuality(speciesFactQualityId);
        }

        public static List<Int32> GetSpeciesFactIds()
        {
            List<Int32> speciesFactIds;

            speciesFactIds = new List<int>();
            speciesFactIds.Add(388861);
            speciesFactIds.Add(388873);
            speciesFactIds.Add(388874);
            speciesFactIds.Add(388875);
            speciesFactIds.Add(388876);
            speciesFactIds.Add(388877);

            return speciesFactIds;
        }

        public static Data.ArtDatabankenService.SpeciesFact GetASpeciesFact()
        {
            Data.ArtDatabankenService.SpeciesFact speciesFact;
            Data.ArtDatabankenService.SpeciesFactList speciesFacts;

            speciesFacts = Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFacts(GetSpeciesFactIds());
            speciesFact = speciesFacts[0];
            return speciesFact;
        }


        public static Data.ArtDatabankenService.SpeciesFactField GetASpeciesFactField()
        {
            Data.ArtDatabankenService.SpeciesFact speciesFact;
            Data.ArtDatabankenService.SpeciesFactField speciesFactField;

            speciesFact = GetASpeciesFact();
            speciesFactField = speciesFact.Fields[0];
            return speciesFactField;
        }


        [TestMethod]
        public void GetSpeciesFacts()
        {
            Boolean speciesFactFound;
            Data.ArtDatabankenService.SpeciesFactList speciesFacts;

            speciesFacts = Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFacts(GetSpeciesFactIds());
            Assert.IsNotNull(speciesFacts);
            Assert.AreEqual(speciesFacts.Count, GetSpeciesFactIds().Count);
            foreach (SpeciesFact speciesFact in speciesFacts)
            {
                Assert.IsNotNull(speciesFact.Factor.Label);

                speciesFactFound = false;
                foreach (Int32 speciesFactId in GetSpeciesFactIds())
                {
                    if (speciesFactId == speciesFact.Id)
                    {
                        speciesFactFound = true;
                        break;
                    }
                }
                Assert.IsTrue(speciesFactFound);

            }
        }

        [TestMethod]
        public void GetManySpeciesFactsById()
        {
            Data.ArtDatabankenService.SpeciesFactList speciesFacts;
            List<Int32> speciesFactIds;
            speciesFactIds = new List<Int32>();

            for (Int32 i = 1; i < 20; i++)
            {
                speciesFactIds.Add(i);
            }

            speciesFacts = Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFacts(speciesFactIds);
            Assert.AreEqual(speciesFactIds.Count, speciesFacts.Count);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetUserDataSetByParameterSelectionNoTaxaError()
        {
            UserDataSet userDataSet;
            UserParameterSelection userParmeterSelection;

            userParmeterSelection = new UserParameterSelection();
            userDataSet = Data.ArtDatabankenService.SpeciesFactManager.GetUserDataSetByParameterSelection(userParmeterSelection);
        }

        public static Data.ArtDatabankenService.SpeciesFactList GetSomeSpeciesFacts()
        {
            return Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFacts(GetSpeciesFactIds());
        }

        [TestMethod]
        public void GetUserDataSetByParameterSelection()
        {
            UserDataSet userDataSet = null, userDataSet2;
            UserParameterSelection userParmeterSelection;
            userParmeterSelection = new UserParameterSelection();

            userParmeterSelection.Taxa.Merge(ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxa(TaxonManagerTest.GetTaxaIds(), TaxonInformationType.Basic));
            Data.ArtDatabankenService.FactorList factors;
            factors = new Data.ArtDatabankenService.FactorList();
            factors.Add(Data.ArtDatabankenService.FactorManager.GetFactor(LANDSCAPE_FACTOR_ID));
            userParmeterSelection.Factors.Merge(factors);

            userDataSet = Data.ArtDatabankenService.SpeciesFactManager.GetUserDataSetByParameterSelection(userParmeterSelection);
            Assert.IsNotNull(userDataSet);
            Assert.IsTrue(userDataSet.SpeciesFacts.IsNotEmpty());
            Assert.IsTrue(userDataSet.SpeciesFacts.Count > 2);

            // Test problem where automatic SpeciesFact is 
            // added to the UserDataSet but dependent 
            // SpeicesFact is not in the UserDataSet.
            userParmeterSelection = new UserParameterSelection();
            userParmeterSelection.Taxa.Add(TaxonManagerTest.GetOneTaxon());
            userParmeterSelection.Factors.Add(Data.ArtDatabankenService.FactorManager.GetFactor(Data.ArtDatabankenService.FactorId.RedListCategoryAutomatic));
            userDataSet = Data.ArtDatabankenService.SpeciesFactManager.GetUserDataSetByParameterSelection(userParmeterSelection);
            Assert.IsTrue(userDataSet.SpeciesFacts.IsNotEmpty());
            Assert.IsTrue(userDataSet.Factors.Count > 30);

            // Test problem where periodic SpeciesFact are combined
            // with none default IndividualCategory.
            userParmeterSelection = new UserParameterSelection();
            userParmeterSelection.Taxa.Add(TaxonManagerTest.GetOneTaxon());
            userParmeterSelection.Factors.Add(Data.ArtDatabankenService.FactorManager.GetFactor(Data.ArtDatabankenService.FactorId.PopulationSize_Total));
            userDataSet = Data.ArtDatabankenService.SpeciesFactManager.GetUserDataSetByParameterSelection(userParmeterSelection);
            userParmeterSelection.IndividualCategories.Add(IndividualCategoryManager.GetIndividualCategory(INDIVIDUAL_CATEGORY_ID_IMAGO));
            userDataSet2 = Data.ArtDatabankenService.SpeciesFactManager.GetUserDataSetByParameterSelection(userParmeterSelection);
            Assert.AreEqual(userDataSet.SpeciesFacts.Count, userDataSet2.SpeciesFacts.Count);
        }

        [TestMethod]
        public void GetSpeciesFactByUserParameterSelection()
        {
            FactorList factors;
            Data.ArtDatabankenService.IndividualCategoryList individualCategories;
            Int32 count;
            Data.ArtDatabankenService.PeriodList periods;
            ReferenceList references;
            Data.ArtDatabankenService.SpeciesFactList speciesFacts;
            Data.ArtDatabankenService.TaxonList hosts, taxa;
            UserParameterSelection userParmeterSelection;

            userParmeterSelection = new UserParameterSelection();
            factors = FactorManagerTest.GetSomeFactors();
            hosts = TaxonManagerTest.GetSomeTaxa();
            individualCategories = IndividualCategoryManagerTest.GetSomeIndividualCategories();
            periods = PeriodManagerTest.GetSomePeriods();
            references = ReferenceManagerTest.GetSomeReferences();
            taxa = hosts;

            userParmeterSelection.Taxa.Merge(taxa);
            speciesFacts = Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFactsByUserParameterSelection(userParmeterSelection);
            Assert.IsTrue(speciesFacts.IsNotEmpty());
            count = speciesFacts.Count;

            userParmeterSelection.Factors.Merge(factors);
            speciesFacts = Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFactsByUserParameterSelection(userParmeterSelection);
            if (speciesFacts.IsNotEmpty())
            {
                Assert.IsTrue(count > speciesFacts.Count);
            }
            userParmeterSelection.Factors.Clear();

            userParmeterSelection.Hosts.Merge(hosts);
            speciesFacts = Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFactsByUserParameterSelection(userParmeterSelection);
            if (speciesFacts.IsNotEmpty())
            {
                Assert.IsTrue(count > speciesFacts.Count);
            }
            userParmeterSelection.Hosts.Clear();

            userParmeterSelection.IndividualCategories.Merge(individualCategories);
            speciesFacts = Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFactsByUserParameterSelection(userParmeterSelection);
            if (speciesFacts.IsNotEmpty())
            {
                Assert.IsTrue(count > speciesFacts.Count);
            }
            userParmeterSelection.IndividualCategories.Clear();

            userParmeterSelection.Periods.Merge(periods);
            speciesFacts = Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFactsByUserParameterSelection(userParmeterSelection);
            if (speciesFacts.IsNotEmpty())
            {
                Assert.IsTrue(count > speciesFacts.Count);
            }
            userParmeterSelection.Periods.Clear();

            userParmeterSelection.References.Merge(references);
            speciesFacts = Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFactsByUserParameterSelection(userParmeterSelection);
            if (speciesFacts.IsNotEmpty())
            {
                Assert.IsTrue(count > speciesFacts.Count);
            }
            userParmeterSelection.References.Clear();
        }

        [TestMethod]
        public void GetDyntaxaSpeciesFactsByUserParameterSelection()
        {
            Data.ArtDatabankenService.FactorList factors;
            Int32 count;
            Data.ArtDatabankenService.SpeciesFactList speciesFacts;
            Data.ArtDatabankenService.TaxonList taxa;
            UserParameterSelection userParmeterSelection;

            Data.ArtDatabankenService.Taxon lumpedTaxon = Data.ArtDatabankenService.TaxonManager.GetTaxon((Int32)(TaxonId.Bear), TaxonInformationType.Basic);

            userParmeterSelection = new UserParameterSelection();
            factors = new Data.ArtDatabankenService.FactorList();
            factors.Add(Data.ArtDatabankenService.FactorManager.GetFactor(Data.ArtDatabankenService.FactorId.SwedishOccurrence));
            factors.Add(Data.ArtDatabankenService.FactorManager.GetFactor(Data.ArtDatabankenService.FactorId.SwedishHistory));
            factors.Add(Data.ArtDatabankenService.FactorManager.GetFactor(Data.ArtDatabankenService.FactorId.QualityInDyntaxa));
            factors.Add(Data.ArtDatabankenService.FactorManager.GetFactor(Data.ArtDatabankenService.FactorId.NumberOfSwedishSpecies));
            factors.Add(Data.ArtDatabankenService.FactorManager.GetFactor(Data.ArtDatabankenService.FactorId.BanndedForReporting));
            factors.Add(Data.ArtDatabankenService.FactorManager.GetFactor(Data.ArtDatabankenService.FactorId.ExcludeFromReportingSystem));

            taxa = new Data.ArtDatabankenService.TaxonList();
            taxa.Add(lumpedTaxon);

            userParmeterSelection.Taxa.Merge(taxa);
            speciesFacts = Data.ArtDatabankenService.SpeciesFactManager.GetDyntaxaSpeciesFactsByUserParameterSelection(userParmeterSelection);
            Assert.IsTrue(speciesFacts.IsNotEmpty());
            count = speciesFacts.Count;

            userParmeterSelection.Factors.Merge(factors);
            speciesFacts = Data.ArtDatabankenService.SpeciesFactManager.GetDyntaxaSpeciesFactsByUserParameterSelection(userParmeterSelection);
            if (speciesFacts.IsNotEmpty())
            {
                Assert.IsTrue(count > speciesFacts.Count);
            }

            

            //TestUpdateSpeciesFacts
            Data.ArtDatabankenService.SpeciesFact numberOfSpecies1 = speciesFacts.GetSpeciesFactsByParameters(Data.ArtDatabankenService.FactorManager.GetFactor(Data.ArtDatabankenService.FactorId.NumberOfSwedishSpecies))[0];
            Int32 testValue = 1001;
            Int32 originalValue = Int32.MinValue;
            
            if (numberOfSpecies1.MainField.HasValue)
            {
                originalValue = numberOfSpecies1.MainField.Int32Value;
            }
            numberOfSpecies1.MainField.Value = testValue;

            //This is the critical part of the test but as the method should be restricted to Dyntaxa it is not possible to run it by EVA. Change Application to Dyntaxa in testBase
            /*
            using (WebTransaction transaction = new WebTransaction(5))
            {
                SpeciesFactManager.UpdateDyntaxaSpeciesFacts(speciesFacts, ReferenceManagerTest.GetAReference(), "DyntaxaTestPerson");
                speciesFacts = SpeciesFactManager.GetDyntaxaSpeciesFactsByUserParameterSelection(userParmeterSelection);
                numberOfSpecies2 = speciesFacts.GetSpeciesFactsByParameters(ArtDatabanken.Data.ArtDatabankenService.FactorManager.GetFactor(FactorId.NumberOfSwedishSpecies))[0];
                savedValue = numberOfSpecies2.MainField.Int32Value;
            }
            Assert.AreNotEqual(originalValue, savedValue);
            Assert.AreEqual(testValue, savedValue);
            Assert.AreEqual("DyntaxaTestPerson", numberOfSpecies2.UpdateUserFullName);
             */
        }

        #region Tests on ExpandSpeciesFactListWithEmptySpeciesFacts

        public static Data.ArtDatabankenService.FactorList GetSomeNoHeaderNoPeriodicFactors()
        {
            Data.ArtDatabankenService.FactorList list = new Data.ArtDatabankenService.FactorList();
            Data.ArtDatabankenService.FactorList initialList;
            initialList = Data.ArtDatabankenService.FactorManager.GetFactors(new List<int> { 983, 984, 743, 123, 435 });
            foreach (Data.ArtDatabankenService.Factor factor in initialList)
            {
                if (!factor.FactorUpdateMode.IsHeader)
                {
                    if (!factor.IsPeriodic)
                    {
                        list.Add(factor);
                    }
                }
            }
            return list;
        }

        public static TaxonCountyOccurrenceList GetSomeTaxonCountyOccurencies()
        {
            Int32 taxonId;
            Data.ArtDatabankenService.Taxon taxon;
            TaxonCountyOccurrenceList countyOccurrencies;

            taxonId = BEAR_TAXON_ID;
            taxon = Data.ArtDatabankenService.TaxonManager.GetTaxon(taxonId, TaxonInformationType.Basic);
            countyOccurrencies = Data.ArtDatabankenService.SpeciesFactManager.GetTaxonCountyOccurence(taxon);
            return countyOccurrencies;
        }

        [TestMethod]
        public void GetTaxonCountyOccurence()
        {
            Int32 taxonId;
            Data.ArtDatabankenService.Taxon taxon;
            TaxonCountyOccurrenceList countyOccurrencies;

            taxonId = BEAR_TAXON_ID;
            taxon = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxon(taxonId, TaxonInformationType.Basic);
            countyOccurrencies = Data.ArtDatabankenService.SpeciesFactManager.GetTaxonCountyOccurence(taxon);
            Assert.IsTrue(countyOccurrencies.IsNotEmpty());
            foreach (TaxonCountyOccurrence countyOccurrence in countyOccurrencies)
            {
                Assert.IsNotNull(countyOccurrence);
                Assert.AreEqual(taxon, countyOccurrence.Taxon);
            }
        }

        public static Data.ArtDatabankenService.IndividualCategoryList GetTwoIndividualCategories()
        {
            Data.ArtDatabankenService.IndividualCategoryList categories = new Data.ArtDatabankenService.IndividualCategoryList();
            categories.Add(IndividualCategoryManager.GetIndividualCategory(12));
            categories.Add(IndividualCategoryManager.GetIndividualCategory(8));
            return categories;
        }

        //Felaktiga parametrar
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpandSpeciesFactListWithEmptySpeciesFactsUserParameterNull()
        {
            Data.ArtDatabankenService.SpeciesFactManager.ExpandSpeciesFactListWithEmptySpeciesFacts(null, new Data.ArtDatabankenService.SpeciesFactList());
        }

        //Dåliga parametrar
        //

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExpandSpeciesFactListWithEmptySpeciesFactsTaxonEmpty()
        {
            UserParameterSelection userparams = new UserParameterSelection();
            Data.ArtDatabankenService.SpeciesFactManager.ExpandSpeciesFactListWithEmptySpeciesFacts(userparams, new Data.ArtDatabankenService.SpeciesFactList());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExpandSpeciesFactListWithEmptySpeciesFactsFactorEmpty()
        {
            UserParameterSelection userparams = new UserParameterSelection();
            userparams.Taxa.Merge(TaxonManagerTest.GetTaxaList());
            Assert.IsFalse(userparams.Taxa.IsEmpty());
            Data.ArtDatabankenService.SpeciesFactManager.ExpandSpeciesFactListWithEmptySpeciesFacts(userparams, new Data.ArtDatabankenService.SpeciesFactList());
        }

        //funktionskontroll

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpandSpeciesFactListWithEmptySpeciesFactsSpeciesFactsNull()
        {
            UserParameterSelection userparams = new UserParameterSelection();
            userparams.Taxa.Merge(TaxonManagerTest.GetTaxaList());
            Assert.AreNotEqual(0, userparams.Taxa.Count);
            userparams.Factors.Clear();
            userparams.Factors.AddRange(GetSomeNoHeaderNoPeriodicFactors());
            Assert.AreNotEqual(0, userparams.Factors.Count);
            Data.ArtDatabankenService.SpeciesFactList facts = null;
            Data.ArtDatabankenService.SpeciesFactManager.ExpandSpeciesFactListWithEmptySpeciesFacts(userparams, facts);

        }

        [TestMethod]
        public void ExpandSpeciesFactListWithEmptySpeciesFactsEmptySpeciesFacts()
        {
            UserParameterSelection userparams = new UserParameterSelection();
            userparams.Taxa.Merge(TaxonManagerTest.GetTaxaList());
            Assert.AreNotEqual(0, userparams.Taxa.Count);
            userparams.Factors.Merge(GetSomeNoHeaderNoPeriodicFactors());
            Assert.AreNotEqual(0, userparams.Factors.Count);
            Data.ArtDatabankenService.SpeciesFactList facts = new Data.ArtDatabankenService.SpeciesFactList();
            Data.ArtDatabankenService.SpeciesFactManager.ExpandSpeciesFactListWithEmptySpeciesFacts(userparams, facts);
            Assert.IsNotNull(facts);
            Assert.AreNotEqual(0, facts.Count);
            Assert.AreEqual((userparams.Taxa.Count * userparams.Factors.Count), facts.Count);
        }

        [TestMethod]
        public void ExpandSpeciesFactListWithEmptySpeciesFactsUsingTwoCategories()
        {
            UserParameterSelection userparams = new UserParameterSelection();
            userparams.Taxa.Merge(TaxonManagerTest.GetTaxaList());
            Assert.AreNotEqual(0, userparams.Taxa.Count);
            userparams.Factors.Merge(GetSomeNoHeaderNoPeriodicFactors());
            Assert.AreNotEqual(0, userparams.Factors.Count);

            // add some categories
            userparams.IndividualCategories.Merge(GetTwoIndividualCategories());
            Assert.AreNotEqual(0, userparams.IndividualCategories.Count);

            Data.ArtDatabankenService.SpeciesFactList facts = new Data.ArtDatabankenService.SpeciesFactList();
            Data.ArtDatabankenService.SpeciesFactManager.ExpandSpeciesFactListWithEmptySpeciesFacts(userparams, facts);

            //The real test
            Assert.AreEqual((userparams.Taxa.Count * userparams.Factors.Count * userparams.IndividualCategories.Count), facts.Count);
        }

        [TestMethod]
        public void ExpandSpeciesFactListWithEmptySpeciesFactsUsingTwoCategoriesAndOldSpeciesFacts()
        {
            UserParameterSelection userparams = new UserParameterSelection();
            userparams.Taxa.Merge(TaxonManagerTest.GetTaxaList());
            Assert.AreNotEqual(0, userparams.Taxa.Count);
            userparams.Factors.Merge(GetSomeNoHeaderNoPeriodicFactors());
            Assert.AreNotEqual(0, userparams.Factors.Count);

            // add some categories
            userparams.IndividualCategories.Merge(GetTwoIndividualCategories());
            Assert.AreNotEqual(0, userparams.IndividualCategories.Count);

            Data.ArtDatabankenService.SpeciesFactList facts = SpeciesFactManagerTest.GetSomeSpeciesFacts();
            int initialCount = facts.Count;

            Data.ArtDatabankenService.SpeciesFactManager.ExpandSpeciesFactListWithEmptySpeciesFacts(userparams, facts);

            //The real test
            Assert.AreEqual((initialCount + (userparams.Taxa.Count * userparams.Factors.Count * userparams.IndividualCategories.Count)), facts.Count);
        }

        #endregion
        [TestMethod]
        public void UpdateSpeciesFacts()
        {
            DateTime oldUpdateDate;
            Int32 fieldIndex, oldReferenceId, oldQualityId;
            Data.ArtDatabankenService.Reference reference;
            Data.ArtDatabankenService.SpeciesFact changedSpeciesFact;
            Data.ArtDatabankenService.SpeciesFactField field;
            Data.ArtDatabankenService.SpeciesFactList changedSpeciesFacts, speciesFacts;
            String oldUpdateUser;
            String stringValue;
            UserParameterSelection userParameterSelection;

            // Test delete of species fact.
            using (WebTransaction transaction = new WebTransaction(5))
            {
                // Delete some species facts.
                speciesFacts = GetSomeSpeciesFacts();
                foreach (Data.ArtDatabankenService.SpeciesFact speciesFact in speciesFacts)
                {
                    if (speciesFact.AllowManualUpdate)
                    {
                        for (fieldIndex = 0; fieldIndex < speciesFact.Fields.Count; fieldIndex++)
                        {
                            speciesFact.Fields[fieldIndex].Value = null;
                        }
                    }
                }
                Data.ArtDatabankenService.SpeciesFactManager.UpdateSpeciesFacts(speciesFacts, ReferenceManagerTest.GetAReference());

                // Verify that delete has been done.
                Data.ArtDatabankenService.SpeciesFactManager.UpdateSpeciesFacts(speciesFacts);
                foreach (Data.ArtDatabankenService.SpeciesFact speciesFact in speciesFacts)
                {
                    if (speciesFact.AllowManualUpdate)
                    {
                        Assert.IsTrue(!speciesFact.HasId ||
                                       (speciesFact.MainField.Type.DataType == FactorFieldDataTypeId.Enum));
                    }
                }
            }
            Thread.Sleep(6000);

            // Test creation of species fact.
            using (WebTransaction transaction = new WebTransaction(5))
            {
                // Create some species facts.
                changedSpeciesFacts = new Data.ArtDatabankenService.SpeciesFactList();
                userParameterSelection = new UserParameterSelection();
                userParameterSelection.Taxa.Merge(ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxa(TaxonManagerTest.GetTaxaIds(), TaxonInformationType.Basic));
                userParameterSelection.Factors.Merge(Data.ArtDatabankenService.FactorManager.GetFactors(FactorManagerTest.GetFactorIds()));
                speciesFacts = Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFactsByUserParameterSelection(userParameterSelection);
                foreach (Data.ArtDatabankenService.SpeciesFact speciesFact in speciesFacts)
                {
                    if (speciesFact.AllowManualUpdate &&
                        !speciesFact.HasId)
                    {
                        switch (speciesFact.MainField.Type.DataType)
                        {
                            case FactorFieldDataTypeId.Boolean:
                                speciesFact.MainField.Value = true;
                                break;
                            case FactorFieldDataTypeId.Double:
                                speciesFact.MainField.Value = 1;
                                break;
                            case FactorFieldDataTypeId.Int32:
                                speciesFact.MainField.Value = 1;
                                break;
                            case FactorFieldDataTypeId.String:
                                speciesFact.MainField.Value = "1";
                                break;
                            default:
                                continue;
                        }
                        changedSpeciesFacts.Add(speciesFact);
                    }
                }
                Data.ArtDatabankenService.SpeciesFactManager.UpdateSpeciesFacts(speciesFacts, ReferenceManagerTest.GetAReference());

                // Verify that creation of species facts has been done.
                Data.ArtDatabankenService.SpeciesFactManager.UpdateSpeciesFacts(speciesFacts);
                foreach (Data.ArtDatabankenService.SpeciesFact speciesFact in speciesFacts)
                {
                    if (changedSpeciesFacts.Exists(speciesFact))
                    {
                        Assert.IsTrue(speciesFact.HasId);
                        Assert.IsTrue(speciesFact.HasUpdateDate);
                        Assert.IsTrue(speciesFact.UpdateUserFullName.IsNotEmpty());
                        Assert.AreEqual(1, Int32.Parse(speciesFact.MainField.Value.ToString()));
                    }
                }
            }
            Thread.Sleep(6000);

            // Test update of data.
            using (WebTransaction transaction = new WebTransaction(5))
            {
                // Get species fact.
                changedSpeciesFacts = new Data.ArtDatabankenService.SpeciesFactList();
                foreach (Data.ArtDatabankenService.SpeciesFact speciesFact in GetSomeSpeciesFacts())
                {
                    if (speciesFact.AllowManualUpdate &&
                        speciesFact.HasId)
                    {
                        changedSpeciesFacts.Add(speciesFact);
                        break;
                    }
                }
                changedSpeciesFact = changedSpeciesFacts[0];

                // Test change of reference id.
                oldReferenceId = changedSpeciesFact.Reference.Id;
                oldUpdateUser = changedSpeciesFact.UpdateUserFullName;
                reference = Data.ArtDatabankenService.ReferenceManager.GetReferences()[3];
                changedSpeciesFact.Reference = reference;
                Data.ArtDatabankenService.SpeciesFactManager.UpdateSpeciesFacts(changedSpeciesFacts, ReferenceManagerTest.GetAReference());
                Data.ArtDatabankenService.SpeciesFactManager.UpdateSpeciesFacts(changedSpeciesFacts);
                Assert.AreNotEqual(oldReferenceId, changedSpeciesFact.Reference.Id);
                Assert.AreEqual(reference.Id, changedSpeciesFact.Reference.Id);

                // Test change of update date.
                oldUpdateDate = changedSpeciesFact.UpdateDate;
                changedSpeciesFact.Reference = Data.ArtDatabankenService.ReferenceManager.GetReferences()[4];
                Data.ArtDatabankenService.SpeciesFactManager.UpdateSpeciesFacts(changedSpeciesFacts, ReferenceManagerTest.GetAReference());
                Data.ArtDatabankenService.SpeciesFactManager.UpdateSpeciesFacts(changedSpeciesFacts);
                Assert.IsTrue(oldUpdateDate <= changedSpeciesFact.UpdateDate);

                // Test change of update user.
                Assert.AreNotEqual(oldUpdateUser, changedSpeciesFact.UpdateUserFullName);

                // Test change of field values.
                field = changedSpeciesFact.Field5;
                stringValue = "4232";
                Assert.AreNotEqual(stringValue, field.Value.ToString());
                field.Value = stringValue;
                Data.ArtDatabankenService.SpeciesFactManager.UpdateSpeciesFacts(changedSpeciesFacts, ReferenceManagerTest.GetAReference());
                Data.ArtDatabankenService.SpeciesFactManager.UpdateSpeciesFacts(changedSpeciesFacts);
                Assert.IsTrue(field.HasValue);
                Assert.AreEqual(stringValue, field.Value.ToString());

                // Test change of quality.
                oldQualityId = changedSpeciesFact.Quality.Id;
                changedSpeciesFact.Quality = Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFactQualities()[5];
                Data.ArtDatabankenService.SpeciesFactManager.UpdateSpeciesFacts(changedSpeciesFacts, ReferenceManagerTest.GetAReference());
                Data.ArtDatabankenService.SpeciesFactManager.UpdateSpeciesFacts(changedSpeciesFacts);
                Assert.AreNotEqual(oldQualityId, changedSpeciesFact.Quality.Id);
                Assert.AreEqual(Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFactQualities()[5].Id, changedSpeciesFact.Quality.Id);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateSpeciesFactsNullSpeciesFactsError()
        {
            Data.ArtDatabankenService.SpeciesFactManager.UpdateSpeciesFacts(null, ReferenceManagerTest.GetAReference());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateSpeciesFactsNullReferenceError()
        {
            Data.ArtDatabankenService.SpeciesFactManager.UpdateSpeciesFacts(GetSomeSpeciesFacts(), null);
        }

        [TestMethod]
        public void CanLoadSpeciesFactForNonExistingTaxon()
        {
            var paramSelection = new UserParameterSelection();
            paramSelection.Taxa = new Data.ArtDatabankenService.TaxonList();
            paramSelection.Taxa.Add(11);

            paramSelection.Factors = new Data.ArtDatabankenService.FactorList();
            paramSelection.Factors.Add(1991);

            paramSelection.IndividualCategories = IndividualCategoryManagerTest.GetSomeIndividualCategories();
            paramSelection.Periods = PeriodManagerTest.GetSomePeriods();

            var speciesFacts = Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFactsByUserParameterSelection(paramSelection);

            Assert.IsTrue(speciesFacts.Count == 1);
        }

        [TestMethod]
        public void CanCreateNewSpeciesFactForNonExistentTaxon()
        {
            var paramSelection = new UserParameterSelection();
            paramSelection.Taxa = new Data.ArtDatabankenService.TaxonList();
            paramSelection.Taxa.Add(11);

            paramSelection.Factors = new Data.ArtDatabankenService.FactorList();
            paramSelection.Factors.Add(1991);

            paramSelection.IndividualCategories = IndividualCategoryManagerTest.GetSomeIndividualCategories();
            paramSelection.Periods = PeriodManagerTest.GetSomePeriods();

            var speciesFacts = Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFactsByUserParameterSelection(paramSelection);
            Data.ArtDatabankenService.SpeciesFactManager.UpdateSpeciesFacts(speciesFacts);

        }

        [TestMethod]
        public void CanUpdateExistingSpeciesFactForNonExistentTaxon()
        {
            var paramSelection = new UserParameterSelection();
            paramSelection.Taxa = new Data.ArtDatabankenService.TaxonList();
            paramSelection.Taxa.Add(11);

            paramSelection.Factors = new Data.ArtDatabankenService.FactorList();
            paramSelection.Factors.Add(1991);

            paramSelection.IndividualCategories = IndividualCategoryManagerTest.GetSomeIndividualCategories();
            paramSelection.Periods = PeriodManagerTest.GetSomePeriods();

            var speciesFacts = Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFactsByUserParameterSelection(paramSelection);
            Data.ArtDatabankenService.SpeciesFactManager.UpdateSpeciesFacts(speciesFacts);


            Data.ArtDatabankenService.SpeciesFactManager.UpdateSpeciesFacts(speciesFacts);


        }
    }
}

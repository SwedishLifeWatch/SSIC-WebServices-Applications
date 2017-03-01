using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    using FactorFieldEnumValue = ArtDatabanken.Data.ArtDatabankenService.FactorFieldEnumValue;
    using PeriodId = ArtDatabanken.Data.ArtDatabankenService.PeriodId;

    [TestClass]
    public class TaxonManagerTest : TestBase
    {
        private const Int32 SCIENTIFIC_TAXON_NAME_TYPE_ID = 0;
        private const Int32 VALID_TAXON_NAME_USE_TYPE_ID = 0;

        [TestMethod]
        public void GetActionPlanTaxa()
        {
            Data.ArtDatabankenService.Factor factor;
            Data.ArtDatabankenService.FactorFieldEnum factorFieldEnum;
            SpeciesFactCondition speciesFactCondition;
            SpeciesFactFieldCondition speciesFactFieldCondition;
            Data.ArtDatabankenService.TaxonList taxa;

            speciesFactCondition = new SpeciesFactCondition();
            // factor = FactorManager.GetFactor(FactorId.ActionPlan);
            factor = Data.ArtDatabankenService.FactorManager.GetFactor(2017);
            speciesFactCondition.Factors.Add(factor);
            speciesFactCondition.IndividualCategories.Add(IndividualCategoryManager.GetDefaultIndividualCategory());
            factorFieldEnum = factor.FactorDataType.Field1.FactorFieldEnum;
            foreach (FactorFieldEnumValue enumValue in factorFieldEnum.Values)
            {
                speciesFactFieldCondition = new SpeciesFactFieldCondition();
                speciesFactFieldCondition.FactorField = factor.FactorDataType.Field1;
                speciesFactFieldCondition.SetValue(enumValue.KeyInt);
                speciesFactCondition.SpeciesFactFieldConditions.Add(speciesFactFieldCondition);
            }

            // The merge is necessary if there are problems in Dyntaxa.
            taxa = new Data.ArtDatabankenService.TaxonList();
            taxa.Merge(Data.ArtDatabankenService.TaxonManager.GetTaxaByQuery(speciesFactCondition, TaxonInformationType.Basic));
            Assert.IsTrue(taxa.IsNotEmpty());
            Assert.AreEqual(1, taxa.Count);
        }

        public static Data.ArtDatabankenService.TaxonTreeNode GetBearTaxonTreeNode()
        {
            List<Int32> taxonIds;
            Data.ArtDatabankenService.TaxonTreeNodeList taxonTrees;
            Data.ArtDatabankenService.TaxonTreeSearchCriteria searchCriteria;

            searchCriteria = new Data.ArtDatabankenService.TaxonTreeSearchCriteria();
            taxonIds = new List<Int32>();
            taxonIds.Add(BEAR_TAXON_ID);
            searchCriteria.RestrictSearchToTaxonIds = taxonIds;
            taxonTrees = Data.ArtDatabankenService.TaxonManager.GetTaxonTreesBySearchCriteria(searchCriteria);
            return taxonTrees[0];
        }

        public static Data.ArtDatabankenService.TaxonTreeNode GetHawkBirdsTaxonTree()
        {
            List<Int32> taxonIds;
            Data.ArtDatabankenService.TaxonTreeNodeList taxonTrees;
            Data.ArtDatabankenService.TaxonTreeSearchCriteria searchCriteria;

            searchCriteria = new Data.ArtDatabankenService.TaxonTreeSearchCriteria();
            taxonIds = new List<Int32>();
            taxonIds.Add(HAWK_BIRDS_TAXON_ID);
            searchCriteria.RestrictSearchToTaxonIds = taxonIds;
            taxonTrees = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonTreesBySearchCriteria(searchCriteria);
            return taxonTrees[0];
        }

        [TestMethod]
        public void GetMammalsTaxonTree()
        {
            const Int32 MAMMALS_TAXON_ID = 4000107;
            List<Int32> taxonIds;
            Data.ArtDatabankenService.TaxonTreeNodeList taxonTrees;
            Data.ArtDatabankenService.TaxonTreeSearchCriteria searchCriteria;

            searchCriteria = new Data.ArtDatabankenService.TaxonTreeSearchCriteria();
            taxonIds = new List<Int32>();
            taxonIds.Add(MAMMALS_TAXON_ID);
            searchCriteria.RestrictSearchToTaxonIds = taxonIds;
            taxonTrees = Data.ArtDatabankenService.TaxonManager.GetTaxonTreesBySearchCriteria(searchCriteria);
            Assert.IsNotNull(taxonTrees);
            Assert.IsTrue(taxonTrees[0].Children.IsNotEmpty());
        }

        [TestMethod]
        public void GetNatura2000Taxa()
        {
            Data.ArtDatabankenService.Factor factor;
            SpeciesFactCondition speciesFactCondition;
            SpeciesFactFieldCondition speciesFactFieldCondition;
            Data.ArtDatabankenService.TaxonList taxa;

            speciesFactCondition = new SpeciesFactCondition();
            factor = Data.ArtDatabankenService.FactorManager.GetFactor(Data.ArtDatabankenService.FactorId.Natura2000BirdsDirective);
            speciesFactCondition.Factors.Add(factor);
            speciesFactCondition.Factors.Add(Data.ArtDatabankenService.FactorManager.GetFactor(Data.ArtDatabankenService.FactorId.Natura2000HabitatsDirectiveArticle2));
            speciesFactCondition.Factors.Add(Data.ArtDatabankenService.FactorManager.GetFactor(Data.ArtDatabankenService.FactorId.Natura2000HabitatsDirectiveArticle4));
            speciesFactCondition.Factors.Add(Data.ArtDatabankenService.FactorManager.GetFactor(Data.ArtDatabankenService.FactorId.Natura2000HabitatsDirectiveArticle5));
            speciesFactCondition.IndividualCategories.Add(IndividualCategoryManager.GetDefaultIndividualCategory());
            speciesFactFieldCondition = new SpeciesFactFieldCondition();
            speciesFactFieldCondition.FactorField = factor.FactorDataType.Field1;
            speciesFactFieldCondition.SetValue(true);
            speciesFactCondition.SpeciesFactFieldConditions.Add(speciesFactFieldCondition);

            // The merge is necessary if there are problems in Dyntaxa.
            taxa = new Data.ArtDatabankenService.TaxonList();
            taxa.Merge(Data.ArtDatabankenService.TaxonManager.GetTaxaByQuery(speciesFactCondition, TaxonInformationType.Basic));
            Assert.IsTrue(taxa.IsNotEmpty());
            Assert.AreEqual(308, taxa.Count);
        }

        public static ArtDatabanken.Data.ArtDatabankenService.Taxon GetOneTaxon()
        {
            return ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxon(BEAR_TAXON_ID, TaxonInformationType.Basic);
        }

        [TestMethod]
        public void GetProtectedByLawTaxa()
        {
            Data.ArtDatabankenService.Factor factor;
            SpeciesFactCondition speciesFactCondition;
            Data.ArtDatabankenService.TaxonList taxa;

            speciesFactCondition = new SpeciesFactCondition();
            //factor = FactorManager.GetFactor(FactorId.ProtectedByLaw);
            factor = Data.ArtDatabankenService.FactorManager.GetFactor(2009);
            speciesFactCondition.Factors.Add(factor);
            speciesFactCondition.IndividualCategories.Add(IndividualCategoryManager.GetDefaultIndividualCategory());

            // The merge is necessary if there are problems in Dyntaxa.
            taxa = new Data.ArtDatabankenService.TaxonList();
            taxa.Merge(Data.ArtDatabankenService.TaxonManager.GetTaxaByQuery(speciesFactCondition, TaxonInformationType.Basic));
            Assert.IsTrue(taxa.IsNotEmpty());
            Assert.AreEqual(508, taxa.Count);
        }

        [TestMethod]
        public void GetProtectedTaxa()
        {
            Data.ArtDatabankenService.TaxonList taxa;

            taxa = Data.ArtDatabankenService.TaxonManager.GetProtectedTaxa(false, TaxonInformationType.Basic);
            Assert.IsTrue(taxa.IsNotEmpty());
            Assert.IsTrue(100 < taxa.Count);
            Assert.IsTrue(taxa.Count < 1000);
        }

        [TestMethod]
        public void GetRelistedTaxa()
        {
            Data.ArtDatabankenService.Factor factor;
            RedListCategoryEnum redListCategory;
            SpeciesFactCondition speciesFactCondition;
            SpeciesFactFieldCondition speciesFactFieldCondition;
            Data.ArtDatabankenService.TaxonList taxa;

            speciesFactCondition = new SpeciesFactCondition();
            factor = Data.ArtDatabankenService.FactorManager.GetFactor(Data.ArtDatabankenService.FactorId.RedlistCategory);
            speciesFactCondition.Factors.Add(factor);
            speciesFactCondition.IndividualCategories.Add(IndividualCategoryManager.GetDefaultIndividualCategory());
            speciesFactCondition.Periods.Add(PeriodManager.GetCurrentPublicPeriod());

            for (redListCategory = RedListCategoryEnum.DD; redListCategory <= RedListCategoryEnum.NT; redListCategory++)
            {
                speciesFactFieldCondition = new SpeciesFactFieldCondition();
                speciesFactFieldCondition.FactorField = factor.FactorDataType.Field1;
                speciesFactFieldCondition.SetValue((Int32)redListCategory);
                speciesFactCondition.SpeciesFactFieldConditions.Add(speciesFactFieldCondition);
            }

            // The merge is necessary if there are problems in Dyntaxa.
            taxa = new Data.ArtDatabankenService.TaxonList();
            taxa.Merge(Data.ArtDatabankenService.TaxonManager.GetTaxaByQuery(speciesFactCondition, TaxonInformationType.Basic));
            Assert.IsTrue(taxa.IsNotEmpty());
            Assert.AreEqual(4261, taxa.Count);
        }

        public static TaxonNameType GetScientificTaxonNameType()
        {
            return ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonNameType(SCIENTIFIC_TAXON_NAME_TYPE_ID);
        }

        public static ArtDatabanken.Data.ArtDatabankenService.TaxonList GetSomeTaxa()
        {
            return GetSomeTaxa(4);
        }

        public static ArtDatabanken.Data.ArtDatabankenService.TaxonList GetSomeTaxa(Int32 taxonCount)
        {
            ArtDatabanken.Data.ArtDatabankenService.TaxonList taxa;

            taxa = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxa(GetSomeTaxonIds(taxonCount), TaxonInformationType.Basic);
            return taxa;
        }

        public static List<Int32> GetSomeTaxonIds(Int32 taxonIdCount)
        {
            List<Int32> taxonIds;

            taxonIds = new List<Int32>();
            if (taxonIdCount > 0)
            {
                taxonIds.Add(BADGER_TAXON_ID);
            }
            if (taxonIdCount > 1)
            {
                taxonIds.Add(BEAVER_TAXON_ID);
            }
            if (taxonIdCount > 2)
            {
                taxonIds.Add(HEDGEHOG_TAXON_ID);
            }
            if (taxonIdCount > 3)
            {
                taxonIds.Add(FALLOW_DEER_TAXON_ID);
            }
            return taxonIds;
        }

        public static TaxonType GetSpeciesTaxonType()
        {
            return ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonType(TaxonTypeId.Species);
        }

        [TestMethod]
        public void GetTaxa()
        {
            Boolean taxonFound;
            Data.ArtDatabankenService.TaxonList taxa;

            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            {
                taxa = Data.ArtDatabankenService.TaxonManager.GetTaxa(GetTaxaIds(), taxonInformationType);
                Assert.IsNotNull(taxa);
                Assert.AreEqual(taxa.Count, GetTaxaIds().Count);

                foreach (Data.ArtDatabankenService.Taxon taxon in taxa)
                {
                    Assert.AreEqual(taxon.TaxonInformationType, taxonInformationType);

                    taxonFound = false;
                    foreach (Int32 taxonId in GetTaxaIds())
                    {
                        if (taxonId == taxon.Id)
                        {
                            taxonFound = true;
                            break;
                        }
                    }
                    Assert.IsTrue(taxonFound);
                }
            }
        }

        [TestMethod]
        public void GetTaxaByOrganismOrRedlist()
        {
            Data.ArtDatabankenService.TaxonList taxa;
            Int32? endangeredListId, organismGroupId, redlistCategoryId;

            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            {
                organismGroupId = 5;
                endangeredListId = 1;
                redlistCategoryId = null;
                taxa = Data.ArtDatabankenService.TaxonManager.GetTaxaByOrganismOrRedlist(organismGroupId,
                                                               endangeredListId,
                                                               redlistCategoryId,
                                                               taxonInformationType);
                Assert.IsTrue(taxa.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetHostTaxaByTaxonId()
        {
            Data.ArtDatabankenService.TaxonList taxa;
            Int32 taxonId;

            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            {
                taxonId = 101656; //Trummgräshoppa
                taxa = Data.ArtDatabankenService.TaxonManager.GetHostTaxaByTaxonId(taxonId, taxonInformationType);
                Assert.IsTrue(taxa.IsNotEmpty());
                Assert.IsTrue(taxa.Count > 5);

            }
        }

        [TestMethod]
        public void GetTaxaByHostTaxonId()
        {
            Data.ArtDatabankenService.TaxonList taxa;
            Int32 hostTaxonId;

            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            {
                hostTaxonId = 1006592; //Salix
                taxa = Data.ArtDatabankenService.TaxonManager.GetTaxaByHostTaxonId(hostTaxonId, taxonInformationType);
                Assert.IsTrue(taxa.IsNotEmpty());
                Assert.IsTrue(taxa.Count > 25);

            }
        }

        [TestMethod]
        public void GetTaxaByQuery()
        {
            Data.ArtDatabankenService.Factor factor;
            Data.ArtDatabankenService.FactorField factorField;
            SpeciesFactCondition speciesFactCondition;
            SpeciesFactFieldCondition speciesFactFieldCondition;
            TaxonInformationType taxonInformationType;
            Data.ArtDatabankenService.TaxonList taxa;

            taxonInformationType = TaxonInformationType.Basic;

            // Test one factor.
            speciesFactCondition = new SpeciesFactCondition();
            speciesFactCondition.Factors.Add(FactorManagerTest.GetOneFactor());
            taxa = Data.ArtDatabankenService.TaxonManager.GetTaxaByQuery(speciesFactCondition, taxonInformationType);
            Assert.IsTrue(taxa.IsNotEmpty());

            // Test several factors.
            speciesFactCondition = new SpeciesFactCondition();
            speciesFactCondition.Factors.AddRange(FactorManagerTest.GetSomeFactors());
            taxa = Data.ArtDatabankenService.TaxonManager.GetTaxaByQuery(speciesFactCondition, taxonInformationType);
            Assert.IsTrue(taxa.IsNotEmpty());

            // Test one period.
            speciesFactCondition = new SpeciesFactCondition();
            speciesFactCondition.Factors.Add(Data.ArtDatabankenService.FactorManager.GetFactor(Data.ArtDatabankenService.FactorId.RedlistCategory));
            speciesFactCondition.Periods.Add(PeriodManager.GetPeriod(PeriodId.Year2005));
            taxa = Data.ArtDatabankenService.TaxonManager.GetTaxaByQuery(speciesFactCondition, taxonInformationType);
            Assert.IsTrue(taxa.IsNotEmpty());

            // Test several periods.
            speciesFactCondition = new SpeciesFactCondition();
            speciesFactCondition.Factors.Add(Data.ArtDatabankenService.FactorManager.GetFactor(Data.ArtDatabankenService.FactorId.RedlistCategory));
            speciesFactCondition.Periods.AddRange(PeriodManager.GetPeriods());
            taxa = Data.ArtDatabankenService.TaxonManager.GetTaxaByQuery(speciesFactCondition, taxonInformationType);
            Assert.IsTrue(taxa.IsNotEmpty());

            // Test one species fact field enum (KeyInt) condition.
            speciesFactCondition = new SpeciesFactCondition();
            factor = Data.ArtDatabankenService.FactorManager.GetFactor(Data.ArtDatabankenService.FactorId.Redlist_OrganismLabel1);
            Assert.IsTrue(taxa.IsNotEmpty());
            speciesFactCondition.Factors.Add(factor);
            speciesFactFieldCondition = new SpeciesFactFieldCondition();
            factorField = factor.FactorDataType.MainField;
            speciesFactFieldCondition.FactorField = factorField;
            speciesFactFieldCondition.SetValue(factorField.FactorFieldEnum.Values[3].KeyInt);
            speciesFactCondition.SpeciesFactFieldConditions.Add(speciesFactFieldCondition);
            taxa = Data.ArtDatabankenService.TaxonManager.GetTaxaByQuery(speciesFactCondition, taxonInformationType);
            Assert.IsTrue(taxa.IsNotEmpty());

            // Test several species fact field enum (KeyInt) conditions.
            speciesFactCondition = new SpeciesFactCondition();
            factor = Data.ArtDatabankenService.FactorManager.GetFactor(Data.ArtDatabankenService.FactorId.Redlist_OrganismLabel1);
            Assert.IsTrue(taxa.IsNotEmpty());
            speciesFactCondition.Factors.Add(factor);

            speciesFactFieldCondition = new SpeciesFactFieldCondition();
            factorField = factor.FactorDataType.MainField;
            speciesFactFieldCondition.FactorField = factorField;
            speciesFactFieldCondition.SetValue(factorField.FactorFieldEnum.Values[3].KeyInt);
            speciesFactCondition.SpeciesFactFieldConditions.Add(speciesFactFieldCondition);

            speciesFactFieldCondition = new SpeciesFactFieldCondition();
            factorField = factor.FactorDataType.MainField;
            speciesFactFieldCondition.FactorField = factorField;
            speciesFactFieldCondition.SetValue(factorField.FactorFieldEnum.Values[4].KeyInt);
            speciesFactCondition.SpeciesFactFieldConditions.Add(speciesFactFieldCondition);

            speciesFactFieldCondition = new SpeciesFactFieldCondition();
            factorField = factor.FactorDataType.MainField;
            speciesFactFieldCondition.FactorField = factorField;
            speciesFactFieldCondition.SetValue(factorField.FactorFieldEnum.Values[5].KeyInt);
            speciesFactCondition.SpeciesFactFieldConditions.Add(speciesFactFieldCondition);

            taxa = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxaByQuery(speciesFactCondition, taxonInformationType);
            Assert.IsTrue(taxa.IsNotEmpty());

            // Test one species fact field enum (KeyText) condition.
            speciesFactCondition = new SpeciesFactCondition();
            factor = Data.ArtDatabankenService.FactorManager.GetFactor(Data.ArtDatabankenService.FactorId.Redlist_TaxonType);
            Assert.IsTrue(taxa.IsNotEmpty());
            speciesFactCondition.Factors.Add(factor);
            speciesFactCondition.Periods.Add(PeriodManager.GetPeriod(2));
            speciesFactFieldCondition = new SpeciesFactFieldCondition();
            factorField = factor.FactorDataType.MainField;
            speciesFactFieldCondition.FactorField = factorField;
            speciesFactFieldCondition.SetValue(factorField.FactorFieldEnum.Values[0].KeyText);
            speciesFactFieldCondition.IsEnumAsString = true;
            speciesFactCondition.SpeciesFactFieldConditions.Add(speciesFactFieldCondition);
            taxa = Data.ArtDatabankenService.TaxonManager.GetTaxaByQuery(speciesFactCondition, taxonInformationType);
            Assert.IsTrue(taxa.IsNotEmpty());

            // Test several species fact field enum (KeyText) condition.
            speciesFactCondition = new SpeciesFactCondition();
            factor = Data.ArtDatabankenService.FactorManager.GetFactor(Data.ArtDatabankenService.FactorId.Redlist_TaxonType);
            Assert.IsTrue(taxa.IsNotEmpty());
            speciesFactCondition.Factors.Add(factor);
            speciesFactCondition.Periods.Add(PeriodManager.GetPeriod(2));

            speciesFactFieldCondition = new SpeciesFactFieldCondition();
            factorField = factor.FactorDataType.MainField;
            speciesFactFieldCondition.FactorField = factorField;
            speciesFactFieldCondition.SetValue(factorField.FactorFieldEnum.Values[0].KeyText);
            speciesFactFieldCondition.IsEnumAsString = true;
            speciesFactCondition.SpeciesFactFieldConditions.Add(speciesFactFieldCondition);

            speciesFactFieldCondition = new SpeciesFactFieldCondition();
            factorField = factor.FactorDataType.MainField;
            speciesFactFieldCondition.FactorField = factorField;
            speciesFactFieldCondition.SetValue(factorField.FactorFieldEnum.Values[1].KeyText);
            speciesFactFieldCondition.IsEnumAsString = true;
            speciesFactCondition.SpeciesFactFieldConditions.Add(speciesFactFieldCondition);

            speciesFactFieldCondition = new SpeciesFactFieldCondition();
            factorField = factor.FactorDataType.MainField;
            speciesFactFieldCondition.FactorField = factorField;
            speciesFactFieldCondition.SetValue(factorField.FactorFieldEnum.Values[2].KeyText);
            speciesFactFieldCondition.IsEnumAsString = true;
            speciesFactCondition.SpeciesFactFieldConditions.Add(speciesFactFieldCondition);

            taxa = Data.ArtDatabankenService.TaxonManager.GetTaxaByQuery(speciesFactCondition, taxonInformationType);
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxaBySearchCriteria()
        {
            Data.ArtDatabankenService.TaxonList taxa;
            Data.ArtDatabankenService.TaxonSearchCriteria searchCriteria;

            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            {
                for (WebService.TaxonSearchScope taxonReturnScope = WebService.TaxonSearchScope.NoScope; taxonReturnScope <= WebService.TaxonSearchScope.AllChildTaxa; taxonReturnScope++)
                {
                    searchCriteria = new Data.ArtDatabankenService.TaxonSearchCriteria();
                    searchCriteria.TaxonInformationType = taxonInformationType;
                    searchCriteria.RestrictReturnToScope = taxonReturnScope;

                    searchCriteria.RestrictSearchToTaxonIds = null;
                    searchCriteria.RestrictSearchToTaxonTypeIds = null;
                    searchCriteria.TaxonNameSearchString = null;
                    searchCriteria.RestrictSearchToSwedishSpecies = false;
                    taxa = Data.ArtDatabankenService.TaxonManager.GetTaxaBySearchCriteria(searchCriteria);
                    if (taxonReturnScope == WebService.TaxonSearchScope.NoScope)
                    {
                        Assert.IsTrue(taxa.IsNotEmpty());
                    }
                    else
                    {
                        Assert.IsTrue(taxa.IsEmpty());
                    }

                    searchCriteria.RestrictSearchToTaxonIds = null;
                    searchCriteria.RestrictSearchToTaxonTypeIds = null;
                    searchCriteria.TaxonNameSearchString = "björn";
                    searchCriteria.RestrictSearchToSwedishSpecies = true;
                    taxa = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxaBySearchCriteria(searchCriteria);
                    Assert.IsNotNull(taxa);
                    Assert.IsTrue(taxa.IsNotEmpty());
                    Assert.AreEqual(taxa.Count, 1);

                    searchCriteria.RestrictSearchToTaxonIds = null;
                    searchCriteria.RestrictSearchToTaxonTypeIds = null;
                    searchCriteria.TaxonNameSearchString = "björn%";
                    searchCriteria.RestrictSearchToSwedishSpecies = false;
                    taxa = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxaBySearchCriteria(searchCriteria);
                    Assert.IsNotNull(taxa);
                    Assert.IsTrue(taxa.IsNotEmpty());
                    Assert.IsTrue(taxa.Count > 1);

                    searchCriteria.RestrictSearchToTaxonIds = GetTaxaIds();
                    searchCriteria.RestrictSearchToTaxonTypeIds = null;
                    searchCriteria.TaxonNameSearchString = "björn%";
                    searchCriteria.RestrictSearchToSwedishSpecies = true;
                    taxa = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxaBySearchCriteria(searchCriteria);
                    Assert.IsNotNull(taxa);
                    Assert.IsTrue(taxa.IsNotEmpty());
                    Assert.AreEqual(taxa.Count, 1);

                    searchCriteria.RestrictSearchToTaxonIds = GetTaxaIds();
                    searchCriteria.RestrictSearchToTaxonTypeIds = GetTaxonTypeIds();
                    searchCriteria.TaxonNameSearchString = "björn%";
                    searchCriteria.RestrictSearchToSwedishSpecies = false;
                    taxa = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxaBySearchCriteria(searchCriteria);
                    Assert.IsNotNull(taxa);
                    Assert.IsTrue(taxa.IsNotEmpty());
                    Assert.AreEqual(taxa.Count, 1);

                    searchCriteria.RestrictSearchToTaxonIds = null;
                    searchCriteria.RestrictSearchToTaxonTypeIds = GetTaxonTypeIds();
                    searchCriteria.TaxonNameSearchString = "%björn%";
                    searchCriteria.RestrictSearchToSwedishSpecies = true;
                    taxa = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxaBySearchCriteria(searchCriteria);
                    Assert.IsNotNull(taxa);
                    Assert.IsTrue(taxa.IsNotEmpty());
                    Assert.IsTrue(taxa.Count > 1);

                    searchCriteria.RestrictSearchToTaxonIds = null;
                    searchCriteria.RestrictSearchToTaxonTypeIds = null;
                    searchCriteria.TaxonNameSearchString = "blåvingad sandgräshoppa";
                    searchCriteria.RestrictReturnToSwedishSpecies = true;
                    taxa = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxaBySearchCriteria(searchCriteria);
                    Assert.IsTrue(taxa.IsEmpty());
                }
            }

            // Test getting parent taxa.
            searchCriteria = new ArtDatabanken.Data.ArtDatabankenService.TaxonSearchCriteria();
            searchCriteria.TaxonInformationType = TaxonInformationType.Basic;
            searchCriteria.RestrictReturnToScope = WebService.TaxonSearchScope.AllParentTaxa;
            searchCriteria.TaxonNameSearchString = "björn";
            taxa = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxaBySearchCriteria(searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetTaxaBySearchCriteriaNullError()
        {
            ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxaBySearchCriteria(null);
        }

        [TestMethod]
        public void GetHostTaxa()
        {

            Int32 factorId = 1142;
            ArtDatabanken.Data.ArtDatabankenService.TaxonList taxa;

            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            {
                taxa = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetHostTaxa(factorId, taxonInformationType);
                Assert.IsNotNull(taxa);
                Assert.IsTrue(taxa.Count > 10);
                if (taxonInformationType == TaxonInformationType.Basic)
                {
                    ArtDatabanken.Data.ArtDatabankenService.Taxon taxon = taxa.Get(0);
                    Assert.AreEqual(taxon.ScientificName, "Saknas");
                }
            }
        }

        public static List<Int32> GetTaxaIds()
        {
            List<Int32> taxaIds;

            taxaIds = new List<int>();
            taxaIds.Add(BEAR_TAXON_ID);
            taxaIds.Add(GOLDEN_EAGLE_TAXON_ID);
            taxaIds.Add(NORTHERN_HAWK_OWL_TAXON_ID);
            taxaIds.Add(RED_FOX_TAXON_ID);
            return taxaIds;
        }

        public static ArtDatabanken.Data.ArtDatabankenService.TaxonList GetTaxaList()
        {
            return ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxa(TaxonManagerTest.GetTaxaIds(), TaxonInformationType.Basic);
        }

        [TestMethod]
        public void GetTaxon()
        {
            ArtDatabanken.Data.ArtDatabankenService.Taxon taxon;

            foreach (Int32 taxonId in GetTaxaIds())
            {
                foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
                {
                    taxon = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxon(taxonId, taxonInformationType);
                    Assert.IsNotNull(taxon);
                    Assert.AreEqual(taxon.Id, taxonId);
                    Assert.AreEqual(taxon.TaxonInformationType, taxonInformationType);
                }
            }
        }

        public static ArtDatabanken.Data.ArtDatabankenService.TaxonName GetTaxonName()
        {
            ArtDatabanken.Data.ArtDatabankenService.TaxonNameList taxonNames;

            var searchCriteria = new ArtDatabanken.Data.ArtDatabankenService.TaxonNameSearchCriteria();
            searchCriteria.NameSearchString = "björn";
            taxonNames = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonNamesBySearchCriteria(searchCriteria);
            return taxonNames[0];
        }

        public static ArtDatabanken.Data.ArtDatabankenService.TaxonNameList GetSomeTaxonNames()
        {
            // Get several taxa names.
            var searchCriteria = new ArtDatabanken.Data.ArtDatabankenService.TaxonNameSearchCriteria();
            searchCriteria.NameSearchString = "%björn%";
            return ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonNamesBySearchCriteria(searchCriteria);
        }

        [TestMethod]
        public void GetTaxonNames()
        {
            Int32 taxonId;
            Data.ArtDatabankenService.TaxonNameList taxonNames;

            taxonId = GetSomeTaxonIds(1)[0];
            taxonNames = Data.ArtDatabankenService.TaxonManager.GetTaxonNames(taxonId);
            Assert.IsTrue(taxonNames.IsNotEmpty());

            // Test problem with searching for taxon names equal to 'mossa'.
            var searchCriteria = new Data.ArtDatabankenService.TaxonNameSearchCriteria();
            searchCriteria.NameSearchString = "mossa";
            searchCriteria.NameSearchMethod = WebService.SearchStringComparisonMethod.Iterative;
            taxonNames = Data.ArtDatabankenService.TaxonManager.GetTaxonNamesBySearchCriteria(searchCriteria);
            taxonNames.UpdateTaxa();
        }

        [TestMethod]
        public void GetTaxonNamesBySearchCriteria()
        {
            ArtDatabanken.Data.ArtDatabankenService.TaxonNameList taxonNames;
            ArtDatabanken.Data.ArtDatabankenService.TaxonNameSearchCriteria searchCriteria;

            foreach (WebService.SearchStringComparisonMethod nameSearchMethod in Enum.GetValues(typeof(WebService.SearchStringComparisonMethod)))
            {
                // Get no taxa names.
                searchCriteria = new ArtDatabanken.Data.ArtDatabankenService.TaxonNameSearchCriteria();
                searchCriteria.NameSearchString = "No taxon name";
                searchCriteria.NameSearchMethod = nameSearchMethod;
                taxonNames = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonNamesBySearchCriteria(searchCriteria);
                Assert.IsNull(taxonNames);

                // Get one taxon name.
                if (nameSearchMethod != WebService.SearchStringComparisonMethod.Contains)
                {
                    searchCriteria = new ArtDatabanken.Data.ArtDatabankenService.TaxonNameSearchCriteria();
                    searchCriteria.NameSearchString = "björn";
                    searchCriteria.NameSearchMethod = nameSearchMethod;
                    taxonNames =
                        ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonNamesBySearchCriteria(
                            searchCriteria);
                    Assert.IsNotNull(taxonNames);
                    Assert.AreEqual(taxonNames.Count, 1);
                }

                // Get several taxa names.
                if (nameSearchMethod != WebService.SearchStringComparisonMethod.Exact)
                {
                    searchCriteria = new ArtDatabanken.Data.ArtDatabankenService.TaxonNameSearchCriteria();
                    searchCriteria.NameSearchString = "björn%";
                    searchCriteria.NameSearchMethod = nameSearchMethod;
                    taxonNames = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonNamesBySearchCriteria(searchCriteria);
                    Assert.IsNotNull(taxonNames);
                    Assert.IsTrue(taxonNames.Count > 1);
                }
            }

            // Test iterativ search.
            searchCriteria = new ArtDatabanken.Data.ArtDatabankenService.TaxonNameSearchCriteria();
            searchCriteria.NameSearchString = "björn";
            searchCriteria.NameSearchMethod = WebService.SearchStringComparisonMethod.Iterative;
            taxonNames = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonNamesBySearchCriteria(searchCriteria);
            Assert.IsNotNull(taxonNames);
            Assert.AreEqual(taxonNames.Count, 1);

            searchCriteria.NameSearchString = "björ";
            taxonNames = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonNamesBySearchCriteria(searchCriteria);
            Assert.IsNotNull(taxonNames);
            Assert.IsTrue(taxonNames.Count > 1);

            searchCriteria.NameSearchString = "jörn";
            taxonNames = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonNamesBySearchCriteria(searchCriteria);
            Assert.IsNotNull(taxonNames);
            Assert.IsTrue(taxonNames.Count > 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetTaxonNamesBySearchCriteriaNullSearchCriteriaError()
        {
            ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonNamesBySearchCriteria(null);
        }

        [TestMethod]
        public void GetTaxonNameType()
        {
            Int32 taxonNameTypeId;
            TaxonNameType taxonNameType;

            taxonNameTypeId = SCIENTIFIC_TAXON_NAME_TYPE_ID;
            taxonNameType = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonNameType(taxonNameTypeId);
            Assert.IsNotNull(taxonNameType);
            Assert.AreEqual(taxonNameTypeId, taxonNameType.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetTaxonNameTypeIdError()
        {
            Int32 taxonNameTypeId;

            taxonNameTypeId = Int32.MinValue;
            ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonNameType(taxonNameTypeId);
        }

        [TestMethod]
        public void GetTaxonNameTypes()
        {
            TaxonNameTypeList taxonNameTypes;

            taxonNameTypes = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonNameTypes();
            Assert.IsNotNull(taxonNameTypes);
            Assert.IsTrue(taxonNameTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonNameUseType()
        {
            Int32 taxonNameUseTypeId;
            TaxonNameUseType taxonNameUseType;

            taxonNameUseTypeId = VALID_TAXON_NAME_USE_TYPE_ID;
            taxonNameUseType = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonNameUseType(taxonNameUseTypeId);
            Assert.IsNotNull(taxonNameUseType);
            Assert.AreEqual(taxonNameUseTypeId, taxonNameUseType.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetTaxonNameUseTypeIdError()
        {
            Int32 taxonNameUseTypeId;

            taxonNameUseTypeId = Int32.MinValue;
            ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonNameUseType(taxonNameUseTypeId);
        }

        [TestMethod]
        public void GetTaxonNameUseTypes()
        {
            TaxonNameUseTypeList taxonNameUseTypes;

            taxonNameUseTypes = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonNameUseTypes();
            Assert.IsNotNull(taxonNameUseTypes);
            Assert.IsTrue(taxonNameUseTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonTreesBySearchCriteria()
        {
            List<Int32> taxonIds;
            Data.ArtDatabankenService.TaxonTreeNodeList taxonTrees;
            Data.ArtDatabankenService.TaxonTreeSearchCriteria searchCriteria;

            taxonIds = new List<Int32>();
            taxonIds.Add(HAWK_BIRDS_TAXON_ID);
            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            {
                searchCriteria = new Data.ArtDatabankenService.TaxonTreeSearchCriteria();
                searchCriteria.TaxonInformationType = taxonInformationType;

                if (RunAllTests)
                {
                    searchCriteria.RestrictSearchToTaxonTypeIds = GetTaxonTypeIds();
                    taxonTrees = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonTreesBySearchCriteria(searchCriteria);
                    Assert.IsNotNull(taxonTrees);
                    Assert.IsTrue(taxonTrees.IsNotEmpty());
                    searchCriteria.RestrictSearchToTaxonTypeIds = null;

                    searchCriteria.RestrictSearchToTaxonTypeIds = null;
                    taxonTrees = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonTreesBySearchCriteria(searchCriteria);
                    Assert.IsNotNull(taxonTrees);
                    Assert.IsTrue(taxonTrees.IsNotEmpty());
                }

                searchCriteria.RestrictSearchToTaxonIds = taxonIds;
                taxonTrees = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonTreesBySearchCriteria(searchCriteria);
                Assert.IsNotNull(taxonTrees);
                Assert.AreEqual(taxonTrees.Count, 1);

                searchCriteria.RestrictSearchToTaxonTypeIds = GetTaxonTypeIds();
                taxonTrees = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonTreesBySearchCriteria(searchCriteria);
                Assert.IsNotNull(taxonTrees);
                Assert.IsTrue(taxonTrees.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetTaxonType()
        {
            TaxonType taxonType;

            // Get taxon type by Int32 id.
            {
                Int32 taxonTypeId;

                taxonTypeId = (Int32)(TaxonTypeId.Species);
                taxonType = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonType(taxonTypeId);
                Assert.IsNotNull(taxonType);
                Assert.AreEqual(taxonTypeId, taxonType.Id);
            }

            // Get taxon type by name.
            foreach (TaxonType taxonTypeTest in ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonTypes())
            {
                taxonType = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonType(taxonTypeTest.Name);
                Assert.IsNotNull(taxonType);
                Assert.AreEqual(taxonTypeTest, taxonType);
            }

            // Get taxon type by TaxonTypeId id.
            foreach (TaxonTypeId taxonTypeId in Enum.GetValues(typeof(TaxonTypeId)))
            {
                taxonType = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonType(taxonTypeId);
                Assert.IsNotNull(taxonType);
                Assert.AreEqual((Int32)taxonTypeId, taxonType.Id);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetTaxonTypeIdError()
        {
            Int32 taxonTypeId;

            taxonTypeId = Int32.MinValue;
            ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonType(taxonTypeId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetTaxonTypeNameError()
        {
            ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonType("No taxon type name");
        }

        public static List<Int32> GetTaxonTypeIds()
        {
            List<Int32> taxonTypeIds;

            taxonTypeIds = new List<Int32>();
            taxonTypeIds.Add(SPECIES_TAXON_TYPE_ID);
            taxonTypeIds.Add(GENUS_TAXON_TYPE_ID);
            taxonTypeIds.Add(FAMILY_TAXON_TYPE_ID);
            return taxonTypeIds;
        }

        [TestMethod]
        public void GetTaxonTypes()
        {
            TaxonTypeList taxonTypes;

            taxonTypes = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonTypes();
            Assert.IsNotNull(taxonTypes);
            Assert.IsTrue(taxonTypes.IsNotEmpty());
        }

        public static TaxonNameUseType GetValidTaxonNameUseType()
        {
            return ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonNameUseType(VALID_TAXON_NAME_USE_TYPE_ID);
        }
    }
}

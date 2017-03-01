using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.TaxonService;

namespace ArtDatabanken.WebService.Client.Test.TaxonService
{
    [TestClass]
    public class TaxonDataSourceTest : TestBase
    {
        private TaxonDataSource _taxonDataSource;

        public TaxonDataSourceTest()
        {
            _taxonDataSource = null;
        }

        [TestMethod]
        public void Constructor()
        {
            TaxonDataSourceTest taxonDataSource = new TaxonDataSourceTest();
            Assert.IsNotNull(taxonDataSource);
        }

        [TestMethod]
        public void GetLumpSplitEventTypes()
        {
            LumpSplitEventTypeList lumpSplitEventTypes;

            lumpSplitEventTypes = GetTaxonDataSource(true).GetLumpSplitEventTypes(GetUserContext());
            Assert.IsTrue(lumpSplitEventTypes.IsNotEmpty());
            foreach (ILumpSplitEventType lumpSplitEventType in lumpSplitEventTypes)
            {
                Assert.IsTrue(0 < lumpSplitEventType.Id);
                Assert.IsTrue(lumpSplitEventType.Identifier.IsNotEmpty());
            }
        }

        override protected string GetTestApplicationName()
        {
            return Settings.Default.DyntaxaApplicationIdentifier;
        }

        private TaxonDataSource GetTaxonDataSource(Boolean refresh = false)
        {
            if (_taxonDataSource.IsNull() || refresh)
            {
                _taxonDataSource = new TaxonDataSource();
            }
            return _taxonDataSource;
        }

        [TestMethod]
        public void GetTaxonNameCategoryTypes()
        {
            TaxonNameCategoryTypeList taxonNameCategoryTypes;

            taxonNameCategoryTypes = GetTaxonDataSource(true).GetTaxonNameCategoryTypes(GetUserContext());
            Assert.IsTrue(taxonNameCategoryTypes.IsNotEmpty());
            foreach (ITaxonNameCategoryType taxonNameCategoryType in taxonNameCategoryTypes)
            {
                Assert.IsTrue(-1 < taxonNameCategoryType.Id);
                Assert.IsTrue(taxonNameCategoryType.Identifier.IsNotEmpty());
            }
        }

        [TestMethod]
        public void CreateTaxonCategory()
        {
          // No need for a test here; the test in TaxonManagerTest will cover it all......
        }

        [TestMethod]
        public void GetTaxaByIds()
        {
            // No need for a test here; the test in TaxonManagerTest will cover it all......
        }

        [TestMethod]
        public void GetTaxonById()
        {
            ITaxon taxon;

            taxon = GetTaxonDataSource(true).GetTaxon(GetUserContext(), (Int32)(TaxonId.Bear));
            Assert.IsNotNull(taxon);
            Assert.AreEqual((Int32)(TaxonId.Bear), taxon.Id);
        }

        [TestMethod]
        public void GetTaxonAlertStatuses()
        {
            TaxonAlertStatusList taxonAlertStatuses;

            taxonAlertStatuses = GetTaxonDataSource(true).GetTaxonAlertStatuses(GetUserContext());
            Assert.IsTrue(taxonAlertStatuses.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonChangeStatuses()
        {
            TaxonChangeStatusList taxonChangeStatuses;

            taxonChangeStatuses = GetTaxonDataSource(true).GetTaxonChangeStatuses(GetUserContext());
            Assert.IsTrue(taxonChangeStatuses.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonCategories()
        {
            ITaxon taxon;
            TaxonCategoryList taxonCategories;

            taxonCategories = GetTaxonDataSource(true).GetTaxonCategories(GetUserContext());
            Assert.IsTrue(taxonCategories.IsNotEmpty());

            taxon = CoreData.TaxonManager.GetTaxon(GetUserContext(), (Int32) (TaxonId.Mammals));
            taxonCategories = GetTaxonDataSource().GetTaxonCategories(GetUserContext(), taxon);
            Assert.IsTrue(taxonCategories.IsNotEmpty());
            Assert.IsTrue(10 < taxonCategories.Count);
        }

        [TestMethod]
        public void GetTaxonNameStatus()
        {
            TaxonNameStatusList taxonNameStatus;

            taxonNameStatus = GetTaxonDataSource(true).GetTaxonNameStatuses(GetUserContext());
            Assert.IsTrue(taxonNameStatus.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonNamesByTaxon()
        {
            ITaxon taxon;
            TaxonNameList taxonNames;

            taxon = GetTaxonDataSource(true).GetTaxon(GetUserContext(), (Int32) (TaxonId.Bear));
            taxonNames = GetTaxonDataSource().GetTaxonNames(GetUserContext(), taxon);
            Assert.IsTrue(taxonNames.IsNotEmpty());
            foreach (ITaxonName taxonName in taxonNames)
            {
                Assert.AreEqual(taxon.Id, taxonName.Taxon.Id);
            }
        }

        [TestMethod]
        public void GetTaxonNamesByTaxonIds()
        {
            Int32 index;
            List<TaxonNameList> allTaxonNames;
            TaxonList taxa;
            TaxonTreeNodeList taxonTrees;
            ITaxonTreeSearchCriteria searchCriteria;

            // Test with a few taxa.
            taxa = new TaxonList();
            taxa.Add(GetTaxonDataSource(true).GetTaxon(GetUserContext(), (Int32)(TaxonId.Bear)));
            taxa.Add(GetTaxonDataSource().GetTaxon(GetUserContext(), (Int32)(TaxonId.Mammals)));
            allTaxonNames = GetTaxonDataSource().GetTaxonNames(GetUserContext(), taxa);
            Assert.IsTrue(allTaxonNames.IsNotEmpty());
            Assert.AreEqual(taxa.Count, allTaxonNames.Count);
            for (index = 0; index < taxa.Count; index++)
            {
                Assert.IsTrue(allTaxonNames[index].IsNotEmpty());
                foreach (ITaxonName taxonName in allTaxonNames[index])
                {
                    Assert.AreEqual(taxa[index].Id, taxonName.Taxon.Id);
                }
            }

            // Test with lots of taxa.
            if (Configuration.IsAllTestsRun)
            {
                taxa = new TaxonList(true);
                searchCriteria = new TaxonTreeSearchCriteria();
                searchCriteria.IsValidRequired = true;
                taxonTrees = CoreData.TaxonManager.GetTaxonTrees(GetUserContext(), searchCriteria);
                foreach (ITaxonTreeNode taxonTree in taxonTrees)
                {
                    taxa.Merge(taxonTree.GetTaxa());
                }
                allTaxonNames = GetTaxonDataSource().GetTaxonNames(GetUserContext(), taxa);
                Assert.IsTrue(allTaxonNames.IsNotEmpty());
                Assert.AreEqual(taxa.Count, allTaxonNames.Count);
                for (index = 0; index < taxa.Count; index++)
                {
                    Assert.IsTrue(allTaxonNames[index].IsNotEmpty());
                    foreach (ITaxonName taxonName in allTaxonNames[index])
                    {
                        Assert.AreEqual(taxa[index].Id, taxonName.Taxon.Id);
                    }
                }
            }
        }

        [TestMethod]
        public void GetTaxonConceptDefinition()
        {
            Int32 taxonId;
            String definition;
            ITaxon taxon;

            taxonId = 2002844;
            taxon = GetTaxonDataSource(true).GetTaxon(GetUserContext(), taxonId);
            definition = GetTaxonDataSource(true).GetTaxonConceptDefinition(GetUserContext(), taxon);
            Assert.IsTrue(definition.IsNotEmpty());
            Assert.AreEqual(@"Taxonet har slagits samman med ett annat taxon och har därmed ersatts med Acrididae [2000897]. Catantopidae har upphört som familj och dessa arter ingår numera i familj Acrididae", definition);
        }

        [TestMethod]
        public void GetTaxonNameById()
        {
            //Int32 taxonNameId = 114601; // Omocestus ventralis
            Int32 taxonNameId = 70397; // Omocestus ventralis
            ITaxonName taxonName = GetTaxonDataSource(true).GetTaxonName(GetUserContext(), taxonNameId);
            Assert.IsNotNull(taxonName.Taxon);
            Assert.IsNotNull(taxonName.Taxon.Id);
        }

        [TestMethod]
        public void GetTaxonRelationsBySearchCriteria()
        {
            ITaxonRelationSearchCriteria searchCriteria;
            TaxonRelationList taxonRelations1, taxonRelations2;
            TaxonList taxa;

            // Test all taxon relations.
            searchCriteria = new TaxonRelationSearchCriteria();
            searchCriteria.IsMainRelation = null;
            searchCriteria.IsValid = null;
            searchCriteria.Taxa = null;
            taxonRelations1 = GetTaxonDataSource(true).GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());

            // Test is valid.
            searchCriteria.IsMainRelation = null;
            searchCriteria.IsValid = true;
            searchCriteria.Taxa = null;
            taxonRelations1 = GetTaxonDataSource().GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.IsValid = false;
            taxonRelations2 = GetTaxonDataSource().GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations1.Count > taxonRelations2.Count);

            // Test is main relation.
            searchCriteria.IsValid = null;
            searchCriteria.IsMainRelation = true;
            searchCriteria.Taxa = null;
            taxonRelations1 = GetTaxonDataSource().GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.IsMainRelation = false;
            taxonRelations2 = GetTaxonDataSource().GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations1.Count > taxonRelations2.Count);

            // Test with one taxon.
            taxa = new TaxonList();
            taxa.Add(CoreData.TaxonManager.GetTaxon(GetUserContext(), (Int32)(TaxonId.Mammals)));
            searchCriteria = new TaxonRelationSearchCriteria();
            searchCriteria.IsMainRelation = null;
            searchCriteria.IsValid = null;
            searchCriteria.Taxa = taxa;
            searchCriteria.Scope = TaxonRelationSearchScope.AllParentRelations;
            taxonRelations1 = GetTaxonDataSource().GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestParentRelations;
            taxonRelations2 = GetTaxonDataSource().GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations2.Count < taxonRelations1.Count);

            searchCriteria.Scope = TaxonRelationSearchScope.AllChildRelations;
            taxonRelations1 = GetTaxonDataSource().GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestChildRelations;
            taxonRelations2 = GetTaxonDataSource().GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations2.Count < taxonRelations1.Count);

            // Test with two taxa.
            taxa = new TaxonList();
            taxa.Add(CoreData.TaxonManager.GetTaxon(GetUserContext(), (Int32)(TaxonId.Mammals)));
            taxa.Add(CoreData.TaxonManager.GetTaxon(GetUserContext(), (Int32)(TaxonId.DrumGrasshopper)));
            searchCriteria = new TaxonRelationSearchCriteria();
            searchCriteria.IsMainRelation = null;
            searchCriteria.IsValid = null;
            searchCriteria.Taxa = taxa;
            searchCriteria.Scope = TaxonRelationSearchScope.AllParentRelations;
            taxonRelations1 = GetTaxonDataSource().GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestParentRelations;
            taxonRelations2 = GetTaxonDataSource().GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations2.Count < taxonRelations1.Count);

            searchCriteria.Scope = TaxonRelationSearchScope.AllChildRelations;
            taxonRelations1 = GetTaxonDataSource().GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestChildRelations;
            taxonRelations2 = GetTaxonDataSource().GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations2.Count < taxonRelations1.Count);
        }

        [TestMethod]
        public void GetTaxonRevisionEventTypes()
        {
            TaxonRevisionEventTypeList taxonRevisionEventTypes;

            taxonRevisionEventTypes = GetTaxonDataSource().GetTaxonRevisionEventTypes(GetUserContext());
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

            taxonRevisionStates = GetTaxonDataSource().GetTaxonRevisionStates(GetUserContext());
            Assert.IsTrue(taxonRevisionStates.IsNotEmpty());
            foreach (ITaxonRevisionState taxonRevisionState in taxonRevisionStates)
            {
                Assert.IsTrue(0 < taxonRevisionState.Id);
                Assert.IsTrue(taxonRevisionState.Identifier.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetTaxonTreesBySearchCriteria()
        {
            TaxonTreeNodeList taxonTrees;
            ITaxonTreeSearchCriteria searchCriteria;

            // Get a part of the taxon tree.
            GetTaxonDataSource(true);
            foreach (TaxonTreeSearchScope scope in Enum.GetValues(typeof(TaxonTreeSearchScope)))
            {
                searchCriteria = new TaxonTreeSearchCriteria();
                searchCriteria.IsValidRequired = true;
                searchCriteria.Scope = scope;
                searchCriteria.TaxonIds = new List<Int32>();
                searchCriteria.TaxonIds.Add((Int32)(TaxonId.Mammals));
                taxonTrees = GetTaxonDataSource().GetTaxonTrees(GetUserContext(), searchCriteria);
                Assert.IsTrue(taxonTrees.IsNotEmpty());
                if ((searchCriteria.Scope == TaxonTreeSearchScope.AllChildTaxa) ||
                    (searchCriteria.Scope == TaxonTreeSearchScope.NearestChildTaxa))
                {
                    Assert.AreEqual(1, taxonTrees.Count);
                    Assert.AreEqual((Int32)(TaxonId.Mammals), taxonTrees[0].Taxon.Id);
                }
            }

            // Get the entire taxon tree (valid taxa and relations).
            searchCriteria = new TaxonTreeSearchCriteria();
            searchCriteria.IsValidRequired = true;
            taxonTrees = GetTaxonDataSource().GetTaxonTrees(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonTrees.IsNotEmpty());

            // Get the entire taxon tree (include not valid taxa and relations).
            searchCriteria = new TaxonTreeSearchCriteria();
            searchCriteria.IsValidRequired = false;
            taxonTrees = GetTaxonDataSource().GetTaxonTrees(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonTrees.IsNotEmpty());
        }


        #region Helper functions

        /// <summary>
        /// Creates a taxon (id 2000446) that matches/exist in Taxon DB 2011-09-27.
        /// </summary>
        /// <returns></returns>
        private ITaxon GetReferenceTaxon()
        {

            ITaxon refTaxon = new Taxon();

            string conceptDefinitionPartString = null;
            DateTime createdDate = new DateTime(2004, 01, 20);
            const String GUID = "urn:lsid:dyntaxa.se:Taxon:2000446";
            const int taxonId = 2000446;
            DateTime validFromDate = new DateTime(1763, 02, 08);
            DateTime validToDate = new DateTime(2447, 08, 01);
           
            // refTaxon.ConceptDefinitionFullGeneratedString = conceptDefinitionFullGeneratedString;
            refTaxon.PartOfConceptDefinition = conceptDefinitionPartString;
            // This old taxon is created by the olduserid whick is  one...
            refTaxon.CreatedBy = Settings.Default.TestUserId;
            refTaxon.CreatedDate = createdDate;
            refTaxon.DataContext = new DataContext(GetUserContext());
            (refTaxon as Taxon).Guid = GUID;
            refTaxon.Id = taxonId;
            refTaxon.ValidFromDate = validFromDate;
            refTaxon.ValidToDate = validToDate;

            return refTaxon;
        }
        #endregion

    }
}

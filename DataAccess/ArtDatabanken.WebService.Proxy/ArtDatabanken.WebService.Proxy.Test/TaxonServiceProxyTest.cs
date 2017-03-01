using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Proxy.Test
{
    /// <summary>
    /// This class handles taxon proxy tests
    /// </summary>
    [TestClass]
    public class TaxonServiceProxyTest
    {
        private WebClientInformation _clientInformation;

        public TaxonServiceProxyTest()
        {
            _clientInformation = null;
        }

        [TestMethod]
        public void ClearCache()
        {
            WebServiceProxy.TaxonService.ClearCache(GetClientInformation());
        }

        [TestMethod]
        [Ignore]
        public void CreateTaxon()
        {
            WebTaxon taxon, refTaxon, taxon2;
            refTaxon = GetReferenceTaxon();
           
            taxon = new WebTaxon { CreatedBy = Settings.Default.TestUserId, 
                                   ValidFromDate = new DateTime(1763, 02, 08),
                                   ValidToDate = new DateTime(2447, 08, 01)
            };
            

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.TaxonService))
            {
                // We now create the taxon and complete it with Id and GUID from DB
                refTaxon = WebServiceProxy.TaxonService.CreateTaxon(GetClientInformation(), refTaxon, null);
                // We now create the taxon and complete it with Id and GUID from DB
                taxon = WebServiceProxy.TaxonService.CreateTaxon(GetClientInformation(), taxon, null);
                     Assert.IsNotNull(taxon);
                Assert.IsTrue( taxon.Id > 0);
                Assert.AreEqual(refTaxon.CreatedBy, taxon.CreatedBy);
                Assert.IsNotNull(taxon.CreatedDate);
                Assert.IsTrue(!string.IsNullOrEmpty(taxon.Guid));
                Assert.AreEqual(refTaxon.ValidFromDate, taxon.ValidFromDate);
                Assert.AreEqual(refTaxon.ValidToDate, taxon.ValidToDate);

            }

            // Create another taxon with different data..TODO Include test for ParentTaxa, ChildTaxa and TaxonNames when they exsit...
            taxon2 = new WebTaxon()
            {
                CreatedBy = Settings.Default.TestUserId,
                CreatedDate = DateTime.Now,
                ValidFromDate = new DateTime(1763, 02, 08),
                ValidToDate = new DateTime(2447, 08, 01)
            };


            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.TaxonService))
            {
                // We now create the taxon and complete it with Id and GUID from DB
                WebTaxonName taxonName = GetReferenceTaxonName(taxon.Id);
                WebTaxonName taxonName2 = GetReferenceTaxonName(taxon.Id);
                List<WebTaxonName> taxonNameList = new List<WebTaxonName>();
                taxonNameList.Add(taxonName);
                taxonNameList.Add(taxonName2);
                // taxon2.TaxonNames = taxonNameList;


                taxon2 = WebServiceProxy.TaxonService.CreateTaxon(GetClientInformation(), taxon2, null);
                Assert.IsNotNull(taxon2);
                Assert.IsTrue(taxon2.Id > 0);
                Assert.IsFalse(taxon.Id == taxon2.Id);
                Assert.AreEqual(refTaxon.CreatedBy, taxon2.CreatedBy);
                Assert.IsNotNull(taxon2.CreatedDate);
                Assert.IsTrue(!string.IsNullOrEmpty(taxon2.Guid));
                Assert.AreEqual(refTaxon.ValidFromDate, taxon2.ValidFromDate);
                Assert.AreEqual(refTaxon.ValidToDate, taxon2.ValidToDate);
  

            }
        }

        [TestMethod]
        public void DeleteTrace()
        {
            // Create some trace information.
            WebServiceProxy.TaxonService.StartTrace(GetClientInformation(), Settings.Default.TestUserName);
            //WebServiceProxy.TaxonService.GetRegionTypes(GetClientInformation());
            WebServiceProxy.TaxonService.StopTrace(GetClientInformation());

            // Delete trace information.
            WebServiceProxy.TaxonService.DeleteTrace(GetClientInformation());
        }

        protected WebClientInformation GetClientInformation()
        {
            return _clientInformation;
        }


        [TestMethod]
        public void GetLog()
        {
            List<WebLogRow> logRows;

            logRows = WebServiceProxy.TaxonService.GetLog(GetClientInformation(), LogType.None, null, 100);
            Assert.IsTrue(logRows.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonConceptDefinition()
        {
            Int32 taxonId;
            String definition;
            WebTaxon taxon;

            taxonId = 2002844;
            taxon = WebServiceProxy.TaxonService.GetTaxonById(GetClientInformation(), taxonId);
            definition = WebServiceProxy.TaxonService.GetTaxonConceptDefinition(GetClientInformation(), taxon);
            Assert.IsTrue(definition.IsNotEmpty());
            Assert.AreEqual(@"Taxonet har slagits samman med ett annat taxon och har därmed ersatts med Acrididae [2000897]. Catantopidae har upphört som familj och dessa arter ingår numera i familj Acrididae", definition);
        }

        [TestMethod]
        public void GetLumpSplitEventsByOldReplacedTaxon()
        {
            Int32 oldReplacedTaxonId;
            List<WebLumpSplitEvent> lumpSplitEvents;

            oldReplacedTaxonId = 1637;
            lumpSplitEvents = WebServiceProxy.TaxonService.GetLumpSplitEventsByOldReplacedTaxon(GetClientInformation(), oldReplacedTaxonId);
            Assert.IsTrue(lumpSplitEvents.IsNotEmpty());
        }

        [TestMethod]
        public void GetLumpSplitEventTypes()
        {
            List<WebLumpSplitEventType> lumpSplitEventTypes;

            lumpSplitEventTypes = WebServiceProxy.TaxonService.GetLumpSplitEventTypes(GetClientInformation());
            Assert.IsTrue(lumpSplitEventTypes.IsNotEmpty());
            foreach (WebLumpSplitEventType lumpSplitEventType in lumpSplitEventTypes)
            {
                Assert.IsTrue(0 < lumpSplitEventType.Id);
                Assert.IsTrue(lumpSplitEventType.Identifier.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetRevisionEventByRevisionId()
        {
            List<WebTaxonRevisionEvent> revisionEvents;

            revisionEvents = WebServiceProxy.TaxonService.GetTaxonRevisionEventsByTaxonRevisionId(GetClientInformation(), 1);
            Assert.IsTrue(revisionEvents.IsNotEmpty());
            foreach (WebTaxonRevisionEvent revisionEvent in revisionEvents)
            {
                Assert.AreEqual(1, revisionEvent.RevisionId);
            }
        }

        [TestMethod]
        public void GetTaxonRevisionsBySearchCriteria()
        {
            List<Int32> taxonIds;
            List<WebTaxonRevision> revisions = new List<WebTaxonRevision>();
            WebTaxonRevisionSearchCriteria searchCriteria = new WebTaxonRevisionSearchCriteria();

            // Create list of taxon ids
            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.Mammals));
            searchCriteria.TaxonIds = taxonIds;
            revisions = WebServiceProxy.TaxonService.GetTaxonRevisionsBySearchCriteria(GetClientInformation(), searchCriteria);

            // Check revisions set
            Assert.IsNotNull(revisions);
            Assert.AreEqual(1, revisions.Count);
            Assert.IsNotNull(revisions[0].CreatedDate);
            Assert.IsNotNull(revisions[0].ExpectedEndDate);
            Assert.IsNotNull(revisions[0].Description);
            Assert.IsNotNull(revisions[0].Guid);
            Assert.IsNotNull(revisions[0].StateId);
            Assert.IsNotNull(revisions[0].RootTaxon.Id);
            Assert.IsTrue(revisions[0].CreatedBy >= 0);

            // Check taxon categories set 
            // TODO Update this test when we can create taxon with properties so that categories can be set....
            // Create initial data..
            List<Int32> revisionStateIds = new List<Int32>();
            foreach (int revisionState in Enum.GetValues(typeof(TaxonRevisionStateId)))
            {
                revisionStateIds.Add(revisionState);
            }

            searchCriteria.TaxonIds = null;
            searchCriteria.StateIds = revisionStateIds;
            revisions = WebServiceProxy.TaxonService.GetTaxonRevisionsBySearchCriteria(GetClientInformation(), searchCriteria);

            // Check revisions set
            Assert.IsNotNull(revisions);
            Assert.IsTrue(revisions.Count >= 3);
            Assert.IsNotNull(revisions[0].CreatedDate);
            Assert.IsNotNull(revisions[2].ExpectedEndDate);
            Assert.IsNotNull(revisions[1].Guid);
            Assert.IsNotNull(revisions[1].StateId);
            Assert.IsNotNull(revisions[1].RootTaxon.Id);
            Assert.AreNotEqual(revisions[2].Id, revisions[1].Id);
        }

        [TestMethod]
        public void GetStatus()
        {
            List<WebResourceStatus> status;

            status = WebServiceProxy.TaxonService.GetStatus(GetClientInformation());
            Assert.IsTrue(status.IsNotEmpty());
            status = WebServiceProxy.TaxonService.GetStatus(GetClientInformation());
            Assert.IsTrue(status.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxaByIds()
        {
            // Create list of taxon ids
            List<Int32> taxonIds = new List<Int32>();
            List<WebTaxon> taxa;

            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.AnthophoraQuadrimaculata));
            taxonIds.Add((Int32)(TaxonId.Bear));
            taxonIds.Add((Int32)(TaxonId.Bears));
            taxonIds.Add((Int32)(TaxonId.Butterflies));
            taxonIds.Add((Int32)(TaxonId.Beaver));
            taxonIds.Add((Int32)(TaxonId.Wolverine));
            taxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            taxonIds.Add((Int32)(TaxonId.GreenhouseMoths));
            taxonIds.Add((Int32)(TaxonId.Hedgehog));
            taxonIds.Add((Int32)(TaxonId.Mammals));

            // Get all taxon for the created list
            taxa = WebServiceProxy.TaxonService.GetTaxaByIds(GetClientInformation(), taxonIds);
            Assert.IsNotNull(taxa);
            Assert.AreEqual(10, taxa.Count);
            Assert.IsNotNull(taxa[0].CreatedDate);
            Assert.IsNotNull(taxa[1].Guid);
            Assert.AreNotEqual(taxa[9].Id, taxa[8].Id);
            Assert.AreEqual(taxa[9].CreatedBy, taxa[0].CreatedBy);

            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.Bear));
            taxa = WebServiceProxy.TaxonService.GetTaxaByIds(GetClientInformation(), taxonIds);
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxaBySearchCritera()
        {
            List<Int32> taxonIds;
            List<WebTaxon> taxa;
            String taxonName;
            WebTaxonSearchCriteria searchCriteria;

            // Search by taxon name.
            taxonName = "trumgräshoppa";
            searchCriteria = new WebTaxonSearchCriteria();
            searchCriteria.TaxonNameSearchString = new WebStringSearchCriteria();
            searchCriteria.TaxonNameSearchString.SearchString = taxonName;
            taxa = WebServiceProxy.TaxonService.GetTaxaBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsNotNull(taxa);
            Assert.AreEqual(taxonName, taxa[0].CommonName);

            // Search by taxon ids.
            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.Bear));
            taxonIds.Add((Int32)(TaxonId.Bears));
            taxonIds.Add((Int32)(TaxonId.Beaver));
            taxonIds.Add((Int32)(TaxonId.Butterflies));
            taxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            taxonIds.Add((Int32)(TaxonId.GreenhouseMoths));
            taxonIds.Add((Int32)(TaxonId.Hedgehog));
            taxonIds.Add((Int32)(TaxonId.Life));
            taxonIds.Add((Int32)(TaxonId.Lynx));
            taxonIds.Add((Int32)(TaxonId.Mammals));
            searchCriteria = new WebTaxonSearchCriteria();
            searchCriteria.IsIsValidTaxonSpecified = true;
            searchCriteria.IsValidTaxon = true;
            searchCriteria.TaxonIds = taxonIds;
            taxa = WebServiceProxy.TaxonService.GetTaxaBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());
            Assert.AreEqual(10, taxa.Count);
            Assert.IsNotNull(taxa[0].CreatedDate);
            Assert.IsNotNull(taxa[1].Guid);
            Assert.AreNotEqual(taxa[9].Id, taxa[8].Id);

            // Check taxon categories set 
            // TODO Update this test when we can create taxon with properties so that categories can be set....
            // Create initial data..
            List<Int32> taxonCategoryIdList = new List<int>();
            taxonCategoryIdList.Add(111111);
            searchCriteria.TaxonNameSearchString = null;
            searchCriteria.TaxonIds = null;
            searchCriteria.TaxonCategoryIds = taxonCategoryIdList;
            taxa = WebServiceProxy.TaxonService.GetTaxaBySearchCriteria(GetClientInformation(), searchCriteria);

            Assert.IsNotNull(taxa);
            Assert.AreEqual(0, taxa.Count);
        }

        [TestMethod]
        public void GetTaxonAlertStatuses()
        {
            List<WebTaxonAlertStatus> taxonAlertStatuses;

            taxonAlertStatuses = WebServiceProxy.TaxonService.GetTaxonAlertStatuses(GetClientInformation());
            Assert.IsTrue(taxonAlertStatuses.IsNotEmpty());
            foreach (WebTaxonAlertStatus taxonAlertStatus in taxonAlertStatuses)
            {
                Assert.IsTrue(-1 < taxonAlertStatus.Id);
                Assert.IsTrue(taxonAlertStatus.Identifier.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetTaxonById()
        {
            WebTaxon taxon;

            taxon = WebServiceProxy.TaxonService.GetTaxonById(GetClientInformation(), (Int32)(TaxonId.Bear));
            Assert.IsNotNull(taxon);
            Assert.AreEqual((Int32)(TaxonId.Bear), taxon.Id);
        }

        [TestMethod]
        public void GetTaxonByGuid()
        {
            WebTaxon taxon;
            string guid = "urn:lsid:dyntaxa.se:Taxon:" + (Int32)(TaxonId.Bear);
            taxon = WebServiceProxy.TaxonService.GetTaxonByGuid(GetClientInformation(), guid);
            Assert.IsNotNull(taxon);
            Assert.AreEqual((Int32)(TaxonId.Bear), taxon.Id);
        }

        [TestMethod]
        public void GetTaxonCategories()
        {
            List<WebTaxonCategory> taxonCategories;

            taxonCategories = WebServiceProxy.TaxonService.GetTaxonCategories(GetClientInformation());
            Assert.IsTrue(taxonCategories.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonCategoriesByTaxonId()
        {
            List<WebTaxonCategory> taxonCategories;

            taxonCategories = WebServiceProxy.TaxonService.GetTaxonCategoriesByTaxonId(GetClientInformation(), (Int32)(TaxonId.Mammals));
            Assert.IsTrue(taxonCategories.IsNotEmpty());
            Assert.IsTrue(10 < taxonCategories.Count);
        }

        [TestMethod]
        public void GetTaxonCategoriesForTaxonInTree()
        {
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.TaxonService))
            {
                /* TODO This test is to be used when relation between taxon and taxoncategory is implemented....
                // Create  taxa in a tree first
                List<WebTaxon> taxa = new List<WebTaxon>();
                WebTaxon lastTaxon = GetReferenceTaxon("Root taxon");
                //Here is my root taxon
                lastTaxon = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.CreateTaxon(GetContext(), lastTaxon);
                 for (int i = 0; i < 10; i++)
                {
                    // Create a taxon and set its parents
                    WebTaxon taxon = GetReferenceTaxon("Child taxon 1");
                     //Create the relation and set it to child taxon
                    WebTaxonRelation taxonRelation = new WebTaxonRelation() { Id = 77 + i, RelatedTaxon = lastTaxon};
                    List<WebTaxonRelation> parentTaxonRelation = new List<WebTaxonRelation>();
                    parentTaxonRelation.Add(taxonRelation);
                    taxon.ParentTaxa = parentTaxonRelation;
                    // Create category for taxon
                    WebTaxonCategory refTaxonCategory = GetReferenceTaxonCategory(i);
                    refTaxonCategory = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.CreateTaxonCategory(GetContext(), refTaxonCategory);
                    taxon.TaxonCategories = refTaxonCategory;
                }*/
                // For the time beeing we are testing on existing taxon...

                // Arrange
                int taxonId = 100049; // Detta är en spillkråka...

                // Act
                List<WebTaxonCategory> categoryList = WebServiceProxy.TaxonService.GetTaxonCategoriesByTaxonId(GetClientInformation(), taxonId);

                // Assert
                Assert.IsNotNull(categoryList);
                Assert.AreEqual(categoryList[0].Name, "Rike");
                //Root category
                Assert.AreEqual(categoryList[0].ParentId, 0);
                Assert.AreEqual(categoryList[1].Name, "Stam");
                Assert.AreEqual(categoryList[2].Name, "Understam");
                Assert.AreEqual(categoryList[3].Name, "Klass");
                Assert.AreEqual(categoryList[4].Name, "Ordning");
                Assert.AreEqual(categoryList[5].Name, "Familj");
                //spillkråkans släkte
                Assert.AreEqual(categoryList[6].ParentId, 11);
                Assert.AreEqual(categoryList[6].Name, "Släkte");
                Assert.AreEqual(categoryList[6].Id, 14);
                Assert.AreEqual(categoryList[6].SortOrder, 33);
                // Detta är en spillkråka...
                Assert.AreEqual(categoryList[7].ParentId, 14);
                Assert.AreEqual(categoryList[7].Name, "Art");
                Assert.AreEqual(categoryList[7].Id, 17);
                Assert.AreEqual(categoryList[7].SortOrder, 42);
                // det finns en underart till denna ... 
                Assert.AreEqual(categoryList[8].Name, "Underart");
            }
        }

        [TestMethod]
        public void GetTaxonChangeStatuses()
        {
            List<WebTaxonChangeStatus> taxonChangeStatuses;

            taxonChangeStatuses = WebServiceProxy.TaxonService.GetTaxonChangeStatuses(GetClientInformation());
            Assert.IsTrue(taxonChangeStatuses.IsNotEmpty());
            foreach (WebTaxonChangeStatus taxonChangeStatus in taxonChangeStatuses)
            {
                Assert.IsTrue(-5 < taxonChangeStatus.Id);
                Assert.IsTrue(taxonChangeStatus.Identifier.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetTaxonNameCategories()
        {
            List<WebTaxonNameCategory> taxonNameCategories;

            taxonNameCategories = WebServiceProxy.TaxonService.GetTaxonNameCategories(GetClientInformation());
            Assert.IsTrue(taxonNameCategories.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonNameCategoryTypes()
        {
            List<WebTaxonNameCategoryType> taxonNameCategoryTypes;

            taxonNameCategoryTypes = WebServiceProxy.TaxonService.GetTaxonNameCategoryTypes(GetClientInformation());
            Assert.IsTrue(taxonNameCategoryTypes.IsNotEmpty());
            foreach (WebTaxonNameCategoryType taxonNameCategoryType in taxonNameCategoryTypes)
            {
                Assert.IsTrue(-2 < taxonNameCategoryType.Id);
                Assert.IsTrue(taxonNameCategoryType.Identifier.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetTaxonNamesBySearchCriteria()
        {
            List<WebTaxonName> taxonNames;
            WebTaxonNameSearchCriteria searchCriteria;
            
            searchCriteria = new WebTaxonNameSearchCriteria();
            searchCriteria.NameSearchString = new WebStringSearchCriteria();
            searchCriteria.NameSearchString.SearchString = "Trumgräshoppa";
            taxonNames = WebServiceProxy.TaxonService.GetTaxonNamesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxonNames.IsNotEmpty());

            searchCriteria = new WebTaxonNameSearchCriteria();
            searchCriteria.NameSearchString = new WebStringSearchCriteria();
            searchCriteria.NameSearchString.SearchString = "Björn";
            taxonNames = WebServiceProxy.TaxonService.GetTaxonNamesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxonNames.IsNotEmpty());

            searchCriteria = new WebTaxonNameSearchCriteria();
            searchCriteria.IsIsOriginalNameSpecified = true;
            searchCriteria.IsOriginalName = true;
            taxonNames = WebServiceProxy.TaxonService.GetTaxonNamesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxonNames.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonNamesByTaxonId()
        {
            Int32 taxonId = Settings.Default.TestTaxonId;
        
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.TaxonService))
            {

                var taxonNames = WebServiceProxy.TaxonService.GetTaxonNamesByTaxonId(GetClientInformation(), taxonId);
                Assert.IsNotNull(taxonNames);
                Assert.IsTrue(taxonNames.Count > 0);
            }
        }

        [TestMethod]
        public void GetTaxonNamesByTaxonIds()
        {
            Int32 index;
            List<Int32> taxonIds;
            List<List<WebTaxonName>> allTaxonNames;

            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.Bear));
            taxonIds.Add((Int32)(TaxonId.Mammals));
            allTaxonNames = WebServiceProxy.TaxonService.GetTaxonNamesByTaxonIds(GetClientInformation(), taxonIds);
            Assert.IsTrue(allTaxonNames.IsNotEmpty());
            Assert.AreEqual(taxonIds.Count, allTaxonNames.Count);
            for (index = 0; index < taxonIds.Count; index++)
            {
                Assert.IsTrue(allTaxonNames[index].IsNotEmpty());
                foreach (WebTaxonName taxonName in allTaxonNames[index])
                {
                    Assert.AreEqual(taxonIds[index], taxonName.Taxon.Id);
                }
            }
        }

        [TestMethod]
        public void GetTaxonNameStatus()
        {
            List<WebTaxonNameStatus> taxonNameStatus;

            taxonNameStatus = WebServiceProxy.TaxonService.GetTaxonNameStatuses(GetClientInformation());
            Assert.IsTrue(taxonNameStatus.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonRelationsBySearchCriteria()
        {
            List<WebTaxonRelation> taxonRelations1, taxonRelations2;
            List<Int32> taxonIds;
            WebTaxonRelationSearchCriteria searchCriteria;

            // Test all taxon relations.
            searchCriteria = new WebTaxonRelationSearchCriteria();
            searchCriteria.IsIsMainRelationSpecified = false;
            searchCriteria.IsIsValidSpecified = false;
            searchCriteria.TaxonIds = null;
            taxonRelations1 = WebServiceProxy.TaxonService.GetTaxonRelationsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());

            // Test is valid.
            searchCriteria.IsIsMainRelationSpecified = false;
            searchCriteria.IsIsValidSpecified = true;
            searchCriteria.IsValid = true;
            searchCriteria.TaxonIds = null;
            taxonRelations1 = WebServiceProxy.TaxonService.GetTaxonRelationsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.IsValid = false;
            taxonRelations2 = WebServiceProxy.TaxonService.GetTaxonRelationsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations1.Count > taxonRelations2.Count);

            // Test is main relation.
            searchCriteria.IsIsMainRelationSpecified = true;
            searchCriteria.IsIsValidSpecified = false;
            searchCriteria.IsMainRelation = true;
            searchCriteria.TaxonIds = null;
            taxonRelations1 = WebServiceProxy.TaxonService.GetTaxonRelationsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.IsMainRelation = false;
            taxonRelations2 = WebServiceProxy.TaxonService.GetTaxonRelationsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations1.Count > taxonRelations2.Count);

            // Test with one taxon.
            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.Mammals));
            searchCriteria = new WebTaxonRelationSearchCriteria();
            searchCriteria.IsIsMainRelationSpecified = false;
            searchCriteria.IsIsValidSpecified = false;
            searchCriteria.TaxonIds = taxonIds;
            searchCriteria.Scope = TaxonRelationSearchScope.AllParentRelations;
            taxonRelations1 = WebServiceProxy.TaxonService.GetTaxonRelationsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestParentRelations;
            taxonRelations2 = WebServiceProxy.TaxonService.GetTaxonRelationsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations2.Count < taxonRelations1.Count);

            searchCriteria.Scope = TaxonRelationSearchScope.AllChildRelations;
            taxonRelations1 = WebServiceProxy.TaxonService.GetTaxonRelationsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestChildRelations;
            taxonRelations2 = WebServiceProxy.TaxonService.GetTaxonRelationsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations2.Count < taxonRelations1.Count);

            // Test with two taxa.
            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.Mammals));
            taxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria = new WebTaxonRelationSearchCriteria();
            searchCriteria.IsIsMainRelationSpecified = false;
            searchCriteria.IsIsValidSpecified = false;
            searchCriteria.TaxonIds = taxonIds;
            searchCriteria.Scope = TaxonRelationSearchScope.AllParentRelations;
            taxonRelations1 = WebServiceProxy.TaxonService.GetTaxonRelationsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestParentRelations;
            taxonRelations2 = WebServiceProxy.TaxonService.GetTaxonRelationsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations2.Count < taxonRelations1.Count);

            searchCriteria.Scope = TaxonRelationSearchScope.AllChildRelations;
            taxonRelations1 = WebServiceProxy.TaxonService.GetTaxonRelationsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestChildRelations;
            taxonRelations2 = WebServiceProxy.TaxonService.GetTaxonRelationsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations2.Count < taxonRelations1.Count);
        }

        [TestMethod]
        public void GetTaxonRevisionEventTypes()
        {
            List<WebTaxonRevisionEventType> taxonRevisionEventTypes;

            taxonRevisionEventTypes = WebServiceProxy.TaxonService.GetTaxonRevisionEventTypes(GetClientInformation());
            Assert.IsTrue(taxonRevisionEventTypes.IsNotEmpty());
            foreach (WebTaxonRevisionEventType taxonRevisionEventType in taxonRevisionEventTypes)
            {
                Assert.IsTrue(0 < taxonRevisionEventType.Id);
                Assert.IsTrue(taxonRevisionEventType.Identifier.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetTaxonRevisionStates()
        {
            List<WebTaxonRevisionState> taxonRevisionStates;

            taxonRevisionStates = WebServiceProxy.TaxonService.GetTaxonRevisionStates(GetClientInformation());
            Assert.IsTrue(taxonRevisionStates.IsNotEmpty());
            foreach (WebTaxonRevisionState taxonRevisionState in taxonRevisionStates)
            {
                Assert.IsTrue(0 < taxonRevisionState.Id);
                Assert.IsTrue(taxonRevisionState.Identifier.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetTaxonTreesBySearchCriteria()
        {
            List<WebTaxonTreeNode> taxonTrees;
            WebTaxonTreeSearchCriteria searchCriteria;

            // Get a part of the taxon tree.
            foreach (TaxonTreeSearchScope scope in Enum.GetValues(typeof(TaxonTreeSearchScope)))
            {
                searchCriteria = new WebTaxonTreeSearchCriteria();
                searchCriteria.IsMainRelationRequired = false;
                searchCriteria.IsValidRequired = true;
                searchCriteria.Scope = scope;
                searchCriteria.TaxonIds = new List<Int32>();
                searchCriteria.TaxonIds.Add((Int32)(TaxonId.Mammals));
                taxonTrees = WebServiceProxy.TaxonService.GetTaxonTreesBySearchCriteria(GetClientInformation(), searchCriteria);
                Assert.IsTrue(taxonTrees.IsNotEmpty());
                if ((searchCriteria.Scope == TaxonTreeSearchScope.AllChildTaxa) ||
                    (searchCriteria.Scope == TaxonTreeSearchScope.NearestChildTaxa))
                {
                    Assert.AreEqual(1, taxonTrees.Count);
                    Assert.AreEqual((Int32)(TaxonId.Mammals), taxonTrees[0].Taxon.Id);
                }
            }

            // Get the entire taxon tree (valid taxa and relations).
            searchCriteria = new WebTaxonTreeSearchCriteria();
            searchCriteria.IsMainRelationRequired = false;
            searchCriteria.IsValidRequired = true;
            taxonTrees = WebServiceProxy.TaxonService.GetTaxonTreesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxonTrees.IsNotEmpty());

            // Get the entire taxon tree (include not valid taxa and relations).
            searchCriteria = new WebTaxonTreeSearchCriteria();
            searchCriteria.IsMainRelationRequired = false;
            searchCriteria.IsValidRequired = false;
            taxonTrees = WebServiceProxy.TaxonService.GetTaxonTreesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxonTrees.IsNotEmpty());
        }

        [TestMethod]
        public void Ping()
        {
            Boolean ping;

            ping = WebServiceProxy.TaxonService.Ping();
            Assert.IsTrue(ping);
        }

        [TestMethod]
        public void Login()
        {
            WebLoginResponse loginResponse;

            loginResponse = WebServiceProxy.TaxonService.Login(Settings.Default.TestUserName,
                                                                      Settings.Default.TestPassword,
                                                                      Settings.Default.DyntaxaApplicationIdentifier,
                                                                      false);
            Assert.IsNotNull(loginResponse);
        }

        [TestMethod]
        public void Logout()
        {
            WebClientInformation clientInformation;
            WebLoginResponse loginResponse;

            loginResponse = WebServiceProxy.TaxonService.Login(Settings.Default.TestUserName,
                                                                      Settings.Default.TestPassword,
                                                                      Settings.Default.DyntaxaApplicationIdentifier,
                                                                      false);
            Assert.IsNotNull(loginResponse);
            clientInformation = new WebClientInformation();
            clientInformation.Token = loginResponse.Token;
            WebServiceProxy.TaxonService.Logout(clientInformation);
        }

        [TestMethod]
        public void StartTrace()
        {
            WebServiceProxy.TaxonService.StartTrace(GetClientInformation(), "kalle kula");
            WebServiceProxy.TaxonService.StopTrace(GetClientInformation());
        }

        [TestMethod]
        public void StopTrace()
        {
            WebServiceProxy.TaxonService.StartTrace(GetClientInformation(), "kalle kula");
            WebServiceProxy.TaxonService.StopTrace(GetClientInformation());
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                WebServiceProxy.TaxonService.Logout(_clientInformation);
                _clientInformation = null;
            }
            catch
            {
                // Test is done.
                // We are not interested in problems that
                // occures due to test of error handling.
            }
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void TestInitialize()
        {
            WebLoginResponse loginResponse;

            // Production test.
            // Configuration.InstallationType = InstallationType.Production;
            // WebServiceProxy.TaxonService.WebServiceAddress = @"Taxon.ArtDatabankenSOA.se/TaxonService.svc";

            // Local test
            // WebServiceProxy.TaxonService.InternetProtocol = InternetProtocol.Http;
            // WebServiceProxy.TaxonService.WebServiceAddress = @"localhost:4983/TaxonService.svc";
            // WebServiceProxy.TaxonService.WebServiceComputer = WebServiceComputer.Local;
            // WebServiceProxy.TaxonService.WebServiceProtocol = WebServiceProtocol.SOAP11;

            // Normal test server tests.
            Configuration.InstallationType = InstallationType.ServerTest;

            loginResponse = WebServiceProxy.TaxonService.Login(Settings.Default.TestUserName,
                                                               Settings.Default.TestPassword,
                                                               Settings.Default.DyntaxaApplicationIdentifier,
                                                               false);
            if (loginResponse.IsNotNull())
            {
                _clientInformation = new WebClientInformation();
                _clientInformation.Locale = loginResponse.Locale;
                _clientInformation.Role = loginResponse.Roles[0];
                _clientInformation.Token = loginResponse.Token;
            }
        }

 #region Helper methods

        /// <summary>
        /// Gets a Revision for test
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private WebTaxonRevision GetReferenceRevision(int i)
        {
            // Create a revisionstate that we later use...
            WebTaxonRevision revision = new WebTaxonRevision();
            revision.Description = "My revision description " + i;
            revision.ExpectedEndDate = new DateTime(2022, 1, 30);
            revision.ExpectedStartDate = DateTime.Now;
            revision.CreatedDate = DateTime.Now;
            revision.StateId = 2222 + i;

            return revision;
        }

        /// <summary>
        /// gets a RevisionEvent for test 
        /// </summary>
        /// <returns></returns>
        private WebTaxonRevisionEvent GetReferenceRevisionEvent()
        {
            WebTaxonRevisionEvent revEvent = new WebTaxonRevisionEvent();
            revEvent.CreatedBy = Settings.Default.TestUserId;
            revEvent.CreatedDate = DateTime.Now;
            revEvent.TypeId = 3300;
            revEvent.RevisionId = 44;
            return revEvent;
        }

        /// <summary>
        /// Creates a taxon.
        /// </summary>
        /// <returns></returns>
        private WebTaxon GetReferenceTaxon()
        {
            WebTaxon refTaxon = new WebTaxon();

            DateTime createdDate = new DateTime(2004, 01, 20);
            DateTime validFromDate = new DateTime(1763, 02, 08);
            DateTime validToDate = new DateTime(2447, 08, 01);

            refTaxon.CreatedDate = createdDate;
            refTaxon.ValidFromDate = validFromDate;
            refTaxon.ValidToDate = validToDate;
            // Not used when creating a taxon id will be overwritten from DB
            refTaxon.Id = 1;
            return refTaxon;
        }

        /// <summary>
        /// Creates a taxon out of predefined data when a text as identifier is set to
        /// differentiate taxon from each other. To be used in test cases when
        /// a list of taxon is needed.
        /// </summary>
        /// <returns>WebTaxon </returns>
        private WebTaxon GetReferenceTaxon(string text)
        {
            WebTaxon refTaxon = new WebTaxon();

            // First we create a taxon 
            string conceptDefinitionPart = "conceptDefinitionPart" + " " + text;
            Int32 createdBy = Settings.Default.TestUserId;
            DateTime validFromDate = new DateTime(DateTime.Now.Ticks);
            DateTime validToDate = new DateTime(2022, 1, 30);

            refTaxon.PartOfConceptDefinition = conceptDefinitionPart;
            refTaxon.CreatedBy = createdBy;
            refTaxon.ValidFromDate = validFromDate;
            refTaxon.ValidToDate = validToDate;
            // Not used when creating a taxon id will be overwritten from DB
            refTaxon.Id = 1;

            return refTaxon;
        }

        /// <summary>
        /// Creates a taxon name out of predefined data
        /// </summary>
        /// <returns>WebTaxonName </returns>
        private WebTaxonName GetReferenceTaxonName(int taxonId)
        {
            WebTaxonName refTaxonName = new WebTaxonName();

            refTaxonName.Taxon = new WebTaxon { Id = taxonId };
            refTaxonName.Description = "test description";
            refTaxonName.Name = "TestTaxonName";
            refTaxonName.CategoryId = 1;
            refTaxonName.StatusId = 0;
            refTaxonName.IsOkForSpeciesObservation = true;
            refTaxonName.IsPublished = false;
            refTaxonName.IsRecommended = true;
            refTaxonName.IsUnique = false;
            refTaxonName.CreatedBy = Settings.Default.TestUserId;
            // refTaxonName.IsRevisionEventIdSpecified = true;
            refTaxonName.ChangedInTaxonRevisionEventId = 1;
            return refTaxonName;
        }

        /// <summary>
        /// Create a taxon category for test.
        /// </summary>
        /// <returns></returns>
        private WebTaxonCategory GetReferenceTaxonCategory(int i = 0)
        {
            WebTaxonCategory refTaxonCategory = new WebTaxonCategory();
            // First we create a taxon category that we later use...
            string categoryName = "Svenskt" + i;
            Int32 parentCategory = 2 + i;
            Int32 sortOrder = 20 + i;
            bool mainCategory = false;
            bool taxonomic = true;
            Int32 categoryId = 1234 + i;

            refTaxonCategory.Name = categoryName;
            refTaxonCategory.Id = categoryId;
            refTaxonCategory.IsMainCategory = mainCategory;
            refTaxonCategory.ParentId = parentCategory;
            refTaxonCategory.SortOrder = sortOrder;
            refTaxonCategory.IsTaxonomic = taxonomic;

            return refTaxonCategory;
        }

        /// <summary>
        /// Gets a reference taxon name category.
        /// </summary>
        /// <returns></returns>
        private WebTaxonNameCategory GetReferenceTaxonNameCategory()
        {
            WebTaxonNameCategory refTaxonNameCategory = new WebTaxonNameCategory();
            // First we create a taxon category that we later use...
            string categoryName = "Svenskt";
            Int32 sortOrder = 20;
            string shortName = @"Mitt söta korta namn";
            
            refTaxonNameCategory.Name = categoryName;
            refTaxonNameCategory.ShortName = shortName;
            refTaxonNameCategory.SortOrder = sortOrder;
            return refTaxonNameCategory;
        }

#endregion
    }
}

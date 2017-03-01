using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Database;
using ArtDatabanken.WebService.TaxonService.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.TaxonService.Test.Database
{
    [TestClass]
    public class TaxonServerTest
    {
        private TaxonServer _database;

        public TaxonServerTest()
        {
            _database = null;
        }

        [TestMethod]
        public void GetAddress()
        {
            String address;

            address = TaxonServer.GetAddress();
            Assert.IsTrue(address.IsNotEmpty());
        }

        private TaxonServer GetDatabase(Boolean refresh = false)
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
            // GuNy 2011-11-22
            _database.CommandTimeout = 60;
            return _database;
        }

        /// <summary>
        /// Create a taxon category and 
        /// getting the created one.
        /// </summary>
        [TestMethod]
        public void CreateTaxonCategory()
        {
            string categoryName = "CategoryName som är Svenskt";
            Int32 parentCategory = 2;
            Int32 sortOrder = 20;
            bool mainCategory = false;
            bool taxonomic = true;
            int categoryId = 131;

            int taxonCategoryId = GetDatabase(true).CreateTaxonCategory(categoryId, categoryName, mainCategory, parentCategory, sortOrder, taxonomic, Settings.Default.TestLocaleId);
            Assert.IsTrue(taxonCategoryId > 0);

            categoryName = "CategoryName som är Finskt";
            parentCategory = 1;
            sortOrder = 10;
            mainCategory = true;
            taxonomic = false;
            categoryId = 88;

            int taxonCategoryId2 = GetDatabase(true).CreateTaxonCategory(categoryId, categoryName, mainCategory, parentCategory, sortOrder, taxonomic, Settings.Default.TestLocaleId);
            Assert.AreNotEqual(taxonCategoryId2, taxonCategoryId);

        }

        /// <summary>
        /// Test create a Dyntaxa revision species fact.
        /// </summary>
        [TestMethod]
        public void CreateDyntaxaRevisionSpeciesFact()
        {
            Int32 factorId = 1;
            Int32 revisionId = 1;
            Int32 taxonId = 1;
            Int32 statusId = 1;
            Int32 qualityId = 1;
            String description = "Test description";
            Int32 referenceId = 1;
            Int32 createdBy = 1;
            DateTime createdDate = DateTime.Now;            
            Int32 revisionEventId = 1;
            Boolean speciesFactExists = true;
            Int32? originalStatusId = 2;
            Int32? originalQualityId = 2;
            String originalDescription = "original test";
            Int32? originalReferenceId = 2;

            
            int Id = GetDatabase(true).CreateDyntaxaRevisionSpeciesFact(factorId, taxonId, revisionId, statusId, qualityId, description, referenceId,
                createdDate, createdBy, revisionEventId, speciesFactExists, originalStatusId, originalQualityId, originalReferenceId, originalDescription);
                        
            Assert.IsTrue(Id > 0);
        }

        [TestMethod]
        public void TestCreateDyntaxaRevisionReferenceRelation()
        {            
            Int32 revisionId = 1;
            Int32? referenceRelationId = null;
            String relatedObjectGUID = "urn:lsid:dyntaxa.se:Taxon:6010174";
            Int32 referenceId = 1;
            Int32? oldReferenceType = null;
            Int32 referenceType = 2;
            String action = "Add";
            Int32 createdBy = 1;
            DateTime createdDate = DateTime.Now;
            Int32? revisionEventId = 1;

            int Id = GetDatabase(true).CreateDyntaxaRevisionReferenceRelation(
                revisionId,
                action,
                relatedObjectGUID,
                referenceId,
                referenceType,
                oldReferenceType,
                referenceRelationId,                
                createdDate,
                createdBy,
                revisionEventId);

            Assert.IsTrue(Id > 0);
        }

        /// <summary>
        /// Create a taxon name
        /// </summary>
        [TestMethod]
        public void CreateTaxonName()
        {
            Int32 taxonId = 1;
            Int32? taxonNameId = 1;
            String name = "TestTaxonName";
            String author = "TestAuthor Żlatan";
            Int32 nameCategory = 0;
            Int32 nameUsage = 1;
            Int32 nameUsageNew = 1;
            String personName = null;
            Boolean isRecommended = true;
            Int32 createdBy = 1;
            DateTime createdDate = DateTime.Now;
            DateTime validFromDate = new DateTime(DateTime.Now.Ticks);
            DateTime validToDate = new DateTime(2022,1,30);
            String description = "Test description";
            Int32 revisionEventId = 1;
            Boolean isPublished = false;
            Boolean isOriginal = false;
            Boolean isOkForObsSystems = true;
            int Id = GetDatabase(true).CreateTaxonName(taxonId, taxonNameId, name, author, nameCategory, nameUsage, nameUsageNew,
                                                                personName, isRecommended, createdDate, createdBy,
                                                                validFromDate, validToDate, description, revisionEventId,
                                                                isPublished, isOkForObsSystems, isOriginal, Settings.Default.TestLocaleId);
            Assert.IsTrue(Id > 0);

        }

        /// <summary>
        /// Create a taxon property
        /// </summary>
        [TestMethod]
        public void CreateTaxonProperties()
        {
            
            int taxonId = 1;
            int taxonCategory = 17;
            string conceptDefinitionPart = "Test Żlatan";
            string conceptDefinitionFullGenerated = "Test Żlatan";
            int alertStatus = 0;
            DateTime validFromDate = new DateTime(DateTime.Now.Ticks);
            DateTime validToDate = new DateTime(2022,1,30);
            bool isValid = true;
            string personName = "Person Żlatan";
            int modifiedById= 1;
            int revisionEventId= 1;
            int changedInRevisionEventId = 1;
            bool isPublished = false;
            var isMicrospecies = true;
            int localeId = 175;
            int id = GetDatabase(true).CreateTaxonProperties(taxonId, taxonCategory, conceptDefinitionPart,
                                                             conceptDefinitionFullGenerated, alertStatus, validFromDate,
                                                             validToDate, isValid, personName, modifiedById,
                                                             revisionEventId, changedInRevisionEventId, isPublished, isMicrospecies, localeId);
            Assert.IsTrue(id > 0);
        }

        /// <summary>
        /// Create a taxon category and 
        /// getting the created one.
        /// </summary>
        [TestMethod]
        public void CreateTaxonNameCategory()
        {
            string categoryName = "CategoryName som är Svenskt";
            Int32 sortOrder = 20;
            string shortName = "hej";
            Int32 type = 1;

            int taxonNameCategoryId = GetDatabase(true).CreateTaxonNameCategory(categoryName, shortName, sortOrder, Settings.Default.TestLocaleId,type);
            Assert.IsTrue(taxonNameCategoryId > 0);

            categoryName = "CategoryName som är Finskt";
            sortOrder = 10;
            shortName = "hej igen";

            int taxonCategoryId2 = GetDatabase(true).CreateTaxonNameCategory(categoryName, shortName, sortOrder, Settings.Default.TestLocaleId, type);
            Assert.AreNotEqual(taxonCategoryId2, taxonNameCategoryId);

        }


        /// <summary>
        /// Test getting revision event by valid and invalid  revision id.
        /// </summary>
        [TestMethod]
        public void GetRevisionEventByRevisionId()
        {
            // First we create a temporary revision and then we create the revEvent and getting it by revision...
            Int32 revisionId = 1;
            GetDatabase().CreateRevisionEvent(revisionId, (int)TaxonRevisionStateId.Ongoing, Settings.Default.TestUserId);

            using (DataReader dataReader = GetDatabase().GetRevisionEventsByRevisionId(revisionId, Settings.Default.TestLocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
            revisionId = -1;
            using (DataReader dataReader = GetDatabase(true).GetRevisionEventsByRevisionId(revisionId, Settings.Default.TestLocaleId))
            {
                Assert.IsFalse(dataReader.Read());
            }
        }

        /// <summary>
        /// Test create complete revision event.
        /// </summary>
        [TestMethod]
        public void CreateCompleteRevisionEvent()
        {
            Int32 revisionId = 1;

            GetDatabase().CreateCompleteRevisionEvent(revisionId, 15, Settings.Default.TestUserId, DateTime.Now, "Straminergon [1004721]", "my new value", "my old value");

            using (DataReader dataReader = GetDatabase().GetRevisionEventsByRevisionId(revisionId, Settings.Default.TestLocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
            revisionId = -1;
            using (DataReader dataReader = GetDatabase(true).GetRevisionEventsByRevisionId(revisionId, Settings.Default.TestLocaleId))
            {
                Assert.IsFalse(dataReader.Read());
            }
        }

        
        /// <summary>
        /// Test getting taxa by ids.
        /// getting the created one.
        /// </summary>
        [TestMethod]
        public void GetTaxaByIds()
        {
            List<Int32> taxonIds;

            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.Bear));
            taxonIds.Add((Int32)(TaxonId.Beaver));
            taxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            taxonIds.Add((Int32)(TaxonId.Hedgehog));
            taxonIds.Add((Int32)(TaxonId.Mammals));
            using (DataReader dataReader = GetDatabase().GetTaxaByIds(taxonIds, null, Settings.Default.TestLocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetTaxa()
        {
            using (DataReader dataReader = GetDatabase().GetTaxa((Int32)(LocaleId.sv_SE)))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        /// <summary>
        /// Test getting taxon by valid and invalid id.
        /// </summary>
        [TestMethod]
        public void GetTaxonById()
        {
            Int32 taxonId;

            taxonId = (Int32)(TaxonId.Bear);
            using (DataReader dataReader = GetDatabase().GetTaxonById(taxonId, null, Settings.Default.TestLocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }

            taxonId = -1;
            using (DataReader dataReader = GetDatabase(true).GetTaxonById(taxonId, null, Settings.Default.TestLocaleId))
            {
                Assert.IsFalse(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetRevisionsBySearchCriteria()
        {
            List<Int32> revisionStateIds, taxonIds;

            // Check revision state ids set.
            revisionStateIds = new List<Int32>();
            foreach (Int32 value in Enum.GetValues(typeof(TaxonRevisionStateId)))
            {
                revisionStateIds.Add(value);
            }
            using (DataReader dataReader = GetDatabase().GetRevisionsBySearchCriteria(null, revisionStateIds, (Int32)(LocaleId.sv_SE)))
            {
                Assert.IsTrue(dataReader.Read());
            }

            // Check only taxonIds set
            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.Mammals));
            using (DataReader dataReader = GetDatabase().GetRevisionsBySearchCriteria(taxonIds, null, (Int32)(LocaleId.sv_SE)))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }
        
        [TestMethod]
        public void GetTaxaBySearchCriteria()
        {
            int? revisionId = null;

            // Create initial data..
            List<Int32> taxonCategoryIds = new List<int>();
            taxonCategoryIds.Add(17);
            List<Int32> taxonIds = new List<int>();
            /*
            for (int i = 0; i < 10; i++)
            {
                i++;
                taxonCategoryIds.Add(i == 1 ? GetReferenceTaxonCategory(i, true) : GetReferenceTaxonCategory(i, false));
                i--;
            }
             
            List<Int32> taxonIds = new List<int>();
            
            // TODO Update this test when we can create taxon using taxon names....
            String taxonName = "Trumgräshoppa";
            for (int i = 0; i < 10; i++)
            {

                i++;
                taxonIds.Add(GetReferenceTaxon("taxon" + i, false));  
                i--;
                
            }
             */
            taxonIds.Add(251729);
            taxonIds.Add(251730);
            String taxonName = "Be%";

            // Check only taxon name set
            using (DataReader dataReader = GetDatabase().GetTaxaBySearchCriteria(null, null, null, null, taxonName, null, null, revisionId, Settings.Default.TestLocaleId))
            
            {
                Assert.IsTrue(dataReader.Read());
            }
            // Check taxonIds set
            using (DataReader dataReader = GetDatabase().GetTaxaBySearchCriteria(taxonIds, null, null, null, null,  null, null, revisionId, Settings.Default.TestLocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
            //Check taxon categories set 
            // TODO Update this test when we can create taxon with properties so that categories can be set....
            /*
            using (DataReader dataReader = GetDatabase().GetTaxaBySearchCriteria(null, taxonCategoryIds, null, revisionId, Settings.Default.TestLocaleId))
            {
                //Assert.IsFalse(dataReader.Read());
                // Use code below instead when Taxon properties are implemented...
               Assert.IsTrue(dataReader.Read());
            }
             */
        }

        /// <summary>
        /// Test getting all taxon name categories add 10 then we know that taxa exists in the db.
        /// </summary>
        [TestMethod]
        public void GetTaxonCategories()
        {
            // First create 10 categories and then reda then and all others....
            List<Int32> taxonCategoriesIdList = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                i++;
                taxonCategoriesIdList.Add(i == 1 ? GetReferenceTaxonCategory(i, true) : GetReferenceTaxonCategory(i, false));
                i--;
            }

            using (DataReader dataReader = GetDatabase().GetTaxonCategories(Settings.Default.TestLocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        /// <summary>
        /// Test getting taxon categories for children and parents.
        /// </summary>
        [TestMethod]
        public void GetTaxonCategoriesForTaxonInTree()
        {
            bool isPublished = true;
            int parentTaxonId = 1001546; //spillkråkans släkte
            int taxonId = 100049; // Detta är en spillkråka...
            using (DataReader dataReader = GetDatabase().GetTaxonCategoriesForTaxonInTree(parentTaxonId, taxonId, Settings.Default.TestLocaleId, isPublished))
            {
                Assert.IsTrue(dataReader.Read());
            }
            isPublished = false;
            using (DataReader dataReader = GetDatabase().GetTaxonCategoriesForTaxonInTree(parentTaxonId, taxonId, Settings.Default.TestLocaleId, isPublished))
            {
                Assert.IsTrue(dataReader.Read());
            }

           taxonId = -1;        // Inget giltigt taxon
           parentTaxonId = -1;  // Inget giltigt taxon   
           using (DataReader dataReader = GetDatabase().GetTaxonCategoriesForTaxonInTree(parentTaxonId, taxonId, Settings.Default.TestLocaleId, isPublished))
           {
               Assert.IsFalse(dataReader.Read());
           } 
        }

        /// <summary>
        /// Test getting all taxon name categories add 10 then we know that taxon category name exists in the db.
        /// </summary>
        [TestMethod]
        public void GetTaxonNameCategories()
        {
            // First create 10 categories and then reda then and all others....
            List<Int32> taxonNameCategoriesIdList = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                i++;
                taxonNameCategoriesIdList.Add(GetReferenceTaxonNameCategory(i));
                i--;
            }

            using (DataReader dataReader = GetDatabase().GetTaxonNameCategories(Settings.Default.TestLocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetTaxonNamesBySearchCriteria()
        {
            int? revisionId = null;
            using (DataReader dataReader = GetDatabase(true).GetTaxonNamesBySearchCriteria("Trumgräshoppa", StringCompareOperator.Like.ToString(), null, null, null, 
                null, null, null, null, null, null, null,false, null, null, revisionId, Settings.Default.TestLocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetTaxonTrees()
        {
            using (DataReader dataReader = GetDatabase(true).GetTaxonTrees())
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetTaxonNamesByTaxonId()
        {
            //TODO use getReferenceTaxon when we added get TaxonNames functionality...  
            Int32 existingTaxonId = 2000446;
            int? revisionId = null;
            using (DataReader dataReader = GetDatabase(true).GetTaxonNamesByTaxonId(existingTaxonId, revisionId, Settings.Default.TestLocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetTaxonNamesByTaxonIds()
        {
            List<Int32> taxonIds;

            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.Bear));
            taxonIds.Add((Int32)(TaxonId.Mammals));
            using (DataReader dataReader = GetDatabase(true).GetTaxonNamesByTaxonIds(taxonIds, null, (Int32)(LocaleId.sv_SE)))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetTaxonRelations()
        {
            using (DataReader dataReader = GetDatabase(true).GetTaxonRelations(null))
            {
                Assert.IsTrue(dataReader.Read());
            }
            using (DataReader dataReader = GetDatabase(true).GetTaxonRelations(1))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetTaxonRelationsByTaxa()
        {
            List<Int32> taxonIds;

            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.Mammals));
            foreach (TaxonRelationSearchScope searchScope in Enum.GetValues(typeof(TaxonRelationSearchScope)))
            {
                using (DataReader dataReader = GetDatabase(true).GetTaxonRelationsByTaxa(taxonIds, searchScope))
                {
                    Assert.IsTrue(dataReader.Read());
                }
            }
        }

        [TestMethod]
        public void GetTaxonRevisionEventTypes()
        {
            using (DataReader dataReader = GetDatabase(true).GetTaxonRevisionEventTypes((Int32)(LocaleId.sv_SE)))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetTaxonRevisionStates()
        {
            using (DataReader dataReader = GetDatabase(true).GetTaxonRevisionStates((Int32)(LocaleId.sv_SE)))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void IsTaxonInRevision()
        {
            Int32 taxonId;

            taxonId = (Int32) (TaxonId.Bear);
            Assert.IsTrue(GetDatabase(true).IsTaxonInRevision(taxonId));
            taxonId = (Int32)(TaxonId.DrumGrasshopper);
            Assert.IsFalse(GetDatabase().IsTaxonInRevision(taxonId));
        }

        [TestMethod]
        public void Ping()
        {
            using (WebServiceDataServer database = new TaxonServer())
            {
                Assert.IsTrue(database.Ping());
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

 #region Helper metods


        private int GetReferenceRevision()
        {
            // Create a revisionstate that we later use...
            int taxonId = GetReferenceTaxon();
            string descString = "My revision description";
            Int32 createdById = Settings.Default.TestUserId;
            Int32 revisionStateId = (Int32)(TaxonRevisionStateId.Ongoing);
            DateTime expectedStartTime = new DateTime(DateTime.Now.Ticks);
            DateTime expectedEndTime = new DateTime(2022, 1, 30);
            int revisionId = GetDatabase().CreateRevision(taxonId, descString, expectedStartTime, expectedEndTime, revisionStateId, createdById, Settings.Default.TestLocaleId, new List<int>());
              
            //return stateId;
            return revisionId;
        }
        
        
        private int GetTaxonIdFromReferenceRevision(int i, bool update)
        {
            // Create a revisionstate that we later use...
            int taxonId = GetReferenceTaxon("ReferenceTaxon", update);
            string descString = "My revision description";
            Int32 createdById = Settings.Default.TestUserId;
            Int32 revisionStateId =(Int32)(TaxonRevisionStateId.Ongoing);
            DateTime expectedStartTime = new DateTime(DateTime.Now.Ticks);
            DateTime createdDate = new DateTime(DateTime.Now.Ticks);
            DateTime expectedEndTime = new DateTime(2022, 1, 30);
            int revisionId = GetDatabase().CreateRevision(taxonId, descString, expectedStartTime, expectedEndTime, revisionStateId, createdById, Settings.Default.TestLocaleId, new List<int>());

            //return stateId;
            return taxonId;
        }
        
       
        /// <summary>
        /// Creates a taxon out of predefined data. To be used in test cases when
        /// one taxon is needed...
        /// </summary>
        /// <returns>Taxon Id</returns>
        private int GetReferenceTaxon()
        {
            // First we create a taxon that we later use...
            Int32 createdBy = Settings.Default.TestUserId;
            string personName = @"Mr Strömgärdegård";
            Boolean isPublished = false;
            DateTime validFromDate = new DateTime(DateTime.Now.Ticks);
            DateTime validToDate = new DateTime(2022, 1, 30);

            int taxonId = GetDatabase(true).CreateTaxon(createdBy, personName, validFromDate, validToDate, Settings.Default.TestLocaleId, 0, isPublished, new List<int>());
            return taxonId;
        }

        /// <summary>
        /// Creates a taxon out of predefined data when a text as identifier is set to
        /// Differentiate taxon from eachother. To be used in test cases when
        /// a list of taxon is needed.
        /// </summary>
        /// <param name="text">Test text</param>
        /// <param name="disposed"> sets if db are to be disposed</param>
        /// <returns>Taxon Id</returns>
        private int GetReferenceTaxon(string text, bool disposed)
        {
            // First we create a taxon that we later use...
            Int32 createdBy = Settings.Default.TestUserId;
            string personName = @"Mr Strömgärdegård";
            DateTime validFromDate = new DateTime(DateTime.Now.Ticks);
            DateTime validToDate = new DateTime(2022, 1, 30);
            Boolean isPublished = false;

            int taxonId = GetDatabase(disposed).CreateTaxon(createdBy, personName, validFromDate, validToDate, Settings.Default.TestLocaleId, 0, isPublished, new List<int>());
            return taxonId;
        }

        /// <summary>
        /// Creates a taxon category out of predefined data. To be used in test cases when
        /// one taxon category is needed...
        /// </summary>
        /// <returns>Taxon Category Id</returns>
        private int GetReferenceTaxonCategory()
        {
            // First we create a taxon category that we later use...
            string categoryName = "Svenskt";
            Int32 parentCategory = 2;
            Int32 sortOrder = 20;
            bool mainCategory = false;
            bool taxonomic = true;
            Int32 categoryId = 131;

            int taxonCategoryId = GetDatabase(true).CreateTaxonCategory(categoryId, categoryName, mainCategory, parentCategory,
                                                                        sortOrder, taxonomic, Settings.Default.TestLocaleId);
            return taxonCategoryId;
        }

        /// <summary>
        /// Creates a taxon category out of predefined data. To be used in test cases when
        /// one taxon category is needed...
        /// </summary>
        /// <param name="i">Test number</param>
        /// <param name="disposed"> sets if db are to be disposed</param>
        /// <returns>Taxon Category Id</returns>
        private int GetReferenceTaxonCategory(int i, bool disposed)
        {
            // First we create a taxon category that we later use...
            string categoryName = "Svenskt" + " " + i;
            Int32 parentCategory = 2;
            Int32 sortOrder = 20;
            bool mainCategory = false;
            bool taxonomic = true;
            Int32 categoryId = 141 + i;
            // Only dispose db first time

            int taxonCategoryId = GetDatabase(disposed).CreateTaxonCategory(categoryId, categoryName, mainCategory, parentCategory,
                                                                            sortOrder, taxonomic, Settings.Default.TestLocaleId);
            return taxonCategoryId;
        }


        private int GetReferenceTaxonNameCategory(int i)
        {
            // First we create a taxon category that we later use...
            string categoryName = "Svenskt" + " " + i;
            Int32 sortOrder = 20;
            string shortName = "Shorty";
            int type = 1;

            int taxonCategoryId = GetDatabase(true).CreateTaxonNameCategory(categoryName, shortName, sortOrder, Settings.Default.TestLocaleId, type);
            return taxonCategoryId;
        }

#endregion
    }
}

using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxonManager = ArtDatabanken.WebService.TaxonService.Data.TaxonManager;

namespace ArtDatabanken.WebService.TaxonService.Test.Data
{
    [TestClass]
    public class TaxonManagerTest: TestBase
    {
        private const string IS_MICROSPECIES = "IsMicrospecies";

        public TaxonManagerTest()
            : base(useTransaction, 50)
        {
        }

        #region Additional test attributes
        private TestContext testContextInstance;
        private static Boolean useTransaction = true;

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

        private void CheckCircularTree(List<WebTaxonTreeNode> taxonTrees)
        {
            List<Int32> taxonIds;

            if (taxonTrees.IsNotEmpty())
            {
                foreach (WebTaxonTreeNode taxonTree in taxonTrees)
                {
                    taxonIds = new List<Int32>();
                    CheckCircularTree(taxonTree, taxonIds);
                }
            }
        }

        private void CheckCircularTree(WebTaxonTreeNode taxonTree,
                                       List<Int32> taxonIds)
        {
            List<Int32> childTaxonIds;

            if (taxonTree.IsNotNull())
            {
                if (taxonIds.Contains(taxonTree.Taxon.Id))
                {
                    // Circular taxon tree found.
                    throw new Exception("Circular taxon tree found! Taxon id = " + taxonTree.Taxon.Id);
                }
                else
                {
                    taxonIds.Add(taxonTree.Taxon.Id);
                }

                if (taxonTree.Children.IsNotEmpty())
                {
                    foreach (WebTaxonTreeNode child in taxonTree.Children)
                    {
                        childTaxonIds = new List<Int32>();
                        childTaxonIds.AddRange(taxonIds);
                        CheckCircularTree(child, childTaxonIds);
                    }
                }
            }
        }

        private void CheckDuplicateTaxa(List<WebTaxonTreeNode> taxonTrees)
        {
            Dictionary<Int32, WebTaxonTreeNode> taxonTreeNodes;

            if (taxonTrees.IsNotEmpty())
            {
                foreach (WebTaxonTreeNode taxonTree in taxonTrees)
                {
                    taxonTreeNodes = new Dictionary<Int32, WebTaxonTreeNode>();
                    CheckDuplicateTaxa(taxonTree, taxonTreeNodes);
                }
            }
        }

        private void CheckDuplicateTaxa(WebTaxonTreeNode taxonTree,
                                        Dictionary<Int32, WebTaxonTreeNode> taxonTreeNodes)
        {
            WebTaxonTreeNode duplicateTaxonTreeNode;

            if (taxonTree.IsNotNull())
            {
                if (taxonTreeNodes.ContainsKey(taxonTree.Taxon.Id))
                {
                    // Duplicate factor tree found.
                    duplicateTaxonTreeNode = taxonTreeNodes[taxonTree.Taxon.Id];
                }
                else
                {
                    taxonTreeNodes[taxonTree.Taxon.Id] = taxonTree;
                }

                if (taxonTree.Children.IsNotEmpty())
                {
                    foreach (WebTaxonTreeNode child in taxonTree.Children)
                    {
                        CheckDuplicateTaxa(child, taxonTreeNodes);
                    }
                }
            }
        }

        [TestMethod]
        [Ignore]
        public void CreateTaxon()
        {

            int revisionId = 1;

            var revisionEvent = new WebTaxonRevisionEvent()
            {
                CreatedBy = 2,
                CreatedDate = DateTime.Now,
                TypeId = 1,
                RevisionId = revisionId
            };

            revisionEvent = TaxonManager.CreateRevisionEvent(GetRevisionContext(), revisionEvent);

            WebTaxon newTaxon, taxon;

            taxon = new WebTaxon();
            taxon.CreatedBy = GetRevisionContext().GetUser().Id;
            taxon.IsPublished = false;
            taxon.SetModifiedByPerson(@"");
            newTaxon = TaxonManager.CreateTaxon(GetRevisionContext(), taxon, revisionEvent);
            Assert.IsNotNull(newTaxon);
            Assert.IsTrue(newTaxon.Id >= 0);
            Assert.IsTrue(newTaxon.Guid.IsNotEmpty());
            Assert.AreEqual(GetRevisionContext().GetUser().Id, newTaxon.CreatedBy);
            Assert.IsTrue((DateTime.Now - newTaxon.CreatedDate) < new TimeSpan(0, 0, Settings.Default.ComputerTimeDifference));
            Assert.IsFalse(newTaxon.IsPublished);
            Assert.AreEqual("TestFirstName TestLastName", newTaxon.GetModifiedByPerson());
        }

        [TestMethod]
       // [TestCategory("NightlyTest")]
        public void CreateTaxonName()
        {
            WebTaxonName newTaxonName, taxonName;

            // Create test taxon name.
            taxonName = GetReferenceTaxonName((Int32)(TaxonId.Bears));
            taxonName.StatusId = (Int32)(TaxonNameStatusId.InvalidNaming);
            taxonName.Name = "te'st";
            newTaxonName = TaxonManager.CreateTaxonName(GetRevisionContext(), taxonName);

            // GUID and ID should differ...
            Assert.IsNotNull(newTaxonName);
            Assert.AreNotEqual(taxonName.GetVersion(), newTaxonName.GetVersion());
            Assert.AreNotEqual(taxonName.Guid, newTaxonName.Guid);
            Assert.AreEqual(taxonName.Description, newTaxonName.Description);
            Assert.AreEqual((Int32)(TaxonNameStatusId.InvalidNaming), taxonName.StatusId);
            Assert.AreEqual(GetContext().GetUser().Id, newTaxonName.CreatedBy);
            Assert.AreEqual(taxonName.Author, newTaxonName.Author);
        }

        [TestMethod]
        // TODO failing test[TestCategory("NightlyTest")]
        public void CopyTaxonName()
        {
            int taxonNameId = 3566;
            WebTaxonName taxonName = TaxonManager.GetTaxonNameById(GetContext(), taxonNameId);
            taxonName.Id = 0;
            WebTaxonName newTaxonName = TaxonManager.CreateTaxonName(GetRevisionContext(), taxonName);
            Assert.IsNotNull(newTaxonName);
            // GUID and ID should differ...
            Assert.AreNotEqual(taxonName.GetVersion(), newTaxonName.GetVersion());
            Assert.AreNotEqual(taxonName.Guid, newTaxonName.Guid);

            Assert.AreEqual(taxonName.Description, newTaxonName.Description);
            Assert.AreEqual(taxonName.StatusId, newTaxonName.StatusId);
            Assert.AreEqual(taxonName.Author, newTaxonName.Author);

        }

        [TestMethod]
        public void CanUpdateExistingTaxonName_RecordExistInDatabase()
        {
            // Arrange
            int testTaxonId = 4000107;
            List<WebTaxonName> taxonNames = TaxonManager.GetTaxonNamesByTaxonId(GetContext(), testTaxonId);
            WebTaxonName taxonName = taxonNames[0];

            // Act
            taxonName.ReplacedInTaxonRevisionEventId = 1000;
            taxonName.IsReplacedInTaxonRevisionEventIdSpecified = true;
            TaxonManager.UpdateTaxonName(GetContext(), taxonName);

            taxonNames = TaxonManager.GetTaxonNamesByTaxonId(GetContext(), testTaxonId);
            taxonName = taxonNames[0];
            // Assert
            Assert.AreEqual(taxonName.ReplacedInTaxonRevisionEventId, 1000);
        }

        [TestMethod]
        public void CanUpdateTaxonParent()
        {
            // Arrange
            var taxon = TaxonManager.GetTaxonById(GetContext(), 2000400);
            var parentTaxon = TaxonManager.GetTaxonById(GetContext(), 2000448);

            // Act
            var taxonRelation = new WebTaxonRelation()
            {
                CreatedBy = 1,
                CreatedDate = DateTime.Now,
                ChildTaxonId = taxon.Id,
                ParentTaxonId = parentTaxon.Id
            };

            taxonRelation = TaxonManager.CreateTaxonRelation(GetContext(), taxonRelation);

            // Assert
            Assert.AreEqual(taxon.Id, taxonRelation.ChildTaxonId);
            Assert.AreEqual(parentTaxon.Id, taxonRelation.ParentTaxonId);
        }

        [TestMethod]
       // [TestCategory("NightlyTest")]
        public void CanUpdateTaxonRelation()
        {
            // Arrange
            var taxon = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonById(GetContext(), 2000400);
            var parentTaxon = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonById(GetContext(), 2000448);

            // Act
            var taxonRelation = new WebTaxonRelation()
            {
                CreatedBy = 1,
                CreatedDate = DateTime.Now,
                ChildTaxonId = taxon.Id,
                ParentTaxonId = parentTaxon.Id,
                ValidFromDate = DateTime.Now,
                ValidToDate = new DateTime(2022, 01, 01)
            };

            taxonRelation = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.CreateTaxonRelation(GetContext(), taxonRelation);
            taxonRelation.IsPublished = true;
            taxonRelation = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.UpdateTaxonRelation(GetContext(), taxonRelation);

            // Assert
            Assert.AreEqual(taxon.Id, taxonRelation.ChildTaxonId);
            Assert.AreEqual(parentTaxon.Id, taxonRelation.ParentTaxonId);
            Assert.AreEqual(true, taxonRelation.IsPublished);

        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void CreateTaxonNotAuthorized()
        {
            WebServiceContext localContext = new WebServiceContext(Settings.Default.UserDyntaxaReader, Settings.Default.TestApplicationIdentifier);
            // Create test taxon.
            WebTaxon taxon = GetReferenceTaxon();
            WebTaxon newTaxon = TaxonManager.CreateTaxon(localContext, taxon, null);
        }

        [TestMethod]
        public void CanCreateRevision()
        {
            // Arrange
            var taxon = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonById(GetContext(), 4000107);

            var revision = new WebTaxonRevision()
            {
                RootTaxon = taxon,
                CreatedBy = 2,
                CreatedDate = DateTime.Now,
                StateId = 1,
                ExpectedStartDate = DateTime.Now,
                ExpectedEndDate = DateTime.Now
            };

            // Act
            var createdRevision = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.CreateRevision(GetContext(), revision);

            // Assert
            Assert.AreEqual(createdRevision.RootTaxon.Id, taxon.Id);
        }


        [TestMethod]
        public void CanCreateRevisionEvent()
        {
            // Arrange
            var taxon = GetReferenceTaxon();
            var revision = new WebTaxonRevision()
            {
                RootTaxon = taxon,
                CreatedBy = 2,
                CreatedDate = DateTime.Now,
                StateId = 1,
                ExpectedStartDate = DateTime.Now,
                ExpectedEndDate = DateTime.Now
            };
            revision = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.CreateRevision(GetContext(), revision);

            // Act
            var revisionEvent = new WebTaxonRevisionEvent()
            {
                CreatedBy = 2,
                CreatedDate = DateTime.Now,
                TypeId = 1,
                RevisionId = revision.Id
            };

            revisionEvent = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.CreateRevisionEvent(GetContext(), revisionEvent);

            // Assert
            Assert.IsTrue(revisionEvent.Id > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void CreateRevisionNotAuthorized()
        {
            WebServiceContext localContext = new WebServiceContext(Settings.Default.UserDyntaxaReader, Settings.Default.TestApplicationIdentifier);
            // Arrange
            var taxon = GetReferenceTaxon();
            var revision = new WebTaxonRevision()
            {
                RootTaxon = taxon,
                CreatedBy = 2,
                CreatedDate = DateTime.Now,
                StateId = 1,
                ExpectedStartDate = DateTime.Now,
                ExpectedEndDate = DateTime.Now
            };

            // Act
            var createdRevision = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.CreateRevision(localContext, revision);
        }



        [TestMethod]
        public void CanUpdateRevision()
        {
            // Arrange
            var taxon = GetReferenceTaxon();
            var revision = new WebTaxonRevision()
            {
                RootTaxon = taxon,
                CreatedBy = 2,
                CreatedDate = DateTime.Now,
                StateId = 1,
                ExpectedStartDate = DateTime.Now,
                ExpectedEndDate = DateTime.Now
            };
            var createdRevision = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.CreateRevision(GetContext(), revision);

            // Act
            createdRevision.StateId = 2;
            var updatedRevision = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.CreateRevision(GetContext(), createdRevision);

            // Assert
            Assert.AreEqual(updatedRevision.StateId, 2);
        }

        [TestMethod]
        public void CanCreateNewTaxonProperties()
        {
            // Arrange
            var taxon = TaxonManager.GetTaxonById(GetContext(), 2000448);
            var revision = new WebTaxonRevision()
            {
                RootTaxon = taxon,
                CreatedBy = 2,
                CreatedDate = DateTime.Now,
                StateId = 1,
                ExpectedStartDate = DateTime.Now,
                ExpectedEndDate = DateTime.Now
            };
            revision = TaxonManager.CreateRevision(GetContext(), revision);

            var revisionEvent = new WebTaxonRevisionEvent()
            {
                CreatedBy = 2,
                CreatedDate = DateTime.Now,
                TypeId = 1,
                RevisionId = revision.Id
            };

            revisionEvent = TaxonManager.CreateRevisionEvent(GetContext(), revisionEvent);

            // Act
            var taxonCategory = TaxonManager.GetTaxonCategoryById(GetContext(), 1);
            var taxonProperties = new WebTaxonProperties()
            {
                IsPublished = false,
                ModifiedBy = new WebUser() { Id = 2 },
                IsValid = true,
                ModifiedDate = DateTime.Now,
                ChangedInTaxonRevisionEvent = revisionEvent,
                Taxon = taxon,
                TaxonCategory = taxonCategory,
                AlertStatusId = (Int32)(TaxonAlertStatusId.Yellow),
                PartOfConceptDefinition = "te'st",
                ConceptDefinition = "testFull"
            };

            taxonProperties.DataFields = new List<WebDataField>();
            taxonProperties.DataFields.Add(new WebDataField
            {
                Name = IS_MICROSPECIES,
                Type = WebDataType.Boolean,
                Value = Boolean.FalseString
            });


            taxonProperties = TaxonManager.CreateTaxonProperties(GetContext(), taxonProperties);

            // Assert
            Assert.AreEqual(1, taxonProperties.TaxonCategory.Id);
            Assert.AreEqual("te'st", taxonProperties.PartOfConceptDefinition);
        }

        [TestMethod]
     //   [TestCategory("NightlyTest")]
        public void CanLoadTaxonPropertiesByTaxonId()
        {
            // Arrange
            var taxonId = 2000448;

            // Act
            var taxonProperties = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonPropertiesByTaxonId(GetContext(), taxonId);

            // Assert
            Assert.IsTrue(taxonProperties.Count > 0);
        }


        [TestMethod]
        public void CanLoadParentTaxonRelationsByTaxonId()
        {
            // Arrange
            var taxonId = 100120;

            // Act
            var taxonRelations = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetParentTaxonRelationsByTaxon(GetContext(), taxonId);

            // Assert
            Assert.IsTrue(taxonRelations.Count > 0);
        }

        [TestMethod]
        public void CanLoadAllChildTaxonRelationsByTaxonId()
        {
            // Arrange
            var taxonId = 2002138;

            // Act
            var taxonRelations = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetAllChildTaxonRelationsByTaxon(GetContext(), taxonId);

            // Assert
            Assert.IsTrue(taxonRelations.Count > 0);
        }

        [TestMethod]
        public void CanLoadTaxonCategoryByTaxonPropertiesId()
        {
            // Arrange
            var taxonPropertiesId = 41488;

            // Act
            var taxonCategory = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonCategoryByTaxonPropertiesId(GetContext(), taxonPropertiesId);

            // Assert
            Assert.IsTrue(taxonCategory.SortOrder > 0);
        }

        [TestMethod]
        public void CanLoadLumpSplitEventsByTaxon()
        {
            // Arrange
            var taxonId = 230550;

            // Act
            var lumpSplitEvents = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetLumpSplitEventsByTaxon(GetContext(), taxonId);

            // Assert
            Assert.IsTrue(lumpSplitEvents.Count > 0);
        }

        [TestMethod]
        public void CanLoadLumpSplitEventsById()
        {
            // Arrange
            var lumpSplitEventId = 1500;

            // Act
            var lumpSplitEvents = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetLumpSplitEventById(GetContext(), lumpSplitEventId);

            // Assert
            Assert.IsTrue(lumpSplitEvents.Id > 0);
        }

        [TestMethod]
        public void CanCreateLumpSplitEvent()
        {
            // Arrange
            var taxonId = 251153;
            var taxon = TaxonManager.GetTaxonById(GetContext(), taxonId);

            // Act
            var newLumpSplitEvent = new WebLumpSplitEvent();
            newLumpSplitEvent.TaxonIdBefore = taxon.Id;
            newLumpSplitEvent.TaxonIdAfter = taxon.Id;
            newLumpSplitEvent.Description = "test";
            newLumpSplitEvent.TypeId = (Int32)(LumpSplitEventTypeId.Split);
            newLumpSplitEvent = TaxonManager.CreateLumpSplitEvent(GetContext(), newLumpSplitEvent);

            // Assert
            Assert.IsTrue(newLumpSplitEvent.Id > 0);

            // Act
            newLumpSplitEvent = new WebLumpSplitEvent();
            newLumpSplitEvent.TaxonIdBefore = (Int32)(TaxonId.DrumGrasshopper);
            newLumpSplitEvent.TaxonIdAfter = (Int32)(TaxonId.DrumGrasshopper);
            newLumpSplitEvent.Description = "Foo ' Bar";
            newLumpSplitEvent.TypeId = (Int32)(LumpSplitEventTypeId.Split);
            newLumpSplitEvent = TaxonManager.CreateLumpSplitEvent(GetContext(), newLumpSplitEvent);

            // Assert
            Assert.IsTrue(newLumpSplitEvent.Id > 0);
        }



        [TestMethod]
        public void DeleteTaxonRevision()
        {
            WebTaxonRevision taxonRevision;

            taxonRevision = TaxonManager.GetRevisionById(GetRevisionContext(), 3691);
            if (taxonRevision.IsNotNull())
            {
                TaxonManager.DeleteRevision(GetRevisionContext(), taxonRevision.Id);
                GetRevisionContext().CommitTransaction();
            }
        }

        [TestMethod]
        public void UpdateTaxonProperties()
        {
            // Arrange
            var taxon = GetReferenceTaxon();
            var revision = new WebTaxonRevision()
            {
                RootTaxon = taxon,
                CreatedBy = 2,
                CreatedDate = DateTime.Now,
                StateId = 1,
                ExpectedStartDate = DateTime.Now,
                ExpectedEndDate = DateTime.Now
            };
            revision = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.CreateRevision(GetContext(), revision);

            var revisionEvent = new WebTaxonRevisionEvent()
            {
                CreatedBy = 2,
                CreatedDate = DateTime.Now,
                TypeId = 1,
                RevisionId = revision.Id
            };

            revisionEvent = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.CreateRevisionEvent(GetContext(), revisionEvent);
            var taxonCategory = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonCategoryById(GetContext(), 1);
            var taxonProperties = new WebTaxonProperties()
            {
                IsPublished = false,
                ModifiedBy = new WebUser() { Id = 2 },
                ModifiedDate = DateTime.Now,
                ValidFromDate = DateTime.Now,
                ValidToDate = new DateTime(2022, 1, 1),
                ChangedInTaxonRevisionEvent = revisionEvent,
                AlertStatusId = (Int32)(TaxonAlertStatusId.Yellow),
                Taxon = taxon,
                TaxonCategory = taxonCategory
            };
            taxonProperties = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.CreateTaxonProperties(GetContext(), taxonProperties);

            // Act
            var taxonPropertiesId = taxonProperties.Id;
            taxonProperties.TaxonCategory = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonCategoryById(GetContext(), 2);
            taxonProperties.IsValid = false;
            taxonProperties = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.UpdateTaxonProperties(GetContext(), taxonProperties);

            // Assert
            Assert.AreEqual(taxonPropertiesId, taxonProperties.Id);
            Assert.AreEqual(2, taxonProperties.TaxonCategory.Id);
            Assert.AreEqual((Int32)(TaxonAlertStatusId.Yellow), taxonProperties.AlertStatusId);
            Assert.IsFalse(taxonProperties.IsValid);
        }

        [TestMethod]
        public void UpdateRevisionEvent()
        {
            // Arrange
            int revisionEventId = 1392;

            // Act
            TaxonManager.UpdateRevisionEvent(GetContext(), revisionEventId);
        }

        [TestMethod]
        public void CreateTaxonCategory()
        {
            WebTaxonCategory createdTaxonCategory, newTaxonCategory;

            newTaxonCategory = new WebTaxonCategory();
            newTaxonCategory.Name = "Svenskt100";
            newTaxonCategory.Id = 1411;
            newTaxonCategory.IsMainCategory = false;
            newTaxonCategory.ParentId = 102;
            newTaxonCategory.SortOrder = 120;
            newTaxonCategory.IsTaxonomic = true;
            createdTaxonCategory = TaxonManager.CreateTaxonCategory(GetContext(), newTaxonCategory);

            Assert.IsNotNull(createdTaxonCategory);
            Assert.AreEqual(newTaxonCategory.Name, createdTaxonCategory.Name);
            Assert.AreEqual(newTaxonCategory.Id, createdTaxonCategory.Id);
            Assert.AreEqual(newTaxonCategory.IsMainCategory, createdTaxonCategory.IsMainCategory);
            Assert.AreEqual(newTaxonCategory.ParentId, createdTaxonCategory.ParentId);
            Assert.AreEqual(newTaxonCategory.SortOrder, createdTaxonCategory.SortOrder);
            Assert.AreEqual(newTaxonCategory.IsTaxonomic, createdTaxonCategory.IsTaxonomic);
        }

        [TestMethod]
        public void CreateTaxonNameCategory()
        {
            // Create test taxon.
            WebTaxonNameCategory taxonNameCategory = GetReferenceTaxonNameCategory();
            WebTaxonNameCategory newTaxonNameCategory;
            newTaxonNameCategory = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.CreateTaxonNameCategory(GetContext(), taxonNameCategory);
            Assert.IsNotNull(newTaxonNameCategory);
            // GUID and ID should differ...
            Assert.IsNotNull(taxonNameCategory);
            Assert.AreEqual(newTaxonNameCategory.Name, taxonNameCategory.Name);
            Assert.AreNotEqual(newTaxonNameCategory.Id, taxonNameCategory.Id);
            Assert.AreEqual(newTaxonNameCategory.ShortName, taxonNameCategory.ShortName);
            Assert.AreEqual(newTaxonNameCategory.SortOrder, taxonNameCategory.SortOrder);
            Assert.AreEqual(newTaxonNameCategory.TypeId, taxonNameCategory.TypeId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetLumpSplitEventByGUIDErrorWrongGuid()
        {
            // Arrange
            var GUID = "urn:lsid:dyntaxa.se:Foo'Bar:1";

            // Act
            var lumpSplitEvent = TaxonManager.GetLumpSplitEventByGUID(GetContext(), GUID);

            // Assert
            Assert.IsTrue(lumpSplitEvent.Id > 0);
        }

        [TestMethod]
        public void GetLumpSplitEventTypes()
        {
            List<WebLumpSplitEventType> lumpSplitEventTypes;

            lumpSplitEventTypes = TaxonManager.GetLumpSplitEventTypes(GetContext());
            Assert.IsTrue(lumpSplitEventTypes.IsNotEmpty());
            foreach (WebLumpSplitEventType lumpSplitEventType in lumpSplitEventTypes)
            {
                Assert.IsTrue(0 < lumpSplitEventType.Id);
                Assert.IsTrue(lumpSplitEventType.Identifier.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetTaxonAlertStatuses()
        {
            List<WebTaxonAlertStatus> taxonAlertStatuses;

            taxonAlertStatuses = TaxonManager.GetTaxonAlertStatuses(GetContext());
            Assert.IsTrue(taxonAlertStatuses.IsNotEmpty());
            foreach (WebTaxonAlertStatus taxonAlertStatus in taxonAlertStatuses)
            {
                Assert.IsTrue(-1 < taxonAlertStatus.Id);
                Assert.IsTrue(taxonAlertStatus.Identifier.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetTaxonChangeStatuses()
        {
            List<WebTaxonChangeStatus> taxonChangeStatuses;

            taxonChangeStatuses = TaxonManager.GetTaxonChangeStatuses(GetContext());
            Assert.IsTrue(taxonChangeStatuses.IsNotEmpty());
            foreach (WebTaxonChangeStatus taxonChangeStatus in taxonChangeStatuses)
            {
                Assert.IsTrue(-5 < taxonChangeStatus.Id);
                Assert.IsTrue(taxonChangeStatus.Identifier.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetTaxonNameCategoryTypes()
        {
            List<WebTaxonNameCategoryType> taxonNameCategoryTypes;

            taxonNameCategoryTypes = TaxonManager.GetTaxonNameCategoryTypes(GetContext());
            Assert.IsTrue(taxonNameCategoryTypes.IsNotEmpty());
            foreach (WebTaxonNameCategoryType taxonNameCategoryType in taxonNameCategoryTypes)
            {
                Assert.IsTrue(-2 < taxonNameCategoryType.Id);
                Assert.IsTrue(taxonNameCategoryType.Identifier.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetNonExistingTaxon()
        {
            Int32 taxonId;
            WebTaxon taxon;

            // Set testdata
            taxonId = -11;

            // Try to get non-existing taxon.
            taxon = TaxonManager.GetTaxonById(GetContext(), taxonId);
            Assert.IsNull(taxon);
        }

        [TestMethod]
        public void GetRevisionBySearchCriteria()
        {
            List<Int32> taxonIds;
            List<WebTaxonRevision> revisions = new List<WebTaxonRevision>();
            WebTaxonRevisionSearchCriteria searchCriteria = new WebTaxonRevisionSearchCriteria();

            // Create list of taxon ids
            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.Mammals));
            searchCriteria.TaxonIds = taxonIds;
            revisions = TaxonManager.GetRevisionBySearchCriteria(GetContext(), searchCriteria);

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
            revisions = TaxonManager.GetRevisionBySearchCriteria(GetContext(), searchCriteria);

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
        public void GetRevisionBySerachCriteria_Test()
        {
            // TEMP TEST - REMOVE
            WebTaxonRevisionSearchCriteria searchCriteria = new WebTaxonRevisionSearchCriteria();
            List<WebTaxonRevision> revisions = new List<WebTaxonRevision>();
            searchCriteria.TaxonIds = new List<int>();
            searchCriteria.TaxonIds.Add(4000107);
            searchCriteria.TaxonIds.Add(239209);
            searchCriteria.StateIds = new List<int>();
            searchCriteria.StateIds.Add(2);
            revisions = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetRevisionBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsNotNull(revisions);
        }

        [TestMethod]
        public void GetRevisionsByTaxon()
        {
            int taxonId = 4000099;
            List<WebTaxonRevision> revisions;

            revisions = TaxonManager.GetRevisionsByTaxon(GetContext(), taxonId);
            Assert.IsNotNull(revisions);
        }

        [TestMethod]
        public void GetRevisionEventsByRevisionId()
        {
            List<WebTaxonRevisionEvent> revisionEvents;

            revisionEvents = TaxonManager.GetRevisionEventsByRevisionId(GetContext(), 1);
            Assert.IsTrue(revisionEvents.IsNotEmpty());
            foreach (WebTaxonRevisionEvent revisionEvent in revisionEvents)
            {
                Assert.AreEqual(1, revisionEvent.RevisionId);
            }
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
            taxa = TaxonManager.GetTaxaByIds(GetContext(), taxonIds);
            Assert.IsNotNull(taxa);
            Assert.AreEqual(10, taxa.Count);
            Assert.IsNotNull(taxa[0].CreatedDate);
            Assert.IsNotNull(taxa[1].Guid);
            Assert.AreNotEqual(taxa[9].Id, taxa[8].Id);
            Assert.AreEqual(taxa[9].CreatedBy, taxa[0].CreatedBy);
            Assert.AreEqual(taxa[5].GetModifiedByPerson(), taxa[6].GetModifiedByPerson());

            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.Bear));
            taxa = TaxonManager.GetTaxaByIds(GetContext(), taxonIds);
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxaByIds_RecordsExists()
        {
            // Create list of taxon ids from taxa in revision # 1
            List<Int32> taxonIdList = new List<Int32>();
            taxonIdList.Add(100084);
            taxonIdList.Add(100085);

            List<WebTaxon> taxaList = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxaByIds(GetRevisionContext(), taxonIdList);
            Assert.IsNotNull(taxaList);
            Assert.AreEqual(2, taxaList.Count);
            Assert.AreEqual(taxaList[0].ScientificName, "Muscardinus avellanarius");
        }

        [TestMethod]
        public void GetTaxaPossibleParents()
        {
            Int32 taxonId = 2002137; // Taxonkategori: Familj
            List<WebTaxon> taxa = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxaPossibleParents(GetRevisionContext(), taxonId);
            Assert.IsNotNull(taxa);

        }

        [TestMethod]
        public void GetAllParentTaxonRelationsByTaxon()
        {
            Int32 taxonId; 
            List<WebTaxonRelation> taxaRelationList;

            taxonId = 2002137;
            taxaRelationList = TaxonService.Data.TaxonManager.GetAllParentTaxonRelationsByTaxon(GetRevisionContext(), taxonId);
            Assert.IsNotNull(taxaRelationList);

            taxonId = (Int32)(TaxonId.Bear);
            taxaRelationList = TaxonService.Data.TaxonManager.GetAllParentTaxonRelationsByTaxon(GetContext(), taxonId);
            Assert.IsNotNull(taxaRelationList);
        }

        [TestMethod]
        public void CanLoadTaxonNameBasedOnGuid()
        {
            // Arrange
            const String guid = "urn:lsid:dyntaxa.se:TaxonName:230";

            // Act
            WebTaxonName taxonName = TaxonManager.GetTaxonNameByGUID(GetContext(), guid);

            // Assert
            Assert.IsTrue(taxonName.GetVersion() > 0);
        }

        [TestMethod]
        public void CanLoadLumpSplitEventBasedOnGuid()
        {
            // Arrange
            var GUID = "urn:lsid:dyntaxa.se:LumpSplitEvent:1";

            // Act
            var lumpSplitEvent = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetLumpSplitEventByGUID(GetContext(), GUID);

            // Assert
            Assert.IsTrue(lumpSplitEvent.Id > 0);            
        }

        [TestMethod]
        public void CanLoadRevisionBasedOnGuid()
        {
            // Arrange
            var GUID = "urn:lsid:dyntaxa.se:Revision:1";

            // Act
            var revision = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetRevisionByGUID(GetContext(), GUID);

            // Assert
            Assert.IsTrue(revision.Id > 0);                        
        }

        [TestMethod]
        public void GetTaxaBySearchCriteria_RecordsExists()
        {
            WebTaxonSearchCriteria searchCriteria = new WebTaxonSearchCriteria();
            List<Int32> taxonIdList = new List<Int32>();

            //TEST
            taxonIdList.Add(265378);
            searchCriteria.TaxonNameSearchString = null;
            searchCriteria.TaxonIds = taxonIdList;
            searchCriteria.Scope = TaxonSearchScope.AllChildTaxa;
            List<WebTaxon> taxa = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxaBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsNotNull(taxa);
            Assert.AreEqual(1, taxa.Count);
            Assert.IsNotNull(taxa[0].GetModifiedByPerson());
            Assert.IsNotNull(taxa[0].CreatedDate);
            

            taxonIdList.Clear();
            taxonIdList.Add(251729);
            taxonIdList.Add(251730);
            searchCriteria.TaxonNameSearchString = null;
            searchCriteria.TaxonIds = taxonIdList;
            taxa = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxaBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsNotNull(taxa);
            Assert.AreEqual(2, taxa.Count);
            Assert.IsNotNull(taxa[0].GetModifiedByPerson());
            Assert.IsNotNull(taxa[0].CreatedDate);
            Assert.IsNotNull(taxa[1].Guid);
            Assert.AreNotEqual(taxa[0].Id, taxa[1].Id);

            // Within  revision # 1
            taxonIdList.Clear();
            taxonIdList.Add(100015);
            taxonIdList.Add(100024);
            searchCriteria.TaxonNameSearchString = null;
            searchCriteria.TaxonIds = taxonIdList;
            taxa = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxaBySearchCriteria(GetRevisionContext(), searchCriteria);
            Assert.IsNotNull(taxa);
            Assert.AreEqual(2, taxa.Count);
            Assert.IsNotNull(taxa[0].GetModifiedByPerson());
            Assert.IsNotNull(taxa[0].CreatedDate);
            Assert.IsNotNull(taxa[1].Guid);
            Assert.AreNotEqual(taxa[0].Id, taxa[1].Id);

            // AllChildTaxa
            taxonIdList.Clear();
            taxonIdList.Add(4000107);
            searchCriteria.TaxonNameSearchString = null;
            searchCriteria.TaxonIds = taxonIdList;
            searchCriteria.Scope = TaxonSearchScope.AllChildTaxa;
            taxa = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxaBySearchCriteria(GetRevisionContext(), searchCriteria);
            Assert.IsNotNull(taxa);
            Assert.IsTrue(taxa.Count > 10);

            //Test of problematic taxon that generates duplicates.
            //Test that the TaxonManager.GetTaxaBySearchCriteria() method removes the duplicates.
            taxonIdList.Clear();
            taxonIdList.Add(265378);
            searchCriteria.TaxonNameSearchString = null;
            searchCriteria.TaxonIds = taxonIdList;
            taxa = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxaBySearchCriteria(GetRevisionContext(964), searchCriteria);
            Assert.IsNotNull(taxa);
            Assert.AreEqual(1, taxa.Count);

            // SwedishOccurrence
            List<Int32> swedishOccurrenceList = new List<Int32>();
            List<Int32> taxonCategoryIdList = new List<int>();
            taxa.Clear();
            taxonIdList.Clear();
            //taxonIdList.Add(2002161);
            taxonIdList.Add(2002154);

            taxonCategoryIdList.Add(11);
            taxonCategoryIdList.Add(14);
            taxonCategoryIdList.Add(17);
            
            searchCriteria.TaxonNameSearchString = null;
            searchCriteria.TaxonIds = taxonIdList;
            searchCriteria.TaxonCategoryIds = taxonCategoryIdList;
            searchCriteria.Scope = TaxonSearchScope.AllChildTaxa;
            swedishOccurrenceList.Add(5);
            searchCriteria.SwedishOccurrence = swedishOccurrenceList;
            taxa = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxaBySearchCriteria(GetRevisionContext(), searchCriteria);
            Assert.IsNotNull(taxa);
            Assert.IsTrue(taxa.Count > 0);
            
            /*
            List<Int32> taxonCategoryIdList = new List<int>();
            taxonCategoryIdList.Add(11);
            searchCriteria.TaxonName = null;
            searchCriteria.TaxonIds = null;
            searchCriteria.TaxonCategoryIds = taxonCategoryIdList;
            taxa = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxaBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsNotNull(taxa);
            Assert.AreEqual(11, taxa[0].Category);
             */

        }

        [TestMethod]
        public void GetTaxaBySearchCriteria()
        {    
            String taxonName = "Trumgräshoppa";

            WebTaxonSearchCriteria searchCriteria = new WebTaxonSearchCriteria();
            searchCriteria.TaxonNameSearchString = new WebStringSearchCriteria();
            searchCriteria.TaxonNameSearchString.SearchString = taxonName;

            List<WebTaxon> taxa = TaxonManager.GetTaxaBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsNotNull(taxa);
            Assert.IsTrue(taxa.Count > 0 );
            Assert.IsTrue(taxa[0].CommonName.Equals("trumgräshoppa"));

            searchCriteria = new WebTaxonSearchCriteria();
            searchCriteria.TaxonNameSearchString = null;
            searchCriteria.IsValidTaxon = true;
            searchCriteria.IsIsValidTaxonSpecified = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.AnthophoraQuadrimaculata));
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Bear));
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Bears));
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Butterflies));
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Beaver));
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Wolverine));
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.GreenhouseMoths));
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Hedgehog));
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Mammals));
            taxa = TaxonManager.GetTaxaBySearchCriteria(GetContext(), searchCriteria);
           
            // Check taxonIds set
            Assert.IsTrue(taxa.IsNotEmpty());
            Assert.AreEqual(searchCriteria.TaxonIds.Count, taxa.Count);
            Assert.IsNotNull(taxa[0].CreatedDate);
            Assert.IsNotNull(taxa[1].Guid);
            Assert.AreNotEqual(taxa[9].Id, taxa[8].Id);
            Assert.AreEqual(taxa[9].CreatedBy, taxa[0].CreatedBy);
            Assert.AreEqual(taxa[7].GetModifiedByPerson(), taxa[6].GetModifiedByPerson());

            //Check taxon categories set 
            // TODO Update this test when we can create taxon with properties so that categories can be set....
            // Create initial data..
            List<Int32> taxonCategoryIdList = new List<int>();
            //for (int i = 0; i < 10; i++)
            //{
            //    i++;
            //    WebTaxonCategory refTaxonCategory = GetReferenceTaxonCategory(i);
            //    i--;
            //    refTaxonCategory = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.CreateTaxonCategory(GetContext(), refTaxonCategory);
            //    taxonCategoryIdList.Add(refTaxonCategory.Id);
            //}
            taxonCategoryIdList.Add(0);
            searchCriteria.TaxonNameSearchString = null;
            searchCriteria.TaxonIds = null;
            searchCriteria.TaxonCategoryIds = taxonCategoryIdList;
            taxa = TaxonManager.GetTaxaBySearchCriteria(GetContext(), searchCriteria);

            Assert.IsNotNull(taxa);
            Assert.AreEqual(1, taxa.Count);
            //Assert.IsNotNull(taxa);
            //Assert.AreEqual(10, taxa.Count);
            //Assert.IsNotNull(taxa[0].ConceptDefinitionFullGeneratedString);
            //Assert.IsNotNull(taxa[0].CreatedDate);
            //Assert.IsNotNull(taxa[1].GUID);
            //Assert.AreNotEqual(taxa[9].Id, taxa[8].Id);
            //Assert.AreEqual(taxa[9].CreatedBy, taxa[0].CreatedBy);
            //Assert.AreEqual(taxa[7].PersonName, taxa[6].PersonName);

            taxonName = "Foo ' Bar";
            searchCriteria = new WebTaxonSearchCriteria();
            searchCriteria.TaxonNameSearchString = new WebStringSearchCriteria();
            searchCriteria.TaxonNameSearchString.SearchString = taxonName;
            taxa = TaxonManager.GetTaxaBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxa.IsEmpty());
        }

        [TestMethod]
        public void GetTaxonById()
        {
            Int32 taxonId;
            WebTaxon taxon;
            taxonId = (Int32)(TaxonId.Bear);
            taxon = TaxonManager.GetTaxonById(GetRevisionContext(), taxonId);
            Assert.IsNotNull(taxon);

            Assert.AreEqual(taxonId, taxon.Id);
        }


        [TestMethod]
        public void GetTaxonByGUID()
        {
            String taxonGuid;
            WebTaxon taxon;

            taxonGuid = "urn:lsid:dyntaxa.se:Taxon:" + (Int32)(TaxonId.Bear);
            taxon = TaxonManager.GetTaxonByGUID(GetContext(), taxonGuid);
            Assert.IsNotNull(taxon);
            Assert.AreEqual(taxonGuid, taxon.Guid);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetTaxonByGUIDErrorTaxonNotFound()
        {
            String taxonGuid;
            WebTaxon taxon;

            taxonGuid = "urn:lsid:dyntaxa.se:Taxon:Foo'Bar";
            taxon = TaxonManager.GetTaxonByGUID(GetContext(), taxonGuid);
            Assert.IsNull(taxon);
        }

        /// <summary>
        /// This test uses TaxonManager from WebService.Data namespace.
        /// GetTaxaByGUIDs is user by the AnalysisService and therfore moved to the common namspace 
        /// WebService.Data
        /// </summary>
        [TestMethod]
        public void GetTaxaByGUIDs()
        {
            List<string> taxaGUIDs = new List<string>();
            taxaGUIDs.Add("urn:lsid:dyntaxa.se:Taxon:" + (Int32)(TaxonId.DrumGrasshopper));
            taxaGUIDs.Add("urn:lsid:dyntaxa.se:Taxon:" + (Int32)(TaxonId.Bear));
            WebService.Data.TaxonManager serviceTaxonManger = new WebService.Data.TaxonManager();
            List<WebTaxon> taxa = serviceTaxonManger.GetTaxaByGUIDs(GetContext(), taxaGUIDs);
            Assert.IsTrue(taxa.Count > 0);
        }

        [TestMethod]
        public void GetTaxonById_RecordsExistsInDatabase()
        {
            WebTaxon taxon;

            taxon = TaxonService.Data.TaxonManager.GetTaxonById(GetRevisionContext(), (Int32)(TaxonId.Bear));
            Assert.IsNotNull(taxon);
            Assert.IsTrue(taxon.IsPublished);
            Assert.IsNotNull(taxon.CommonName);
        }

        [TestMethod]
        public void GetTaxonCategoriesByTaxonId()
        {
            List<WebTaxonCategory> taxonCategories1, taxonCategories2;

            taxonCategories1 = TaxonManager.GetTaxonCategoriesByTaxonId(GetContext(), (Int32) (TaxonId.Mammals));
            Assert.IsTrue(taxonCategories1.IsNotEmpty());
            Assert.IsTrue(10 < taxonCategories1.Count);
            taxonCategories2 = TaxonManager.GetTaxonCategoriesByTaxonId(GetRevisionContext(), (Int32)(TaxonId.Mammals));
            Assert.IsTrue(taxonCategories2.IsNotEmpty());
            Assert.IsTrue(10 < taxonCategories2.Count);
            Assert.IsTrue(taxonCategories1.Count <= taxonCategories2.Count);
        }

        [TestMethod]
        public void GetTaxonCategoriesForTaxonInTree()
        {
            // Arrange
            int parentTaxonId = 1001546; //spillkråkans släkte
            int taxonId = 100049; // Detta är en spillkråka...

            // Act
            List<WebTaxonCategory> categoryList = TaxonService.Data.TaxonManager.GetTaxonCategoriesForTaxonInTree(GetContext(), parentTaxonId, taxonId, true);

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

        [TestMethod]
        public void GetTaxonCategories()
        {
            List<WebTaxonCategory> taxonCategories;

            taxonCategories = TaxonService.Data.TaxonManager.GetTaxonCategories(GetContext());
            Assert.IsTrue(taxonCategories.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonCategoryById()
        {
            WebTaxonCategory taxonCategory;
                
            taxonCategory = TaxonService.Data.TaxonManager.GetTaxonCategoryById(GetContext(), (Int32)(TaxonCategoryId.Genus));
            Assert.IsNotNull(taxonCategory);
            Assert.IsTrue(taxonCategory.Name.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonChange()
        {
            List<WebTaxonChange> taxonChangeList;
            // Changes from 2012-06-01
            DateTime dateFrom = new DateTime(2012, 06, 01);
            DateTime dateTo = new DateTime(2444, 08, 01);
            taxonChangeList = TaxonManager.GetTaxonChange(GetContext(), 0, false, dateFrom, dateTo);
            Assert.IsTrue(taxonChangeList.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonNameCategories()
        {
            // Create test taxon category first.
            WebTaxonNameCategory refTaxonNameCategory = GetReferenceTaxonNameCategory();
            refTaxonNameCategory = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.CreateTaxonNameCategory(GetContext(), refTaxonNameCategory);

            List<WebTaxonNameCategory> taxonNameCategories;
            taxonNameCategories = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonNameCategories(GetContext());
            Assert.IsNotNull(taxonNameCategories);
            Assert.AreEqual(refTaxonNameCategory.Id, taxonNameCategories[(taxonNameCategories.Count - 1)].Id);
        }

        [TestMethod]
        public void GetTaxonNamesByTaxonId ()
        {
            Int32 taxonId = 2000446;
            List<WebTaxonName> taxonNames = new List<WebTaxonName>();
            taxonNames = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonNamesByTaxonId(GetContext(),taxonId);
            Assert.IsTrue(taxonNames.Count > 0);
            Assert.IsNotNull(taxonNames[0].Guid);
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
            allTaxonNames = TaxonService.Data.TaxonManager.GetTaxonNamesByTaxonIds(GetContext(), taxonIds);
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

            // Test with all valid taxa. Takes about 12 minutes.
/*            taxonIds = new List<Int32>();
            foreach (WebTaxon taxon in TaxonService.Data.TaxonManager.GetCachedTaxa(GetContext()).Values)
            {
                if (taxon.IsValid)
                {
                    taxonIds.Add(taxon.Id);
                }
            }
            allTaxonNames = TaxonService.Data.TaxonManager.GetTaxonNamesByTaxonIds(GetContext(), taxonIds);
            Assert.IsTrue(allTaxonNames.IsNotEmpty());
            Assert.AreEqual(taxonIds.Count, allTaxonNames.Count);
            for (index = 0; index < taxonIds.Count; index++)
            {
                if (allTaxonNames[index].IsNotEmpty())
                {
                    foreach (WebTaxonName taxonName in allTaxonNames[index])
                    {
                        Assert.AreEqual(taxonIds[index], taxonName.Taxon.Id);
                    }
                }
            }*/
        }

        [TestMethod]
        public void GetTaxonNameById()
        {
            Int32 taxonNameId = 114601; // Omocestus ventralis
            WebTaxonName taxonName = TaxonManager.GetTaxonNameById(GetContext(), taxonNameId);
            Assert.IsNotNull(taxonName.Taxon);
        }

        [TestMethod]
        public void GetTaxonNamesByTaxonId_WithinRevision()
        {
            Int32 taxonId = 100085;
            List<WebTaxonName> taxonNames = new List<WebTaxonName>();
            taxonNames = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonNamesByTaxonId(GetRevisionContext(), taxonId);
            Assert.IsTrue(taxonNames.Count > 0);
            Assert.IsNotNull(taxonNames[0].Taxon);
        }

        [TestMethod]
        public void GetTaxonNameByGuid()
        {
            List<WebTaxonName> taxonNames;
            String guid;
            WebTaxonName taxonName;

            taxonNames = TaxonManager.GetTaxonNamesByTaxonId(GetContext(), (Int32) TaxonId.Bear);
            Assert.IsTrue(taxonNames.IsNotEmpty());

            foreach (WebTaxonName tempTaxonName in taxonNames)
            {
                // Test GUID without version information.
                taxonName = TaxonManager.GetTaxonNameByGUID(GetContext(), tempTaxonName.Guid);
                Assert.IsNotNull(taxonName);
                Assert.AreEqual(tempTaxonName.GetVersion(), taxonName.GetVersion());

                // Test GUID with version information.
                guid = tempTaxonName.Guid + ":" + tempTaxonName.GetVersion();
                taxonName = TaxonManager.GetTaxonNameByGUID(GetContext(), guid);
                Assert.IsNotNull(taxonName);
                Assert.AreEqual(tempTaxonName.GetVersion(), taxonName.GetVersion());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetTaxonNameByGuidErrorInvalidGuid()
        {
            // Arrange
            const String guid = "urn:lsid:dyntaxa.se:TaxonName':230";

            // Act
            WebTaxonName taxonName = TaxonManager.GetTaxonNameByGUID(GetContext(), guid);

            // Assert
            Assert.IsTrue(taxonName.GetVersion() > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetTaxonNameByGuidErrorWrongVersionFormat()
        {
            List<WebTaxonName> taxonNames;
            String guid;
            WebTaxonName taxonName;

            taxonNames = TaxonManager.GetTaxonNamesByTaxonId(GetContext(), (Int32)TaxonId.Bear);
            Assert.IsTrue(taxonNames.IsNotEmpty());

            foreach (WebTaxonName tempTaxonName in taxonNames)
            {
                // Test GUID with version information.
                guid = tempTaxonName.Guid + ":" + "no version information";
                taxonName = TaxonManager.GetTaxonNameByGUID(GetContext(), guid);
                Assert.IsNotNull(taxonName);
                Assert.AreEqual(tempTaxonName.GetVersion(), taxonName.GetVersion());
            }
        }

        [TestMethod]
        public void GetTaxonNamesBySearchCriteria()
        {
            List<WebTaxonName> taxonNames;
            WebTaxonNameSearchCriteria searchCriteria;

            // Test with strange character.
            searchCriteria = new WebTaxonNameSearchCriteria();
            searchCriteria.NameSearchString = new WebStringSearchCriteria();
            searchCriteria.NameSearchString.SearchString = "björn'";
            searchCriteria.NameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            searchCriteria.IsValidTaxon = true;
            taxonNames = TaxonManager.GetTaxonNamesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonNames.IsEmpty());

            searchCriteria = new WebTaxonNameSearchCriteria();
            searchCriteria.AuthorSearchString = new WebStringSearchCriteria();
            searchCriteria.AuthorSearchString.SearchString = "björn'";
            searchCriteria.AuthorSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.AuthorSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            searchCriteria.IsValidTaxon = true;
            taxonNames = TaxonManager.GetTaxonNamesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonNames.IsEmpty());

            // Test with strange character.
            searchCriteria = new WebTaxonNameSearchCriteria();
            searchCriteria.NameSearchString = new WebStringSearchCriteria();
            searchCriteria.NameSearchString.SearchString = "<varg>";
            searchCriteria.NameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            searchCriteria.IsValidTaxon = true;
            taxonNames = TaxonManager.GetTaxonNamesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonNames.IsEmpty());

            // Test with one string compare operator.
            searchCriteria = new WebTaxonNameSearchCriteria();
            searchCriteria.NameSearchString = new WebStringSearchCriteria();
            searchCriteria.NameSearchString.SearchString = "björn";
            searchCriteria.NameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            searchCriteria.IsValidTaxon = true;
            taxonNames = TaxonManager.GetTaxonNamesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonNames.IsNotEmpty());

            // Test with more than one string compare operator.
            searchCriteria = new WebTaxonNameSearchCriteria();
            searchCriteria.NameSearchString = new WebStringSearchCriteria();
            searchCriteria.NameSearchString.SearchString = "björn";
            searchCriteria.NameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.Equal);
            searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.BeginsWith);
            searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            searchCriteria.IsValidTaxon = true;
            taxonNames = TaxonManager.GetTaxonNamesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonNames.IsNotEmpty());
            searchCriteria.NameSearchString.SearchString = "jörn";
            taxonNames = TaxonManager.GetTaxonNamesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonNames.IsNotEmpty());

            // Test taxon name status search criteria.
            searchCriteria = new WebTaxonNameSearchCriteria();
            searchCriteria.NameSearchString = new WebStringSearchCriteria();
            searchCriteria.NameSearchString.SearchString = "björn";
            searchCriteria.NameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            searchCriteria.StatusId = (Int32)(TaxonNameStatusId.ApprovedNaming);
            searchCriteria.IsStatusIdSpecified = true;
            taxonNames = TaxonManager.GetTaxonNamesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonNames.IsNotEmpty());

            // Test with author search string.
            searchCriteria = new WebTaxonNameSearchCriteria();
            searchCriteria.AuthorSearchString = new WebStringSearchCriteria();
            searchCriteria.AuthorSearchString.SearchString = "Kuyper";
            taxonNames = TaxonManager.GetTaxonNamesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonNames.IsNotEmpty());

            // Test with IsAuthorIncludedInNameSearchString w/ multiple spaces in NameSearchString
            searchCriteria = new WebTaxonNameSearchCriteria();
            searchCriteria.NameSearchString = new WebStringSearchCriteria();
            searchCriteria.NameSearchString.SearchString = "Vespertilio auritus     Linnaeus, 175";
            searchCriteria.NameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.BeginsWith);
            searchCriteria.IsAuthorIncludedInNameSearchString = true;
            taxonNames = TaxonManager.GetTaxonNamesBySearchCriteria(GetContext(), searchCriteria);
            Assert.AreEqual(1, taxonNames.Count);

            // Test within revision # 1.
            searchCriteria = new WebTaxonNameSearchCriteria();
            searchCriteria.NameSearchString = new WebStringSearchCriteria();
            searchCriteria.NameSearchString.SearchString = "fjäll";
            searchCriteria.NameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            taxonNames = TaxonService.Data.TaxonManager.GetTaxonNamesBySearchCriteria(GetRevisionContext(), searchCriteria);
            Assert.IsTrue(taxonNames.IsNotEmpty());

            // Test ModifiedDate interval
            searchCriteria = new WebTaxonNameSearchCriteria();
            searchCriteria.IsRecommended = true;
            searchCriteria.IsIsRecommendedSpecified = true;
            searchCriteria.CategoryId = 0;
            searchCriteria.IsCategoryIdSpecified = true;
            searchCriteria.IsValidTaxonName = true;
            searchCriteria.IsIsValidTaxonNameSpecified = true;
            searchCriteria.IsValidTaxon = true;
            searchCriteria.IsIsValidTaxonSpecified = true;
            searchCriteria.IsAuthorIncludedInNameSearchString = false;

            WebDataField dataField;
            dataField = new WebDataField();
            dataField.Name = "ModifiedDateStart";
            dataField.Type = WebDataType.DateTime;
            dataField.Value = "2012-04-20";
            if (searchCriteria.DataFields.IsNull())
            {
                searchCriteria.DataFields = new List<WebDataField>();
            }
            searchCriteria.DataFields.Add(dataField);

            dataField = new WebDataField();
            dataField.Name = "ModifiedDateEnd";
            dataField.Type = WebDataType.DateTime;
            dataField.Value = "2013-06-11";
            searchCriteria.DataFields.Add(dataField);

            taxonNames = TaxonManager.GetTaxonNamesBySearchCriteria(this.GetContext(), searchCriteria);
            Assert.IsTrue(taxonNames.IsNotEmpty());

        }

        [TestMethod]
        public void GetReferenceRelationById()
        {
            Int32 referenceRelationId;
            WebReferenceRelation referenceRelation;

            referenceRelationId = 100;
            referenceRelation = TaxonManager.GetReferenceRelationById(GetContext(), referenceRelationId);
            Assert.IsNotNull(referenceRelation);
            Assert.AreEqual(referenceRelationId, referenceRelation.Id);
        }

        [TestMethod]
        public void GetReferenceRelationsByGuid()
        {
            // Arrange
            int taxonNameId = 3567;
            WebTaxonName taxonName = TaxonManager.GetTaxonNameById(GetContext(), taxonNameId);

            // Act
            var references = TaxonManager.GetReferenceRelationsByGuid(GetContext(), taxonName.Guid);

            // Assert
            Assert.AreEqual(1, references.Count);
        }

        [TestMethod]
        public void GetReferenceRelationTypes()
        {
            List<WebReferenceRelationType> referenceRelationTypes;

            referenceRelationTypes = TaxonManager.GetReferenceRelationTypes(GetContext());
            Assert.IsTrue(referenceRelationTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonNameStatus()
        {
            List<WebTaxonNameStatus> taxonNameStatus;

            taxonNameStatus = TaxonManager.GetTaxonNameStatus(GetContext());
            Assert.IsTrue(taxonNameStatus.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonNameUsage()
        {
            List<WebTaxonNameUsage> taxonNameUsage;

            taxonNameUsage = TaxonManager.GetTaxonNameUsage(GetContext());
            Assert.IsTrue(taxonNameUsage.IsNotEmpty());
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
            taxonRelations1 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());

            // Test is valid.
            searchCriteria.IsIsMainRelationSpecified = false;
            searchCriteria.IsIsValidSpecified = true;
            searchCriteria.IsValid = true;
            searchCriteria.TaxonIds = null;
            taxonRelations1 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.IsValid = false;
            taxonRelations2 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations1.Count > taxonRelations2.Count);

            // Test is main relation.
            searchCriteria.IsIsMainRelationSpecified = true;
            searchCriteria.IsIsValidSpecified = false;
            searchCriteria.IsMainRelation = true;
            searchCriteria.TaxonIds = null;
            taxonRelations1 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.IsMainRelation = false;
            taxonRelations2 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations1.Count > taxonRelations2.Count);

            // Test with one taxon.
            taxonIds = new List<Int32>();
            //taxonIds.Add((Int32)(TaxonId.Mammals));
            taxonIds.Add(5000046);
            searchCriteria = new WebTaxonRelationSearchCriteria();
            searchCriteria.IsIsMainRelationSpecified = false;
            searchCriteria.IsIsValidSpecified = false;
            searchCriteria.TaxonIds = taxonIds;
            searchCriteria.Scope = TaxonRelationSearchScope.AllParentRelations;
            taxonRelations1 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestParentRelations;
            taxonRelations2 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations2.Count < taxonRelations1.Count);

            searchCriteria.Scope = TaxonRelationSearchScope.AllChildRelations;
            taxonRelations1 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestChildRelations;
            taxonRelations2 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetContext(), searchCriteria);
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
            taxonRelations1 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestParentRelations;
            taxonRelations2 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations2.Count < taxonRelations1.Count);

            searchCriteria.Scope = TaxonRelationSearchScope.AllChildRelations;
            taxonRelations1 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestChildRelations;
            taxonRelations2 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations2.Count < taxonRelations1.Count);

            // Test with circular trees.
            taxonIds = new List<Int32>();
            taxonIds.Add(6008300);
            searchCriteria = new WebTaxonRelationSearchCriteria();
            searchCriteria.IsIsMainRelationSpecified = false;
            searchCriteria.IsIsValidSpecified = false;
            searchCriteria.IsValid = false;
            searchCriteria.TaxonIds = taxonIds;
            searchCriteria.Scope = TaxonRelationSearchScope.AllParentRelations;
            taxonRelations1 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());

            // Test with revision context.
            // Test all taxon relations.
            searchCriteria = new WebTaxonRelationSearchCriteria();
            searchCriteria.IsIsMainRelationSpecified = false;
            searchCriteria.IsIsValidSpecified = false;
            searchCriteria.TaxonIds = null;
            taxonRelations1 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetRevisionContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());

            // Test is valid.
            searchCriteria.IsIsMainRelationSpecified = false;
            searchCriteria.IsIsValidSpecified = true;
            searchCriteria.IsValid = true;
            searchCriteria.TaxonIds = null;
            taxonRelations1 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetRevisionContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.IsValid = false;
            taxonRelations2 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetRevisionContext(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations1.Count > taxonRelations2.Count);

            // Test is main relation.
            searchCriteria.IsIsMainRelationSpecified = true;
            searchCriteria.IsIsValidSpecified = false;
            searchCriteria.IsMainRelation = true;
            searchCriteria.TaxonIds = null;
            taxonRelations1 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetRevisionContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.IsMainRelation = false;
            taxonRelations2 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetRevisionContext(), searchCriteria);
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
            taxonRelations1 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetRevisionContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestParentRelations;
            taxonRelations2 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetRevisionContext(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations2.Count < taxonRelations1.Count);

            searchCriteria.Scope = TaxonRelationSearchScope.AllChildRelations;
            taxonRelations1 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetRevisionContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestChildRelations;
            taxonRelations2 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetRevisionContext(), searchCriteria);
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
            taxonRelations1 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetRevisionContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestParentRelations;
            taxonRelations2 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetRevisionContext(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations2.Count < taxonRelations1.Count);

            searchCriteria.Scope = TaxonRelationSearchScope.AllChildRelations;
            taxonRelations1 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetRevisionContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestChildRelations;
            taxonRelations2 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetRevisionContext(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations2.Count < taxonRelations1.Count);

            // Test with circular trees.
            taxonIds = new List<Int32>();
            taxonIds.Add(6008300);
            searchCriteria = new WebTaxonRelationSearchCriteria();
            searchCriteria.IsIsMainRelationSpecified = false;
            searchCriteria.IsIsValidSpecified = false;
            searchCriteria.IsValid = false;
            searchCriteria.TaxonIds = taxonIds;
            searchCriteria.Scope = TaxonRelationSearchScope.AllParentRelations;
            taxonRelations1 = TaxonManager.GetTaxonRelationsBySearchCriteria(GetRevisionContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetTaxonRevisionByGuidErrorInvalidGuid()
        {
            // Arrange
            var GUID = "urn:lsid:dyntaxa.se:Revision':1";

            // Act
            var revision = TaxonManager.GetRevisionByGUID(GetContext(), GUID);

            // Assert
            Assert.IsTrue(revision.Id > 0);
        }

        [TestMethod]
        public void GetTaxonRevisionEventTypes()
        {
            List<WebTaxonRevisionEventType> taxonRevisionEventTypes;

            taxonRevisionEventTypes = TaxonManager.GetTaxonRevisionEventTypes(GetContext());
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

            taxonRevisionStates = TaxonManager.GetTaxonRevisionStates(GetContext());
            Assert.IsTrue(taxonRevisionStates.IsNotEmpty());
            foreach (WebTaxonRevisionState taxonRevisionState in taxonRevisionStates)
            {
                Assert.IsTrue(0 < taxonRevisionState.Id);
                Assert.IsTrue(taxonRevisionState.Identifier.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetTaxonStatistics()
        {
            int taxonId = 4000107;
            
            List<WebTaxonChildStatistics> taxonStatistics;
            taxonStatistics = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonStatistics(GetContext(), taxonId);
            Assert.IsNotNull(taxonStatistics);
        }

        [TestMethod]
        public void GetTaxonQualitySummary()
        {
            //int taxonId = 4000107;
            int taxonId = 5000001;

            List<WebTaxonChildQualityStatistics> taxonQualitySummary;
            taxonQualitySummary = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonQualitySummary(GetContext(), taxonId);
            Assert.IsNotNull(taxonQualitySummary);
        }

        [TestMethod]
        public void IsTaxonInRevision()
        {
            Int32 taxonId = 3000303;
            Assert.IsTrue(ArtDatabanken.WebService.TaxonService.Data.TaxonManager.IsTaxonInRevision(GetContext(), taxonId));
            taxonId = 30003031;
            Assert.IsFalse(ArtDatabanken.WebService.TaxonService.Data.TaxonManager.IsTaxonInRevision(GetContext(), taxonId));
        }


        [TestMethod]
        public void RevisionCheckIn()
        {
            // Arrange
            var taxon = GetReferenceTaxon();
            var revision = new WebTaxonRevision()
            {
                RootTaxon = taxon,
                CreatedBy = 2,
                CreatedDate = DateTime.Now,
                StateId = (int)TaxonRevisionStateId.Created,
                ExpectedStartDate = DateTime.Now,
                ExpectedEndDate = DateTime.Now
            };
            var createdRevision = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.CreateRevision(GetContext(), revision);

            // Act
            var checkedInRevision = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.RevisionCheckIn(GetContext(), createdRevision);

            // Assert
            Assert.AreEqual(checkedInRevision.StateId, (int)TaxonRevisionStateId.Closed);
        }

        [TestMethod]
        public void RevisionCheckOut()
        {
            // Arrange
            WebTaxonRevision revision = new WebTaxonRevision()
            {
                RootTaxon = new WebTaxon() { Id = (Int32)(TaxonId.DrumGrasshopper) },
                CreatedBy = 2,
                CreatedDate = DateTime.Now,
                StateId = (int)TaxonRevisionStateId.Created,
                ExpectedStartDate = DateTime.Now,
                ExpectedEndDate = DateTime.Now
            };
            WebTaxonRevision createdRevision = TaxonService.Data.TaxonManager.CreateRevision(GetContext(), revision);

            // Act
            WebTaxonRevision checkedOutRevision = TaxonService.Data.TaxonManager.RevisionCheckOut(GetContext(), createdRevision);

            // Assert
            Assert.AreEqual(checkedOutRevision.StateId, (int)TaxonRevisionStateId.Ongoing);
        }

        [TestMethod]
        public void SetTaxonTreeSortOrder()
        {
            Int32 revisionEventId = 11;
            Int32 taxonIdParent = 267320;
            List<Int32> taxonIdChildList = new List<Int32>();
            List<WebTaxonRelation> taxaRelationList;
            WebTaxonRelationSearchCriteria searchCriteria;

            taxonIdChildList.Add(100024);  // Varg
            taxonIdChildList.Add(233621);  // Hund
            // Do the sorting
            TaxonManager.SetTaxonTreeSortOrder(GetRevisionContext(), taxonIdParent, taxonIdChildList, revisionEventId);

            // Check result
            searchCriteria = new WebTaxonRelationSearchCriteria();
            searchCriteria.Scope = TaxonRelationSearchScope.NearestChildRelations;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(taxonIdParent);
            taxaRelationList = TaxonManager.GetTaxonRelationsBySearchCriteria(GetRevisionContext(), searchCriteria);
            Assert.IsTrue(taxaRelationList.IsNotEmpty());
            Assert.IsTrue(taxaRelationList[0].ParentTaxonId.Equals(267320));
            Assert.IsTrue(taxaRelationList[1].ParentTaxonId.Equals(267320));
        }

        private WebServiceContext getEnglishContext()
        {
            WebServiceContext context = GetContext();
            context.Locale.ISOCode = "en-GB";
            return context;
        }

        [TestMethod]
        public void GetTaxonConceptDefinitionRemoved()
        {
            string definition = "";
            WebTaxon taxon;

            //case Removed
            taxon = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonById(GetContext(), 4000075);
            Assert.AreEqual(taxon.ChangeStatusId, (Int32)(TaxonChangeStatusId.InvalidDueToDelete));
            definition = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonConceptDefinition(GetContext(), taxon);
            Assert.IsTrue(definition.Length > taxon.PartOfConceptDefinition.Length);
        }


        [TestMethod]
        public void GetTaxonConceptDefinitionUnchanged()
        {
            string definition = "";
            WebTaxon taxon;

            //case Unchanged
            taxon = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonById(GetContext(), 2002964);
            Assert.AreEqual(taxon.ChangeStatusId, (Int32)(TaxonChangeStatusId.Unchanged));
            definition = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonConceptDefinition(GetContext(), taxon);
            Assert.AreEqual(definition, taxon.PartOfConceptDefinition);
        }

        [TestMethod]
        public void GetTaxonconceptDefinitionSplitInEnglish()
        {
            string definition = "";
            WebTaxon taxon;

            //Case Splitted.  Exempel Conocephalum conicum [2632]:
            //This taxon has been splitted and is now replaced by Conocephalum conicum [233220] and Conocephalum salebrosum [233219]. 
            //The taxon is no longer valid as a species but can be used as a collective taxon representing the group of replacing taxa.
            taxon = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonById(getEnglishContext(), 2632);
            Assert.AreEqual(taxon.ChangeStatusId, (Int32)(TaxonChangeStatusId.InvalidDueToSplit));
            definition = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonConceptDefinition(getEnglishContext(), taxon);
            Assert.IsTrue(definition.IndexOf("[233220]") > -1);
            Assert.IsTrue(definition.IndexOf("[233220]") < definition.IndexOf("[233219]"));
            //Assert.IsTrue((definition.IndexOf(" art ") > -1) || (definition.IndexOf(" species ") > -1)); //Detta kan inte fungera förrän vi fört över information om vad taxonet varit innan det blev splittat från dt_taxon vilket inte görs idag.
            //Assert.IsTrue(definition.IndexOf("This taxon has been splitted and is now replaced by Conocephalum conicum [233220] and Conocephalum salebrosum [233219]. The taxon is no longer valid as a species but can be used as a collective taxon representing the group of replacing taxa.") > -1);
        }

        [TestMethod]
        public void GetTaxonConceptDefinitionLumpedInSwedsih()
        {
            string definition = "";
            WebTaxon taxon;

            //Case Lumped into another taxon which has replaced it.
            //Exemple Catantopidae [2002844]
            taxon = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonById(GetContext(), 2002844);
            Assert.AreEqual(taxon.ChangeStatusId, (Int32)(TaxonChangeStatusId.InvalidDueToLump));
            definition = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonConceptDefinition(GetContext(), taxon);
            Assert.IsTrue(definition.IndexOf("2000897") > -1);
            Assert.IsTrue(definition.IndexOf("Taxonet har slagits samman med ett annat taxon och har därmed ersatts med Acrididae [2000897].") > -1);
            //Case Lumped (Classic) Two taxa becomes replaced by a new taxon concept.
            //taxon Phalacrus brisouti [101533] (enelskspåkig version)
            taxon = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonById(GetContext(), 101533);
            Assert.AreEqual(taxon.ChangeStatusId, (Int32)(TaxonChangeStatusId.InvalidDueToLump));
            definition = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonConceptDefinition(GetContext(), taxon);
            Assert.IsTrue(definition.IndexOf("[105586]") > -1);
            Assert.IsTrue(definition.IndexOf("[105586]") < definition.IndexOf("[230544]"));
            //Testet nedan fungerar inte om inte språkhanteringen fungerar.
            //Assert.IsTrue(definition.IndexOf("This taxon has been lumped together with Phalacrus fimetarius [105586] and is now replaced by the species Phalacrus fimetarius [230544]. The taxon is no longer a valid species.") > -1);
        }

        [TestMethod]
        public void GetTaxonConceptDefinitionLumpedInEnglish()
        {

            string definition = "";
            WebTaxon taxon;

            //Case Lumped into another taxon which has replaced it.
            //Exemple Catantopidae [2002844]
            taxon = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonById(getEnglishContext(), 2002844);
            Assert.AreEqual(taxon.ChangeStatusId, (Int32)(TaxonChangeStatusId.InvalidDueToLump));
            definition = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonConceptDefinition(getEnglishContext(), taxon);
            Assert.IsTrue(definition.IndexOf("2000897") > -1);
            Assert.IsTrue(definition.IndexOf("This taxon has been lumped into Acrididae [2000897] which has now replaced it.") > -1);
            //Case Lumped (Classic) Two taxa becomes replaced by a new taxon concept.
            //taxon Phalacrus brisouti [101533] (enelskspåkig version)
            taxon = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonById(getEnglishContext(), 101533);
            Assert.AreEqual(taxon.ChangeStatusId, (Int32)(TaxonChangeStatusId.InvalidDueToLump));
            definition = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonConceptDefinition(getEnglishContext(), taxon);
            Assert.IsTrue(definition.IndexOf("[105586]") > -1);
            Assert.IsTrue(definition.IndexOf("[105586]") < definition.IndexOf("[230544]"));
            //Testet nedan fungerar inte om inte språkhanteringen fungerar.
            //Assert.IsTrue(definition.IndexOf("This taxon has been lumped together with Phalacrus fimetarius [105586] and is now replaced by the species Phalacrus fimetarius [230544]. The taxon is no longer a valid species.") > -1);
        }

        [TestMethod]
        public void GetTaxonConceptDefinitionLumpingInSwedish()
        {
            string definition = "";
            WebTaxon taxon;

            //case Lumping (classic)
            //taxon Phalacrus fimetarius [230544] (engelskspåkig version)
            taxon = TaxonManager.GetTaxonById(GetContext(), 230544);
            Assert.AreEqual(taxon.ChangeStatusId, (Int32)(TaxonChangeStatusId.ValidAfterLump));
            definition = TaxonManager.GetTaxonConceptDefinition(GetContext(), taxon);
            Assert.AreEqual(definition, "Detta taxon har ersatt Phalacrus fimetarius [105586] och Phalacrus brisouti [101533]. Dessa taxa betraktas inte längre som separata taxa och har därför sammanfogats.");

            //case Lumping where replacing taxon is the same as one of the lumped taxa.
            /*"Lumping" when one taxon has been lumped into another which is now replacing both of them could be better.
             * Exempel Acrididae [2000897]: "This taxon is a result of a lumping of Catantopidae [2002844]." 
             * En bättre text: "This taxon is a result of a lumping of Catantopidae [2002844] into itself."*/
            taxon = TaxonManager.GetTaxonById(GetContext(), 2000897);
            Assert.AreEqual(taxon.ChangeStatusId, (Int32)(TaxonChangeStatusId.ValidAfterLump));
            definition = TaxonManager.GetTaxonConceptDefinition(GetContext(), taxon);
            Assert.IsTrue(definition.Length > 50);
        }

        [TestMethod]
        public void GetTaxonConceptDefinitionLumpingInEnglish()
        {
            string definition = "";
            WebTaxon taxon;

            //case Lumping (classic)
            //taxon Phalacrus fimetarius [230544] (engelskspåkig version)
            WebServiceContext context = getEnglishContext();
            taxon = TaxonManager.GetTaxonById(context, 230544);
            Assert.AreEqual(taxon.ChangeStatusId, (Int32)(TaxonChangeStatusId.ValidAfterLump));
            definition = TaxonManager.GetTaxonConceptDefinition(context, taxon);
            Assert.AreEqual(definition, "This taxon is a result of a lumping of Phalacrus fimetarius [105586] and Phalacrus brisouti [101533].");

            //case Lumping where replacing taxon is the same as one of the lumped taxa.
            /*"Lumping" when one taxon has been lumped into another which is now replacing both of them could be better.
             * Exempel Acrididae [2000897]: "This taxon is a result of a lumping of Catantopidae [2002844]." 
             * En bättre text: "This taxon is a result of a lumping of Catantopidae [2002844] into itself."*/
            taxon = TaxonManager.GetTaxonById(context, 2000897);
            Assert.AreEqual(taxon.ChangeStatusId, (Int32)(TaxonChangeStatusId.ValidAfterLump));
            definition = TaxonManager.GetTaxonConceptDefinition(context, taxon);
            Assert.IsTrue(definition.Length > 50);
        }

        [TestMethod]
        public void GetTaxonConceptDefinition()
        {
            String definition;
            WebTaxon taxon;

            taxon = TaxonManager.GetTaxonById(GetContext(), 257064);
            definition = TaxonManager.GetTaxonConceptDefinition(GetContext(), taxon);
            Assert.IsNotNull(taxon);
            Assert.IsTrue(definition.IsNotEmpty() || definition.IsEmpty());
        }

        [TestMethod]
        public void GetTaxonConceptDefinitionGraded()
        {
            string definition = "";
            WebTaxon taxon;

            //case Graded
            taxon = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonById(GetContext(), 5000030);
            definition = ArtDatabanken.WebService.TaxonService.Data.TaxonManager.GetTaxonConceptDefinition(GetContext(), taxon);
            Assert.IsTrue(definition.Length > 50);
        }

        [TestMethod]
        public void GetTaxonTreesBySearchCriteria()
        {
            Int32 taxonId;
            List<WebTaxonTreeNode> taxonTrees;
            WebTaxonTreeSearchCriteria searchCriteria;

            // Get a part of the taxon tree.
            taxonId = (Int32) (TaxonId.Mammals);
            //taxonId = 5000046;
            foreach (TaxonTreeSearchScope scope in Enum.GetValues(typeof(TaxonTreeSearchScope)))
            {
                searchCriteria = new WebTaxonTreeSearchCriteria();
                searchCriteria.IsMainRelationRequired = false;
                searchCriteria.IsValidRequired = true;
                searchCriteria.Scope = scope;
                searchCriteria.TaxonIds = new List<Int32>();
                searchCriteria.TaxonIds.Add(taxonId);
                taxonTrees = TaxonManager.GetTaxonTreesBySearchCriteria(GetContext(), searchCriteria);
                Assert.IsTrue(taxonTrees.IsNotEmpty());
                if ((searchCriteria.Scope == TaxonTreeSearchScope.AllChildTaxa) ||
                    (searchCriteria.Scope == TaxonTreeSearchScope.NearestChildTaxa))
                {
                    Assert.AreEqual(1, taxonTrees.Count);
                    Assert.AreEqual(taxonId, taxonTrees[0].Taxon.Id);
                }
            }

            // Get the entire taxon tree (valid taxa and relations).
            searchCriteria = new WebTaxonTreeSearchCriteria();
            searchCriteria.IsMainRelationRequired = false;
            searchCriteria.IsValidRequired = true;
            taxonTrees = TaxonManager.GetTaxonTreesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonTrees.IsNotEmpty());

            // Get the entire taxon tree (include not valid taxa and relations).
            searchCriteria = new WebTaxonTreeSearchCriteria();
            searchCriteria.IsMainRelationRequired = false;
            searchCriteria.IsValidRequired = false;
            taxonTrees = TaxonManager.GetTaxonTreesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonTrees.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonTreesBySearchCriteria_Special()
        {
            // This test only works on production servers.

            List<WebTaxonTreeNode> taxonTrees;
            WebTaxonTreeSearchCriteria searchCriteria = new WebTaxonTreeSearchCriteria();
            WebTaxonTreeNode taxonTreeNode;
            /*
            searchCriteria.IsMainRelationRequired = false;
            searchCriteria.IsValidRequired = false;
            searchCriteria.Scope = TaxonTreeSearchScope.AllParentTaxa;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(1008571);
            taxonTrees = TaxonManager.GetTaxonTreesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonTrees.IsNotEmpty());
            */
            // Test problem with taxon with id 6000738
            searchCriteria = new WebTaxonTreeSearchCriteria();
            searchCriteria.IsMainRelationRequired = false;
            searchCriteria.IsValidRequired = true;
            searchCriteria.Scope = TaxonTreeSearchScope.AllChildTaxa;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(4000104);
            
            //searchCriteria.TaxonIds.Add(5000046);

            //searchCriteria.TaxonIds.Add(4000241);

            //searchCriteria.TaxonIds.Add(3000511);
            //searchCriteria.TaxonIds.Add(1008603);
            //searchCriteria.TaxonIds.Add(3000166);

            taxonTrees = TaxonManager.GetTaxonTreesBySearchCriteria(GetRevisionContext(416), searchCriteria);
            Assert.IsTrue(taxonTrees.IsNotEmpty());
            //Assert.AreEqual(1, taxonTrees.Count);
            //taxonTreeNode = taxonTrees[0];
            //while (taxonTreeNode.Children.IsNotEmpty())
            //{
            //    taxonTreeNode = taxonTreeNode.Children[0];
            //}
            //Assert.IsNotNull(taxonTreeNode.Taxon);
           // Assert.AreEqual(6000738, taxonTreeNode.Taxon.Id);
        }

        [TestMethod]
        public void GetTaxonTreesCheckCircularTree()
        {
            Int32 taxonId;
            List<WebTaxonTreeNode> taxonTrees;
            WebTaxonTreeSearchCriteria searchCriteria;

            taxonId = (Int32)(TaxonId.Vertebrate);
            searchCriteria = new WebTaxonTreeSearchCriteria();
            searchCriteria.IsMainRelationRequired = false;
            searchCriteria.IsValidRequired = false;
            searchCriteria.Scope = TaxonTreeSearchScope.AllChildTaxa;
            //searchCriteria.TaxonIds = new List<Int32>();
            //searchCriteria.TaxonIds.Add(taxonId);
            taxonTrees = TaxonManager.GetTaxonTreesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonTrees.IsNotEmpty());
            CheckCircularTree(taxonTrees);
            CheckDuplicateTaxa(taxonTrees);
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
            revEvent.RevisionId = 1;
            revEvent.Id = 1;
            return revEvent;
        }

        
        /// <summary>
        /// Creates a taxon for test.
        /// </summary>
        /// <returns>webTaxon</returns>
        private WebTaxon GetReferenceTaxon()
        {

            WebTaxon refTaxon = new WebTaxon();

            var createdBy = 2;
            string personName = @"Hölje Soderås";

            refTaxon.CreatedBy = createdBy;
            refTaxon.SetModifiedByPerson(personName);
            refTaxon.IsPublished = true;
            int testTaxonId = 1;
            WebTaxonName taxonName = GetReferenceTaxonName(testTaxonId);
            WebTaxonName taxonName2 = GetReferenceTaxonName(testTaxonId); 
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
            string conceptDefinitionFullGenerated = "conceptDefinitionFullGenerated" + " " + text;
            Int32 createdBy = Settings.Default.TestUserId;
            DateTime validFromDate = new DateTime(DateTime.Now.Ticks);
            DateTime validToDate = new DateTime(2022, 1, 30);
            refTaxon.IsPublished = false;
            refTaxon.PartOfConceptDefinition = conceptDefinitionPart;
            refTaxon.CreatedBy = createdBy;
            refTaxon.ValidFromDate = validFromDate;
            refTaxon.ValidToDate = validToDate;

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
            refTaxonName.SetModifiedByPerson("Test PersonName");
            refTaxonName.SetNameUsageId(0);
            refTaxonName.IsChangedInTaxonRevisionEventIdSpecified = true;
            refTaxonName.ChangedInTaxonRevisionEventId = 1;
            refTaxonName.SetVersion(1);
            return refTaxonName;
        }


        /// <summary>
        /// Create a taxon category for test.
        /// </summary>
        /// <returns></returns>
        private WebTaxonCategory GetReferenceTaxonCategory(int i)
        {
            WebTaxonCategory refTaxonCategory = new WebTaxonCategory();
            // First we create a taxon category that we later use...
            string categoryName = "Svenskt" + i;
            Int32 parentCategory = 2+i;
            Int32 sortOrder = 20 + i;
            bool mainCategory = false;
            bool taxonomic = true;
            Int32 categoryId = 1311 +i;

            refTaxonCategory.Name = categoryName;
            refTaxonCategory.Id = categoryId;
            refTaxonCategory.IsMainCategory = mainCategory;
            refTaxonCategory.ParentId = parentCategory;
            refTaxonCategory.SortOrder = sortOrder;
            refTaxonCategory.IsTaxonomic = taxonomic;

            return refTaxonCategory;
        }

        /// <summary>
        /// Create a taxon name category for test.
        /// </summary>
        /// <returns></returns>
        private WebTaxonNameCategory GetReferenceTaxonNameCategory()
        {
            WebTaxonNameCategory refTaxonNameCategory = new WebTaxonNameCategory();
            // First we create a taxon category that we later use...
            string categoryName = "Svenskt";
            string shortName = "kort namn";
            Int32 sortOrder = 20;
            TaxonNameCategoryTypeId type = TaxonNameCategoryTypeId.ScientificName;
             
            refTaxonNameCategory.Name = categoryName;
            refTaxonNameCategory.ShortName = shortName;
            refTaxonNameCategory.SortOrder = sortOrder;
            refTaxonNameCategory.TypeId = (Int32)type;
            
            return refTaxonNameCategory;
        }

        

        #endregion
    }
}

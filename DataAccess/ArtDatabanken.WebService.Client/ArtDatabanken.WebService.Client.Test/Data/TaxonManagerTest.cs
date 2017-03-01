using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ArtDatabanken.WebService.Client.PESINameService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Client.TaxonService;
using Taxon = ArtDatabanken.Data.Taxon;
using TaxonList = ArtDatabanken.Data.TaxonList;
using TaxonManager = ArtDatabanken.Data.TaxonManager;
using TaxonNameList = ArtDatabanken.Data.TaxonNameList;
using TaxonNameSearchCriteria = ArtDatabanken.Data.TaxonNameSearchCriteria;
using TaxonSearchCriteria = ArtDatabanken.Data.TaxonSearchCriteria;
using User = ArtDatabanken.Data.User;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    /// <summary>
    /// This class handles taxon manager test.
    /// </summary>
    [TestClass]
    public class TaxonManagerTest : TestBase
    {
        private ITaxonRevision _taxonRevision;
        private TaxonManager _taxonManager;

        [TestCleanup]
        public override void TestCleanup()
        {
            if (_taxonRevision.IsNotNull())
            {
                GetTaxonManager().DeleteTaxonRevision(GetUserContext(), _taxonRevision);
            }

            base.TestCleanup();
        }

        public TaxonManagerTest()
        {
            _taxonManager = null;
        }

        [TestMethod]
        public void Constructor()
        {
            TaxonManager taxonManager;

            taxonManager = new TaxonManager();
            Assert.IsNotNull(taxonManager);
        }

        [TestMethod]
        public void AddTaxonName_RevisionCheckedOut_TaxonNameAdded()
        {
            // Arrange
            int taxonId = 100130; // buskmus
            int revisionId = 1;
            var taxon = GetTaxonManager().GetTaxon(GetRevisionUserContext(), taxonId);
            var revision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);

            // Act
            ITaxonName taxonName = GetReferenceTaxonName();
            taxonName.Taxon = taxon;
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                GetTaxonManager().CreateTaxonName(GetUserContext(), revision, taxonName);
            }
        }


        [TestMethod]
        public void AddTaxonNameTwice_RevisionCheckedOut_TaxonNameAdded()
        {
            int taxonId = 100130; // buskmus
            int revisionId = 1;
            var taxon = GetTaxonManager().GetTaxon(GetRevisionUserContext(), taxonId);
            var revision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);

            // Act
            ITaxonName taxonName = GetReferenceTaxonName();
            taxonName.Taxon = taxon;
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                // 1st name add

                GetTaxonManager().CreateTaxonName(GetUserContext(), revision, taxonName);

                // 2nd name add
                taxon = GetTaxonManager().GetTaxon(GetRevisionUserContext(), taxonId);
                ITaxonName taxonName2 = GetReferenceTaxonName();
                taxonName2.Name = taxonName2.Name + "-2";
                taxonName2.Taxon = taxon;
                GetTaxonManager().CreateTaxonName(GetUserContext(), revision, taxonName2);
            }
        }

        [TestMethod]
        public void AddTaxonName_ToTestRevision()
        {
            // Arrange
            int revisionId = 1;
            var taxon = GetTaxonManager().GetTaxon(GetRevisionUserContext(), 102101);
            var revision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);

            // Act
            ITaxonName taxonName = GetReferenceTaxonName();
            taxonName.Taxon = taxon;
            // make it scientific
            taxonName.Category.Id = 0;
            taxonName.Taxon = taxon;
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                GetTaxonManager().CreateTaxonName(GetRevisionUserContext(), revision, taxonName);
            }
        }

        
        [TestMethod]
        public void EditTaxonName_SetRecommendedCommon()
        {
            ITaxonName taxonNameNewRecommended;

            int revisionId = 1;
            var revision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);

            // Act
            // "lodjur"
             taxonNameNewRecommended = GetTaxonNameByVersion(GetRevisionUserContext(), 74324);
            taxonNameNewRecommended.IsRecommended = true;
            
            GetTaxonManager().UpdateTaxonName(GetUserContext(), revision, taxonNameNewRecommended);
        }

        [TestMethod]
        public void EditTaxonName_SetRecommendedScientific()
        {
            int revisionId = 1;
            var revision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);

            // Act
            ITaxonName taxonNameNewRecommended = GetTaxonNameByVersion(GetRevisionUserContext(), 69630);
            taxonNameNewRecommended.IsRecommended = true;

            GetTaxonManager().UpdateTaxonName(GetUserContext(), revision, taxonNameNewRecommended);
        }


        [TestMethod]
        public void CheckTransactionNotTimeout()
        {
            ITaxon taxon;
            ITaxonRevisionEvent taxonRevisionEvent = null;
            // Transaction timeout set to 5 mins.
            int transactionTimeout = 300;
            using (ITransaction transaction = GetUserContext().StartTransaction(transactionTimeout))
            {
                taxon = GetReferenceTaxon2();
                GetTaxonManager(true).UpdateTaxon(GetUserContext(), taxon, taxonRevisionEvent, null, null);
                Assert.IsNotNull(taxon);

                // waitTime in ms.
                int waitTime = 120000;
                // Sleep for two minutes
                Thread.Sleep(waitTime);

                // The transaction sholud still be active.
                GetTaxonManager(true).UpdateTaxon(GetUserContext(), taxon, taxonRevisionEvent, null, null);
                Assert.IsNotNull(taxon);
            }
        }

        [TestMethod]
        public void EditTaxonName_RecommendedScientific()
        {
            // Arrange
            int taxonId = 102101;  // Sorex isodon
            int revisionId = 1;
            var taxon = GetTaxonManager().GetTaxon(GetRevisionUserContext(), taxonId);
            var revision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);

            // Act
            ITaxonName taxonName = taxon.GetScientificName(GetUserContext());
            taxonName.Name = "Sorex isodon 2";
            taxonName.Taxon = taxon;

            GetTaxonManager().UpdateTaxonName(GetUserContext(), revision, taxonName);

        }

        [TestMethod]
        public void EditTaxonName_NoChange()
        {
            Int32 taxonNameId = 140917;  // Sorex isodon
            Int32 revisionId = 1;
            ITaxonName taxonName;
            ITaxonRevision revision;

            taxonName = GetTaxonManager(true).GetTaxonName(GetUserContext(), taxonNameId);
            revision = GetTaxonManager().GetTaxonRevision(GetRevisionUserContext(), revisionId);
            GetTaxonManager().UpdateTaxonName(GetRevisionUserContext(), revision, taxonName);
        }

        [TestMethod]
        public void EditTaxonName_RevisionCheckedOut_TaxonNameChanged()
        {
            // Arrange
            // comment 2012-01-11 int taxonId = 100130; // buskmus
            int taxonId = 100130; 
            int revisionId = 1;
            var taxon = GetTaxonManager().GetTaxon(GetRevisionUserContext(), taxonId);
            var revision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);

            // Act
            String editDescription = "Edited description";
            TaxonNameList editTaxonNames = new TaxonNameList();
            ITaxonName taxonName = taxon.GetScientificName(GetUserContext());
            taxonName.Taxon = taxon;
            taxonName.Description = editDescription;

            editTaxonNames.Add(taxonName);

            GetTaxonManager().UpdateTaxonNames(GetUserContext(), revision, editTaxonNames);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EditTaxonName_ChangeCategory()
        {
            // Arrange
            int taxonId = 233624;
            int revisionId = 1;
            var taxon = GetTaxonManager().GetTaxon(GetRevisionUserContext(), taxonId);
            var revision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);

            // Act
            TaxonNameList editTaxonNames = new TaxonNameList();
            ITaxonName taxonName = taxon.GetCommonName(GetRevisionUserContext());
            // taxonName.Taxon = taxon;
            ITaxonNameCategory nameCategory = GetTaxonManager().GetTaxonNameCategory(GetUserContext(), 0);
            taxonName.Category = nameCategory;

            editTaxonNames.Add(taxonName);
            GetTaxonManager().UpdateTaxonNames(GetRevisionUserContext(), revision, editTaxonNames);

            Assert.Fail();
        }

        [TestMethod]
        public void EditTaxonNameTwice_RevisionCheckedOut_TaxonNameChanged()
        {
            // Arrange            
            int taxonId = 100130; // buskmus
            int revisionId = 1;
            var taxon = GetTaxonManager().GetTaxon(GetRevisionUserContext(), taxonId);
            var revision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);

            // Act
            String editDescription = "Edited description #1";

            ITaxonName editTaxonName = new TaxonName();
            editTaxonName.DataContext = new DataContext(GetUserContext());
            editTaxonName.IsOkForSpeciesObservation = true;
            editTaxonName.IsRecommended = true;
            editTaxonName = taxon.GetScientificName(GetUserContext());
            
            // make changes #1
            editTaxonName.Status.Id = 2;
            editTaxonName.Description = editDescription;
            editTaxonName.Taxon = taxon;

            GetTaxonManager().UpdateTaxonName(GetUserContext(), revision, editTaxonName);

            // make changes #2
            editTaxonName.Status.Id = 1;
            editDescription = "Edited description #2";
            editTaxonName.Description = editDescription;
            
            GetTaxonManager().UpdateTaxonName(GetUserContext(), revision, editTaxonName);
        }


        [TestMethod]
        public void CreateTaxon()
        {
            ITaxon taxon;

            // Login user w/ DyntaxaEditor authority NOT TaxonRevision authority
            if (!UserLogin(Settings.Default.DyntaxaWriterUserName, Settings.Default.DyntaxaWriterPassword))
            {
                Console.WriteLine("Login failed!");
            }

            int revisionId = 1;
            var revision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);
            taxon = GetReferenceTaxon();
            
            int theArtCategory = 17;
            
            ITaxonCategory taxonCategory = GetTaxonManager(true).GetTaxonCategory(GetUserContext(), theArtCategory);

            int theParentTaxonId = 1012705; //Erignathus
            
            ITaxon theParentTaxon = GetTaxonManager(true).GetTaxon(GetUserContext(), theParentTaxonId);

            string scientificName = "Erignathus barbatus";

            GetTaxonManager(true).CreateTaxon(GetRevisionUserContext(), revision, taxon, scientificName, "storsäl",
                                              "Aohlis Barbatus", 0, theParentTaxon, taxonCategory, "test");

            Assert.IsNotNull(taxon);

            // Test created by user.
            Assert.AreNotEqual(Int32.MinValue, taxon.CreatedBy);

            // Test created date.
            Assert.IsTrue((DateTime.Now - taxon.CreatedDate) <
                          new TimeSpan(0, 0, Settings.Default.ComputerTimeDifference));

            // Test DataContext.
            Assert.IsNotNull(taxon.DataContext);

            // Test GUID.
            Assert.IsTrue(taxon.Guid.IsNotEmpty());

            // Test id.
            Assert.AreNotEqual(Int32.MinValue, taxon.Id);

            // IsPublished = false
            Assert.IsFalse(taxon.IsPublished);
        }

        [TestMethod]
        public void GetTaxonAlertStatus()
        {
            ITaxonAlertStatus taxonAlertStatus;

            GetTaxonManager(true);
            foreach (TaxonAlertStatusId taxonAlertStatusId in Enum.GetValues(typeof(TaxonAlertStatusId)))
            {
                taxonAlertStatus = GetTaxonManager().GetTaxonAlertStatus(GetUserContext(), (Int32)taxonAlertStatusId);
                Assert.IsNotNull(taxonAlertStatus);
                taxonAlertStatus = GetTaxonManager().GetTaxonAlertStatus(GetUserContext(), taxonAlertStatusId);
                Assert.IsNotNull(taxonAlertStatus);
            }
        }

        [TestMethod]
        public void GetTaxonAlertStatuses()
        {
            TaxonAlertStatusList taxonAlertStatuses;

            taxonAlertStatuses = GetTaxonManager(true).GetTaxonAlertStatuses(GetUserContext());
            Assert.IsTrue(taxonAlertStatuses.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonChangeStatus()
        {
            ITaxonChangeStatus taxonChangeStatus;

            GetTaxonManager(true);
            foreach (TaxonChangeStatusId taxonChangeStatusId in Enum.GetValues(typeof(TaxonChangeStatusId)))
            {
                taxonChangeStatus = GetTaxonManager().GetTaxonChangeStatus(GetUserContext(), (Int32)taxonChangeStatusId);
                Assert.IsNotNull(taxonChangeStatus);
                taxonChangeStatus = GetTaxonManager().GetTaxonChangeStatus(GetUserContext(), taxonChangeStatusId);
                Assert.IsNotNull(taxonChangeStatus);
            }
        }

        [TestMethod]
        public void GetTaxonChangeStatuses()
        {
            TaxonChangeStatusList taxonChangeStatuses;

            taxonChangeStatuses = GetTaxonManager(true).GetTaxonChangeStatuses(GetUserContext());
            Assert.IsTrue(taxonChangeStatuses.IsNotEmpty());
        }

        [TestMethod]
        public void GetLumpSplitEventType()
        {
            ILumpSplitEventType lumpSplitEventType;

            GetTaxonManager(true);
            foreach (LumpSplitEventTypeId lumpSplitEventTypeId in Enum.GetValues(typeof(LumpSplitEventTypeId)))
            {
                lumpSplitEventType = GetTaxonManager().GetLumpSplitEventType(GetUserContext(), (Int32)lumpSplitEventTypeId);
                Assert.IsNotNull(lumpSplitEventType);
                lumpSplitEventType = GetTaxonManager().GetLumpSplitEventType(GetUserContext(), lumpSplitEventTypeId);
                Assert.IsNotNull(lumpSplitEventType);
            }
        }

        [TestMethod]
        public void GetLumpSplitEventTypes()
        {
            LumpSplitEventTypeList lumpSplitEventTypes;

            lumpSplitEventTypes = GetTaxonManager(true).GetLumpSplitEventTypes(GetUserContext());
            Assert.IsTrue(lumpSplitEventTypes.IsNotEmpty());
            foreach (ILumpSplitEventType lumpSplitEventType in lumpSplitEventTypes)
            {
                Assert.IsTrue(0 < lumpSplitEventType.Id);
                Assert.IsTrue(lumpSplitEventType.Identifier.IsNotEmpty());
            }
        }

        /// <summary>
        /// Get taxon name by version.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="version">Taxon name version.</param>
        /// <returns>Taxon name with specified version.</returns>
        private ITaxonName GetTaxonNameByVersion(IUserContext userContext,
                                                 Int32 version)
        {
            String guid;

            guid = "urn:lsid:dyntaxa.se:TaxonName:-1:" + version;
            return CoreData.TaxonManager.GetTaxonName(userContext, guid);
        }

        /// <summary>
        ///  Login with other user than the default user (testuser)
        /// </summary>
        private Boolean UserLogin (String userName, String password)
        {
            if (!GetUserContext().IsNull())
            {
                try
                {
                    CoreData.UserManager.Logout(GetUserContext());

                }
                catch
                {
                    // Test is done.
                    // We are not interested in problems that
                    // occures due to test of error handling.
                }
            }
            return Login(Settings.Default.DyntaxaWriterUserName, Settings.Default.DyntaxaWriterPassword);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateTaxon_ParentNotInRevision()
        {
            int revisionId = 1;
            var revision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);
            ITaxon taxon = GetReferenceTaxon();

            int theUnderArtCategory = 18;
            ITaxonCategory taxonCategory = GetTaxonManager(true).GetTaxonCategory(GetUserContext(), theUnderArtCategory);

            int theParentTaxonId = 200023; // Locusta migratoria - europeisk vandringsgräshoppa 
            ITaxon theParentTaxon = GetTaxonManager(true).GetTaxon(GetUserContext(), theParentTaxonId);

            GetTaxonManager(true).CreateTaxon(GetRevisionUserContext(), revision, taxon, "TEST", "TEST", "Author", 0, theParentTaxon, taxonCategory, "TEST");

            Assert.Fail();
        }

        [TestMethod]
        public void EditTaxonMultipleTimes()
        {
            // Arrange
            int taxonId = 2002171;
            int revisionId = 1;
            var taxon = GetTaxonManager().GetTaxon(GetUserContext(), taxonId);

            var revision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);
            String conceptDefinitionPartString = "Testar";
            int taxonPropertiesCount = taxon.GetTaxonProperties(GetUserContext()).Count();

            // Act
            GetTaxonManager().UpdateTaxon(GetRevisionUserContext(), taxon, revision, conceptDefinitionPartString, taxon.Category, 0, false);
            GetTaxonManager().UpdateTaxon(GetRevisionUserContext(), taxon, revision, conceptDefinitionPartString, taxon.Category, 0, false);
            GetTaxonManager().UpdateTaxon(GetRevisionUserContext(), taxon, revision, conceptDefinitionPartString, taxon.Category, 0, false);
            taxon = GetTaxonManager().GetTaxon(GetRevisionUserContext(), taxonId);

            // Assert
            Assert.AreEqual(taxon.PartOfConceptDefinition, conceptDefinitionPartString);
        }


        [TestMethod]
        public void CanLoad223597()
        {
            // Arrange
            int taxonId = 223597;
                        
            // Act
            var taxon = GetTaxonManager().GetTaxon(GetUserContext(), taxonId);

            // Assert
            Assert.IsTrue(taxon.ScientificName.IsNotEmpty());
            Assert.IsTrue(taxon.GetConceptDefinition(GetUserContext()) != string.Empty);
        }


        [TestMethod]
        public void EditTaxon()
        {
            // Arrange
            int taxonId = 246283;
            int revisionId = 1;

            var taxon = GetTaxonManager().GetTaxon(GetRevisionUserContext(), taxonId);

            var parents = taxon.GetAllParentTaxonRelations(GetRevisionUserContext(), null, true, false, true);

            var closeparents = taxon.GetParentTaxonRelations(GetRevisionUserContext(), true, false, true);

            var revision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);
            String conceptDefinitionPartString = "Testar2";
            int taxonPropertiesCount = taxon.GetTaxonProperties(GetUserContext()).Count();

            // Act
            GetTaxonManager().UpdateTaxon(GetRevisionUserContext(),taxon, revision, conceptDefinitionPartString, taxon.GetCheckedOutChangesTaxonProperties(GetUserContext()).TaxonCategory, 0, false);
            taxon = GetTaxonManager().GetTaxon(GetRevisionUserContext(), taxonId);

            // Assert
            Assert.AreEqual(taxon.PartOfConceptDefinition, conceptDefinitionPartString);
        }

        [TestMethod]
        public void CanUpdateTaxonProperty_ConceptDefinitionString()
        {
            // Arrange
            var taxon = GetReferenceTaxon();
            ITaxonRevisionEvent taxonRevisionEvent = null;
            
            // Act
            taxon.PartOfConceptDefinition = "Updated!";
            GetTaxonManager().UpdateTaxon(GetUserContext(), taxon, taxonRevisionEvent, null, null);

            // Assert
            Assert.AreEqual("Updated!", taxon.PartOfConceptDefinition);
        }

        [TestMethod]
        public void CanSaveNewRevision()
        {
            // Arrange
            var revision = new TaxonRevision();
            revision.RootTaxon = CoreData.TaxonManager.GetTaxon(GetUserContext(), (Int32)TaxonId.Hedgehog);
            revision.CreatedBy = Settings.Default.TestUserId;
            revision.State = new TaxonRevisionState() { Id = 1, Identifier = TaxonRevisionStateId.Created.ToString()};
            revision.ExpectedEndDate = DateTime.Now;
            revision.ExpectedStartDate = DateTime.Now;
            

            // Act
            GetTaxonManager().UpdateTaxonRevision(GetUserContext(), revision);

            // Assert
            Assert.IsTrue(revision.Id > 0);
            Assert.AreNotEqual(revision.Guid, string.Empty);
        }

        [TestMethod]
        public void CanAddRevisionEventToRevision()
        {
            ITaxonRevision revision = new TaxonRevision();

            try
            {
                // Arrange
                revision.RootTaxon = GetTaxonManager(true).GetTaxon(GetUserContext(), (Int32)TaxonId.Wolverine);
                revision.State = GetTaxonManager().GetTaxonRevisionState(GetUserContext(), TaxonRevisionStateId.Created);
                revision.ExpectedEndDate = DateTime.Now;
                revision.ExpectedStartDate = DateTime.Now;
                revision.CreatedBy = GetUserContext().User.Id;
                revision.CreatedDate = DateTime.Now;
                revision.SetRevisionEvents(new List<ITaxonRevisionEvent>());

                GetTaxonManager().UpdateTaxonRevision(GetUserContext(), revision);

                // Act
                revision.GetRevisionEvents(GetUserContext()).Add(new TaxonRevisionEvent() { CreatedBy = GetUserContext().User.Id, CreatedDate = DateTime.Now, Type = new TaxonRevisionEventType() { Id = 1 }, RevisionId = revision.Id });
                GetTaxonManager().UpdateTaxonRevision(GetUserContext(), revision);

                // Assert
                Assert.IsTrue(revision.GetRevisionEvents(GetUserContext()).Count == 1);
            }
            finally
            {
                GetTaxonManager().DeleteTaxonRevision(GetUserContext(), revision);
            }
        }

        [TestMethod]
        public void CanUpdateExistingRevision()
        {
            ITaxonRevision taxonRevision;

            // Arrange
            taxonRevision = new TaxonRevision();
            try
            {
                taxonRevision.RootTaxon = CoreData.TaxonManager.GetTaxon(GetUserContext(), (Int32)TaxonId.DrumGrasshopper);
                taxonRevision.CreatedBy = Settings.Default.TestUserId;
                taxonRevision.State = new TaxonRevisionState() { Id = 1, Identifier = TaxonRevisionStateId.Created.ToString() };
                taxonRevision.ExpectedEndDate = DateTime.Now;
                taxonRevision.ExpectedStartDate = DateTime.Now;
                GetTaxonManager().UpdateTaxonRevision(GetUserContext(), taxonRevision);

                // Act
                taxonRevision.State = new TaxonRevisionState() { Id = 2, Identifier = TaxonRevisionStateId.Ongoing.ToString() };
                GetTaxonManager().UpdateTaxonRevision(GetUserContext(), taxonRevision);

                // Assert
                Assert.IsNotNull(taxonRevision);
                Assert.AreEqual(taxonRevision.State.Id, 2);
            }
            finally 
            {
                CoreData.TaxonManager.DataSource.DeleteTaxonRevision(GetUserContext(), taxonRevision);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ChangeTaxonCategory_RevisionNotCheckedOut_ExceptionThrown()
        {
            // Arrange
            _taxonRevision = new TaxonRevision();
            var taxon = GetTaxonManager().GetTaxon(GetUserContext(), 2000448);
            _taxonRevision.RootTaxon = taxon;
            _taxonRevision.State = new TaxonRevisionState() { Id = 1, Identifier = TaxonRevisionStateId.Created.ToString() };
            _taxonRevision.ExpectedEndDate = DateTime.Now;
            _taxonRevision.ExpectedStartDate = DateTime.Now;
            _taxonRevision.CreatedBy = GetUserContext().User.Id;
            _taxonRevision.CreatedDate = DateTime.Now;
            GetTaxonManager().UpdateTaxonRevision(GetUserContext(), _taxonRevision);
            var revisionId = _taxonRevision.Id;

            // Act
            var newCategory = GetTaxonManager().GetTaxonCategory(GetUserContext(), 3);
            GetTaxonManager().UpdateTaxon(GetUserContext(), taxon, _taxonRevision, newCategory);

            Assert.Fail();
         }

        [TestMethod]
        public void ResortTaxonTree()
        {
            Int32 revisionId = 1;
            Int32 taxonIdParent = 267320;
            ITaxon parentTaxon;
            ITaxonRevision taxonRevision;
            List<Int32> taxonIdChildList = new List<Int32>();
            TaxonRelationList taxaRelationList;

            // Do the sorting
            taxonIdChildList.Add(100024);  // Varg
            taxonIdChildList.Add(233621);  // Hund
            taxonRevision = GetTaxonManager(true).GetTaxonRevision(GetRevisionUserContext(), revisionId);
            GetTaxonManager().UpdateTaxonTreeSortOrder(GetRevisionUserContext(), taxonIdChildList, taxonIdParent, taxonRevision);

            // Check result
            parentTaxon = GetTaxonManager().GetTaxon(GetRevisionUserContext(), taxonIdParent);
            taxaRelationList = parentTaxon.GetNearestChildTaxonRelations(GetRevisionUserContext());
            Assert.IsTrue(taxaRelationList.IsNotEmpty());
            Assert.IsTrue(taxaRelationList[0].ParentTaxon.Id.Equals(267320));
            Assert.IsTrue(taxaRelationList[1].ParentTaxon.Id.Equals(267320));
        }

        [TestMethod]
        public void ResortTaxon_ChangeEverything_SortOrderChanged()
        {
            // Arrange
            int revisionId = 1;
            ITaxonRevision taxonRevision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);

            List<Int32> taxa = new List<int>();
            taxa.Add(248241);
            taxa.Add(248239);
            taxa.Add(248240);

            // Act
            GetTaxonManager().UpdateTaxonTreeSortOrder(GetRevisionUserContext(), taxa, 1013321, taxonRevision);

            // Assert
            var parentTaxon = GetTaxonManager().GetTaxon(GetRevisionUserContext(), 1013321);

            Assert.IsTrue(parentTaxon.GetNearestChildTaxonRelations(GetUserContext()).Count == 3);
            Assert.IsTrue(parentTaxon.GetNearestChildTaxonRelations(GetUserContext()).Count == 3);
            Assert.IsTrue(parentTaxon.GetNearestChildTaxonRelations(GetUserContext())[0].ChildTaxon.Id == 248241);
            Assert.IsTrue(parentTaxon.GetNearestChildTaxonRelations(GetUserContext())[1].ChildTaxon.Id == 248239);
            Assert.IsTrue(parentTaxon.GetNearestChildTaxonRelations(GetUserContext())[2].ChildTaxon.Id == 248240);
        }

        [TestMethod]
        public void ResortTaxon_ChangeEverythingTwice_SortOrderChanged()
        {
            // Arrange
            int revisionId = 1;
            ITaxonRevision taxonRevision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);

            List<Int32> firstSortOrder = new List<Int32>();
            firstSortOrder.Add(248241);
            firstSortOrder.Add(248239);
            firstSortOrder.Add(248240);

            List<Int32> secondSortOrder = new List<Int32>();
            secondSortOrder.Add(248239);
            secondSortOrder.Add(248241);
            secondSortOrder.Add(248240);

            // Act
            GetTaxonManager().UpdateTaxonTreeSortOrder(GetRevisionUserContext(), firstSortOrder, 1013321, taxonRevision);
            GetTaxonManager().UpdateTaxonTreeSortOrder(GetRevisionUserContext(), secondSortOrder, 1013321, taxonRevision);

            // Assert
            ITaxon parentTaxon = GetTaxonManager().GetTaxon(GetRevisionUserContext(), 1013321);
            Assert.IsTrue(parentTaxon.GetNearestChildTaxonRelations(GetUserContext()).Count == 3);
            Assert.IsTrue(parentTaxon.GetNearestChildTaxonRelations(GetUserContext()).Count == 3);
            Assert.IsTrue(parentTaxon.GetNearestChildTaxonRelations(GetUserContext())[0].ChildTaxon.Id == secondSortOrder[0]);
            Assert.IsTrue(parentTaxon.GetNearestChildTaxonRelations(GetUserContext())[1].ChildTaxon.Id == secondSortOrder[1]);
            Assert.IsTrue(parentTaxon.GetNearestChildTaxonRelations(GetUserContext())[2].ChildTaxon.Id == secondSortOrder[2]);
        }

        [TestMethod]
        public void ParentRelationsAreLazyLoaded_ScientificNameSet()
        {
            // Arrange
            var childTaxon = GetTaxonManager().GetTaxon(GetUserContext(), 2002138);
                
            // Act
            var parentTaxon = childTaxon.GetNearestParentTaxonRelations(GetUserContext());

            // Assert
            Assert.IsTrue(parentTaxon.First().ParentTaxon.ScientificName.IsNotEmpty());
        }


        [TestMethod]
        public void CanSplitTaxonIntoTwoNew()
        {

            // Arrange
            int revisionId = 1;

            var revision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);
/*           
            var taxonBefore = GetTaxonManager().GetTaxonById(GetUserContext(), 248239);
            var taxonAfter1 = GetTaxonManager().GetTaxonById(GetUserContext(), 248240);
            var taxonAfter2 = GetTaxonManager().GetTaxonById(GetUserContext(), 248241);
*/
            // splitta Storsälen till Mellansälen och Lillsälen
 
            var taxonBefore = GetTaxonManager().GetTaxon(GetRevisionUserContext(revisionId), 246126);
            var taxonAfter1 = GetTaxonManager().GetTaxon(GetRevisionUserContext(revisionId), 6000469);
            var taxonAfter2 = GetTaxonManager().GetTaxon(GetRevisionUserContext(revisionId), 6000470);

                // Act
            TaxonList taxaAfter = new TaxonList();
            taxaAfter.Add(taxonAfter1);
            taxaAfter.Add(taxonAfter2);
            GetTaxonManager().SplitTaxon(GetRevisionUserContext(), taxonBefore, taxaAfter, revision);

            // Assert
            //// Assert.IsFalse(taxonBefore.CheckedOutChangesTaxonProperties.IsValid);
            Assert.IsTrue(GetTaxonManager().GetLumpSplitEventsByNewReplacingTaxon(GetUserContext(), taxaAfter[0])[0].TaxonBefore.Id == taxonBefore.Id);
            Assert.IsTrue(GetTaxonManager().GetLumpSplitEventsByNewReplacingTaxon(GetUserContext(), taxaAfter[1])[0].TaxonBefore.Id == taxonBefore.Id);
        }

        [TestMethod]
        public void CanSplitTaxonIntoTwoNewAndOneExisting()
        {
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                // Arrange
                var revision = new TaxonRevision();
                revision.RootTaxon = GetTaxonManager().GetTaxon(GetUserContext(), 2000448);
                revision.State = new TaxonRevisionState() { Id = 1, Identifier = TaxonRevisionStateId.Created.ToString() };
                revision.ExpectedEndDate = DateTime.Now;
                revision.ExpectedStartDate = DateTime.Now;
                revision.CreatedBy = GetUserContext().User.Id;
                revision.CreatedDate = DateTime.Now;
                revision.SetReferences(new List<IReferenceRelation>());
                revision.SetRevisionEvents(new List<ITaxonRevisionEvent>());
                GetTaxonManager().UpdateTaxonRevision(GetUserContext(), revision);
                var revisionId = revision.Id;
                GetTaxonManager().CheckOutTaxonRevision(GetUserContext(), revision);

                var taxonBefore = GetTaxonManager().GetTaxon(GetUserContext(), 248239);
                var taxonAfter1 = GetTaxonManager().GetTaxon(GetUserContext(), 248240);
                var taxonAfter2 = GetTaxonManager().GetTaxon(GetUserContext(), 248241);

                // Act
                TaxonList taxaAfter = new TaxonList();
                taxaAfter.Add(taxonAfter1);
                taxaAfter.Add(taxonAfter2);
                taxaAfter.Add(taxonBefore);
                GetTaxonManager().SplitTaxon(GetUserContext(), taxonBefore, taxaAfter, revision);

                // Assert
                Assert.IsTrue(taxonBefore.GetCheckedOutChangesTaxonProperties(GetUserContext()).IsValid);
                Assert.IsTrue(GetTaxonManager().GetLumpSplitEventsByNewReplacingTaxon(GetUserContext(), taxaAfter[0])[0].TaxonBefore.Id == taxonBefore.Id);
                Assert.IsTrue(GetTaxonManager().GetLumpSplitEventsByNewReplacingTaxon(GetUserContext(), taxaAfter[1])[0].TaxonBefore.Id == taxonBefore.Id);
                Assert.IsTrue(GetTaxonManager().GetLumpSplitEventsByNewReplacingTaxon(GetUserContext(), taxaAfter[2])[0].TaxonBefore.Id == taxonBefore.Id);
            }
        }

        [TestMethod]
        public void CanLumpTaxon()
        {
            // Arrange
            int revisionId = 1;
            var revision = GetTaxonManager().GetTaxonRevision(GetRevisionUserContext(revisionId), revisionId);

            var taxonBefore = GetTaxonManager().GetTaxon(GetRevisionUserContext(), 1001643);
            var taxonAfter = GetTaxonManager().GetTaxon(GetRevisionUserContext(), 1001644);

            // Act
            TaxonList taxaBefore = new TaxonList();
            taxaBefore.Add(taxonBefore);
            GetTaxonManager().LumpTaxon(GetRevisionUserContext(), taxaBefore, taxonAfter, revision);

            // Assert
            var taxon = GetTaxonManager().GetTaxon(GetRevisionUserContext(), 1001644);
            Assert.AreEqual(2, taxon.GetNearestChildTaxonRelations(GetUserContext()).Count);
        }


        [TestMethod]
        public void TryCanLumpTwoTaxon()
        {
            // Arrange
            int revisionId = 1;
            var revision = GetTaxonManager().GetTaxonRevision(GetRevisionUserContext(revisionId), revisionId);

            var t = revision.GetRevisionEvents(GetUserContext());

            var taxonBefore = GetTaxonManager().GetTaxon(GetRevisionUserContext(revisionId), 246126); // storsäl
            var secondTaxonBefore = GetTaxonManager().GetTaxon(GetRevisionUserContext(revisionId), 6000641); // mellansäl
            var taxonAfter = GetTaxonManager().GetTaxon(GetRevisionUserContext(revisionId), 6000642); // klumpsäl   

            // Act
            var taxaBefore = new TaxonList();
            taxaBefore.Add(taxonBefore);
            taxaBefore.Add(secondTaxonBefore);
            var canLumpTaxon = GetTaxonManager().IsOkToLumpTaxa(GetRevisionUserContext(revisionId), taxaBefore, taxonAfter);
            GetTaxonManager().LumpTaxon(GetRevisionUserContext(revisionId), taxaBefore, taxonAfter, revision);

            // Assert
            Assert.IsTrue(canLumpTaxon);
 
        }

        [TestMethod]
        public void CanLumpTwoTaxon()
        {
            // Arrange
            int revisionId = 1;
            var revision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);

            var taxonBefore = GetTaxonManager().GetTaxon(GetRevisionUserContext(), 263710); // 
            var secondTaxonBefore = GetTaxonManager().GetTaxon(GetRevisionUserContext(), 263711); // 
            var taxonAfter = GetTaxonManager().GetTaxon(GetRevisionUserContext(), 6000468); // supersäl

            // Act
            TaxonList taxaBefore = new TaxonList();
            taxaBefore.Add(taxonBefore);
            taxaBefore.Add(secondTaxonBefore);
            GetTaxonManager().LumpTaxon(GetRevisionUserContext(), taxaBefore, taxonAfter, revision);

            // Assert
            Assert.IsFalse(taxonBefore.GetCheckedOutChangesTaxonProperties(GetUserContext()).IsValid);
            Assert.IsTrue(GetTaxonManager().GetLumpSplitEventsByOldReplacedTaxon(GetRevisionUserContext(), taxonBefore).Last().TaxonAfter.Id == taxonAfter.Id);
            Assert.IsTrue(GetTaxonManager().GetLumpSplitEventsByOldReplacedTaxon(GetRevisionUserContext(), secondTaxonBefore).Last().TaxonAfter.Id == taxonAfter.Id);
            Assert.IsTrue(GetTaxonManager().GetLumpSplitEventsByNewReplacingTaxon(GetUserContext(), taxonAfter).Count == 2);
            foreach (ILumpSplitEvent lumpSplitEvent in GetTaxonManager().GetLumpSplitEventsByNewReplacingTaxon(GetUserContext(), taxonAfter))
            {
                Assert.IsTrue(lumpSplitEvent.TaxonBefore.Id == taxonBefore.Id ||
                              lumpSplitEvent.TaxonBefore.Id == secondTaxonBefore.Id);
            }

        }

        [TestMethod]
        public void AddAndRemoveTaxonParent()
        {
            // Arrange
            int revisionId = 1;
            var revision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);

            ITaxon taxon = GetTaxonManager().GetTaxon(GetUserContext(), 206002); // långörad fladdermus
            ITaxon newParent = GetTaxonManager().GetTaxon(GetUserContext(), 1001625);  // Barbastella Gray

            // Act -- add new parent
//            GetTaxonManager().MoveTaxon(GetUserContext(), taxon, null, newParent, revision);

            // Assert
            taxon = GetTaxonManager().GetTaxon(GetRevisionUserContext(), 206002);
            // Oklart vad vi ska kolla på för lista -- GuNy 2012-01-17
            Assert.IsTrue(taxon.GetNearestParentTaxonRelations(GetUserContext()).Count > 0);

           // Act -- delete the parent
           GetTaxonManager().MoveTaxon(GetUserContext(), taxon, newParent, null, revision);

           // Assert
           taxon = GetTaxonManager().GetTaxon(GetRevisionUserContext(), 206002);
           // Oklart vad vi ska kolla på för lista -- GuNy 2012-01-17
           Assert.IsTrue(taxon.GetNearestParentTaxonRelations(GetUserContext()).Count > 0);
           // ... check the Barbastella Gray
           // Assert.IsTrue(taxon.ParentTaxa[0].RelatedTaxon.Id == newParent.Id);
           Assert.IsTrue(taxon.GetNearestParentTaxonRelations(GetUserContext())[0].ChangedInTaxonRevisionEventId.HasValue);
//           Assert.IsTrue(taxon.GetNearestParentTaxonRelations(GetUserContext())[0].ReplacedInTaxonRevisionEventId.HasValue);
       }

       [TestMethod]
       public void MoveTaxon_CategoryIsAllowed_TaxonMoved()
       {
           // Arrange
           int revisionId = 1;
           var revision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);

           ITaxon taxon = GetTaxonManager().GetTaxon(GetUserContext(), 206002); // långörad fladdermus
           ITaxon previousParent = GetTaxonManager().GetTaxon(GetUserContext(), 1001626); // Plecotus 
           ITaxon newParent = GetTaxonManager().GetTaxon(GetUserContext(), 1001625);  // Barbastella Gray

           // Act
           GetTaxonManager().MoveTaxon(GetUserContext(), taxon, previousParent, newParent, revision);

           // Assert
           taxon = GetTaxonManager().GetTaxon(GetUserContext(), 206002);
           Assert.IsTrue(taxon.GetNearestParentTaxonRelations(GetUserContext()).Count > 0);
           /* Oklart vad vi ska kolla på för lista -- GuNy 2012-01-17
           Assert.IsTrue(taxon.ParentTaxa[1].RelatedTaxon.Id == previousParent.Id);
           Assert.IsTrue(taxon.ParentTaxa[1].ChangedInRevisionEvent.Id.IsNotNull());
           Assert.IsTrue(taxon.ParentTaxa[0].RelatedTaxon.Id == newParent.Id);
           Assert.IsTrue(taxon.ParentTaxa[0].RevisionEvent.Id.IsNotNull());
            */
        }

        [TestMethod]
        public void MoveAllChildTaxa_ChildTaxaShouldBeEmpty()
        {
            // Arrange
           int revisionId = 1;
           var revision = GetTaxonManager().GetTaxonRevision(GetRevisionUserContext(), revisionId);

           ITaxon previousParent = GetTaxonManager().GetTaxon(GetRevisionUserContext(), 1001643);

           TaxonList taxa = new TaxonList();
           foreach (ITaxonRelation child in previousParent.GetNearestChildTaxonRelations(GetUserContext()))
            {
                taxa.Add(GetTaxonManager().GetTaxon(GetRevisionUserContext(), child.ChildTaxon.Id));     
            }

            ITaxon newParent = GetTaxonManager().GetTaxon(GetRevisionUserContext(), 1001644);

           // Act
            GetTaxonManager().MoveTaxa(GetRevisionUserContext(), taxa, previousParent, newParent, revision);

           // Assert
            ITaxon taxon = GetTaxonManager().GetTaxon(GetRevisionUserContext(), 1001643); // Barbastella Gray
            Assert.IsTrue(taxon.GetNearestChildTaxonRelations(GetUserContext()).Count == 0);
        }


       [TestMethod]
       public void MoveTaxa()
       {
           // Arrange
           int revisionId = 1;
           var revision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);

           TaxonList taxa = new TaxonList();
           taxa.Add(GetTaxonManager().GetTaxon(GetUserContext(), 206002)); // långörad fladdermus
           taxa.Add(GetTaxonManager().GetTaxon(GetUserContext(), 232267)); 

           ITaxon previousParent = GetTaxonManager().GetTaxon(GetUserContext(), 1001626); // Plecotus 
           ITaxon newParent = GetTaxonManager().GetTaxon(GetUserContext(), 1001625);  // Barbastella Gray

           // Act
           GetTaxonManager().MoveTaxa(GetUserContext(), taxa, previousParent, newParent, revision);

           // Assert
           ITaxon taxon = GetTaxonManager().GetTaxon(GetRevisionUserContext(), 1001625); // Barbastella Gray
           Assert.IsTrue(taxon.GetNearestChildTaxonRelations(GetUserContext()).Count > 0);
       }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void MoveTaxon_CategoryIsNotAllowed_ExceptionThrown()
        {
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                // Arrange
                var revision = new TaxonRevision();
                revision.RootTaxon = GetTaxonManager().GetTaxon(GetUserContext(), 2000448);
                revision.State = new TaxonRevisionState() { Id = 1, Identifier = TaxonRevisionStateId.Created.ToString() };
                revision.ExpectedEndDate = DateTime.Now;
                revision.ExpectedStartDate = DateTime.Now;
                revision.CreatedBy = GetUserContext().User.Id;
                revision.CreatedDate = DateTime.Now;
                revision.SetReferences(new List<IReferenceRelation>());
                revision.SetRevisionEvents(new List<ITaxonRevisionEvent>());
                GetTaxonManager().UpdateTaxonRevision(GetUserContext(), revision);
                GetTaxonManager().CheckOutTaxonRevision(GetUserContext(), revision);

                var taxon = GetTaxonManager().GetTaxon(GetUserContext(), 248239);
                var previousParent = GetTaxonManager().GetTaxon(GetUserContext(), 1013321);
                var newParent = GetTaxonManager().GetTaxon(GetUserContext(), 248240);

                // Act
                try
                {
                    GetTaxonManager().MoveTaxon(GetUserContext(), taxon, previousParent, newParent, revision);
                }
                catch (Exception exception)
                {
                    throw exception;
                }

                // Assert
                Assert.Fail();
            }
        }

        [TestMethod]
        public void CanFilterTaxonNames_AllIcelandic()
        {
            // Arrange
            var taxon = GetTaxonManager().GetTaxon(GetUserContext(), 2000448);

            // Act
            var filteredNameList = taxon.GetTaxonNamesBySearchCriteria(GetUserContext(), 6, null, null, null, false, true);

            // Assert
            Assert.IsTrue(filteredNameList.Count == 0);
        }

        [TestMethod]
        public void CanFilterTaxonNames_AllPublished()
        {
            // Arrange
            var taxon = GetTaxonManager().GetTaxon(GetUserContext(), 2000448);

            // Act
            var filteredNameList = taxon.GetTaxonNamesBySearchCriteria(GetUserContext(), null, null, null, null, false, true);

            // Assert
            Assert.IsTrue(filteredNameList.Count > 0);
        }
        
        [TestMethod]
        public void ChangeTaxonCategory_RevisionCheckedOut_CategoryChanged()
        {
            // Arrange
            var revision = GetTaxonManager().GetTaxonRevision(GetUserContext(), 1);
            var taxon = GetTaxonManager().GetTaxon(GetUserContext(), 2000448);

            // Act
            var taxonPropertiesCount = taxon.GetTaxonProperties(GetUserContext()).Count;
            var newCategory = GetTaxonManager().GetTaxonCategory(GetRevisionUserContext(), 3);
            GetTaxonManager().UpdateTaxon(GetRevisionUserContext(), taxon, revision, newCategory);

            // Assert
            Assert.AreEqual(taxonPropertiesCount + 1, taxon.GetTaxonProperties(GetUserContext()).Count);
            Assert.AreNotEqual(newCategory, taxon.Category.Id);
            Assert.AreEqual(newCategory.Id, taxon.GetCheckedOutChangesTaxonProperties(GetUserContext()).TaxonCategory.Id);
        }

        [TestMethod]
        public void ChangeCategory_RevisionCheckedIn_ChangesVisible()
        {
            // Arrange
            var revision = new TaxonRevision();
            var taxon = GetTaxonManager().GetTaxon(GetUserContext(), 2000440);
            revision.RootTaxon = taxon;
            revision.State = new TaxonRevisionState() { Id = 1, Identifier = TaxonRevisionStateId.Created.ToString() };
            revision.ExpectedEndDate = DateTime.Now;
            revision.ExpectedStartDate = DateTime.Now;
            revision.CreatedBy = GetUserContext().User.Id;
            revision.CreatedDate = DateTime.Now;
            GetTaxonManager().UpdateTaxonRevision(GetUserContext(), revision);
            var revisionId = revision.Id;
            GetTaxonManager().CheckOutTaxonRevision(GetUserContext(), revision);

            var taxonPropertiesCount = taxon.GetTaxonProperties(GetUserContext()).Count;
            var newCategory = GetTaxonManager().GetTaxonCategory(GetUserContext(), 3);
            GetTaxonManager().UpdateTaxon(GetUserContext(), taxon, revision, newCategory);

            // Act
            GetTaxonManager().CheckInTaxonRevision(GetUserContext(), revision);
            taxon = GetTaxonManager().GetTaxon(GetUserContext(), taxon.Id);

            // Assert
            Assert.AreEqual(taxonPropertiesCount + 1, taxon.GetTaxonProperties(GetUserContext()).Count);
            Assert.AreEqual(newCategory.Id, taxon.Category.Id);
            Assert.AreEqual(newCategory.Id, taxon.GetCheckedOutChangesTaxonProperties(GetUserContext()).TaxonCategory.Id);
        }

        [TestMethod]
        public void ChangeCategory_RevisionEventUndone_ChangesRolledBack()
        {
            ITaxonRevision revision = new TaxonRevision();
            try
            {
                // Arrange
                var taxon = GetTaxonManager(true).GetTaxon(GetRevisionUserContext(), 2000400);
                var oldCategory = taxon.GetTaxonProperties(GetRevisionUserContext()).Last().TaxonCategory;
                revision.RootTaxon = taxon;
                revision.State = new TaxonRevisionState() { Id = 1, Identifier = TaxonRevisionStateId.Created.ToString() };
                revision.ExpectedEndDate = DateTime.Now;
                revision.ExpectedStartDate = DateTime.Now;
                revision.CreatedBy = GetUserContext().User.Id;
                revision.CreatedDate = DateTime.Now;
                GetTaxonManager().UpdateTaxonRevision(GetRevisionUserContext(), revision);
                var revisionId = revision.Id;
                GetTaxonManager().CheckOutTaxonRevision(GetRevisionUserContext(), revision);

                var taxonPropertiesCount = taxon.GetTaxonProperties(GetRevisionUserContext()).Count;
                var newCategory = GetTaxonManager().GetTaxonCategory(GetRevisionUserContext(), 3);
                GetTaxonManager().UpdateTaxon(GetRevisionUserContext(), taxon, revision, newCategory);

                // Act
                GetTaxonManager().DeleteTaxonRevisionEvent(GetRevisionUserContext(), revision.GetRevisionEvents(GetRevisionUserContext()).Last(), revision);
                taxon = GetTaxonManager().GetTaxon(GetRevisionUserContext(), taxon.Id);

                // Assert
                Assert.AreEqual(taxonPropertiesCount, taxon.GetTaxonProperties(GetRevisionUserContext()).Count);
                Assert.AreEqual(oldCategory.Id, taxon.Category.Id);
                Assert.AreEqual(oldCategory.Id, taxon.GetCheckedOutChangesTaxonProperties(GetRevisionUserContext()).TaxonCategory.Id);
            }
            finally
            {
                // Clean up.
                GetTaxonManager().DataSource.DeleteTaxonRevision(GetRevisionUserContext(), revision);
            }
        }

        [TestMethod]
        public void ChangeCategoryTwice_RevisionCheckedIn_ChangesVisible()
        {
            // Arrange
            var revision = GetTaxonManager().GetTaxonRevision(GetRevisionUserContext(), 1);
            var taxon = GetTaxonManager().GetTaxon(GetRevisionUserContext(), 1001660);

            var taxonPropertiesCount = taxon.GetTaxonProperties(GetUserContext()).Count;
            var newCategory = GetTaxonManager().GetTaxonCategory(GetRevisionUserContext(), 3);
            GetTaxonManager().UpdateTaxon(GetRevisionUserContext(), taxon, revision, newCategory);

            taxonPropertiesCount = taxon.GetTaxonProperties(GetUserContext()).Count;
            newCategory = GetTaxonManager().GetTaxonCategory(GetRevisionUserContext(), 4);
            GetTaxonManager().UpdateTaxon(GetRevisionUserContext(), taxon, revision, newCategory);

            // Act
            //GetTaxonManager().RevisionCheckIn(GetUserContext(), revision);
            //taxon = GetTaxonManager().GetTaxonById(GetUserContext(), taxon.Id);

            // Assert
            //Assert.AreEqual(taxonPropertiesCount + 1, taxon.TaxonProperties.Count);
            //Assert.AreEqual(newCategory.Id, taxon.CurrentTaxonProperties.TaxonCategory.Id);
            //Assert.AreEqual(newCategory.Id, taxon.CheckedOutChangesTaxonProperties.TaxonCategory.Id);
        }

        [TestMethod]
        public void CreateTaxon_RevisionCheckedOut_TaxonCreated()
        {
            // Arrange
            var revision = new TaxonRevision();
            var taxon = GetTaxonManager().GetTaxon(GetUserContext(), 2000320);
            revision.RootTaxon = taxon;
            revision.State = new TaxonRevisionState() { Id = 1, Identifier = TaxonRevisionStateId.Created.ToString() };
            revision.ExpectedEndDate = DateTime.Now;
            revision.ExpectedStartDate = DateTime.Now;
            revision.CreatedBy = GetUserContext().User.Id;
            revision.CreatedDate = DateTime.Now;
            revision.SetReferences(new List<IReferenceRelation>());
            revision.SetRevisionEvents(new List<ITaxonRevisionEvent>());
            GetTaxonManager().UpdateTaxonRevision(GetUserContext(), revision);
            GetTaxonManager().CheckOutTaxonRevision(GetUserContext(), revision);

            // Act
            var newCategory = GetTaxonManager().GetTaxonCategory(GetRevisionUserContext(), 3);

            var newTaxon = new Taxon();
            newTaxon.Id = Int32.MinValue;
            newTaxon.DataContext = new DataContext(GetRevisionUserContext(revision.Id));

            // TaxonProperties
            newTaxon.SetTaxonProperties(new List<ITaxonProperties>());
            newTaxon.GetTaxonProperties(GetUserContext()).Add(new TaxonProperties()
                                                {
                                                    DataContext = new DataContext(GetRevisionUserContext(revision.Id)),
                                                    IsValid = true,
                                                    TaxonCategory = GetTaxonManager().GetTaxonCategory(GetUserContext(), 3),
                                                    Taxon = newTaxon,
                                                    ModifiedBy = GetUserContext().User,
                                                    ModifiedDate = DateTime.Now,
                                                    ValidFromDate = DateTime.Now,
                                                    ValidToDate = new DateTime(2022, 1, 30)
                                                });

            // TaxonNames
            // TODO Fix TaxonNames 

            // TaxonRelation
            newTaxon.SetParentTaxa(new TaxonRelationList());
            newTaxon.GetNearestParentTaxonRelations(GetUserContext()).Add(new TaxonRelation() { ParentTaxon = taxon, ValidFromDate = DateTime.Now, ValidToDate = new DateTime(2022, 1, 30) });
            GetTaxonManager().CreateTaxon(GetRevisionUserContext(revision.Id), revision, newTaxon, string.Empty, string.Empty, string.Empty,0, null, null, string.Empty);

            // Assert
            Assert.IsTrue(newTaxon.Id > 0);
            Assert.IsTrue(newTaxon.GetNearestParentTaxonRelations(GetUserContext()).Count == 1);
            Assert.IsNotNull(newTaxon.Category.Id);
            
        }

        [TestMethod]
        public void RemoveTaxon_RevisionCheckedOut_TaxonRemoved()
        {
            // Arrange
            int revisionId = 1;
            var revision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);
            var taxon = GetTaxonManager().GetTaxon(GetUserContext(), 100005);

            // Act
            var taxonPropertiesCount = taxon.GetTaxonProperties(GetUserContext()).Count;
            GetTaxonManager().DeleteTaxon(GetUserContext(), taxon, revision);

            // Assert
            Assert.AreEqual(taxonPropertiesCount + 1, taxon.GetTaxonProperties(GetUserContext()).Count);
            Assert.IsTrue(taxon.IsValid);
            Assert.IsFalse(taxon.GetCheckedOutChangesTaxonProperties(GetUserContext()).IsValid);
        }

        [TestMethod]
        public void UndoRemoveTaxon()
        {
            // Arrange
            ITaxon parentTaxon = GetTaxonManager().GetTaxon(GetUserContext(), (Int32)(TaxonId.Bears));

            ITaxon newTaxon = new Taxon();
            newTaxon.Id = Int32.MinValue;
            newTaxon.DataContext = new DataContext(GetRevisionUserContext());
            ITaxonRevision taxonRevision = GetTaxonManager().GetTaxonRevision(GetUserContext(), 1);

            GetTaxonManager().CreateTaxon(GetRevisionUserContext(), taxonRevision, newTaxon, "SCiName", "commonName", "Tester",
                                          TaxonAlertStatusId.Yellow, parentTaxon, CoreData.TaxonManager.GetTaxonCategory(GetUserContext(), TaxonCategoryId.Species), "");
            
            // Act
            GetTaxonManager().DeleteTaxon(GetRevisionUserContext(), newTaxon, taxonRevision);
            GetTaxonManager().DeleteTaxonRevisionEvent(GetRevisionUserContext(), taxonRevision.GetRevisionEvents(GetUserContext()).Last(), taxonRevision);


            // Assert
            newTaxon = GetTaxonManager().GetTaxon(GetUserContext(), newTaxon.Id);
            Assert.IsTrue(newTaxon.Category.Id == (Int32)(TaxonCategoryId.Species));
        }

        [TestMethod]
        public void RemoveTaxon_RevisionCheckedIn_ChangesVisible()
        {
            // Arrange
            var taxon = GetTaxonManager().GetTaxon(GetUserContext(), 2000045);
            var revision = GetRevisionInOngoingState(taxon);

            var taxonPropertiesCount = taxon.GetTaxonProperties(GetUserContext()).Count;
            GetTaxonManager().DeleteTaxon(GetUserContext(), taxon, revision);

            // Act
            GetTaxonManager().CheckInTaxonRevision(GetUserContext(), revision);
            taxon = GetTaxonManager().GetTaxon(GetUserContext(), taxon.Id);

            // Assert
            Assert.AreEqual(taxonPropertiesCount + 1, taxon.GetTaxonProperties(GetUserContext()).Count);
            Assert.IsFalse(taxon.IsValid);
            
        }

        [TestMethod]
        public void InvalidTaxonHasOnlyHistoricRelations()
        {
            // Arrange
            var taxon = GetTaxonManager().GetTaxon(GetUserContext(), 2052);

            // Act
            var historicRelations = taxon.GetAllParentTaxonRelations(GetUserContext(), null, false, true);
            var currentRelations = taxon.GetAllParentTaxonRelations(GetUserContext(), null, false, false);

            // Assert
            Assert.IsTrue(currentRelations.Count == 0);
            Assert.IsTrue(historicRelations.Count > 0);
        }


        [TestMethod]
        public void CanLoadRevisionBasedOnId()
        {
            // Arrange
            var revisionId = 1;

            // Act
            var fetchedRevision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);

            // Assert

            Assert.IsTrue(fetchedRevision.RootTaxon.ScientificName.IsNotEmpty());
        }

        /// <summary>
        /// Must be set to if not UserAdmin is needed.
        /// </summary>
        /// <returns></returns>
        override protected string GetTestApplicationName()
        {
            return Settings.Default.DyntaxaApplicationIdentifier;
        }
          
        [TestMethod]
        public void DataSource()
        {
            ITaxonDataSource dataSource = null;
            GetTaxonManager(true).DataSource = dataSource;
            Assert.AreEqual(dataSource, GetTaxonManager().DataSource);

            dataSource = new TaxonDataSource();
            GetTaxonManager().DataSource = dataSource;
            Assert.AreEqual(dataSource, GetTaxonManager().DataSource);
        }

        [TestMethod]
        public void GetRevisionBySearchCriteria_TaxonIsLoaded()
        {
            TaxonRevisionSearchCriteria searchCriteria = new TaxonRevisionSearchCriteria();
            searchCriteria.TaxonIds = new List<int>();
            searchCriteria.TaxonIds.Add(4000107);
            var revisions = GetTaxonManager().GetTaxonRevisions(GetUserContext(), searchCriteria);
            
            Assert.IsTrue(revisions[0].RootTaxon.Id > 0);
            Assert.IsTrue(revisions[0].RootTaxon.Category.Id > 0);
        }

        [TestMethod]
        public void GetRevisionBySearchCriteria_FilterOnRevisionState_TaxonIsLoaded()
        {
            TaxonRevisionSearchCriteria searchCriteria = new TaxonRevisionSearchCriteria();
            searchCriteria.StateIds = new List<int>();
            searchCriteria.StateIds.Add(1);
            searchCriteria.StateIds.Add(2);
            searchCriteria.StateIds.Add(3);
            var revisions = GetTaxonManager().GetTaxonRevisions(GetUserContext(), searchCriteria);

            Assert.IsTrue(revisions[0].RootTaxon.Id > 0);
            Assert.IsTrue(revisions[0].RootTaxon.Category.Id > 0);
        }

        [TestMethod]
        public void GetRevisionsByTaxon()
        {
            int taxonId = 4000099;
            TaxonRevisionList revisions = new TaxonRevisionList();
            ITaxon taxon = GetTaxonManager(true).GetTaxon(GetUserContext(), taxonId);
            revisions = GetTaxonManager().GetTaxonRevisions(GetUserContext(), taxon);

            foreach (var revision in revisions)
            {
                var lastId = 0;
                foreach (var revisionEvent in ((TaxonRevision)revision).GetRevisionEvents(GetUserContext()))
                {
                    Assert.IsTrue(lastId < revisionEvent.Id);
                    lastId = revisionEvent.Id;
                }
            }

            Assert.IsNotNull(revisions);
        }

        [TestMethod]
        public void GetTaxonRevisionsBySearchCriteria()
        {
            TaxonRevisionSearchCriteria searchCriteria = new TaxonRevisionSearchCriteria();
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                TaxonRevisionList revisions;

                // Create list of taxon ids
                List<Int32> taxonIdList = new List<Int32>();
                taxonIdList.Add(1001645);
                /*
                for (int i = 0; i < 10; i++)
                {
                    i++;
                    IRevision refRevision = GetReferenceRevision(i); 
                    ITaxon refTaxon = GetReferenceTaxon("Taxa created by revision test " + i);
                    i--;
                    GetTaxonManager().SaveTaxon(GetRevisionUserContext(), refTaxon);
                    refRevision.Taxon = refTaxon;
                    GetTaxonManager().SaveRevision(GetRevisionUserContext(), refRevision);
                    taxonIdList.Add(refRevision.Taxon.Id);
                }
                  
                searchCriteria.TaxonIds = taxonIdList;
                revisions = GetTaxonManager().GetRevisionsBySearchCriteria(GetUserContext(), searchCriteria);

                // Check revisions set
                Assert.IsNotNull(revisions);
                Assert.AreEqual(10, revisions.Count);
                Assert.IsNotNull(revisions[0].CreatedDate);
                Assert.IsNotNull(revisions[2].ExpectedEndTime);
                Assert.IsNotNull(revisions[0].DescriptionString);
                Assert.IsNotNull(revisions[1].GUID);
                Assert.IsNotNull(revisions[1].RevisionState);
                Assert.IsNotNull(revisions[1].Taxon.Id);
                Assert.AreNotEqual(revisions[9].Id, revisions[8].Id);
                Assert.AreEqual(revisions[9].CreatedBy, revisions[0].CreatedBy);
                Assert.AreEqual(revisions[7].ExpectedEndTime, revisions[6].ExpectedEndTime);
                Assert.IsTrue((DateTime.Now - revisions[9].ExpectedStartTime) < new TimeSpan(0, 0, Settings.Default.ComputerTimeDifference));

                Assert.AreEqual(revisions[0].Taxon.Id, taxonIdList[0]);
                Assert.AreEqual(revisions[9].Taxon.Id, taxonIdList[9]);
                Assert.AreNotEqual(revisions[0].RevisionState.Id, revisions[1].RevisionState.Id);
                */

                 List<Int32> revisionStateIds = new List<int>();
                // include all revision states
                 foreach (int revisionState in Enum.GetValues(typeof(TaxonRevisionStateId)))
                {
                    revisionStateIds.Add(revisionState);
                }

                searchCriteria.TaxonIds = null;
                searchCriteria.StateIds = revisionStateIds;
                revisions = GetTaxonManager().GetTaxonRevisions(GetUserContext(), searchCriteria);

                // Check revisions set
                Assert.IsNotNull(revisions);
                Assert.IsTrue(revisions.Count >= 3);

                // include one  revision states
                revisionStateIds = new List<int>();
                int revisionStateTemp = (int)TaxonRevisionStateId.Created;
                revisionStateIds.Add(revisionStateTemp);
               

                searchCriteria.TaxonIds = null;
                searchCriteria.StateIds = revisionStateIds;
                revisions = GetTaxonManager().GetTaxonRevisions(GetUserContext(), searchCriteria);

                // Check revisions set
                Assert.IsNotNull(revisions);
                Assert.IsTrue(revisions.Count >= 1);

                // Check that revison state matches revision, the newly created revisions are at the end of the list...
                Assert.AreEqual(revisions[revisions.Count - 1].State.Id, (int)TaxonRevisionStateId.Created);
            }
            

        }

        [TestMethod]
        public void CanRecurseThroughSortedTree()
        {
            var rootTaxon = GetTaxonManager().GetTaxon(GetUserContext(), 4000107);

            var firstParent = rootTaxon.GetNearestParentTaxonRelations(GetUserContext()).First();

            Assert.IsTrue(firstParent.IsMainRelation);


            int depth = 1;
            Recurse(GetUserContext(), rootTaxon, ref depth);
        }

        private void Recurse(IUserContext userContext, ITaxon root, ref int depth)
        {
            foreach (ITaxonRelation taxonRelation in root.GetAllChildTaxonRelations(userContext))
            {
                depth++;
                Recurse(userContext, taxonRelation.ChildTaxon, ref depth);
            }
        }


        [TestMethod]
        public void GetRevisionEventsByRevisionId()
        {
            // Arrange
            int revisionId = 1;
            var revision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);

            //Act
            TaxonRevisionEventList revisionEvents = GetTaxonManager().GetTaxonRevisionEvents(GetUserContext(), revisionId);

            //Assert
            Assert.IsTrue(revisionEvents[0].Type.Description != null);
            Assert.IsTrue(revisionEvents[0].Type.Description != string.Empty);
            Assert.IsNotNull(revisionEvents);
            Assert.AreEqual(revisionEvents.Count, revision.GetRevisionEvents(GetUserContext()).Count);
            Assert.AreEqual(revisionEvents[0].RevisionId, revision.Id);
        }

        [TestMethod]
        public void GetConceptDefinition()
        {
            //Exemple Catantopidae [2002844]
            int taxonId = 2002844;
            ITaxon taxon = GetTaxonManager().GetTaxon(GetUserContext(), taxonId);
            String conceptDefinition = taxon.GetConceptDefinition(GetUserContext());
            Debug.Print(conceptDefinition);
            Assert.IsNotNull(conceptDefinition);
        }

        [TestMethod]
        public void GetLumpSplitEventByGuid()
        {
            // Arrange
            var GUID = "urn:lsid:dyntaxa.se:LumpSplitEvent:1";

            // Act
            var testObject = GetTaxonManager().GetLumpSplitEvent(GetUserContext(), GUID);

            // Assert
            Assert.IsTrue(testObject.Id > 0);
        }

        [TestMethod]
        public void GetRevisionByGuid()
        {
            // Arrange
            var GUID = "urn:lsid:dyntaxa.se:Revision:1";

            // Act
            var testObject = GetTaxonManager().GetTaxonRevision(GetUserContext(), GUID);

            // Assert
            Assert.IsTrue(testObject.Id > 0);
        }

        [TestMethod]
        public void GetTaxonNameByGuid()
        {
            ITaxonName taxonName;
            String guid;

            // Arrange
            guid = "urn:lsid:dyntaxa.se:TaxonName:230";

            // Act
            taxonName = GetTaxonManager().GetTaxonName(GetUserContext(), guid);

            // Assert
            Assert.IsNotNull(taxonName);
            Assert.IsNotNull(taxonName.Taxon);
            Assert.AreEqual(230, taxonName.Id);


            // Arrange
            guid = "urn:lsid:dyntaxa.se:TaxonName:230:100145";

            // Act
            taxonName = GetTaxonManager().GetTaxonName(GetUserContext(), guid);

            // Assert
            Assert.IsNotNull(taxonName);
            Assert.IsNotNull(taxonName.Taxon);
            Assert.AreEqual(100145, taxonName.Version);
        }

        [TestMethod]
        public void GetTaxaByIds()
        {
            // Create list of taxon ids
            TaxonList taxa;
            List<Int32> taxonIdList = new List<Int32>();
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                for (int i = 0; i < 10; i++)
                {
                //    i++;
               //     ITaxon refTaxon = GetReferenceTaxon("Taxa" + i);
               //     i--;
               //     GetTaxonManager(true).SaveTaxon(GetUserContext(), refTaxon, null);
                    taxonIdList.Add(i);
                }
                // Get all taxon for the created list
                taxa = GetTaxonManager().GetTaxa(GetUserContext(), taxonIdList);

                //Test
                Assert.IsNotNull(taxa);
                Assert.AreEqual(10, taxa.Count);
                Assert.IsNotNull(taxa[0].GetConceptDefinition(GetUserContext()));
                Assert.IsNotNull(taxa[0].CreatedDate);
                Assert.IsNotNull(taxa[1].Guid);
                Assert.AreNotEqual(taxa[9].Id, taxa[8].Id);
                Assert.AreEqual(taxa[9].CreatedBy, taxa[0].CreatedBy);
                Assert.AreEqual(taxa[7].ModifiedByPerson, taxa[6].ModifiedByPerson);
            }
        }

        [TestMethod]
        public void CanCreateTaxon()
        {
            // Arrange
            int revisionId = 1;
            var revision = GetTaxonManager().GetTaxonRevision(GetUserContext(), revisionId);
            ITaxon newTaxon = new Taxon();
            var parentTaxon = GetTaxonManager().GetTaxon(GetRevisionUserContext(revision.Id), 100111);

            newTaxon.AlertStatus = CoreData.TaxonManager.GetTaxonAlertStatus(GetUserContext(),
                                                                             TaxonAlertStatusId.Green);
            newTaxon.ValidFromDate = DateTime.Now;
            newTaxon.ValidToDate = DateTime.Now.AddDays(1000);
            newTaxon.ChangeStatus = CoreData.TaxonManager.GetTaxonChangeStatus(GetUserContext(),
                                                                               TaxonChangeStatusId.Unchanged);
            newTaxon.CreatedDate = DateTime.Now;
            newTaxon.CreatedBy = GetUserContext().User.Id;
            newTaxon.DataContext = new DataContext(GetUserContext());
            newTaxon.Id = Int32.MinValue;
            newTaxon.ModifiedByPerson = "testuser";
            newTaxon.IsPublished = false;
            newTaxon.ModifiedDate = DateTime.Now;
            newTaxon.PartOfConceptDefinition = string.Empty;

            // Act
            GetTaxonManager().CreateTaxon(GetUserContext(), revision, newTaxon, "scientificName", "commonName", "author", 0, parentTaxon, GetTaxonManager().GetTaxonCategory(GetUserContext(),2), string.Empty  );

            // Assert
            newTaxon = GetTaxonManager().GetTaxon(GetUserContext(), newTaxon.Id);
            Assert.IsTrue(newTaxon.Id > 0);
            Assert.IsTrue(newTaxon.Author.IsNotEmpty());
            Assert.IsTrue(newTaxon.CommonName.IsNotEmpty());
            Assert.IsTrue(newTaxon.ScientificName.IsNotEmpty());

           
        }

        [TestMethod]
        public void GetTaxaByIds_RecordsExists()
        {
            // Create list of taxon ids from taxa in revision # 1
            List<Int32> taxonIdList = new List<Int32>();
            taxonIdList.Add(100084);
            taxonIdList.Add(100085);

            TaxonList taxa = GetTaxonManager().GetTaxa(GetUserContext(), taxonIdList);
            Assert.IsNotNull(taxa);
            Assert.AreEqual(2, taxa.Count);
            ITaxonName recommendedScientificNanme = taxa[0].GetScientificName(GetUserContext());
            Assert.AreEqual(recommendedScientificNanme.Name, "Muscardinus avellanarius");
        }

        [TestMethod]
        public void GetTaxaBySearchCriteria()
        {

            TaxonSearchCriteria searchCriteria = new TaxonSearchCriteria();
            List<Int32> taxonIdList = new List<Int32>();

            taxonIdList.Add(3000303);
            taxonIdList.Add(4);
            taxonIdList.Add(199);

            searchCriteria.TaxonNameSearchString = null;
            searchCriteria.TaxonIds = taxonIdList;
            TaxonList taxa = GetTaxonManager().GetTaxa(GetUserContext(), searchCriteria);
            Assert.IsNotNull(taxa);
            Assert.AreEqual(3, taxa.Count);
            Assert.IsNotNull(taxa[0].ModifiedByPerson);
            Assert.IsNotNull(taxa[0].CreatedDate);
            Assert.IsNotNull(taxa[1].Guid);
            Assert.AreNotEqual(taxa[0].Id, taxa[1].Id);

            // Within  revision # 1
            taxa.Clear();
            taxonIdList.Add(100015);
            taxonIdList.Add(100024);
            searchCriteria.TaxonNameSearchString = null;
            searchCriteria.TaxonIds = taxonIdList;
            taxa = GetTaxonManager().GetTaxa(GetRevisionUserContext(), searchCriteria);
            Assert.IsNotNull(taxa);
            Assert.AreEqual(2, taxa.Count);
            Assert.IsNotNull(taxa[0].ModifiedByPerson);
            Assert.IsNotNull(taxa[0].CreatedDate);
            Assert.IsNotNull(taxa[1].Guid);
            Assert.AreNotEqual(taxa[0].Id, taxa[1].Id);

            // AllChildTaxa
            taxa.Clear();
            taxonIdList.Add(4000107);
            searchCriteria.TaxonNameSearchString = null;
            searchCriteria.TaxonIds = taxonIdList;
            searchCriteria.Scope = TaxonSearchScope.AllChildTaxa;
            taxa = GetTaxonManager().GetTaxa(GetRevisionUserContext(), searchCriteria);
            Assert.IsNotNull(taxa);
            Assert.IsTrue(taxa.Count > 10);
        }

        [TestMethod]
        public void GetTaxonById()
        {
            Int32 taxonId;
            ITaxon taxon;
            
            taxonId = (Int32)(TaxonId.Bear);
            taxon = GetTaxonManager(true).GetTaxon(GetUserContext(), taxonId);
            Assert.IsNotNull(taxon);
            Assert.AreEqual(taxonId, taxon.Id);
        }

        [TestMethod]
        public void GetTaxonByGUID()
        {
            String GUID = "urn:lsid:dyntaxa.se:Taxon:3";
            ITaxon taxon = GetTaxonManager(true).GetTaxon(GetUserContext(), GUID);
            Assert.IsNotNull(taxon);
            Assert.IsTrue(taxon.GetTaxonNames(GetUserContext()).Count > 0);
        }

        [TestMethod]
        public void GetTaxonById_ExistingData()
        {
            // signalkräfta = 233833
            Int32 taxonId = 233833;
            
            ITaxon taxon = GetTaxonManager(true).GetTaxon(GetRevisionUserContext(), taxonId);
            Assert.IsTrue(taxon.ScientificName.IsNotEmpty());
            Assert.AreEqual(taxonId, taxon.Id);
            Assert.IsTrue(taxon.GetTaxonNames(GetRevisionUserContext()).Count > 0);

        }

        //[TestMethod]
        //public void CanLoadAllChildTaxa()
        //{
        //    // Arrange
        //    Int32 taxonId = 2000448;

        //    // Act
        //    ITaxon taxon = GetTaxonManager(true).GetTaxonById(GetUserContext(), taxonId);
        //    var children = taxon.AllChildTaxa;

        //    // Assert
        //    Assert.IsTrue(children.Count > 0);
        //    Assert.IsNotNull(children.First().RelatedTaxon.CreatedDate);
        //}

        [TestMethod]
        public void SaveTaxon()
        {

            ITaxon taxon;
            ITaxonRevisionEvent taxonRevisionEvent = null;
            string conceptPartDesc;
            DateTime validFromDate, validToDate;

            // Test data that is not set in the client.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                taxon = GetReferenceTaxon2();
                GetTaxonManager(true).UpdateTaxon(GetUserContext(), taxon, taxonRevisionEvent, null, null);
                Assert.IsNotNull(taxon);

                // Test created by user.
                Assert.AreNotEqual(Int32.MinValue, taxon.CreatedBy);
                Assert.AreEqual(Settings.Default.TestUserId, taxon.CreatedBy);

                // Test DataContext.
                Assert.IsNotNull(taxon.DataContext);

                // Test GUID.
                Assert.IsTrue(taxon.Guid.IsNotEmpty());

                // Test id.
                Assert.AreNotEqual(Int32.MinValue, taxon.Id);

            }

            // Test part concept description
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                conceptPartDesc = @"Halloj lien räksmörgås RÄKSMÖRGÅS nr 0.9";
                taxon = GetReferenceTaxon2();
                taxon.PartOfConceptDefinition = conceptPartDesc;
                GetTaxonManager().UpdateTaxon(GetUserContext(), taxon, taxonRevisionEvent, null, null);
                Assert.IsNotNull(taxon);
                Assert.AreEqual(conceptPartDesc, taxon.PartOfConceptDefinition);
            }

            // Test valid from date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validFromDate = new DateTime(2000, 6, 5);
                taxon = GetReferenceTaxon2();
                taxon.ValidFromDate = validFromDate;
                GetTaxonManager().UpdateTaxon(GetUserContext(), taxon, taxonRevisionEvent, null, null);
                Assert.IsNotNull(taxon);
                Assert.AreEqual(validFromDate, taxon.ValidFromDate);
            }

            // Test valid to date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validToDate = new DateTime(2000, 6, 5);
                taxon = GetReferenceTaxon2();
                taxon.ValidToDate = validToDate;
                GetTaxonManager().UpdateTaxon(GetUserContext(), taxon, taxonRevisionEvent, null, null);
                Assert.IsNotNull(taxon);
                Assert.AreEqual(validToDate, taxon.ValidToDate);
            }

            // TODO: Create test for child, parent and taxon Names..

        }

        [TestMethod]
        [Ignore]
        public void SpeedTest()
        {
            IList<ITaxon> taxa;
            Int32 taxonIdIndex;
            List<Int32> taxonIds;
            Stopwatch csharpStopwatch, linqStopwatch;
            IList<ITaxon> csharpTaxa, linqTaxa;

            taxonIds = new List<Int32>();
            for (taxonIdIndex = 1; taxonIdIndex <= 500; taxonIdIndex++)
            {
                taxonIds.Add(taxonIdIndex);
            }

            taxa = GetTaxonManager(true).GetTaxa(GetUserContext(), taxonIds);
            csharpStopwatch = new Stopwatch();
            csharpStopwatch.Start();
            csharpTaxa = new List<ITaxon>();
            foreach (ITaxon taxon in taxa)
            {
                if ((taxon.Id % 2) == 0)
                {
                    csharpTaxa.Add(taxon);
                }
            }
            csharpStopwatch.Stop();

            linqStopwatch = new Stopwatch();
            linqStopwatch.Start();
            linqTaxa = (from taxon in taxa where ((taxon.Id % 2) == 0) select taxon).ToList();
            linqStopwatch.Stop();

            Assert.AreEqual(csharpTaxa.Count, linqTaxa.Count);
        }
        

        //[TestMethod]
        //public void CanLoadAllParentTaxa()
        //{
        //    // Arrange
        //    Int32 taxonId = 4000107;

        //    // Act
        //    ITaxon taxon = GetTaxonManager(true).GetTaxonById(GetUserContext(), taxonId);
        //    var parents = taxon.AllParentTaxa;

        //    var t = taxon.GetAllParentTaxonRelations(GetUserContext(), null, false, true, false);

        //    // Assert
        //    Assert.IsTrue(parents.Count > 0);
        //    Assert.IsNotNull(parents.First().RelatedTaxon.CreatedDate);
        //}

        [TestMethod]
        public void CanLazyLoadParentTaxonRelations()
        {
            // Arrange
            var taxon = GetTaxonManager(true).GetTaxon(GetUserContext(), 2000448);

            // Act
            var parents = taxon.GetNearestParentTaxonRelations(GetUserContext());

            // Assert
            Assert.IsTrue(parents.Count > 0);
        }

        [TestMethod]
        public void CanLazyLoadTaxonProperties()
        {
            // Arrange
            var taxon = GetTaxonManager(true).GetTaxon(GetUserContext(), 2000448);

            // Act
            var properties = taxon.GetTaxonProperties(GetUserContext());

            // Assert
            Assert.IsTrue(properties.Count > 0);
            Assert.IsTrue(properties[0].TaxonCategory.Name.Length > 0);
        }

        [TestMethod]
        public void GetTaxonById_WithExistingTaxonId()
        {
            Int32 taxonId = 2000045;  // taxa with two names.
            IPerson person;
            ITaxon taxon;

            taxon = GetTaxonManager(true).GetTaxon(GetUserContext(), taxonId);
            Assert.IsNotNull(taxon);
            Assert.IsTrue(taxon.GetTaxonNames(GetUserContext()).Count > 0);
            person = taxon.GetModifiedByPerson(GetUserContext());
            Assert.AreEqual("TestFirstName TestLastName", person.FullName);
        }

        [TestMethod]
        public void GetTaxonCategories()
        {
            ITaxon taxon;
            TaxonCategoryList taxonCategories1, taxonCategories2;

            taxonCategories1 = GetTaxonManager(true).GetTaxonCategories(GetUserContext());
            Assert.IsTrue(taxonCategories1.IsNotEmpty());

            taxon = CoreData.TaxonManager.GetTaxon(GetUserContext(), (Int32)(TaxonId.Mammals));
            taxonCategories1 = GetTaxonManager(true).GetTaxonCategories(GetUserContext(), taxon);
            Assert.IsTrue(taxonCategories1.IsNotEmpty());
            Assert.IsTrue(10 < taxonCategories1.Count);
            taxonCategories2 = GetTaxonManager().GetTaxonCategories(GetRevisionUserContext(), taxon);
            Assert.IsTrue(taxonCategories2.IsNotEmpty());
            Assert.IsTrue(10 < taxonCategories2.Count);
            Assert.IsTrue(taxonCategories1.Count < taxonCategories2.Count);
        }

        [TestMethod]
        public void GetTaxonCategoryById()
        {
            ITaxonCategory taxonCategory;

            foreach (ITaxonCategory tempTaxonCategory in GetTaxonManager(true).GetTaxonCategories(GetUserContext()))
            {
                taxonCategory = GetTaxonManager().GetTaxonCategory(GetUserContext(), tempTaxonCategory.Id);
                Assert.IsNotNull(taxonCategory);
                Assert.AreEqual(tempTaxonCategory.Id, taxonCategory.Id);
            }
        }

        [TestMethod]
        public void GetTaxonNameCategories()
        {
            TaxonNameCategoryList taxonNameCategories;

            taxonNameCategories = GetTaxonManager(true).GetTaxonNameCategories(GetUserContext());
            Assert.IsTrue(taxonNameCategories.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonNameCategoryById()
        {
            ITaxonNameCategory taxonCategory;

            taxonCategory = GetTaxonManager(true).GetTaxonNameCategory(GetUserContext(), (Int32)(TaxonNameCategoryId.ScientificName));
            Assert.IsNotNull(taxonCategory);
            Assert.AreEqual((Int32)(TaxonNameCategoryId.ScientificName), taxonCategory.Id);
        }

        [TestMethod]
        public void GetTaxonNameStatus()
        {
            ITaxonNameStatus taxonNameStatus;
            TaxonNameStatusList taxonNameStatusList;

            // Get all taxon name status.
            taxonNameStatusList = GetTaxonManager(true).GetTaxonNameStatuses(GetUserContext());
            Assert.IsTrue(taxonNameStatusList.IsNotEmpty());

            // Get taxon name status by id.
            foreach (ITaxonNameStatus tempTaxonNameStatus in taxonNameStatusList)
            {
                taxonNameStatus = GetTaxonManager().GetTaxonNameStatus(GetUserContext(), tempTaxonNameStatus.Id);
                Assert.IsNotNull(taxonNameStatus);
                Assert.AreEqual(tempTaxonNameStatus.Id, taxonNameStatus.Id);
            }
        }

        [TestMethod]
        public void GetTaxonNameUsage()
        {
            ITaxonNameUsage taxonNameUsage;
            TaxonNameUsageList taxonNameUsageList;

            // Get all taxon name usage.
            taxonNameUsageList = GetTaxonManager(true).GetTaxonNameUsages(GetUserContext());
            Assert.IsTrue(taxonNameUsageList.IsNotEmpty());

            // Get taxon name usage by id.
            foreach (ITaxonNameUsage tempTaxonNameStatus in taxonNameUsageList)
            {
                taxonNameUsage = GetTaxonManager().GetTaxonNameUsage(GetUserContext(), tempTaxonNameStatus.Id);
                Assert.IsNotNull(taxonNameUsage);
                Assert.AreEqual(tempTaxonNameStatus.Id, taxonNameUsage.Id);
            }
        }


        [TestMethod]
        public void GetTaxonNameCategoryType()
        {
            ITaxonNameCategoryType taxonNameCategoryType;

            GetTaxonManager(true);
            foreach (TaxonNameCategoryTypeId taxonNameCategoryTypeId in Enum.GetValues(typeof(TaxonNameCategoryTypeId)))
            {
                taxonNameCategoryType = GetTaxonManager().GetTaxonNameCategoryType(GetUserContext(), (Int32)taxonNameCategoryTypeId);
                Assert.IsNotNull(taxonNameCategoryType);
                taxonNameCategoryType = GetTaxonManager().GetTaxonNameCategoryType(GetUserContext(), taxonNameCategoryTypeId);
                Assert.IsNotNull(taxonNameCategoryType);
            }
        }

        [TestMethod]
        public void GetTaxonNameCategoryTypes()
        {
            TaxonNameCategoryTypeList taxonNameCategoryTypes;

            taxonNameCategoryTypes = GetTaxonManager(true).GetTaxonNameCategoryTypes(GetUserContext());
            Assert.IsTrue(taxonNameCategoryTypes.IsNotEmpty());
            foreach (ITaxonNameCategoryType taxonNameCategoryType in taxonNameCategoryTypes)
            {
                Assert.IsTrue(-1 < taxonNameCategoryType.Id);
                Assert.IsTrue(taxonNameCategoryType.Identifier.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetTaxonNamesBySearchCriteria()
        {
            TaxonNameSearchCriteria searchCriteria = new TaxonNameSearchCriteria();
            searchCriteria.NameSearchString = new StringSearchCriteria();
            // Test EXACT search
            searchCriteria.NameSearchString.SearchString = "björn";
            searchCriteria.NameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.Equal);

            TaxonNameList taxonNames = GetTaxonManager(true).GetTaxonNames(GetUserContext(), searchCriteria);
            Assert.AreEqual(1, taxonNames.Count);

            // default to LIKE ? 
            searchCriteria.NameSearchString.CompareOperators = new List<StringCompareOperator>();
            taxonNames = GetTaxonManager().GetTaxonNames(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonNames.Count > 1);

            taxonNames = GetTaxonManager().GetTaxonNames(GetUserContext(), new TaxonNameSearchCriteria() { NameSearchString = new StringSearchCriteria() { SearchString = "Trumgräshoppa"}});
       
            Assert.IsNotNull(taxonNames);
            Assert.IsTrue(taxonNames.Count > 0);

            searchCriteria = new TaxonNameSearchCriteria();
            // check NameUsage criteria + author
            searchCriteria.Status = GetTaxonManager().GetTaxonNameStatus(GetUserContext(), TaxonNameStatusId.ApprovedNaming);
            searchCriteria.NameSearchString = new StringSearchCriteria() { SearchString = "Polytrich"};
            searchCriteria.AuthorSearchString = new StringSearchCriteria() {SearchString = "long"};
            searchCriteria.IsValidTaxon = true;
            taxonNames = GetTaxonManager().GetTaxonNames(GetUserContext(), searchCriteria);
            Assert.IsNotNull(taxonNames);
            Assert.IsTrue(taxonNames.Count > 0);

            // IsAuthorIncludedInNameSearchString
            searchCriteria = new TaxonNameSearchCriteria();
            searchCriteria.NameSearchString = new StringSearchCriteria() { SearchString = "Vespertilio auritus     Linnaeus, 175"};
            searchCriteria.NameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.BeginsWith);
            searchCriteria.IsAuthorIncludedInNameSearchString = true;
            searchCriteria.IsValidTaxon = true;
            taxonNames = GetTaxonManager().GetTaxonNames(GetUserContext(), searchCriteria);
            Assert.IsNotNull(taxonNames);
            Assert.AreEqual(1, taxonNames.Count);

            // IsOriginalName
            searchCriteria = new TaxonNameSearchCriteria();
            searchCriteria.IsOriginalName = true;
            searchCriteria.IsValidTaxon = true;
            taxonNames = GetTaxonManager().GetTaxonNames(GetUserContext(), searchCriteria);
            Assert.IsNotNull(taxonNames);
            foreach (ITaxonName taxonName in taxonNames)
            {
                Assert.IsTrue(taxonName.IsOriginalName);    
            }

            // revision = 1
            searchCriteria = new TaxonNameSearchCriteria();
            searchCriteria.NameSearchString = new StringSearchCriteria() {SearchString = "t"};

            taxonNames = GetTaxonManager().GetTaxonNames(GetRevisionUserContext(), searchCriteria);
            Assert.IsTrue(taxonNames.Count > 0);

            // Date when taxon was modified
            searchCriteria = new TaxonNameSearchCriteria();
            searchCriteria.LastModifiedStartDate = new DateTime(2012, 4, 20, 0, 0, 0);
            searchCriteria.LastModifiedEndDate = new DateTime(2013, 6, 11, 0, 0, 0);
            searchCriteria.IsRecommended = true;
            searchCriteria.Category = CoreData.TaxonManager.GetTaxonNameCategory(this.GetUserContext(), (int)TaxonNameCategoryId.ScientificName);
            searchCriteria.IsValidTaxonName = true;
            searchCriteria.IsValidTaxon = true;
            searchCriteria.IsAuthorIncludedInNameSearchString = false;
            taxonNames = this.GetTaxonManager().GetTaxonNames(this.GetUserContext(), searchCriteria);
            Assert.IsNotNull(taxonNames);
            foreach (ITaxonName taxonName in taxonNames)
            {
                Assert.IsTrue(taxonName.Taxon.ModifiedDate > searchCriteria.LastModifiedStartDate.Value &&
                    taxonName.Taxon.ModifiedDate < searchCriteria.LastModifiedEndDate.Value);
            }

            // Test strange characters.
            searchCriteria = new TaxonNameSearchCriteria();
            searchCriteria.NameSearchString = new StringSearchCriteria() { SearchString = "Polytrich" };
            searchCriteria.NameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            searchCriteria.IsValidTaxon = true;

            searchCriteria.NameSearchString.SearchString = "björn'";
            taxonNames = GetTaxonManager().GetTaxonNames(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonNames.IsEmpty());

            searchCriteria.NameSearchString.SearchString = "<varg";
            taxonNames = GetTaxonManager().GetTaxonNames(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonNames.IsEmpty());

            searchCriteria.NameSearchString.SearchString = "varg>";
            taxonNames = GetTaxonManager().GetTaxonNames(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonNames.IsEmpty());

            searchCriteria.NameSearchString.SearchString = "<varg>";
            taxonNames = GetTaxonManager().GetTaxonNames(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonNames.IsEmpty());
        }

        //[TestMethod]
        //public void IsTaxonNameUnique()
        //{
        //    String taxonName = "björn";
        //    Boolean bUnique =  GetTaxonManager(true).IsTaxonNameUnique(GetUserContext(), taxonName);
        //    Assert.IsFalse(bUnique);

        //    taxonName = "falukorv";
        //    bUnique =  GetTaxonManager().IsTaxonNameUnique(GetUserContext(), taxonName);
        //    Assert.IsTrue(bUnique);
        //}

        [TestMethod]
        public void GetTaxonNamesByTaxa()
        {
            Int32 index;
            TaxonList taxa;
            List<TaxonNameList> allTaxonNames;

            taxa = new TaxonList();
            taxa.Add(GetTaxonManager(true).GetTaxon(GetUserContext(), TaxonId.Bear));
            taxa.Add(GetTaxonManager().GetTaxon(GetUserContext(), TaxonId.Mammals));
            allTaxonNames = GetTaxonManager().GetTaxonNames(GetUserContext(), taxa);
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

        [TestMethod]
        public void GetTaxonNamesByTaxon()
        {
            ITaxon taxon;
            TaxonNameList taxonNames;

            Int32 taxonId = Settings.Default.TestTaxonId;
            taxon = GetTaxonManager(true).GetTaxon(GetUserContext(), taxonId);
            taxonNames = GetTaxonManager().GetTaxonNames(GetUserContext(), taxon);
            Assert.IsNotNull(taxonNames);
        }

        [TestMethod]
        public void GetTaxonNamesByTaxonId_TestRevision()
        {
            ITaxon taxon;
            TaxonNameList taxonNames, taxonNamesInRevision;

            taxon = GetTaxonManager(true).GetTaxon(GetUserContext(), TaxonId.TaigaShrew);
            taxonNames = GetTaxonManager().GetTaxonNames(GetUserContext(), taxon);
            Assert.IsTrue(taxonNames.IsNotEmpty());

            taxonNamesInRevision = GetTaxonManager().GetTaxonNames(GetRevisionUserContext(), taxon);
            Assert.IsTrue(taxonNamesInRevision.IsNotEmpty());
            Assert.IsTrue(taxonNames.Count <= taxonNamesInRevision.Count);
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
            taxonRelations1 = GetTaxonManager(true).GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());

            // Test is valid.
            searchCriteria.IsMainRelation = null;
            searchCriteria.IsValid = true;
            searchCriteria.Taxa = null;
            taxonRelations1 = GetTaxonManager().GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.IsValid = false;
            taxonRelations2 = GetTaxonManager().GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations1.Count > taxonRelations2.Count);

            // Test is main relation.
            searchCriteria.IsValid = null;
            searchCriteria.IsMainRelation = true;
            searchCriteria.Taxa = null;
            taxonRelations1 = GetTaxonManager().GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.IsMainRelation = false;
            taxonRelations2 = GetTaxonManager().GetTaxonRelations(GetUserContext(), searchCriteria);
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
            taxonRelations1 = GetTaxonManager().GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestParentRelations;
            taxonRelations2 = GetTaxonManager().GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations2.Count < taxonRelations1.Count);

            searchCriteria.Scope = TaxonRelationSearchScope.AllChildRelations;
            taxonRelations1 = GetTaxonManager().GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestChildRelations;
            taxonRelations2 = GetTaxonManager().GetTaxonRelations(GetUserContext(), searchCriteria);
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
            taxonRelations1 = GetTaxonManager().GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestParentRelations;
            taxonRelations2 = GetTaxonManager().GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations2.Count < taxonRelations1.Count);

            searchCriteria.Scope = TaxonRelationSearchScope.AllChildRelations;
            taxonRelations1 = GetTaxonManager().GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestChildRelations;
            taxonRelations2 = GetTaxonManager().GetTaxonRelations(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations2.Count < taxonRelations1.Count);


            // Test in revision.
            // Test with one taxon.
            taxa = new TaxonList();
            taxa.Add(CoreData.TaxonManager.GetTaxon(GetUserContext(), (Int32)(TaxonId.Mammals)));
            searchCriteria = new TaxonRelationSearchCriteria();
            searchCriteria.IsMainRelation = null;
            searchCriteria.IsValid = null;
            searchCriteria.Taxa = taxa;
            searchCriteria.Scope = TaxonRelationSearchScope.AllParentRelations;
            taxonRelations1 = GetTaxonManager().GetTaxonRelations(GetRevisionUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestParentRelations;
            taxonRelations2 = GetTaxonManager().GetTaxonRelations(GetRevisionUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations2.Count < taxonRelations1.Count);

            searchCriteria.Scope = TaxonRelationSearchScope.AllChildRelations;
            taxonRelations1 = GetTaxonManager().GetTaxonRelations(GetRevisionUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestChildRelations;
            taxonRelations2 = GetTaxonManager().GetTaxonRelations(GetRevisionUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations2.Count < taxonRelations1.Count);

            // Test with two taxa.
            taxa = new TaxonList();
            taxa.Add(CoreData.TaxonManager.GetTaxon(GetRevisionUserContext(), (Int32)(TaxonId.Mammals)));
            taxa.Add(CoreData.TaxonManager.GetTaxon(GetRevisionUserContext(), (Int32)(TaxonId.DrumGrasshopper)));
            searchCriteria = new TaxonRelationSearchCriteria();
            searchCriteria.IsMainRelation = null;
            searchCriteria.IsValid = null;
            searchCriteria.Taxa = taxa;
            searchCriteria.Scope = TaxonRelationSearchScope.AllParentRelations;
            taxonRelations1 = GetTaxonManager().GetTaxonRelations(GetRevisionUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestParentRelations;
            taxonRelations2 = GetTaxonManager().GetTaxonRelations(GetRevisionUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations2.Count < taxonRelations1.Count);

            searchCriteria.Scope = TaxonRelationSearchScope.AllChildRelations;
            taxonRelations1 = GetTaxonManager().GetTaxonRelations(GetRevisionUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations1.IsNotEmpty());
            searchCriteria.Scope = TaxonRelationSearchScope.NearestChildRelations;
            taxonRelations2 = GetTaxonManager().GetTaxonRelations(GetRevisionUserContext(), searchCriteria);
            Assert.IsTrue(taxonRelations2.IsNotEmpty());
            Assert.IsTrue(taxonRelations2.Count < taxonRelations1.Count);
        }

        [TestMethod]
        public void GetTaxonRevisionEventType()
        {
            ITaxonRevisionEventType taxonTaxonRevisionEventType;

            foreach (TaxonRevisionEventTypeId taxonRevisionEventTypeId in Enum.GetValues(typeof(TaxonRevisionEventTypeId)))
            {
                taxonTaxonRevisionEventType = GetTaxonManager().GetTaxonRevisionEventType(GetUserContext(), (Int32)taxonRevisionEventTypeId);
                Assert.IsNotNull(taxonTaxonRevisionEventType);
                Assert.AreEqual((Int32)taxonRevisionEventTypeId, taxonTaxonRevisionEventType.Id);
            }

            foreach (TaxonRevisionEventTypeId taxonRevisionEventTypeId in Enum.GetValues(typeof(TaxonRevisionEventTypeId)))
            {
                taxonTaxonRevisionEventType = GetTaxonManager().GetTaxonRevisionEventType(GetUserContext(), taxonRevisionEventTypeId);
                Assert.IsNotNull(taxonTaxonRevisionEventType);
                Assert.AreEqual((Int32)taxonRevisionEventTypeId, taxonTaxonRevisionEventType.Id);
            }
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
        public void GetTaxonRevisionState()
        {
            ITaxonRevisionState taxonTaxonRevisionState;

            foreach (TaxonRevisionStateId taxonRevisionStateId in Enum.GetValues(typeof(TaxonRevisionStateId)))
            {
                taxonTaxonRevisionState = GetTaxonManager().GetTaxonRevisionState(GetUserContext(), (Int32)taxonRevisionStateId);
                Assert.IsNotNull(taxonTaxonRevisionState);
                Assert.AreEqual((Int32)taxonRevisionStateId, taxonTaxonRevisionState.Id);
            }

            foreach (TaxonRevisionStateId taxonRevisionStateId in Enum.GetValues(typeof(TaxonRevisionStateId)))
            {
                taxonTaxonRevisionState = GetTaxonManager().GetTaxonRevisionState(GetUserContext(), taxonRevisionStateId);
                Assert.IsNotNull(taxonTaxonRevisionState);
                Assert.AreEqual((Int32)taxonRevisionStateId, taxonTaxonRevisionState.Id);
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

        [TestMethod]
        public void GetTaxonStatistics()
        {
            int taxonId = 4000107;
            ITaxon taxon;
            TaxonChildStatisticsList taxonStatistics;

            taxon = GetTaxonManager(true).GetTaxon(GetUserContext(), taxonId);
            taxonStatistics = GetTaxonManager().GetTaxonChildStatistics(GetUserContext(), taxon);
            Assert.IsTrue(taxonStatistics.Count > 5);
        }

        [TestMethod]
        public void GetTaxonTreesBySearchCriteria()
        {
            TaxonTreeNodeList taxonTrees;
            ITaxonTreeSearchCriteria searchCriteria;

            // Get a part of the taxon tree.
            GetTaxonManager(true);
            foreach (TaxonTreeSearchScope scope in Enum.GetValues(typeof(TaxonTreeSearchScope)))
            {
                searchCriteria = new TaxonTreeSearchCriteria();
                searchCriteria.IsValidRequired = true;
                searchCriteria.Scope = scope;
                searchCriteria.TaxonIds = new List<Int32>();
                searchCriteria.TaxonIds.Add((Int32)(TaxonId.Mammals));
                taxonTrees = GetTaxonManager().GetTaxonTrees(GetUserContext(), searchCriteria);
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
            taxonTrees = GetTaxonManager().GetTaxonTrees(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxonTrees.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonQualitySummary()
        {
            TaxonChildQualityStatisticsList taxonQualitySummary;
            ITaxon taxon;

            taxon = GetTaxonManager(true).GetTaxon(GetUserContext(), TaxonId.Carnivore);
            taxonQualitySummary = GetTaxonManager().GetTaxonChildQualityStatistics(GetUserContext(), taxon);
            Assert.IsTrue(taxonQualitySummary.Count > 2);
        }

        [TestMethod] 
        public void CanSaveReferenceOnTaxon()
        {
            // Arrange
            ITaxon refTaxon = GetReferenceTaxon();
            ITaxon taxon;
            ITaxonRevisionEvent taxonRevisionEvent = null;

            // Act
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                refTaxon.GetReferenceRelations(GetUserContext()).Add(new ReferenceRelation { Reference = null, ReferenceId = 11});
                GetTaxonManager(true).UpdateTaxon(GetUserContext(), refTaxon, taxonRevisionEvent, null, null);
                taxon = GetTaxonManager(true).GetTaxon(GetUserContext(), refTaxon.Id);

                // Assert
                Assert.AreEqual(taxon.GetReferenceRelations(GetUserContext()).Count, 1);
            }
        }

        [TestMethod]
        public void RevisionCheckOut()
        {
            // Arrange
            ITaxonRevision revision = new TaxonRevision();

            try
            {
                revision.RootTaxon = GetTaxonManager(true).GetTaxon(GetUserContext(), TaxonId.DrumGrasshopper);
                revision.State = GetTaxonManager().GetTaxonRevisionState(GetUserContext(), TaxonRevisionStateId.Created);
                revision.ExpectedEndDate = DateTime.Now;
                revision.ExpectedStartDate = DateTime.Now;
                GetTaxonManager().UpdateTaxonRevision(GetUserContext(), revision);

                // Act
                GetTaxonManager().CheckOutTaxonRevision(GetUserContext(), revision);

                // Assert
                Assert.AreEqual(revision.State.Id, (Int32)TaxonRevisionStateId.Ongoing);
            }
            finally 
            {
                GetTaxonManager().DataSource.DeleteTaxonRevision(GetUserContext(), revision);
            }
        }

        [TestMethod]
        public void CanGetTaxonRelations()
        {
            ITaxonRelationSearchCriteria searchCriteria;
            TaxonRelationList allParentRelations = new TaxonRelationList();
            TaxonRelationList validParentRelations = new TaxonRelationList();
            TaxonRelationList notValidParentRelations = new TaxonRelationList(); 

            Taxon testTaxon = (Taxon)this.GetTaxonManager(true).GetTaxon(this.GetUserContext(), 220691);

            searchCriteria = new TaxonRelationSearchCriteria();
            searchCriteria.IsMainRelation = null;
            searchCriteria.IsValid = null;
            searchCriteria.Scope = TaxonRelationSearchScope.AllParentRelations;

            searchCriteria.Taxa = new TaxonList();
            searchCriteria.Taxa.Add(testTaxon);
            allParentRelations = CoreData.TaxonManager.GetTaxonRelations(this.GetUserContext(), searchCriteria);
            foreach (var relation in allParentRelations.Distinct())
            {
                if (relation.ValidToDate >= DateTime.Now)
                {
                    validParentRelations.Add(relation);
                }
                else
                {
                    notValidParentRelations.Add(relation);
                }
            }
            Assert.IsNotNull(validParentRelations);
            Assert.IsNotNull(notValidParentRelations);
        }

        [TestMethod]
        public void GetTaxonConceptDefinition()
        {
            ITaxon taxon = GetTaxonManager(true).GetTaxon(GetUserContext(), 105572);
            string taxonConceptDefinition = GetTaxonManager().GetTaxonConceptDefinition(GetUserContext(), taxon);
            Assert.IsTrue(taxonConceptDefinition.IsNotEmpty());
        }

        private TaxonManager GetTaxonManager()
        {
            return GetTaxonManager(false);
        }

        private TaxonManager GetTaxonManager(Boolean refresh)
        {
            if (_taxonManager.IsNull() || refresh)
            {
                _taxonManager = new TaxonManager();
                _taxonManager.DataSource = new TaxonDataSource();
                _taxonManager.PesiNameDataSource = new PesiNameDataSource();
            }
            return _taxonManager;
        }

        #region Helper functions

        [TestMethod]
        public void GetLumpSplitEventsByOldReplacedTaxon()
        {
            ITaxonSearchCriteria searchCriteria;
            LumpSplitEventList changes;
            TaxonList taxa;

            searchCriteria = new TaxonSearchCriteria();
            searchCriteria.IsValidTaxon = false;
            taxa = CoreData.TaxonManager.GetTaxa(GetUserContext(), searchCriteria);
            Assert.IsFalse(taxa.IsEmpty());

            foreach (ITaxon taxon in taxa)
            {
                try
                {
                    if (!taxon.IsValid)
                    {
                        changes = CoreData.TaxonManager.GetLumpSplitEventsByOldReplacedTaxon(GetUserContext(), taxon);
                        Debug.WriteLine("Invalid taxon = " + taxon.Id);
                    }
                }
                catch (Exception exception)
                {
                    Assert.IsNotNull(exception);
                }
            }
        }

        /// <summary>
        /// Gets a RevisionE for test TODO: for now this event exist in DB
        /// </summary>
        /// <returns></returns>
        private ITaxonRevision GetReferenceRevision(int i = 0)
        {
            ITaxonRevisionEvent revEvent = new TaxonRevisionEvent();
            IUser user = new User(GetUserContext());
            ITaxonRevision rev = new TaxonRevision();
            user.Id = Settings.Default.TestUserId;

            revEvent.CreatedBy = user.Id;
            revEvent.CreatedDate = DateTime.Now;
            if(i==0 || i==1)
            {
                revEvent.Type = new TaxonRevisionEventType() { Description = "", Id = 1, Identifier = "" };
            
            }
            else if (i==2)
            {
                revEvent.Type = new TaxonRevisionEventType() { Description = "", Id = 2, Identifier = "" };
            
            }
            else
            {
                revEvent.Type = new TaxonRevisionEventType() { Description = "", Id = 3, Identifier = "" };
            
            }
           revEvent.RevisionId = rev.Id;
            List<ITaxonRevisionEvent> revisionEventList = new List<ITaxonRevisionEvent>();
            revisionEventList.Add((TaxonRevisionEvent) revEvent);

            rev.CreatedBy = user.Id;
            rev.CreatedDate = DateTime.Now;
            rev.Description = "My revision no " + i;
            rev.ExpectedEndDate = new DateTime(2447, 08, 01);
            rev.ExpectedStartDate = DateTime.Now;
            if (i == 0 || i==1)
            {
                rev.State = new TaxonRevisionState() { Id = 1, Identifier = TaxonRevisionStateId.Created.ToString() };

            }
            else if (i == 2)
            {
                rev.State = new TaxonRevisionState() { Id = 2, Identifier = TaxonRevisionStateId.Ongoing.ToString() };

            }
            else
            {
                rev.State = new TaxonRevisionState() { Id = 3, Identifier = TaxonRevisionStateId.Closed.ToString() };

            }
            
            rev.SetRevisionEvents(revisionEventList);
           
            return rev;
        }

        /// <summary>
        /// Gets a RevisionEvent for test TODO: for now this event exist in DB
        /// </summary>
        /// <returns></returns>
        private ITaxonRevisionEvent GetReferenceRevisionEvent(int i = 0)
        {
            ITaxonRevisionEvent revEvent = new TaxonRevisionEvent();
            ITaxonRevision rev = new TaxonRevision();
            revEvent.CreatedBy = Settings.Default.TestUserId;
            revEvent.CreatedDate = DateTime.Now;
            revEvent.RevisionId = rev.Id;
            revEvent.Id = i;
            return revEvent;
        }
        
        /// <summary>
        /// Creates a taxon 
        /// </summary>
        /// <returns></returns>
        private ITaxon GetReferenceTaxon()
        {

            ITaxon refTaxon = new Taxon();

            //DateTime createdDate = new DateTime(2004, 01, 20);
            Int32 createdBy = Settings.Default.TestUserId;
            string personName = @"Hölje Soderås";
            DateTime validFromDate = DateTime.MinValue;
            DateTime validToDate = new DateTime(2014, 08, 01);

            //refTaxon.Id = 0;
            refTaxon.Category = CoreData.TaxonManager.GetTaxonCategory(GetUserContext(), TaxonCategoryId.Species);
            refTaxon.CreatedBy = createdBy;
            //refTaxon.CreatedDate = createdDate;
            refTaxon.DataContext = new DataContext(GetUserContext());
            refTaxon.Id = Int32.MinValue;
            refTaxon.ModifiedByPerson = personName;
            refTaxon.ValidFromDate = validFromDate;
            refTaxon.ValidToDate = validToDate;
            refTaxon.IsValid = true;
            refTaxon.IsPublished = false;
            //refTaxon.References = new List<IReferenceRelation>();

            return refTaxon;
        }

        /// <summary>
        /// Creates a taxon out of predefined data when a text as identifier is set to
        /// differentiate taxon from each other. To be used in test cases when
        /// a list of taxon is needed.
        /// </summary>
        /// <returns>WebTaxon </returns>
        private ITaxon GetReferenceTaxon(string text)
        {
            ITaxon refTaxon = new Taxon();

            // First we create a taxon 
            string conceptDefinitionPart = "conceptDefinitionPart" + " " + text;
            string conceptDefinitionFullGenerated = "conceptDefinitionFullGenerated" + " " + text;
            Int32 createdBy = Settings.Default.TestUserId;
            string personName = @"Hölje Soderås";
            DateTime validFromDate = new DateTime(DateTime.Now.Ticks);
            DateTime validToDate = new DateTime(2022, 1, 30);

            // refTaxon.ConceptDefinitionFullGeneratedString = conceptDefinitionFullGenerated;
            refTaxon.PartOfConceptDefinition = conceptDefinitionPart;
            refTaxon.CreatedBy = createdBy;
            refTaxon.DataContext = new DataContext(GetUserContext());
            refTaxon.Id = Int32.MinValue;
            refTaxon.ModifiedByPerson = personName;
            refTaxon.ValidFromDate = validFromDate;
            refTaxon.ValidToDate = validToDate;
            //refTaxon.References = new List<IReferenceRelation>();

            ITaxonName refTaxonName = GetReferenceTaxonName();

            return refTaxon;
        }

        /// <summary>
        /// Create a taxon category for test.
        /// </summary>
        /// <returns></returns>
        private ITaxonCategory GetReferenceTaxonCategory(int i = 0)
        {
            ITaxonCategory refTaxonCategory = new TaxonCategory();
            // First we create a taxon category that we later use...
            string categoryName = "Svenskt" + i;
            Int32 parentCategory = 2 + i;
            Int32 sortOrder = 20 + i;
            bool mainCategory = true;
            bool taxonomic = true;
            Int32 categoryId = 1230 + i;

            refTaxonCategory.DataContext = new DataContext(GetUserContext());
            refTaxonCategory.Name = categoryName;
            refTaxonCategory.Id = categoryId;
            refTaxonCategory.IsMainCategory = mainCategory;
            refTaxonCategory.ParentId = parentCategory;
            refTaxonCategory.SortOrder = sortOrder;
            refTaxonCategory.IsTaxonomic = taxonomic;

            return refTaxonCategory;
        }

        /// <summary>
        /// Creates a taxon name
        /// </summary>
        /// <returns></returns>
        private ITaxonName GetReferenceTaxonName()
        {
            ITaxonName refTaxonName = new TaxonName();

            //DateTime validFromDate = new DateTime(DateTime.Now.Ticks);
            //DateTime validToDate = new DateTime(2022, 1, 30);
            refTaxonName.DataContext = new DataContext(GetUserContext());
            refTaxonName.Taxon = GetReferenceTaxon();
            refTaxonName.Description= "test description";
            refTaxonName.Name = "TestTaxonName sci";
            refTaxonName.Category = CoreData.TaxonManager.GetTaxonNameCategory(GetUserContext(), (Int32)TaxonNameCategoryId.SwedishName);
            refTaxonName.Status = new TaxonNameStatus();
            refTaxonName.Status.DataContext = new DataContext(GetUserContext());
            refTaxonName.Status.Id = 0;
            refTaxonName.IsOkForSpeciesObservation = true;
            refTaxonName.IsPublished = false;
            refTaxonName.IsRecommended = true;
            refTaxonName.IsUnique = false;
            refTaxonName.CreatedBy = Settings.Default.TestUserId;
            refTaxonName.ModifiedByPerson = "Test PersonName";
            //refTaxonName.ValidFromDate = validFromDate;
            //refTaxonName.ValidToDate = validToDate;
            //refTaxonName.TaxonRevisionEvent = new TaxonRevisionEvent();
            //refTaxonName.TaxonRevisionEvent.Id = 1;
            refTaxonName.SetReferences(new List<IReferenceRelation>());

            return refTaxonName;
        }

        //[TestMethod]
        //public void CanGetDistinctParentsLoadedToViewModel()
        //{
        //    var taxon = GetTaxonManager().GetTaxonById(GetUserContext(), 230260);
        //    var t = taxon.GetConceptDefinitionFullGeneratedString(GetUserContext());
            
        //    var distinctParentTaxa = taxon.AllParentTaxa.GroupBy(x => x.RelatedTaxon.Id).Select(x => new TestViewModel(x.First().RelatedTaxon, x.First().RelatedTaxon.Category.Id, string.Empty)).ToList();
            

        //}

        class TestViewModel
        {
            public TestViewModel(ITaxon taxon, int category, string test)
            {
                
            }
        }

        /// <summary>
        /// Creates a new Revision and make CheckOut
        /// </summary>
        /// <param name="taxon">The taxonId</param>
        /// <returns></returns>
        private TaxonRevision GetRevisionInOngoingState(ITaxon taxon)
        {
            var revision = new TaxonRevision();
            revision.RootTaxon = taxon;
            revision.State = new TaxonRevisionState() { Id = 1, Identifier = TaxonRevisionStateId.Created.ToString() };
            revision.ExpectedEndDate = DateTime.Now;
            revision.ExpectedStartDate = DateTime.Now;
            revision.CreatedBy = GetUserContext().User.Id;
            revision.CreatedDate = DateTime.Now;
            revision.SetReferences(new List<IReferenceRelation>());
            revision.SetRevisionEvents(new List<ITaxonRevisionEvent>());
            GetTaxonManager().UpdateTaxonRevision(GetUserContext(), revision);
            GetTaxonManager().CheckOutTaxonRevision(GetUserContext(), revision);
            return revision;
        }


        #endregion

        /// <summary>
        /// Creates a taxon (id 2000446) that matches/exist in Taxon DB 2011-09-27.
        /// </summary>
        /// <returns></returns>
        private ITaxon GetReferenceTaxon2()
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
    }
}

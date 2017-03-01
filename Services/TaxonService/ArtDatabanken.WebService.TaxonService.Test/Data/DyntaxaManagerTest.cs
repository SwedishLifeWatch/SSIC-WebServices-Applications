using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Data;
using ArtDatabanken.WebService.TaxonService.Test.TestFactories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxonManager = ArtDatabanken.WebService.Data.TaxonManager;

namespace ArtDatabanken.WebService.TaxonService.Test.Data
{
    /// <summary>
    /// DyntaxaManager tests.
    /// </summary>
    [TestClass]
    public class DyntaxaManagerTest : TestBase
    {
        public DyntaxaManagerTest()
            : base(useTransaction, 50)
        {
        }

        private TestContext testContextInstance;
        private static Boolean useTransaction = true;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }


        [TestMethod]        
        public void CreateCompleteRevisionEvent_WithValidData_ThenRevisionEventIsCreated()
        {            
            Int32 revisionId = 1;
            String affectedTaxa, oldValue, newValue;
            WebTaxonRevisionEvent revisionEvent, newRevisionEvent;

            revisionEvent = new WebTaxonRevisionEvent()
            {
                CreatedBy = 2,
                CreatedDate = DateTime.Now,
                TypeId = 1,
                RevisionId = revisionId,
                AffectedTaxa = "Straminergon [1004721]",
                OldValue = "My old value",
                NewValue = "My new value"                           
            };
            newRevisionEvent = DyntaxaManager.CreateCompleteRevisionEvent(GetRevisionContext(), revisionEvent);            

            Assert.AreEqual(revisionEvent.CreatedBy, newRevisionEvent.CreatedBy);
            Assert.AreEqual(revisionEvent.TypeId, newRevisionEvent.TypeId);
            Assert.AreEqual(revisionEvent.OldValue, newRevisionEvent.OldValue);
            Assert.AreEqual(revisionEvent.NewValue, newRevisionEvent.NewValue);
            Assert.AreNotEqual(revisionEvent.Id, newRevisionEvent.Id);


            affectedTaxa = "Strami ' nergon [1004721]";
            oldValue = "My old ' value";
            newValue = "My new ' value";
            revisionEvent = new WebTaxonRevisionEvent()
            {
                CreatedBy = 2,
                CreatedDate = DateTime.Now,
                TypeId = 1,
                RevisionId = revisionId,
                AffectedTaxa = affectedTaxa,
                OldValue = oldValue,
                NewValue = newValue
            };
            newRevisionEvent = DyntaxaManager.CreateCompleteRevisionEvent(GetRevisionContext(), revisionEvent);

            Assert.AreEqual(revisionEvent.CreatedBy, newRevisionEvent.CreatedBy);
            Assert.AreEqual(revisionEvent.TypeId, newRevisionEvent.TypeId);
            Assert.AreEqual(oldValue, newRevisionEvent.OldValue);
            Assert.AreEqual(newValue, newRevisionEvent.NewValue);
            Assert.AreNotEqual(revisionEvent.Id, newRevisionEvent.Id);
        }

        [TestMethod]
        public void CreateDyntaxaRevisionSpeciesFact_WithValidData_ThenDyntaxaRevisionSpeciesFactIsCreated()
        {
            WebDyntaxaRevisionSpeciesFact dyntaxaRevisionSpeciesFact, newDyntaxaRevisionSpeciesFact;
            
            // Create test Dyntaxa revision species fact.
            dyntaxaRevisionSpeciesFact = WebDyntaxaSpeciesFactTestFactory.Create((Int32)TaxonId.Bear);
            const int STATUS_ID = 400;
            dyntaxaRevisionSpeciesFact.StatusId = STATUS_ID;            

            newDyntaxaRevisionSpeciesFact = DyntaxaManager.CreateDyntaxaRevisionSpeciesFact(GetRevisionContext(), dyntaxaRevisionSpeciesFact);
            
            Assert.IsNotNull(newDyntaxaRevisionSpeciesFact);
            Assert.IsFalse(dyntaxaRevisionSpeciesFact.SpeciesFactExists);
            Assert.IsNull(dyntaxaRevisionSpeciesFact.OriginalStatusId);
            Assert.AreNotEqual(dyntaxaRevisionSpeciesFact.Id, newDyntaxaRevisionSpeciesFact.Id);            
            Assert.AreEqual(dyntaxaRevisionSpeciesFact.Description, newDyntaxaRevisionSpeciesFact.Description);
            Assert.AreEqual(STATUS_ID, newDyntaxaRevisionSpeciesFact.StatusId);
            Assert.AreEqual(GetContext().GetUser().Id, newDyntaxaRevisionSpeciesFact.CreatedBy);

            dyntaxaRevisionSpeciesFact = WebDyntaxaSpeciesFactTestFactory.Create((Int32)TaxonId.Wolf);
            dyntaxaRevisionSpeciesFact.Description = "Foo'Bar";
            dyntaxaRevisionSpeciesFact.StatusId = STATUS_ID;
            newDyntaxaRevisionSpeciesFact = DyntaxaManager.CreateDyntaxaRevisionSpeciesFact(GetRevisionContext(), dyntaxaRevisionSpeciesFact);

            Assert.IsNotNull(newDyntaxaRevisionSpeciesFact);
            Assert.IsFalse(dyntaxaRevisionSpeciesFact.SpeciesFactExists);
            Assert.IsNull(dyntaxaRevisionSpeciesFact.OriginalStatusId);
            Assert.AreNotEqual(dyntaxaRevisionSpeciesFact.Id, newDyntaxaRevisionSpeciesFact.Id);
            Assert.AreEqual("Foo'Bar", newDyntaxaRevisionSpeciesFact.Description);
            Assert.AreEqual(STATUS_ID, newDyntaxaRevisionSpeciesFact.StatusId);
            Assert.AreEqual(GetContext().GetUser().Id, newDyntaxaRevisionSpeciesFact.CreatedBy);

            dyntaxaRevisionSpeciesFact = WebDyntaxaSpeciesFactTestFactory.Create((Int32)TaxonId.Wolverine);
            dyntaxaRevisionSpeciesFact.OriginalDescription = "Foo'Bar";
            dyntaxaRevisionSpeciesFact.StatusId = STATUS_ID;
            newDyntaxaRevisionSpeciesFact = DyntaxaManager.CreateDyntaxaRevisionSpeciesFact(GetRevisionContext(), dyntaxaRevisionSpeciesFact);

            Assert.IsNotNull(newDyntaxaRevisionSpeciesFact);
            Assert.IsFalse(dyntaxaRevisionSpeciesFact.SpeciesFactExists);
            Assert.IsNull(dyntaxaRevisionSpeciesFact.OriginalStatusId);
            Assert.AreNotEqual(dyntaxaRevisionSpeciesFact.Id, newDyntaxaRevisionSpeciesFact.Id);
            Assert.AreEqual(dyntaxaRevisionSpeciesFact.Description, newDyntaxaRevisionSpeciesFact.Description);
            Assert.AreEqual(STATUS_ID, newDyntaxaRevisionSpeciesFact.StatusId);
            Assert.AreEqual(GetContext().GetUser().Id, newDyntaxaRevisionSpeciesFact.CreatedBy);
            Assert.AreEqual("Foo'Bar", newDyntaxaRevisionSpeciesFact.OriginalDescription);
        }

        [TestMethod]
        public void CreateDyntaxaRevisionSpeciesFact_WithSpeciesFactStatusNullData_ThenDyntaxaRevisionSpeciesFactIsCreated()
        {
            WebDyntaxaRevisionSpeciesFact dyntaxaRevisionSpeciesFact, newDyntaxaRevisionSpeciesFact;

            // Create test Dyntaxa revision species fact.
            dyntaxaRevisionSpeciesFact = WebDyntaxaSpeciesFactTestFactory.Create((Int32)TaxonId.Bear);
            const int STATUS_ID = 400;
            dyntaxaRevisionSpeciesFact.StatusId = STATUS_ID;
            dyntaxaRevisionSpeciesFact.StatusId = null;
            dyntaxaRevisionSpeciesFact.QualityId = null;
            dyntaxaRevisionSpeciesFact.ReferenceId = null;
            
            dyntaxaRevisionSpeciesFact.SpeciesFactExists = true;
            dyntaxaRevisionSpeciesFact.OriginalStatusId = STATUS_ID;
            dyntaxaRevisionSpeciesFact.OriginalQualityId = 10;
            dyntaxaRevisionSpeciesFact.OriginalReferenceId = 23;
            dyntaxaRevisionSpeciesFact.OriginalDescription = "test";
            
            newDyntaxaRevisionSpeciesFact = DyntaxaManager.CreateDyntaxaRevisionSpeciesFact(GetRevisionContext(), dyntaxaRevisionSpeciesFact);

            Assert.IsNotNull(newDyntaxaRevisionSpeciesFact);
            Assert.IsTrue(dyntaxaRevisionSpeciesFact.SpeciesFactExists);
            Assert.IsNull(dyntaxaRevisionSpeciesFact.StatusId);
            Assert.AreNotEqual(dyntaxaRevisionSpeciesFact.Id, newDyntaxaRevisionSpeciesFact.Id);
            Assert.AreEqual(dyntaxaRevisionSpeciesFact.Description, newDyntaxaRevisionSpeciesFact.Description);
            Assert.AreEqual(STATUS_ID, newDyntaxaRevisionSpeciesFact.OriginalStatusId);
            Assert.AreEqual(GetContext().GetUser().Id, newDyntaxaRevisionSpeciesFact.CreatedBy);
        }





        [TestMethod]
        public void GetDyntaxaRevisionSpeciesFactById_WhenDyntaxaRevisionSpeciesFactCreated_ThenCreatedRevisionSpeciesFactIsReturned()
        {
            WebDyntaxaRevisionSpeciesFact dyntaxaRevisionSpeciesFact, newDyntaxaRevisionSpeciesFact, getDyntaxaRevisionSpeciesFact;

            // Create test Dyntaxa revision species fact.
            dyntaxaRevisionSpeciesFact = WebDyntaxaSpeciesFactTestFactory.Create((Int32)TaxonId.Bear);
            const int STATUS_ID = 400;
            dyntaxaRevisionSpeciesFact.StatusId = STATUS_ID;
            dyntaxaRevisionSpeciesFact.SpeciesFactExists = true;
            dyntaxaRevisionSpeciesFact.OriginalStatusId = 380;
            dyntaxaRevisionSpeciesFact.OriginalQualityId = 150;
            dyntaxaRevisionSpeciesFact.OriginalReferenceId = 250;
            dyntaxaRevisionSpeciesFact.OriginalDescription = "test";            

            newDyntaxaRevisionSpeciesFact = DyntaxaManager.CreateDyntaxaRevisionSpeciesFact(GetRevisionContext(), dyntaxaRevisionSpeciesFact);

            getDyntaxaRevisionSpeciesFact = DyntaxaManager.GetDyntaxaRevisionSpeciesFactById(GetRevisionContext(), newDyntaxaRevisionSpeciesFact.Id);
            Assert.IsNotNull(getDyntaxaRevisionSpeciesFact);
            Assert.AreEqual(STATUS_ID, getDyntaxaRevisionSpeciesFact.StatusId);   
            Assert.IsTrue(dyntaxaRevisionSpeciesFact.SpeciesFactExists);
            Assert.AreEqual(380, dyntaxaRevisionSpeciesFact.OriginalStatusId);
            Assert.AreEqual(150, dyntaxaRevisionSpeciesFact.OriginalQualityId);
            Assert.AreEqual(250, dyntaxaRevisionSpeciesFact.OriginalReferenceId);
            Assert.AreEqual("test", dyntaxaRevisionSpeciesFact.OriginalDescription);
        }

        [TestMethod]
        public void GetDyntaxaRevisionSpeciesFact_WhenDyntaxaRevisionSpeciesFactCreatedWithSpeciesFactExistsData_ThenCreatedRevisionSpeciesFactIsReturned()
        {
            WebDyntaxaRevisionSpeciesFact dyntaxaRevisionSpeciesFact, newDyntaxaRevisionSpeciesFact, getDyntaxaRevisionSpeciesFact;

            // Create test Dyntaxa revision species fact.
            dyntaxaRevisionSpeciesFact = WebDyntaxaSpeciesFactTestFactory.Create((Int32)TaxonId.Bear);
            const int STATUS_ID = 400;
            dyntaxaRevisionSpeciesFact.StatusId = STATUS_ID;
            dyntaxaRevisionSpeciesFact.SpeciesFactExists = true;
            dyntaxaRevisionSpeciesFact.OriginalStatusId = 380;
            dyntaxaRevisionSpeciesFact.OriginalQualityId = 150;
            dyntaxaRevisionSpeciesFact.OriginalReferenceId = 250;
            dyntaxaRevisionSpeciesFact.OriginalDescription = "test";

            newDyntaxaRevisionSpeciesFact = DyntaxaManager.CreateDyntaxaRevisionSpeciesFact(GetRevisionContext(), dyntaxaRevisionSpeciesFact);

            getDyntaxaRevisionSpeciesFact = DyntaxaManager.GetDyntaxaRevisionSpeciesFact(GetRevisionContext(), dyntaxaRevisionSpeciesFact.FactorId, dyntaxaRevisionSpeciesFact.TaxonId, dyntaxaRevisionSpeciesFact.RevisionId);
            Assert.IsNotNull(getDyntaxaRevisionSpeciesFact);
            Assert.AreEqual(STATUS_ID, getDyntaxaRevisionSpeciesFact.StatusId);
            Assert.IsTrue(dyntaxaRevisionSpeciesFact.SpeciesFactExists);
            Assert.AreEqual(380, dyntaxaRevisionSpeciesFact.OriginalStatusId);
            Assert.AreEqual(150, dyntaxaRevisionSpeciesFact.OriginalQualityId);
            Assert.AreEqual(250, dyntaxaRevisionSpeciesFact.OriginalReferenceId);
            Assert.AreEqual("test", dyntaxaRevisionSpeciesFact.OriginalDescription);
        }

        [TestMethod]
        public void GetDyntaxaRevisionSpeciesFact_WhenSpeciesFactExists_ThenSpeciesFactIsReturned()
        {
            WebDyntaxaRevisionSpeciesFact dyntaxaRevisionSpeciesFact, newDyntaxaRevisionSpeciesFact, getDyntaxaRevisionSpeciesFact;
            const int STATUS_ID = 400;

            // Create test Dyntaxa revision species fact.
            dyntaxaRevisionSpeciesFact = WebDyntaxaSpeciesFactTestFactory.Create((Int32)TaxonId.Bear);
            dyntaxaRevisionSpeciesFact.FactorId = (Int32) FactorId.SwedishHistory;
            dyntaxaRevisionSpeciesFact.RevisionId = 24;
            dyntaxaRevisionSpeciesFact.IsPublished = false;
            dyntaxaRevisionSpeciesFact.StatusId = STATUS_ID;
            newDyntaxaRevisionSpeciesFact = DyntaxaManager.CreateDyntaxaRevisionSpeciesFact(GetRevisionContext(), dyntaxaRevisionSpeciesFact);
            
            getDyntaxaRevisionSpeciesFact = DyntaxaManager.GetDyntaxaRevisionSpeciesFact(
                GetRevisionContext(), (Int32) FactorId.SwedishHistory, (Int32) TaxonId.Bear, 24);
            Assert.IsNotNull(getDyntaxaRevisionSpeciesFact);            
            Assert.AreEqual(STATUS_ID, getDyntaxaRevisionSpeciesFact.StatusId);
        }

        [TestMethod]
        public void GetDyntaxaRevisionSpeciesFact_WhenWrongRevision_ThenNullIsReturned()
        {
            WebDyntaxaRevisionSpeciesFact dyntaxaRevisionSpeciesFact, newDyntaxaRevisionSpeciesFact, getDyntaxaRevisionSpeciesFact;
            const int STATUS_ID = 400;

            // Create test Dyntaxa revision species fact.
            dyntaxaRevisionSpeciesFact = WebDyntaxaSpeciesFactTestFactory.Create((Int32)TaxonId.Bear);
            dyntaxaRevisionSpeciesFact.FactorId = (Int32)FactorId.SwedishHistory;
            dyntaxaRevisionSpeciesFact.RevisionId = 24;
            dyntaxaRevisionSpeciesFact.IsPublished = false;
            dyntaxaRevisionSpeciesFact.StatusId = STATUS_ID;
            newDyntaxaRevisionSpeciesFact = DyntaxaManager.CreateDyntaxaRevisionSpeciesFact(GetRevisionContext(), dyntaxaRevisionSpeciesFact);

            getDyntaxaRevisionSpeciesFact = DyntaxaManager.GetDyntaxaRevisionSpeciesFact(
                GetRevisionContext(), (Int32)FactorId.SwedishHistory, (Int32)TaxonId.Bear, 25);
            Assert.IsNull(getDyntaxaRevisionSpeciesFact);
        }

        [TestMethod]
        public void GetDyntaxaRevisionSpeciesFact_WhenWrongTaxonId_ThenNullIsReturned()
        {
            WebDyntaxaRevisionSpeciesFact dyntaxaRevisionSpeciesFact, newDyntaxaRevisionSpeciesFact, getDyntaxaRevisionSpeciesFact;
            const int STATUS_ID = 400;

            // Create test Dyntaxa revision species fact.
            dyntaxaRevisionSpeciesFact = WebDyntaxaSpeciesFactTestFactory.Create((Int32)TaxonId.Bear);
            dyntaxaRevisionSpeciesFact.FactorId = (Int32)FactorId.SwedishHistory;
            dyntaxaRevisionSpeciesFact.RevisionId = 24;
            dyntaxaRevisionSpeciesFact.IsPublished = false;
            dyntaxaRevisionSpeciesFact.StatusId = STATUS_ID;
            newDyntaxaRevisionSpeciesFact = DyntaxaManager.CreateDyntaxaRevisionSpeciesFact(GetRevisionContext(), dyntaxaRevisionSpeciesFact);

            getDyntaxaRevisionSpeciesFact = DyntaxaManager.GetDyntaxaRevisionSpeciesFact(
                GetRevisionContext(), (Int32)FactorId.SwedishHistory, (Int32)TaxonId.Wolf, 24);
            Assert.IsNull(getDyntaxaRevisionSpeciesFact);
        }

        [TestMethod]
        public void GetDyntaxaRevisionSpeciesFact_WhenWrongFactorId_ThenNullIsReturned()
        {
            WebDyntaxaRevisionSpeciesFact dyntaxaRevisionSpeciesFact, newDyntaxaRevisionSpeciesFact, getDyntaxaRevisionSpeciesFact;
            const int STATUS_ID = 400;

            // Create test Dyntaxa revision species fact.
            dyntaxaRevisionSpeciesFact = WebDyntaxaSpeciesFactTestFactory.Create((Int32)TaxonId.Bear);
            dyntaxaRevisionSpeciesFact.FactorId = (Int32)FactorId.SwedishHistory;
            dyntaxaRevisionSpeciesFact.RevisionId = 24;
            dyntaxaRevisionSpeciesFact.IsPublished = false;
            dyntaxaRevisionSpeciesFact.StatusId = STATUS_ID;
            newDyntaxaRevisionSpeciesFact = DyntaxaManager.CreateDyntaxaRevisionSpeciesFact(GetRevisionContext(), dyntaxaRevisionSpeciesFact);

            getDyntaxaRevisionSpeciesFact = DyntaxaManager.GetDyntaxaRevisionSpeciesFact(
                GetRevisionContext(), (Int32)FactorId.SwedishOccurrence, (Int32)TaxonId.Bear, 24);
            Assert.IsNull(getDyntaxaRevisionSpeciesFact);
        }

        [TestMethod]
        public void GetDyntaxaRevisionSpeciesFact_WhenRevisionSpeciesFactIsCreatedAndLaterChanged_ThenLastChangedRevisionSpeciesFactIsReturned()
        {
            WebDyntaxaRevisionSpeciesFact dyntaxaRevisionSpeciesFact1, getDyntaxaRevisionSpeciesFact1, dyntaxaRevisionSpeciesFact2, getDyntaxaRevisionSpeciesFact2, currentDyntaxaRevisionSpeciesFact;
            WebTaxonRevisionEvent revisionEvent1, revisionEvent2;
            Int32 revisionId = 1;

            //-------------------------------------------------------------------
            // Create first dyntaxa revision species fact and revision event.
            dyntaxaRevisionSpeciesFact1 = WebDyntaxaSpeciesFactTestFactory.Create((Int32)TaxonId.Bear);
            dyntaxaRevisionSpeciesFact1.FactorId = (Int32)FactorId.SwedishHistory;
            dyntaxaRevisionSpeciesFact1.RevisionId = revisionId;
            dyntaxaRevisionSpeciesFact1.IsPublished = false;
            dyntaxaRevisionSpeciesFact1.StatusId = 400;

            revisionEvent1 = new WebTaxonRevisionEvent()
            {
                CreatedBy = 2,
                CreatedDate = DateTime.Now,
                TypeId = 1,
                RevisionId = revisionId,
                AffectedTaxa = "Bear",
                OldValue = "StatusId=Unknown",
                NewValue = "StatusId=400"
            };
            revisionEvent1 = DyntaxaManager.CreateCompleteRevisionEvent(GetRevisionContext(), revisionEvent1);
            dyntaxaRevisionSpeciesFact1.RevisionEventId = revisionEvent1.Id;
            dyntaxaRevisionSpeciesFact1 = DyntaxaManager.CreateDyntaxaRevisionSpeciesFact(GetRevisionContext(), dyntaxaRevisionSpeciesFact1);            
            currentDyntaxaRevisionSpeciesFact = DyntaxaManager.GetDyntaxaRevisionSpeciesFact(
                GetRevisionContext(), (Int32)FactorId.SwedishHistory, (Int32)TaxonId.Bear, revisionId);

            //Assert first Insert
            Assert.AreEqual(revisionEvent1.Id, dyntaxaRevisionSpeciesFact1.RevisionEventId);            
            Assert.IsFalse(currentDyntaxaRevisionSpeciesFact.IsChangedInRevisionEventIdSpecified);
            Assert.AreEqual(400, currentDyntaxaRevisionSpeciesFact.StatusId);
            Assert.AreEqual(dyntaxaRevisionSpeciesFact1.StatusId, currentDyntaxaRevisionSpeciesFact.StatusId);            

            //---------------------------------------------------------------------
            // Create second dyntaxa revision species fact and revision event.
            dyntaxaRevisionSpeciesFact2 = WebDyntaxaSpeciesFactTestFactory.Create((Int32)TaxonId.Bear);
            dyntaxaRevisionSpeciesFact2.FactorId = (Int32)FactorId.SwedishHistory;
            dyntaxaRevisionSpeciesFact2.RevisionId = revisionId;
            dyntaxaRevisionSpeciesFact2.IsPublished = false;
            dyntaxaRevisionSpeciesFact2.StatusId = 450; // changed from 400 to 450.

            revisionEvent2 = new WebTaxonRevisionEvent()
            {
                CreatedBy = 3, //Changed
                CreatedDate = DateTime.Now,
                TypeId = 1,
                RevisionId = revisionId,
                AffectedTaxa = "Bear",
                OldValue = "StatusId=400", //Changed
                NewValue = "StatusId=450" //Changed
            };
            revisionEvent2 = DyntaxaManager.CreateCompleteRevisionEvent(GetRevisionContext(), revisionEvent2);
            dyntaxaRevisionSpeciesFact2.RevisionEventId = revisionEvent2.Id;
            dyntaxaRevisionSpeciesFact2 = DyntaxaManager.CreateDyntaxaRevisionSpeciesFact(GetRevisionContext(), dyntaxaRevisionSpeciesFact2);
            currentDyntaxaRevisionSpeciesFact = DyntaxaManager.GetDyntaxaRevisionSpeciesFact(
                GetRevisionContext(), (Int32)FactorId.SwedishHistory, (Int32)TaxonId.Bear, revisionId);
            getDyntaxaRevisionSpeciesFact1 = DyntaxaManager.GetDyntaxaRevisionSpeciesFactById(GetRevisionContext(), dyntaxaRevisionSpeciesFact1.Id);
            getDyntaxaRevisionSpeciesFact2 = DyntaxaManager.GetDyntaxaRevisionSpeciesFactById(GetRevisionContext(), dyntaxaRevisionSpeciesFact2.Id);
                
            // Check values.
            Assert.AreEqual(450, currentDyntaxaRevisionSpeciesFact.StatusId);
            Assert.AreEqual(getDyntaxaRevisionSpeciesFact2.StatusId, currentDyntaxaRevisionSpeciesFact.StatusId);
            Assert.AreEqual(400, getDyntaxaRevisionSpeciesFact1.StatusId);
            Assert.AreNotEqual(getDyntaxaRevisionSpeciesFact1.StatusId, currentDyntaxaRevisionSpeciesFact.StatusId);

            Assert.AreEqual(revisionEvent1.Id, getDyntaxaRevisionSpeciesFact1.RevisionEventId);
            Assert.AreEqual(revisionEvent2.Id, getDyntaxaRevisionSpeciesFact1.ChangedInRevisionEventId);
            Assert.AreEqual(revisionEvent2.Id, currentDyntaxaRevisionSpeciesFact.RevisionEventId);
            Assert.IsFalse(currentDyntaxaRevisionSpeciesFact.IsChangedInRevisionEventIdSpecified);
        }

        [TestMethod]
        public void GetAllDyntaxaRevisionSpeciesFacts_WhenSpeciesFactExists_ThenSpeciesFactIsReturned()
        {
            WebDyntaxaRevisionSpeciesFact dyntaxaRevisionSpeciesFact, getDyntaxaRevisionSpeciesFact;
            List<WebDyntaxaRevisionSpeciesFact> dyntaxaRevisionSpeciesFacts;
            const int STATUS_ID = 400;

            // Create test Dyntaxa revision species fact.
            dyntaxaRevisionSpeciesFact = WebDyntaxaSpeciesFactTestFactory.Create((Int32)TaxonId.Bear);
            dyntaxaRevisionSpeciesFact.FactorId = (Int32)FactorId.SwedishHistory;
            dyntaxaRevisionSpeciesFact.RevisionId = 24;
            dyntaxaRevisionSpeciesFact.IsPublished = false;
            dyntaxaRevisionSpeciesFact.StatusId = STATUS_ID;
            DyntaxaManager.CreateDyntaxaRevisionSpeciesFact(GetRevisionContext(), dyntaxaRevisionSpeciesFact);

            dyntaxaRevisionSpeciesFacts = DyntaxaManager.GetAllDyntaxaRevisionSpeciesFacts(
                GetRevisionContext(), 24);

            Assert.AreEqual(1, dyntaxaRevisionSpeciesFacts.Count);
        }        

        [TestMethod]
        public void SetRevisionSpeciesFactPublished()
        {
            const int STATUS_ID = 400;

            // Create test Dyntaxa revision species fact.
            var dyntaxaRevisionSpeciesFact = WebDyntaxaSpeciesFactTestFactory.Create((Int32)TaxonId.Bear);
            dyntaxaRevisionSpeciesFact.FactorId = (Int32)FactorId.SwedishHistory;
            dyntaxaRevisionSpeciesFact.RevisionId = 24;
            dyntaxaRevisionSpeciesFact.IsPublished = false;
            dyntaxaRevisionSpeciesFact.StatusId = STATUS_ID;
            DyntaxaManager.CreateDyntaxaRevisionSpeciesFact(GetRevisionContext(), dyntaxaRevisionSpeciesFact);
            
            var result = DyntaxaManager.SetRevisionSpeciesFactPublished(GetRevisionContext(), 24);

            Assert.AreEqual(true, result);
        }


        [TestMethod]
        public void CreateDyntaxaRevisionReferenceRelation_WithValidData_ThenDyntaxaRevisionReferenceRelationIsCreated()
        {
            // Arrange
            const string relatedObjectGUID = "urn:lsid:dyntaxa.se:Taxon:6010174";
            const string action = "Add";
            const int revisionId = 1;
            WebDyntaxaRevisionReferenceRelation dyntaxaReferenceRelation, newDyntaxaReferenceRelation;            
            dyntaxaReferenceRelation = WebDyntaxaReferenceRelationTestFactory.Create(
                revisionId, relatedObjectGUID, action);

            // Act
            // Create test Dyntaxa revision reference relation.
            dyntaxaReferenceRelation.ReferenceId = 3342;           
            newDyntaxaReferenceRelation = DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(
                GetRevisionContext(), 
                dyntaxaReferenceRelation);

            // Assert
            Assert.IsNotNull(newDyntaxaReferenceRelation);            
            Assert.AreNotEqual(dyntaxaReferenceRelation.Id, newDyntaxaReferenceRelation.Id);
            Assert.AreEqual(dyntaxaReferenceRelation.RelatedObjectGUID, newDyntaxaReferenceRelation.RelatedObjectGUID);
            Assert.AreEqual(3342, newDyntaxaReferenceRelation.ReferenceId);
            Assert.AreEqual(GetContext().GetUser().Id, newDyntaxaReferenceRelation.CreatedBy);
        }

        [TestMethod]
        public void GetAllDyntaxaRevisionReferenceRelations_WhenReferenceRelationExists_ThenReferenceRelationIsReturned()
        {
            //Arrange            
            List<WebDyntaxaRevisionReferenceRelation> referenceRelations;
            const int revisionId = 24;
            const string relatedObjectGUID = "urn:lsid:dyntaxa.se:Taxon:6010174";
            const string action = "Add";
            var dyntaxaReferenceRelation = WebDyntaxaReferenceRelationTestFactory.Create(
                revisionId, relatedObjectGUID, action);            
            DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(GetRevisionContext(), dyntaxaReferenceRelation);

            // Act            
            referenceRelations = DyntaxaManager.GetAllDyntaxaRevisionReferenceRelations(
                GetRevisionContext(), revisionId);

            // Assert
            Assert.AreEqual(1, referenceRelations.Count);
        }

        [TestMethod]
        public void GetDyntaxaRevisionReferenceRelation_WhenReferenceRelationExists_ThenReferenceRelationIsReturned()
        {
            // Arrange            
            const int revisionId = 24;
            const string relatedObjectGUID = "urn:lsid:dyntaxa.se:Taxon:6010174";
            const string action = "Add";
            var dyntaxaReferenceRelation = WebDyntaxaReferenceRelationTestFactory.Create(
                revisionId, relatedObjectGUID, action);
            DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(GetRevisionContext(), dyntaxaReferenceRelation);

            // Act
            var newReferenceRelations = DyntaxaManager.GetDyntaxaRevisionReferenceRelation(
                GetRevisionContext(), revisionId, relatedObjectGUID);

            // Assert
            Assert.IsNotNull(newReferenceRelations);
            Assert.AreEqual(1, newReferenceRelations.Count);
            Assert.AreEqual(action, newReferenceRelations[0].Action);
        }

        [TestMethod]
        public void GetDyntaxaRevisionReferenceRelation_WhenWrongRevisionId_ThenEmptyListIsReturned()
        {
            // Arrange            
            const int revisionId = 24;
            const int wrongRevisionId = 25;
            const string relatedObjectGUID = "urn:lsid:dyntaxa.se:Taxon:6010174";
            const string action = "Add";
            var dyntaxaReferenceRelation = WebDyntaxaReferenceRelationTestFactory.Create(
                revisionId, relatedObjectGUID, action);
            DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(GetRevisionContext(), dyntaxaReferenceRelation);

            // Act
            var newReferenceRelations = DyntaxaManager.GetDyntaxaRevisionReferenceRelation(
                GetRevisionContext(), wrongRevisionId, relatedObjectGUID);

            // Assert
            Assert.IsNotNull(newReferenceRelations);
            Assert.AreEqual(0, newReferenceRelations.Count);
        }


        [TestMethod]
        public void GetDyntaxaRevisionReferenceRelation_WhenTwoDifferentReferenceRelationExists_ThenTwoDifferentReferenceRelationIsReturned()
        {
            // Arrange            
            const int revisionId = 24;
            const string rel1RelatedObjectGUID = "urn:lsid:dyntaxa.se:Taxon:6010174";
            const string rel1Action = "Add";
            const string rel2RelatedObjectGUID = "urn:lsid:dyntaxa.se:Taxon:4000107";
            const string rel2Action = "Modify";            

            DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(
                GetRevisionContext(),
                WebDyntaxaReferenceRelationTestFactory.Create(revisionId, rel1RelatedObjectGUID, rel1Action));
            DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(
                GetRevisionContext(),
                WebDyntaxaReferenceRelationTestFactory.Create(revisionId, rel2RelatedObjectGUID, rel2Action));

            // Act
            var newReferenceRelations = DyntaxaManager.GetDyntaxaRevisionReferenceRelation(
                GetRevisionContext(), revisionId, rel1RelatedObjectGUID);

            // Assert
            Assert.IsNotNull(newReferenceRelations);
            Assert.AreEqual(1, newReferenceRelations.Count);
            Assert.AreEqual(rel1Action, newReferenceRelations[0].Action);            
        }

        [TestMethod]
        public void GetDyntaxaRevisionReferenceRelation_WhenTwoReferenceRelationWithSameRelatedObjectGUIDExists_ThenTwoReferenceRelationIsReturned()
        {
            // Arrange            
            const int revisionId = 24;
            const string relatedObjectGUID = "urn:lsid:dyntaxa.se:Taxon:6010174";
            const string rel1Action = "Add";            
            const string rel2Action = "Delete";

            DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(
                GetRevisionContext(),
                WebDyntaxaReferenceRelationTestFactory.Create(revisionId, relatedObjectGUID, rel1Action));
            DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(
                GetRevisionContext(),
                WebDyntaxaReferenceRelationTestFactory.Create(revisionId, relatedObjectGUID, rel2Action));

            // Act
            var newReferenceRelations = DyntaxaManager.GetDyntaxaRevisionReferenceRelation(
                GetRevisionContext(), revisionId, relatedObjectGUID);

            // Assert
            Assert.IsNotNull(newReferenceRelations);
            Assert.AreEqual(2, newReferenceRelations.Count);
            Assert.AreEqual(rel1Action, newReferenceRelations[0].Action);
            Assert.AreEqual(rel2Action, newReferenceRelations[1].Action);
        }

        [TestMethod]
        public void GetDyntaxaRevisionReferenceRelationById_WhenDyntaxaRevisionReferenceRelationCreated_ThenCreatedRevisionReferenceRelationIsReturned()
        {
            // Arrange
            const int revisionId = 24;
            const string relatedObjectGUID = "urn:lsid:dyntaxa.se:Taxon:6010174";
            const string action = "Add";
            const int referenceId = 252;
            WebDyntaxaRevisionReferenceRelation dyntaxaReferenceRelation = WebDyntaxaReferenceRelationTestFactory.Create(
                revisionId, relatedObjectGUID, action);
            dyntaxaReferenceRelation.ReferenceId = referenceId;
            var newReferenceRelations = DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(
                GetRevisionContext(), dyntaxaReferenceRelation);

            // Act
            WebDyntaxaRevisionReferenceRelation getReferenceRelations = DyntaxaManager.GetDyntaxaRevisionReferenceRelationById(
                GetRevisionContext(), newReferenceRelations.Id);

            // Assert
            Assert.IsNotNull(getReferenceRelations);
            Assert.AreEqual(relatedObjectGUID, getReferenceRelations.RelatedObjectGUID);
            Assert.AreEqual(action, getReferenceRelations.Action);
            Assert.AreEqual(referenceId, getReferenceRelations.ReferenceId);                        
        }


        [TestMethod]
        public void TestSetRevisionReferenceRelationPublished()
        {            
            // Act
            var result = DyntaxaManager.SetRevisionReferenceRelationPublished(GetRevisionContext(), 24);

            // Assert
            Assert.AreEqual(true, result);
        }
    }
}
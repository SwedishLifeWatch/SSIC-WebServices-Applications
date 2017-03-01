using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Data;
using ArtDatabanken.WebService.TaxonService.Test.TestFactories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxonManager = ArtDatabanken.WebService.TaxonService.Data.TaxonManager;

namespace ArtDatabanken.WebService.TaxonService.Test.Data
{
    [TestClass]
    public class DyntaxaManagerReferenceRelationsTests : TestBase
    {
        const string ReferenceModifyAction = "Modify";
        const string ReferenceAddAction = "Add";
        const string ReferenceDeleteAction = "Delete";

        public DyntaxaManagerReferenceRelationsTests()
            : base(useTransaction, 120)
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
        public void TestModifyReferenceRelationOneTime()
        {
            // Consts
            const int TestRevisionId = 24;
            const string TestObjectGUID = "urn:lsid:dyntaxa.se:Taxon:6010174";
            const int TestReferenceId = 1030;
            

            // Step 1 - change original artfaktaReferenceRelation from TypeId = 1 to TypeId = 2
            WebDyntaxaRevisionReferenceRelation revisionReferenceRelation = DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(
                GetRevisionContext(),
                WebDyntaxaReferenceRelationTestFactory.CreateModifyAction(
                    TestRevisionId, 
                    5000, 
                    TestObjectGUID, 
                    TestReferenceId, 
                    2, 
                    1));

            // Get reference relations for current revision and object GUID.
            List<WebDyntaxaRevisionReferenceRelation> referenceRelations =
                DyntaxaManager.GetDyntaxaRevisionReferenceRelation(
                    GetRevisionContext(),
                    TestRevisionId,
                    TestObjectGUID);
            Assert.AreEqual(1, referenceRelations.Count);
            Assert.AreEqual(2, referenceRelations[0].ReferenceType);

            // Test with character ' in GUID.
            referenceRelations =
                DyntaxaManager.GetDyntaxaRevisionReferenceRelation(
                    GetRevisionContext(),
                    TestRevisionId,
                    "urn:lsid:dyntaxa.se:Taxon:'6010174");
            Assert.IsTrue(referenceRelations.IsEmpty());
        }



        [TestMethod]
        public void TestModifyReferenceRelationAndThenModifyBackToOriginalState()
        {
            // Consts
            const int TestRevisionId = 24;
            const string TestObjectGUID = "urn:lsid:dyntaxa.se:Taxon:6010174";
            const int TestReferenceId = 1030;

            // Arrange
            WebTaxonRevisionEvent revisionEvent1;
            WebTaxonRevisionEvent revisionEvent2;

            // Act
            // Step 1 - change original artfaktaReferenceRelation from TypeId = 1 to TypeId = 2            
            revisionEvent1 = DyntaxaManager.CreateCompleteRevisionEvent(GetRevisionContext(),
                new WebTaxonRevisionEvent()
                {
                    CreatedBy = 2,
                    CreatedDate = DateTime.Now,
                    TypeId = 19,
                    RevisionId = TestRevisionId,
                    AffectedTaxa = "Taxon [6010174]",
                    OldValue = "Type=1",
                    NewValue = "Type=2"
                });
            WebDyntaxaRevisionReferenceRelation revisionReferenceRelationStep1 = DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(
                GetRevisionContext(),
                WebDyntaxaReferenceRelationTestFactory.CreateModifyAction(
                    TestRevisionId,
                    revisionEvent1.Id,
                    TestObjectGUID, 
                    TestReferenceId, 
                    2, 
                    1));



            // Step 2 - change from TypeId = 2 to TypeId = 1
            revisionEvent2 = DyntaxaManager.CreateCompleteRevisionEvent(GetRevisionContext(),
                new WebTaxonRevisionEvent()
                {
                    CreatedBy = 2,
                    CreatedDate = DateTime.Now,
                    TypeId = 19,
                    RevisionId = TestRevisionId,
                    AffectedTaxa = "Taxon [6010174]",
                    OldValue = "Type=2",
                    NewValue = "Type=1"
                });
            WebDyntaxaRevisionReferenceRelation revisionReferenceRelationStep2 = DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(
                GetRevisionContext(),
                WebDyntaxaReferenceRelationTestFactory.CreateModifyAction(
                    TestRevisionId,
                    revisionEvent2.Id, 
                    TestObjectGUID, 
                    TestReferenceId,                     
                    1, 
                    2));

            // Get reference relations for current revision and object GUID.
            List<WebDyntaxaRevisionReferenceRelation> referenceRelations = 
                DyntaxaManager.GetDyntaxaRevisionReferenceRelation(
                    GetRevisionContext(), 
                    TestRevisionId, 
                    TestObjectGUID);

            Assert.AreEqual(1, referenceRelations.Count);            
            Assert.AreEqual(1, referenceRelations[0].ReferenceType);
        }

        /// <summary>
        /// 1. Modifiera en referensrelation från typ=1 till typ=2.
        /// 2. Gör sedan en till modifiering från typ=2 till typ=1.
        ///    Steg 1 och 2 görs i separata event.      
        /// 3. Gör Undo på senaste revisionevent.
        /// </summary>
        [TestMethod]
        public void TestModifyReferenceRelationAndThenModifyBackToOriginalStateAndThenUndo()
        {
            // Consts
            const int TestRevisionId = 24;
            const string TestObjectGUID = "urn:lsid:dyntaxa.se:Taxon:6010174";
            const int TestReferenceId = 1030;

            // Arrange            
            WebTaxonRevisionEvent revisionEvent1;
            WebTaxonRevisionEvent revisionEvent2;

            // Act

            // Step 1 - change original artfaktaReferenceRelation from TypeId = 1 to TypeId = 2            
            revisionEvent1 = DyntaxaManager.CreateCompleteRevisionEvent(GetRevisionContext(),
                new WebTaxonRevisionEvent()
                {
                    CreatedBy = 2,
                    CreatedDate = DateTime.Now,
                    TypeId = 19,
                    RevisionId = TestRevisionId,
                    AffectedTaxa = "Taxon [6010174]",
                    OldValue = "Type=1",
                    NewValue = "Type=2"
                });
            WebDyntaxaRevisionReferenceRelation revisionReferenceRelationStep1 = DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(
                GetRevisionContext(),
                WebDyntaxaReferenceRelationTestFactory.CreateModifyAction(
                    TestRevisionId,
                    revisionEvent1.Id,
                    TestObjectGUID,
                    TestReferenceId,
                    2,
                    1));



            // Step 2 - change from TypeId = 2 to TypeId = 1
            revisionEvent2 = DyntaxaManager.CreateCompleteRevisionEvent(GetRevisionContext(),
                new WebTaxonRevisionEvent()
                {
                    CreatedBy = 2,
                    CreatedDate = DateTime.Now,
                    TypeId = 19,
                    RevisionId = TestRevisionId,
                    AffectedTaxa = "Taxon [6010174]",
                    OldValue = "Type=2",
                    NewValue = "Type=1"
                });
            WebDyntaxaRevisionReferenceRelation revisionReferenceRelationStep2 = DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(
                GetRevisionContext(),
                WebDyntaxaReferenceRelationTestFactory.CreateModifyAction(
                    TestRevisionId,
                    revisionEvent2.Id,
                    TestObjectGUID,
                    TestReferenceId,
                    1,
                    2));

            // Get reference relations for current revision and object GUID.
            List<WebDyntaxaRevisionReferenceRelation> referenceRelations =
                DyntaxaManager.GetDyntaxaRevisionReferenceRelation(
                    GetRevisionContext(),
                    TestRevisionId,
                    TestObjectGUID);

            Assert.AreEqual(1, referenceRelations.Count);            
            Assert.AreEqual(1, referenceRelations[0].ReferenceType);

            
            // undo last revision event
            TaxonManager.DeleteTaxonRevisionEvent(GetRevisionContext(), revisionEvent2.Id);
            referenceRelations =
                DyntaxaManager.GetDyntaxaRevisionReferenceRelation(
                    GetRevisionContext(),
                    TestRevisionId,
                    TestObjectGUID);
            Assert.AreEqual(2, referenceRelations[0].ReferenceType);


            // undo last revision event once more
            TaxonManager.DeleteTaxonRevisionEvent(GetRevisionContext(), revisionEvent1.Id);
            referenceRelations =
                DyntaxaManager.GetDyntaxaRevisionReferenceRelation(
                    GetRevisionContext(),
                    TestRevisionId,
                    TestObjectGUID);
            Assert.AreEqual(0, referenceRelations.Count);            
        }

        /// <summary>
        /// 1. Modifiera en referensrelation från typ=1 till typ=2.
        /// 2. Gör sedan en till modifiering från typ=2 till typ=1.
        ///    Steg 1 och 2 görs i samma revisionsevent
        /// 3. Gör Undo på senaste revisionevent.
        /// </summary>
        [TestMethod]
        public void TestModifyReferenceRelationAndThenModifyBackToOriginalStateInSameRevisionEventAndThenUndo()
        {
            // Consts
            const int TestRevisionId = 24;
            const string TestObjectGUID = "urn:lsid:dyntaxa.se:Taxon:6010174";
            const int TestReferenceId = 1030;
            
            // Step 1 - change original artfaktaReferenceRelation from TypeId = 1 to TypeId = 2            
            WebTaxonRevisionEvent revisionEvent1 = DyntaxaManager.CreateCompleteRevisionEvent(GetRevisionContext(),
                new WebTaxonRevisionEvent()
                {
                    CreatedBy = 2,
                    CreatedDate = DateTime.Now,
                    TypeId = 19,
                    RevisionId = TestRevisionId,
                    AffectedTaxa = "Taxon' [6010174]",
                    OldValue = "Type='1",
                    NewValue = "Type='2"
                });
            WebDyntaxaRevisionReferenceRelation revisionReferenceRelationStep1 = DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(
                GetRevisionContext(),
                WebDyntaxaReferenceRelationTestFactory.CreateModifyAction(
                    TestRevisionId,
                    revisionEvent1.Id,
                    TestObjectGUID,
                    TestReferenceId,
                    2,
                    1));
            
            // Step 2 - change from TypeId = 2 to TypeId = 1            
            WebDyntaxaRevisionReferenceRelation revisionReferenceRelationStep2 = DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(
                GetRevisionContext(),
                WebDyntaxaReferenceRelationTestFactory.CreateModifyAction(
                    TestRevisionId,
                    revisionEvent1.Id,
                    TestObjectGUID,
                    TestReferenceId,
                    1,
                    2));

            // Get reference relations for current revision and object GUID.
            List<WebDyntaxaRevisionReferenceRelation> referenceRelations =
                DyntaxaManager.GetDyntaxaRevisionReferenceRelation(
                    GetRevisionContext(),
                    TestRevisionId,
                    TestObjectGUID);
            Assert.AreEqual(1, referenceRelations.Count);            
            Assert.AreEqual(1, referenceRelations[0].ReferenceType);


            // undo last revision event
            TaxonManager.DeleteTaxonRevisionEvent(GetRevisionContext(), revisionEvent1.Id);
            referenceRelations =
                DyntaxaManager.GetDyntaxaRevisionReferenceRelation(
                    GetRevisionContext(),
                    TestRevisionId,
                    TestObjectGUID);            
            Assert.AreEqual(0, referenceRelations.Count);
        }




        /// <summary>
        /// 1. Lägg till två nya referensrelationer till ett objekt. Detta görs som 1 revisionevent.
        /// 2. Ändra typen på en av de skapade referensrelationerna till 1.
        /// 3. Gör Undo på senaste revisionevent.
        /// </summary>
        [TestMethod]
        public void TestAddTwoReferenceRelationsToOneObjectGUIDAndThenModifyTypeOfOneOfTheReferences()
        {
            // Consts
            const int TestRevisionId = 24;
            const string TestObjectGUID = "urn:lsid:dyntaxa.se:Taxon:6010174";
            const int TestReferenceId3200 = 3200;
            const int TestReferenceId1500 = 1500;

            // Arrange            
            WebTaxonRevisionEvent revisionEvent1;
            WebTaxonRevisionEvent revisionEvent2;

            // Act
            // Add new reference 3200 with type 2
            revisionEvent1 = DyntaxaManager.CreateCompleteRevisionEvent(GetRevisionContext(),
                new WebTaxonRevisionEvent()
                {
                    CreatedBy = 2,
                    CreatedDate = DateTime.Now,
                    TypeId = 19,
                    RevisionId = TestRevisionId,
                    AffectedTaxa = "Taxon [6010174]",
                    OldValue = "NULL",
                    NewValue = "Reference3200"
                });
            WebDyntaxaRevisionReferenceRelation revisionReferenceRelationStep1 = DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(
                GetRevisionContext(),
                WebDyntaxaReferenceRelationTestFactory.CreateAddAction(
                    TestRevisionId,
                    revisionEvent1.Id,
                    TestObjectGUID,
                    TestReferenceId3200,
                    2));
            
            // Add new reference 1500 with type 1            
            WebDyntaxaRevisionReferenceRelation revisionReferenceRelationStep2 = DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(
                GetRevisionContext(),
                WebDyntaxaReferenceRelationTestFactory.CreateAddAction(
                    TestRevisionId,
                    revisionEvent1.Id,
                    TestObjectGUID,
                    TestReferenceId1500,
                    1));

            // Get reference relations for current revision and object GUID.
            List<WebDyntaxaRevisionReferenceRelation> referenceRelations =
                DyntaxaManager.GetDyntaxaRevisionReferenceRelation(
                    GetRevisionContext(),
                    TestRevisionId,
                    TestObjectGUID);

            Assert.AreEqual(2, referenceRelations.Count);            
            Assert.AreEqual(TestReferenceId3200, referenceRelations[0].ReferenceId);
            Assert.AreEqual(2, referenceRelations[0].ReferenceType);
            Assert.AreEqual(TestReferenceId1500, referenceRelations[1].ReferenceId);



            // Change type of Reference 3200 to type 1
            revisionEvent2 = DyntaxaManager.CreateCompleteRevisionEvent(GetRevisionContext(),
                new WebTaxonRevisionEvent()
                {
                    CreatedBy = 2,
                    CreatedDate = DateTime.Now,
                    TypeId = 19,
                    RevisionId = TestRevisionId,
                    AffectedTaxa = "Taxon [6010174]",
                    OldValue = "Type=2",
                    NewValue = "Type=1"
                });
            // Add eller Modify? 
            //WebDyntaxaRevisionReferenceRelation revisionReferenceRelationStep3 = DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(
            //    GetRevisionContext(),
            //    WebDyntaxaReferenceRelationTestFactory.CreateAddAction(
            //        TestRevisionId,
            //        revisionEvent2.Id,
            //        TestObjectGUID,
            //        TestReferenceId3200,
            //        1));
            WebDyntaxaRevisionReferenceRelation revisionReferenceRelationStep3 = DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(
                GetRevisionContext(),
                WebDyntaxaReferenceRelationTestFactory.CreateModifyAction(
                    TestRevisionId,
                    revisionEvent2.Id,
                    TestObjectGUID,
                    TestReferenceId3200,
                    1,
                    2)); // 2 eller null?

            // Get reference relations for current revision and object GUID.
            referenceRelations =
                DyntaxaManager.GetDyntaxaRevisionReferenceRelation(
                    GetRevisionContext(),
                    TestRevisionId,
                    TestObjectGUID);

            Assert.AreEqual(2, referenceRelations.Count);            
            Assert.AreEqual(TestReferenceId1500, referenceRelations[0].ReferenceId);
            Assert.AreEqual(TestReferenceId3200, referenceRelations[1].ReferenceId);
            Assert.AreEqual(1, referenceRelations[1].ReferenceType);





            // undo last revision event
            TaxonManager.DeleteTaxonRevisionEvent(GetRevisionContext(), revisionEvent2.Id);
            referenceRelations =
                DyntaxaManager.GetDyntaxaRevisionReferenceRelation(
                    GetRevisionContext(),
                    TestRevisionId,
                    TestObjectGUID);
            Assert.AreEqual(2, referenceRelations[0].ReferenceType);

            // Get reference relations for current revision and object GUID.
            referenceRelations =
                DyntaxaManager.GetDyntaxaRevisionReferenceRelation(
                    GetRevisionContext(),
                    TestRevisionId,
                    TestObjectGUID);

            Assert.AreEqual(2, referenceRelations.Count);
            Assert.AreEqual(TestReferenceId3200, referenceRelations[0].ReferenceId);
            Assert.AreEqual(2, referenceRelations[0].ReferenceType);
            Assert.AreEqual(TestReferenceId1500, referenceRelations[1].ReferenceId);

            // undo last revision event once more
            TaxonManager.DeleteTaxonRevisionEvent(GetRevisionContext(), revisionEvent1.Id);
            referenceRelations =
                DyntaxaManager.GetDyntaxaRevisionReferenceRelation(
                    GetRevisionContext(),
                    TestRevisionId,
                    TestObjectGUID);
            Assert.AreEqual(0, referenceRelations.Count);
        }






        /// <summary>
        /// 1. Ta bort referensrelation
        /// 2. Lägg tillbaka referensrelationen.
        /// 3. Gör undo.
        /// </summary>
        [TestMethod]
        public void TestDeleteReferenceAndThenAddItAgainAndThenUndo()
        {
            // Consts
            const int TestRevisionId = 24;
            const string TestObjectGUID = "urn:lsid:dyntaxa.se:Taxon:6010174";
            const int TestReferenceId3200 = 3200;
            
            WebTaxonRevisionEvent revisionEvent1;
            WebTaxonRevisionEvent revisionEvent2;

            // Act

            // Step 1 - Delete reference 3200, Type=2
            revisionEvent1 = DyntaxaManager.CreateCompleteRevisionEvent(GetRevisionContext(),
                new WebTaxonRevisionEvent()
                {
                    CreatedBy = 2,
                    CreatedDate = DateTime.Now,
                    TypeId = 19,
                    RevisionId = TestRevisionId,
                    AffectedTaxa = "Taxon [6010174]",
                    OldValue = "Ref3200,Typ1",
                    NewValue = "Deleted"
                });
            WebDyntaxaRevisionReferenceRelation revisionReferenceRelationStep1 = DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(
                GetRevisionContext(),
                WebDyntaxaReferenceRelationTestFactory.CreateDeleteAction(
                    TestRevisionId,
                    revisionEvent1.Id,
                    TestObjectGUID,
                    TestReferenceId3200,
                    2,
                    2));

            // Get reference relations for current revision and object GUID.
            List<WebDyntaxaRevisionReferenceRelation> referenceRelations =
                DyntaxaManager.GetDyntaxaRevisionReferenceRelation(
                    GetRevisionContext(),
                    TestRevisionId,
                    TestObjectGUID);
            Assert.AreEqual(1, referenceRelations.Count);
            Assert.AreEqual(TestReferenceId3200, referenceRelations[0].ReferenceId);
            Assert.AreEqual("Delete", referenceRelations[0].Action);


            // Step 2 - Add reference 3200, Type=1
            revisionEvent2 = DyntaxaManager.CreateCompleteRevisionEvent(GetRevisionContext(),
                new WebTaxonRevisionEvent()
                {
                    CreatedBy = 2,
                    CreatedDate = DateTime.Now,
                    TypeId = 19,
                    RevisionId = TestRevisionId,
                    AffectedTaxa = "Taxon [6010174]",
                    OldValue = "Deleted",
                    NewValue = "Ref3200"
                });
            WebDyntaxaRevisionReferenceRelation revisionReferenceRelationStep2 = DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(
                GetRevisionContext(),
                WebDyntaxaReferenceRelationTestFactory.CreateAddAction(
                    TestRevisionId,
                    revisionEvent2.Id,
                    TestObjectGUID,
                    TestReferenceId3200,
                    2));

            // Get reference relations for current revision and object GUID.
            referenceRelations =
                DyntaxaManager.GetDyntaxaRevisionReferenceRelation(
                    GetRevisionContext(),
                    TestRevisionId,
                    TestObjectGUID);
            Assert.AreEqual(1, referenceRelations.Count);
            Assert.AreEqual(TestReferenceId3200, referenceRelations[0].ReferenceId);
            Assert.AreEqual("Add", referenceRelations[0].Action);               
        }


        /// <summary>
        /// 1. Lägg till två nya referensrelationer till ett objekt med samma referens men med olika typ.        
        /// </summary>
        [TestMethod]
        public void TestAddTwoReferenceRelationsWithSameReferenceButDifferentType()
        {
            // Consts
            const int TestRevisionId = 24;
            const string TestObjectGUID = "urn:lsid:dyntaxa.se:Taxon:6010174";
            const int TestReferenceId3200 = 3200;            

            // Arrange            
            WebTaxonRevisionEvent revisionEvent1;            

            // Act
            // Add new reference 3200 with type 1
            revisionEvent1 = DyntaxaManager.CreateCompleteRevisionEvent(GetRevisionContext(),
                new WebTaxonRevisionEvent()
                {
                    CreatedBy = 2,
                    CreatedDate = DateTime.Now,
                    TypeId = 19,
                    RevisionId = TestRevisionId,
                    AffectedTaxa = "Taxon [6010174]",
                    OldValue = "NULL",
                    NewValue = "Reference3200, Type=1"
                });
            WebDyntaxaRevisionReferenceRelation revisionReferenceRelationStep1 = DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(
                GetRevisionContext(),
                WebDyntaxaReferenceRelationTestFactory.CreateAddAction(
                    TestRevisionId,
                    revisionEvent1.Id,
                    TestObjectGUID,
                    TestReferenceId3200,
                    1));

            // Add new reference 3200 with type 2          
            WebDyntaxaRevisionReferenceRelation revisionReferenceRelationStep2 = DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(
                GetRevisionContext(),
                WebDyntaxaReferenceRelationTestFactory.CreateAddAction(
                    TestRevisionId,
                    revisionEvent1.Id,
                    TestObjectGUID,
                    TestReferenceId3200,
                    2));

            // Get reference relations for current revision and object GUID.
            List<WebDyntaxaRevisionReferenceRelation> referenceRelations =
                DyntaxaManager.GetDyntaxaRevisionReferenceRelation(
                    GetRevisionContext(),
                    TestRevisionId,
                    TestObjectGUID);

            Assert.AreEqual(2, referenceRelations.Count);
            Assert.AreEqual(TestReferenceId3200, referenceRelations[0].ReferenceId);
            Assert.AreEqual(1, referenceRelations[0].ReferenceType);
            Assert.AreEqual(TestReferenceId3200, referenceRelations[1].ReferenceId);
            Assert.AreEqual(2, referenceRelations[1].ReferenceType);            
        }


    }
}

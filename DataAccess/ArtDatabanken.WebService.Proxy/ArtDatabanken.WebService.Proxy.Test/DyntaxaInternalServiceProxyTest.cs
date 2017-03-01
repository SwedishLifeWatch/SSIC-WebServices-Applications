using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy.Test.TestDataFactories;

namespace ArtDatabanken.WebService.Proxy.Test
{
    /// <summary>
    /// This class handles dyntaxa internal proxy tests.
    /// </summary>
    [TestClass]
    public class DyntaxaInternalServiceProxyTest
    {
        private WebClientInformation _clientInformation;
        private WebLoginResponse _loginResponse;

        public DyntaxaInternalServiceProxyTest()
        {
            _clientInformation = null;
        }

        protected WebClientInformation GetClientInformation()
        {
            return _clientInformation;
        }

        /// <summary>
        /// Gets the revision client information.
        /// </summary>
        /// <param name="revisionId">The revision identifier.</param>
        /// <returns></returns>
        protected WebClientInformation GetRevisionClientInformation(int revisionId=1)
        {
            // set CurrentRole = Role w/ identifier = "urn:lsid:dyntaxa.se:Revision:id for revision"
            string identifier = string.Format("urn:lsid:dyntaxa.se:Revision:{0}", revisionId);
            _clientInformation.Role.Identifier = string.Format("urn:lsid:dyntaxa.se:Revision:{0}", revisionId);

            foreach (WebRole role in _loginResponse.Roles)
            {
                if (role.Identifier.IsNotNull() && role.Identifier.EndsWith(identifier))
                {
                    role.Identifier = identifier;
                    _clientInformation.Role = role;                    
                }
            }       
            return _clientInformation;                       
        }

        [TestMethod]
        public void GetStatus()
        {
            List<WebResourceStatus> status;         
            status = WebServiceProxy.DyntaxaInternalService.GetStatus(GetClientInformation());            
            Assert.IsTrue(status.IsNotEmpty());
            status = WebServiceProxy.DyntaxaInternalService.GetStatus(GetClientInformation());
            Assert.IsTrue(status.IsNotEmpty());
        }

        [TestMethod]
        public void CreateSpeciesFact()
        {            
            Int32 revisionId = 1;            

            WebDyntaxaRevisionSpeciesFact speciesFact = GetReferenceDyntaxaRevisionSpeciesFact((Int32)TaxonId.Bear);
            speciesFact.FactorId = (Int32)FactorId.SwedishHistory;
            speciesFact.RevisionId = revisionId;
            speciesFact.IsPublished = false;
            speciesFact.StatusId = 400;
            speciesFact.SpeciesFactExists = false;

            WebTaxonRevisionEvent revisionEvent;
            revisionEvent = new WebTaxonRevisionEvent()
            {
                CreatedBy = 2,
                CreatedDate = DateTime.Now,
                TypeId = 1,
                RevisionId = revisionId,
                AffectedTaxa = "Bear",
                OldValue = "StatusId=Unknown",
                NewValue = "StatusId=400"
            };
            
            //using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.DyntaxaInternalService))
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetRevisionClientInformation(), WebServiceProxy.TaxonService))
            {
                // We now create revision event.
                WebTaxonRevisionEvent createdRevisionEvent = WebServiceProxy.DyntaxaInternalService.CreateCompleteRevisionEvent(GetClientInformation(), revisionEvent);
                speciesFact.RevisionEventId = createdRevisionEvent.Id;
                // We now create the revision species fact.
                speciesFact = WebServiceProxy.DyntaxaInternalService.CreateDyntaxaRevisionSpeciesFact(GetClientInformation(), speciesFact);
                var currentDyntaxaRevisionSpeciesFact = WebServiceProxy.DyntaxaInternalService.GetDyntaxaRevisionSpeciesFact(
                    GetClientInformation(), (Int32)FactorId.SwedishHistory, (Int32)TaxonId.Bear, revisionId);

                // Assert first Insert.
                Assert.AreEqual(createdRevisionEvent.Id, speciesFact.RevisionEventId);
                Assert.IsFalse(currentDyntaxaRevisionSpeciesFact.IsChangedInRevisionEventIdSpecified);
                Assert.AreEqual(400, currentDyntaxaRevisionSpeciesFact.StatusId);
                Assert.AreEqual(speciesFact.StatusId, currentDyntaxaRevisionSpeciesFact.StatusId);
                Assert.IsFalse(currentDyntaxaRevisionSpeciesFact.SpeciesFactExists);
                Assert.IsNull(currentDyntaxaRevisionSpeciesFact.OriginalStatusId);
            }
        }

        [TestMethod]
        public void CreateSpeciesFactWithSpeciesFactExistsData()
        {
            Int32 revisionId = 1;

            WebDyntaxaRevisionSpeciesFact speciesFact = GetReferenceDyntaxaRevisionSpeciesFact((Int32)TaxonId.Bear);
            speciesFact.FactorId = (Int32)FactorId.SwedishHistory;
            speciesFact.RevisionId = revisionId;
            speciesFact.IsPublished = false;
            speciesFact.StatusId = 400;
            speciesFact.SpeciesFactExists = true;
            speciesFact.OriginalStatusId = 2;
            speciesFact.OriginalQualityId = 3;
            speciesFact.OriginalReferenceId = 4;
            speciesFact.OriginalDescription = "test test";

            WebTaxonRevisionEvent revisionEvent;
            revisionEvent = new WebTaxonRevisionEvent()
            {
                CreatedBy = 2,
                CreatedDate = DateTime.Now,
                TypeId = 1,
                RevisionId = revisionId,
                AffectedTaxa = "Bear",
                OldValue = "StatusId=Unknown",
                NewValue = "StatusId=400"
            };

            //using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.DyntaxaInternalService))
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetRevisionClientInformation(), WebServiceProxy.TaxonService))
            {
                // We now create revision event.
                WebTaxonRevisionEvent createdRevisionEvent = WebServiceProxy.DyntaxaInternalService.CreateCompleteRevisionEvent(GetClientInformation(), revisionEvent);
                speciesFact.RevisionEventId = createdRevisionEvent.Id;
                // We now create the revision species fact.
                speciesFact = WebServiceProxy.DyntaxaInternalService.CreateDyntaxaRevisionSpeciesFact(GetClientInformation(), speciesFact);
                var currentDyntaxaRevisionSpeciesFact = WebServiceProxy.DyntaxaInternalService.GetDyntaxaRevisionSpeciesFact(
                    GetClientInformation(), (Int32)FactorId.SwedishHistory, (Int32)TaxonId.Bear, revisionId);

                // Assert first Insert.
                Assert.AreEqual(createdRevisionEvent.Id, speciesFact.RevisionEventId);
                Assert.IsFalse(currentDyntaxaRevisionSpeciesFact.IsChangedInRevisionEventIdSpecified);
                Assert.AreEqual(400, currentDyntaxaRevisionSpeciesFact.StatusId);
                Assert.AreEqual(speciesFact.StatusId, currentDyntaxaRevisionSpeciesFact.StatusId);
                Assert.IsTrue(currentDyntaxaRevisionSpeciesFact.SpeciesFactExists);
                Assert.AreEqual(2, speciesFact.OriginalStatusId);
                Assert.AreEqual(3, speciesFact.OriginalQualityId);
                Assert.AreEqual(4, speciesFact.OriginalReferenceId);
                Assert.AreEqual("test test", speciesFact.OriginalDescription);
            }
        }

        [TestMethod]
        public void CreateSpeciesFact_WithSpeciesFactStatusNullData_ThenDyntaxaRevisionSpeciesFactCreated()
        {
            Int32 revisionId = 1;

            WebDyntaxaRevisionSpeciesFact speciesFact = GetReferenceDyntaxaRevisionSpeciesFact((Int32)TaxonId.Bear);
            speciesFact.FactorId = (Int32)FactorId.SwedishHistory;
            speciesFact.RevisionId = revisionId;
            speciesFact.IsPublished = false;
            speciesFact.StatusId = null;
            speciesFact.ReferenceId = null;
            speciesFact.QualityId = null;
            speciesFact.SpeciesFactExists = true;
            speciesFact.Description = null;
            speciesFact.OriginalStatusId = 2;
            speciesFact.OriginalQualityId = 3;
            speciesFact.OriginalReferenceId = 4;
            speciesFact.OriginalDescription = "test test";

            WebTaxonRevisionEvent revisionEvent;
            revisionEvent = new WebTaxonRevisionEvent()
            {
                CreatedBy = 2,
                CreatedDate = DateTime.Now,
                TypeId = 1,
                RevisionId = revisionId,
                AffectedTaxa = "Bear",
                OldValue = "StatusId=Unknown",
                NewValue = "StatusId=400"
            };

            //using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.DyntaxaInternalService))
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetRevisionClientInformation(), WebServiceProxy.TaxonService))
            {
                // We now create revision event.
                WebTaxonRevisionEvent createdRevisionEvent = WebServiceProxy.DyntaxaInternalService.CreateCompleteRevisionEvent(GetClientInformation(), revisionEvent);
                speciesFact.RevisionEventId = createdRevisionEvent.Id;
                // We now create the revision species fact.
                speciesFact = WebServiceProxy.DyntaxaInternalService.CreateDyntaxaRevisionSpeciesFact(GetClientInformation(), speciesFact);
                var currentDyntaxaRevisionSpeciesFact = WebServiceProxy.DyntaxaInternalService.GetDyntaxaRevisionSpeciesFact(
                    GetClientInformation(), (Int32)FactorId.SwedishHistory, (Int32)TaxonId.Bear, revisionId);

                // Assert first Insert.
                Assert.AreEqual(createdRevisionEvent.Id, speciesFact.RevisionEventId);
                Assert.IsFalse(currentDyntaxaRevisionSpeciesFact.IsChangedInRevisionEventIdSpecified);
                Assert.IsNull(currentDyntaxaRevisionSpeciesFact.StatusId);
                Assert.IsNull(currentDyntaxaRevisionSpeciesFact.ReferenceId);
                Assert.IsNull(currentDyntaxaRevisionSpeciesFact.QualityId);
                Assert.IsNull(currentDyntaxaRevisionSpeciesFact.Description);
                Assert.AreEqual(speciesFact.StatusId, currentDyntaxaRevisionSpeciesFact.StatusId);
                Assert.IsTrue(currentDyntaxaRevisionSpeciesFact.SpeciesFactExists);
                Assert.AreEqual(2, speciesFact.OriginalStatusId);
                Assert.AreEqual(3, speciesFact.OriginalQualityId);
                Assert.AreEqual(4, speciesFact.OriginalReferenceId);
                Assert.AreEqual("test test", speciesFact.OriginalDescription);
            }
        }



        [TestMethod]
        public void GetDyntaxaRevisionSpeciesFact_WhenRevisionSpeciesFactIsCreatedAndLaterChanged_ThenLastChangedRevisionSpeciesFactIsReturned()
        {
            WebDyntaxaRevisionSpeciesFact dyntaxaRevisionSpeciesFact1, dyntaxaRevisionSpeciesFact2, currentDyntaxaRevisionSpeciesFact;
            WebTaxonRevisionEvent revisionEvent1, revisionEvent2;
            Int32 revisionId = 1;

            //-------------------------------------------------------------------
            // Create first dyntaxa revision species fact and revision event.
            dyntaxaRevisionSpeciesFact1 = GetReferenceDyntaxaRevisionSpeciesFact((Int32)TaxonId.Bear);
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

            //using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.DyntaxaInternalService))
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetRevisionClientInformation(), WebServiceProxy.TaxonService))
            {
                revisionEvent1 = WebServiceProxy.DyntaxaInternalService.CreateCompleteRevisionEvent(GetClientInformation(), revisionEvent1);
                dyntaxaRevisionSpeciesFact1.RevisionEventId = revisionEvent1.Id;
                dyntaxaRevisionSpeciesFact1 = WebServiceProxy.DyntaxaInternalService.CreateDyntaxaRevisionSpeciesFact(GetClientInformation(),
                    dyntaxaRevisionSpeciesFact1);
                currentDyntaxaRevisionSpeciesFact = WebServiceProxy.DyntaxaInternalService.GetDyntaxaRevisionSpeciesFact(
                    GetClientInformation(), (Int32)FactorId.SwedishHistory, (Int32)TaxonId.Bear, revisionId);

                //Assert first Insert
                Assert.AreEqual(revisionEvent1.Id, dyntaxaRevisionSpeciesFact1.RevisionEventId);
                Assert.IsFalse(currentDyntaxaRevisionSpeciesFact.IsChangedInRevisionEventIdSpecified);
                Assert.AreEqual(400, currentDyntaxaRevisionSpeciesFact.StatusId);
                Assert.AreEqual(dyntaxaRevisionSpeciesFact1.StatusId, currentDyntaxaRevisionSpeciesFact.StatusId);

                //---------------------------------------------------------------------
                // Create second dyntaxa revision species fact and revision event.
                dyntaxaRevisionSpeciesFact2 = GetReferenceDyntaxaRevisionSpeciesFact((Int32) TaxonId.Bear);
                dyntaxaRevisionSpeciesFact2.FactorId = (Int32) FactorId.SwedishHistory;
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

                revisionEvent2 = WebServiceProxy.DyntaxaInternalService.CreateCompleteRevisionEvent(GetClientInformation(), revisionEvent2);
                dyntaxaRevisionSpeciesFact2.RevisionEventId = revisionEvent2.Id;
                dyntaxaRevisionSpeciesFact2 = WebServiceProxy.DyntaxaInternalService.CreateDyntaxaRevisionSpeciesFact(GetClientInformation(),
                    dyntaxaRevisionSpeciesFact2);
                currentDyntaxaRevisionSpeciesFact = WebServiceProxy.DyntaxaInternalService.GetDyntaxaRevisionSpeciesFact(
                    GetClientInformation(), (Int32)FactorId.SwedishHistory, (Int32)TaxonId.Bear, revisionId);                

                // Check values.
                Assert.AreEqual(450, currentDyntaxaRevisionSpeciesFact.StatusId);                
                Assert.AreEqual(revisionEvent2.Id, currentDyntaxaRevisionSpeciesFact.RevisionEventId);
                Assert.IsFalse(currentDyntaxaRevisionSpeciesFact.IsChangedInRevisionEventIdSpecified); 
                // Check latest change is returned.
                Assert.AreEqual(dyntaxaRevisionSpeciesFact2.Id, currentDyntaxaRevisionSpeciesFact.Id); 
                Assert.AreNotEqual(dyntaxaRevisionSpeciesFact1.Id, currentDyntaxaRevisionSpeciesFact.Id);
            }            
        }

        [TestMethod]
        public void GetAllDyntaxaRevisionSpeciesFacts_WhenOneRevisionSpeciesFactIsCreated_ThenOneChangedRevisionSpeciesFactIsReturned()
        {
            WebDyntaxaRevisionSpeciesFact dyntaxaRevisionSpeciesFact1;
            WebTaxonRevisionEvent revisionEvent1;
            Int32 revisionId = 1;

            //-------------------------------------------------------------------
            // Create first dyntaxa revision species fact and revision event.
            dyntaxaRevisionSpeciesFact1 = GetReferenceDyntaxaRevisionSpeciesFact((Int32)TaxonId.Bear);
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

            //using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.DyntaxaInternalService))
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetRevisionClientInformation(), WebServiceProxy.TaxonService))
            {
                revisionEvent1 = WebServiceProxy.DyntaxaInternalService.CreateCompleteRevisionEvent(GetClientInformation(), revisionEvent1);
                dyntaxaRevisionSpeciesFact1.RevisionEventId = revisionEvent1.Id;
                dyntaxaRevisionSpeciesFact1 = WebServiceProxy.DyntaxaInternalService.CreateDyntaxaRevisionSpeciesFact(GetClientInformation(),
                    dyntaxaRevisionSpeciesFact1);
                var currentDyntaxaRevisionSpeciesFacts = WebServiceProxy.DyntaxaInternalService.GetAllDyntaxaRevisionSpeciesFacts(
                    GetClientInformation(), revisionId);

                //Assert first Insert
                Assert.AreEqual(1, currentDyntaxaRevisionSpeciesFacts.Count);
            }
        }

        [TestMethod]
        public void SetRevisionSpeciesFactPublished()
        {
            WebDyntaxaRevisionSpeciesFact dyntaxaRevisionSpeciesFact;
            WebTaxonRevisionEvent revisionEvent;
            Int32 revisionId = 1;

            //-------------------------------------------------------------------
            // Create first dyntaxa revision species fact and revision event.
            dyntaxaRevisionSpeciesFact = GetReferenceDyntaxaRevisionSpeciesFact((Int32)TaxonId.Bear);
            dyntaxaRevisionSpeciesFact.FactorId = (Int32)FactorId.SwedishHistory;
            dyntaxaRevisionSpeciesFact.RevisionId = revisionId;
            dyntaxaRevisionSpeciesFact.IsPublished = false;
            dyntaxaRevisionSpeciesFact.StatusId = 400;

            revisionEvent = new WebTaxonRevisionEvent()
            {
                CreatedBy = 2,
                CreatedDate = DateTime.Now,
                TypeId = 1,
                RevisionId = revisionId,
                AffectedTaxa = "Bear",
                OldValue = "StatusId=Unknown",
                NewValue = "StatusId=400"
            };

            //using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.DyntaxaInternalService))
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetRevisionClientInformation(), WebServiceProxy.TaxonService))
            {
                revisionEvent = WebServiceProxy.DyntaxaInternalService.CreateCompleteRevisionEvent(GetClientInformation(), revisionEvent);
                dyntaxaRevisionSpeciesFact.RevisionEventId = revisionEvent.Id;
                WebServiceProxy.DyntaxaInternalService.CreateDyntaxaRevisionSpeciesFact(GetClientInformation(), dyntaxaRevisionSpeciesFact);
                var result = WebServiceProxy.DyntaxaInternalService.SetRevisionSpeciesFactPublished(
                    GetClientInformation(), revisionId);

                //Assert first Insert
                Assert.AreEqual(true, result);
            }
        }
        
        [TestMethod]
        public void CreateDyntaxaRevisionReferenceRelation_WithValidData_ThenDyntaxaRevisionReferenceRelationIsCreated()
        {
            // Arrange
            const string relatedObjectGUID = "urn:lsid:dyntaxa.se:Taxon:6010174";
            const string action = "Add";
            const int revisionId = 1;
            WebDyntaxaRevisionReferenceRelation dyntaxaReferenceRelation, newDyntaxaReferenceRelation;
            dyntaxaReferenceRelation = WebDyntaxaRevisionReferenceRelationTestFactory.Create(
                revisionId, relatedObjectGUID, action);

            // Act
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetRevisionClientInformation(), WebServiceProxy.TaxonService))
            {
                // Create test Dyntaxa revision reference relation.
                dyntaxaReferenceRelation.ReferenceId = 3342;
                newDyntaxaReferenceRelation = WebServiceProxy.DyntaxaInternalService.CreateDyntaxaRevisionReferenceRelation(
                    GetClientInformation(),
                    dyntaxaReferenceRelation);

                // Assert
                Assert.IsNotNull(newDyntaxaReferenceRelation);
                Assert.AreNotEqual(dyntaxaReferenceRelation.Id, newDyntaxaReferenceRelation.Id);
                Assert.AreEqual(dyntaxaReferenceRelation.RelatedObjectGUID,
                    newDyntaxaReferenceRelation.RelatedObjectGUID);
                Assert.AreEqual(3342, newDyntaxaReferenceRelation.ReferenceId);                
            }
        }

        [TestMethod]
        public void GetAllDyntaxaRevisionReferenceRelations_WhenReferenceRelationExists_ThenReferenceRelationIsReturned()
        {
            //Arrange            
            List<WebDyntaxaRevisionReferenceRelation> referenceRelations;
            const int revisionId = 24;
            const string relatedObjectGUID = "urn:lsid:dyntaxa.se:Taxon:6010174";
            const string action = "Add";
            var dyntaxaReferenceRelation = WebDyntaxaRevisionReferenceRelationTestFactory.Create(
                revisionId, relatedObjectGUID, action);
            
            // Act            
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetRevisionClientInformation(), WebServiceProxy.TaxonService))
            {
                WebServiceProxy.DyntaxaInternalService.CreateDyntaxaRevisionReferenceRelation(GetClientInformation(), dyntaxaReferenceRelation);
                referenceRelations = WebServiceProxy.DyntaxaInternalService.GetAllDyntaxaRevisionReferenceRelations(
                    GetClientInformation(), revisionId);

                // Assert
                Assert.AreEqual(1, referenceRelations.Count);
            }
        }

        [TestMethod]
        public void GetDyntaxaRevisionReferenceRelation_WhenReferenceRelationExists_ThenReferenceRelationIsReturned()
        {
            // Arrange            
            const int revisionId = 24;
            const string relatedObjectGUID = "urn:lsid:dyntaxa.se:Taxon:6010174";
            const string action = "Add";
            var dyntaxaReferenceRelation = WebDyntaxaRevisionReferenceRelationTestFactory.Create(
                revisionId, relatedObjectGUID, action);

            // Act
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetRevisionClientInformation(), WebServiceProxy.TaxonService))
            {
                WebServiceProxy.DyntaxaInternalService.CreateDyntaxaRevisionReferenceRelation(GetClientInformation(), dyntaxaReferenceRelation);
                var newReferenceRelations = WebServiceProxy.DyntaxaInternalService.GetDyntaxaRevisionReferenceRelation(
                    GetClientInformation(), revisionId, relatedObjectGUID);

                // Assert
                Assert.IsNotNull(newReferenceRelations);
                Assert.AreEqual(1, newReferenceRelations.Count);
                Assert.AreEqual(action, newReferenceRelations[0].Action);
            }
        }

        [TestMethod]
        public void GetDyntaxaRevisionReferenceRelation_WhenWrongRevisionId_ThenEmptyListIsReturned()
        {
            // Arrange            
            const int revisionId = 24;
            const int wrongRevisionId = 25;
            const string relatedObjectGUID = "urn:lsid:dyntaxa.se:Taxon:6010174";
            const string action = "Add";
            var dyntaxaReferenceRelation = WebDyntaxaRevisionReferenceRelationTestFactory.Create(
                revisionId, relatedObjectGUID, action);            

            // Act
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetRevisionClientInformation(), WebServiceProxy.TaxonService))
            {
                WebServiceProxy.DyntaxaInternalService.CreateDyntaxaRevisionReferenceRelation(GetClientInformation(), dyntaxaReferenceRelation);
                var newReferenceRelations = WebServiceProxy.DyntaxaInternalService.GetDyntaxaRevisionReferenceRelation(
                    GetClientInformation(), wrongRevisionId, relatedObjectGUID);

                // Assert
                Assert.IsNotNull(newReferenceRelations);
                Assert.AreEqual(0, newReferenceRelations.Count);
            }
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

            // Act
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetRevisionClientInformation(), WebServiceProxy.TaxonService))
            {
                WebServiceProxy.DyntaxaInternalService.CreateDyntaxaRevisionReferenceRelation(
                GetClientInformation(),
                WebDyntaxaRevisionReferenceRelationTestFactory.Create(revisionId, rel1RelatedObjectGUID, rel1Action));
                WebServiceProxy.DyntaxaInternalService.CreateDyntaxaRevisionReferenceRelation(
                    GetClientInformation(),
                    WebDyntaxaRevisionReferenceRelationTestFactory.Create(revisionId, rel2RelatedObjectGUID, rel2Action));

                var newReferenceRelations = WebServiceProxy.DyntaxaInternalService.GetDyntaxaRevisionReferenceRelation(
                    GetClientInformation(), revisionId, rel1RelatedObjectGUID);

                // Assert
                Assert.IsNotNull(newReferenceRelations);
                Assert.AreEqual(1, newReferenceRelations.Count);
                Assert.AreEqual(rel1Action, newReferenceRelations[0].Action);
            }
        }

        [TestMethod]
        public void GetDyntaxaRevisionReferenceRelation_WhenTwoReferenceRelationWithSameRelatedObjectGUIDExists_ThenTwoReferenceRelationIsReturned()
        {
            // Arrange            
            const int revisionId = 24;
            const string relatedObjectGUID = "urn:lsid:dyntaxa.se:Taxon:6010174";
            const string rel1Action = "Add";
            const string rel2Action = "Delete";            

            // Act
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetRevisionClientInformation(), WebServiceProxy.TaxonService))
            {
                WebServiceProxy.DyntaxaInternalService.CreateDyntaxaRevisionReferenceRelation(
                GetClientInformation(),
                WebDyntaxaRevisionReferenceRelationTestFactory.Create(revisionId, relatedObjectGUID, rel1Action));
                WebServiceProxy.DyntaxaInternalService.CreateDyntaxaRevisionReferenceRelation(
                    GetClientInformation(),
                    WebDyntaxaRevisionReferenceRelationTestFactory.Create(revisionId, relatedObjectGUID, rel2Action));

                var newReferenceRelations = WebServiceProxy.DyntaxaInternalService.GetDyntaxaRevisionReferenceRelation(
                    GetClientInformation(), revisionId, relatedObjectGUID);

                // Assert
                Assert.IsNotNull(newReferenceRelations);
                Assert.AreEqual(2, newReferenceRelations.Count);
                Assert.AreEqual(rel1Action, newReferenceRelations[0].Action);
                Assert.AreEqual(rel2Action, newReferenceRelations[1].Action);
            }
        }

        [TestMethod]
        public void GetDyntaxaRevisionReferenceRelationById_WhenDyntaxaRevisionReferenceRelationCreated_ThenCreatedRevisionReferenceRelationIsReturned()
        {
            // Arrange
            const int revisionId = 24;
            const string relatedObjectGUID = "urn:lsid:dyntaxa.se:Taxon:6010174";
            const string action = "Add";
            const int referenceId = 252;
            WebDyntaxaRevisionReferenceRelation dyntaxaReferenceRelation = WebDyntaxaRevisionReferenceRelationTestFactory.Create(
                revisionId, relatedObjectGUID, action);
            dyntaxaReferenceRelation.ReferenceId = referenceId;            

            // Act
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetRevisionClientInformation(), WebServiceProxy.TaxonService))
            {
                var newReferenceRelations = WebServiceProxy.DyntaxaInternalService.CreateDyntaxaRevisionReferenceRelation(
                    GetClientInformation(), dyntaxaReferenceRelation);

                WebDyntaxaRevisionReferenceRelation getReferenceRelations = WebServiceProxy.DyntaxaInternalService
                    .GetDyntaxaRevisionReferenceRelationById(
                        GetClientInformation(), newReferenceRelations.Id);

                // Assert
                Assert.IsNotNull(getReferenceRelations);
                Assert.AreEqual(relatedObjectGUID, getReferenceRelations.RelatedObjectGUID);
                Assert.AreEqual(action, getReferenceRelations.Action);
                Assert.AreEqual(referenceId, getReferenceRelations.ReferenceId);
            }
        }


        [TestMethod]
        public void TestSetRevisionReferenceRelationPublished()
        {
            // Act
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetRevisionClientInformation(),  WebServiceProxy.TaxonService))
            {
                var result = WebServiceProxy.DyntaxaInternalService.SetRevisionReferenceRelationPublished(GetClientInformation(), 24);

                // Assert
                Assert.AreEqual(true, result);
            }
        }
        
        [TestMethod]
        public void Ping()
        {
            Boolean ping;
            
            ping = WebServiceProxy.DyntaxaInternalService.Ping();
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
            Configuration.InstallationType = InstallationType.TwoBlueberriesTest;

            loginResponse = WebServiceProxy.TaxonService.Login(Settings.Default.TestUserName,
                                                               Settings.Default.TestPassword,
                                                               Settings.Default.DyntaxaApplicationIdentifier,
                                                               false);
            _loginResponse = loginResponse;
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
        /// Creates a taxon name out of predefined data
        /// </summary>
        /// <returns>WebTaxonName </returns>
        private WebDyntaxaRevisionSpeciesFact GetReferenceDyntaxaRevisionSpeciesFact(int taxonId)
        {
            WebDyntaxaRevisionSpeciesFact refSpeciesFact = new WebDyntaxaRevisionSpeciesFact();
            refSpeciesFact.TaxonId = taxonId;
            refSpeciesFact.RevisionId = 1;
            refSpeciesFact.FactorId = (Int32)FactorId.SwedishOccurrence;
            refSpeciesFact.StatusId = 1;
            refSpeciesFact.QualityId = 1;
            refSpeciesFact.Description = "Test description";
            refSpeciesFact.ReferenceId = 1;
            refSpeciesFact.CreatedBy = Settings.Default.TestUserId;
            refSpeciesFact.CreatedDate = DateTime.Now;
            refSpeciesFact.IsRevisionEventIdSpecified = true;
            refSpeciesFact.RevisionEventId = 1;
            refSpeciesFact.IsChangedInRevisionEventIdSpecified = true;
            refSpeciesFact.ChangedInRevisionEventId = 1;
            refSpeciesFact.IsPublished = false;

            return refSpeciesFact;
        }

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

        

#endregion
    }
}

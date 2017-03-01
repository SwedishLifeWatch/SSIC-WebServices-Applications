using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.DyntaxaInternalService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebApplication.Dyntaxa.Test.Data
{
    [TestClass]
    public class DyntaxaInternalTaxonServiceTest : TestBase    
    {
        private DyntaxaInternalDataSource _dyntaxaInternalDataSource;

        //private DyntaxaInternalDataSource GetDyntaxaInternalDataSource(Boolean refresh = false)
        //{
        //    if (_dyntaxaInternalDataSource.IsNull() || refresh)
        //    {
        //        _dyntaxaInternalDataSource = new DyntaxaInternalDataSource();
        //    }
        //    return _dyntaxaInternalDataSource;
        //}

     
        [TestMethod]
        public void GetDyntaxaRevisionSpeciesFact_WhenRowIsCreatedAndLaterChanged_ThenLastChangedRowReturned()
        {
            DyntaxaRevisionSpeciesFact dyntaxaRevisionSpeciesFact1, dyntaxaRevisionSpeciesFact2, currentDyntaxaRevisionSpeciesFact;
            TaxonRevisionEvent revisionEvent1, revisionEvent2;
            Int32 revisionId = 1;

            //-------------------------------------------------------------------
            // Create first dyntaxa revision species fact and revision event.
            dyntaxaRevisionSpeciesFact1 = GetReferenceDyntaxaRevisionSpeciesFact((Int32)TaxonId.Bear);
            dyntaxaRevisionSpeciesFact1.FactorId = (Int32)FactorId.SwedishHistory;
            dyntaxaRevisionSpeciesFact1.RevisionId = revisionId;
            dyntaxaRevisionSpeciesFact1.IsPublished = false;
            dyntaxaRevisionSpeciesFact1.StatusId = 400;
            dyntaxaRevisionSpeciesFact1.ChangedInRevisionEventId = null;

            revisionEvent1 = new TaxonRevisionEvent()
            {
                CreatedBy = 2,
                CreatedDate = DateTime.Now,
                Type = new TaxonRevisionEventType() { Description = "", Id = 1, Identifier = "" },
                RevisionId = revisionId,
                AffectedTaxa = "Bear",
                OldValue = "StatusId=Unknown",
                NewValue = "StatusId=400"
            };
            
            IUserContext userContext = GetRevisionUserContext();
            DyntaxaInternalTaxonServiceManager manager = new DyntaxaInternalTaxonServiceManager();

            using (ITransaction transaction = userContext.StartTransaction())
            {
                revisionEvent1 = manager.CreateCompleteRevisionEvent(userContext, revisionEvent1);

                dyntaxaRevisionSpeciesFact1.RevisionEventId = revisionEvent1.Id;
                dyntaxaRevisionSpeciesFact1 = manager.CreateDyntaxaRevisionSpeciesFact(userContext,
                    dyntaxaRevisionSpeciesFact1);
                currentDyntaxaRevisionSpeciesFact = manager.GetDyntaxaRevisionSpeciesFact(
                    userContext, (Int32)FactorId.SwedishHistory, (Int32)TaxonId.Bear, revisionId);

                //Assert first Insert
                Assert.AreEqual(revisionEvent1.Id, dyntaxaRevisionSpeciesFact1.RevisionEventId);
                Assert.IsFalse(currentDyntaxaRevisionSpeciesFact.ChangedInRevisionEventId.HasValue);
                Assert.AreEqual(400, currentDyntaxaRevisionSpeciesFact.StatusId);
                Assert.AreEqual(dyntaxaRevisionSpeciesFact1.StatusId, currentDyntaxaRevisionSpeciesFact.StatusId);

                //---------------------------------------------------------------------
                // Create second dyntaxa revision species fact and revision event.
                dyntaxaRevisionSpeciesFact2 = GetReferenceDyntaxaRevisionSpeciesFact((Int32)TaxonId.Bear);
                dyntaxaRevisionSpeciesFact2.FactorId = (Int32)FactorId.SwedishHistory;
                dyntaxaRevisionSpeciesFact2.RevisionId = revisionId;
                dyntaxaRevisionSpeciesFact2.IsPublished = false;
                dyntaxaRevisionSpeciesFact2.StatusId = 450; // changed from 400 to 450.

                revisionEvent2 = new TaxonRevisionEvent()
                {
                    CreatedBy = 3, //Changed
                    CreatedDate = DateTime.Now,
                    Type = new TaxonRevisionEventType() { Description = "", Id = 1, Identifier = "" },
                    RevisionId = revisionId,
                    AffectedTaxa = "Bear",
                    OldValue = "StatusId=400", //Changed
                    NewValue = "StatusId=450" //Changed
                };
                revisionEvent2 = manager.CreateCompleteRevisionEvent(userContext, revisionEvent2);
                dyntaxaRevisionSpeciesFact2.RevisionEventId = revisionEvent2.Id;
                dyntaxaRevisionSpeciesFact2 = manager.CreateDyntaxaRevisionSpeciesFact(userContext,
                    dyntaxaRevisionSpeciesFact2);
                currentDyntaxaRevisionSpeciesFact = manager.GetDyntaxaRevisionSpeciesFact(
                    userContext, (Int32)FactorId.SwedishHistory, (Int32)TaxonId.Bear, revisionId);

                // Check values.
                Assert.AreEqual(450, currentDyntaxaRevisionSpeciesFact.StatusId);
                Assert.AreEqual(revisionEvent2.Id, currentDyntaxaRevisionSpeciesFact.RevisionEventId);
                Assert.IsFalse(currentDyntaxaRevisionSpeciesFact.ChangedInRevisionEventId.HasValue);
                // Check latest change is returned.
                Assert.AreEqual(dyntaxaRevisionSpeciesFact2.Id, currentDyntaxaRevisionSpeciesFact.Id);
                Assert.AreNotEqual(dyntaxaRevisionSpeciesFact1.Id, currentDyntaxaRevisionSpeciesFact.Id);
            }
        }

        [TestMethod]
        public void GetDyntaxaRevisionSpeciesFact_WhenRowDoesNotExist_ThenNullIsReturned()
        {
            DyntaxaRevisionSpeciesFact currentDyntaxaRevisionSpeciesFact;
            Int32 revisionId = -5000;
            Int32 taxonId = -50;

            IUserContext userContext = GetUserContext();
            DyntaxaInternalTaxonServiceManager manager = new DyntaxaInternalTaxonServiceManager();

            using (ITransaction transaction = userContext.StartTransaction())
            {                
                currentDyntaxaRevisionSpeciesFact = manager.GetDyntaxaRevisionSpeciesFact(userContext, (Int32)FactorId.SwedishHistory, taxonId, revisionId);

                Assert.IsNull(currentDyntaxaRevisionSpeciesFact);
            }
        }
        

        /// <summary>
        /// Creates a taxon name out of predefined data
        /// </summary>
        /// <returns>WebTaxonName </returns>
        private DyntaxaRevisionSpeciesFact GetReferenceDyntaxaRevisionSpeciesFact(int taxonId)
        {
            DyntaxaRevisionSpeciesFact refSpeciesFact = new DyntaxaRevisionSpeciesFact();
            refSpeciesFact.TaxonId = taxonId;
            refSpeciesFact.RevisionId = 1;
            refSpeciesFact.FactorId = (Int32)FactorId.SwedishOccurrence;
            refSpeciesFact.StatusId = 1;
            refSpeciesFact.QualityId = 1;
            refSpeciesFact.Description = "Test description";
            refSpeciesFact.ReferenceId = 1;
            refSpeciesFact.CreatedBy = Settings.Default.TestUserId;
            refSpeciesFact.CreatedDate = DateTime.Now;
            refSpeciesFact.RevisionEventId = 1;
            refSpeciesFact.ChangedInRevisionEventId = null;
            refSpeciesFact.IsPublished = false;

            return refSpeciesFact;
        }
    }
}

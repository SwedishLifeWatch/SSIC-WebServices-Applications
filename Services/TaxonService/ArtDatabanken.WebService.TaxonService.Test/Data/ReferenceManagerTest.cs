using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.TaxonService.Test.Data
{
    [TestClass]
    public class ReferenceManagerTest : TestBase
    {
        private ReferenceManager _referenceManager;

        public ReferenceManagerTest()
        {
            _referenceManager = null;
        }

        private ReferenceManager GetReferenceManager(Boolean refresh = false)
        {
            if (_referenceManager.IsNull() || refresh)
            {
                _referenceManager = new ReferenceManager();
            }

            return _referenceManager;
        }

        [TestMethod]
        public void GetReferenceRelationById()
        {
            Int32 referenceRelationId;
            WebReferenceRelation referenceRelation;

            referenceRelationId = 1;
            referenceRelation = GetReferenceManager(true).GetReferenceRelationById(GetContext(), referenceRelationId);
            Assert.IsNotNull(referenceRelation);
            Assert.AreEqual(referenceRelationId, referenceRelation.Id);
        }

        [TestMethod]
        public void GetReferenceRelationsByRelatedObjectGuid()
        {
            List<WebReferenceRelation> referenceRelations;
            WebReferenceRelation referenceRelation;

            referenceRelation = GetReferenceManager(true).GetReferenceRelationById(GetContext(), 1);
            referenceRelations = GetReferenceManager().GetReferenceRelationsByRelatedObjectGuid(GetContext(), referenceRelation.RelatedObjectGuid);
            Assert.IsTrue(referenceRelations.IsNotEmpty());
        }

        [TestMethod]
        public void GetReferenceRelationTypes()
        {
            List<WebReferenceRelationType> referenceRelationTypes;

            referenceRelationTypes = GetReferenceManager(true).GetReferenceRelationTypes(GetContext());
            Assert.IsTrue(referenceRelationTypes.IsNotEmpty());
        }
    }
}

using System;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebApplication.Dyntaxa.Test.Data.Extensions
{
    [TestClass]
    public class IReferenceRelationExtensionTest : TestBase
    {
        private IReferenceRelation _referenceRelation;

        [TestMethod]
        public void GetReference()
        {
            IReference reference;

            reference = GetReferenceRelation(true).GetReference(GetUserContext());
            Assert.IsNotNull(reference);
        }

        private IReferenceRelation GetReferenceRelation(Boolean refresh = false)
        {
            ReferenceRelationList referenceRelations;
            ITaxon taxon;

            if (_referenceRelation.IsNull() || refresh)
            {
                taxon = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Bear);
                referenceRelations = CoreData.ReferenceManager.GetReferenceRelations(GetUserContext(), taxon.Guid);
                _referenceRelation = referenceRelations[0];
            }
            return _referenceRelation;
        }
    }
}

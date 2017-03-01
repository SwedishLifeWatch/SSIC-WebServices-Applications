using System;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions;
using ArtDatabanken.WebService.Client.ReferenceService;
using ArtDatabanken.WebService.Client.TaxonService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebApplication.Dyntaxa.Test.Data.Extensions
{
    [TestClass]
    public class IReferenceManagerExtensionTest : TestBase
    {
        private IReferenceManager _referenceManager;

        [TestMethod]
        public void CreateDeleteReferenceRelations()
        {
            IReferenceRelation referenceRelation;
            ITaxon taxon;
            ReferenceRelationList referenceRelations;

            referenceRelations = new ReferenceRelationList();
            taxon = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Bear);
            referenceRelation = new ReferenceRelation();
            referenceRelation.DataContext = new DataContext(GetUserContext());
            referenceRelation.Reference = null;
            referenceRelation.ReferenceId = 100;
            referenceRelation.RelatedObjectGuid = taxon.Guid;
            referenceRelation.Type = CoreData.ReferenceManager.GetReferenceRelationType(GetUserContext(), ReferenceRelationTypeId.Source);
            referenceRelations.Add(referenceRelation);

            taxon = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Beaver);
            referenceRelation = new ReferenceRelation();
            referenceRelation.DataContext = new DataContext(GetUserContext());
            referenceRelation.Reference = null;
            referenceRelation.ReferenceId = 100;
            referenceRelation.RelatedObjectGuid = taxon.Guid;
            referenceRelation.Type = CoreData.ReferenceManager.GetReferenceRelationType(GetUserContext(), ReferenceRelationTypeId.Source);
            referenceRelations.Add(referenceRelation);
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                GetReferenceManager(true).CreateDeleteReferenceRelations(GetUserContext(), referenceRelations, referenceRelations);
            }
        }

        private IReferenceManager GetReferenceManager(Boolean refresh = false)
        {
            if (_referenceManager.IsNull() || refresh)
            {
                _referenceManager = new ReferenceManager();
                _referenceManager.DataSource = new ReferenceDataSource();
            }
            return _referenceManager;
        }
    }
}

using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.ReferenceService;
using ArtDatabanken.WebService.Client.TaxonService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class ReferenceManagerTest : TestBase
    {
        private ReferenceManager _referenceManager;

        public ReferenceManagerTest()
        {
            _referenceManager = null;
        }

        [TestMethod]
        public void CreateReference()
        {
            ReferenceList references;
            IReference reference;
            Int32 year;
            String title;
            String name;

            reference = new Reference();
            year = 2008;
            title = "Testtext";
            name = "TestName InsertTest";
            reference.Year = year;
            reference.Title = title;
            reference.Name = name;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                GetReferenceManager(true).CreateReference(GetUserContext(), reference);
                references = GetReferenceManager().GetReferences(GetUserContext());
                reference = references[references.Count - 1];

                Assert.AreEqual(year, reference.Year);
                Assert.AreEqual(name, reference.Name);
                Assert.AreEqual(title, reference.Title);
            }
        }

        [TestMethod]
        public void CreateReferenceRelation()
        {
            IReferenceRelation referenceRelation;
            ITaxon taxon;

            taxon = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Bear);
            referenceRelation = new ReferenceRelation();
            referenceRelation.DataContext = new DataContext(GetUserContext());
            referenceRelation.Reference = null;
            referenceRelation.ReferenceId = 100;
            referenceRelation.RelatedObjectGuid = taxon.Guid;
            referenceRelation.Type = CoreData.ReferenceManager.GetReferenceRelationType(GetUserContext(), ReferenceRelationTypeId.Source);
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                GetReferenceManager(true).CreateReferenceRelation(GetUserContext(), referenceRelation);
            }
        }

        [TestMethod]
        public void CreateReferenceRelations()
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
                GetReferenceManager(true).CreateReferenceRelations(GetUserContext(), referenceRelations);
            }
        }

        [TestMethod]
        public void DataSource()
        {
            Assert.IsNotNull(GetReferenceManager(true).DataSource);
        }

        [TestMethod]
        public void DeleteReferenceRelation()
        {
            IReferenceRelation referenceRelation;
            ITaxon taxon;

            taxon = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Bear);
            referenceRelation = new ReferenceRelation();
            referenceRelation.DataContext = new DataContext(GetUserContext());
            referenceRelation.Reference = null;
            referenceRelation.ReferenceId = 100;
            referenceRelation.RelatedObjectGuid = taxon.Guid;
            referenceRelation.Type = CoreData.ReferenceManager.GetReferenceRelationType(GetUserContext(), ReferenceRelationTypeId.Source);
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                GetReferenceManager(true).CreateReferenceRelation(GetUserContext(), referenceRelation);
                GetReferenceManager().DeleteReferenceRelation(GetUserContext(), referenceRelation);
            }
        }

        [TestMethod]
        public void DeleteReferenceRelations()
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
                GetReferenceManager(true).CreateReferenceRelations(GetUserContext(), referenceRelations);
                GetReferenceManager().DeleteReferenceRelations(GetUserContext(), referenceRelations);
            }
        }

        [TestMethod]
        public void GetDataSourceInformation()
        {
            IDataSourceInformation dataSourceInformation;

            dataSourceInformation = GetReferenceManager(true).GetDataSourceInformation();
            Assert.IsNotNull(dataSourceInformation);
        }

        [TestMethod]
        public void GetReferenceById()
        {
            Int32 referenceId;
            IReference reference;

            referenceId = 1;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                reference = GetReferenceManager(true).GetReference(GetUserContext(), referenceId);
                Assert.IsTrue(reference.IsNotNull());
                Assert.AreEqual(referenceId, reference.Id);
            }

            reference = GetReferenceManager().GetReference(GetUserContext(), referenceId);
            Assert.IsTrue(reference.IsNotNull());
            Assert.AreEqual(referenceId, reference.Id);
        }

        private ReferenceManager GetReferenceManager(Boolean refresh = false)
        {
            if (_referenceManager.IsNull() || refresh)
            {
                _referenceManager = new ReferenceManager();
                _referenceManager.DataSource = new ReferenceDataSource();
            }

            return _referenceManager;
        }

        [TestMethod]
        public void GetReferenceRelation()
        {
            Int32 referenceRelationId;
            IReferenceRelation referenceRelation;

            referenceRelationId = 1;
            referenceRelation = GetReferenceManager(true).GetReferenceRelation(GetUserContext(), referenceRelationId);
            Assert.IsNotNull(referenceRelation);
            Assert.AreEqual(referenceRelationId, referenceRelation.Id);
        }

        [TestMethod]
        public void GetReferenceRelations()
        {
            ReferenceRelationList referenceRelations;
            ITaxon taxon;

            taxon = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Bear);
            referenceRelations = GetReferenceManager(true).GetReferenceRelations(GetUserContext(), taxon.Guid);
            Assert.IsTrue(referenceRelations.IsNotEmpty());
            foreach (IReferenceRelation webReferenceRelation in referenceRelations)
            {
                Assert.AreEqual(taxon.Guid, webReferenceRelation.RelatedObjectGuid);
            }
        }

        [TestMethod]
        public void GetReferenceRelationType()
        {
            IReferenceRelationType referenceRelationType;

            GetReferenceManager(true);
            foreach (ReferenceRelationTypeId referenceRelationTypeId in Enum.GetValues(typeof(ReferenceRelationTypeId)))
            {
                referenceRelationType = GetReferenceManager().GetReferenceRelationType(GetUserContext(), (Int32)referenceRelationTypeId);
                Assert.IsNotNull(referenceRelationType);
                referenceRelationType = GetReferenceManager().GetReferenceRelationType(GetUserContext(), referenceRelationTypeId);
                Assert.IsNotNull(referenceRelationType);
            }
        }

        [TestMethod]
        public void GetReferenceRelationTypes()
        {
            ReferenceRelationTypeList referenceRelationTypes;

            referenceRelationTypes = GetReferenceManager(true).GetReferenceRelationTypes(GetUserContext());
            Assert.IsTrue(referenceRelationTypes.IsNotEmpty());
        }


        [TestMethod]
        public void GetReferences()
        {
            ReferenceList references;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                references = GetReferenceManager(true).GetReferences(GetUserContext());
                Assert.IsTrue(references.IsNotEmpty());
            }

            references = GetReferenceManager().GetReferences(GetUserContext());
            Assert.IsTrue(references.IsNotEmpty());
        }

        [TestMethod]
        public void GetReferencesByIds()
        {
            Int32 index;
            List<Int32> referenceIds;
            ReferenceList references;

            referenceIds = new List<Int32>();
            referenceIds.Add(1);
            referenceIds.Add(2);

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                references = GetReferenceManager(true).GetReferences(GetUserContext(), referenceIds);
                Assert.IsTrue(references.IsNotEmpty());
                Assert.AreEqual(referenceIds.Count, references.Count);
                for (index = 0; index < referenceIds.Count; index++)
                {
                    Assert.AreEqual(referenceIds[index], references[index].Id);
                }
            }

            references = GetReferenceManager().GetReferences(GetUserContext(), referenceIds);
            Assert.IsTrue(references.IsNotEmpty());
            Assert.AreEqual(referenceIds.Count, references.Count);
            for (index = 0; index < referenceIds.Count; index++)
            {
                Assert.AreEqual(referenceIds[index], references[index].Id);
            }
        }

        [TestMethod]
        public void GetReferencesBySearchCriteria()
        {
            ReferenceList references;
            IReferenceSearchCriteria searchCriteria;

            // Test name search criteria.
            searchCriteria = new ReferenceSearchCriteria();
            searchCriteria.NameSearchString = new StringSearchCriteria();
            searchCriteria.NameSearchString.SearchString = "2003";
            searchCriteria.NameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            references = GetReferenceManager(true).GetReferences(GetUserContext(), searchCriteria);
            Assert.IsTrue(references.IsNotEmpty());

            // Test title search criteria.
            searchCriteria = new ReferenceSearchCriteria();
            searchCriteria.TitleSearchString = new StringSearchCriteria();
            searchCriteria.TitleSearchString.SearchString = "2003";
            searchCriteria.TitleSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.TitleSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            references = GetReferenceManager().GetReferences(GetUserContext(), searchCriteria);
            Assert.IsTrue(references.IsNotEmpty());

            // Test year search criteria.
            searchCriteria = new ReferenceSearchCriteria();
            searchCriteria.Years = new List<Int32>();
            searchCriteria.Years.Add(2003);
            references = GetReferenceManager().GetReferences(GetUserContext(), searchCriteria);
            Assert.IsTrue(references.IsNotEmpty());
            searchCriteria.Years.Add(2004);
            references = GetReferenceManager().GetReferences(GetUserContext(), searchCriteria);
            Assert.IsTrue(references.IsNotEmpty());

            // Test logical operator.
            searchCriteria = new ReferenceSearchCriteria();
            searchCriteria.NameSearchString = new StringSearchCriteria();
            searchCriteria.NameSearchString.SearchString = "2003";
            searchCriteria.NameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            searchCriteria.TitleSearchString = new StringSearchCriteria();
            searchCriteria.TitleSearchString.SearchString = "2003";
            searchCriteria.TitleSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.TitleSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            searchCriteria.Years = new List<Int32>();
            searchCriteria.Years.Add(2003);

            searchCriteria.LogicalOperator = LogicalOperator.Or;
            references = GetReferenceManager().GetReferences(GetUserContext(), searchCriteria);
            Assert.IsTrue(references.IsNotEmpty());

            searchCriteria.LogicalOperator = LogicalOperator.And;
            references = GetReferenceManager().GetReferences(GetUserContext(), searchCriteria);
            Assert.IsTrue(references.IsEmpty());
        }

        protected override String GetTestApplicationName()
        {
            return ApplicationIdentifier.Dyntaxa.ToString();
        }

        [TestMethod]
        public void UpdateReference()
        {
            IReference oldReference;
            IReference reference;
            Int32? oldYear;
            String oldName;
            String oldText;

            oldReference = GetReferenceManager(true).GetReferences(GetUserContext())[1];
            Assert.IsTrue(oldReference.IsNotNull());

            oldYear = oldReference.Year;
            oldName = oldReference.Name;
            oldText = oldReference.Title;

            oldReference.Year = 1912;
            oldReference.Title = "Testtext";
            oldReference.Name = "TestName Test";

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                GetReferenceManager().UpdateReference(GetUserContext(), oldReference);
                reference = GetReferenceManager().GetReferences(GetUserContext())[1];

                Assert.AreEqual(oldReference.Id, reference.Id);
                if (oldYear.HasValue && reference.Year.HasValue)
                {
                    Assert.AreNotEqual(oldYear.Value, reference.Year.Value);
                }

                Assert.AreNotEqual(oldName, reference.Name);
                Assert.AreNotEqual(oldText, reference.Title);
            }
        }
    }
}

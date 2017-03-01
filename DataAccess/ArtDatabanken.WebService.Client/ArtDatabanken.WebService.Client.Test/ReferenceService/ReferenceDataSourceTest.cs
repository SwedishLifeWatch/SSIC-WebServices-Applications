using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.ReferenceService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.ReferenceService
{
    [TestClass]
    public class ReferenceDataSourceTest : TestBase
    {
        private ReferenceDataSource _referenceDataSource;

        public ReferenceDataSourceTest()
        {
            _referenceDataSource = null;
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
                GetReferenceDataSource(true).CreateReference(GetUserContext(), reference);
                references = GetReferenceDataSource().GetReferences(GetUserContext());
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

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                referenceRelation = new ReferenceRelation();
                referenceRelation.RelatedObjectGuid = "test:dyntaxa.se:1";
                referenceRelation.Type = CoreData.ReferenceManager.GetReferenceRelationType(GetUserContext(), 1);
                referenceRelation.ReferenceId = 171;
                GetReferenceDataSource(true).CreateReferenceRelation(GetUserContext(), referenceRelation);
                Assert.IsNotNull(referenceRelation);
                Assert.IsTrue(referenceRelation.Id > 0);
            }
        }

        [TestMethod]
        public void DeleteReferenceRelation()
        {
            IReferenceRelation referenceRelation;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                referenceRelation = new ReferenceRelation();
                referenceRelation.RelatedObjectGuid = "test:dyntaxa.se:1";
                referenceRelation.Type = CoreData.ReferenceManager.GetReferenceRelationType(GetUserContext(), 1);
                referenceRelation.ReferenceId = 171;
                GetReferenceDataSource(true).CreateReferenceRelation(GetUserContext(), referenceRelation);
                Assert.IsNotNull(referenceRelation);
                Assert.IsTrue(referenceRelation.Id > 0);
                GetReferenceDataSource().DeleteReferenceRelation(GetUserContext(), referenceRelation);
            }
        }

        private ReferenceDataSource GetReferenceDataSource(Boolean refresh = false)
        {
            if (_referenceDataSource.IsNull() || refresh)
            {
                _referenceDataSource = new ReferenceDataSource();
            }

            return _referenceDataSource;
        }

        [TestMethod]
        public void GetReferenceRelation()
        {
            Int32 referenceRelationId;
            IReferenceRelation referenceRelation;

            referenceRelationId = 1;
            referenceRelation = GetReferenceDataSource(true).GetReferenceRelation(GetUserContext(), referenceRelationId);
            Assert.IsNotNull(referenceRelation);
            Assert.AreEqual(referenceRelationId, referenceRelation.Id);
        }

        [TestMethod]
        public void GetReferenceRelationsByRelatedObjectGuid()
        {
            ReferenceRelationList referenceRelations;
            IReferenceRelation referenceRelation;

            referenceRelation = GetReferenceDataSource(true).GetReferenceRelation(GetUserContext(), 1);
            referenceRelations = GetReferenceDataSource().GetReferenceRelations(GetUserContext(), referenceRelation.RelatedObjectGuid);
            Assert.IsTrue(referenceRelations.IsNotEmpty());
        }

        [TestMethod]
        public void GetReferenceRelationTypes()
        {
            ReferenceRelationTypeList referenceRelationTypes;

            referenceRelationTypes = GetReferenceDataSource(true).GetReferenceRelationTypes(GetUserContext());
            Assert.IsTrue(referenceRelationTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetReferences()
        {
            ReferenceList references;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                references = GetReferenceDataSource(true).GetReferences(GetUserContext());
                Assert.IsTrue(references.IsNotEmpty());
            }

            references = GetReferenceDataSource().GetReferences(GetUserContext());
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
                references = GetReferenceDataSource(true).GetReferences(GetUserContext(), referenceIds);
                Assert.IsTrue(references.IsNotEmpty());
                Assert.AreEqual(referenceIds.Count, references.Count);
                for (index = 0; index < referenceIds.Count; index++)
                {
                    Assert.AreEqual(referenceIds[index], references[index].Id);
                }
            }

            references = GetReferenceDataSource().GetReferences(GetUserContext(), referenceIds);
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
            references = GetReferenceDataSource(true).GetReferences(GetUserContext(), searchCriteria);
            Assert.IsTrue(references.IsNotEmpty());

            // Test title search criteria.
            searchCriteria = new ReferenceSearchCriteria();
            searchCriteria.TitleSearchString = new StringSearchCriteria();
            searchCriteria.TitleSearchString.SearchString = "2003";
            searchCriteria.TitleSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.TitleSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            references = GetReferenceDataSource().GetReferences(GetUserContext(), searchCriteria);
            Assert.IsTrue(references.IsNotEmpty());

            // Test year search criteria.
            searchCriteria = new ReferenceSearchCriteria();
            searchCriteria.Years = new List<Int32>();
            searchCriteria.Years.Add(2003);
            references = GetReferenceDataSource().GetReferences(GetUserContext(), searchCriteria);
            Assert.IsTrue(references.IsNotEmpty());
            searchCriteria.Years.Add(2004);
            references = GetReferenceDataSource().GetReferences(GetUserContext(), searchCriteria);
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
            references = GetReferenceDataSource().GetReferences(GetUserContext(), searchCriteria);
            Assert.IsTrue(references.IsNotEmpty());

            searchCriteria.LogicalOperator = LogicalOperator.And;
            references = GetReferenceDataSource().GetReferences(GetUserContext(), searchCriteria);
            Assert.IsTrue(references.IsEmpty());
        }

        protected override String GetTestApplicationName()
        {
            return ApplicationIdentifier.EVA.ToString();
        }

        [TestMethod]
        public void UpdateReference()
        {
            IReference oldReference;
            IReference reference;
            Int32? oldYear;
            String oldName;
            String oldText;

            oldReference = GetReferenceDataSource(true).GetReferences(GetUserContext())[1];
            Assert.IsTrue(oldReference.IsNotNull());

            oldYear = oldReference.Year;
            oldName = oldReference.Name;
            oldText = oldReference.Title;

            oldReference.Year = 1912;
            oldReference.Title = "Testtext";
            oldReference.Name = "TestName Test";

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                GetReferenceDataSource().UpdateReference(GetUserContext(), oldReference);
                reference = GetReferenceDataSource().GetReferences(GetUserContext())[1];

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

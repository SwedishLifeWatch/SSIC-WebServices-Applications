using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.ReferenceService.Test.Data
{
    using ReferenceManager = ArtDatabanken.WebService.ReferenceService.Data.ReferenceManager;

    [TestClass]
    public class ReferenceManagerTest : TestBase 
    {
        [TestMethod]
        public void CreateReference()
        {
            List<WebReference> references;
            WebReference reference;
            Int32 year;
            String title;
            String name;

            reference = new WebReference();
            year = 2008;
            title = "Testtext";
            name = "TestName InsertTest";
            reference.Year = year;
            reference.Title = title;
            reference.Name = name;

            ReferenceManager.CreateReference(GetContext(), reference);
            references = ReferenceManager.GetReferences(GetContext());
            reference = references[references.Count - 1];

            Assert.AreEqual(year, reference.Year);
            Assert.AreEqual(name, reference.Name);
            Assert.AreEqual(title, reference.Title);

            // Test with polish characters.
            reference = new WebReference();
            year = 2008;
            title = @"TestŁtext";
            name = @"TestłName InsertTest";
            reference.Year = year;
            reference.Title = title;
            reference.Name = name;

            ReferenceManager.CreateReference(GetContext(), reference);
            references = ReferenceManager.GetReferences(GetContext());
            reference = references[references.Count - 1];

            Assert.AreEqual(year, reference.Year);
            Assert.AreEqual(name, reference.Name);
            Assert.AreEqual(title, reference.Title);
        }

        [TestMethod]
        public void CreateReferenceRelation()
        {
            WebReferenceRelation newReferenceRelation, referenceRelation;
            
            referenceRelation = new WebReferenceRelation();
            referenceRelation.RelatedObjectGuid = "test:dyntaxa.se:1";
            referenceRelation.TypeId = 1;
            referenceRelation.ReferenceId = 171;
            newReferenceRelation = ReferenceManager.CreateReferenceRelation(GetContext(), referenceRelation);
            Assert.IsNotNull(newReferenceRelation);
            Assert.IsTrue(newReferenceRelation.Id > 0);
        }

        [TestMethod]
        public void DeleteReferenceRelation()
        {
            WebReferenceRelation newReferenceRelation, referenceRelation;

            referenceRelation = new WebReferenceRelation();
            referenceRelation.RelatedObjectGuid = "test:dyntaxa.se:1";
            referenceRelation.TypeId = 1;
            referenceRelation.ReferenceId = 171;
            newReferenceRelation = ReferenceManager.CreateReferenceRelation(GetContext(), referenceRelation);
            Assert.IsNotNull(newReferenceRelation);
            Assert.IsTrue(newReferenceRelation.Id > 0);
            ReferenceManager.DeleteReferenceRelation(GetContext(), newReferenceRelation.Id);
        }

        [TestMethod]
        public void GetReferenceRelationById()
        {
            Int32 referenceRelationId;
            WebReferenceRelation referenceRelation;

            referenceRelationId = 1;
            referenceRelation = ReferenceManager.GetReferenceRelationById(GetContext(), referenceRelationId);
            Assert.IsNotNull(referenceRelation);
            Assert.AreEqual(referenceRelationId, referenceRelation.Id);
        }

        [TestMethod]
        public void GetReferenceRelationsByGuid()
        {
            List<WebReferenceRelation> referenceRelations;
            WebReferenceRelation referenceRelation;

            referenceRelation = ReferenceManager.GetReferenceRelationById(GetContext(), 1);
            referenceRelations = ReferenceManager.GetReferenceRelationsByGuid(GetContext(), referenceRelation.RelatedObjectGuid);
            Assert.IsTrue(referenceRelations.IsNotEmpty());
        }

        [TestMethod]
        public void GetReferenceRelationTypes()
        {
            List<WebReferenceRelationType> referenceRelationTypes;

            referenceRelationTypes = ReferenceManager.GetReferenceRelationTypes(GetContext());
            Assert.IsTrue(referenceRelationTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetReferences()
        {
            List<WebReference> references;

            references = ReferenceManager.GetReferences(GetContext());
            Assert.IsTrue(references.IsNotEmpty());
        }

        [TestMethod]
        public void GetReferencesByIds()
        {
            Int32 index;
            List<Int32> referenceIds;
            List<WebReference> references;

            referenceIds = new List<Int32>();
            referenceIds.Add(1);
            referenceIds.Add(2);
            references = ReferenceManager.GetReferencesByIds(GetContext(), referenceIds);
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
            List<WebReference> references;
            WebReferenceSearchCriteria searchCriteria;

            // Test name search criteria.
            searchCriteria = new WebReferenceSearchCriteria();
            searchCriteria.NameSearchString = new WebStringSearchCriteria();
            searchCriteria.NameSearchString.SearchString = "2003";
            searchCriteria.NameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            references = ReferenceManager.GetReferencesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(references.IsNotEmpty());

            // Test title search criteria.
            searchCriteria = new WebReferenceSearchCriteria();
            searchCriteria.TitleSearchString = new WebStringSearchCriteria();
            searchCriteria.TitleSearchString.SearchString = "2003";
            searchCriteria.TitleSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.TitleSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            references = ReferenceManager.GetReferencesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(references.IsNotEmpty());

            // Test year search criteria.
            searchCriteria = new WebReferenceSearchCriteria();
            searchCriteria.Years = new List<Int32>();
            searchCriteria.Years.Add(2003);
            references = ReferenceManager.GetReferencesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(references.IsNotEmpty());
            searchCriteria.Years.Add(2004);
            references = ReferenceManager.GetReferencesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(references.IsNotEmpty());

            // Test logical operator.
            searchCriteria = new WebReferenceSearchCriteria();
            searchCriteria.NameSearchString = new WebStringSearchCriteria();
            searchCriteria.NameSearchString.SearchString = "2003";
            searchCriteria.NameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            searchCriteria.TitleSearchString = new WebStringSearchCriteria();
            searchCriteria.TitleSearchString.SearchString = "2003";
            searchCriteria.TitleSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.TitleSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            searchCriteria.Years = new List<Int32>();
            searchCriteria.Years.Add(2003);

            searchCriteria.LogicalOperator = LogicalOperator.Or;
            references = ReferenceManager.GetReferencesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(references.IsNotEmpty());

            searchCriteria.LogicalOperator = LogicalOperator.And;
            references = ReferenceManager.GetReferencesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(references.IsEmpty());
        }

        [TestMethod]
        public void UpdateReference()
        {
            WebReference oldReference;
            WebReference reference;
            Int32 oldYear;
            String oldName;
            String oldText;

            oldReference = ReferenceManager.GetReferences(GetContext())[1];
            Assert.IsTrue(oldReference.IsNotNull());

            oldYear = oldReference.Year;
            oldName = oldReference.Name;
            oldText = oldReference.Title;

            oldReference.Year = 1912;
            oldReference.Title = "Testtext";
            oldReference.Name = "TestName Test";

            ReferenceManager.UpdateReference(GetContext(), oldReference);
            reference = ReferenceManager.GetReferences(GetContext())[1];

            Assert.AreEqual(oldReference.Id, reference.Id);
            Assert.AreNotEqual(oldYear, reference.Year);
            Assert.AreNotEqual(oldName, reference.Name);
            Assert.AreNotEqual(oldText, reference.Title);
        }
    }
}

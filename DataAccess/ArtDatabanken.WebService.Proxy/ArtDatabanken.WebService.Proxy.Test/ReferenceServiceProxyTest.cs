using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Proxy.Test
{
    [TestClass]
    public class ReferenceServiceProxyTest
    {
        private WebClientInformation _clientInformation;

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

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.ReferenceService))
            {
                WebServiceProxy.ReferenceService.CreateReference(GetClientInformation(), reference);
                references = WebServiceProxy.ReferenceService.GetReferences(GetClientInformation());
                reference = references[references.Count - 1];

                Assert.AreEqual(year, reference.Year);
                Assert.AreEqual(name, reference.Name);
                Assert.AreEqual(title, reference.Title);
            }
        }

        [TestMethod]
        public void CreateReferenceRelation()
        {
            WebReferenceRelation newReferenceRelation, referenceRelation;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.ReferenceService))
            {
                referenceRelation = new WebReferenceRelation();
                referenceRelation.RelatedObjectGuid = "test:dyntaxa.se:1";
                referenceRelation.TypeId = 1;
                referenceRelation.ReferenceId = 171;
                newReferenceRelation = WebServiceProxy.ReferenceService.CreateReferenceRelation(GetClientInformation(), referenceRelation);
                Assert.IsNotNull(newReferenceRelation);
                Assert.IsTrue(newReferenceRelation.Id > 0);
            }
        }

        [TestMethod]
        public void DeleteReferenceRelation()
        {
            WebReferenceRelation newReferenceRelation, referenceRelation;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.ReferenceService))
            {
                referenceRelation = new WebReferenceRelation();
                referenceRelation.RelatedObjectGuid = "test:dyntaxa.se:1";
                referenceRelation.TypeId = 1;
                referenceRelation.ReferenceId = 171;
                newReferenceRelation = WebServiceProxy.ReferenceService.CreateReferenceRelation(GetClientInformation(), referenceRelation);
                Assert.IsNotNull(newReferenceRelation);
                Assert.IsTrue(newReferenceRelation.Id > 0);
                WebServiceProxy.ReferenceService.DeleteReferenceRelation(GetClientInformation(), newReferenceRelation.Id);
            }
        }

        private WebClientInformation GetClientInformation()
        {
            return _clientInformation;
        }

        [TestMethod]
        public void GetReferenceRelationById()
        {
            Int32 referenceRelationId;
            WebReferenceRelation referenceRelation;

            referenceRelationId = 1;
            referenceRelation = WebServiceProxy.ReferenceService.GetReferenceRelationById(GetClientInformation(), referenceRelationId);
            Assert.IsNotNull(referenceRelation);
            Assert.AreEqual(referenceRelationId, referenceRelation.Id);
        }

        [TestMethod]
        public void GetReferenceRelationsByRelatedObjectGuid()
        {
            List<WebReferenceRelation> referenceRelations;
            WebReferenceRelation referenceRelation;

            referenceRelation = WebServiceProxy.ReferenceService.GetReferenceRelationById(GetClientInformation(), 1);
            referenceRelations = WebServiceProxy.ReferenceService.GetReferenceRelationsByRelatedObjectGuid(GetClientInformation(), referenceRelation.RelatedObjectGuid);
            Assert.IsTrue(referenceRelations.IsNotEmpty());
        }

        [TestMethod]
        public void GetReferenceRelationTypes()
        {
            List<WebReferenceRelationType> referenceRelationTypes;

            referenceRelationTypes = WebServiceProxy.ReferenceService.GetReferenceRelationTypes(GetClientInformation());
            Assert.IsTrue(referenceRelationTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetReferences()
        {
            List<WebReference> references;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.ReferenceService))
            {
                references = WebServiceProxy.ReferenceService.GetReferences(GetClientInformation());
                Assert.IsTrue(references.IsNotEmpty());
            }

            references = WebServiceProxy.ReferenceService.GetReferences(GetClientInformation());
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

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.ReferenceService))
            {
                references = WebServiceProxy.ReferenceService.GetReferencesByIds(GetClientInformation(), referenceIds);
                Assert.IsTrue(references.IsNotEmpty());
                Assert.AreEqual(referenceIds.Count, references.Count);
                for (index = 0; index < referenceIds.Count; index++)
                {
                    Assert.AreEqual(referenceIds[index], references[index].Id);
                }
            }

            references = WebServiceProxy.ReferenceService.GetReferencesByIds(GetClientInformation(), referenceIds);
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
            references = WebServiceProxy.ReferenceService.GetReferencesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(references.IsNotEmpty());

            // Test title search criteria.
            searchCriteria = new WebReferenceSearchCriteria();
            searchCriteria.TitleSearchString = new WebStringSearchCriteria();
            searchCriteria.TitleSearchString.SearchString = "2003";
            searchCriteria.TitleSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.TitleSearchString.CompareOperators.Add(StringCompareOperator.Contains);
            references = WebServiceProxy.ReferenceService.GetReferencesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(references.IsNotEmpty());

            // Test year search criteria.
            searchCriteria = new WebReferenceSearchCriteria();
            searchCriteria.Years = new List<Int32>();
            searchCriteria.Years.Add(2003);
            references = WebServiceProxy.ReferenceService.GetReferencesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(references.IsNotEmpty());
            searchCriteria.Years.Add(2004);
            references = WebServiceProxy.ReferenceService.GetReferencesBySearchCriteria(GetClientInformation(), searchCriteria);
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
            references = WebServiceProxy.ReferenceService.GetReferencesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(references.IsNotEmpty());

            searchCriteria.LogicalOperator = LogicalOperator.And;
            references = WebServiceProxy.ReferenceService.GetReferencesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(references.IsEmpty());
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                WebServiceProxy.UserService.Logout(_clientInformation);
                _clientInformation = null;
            }
            // ReSharper disable once EmptyGeneralCatchClause
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

            Configuration.InstallationType = InstallationType.ServerTest;
            loginResponse = WebServiceProxy.ReferenceService.Login(Settings.Default.TestUserName,
                                                                   Settings.Default.TestPassword,
                                                                   ApplicationIdentifier.EVA.ToString(),
                                                                   false);
            _clientInformation = new WebClientInformation();
            _clientInformation.Locale = loginResponse.Locale;
            _clientInformation.Token = loginResponse.Token;
        }

        [TestMethod]
        public void UpdateReference()
        {
            WebReference oldReference;
            WebReference reference;
            Int32 oldYear;
            String oldName;
            String oldText;

            oldReference = WebServiceProxy.ReferenceService.GetReferences(GetClientInformation())[1];
            Assert.IsTrue(oldReference.IsNotNull());

            oldYear = oldReference.Year;
            oldName = oldReference.Name;
            oldText = oldReference.Title;

            oldReference.Year = 1912;
            oldReference.Title = "Testtext";
            oldReference.Name = "TestName Test";

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.ReferenceService))
            {
                WebServiceProxy.ReferenceService.UpdateReference(GetClientInformation(), oldReference);
                reference = WebServiceProxy.ReferenceService.GetReferences(GetClientInformation())[1];

                Assert.AreEqual(oldReference.Id, reference.Id);
                Assert.AreNotEqual(oldYear, reference.Year);
                Assert.AreNotEqual(oldName, reference.Name);
                Assert.AreNotEqual(oldText, reference.Title);
            }
        }
    }
}

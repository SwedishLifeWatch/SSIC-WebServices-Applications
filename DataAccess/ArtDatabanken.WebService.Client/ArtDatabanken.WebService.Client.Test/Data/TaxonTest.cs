using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using System.Linq;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class TaxonTest : TestBase
    {
        Taxon _taxon;

        public TaxonTest()
        {
            _taxon = null;
        }

        [TestMethod]
        public void Constructor()
        {
            Taxon taxon;

            taxon = new Taxon();
            Assert.IsNotNull(taxon);
        }

        [TestMethod]
        public void DataContext()
        {
            Assert.IsNotNull(GetTaxon(true).DataContext);
        }

        [TestMethod]
        public void GetCheckedOutChangesTaxonName()
        {
            IList<ITaxonName> checkedOutChangesTaxonName;

            checkedOutChangesTaxonName = GetTaxon(true).GetCheckedOutChangesTaxonName(GetUserContext());
            Assert.IsNotNull(checkedOutChangesTaxonName);
            Assert.IsTrue(checkedOutChangesTaxonName.Count > 0);
        }

        [TestMethod]
        public void GetConceptDefinition()
        {            
            String conceptDefintion = GetTaxon(TaxonId.Catantopidae).GetConceptDefinition(GetUserContext());
            Assert.IsNotNull(conceptDefintion);
        }

        [TestMethod]
        public void GetCommonName()
        {
            DateTime today;
            ITaxonName taxonName;

            today = DateTime.Now;
            taxonName = GetTaxon(true).GetCommonName(GetUserContext());
            Assert.IsNotNull(taxonName);
            Assert.IsTrue(taxonName.IsRecommended);
            Assert.IsTrue(taxonName.ValidFromDate <= today);
            Assert.IsTrue(today <= taxonName.ValidToDate);
            Assert.IsTrue(taxonName.Category.Id == (Int32)(TaxonNameCategoryId.SwedishName));
        }

        [TestMethod]
        public void GetCurrentTaxonNames()
        {
            IList<ITaxonName> taxonNames;

            taxonNames = GetTaxon(true).GetCurrentTaxonNames(GetUserContext());
            Assert.IsNotNull(taxonNames);
            Assert.IsTrue(taxonNames.Count > 0);
        }

        [TestMethod]
        public void GetIdentifiers()
        {
            List<ITaxonName> identifiers;

            identifiers = GetTaxon(TaxonId.Butterflies).GetIdentifiers(GetUserContext());
            Assert.IsTrue(identifiers.IsNotEmpty());
            foreach (ITaxonName taxonName in identifiers)
            {
                Assert.IsTrue(taxonName.Category.Type.Id == (Int32)(TaxonNameCategoryTypeId.Identifier));
                Assert.IsTrue(!((taxonName.Category.Id == (Int32)(TaxonNameCategoryId.Guid)) &&
                                taxonName.IsRecommended));
            }
        }

        [TestMethod]
        public void GetModifiedByPerson()
        {
            _taxon = new Taxon();
            _taxon.ModifiedBy = 2;
            Person person = (Person) _taxon.GetModifiedByPerson(GetUserContext());
            Assert.AreEqual("TestFirstName TestLastName", person.FullName);
        }

        [TestMethod]
        public void GetRecommendedGUID()
        {
            DateTime today;
            String taxonName = null;

            today = DateTime.Now;
            taxonName = GetTaxon(true).GetRecommendedGuid(GetUserContext());
            Assert.IsNotNull(taxonName);
        }

        [TestMethod]
        public void GetScentificName()
        {
            DateTime today;
            ITaxonName taxonName;

            today = DateTime.Now;
            taxonName = GetTaxon(true).GetScientificName(GetUserContext());
            Assert.IsNotNull(taxonName);
            Assert.IsTrue(taxonName.IsRecommended);
            Assert.IsTrue(taxonName.ValidFromDate <= today);
            Assert.IsTrue(today <= taxonName.ValidToDate);
            Assert.IsTrue(taxonName.Category.Id == (Int32)(TaxonNameCategoryId.ScientificName));
        }

        [TestMethod]
        public void GetSynonyms()
        {
            DateTime today;
            List<ITaxonName> synonyms;

            today = DateTime.Now;
            synonyms = GetTaxon(TaxonId.GreenhouseMoths).GetSynonyms(GetUserContext(), true);
            Assert.IsTrue(synonyms.IsNotEmpty());

            List<int> nameUsageSynonymIds = new List<int>(4)
                {
                    (int)TaxonNameUsageId.Synonym,
                    (int)TaxonNameUsageId.Heterotypic,
                    (int)TaxonNameUsageId.Homotypic,
                    (int)TaxonNameUsageId.ProParteSynonym
                };
            
            foreach (ITaxonName taxonName in synonyms)
            {
                Assert.IsTrue((taxonName.ValidFromDate > today) ||
                              (today > taxonName.ValidToDate));

                Assert.IsTrue(nameUsageSynonymIds.Any(x => x == taxonName.NameUsage.Id)
                    || (taxonName.NameUsage.Id == (int)TaxonNameUsageId.Accepted
                        && taxonName.IsRecommended == false));                
            }
        }

        public static Taxon GetTaxon(IUserContext userContext)
        {
            Taxon taxon;

            taxon = new Taxon();
            taxon.DataContext = new DataContext(userContext);
            taxon.Id = Int32.MinValue;
            return taxon;
        }

        private Taxon GetTaxon(Boolean refresh = false, TaxonId? taxonId = null)
        {
            if (_taxon.IsNull() || refresh)
            {
               // _taxon = (Taxon)(CoreData.TaxonManager.GetTaxonById(GetUserContext(), TaxonId.Bear));
                if (taxonId.HasValue)
                {
                    _taxon = (Taxon)(CoreData.TaxonManager.GetTaxon(GetUserContext(), taxonId.Value));
                }
                else
                {
                    _taxon = (Taxon)(CoreData.TaxonManager.GetTaxon(GetUserContext(), 246126));
                }
            }
            return _taxon;
        }

        private Taxon GetTaxon(TaxonId taxonId)
        {
            _taxon = GetTaxon(GetUserContext(), taxonId);
            return _taxon;
        }

        private Taxon GetTaxon(IUserContext userContext, TaxonId taxonId)
        {
            _taxon = (Taxon)(CoreData.TaxonManager.GetTaxon(userContext, taxonId));
            return _taxon;
        }

        [TestMethod]
        public void GetTaxonNameById()
        {
            ITaxonName taxonName1;
            TaxonNameList taxonNames;

            taxonNames = GetTaxon(true).GetTaxonNames(GetUserContext());
            foreach (ITaxonName taxonName2 in taxonNames)
            {
                taxonName1 = GetTaxon().GetTaxonNameByVersion(GetUserContext(),
                                                         taxonName2.Id);
                Assert.IsNotNull(taxonName1);
                Assert.AreEqual(taxonName1.Id, taxonName2.Id);
            }
        }

        [TestMethod]
        public void GetTaxonNames()
        {
            TaxonNameList taxonNames;

            taxonNames = GetTaxon(true).GetTaxonNames(GetUserContext());
            Assert.IsTrue(taxonNames.IsNotEmpty());
        }

        /// <summary>
        /// Test some of the taxon properties
        /// </summary>
        [TestMethod]
        public void TaxonProperties()
        {
            // string taxonConceptDefFull = null;
            // GetTaxon(true).ConceptDefinitionFullGeneratedString = taxonConceptDefFull;
            // Assert.IsNull(GetTaxon().ConceptDefinitionFullGeneratedString);

          //  taxonConceptDefFull = "Hej666";
          //  GetTaxon(true).ConceptDefinitionFullGeneratedString = taxonConceptDefFull;
          //  Assert.AreEqual("Hej666", GetTaxon().ConceptDefinitionFullGeneratedString);


         //   string taxonConceptDefPart = null;
         //   GetTaxon(true).ConceptDefinitionFullGeneratedString = taxonConceptDefPart;
         //   Assert.IsNull(GetTaxon().ConceptDefinitionFullGeneratedString);

         //   taxonConceptDefPart = "7";
         //   GetTaxon(true).ConceptDefinitionPartString = taxonConceptDefPart;
         //   Assert.AreEqual("7", GetTaxon().ConceptDefinitionPartString);


            string taxonGuid = null;
            GetTaxon(true).Guid = taxonGuid;
            Assert.IsNull(GetTaxon().Guid);

            taxonGuid = "urn:lsid:dyntaxa.se:Taxon:Id:";
            GetTaxon(true).PartOfConceptDefinition = taxonGuid;
            Assert.AreEqual(taxonGuid, GetTaxon().Guid);

            DateTime taxonValidFromDate = DateTime.Now;
            GetTaxon(true).ValidFromDate = taxonValidFromDate;
            Assert.IsNotNull(GetTaxon().ValidFromDate);

        }

        [TestMethod]
        public void GetChildTaxonTree()
        {
            ITaxonTreeNode childTaxonTree;

            childTaxonTree = GetTaxon(true, TaxonId.Mammals).GetChildTaxonTree(GetUserContext(), true);
            Assert.IsNotNull(childTaxonTree);
            Assert.AreEqual(GetTaxon().Id, childTaxonTree.Taxon.Id);
            Assert.IsTrue(childTaxonTree.Parents.IsEmpty());
            Assert.IsTrue(childTaxonTree.Children.IsNotEmpty());

            childTaxonTree = GetTaxon(true, TaxonId.Mammals).GetChildTaxonTree(GetUserContext(), false);
            Assert.IsNotNull(childTaxonTree);
            Assert.AreEqual(GetTaxon().Id, childTaxonTree.Taxon.Id);
            Assert.IsTrue(childTaxonTree.Parents.IsEmpty());
            Assert.IsTrue(childTaxonTree.Children.IsNotEmpty());
        }

        [TestMethod]
        public void GetParentTaxonTree()
        {
            ITaxonTreeNode parentTaxonTree;

            parentTaxonTree = GetTaxon(true, TaxonId.Mammals).GetParentTaxonTree(GetUserContext(), true);
            //parentTaxonTree = GetTaxon(GetRevisionUserContext(), 6000732).GetParentTaxonTree(GetRevisionUserContext(), true);
            Assert.IsNotNull(parentTaxonTree);
            Assert.AreEqual(GetTaxon().Id, parentTaxonTree.Taxon.Id);
            Assert.IsTrue(parentTaxonTree.Children.IsEmpty());
            Assert.IsTrue(parentTaxonTree.Parents.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonTree()
        {
            ITaxonTreeNode taxonTreeNode;

            taxonTreeNode = GetTaxon(true, TaxonId.Mammals).GetTaxonTree(GetUserContext(), true);
            Assert.IsNotNull(taxonTreeNode);
            Assert.AreEqual(GetTaxon().Id, taxonTreeNode.Taxon.Id);
            Assert.IsTrue(taxonTreeNode.Children.IsNotEmpty());
            Assert.IsTrue(taxonTreeNode.Parents.IsNotEmpty());
        }

        [TestMethod]
        public void GetParentTaxonRelations()
        {
            Boolean isTaxonRevisionEditor = true;
            Boolean includeHistorical = false;
            Boolean isMainRelation = true;

            IList<ITaxonRelation> parentRelations = GetTaxon(true).GetParentTaxonRelations(GetUserContext(),
                                                                                           isTaxonRevisionEditor,
                                                                                           includeHistorical,
                                                                                           isMainRelation);
            Assert.IsTrue(parentRelations.Count> 0);
        }

        [TestMethod]
        public void GetAllParentTaxonRelations()
        {
            int? categoryId = null;
            Boolean isTaxonRevisionEditor = true;
            Boolean includeHistorical = false;
            Boolean isMainRelation = true;

            IList<ITaxonRelation> parentRelations = GetTaxon(true).GetAllParentTaxonRelations(GetUserContext(), categoryId, isTaxonRevisionEditor, includeHistorical, isMainRelation);
            Assert.IsTrue(parentRelations.Count > 0);
        }
    }
}

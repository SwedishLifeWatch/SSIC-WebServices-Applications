using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Test.Data.Extensions
{
    [TestClass]
    public class ITaxonExtensionTest : TestBase
    {
        ITaxon _taxon;

        public ITaxonExtensionTest()
        {
            _taxon = null;
        }

        [TestMethod]
        public void GetChildTaxonRelations()
        {
            // bufo
            Int32 taxonId = 1002294;
            ITaxon taxon;
            IList<ITaxonRelation> relations;
            bool isTaxonRevisionEditor = true;
            bool includeHistorical = false;
            bool isMainRelation = true;

            taxon = CoreData.TaxonManager.GetTaxon(GetUserContext(), taxonId);
            relations = taxon.GetChildTaxonRelations(GetUserContext(), isTaxonRevisionEditor, includeHistorical, isMainRelation);
            Assert.IsTrue(relations.IsNotNull());
            Assert.IsTrue(1 <= relations.Count);
        }

        [TestMethod]
        public void GetChildTaxonRelation_InRevision()
        {
            // Arrange
            ITaxon taxon = CoreData.TaxonManager.GetTaxon(GetRevisionUserContext(), 3000303);

            // Act
            IList<ITaxonRelation> t = taxon.GetChildTaxonRelations(GetRevisionUserContext(), true, false, true);

            // Assert
            Assert.IsTrue(t.Count == 3);
            foreach (var taxonRelation in t)
            {
                Assert.IsFalse(taxonRelation.ReplacedInTaxonRevisionEventId.HasValue);
            }
        }

        [TestMethod]
        public void GetChildTaxonRelation_InsideRevision_Historical()
        {
            // Arrange
            var taxon = CoreData.TaxonManager.GetTaxon(GetRevisionUserContext(), TaxonId.Carnivore);

            // Act
            IList<ITaxonRelation> childTaxonRelations = taxon.GetChildTaxonRelations(GetRevisionUserContext(), true, true, true);

            // Assert
            Assert.IsTrue(childTaxonRelations.IsNotNull());
            Assert.AreEqual(3, childTaxonRelations.Count);
        }

        [TestMethod]
        public void GetChildTaxonRelation_OutsideRevision()
        {
            // Arrange
            var taxon = CoreData.TaxonManager.GetTaxon(GetUserContext(), 3000303);

            // Act
            var t = taxon.GetChildTaxonRelations(GetUserContext(), false, false, true);

            // Assert
            Assert.IsTrue(t.Count == 3);
            foreach (var taxonRelation in t)
            {
                Assert.IsFalse(taxonRelation.ReplacedInTaxonRevisionEventId.HasValue);
            }
        }

        [TestMethod]
        public void GetChildTaxonRelation_OutsideRevision_Historical()
        {
            // Arrange
            var taxon = CoreData.TaxonManager.GetTaxon(GetUserContext(), 3000175);

            // Act
            var t = taxon.GetChildTaxonRelations(GetUserContext(), false, true, true);

            // Assert
            Assert.IsTrue(t.Count == 3);
        }

        [TestMethod]
        // Är beroende av att Revision 272 finns.
        public void GetTaxaPossibleParents()
        {
            ITaxonRevision taxonRevision;
            ITaxon taxon;
            TaxonList possibleParents;

            // Test taxon below genus.
            taxon = CoreData.TaxonManager.GetTaxon(GetRevisionUserContext(), TaxonId.Bear);
            taxonRevision = CoreData.TaxonManager.GetTaxonRevision(GetRevisionUserContext(), 272);
            possibleParents = taxon.GetTaxaPossibleParents(GetRevisionUserContext(), taxonRevision);
            Assert.IsTrue(possibleParents.IsNotEmpty());

            // Test taxon above genus.
            taxon = CoreData.TaxonManager.GetTaxon(GetRevisionUserContext(), TaxonId.Whales);
            taxonRevision = CoreData.TaxonManager.GetTaxonRevision(GetRevisionUserContext(), 272);
            possibleParents = taxon.GetTaxaPossibleParents(GetRevisionUserContext(), taxonRevision);
            Assert.IsTrue(possibleParents.IsNotEmpty());
        }

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTestApp")]
        public void GetTaxaPossibleParentsRecursion()
        {
            // This test does not work because infinite recursion occurs
            // in the stored procedure GetTaxonRelationsByTaxa().
            // The data model and the code must be changed to handle
            // this problem.
            ITaxonRevision taxonRevision;
            ITaxon taxon;
            TaxonList possibleParents;

            // Test taxon below genus.
            taxon = CoreData.TaxonManager.GetTaxon(GetRevisionUserContext(), TaxonId.Bear);
            taxonRevision = CoreData.TaxonManager.GetTaxonRevision(GetRevisionUserContext(), 1);
            possibleParents = taxon.GetTaxaPossibleParents(GetRevisionUserContext(), taxonRevision);
            Assert.IsTrue(possibleParents.IsNotEmpty());

            // Test taxon above genus.
            taxon = CoreData.TaxonManager.GetTaxon(GetRevisionUserContext(), TaxonId.Whales);
            taxonRevision = CoreData.TaxonManager.GetTaxonRevision(GetRevisionUserContext(), 1);
            possibleParents = taxon.GetTaxaPossibleParents(GetRevisionUserContext(), taxonRevision);
            Assert.IsTrue(possibleParents.IsNotEmpty());
        }

        private ITaxon GetTaxon(Boolean refresh = false, TaxonId? taxonId = null)
        {
            if (_taxon.IsNull() || refresh)
            {
                if (taxonId.HasValue)
                {
                    _taxon = CoreData.TaxonManager.GetTaxon(GetUserContext(), taxonId.Value);
                }
                else
                {
                    _taxon = CoreData.TaxonManager.GetTaxon(GetUserContext(), 246126);
                }
            }
            return _taxon;
        }

        [TestMethod]
        public void IsBelowGenus()
        {
            Assert.IsTrue(GetTaxon(true, TaxonId.Bear).IsBelowGenus(GetUserContext()));
            Assert.IsFalse(GetTaxon(true, TaxonId.Mammals).IsBelowGenus(GetUserContext()));
        }
    }
}

using System;
using ArtDatabanken.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class TaxonTreeNodeTest : TestBase
    {
        private TaxonTreeNode _taxonTreeNode;

        public TaxonTreeNodeTest()
        {
            _taxonTreeNode = null;
        }

        [TestMethod]
        public void Constructor()
        {
            TaxonTreeNode taxonTreeNode;

            taxonTreeNode = new TaxonTreeNode();
            Assert.IsNotNull(taxonTreeNode);
        }

        [TestMethod]
        public void GetChildTaxa()
        {
            TaxonList childTaxa;

            childTaxa = GetTaxonTreeNode(true).GetChildTaxa();
            Assert.IsTrue(childTaxa.IsNotEmpty());
        }

        [TestMethod]
        public void GetChildTaxonCategories()
        {
            TaxonCategoryList childTaxonCategories;

            childTaxonCategories = GetTaxonTreeNode(true).GetChildTaxonCategories();
            Assert.IsTrue(childTaxonCategories.IsNotEmpty());
        }

        [TestMethod]
        public void GetParentTaxa()
        {
            TaxonList parentTaxa;

            parentTaxa = GetTaxonTreeNode(true).GetParentTaxa();
            Assert.IsTrue(parentTaxa.IsNotEmpty());
        }

        [TestMethod]
        public void GetParentTaxonCategories()
        {
            TaxonCategoryList parentTaxonCategories;

            parentTaxonCategories = GetTaxonTreeNode(true).GetParentTaxonCategories();
            Assert.IsTrue(parentTaxonCategories.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxa()
        {
            TaxonList taxa;

            taxa = GetTaxonTreeNode(true).GetTaxa();
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        private TaxonTreeNode GetTaxonTreeNode(Boolean refresh = false, TaxonId? taxonId = null)
        {
            ITaxon taxon;

            if (_taxonTreeNode.IsNull() || refresh)
            {
                if (taxonId.HasValue)
                {
                    taxon = CoreData.TaxonManager.GetTaxon(GetUserContext(), taxonId.Value);
                }
                else
                {
                    taxon = CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.Mammals);
                }
                _taxonTreeNode = (TaxonTreeNode)(taxon.GetTaxonTree(GetUserContext(), true));
            }
            return _taxonTreeNode;
        }

        [TestMethod]
        public void GetTaxonTreeNodes()
        {
            TaxonTreeNodeList taxonTreeNodes;

            taxonTreeNodes = GetTaxonTreeNode(true).GetTaxonTreeNodes();
            Assert.IsTrue(taxonTreeNodes.IsNotEmpty());
        }
    }
}

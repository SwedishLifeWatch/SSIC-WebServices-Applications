using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class TaxonCategoryListTest : TestBase
    {
        private TaxonCategoryList _taxonCategoryList;

        public TaxonCategoryListTest()
        {
            _taxonCategoryList = null;
        }

        [TestMethod]
        public void Constructor()
        {
            TaxonCategoryList taxonCategoryList;

            taxonCategoryList = new TaxonCategoryList();
            Assert.IsNotNull(taxonCategoryList);
        }

        [TestMethod]
        public void Get()
        {
            GetTaxonCategories(true);
            foreach (ITaxonCategory taxonCategory in GetTaxonCategories())
            {
                Assert.AreEqual(taxonCategory, GetTaxonCategories().Get(taxonCategory.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 taxonCategoryId;

            taxonCategoryId = Int32.MaxValue;
            GetTaxonCategories(true).Get(taxonCategoryId);
        }

        private TaxonCategoryList GetTaxonCategories()
        {
            return GetTaxonCategories(false);
        }

        private TaxonCategoryList GetTaxonCategories(Boolean refresh)
        {
            if (_taxonCategoryList.IsNull() || refresh)
            {
                _taxonCategoryList = new TaxonCategoryList();
                _taxonCategoryList.Add(TaxonCategoryTest.GetTaxonCategory(GetUserContext()));
            }
            return _taxonCategoryList;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            TaxonCategoryList newTaxonCategoryList, oldTaxonCategoryList;
            Int32 taxonCategoryIndex;

            oldTaxonCategoryList = GetTaxonCategories(true);
            newTaxonCategoryList = new TaxonCategoryList();
            for (taxonCategoryIndex = 0; taxonCategoryIndex < oldTaxonCategoryList.Count; taxonCategoryIndex++)
            {
                newTaxonCategoryList.Add(oldTaxonCategoryList[oldTaxonCategoryList.Count - taxonCategoryIndex - 1]);
            }
            for (taxonCategoryIndex = 0; taxonCategoryIndex < oldTaxonCategoryList.Count; taxonCategoryIndex++)
            {
                Assert.AreEqual(newTaxonCategoryList[taxonCategoryIndex], oldTaxonCategoryList[oldTaxonCategoryList.Count - taxonCategoryIndex - 1]);
            }
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class TaxonNameCategoryListTest : TestBase
    {
        private TaxonNameCategoryList _TaxonNameCategoryList;

        public TaxonNameCategoryListTest()
        {
            _TaxonNameCategoryList = null;
        }

        [TestMethod]
        public void Constructor()
        {
            TaxonNameCategoryList TaxonNameCategoryList;

            TaxonNameCategoryList = new TaxonNameCategoryList();
            Assert.IsNotNull(TaxonNameCategoryList);
        }

        [TestMethod]
        public void Get()
        {
            GetTaxonNameCategories(true);
            foreach (ITaxonNameCategory TaxonNameCategory in GetTaxonNameCategories())
            {
                Assert.AreEqual(TaxonNameCategory, GetTaxonNameCategories().Get(TaxonNameCategory.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 TaxonNameCategoryId;

            TaxonNameCategoryId = Int32.MaxValue;
            GetTaxonNameCategories(true).Get(TaxonNameCategoryId);
        }

        private TaxonNameCategoryList GetTaxonNameCategories()
        {
            return GetTaxonNameCategories(false);
        }

        private TaxonNameCategoryList GetTaxonNameCategories(Boolean refresh)
        {
            if (_TaxonNameCategoryList.IsNull() || refresh)
            {
                _TaxonNameCategoryList = new TaxonNameCategoryList();
                _TaxonNameCategoryList.Add(CoreData.TaxonManager.GetTaxonNameCategory(GetUserContext(), (Int32)(TaxonNameCategoryId.ScientificName)));
            }
            return _TaxonNameCategoryList;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            TaxonNameCategoryList newTaxonNameCategoryList, oldTaxonNameCategoryList;
            Int32 TaxonNameCategoryIndex;

            oldTaxonNameCategoryList = GetTaxonNameCategories(true);
            newTaxonNameCategoryList = new TaxonNameCategoryList();
            for (TaxonNameCategoryIndex = 0; TaxonNameCategoryIndex < oldTaxonNameCategoryList.Count; TaxonNameCategoryIndex++)
            {
                newTaxonNameCategoryList.Add(oldTaxonNameCategoryList[oldTaxonNameCategoryList.Count - TaxonNameCategoryIndex - 1]);
            }
            for (TaxonNameCategoryIndex = 0; TaxonNameCategoryIndex < oldTaxonNameCategoryList.Count; TaxonNameCategoryIndex++)
            {
                Assert.AreEqual(newTaxonNameCategoryList[TaxonNameCategoryIndex], oldTaxonNameCategoryList[oldTaxonNameCategoryList.Count - TaxonNameCategoryIndex - 1]);
            }
        }
    }
}

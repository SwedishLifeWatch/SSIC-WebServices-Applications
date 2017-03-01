using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class RegionCategoryListTest : TestBase
    {
        private RegionCategoryList _regionCategories;

        public RegionCategoryListTest()
        {
            _regionCategories = null;
        }

        [TestMethod]
        public void Constructor()
        {
            RegionCategoryList regionCategories;

            regionCategories = new RegionCategoryList();
            Assert.IsNotNull(regionCategories);
        }

        [TestMethod]
        public void Get()
        {
            GetRegionCategories(true);
            foreach (IRegionCategory regionCategory in GetRegionCategories())
            {
                Assert.AreEqual(regionCategory, GetRegionCategories().Get(regionCategory.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 regionCategoryId;

            regionCategoryId = Int32.MaxValue;
            GetRegionCategories(true).Get(regionCategoryId);
        }

        private RegionCategoryList GetRegionCategories()
        {
            return GetRegionCategories(false);
        }

        private RegionCategoryList GetRegionCategories(Boolean refresh)
        {
            if (_regionCategories.IsNull() || refresh)
            {
                _regionCategories = new RegionCategoryList();
                _regionCategories.Add(RegionCategoryTest.GetOneRegionCategory(GetUserContext()));
            }
            return _regionCategories;
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class SpeciesActivityCategoryListTest : TestBase
    {
        private SpeciesActivityCategoryList _speciesActivityCategoryList;

        public SpeciesActivityCategoryListTest()
        {
            _speciesActivityCategoryList = null;
        }

        [TestMethod]
        public void Constructor()
        {
            SpeciesActivityCategoryList speciesActivityCategoryList = new SpeciesActivityCategoryList();
            Assert.IsNotNull(speciesActivityCategoryList);
        }

        [TestMethod]
        public void Get()
        {
            GetSpeciesActivityCategories(true);
            foreach (ISpeciesActivityCategory speciesActivityCategory in GetSpeciesActivityCategories())
            {
                Assert.AreEqual(speciesActivityCategory, GetSpeciesActivityCategories().Get(speciesActivityCategory.Id));
            }
        }

        private SpeciesActivityCategoryList GetSpeciesActivityCategories()
        {
            return GetSpeciesActivityCategories(false);
        }

        private SpeciesActivityCategoryList GetSpeciesActivityCategories(Boolean refresh)
        {
            if (_speciesActivityCategoryList.IsNull() || refresh)
            {
                _speciesActivityCategoryList = new SpeciesActivityCategoryList();
                _speciesActivityCategoryList.Add(SpeciesActivityCategoryTest.GetOneSpeciesActivityCategory(GetUserContext()));
            }
            return _speciesActivityCategoryList;
        }

    }
}

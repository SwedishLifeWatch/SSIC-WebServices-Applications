using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class SpeciesActivityCategoryTest : TestBase
    {
        SpeciesActivityCategory _speciesActivityCategory;

        public SpeciesActivityCategoryTest()
        {
            _speciesActivityCategory = null;
        }

        [TestMethod]
        public void Constructor()
        {
            SpeciesActivityCategory speciesActivityCategory = new SpeciesActivityCategory();
            Assert.IsNotNull(speciesActivityCategory);
        }

        [TestMethod]
        public void DataContext()
        {
            Assert.IsNotNull(GetSpeciesActivityCategory(true).DataContext);
        }



        private SpeciesActivityCategory GetSpeciesActivityCategory(Boolean refresh)
        {
            if (_speciesActivityCategory.IsNull() || refresh)
            {
                _speciesActivityCategory = new SpeciesActivityCategory();
            }
            return _speciesActivityCategory;
        }

        public static SpeciesActivityCategory GetOneSpeciesActivityCategory(IUserContext userContext)
        {
            return new SpeciesActivityCategory();
        }

    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class RegionCategoryTest : TestBase
    {


        [TestMethod]
        public void Constructor()
        {
            RegionCategory regionCategory;

            regionCategory = GetOneRegionCategory(GetUserContext());
            Assert.IsNotNull(regionCategory);
        }

        public static RegionCategory GetOneRegionCategory(IUserContext userContext)
        {
            Int32 regionCategoryId = 1;
            Int32? countryIsoCode = 752;
            String guid = "urn:ArtDatabanken.RegionCategory:1";
            Int32 level = 1;
            String name = "Name";
            String nativeSourceId = "1";
            Int32 sortOrder = 1;
            Int32 typeId = 1;
            return new RegionCategory(regionCategoryId, countryIsoCode, guid, level, name, nativeSourceId, sortOrder, typeId, new DataContext(userContext));

        }

        [TestMethod]
        public void RegionCategoryId()
        {
            Int32 regionCategoryId;

            regionCategoryId = 1;
            RegionCategory regionCategory = GetOneRegionCategory(GetUserContext());
            regionCategory.Id = regionCategoryId;
            Assert.IsNotNull(regionCategory.Id);
            Assert.AreEqual(regionCategory.Id, regionCategoryId);

        }

        [TestMethod]
        public void RegionCategoryName()
        {
            String name = "Name";
            RegionCategory regionCategory = GetOneRegionCategory(GetUserContext());
            Assert.AreEqual(regionCategory.Name, name);
        }
        
    }
}

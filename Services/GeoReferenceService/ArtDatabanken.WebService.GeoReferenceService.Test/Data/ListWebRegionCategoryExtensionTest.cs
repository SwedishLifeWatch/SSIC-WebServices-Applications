using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.GeoReferenceService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.GeoReferenceService.Test.Data
{
    [TestClass]
    public class ListWebRegionCategoryExtensionTest : TestBase
    {
        [TestMethod]
        public void WebToString()
        {
            List<WebRegionCategory> regionCategories;
            String regionCategoriesString;

            regionCategories = GeoReferenceService.Data.RegionManager.GetRegionCategories(GetContext(), false, 1);
            regionCategoriesString = regionCategories.WebToString();
            Assert.IsTrue(regionCategoriesString.IsNotEmpty());
        }
    }
}

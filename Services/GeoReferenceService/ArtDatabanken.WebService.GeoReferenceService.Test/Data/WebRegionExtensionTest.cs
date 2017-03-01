using System;
using System.Collections.Generic;
using ArtDatabanken.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Data;

namespace ArtDatabanken.WebService.GeoReferenceService.Test.Data
{
    [TestClass]
    public class WebRegionExtensionTest : TestBase
    {
        [TestMethod]
        public void LoadData()
        {
            Boolean isRegionLoaded;
            List<Int32> regionCategoryIds;
            RegionGUID regionGUID;
            WebRegion region;

            isRegionLoaded = false;
            regionCategoryIds = new List<Int32>();
            regionCategoryIds.Add(18);
            regionCategoryIds.Add(21);
            using (DataReader dataReader = GetContext().GetGeoReferenceDatabase().GetRegionsByCategories(regionCategoryIds))
            {
                while (dataReader.Read())
                {
                    isRegionLoaded = true;
                    region = new WebRegion();
                    region.LoadData(dataReader);

                    Assert.IsTrue(0 <= region.CategoryId);
                    regionGUID = new RegionGUID(region.GUID);
                    Assert.AreEqual(region.CategoryId, regionGUID.CategoryId);
                    Assert.AreEqual(region.NativeId, regionGUID.NativeId);
                    Assert.IsTrue(0 <= region.Id);
                    Assert.IsTrue(region.Name.IsNotEmpty());
                    Assert.IsTrue(region.NativeId.IsNotEmpty());
                    // ShortName can have any value including null.
                    Assert.AreEqual(Int32.MinValue, region.SortOrder);
                }
            }
            Assert.IsTrue(isRegionLoaded);
        }
    }
}

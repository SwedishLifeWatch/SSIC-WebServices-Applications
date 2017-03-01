using System;
using ArtDatabanken.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.GeoReferenceService.Data;

namespace ArtDatabanken.WebService.GeoReferenceService.Test.Data
{
    [TestClass]
    public class WebRegionCategoryExtensionTest : TestBase
    {
        [TestMethod]
        public void LoadData()
        {
            Boolean isRegionCategoryLoaded;
            WebRegionCategory regionCategory;

            isRegionCategoryLoaded = false;
            using (DataReader dataReader = GetContext().GetGeoReferenceDatabase().GetRegionCategories())
            {
                while (dataReader.Read())
                {
                    isRegionCategoryLoaded = true;
                    regionCategory = new WebRegionCategory();
                    regionCategory.LoadData(dataReader);

                    if (regionCategory.IsCountryIsoCodeSpecified)
                    {
                        Assert.IsTrue(0 <= regionCategory.CountryIsoCode);
                    }
                    Assert.IsTrue(regionCategory.GUID.IsEmpty());
                    Assert.IsTrue(0 <= regionCategory.Id);
                    if (regionCategory.IsLevelSpecified)
                    {
                        Assert.IsTrue(0 <= regionCategory.Level);
                    }
                    Assert.IsTrue(regionCategory.Name.IsNotEmpty());
                    Assert.IsTrue(regionCategory.NativeIdSource.IsEmpty());
                    Assert.IsTrue(0 <= regionCategory.SortOrder);
                    Assert.IsTrue(0 <= regionCategory.TypeId);
                }
            }

            Assert.IsTrue(isRegionCategoryLoaded);
        }
    }
}

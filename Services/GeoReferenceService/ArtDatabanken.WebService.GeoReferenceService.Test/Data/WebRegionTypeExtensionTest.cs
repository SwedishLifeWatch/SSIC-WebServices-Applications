using System;
using ArtDatabanken.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.GeoReferenceService.Data;

namespace ArtDatabanken.WebService.GeoReferenceService.Test.Data
{
    [TestClass]
    public class WebRegionTypeExtensionTest : TestBase
    {
        [TestMethod]
        public void LoadData()
        {
            Boolean isRegionTypeLoaded;
            WebRegionType regionType;

            isRegionTypeLoaded = false;
            using (DataReader dataReader = GetContext().GetGeoReferenceDatabase().GetRegionTypes())
            {
                while (dataReader.Read())
                {
                    isRegionTypeLoaded = true;
                    regionType = new WebRegionType();
                    regionType.LoadData(dataReader);

                    Assert.IsTrue(0 <= regionType.Id);
                    Assert.IsTrue(regionType.Name.IsNotEmpty());
                }
            }
            Assert.IsTrue(isRegionTypeLoaded);
        }
    }
}

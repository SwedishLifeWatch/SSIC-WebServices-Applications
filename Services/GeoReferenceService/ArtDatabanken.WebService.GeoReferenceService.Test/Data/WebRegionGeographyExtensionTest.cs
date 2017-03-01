using System;
using System.Collections.Generic;
using ArtDatabanken.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.GeoReferenceService.Data;

namespace ArtDatabanken.WebService.GeoReferenceService.Test.Data
{
    [TestClass]
    public class WebRegionGeographyExtensionTest : TestBase
    {
        [TestMethod]
        public void LoadData()
        {
            Boolean isRegionGeographyLoaded;
            List<String> regionGuids;
            WebRegionGeography regionGeography;

            isRegionGeographyLoaded = false;
            regionGuids = new List<String>();
            regionGuids.Add(Settings.Default.ProvinceBlekingeGUID);
            regionGuids.Add(Settings.Default.ProvinceSkaneGUID);
            using (DataReader dataReader = GetContext().GetGeoReferenceDatabase().GetRegionsGeographyByGUIDs(regionGuids))
            {
                while (dataReader.Read())
                {
                    isRegionGeographyLoaded = true;
                    regionGeography = new WebRegionGeography();
                    regionGeography.LoadData(dataReader);

                    Assert.IsTrue(0 <= regionGeography.Id);
                    Assert.IsNotNull(regionGeography.BoundingBox);
                    Assert.IsTrue(regionGeography.GUID.IsNotEmpty());
                    Assert.IsNotNull(regionGeography.MultiPolygon);
                }
            }
            Assert.IsTrue(isRegionGeographyLoaded);
        }
    }
}

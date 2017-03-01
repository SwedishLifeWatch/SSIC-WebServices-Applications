using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Data;
using ArtDatabanken.WebService.SwedishSpeciesObservationService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Test.Data
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
            using (DataReader dataReader = Context.GetSpeciesObservationDatabase().GetCountyRegions())
            {
                while (dataReader.Read())
                {
                    isRegionLoaded = true;
                    region = new WebRegion();
                    region.LoadData(dataReader);

                    Assert.IsTrue(0 <= region.CategoryId);
                    regionGUID = new RegionGUID(region.GUID);
                    Assert.AreEqual((object) region.CategoryId, regionGUID.CategoryId);
                    Assert.AreEqual((object) region.NativeId, regionGUID.NativeId);
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

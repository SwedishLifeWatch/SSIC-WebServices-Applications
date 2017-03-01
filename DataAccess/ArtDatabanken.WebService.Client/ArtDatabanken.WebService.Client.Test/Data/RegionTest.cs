using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class RegionTest : TestBase
    {
        Region _region;

        public RegionTest()
        {
            _region = null;
        }

        [TestMethod]
        public void Constructor()
        {
            Region region;

            region = new Region(new DataContext(GetUserContext()));
            Assert.IsNotNull(region);
        }

        [TestMethod]
        public void DataContext()
        {
            Assert.IsNotNull(GetRegion(true).DataContext);
        }

        public static Region GetOneRegion(IUserContext userContext)
        {
            return new Region(new DataContext(userContext));
        }

        public static Region GetOneRegion(IUserContext userContext, Boolean createEmpty)
        {
            if (createEmpty)
            {
                return GetOneRegion(userContext);
            }
            else
            {
                Int32 regionId = 1;
                Int32 categoryId = 1;
                String guid = "urn:ArtDatabanken.Region:1";
                String name = "Uppsala";
                String nativeId = "1";
                String shortName = "Uppsala";
                Int32 sortOrder = 1;
                return new Region(regionId, categoryId, guid, name, nativeId, shortName, sortOrder, new DataContext(userContext));
            }
        }

        private Region GetRegion()
        {
            return GetRegion(false);
        }

        private Region GetRegion(Boolean refresh)
        {
            if (_region.IsNull() || refresh)
            {
                _region = new Region(new DataContext(GetUserContext()));
            }
            return _region;
        }

        [TestMethod]
        public void RegionId()
        {
            Int32 regionId;

            regionId = 1;
            GetRegion(true).Id = regionId;
            Assert.IsNotNull(GetRegion().Id);

            regionId = 1;
            GetRegion().Id = regionId;
            Assert.AreEqual(GetRegion().Id, regionId);

        }

        [TestMethod]
        public void RegionName()
        {
            String name = "Uppsala";
            Region region = GetOneRegion(GetUserContext(), false);
            Assert.AreEqual(region.Name, name);
        }
        
    }
}

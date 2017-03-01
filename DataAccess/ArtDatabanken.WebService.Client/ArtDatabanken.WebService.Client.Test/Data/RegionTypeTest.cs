using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class RegionTypeTest : TestBase
    {
        RegionType _regionType;

        public RegionTypeTest()
        {
            _regionType = null;
        }

        [TestMethod]
        public void Constructor()
        {
            RegionType region;
            Int32 id = 1;
            String name = "RegionTypeName";

            region = new RegionType(id, name, new DataContext(GetUserContext()));
            Assert.IsNotNull(region);
        }

        [TestMethod]
        public void DataContext()
        {
            Assert.IsNotNull(GetRegionType(true).DataContext);
        }

        public static RegionType GetOneRegionType(IUserContext userContext)
        {
            Int32 id = 1;
            String name = "RegionTypeName";
            return new RegionType(id, name, new DataContext(userContext));
        }

        private RegionType GetRegionType()
        {
            return GetRegionType(false);
        }

        private RegionType GetRegionType(Boolean refresh)
        {
            if (_regionType.IsNull() || refresh)
            {
                _regionType = new RegionType(1, "RegionTypeName", new DataContext(GetUserContext()));
            }
            return _regionType;
        }

        [TestMethod]
        public void RegionTypeId()
        {
            Int32 id = 1;

            GetRegionType(true).Id = id;
            Assert.IsNotNull(GetRegionType().Id);

            GetRegionType().Id = id;
            Assert.AreEqual(GetRegionType().Id, id);

        }

        [TestMethod]
        public void RegionTypeName()
        {
            String name = "RegionTypeName";
            RegionType regionType = GetOneRegionType(GetUserContext());
            Assert.AreEqual(regionType.Name, name);
        }
        
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class RegionListTest : TestBase
    {
        private RegionList _regions;

        public RegionListTest()
        {
            _regions = null;
        }

        [TestMethod]
        public void Constructor()
        {
            RegionList Regions;

            Regions = new RegionList();
            Assert.IsNotNull(Regions);
        }

        [TestMethod]
        public void Get()
        {
            GetRegions(true);
            foreach (IRegion Region in GetRegions())
            {
                Assert.AreEqual(Region, GetRegions().Get(Region.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 RegionId;

            RegionId = Int32.MaxValue;
            GetRegions(true).Get(RegionId);
        }

        private RegionList GetRegions()
        {
            return GetRegions(false);
        }

        private RegionList GetRegions(Boolean refresh)
        {
            if (_regions.IsNull() || refresh)
            {
                _regions = new RegionList();
                _regions.Add(RegionTest.GetOneRegion(GetUserContext()));
            }
            return _regions;
        }
    }
}

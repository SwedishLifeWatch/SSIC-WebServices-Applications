using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Data.Test
{
    [TestClass]
    public class WebRegionTypeTest
    {
        private WebRegionType _regionType;

        public WebRegionTypeTest()
        {
            _regionType = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebRegionType regionType;

            regionType = new WebRegionType();
            Assert.IsNotNull(regionType);
        }

        private WebRegionType GetRegionType()
        {
            return GetRegionType(false);
        }

        private WebRegionType GetRegionType(Boolean refresh)
        {
            if (_regionType.IsNull() || refresh)
            {
                _regionType = new WebRegionType();
            }
            return _regionType;
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;

            id = 752;
            GetRegionType(true).Id = id;
            Assert.AreEqual(id, GetRegionType().Id);
        }

        [TestMethod]
        public void Name()
        {
            String name;

            name = null;
            GetRegionType(true).Name = name;
            Assert.IsNull(GetRegionType().Name);

            name = String.Empty;
            GetRegionType().Name = name;
            Assert.IsTrue(GetRegionType().Name.IsEmpty());

            name = "LKdakldf-sdf";
            GetRegionType().Name = name;
            Assert.AreEqual(name, GetRegionType().Name);
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Data.Test
{
    [TestClass]
    public class WebRegionTest
    {
        private WebRegion _region;

        public WebRegionTest()
        {
            _region = null;
        }

        [TestMethod]
        public void CategoryId()
        {
            Int32 categoryId;

            categoryId = 752;
            GetRegion(true).CategoryId = categoryId;
            Assert.AreEqual(categoryId, GetRegion().CategoryId);
        }

        [TestMethod]
        public void Constructor()
        {
            WebRegion region;

            region = new WebRegion();
            Assert.IsNotNull(region);
        }

        private WebRegion GetRegion()
        {
            return GetRegion(false);
        }

        private WebRegion GetRegion(Boolean refresh)
        {
            if (_region.IsNull() || refresh)
            {
                _region = new WebRegion();
            }
            return _region;
        }

        [TestMethod]
        public void GUID()
        {
            String guid;

            guid = null;
            GetRegion(true).GUID = guid;
            Assert.IsNull(GetRegion().GUID);

            guid = String.Empty;
            GetRegion().GUID = guid;
            Assert.IsTrue(GetRegion().GUID.IsEmpty());

            guid = "LKdakldf-sdf";
            GetRegion().GUID = guid;
            Assert.AreEqual(guid, GetRegion().GUID);
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;

            id = 752;
            GetRegion(true).Id = id;
            Assert.AreEqual(id, GetRegion().Id);
        }

        [TestMethod]
        public void Name()
        {
            String name;

            name = null;
            GetRegion(true).Name = name;
            Assert.IsNull(GetRegion().Name);

            name = String.Empty;
            GetRegion().Name = name;
            Assert.IsTrue(GetRegion().Name.IsEmpty());

            name = "LKdakldf-sdf";
            GetRegion().Name = name;
            Assert.AreEqual(name, GetRegion().Name);
        }

        [TestMethod]
        public void NativeId()
        {
            String nativeId;

            nativeId = null;
            GetRegion(true).NativeId = nativeId;
            Assert.IsNull(GetRegion().NativeId);

            nativeId = String.Empty;
            GetRegion().NativeId = nativeId;
            Assert.IsTrue(GetRegion().NativeId.IsEmpty());

            nativeId = "LKdakldf-sdf";
            GetRegion().NativeId = nativeId;
            Assert.AreEqual(nativeId, GetRegion().NativeId);
        }

        [TestMethod]
        public void ShortName()
        {
            String shortName;

            shortName = null;
            GetRegion(true).ShortName = shortName;
            Assert.IsNull(GetRegion().ShortName);

            shortName = String.Empty;
            GetRegion().ShortName = shortName;
            Assert.IsTrue(GetRegion().ShortName.IsEmpty());

            shortName = "LKdakldf-sdf";
            GetRegion().ShortName = shortName;
            Assert.AreEqual(shortName, GetRegion().ShortName);
        }

        [TestMethod]
        public void SortOrder()
        {
            Int32 sortOrder;

            sortOrder = 752;
            GetRegion(true).SortOrder = sortOrder;
            Assert.AreEqual(sortOrder, GetRegion().SortOrder);
        }
    }
}

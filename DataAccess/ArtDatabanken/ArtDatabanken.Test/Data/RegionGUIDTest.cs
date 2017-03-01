using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.Test.Data
{
    [TestClass]
    public class RegionGUIDTest
    {
        [TestMethod]
        public void CategoryId()
        {
            Int32 categoryId;
            RegionGUID regionGUID;
            String nativeId, GUID;

            categoryId = 42;
            GUID = "URN:LSID:artportalen.se:area:DataSet42Feature43";
            regionGUID = new RegionGUID(GUID);
            Assert.AreEqual(categoryId, regionGUID.CategoryId);

            categoryId = 42;
            nativeId = "43";
            GUID = "URN:LSID:artportalen.se:area:DataSet42Feature43";
            regionGUID = new RegionGUID(categoryId, nativeId);
            Assert.AreEqual(categoryId, regionGUID.CategoryId);
        }

        [TestMethod]
        public void Constructor()
        {
            Int32 categoryId;
            RegionGUID regionGUID;
            String nativeId, GUID;

            GUID = "URN:LSID:artportalen.se:area:DataSet1Feature2";
            regionGUID = new RegionGUID(GUID);
            Assert.IsNotNull(regionGUID);
            Assert.AreEqual(1, regionGUID.CategoryId);
            Assert.AreEqual("2", regionGUID.NativeId);

            GUID = "URN:LSID:artportalen.se:area:DataSet42FeatureHejHopp";
            regionGUID = new RegionGUID(GUID);
            Assert.IsNotNull(regionGUID);
            Assert.AreEqual(42, regionGUID.CategoryId);
            Assert.AreEqual("HejHopp", regionGUID.NativeId);

            categoryId = 42;
            nativeId = "HejHopp";
            GUID = "URN:LSID:artportalen.se:area:DataSet42FeatureHejHopp";
            regionGUID = new RegionGUID(categoryId, nativeId);
            Assert.IsNotNull(regionGUID);
            Assert.AreEqual(GUID.ToUpper(), regionGUID.GUID.ToUpper());
            Assert.AreEqual(categoryId, regionGUID.CategoryId);
            Assert.AreEqual(nativeId, regionGUID.NativeId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorShortGUIDError()
        {
            RegionGUID regionGUID;
            String GUID;

            GUID = "URN:LSID:artportalen.se:area:DataSet1Feature";
            regionGUID = new RegionGUID(GUID);
            Assert.IsNotNull(regionGUID);
            Assert.AreEqual(1, regionGUID.CategoryId);
            Assert.AreEqual("2", regionGUID.NativeId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorCategoryStringError()
        {
            RegionGUID regionGUID;
            String GUID;

            GUID = "URN:LSID:artportalen.se:area:DataSep1Feature2";
            regionGUID = new RegionGUID(GUID);
            Assert.IsNotNull(regionGUID);
            Assert.AreEqual(1, regionGUID.CategoryId);
            Assert.AreEqual("2", regionGUID.NativeId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorCategoryIdEmptyError()
        {
            RegionGUID regionGUID;
            String GUID;

            GUID = "URN:LSID:artportalen.se:area:DataSetFeature245";
            regionGUID = new RegionGUID(GUID);
            Assert.IsNotNull(regionGUID);
            Assert.AreEqual(1, regionGUID.CategoryId);
            Assert.AreEqual("2", regionGUID.NativeId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorCategoryIdFormatError()
        {
            RegionGUID regionGUID;
            String GUID;

            GUID = "URN:LSID:artportalen.se:area:DataSetabFeature245";
            regionGUID = new RegionGUID(GUID);
            Assert.IsNotNull(regionGUID);
            Assert.AreEqual(1, regionGUID.CategoryId);
            Assert.AreEqual("2", regionGUID.NativeId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorNativeIdEmptyError()
        {
            RegionGUID regionGUID;
            String GUID;

            GUID = "URN:LSID:artportalen.se:area:DataSet234Feature";
            regionGUID = new RegionGUID(GUID);
            Assert.IsNotNull(regionGUID);
            Assert.AreEqual(1, regionGUID.CategoryId);
            Assert.AreEqual("2", regionGUID.NativeId);
        }

        [TestMethod]
        public void NativeId()
        {
            Int32 categoryId;
            RegionGUID regionGUID;
            String nativeId, GUID;

            nativeId = "43";
            GUID = "URN:LSID:artportalen.se:area:DataSet42Feature43";
            regionGUID = new RegionGUID(GUID);
            Assert.AreEqual(nativeId, regionGUID.NativeId);

            categoryId = 42;
            nativeId = "43";
            GUID = "URN:LSID:artportalen.se:area:DataSet42Feature43";
            regionGUID = new RegionGUID(categoryId, nativeId);
            Assert.AreEqual(nativeId, regionGUID.NativeId);
        }
    }
}

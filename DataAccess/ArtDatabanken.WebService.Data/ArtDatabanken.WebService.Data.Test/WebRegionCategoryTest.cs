using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Data.Test
{
    [TestClass]
    public class WebRegionCategoryTest
    {
        private WebRegionCategory _regionCategory;

        public WebRegionCategoryTest()
        {
            _regionCategory = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebRegionCategory regionCategory;

            regionCategory = new WebRegionCategory();
            Assert.IsNotNull(regionCategory);
        }

        [TestMethod]
        public void CountryIsoCode()
        {
            Int32 countryIsoCode;

            countryIsoCode = 752;
            GetRegionCategory(true).CountryIsoCode = countryIsoCode;
            Assert.AreEqual(countryIsoCode, GetRegionCategory().CountryIsoCode);
        }

        private WebRegionCategory GetRegionCategory()
        {
            return GetRegionCategory(false);
        }

        private WebRegionCategory GetRegionCategory(Boolean refresh)
        {
            if (_regionCategory.IsNull() || refresh)
            {
                _regionCategory = new WebRegionCategory();
            }
            return _regionCategory;
        }

        [TestMethod]
        public void GUID()
        {
            String guid;

            guid = null;
            GetRegionCategory(true).GUID = guid;
            Assert.IsNull(GetRegionCategory().GUID);

            guid = String.Empty;
            GetRegionCategory().GUID = guid;
            Assert.IsTrue(GetRegionCategory().GUID.IsEmpty());

            guid = "LKdakldf-sdf";
            GetRegionCategory().GUID = guid;
            Assert.AreEqual(guid, GetRegionCategory().GUID);
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;

            id = 752;
            GetRegionCategory(true).Id = id;
            Assert.AreEqual(id, GetRegionCategory().Id);
        }

        [TestMethod]
        public void IsCountryIsoCodeSpecified()
        {
            GetRegionCategory(true).IsCountryIsoCodeSpecified = true;
            Assert.IsTrue(GetRegionCategory().IsCountryIsoCodeSpecified);

            GetRegionCategory().IsCountryIsoCodeSpecified = false;
            Assert.IsFalse(GetRegionCategory().IsCountryIsoCodeSpecified);
        }

        [TestMethod]
        public void IsLevelSpecified()
        {
            GetRegionCategory(true).IsLevelSpecified = true;
            Assert.IsTrue(GetRegionCategory().IsLevelSpecified);

            GetRegionCategory().IsLevelSpecified = false;
            Assert.IsFalse(GetRegionCategory().IsLevelSpecified);
        }

        [TestMethod]
        public void Level()
        {
            Int32 level;

            level = 752;
            GetRegionCategory(true).Level = level;
            Assert.AreEqual(level, GetRegionCategory().Level);
        }

        [TestMethod]
        public void Name()
        {
            String name;

            name = null;
            GetRegionCategory(true).Name = name;
            Assert.IsNull(GetRegionCategory().Name);

            name = String.Empty;
            GetRegionCategory().Name = name;
            Assert.IsTrue(GetRegionCategory().Name.IsEmpty());

            name = "LKdakldf-sdf";
            GetRegionCategory().Name = name;
            Assert.AreEqual(name, GetRegionCategory().Name);
        }

        [TestMethod]
        public void NativeIdSource()
        {
            String nativeIdSource;

            nativeIdSource = null;
            GetRegionCategory(true).NativeIdSource = nativeIdSource;
            Assert.IsNull(GetRegionCategory().NativeIdSource);

            nativeIdSource = String.Empty;
            GetRegionCategory().NativeIdSource = nativeIdSource;
            Assert.IsTrue(GetRegionCategory().NativeIdSource.IsEmpty());

            nativeIdSource = "LKdakldf-sdf";
            GetRegionCategory().NativeIdSource = nativeIdSource;
            Assert.AreEqual(nativeIdSource, GetRegionCategory().NativeIdSource);
        }

        [TestMethod]
        public void SortOrder()
        {
            Int32 sortOrder;

            sortOrder = 752;
            GetRegionCategory(true).SortOrder = sortOrder;
            Assert.AreEqual(sortOrder, GetRegionCategory().SortOrder);
        }

        [TestMethod]
        public void TypeId()
        {
            Int32 typeId;

            typeId = 752;
            GetRegionCategory(true).TypeId = typeId;
            Assert.AreEqual(typeId, GetRegionCategory().TypeId);
        }
    }
}

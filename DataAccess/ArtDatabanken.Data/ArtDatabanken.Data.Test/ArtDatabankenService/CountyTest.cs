using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class CountyTest : TestBase
    {
        private County _county;

        public CountyTest()
        {
            _county = null;
        }

        #region Additional test attributes
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #endregion

        [TestMethod]
        public void Constructor()
        {
            County county;

            county = GetCounty(true);
            Assert.IsNotNull(county);
        }

        private County GetCounty()
        {
            return GetCounty(false);
        }

        private County GetCounty(Boolean refresh)
        {
            if (_county.IsNull() || refresh)
            {
                _county = GeographicManagerTest.GetCounty();
            }
            return _county;
        }

        [TestMethod]
        public void HasNumber()
        {
            Boolean hasNumber;

            hasNumber = GetCounty(true).HasNumber;
        }

        [TestMethod]
        public void Identifier()
        {
            Assert.IsTrue(GetCounty(true).Identifier.IsNotEmpty());
        }

        [TestMethod]
        public void IsCountyPart()
        {
            Boolean isCountyPart;

            isCountyPart = GetCounty(true).IsCountyPart;
        }

        [TestMethod]
        public void Name()
        {
            Assert.IsTrue(GetCounty(true).Name.IsNotEmpty());
        }

        [TestMethod]
        public void Number()
        {
            Int32 number;

            number = GetCounty(true).Number;
        }

        [TestMethod]
        public void PartOfCountyId()
        {
            Int32 partOfCountyId;

            partOfCountyId = GetCounty(true).PartOfCountyId;
        }
    }
}

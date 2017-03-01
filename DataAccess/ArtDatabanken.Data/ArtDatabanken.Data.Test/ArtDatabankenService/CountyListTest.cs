using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class CountyListTest : TestBase
    {
        private CountyList _counties;

        public CountyListTest()
        {
            _counties = null;
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
        public void Get()
        {
            foreach (County county in GetCounties(true))
            {
                Assert.AreEqual(county, GetCounties().Get(county.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 countyId;

            countyId = Int32.MinValue;
            GetCounties(true).Get(countyId);
        }

        private CountyList GetCounties()
        {
            return GetCounties(false);
        }

        private CountyList GetCounties(Boolean refresh)
        {
            if (_counties.IsNull() || refresh)
            {
                _counties = GeographicManagerTest.GetAllCounties();
            }
            return _counties;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            Int32 countyIndex;
            CountyList newCountyList, oldCountyList;

            oldCountyList = GetCounties(true);
            newCountyList = new CountyList();
            for (countyIndex = 0; countyIndex < oldCountyList.Count; countyIndex++)
            {
                newCountyList.Add(oldCountyList[oldCountyList.Count - countyIndex - 1]);
            }
            for (countyIndex = 0; countyIndex < oldCountyList.Count; countyIndex++)
            {
                Assert.AreEqual(newCountyList[countyIndex], oldCountyList[oldCountyList.Count - countyIndex - 1]);
            }
        }
    }
}

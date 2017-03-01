using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class ProvinceListTest : TestBase
    {
        private ProvinceList _provinces;

        public ProvinceListTest()
        {
            _provinces = null;
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

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Get()
        {
            foreach (Province province in GetProvinces(true))
            {
                Assert.AreEqual(province, GetProvinces().Get(province.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 provinceId;

            provinceId = Int32.MinValue;
            GetProvinces(true).Get(provinceId);
        }

        private ProvinceList GetProvinces()
        {
            return GetProvinces(false);
        }

        private ProvinceList GetProvinces(Boolean refresh)
        {
            if (_provinces.IsNull() || refresh)
            {
                _provinces = GeographicManagerTest.GetAllProvinces();
            }
            return _provinces;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            Int32 provinceIndex;
            ProvinceList newProvinceList, oldProvinceList;

            oldProvinceList = GetProvinces(true);
            newProvinceList = new ProvinceList();
            for (provinceIndex = 0; provinceIndex < oldProvinceList.Count; provinceIndex++)
            {
                newProvinceList.Add(oldProvinceList[oldProvinceList.Count - provinceIndex - 1]);
            }
            for (provinceIndex = 0; provinceIndex < oldProvinceList.Count; provinceIndex++)
            {
                Assert.AreEqual(newProvinceList[provinceIndex], oldProvinceList[oldProvinceList.Count - provinceIndex - 1]);
            }
        }
    }
}

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class GeographicManagerTest : TestBase
    {
        public GeographicManagerTest()
        {
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
        public void GetCitiesBySearchString()
        {
            List<WebCity> cities;
            String searchString;

            searchString = "Uppsala";
            cities = GeographicManager.GetCitiesBySearchString(GetContext(), searchString);
            Assert.IsTrue(cities.IsNotEmpty());
        }

        [TestMethod]
        public void GetCounties()
        {
            List<WebCounty> counties;

            counties = GeographicManager.GetCounties(GetContext());
            Assert.IsTrue(counties.IsNotEmpty());
        }

        public static WebCity GetOneCity(WebServiceContext context)
        {
            return GeographicManager.GetCitiesBySearchString(context, "Uppsala")[0];
        }

        public static WebCounty GetOneCounty(WebServiceContext context)
        {
            return GeographicManager.GetCounties(context)[0];
        }

        public static WebProvince GetOneProvince(WebServiceContext context)
        {
            return GeographicManager.GetProvinces(context)[0];
        }

        [TestMethod]
        public void GetProvinces()
        {
            List<WebProvince> provinces;

            provinces = GeographicManager.GetProvinces(GetContext());
            Assert.IsTrue(provinces.IsNotEmpty());
        }

        public static List<WebCounty> GetSomeCounties(WebServiceContext context)
        {
            return GeographicManager.GetCounties(context);
        }

        public static List<WebProvince> GetSomeProvinces(WebServiceContext context)
        {
            return GeographicManager.GetProvinces(context);
        }
    }
}

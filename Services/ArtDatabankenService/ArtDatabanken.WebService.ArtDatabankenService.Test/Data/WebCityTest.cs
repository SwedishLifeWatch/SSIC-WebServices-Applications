using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebCityTest : TestBase
    {
        private WebCity _city;

        public WebCityTest()
        {
            _city = null;
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
        public void CheckData()
        {
            GetCity(true).County = null;
            GetCity().CheckData(GetContext());
            GetCity().County = "";
            GetCity().CheckData(GetContext());
            GetCity().County = GetString(WebCity.GetCountyMaxLength(GetContext()));
            GetCity().CheckData(GetContext());

            GetCity().Municipality = null;
            GetCity().CheckData(GetContext());
            GetCity().Municipality = "";
            GetCity().CheckData(GetContext());
            GetCity().Municipality = GetString(WebCity.GetMunicipalityMaxLength(GetContext()));
            GetCity().CheckData(GetContext());

            GetCity().Name = null;
            GetCity().CheckData(GetContext());
            GetCity().Name = "";
            GetCity().CheckData(GetContext());
            GetCity().Name = GetString(WebCity.GetNameMaxLength(GetContext()));
            GetCity().CheckData(GetContext());

            GetCity().Parish = null;
            GetCity().CheckData(GetContext());
            GetCity().Parish = "";
            GetCity().CheckData(GetContext());
            GetCity().Parish = GetString(WebCity.GetParishMaxLength(GetContext()));
            GetCity().CheckData(GetContext());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckDataCountyToLongError()
        {
            GetCity(true).County = GetString(WebCity.GetCountyMaxLength(GetContext()) + 1);
            GetCity().CheckData(GetContext());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckDataMunicipalityToLongError()
        {
            GetCity(true).Municipality = GetString(WebCity.GetMunicipalityMaxLength(GetContext()) + 1);
            GetCity().CheckData(GetContext());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckDataNameToLongError()
        {
            GetCity(true).Name = GetString(WebCity.GetNameMaxLength(GetContext()) + 1);
            GetCity().CheckData(GetContext());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckDataParishToLongError()
        {
            GetCity(true).Parish = GetString(WebCity.GetParishMaxLength(GetContext()) + 1);
            GetCity().CheckData(GetContext());
        }

        [TestMethod]
        public void Constructor()
        {
            WebCity city;

            city = GetCity(true);
            Assert.IsNotNull(city);
        }

        [TestMethod]
        public void County()
        {
            String county;

            county = null;
            GetCity(true).County = county;
            Assert.IsNull(GetCity().County);
            county = "";
            GetCity().County = county;
            Assert.AreEqual(GetCity().County, county);
            county = "Test county";
            GetCity().County = county;
            Assert.AreEqual(GetCity().County, county);
        }

        private WebCity GetCity()
        {
            return GetCity(false);
        }

        private WebCity GetCity(Boolean refresh)
        {
            if (_city.IsNull() || refresh)
            {
                _city = GeographicManagerTest.GetOneCity(GetContext());
            }
            return _city;
        }

        [TestMethod]
        public void GetCountyMaxLength()
        {
            Int32 maxLength;

            maxLength = WebCity.GetCountyMaxLength(GetContext());
            Assert.IsTrue(0 < maxLength);
        }

        [TestMethod]
        public void GetMunicipalityMaxLength()
        {
            Int32 maxLength;

            maxLength = WebCity.GetMunicipalityMaxLength(GetContext());
            Assert.IsTrue(0 < maxLength);
        }

        [TestMethod]
        public void GetNameMaxLength()
        {
            Int32 maxLength;

            maxLength = WebCity.GetNameMaxLength(GetContext());
            Assert.IsTrue(0 < maxLength);
        }

        [TestMethod]
        public void GetParishMaxLength()
        {
            Int32 maxLength;

            maxLength = WebCity.GetParishMaxLength(GetContext());
            Assert.IsTrue(0 < maxLength);
        }

        [TestMethod]
        public void Municipality()
        {
            String municipality;

            municipality = null;
            GetCity(true).Municipality = municipality;
            Assert.IsNull(GetCity().Municipality);
            municipality = "";
            GetCity().Municipality = municipality;
            Assert.AreEqual(GetCity().Municipality, municipality);
            municipality = "Test municipality";
            GetCity().Municipality = municipality;
            Assert.AreEqual(GetCity().Municipality, municipality);
        }

        [TestMethod]
        public void Name()
        {
            String name;

            name = null;
            GetCity(true).Name = name;
            Assert.IsNull(GetCity().Name);
            name = "";
            GetCity().Name = name;
            Assert.AreEqual(GetCity().Name, name);
            name = "Test name";
            GetCity().Name = name;
            Assert.AreEqual(GetCity().Name, name);
        }

        [TestMethod]
        public void Parish()
        {
            String parish;

            parish = null;
            GetCity(true).Parish = parish;
            Assert.IsNull(GetCity().Parish);
            parish = "";
            GetCity().Parish = parish;
            Assert.AreEqual(GetCity().Parish, parish);
            parish = "Test parish";
            GetCity().Parish = parish;
            Assert.AreEqual(GetCity().Parish, parish);
        }

        [TestMethod]
        public void XCoordinate()
        {
            Int32 xCoordinate;

            xCoordinate = 0;
            GetCity(true).XCoordinate = xCoordinate;
            Assert.AreEqual(xCoordinate, GetCity().XCoordinate);
            xCoordinate = 42;
            GetCity().XCoordinate = xCoordinate;
            Assert.AreEqual(xCoordinate, GetCity().XCoordinate);
        }

        [TestMethod]
        public void YCoordinate()
        {
            Int32 yCoordinate;

            yCoordinate = 0;
            GetCity(true).YCoordinate = yCoordinate;
            Assert.AreEqual(yCoordinate, GetCity().YCoordinate);
            yCoordinate = 42;
            GetCity().YCoordinate = yCoordinate;
            Assert.AreEqual(yCoordinate, GetCity().YCoordinate);
        }
    }
}

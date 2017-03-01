using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebBirdNestActivityTest : TestBase
    {
        private WebBirdNestActivity _birdNestActivity;

        public WebBirdNestActivityTest()
        {
            _birdNestActivity = null;
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
            GetBirdNestActivity(true).Name = null;
            GetBirdNestActivity().CheckData(GetContext());
            GetBirdNestActivity().Name = "";
            GetBirdNestActivity().CheckData(GetContext());
            GetBirdNestActivity().Name = GetString(WebBirdNestActivity.GetNameMaxLength(GetContext()));
            GetBirdNestActivity().CheckData(GetContext());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckDataNameToLongError()
        {
            GetBirdNestActivity(true).Name = GetString(WebBirdNestActivity.GetNameMaxLength(GetContext()) + 1);
            GetBirdNestActivity().CheckData(GetContext());
        }

        [TestMethod]
        public void Constructor()
        {
            WebBirdNestActivity birdNestActivity;

            birdNestActivity = GetBirdNestActivity(true);
            Assert.IsNotNull(birdNestActivity);
        }

        private WebBirdNestActivity GetBirdNestActivity()
        {
            return GetBirdNestActivity(false);
        }

        private WebBirdNestActivity GetBirdNestActivity(Boolean refresh)
        {
            if (_birdNestActivity.IsNull() || refresh)
            {
                _birdNestActivity = SpeciesObservationManagerTest.GetOneBirdNestActivity(GetContext());
            }
            return _birdNestActivity;
        }

        [TestMethod]
        public void GetNameMaxLength()
        {
            Int32 maxLength;

            maxLength = WebBirdNestActivity.GetNameMaxLength(GetContext());
            Assert.IsTrue(0 < maxLength);
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;

            id = 0;
            GetBirdNestActivity(true).Id = id;
            Assert.AreEqual(id, GetBirdNestActivity().Id);
            id = 42;
            GetBirdNestActivity().Id = id;
            Assert.AreEqual(id, GetBirdNestActivity().Id);
        }

        [TestMethod]
        public void Name()
        {
            String name;

            name = null;
            GetBirdNestActivity(true).Name = name;
            Assert.IsNull(GetBirdNestActivity().Name);
            name = "";
            GetBirdNestActivity().Name = name;
            Assert.AreEqual(GetBirdNestActivity().Name, name);
            name = "Test name";
            GetBirdNestActivity().Name = name;
            Assert.AreEqual(GetBirdNestActivity().Name, name);
        }
    }
}

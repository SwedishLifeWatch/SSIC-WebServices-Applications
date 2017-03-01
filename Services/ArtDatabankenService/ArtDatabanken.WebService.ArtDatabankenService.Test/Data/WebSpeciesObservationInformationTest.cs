using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebSpeciesObservationInformationTest : TestBase
    {
        private WebSpeciesObservationInformation _speciesObservationInformation;

        public WebSpeciesObservationInformationTest()
        {
            ApplicationIdentifier = ArtDatabanken.WebService.ArtDatabankenService.Data.ApplicationIdentifier.PrintObs.ToString();
            _speciesObservationInformation = null;
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
        public void Constructor()
        {
            WebSpeciesObservationInformation speciesObservationInformation;

            speciesObservationInformation = GetSpeciesObservationInformation(true);
            Assert.IsNotNull(speciesObservationInformation);
        }

        private WebSpeciesObservationInformation GetSpeciesObservationInformation()
        {
            return GetSpeciesObservationInformation(false);
        }

        private WebSpeciesObservationInformation GetSpeciesObservationInformation(Boolean refresh)
        {
            if (_speciesObservationInformation.IsNull() || refresh)
            {
                _speciesObservationInformation = SpeciesObservationManagerTest.GetOneSpeciesObservationInformation(GetContext());
            }
            return _speciesObservationInformation;
        }


        [TestMethod]
        public void SpeciesObservations()
        {
            List<WebSpeciesObservation> speciesObservations;

            speciesObservations = null;
            GetSpeciesObservationInformation(true).SpeciesObservations = speciesObservations;
            Assert.IsNull(GetSpeciesObservationInformation().SpeciesObservations);

            speciesObservations = new List<WebSpeciesObservation>();
            GetSpeciesObservationInformation().SpeciesObservations = speciesObservations;
            Assert.IsNotNull(GetSpeciesObservationInformation().SpeciesObservations);
            Assert.IsTrue(GetSpeciesObservationInformation().SpeciesObservations.IsEmpty());

            speciesObservations = SpeciesObservationManagerTest.GetSomeSpeciesObservations(GetContext());
            GetSpeciesObservationInformation().SpeciesObservations = speciesObservations;
            Assert.IsTrue(GetSpeciesObservationInformation().SpeciesObservations.IsNotEmpty());
            Assert.AreEqual(speciesObservations.Count, GetSpeciesObservationInformation().SpeciesObservations.Count);
        }
    }
}

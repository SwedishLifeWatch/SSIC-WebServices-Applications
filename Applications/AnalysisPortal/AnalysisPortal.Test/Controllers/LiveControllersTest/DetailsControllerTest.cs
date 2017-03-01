using System;
using AnalysisPortal.Controllers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Details;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable once CheckNamespace
namespace AnalysisPortal.Tests
{
    using Microsoft.QualityTools.Testing.Fakes;

    /// <summary>
    /// This is a test class for DetailsControllerTest and is intended
    /// to contain all DetailsControllerTest Unit Tests.
    /// </summary>
    [TestClass]
    public class DetailsControllerTest : DBTestControllerBaseTest
    {
        /// <summary>
        /// Holding test context.
        /// </summary>
        private TestContext testContextInstance;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
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

        #region Additional test attributes
    
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext)
        // {
        // }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup()
        // {
        // }
        //
        // Use TestInitialize to run code before running each test
        // [TestInitialize()]
        // public void MyTestInitialize()
        // {
        // }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup()
        // {
        // }
        #endregion

        /// <summary>
        /// A test for Detail, using observation id which might change from day to day.
        /// In the future we should us A GUID to get the correct observation.
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void DetailGetTest()
        {
            // Arrange
            const string ObservationGuid = "271542";

            // observationGUID = "urn:lsid:artportalen.se:Sighting:271542";
            DetailsController detailsController = new DetailsController();

            // Act
            // Returning "hard coded" value för first observation for luktsmåborre
            var result = detailsController.ObservationDetailPartial(ObservationGuid, false);

            // Assert
            Assert.IsNotNull(result);
            var detailsViewModel = result.ViewData.Model as ObservationDetailViewModel;

            Assert.IsNotNull(detailsViewModel);            

            // TODO disable theese tests since data dont exit all the time...Check number of properties
            // Assert.IsTrue(detailsViewModel.Fields.Count > 0);
        }

        /// <summary>
        /// A test for getting detailed view with invalid data set, using observation id which might change from day to day.
        /// In the future we should us A GUID to get the correct observation.
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void DetailInvalidObservationGetTest()
        {
            // Arrange
            const string ObservationGuid = "ogiltigt id";
            DetailsController detailsController = new DetailsController();
            
            // Act
             var result = detailsController.ObservationDetailPartial(ObservationGuid, false);

            // Assert
            Assert.IsNotNull(result);
            var detailsViewModel = result.ViewData.Model as ObservationDetailViewModel;

             Assert.IsNotNull(detailsViewModel);
   
            // Check number of properties
            Assert.IsNull(detailsViewModel.Fields);
        }

        /// <summary>
        /// Testing exception is throw from detailed view, using observation id which might change from day to day.
        /// In the future we should us A GUID to get the correct observation.
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(ApplicationException))]
        public void GetDetailsUnexpectedErrorTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                MakeGetCurrentUserFunctionCallThrowException();

                const string ObservationGuid = "22462731";
                DetailsController detailsController = new DetailsController();
                
                // Act
                detailsController.ObservationDetailPartial(ObservationGuid, false);

                // Assert
                Assert.Fail("No application exception was thrown");
            }
        }
    }
}

using AnalysisPortal.Controllers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Error;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Web.Mvc;

namespace AnalysisPortal.Tests
{
    using Microsoft.QualityTools.Testing.Fakes;

    /// <summary>
    ///This is a test class for ErrorsControllerTest and is intended
    ///to contain all ErrorsControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ErrorsControllerTest : DBTestControllerBaseTest
    {


        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for ErrorsController Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void ErrorsControllerConstructorTest()
        {
            ErrorsController target = new ErrorsController();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for General
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void GeneralTest()
        {
            using (ShimsContext.Create())
            {
                ErrorsController controller = new ErrorsController();
                string errorMsg = "TestException";
                string controllerName = "Error";
                string actionName = "General";
                Exception ex = new Exception(errorMsg);

                controller.ControllerContext = GetErrorControllerContext(actionName, controllerName);

                ViewResult result = controller.General(ex) as ViewResult;
                ErrorViewModel viewModel = result.ViewData.Model as ErrorViewModel;

                Assert.IsNotNull(result);
                Assert.IsTrue(result.ViewName.Equals("ErrorInfo"));
                Assert.IsNotNull(viewModel);

                Assert.IsTrue(viewModel.ErrorInformationText.Equals(errorMsg));
                Assert.IsTrue(viewModel.ErrorAction.Equals(actionName));
                Assert.IsTrue(viewModel.ErrorController.Equals(controllerName));
            }
            
        }

        /// <summary>
        ///A test for Http403
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
       public void Http403Test()
        {
            ErrorsController controller = new ErrorsController(); 
            ViewResult result = controller.Http403() as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewName.Equals("403"));
        }

        /// <summary>
        ///A test for Http404
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void Http404Test()
        {
            ErrorsController controller = new ErrorsController();
            ViewResult result = controller.Http404() as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewName.Equals("404"));
        }

        /// <summary>
        ///A test for Http400
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void Http400Test()
        {
            ErrorsController controller = new ErrorsController();
            ViewResult result = controller.Http400() as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewName.Equals("400"));
        }

        /// <summary>
        ///A test for Http404
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void Http500Test()
        {
            ErrorsController controller = new ErrorsController();
            ViewResult result = controller.Http500() as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewName.Equals("500"));
        }
    }
}

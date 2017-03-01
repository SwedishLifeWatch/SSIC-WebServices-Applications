using AnalysisPortal.Controllers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Web.Mvc;

namespace AnalysisPortal.Tests
{
    
    
    /// <summary>
    ///This is a test class for DebugControllerTest and is intended
    ///to contain all DebugControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DebugControllerTest : DBTestControllerBaseTest
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
        ///A test for DebugController Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void DebugControllerConstructorTest()
        {
            DebugController target = new DebugController();
            Assert.IsNotNull(target);
        }

 #if DEBUG      

        /// <summary>
        ///A test for SessionVariables
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void SessionVariablesTest()
        {
            DebugController controller = new DebugController(); 
            PartialViewResult result;
            result = controller.SessionVariables();
            DebugSessionVariablesViewModel model = result.ViewData.Model as DebugSessionVariablesViewModel;
            //Assert.AreEqual(expected, actual);
            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
        }
#endif
    }
}

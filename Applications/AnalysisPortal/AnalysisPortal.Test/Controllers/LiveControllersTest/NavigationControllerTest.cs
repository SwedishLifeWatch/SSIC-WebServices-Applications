using AnalysisPortal.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;

namespace AnalysisPortal.Tests
{
    
    
    /// <summary>
    ///This is a test class for NavigationControllerTest and is intended
    ///to contain all NavigationControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class NavigationControllerTest : DBTestControllerBaseTest
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
        ///A test for NavigationController Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void NavigationControllerConstructorTest()
        {
            NavigationController target = new NavigationController();
            Assert.IsNotNull(target);
        }
    }
}

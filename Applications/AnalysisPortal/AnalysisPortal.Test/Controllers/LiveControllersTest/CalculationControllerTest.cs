using AnalysisPortal.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;

namespace AnalysisPortal.Tests
{
    
    
    /// <summary>
    ///This is a test class for CalculationControllerTest and is intended
    ///to contain all CalculationControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CalculationControllerTest:DBTestControllerBaseTest
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
      
        #endregion


        /// <summary>
        ///A test for CalculationController Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void CalculationControllerConstructorTest()
        {
            CalculationController target = new CalculationController();
            Assert.IsNotNull(target);
        }
    }
}

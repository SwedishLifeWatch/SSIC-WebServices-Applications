using AnalysisPortal.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;

namespace AnalysisPortal.Tests
{
    
    
    /// <summary>
    ///This is a test class for DataSourcesControllerTest and is intended
    ///to contain all DataSourcesControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DataSourcesControllerTest:DBTestControllerBaseTest
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
        ///A test for DataProvidersController Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void DataSourcesControllerConstructorTest()
        {
            DataController target = new DataController();
            Assert.IsNotNull(target);
        }
    }
}

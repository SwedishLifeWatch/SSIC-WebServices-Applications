using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;


namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    /// <summary>
    /// Summary description for WebUserParameterSelection
    /// </summary>
    [TestClass]
    public class WebUserParameterSelectionTest : TestBase
    {
       private WebUserParameterSelection _userParameterSelection;

        public WebUserParameterSelectionTest()
        {
            _userParameterSelection = null;
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
            GetUserParameterSelection(true).TaxonIds = TaxonManagerTest.GetSomeTaxonIds();
            GetUserParameterSelection().CheckData();
        }

        private WebUserParameterSelection GetUserParameterSelection()
        {
            return GetUserParameterSelection(false);
        }

        private WebUserParameterSelection GetUserParameterSelection(Boolean refresh)
        {
            if (_userParameterSelection.IsNull() || refresh)
            {
                _userParameterSelection = new WebUserParameterSelection();
            }
            return _userParameterSelection;
        }

  
    }
}

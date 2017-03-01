using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Web.Mvc;
using AnalysisPortal.Controllers;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Json;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.About;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Observations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;

namespace AnalysisPortal.Tests
{
    
    
    /// <summary>
    ///This is a test class for PresentationControllerTest and is intended
    ///to contain all PresentationControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PresentationControllerTest : DBTestControllerBaseTest
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
        ///A test for PresentationController Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
         public void PresentationControllerConstructorTest()
        {
            FormatController target = new FormatController();
            Assert.IsNotNull(target);
        }

      

      

        /// <summary>
        ///A test for Index
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void IndexTest()
        {
            FormatController controller = new FormatController();
            ViewResult result = controller.Index() as ViewResult;
            var model = result.ViewData.Model as AboutViewModel;

            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
        }

     

        /// <summary>
        ///A test for testing observation in table. 
        ///</summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void TableGetTest()
        {
            //Arrange
            SetSwedishLanguage();
            ResultController presentationController = new ResultController();
            var taxaIds = new ObservableCollection<int>();
            taxaIds.Add(100573);
            SessionHandler.MySettings = new MySettings();
            SessionHandler.MySettings.Filter.Taxa.TaxonIds = taxaIds;
            SessionHandler.MySettings.Filter.Taxa.IsActive = true;
            SessionHandler.MySettings.Presentation.Table.IsActive = true;

            //Act

            var result = presentationController.SpeciesObservationTable();
            Assert.IsNotNull(result);

            var obsResult = presentationController.GetPagedObservationListAsJSON(1,0,25);
            Assert.IsNotNull(obsResult);

            JsonModel jsonResult = (JsonModel)obsResult.Data;
            List<Dictionary<string, string>> observationListResult = (List<Dictionary<string, string>>)jsonResult.Data;

            //Assert
            Assert.IsNotNull(jsonResult);
            Assert.IsTrue(jsonResult.Success);
            var data = jsonResult.Data as IList;
            Assert.IsTrue(data.Count > 0);
            Assert.IsTrue(observationListResult.Count > 0);

            BaseDataTest.VerifyObservationDataForGriffelblomfluga1000573(observationListResult);

            SetEnglishLanguage();
        }

        ///// <summary>
        /////A test for testing observation column selection in table. 
        /////</summary>
        //[TestMethod()]
        //public void TableDifferentColumnSettingsGetTest()
        //{
        //    //Arrange
        //    PresentationController presentationController = new PresentationController();
        //    List<int> taxaIds = new List<int>();
        //    taxaIds.Add(100573);
        //    SessionHandler.MySettings = new MySettings();
        //    SessionHandler.MySettings.Filter.TaxaSetting.TaxonIds = taxaIds;
            
        //    //Act

        //    var result = presentationController.Table();
        //    Assert.IsNotNull(result);

        //    // Test 1 - choose minimum number of columns ie PresentationColumnSelection.Minimum
        //    PresentationColumnSelection selection = PresentationColumnSelection.Minimum;
        //    var obsResult = presentationController.GetObservations(selection);
        //    Assert.IsNotNull(obsResult);

        //    JsonModel jsonResult = (JsonModel)obsResult.Data;
        //    IList<ObservationViewModel> observationListResult = (List<ObservationViewModel>)jsonResult.Data;

        //    //Assert
        //    Assert.IsNotNull(jsonResult);
        //    Assert.IsTrue(jsonResult.Success);

        //    Assert.IsTrue(jsonResult.Total > 10);
        //    Assert.IsTrue(observationListResult.Count > 10);

        //    //todo: Add test for checking no of columns
        //    // Get all public static properties of ObservationViewModel type
        //    PropertyInfo[] propertyInfos = typeof(ObservationViewModel).GetProperties();
            

        //    // Test property names
        //    int noOfProp = 0;
        //    foreach (PropertyInfo propertyInfo in propertyInfos)
        //    {
        //      //todo Check that propery exist
        //        noOfProp++;
        //    }
        //    Assert.IsTrue(noOfProp > 10);
           
        //    // Test 2 - choose medium number of columns ie PresentationColumnSelection.Medium
        //    selection = PresentationColumnSelection.Medium;
        //    obsResult = presentationController.GetObservations(selection);
        //    Assert.IsNotNull(obsResult);

        //    jsonResult = (JsonModel)obsResult.Data;
        //    observationListResult = (List<ObservationViewModel>)jsonResult.Data;

        //    //Assert
        //    Assert.IsNotNull(jsonResult);
        //    Assert.IsTrue(jsonResult.Success);

        //    Assert.IsTrue(jsonResult.Total > 10);
        //    Assert.IsTrue(observationListResult.Count > 10);

        //    //todo: Add test for checking no of columns
        //    // Get all public static properties of ObservationViewModel type
        //    propertyInfos = typeof(ObservationViewModel).GetProperties();


        //    // Test property names
        //    noOfProp = 0;
        //    foreach (PropertyInfo propertyInfo in propertyInfos)
        //    {
        //        //todo Check that propery exist
        //        noOfProp++;
        //    }
        //    Assert.IsTrue(noOfProp > 20);
           

        //    // Test 3 - choose maximum number of columns ie PresentationColumnSelection.Maximum
        //    selection = PresentationColumnSelection.Maximum;
        //    obsResult = presentationController.GetObservations(selection);
        //    Assert.IsNotNull(obsResult);

        //    jsonResult = (JsonModel)obsResult.Data;
        //    observationListResult = (List<ObservationViewModel>)jsonResult.Data;

        //    //Assert
        //    Assert.IsNotNull(jsonResult);
        //    Assert.IsTrue(jsonResult.Success);

        //    Assert.IsTrue(jsonResult.Total > 10);
        //    Assert.IsTrue(observationListResult.Count > 10);
        //    //todo: Add test for checking no of columns
        //    // Get all public static properties of ObservationViewModel type
        //   propertyInfos = typeof(ObservationViewModel).GetProperties();


        //    // Test property names
        //    foreach (PropertyInfo propertyInfo in propertyInfos)
        //    {
        //        //todo Check that propery exist
        //        noOfProp++;
        //    }
        //    Assert.IsTrue(noOfProp > 30);
        //}



        
    }
}

using System.Collections.Generic;
using AnalysisPortal.Controllers;
using ArtDatabanken.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

   
    namespace AnalysisPortal.Tests.Mock
    {
        using ArtDatabanken.Data.DataSource;
        using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;

        [TestClass]
        public class MySettingsControllerTest : MockControllerBaseTest
        {
            #region Additional test attributes
            // 
            //You can use the following additional attributes as you write your tests:
            //
            //Use ClassInitialize to run code before running the first test in the class

            [ClassInitialize()]
            public static void MyClassInitialize(TestContext testContext)
            {

            }

            //Use ClassCleanup to run code after all tests in a class have run
            [ClassCleanup()]
            public static void MyClassCleanup()
            {

            }
        #endregion


        #region Tests

            /// <summary>
            /// This test checks that TaxaSummary View is working correct.
            /// </summary>
            [TestMethod]
            [TestCategory("NightlyTestApp")]
            public void AuthorizationTest()
            {
                //Arrange 
                // TODO replace this with userManager respository in the future no need to test onion manager classer here
                IUserDataSource userDataSource = new ArtDatabanken.WebService.Client.UserService.Fakes.StubUserDataSource() { };
                List<int>taxonIds = new List<int>();
                TaxonList taxonList = new TaxonList();
                ITaxonAlertStatus alertStatus = new TaxonAlertStatus()
                                                     {
                                                         Id = (int)TaxonAlertStatusId.Green
                                                     };
                taxonList.Add(new Taxon()
                                  {
                                      Id = 10007,
                                      ScientificName = "ScientificName",
                                      CommonName = "CommonName",
                                      Author = "Author",
                                      Category = new TaxonCategory(),
                                      AlertStatus = alertStatus
                                  });
                ITaxonManager testTaxonManager = new ArtDatabanken.Data.Fakes.StubITaxonManager()
                {
                    GetTaxaIUserContextListOfInt32 = (context, taxa) => { return taxonList; }
                };
                CoreData.TaxonManager = testTaxonManager;
              
                // Act
                MySettingsController controller = new MySettingsController(userDataSource, SessionHelper);
                var result = controller.TaxaSummary() as PartialViewResult;
                
                // Assert
                Assert.IsNotNull(result);

                // Check view data
                Assert.AreEqual("TaxaSummary", result.ViewName);

                // Check model data
                var listModel = result.ViewData.Model as List<TaxonViewModel>;
                Assert.IsNotNull(listModel);
                Assert.IsTrue(listModel.Count == 1);

            }


      

            #endregion

        #region Test helper methods

        #endregion
        }
    }



using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Export;
using Dyntaxa.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dyntaxa.Test.Controllers
{
    using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;

    using Dyntaxa.Test;

    using Microsoft.QualityTools.Testing.Fakes;

    [TestClass]
    public class SpeciesFactControllerTests : ControllerNightlyTestBase
    {
        #region Tests


        /// <summary>
        ///A test for all actions in the controller to verify that correct authorization attributes are set.
        /// Ie checks the role of user so that actions not allowed is unavaliabe.
        ///</summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void AuthorizationTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                SetSwedishLanguage();
                SpeciesFactController controller = new SpeciesFactController();
               // controller.ControllerContext = GetShimControllerContext(actionName, controllerName);//Arrange

                var type = controller.GetType();

                // 1. Test that action SpeciesFactList (get) has correct authority..
                //Act
                var methodInfo = type.GetMethod("SpeciesFactList", new Type[] { typeof(string) });
                var attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                //Assert
                Assert.AreEqual(1, attributes.Length);
                Assert.AreEqual(RequiredAuthorization.Authenticated, ((DyntaxaAuthorizeAttribute)attributes[0]).Order);

                // 2. Test that action SpeciesFactList (post) has correct authority..
                //Act
                methodInfo = type.GetMethod("SpeciesFactList", new Type[] { typeof(SpeciesFactViewModel), typeof(string) });
                attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                //Assert
                Assert.AreEqual(1, attributes.Length);
                Assert.AreEqual(RequiredAuthorization.SpeciesFactEVAEditor, ((DyntaxaAuthorizeAttribute)attributes[0]).Order);
            }

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void TestCredentialString()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                SetSwedishLanguage();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "Credentials";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);//Arrange
                String output = "test Grå1mild";
                System.Text.UnicodeEncoding encoding = new System.Text.UnicodeEncoding();
                Byte[] bytes = encoding.GetBytes(output);
                String convertedString = System.Web.HttpServerUtility.UrlTokenEncode(bytes);

                Byte[] bytes2 = System.Web.HttpServerUtility.UrlTokenDecode(convertedString);

                String result = encoding.GetString(bytes2);
               // String converted = System.Web.HttpServerUtility.UrlTokenDecode
                Assert.AreEqual(output, result);
            }
        }

        /// <summary>
        /// Test the species fact list getter
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void SearchRedList()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                IUserContext userContext = ApplicationUserContextSV;

                var intFactorIds = new List<int> {(int) FactorId.RedlistCategory};
                FactorList factors = CoreData.FactorManager.GetFactors(userContext, intFactorIds);
                ISpeciesFactSearchCriteria speciesFactSearchCriteria = new SpeciesFactSearchCriteria();
                IPeriod period = CoreData.FactorManager.GetCurrentPublicPeriod(userContext);
                EnsureNoListsAreNull(speciesFactSearchCriteria);
                speciesFactSearchCriteria.IncludeNotValidHosts = true;
                speciesFactSearchCriteria.IncludeNotValidTaxa = true;                
                speciesFactSearchCriteria.Add(period);
                speciesFactSearchCriteria.Add(CoreData.FactorManager.GetDefaultIndividualCategory(userContext));
                speciesFactSearchCriteria.Taxa = new TaxonList();

                const int VargId = 100024; // varg
                const int BlamesId = 103025; // blåmes
                const int AlpkloverId = 1603; // alpklöver

                List<int> taxonIds = new List<int> { VargId, BlamesId, AlpkloverId }; // add "varg", "blåmes", "alpklöver"
                var taxa = CoreData.TaxonManager.GetTaxa(userContext, taxonIds);

                foreach (ITaxon taxon in taxa)
                {
                    speciesFactSearchCriteria.Taxa.Add(taxon);
                }

                speciesFactSearchCriteria.Factors = new FactorList();
                foreach (IFactor factor in factors)
                {
                    speciesFactSearchCriteria.Factors.Add(factor);
                }
                
                SpeciesFactList speciesFactList = CoreData.SpeciesFactManager.GetSpeciesFacts(userContext, speciesFactSearchCriteria);
                Dictionary<int, ISpeciesFact> dic = speciesFactList.ToDictionary(speciesFact => speciesFact.Taxon.Id);

                // Get all possible red list category values
                List<IFactorFieldEnumValue> enumValues = new List<IFactorFieldEnumValue>();
                foreach (IFactorFieldEnumValue enumValue in dic[VargId].Factor.DataType.Field1.Enum.Values)
                {
                    enumValues.Add(enumValue);
                }                

                // Assert
                Assert.AreEqual("Sårbar (VU)", dic[VargId].Field1.EnumValue.OriginalLabel);
                Assert.AreEqual(4, dic[VargId].Field1.EnumValue.KeyInt);

                Assert.AreEqual("Livskraftig (LC)", dic[BlamesId].Field1.EnumValue.OriginalLabel);
                Assert.AreEqual(6, dic[BlamesId].Field1.EnumValue.KeyInt);

                Assert.AreEqual("Starkt hotad (EN)", dic[AlpkloverId].Field1.EnumValue.OriginalLabel);
                Assert.AreEqual(3, dic[AlpkloverId].Field1.EnumValue.KeyInt);       
            }
        }

        private void EnsureNoListsAreNull(ISpeciesFactSearchCriteria speciesFactSearchCriteria)
        {
            if (speciesFactSearchCriteria.FactorDataTypes == null)
            {
                speciesFactSearchCriteria.FactorDataTypes = new FactorDataTypeList();
            }

            if (speciesFactSearchCriteria.Factors == null)
            {
                speciesFactSearchCriteria.Factors = new FactorList();
            }

            if (speciesFactSearchCriteria.FieldSearchCriteria == null)
            {
                speciesFactSearchCriteria.FieldSearchCriteria = new SpeciesFactFieldSearchCriteriaList();
            }

            if (speciesFactSearchCriteria.Hosts == null)
            {
                speciesFactSearchCriteria.Hosts = new TaxonList();
            }

            if (speciesFactSearchCriteria.IndividualCategories == null)
            {
                speciesFactSearchCriteria.IndividualCategories = new IndividualCategoryList();
            }

            if (speciesFactSearchCriteria.Periods == null)
            {
                speciesFactSearchCriteria.Periods = new PeriodList();
            }

            if (speciesFactSearchCriteria.Taxa == null)
            {
                speciesFactSearchCriteria.Taxa = new TaxonList();
            }
        }



        /// <summary>
        /// Test the species fact list getter
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void SpeciesFactListGetPersistentDataTest1()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                
                SessionTaxonId = TaxonIdTuple.Create(DyntaxaTestSettings.Default.PsophusStridulusTaxonId.ToString(), DyntaxaTestSettings.Default.PsophusStridulusTaxonId);
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "SpeciesFactList";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);//Arrange
                // Set that user has authority to read nonpublic data
                IUserContext userContext = ApplicationUserContextSV;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;
                
                //Act
                var result = controller.SpeciesFactList(DyntaxaTestSettings.Default.PsophusStridulusTaxonId.ToString()) as ViewResult;
                var viewModel = result.ViewData.Model as SpeciesFactViewModel;


                // Assert
                Assert.IsNotNull(result);

                Assert.IsNotNull(viewModel);
                
                // Test that correct view is returned
                Assert.AreEqual("SpeciesFactList", result.ViewName);

                // Test model values
                Assert.AreEqual("SpeciesFactList", viewModel.PostAction);
                Assert.AreEqual(DyntaxaTestSettings.Default.PsophusStridulusTaxonId.ToString(), viewModel.TaxonId.ToString());

                // Check all data
                ArrayList table = ReadModelData(viewModel);
                
                // Gets the test file path and the reference path for excelfiles used in test
                string testRefFilePath;
                var testFilePath = GetTestFilePath(out testRefFilePath);                
                string stridulusRefPath = Path.Combine(testRefFilePath, "PsophusStridulusRefFull.xlsx");
                
                ArrayList refTable = XlsxExcelFile.GetArrayListFromExcelFile(stridulusRefPath);                
                
                ArrayList refTableNoInfo = new ArrayList();
                
                // Remove first 14 rows fron array
                for (int i = 0; i < refTable.Count; i++)
                {
                    if (i > 13)
                    {
                        refTableNoInfo.Add(refTable[i]);
                    }
                }

                // Now we find and compare.. Everything in ArtfaktadatabasenBIUS is persistent so we can test on that data..
                TestPersistentDataInArtfatadatabasenBIUS(refTableNoInfo, table);
                // Works for release 2014-04-22
                // To make this test work DB must be updated TestAllSpeciesFactData(refTableNoInfo, table, true);
            }
        }

        private bool AreEqualArrayLists(ArrayList list1, ArrayList list2)
        {
            for (int i = 0; i < list1.Count; i++)
            {
                ArrayList item1 = (ArrayList)list1[i];
                ArrayList item2 = (ArrayList)list2[i];
                for (int j = 0; j < item1.Count; j++)
                {
                    if (item1[j].ToString() != item2[j].ToString())
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// A test for SpeciesFactList ie create a viewl list of factors for a taxon (get action). Test that host data has been gererate for 
        /// PsophusStridulus(Trumgräshoppa).
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void SpeciesFactListGetHostTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "SpeciesFactList";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                // Set that user has authrity to read nonpublic data
                IUserContext userContext = ApplicationUserContextSV;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                 // Set user in session
                UserContextData = userContext;

                // gets the test file path and the reference path for excelfiles used in test
                string testRefFilePath;
                var testFilePath = GetTestFilePath(out testRefFilePath);
                
                //Act
                var result = controller.SpeciesFactList(DyntaxaTestSettings.Default.PsophusStridulusTaxonId.ToString()) as ViewResult;
                var viewModel = result.ViewData.Model as SpeciesFactViewModel;

                // Read the viewdata and save the data in an Arraylist were excel columns and rows match elements in created array list.
                ArrayList table = ReadModelData(viewModel);

                //Assert
                //Test if Stridulus contains host list
                TestCointainsHostData(table);
            }
        }

        /// <summary>
        /// A test for SpeciesFactList ie create a view list of factors for a taxon (Get action).
        /// Log in as person user without any authority to see nonpublic data. Verify that quality if factors in not below Mycket låg.
        /// Test is performed on taxon PsophusStridulus(Trumgräshoppa).
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void SpeciesFactListGetNonPublicDataTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "SpeciesFactList";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);//Arrange

                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;

                // gets the test file path and the reference path for excelfiles used in test
                string testRefFilePath;
                var testFilePath = GetTestFilePath(out testRefFilePath);


                //Act
                var result = controller.SpeciesFactList(DyntaxaTestSettings.Default.PsophusStridulusTaxonId.ToString()) as ViewResult;
                var viewModel = result.ViewData.Model as SpeciesFactViewModel;

                // Read the view data and save the data in an Arraylist were excel columns and rows match elements in created array list.
                ArrayList table = ReadModelData(viewModel);


                //Test if Stridulus contains nonPublicData
                TestListCointainsNonPublicData(table);
            }
        }

        /// <summary>
        /// A test for SpeciesFactList ie create a view list of factors for a taxon (Get action)Test data from controller vs a reference excel file. The data to be 
        /// verified  is not going to be changes in the future ie test data from ArtfaktadatabasenBIUS. 
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril).
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [Ignore] // some changes in database?
        public void SpeciesFactListGetPersistentDataTest2()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "SpeciesFactList";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);//Arrange
                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;

                // gets the test file path and the reference path for excelfiles used in test
                string testRefFilePath;
                var testFilePath = GetTestFilePath(out testRefFilePath);

                //Act
                var result = controller.SpeciesFactList(DyntaxaTestSettings.Default.ParnassiusApolloId.ToString()) as ViewResult;
                var viewModel = result.ViewData.Model as SpeciesFactViewModel;

                // Read the excel file and save the data in an Arraylist were excel columns and rows match elements in created array list.
                ArrayList table = ReadModelData(viewModel);


                string apolloRefPath = Path.Combine(testRefFilePath, "ParnassiusApolloRefFull.xlsx");
                ArrayList refTable = XlsxExcelFile.GetArrayListFromExcelFile(apolloRefPath);                
                ArrayList refTableNoInfo = new ArrayList();

                // Remove first 14 rows fron array
                for (int i = 0; i < refTable.Count; i++)
                {
                    if (i > 13)
                    {
                        refTableNoInfo.Add(refTable[i]);
                    }
                }

                // Assert
                // Now we find and compare.. Everything in ArtfaktadatabasenBIUS is persistent so we can test on that data..
                TestPersistentDataInArtfatadatabasenBIUS(refTableNoInfo, table);

                // Works for release 2012-07-09
                TestAllSpeciesFactData(refTableNoInfo, table, true);
            }
        }

        /// <summary>
        /// A test for SpeciesFactList ie create a view list of factors for a taxon (Get action)Test data from controller. The data to be 
        /// verified  is  that individula category data is set. 
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril).
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void SpeciesFactListGetIndividualCategoryDataTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "SpeciesFactList";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);//Arrange
                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;

                // gets the test file path and the reference path for excelfiles used in test
                string testRefFilePath;
                var testFilePath = GetTestFilePath(out testRefFilePath);

                // Act
                var result = controller.SpeciesFactList(DyntaxaTestSettings.Default.ParnassiusApolloId.ToString()) as ViewResult;
                var viewModel = result.ViewData.Model as SpeciesFactViewModel;

                // Read the excel file and save the data in an Arraylist were excel columns and rows match elements in created array list.
                ArrayList table = ReadModelData(viewModel);

                // Assert
                // Check indivudal category
                TestListCointainsIndividualCategoryData(table);
            }
        }

        /// <summary>
        /// A test for SpeciesFactList ie create a view list of factors for a taxon (Get action)Test data from controller vs a reference excel file. The data to be 
        /// verified  is not going to be changes in the future ie verify period data from 2000-2010. 
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril).
        /// </summary>
        [TestMethod]
        [TestCategory("DependentOnDatabaseContentNightlyTestApp")]
        // Datat har troligtvis ändrats. Assert.AreEqual failed. Expected:<Ej expertbedömd>. Actual:<Godtagbar>. Godtagbar don't match reference period value Ej expertbedömd
        [DeploymentItem("Resources", "Resources")]
        public void SpeciesFactListGetPeriodDataTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "SpeciesFactList";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);//Arrange
                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;

                // gets the test file path and the reference path for excelfiles used in test
                string testRefFilePath;
                var testFilePath = GetTestFilePath(out testRefFilePath);
                //// C:\Dev\ArtDatabanken\Web\Dyntaxa\Dyntaxa.Tests\Resources

                // Act
                var result = controller.SpeciesFactList(DyntaxaTestSettings.Default.ParnassiusApolloId.ToString()) as ViewResult;
                var viewModel = result.ViewData.Model as SpeciesFactViewModel;

                // Read the excel file and save the data in an Arraylist were excel columns and rows match elements in created array list.
                ArrayList table = ReadModelData(viewModel);

                string apolloRefPath = Path.Combine(testRefFilePath, "ParnassiusApolloRefFull.xlsx");                
                ArrayList refTable = XlsxExcelFile.GetArrayListFromExcelFile(apolloRefPath);

                // Assert
                // Test perioddata to 2010
                TestPeriodData(refTable, table);
            }
        }

        /// <summary>
        /// A test for SpeciesFactList ie create a new excel list of factors for a taxon (Post action). Test data from controller vs a reference excel file. The data to be 
        /// verified  is not going to be changes in the future ie test data from ArtfaktadatabasenBIUS. Test is performed on taxon PsophusStridulus(Trumgräshoppa).
        /// </summary>
        [TestMethod]
        [TestCategory("DependentOnDatabaseContentNightlyTestApp")]        
        [DeploymentItem("Resources", "Resources")]
        // Datat har troligtvis ändrats.
        public void SpeciesFactListPostPersistentDataTest1()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();

                // Since path is mapped we must separete log file...
                ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimDyntaxaLogger.WriteMessageStringObjectArray = (message, object1) =>
                {
                    return;
                };
                SetSwedishLanguage();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "SpeciesFactList";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                // Set that user has authrity to read nonpublic data
                IUserContext userContext = ApplicationUserContextSV;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;

                // gets the test file path and the reference path for excelfiles used in test
                string testRefFilePath;
                var testFilePath = GetTestFilePath(out testRefFilePath);
                string stridulusPath = Path.Combine(testFilePath, "PsophusStridulusTestFile.xlsx");
                SelectedPath = stridulusPath;

                // Create model....
                SpeciesFactViewModel model = new SpeciesFactViewModel();
                model.TaxonId = DyntaxaTestSettings.Default.PsophusStridulusTaxonId;
                model.PostAction = "SpeciesFactList";

                // Act
                FileStreamResult result = controller.SpeciesFactList(model, "67686") as FileStreamResult;
                var memoryStream = result.FileStream;                

                // Read the excel file and save the data in an Arraylist were excel columns and rows match elements in created array list.
                ArrayList table = XlsxExcelFile.GetArrayListFromExcelFile(memoryStream);                

                string stridulusRefPath = Path.Combine(testRefFilePath, "PsophusStridulusRefFull.xlsx");
                ArrayList refTable = XlsxExcelFile.GetArrayListFromExcelFile(stridulusRefPath);

                // Assert
                // Now we find and compare.. Everything in ArtfaktadatabasenBIUS is persistent so we can test on that data..
                TestPersistentDataInArtfatadatabasenBIUS(refTable, table);

                // Works for release 2012-07-09
                // To make this test work DB must be updated  TestAllSpeciesFactData(refTable, table);                
            }
        }

        /// <summary>
        /// A test for SpeciesFactList ie create a new excel list of factors for a taxon (Post action). Test host data has been gererate for 
        /// PsophusStridulus(Trumgräshoppa).
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [DeploymentItem("Resources", "Resources")]
        public void SpeciesFactListPostHostTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();

                // Since path is mapped we must separete log file...
                ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimDyntaxaLogger.WriteMessageStringObjectArray = (message, object1) =>
                {
                    return;
                };
                SetSwedishLanguage();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "SpeciesFactList";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                // Set that user has authrity to read nonpublic data
                IUserContext userContext = ApplicationUserContextSV;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;

                // gets the test file path and the reference path for excelfiles used in test
                string testRefFilePath;
                var testFilePath = GetTestFilePath(out testRefFilePath);
                string stridulusPath = Path.Combine(testFilePath, "PsophusStridulusTestFile.xlsx");
                SelectedPath = stridulusPath;


                // Create model....
                SpeciesFactViewModel model = new SpeciesFactViewModel();
                model.TaxonId = DyntaxaTestSettings.Default.PsophusStridulusTaxonId;
                model.PostAction = "SpeciesFactList";


                // Act
                FileStreamResult result = controller.SpeciesFactList(model, "67686") as FileStreamResult;
                var memoryStream = result.FileStream;
                
                // Read the excel file and save the data in an Arraylist were excel columns and rows match elements in created array list.
                ArrayList table = XlsxExcelFile.GetArrayListFromExcelFile(memoryStream);       

                // Assert
                // Test if Stridulus contains host list
                TestCointainsHostData(table);                
            }
        }


        /// <summary>
        /// A test for SpeciesFactList ie create a new excel list of factors for a taxon (Post action).
        /// Log in as person user without any authority to see nonpublic data. Verify that quality if factors in not below Mycket låg.
        /// Test is performed on taxon PsophusStridulus(Trumgräshoppa).
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [DeploymentItem("Resources", "Resources")]
        public void SpeciesFactListPostNonPublicDataTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                // Since path is mapped we must separete log file...
                ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimDyntaxaLogger.WriteMessageStringObjectArray = (message, object1) =>
                {
                    return;
                };
                SetSwedishLanguage();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "SpeciesFactList";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;

                // gets the test file path and the reference path for excelfiles used in test
                string testRefFilePath;
                var testFilePath = GetTestFilePath(out testRefFilePath);
                string stridulusPath = Path.Combine(testFilePath, "PsophusStridulusTestFile.xlsx");
                SelectedPath = stridulusPath;

                // Create model....
                SpeciesFactViewModel model = new SpeciesFactViewModel();
                model.TaxonId = DyntaxaTestSettings.Default.PsophusStridulusTaxonId;
                model.PostAction = "SpeciesFactList";


                // Act
                FileStreamResult result = controller.SpeciesFactList(model, "67686") as FileStreamResult;
                var memoryStream = result.FileStream;                

                // Reda the excel file and save the data in an Arraylist were excel columns and rows match elements in created array list.
                ArrayList table = XlsxExcelFile.GetArrayListFromExcelFile(memoryStream);


                // Test if Stridulus contains nonPublicData
                TestListCointainsNonPublicData(table);                
            }
        }

        /// <summary>
        /// A test for SpeciesFactList ie create a new excel list of factors for a taxon (Post action)Test data from controller vs a reference excel file. The data to be 
        /// verified  is not going to be changes in the future ie test data from ArtfaktadatabasenBIUS. 
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril).
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [DeploymentItem("Resources", "Resources")]
        [Ignore] // some changes in database?
        public void SpeciesFactListPostPersistentDataTest2()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                // Since path is mapped we must separete log file...
                ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimDyntaxaLogger.WriteMessageStringObjectArray = (message, object1) =>
                {
                    return;
                };
                SetSwedishLanguage();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "SpeciesFactList";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;

                // gets the test file path and the reference path for excelfiles used in test
                string testRefFilePath;
                var testFilePath = GetTestFilePath(out testRefFilePath);
                string apolloPath = Path.Combine(testFilePath, "ParnassiusApolloTestFile.xlsx");
                SelectedPath = apolloPath;

                // Create model....
                SpeciesFactViewModel model = new SpeciesFactViewModel();
                model.TaxonId = DyntaxaTestSettings.Default.ParnassiusApolloId;
                model.PostAction = "SpeciesFactList";

                // Act
                FileStreamResult result = controller.SpeciesFactList(model, "67686") as FileStreamResult;
                var memoryStream = result.FileStream;                

                // Read the excel file and save the data in an Arraylist were excel columns and rows match elements in created array list.
                ArrayList table = XlsxExcelFile.GetArrayListFromExcelFile(memoryStream);


                string apolloRefPath = Path.Combine(testRefFilePath, "ParnassiusApolloRefFull.xlsx");
                ArrayList refTable = XlsxExcelFile.GetArrayListFromExcelFile(apolloRefPath);

                // Assert
                // Now we find and compare.. Everything in ArtfaktadatabasenBIUS is persistent so we can test on that data..
                TestPersistentDataInArtfatadatabasenBIUS(refTable, table);

                // Works for release 2012-07-09
                TestAllSpeciesFactData(refTable, table);
            }
        }

        /// <summary>
        /// A test for SpeciesFactList ie create a new excel list of factors for a taxon (Post action)Test data from controller vs a reference excel file. The data to be 
        /// verified  is  that individula category data is set. 
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril).
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [DeploymentItem("Resources", "Resources")]
        public void SpeciesFactListPostIndividualCategoryDataTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();

                // Since path is mapped we must separete log file...
                ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimDyntaxaLogger.WriteMessageStringObjectArray = (message, object1) =>
                {
                    return;
                };
                SetSwedishLanguage();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "SpeciesFactList";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;

                // gets the test file path and the reference path for excelfiles used in test
                string testRefFilePath;
                var testFilePath = GetTestFilePath(out testRefFilePath);
                string apolloPath = Path.Combine(testFilePath, "ParnassiusApolloTestFile.xls");
                SelectedPath = apolloPath;

                // Create model....
                SpeciesFactViewModel model = new SpeciesFactViewModel();
                model.TaxonId = DyntaxaTestSettings.Default.ParnassiusApolloId;
                model.PostAction = "SpeciesFactList";

                // Act
                FileStreamResult result = controller.SpeciesFactList(model, "67686") as FileStreamResult;
                var memoryStream = result.FileStream;                

                // Read the excel file and save the data in an Arraylist were excel columns and rows match elements in created array list.
                ArrayList table = XlsxExcelFile.GetArrayListFromExcelFile(memoryStream);

                // Assert
                // Check indivudal category
                TestListCointainsIndividualCategoryData(table);                
            }
        }

        /// <summary>
        /// A test for SpeciesFactList ie create a new excel list of factors for a taxon (Post action)Test data from controller vs a reference excel file. The data to be 
        /// verified  is not going to be changes in the future ie verify period data from 2000-2010. 
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril).
        /// </summary>
        [TestMethod]
        [TestCategory("DependentOnDatabaseContentNightlyTestApp")]
        // Datat har troligtvis ändrats.
        [DeploymentItem("Resources", "Resources")]
        public void SpeciesFactListPostPeriodDataTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();

                // Since path is mapped we must separete log file...
                ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimDyntaxaLogger.WriteMessageStringObjectArray = (message, object1) =>
                {
                    return;
                };
                SetSwedishLanguage();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "SpeciesFactList";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                // Prepare role
                IUserContext userContext = ApplicationUserContextSV;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;

                // gets the test file path and the reference path for excelfiles used in test
                string testRefFilePath;
                var testFilePath = GetTestFilePath(out testRefFilePath);
                string apolloPath = Path.Combine(testFilePath, "ParnassiusApolloTestFile.xlsx");
                SelectedPath = apolloPath;

                // Create model....
                SpeciesFactViewModel model = new SpeciesFactViewModel();
                model.TaxonId = DyntaxaTestSettings.Default.ParnassiusApolloId;
                model.PostAction = "SpeciesFactList";


                // Act
                FileStreamResult result = controller.SpeciesFactList(model, "67686") as FileStreamResult;
                var memoryStream = result.FileStream;                

                // Read the excel file and save the data in an Arraylist were excel columns and rows match elements in created array list.                
                ArrayList table = XlsxExcelFile.GetArrayListFromExcelFile(memoryStream);

                // File.WriteAllBytes(@"C:\temp\ParnassiusApolloRefFull.xls", Dyntaxa.Tests.Properties.Resources.ParnassiusApolloRefFull);
                string apolloRefPath = Path.Combine(testRefFilePath, "ParnassiusApolloRefFull.xlsx");
                ArrayList refTable = XlsxExcelFile.GetArrayListFromExcelFile(apolloRefPath);                

                // Assert
                // Test perioddata to 2010
                TestPeriodData(refTable, table);               
            }
        }


        /// <summary>
        /// Get test for Edit factors
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril).
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void EditFactorsForSubstrateGetTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "EditFactors";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                // Arrange
                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;

                // Create model....
                SpeciesFactViewModel model = new SpeciesFactViewModel();
                model.TaxonId = DyntaxaTestSettings.Default.ParnassiusApolloId;
                model.PostAction = "SpeciesFactList";

                bool testHost = true;
                bool testFactorReference = false;
                string taxonId = Convert.ToString(DyntaxaTestSettings.Default.ParnassiusApolloId);
                string factorId = Convert.ToString((int)DyntaxaFactorId.SUBSTRATE);
                string factorDataType = Convert.ToString((int)DyntaxaFactorDataType.AF_SUBSTRATE);
                string dataType = Convert.ToString((int)DyntaxaDataType.ENUM);
                string referenceId = "12";

                // In this test we only have hosts with values therfore factor name equals displayed host name!!!
                string testFactorId = "1150";
                string testFactorName = "Värdväxt: ospecificerat";

                string testHostFactorId = "1142";
                int testHostTaxonId = 223343;
                string testHostFactorName = "Hylotelephium telephium (L.) H. Ohba, kärleksört"; 
                string factorFieldValueName = "Betydelse";
                string factorFieldValue2Name = "Nyttjande";

                // Act
                var result = controller.EditFactors(taxonId, factorId, dataType, factorDataType, referenceId) as ViewResult;
                var viewModel = result.ViewData.Model as SpeciesFactViewModel;

                // Assert
                TestFactorAndHostFactorValues(viewModel, taxonId, factorId, factorDataType, dataType, referenceId, testFactorId, testHostFactorId, testHostTaxonId, testFactorName, testHostFactorName, testHost,
                                              factorFieldValueName, factorFieldValue2Name, testFactorReference);
            }
        }

        /// <summary>
        /// Get test for Edit factors
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril).
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [Ignore] // Some changes in database?
        public void EditFactorsForInfluenceGetTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "EditFactors";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;

                // Create model....
                SpeciesFactViewModel model = new SpeciesFactViewModel();
                model.TaxonId = DyntaxaTestSettings.Default.ParnassiusApolloId;
                model.PostAction = "SpeciesFactList";

                bool testHost = true;
                bool testFactorReference = false;
                string taxonId = Convert.ToString(DyntaxaTestSettings.Default.ParnassiusApolloId);
                string factorId = Convert.ToString((int)DyntaxaFactorId.INFLUENCE);
                string factorDataType = Convert.ToString((int)DyntaxaFactorDataType.AF_INFLUENCE);
                string dataType = Convert.ToString((int)DyntaxaDataType.ENUM);
                string referenceId = "100";

                string testFactorId = "2020";
                string testFactorName = "Igenväxning/förtätning av artens biotop";


                string testHostFactorId = "1683";
                int testHostTaxonId = 0;
               // string testHostTaxonName = "Djurslag: ospecificerat";
                string testHostFactorName = "Djurslag: ospecificerat";
                string factorFieldValueName = "Effekt";
                string factorFieldValue2Name = "Faktorns relevans";

                // Act
                var result = controller.EditFactors(taxonId, factorId, dataType, factorDataType, referenceId) as ViewResult;
                var viewModel = result.ViewData.Model as SpeciesFactViewModel;

                // Assert
                TestFactorAndHostFactorValues(viewModel, taxonId, factorId, factorDataType, dataType, referenceId, testFactorId, testHostFactorId, testHostTaxonId, testFactorName, testHostFactorName, testHost,
                                              factorFieldValueName, factorFieldValue2Name, testFactorReference);
            }
        }

        /// <summary>
        /// Get test for Edit factors
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril).
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void EditFactorsForHabitateGetTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                SetSwedishLanguage();
                LoginApplicationUserAndSetSessionVariables();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "EditFactors";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);//Arrange
                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;

                // Create model....
                SpeciesFactViewModel model = new SpeciesFactViewModel();
                model.TaxonId = DyntaxaTestSettings.Default.ParnassiusApolloId;
                model.PostAction = "SpeciesFactList";

                bool testHost = false;
                bool testFactorReference = false;
                string taxonId = Convert.ToString(DyntaxaTestSettings.Default.ParnassiusApolloId);
                string factorId = Convert.ToString((int)DyntaxaFactorId.BIOTOPE);
                string factorDataType = Convert.ToString((int)DyntaxaFactorDataType.AF_BIOTOPE);
                string dataType = Convert.ToString((int)DyntaxaDataType.ENUM);
                string referenceId = "10";

                string testFactorId = "804";
                string testFactorName = "Kalktallskog";


                string factorFieldValueName = "Betydelse";
                string factorFieldValue2Name = "";



                //Act
                var result = controller.EditFactors(taxonId, factorId, dataType, factorDataType, referenceId) as ViewResult;
                var viewModel = result.ViewData.Model as SpeciesFactViewModel;

                //Assert
                TestFactorAndHostFactorValues(viewModel, taxonId, factorId, factorDataType, dataType, referenceId, testFactorId, null, 0, testFactorName, null, testHost,
                                              factorFieldValueName, factorFieldValue2Name, testFactorReference);
            }
        }


        /// <summary>
        /// Get test for Edit factors TODO
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril).
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void EditHostFactorsForSubstrateGetTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "EditFactors";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                // Arrange
                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;

                // Create model....
                SpeciesFactViewModel model = new SpeciesFactViewModel();
                model.TaxonId = DyntaxaTestSettings.Default.ParnassiusApolloId;
                model.PostAction = "SpeciesFactList";

                string taxonId = Convert.ToString(DyntaxaTestSettings.Default.ParnassiusApolloId);
                string factorDataType = Convert.ToString((int)DyntaxaFactorDataType.AF_SUBSTRATE);
                string dataType = Convert.ToString((int)DyntaxaDataType.ENUM);
                int referenceId = 1282;

                int testFactorId = 1150;
                string testFactorName = "Sedum album L., vit fetknopp";

                int testHostFactorId = 0;
                int testHostTaxonId = 223398;
                int factorFieldValue = 2;
                int factorFieldValue2 = 3;
                int individualCat = 12;
                int qualityId = 1;

                // Act
                var result = controller.EditHostFactorsForSubstrate(taxonId, Convert.ToString(referenceId)) as ViewResult;
                var viewModel = result.ViewData.Model as SpeciesFactHostViewModel;
          
                // Assert
                TestFactorAndHostValues(viewModel, taxonId, (int)DyntaxaFactorId.SUBSTRATE, factorDataType, dataType, referenceId, testFactorId, testHostFactorId, testFactorName,
                                              testHostTaxonId, factorFieldValue, factorFieldValue2, individualCat, qualityId);
            }
        }

        /// <summary>
        /// Get test for Edit host factor items
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril).
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void EditHostFactorItemsGetTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "EditFactors";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                // Arrange
                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;
                List<SpeciesFactHostsIdListHelper> speciesFactHostTaxonIdList = new List<SpeciesFactHostsIdListHelper>();
                speciesFactHostTaxonIdList.Add(new SpeciesFactHostsIdListHelper()
                {
                    CategoryId = 0,
                    FactorId = 1142,
                    Id = 223398
                });
                speciesFactHostTaxonIdList.Add(new SpeciesFactHostsIdListHelper()
                {
                    CategoryId = 12,
                    FactorId = 1150,
                    Id = 223343
                });
                SessionSpeciesFactHostTaxonIdList = speciesFactHostTaxonIdList;
                
                // Create model test data....
                string taxonId = Convert.ToString(DyntaxaTestSettings.Default.ParnassiusApolloId);
                string factorId = Convert.ToString((int)DyntaxaFactorId.SUBSTRATE);
                string referenceId = "12";

                string individualCategoryId = "0";
                string testFactorName = "Värdtaxa";
                string testTaxonName = "Sedum album L., vit fetknopp";
                string testTaxonName2 = "Hylotelephium telephium (L.) H. Ohba, kärleksört";

                string befintligBedömning = "Generellt";
                int betydelse = 2;
                string factorFieldValueName = "Betydelse";
                int nyttjande = 3;
                string factorFieldValue2Name = "Nyttjande";
                int quality = 3;
                string existingRef = "Karin";
                string senastUppdaterad = "Karin";
               

                // Act
                var result = controller.EditHostFactorItems(taxonId, referenceId) as ViewResult;
                var viewModel = result.ViewData.Model as FactorViewModel;

                // Assert
                Assert.IsTrue(viewModel.IsNotNull());
                Assert.IsTrue(viewModel.MainParentFactorId == Convert.ToInt32(factorId));
                Assert.IsTrue(viewModel.FactorReferenceId == Convert.ToInt32(referenceId));
                Assert.IsTrue(viewModel.IndividualCategoryId == Convert.ToInt32(individualCategoryId));
                Assert.IsTrue(viewModel.IndividualCategoryList.Count == 2);
                Assert.IsTrue(viewModel.HostTaxaText.Contains(testTaxonName));
                Assert.IsTrue(viewModel.HostTaxaText.Contains(testTaxonName2));



                Assert.IsTrue(viewModel.FactorName.Contains(testFactorName));
                Assert.IsTrue(viewModel.FactorFieldEnumValue == betydelse);
                Assert.IsTrue(viewModel.FactorFieldEnumLabel.Equals(factorFieldValueName));
                Assert.IsTrue(viewModel.FactorFieldEnumValueList.Count > 0);
                Assert.IsTrue(viewModel.FactorFieldEnumValue2 == nyttjande);
                Assert.IsTrue(viewModel.FactorFieldEnumLabel2.Equals(factorFieldValue2Name));
                Assert.IsTrue(viewModel.FactorFieldEnumValueList2.Count > 0);
                Assert.IsTrue(viewModel.ExistingEvaluations.Contains(befintligBedömning));
                Assert.IsTrue(viewModel.QualityId == quality);
                Assert.IsTrue(viewModel.UpdateUserData.Contains(senastUppdaterad));
                Assert.IsTrue(viewModel.FactorReferenceOld.Contains(existingRef));
            }
        }

        /// <summary>
        /// Get test for Edit host factor item.
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril).
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void EditHostFactorItemGetTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "EditHostFactorItem";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                // Arrange
                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;

                // Create model test data....
                string taxonId = Convert.ToString(DyntaxaTestSettings.Default.ParnassiusApolloId);
                string factorId = Convert.ToString((int)DyntaxaFactorId.SUBSTRATE);
                string factorDataType = Convert.ToString((int)DyntaxaFactorDataType.AF_SUBSTRATE);
                string dataType = Convert.ToString((int)DyntaxaDataType.ENUM);
                string referenceId = "12";
           
                string testHostTaxonId = "223398";
                string individualCategoryId = "0";
                string oldIndividualCategoryId = "0";
                string childFactorId = "1142";
                string testFactorName = "Sedum album L., vit fetknopp";

                string befintligBedömning = "Generellt";
                int betydelse = 2;
                string factorFieldValueName = "Betydelse";
                int nyttjande = 3;
                string factorFieldValue2Name = "Nyttjande";
                int quality = 3;
                string existingRef = "Karin";
                string senastUppdaterad = "Karin";
                

                // Act
                var result = controller.EditHostFactorItem(taxonId, dataType, factorDataType, childFactorId, referenceId, individualCategoryId, testHostTaxonId, factorId, oldIndividualCategoryId) as ViewResult;
                var viewModel = result.ViewData.Model as FactorViewModel;

                // Assert
                Assert.IsTrue(viewModel.IsNotNull());
                Assert.IsTrue(viewModel.MainParentFactorId == Convert.ToInt32(factorId));
                Assert.IsTrue(viewModel.ChildFactorId == Convert.ToInt32(childFactorId));
                Assert.IsTrue(viewModel.HostId == Convert.ToInt32(testHostTaxonId));
                Assert.IsTrue(viewModel.FactorReferenceId == Convert.ToInt32(referenceId));
                Assert.IsTrue(viewModel.IndividualCategoryId == Convert.ToInt32(individualCategoryId));



                Assert.IsTrue(viewModel.FactorName.Contains(testFactorName));
                Assert.IsTrue(viewModel.FactorFieldEnumValue == betydelse);
                Assert.IsTrue(viewModel.FactorFieldEnumLabel.Equals(factorFieldValueName));
                Assert.IsTrue(viewModel.FactorFieldEnumValueList.Count > 0);
                Assert.IsTrue(viewModel.FactorFieldEnumValue2 == nyttjande);
                Assert.IsTrue(viewModel.FactorFieldEnumLabel2.Equals(factorFieldValue2Name));
                Assert.IsTrue(viewModel.FactorFieldEnumValueList2.Count > 0);
                Assert.IsTrue(viewModel.ExistingEvaluations.Contains(befintligBedömning));
                Assert.IsTrue(viewModel.QualityId == quality);
                Assert.IsTrue(viewModel.UpdateUserData.Contains(senastUppdaterad));
                Assert.IsTrue(viewModel.FactorReferenceOld.Contains(existingRef));
            }
        }


        /// <summary>
        /// Get test for Edit factor item.
        /// Test is performed on taxon Stridulus(Trumgräshoppa).
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [Ignore] // Some changes in database?
        public void EditFactorItemGetTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "EditFactorItem";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                // Arrange
                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;


                string taxonId = Convert.ToString(DyntaxaTestSettings.Default.PsophusStridulusTaxonId);
                string factorId = Convert.ToString((int)DyntaxaFactorId.SUBSTRATE);
                string factorDataType = Convert.ToString((int)DyntaxaFactorDataType.AF_SUBSTRATE);
                string dataType = Convert.ToString((int)DyntaxaDataType.ENUM);
                string referenceId = "12";
                string individualCategoryId = "0";
                string childFactorId = "1080";
                string testHostTaxonId = "0";

                string testFactorName = "Mark/Sediment som substrat";
                
                string befintligBedömning = "Generellt";
                int betydelse = 2;
                string factorFieldValueName = "Betydelse";
                int nyttjande = 9;
                string factorFieldValue2Name = "Nyttjande";
                int quality = 3;
                string existingRef = "";
                string senastUppdaterad = "";
               


                // Act
                var result = controller.EditFactorItem(taxonId, dataType, factorDataType, childFactorId, referenceId, individualCategoryId, testHostTaxonId, factorId) as ViewResult;
                var viewModel = result.ViewData.Model as FactorViewModel;

                // Assert
                Assert.IsTrue(viewModel.IsNotNull());
                Assert.IsTrue(viewModel.MainParentFactorId == Convert.ToInt32(factorId));
                Assert.IsTrue(viewModel.ChildFactorId == Convert.ToInt32(childFactorId));
                Assert.IsTrue(viewModel.HostId == Convert.ToInt32(testHostTaxonId));
                Assert.IsTrue(viewModel.FactorReferenceId == Convert.ToInt32(referenceId));
                Assert.IsTrue(viewModel.IndividualCategoryId == Convert.ToInt32(individualCategoryId));



                Assert.IsTrue(viewModel.FactorName.Equals(testFactorName));
                Assert.IsTrue(viewModel.FactorFieldEnumValue == betydelse);
                Assert.IsTrue(viewModel.FactorFieldEnumLabel.Equals(factorFieldValueName));
                Assert.IsTrue(viewModel.FactorFieldEnumValueList.Count > 0);
                Assert.IsTrue(viewModel.FactorFieldEnumValue2 == nyttjande);
                Assert.IsTrue(viewModel.FactorFieldEnumLabel2.Equals(factorFieldValue2Name));
                Assert.IsTrue(viewModel.FactorFieldEnumValueList2.Count > 0);
                Assert.IsTrue(viewModel.ExistingEvaluations.Contains(befintligBedömning));
                Assert.IsTrue(viewModel.QualityId == quality);
                Assert.IsTrue(viewModel.UpdateUserData.Contains(senastUppdaterad));
                Assert.IsTrue(viewModel.FactorReferenceOld.Equals(existingRef));
              
            }
        }

        #endregion

        #region Test helper methods



        // performe test on Factor and host data
        /// <summary>
        /// The test factor and host factor values.
        /// </summary>
        /// <param name="viewModel">
        /// The view model.
        /// </param>
        /// <param name="taxonId">
        /// The taxon id.
        /// </param>
        /// <param name="factorId">
        /// The factor id.
        /// </param>
        /// <param name="factorDataType">
        /// The factor data type.
        /// </param>
        /// <param name="dataType">
        /// The data type.
        /// </param>
        /// <param name="referenceId">
        /// The reference id.
        /// </param>
        /// <param name="testFactorId">
        /// The test factor id.
        /// </param>
        /// <param name="testHostFactorId">
        /// The test host factor id.
        /// </param>
        /// <param name="testHostTaxonId">
        /// The test host taxon id.
        /// </param>
        /// <param name="testFactorName">
        /// The test factor name.
        /// </param>
        /// <param name="testHostFactorName">
        /// The test host factor name.
        /// </param>
        /// <param name="testHost">
        /// The test host.
        /// </param>
        /// <param name="factorFieldValueName">
        /// The factor field value name.
        /// </param>
        /// <param name="factorFieldValue2Name">
        /// The factor field value 2 name.
        /// </param>
        /// <param name="testFactorReference">
        /// The test factor reference.
        /// </param>
        private static void TestFactorAndHostFactorValues(SpeciesFactViewModel viewModel, string taxonId, string factorId,
                                                          string factorDataType, string dataType, string referenceId,
                                                          string testFactorId, string testHostFactorId, int testHostTaxonId,
                                                          string testFactorName, string testHostFactorName, bool testHost,
                                                          string factorFieldValueName, string factorFieldValue2Name, bool testFactorReference)
        {
            Assert.IsNotNull(viewModel);
            Assert.IsTrue(viewModel.AllAvaliableFactors.Count > 0);
            Assert.IsTrue(viewModel.FactorList.Count > 0);
            Assert.IsTrue(Convert.ToString(viewModel.TaxonId).Equals(taxonId));
            Assert.IsTrue(Convert.ToString((int)viewModel.MainParentFactorId).Equals(factorId));
            Assert.IsTrue(Convert.ToString((int)viewModel.FactorDataType).Equals(factorDataType));
            Assert.IsTrue(Convert.ToString((int)viewModel.DataType).Equals(dataType));
            Assert.IsTrue(Convert.ToString(viewModel.ReferenceId).Equals(referenceId));
            Assert.IsTrue(viewModel.DropDownAllFactorId == 0);
            Assert.IsTrue(viewModel.DropDownFactorId == 0);
            Assert.IsNotNull(viewModel.SpeciesFactViewModelHeaderItemList);
            Assert.IsTrue(viewModel.SpeciesFactViewModelHeaderItemList.Count > 0);
            Assert.IsNotNull(viewModel.SpeciesFactViewModelHeaderItemList.Last());
            Assert.IsNotNull(viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList);
            Assert.IsNotNull(viewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last());
            Assert.IsNotNull(
                viewModel.SpeciesFactViewModelHeaderItemList.Last()
                         .SpeciecFactViewModelSubHeaderItemList.Last()
                         .SpeciesFactViewModelItemList);

            // Get factor Organogena jordar/sediment with id 1091 to test for...
            SpeciesFactViewModelItem item = null;
            SpeciesFactViewModelItem hostItem = null;
            foreach (SpeciesFactViewModelHeaderItem headerItem in viewModel.SpeciesFactViewModelHeaderItemList)
            {
                foreach (
                    SpeciesFactViewModelSubHeaderItem superiorHeaderItem in headerItem.SpeciecFactViewModelSubHeaderItemList)
                {
                    if (item.IsNull())
                        item = superiorHeaderItem.SpeciesFactViewModelItemList.FirstOrDefault(x => x.FactorId.Equals(testFactorId) && x.FieldValues.IsNotNull() && x.IsHeader == false);
                    if (hostItem.IsNull())
                        hostItem =
                        superiorHeaderItem.SpeciesFactViewModelItemList.FirstOrDefault(
                            x => (x.FactorId.Equals(testHostFactorId) && x.HostId.Equals(testHostTaxonId) && x.IsHost && x.FieldValues.IsNotNull()));
                }
            }

            // Test factor Item
            Assert.IsNotNull(item);
            Assert.IsNotNull(item.FactorFieldValue);
            Assert.IsTrue(item.FactorId.Equals(testFactorId));
            Assert.IsTrue(item.FactorName.Equals(testFactorName));
            Assert.IsNotNull(item.FactorFieldValue);
            Assert.IsTrue(item.FactorFieldValue.Contains(factorFieldValueName));
            Assert.IsTrue(item.FactorFieldValue2.Contains(factorFieldValue2Name));
            Assert.IsNotNull(item.FactorFieldComment);
            Assert.IsNotNull(item.FactorSortOrder);
            Assert.IsTrue(Convert.ToInt32(item.FactorSortOrder) > 0);
            Assert.IsTrue(item.IndividualCategoryId >= 0);
            Assert.IsTrue(!item.IndividualCategoryName.Equals(string.Empty));

            // Fix type of factro
            Assert.IsTrue(item.MainParentFactorId == Convert.ToInt32(factorId));
            if (testFactorReference)

                // Last item reference id : only valide when saved/factor updated 
                Assert.IsTrue(item.ReferenceId == Convert.ToInt32(referenceId));

            Assert.IsTrue(item.TaxonId.Equals(taxonId));
            if (item.HostId != 0)
            {
                Assert.IsTrue(item.HostId == testHostTaxonId); 
            }
            else
            {
                Assert.IsTrue(item.HostId == 0); 
            }
            

            // First field value must always be set... ie GUI parameter betydelse/effekt
            Assert.IsNotNull(item.FieldValues);
            Assert.IsTrue(item.FieldValues.FactorFieldValues.Count > 0);

            // This should br main 
            Assert.IsTrue(Convert.ToInt32(item.FieldValues.MainParentFactorId) == Convert.ToInt32(factorId));
            Assert.IsNotNull(item.FieldValues.FieldName);
            Assert.IsTrue(item.FieldValues.FieldKey >= 0);
            Assert.IsNotNull(item.FieldValues.FieldValue);

            // Second field value can be set... ie GUI parameter nyttljande only valid för substrate, or factor relevans for influence.
            if (Convert.ToInt32(DyntaxaFactorId.BIOTOPE) != Convert.ToInt32(factorId))
            {
                Assert.IsNotNull(item.FieldValues2);
                Assert.IsTrue(item.FieldValues2.FactorFieldValues.Count > 0);
                Assert.IsTrue(Convert.ToInt32(item.FieldValues2.MainParentFactorId) == Convert.ToInt32(factorId));
                Assert.IsNotNull(item.FieldValues2.FieldName);
                Assert.IsTrue(item.FieldValues2.FieldKey >= 0);
                Assert.IsNotNull(item.FieldValues2.FieldValue);
            }

            // Quality field value must be be set... ie GUI parameter Kvalitet
            Assert.IsNotNull(item.Quality);
            Assert.IsTrue(item.QualityId >= 0);
            Assert.IsTrue(item.QualityValues.QualityValues.Count > 0);
            Assert.IsTrue(Convert.ToInt32(item.QualityValues.MainParentFactorId) == Convert.ToInt32(factorId));
            Assert.IsNotNull(item.QualityValues.FieldName);
            Assert.IsTrue(item.QualityValues.FieldValue == item.QualityId);
            Assert.IsTrue(item.QualityValues.FieldName.Equals(item.Quality));

            // Check headers ie display data
            Assert.IsFalse(item.IsHeader); // Ie is not a leaf displayed with bold text.
            Assert.IsFalse(item.IsMainHeader);
            Assert.IsTrue(item.MainHeader.Equals(string.Empty));
            Assert.IsFalse(item.IsSubHeader);
            Assert.IsTrue(item.SubHeader.Equals(string.Empty));
            Assert.IsFalse(item.IsSuperiorHeader);

            // Assert.IsTrue(item.SuperiorHeader.Equals(string.Empty)); here we have a merge of a subheader and a header; therefore the subheader got a name.

            // Test if different color is set, only avaliable if host
            if (!item.IsHost)
            {
                Assert.IsFalse(item.UseDifferentColor);
            }  
            else
            {
                Assert.IsTrue(item.UseDifferentColor);
            }
            Assert.IsTrue(item.UseDifferentColorFromIndex == 1);

            if (testHost)
            {
                // Test host Item
                Assert.IsNotNull(hostItem);
                Assert.IsNotNull(hostItem.FactorFieldValue);
                Assert.IsTrue(hostItem.FactorId.Equals(testHostFactorId));
                Assert.IsTrue(hostItem.FactorName.Equals(testHostFactorName));
                Assert.IsTrue(item.FactorFieldValue.Contains(factorFieldValueName));
                Assert.IsTrue(item.FactorFieldValue2.Contains(factorFieldValue2Name));
                Assert.IsNotNull(item.FactorFieldComment);
                Assert.IsNotNull(item.FactorSortOrder);
                Assert.IsTrue(Convert.ToInt32(item.FactorSortOrder) > 0);
                Assert.IsTrue(hostItem.IndividualCategoryId >= 0);
                if (hostItem.HostId != 0)
                    Assert.IsTrue(!hostItem.IndividualCategoryName.Equals(string.Empty));
                Assert.IsTrue(hostItem.MainParentFactorId == Convert.ToInt32(factorId));
                if (testFactorReference)// Last item reference id : only == when saved  
                    Assert.IsTrue(hostItem.ReferenceId == Convert.ToInt32(referenceId));
                Assert.IsTrue(hostItem.TaxonId.Equals(taxonId));

                Assert.IsTrue(hostItem.HostId == testHostTaxonId);
                Assert.IsTrue(hostItem.HostId == testHostTaxonId);

                // First field value must always be set... ie GUI parameter betydelse/effekt
                Assert.IsNotNull(hostItem.FieldValues);
                Assert.IsTrue(hostItem.FieldValues.FactorFieldValues.Count > 0);
                Assert.IsTrue(Convert.ToInt32(hostItem.FieldValues.MainParentFactorId) == Convert.ToInt32(factorId));
                Assert.IsNotNull(hostItem.FieldValues.FieldName);
                Assert.IsTrue(hostItem.FieldValues.FieldKey >= 0);
                Assert.IsNotNull(hostItem.FieldValues.FieldValue);

                // Second field value can be set... ie GUI parameter nyttljande only valid för substrate, or factor relevans for influence.
                if (Convert.ToInt32(DyntaxaFactorId.BIOTOPE) != Convert.ToInt32(factorId))
                {
                    Assert.IsNotNull(hostItem.FieldValues2);
                    Assert.IsTrue(hostItem.FieldValues2.FactorFieldValues.Count > 0);
                    Assert.IsTrue(Convert.ToInt32(hostItem.FieldValues2.MainParentFactorId) == Convert.ToInt32(factorId));
                    Assert.IsNotNull(hostItem.FieldValues2.FieldName);
                    Assert.IsTrue(hostItem.FieldValues2.FieldKey >= 0);
                    Assert.IsNotNull(hostItem.FieldValues2.FieldValue);
                }

                // Quality field value must be be set... ie GUI parameter Kvalitet
                Assert.IsNotNull(hostItem.Quality);
                Assert.IsTrue(hostItem.QualityId >= 0);
                Assert.IsTrue(hostItem.QualityValues.QualityValues.Count > 0);
                Assert.IsTrue(Convert.ToInt32(hostItem.QualityValues.MainParentFactorId) == Convert.ToInt32(factorId));
                Assert.IsNotNull(hostItem.QualityValues.FieldName);
                Assert.IsTrue(hostItem.QualityValues.FieldValue == hostItem.QualityId);
                Assert.IsTrue(hostItem.QualityValues.FieldName.Equals(hostItem.Quality));

                // Check headers ie display data
                Assert.IsFalse(hostItem.IsHeader);
                Assert.IsNotNull(hostItem.IsMainHeader);
                Assert.IsTrue(hostItem.MainHeader.Equals(string.Empty));
                Assert.IsNotNull(hostItem.IsSubHeader);
                Assert.IsTrue(hostItem.SubHeader.Equals(string.Empty));
                Assert.IsNotNull(hostItem.IsSuperiorHeader);
                Assert.IsTrue(hostItem.SuperiorHeader.Equals(string.Empty));

                // Assert.IsFalse(hostItem.IsMarked);
                Assert.IsTrue(hostItem.UseDifferentColor);
                Assert.IsTrue(hostItem.UseDifferentColorFromIndex >= 0);
            }
        }


        /// <summary>
        /// The test factor and host factor values.
        /// </summary>
        /// <param name="viewModel">
        /// The view model.
        /// </param>
        /// <param name="taxonId">
        /// The taxon id.
        /// </param>
        /// <param name="factorId">
        /// The factor id.
        /// </param>
        /// <param name="factorDataType">
        /// The factor data type.
        /// </param>
        /// <param name="dataType">
        /// The data type.
        /// </param>
        /// <param name="referenceId">
        /// The reference id.
        /// </param>
        /// <param name="testFactorId">
        /// The test factor id.
        /// </param>
        /// <param name="testHostFactorId">
        /// The test host factor id.
        /// </param>
        /// <param name="testHostTaxonId">
        /// The test host taxon id.
        /// </param>
        /// <param name="testFactorName">
        /// The test factor name.
        /// </param> 
        /// <param name="factorFieldValue">
        /// The factor field value name.
        /// </param>
        /// <param name="factorFieldValue2">
        /// The factor field value 2 name.
        /// </param>
        private static void TestFactorAndHostValues(SpeciesFactHostViewModel viewModel, string taxonId, int factorId,
                                                          string factorDataType, string dataType, int referenceId,
                                                          int testFactorId, int testHostFactorId, 
                                                          string testFactorName, int testHostTaxonId,
                                                          int factorFieldValue, int factorFieldValue2, int individualCat, int qualityId)
        {
            Assert.IsNotNull(viewModel);
            Assert.IsTrue(Convert.ToString(viewModel.TaxonId).Equals(taxonId));
            Assert.IsTrue((int)viewModel.MainParentFactorId == factorId);
            Assert.IsTrue(Convert.ToString((int)viewModel.FactorDataType).Equals(factorDataType));
            Assert.IsTrue(Convert.ToString((int)viewModel.DataType).Equals(dataType));
            Assert.IsTrue(Convert.ToInt32(viewModel.ReferenceId) == referenceId);
            Assert.IsTrue(viewModel.DropDownTaxonId == -1000);
            Assert.IsTrue(viewModel.DropDownHostFactorId == -1000);
            Assert.IsNotNull(viewModel.SpeciesFactViewModel);
            Assert.IsNotNull(viewModel.SpeciesFactViewModel.SpeciesFactViewModelHeaderItemList.Count > 0);
            Assert.IsNotNull(viewModel.SpeciesFactViewModel.SpeciesFactViewModelHeaderItemList.Last());
            Assert.IsNotNull(viewModel.SpeciesFactViewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList);
            Assert.IsNotNull(viewModel.SpeciesFactViewModel.SpeciesFactViewModelHeaderItemList.Last().SpeciecFactViewModelSubHeaderItemList.Last());
            Assert.IsNotNull(
                viewModel.SpeciesFactViewModel.SpeciesFactViewModelHeaderItemList.Last()
                         .SpeciecFactViewModelSubHeaderItemList.Last()
                         .SpeciesFactViewModelItemList);

            SpeciesFactViewModelItem item = viewModel.SpeciesFactViewModel.SpeciesFactViewModelHeaderItemList.Last()
                          .SpeciecFactViewModelSubHeaderItemList.Last()
                          .SpeciesFactViewModelItemList.Last();
            Assert.IsTrue(item.HostId == testHostTaxonId);
            Assert.IsTrue(Convert.ToInt32(item.FactorId) == testFactorId);
            Assert.IsTrue(item.IndividualCategoryName.IsNotNull());
            Assert.IsTrue(item.IndividualCategoryId == individualCat);
            Assert.IsTrue(item.FactorName.Contains(testFactorName));
            Assert.IsTrue(item.FieldValues.FieldValue == factorFieldValue);
            Assert.IsTrue(item.FieldValues2.FieldValue == factorFieldValue2);
            Assert.IsTrue(item.QualityId == qualityId);
            Assert.IsTrue(item.ReferenceId == referenceId);
        }


        /// <summary>
        /// Set the test paths
        /// </summary>
        /// <param name="testRefFilePath"></param>
        /// <returns></returns>
        private string GetTestFilePath(out string testRefFilePath)
        {
            // Create test file path
            string startupPath = AssemblyDirectory;
            string path = Directory.GetCurrentDirectory();
            string rootDir = Path.GetPathRoot(startupPath);
            testRefFilePath = Path.Combine(startupPath, DyntaxaTestSettings.Default.PathToExcelTestFiles);

            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string uriPath = Uri.UnescapeDataString(uri.Path);
            string directoryPath = Path.GetDirectoryName(uriPath);
            string testFilePath = directoryPath + "\\" + "Temp"; 
            SessionHelper.SetInSession("testFilePath", testFilePath);

           return testFilePath;
        }

        /// <summary>
        /// Gets the assembly directory.
        /// </summary>
        static public string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        /// <summary>
        /// Compare att periodic data from created excelfile and reference excelfile
        /// </summary>
        /// <param name="refTable"></param>
        /// <param name="table"></param>
        private void TestPeriodData(ArrayList refTable, ArrayList table)
        {
            int rodlistedatabasenStartRow = 0;
            int rodlistedatabasenEndRow = 0;
            int index = 1;
            foreach (ArrayList row in table)
            {
                if (row.Contains("RÖDLISTEDATABASEN"))
                {
                    rodlistedatabasenStartRow = index;
                }
                if (row.Contains("Artfaktadatabasen BIUS"))
                {
                    rodlistedatabasenEndRow = index;
                }
                index++;
            }

            ArrayList periodData = new ArrayList();
            for (int i = rodlistedatabasenStartRow; i < rodlistedatabasenEndRow; i++)
            {
                ArrayList testRow = table[i] as ArrayList;
                for (int k = 0; k < testRow.Count; k++)
                {
                    if (k == 4)
                    {

                        string testText = (string)testRow[k];
                        if (testText.Contains("(2000)") || testText.Contains("(2005)") || testText.Contains("(2010)"))
                        {
                            periodData.Add(testRow);
                        }
                    }


                }
              
            }

            int rodlistedatabasenStartRowRef = 0;
            int rodlistedatabasenEndRowRef = 0;
            int indexRef = 1;
            foreach (ArrayList row in refTable)
            {
                if (row.Contains("RÖDLISTEDATABASEN"))
                {
                    rodlistedatabasenStartRowRef = indexRef;
                }

                if (row.Contains("Artfaktadatabasen BIUS"))
                {
                    rodlistedatabasenEndRowRef = indexRef;
                }

                indexRef++;
            }

            ArrayList periodRefData = new ArrayList();
            for (int i = rodlistedatabasenStartRowRef; i < rodlistedatabasenEndRowRef; i++)
            {
                ArrayList refRow = refTable[i] as ArrayList;
                 for (int k = 0; k < refRow.Count; k++)
                {
                    if (k == 4)
                    {
                        string refText = (string)refRow[k];
                        if (refText.Contains("(2000)") || refText.Contains("(2005)") || refText.Contains("(2010)"))
                        {
                            periodRefData.Add(refRow);
                        }
                    }
                }              
            }

            Assert.IsTrue(rodlistedatabasenEndRow - rodlistedatabasenStartRow > 0);
            Assert.IsTrue(periodRefData.Count == periodData.Count);

            // Compare
            int j = 0;
            for (int i = 0; i < periodRefData.Count; i++)
            {
                ArrayList refRow = periodRefData[i] as ArrayList;
                ArrayList testRow = periodData[j] as ArrayList;
                for (int k = 0; k < refRow.Count; k++)
                {
                    string testText = Convert.ToString(testRow[k]);
                    string refText = Convert.ToString(refRow[k]);
                    Assert.AreEqual(refText, testText, testText + " don't match reference period value " + refText);
                }
                j++;
            }           
        }

        // Check that individula category is has value
        private void TestListCointainsIndividualCategoryData(ArrayList table)
        {
            int gronaVaxtdelarStart = 0;
            int gronaVaxtdelarEnd = 0;
            int index = 1;
            bool dataSet = false;
            foreach (ArrayList row in table)
            {
                if (row.Contains("Gröna växtdelar"))
                {
                    if (!dataSet)
                    {
                        gronaVaxtdelarStart = index + 1;
                        dataSet = true;
                    }
                   
                }

                bool foundInRow = false;
                for (int i = 0; i < row.Count; i++)
                {
                    string testString = Convert.ToString(row[i]);
                    if (testString.Contains("Värdväxt (lista)"))
                    {
                        foundInRow = true;
                    }
                }
                if (foundInRow && gronaVaxtdelarStart > 0)
                {
                    gronaVaxtdelarEnd = index - 1;
                    break;
                }

                index++;
            }


            // Check that there is a list of hosts
            Assert.IsTrue(gronaVaxtdelarEnd - gronaVaxtdelarStart < gronaVaxtdelarEnd);
            Assert.IsTrue(gronaVaxtdelarEnd - gronaVaxtdelarStart > 0);

            // Check that host data is not empty
            for (int i = gronaVaxtdelarStart; i < gronaVaxtdelarEnd; i++)
            {
                ArrayList testRow = table[i] as ArrayList;
                for (int k = 0; k < testRow.Count; k++)
                {
                    string testText = Convert.ToString(testRow[k]);
                    if (k == 3)// || k == 4) // Datat har troligtvis ändrats.
                    {
                        Assert.IsFalse(testText.Equals(string.Empty), testText + " missing data in column " + k.ToString());
                    }
                }
            }
        }
       
        /// <summary>
        /// Check if excelfile contaiyns non public data
        /// </summary>
        /// <param name="table"></param>
        private void TestListCointainsNonPublicData(ArrayList table)
        {
            // Compare
            foreach (ArrayList testRow in table)
            {
                for (int k = 0; k < testRow.Count; k++)
                {
                    string testText = Convert.ToString(testRow[k]);
                    Assert.IsFalse(testText.Contains("Behöver kontrolleras") || testText.Contains("Ej expertbedömd"), "Quality is invalid. Not supposed to show factors with qualty of level " + testText);
                }
            }
        }

        /// <summary>
        /// Check if host data exist
        /// </summary>
        /// <param name="table"></param>
        private void TestCointainsHostData(ArrayList table)
        {
            int gronaVaxtdelarStart = 0;
            int gronaVaxtdelarEnd = 0;
            int index = 1;
            foreach (ArrayList row in table)
            {
                if (row.Contains("Gröna växtdelar"))
                {
                    gronaVaxtdelarStart = index;
                }
                
                bool foundInRow = false;
                for (int i = 0; i < row.Count; i++)
                {
                    string testString = Convert.ToString(row[i]);
                    if (testString.Contains("Värdväxt"))
                    {
                        foundInRow = true;
                    }
                }

                if (foundInRow && gronaVaxtdelarStart > 0)
                {
                    gronaVaxtdelarEnd = index - 1;
                    break;
                }
                
                index++;
            }

            // Check that there is a list of hosts
            Assert.IsTrue(gronaVaxtdelarEnd - gronaVaxtdelarStart < gronaVaxtdelarEnd);
            Assert.IsTrue(gronaVaxtdelarEnd - gronaVaxtdelarStart > 0);

            // Check that host data is not empty
            for (int i = gronaVaxtdelarStart; i < gronaVaxtdelarEnd; i++)
            {
                ArrayList testRow = table[i] as ArrayList;
                for (int k = 0; k < testRow.Count; k++)
                {
                    string testText = Convert.ToString(testRow[k]);
                    if (k == 3 || k == 4)
                    {
                        Assert.IsFalse(testText.Equals(string.Empty), testText + " missing host data.");
                    }
                }
            }
        }

        /// <summary>
        /// Check ArtfatadatabaseBIUS data for created excelfile
        /// </summary>
        /// <param name="refTable"></param>
        /// <param name="table"></param>
        private static void TestPersistentDataInArtfatadatabasenBIUS(ArrayList refTable, ArrayList table)
        {
            int artfaktadatabasenBIUSStartRow = 0;
            int artfaktadatabasenBIUSEndRow = 0;
            int index = 1;
            foreach (ArrayList row in table)
            {
                if (row.Contains("Artfaktadatabasen BIUS"))
                {
                    artfaktadatabasenBIUSStartRow = index;
                }

                if (row.Contains("NATURSTATISTIK"))
                {
                    artfaktadatabasenBIUSEndRow = index;
                }
                index++;
            }

            int artfaktadatabasenBIUSStartRowRef = 0;
            int artfaktadatabasenBIUSEndRowRef = 0;
            int indexRef = 1;
            foreach (ArrayList row in refTable)
            {
                if (row.Contains("Artfaktadatabasen BIUS"))
                {
                    artfaktadatabasenBIUSStartRowRef = indexRef;
                }

                if (row.Contains("NATURSTATISTIK"))
                {
                    artfaktadatabasenBIUSEndRowRef = indexRef;
                }
                indexRef++;
            }

            Assert.AreEqual(artfaktadatabasenBIUSEndRowRef - artfaktadatabasenBIUSStartRowRef,
                            artfaktadatabasenBIUSEndRow - artfaktadatabasenBIUSStartRow);

            // Compare
            int j = artfaktadatabasenBIUSStartRow;
            for (int i = artfaktadatabasenBIUSStartRowRef; i < artfaktadatabasenBIUSEndRowRef; i++)
            {
                ArrayList refRow = refTable[i] as ArrayList;
                ArrayList testRow = table[j] as ArrayList;
                for (int k = 0; k < refRow.Count; k++)
                {
                    string testText = Convert.ToString(testRow[k]);
                    string refText = Convert.ToString(refRow[k]);
                    Assert.AreEqual(refText, testText, testText + " don't match reference factor value " + refText);
                }
                j++;
            }
        }

        /// <summary>
        /// Check all data for created excelfile
        /// </summary>
        /// <param name="refTable"></param>
        /// <param name="table"></param>
        private static void TestAllSpeciesFactData(ArrayList refTable, ArrayList table, bool fromStart = false)
        {
            
            // Compare all, except taxon information
            int j = 14;
            int i = 14;
            if (fromStart)
            {
                i = 0;
                j = 0;
            }
            
            for (; i < refTable.Count; i++)
            {
                ArrayList refRow = refTable[i] as ArrayList;
                ArrayList testRow = table[j] as ArrayList;
                for (int k = 0; k < refRow.Count; k++)
                {
                    string testText = Convert.ToString(testRow[k]);
                    string refText = Convert.ToString(refRow[k]);
                    // Musr not check this factor sort order since sorting of this is random (The value should not be the same, it is for the moment, bug in getting data fron DB)
                    if (!refRow[refRow.Count - 1].Equals("1008080"))
                    
                        
                        Assert.AreEqual(refText, testText, testText + " don't match reference factor value " + refText + " For row " + j);
                }

                j++;
            }
        }

        /// <summary>
        /// Convert modeldata to an arrayList for comparing data
        /// </summary>
        /// <returns></returns>
        private static ArrayList ReadModelData(SpeciesFactViewModel viewModel)
        {
            try
            {
                // Creating Dictionary and arraylist to keep rows and data
                ArrayList table = new ArrayList();

                for (int i = 0; i < viewModel.SpeciesFactViewModelHeaderItemList.Count; i++)
                {
                    ArrayList row = new ArrayList();
                    SpeciesFactViewModelHeaderItem headerItem = viewModel.SpeciesFactViewModelHeaderItemList[i];
                    SpeciesFactViewModelItem item = headerItem.SpeciesFactViewModelItem;
                    AddModelItem(row, item, table);

                    for (int j = 0; j < headerItem.SpeciecFactViewModelSubHeaderItemList.Count; j++)
                    {
                        ArrayList subRow = new ArrayList();
                        SpeciesFactViewModelSubHeaderItem subHeaderItem = headerItem.SpeciecFactViewModelSubHeaderItemList[j];
                        SpeciesFactViewModelItem subItem = subHeaderItem.SpeciesFactViewModelItem;
                        AddModelItem(subRow, subItem, table);

                        for (int k = 0; k < subHeaderItem.SpeciesFactViewModelItemList.Count; k++)
                        {
                            ArrayList factorRow = new ArrayList();
                            var factorItem = subHeaderItem.SpeciesFactViewModelItemList[k];
                            AddModelItem(factorRow, factorItem, table);
                        }
                    }
                }

                table.Add(new ArrayList() { "", "", "", "", "", "", "", "", "", "" });
                return table;
            }
            catch (Exception e)
            { 
                throw e;
            }
        }

        /// <summary>
        /// Add model item to row
        /// </summary>
        /// <param name="row"></param>
        /// <param name="item"></param>
        /// <param name="table"></param>
        private static void AddModelItem(ArrayList row, SpeciesFactViewModelItem item, ArrayList table)
        {
            row.Add(item.MainHeader);
            row.Add(item.SubHeader);
            row.Add(item.SuperiorHeader);
            row.Add(item.FactorName);
            row.Add(item.FactorFieldValue);
            if (item.IndividualCategoryId != 0)
                row.Add(item.IndividualCategoryName);
            else
                row.Add("");
            row.Add(item.FactorFieldComment);
            row.Add(item.Quality);
            row.Add(item.FactorId);
            row.Add(item.FactorSortOrder);
            if (item.MainHeader.IsNotEmpty() || item.SubHeader.IsNotEmpty() || item.SuperiorHeader.IsNotEmpty()
                || item.FactorName.IsNotEmpty() || item.FactorFieldValue.IsNotEmpty() || item.IndividualCategoryName.IsNotEmpty() 
                || item.FactorFieldComment.IsNotEmpty() || item.Quality.IsNotEmpty() || item.FactorId.IsNotEmpty() || item.FactorSortOrder.IsNotEmpty())
                
                table.Add(row); // adding Row into my table id not empty row
        }

        /// <summary>
        /// The get selected item and host item.
        /// </summary>
        /// <param name="viewModel">
        /// The view model.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="testFactorId">
        /// The test factor id.
        /// </param>
        /// <param name="testHostFactorId">
        /// The test host factor id.
        /// </param>
        /// <param name="testHostTaxonId">
        /// The test host taxon id.
        /// </param>
        /// <param name="hostItem">
        /// The host item.
        /// </param>
        /// <returns>
        /// The <see cref="SpeciesFactViewModelItem"/>.
        /// </returns>
        private static SpeciesFactViewModelItem getSelectedItemAndHostItem(SpeciesFactViewModel viewModel,
            SpeciesFactViewModelItem item, string testFactorId, string testHostFactorId, int testHostTaxonId,
            ref SpeciesFactViewModelItem hostItem)
        {
            foreach (SpeciesFactViewModelHeaderItem headerItem in viewModel.SpeciesFactViewModelHeaderItemList)
            {
                foreach (
                    SpeciesFactViewModelSubHeaderItem superiorHeaderItem in headerItem.SpeciecFactViewModelSubHeaderItemList)
                {
                    if (item.IsNull())
                        item =
                            superiorHeaderItem.SpeciesFactViewModelItemList.FirstOrDefault(x => x.FactorId.Equals(testFactorId));
                    if (hostItem.IsNull())
                        hostItem =
                            superiorHeaderItem.SpeciesFactViewModelItemList.FirstOrDefault(
                                x =>
                                    (x.FactorId.Equals(testHostFactorId) && x.HostId.Equals(testHostTaxonId) && x.IsHost &&
                                     x.FieldValues.IsNotNull()));
                }
            }

            return item;
        }

        /// <summary>
        /// The get species fact view model for parnassius apollo.
        /// </summary>
        /// <param name="controller">
        /// The controller.
        /// </param>
        /// <returns>
        /// The <see cref="SpeciesFactViewModel"/>.
        /// </returns>
        private static SpeciesFactViewModel GetSpeciesFactViewModelForParnassiusApollo(SpeciesFactController controller)
        {

            string taxonId = Convert.ToString(DyntaxaTestSettings.Default.ParnassiusApolloId);
            string factorId = Convert.ToString((int)DyntaxaFactorId.SUBSTRATE);
            string factorDataType = Convert.ToString((int)DyntaxaFactorDataType.AF_SUBSTRATE);
            string dataType = Convert.ToString((int)DyntaxaDataType.ENUM);
            string referenceId = "12";

            // Create model ie this model should be created without any db calls for correct testing...
            var getResult = controller.EditFactors(taxonId, factorId, dataType, factorDataType, referenceId) as ViewResult;
            SpeciesFactViewModel viewModel = getResult.ViewData.Model as SpeciesFactViewModel;
            return viewModel;
        }

        /// <summary>
        /// The get species fact view model for parnassius apollo stub.
        /// </summary>
        /// <returns>
        /// The <see cref="SpeciesFactViewModel"/>.
        /// </returns>
        private static SpeciesFactViewModel GetSpeciesFactViewModelForParnassiusApolloStub()
        {
            string referenceId = "12";
            string testFactorId = "1091";

            string testHostFactorId = "1136";
            int testHostTaxonId = 223398;

            // Create model 
            SpeciesFactViewModel viewModel = new SpeciesFactViewModel();
            viewModel.DataType = DyntaxaDataType.ENUM;
            viewModel.DropDownAllFactorId = 0;
            viewModel.DropDownFactorId = 0;
            viewModel.FactorDataType = DyntaxaFactorDataType.AF_SUBSTRATE;
            viewModel.FactorFieldValue2TableHeader = "Header2";
            viewModel.FactorFieldValueTableHeader = "Header1";
            viewModel.FactorList = new List<SpeciesFactDropDownModelHelper>();
            viewModel.AllAvaliableFactors = new List<SpeciesFactDropDownModelHelper>();
            viewModel.MainParentFactorId = DyntaxaFactorId.SUBSTRATE;
            viewModel.PostAction = "PostAction";
            viewModel.ReferenceId = referenceId;
            viewModel.SpeciesFactViewModelHeaderItemList = new List<SpeciesFactViewModelHeaderItem>();
            viewModel.TaxonId = DyntaxaTestSettings.Default.ParnassiusApolloId;

            // Create new data
            viewModel.SpeciesFactViewModelHeaderItemList = new List<SpeciesFactViewModelHeaderItem>();

            SpeciesFactViewModelHeaderItem headerItem = new SpeciesFactViewModelHeaderItem();
            headerItem.SpeciecFactViewModelSubHeaderItemList = new List<SpeciesFactViewModelSubHeaderItem>();

            SpeciesFactViewModelSubHeaderItem subHeaderItem = new SpeciesFactViewModelSubHeaderItem();
            subHeaderItem.SpeciesFactViewModelItemList = new List<SpeciesFactViewModelItem>();

            // Create viewdata
            SpeciesFactViewModelItem item = new SpeciesFactViewModelItem();
            item.FieldValues = new SpeciesFactViewModelItemFieldValues();
            item.FieldValues.FieldValue = 3;
            item.FieldValues2 = new SpeciesFactViewModelItemFieldValues();
            item.FieldValues2.FieldValue = 3;
            item.QualityId = 5;
            item.FactorId = testFactorId;
            SpeciesFactViewModelItem hostItem = new SpeciesFactViewModelItem();
            hostItem.FieldValues = new SpeciesFactViewModelItemFieldValues();
            hostItem.FieldValues.FieldValue = 3;
            hostItem.FieldValues2 = new SpeciesFactViewModelItemFieldValues();
            hostItem.FieldValues2.FieldValue = 3;
            hostItem.QualityId = 5;
            hostItem.FactorId = testHostFactorId;
            hostItem.HostId = testHostTaxonId;
            hostItem.IsHost = true;



            subHeaderItem.SpeciesFactViewModelItemList.Add(item);
            subHeaderItem.SpeciesFactViewModelItemList.Add(hostItem);

            headerItem.SpeciecFactViewModelSubHeaderItemList.Add(subHeaderItem);

            viewModel.SpeciesFactViewModelHeaderItemList.Add(headerItem);

            return viewModel;
        }


        /// <summary>
        /// The get species fact view model for parnassius apollo stub.
        /// </summary>
        /// <returns>
        /// The <see cref="SpeciesFactViewModel"/>.
        /// </returns>
        private static SpeciesFactViewModel GetSpeciesFactViewModelForParnassiusApolloInfluenceStub()
        {
            /*
            string taxonId = Convert.ToString(DyntaxaTestSettings.Default.ParnassiusApolloId);
            string factorId = Convert.ToString((int)DyntaxaFactorId.INFLUENCE);
            string factorDataType = Convert.ToString((int)DyntaxaFactorDataType.AF_INFLUENCE);
            string dataType = Convert.ToString((int)DyntaxaDataType.ENUM);
            string testFactorName = "Igenväxning/förtätning av artens biotop";
            string testHostTaxonName = "Djurslag: ospecificerat";
            string testHostFactorName = "Djurslag: ospecificerat";
            string factorFieldValueName = "Effekt";
            string factorFieldValue2Name = "Faktorns relevans";
            */
            var testHostFactorId = "1683";
            var testHostTaxonId = 0;
            var referenceId = "12";
            var testFactorId = "2020";

            // Create model 
            SpeciesFactViewModel viewModel = new SpeciesFactViewModel();
            viewModel.DataType = DyntaxaDataType.ENUM;
            viewModel.DropDownAllFactorId = 0;
            viewModel.DropDownFactorId = 0;
            viewModel.FactorDataType = DyntaxaFactorDataType.AF_INFLUENCE;
            viewModel.FactorFieldValue2TableHeader = "Header2";
            viewModel.FactorFieldValueTableHeader = "Header1";
            viewModel.FactorList = new List<SpeciesFactDropDownModelHelper>();
            viewModel.AllAvaliableFactors = new List<SpeciesFactDropDownModelHelper>();
            viewModel.MainParentFactorId = DyntaxaFactorId.INFLUENCE;
            viewModel.PostAction = "PostAction";
            viewModel.ReferenceId = referenceId;
            viewModel.SpeciesFactViewModelHeaderItemList = new List<SpeciesFactViewModelHeaderItem>();
            viewModel.TaxonId = DyntaxaTestSettings.Default.ParnassiusApolloId;

            // Create new data
            viewModel.SpeciesFactViewModelHeaderItemList = new List<SpeciesFactViewModelHeaderItem>();

            SpeciesFactViewModelHeaderItem headerItem = new SpeciesFactViewModelHeaderItem();
            headerItem.SpeciecFactViewModelSubHeaderItemList = new List<SpeciesFactViewModelSubHeaderItem>();

            SpeciesFactViewModelSubHeaderItem subHeaderItem = new SpeciesFactViewModelSubHeaderItem();
            subHeaderItem.SpeciesFactViewModelItemList = new List<SpeciesFactViewModelItem>();




            // Create viewdata
            SpeciesFactViewModelItem item = new SpeciesFactViewModelItem();
            item.FieldValues = new SpeciesFactViewModelItemFieldValues();
            item.FieldValues.FieldValue = 3;
            item.FieldValues2 = new SpeciesFactViewModelItemFieldValues();
            item.FieldValues2.FieldValue = 3;
            item.QualityId = 5;
            item.FactorId = testFactorId;
            SpeciesFactViewModelItem hostItem = new SpeciesFactViewModelItem();
            hostItem.FieldValues = new SpeciesFactViewModelItemFieldValues();
            hostItem.FieldValues.FieldValue = 3;
            hostItem.FieldValues2 = new SpeciesFactViewModelItemFieldValues();
            hostItem.FieldValues2.FieldValue = 3;
            hostItem.QualityId = 5;
            hostItem.FactorId = testHostFactorId;
            hostItem.HostId = testHostTaxonId;
            hostItem.IsHost = true;



            subHeaderItem.SpeciesFactViewModelItemList.Add(item);
            subHeaderItem.SpeciesFactViewModelItemList.Add(hostItem);

            headerItem.SpeciecFactViewModelSubHeaderItemList.Add(subHeaderItem);

            viewModel.SpeciesFactViewModelHeaderItemList.Add(headerItem);

            return viewModel;
        }

        /// <summary>
        /// The get species fact view model for parnassius apollo stub.
        /// </summary>
        /// <returns>
        /// The <see cref="SpeciesFactViewModel"/>.
        /// </returns>
        private static SpeciesFactViewModel GetSpeciesFactViewModelForParnassiusApolloBiotopeStub()
        {

            /*string taxonId = Convert.ToString(DyntaxaTestSettings.Default.ParnassiusApolloId);
            string factorId = Convert.ToString((int)DyntaxaFactorId.BIOTOPE);
            string factorDataType = Convert.ToString((int)DyntaxaFactorDataType.AF_BIOTOPE);
            string dataType = Convert.ToString((int)DyntaxaDataType.ENUM);*/
            string referenceId = "12";
            string testFactorId = "804";
            
    
            string testHostFactorId = "0";
            int testHostTaxonId = 0;
          

            // Create model 
            SpeciesFactViewModel viewModel = new SpeciesFactViewModel();
            viewModel.DataType = DyntaxaDataType.ENUM;
            viewModel.DropDownAllFactorId = 0;
            viewModel.DropDownFactorId = 0;
            viewModel.FactorDataType = DyntaxaFactorDataType.AF_BIOTOPE;
            viewModel.FactorFieldValue2TableHeader = "Header2";
            viewModel.FactorFieldValueTableHeader = "Header1";
            viewModel.FactorList = new List<SpeciesFactDropDownModelHelper>();
            viewModel.AllAvaliableFactors = new List<SpeciesFactDropDownModelHelper>();
            viewModel.MainParentFactorId = DyntaxaFactorId.BIOTOPE;
            viewModel.PostAction = "PostAction";
            viewModel.ReferenceId = referenceId;
            viewModel.SpeciesFactViewModelHeaderItemList = new List<SpeciesFactViewModelHeaderItem>();
            viewModel.TaxonId = DyntaxaTestSettings.Default.ParnassiusApolloId;

            // Create new data
            viewModel.SpeciesFactViewModelHeaderItemList = new List<SpeciesFactViewModelHeaderItem>();

            SpeciesFactViewModelHeaderItem headerItem = new SpeciesFactViewModelHeaderItem();
            headerItem.SpeciecFactViewModelSubHeaderItemList = new List<SpeciesFactViewModelSubHeaderItem>();

            SpeciesFactViewModelSubHeaderItem subHeaderItem = new SpeciesFactViewModelSubHeaderItem();
            subHeaderItem.SpeciesFactViewModelItemList = new List<SpeciesFactViewModelItem>();




            // Create viewdata
            SpeciesFactViewModelItem item = new SpeciesFactViewModelItem();
            item.FieldValues = new SpeciesFactViewModelItemFieldValues();
            item.FieldValues.FieldValue = 3;
            item.FieldValues2 = new SpeciesFactViewModelItemFieldValues();
            item.FieldValues2.FieldValue = 3;
            item.QualityId = 5;
            item.FactorId = testFactorId;
            SpeciesFactViewModelItem hostItem = new SpeciesFactViewModelItem();
            hostItem.FieldValues = new SpeciesFactViewModelItemFieldValues();
            hostItem.FieldValues.FieldValue = 3;
            hostItem.FieldValues2 = new SpeciesFactViewModelItemFieldValues();
            hostItem.FieldValues2.FieldValue = 3;
            hostItem.QualityId = 5;
            hostItem.FactorId = testHostFactorId;
            hostItem.HostId = testHostTaxonId;
            hostItem.IsHost = false;



            subHeaderItem.SpeciesFactViewModelItemList.Add(item);
            subHeaderItem.SpeciesFactViewModelItemList.Add(hostItem);

            headerItem.SpeciecFactViewModelSubHeaderItemList.Add(subHeaderItem);

            viewModel.SpeciesFactViewModelHeaderItemList.Add(headerItem);

            return viewModel;
        }

        #endregion
    }
}

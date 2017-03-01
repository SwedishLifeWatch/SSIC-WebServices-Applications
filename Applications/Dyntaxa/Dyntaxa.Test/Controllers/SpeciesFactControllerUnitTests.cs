using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.Data.Fakes;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;
using Dyntaxa.Controllers;
using Dyntaxa.Test.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Dyntaxa.Test.Controllers
{
    using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;

    using Dyntaxa.Test;

    using Microsoft.QualityTools.Testing.Fakes;

    [TestClass]
    public class SpeciesFactControllerUnitTests : ControllerUnitTestBase
    {
        #region Tests

        /// <summary>
        /// Get test for add factor TODO Test needes more asserts... And correct redirects
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril).
        /// </summary>
        [TestMethod]   
        [Ignore]
        [TestCategory("UnitTestApp")]
        public void AddFactorPostTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                ShimSpeciesFactModelManager();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "EditFactors";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);
                Transaction = new ShimTransaction()
                {
                    Commit = () => { return; },
                };

                // Arrange
                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;

                userContext.User.PersonId = DyntaxaTestSettings.Default.TestApplicationUserId;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;


                // Create model....
                SpeciesFactViewModel viewModel = GetSpeciesFactViewModelForParnassiusApolloBiotopeStub();
                viewModel.TaxonId = DyntaxaTestSettings.Default.ParnassiusApolloId;
                var result = controller.AddFactor(viewModel) as RedirectToRouteResult;

                // Assert
                Assert.IsNotNull(result);

                // Test that redirect is working ok.
                Assert.AreEqual("AddFactor", result.RouteValues["action"]);
                Assert.AreEqual("SpeciesFact", result.RouteValues["controller"]);
            }
        }

        /// <summary>
        /// Get test for add fom all avaliable factors TODO Test needes more asserts...And correct redirects
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril).
        /// </summary>
        [TestMethod]
        [Ignore]
        [TestCategory("UnitTestApp")]
        public void AddFromAllAvaliableFactorPostTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                ShimSpeciesFactModelManager();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "EditFactors";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);
                Transaction = new ShimTransaction()
                {
                    Commit = () => { return; },
                };

                // Arrange
                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;

                userContext.User.PersonId = DyntaxaTestSettings.Default.TestApplicationUserId;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;


                // Create model....
                SpeciesFactViewModel viewModel = GetSpeciesFactViewModelForParnassiusApolloBiotopeStub();
                viewModel.TaxonId = DyntaxaTestSettings.Default.ParnassiusApolloId;
                var result = controller.AddFromAllAvaliableFactor(viewModel) as RedirectToRouteResult;

                // Assert
                Assert.IsNotNull(result);

                // Test that redirect is working ok.
                Assert.AreEqual("AddFromAllAvaliableFactor", result.RouteValues["action"]);
                Assert.AreEqual("SpeciesFact", result.RouteValues["controller"]);
            }
        }

        /// <summary>
        /// Get test for add host taxon and factor TODO Test needes more asserts...And correct redirects
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril).
        /// </summary>
        [TestMethod]
        [Ignore]
        [TestCategory("UnitTestApp")]
        public void AddHostTaxonAndFactorPostTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                ShimSpeciesFactModelManager();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "EditFactors";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);
                Transaction = new ShimTransaction()
                {
                    Commit = () => { return; },
                };

                // Arrange
                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;

                userContext.User.PersonId = DyntaxaTestSettings.Default.TestApplicationUserId;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;


                // Create model....
                SpeciesFactHostViewModel viewModel = new SpeciesFactHostViewModel();
                viewModel.TaxonId = DyntaxaTestSettings.Default.ParnassiusApolloId;
                var result = controller.AddHostTaxonAndFactor(viewModel) as RedirectToRouteResult;

                // Assert
                Assert.IsNotNull(result);

                // Test that redirect is working ok.
                Assert.AreEqual("AddHostTaxonAndFactor", result.RouteValues["action"]);
                Assert.AreEqual("SpeciesFact", result.RouteValues["controller"]);
            }
        }


        /// <summary>
        /// Get test for add host taxon and factor TODO Test needes more asserts...And correct redirects
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril).
        /// </summary>
        [TestMethod]
        [Ignore]
        [TestCategory("UnitTestApp")]
        public void DeleteHostTaxonPostTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                ShimSpeciesFactModelManager();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "EditFactors";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);
                Transaction = new ShimTransaction()
                {
                    Commit = () => { return; },
                };

                // Arrange
                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;

                userContext.User.PersonId = DyntaxaTestSettings.Default.TestApplicationUserId;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;


                // Create input
                int taxonId = DyntaxaTestSettings.Default.ParnassiusApolloId;
                int hostTaxonId = 88;
                int hostFactorId = (int)DyntaxaFactorId.BIOTOPE;
                int factorId = 567;
                int categoryId = 0;
                int referenceId = 10;
                var result = controller.DeleteHostTaxonItem(hostTaxonId, factorId, taxonId, referenceId, categoryId, hostFactorId) as RedirectToRouteResult;

                // Assert
                Assert.IsNotNull(result);

                // Test that redirect is working ok.
                Assert.AreEqual("DeleteHostTaxonItem", result.RouteValues["action"]);
                Assert.AreEqual("SpeciesFact", result.RouteValues["controller"]);
            }
        }

        /// <summary>
        /// Get test for add host taxon and factor TODO Test needes more asserts...And correct redirects
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril).
        /// </summary>
        [TestMethod]
        [Ignore]
        [TestCategory("UnitTestApp")]
        public void DeleteFactorPostTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                ShimSpeciesFactModelManager();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "EditFactors";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);
                Transaction = new ShimTransaction()
                {
                    Commit = () => { return; },
                };

                // Arrange
                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;

                userContext.User.PersonId = DyntaxaTestSettings.Default.TestApplicationUserId;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;


                // Create input
                int taxonId = DyntaxaTestSettings.Default.ParnassiusApolloId;
                int hostFactorId = (int)DyntaxaFactorId.BIOTOPE;
                int factorId = 567;
                int categoryId = 0;
                int referenceId = 10;
                var result = controller.DeleteHostFactorItem(hostFactorId, taxonId, factorId, referenceId, categoryId) as RedirectToRouteResult;

                // Assert
                Assert.IsNotNull(result);

                // Test that redirect is working ok.
                Assert.AreEqual("DeleteHostFactorItem", result.RouteValues["action"]);
                Assert.AreEqual("SpeciesFact", result.RouteValues["controller"]);
            }
        }

        /// <summary>
        /// Get test for Edit factors
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril).
        /// </summary>
        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void EditFactorsForSubstratePostTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                ShimSpeciesFactModelManager();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "EditFactors";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);
                Transaction = new ShimTransaction()
                {
                    Commit = () => { return; },
                };

                // Arrange
                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;

                userContext.User.PersonId = DyntaxaTestSettings.Default.TestApplicationUserId;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;

                string taxonId = Convert.ToString(DyntaxaTestSettings.Default.ParnassiusApolloId);
                string factorId = Convert.ToString((int)DyntaxaFactorId.SUBSTRATE);
                string factorDataType = Convert.ToString((int)DyntaxaFactorDataType.AF_SUBSTRATE);
                string dataType = Convert.ToString((int)DyntaxaDataType.ENUM);
                string referenceId = "12";
           

                // Create model....
                SpeciesFactViewModel viewModel = GetSpeciesFactViewModelForParnassiusApolloStub();
                viewModel.TaxonId = DyntaxaTestSettings.Default.ParnassiusApolloId;
                var result = controller.EditFactors(viewModel) as RedirectToRouteResult;

                // Assert
                Assert.IsNotNull(result);

                // Test that redirect is working ok.
                Assert.AreEqual("EditFactors", result.RouteValues["action"]);
                Assert.AreEqual("SpeciesFact", result.RouteValues["controller"]);
                Assert.AreEqual(taxonId, result.RouteValues["taxonId"]);
                Assert.AreEqual(factorId, result.RouteValues["factorId"]);
                Assert.AreEqual(factorDataType, result.RouteValues["factorDataType"]);
                Assert.AreEqual(dataType, result.RouteValues["dataType"]);
                Assert.AreEqual(referenceId, result.RouteValues["referenceId"]);
           }
        }

        /// <summary>
        /// Get test for Edit factors
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril).
        /// </summary>
        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void EditFactorsForInfluencePostTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                ShimSpeciesFactModelManager();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "EditFactors";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);
                Transaction = new ShimTransaction()
                {
                    Commit = () => { return; },
                };

                // Arrange
                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;

                userContext.User.PersonId = DyntaxaTestSettings.Default.TestApplicationUserId;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;

                string taxonId = Convert.ToString(DyntaxaTestSettings.Default.ParnassiusApolloId);
                string factorId = Convert.ToString((int)DyntaxaFactorId.INFLUENCE);
                string factorDataType = Convert.ToString((int)DyntaxaFactorDataType.AF_INFLUENCE);
                string dataType = Convert.ToString((int)DyntaxaDataType.ENUM);
                string referenceId = "12";


                // Create model....
                SpeciesFactViewModel viewModel = GetSpeciesFactViewModelForParnassiusApolloInfluenceStub();
                viewModel.TaxonId = DyntaxaTestSettings.Default.ParnassiusApolloId;
                var result = controller.EditFactors(viewModel) as RedirectToRouteResult;

                // Assert
                Assert.IsNotNull(result);

                // Test that redirect is working ok.
                Assert.AreEqual("EditFactors", result.RouteValues["action"]);
                Assert.AreEqual("SpeciesFact", result.RouteValues["controller"]);
                Assert.AreEqual(taxonId, result.RouteValues["taxonId"]);
                Assert.AreEqual(factorId, result.RouteValues["factorId"]);
                Assert.AreEqual(factorDataType, result.RouteValues["factorDataType"]);
                Assert.AreEqual(dataType, result.RouteValues["dataType"]);
                Assert.AreEqual(referenceId, result.RouteValues["referenceId"]);
            }
        } 

        /// <summary>
        /// Get test for Edit factors
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril).
        /// </summary>
        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void EditFactorsForHabitatePostTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                ShimSpeciesFactModelManager();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "EditFactors";                
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);
                Transaction = new ShimTransaction()
                {
                    Commit = () => { return; },
                };

                // Arrange
                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;

                userContext.User.PersonId = DyntaxaTestSettings.Default.TestApplicationUserId;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;

                string taxonId = Convert.ToString(DyntaxaTestSettings.Default.ParnassiusApolloId);
                string factorId = Convert.ToString((int)DyntaxaFactorId.BIOTOPE);
                string factorDataType = Convert.ToString((int)DyntaxaFactorDataType.AF_BIOTOPE);
                string dataType = Convert.ToString((int)DyntaxaDataType.ENUM);
                string referenceId = "12";


                // Create model....
                SpeciesFactViewModel viewModel = GetSpeciesFactViewModelForParnassiusApolloBiotopeStub();
                viewModel.TaxonId = DyntaxaTestSettings.Default.ParnassiusApolloId;
                var result = controller.EditFactors(viewModel) as RedirectToRouteResult;

                // Assert
                Assert.IsNotNull(result);

                // Test that redirect is working ok.
                Assert.AreEqual("EditFactors", result.RouteValues["action"]);
                Assert.AreEqual("SpeciesFact", result.RouteValues["controller"]);
                Assert.AreEqual(taxonId, result.RouteValues["taxonId"]);
                Assert.AreEqual(factorId, result.RouteValues["factorId"]);
                Assert.AreEqual(factorDataType, result.RouteValues["factorDataType"]);
                Assert.AreEqual(dataType, result.RouteValues["dataType"]);
                Assert.AreEqual(referenceId, result.RouteValues["referenceId"]);
            }
        }

        /// <summary>
        /// Get test for Edit host factors TODO Test needes more asserts...And correct redirects
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril)
        /// </summary>
        [TestMethod]
        [Ignore]
        [TestCategory("UnitTestApp")]
        public void EditHostFactorsPostTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                ShimSpeciesFactModelManager();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "EditHostFactors";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);
                Transaction = new ShimTransaction()
                {
                    Commit = () => { return; },
                };

                // Arrange
                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;

                userContext.User.PersonId = DyntaxaTestSettings.Default.TestApplicationUserId;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;

                // Create model....
                SpeciesFactViewModel viewModel = GetSpeciesFactViewModelForParnassiusApolloStub();
                viewModel.TaxonId = DyntaxaTestSettings.Default.ParnassiusApolloId;
                var result = controller.EditHostFactors(viewModel) as RedirectToRouteResult;

                // Assert
                Assert.IsNotNull(result);

                // Test that redirect is working ok.
                Assert.AreEqual("EditHostFactorsForSubstrate", result.RouteValues["action"]);
                Assert.AreEqual("SpeciesFact", result.RouteValues["controller"]); 
            }
        }

        /// <summary>
        /// Get test for Edit host factor items TODO Test needes more asserts...And correct redirects
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril).
        /// </summary>
        [TestMethod]
        [Ignore]
        [TestCategory("UnitTestApp")]
        public void EditHostFactorItemsPostTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                ShimSpeciesFactModelManager();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "EditFactors";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);
                Transaction = new ShimTransaction()
                {
                    Commit = () => { return; },
                };

                // Arrange
                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;

                userContext.User.PersonId = DyntaxaTestSettings.Default.TestApplicationUserId;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;

                // Create model....
                FactorViewModel viewModel = new FactorViewModel();
                viewModel.TaxonId = DyntaxaTestSettings.Default.ParnassiusApolloId;
                var result = controller.EditHostFactorItems(viewModel) as RedirectToRouteResult;

                // Assert
                Assert.IsNotNull(result);

                // Test that redirect is working ok.
                Assert.AreEqual("EditHostFactorItems", result.RouteValues["action"]);
                Assert.AreEqual("SpeciesFact", result.RouteValues["controller"]);
            }
        }

        /// <summary>
        /// Get test for Edit host factor item TODO Test needes more asserts...And correct redirects
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril).
        /// </summary>
        [TestMethod]
        [Ignore]
        [TestCategory("UnitTestApp")]
        public void EditHostFactorItemPostTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                ShimSpeciesFactModelManager();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "EditFactors";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);
                Transaction = new ShimTransaction()
                {
                    Commit = () => { return; },
                };

                // Arrange
                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;

                userContext.User.PersonId = DyntaxaTestSettings.Default.TestApplicationUserId;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;



                // Create model....
                FactorViewModel viewModel = new FactorViewModel();
                viewModel.TaxonId = DyntaxaTestSettings.Default.ParnassiusApolloId;
                var result = controller.EditHostFactorItem(viewModel) as RedirectToRouteResult;

                // Assert
                Assert.IsNotNull(result);

                // Test that redirect is working ok.
                Assert.AreEqual("EditHostFactorItem", result.RouteValues["action"]);
                Assert.AreEqual("SpeciesFact", result.RouteValues["controller"]);
            }

        }

        /// <summary>
        /// Get test for Edit factor item TODO Test needes more asserts...And correct redirects
        /// Test is performed on taxon ParnassiusApollo(Apollofjäril).
        /// </summary>
        [TestMethod]
        [Ignore]
        [TestCategory("UnitTestApp")]
        public void EditFactorItemPostTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                SetSwedishLanguage();
                ShimSpeciesFactModelManager();
                SpeciesFactController controller = new SpeciesFactController();
                string controllerName = "SpeciesFact";
                string actionName = "EditFactors";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);
                Transaction = new ShimTransaction()
                {
                    Commit = () => { return; },
                };

                // Arrange
                // Prepare for next test set role for this revision
                IUserContext userContext = ApplicationUserContextSV;

                userContext.User.PersonId = DyntaxaTestSettings.Default.TestApplicationUserId;
                RoleList roleList = new RoleList();
                roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                roleList.Add(UserDataSourceTestRepositoryData.GetSpeciesFactRole("SpeciesFactEditor", 777, userContext));
                userContext.CurrentRoles = roleList;

                // Set user in session
                UserContextData = userContext;


                // Create model....
                FactorViewModel viewModel = new FactorViewModel();
                viewModel.TaxonId = DyntaxaTestSettings.Default.ParnassiusApolloId;
                var result = controller.EditFactorItem(viewModel) as RedirectToRouteResult;

                // Assert
                Assert.IsNotNull(result);

                // Test that redirect is working ok.
                Assert.AreEqual("EditFactorItems", result.RouteValues["action"]);
                Assert.AreEqual("SpeciesFact", result.RouteValues["controller"]);
             
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
            if(!item.IsHost)
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

                //Quality field value must be be set... ie GUI parameter Kvalitet
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
        /// Set the test paths
        /// </summary>
        /// <param name="testRefFilePath"></param>
        /// <returns></returns>
        private  string GetTestFilePath(out string testRefFilePath)
        {
            // Create test file path
            string startupPath = System.IO.Path.GetFullPath(".\\");
            string path = Directory.GetCurrentDirectory();
            string rootDir = Path.GetPathRoot(startupPath);
            string testFilePath = Path.Combine(rootDir, DyntaxaTestSettings.Default.PathToTempDirectory);
          
            SessionHelper.SetInSession("testFilePath", testFilePath);
            testRefFilePath = Path.Combine(startupPath, DyntaxaTestSettings.Default.PathToExcelTestFiles);
            return testFilePath;
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
                    if (k ==4)
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
                    if (k ==4)
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
                    if(!dataSet)
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
                    if (k == 3 || k == 4)
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
            int gronaVaxtdelarEnd= 0;
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
                    if(testString.Contains("Värdväxt"))
                    {
                        foundInRow = true;
                    }
                }

                if(foundInRow && gronaVaxtdelarStart > 0)
                {
                    gronaVaxtdelarEnd = index-1;
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
                    if( k== 3 || k == 4)
                    {
                        Assert.IsFalse( testText.Equals(string.Empty), testText + " missing host data.");
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
            if(fromStart)
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
                    if (!refRow[refRow.Count-1].Equals("1008080"))
                    
                        
                        Assert.AreEqual(refText, testText, testText + " don't match reference factor value " + refText);
                }

                j++;
            }
        }

        [TestMethod]
        public void FixHtmlCodes()
        {
            List<string> htmlChars = new List<string>();
            htmlChars.Add("&#328;");
            htmlChars.Add("&#345;");
            htmlChars.Add("&#279;");
            htmlChars.Add("&#268;");            
            htmlChars.Add("&#8211;");
            htmlChars.Add("&#65533;");
            htmlChars.Add("&#64258;");
            htmlChars.Add("&#64257;");

            Dictionary<string, string> mappings = new Dictionary<string, string>();
            foreach (string htmlChar in htmlChars)
            {
                string htmlDecode = System.Web.HttpUtility.HtmlDecode(htmlChar);
                mappings.Add(htmlChar, htmlDecode);
                Debug.WriteLine("'{0}', '{1}'", htmlChar, htmlDecode);
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
        private static SpeciesFactViewModelItem GetSelectedItemAndHostItem(SpeciesFactViewModel viewModel,
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
            string referenceId = "12";
            string testFactorId = "2020";
           

            string testHostFactorId = "1683";
            int testHostTaxonId = 0;

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

            string taxonId = Convert.ToString(DyntaxaTestSettings.Default.ParnassiusApolloId);
            string factorId = Convert.ToString((int)DyntaxaFactorId.BIOTOPE);
            string factorDataType = Convert.ToString((int)DyntaxaFactorDataType.AF_BIOTOPE);
            string dataType = Convert.ToString((int)DyntaxaDataType.ENUM);
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

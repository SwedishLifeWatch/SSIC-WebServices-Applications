using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using ArtDatabanken.Data;
using ArtDatabanken.IO;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;
using Dyntaxa.Controllers;
using Dyntaxa.Helpers;
using Dyntaxa.Test.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebApplication.Dyntaxa;
using System.Text.RegularExpressions;




namespace Dyntaxa.Test.Controllers
{
    using ArtDatabanken.Data.Fakes;

    using Dyntaxa.Test;

    using Microsoft.QualityTools.Testing.Fakes;

        /// <summary>
        ///  This is a test class for RevisionController and is intended
        ///  to contain all RevisionController Unit Tests
        ///  1. It is recommended to test that model is not null and verfy that some pmodel prperties is set to correct value..
        ///  var result = controller.Initialize(DyntaxaTestSettings.Default.TestTaxonId.ToString()) as ViewResult;
        ///  var model = (RevisionInitializeViewModel)result.ViewData.Model;
        ///  Assert.IsNotNull(result);
        ///  Assert.Equals(model.taxonId, XXX);
        /// Assert.AreEqual("Green", model.AlertLevel);
        /// 2. It is also recomened to test that correct view is returned.
        /// Assert.AreEqual("Initialize", result.ViewName);
        /// 3. If action contains any redirect is is recommened to test them too..
        /// var actionResult = (RedirectToRouteResult)controller.Initialize(DyntaxaTestSettings.Default.TestTaxonId.ToString());
        ///  Assert.AreEqual("Edit", actionResult.RouteValues["action"]);
        /// </summary>
        [TestClass]
        public class RevisionControllerUnitTest: ControllerUnitTestBase
        {
            
            /// <summary>
            /// A test for Add ie create a new revision (Get action)
            /// The test verifies that correct views is returned and that AddRevisonViwewModel have correct values..
            /// </summary>
            [TestMethod]
            [TestCategory("UnitTestApp")]
            [Ignore]
            public void AddGetTest()
            {

                using (ShimsContext.Create())
                {
                    // Arrange
                    LoginApplicationUserAndSetSessionVariables();
                    RevisionController controller = new RevisionController();
                    string controllerName = "Revision";
                    string actionName = "Add";
                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                    ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                    ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                    TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                    SessionRevision = refTaxonRevision;
                    SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                    SessionTaxonId = taxonIdentifier;

                    // Act
                    var addResult = controller.Add(DyntaxaTestSettings.Default.TestTaxonId.ToString()) as ViewResult;

                    // Assert
                    Assert.IsNotNull(addResult);

                    var addViewModel = addResult.ViewData.Model as RevisionAddViewModel;

                    Assert.IsNotNull(addViewModel);

                    // Test that correct view is returned
                    Assert.AreEqual("Add", addResult.ViewName);

                    // Test model values
                    Assert.IsTrue((DateTime.Now.AddYears(1) - addViewModel.ExpectedPublishingDate) < new TimeSpan(0, 0, DyntaxaTestSettings.Default.ComputerTimeDifference));
                    Assert.IsTrue((DateTime.Now - addViewModel.ExpectedStartDate) < new TimeSpan(0, 0, DyntaxaTestSettings.Default.ComputerTimeDifference));
                    Assert.IsTrue(addViewModel.ShowInitalizeButton);
                    Assert.AreEqual(String.Empty, addViewModel.RevisionDescription);
                    Assert.AreEqual("0", addViewModel.RevisionId);
                    Assert.AreEqual(null, addViewModel.SelectedUsers);
                    Assert.AreEqual(refTaxonRevision.RootTaxon.Id.ToString(), addViewModel.TaxonId);
                    Assert.AreEqual(2, addViewModel.UserList.Count);
                    Assert.AreEqual(Resources.DyntaxaSettings.Default.UrlToGetUserAdminUpdateRolesLinkMoneses, addViewModel.UserAdminLink.Url);
                }

            }

            /// <summary>
            /// A test for Add ie create a new revision (Get action)
            /// The test verifies that correct views is returned and error view if taxon in revision is null.
            /// </summary>
            [TestMethod]
            [TestCategory("UnitTestApp")]
            [Ignore]
            public void AddGetTestInvalidData()
            {
                using (ShimsContext.Create())
                {
                    // Arrange
                    LoginApplicationUserAndSetSessionVariables();

                    RevisionController controller = new RevisionController();
                    string controllerName = "Revision";
                    string actionName = "Add";

                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName);
                    //builder.RouteData.Values.Add("controller","Revision");
                    //builder.RouteData.Values.Add("action","Add");

                    ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                    ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                    TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                    SessionRevision = refTaxonRevision;
                    SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                    SessionTaxonId = taxonIdentifier;
                    //Act
                    // Test what happens if invalid taxon ie taxon = null is set in revision. Check that a proper error message is shown
                    var addInvalidResult = controller.Add(DyntaxaTestSettings.Default.FailingTestTaxonId.ToString()) as RedirectToRouteResult;

                    //Assert
                    Assert.IsNotNull(addInvalidResult);
                    // Test that correct view is returned
                    Assert.AreEqual("SearchResult", addInvalidResult.RouteValues["action"]);
                    Assert.AreEqual("Taxon", addInvalidResult.RouteValues["controller"]);
                    Assert.AreEqual("77", addInvalidResult.RouteValues["search"]);
                    Assert.AreEqual("Add", addInvalidResult.RouteValues["returnAction"]);
                    Assert.AreEqual("Revision", addInvalidResult.RouteValues["returncontroller"]);
                }
               
            }


            /// <summary>
            /// A test for Add ie create a new revision (Post action)
            /// The test verifies that correct views or redirect to route is returned, also tested that revision id is set.
            /// If no users is selected check that an error dialog is shown...
            /// </summary>
            [TestMethod]
            [TestCategory("UnitTestApp")]
            [Ignore]
            public void AddPostTest()
            {
                using (ShimsContext.Create())
                {
                    // Arrange
                    LoginApplicationUserAndSetSessionVariables();

                    // TODO this does not work; how do we set transactions to a user and shim them in code?
                    Transaction = new ShimTransaction()
                    {
                        Commit = () => { return; },
                    };

                    RevisionController controller = new RevisionController();
                    string controllerName = "Revision";
                    string actionName = "Add";
                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                    ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                    ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                    TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                    SessionRevision = refTaxonRevision;
                    SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                    SessionTaxonId = taxonIdentifier;
                    
                    // Create model....
                    RevisionAddViewModel model = new RevisionAddViewModel();
                    model.ExpectedPublishingDate = DateTime.Now.AddYears(1);
                    model.ExpectedStartDate = DateTime.Now;
                    model.RevisionDescription = "Testar min beskrivning";
                    model.RevisionId = refTaxonRevision.Id.ToString();
                    model.TaxonId = refTaxonRevision.RootTaxon.Id.ToString();
                    model.SelectedUsers = new int[] { SessionHelper.GetFromSession<IUserContext>("userContext").User.Id };


                    // Test 1: Test Save button pressed
                    //Act
                    var addResult = controller.Add(model) as RedirectToRouteResult;

                    //Assert
                    Assert.IsNotNull(addResult);

                    // Test that Edit action is returned.
                    Assert.AreEqual(refTaxonRevision.Id, addResult.RouteValues["revisionId"]);
                    Assert.AreEqual("Edit", addResult.RouteValues["action"]);
                }
            }

            /// <summary>
            /// A test for Add ie create a new revision (Post action)
            /// The test verifies that correct views is returned ie error view since taxon in revision is null.
            /// </summary>
            [TestMethod]
            [TestCategory("UnitTestApp")]
            public void AddPostTestInvalidData()
            {
                using (ShimsContext.Create())
                {
                    // Arrange
                    LoginApplicationUserAndSetSessionVariables();

                    // TODO this does not work; how do we set transactions to a user and shim them in code?
                    Transaction = new ShimTransaction()
                    {
                        Commit = () => { return; },
                    };
                    RevisionController controller = new RevisionController();
                    string controllerName = "Revision";
                    string actionName = "Add";

                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName);
                    //builder.RouteData.Values.Add("controller", "Revision");
                    //builder.RouteData.Values.Add("action", "Add");

                    ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                    ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                    TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                    SessionRevision = refTaxonRevision;
                    SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                    SessionTaxonId = taxonIdentifier;

                    // Create model....
                    RevisionAddViewModel model = new RevisionAddViewModel();
                    model.ExpectedPublishingDate = DateTime.Now.AddYears(1);
                    model.ExpectedStartDate = DateTime.Now;
                    model.RevisionDescription = "Testar min beskrivning";
                    model.RevisionId = refTaxonRevision.Id.ToString();
                    model.TaxonId = DyntaxaTestSettings.Default.FailingTestTaxonId.ToString();
                    model.SelectedUsers = new int[] { SessionHelper.GetFromSession<IUserContext>("userContext").User.Id };


                    // Act
                    var addInvalidResult = controller.Add(model) as ViewResult;

                    // Assert
                    Assert.IsNotNull(addInvalidResult);

                    // Test that correct view is returned
                    Assert.AreEqual("ErrorInfo", addInvalidResult.ViewName);
                    var addViewError = addInvalidResult.ViewData.Model as ErrorViewModel;
                    Assert.IsNotNull(addViewError);
                    Assert.AreEqual("Revision", addViewError.ErrorController);
                    Assert.AreEqual("Add", addViewError.ErrorAction);
                }
    
            }

            /// <summary>
            /// A test for all actions in the controller to verify that correct authorization attributes are set.
            /// Ie checks the role of user so that actins not allowed is unavaliabe.
            /// </summary>
            [TestMethod]
            [TestCategory("UnitTestApp")]
            public void AuthorizationTest()
            {
                //Arrange
                using (ShimsContext.Create())
                {
                    // Arrange
                    LoginApplicationUserAndSetSessionVariables();

                     RevisionController controller = new RevisionController();
                    string controllerName = "Revision";
                    string actionName = "Add";
                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                    var type = controller.GetType();


                    // 1. Test that action add (get) has correct authority..
                    //Act
                    var methodInfo = type.GetMethod("Add", new Type[] { typeof(string) });
                    var attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                    //Assert
                    Assert.AreEqual(1, attributes.Length);
                    Assert.AreEqual(RequiredAuthorization.TaxonRevisionAdministrator, ((DyntaxaAuthorizeAttribute)attributes[0]).Order);

                    // 2. Test that action add (post) has correct authority..
                    //Act
                    methodInfo = type.GetMethod("Add", new Type[] { typeof(RevisionAddViewModel) });
                    attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                    //Assert
                    Assert.AreEqual(1, attributes.Length);
                    Assert.AreEqual(RequiredAuthorization.TaxonRevisionAdministrator, ((DyntaxaAuthorizeAttribute)attributes[0]).Order);

                    // 3. Test that action edit (get) has correct authority..
                    //Act
                    methodInfo = type.GetMethod("Edit", new Type[] { typeof(string), typeof(string), typeof(bool) });
                    attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                    //Assert
                    Assert.AreEqual(1, attributes.Length);
                    Assert.AreEqual(RequiredAuthorization.TaxonRevisionAdministrator, ((DyntaxaAuthorizeAttribute)attributes[0]).Order);

                    // 4. Test that action edit (post) has correct authority..
                    //Act
                    methodInfo = type.GetMethod("Edit", new Type[] { typeof(RevisionEditViewModel), typeof(string) });
                    attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                    //Assert
                    Assert.AreEqual(1, attributes.Length);
                    Assert.AreEqual(RequiredAuthorization.TaxonRevisionAdministrator, ((DyntaxaAuthorizeAttribute)attributes[0]).Order);

                    // 5. Test that action start (get) has correct authority..
                    //Act
                    methodInfo = type.GetMethod("StartEditing", new Type[] { typeof(string) });
                    attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                    //Assert
                    Assert.AreEqual(1, attributes.Length);
                    Assert.AreEqual(RequiredAuthorization.TaxonEditor, ((DyntaxaAuthorizeAttribute)attributes[0]).Order);

                    // 6. Test that action start (post) has correct authority..
                    //Act
                    methodInfo = type.GetMethod("StartEditing", new Type[] { typeof(RevisionInfoItemModelHelper) });
                    attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                    //Assert
                    //Assert
                    Assert.AreEqual(1, attributes.Length);
                    Assert.AreEqual(RequiredAuthorization.TaxonEditor, ((DyntaxaAuthorizeAttribute)attributes[0]).Order);


                    // 7. Test that action list (get) has no authority..
                    //Act
                    methodInfo = type.GetMethod("List", new Type[] { typeof(string), typeof(string), typeof(string) });
                    attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                    //Assert
                    Assert.AreEqual(0, attributes.Length);

                    // 8. Test that action List (post) has no authority..
                    //Act
                    methodInfo = type.GetMethod("List", new Type[] { typeof(RevisionListViewModel), typeof(int[]) });
                    attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                    //Assert
                    //Assert
                    Assert.AreEqual(0, attributes.Length);

                    // 9. Test that action stop (get) has correct authority..
                    //Act
                    methodInfo = type.GetMethod("StopEditing", new Type[] { typeof(string) });
                    attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                    //Assert
                    //Assert
                    Assert.AreEqual(1, attributes.Length);
                    Assert.AreEqual(RequiredAuthorization.TaxonEditor, ((DyntaxaAuthorizeAttribute)attributes[0]).Order);


                    // 10. Test that action start (post) has correct authority..
                    //Act
                    methodInfo = type.GetMethod("StopEditing", new Type[] { typeof(RevisionInfoItemModelHelper) });
                    attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                    //Assert
                    //Assert
                    Assert.AreEqual(1, attributes.Length);
                    Assert.AreEqual(RequiredAuthorization.TaxonEditor, ((DyntaxaAuthorizeAttribute)attributes[0]).Order);


                    // 11. Test that action Info (get) has no authority..
                    //Act
                    methodInfo = type.GetMethod("Info", new Type[] { typeof(string) });
                    attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                    //Assert
                    Assert.AreEqual(0, attributes.Length);

                    // 12. Test that action Info (post) has no authority..
                    //Act
                    methodInfo = type.GetMethod("Info", new Type[] { typeof(RevisionInfoViewModel) });
                    attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                    //Assert
                    Assert.AreEqual(0, attributes.Length);

                    // 13. Test that action Initialize has correct authority..
                    //Act
                    methodInfo = type.GetMethod("Initialize", new Type[] { typeof(string) });
                    attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                    //Assert
                    Assert.AreEqual(1, attributes.Length);
                    Assert.AreEqual(RequiredAuthorization.TaxonRevisionAdministrator, ((DyntaxaAuthorizeAttribute)attributes[0]).Order);

                    // 14. Test that action Finalize  has correct authority..
                    //Act
                    methodInfo = type.GetMethod("Finalize", new Type[] { typeof(string) });
                    attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                    //Assert
                    Assert.AreEqual(1, attributes.Length);
                    Assert.AreEqual(RequiredAuthorization.TaxonRevisionAdministrator, ((DyntaxaAuthorizeAttribute)attributes[0]).Order);
                }
            }


            /// <summary>
            /// A test for Edit ie update a  revision (Get action)
            /// The test verifies that correct views is returned and that EditRevisonViwewModel have correct values..
            /// </summary>
            [TestMethod]
            [TestCategory("UnitTestApp")]
            [Ignore]
            public void EditGetTest()
            {

                using (ShimsContext.Create())
                {
                    // Arrange
                    LoginApplicationUserAndSetSessionVariables();

                    RevisionController controller = new RevisionController();
                    string controllerName = "Revision";
                    string actionName = "Edit";
                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                    ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                    ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                    TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                    SessionRevision = refTaxonRevision;
                    SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                    SessionTaxonId = taxonIdentifier;

                    //Act
                    var editResult = controller.Edit(DyntaxaTestSettings.Default.TestRevisionId.ToString()) as ViewResult;
                    //Assert
                    Assert.IsNotNull(editResult);

                    var editViewModel = editResult.ViewData.Model as RevisionEditViewModel;

                    Assert.IsNotNull(editViewModel);
                    // Test that correct view is returned
                    Assert.AreEqual("Edit", editResult.ViewName);
                    // Test model values
                    Assert.IsTrue((refTaxonRevision.ExpectedEndDate - editViewModel.ExpectedPublishingDate) < new TimeSpan(0, 0, DyntaxaTestSettings.Default.ComputerTimeDifference));
                    Assert.IsTrue((refTaxonRevision.ExpectedStartDate - editViewModel.ExpectedStartDate) < new TimeSpan(0, 0, DyntaxaTestSettings.Default.ComputerTimeDifference));
                    Assert.AreEqual(refTaxonRevision.Description, editViewModel.RevisionDescription);
                    Assert.AreEqual(DyntaxaTestSettings.Default.TestRevisionId.ToString(), editViewModel.RevisionId);
                    Assert.AreEqual(null, editViewModel.SelectedUsers);
                    Assert.AreEqual(1, editViewModel.SelectedUserList.Count);
                    Assert.AreEqual(refTaxonRevision.RootTaxon.Id.ToString(), editViewModel.RevisionTaxonId);
                    Assert.AreEqual(1, editViewModel.UserList.Count);
                    Assert.AreEqual(Resources.DyntaxaResource.RevisionListSelectedRevisionStatusCreatedText, editViewModel.RevisionStatus);
                    //Assert.AreEqual(QualityStatus.Good,editViewModel.RevisionTaxonQuality);
                    //Assert.AreEqual(string.Empty,editViewModel.RevisionTaxonQualityDescription);
                    //Assert.AreEqual(3, editViewModel.RevisionTaxonQualityList.Count);
                    Assert.AreEqual(false, editViewModel.ShowFinalizeButton);
                    Assert.AreEqual(true, editViewModel.ShowInitalizeButton);
                    int tempTestTaxonRevisionEditorRoleId = 1396;
                    Assert.IsTrue(editViewModel.UserAdminLink.Url.Contains(tempTestTaxonRevisionEditorRoleId.ToString()));


                    //Act
                    var editOngoingResult = controller.Edit(DyntaxaTestSettings.Default.TestRevisionOngoingId.ToString()) as ViewResult;
                    //Assert
                    Assert.IsNotNull(editOngoingResult);

                    var editOngoingViewModel = editResult.ViewData.Model as RevisionEditViewModel;

                    Assert.IsNotNull(editOngoingViewModel);
                    Assert.AreEqual(Resources.DyntaxaResource.RevisionListSelectedRevisionStatusOngoingText, editOngoingViewModel.RevisionStatus);
                    Assert.AreEqual(true, editOngoingViewModel.ShowFinalizeButton);
                    Assert.AreEqual(false, editOngoingViewModel.ShowInitalizeButton);
                }
            }


            /// <summary>
            /// A test for Edit ie update a revision (Get action)
            /// The test verifies that correct views is returned and error view if revision id is null.
            /// </summary>
            [TestCategory("UnitTestApp")]
            [TestMethod]
            public void EditGetTestInvalidData()
            {

                using (ShimsContext.Create())
                {
                    // Arrange
                    LoginApplicationUserAndSetSessionVariables();

                    RevisionController controller = new RevisionController();
                    string controllerName = "Revision";
                    string actionName = "Edit";
                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                    ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                    ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                    TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                    SessionRevision = refTaxonRevision;
                    SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                    SessionTaxonId = taxonIdentifier;
                    // Act
                    // Test what happens if invalid taxon ie taxon = null is set in revision. Check that a proper error message is shown
                    var editInvalidResult = controller.Edit("0") as ViewResult;

                    //Assert
                    Assert.IsNotNull(editInvalidResult);
                    // Test that correct view is returned
                    Assert.AreEqual("ErrorInfo", editInvalidResult.ViewName);
                    var editViewError = editInvalidResult.ViewData.Model as ErrorViewModel;
                    Assert.IsNotNull(editViewError);
                    Assert.AreEqual("Revision", editViewError.ErrorController);
                    Assert.AreEqual("Edit", editViewError.ErrorAction);
                }

            }


            /// <summary>
            /// A test for Edit ie update revision (Post action)
            /// The test verifies that correct views or redirect to route is returned, also tested that revision id is set.
            /// If no users is selected check that an error dialog is shown...
            /// </summary>
            [TestMethod]
            [TestCategory("UnitTestApp")]
            [Ignore]
            public void EditPostTest()
            {

                using (ShimsContext.Create())
                {
                    // Arrange
                    LoginApplicationUserAndSetSessionVariables();

                    // TODO this does not work; how do we set transactions to a user and shim them in code?
                    Transaction = new ShimTransaction()
                    {
                        Commit = () => { return; },
                    }; 
                    RevisionController controller = new RevisionController();
                    string controllerName = "Revision";
                    string actionName = "Edit";
                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                    // Set person since Artfakta is accsessed here and we need a person name.
                    IPerson person = new Person(UserContextData);
                    person.FirstName = "TestUserFirstName";
                    person.LastName = "TestUserLastName";
                    person.Gender = new PersonGender(1, "test", 1, UserDataSourceTestRepositoryData.GetDataContext(ApplicationUserContext));
                    // Set person to user
                    UserContextData.User.SetPerson(UserContextData, person);

                    IPerson personApp = new Person(ApplicationUserContext);
                    personApp.FirstName = "TestAppUserFirstName";
                    personApp.LastName = "TestAppUserLastName";
                    // Set person to user
                    ApplicationUserContext.User.SetPerson(ApplicationUserContext, personApp);

                    // Mock Controller
                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName);
                    ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                    ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                    TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                    SessionRevision = refTaxonRevision;
                    SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                    SessionTaxonId = taxonIdentifier;

                    // Create model....
                    RevisionEditViewModel model = new RevisionEditViewModel();
                    model.ExpectedPublishingDate = DateTime.Now.AddYears(1);
                    model.ExpectedStartDate = DateTime.Now;
                    model.RevisionDescription = "Testar min beskrivning";
                    model.RevisionId = refTaxonRevision.Id.ToString();
                    model.TaxonId = refTaxonRevision.RootTaxon.Id.ToString();
                    model.NoOfRevisionReferences = 1;
                    model.RevisionTaxonId = refTaxonRevision.RootTaxon.Id.ToString();
                    //model.RevisionTaxonQuality = QualityStatus.Bad;
                    //model.RevisionTaxonQualityDescription = "My quality description";

                    // Set user implemtation
                    model.SelectedUsers = new int[] { SessionHelper.GetFromSession<IUserContext>("userContext").User.Id, SessionHelper.GetFromSession<IUserContext>("applicationUserContext").User.Id };


                    // Test 1: Test Save button pressed
                    //Act
                    var editResult = controller.Edit(model, "GetSelectedSave") as RedirectToRouteResult;

                    //Assert
                    Assert.IsNotNull(editResult);

                    // Test that Edit action is returned.
                    Assert.AreEqual(refTaxonRevision.Id, editResult.RouteValues["revisionId"]);
                    Assert.AreEqual("Edit", editResult.RouteValues["action"]);

                    // Test 2: Test initialize button pressed
                    //Act
                    editResult = controller.Edit(model, "GetSelectedInitialize") as RedirectToRouteResult;

                    //Assert
                    Assert.IsNotNull(editResult);

                    // Test that Initialize action is returned.
                    Assert.AreEqual(refTaxonRevision.Id.ToString(), editResult.RouteValues["revisionId"]);
                    Assert.AreEqual("Initialize", editResult.RouteValues["action"]);

                    // Test 3: Test initialize button pressed and no users is selected
                    //Act
                    model.SelectedUsers = new int[0];
                    var editViewResult = controller.Edit(model, "GetSelectedInitialize") as ViewResult;

                    //Assert
                    Assert.IsNotNull(editViewResult);
                    Assert.AreEqual("Edit", editViewResult.ViewName);

                    var editModel = editViewResult.ViewData.Model as RevisionEditViewModel;

                    //Assert
                    Assert.IsNotNull(editModel);



                    // Test 4: Test finalize button pressed
                    //Act
                    controller = new RevisionController();
                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName);
                    model = new RevisionEditViewModel();
                    model.ExpectedPublishingDate = DateTime.Now.AddYears(1);
                    model.ExpectedStartDate = DateTime.Now;
                    model.RevisionDescription = "Testar min beskrivning";
                    model.RevisionId = refTaxonRevision.Id.ToString();
                    model.TaxonId = refTaxonRevision.RootTaxon.Id.ToString();
                    model.NoOfRevisionReferences = 1;
                    model.RevisionTaxonId = refTaxonRevision.RootTaxon.Id.ToString();
                    model.SelectedUsers = new int[] { SessionHelper.GetFromSession<IUserContext>("applicationUserContext").User.Id };
                    editResult = controller.Edit(model, "GetSelectedFinalize") as RedirectToRouteResult;

                    //Assert
                    Assert.IsNotNull(editResult);

                    // Test that Initialize action is returned.
                    Assert.AreEqual(refTaxonRevision.Id, editResult.RouteValues["revisionId"]);
                    Assert.AreEqual("Finalize", editResult.RouteValues["action"]);

                    // Test 5: Test finalize button pressed and no users is selected
                    //Act
                    model.SelectedUsers = new int[0];
                    editViewResult = controller.Edit(model, "GetSelectedFinalize") as ViewResult;

                    //Assert
                    Assert.IsNotNull(editViewResult);
                    Assert.AreEqual("Edit", editViewResult.ViewName);

                    editModel = editViewResult.ViewData.Model as RevisionEditViewModel;

                    //Assert
                    Assert.IsNotNull(editModel);
                }
            }

            /// <summary>
            /// A test for Edit ie update revision (Post action)
            /// The test verifies that correct views is returned and error view if taxon in revision is null.
            /// </summary>
            [TestMethod]
            [TestCategory("UnitTestApp")]
            public void EditPostTestInvalidTaxon()
            {
                using (ShimsContext.Create())
                {
                    // Arrange
                    LoginApplicationUserAndSetSessionVariables();

                    // TODO this does not work; how do we set transactions to a user and shim them in code?
                    Transaction = new ShimTransaction()
                    {
                        Commit = () => { return; },
                    };
                    RevisionController controller = new RevisionController();
                    string controllerName = "Revision";
                    string actionName = "Edit";
                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                    ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                    ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                    TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                    SessionRevision = refTaxonRevision;
                    SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                    SessionTaxonId = taxonIdentifier;
                    
                    // Create model....
                    RevisionAddViewModel model = new RevisionAddViewModel();
                    model.ExpectedPublishingDate = DateTime.Now.AddYears(1);
                    model.ExpectedStartDate = DateTime.Now;
                    model.RevisionDescription = "Testar min beskrivning";
                    model.RevisionId = refTaxonRevision.Id.ToString();
                    model.TaxonId = DyntaxaTestSettings.Default.FailingTestTaxonId.ToString();
                    model.SelectedUsers = new int[] { SessionHelper.GetFromSession<IUserContext>("userContext").User.Id };

                    // Act
                    var addInvalidResult = controller.Add(model) as ViewResult;

                    // Assert
                    Assert.IsNotNull(addInvalidResult);

                    // Test that correct view is returned
                    Assert.AreEqual("ErrorInfo", addInvalidResult.ViewName);
                    var addViewError = addInvalidResult.ViewData.Model as ErrorViewModel;
                    Assert.IsNotNull(addViewError);
                    Assert.AreEqual("Revision", addViewError.ErrorController);
                    Assert.AreEqual("Edit", addViewError.ErrorAction);
                }
            }

            /// <summary>
            /// A test for finalizing for a revision.
            /// </summary>
            [TestMethod]
            [TestCategory("UnitTestApp")]
            [Ignore]
            public void FinalizeTest()
            {
                using (ShimsContext.Create())
                {
                    // Arrange
                    LoginApplicationUserAndSetSessionVariables();

                    // TODO this does not work; how do we set transactions to a user and shim them in code?
                    Transaction = new ShimTransaction()
                    {
                        Commit = () => { return; },
                    };
                    RevisionController controller = new RevisionController();
                    string controllerName = "Revision";
                    string actionName = "Finalize";
                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                    ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                    ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                    TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                    SessionRevision = refTaxonRevision;
                    SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                    SessionTaxonId = taxonIdentifier;

                    // Test 1 test that view is displayed in a correct way.
                    //Act
                    var result = controller.Finalize(DyntaxaTestSettings.Default.TestRevisionOngoingId.ToString()) as RedirectToRouteResult;

                    //Assert
                    // Test that correct view is returned
                    Assert.IsNotNull(result);

                    // Assert.AreEqual(refRevision.Id.ToString(), result.RouteValues["revisionId"]);
                    Assert.AreEqual("List", result.RouteValues["action"]);


                    // Test 2 verify that error view is displayed-
                    //Act
                    string test = null;
                    //result = controller.Initialize(test, DyntaxaTestSettings.Default.TestTaxonId.ToString()) as ViewResult;
                    var resultView = controller.Finalize(test) as ViewResult;

                    //Assert
                    Assert.IsNotNull(resultView);
                    Assert.AreEqual("ErrorInfo", resultView.ViewName);


                    // Test 3 verify that error information is displayed when revision is not saved.
                    //Act
                    resultView = controller.Finalize("0") as ViewResult;
                    //result = controller.Initialize("0", DyntaxaTestSettings.Default.TestTaxonId.ToString()) as ViewResult;

                    //Assert
                    Assert.IsNotNull(resultView);
                    Assert.AreEqual("ErrorInfo", resultView.ViewName);

                    // Test model values and that Error information is shown
                    var errorModel = resultView.ViewData.Model as ErrorViewModel;
                    Assert.IsNotNull(errorModel);
                    Assert.AreEqual("Finalize", errorModel.ErrorAction);
                    Assert.AreEqual("Revision", errorModel.ErrorController);
                }
            }


            /// <summary>
            /// A test for viewing info for a revision, get action.
            /// </summary>
            [TestMethod]
            [TestCategory("UnitTestApp")]
            [Ignore]
            public void InfoGetTest()
            {
                using (ShimsContext.Create())
                {
                    // Arrange
                    LoginApplicationUserAndSetSessionVariables();

                    // TODO this does not work; how do we set transactions to a user and shim them in code?
                    Transaction = new ShimTransaction()
                    {
                        Commit = () => { return; },
                    };
                    RevisionController controller = new RevisionController();
                    string controllerName = "Revision";
                    string actionName = "Info";
                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                    ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                    ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                    TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                    SessionRevision = refTaxonRevision;
                    SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                    SessionTaxonId = taxonIdentifier;
                    //Act
                    var result = controller.Info(DyntaxaTestSettings.Default.TestRevisionId.ToString()) as ViewResult;

                    //Assert
                    Assert.IsNotNull(result);
                    Assert.AreEqual("RevisionInfo", result.ViewName);

                    var model = result.ViewData.Model as RevisionInfoViewModel;
                    Assert.IsNotNull(model);
                    Assert.AreEqual(Resources.DyntaxaResource.RevisionInfoMainHeaderText, model.RevisionEditingActionHeaderText);
                    Assert.AreEqual(Resources.DyntaxaResource.RevisionInfoActionHeaderText, model.RevisionEditingHeaderText);
                    Assert.AreEqual(1, model.RevisionInfoItems.Count);
                    foreach (RevisionInfoItemModelHelper infoItems in model.RevisionInfoItems)
                    {
                        Assert.AreEqual(refTaxonRevision.Id.ToString(), infoItems.RevisionId);
                        Assert.AreEqual(refTaxonRevision.Description, infoItems.RevisionDescription);
                        Assert.AreEqual(Resources.DyntaxaResource.RevisionListSelectedRevisionStatusCreatedText, infoItems.RevisionStatus);
                        //Assert.IsTrue(infoItems.RevisionStatus.Equals("preliminary") || infoItems.RevisionStatus.Equals("Preliminär"));
                        Assert.AreEqual(refTaxonRevision.ExpectedEndDate.ToShortDateString(), infoItems.ExpectedPublishingDate);
                        Assert.AreEqual(refTaxonRevision.ExpectedStartDate.ToShortDateString(), infoItems.ExpectedStartDate);

                        Assert.AreEqual(refTaxonRevision.RootTaxon.ScientificName, infoItems.ScientificName);
                        Assert.AreEqual(refTaxonRevision.RootTaxon.Id.ToString(), infoItems.TaxonId);
                        Assert.AreEqual(null, infoItems.SelectedRevisionForEditingText);
                        Assert.AreEqual(false, infoItems.ShowRevisionEditingButton);
                        Assert.AreEqual(false, infoItems.EnableRevisionEditingButton);


                    }
                    Assert.AreEqual("List", model.EditingAction);
                    Assert.AreEqual("Revision", model.EditingController);

                    //Act
                    string test = null;
                    result = controller.Info(test) as ViewResult;

                    //Assert
                    Assert.IsNotNull(result);
                    Assert.AreEqual("ErrorInfo", result.ViewName);
                }

            
            }


            /// <summary>
            /// A test for revision info post action.
            /// Verifies the a redirect is taking place.
            /// </summary>
            [TestMethod]
            [TestCategory("UnitTestApp")]
            public void InfoPostTest()
            {

                using (ShimsContext.Create())
                {
                    // Arrange
                    LoginApplicationUserAndSetSessionVariables();

                    // TODO this does not work; how do we set transactions to a user and shim them in code?
                    Transaction = new ShimTransaction()
                    {
                        Commit = () => { return; },
                    };
                    RevisionController controller = new RevisionController();
                    string controllerName = "Revision";
                    string actionName = "Info";
                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName);
                    RevisionInfoViewModel model = new RevisionInfoViewModel();
                    model.RevisionId = DyntaxaTestSettings.Default.TestRevisionId.ToString();
                    model.TaxonId = DyntaxaTestSettings.Default.TestTaxonId.ToString();

                    ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                    ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                    TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                    SessionRevision = refTaxonRevision;
                    SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                    SessionTaxonId = taxonIdentifier;


                    //Act
                    var result = controller.Info(model) as RedirectToRouteResult;

                    //Assert.
                    Assert.IsNotNull(result);
                    // Test that Edit action is returned.
                    Assert.AreEqual("List", result.RouteValues["action"]);
                }
            }

            /// <summary>
            /// A test for initializing for a revision.
            /// </summary>
            [TestMethod]
            [TestCategory("UnitTestApp")]
            [Ignore]
        public void InitializeTest() 
            {
                using (ShimsContext.Create())
                {
                    // Arrange
                    LoginApplicationUserAndSetSessionVariables();

                    // TODO this does not work; how do we set transactions to a user and shim them in code?
                    Transaction = new ShimTransaction()
                    {
                        Commit = () => { return; },
                    };
                    RevisionController controller = new RevisionController();
                    string controllerName = "Revision";
                    string actionName = "Initialize";
                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                    ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                    ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                    TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                    SessionRevision = refTaxonRevision;
                    SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                    SessionTaxonId = taxonIdentifier;
                    // Test 1 test that view is displayed in a correct way.
                    //Act
                    var result = controller.Initialize(DyntaxaTestSettings.Default.TestRevisionOngoingId.ToString()) as RedirectToRouteResult;

                    //Assert
                    // Test that correct view is returned
                    Assert.IsNotNull(result);

                    Assert.AreEqual(refTaxonRevision.RootTaxon.Id, result.RouteValues["taxonId"]);
                    Assert.AreEqual("List", result.RouteValues["action"]);


                    // Test 2 verify that error view is displayed-
                    //Act
                    string test = null;
                    //result = controller.Initialize(test, DyntaxaTestSettings.Default.TestTaxonId.ToString()) as ViewResult;
                    var resultView = controller.Initialize(test) as ViewResult;

                    //Assert
                    Assert.IsNotNull(resultView);
                    Assert.AreEqual("ErrorInfo", resultView.ViewName);


                    // Test 3 verify that error information is displayed when revision is not saved.
                    //Act
                    resultView = controller.Initialize("0") as ViewResult;
                    //result = controller.Initialize("0", DyntaxaTestSettings.Default.TestTaxonId.ToString()) as ViewResult;

                    //Assert
                    Assert.IsNotNull(resultView);
                    Assert.AreEqual("ErrorInfo", resultView.ViewName);

                    // Test model values and that Error information is shown
                    var errorModel = resultView.ViewData.Model as ErrorViewModel;
                    Assert.IsNotNull(errorModel);
                    Assert.AreEqual("Initialize", errorModel.ErrorAction);
                    Assert.AreEqual("Revision", errorModel.ErrorController);
                }
               
            }



            /// <summary>
            /// A test for List ie show all existing revisions. (Get action)
            /// The test verifies that correct views is returned and that ListRevisonViwewModel have correct values.
            /// </summary>
            [TestMethod]
            [TestCategory("UnitTestApp")]
            [Ignore]
            public void ListGetTest()
            {

                using (ShimsContext.Create())
                {
                    // Arrange
                    LoginApplicationUserAndSetSessionVariables();

                    RevisionController controller = new RevisionController();
                    string controllerName = "Revision";
                    string actionName = "List";
                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                    IUserContext userContext = UserContextData;
                    ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(userContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Created.ToString());
                    RoleList roleList = new RoleList();
                    roleList.Add(UserDataSourceTestRepositoryData.GetNewRole("ListTester", 222));
                    userContext.CurrentRoles = roleList;

                    //Act
                    var listResult = controller.List("häst", null, null) as ViewResult;
                    //Assert
                    Assert.IsNotNull(listResult);

                    var listViewModel = listResult.ViewData.Model as RevisionListViewModel;

                    Assert.IsNotNull(listViewModel);
                    // Test that correct view is returned
                    Assert.AreEqual("List", listResult.ViewName);
                    // Test model values
                    Assert.IsTrue(listViewModel.IsViewReadonly);


                    Assert.IsNotNull(listViewModel.TaxonDescription);
                    Assert.IsNotNull(listViewModel.TaxonCategory);
                    Assert.IsNotNull(listViewModel.TaxonScientificName);

                    Assert.AreEqual(Resources.DyntaxaSettings.Default.RevisionSelctionTaxonStatusCheckBoxId, listViewModel.RevisionSelectionItemHelper.RevisionSelctionStatusId);
                    Assert.AreEqual(3, listViewModel.RevisionStatus.Count);


                    Assert.AreEqual(3, listViewModel.Revisions.Count);
                    for (int i = 0; i < listViewModel.Revisions.Count; i++)
                    {
                        RevisionItemModel revisions = listViewModel.Revisions[i];
                        Assert.IsFalse(revisions.IsRevisionEditable);
                        Assert.AreEqual(refTaxonRevision.ExpectedEndDate.ToShortDateString(), revisions.PublishingDate);
                        Assert.AreEqual(refTaxonRevision.ExpectedStartDate.ToShortDateString(), revisions.StartDate);
                        if (i == 0)
                        {
                            Assert.AreEqual(refTaxonRevision.Id, revisions.RevisionId);
                            Assert.AreEqual(Resources.DyntaxaResource.RevisionListSelectedRevisionStatusCreatedText, revisions.RevisionStatus);
                            Assert.IsFalse(revisions.IsRevisionPossibleToDelete);
                            Assert.IsFalse(revisions.IsRevisionPossibleToStart);
                            Assert.IsFalse(revisions.IsRevisionPossibleToStop);

                        }
                        else if (i == 1)
                        {
                            Assert.AreEqual(DyntaxaTestSettings.Default.TestRevisionOngoingId, revisions.RevisionId);
                            Assert.AreEqual(Resources.DyntaxaResource.RevisionListSelectedRevisionStatusOngoingText, revisions.RevisionStatus);
                            Assert.IsFalse(revisions.IsRevisionPossibleToDelete);
                            Assert.IsFalse(revisions.IsRevisionPossibleToStart);
                            Assert.IsFalse(revisions.IsRevisionPossibleToStop);
                            // Prepare for next test set role for this revision
                            roleList.Add(UserDataSourceTestRepositoryData.GetTaxonRevisionRole(userContext.User.UserName, 2021, userContext, revisions.GUID));
                        }
                        else
                        {
                            Assert.AreEqual(DyntaxaTestSettings.Default.TestRevisionPublishedId, revisions.RevisionId);
                            Assert.AreEqual(Resources.DyntaxaResource.RevisionListSelectedRevisionStatusClosedText, revisions.RevisionStatus);
                            Assert.IsFalse(revisions.IsRevisionPossibleToDelete);
                            Assert.IsFalse(revisions.IsRevisionPossibleToStart);
                            Assert.IsFalse(revisions.IsRevisionPossibleToStop);

                        }

                        Assert.IsTrue(revisions.TaxonCategory.Contains(refTaxonRevision.RootTaxon.Category.Name));
                        Assert.AreEqual(refTaxonRevision.RootTaxon.ScientificName, revisions.TaxonScentificRecomendedName);
                    }

                    // Test 2: a taxon id is entered but not selected to display only revisions associated with a taxon.



                    //Arrange
                    //builder.Session.Remove("RevisionList");
                    roleList.Add(UserDataSourceTestRepositoryData.GetTaxonAdministratorRole("AdminTester", 222, userContext));
                    userContext.CurrentRoles = roleList;
                    //Act
                    listResult = controller.List("häst", null, null) as ViewResult;

                    //Assert
                    Assert.IsNotNull(listResult);

                    listViewModel = listResult.ViewData.Model as RevisionListViewModel;
                    Assert.IsNotNull(listViewModel);
                    // Test that correct view is returned
                    Assert.AreEqual("List", listResult.ViewName);
                    // Test model values
                    Assert.IsFalse(listViewModel.IsViewReadonly); // Yes, possible to edit...
                    Assert.IsTrue(listViewModel.ShowTaxonNameLabelForRevisions);
                    Assert.AreEqual(true, listViewModel.RevisionSelectionItemHelper.IsChecked);

                    Assert.AreNotEqual(string.Empty, listViewModel.TaxonDescription);
                    Assert.AreEqual(DyntaxaTestSettings.Default.TestTaxonId.ToString(), listViewModel.TaxonId);
                    Assert.IsTrue(listViewModel.TaxonCategory.Contains(refTaxonRevision.RootTaxon.Category.Name));
                    Assert.AreEqual(refTaxonRevision.RootTaxon.CommonName, listViewModel.TaxonCommonName);
                    Assert.AreEqual(refTaxonRevision.RootTaxon.ScientificName, listViewModel.TaxonScientificName);

                    Assert.AreEqual(Resources.DyntaxaSettings.Default.RevisionSelctionTaxonStatusCheckBoxId, listViewModel.RevisionSelectionItemHelper.RevisionSelctionStatusId);
                    Assert.AreEqual(3, listViewModel.Revisions.Count);

                    for (int i = 0; i < listViewModel.Revisions.Count; i++)
                    {
                        RevisionItemModel revisions = listViewModel.Revisions[i];
                        if (i == 0)
                        {
                            Assert.IsTrue(revisions.IsRevisionEditable);
                            Assert.IsTrue(revisions.IsRevisionPossibleToDelete);
                            Assert.IsFalse(revisions.IsRevisionPossibleToStart);
                            Assert.IsFalse(revisions.IsRevisionPossibleToStop);
                        }
                        else if (i == 1)
                        {
                            Assert.IsTrue(revisions.IsRevisionEditable);
                            Assert.IsFalse(revisions.IsRevisionPossibleToDelete);
                            Assert.IsTrue(revisions.IsRevisionPossibleToStart);
                            Assert.IsFalse(revisions.IsRevisionPossibleToStop);
                        }
                        else
                        {
                            Assert.IsFalse(revisions.IsRevisionEditable);
                            Assert.IsFalse(revisions.IsRevisionPossibleToDelete);
                            Assert.IsFalse(revisions.IsRevisionPossibleToStart);
                            Assert.IsFalse(revisions.IsRevisionPossibleToStop);
                        }
                    }


                    // Test 3: a taxon id is entered and selected to display only revisions associated with a taxon.
                    //Arrange
                   // builder.Session.Remove("RevisionList");

                    roleList.Add(UserDataSourceTestRepositoryData.GetTaxonAdministratorRole("AdminTester", 222, userContext));
                    userContext.CurrentRoles = roleList;
                    //Act
                    listResult = controller.List(DyntaxaTestSettings.Default.TestTaxonId.ToString(), null, null) as ViewResult;

                    //Assert
                    Assert.IsNotNull(listResult);

                    listViewModel = listResult.ViewData.Model as RevisionListViewModel;
                    Assert.IsNotNull(listViewModel);
                    // Test that correct view is returned
                    Assert.AreEqual("List", listResult.ViewName);
                    // Test model values
                    Assert.IsFalse(listViewModel.IsViewReadonly); // Yes, possible to edit...
                    Assert.IsTrue(listViewModel.ShowTaxonNameLabelForRevisions);
                    Assert.AreEqual(true, listViewModel.RevisionSelectionItemHelper.IsChecked);

                    Assert.AreEqual(Resources.DyntaxaSettings.Default.RevisionSelctionTaxonStatusCheckBoxId, listViewModel.RevisionSelectionItemHelper.RevisionSelctionStatusId);
                    Assert.AreEqual(3, listViewModel.Revisions.Count);

                }
            }

            /// <summary>
            /// A test for Add ie create a new revision (Get action)
            /// The test verifies that correct views is returned and error view if taxon in revision is null.
            /// </summary>
            [TestMethod]
            [TestCategory("UnitTestApp")]
            [Ignore]
            public void ListGetTestInvalidData()
            {
                using (ShimsContext.Create())
                {
                    // Arrange
                    LoginApplicationUserAndSetSessionVariables();

                    RevisionController controller = new RevisionController();
                    string controllerName = "Revision";
                    string actionName = "List";
                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName);
                    ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(SessionHelper.GetFromSession<IUserContext>("userContext"), DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Created.ToString());
                    // Resore users context since users might be changed on the way  ....
                    IUserContext test = null;
                    ApplicationUserContext = test;
                    //Act
                    // Test what happens if invalid taxon ie taxon = null is set in revision. Check that a proper error message is shown
                    var listInvalidResult = controller.List(DyntaxaTestSettings.Default.FailingTestTaxonId.ToString(), null, null) as RedirectToRouteResult;

                    //Assert
                    Assert.IsNotNull(listInvalidResult);
                    // Test that correct view is returned
                    Assert.AreEqual("SearchResult", listInvalidResult.RouteValues["action"]);
                    Assert.AreEqual("Taxon", listInvalidResult.RouteValues["controller"]);
                    Assert.AreEqual("77", listInvalidResult.RouteValues["search"]);
                    Assert.AreEqual("List", listInvalidResult.RouteValues["returnAction"]);
                    Assert.AreEqual("Revision", listInvalidResult.RouteValues["returncontroller"]);

                    // Resore users context since users might be changed on the way  ....
                    SessionHelper.SetInSession("applicationUserContext", ApplicationUserContext);
                }
            }


            /// <summary>
            /// A test for List revisions (Post action)
            /// The test verifies that correct values are sent when redirect to route is returned, also tested that taxon id is set.
            /// </summary>
            [TestMethod]
            [TestCategory("UnitTestApp")]
            [Ignore]
            public void ListPostTest()
            {
                using (ShimsContext.Create())
                {
                    // Arrange
                    LoginApplicationUserAndSetSessionVariables();

                    // TODO this does not work; how do we set transactions to a user and shim them in code?
                    Transaction = new ShimTransaction()
                    {
                        Commit = () => { return; },
                    };
                    RevisionController controller = new RevisionController();
                    string controllerName = "Revision";
                    string actionName = "List";
                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                    ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                    ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                    TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                    SessionRevision = refTaxonRevision;
                    SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                    SessionTaxonId = taxonIdentifier;
                    
                    // Create model....
                    RevisionListViewModel model = new RevisionListViewModel();
                    model.TaxonId = refTaxonRevision.RootTaxon.Id.ToString();

                    // Test 1: Test RevisionSelctionTaxonStatusCheckBoxId ie select to show revisions assigned to a taxon and status preliminary.
                    int[] isChecked = new int[] { Resources.DyntaxaSettings.Default.RevisionSelctionTaxonStatusCheckBoxId, (Int32)TaxonRevisionStateId.Created };
                    //Act
                    var listResult = controller.List(model, isChecked) as RedirectToRouteResult;

                    //Assert
                    Assert.IsNotNull(listResult);

                    // Test that List action is returned and correct checkboxes are selected.
                    Assert.AreEqual(refTaxonRevision.RootTaxon.Id.ToString(), listResult.RouteValues["taxonId"]);
                    Assert.AreEqual("List", listResult.RouteValues["action"]);
                    // Get model
                    var type = controller.GetType();


                    // 1. Test that action add (get) has correct authority..
                    //Act
                    //Crate binding flag that will describe the type of object you try to access
                    //the following can be used in most cases.
                    BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
                    PropertyInfo propertyInfo = controller.GetType().GetProperty("RevisionListSettings", bindingFlags);
                    object list = propertyInfo.GetValue(controller, null);
                    List<int?> settingsList = (List<int?>)list;
                    Assert.AreEqual(settingsList.Count, 2);
                    for (int i = 0; i < settingsList.Count; i++)
                    {
                        Assert.AreEqual(settingsList[i], isChecked[i]);
                    }


                    // Test 2: Test RevisionSelctionTaxonStatusCheckBoxId ie select to show revisions assigned to a taxon and status preliminary, ongoing, closed including an additional preliminary,ongoing and closed.
                    //Arrange
                    isChecked = new int[] { Resources.DyntaxaSettings.Default.RevisionSelctionTaxonStatusCheckBoxId, Resources.DyntaxaSettings.Default.RevisionSelectAllStatusCheckBoxId, (int)TaxonRevisionStateId.Created, (int)TaxonRevisionStateId.Ongoing, (int)TaxonRevisionStateId.Closed };
                    //Act
                    listResult = controller.List(model, isChecked) as RedirectToRouteResult;

                    //Assert
                    Assert.IsNotNull(listResult);

                    // Test that List action is returned and correct checkboxes are selected.
                    Assert.AreEqual(refTaxonRevision.RootTaxon.Id.ToString(), listResult.RouteValues["taxonId"]);
                    Assert.AreEqual("List", listResult.RouteValues["action"]);
                    propertyInfo = controller.GetType().GetProperty("RevisionListSettings", bindingFlags);
                    list = propertyInfo.GetValue(controller, null);
                    settingsList = (List<int?>)list;

                    Assert.AreEqual(settingsList.Count, 4);
                    Assert.IsFalse(settingsList.Contains(Resources.DyntaxaSettings.Default.RevisionSelectAllStatusCheckBoxId));

                    for (int i = 0, j = 0; i < settingsList.Count; i++, j++)
                    {
                        if (!(isChecked[j] == Resources.DyntaxaSettings.Default.RevisionSelectAllStatusCheckBoxId))
                        {
                            Assert.AreEqual(settingsList[i], isChecked[j]);
                        }
                        else
                        {
                            j++;
                        }
                    }



                    // Test 3: Test RevisionSelctionTaxonStatusCheckBoxId ie select to show revisions assigned to a taxon and status all. (this should nver happen in real view)
                    //Arrange
                    isChecked = new int[] { Resources.DyntaxaSettings.Default.RevisionSelctionTaxonStatusCheckBoxId, Resources.DyntaxaSettings.Default.RevisionSelectAllStatusCheckBoxId };
                    //Act
                    listResult = controller.List(model, isChecked) as RedirectToRouteResult;

                    //Assert
                    Assert.IsNotNull(listResult);

                    // Test that List action is returned and correct checkboxes are selected.
                    Assert.AreEqual(refTaxonRevision.RootTaxon.Id.ToString(), listResult.RouteValues["taxonId"]);
                    Assert.AreEqual("List", listResult.RouteValues["action"]);
                    propertyInfo = controller.GetType().GetProperty("RevisionListSettings", bindingFlags);
                    list = propertyInfo.GetValue(controller, null);
                    settingsList = (List<int?>)list;

                    Assert.AreEqual(settingsList.Count, 1);
                    Assert.IsFalse(settingsList.Contains(Resources.DyntaxaSettings.Default.RevisionSelectAllStatusCheckBoxId));

                    for (int i = 0, j = 0; i < settingsList.Count; i++, j++)
                    {
                        if (!(isChecked[j] == Resources.DyntaxaSettings.Default.RevisionSelectAllStatusCheckBoxId))
                        {
                            Assert.AreEqual(settingsList[i], isChecked[j]);
                        }
                        else
                        {
                            j++;
                        }
                    }


                    // Test 4: Test  Show revisions not assigned to a taxon and status preliminary, ongoing, closed.
                    //Arrange
                    isChecked = new int[] { (int)TaxonRevisionStateId.Created, (int)TaxonRevisionStateId.Ongoing, (int)TaxonRevisionStateId.Closed };
                    //Act
                    listResult = controller.List(model, isChecked) as RedirectToRouteResult;

                    //Assert
                    Assert.IsNotNull(listResult);

                    // Test that List action is returned and correct checkboxes are selected.
                    Assert.AreEqual(refTaxonRevision.RootTaxon.Id.ToString(), listResult.RouteValues["taxonId"]);
                    Assert.AreEqual("List", listResult.RouteValues["action"]);
                    propertyInfo = controller.GetType().GetProperty("RevisionListSettings", bindingFlags);
                    list = propertyInfo.GetValue(controller, null);
                    settingsList = (List<int?>)list;

                    Assert.AreEqual(settingsList.Count, 3);
                    for (int i = 0; i < settingsList.Count; i++)
                    {
                        Assert.AreEqual(settingsList[i], isChecked[i]);
                    }

                    // Test 5: Test nothing to be shown.
                    //Arrange
                    isChecked = new int[] { };
                    //Act
                    listResult = controller.List(model, isChecked) as RedirectToRouteResult;

                    //Assert
                    Assert.IsNotNull(listResult);

                    // Test that List action is returned and correct checkboxes are selected.
                    Assert.AreEqual(refTaxonRevision.RootTaxon.Id.ToString(), listResult.RouteValues["taxonId"]);
                    Assert.AreEqual("List", listResult.RouteValues["action"]);
                    propertyInfo = controller.GetType().GetProperty("RevisionListSettings", bindingFlags);
                    list = propertyInfo.GetValue(controller, null);
                    settingsList = (List<int?>)list;

                    Assert.AreEqual(settingsList.Count, 0);

                    // Test 5: Test Test nothing to be shown and it shuld not crash.
                    //Arrange
                    isChecked = null;
                    //Act
                    listResult = controller.List(model, isChecked) as RedirectToRouteResult;

                    //Assert
                    Assert.IsNotNull(listResult);

                    // Test that List action is returned and correct checkboxes are selected.
                    Assert.AreEqual(refTaxonRevision.RootTaxon.Id.ToString(), listResult.RouteValues["taxonId"]);
                    Assert.AreEqual("List", listResult.RouteValues["action"]);
                    propertyInfo = controller.GetType().GetProperty("RevisionListSettings", bindingFlags);
                    list = propertyInfo.GetValue(controller, null);
                    settingsList = (List<int?>)list;

                    Assert.AreEqual(settingsList.Count, 0);
                }
            }

            /// <summary>
            /// A test for start editing a revision, get action.
            /// </summary>
            [TestMethod]
            [TestCategory("UnitTestApp")]
            [Ignore]
            public void StartEditingGetTest()
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();

                // Change userContext so that logged in user only has role for created ie ongoing revision revision
                UserContextData = ApplicationUserContext;
  
                using (ShimsContext.Create())
                {
                    // Arrange
                    RevisionController controller = new RevisionController();
                    string controllerName = "Revision";
                    string actionName = "StartEditing";
                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                    ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                    ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                    TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                    SessionRevision = refTaxonRevision;
                    SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                    SessionTaxonId = taxonIdentifier;
                    //Act
                    var result = controller.StartEditing() as ViewResult;

                    //Assert
                    Assert.IsNotNull(result);
                    Assert.AreEqual("Start", result.ViewName);

                    var model = result.ViewData.Model as RevisionCommonInfoViewModel;
                    Assert.IsNotNull(model);
                    Assert.AreEqual(Resources.DyntaxaResource.RevisionStartMainHeaderText, model.RevisionEditingActionHeaderText);
                    Assert.AreEqual(Resources.DyntaxaResource.RevisionStartEditingActionHeaderText, model.RevisionEditingHeaderText);
                    Assert.AreEqual(1, model.RevisionInfoItems.Count);
                    foreach (RevisionInfoItemModelHelper infoItems in model.RevisionInfoItems)
                    {
                        Assert.AreEqual(refTaxonRevision.Id.ToString(), infoItems.RevisionId);
                        Assert.AreEqual(refTaxonRevision.Description, infoItems.RevisionDescription);
                        Assert.AreEqual(Resources.DyntaxaResource.RevisionListSelectedRevisionStatusOngoingText, infoItems.RevisionStatus);
                        Assert.AreEqual(refTaxonRevision.ExpectedEndDate.ToShortDateString(), infoItems.ExpectedPublishingDate);
                        Assert.AreEqual(refTaxonRevision.ExpectedStartDate.ToShortDateString(), infoItems.ExpectedStartDate);

                        Assert.AreEqual(refTaxonRevision.RootTaxon.ScientificName, infoItems.ScientificName);
                        Assert.AreEqual(refTaxonRevision.RootTaxon.Id.ToString(), infoItems.TaxonId);
                        Assert.AreEqual(string.Empty, infoItems.SelectedRevisionForEditingText);
                        Assert.AreEqual(true, infoItems.ShowRevisionEditingButton);
                        Assert.AreEqual(true, infoItems.EnableRevisionEditingButton);


                    }

                    Assert.AreEqual("StartEditing", model.EditingAction);
                    Assert.AreEqual("Revision", model.EditingController);

                    // Test 2 test that button is not enabeled for user that already has the role have the revision role.
                    // Arrange
                    IUserContext userContext = UserContextData;
                    userContext.CurrentRole = UserDataSourceTestRepositoryData.GetTaxonRevisionRole("TestUserDyntaxaAdministratorRole", 1003, userContext, DyntaxaTestSettings.Default.TestRevisionOngoingGUID);
                    //Act
                    result = controller.StartEditing() as ViewResult;
                    model = result.ViewData.Model as RevisionCommonInfoViewModel;

                    //Assert
                    foreach (RevisionInfoItemModelHelper infoItems in model.RevisionInfoItems)
                    {
                        Assert.AreEqual(false, infoItems.EnableRevisionEditingButton);
                        Assert.AreNotEqual(string.Empty, infoItems.SelectedRevisionForEditingText);
                    }

                    // Test 3 test that button is not enabeled for user that don't have the revision role.
                    // Arrange
                   // TODO Remove autorities from UserContextData
                    //Act
                    result = controller.StartEditing() as ViewResult;
                    model = result.ViewData.Model as RevisionCommonInfoViewModel;

                    //Assert
                    foreach (RevisionInfoItemModelHelper infoItems in model.RevisionInfoItems)
                    {
                        Assert.AreEqual(false, infoItems.EnableRevisionEditingButton);
                    }
                }

            }


            /// <summary>
            /// A test for start editing a revision, post action.
            /// </summary>
            [TestMethod]
            [TestCategory("UnitTestApp")]
            [Ignore]
            public void StartEditingPostTest()
            {
                using (ShimsContext.Create())
                {
                    // Arrange
                    LoginApplicationUserAndSetSessionVariables();

                    // TODO this does not work; how do we set transactions to a user and shim them in code?
                    Transaction = new ShimTransaction()
                    {
                        Commit = () => { return; },
                    };
                    RevisionController controller = new RevisionController();
                    string controllerName = "Revision";
                    string actionName = "StartEditing";
                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName); 
                   
                    // Must create controller before we add roles..
                    ApplicationUserContext.CurrentRoles = UserDataSourceTestRepositoryData.GetUserRoles(ApplicationUserContext, DyntaxaTestSettings.Default.TestApplicationUserId, Resources.DyntaxaSettings.Default.DyntaxaApplicationIdentifier);
                    
                    // Change userContext so that logged in user only has role for created ie ongoing revision revision
                    UserContextData = ApplicationUserContext;
                   
                    RevisionInfoItemModelHelper itemHelper = new RevisionInfoItemModelHelper();
                    itemHelper.RevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId.ToString();
                    itemHelper.TaxonId = DyntaxaTestSettings.Default.TestTaxonId.ToString();

                    //Act
                    var result = controller.StartEditing(DyntaxaTestSettings.Default.TestRevisionOngoingId.ToString()) as ViewResult;

                    //Assert
                    Assert.IsNotNull(result);
                    // Test that correct action is returned.
                    Assert.AreEqual("Start", result.ViewName);
                    RevisionCommonInfoViewModel model = result.ViewData.Model as RevisionCommonInfoViewModel;

                    Assert.AreEqual(DyntaxaTestSettings.Default.TestRevisionOngoingId.ToString(), model.RevisionId);

                    // Check that usercontext has set to current user
                    IUserContext userContext = UserContextData;
                    Assert.IsNotNull(userContext.CurrentRole);
                    Assert.AreEqual(1003, userContext.CurrentRole.Id);
                }

            }

            /// <summary>
            /// A test for stop editing a revision, get action.
            /// </summary>
            [TestMethod]
            [TestCategory("UnitTestApp")]
            [Ignore]
            public void StopEditingGetTest()
            {
                using (ShimsContext.Create())
                {
                    // Arrange
                    LoginApplicationUserAndSetSessionVariables();

                    RevisionController controller = new RevisionController();
                    string controllerName = "Revision";
                    string actionName = "StopEditing";
                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                    // Must create controller before we add roles..
                    ApplicationUserContext.CurrentRoles = UserDataSourceTestRepositoryData.GetUserRoles(ApplicationUserContext, DyntaxaTestSettings.Default.TestApplicationUserId, Resources.DyntaxaSettings.Default.DyntaxaApplicationIdentifier);

                    ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                    ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                    TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                    SessionRevision = refTaxonRevision;
                    SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                    SessionTaxonId = taxonIdentifier;
                    
                    IUserContext userContext = UserContextData;
                    userContext.CurrentRole = UserDataSourceTestRepositoryData.GetTaxonRevisionRole("TestUserDyntaxaAdministratorRole", 1003, userContext, DyntaxaTestSettings.Default.TestRevisionOngoingGUID);
                    userContext.CurrentRoles.Add(UserDataSourceTestRepositoryData.GetTaxonRevisionRole("TestUserDyntaxaAdministratorRole", 1003, userContext, DyntaxaTestSettings.Default.TestRevisionOngoingGUID));

                    //Act
                    var result = controller.StopEditing() as ViewResult;

                    //Assert
                    Assert.IsNotNull(result);
                    Assert.AreEqual("CommonInfo", result.ViewName);

                    var model = result.ViewData.Model as RevisionCommonInfoViewModel;
                    Assert.IsNotNull(model);
                    Assert.AreEqual(Resources.DyntaxaResource.RevisionStopMainHeaderText, model.RevisionEditingActionHeaderText);
                    Assert.AreEqual(Resources.DyntaxaResource.RevisionStopEditingActionHeaderText, model.RevisionEditingHeaderText);
                    Assert.AreEqual(1, model.RevisionInfoItems.Count);
                    foreach (RevisionInfoItemModelHelper infoItems in model.RevisionInfoItems)
                    {
                        Assert.AreEqual(refTaxonRevision.Id.ToString(), infoItems.RevisionId);
                        Assert.AreEqual(refTaxonRevision.Description, infoItems.RevisionDescription);
                        Assert.AreEqual(Resources.DyntaxaResource.RevisionListSelectedRevisionStatusOngoingText, infoItems.RevisionStatus);
                        Assert.AreEqual(refTaxonRevision.ExpectedEndDate.ToShortDateString(), infoItems.ExpectedPublishingDate);
                        Assert.AreEqual(refTaxonRevision.ExpectedStartDate.ToShortDateString(), infoItems.ExpectedStartDate);

                        Assert.AreEqual(refTaxonRevision.RootTaxon.ScientificName, infoItems.ScientificName);
                        Assert.AreEqual(refTaxonRevision.RootTaxon.Id.ToString(), infoItems.TaxonId);
                        Assert.IsNull(infoItems.SelectedRevisionForEditingText);
                        Assert.AreEqual(true, infoItems.ShowRevisionEditingButton);
                        Assert.AreEqual(true, infoItems.EnableRevisionEditingButton);


                    }
                    Assert.AreEqual("StopEditing", model.EditingAction);
                    Assert.AreEqual("Revision", model.EditingController);

                    // Test 2 test that button is not enabeled for user that don't have the revision role.
                    // Arrange
                    //TODO reset roles to UserContextData

                    //Act
                    result = controller.StartEditing() as ViewResult;
                    model = result.ViewData.Model as RevisionCommonInfoViewModel;

                    //Assert
                    foreach (RevisionInfoItemModelHelper infoItems in model.RevisionInfoItems)
                    {
                        Assert.AreEqual(false, infoItems.EnableRevisionEditingButton);
                    }
                }

            }


            /// <summary>
            /// A test for stop editing a revision, post action.
            /// </summary>
            [TestMethod]
            [TestCategory("UnitTestApp")]
            [Ignore]
            public void StopEditingPostTest()
            {
                 using (ShimsContext.Create())
                {
                    // Arrange
                    LoginApplicationUserAndSetSessionVariables();

                    // TODO this does not work; how do we set transactions to a user and shim them in code?
                    Transaction = new ShimTransaction()
                    {
                        Commit = () => { return; },
                    };
                    RevisionController controller = new RevisionController();
                    string controllerName = "Revision";
                    string actionName = "StopEditing";
                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                    // Must create controller before we add roles..
                    ApplicationUserContext.CurrentRoles = UserDataSourceTestRepositoryData.GetUserRoles(ApplicationUserContext, DyntaxaTestSettings.Default.TestApplicationUserId, Resources.DyntaxaSettings.Default.DyntaxaApplicationIdentifier);
                    ApplicationUserContext.CurrentRole = UserDataSourceTestRepositoryData.GetTaxonRevisionRole("TestUserDyntaxaAdministratorRole", 1003, ApplicationUserContext, DyntaxaTestSettings.Default.TestRevisionOngoingGUID);
                    // Change userContext so that logged in user only has role for created ie ongoing revision revision
                    UserContextData = ApplicationUserContext;
                    
                    RevisionInfoItemModelHelper itemHelper = new RevisionInfoItemModelHelper();
                    itemHelper.RevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId.ToString();
                    itemHelper.TaxonId = DyntaxaTestSettings.Default.TestTaxonId.ToString();

                    // Act
                    var result = controller.StopEditing(itemHelper) as RedirectToRouteResult;

                    // Assert
                    Assert.IsNotNull(result);
                    // Test that correct action is returned.
                    Assert.AreEqual("List", result.RouteValues["action"]);
                }
            }

            /// <summary>
            /// A test for ListEvent
            /// </summary>
            [TestMethod]
            [TestCategory("UnitTestApp")]
            [Ignore]
            public void ListEventTest()
            {
                 using (ShimsContext.Create())
                {
                    // Arrange
                    LoginApplicationUserAndSetSessionVariables();

                    
                    RevisionController controller = new RevisionController();
                    string controllerName = "Revision";
                    string actionName = "ListEvent";
                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName);
                    
                     string revisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId.ToString(); 
                    string taxonId = DyntaxaTestSettings.Default.TestTaxonId.ToString(); 
                    string expected = "ListEvent"; 
                    ViewResult actual;
                    actual = controller.ListEvent(revisionId, taxonId) as ViewResult;
                    Assert.AreEqual(expected, actual.ViewName);
                 }
            }

            /// <summary>
            /// This is awesome, now it is possible to test all routes...
            /// </summary>
            [TestMethod]
            [TestCategory("UnitTestApp")]
            [Ignore]
            public void RevisionRoutesTest()
            {
                using (ShimsContext.Create())
                {
                    // Arrange
                    LoginApplicationUserAndSetSessionVariables();

                    RevisionController controller = new RevisionController();
                    string controllerName = "Revision";
                    string actionName = "Info";
                    controller.ControllerContext = GetShimControllerContext(actionName, controllerName);


                    // TODO how to test should map to in fakes 2014-04-14
                    //MvcApplication.RegisterRoutes(RouteTable.Routes);
                    //"~/Revision/List/233146".ShouldMapTo<RevisionController>(x => x.List("233146", null, null));
                    //"~/Revision/Add/1008669".ShouldMapTo<RevisionController>(x => x.Add("1008669"));
                    //"~/Revision/StartEditing/1".ShouldMapTo<RevisionController>(x => x.StartEditing("1"));

                    //// Tests when a revision has been selected to edit

                    //// Must create controller before we add roles..
                    //ApplicationUserContext.CurrentRoles = UserDataSourceTestRepositoryData.GetUserRoles(ApplicationUserContext, DyntaxaTestSettings.Default.TestApplicationUserId, Resources.DyntaxaSettings.Default.DyntaxaApplicationIdentifier);
                    //// Change userContext so that logged in user only has role for created ie ongoing revision revision
                    //SessionHelper.SetInSession("userContext", ApplicationUserContext);
                    //SessionHelper.SetInSession("applicationUserContext", UserContext);

                    //RevisionInfoItemModelHelper itemHelper = new RevisionInfoItemModelHelper();
                    //itemHelper.RevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId.ToString();
                    //itemHelper.TaxonId = DyntaxaTestSettings.Default.TestTaxonId.ToString();

                    ////Act
                    //var result = controller.StartEditing(DyntaxaTestSettings.Default.TestRevisionOngoingId.ToString()) as ViewResult;
                    //"~/Revision/ListEvent".ShouldMapTo<RevisionController>(x => x.ListEvent(DyntaxaTestSettings.Default.TestRevisionOngoingId.ToString(),
                    //                                                       DyntaxaTestSettings.Default.TestTaxonId.ToString()));
                    //// Not working properly test must be rewritten since a boolean cant be null (code should be changed to bool? then everything will work agoh
                    //// 2012-05-11  "~/Revision/Edit".ShouldMapTo<RevisionController>(x => x.Edit(DyntaxaTestSettings.Default.TestRevisionOngoingId.ToString(), null, false));
                    //"~/Revision/StartEditing".ShouldMapTo<RevisionController>(x => x.StartEditing(DyntaxaTestSettings.Default.TestRevisionOngoingId.ToString()));
                    //"~/Revision/StopEditing".ShouldMapTo<RevisionController>(x => x.StopEditing(DyntaxaTestSettings.Default.TestRevisionOngoingId.ToString()));
                }
            }
        }
}

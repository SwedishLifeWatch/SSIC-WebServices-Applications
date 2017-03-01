using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;
using ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonName;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;
using Dyntaxa.Controllers;
using Dyntaxa.Test.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dyntaxa.Tests.Controllers
{
    using ArtDatabanken.Data.Fakes;

    using Dyntaxa.Test;

    using Microsoft.QualityTools.Testing.Fakes;

    [TestClass]
    public class TaxonNameControllerUnitTests : ControllerUnitTestBase
    {
        
        [TestMethod]
        [TestCategory("UnitTestApp")]
        [Ignore]
        public void DeleteGetTest()
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
                
                TaxonNameController controller = new TaxonNameController();
                string controllerName = "TaxonName";
                string actionName = "Delete";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                ITaxonName refTaxonName = TaxonDataSourceTestRepositoryData.GetReferenceTaxonName(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, DyntaxaTestSettings.Default.TestTaxonNameId);
                ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                SessionRevision = refTaxonRevision;
                SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                SessionTaxonId = taxonIdentifier;

                var deleteResult = controller.Delete(DyntaxaTestSettings.Default.TestTaxonId.ToString(), refTaxonName.Id) as ViewResult;

                Assert.IsNotNull(deleteResult);
                var deleteViewModel = deleteResult.ViewData.Model as TaxonNameDeleteViewModel;

                Assert.IsNotNull(deleteViewModel);
                Assert.AreEqual(refTaxonName.Name, deleteViewModel.Name);
                Assert.IsTrue(deleteViewModel.IsRecommended);

                // Assert.AreEqual(DyntaxaTestSettings.Default.TestRevisionOngoingId, deleteViewModel.RevisionId);
                Assert.AreEqual(refTaxonName.Taxon.Id.ToString(), deleteViewModel.TaxonId.ToString());
                Assert.AreEqual(refTaxonName.Description, deleteViewModel.Comment);

            }

        }


        /// <summary>
        ///A test for Add ie create a new Taxon Name (Get action)
        /// The test verifies that correct views is returned and that AddTaxonNameViewModel have correct values..
        ///</summary>
        [TestMethod]
        [TestCategory("UnitTestApp")]
        [Ignore]
        public void AddGetTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                // Arrange
                LoginApplicationUserAndSetSessionVariables();

                
                TaxonNameController controller = new TaxonNameController();
                string controllerName = "TaxonName";
                string actionName = "Add";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                ITaxonName refTaxonName = TaxonDataSourceTestRepositoryData.GetReferenceTaxonName(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, DyntaxaTestSettings.Default.TestTaxonNameId);
                ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                SessionRevision = refTaxonRevision;
                SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                SessionTaxonId = taxonIdentifier;

                // Test 1: no category id is selected
                //Act
                int? categoryId = null;
                var addResult = controller.Add(DyntaxaTestSettings.Default.TestTaxonId.ToString(), categoryId) as ViewResult;
                //Assert
                Assert.IsNotNull(addResult);

                var addViewModel = addResult.ViewData.Model as TaxonNameDetailsViewModel;

                Assert.IsNotNull(addViewModel);
                // Test that correct view is returned
                Assert.AreEqual("Add", addResult.ViewName);
                // Test model values
                Assert.AreEqual(null, addViewModel.Author);
                Assert.AreEqual(7, addViewModel.CategoryList.Count);
                Assert.AreEqual(null, addViewModel.Comment);
                Assert.IsFalse(addViewModel.IsNotOkForObsSystem);
                Assert.IsFalse(addViewModel.IsRecommended);
                Assert.AreEqual(null, addViewModel.Name);
                //  Assert.AreEqual(DyntaxaTestSettings.Default.TestRevisionOngoingId, addViewModel.RevisionId);
                Assert.AreEqual(refTaxonName.Category.Id, addViewModel.SelectedCategoryId + 1);
                Assert.AreEqual(refTaxonName.Status.Id, addViewModel.SelectedTaxonNameStatusId + 1);
                Assert.AreEqual(refTaxonName.Taxon.Id.ToString(), addViewModel.TaxonId.ToString());
                Assert.AreEqual(6, addViewModel.TaxonNameStatusList.Count);


                // Test 2: category id is set
                //Act
                categoryId = 2;
                addResult = controller.Add(DyntaxaTestSettings.Default.TestTaxonId.ToString(), categoryId) as ViewResult;
                //Assert
                Assert.IsNotNull(addResult);

                addViewModel = addResult.ViewData.Model as TaxonNameDetailsViewModel;

                Assert.IsNotNull(addViewModel);
                // Test that correct view is returned
                Assert.AreEqual("Add", addResult.ViewName);
                // Test model values
                Assert.AreEqual(7, addViewModel.CategoryList.Count);
                Assert.AreEqual(categoryId, addViewModel.SelectedCategoryId);
            }

        }

        /// <summary>
        ///A test for Add ie create a new taxonName (Post action)
        /// The test verifies redirect to route is returned.
        ///</summary>
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
                
                //Arrange
                TaxonNameController controller = new TaxonNameController();
                string controllerName = "TaxonName";
                string actionName = "Add";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                ITaxonName refTaxonName = TaxonDataSourceTestRepositoryData.GetReferenceTaxonName(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, DyntaxaTestSettings.Default.TestTaxonNameId);
                ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                SessionRevision = refTaxonRevision;
                SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                SessionTaxonId = taxonIdentifier;

                // Create model....
                TaxonNameDetailsViewModel model = new TaxonNameDetailsViewModel();
                // model.RevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                model.TaxonId = DyntaxaTestSettings.Default.TestTaxonId;
                // This model properties must be set otherwise a failure will occure. TODO test this later!!!!!!!!!
                model.SelectedCategoryId = 1;
                model.IsRecommended = true;
               


                // Test 1: Model state is valid
                //Act
                var addResult = controller.Add(model) as RedirectToRouteResult;

                //Assert
                Assert.IsNotNull(addResult);

                // Test that Edit action is returned.
                Assert.AreEqual(DyntaxaTestSettings.Default.TestTaxonId, addResult.RouteValues["taxonId"]);
                Assert.AreEqual("Edit", addResult.RouteValues["action"]);

                // Test 2: Model state is invalid
                //Act
                // Set model state to be invalid
                controller.ModelState.AddModelError("", "dummy error message");
                var addViewResult = controller.Add(model) as ViewResult;

                //Assert
                Assert.IsNotNull(addViewResult);
                // Test that correct view is returned ie th add view is reloaded...
                Assert.AreEqual("Add", addViewResult.ViewName);

                var addViewModel = addViewResult.ViewData.Model as TaxonNameDetailsViewModel;

                Assert.IsNotNull(addViewModel);
                // Test model values.
                Assert.AreEqual(DyntaxaTestSettings.Default.TestTaxonId, addViewModel.TaxonId);
            }
     
        }


        /// <summary>
        ///A test for all actions in the controller to verify that correct authorization attributes are set.
        /// Ie checks the role of user so that actins not allowed is unavaliabe.
        ///</summary>
        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void AuthorizationTest()
        {
            using (ShimsContext.Create())
            {
                //Arrange
                // Arrange
                LoginApplicationUserAndSetSessionVariables();

                
                TaxonNameController controller = new TaxonNameController();
                controller.ControllerContext = GetShimControllerContext("", "");

                var type = controller.GetType();


                // 1. Test that action add (get) has correct authority..
                //Act
                var methodInfo = type.GetMethod("Add", new Type[] { typeof(string), typeof(int?) });
                var attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                //Assert
                Assert.AreEqual(1, attributes.Length);
                Assert.AreEqual(RequiredAuthorization.TaxonRevisionEditor, ((DyntaxaAuthorizeAttribute)attributes[0]).Order);

                // 2. Test that action add (post) has correct authority..
                //Act
                methodInfo = type.GetMethod("Add", new Type[] { typeof(TaxonNameDetailsViewModel) });
                attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                //Assert
                Assert.AreEqual(1, attributes.Length);
                Assert.AreEqual(RequiredAuthorization.TaxonRevisionEditor, ((DyntaxaAuthorizeAttribute)attributes[0]).Order);

                // 3. Test that action edit (get) has correct authority..
                //Act
                methodInfo = type.GetMethod("Edit", new Type[] { typeof(string), typeof(string) });
                attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                //Assert
                Assert.AreEqual(1, attributes.Length);
                Assert.AreEqual(RequiredAuthorization.TaxonRevisionEditor, ((DyntaxaAuthorizeAttribute)attributes[0]).Order);

                // 4. Test that action edit (post) has correct authority..
                //Act
                methodInfo = type.GetMethod("Edit", new Type[] { typeof(TaxonNameDetailsViewModel) });
                attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                //Assert
                Assert.AreEqual(1, attributes.Length);
                Assert.AreEqual(RequiredAuthorization.TaxonRevisionEditor, ((DyntaxaAuthorizeAttribute)attributes[0]).Order);


                // 5. Test that action list (get) has correct authority..
                //Act
                methodInfo = type.GetMethod("List", new Type[] { typeof(string) });
                attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                //Assert
                Assert.AreEqual(RequiredAuthorization.TaxonRevisionEditor, ((DyntaxaAuthorizeAttribute)attributes[0]).Order);

                // 6. Test that action List (post) has correct authority..
                //Act
                methodInfo = type.GetMethod("List", new Type[] { typeof(string[]), typeof(string[]) });
                attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                //Assert
                //Assert
                Assert.AreEqual(RequiredAuthorization.TaxonRevisionEditor, ((DyntaxaAuthorizeAttribute)attributes[0]).Order);


                // 7. Test that action delete (get) has correct authority..
                //Act
                methodInfo = type.GetMethod("Delete", new Type[] { typeof(string), typeof(int) });
                attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                //Assert
                //Assert
                Assert.AreEqual(1, attributes.Length);
                Assert.AreEqual(RequiredAuthorization.TaxonRevisionEditor, ((DyntaxaAuthorizeAttribute)attributes[0]).Order);

                // 8. Test that action delete (post) has correct authority..
                //Act
                methodInfo = type.GetMethod("Delete", new Type[] { typeof(int) });
                attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);
                //Assert
                Assert.AreEqual(1, attributes.Length);
                Assert.AreEqual(RequiredAuthorization.TaxonRevisionEditor, ((DyntaxaAuthorizeAttribute)attributes[0]).Order);
            }

        }


        /// <summary>
        /// A test for Edit ie update selected taxon name (Get action)
        /// The test verifies that correct views is returned and that EditTaxonNameViewModel have correct values..
        ///</summary>
        [TestMethod]
        [TestCategory("UnitTestApp")]
        [Ignore]
        public void EditGetTest()
        {

            using (ShimsContext.Create())
            {
                
                //Arrange
                // Arrange
                LoginApplicationUserAndSetSessionVariables();

                TaxonNameController controller = new TaxonNameController();
                string controllerName = "TaxonName";
                string actionName = "Edit";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                ITaxonName refTaxonName = TaxonDataSourceTestRepositoryData.GetReferenceTaxonName(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, DyntaxaTestSettings.Default.TestTaxonNameId);
                ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                SessionRevision = refTaxonRevision;
                SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                SessionTaxonId = taxonIdentifier;

                //Act
                var editResult = controller.Edit(DyntaxaTestSettings.Default.TestTaxonId.ToString(), refTaxonName.Id.ToString()) as ViewResult;
                //Assert
                Assert.IsNotNull(editResult);

                var editViewModel = editResult.ViewData.Model as TaxonNameDetailsViewModel;

                Assert.IsNotNull(editViewModel);
                // Test that correct view is returned
                Assert.AreEqual("Edit", editResult.ViewName);
                // Test model values, test first name
                Assert.AreEqual(refTaxon.GetTaxonNames(CoreData.UserManager.GetCurrentUser())[0].Author, editViewModel.Author);
                Assert.AreEqual(7, editViewModel.CategoryList.Count);
                Assert.AreEqual(refTaxon.GetTaxonNames(CoreData.UserManager.GetCurrentUser())[0].Description, editViewModel.Comment);
                Assert.IsFalse(editViewModel.IsNotOkForObsSystem);
                Assert.IsTrue(editViewModel.IsRecommended);
                Assert.AreEqual(refTaxon.GetTaxonNames(CoreData.UserManager.GetCurrentUser())[0].Name, editViewModel.Name);
                //  Assert.AreEqual(DyntaxaTestSettings.Default.TestRevisionOngoingId, editViewModel.RevisionId);
                Assert.AreEqual(refTaxon.GetTaxonNames(CoreData.UserManager.GetCurrentUser())[0].Category.Id, editViewModel.SelectedCategoryId);
                Assert.AreEqual(refTaxon.GetTaxonNames(CoreData.UserManager.GetCurrentUser())[0].Status.Id, editViewModel.SelectedTaxonNameStatusId);
                Assert.AreEqual(refTaxon.GetTaxonNames(CoreData.UserManager.GetCurrentUser())[0].Taxon.Id.ToString(), editViewModel.TaxonId.ToString());
                Assert.AreEqual(6, editViewModel.TaxonNameStatusList.Count);
            }
        }

        /// <summary>
        /// A test for Edit ie update taxon name (Post action)
        /// The test verifies that correct views or redirect to route is returned, also tested that revision id and taxon is set.
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
                TaxonNameController controller = new TaxonNameController();
                string controllerName = "Taxon";
                string actionName = "Edit";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                ITaxonName refTaxonName = TaxonDataSourceTestRepositoryData.GetReferenceTaxonName(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, DyntaxaTestSettings.Default.TestTaxonNameId);
                ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                SessionRevision = refTaxonRevision;
                SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                SessionTaxonId = taxonIdentifier;

                // Create model....
                TaxonNameDetailsViewModel model = new TaxonNameDetailsViewModel();
                model.TaxonId = DyntaxaTestSettings.Default.TestTaxonId;
                model.Version = DyntaxaTestSettings.Default.TestTaxonNameId.ToString();
                 // This model properties must be set otherwise a failure will occure. 
                model.SelectedCategoryId = 1;
                model.IsRecommended = true;
                int taxonNameId = DyntaxaTestSettings.Default.TestTaxonNameId;


                // Test 1: Model state is valid
                //Act
                var editResult = controller.Edit(model) as RedirectToRouteResult;

                //Assert
                Assert.IsNotNull(editResult);

                // Test that Edit action is returned.
                Assert.AreEqual(DyntaxaTestSettings.Default.TestTaxonId, editResult.RouteValues["taxonId"]);
                Assert.AreEqual("Edit", editResult.RouteValues["action"]);

                // Test 2: Model state is invalid
                //Act
                // Set model state to be invalid
                controller.ModelState.AddModelError("", "dummy error message");
                var editViewResult = controller.Edit(model) as ViewResult;

                //Assert
                Assert.IsNotNull(editViewResult);
                // Test that correct view is returned ie th add view is reloaded...
                Assert.AreEqual("Edit", editViewResult.ViewName);

                var editViewModel = editViewResult.ViewData.Model as TaxonNameDetailsViewModel;

                Assert.IsNotNull(editViewModel);
                // Test tmodel values..
                Assert.AreEqual(DyntaxaTestSettings.Default.TestTaxonId, editViewModel.TaxonId);
                //    Assert.AreEqual(DyntaxaTestSettings.Default.TestRevisionOngoingId, editViewModel.RevisionId);
            }

        }

        /// <summary>
        /// A test for Edit ie list all taxon names (Get action)
        /// The test verifies that correct views is returned and that ListTaxonNameViewModel have correct values..
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
 
                TaxonNameController controller = new TaxonNameController();
                string controllerName = "TaxonName";
                string actionName = "List";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                ITaxonName refTaxonName = TaxonDataSourceTestRepositoryData.GetReferenceTaxonName(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, DyntaxaTestSettings.Default.TestTaxonNameId);
                ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                SessionRevision = refTaxonRevision;
                SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                SessionTaxonId = taxonIdentifier;

                //Act
                var listResult = controller.List(DyntaxaTestSettings.Default.TestTaxonId.ToString()) as ViewResult;

                //Assert
                Assert.IsNotNull(listResult);

                var listViewModel = listResult.ViewData.Model as ListTaxonNameViewModel;

                Assert.IsNotNull(listViewModel);
                // Test that correct view is returned
                Assert.AreEqual("List", listResult.ViewName);
                // Test model values
                Assert.IsNotNull(listViewModel.NameCategories);
                foreach (TaxonNameCategoryViewModel categoryViewModel in listViewModel.NameCategories)
                {
                    for (int i = 0; i < categoryViewModel.Names.Count; i++)
                    {
                        Assert.IsNotNull(categoryViewModel.Names[i].Version);
                        Assert.AreEqual(refTaxon.GetTaxonNames(CoreData.UserManager.GetCurrentUser())[i].Id, categoryViewModel.Names[i].Version);
                        Assert.AreEqual(refTaxon.GetTaxonNames(CoreData.UserManager.GetCurrentUser())[i].Name, categoryViewModel.Names[i].Name);
                    }
                }

                Assert.AreEqual(refTaxon.GetTaxonNames(CoreData.UserManager.GetCurrentUser())[0].Taxon.Id.ToString(), listViewModel.TaxonId.ToString());
            }
        }

        /// <summary>
        /// A test for List ie update taxon name list (Post action)
        /// The test verifies that correct views or redirect to route is returned.
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
                TaxonNameController controller = new TaxonNameController();
                string controllerName = "TaxonName";
                string actionName = "List";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                ITaxonName refTaxonName = TaxonDataSourceTestRepositoryData.GetReferenceTaxonName(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, DyntaxaTestSettings.Default.TestTaxonNameId);
                ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                SessionRevision = refTaxonRevision;
                SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                SessionTaxonId = taxonIdentifier;

                string[] isNotOkForObs = new string[] { refTaxon.GetTaxonNames(CoreData.UserManager.GetCurrentUser())[0].Category.Id + ";" + refTaxon.GetTaxonNames(CoreData.UserManager.GetCurrentUser())[0].Id };
                string[] isRecommended = new string[] { refTaxon.GetTaxonNames(CoreData.UserManager.GetCurrentUser())[0].Category.Id + ";" + refTaxon.GetTaxonNames(CoreData.UserManager.GetCurrentUser())[0].Id };

                // Act
                var listResult = controller.List(isNotOkForObs, isRecommended) as RedirectToRouteResult;

                // Assert
                Assert.IsNotNull(listResult);

                // Test that Edit action is returned.
                Assert.AreEqual("List", listResult.RouteValues["action"]);
            }
        }
    }
}

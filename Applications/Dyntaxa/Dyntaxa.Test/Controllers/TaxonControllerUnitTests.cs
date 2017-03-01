using System;
using System.Collections.Generic;

using System.Web.Mvc;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;
using Dyntaxa.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.QualityTools.Testing.Fakes;


namespace Dyntaxa.Test.Controllers
{
    using ArtDatabanken.Data.Fakes;
    using ArtDatabanken.WebService.Client;

    using Dyntaxa.Test;

    [TestClass]
    public class TaxonControllerUnitTests : ControllerUnitTestBase
    {
     

        #region Additional test attributes
      

        #endregion


        /// <summary>
        /// A test for Add ie create a new Taxon  (Get action)
        /// The test verifies that correct views is returned and that AddTaxonViewModel have correct values..
        /// </summary>
        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void AddGetTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                TaxonController controller = new TaxonController();
                string controllerName = "Taxon";
                string actionName = "Add";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);
                SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                  
                // Act
                var addResult = controller.Add(DyntaxaTestSettings.Default.TestTaxonId.ToString()) as ViewResult;
                var addViewModel = addResult.ViewData.Model as TaxonAddViewModel;


                // Assert
                Assert.IsNotNull(addResult);

                Assert.IsNotNull(addViewModel);

                // Test that correct view is returned
                Assert.AreEqual("Add", addResult.ViewName);

                // Test model values
                Assert.AreEqual(DyntaxaTestSettings.Default.TestTaxonId, Convert.ToInt32(addViewModel.ParentTaxonId));
                Assert.AreEqual(String.Empty, addViewModel.Author);
                Assert.AreEqual(15, addViewModel.TaxonCategoryList.Count);
                Assert.AreEqual(String.Empty, addViewModel.Description);
                Assert.AreEqual(String.Empty, addViewModel.CommonName);
              
            }
        }

        /// <summary>
        /// A test for Add ie create a new taxonName (Post action)
        /// The test verifies redirect to route is returned.
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

                TaxonController controller = new TaxonController();
                string controllerName = "Taxon";
                string actionName = "Add";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);
                
                SessionRevision = refTaxonRevision;
                SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                SessionTaxonId = taxonIdentifier;

                // Create model....
                TaxonAddViewModel model = new TaxonAddViewModel();
                model.ParentTaxonId = DyntaxaTestSettings.Default.TestParentTaxonId.ToString();
                model.TaxonCategoryId = 14;
                model.ScientificName = "New Parent";

                // Test 1: Model state is valid
                // Act
                var result = controller.Add(model) as RedirectToRouteResult; 

                // Test that correct view is returned
                Assert.IsNotNull(result);

                Assert.AreEqual(DyntaxaTestSettings.Default.TestTaxonId.ToString(), result.RouteValues["taxonId"]);
                Assert.AreEqual("Edit", result.RouteValues["action"]);

                // Test 2: Model state is invalid
                // Act
                // Set model state to be invalid
                controller.ModelState.AddModelError("", "dummy error message");
                var addResult = controller.Add(model) as ViewResult;
                var addViewModel = addResult.ViewData.Model as TaxonAddViewModel;


                // Assert
                Assert.IsNotNull(addResult);

                // Test that correct view is returned ie th add view is reloaded...
                Assert.AreEqual("Add", addResult.ViewName);

                Assert.IsNotNull(addViewModel);

                // Test that Imodel values.
                Assert.AreEqual(DyntaxaTestSettings.Default.TestParentTaxonId.ToString(), addViewModel.ParentTaxonId);
            }
          
        }

        /// <summary>
        /// A test for Add Parent ie create a new parent Taxon  to selected taxon (Get action)
        /// The test verifies that correct views is returned and that AddParentTaxonViewModel have correct values..
        /// </summary>
        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void AddParentGetTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                TaxonController controller = new TaxonController();
                string controllerName = "Taxon";
                string actionName = "AddParent";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);
                
                SessionRevision = refTaxonRevision;
                SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                SessionTaxonId = taxonIdentifier;
                // Mock Controller
                //           builder.InitializeController(controller);

                // Test 1: 
                // Act
                var addParentResult = controller.AddParent(DyntaxaTestSettings.Default.TestTaxonId.ToString()) as ViewResult;
                var addParentViewModel = addParentResult.ViewData.Model as TaxonAddParentViewModel;


                // Assert
                Assert.IsNotNull(addParentResult);
                Assert.IsNotNull(addParentViewModel);

                // Test that correct view is returned
                Assert.AreEqual("AddParent", addParentResult.ViewName);

                // Test model values
                // TODO test more model values....

                Assert.IsNull(addParentViewModel.SelectedTaxonList);
                Assert.AreEqual(1, addParentViewModel.AvailableParents.Count);
                Assert.AreEqual(refTaxon.Id.ToString(), addParentViewModel.TaxonId);
                Assert.AreEqual(refTaxonRevision.Id.ToString(), addParentViewModel.RevisionId);

                foreach (TaxonParentViewModelHelper helper in addParentViewModel.TaxonList)
                {
                    Assert.IsNotNull(helper.Category);
                    Assert.IsNotNull(helper.CommonName);
                    Assert.IsNotNull(helper.SortOrder);
                    Assert.IsNotNull(helper.TaxonId);
                    Assert.IsNotNull(helper.ScientificName);
                }
            }
        }



        /// <summary>
        /// A test for Add Parent ie create a new parent taxon to selected taxon (Post action)
        /// The test verifies redirect to route is returned.
        /// </summary>
        [TestMethod]
        [TestCategory("UnitTestApp")]
        [Ignore]
        public void AddParentPostTest()
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
                TaxonController controller = new TaxonController();
                string controllerName = "Taxon";
                string actionName = "AddParent";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                SessionRevision = refTaxonRevision;
                SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                SessionTaxonId = taxonIdentifier;


                // Mock Controller
                //            builder.InitializeController(controller);
                // Create model....
                TaxonAddParentViewModel model = new TaxonAddParentViewModel();
                model.TaxonId = DyntaxaTestSettings.Default.TestTaxonId.ToString();
                model.RevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId.ToString();
                model.SelectedTaxonList = new List<string>() { DyntaxaTestSettings.Default.TestParentTaxonId.ToString() };


                // Act
                var addParentResult = controller.AddParent(model) as RedirectToRouteResult;

                // Test 1: Test Save button pressed

                // Assert
                Assert.IsNotNull(addParentResult);

                // Test that AddParent action is returned.
                Assert.AreEqual(refTaxon.Id.ToString(), addParentResult.RouteValues["taxonId"]);
                Assert.AreEqual("AddParent", addParentResult.RouteValues["action"]);

                // Test 2: No new parent seleceted reload view
                // Act
                model.SelectedTaxonList = null;
                var addParentResult2 = controller.AddParent(model) as RedirectToRouteResult;


                // Assert
                Assert.IsNotNull(addParentResult2);

                // Test that AddParent action is returned.
                Assert.AreEqual(refTaxon.Id.ToString(), addParentResult2.RouteValues["taxonId"]);
                Assert.AreEqual("AddParent", addParentResult.RouteValues["action"]);
            }
        }


        /// <summary>
        /// A test for all actions in the controller to verify that correct authorization attributes are set.
        /// Ie checks the role of user so that actions not allowed is unavaliabe.
        /// </summary>
        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void AuthorizationTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                TaxonController controller = new TaxonController();
                string controllerName = "Taxon";
                string actionName = "Authorization";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                var type = controller.GetType();


                // 1. Test that action add (get) has correct authority..
                // Act
                var methodInfo = type.GetMethod("Add", new Type[] { typeof(string), typeof(bool) });
                var attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);

                // Assert
                Assert.AreEqual(1, attributes.Length);
                Assert.AreEqual(RequiredAuthorization.TaxonRevisionEditor, ((DyntaxaAuthorizeAttribute)attributes[0]).Order);

                // 2. Test that action add (post) has correct authority..
                // Act
                methodInfo = type.GetMethod("Add", new Type[] { typeof(TaxonAddViewModel) });
                attributes = methodInfo.GetCustomAttributes(typeof(DyntaxaAuthorizeAttribute), true);

                // Assert
                Assert.AreEqual(1, attributes.Length);
                Assert.AreEqual(RequiredAuthorization.TaxonRevisionEditor, ((DyntaxaAuthorizeAttribute)attributes[0]).Order);
            }
        }

        /// <summary>
        ///  A test for Drop Parent ie delete a parent taxon from selected taxon  (Get action)
        /// The test verifies that correct views is returned and that DropParentTaxonViewModel have correct values..
        /// </summary>
        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void DropParentGetTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                LoginApplicationUserAndSetSessionVariables();
                TaxonController controller = new TaxonController();
                string controllerName = "Taxon";
                string actionName = "DropParent";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                SessionRevision = refTaxonRevision;
                SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                SessionTaxonId = taxonIdentifier;

                // Mock Controller
                //        builder.InitializeController(controller);

                // Test 1: 
                // Act
                var dropParentResult = controller.DropParent(DyntaxaTestSettings.Default.TestTaxonId.ToString()) as ViewResult;
                var dropParentViewModel = dropParentResult.ViewData.Model as TaxonDropParentViewModel;


                // Assert
                Assert.IsNotNull(dropParentResult);
                Assert.IsNotNull(dropParentViewModel);

                // Test that correct view is returned.
                Assert.AreEqual("DropParent", dropParentResult.ViewName);

                // Test model values
                // TODO test more model values....

                Assert.IsNull(dropParentViewModel.SelectedTaxonList);
                Assert.AreEqual(1, dropParentViewModel.TaxonList.Count);
                Assert.AreEqual(refTaxon.Id.ToString(), dropParentViewModel.TaxonId);
                Assert.IsFalse(dropParentViewModel.EnableSaveDeleteParentTaxonButton);
                Assert.AreEqual(refTaxonRevision.Id.ToString(), dropParentViewModel.RevisionId);

                foreach (TaxonParentViewModelHelper helper in dropParentViewModel.TaxonList)
                {
                    Assert.IsNotNull(helper.Category);
                    Assert.IsNotNull(helper.CommonName);
                    Assert.IsNotNull(helper.SortOrder);
                    Assert.IsNotNull(helper.TaxonId);
                    Assert.IsNotNull(helper.ScientificName);
                }
            }
        }

        /// <summary>
        /// A test for Drop Parent ie delete a parent taxon from selected taxon (Post action)
        /// The test verifies redirect to route is returned.
        /// </summary>
        [TestMethod]
        [TestCategory("UnitTestApp")]
        [Ignore]
        public void DropParentPostTest()
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
                TaxonController controller = new TaxonController();
                string controllerName = "Taxon";
                string actionName = "DropParent";
                controller.ControllerContext = GetShimControllerContext(actionName, controllerName);

                ITaxonRevision refTaxonRevision = TaxonDataSourceTestRepositoryData.GetReferenceRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId, TaxonRevisionStateId.Ongoing.ToString());
                ITaxon refTaxon = TaxonDataSourceTestRepositoryData.GetReferenceTaxon(ApplicationUserContext, DyntaxaTestSettings.Default.TestTaxonId);
                TaxonIdTuple taxonIdentifier = TaxonIdTuple.Create(refTaxon.ScientificName, refTaxon.Id);

                SessionRevision = refTaxonRevision;
                SessionRevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId;
                SessionTaxonId = taxonIdentifier;
                refTaxon.GetNearestParentTaxonRelations(UserContextData).Add(new TaxonRelation()
                {
                    ReplacedInTaxonRevisionEventId = null,
                    CreatedBy = 1,
                    CreatedDate = new DateTime(2011),
                    ValidToDate = new DateTime(2022),
                    IsMainRelation = false,
                    Id = 55555
                });


                // Mock Controller
                //        builder.InitializeController(controller);
                // Create model....
                TaxonDropParentViewModel model = new TaxonDropParentViewModel();
                model.TaxonId = DyntaxaTestSettings.Default.TestTaxonId.ToString();
                model.RevisionId = DyntaxaTestSettings.Default.TestRevisionOngoingId.ToString();
                model.SelectedTaxonList = new List<string>() { "55555" };


                var addParentResult = controller.DropParent(model) as RedirectToRouteResult;

                // Test 1: Test Save button pressed

                // Assert
                Assert.IsNotNull(addParentResult);

                // Test that AddParent action is returned.
                Assert.AreEqual(refTaxon.Id.ToString(), addParentResult.RouteValues["TaxonId"]);
                Assert.AreEqual("DropParent", addParentResult.RouteValues["action"]);

                // Test 2: No new parent seleceted reload view
                // Act
                model.SelectedTaxonList = null;
                var addParentResult2 = controller.DropParent(model) as RedirectToRouteResult;

                // Assert
                Assert.IsNotNull(addParentResult2);

                // Test that AddParent action is returned.
                Assert.AreEqual(refTaxon.Id.ToString(), addParentResult2.RouteValues["taxonId"]);
                Assert.AreEqual("DropParent", addParentResult.RouteValues["action"]);
            }
        }

        /// <summary>
        /// A test for Edit ie update selected taxon (Get action)
        /// The test verifies that correct views is returned and that EditTaxonViewModel have correct values..
        /// </summary>
        [TestMethod]
        public void EditGetTest()
        {
            // TODO implement test
        }


        /// <summary>
        /// A test for Edit ie update taxon  (Post action)
        /// The test verifies that correct views or redirect to route is returned, also tested that revision id and taxon is set.
        /// </summary>
        [TestMethod]
        public void EditPostTest()
        {
            // TODO implement test
        }
    }
}

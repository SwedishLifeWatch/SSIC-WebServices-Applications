using System;
using System.Collections;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.DyntaxaInternalService;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Reference;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers.Extensions;
using ArtDatabanken.WebService.Client.ReferenceService;
using ArtDatabanken.WebService.Client.TaxonAttributeService;
using ArtDatabanken.WebService.Proxy;
using Resources;
using ITransaction = ArtDatabanken.Data.ITransaction;

namespace Dyntaxa.Controllers
{
    public class RevisionController : DyntaxaBaseController
    {
        // Called "LIVE"
        public RevisionController()
        {
        }

        // Called by test
        public RevisionController(IUserDataSource userDataSourceRepository, ITaxonDataSource taxonDataSourceRepository, IReferenceDataSource referenceDataSource, ISessionHelper session)
            : base(userDataSourceRepository, taxonDataSourceRepository, session)
        {
            CoreData.ReferenceManager.DataSource = referenceDataSource;
        }

        /// <summary>
        /// Create a new revision. This is only valid for Taxon revision administrators.
        /// </summary>
        /// <param name="taxonId"> Selected taxon id to performe revision on.</param>
        /// <returns></returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionAdministrator, ChangeCurrentRole = false)]
        public ActionResult Add(string taxonId)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            try
            {
                // Gets taxon if not redirected to taxon search and when taxon found we continue with add.
                TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
                if (searchResult.NumberOfMatches != 1)
                {
                    return RedirectToSearch(taxonId, null);
                }

                this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);                
                ITaxon taxon = searchResult.Taxon;
                ViewBag.Taxon = taxon;
                // Get dyntaxa users.
                IUserContext loggedInUser = GetLoggedInUser();
                IUserContext dyntaxaApplicationUserContext = GetApplicationUser();

                int roleId;
                List<RevisionUserItemModelHelper> temp = GetAllTaxonEditors(dyntaxaApplicationUserContext, out roleId);

                RevisionModelManager modelManger = new RevisionModelManager();
                RevisionAddViewModel model = modelManger.GetRevisionAddViewModel(loggedInUser, dyntaxaApplicationUserContext, taxonId, taxon, temp);

                if (model.ErrorMessage.IsNotNull())
                {
                    errorMsg = model.ErrorMessage;
                    ModelState.AddModelError(string.Empty, errorMsg);
                    // Links and dropdowns cant be null..
                    modelManger.InitUserListsAndLinks(model);
                }
                ViewData.Model = model;
                  
                return View("Add", model);
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = e.Message;
               additionalErrorMsg = e.StackTrace;
               ModelState.AddModelError(string.Empty, errorMsg);
            }
            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.RevisionAddActionHeaderText,
                Resources.DyntaxaResource.RevisionAddActionHeaderText, 
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// POST: Creates a revision and roles and authorities needed. Only valid for taxon revision administators.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionAdministrator, ChangeCurrentRole = false)]
        [HttpPost]
        public ActionResult Add(RevisionAddViewModel model)
        {
            bool revisionCreated = false;
            bool roleCreated = false;
            ITaxonRevision createdTaxonRevision = null;
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            try
            {
                RevisionModelManager modelManger = new RevisionModelManager();
                if (ModelState.IsValid)
                {
                    bool isValid = ModelState.IsValid;

                    // Get user and dyntaxa application user.
                    IUserContext loggedInUser = GetLoggedInUser();                    
                    IUserContext dyntaxaApplicationUserContext = GetApplicationUser();
                    IUserContext applicationTransactionUser = GetApplicationTransactionUser();

                    // Check users 
                    bool userTest = CheckUserContextValidity(loggedInUser);
                    if (!userTest)
                    {
                        isValid = false;
                    }

                    bool dyntaxaUserTest = CheckDyntaxaApplicationUserContextValidity(dyntaxaApplicationUserContext);
                    if (!dyntaxaUserTest)
                    {
                        isValid = false;
                    }

                    //Get taxon id
                    int taxonId = Int32.Parse(model.TaxonId);
                        
                    // Get taxon and create a temporary role out of taxon revion data
                    ITaxon revTaxon = CoreData.TaxonManager.GetTaxon(loggedInUser, taxonId);
                    string taxonName = revTaxon.ScientificName;

                    // Example on what to do server validation on, might not be relevent in this case though
                    bool taxonTest = CheckTaxonVaildity(model.TaxonId, revTaxon);
                    if (!taxonTest)
                    {
                        isValid = false;
                    }

                    if (isValid)
                    {
                        // Get start and end date
                        DateTime startDate = model.ExpectedStartDate;
                        DateTime endDate = model.ExpectedPublishingDate;
                        DateTime endDateForRoles = model.ExpectedPublishingDate.AddYears(100);

                        //Get selected users by user id for this revision
                        int[] selectedUsersForRevision = model.SelectedUsers;

                        // Set locale to logged in user.
                        dyntaxaApplicationUserContext.Locale = loggedInUser.Locale;

                        ITaxonRevision taxonRevision = null;
                           
                            // Start creation of what is needed to start a revision
                            try
                            {
                                // Set inital revision data
                                taxonRevision = modelManger.InitRevision(loggedInUser, revTaxon, model.RevisionDescription, startDate, endDate);
                                //Create revision
                                int transactionTimeout = 900;
                                using (ITransaction transaction = loggedInUser.StartTransaction(transactionTimeout))
                                {
                                    CoreData.TaxonManager.UpdateTaxonRevision(loggedInUser, taxonRevision);
                                    transaction.Commit();
                                }
                            
                                //List<IReferenceRelation> referencesToAdd = ReferenceHelper.GetDyntaxaRevisionReferenceRelation(revision);
                                //var referencesToRemove = new List<IReferenceRelation>();
                                //CoreData.TaxonManager.CreateRemoveReferenceRelation(loggedInUser, referencesToAdd, referencesToRemove);
                            }
                            catch (ArgumentException argumentException)
                            {
                                DyntaxaLogger.WriteException(argumentException);
                                errorMsg = Resources.DyntaxaResource.RevisionAddErrorInformation + " " + model.RevisionId;
                                additionalErrorMsg = argumentException.Message;
                                var errorCreateModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
                                ErrorViewModel errorCreateModel = errorCreateModelManger.GetErrorViewModel(
                                    Resources.DyntaxaResource.RevisionAddActionHeaderText,
                                    Resources.DyntaxaResource.RevisionAddActionHeaderText,
                                    errorMsg, 
                                    model.TaxonId, 
                                    model.RevisionId, 
                                    additionalErrorMsg);
                                return View("ErrorInfo", errorCreateModel);
                            }

                            if (taxonRevision.IsNotNull())
                            {
                                // Everything is done that belongs to created revision now commit db changes...
                                revisionCreated = true;
                                createdTaxonRevision = taxonRevision;
                                model.RevisionId = taxonRevision.Id.ToString();
                            }

                            // Create roles and authorities needed
                            if (taxonRevision.IsNotNull() && revisionCreated)
                            {
                                DyntaxaLogger.WriteMessage("RevisionController.Add (POST) - innan CreateRoleAndAuthiotyForRevision(...)");
                                // Start creation of what is needed to start a revision roles and authorities
                                bool reloadContext = CreateRoleAndAuthiotyForRevision(startDate, selectedUsersForRevision, loggedInUser, applicationTransactionUser, endDateForRoles, taxonRevision, taxonName, out roleCreated);
                                WebServiceProxy.ReferenceService.ClearCache(new ReferenceDataSource().GetClientInformation(dyntaxaApplicationUserContext));
                                WebServiceProxy.TaxonAttributeService.ClearCache(new TaxonAttributeDataSource().GetClientInformation(dyntaxaApplicationUserContext));
                            DyntaxaLogger.WriteMessage("RevisionController.Add (POST) - efter CreateRoleAndAuthiotyForRevision(...)");
                            // Reload loggedInUser context since roles has been changed.
                            if (reloadContext)
                                {
                                    // Must clear the service cash so that roles can be reloded.
                                    // CoreData.TaxonManager.ClearCacheForUserRoles(loggedInUser);
                                    //Update our user with new roles
                                    CoreData.UserManager.UpdateUserRoles(loggedInUser.User.Id);
                                }
                                // Reload tree.
                                var id = this.RootTaxonId;
                                if (id != null)
                            {
                                this.RedrawTree((int)id, taxonRevision.RootTaxon.Id);
                            }

                            // Set new taxon as identifer
                            this.TaxonIdentifier = TaxonIdTuple.CreateFromId(taxonRevision.RootTaxon.Id);

                                return RedirectToAction("Edit", new { revisionId = taxonRevision.Id, taxonId = taxonRevision.RootTaxon.Id, isRevisionNew = true });
                            }
                            else
                            {
                                errorMsg = Resources.DyntaxaResource.RevisionAddInvalidRevisionErrorText;
                                ModelState.AddModelError("", errorMsg);
                                // Links and dropdowns cant be null..
                                modelManger.InitUserListsAndLinks(model);
                            }
                        }
                    }
                    // Model state not valid...
                    else
                    {
                        ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                        // Links and dropdowns cant be null..
                        modelManger.InitUserListsAndLinks(model);
                    }
                    return View("Add", model);
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                // If we fail in creating roles, we must close the revision. TODO how to renove a revision
                if (revisionCreated && !roleCreated)
                {
                    errorMsg = Resources.DyntaxaResource.SharedRevisionNoRolesCreatedErrorInformation;
                }
                else
                {
                    errorMsg = e.Message;
                    additionalErrorMsg = e.StackTrace;
                }
            }

            var errorModelManger = new ErrorModelManager(
                new Exception(), 
                RouteData.Values["controller"].ToString(), 
                RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.RevisionAddActionHeaderText,
                Resources.DyntaxaResource.RevisionAddActionHeaderText,
                errorMsg, 
                model.TaxonId, 
                model.RevisionId, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// Renders a view where the user can remove a revision.
        /// </summary>
        /// <param name="revisionId"></param>
        /// <returns></returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionAdministrator, ChangeCurrentRole = false)]
        public ActionResult Delete(string revisionId)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            try
            {
                IUserContext currentUser = GetCurrentUser();
                RevisionCommonInfoViewModel model = new RevisionCommonInfoViewModel();
                RevisionModelManager modelManager = new RevisionModelManager();
                if (currentUser.IsNotNull())
                {
                    if (revisionId != null)
                    {
                        IList<RevisionInfoItemModelHelper> revisionInfos = new List<RevisionInfoItemModelHelper>();
                        ITaxonRevision taxonRevision = CoreData.TaxonManager.GetTaxonRevision(currentUser, Int32.Parse(revisionId));
                        if (taxonRevision.IsNotNull())
                        {
                            ITaxon revisionTaxon = taxonRevision.RootTaxon;

                            RevisionInfoItemModelHelper infoItem = new RevisionInfoItemModelHelper();
                            infoItem = modelManager.GetRevisionInformation(currentUser, revisionTaxon, taxonRevision, infoItem);

                            infoItem.ShowRevisionEditingButton = true;
                            infoItem.EnableRevisionEditingButton = true;
                            infoItem.RevisionEditingButtonText = Resources.DyntaxaResource.SharedDeleteButtonText;
                            infoItem.RevisionWaitingLabel = " " + Resources.DyntaxaResource.SharedDeleteButtonText
                                             + " " + Resources.DyntaxaResource.SharedRevisionText + " " + Resources.DyntaxaResource.SharedRevisionIdLabel + ": "
                                             + taxonRevision.Id.ToString();
                            infoItem.ShowRevisionInformation = true;
                            infoItem.EditingAction = "Delete";
                            infoItem.EditingController = "Revision";

                            revisionInfos.Add(infoItem);

                            model.EditingAction = "Delete";
                            model.EditingController = "Revision";
                            model.RevisionInfoItems = revisionInfos;

                            model.RevisionId = taxonRevision.Id.ToString();
                            model.TaxonId = revisionTaxon.Id.ToString();
                            model.RevisionEditingHeaderText = Resources.DyntaxaResource.RevisionDeleteMainHeaderText;
                            model.RevisionEditingActionHeaderText = Resources.DyntaxaResource.RevisionDeleteActionHeaderText;
                            model.Submit = false;

                            model.DialogTextPopUpText = Resources.DyntaxaResource.SharedRevisionNoValidRevisionErrorText;
                            model.DialogTitlePopUpText = Resources.DyntaxaResource.RevisionDeleteMainHeaderText;

                            // Assign view data to model...
                            ViewData.Model = model;
                        }
                        else
                        {
                            model.ErrorMessage = Resources.DyntaxaResource.RevisionSharedNoValidRevisionIdErrorText + taxonRevision.Id + ".";
                        }
                    }
                    else
                    {
                        model.ErrorMessage = Resources.DyntaxaResource.SharedRevisionNoValidRevisionErrorText;
                    }
                }
                else
                {
                    model.ErrorMessage = Resources.DyntaxaResource.SharedInvalidUserContext;
                }

                if (model.ErrorMessage.IsNotNull())
                {
                    errorMsg = model.ErrorMessage;
                    ModelState.AddModelError(string.Empty, errorMsg);
                }
                ViewData.Model = model;
                model.Submit = false;
                return View("CommonInfo", model);
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
                ModelState.AddModelError(string.Empty, errorMsg);
            }
            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.RevisionDeleteActionHeaderText,
                Resources.DyntaxaResource.RevisionDeleteActionHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// Called when a user wants to delete a Revision.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionAdministrator, ChangeCurrentRole = false)]
        [HttpPost]
        public ActionResult Delete(RevisionCommonInfoViewModel model)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            try
            {
                if (ModelState.IsValid)
                {
                    bool isValid = ModelState.IsValid;

                    // Get user and dyntaxa application user.
                    IUserContext loggedInUser = GetLoggedInUser();
                    IUserContext dyntaxaApplicationUserContext = GetApplicationUser();
                    IUserContext applicationTransactionUser = GetApplicationTransactionUser();

                    // Check users 
                    bool userTest = CheckUserContextValidity(loggedInUser);
                    if (!userTest)
                    {
                        isValid = false;
                    }

                    bool dyntaxaUserTest = CheckDyntaxaApplicationUserContextValidity(dyntaxaApplicationUserContext);
                    if (!dyntaxaUserTest)
                    {
                        isValid = false;
                    }

                    //Get taxon id
                    int revId = Int32.Parse(model.RevisionId);

                    // Get revision
                    ITaxonRevision taxonRevision = CoreData.TaxonManager.GetTaxonRevision(loggedInUser, revId);

                    bool revisionTest = CheckRevisionValidity(model.RevisionId, taxonRevision);
                    if (!revisionTest)
                    {
                        isValid = false;
                    }

                    if (isValid)
                    {
                        if (revId != 0)
                        {
                            if (taxonRevision.IsNotNull() && (taxonRevision.State.Id == (int)TaxonRevisionStateId.Created))
                            {
                                using (ITransaction transaction = loggedInUser.StartTransaction())
                                {
                                    CoreData.TaxonManager.DeleteTaxonRevision(loggedInUser, taxonRevision);
                                    transaction.Commit();
                                }
                                
                                // Remove created role..
                                RoleSearchCriteria roleSearch = new RoleSearchCriteria();
                                roleSearch.Identifier = "%" + taxonRevision.Guid + "%";
                                RoleList roleList = CoreData.UserManager.GetRolesBySearchCriteria(dyntaxaApplicationUserContext, roleSearch);
                                if (roleList.IsNotNull() && roleList.Count == 1)
                                {
                                    using (ITransaction transactionUserAdmin = applicationTransactionUser.StartTransaction())
                                    {
                                        CoreData.UserManager.DeleteRole(applicationTransactionUser, roleList[0]);
                                        transactionUserAdmin.Commit();
                                    }
                                }
                                return RedirectToAction("List");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", Resources.DyntaxaResource.RevisionAddInvalidRevisionErrorText);
                        }
                    }
                }
                // Model state not valid...
                else
                {
                    ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                }
                return View("CommonInfo");
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.RevisionDeleteActionHeaderText,
                Resources.DyntaxaResource.RevisionDeleteActionHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary> 
        /// Performe edit on a revision. Only valid for taxon revision administators.
        /// </summary>
        /// <param name="revisionId"></param>
        /// <returns></returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionAdministrator, ChangeCurrentRole = false)]
        public ActionResult Edit(string revisionId, string taxonId = null,  bool isRevisionNew = false)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            try
            {
               int? revisionIdTemp;
               if (revisionId.IsNotNull())
               {
                   revisionIdTemp = Int32.Parse(revisionId);
                   this.RevisionHandlingId = revisionIdTemp;
               }
               else
               {
                   revisionIdTemp = this.RevisionHandlingId;
               }

                if (taxonId.IsNotNull())
                {
                    this.TaxonIdentifier = TaxonIdTuple.CreateFromId(Int32.Parse(taxonId));
                    // Reload tree if not from Add, since add reloade the tree before sending it to edit.
                    if (!isRevisionNew)
                    {
                        var id = this.RootTaxonId;
                        if (id != null)
                        {
                            this.RedrawTree((int)id, Int32.Parse(taxonId));
                        }
                    }
                }

                // Get user and dyntaxa application user.
                IUserContext loggedInUser = GetLoggedInUser();
                IUserContext dyntaxaApplicationUserContext = GetApplicationUser();

                RevisionModelManager modelManager = new RevisionModelManager();
                ITaxonRevision taxonRevision = CoreData.TaxonManager.GetTaxonRevision(loggedInUser, revisionIdTemp.Value);
                ITaxon taxon = CoreData.TaxonManager.GetTaxon(loggedInUser, taxonRevision.RootTaxon.Id);

                bool taxonInRevision = false;
                int state = taxonRevision.State.Id;
                if (state == (int)TaxonRevisionStateId.Created)
                {
                    taxonInRevision = taxon.IsInRevision;
                }
                // Get all users that are allowed to to edit taxon
                int roleId;
                IList<RevisionUserItemModelHelper> revisionUsers = GetTaxonEditorsForSpecificRevision(dyntaxaApplicationUserContext, taxonRevision);
                IList<RevisionUserItemModelHelper> allUsers = GetAllTaxonEditors(dyntaxaApplicationUserContext, out roleId);
                RevisionEditViewModel model = modelManager.GetRevisionEditViewModel(
                    loggedInUser, dyntaxaApplicationUserContext, taxonRevision, taxon, taxonInRevision, revisionUsers, allUsers, roleId);

                // Set selected taxon
                model.TaxonId = this.TaxonIdentifier.Id.ToString();
                if (model.ErrorMessage.IsNotNull())
                {
                    errorMsg = model.ErrorMessage;
                    ModelState.AddModelError(string.Empty, errorMsg);
                    // List can't be null
                    if (model.UserList.IsNull())
                    {
                        model.UserList = new List<RevisionUserItemModelHelper>();
                    }
                }

                ViewData.Model = model;
                return View("Edit", model);
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;               
            }
            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.RevisionEditActionHeaderText,
                Resources.DyntaxaResource.RevisionEditActionHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// POST: Edit revision with new data save the revision with new data. Only valid for taxon revision administators.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionAdministrator, ChangeCurrentRole = false)]
        [HttpPost]
        public ActionResult Edit(RevisionEditViewModel model, string buttonClicked = null)
        {
            string errorMsg = string.Empty;
            bool revisionCreated = false;
            bool rolesUpdated = false;
            string additionalErrorMsg = null;
            bool isSpeciecFactOk = true;
            RevisionModelManager modelManager = new RevisionModelManager();            
            try
            {
                // Get user and dyntaxa application user.
                IUserContext loggedInUser = GetLoggedInUser();
                IUserContext dyntaxaApplicationUserContext = GetApplicationUser();
                IUserContext applicationTransactionUserContext = GetApplicationTransactionUser();
                // List and links can't be null if an error occurs
                modelManager.InitUserListsAndLinks(model);
                int roleId;
                IList<RevisionUserItemModelHelper> allUsers = GetAllTaxonEditors(dyntaxaApplicationUserContext, out roleId);
                if (ModelState.IsValid)
                {
                    bool isValid = ModelState.IsValid;
                         
                    // Check users 
                    bool userTest = CheckUserContextValidity(loggedInUser);
                    if (!userTest)
                    {
                        isValid = false;
                    }

                    bool dyntaxaUserTest = CheckDyntaxaApplicationUserContextValidity(dyntaxaApplicationUserContext);
                    if (!dyntaxaUserTest)
                    {
                        isValid = false;
                    }

                    // Get revision and check
                    int revisionId = Int32.Parse(model.RevisionId);
                    ITaxonRevision taxonRevision = CoreData.TaxonManager.GetTaxonRevision(loggedInUser, revisionId);

                    bool revisionTest = CheckRevisionValidity(model.RevisionId, taxonRevision);
                    if (!revisionTest)
                    {
                        isValid = false;
                    }

                    //Get taxon id
                    ITaxon taxon = CoreData.TaxonManager.GetTaxon(loggedInUser, taxonRevision.RootTaxon.Id);
                    // Check taxon and revision validity
                    bool taxonTest = CheckTaxonVaildity(model.RevisionTaxonId, taxon);
                    if (!taxonTest)
                    {
                        isValid = false;
                    }

                    if (isValid)
                    {
                        //Get selected users by user id for this revision
                        int[] selectedUsersForRevision = model.SelectedUsers;
                        IList<RevisionUserItemModelHelper> revisionUsers = GetTaxonEditorsForSpecificRevision(dyntaxaApplicationUserContext, taxonRevision);
                        // Set locale to logged in user.
                        dyntaxaApplicationUserContext.Locale = loggedInUser.Locale;

                        taxonRevision.ExpectedStartDate = model.ExpectedStartDate;
                        taxonRevision.ExpectedEndDate = model.ExpectedPublishingDate;
                        taxonRevision.Description = model.RevisionDescription;

                        //Create revision
                        int transactionTimeout = 900;
                        using (ITransaction transaction = loggedInUser.StartTransaction(transactionTimeout))
                        {
                            // Save revision first
                            CoreData.TaxonManager.UpdateTaxonRevision(loggedInUser, taxonRevision);
                            transaction.Commit();
                        }
                              
                        // Update speciesfact data
                        try
                        {
                            modelManager.UpdateSpeciesFact(loggedInUser, model, taxon);
                        }
                        catch (Exception ex)
                        {
                            DyntaxaLogger.WriteException(ex);
                            errorMsg = Resources.DyntaxaResource.SharedNotPossibleToUpdateSpeciesFactError;
                            ModelState.AddModelError("", errorMsg);
                            isSpeciecFactOk = false;
                        }

                        if (taxonRevision.IsNotNull())
                        {
                            revisionCreated = true;
                            
                            // First we get users that have correct role
                            RoleSearchCriteria roleSearch = new RoleSearchCriteria();
                            roleSearch.Identifier = "%" + taxonRevision.Guid + "%";
                            RoleList roleList = CoreData.UserManager.GetRolesBySearchCriteria(dyntaxaApplicationUserContext, roleSearch);

                            //#if DEBUG
                            //// MK 2016-01-21 - This is used in Debug mode for creating a user role for a user that is not current in the revision.
                            //if (!roleList.Any())
                            //{
                            //    bool roleCreated = false;
                            //    // Get start and end date
                            //    DateTime startDate = model.ExpectedStartDate;
                            //    DateTime endDate = model.ExpectedPublishingDate;
                            //    DateTime endDateForRoles = model.ExpectedPublishingDate.AddYears(100);
                            //    ITaxon revTaxon = taxonRevision.RootTaxon;
                            //    string taxonName = revTaxon.ScientificName;

                            //    bool reloadContext = CreateRoleAndAuthiotyForRevision(startDate, selectedUsersForRevision, loggedInUser, dyntaxaApplicationUserContext, endDateForRoles, taxonRevision, taxonName, out roleCreated);
                            //    WebServiceProxy.ReferenceService.ClearCache(new ReferenceDataSource().GetClientInformation(dyntaxaApplicationUserContext));
                            //    WebServiceProxy.TaxonAttributeService.ClearCache(new TaxonAttributeDataSource().GetClientInformation(dyntaxaApplicationUserContext));
                            //    // Reload loggedInUser context since roles has been changed.
                            //    if (reloadContext)
                            //    {
                            //        // Must clear the service cash so that roles can be reloded.
                            //        // CoreData.TaxonManager.ClearCacheForUserRoles(loggedInUser);
                            //        //Update our user with new roles
                            //        CoreData.UserManager.UpdateUserRoles(loggedInUser.User.Id);
                            //    }
                            //    roleList = CoreData.UserManager.GetRolesBySearchCriteria(dyntaxaApplicationUserContext, roleSearch);
                            //}                            
                            //#endif

                            // Get role belonging to revision
                            if (roleList.IsNotNull() && roleList.Count == 1)
                            {
                                DyntaxaLogger.WriteMessage("RevisionController.Edit (POST) - innan UpdateRoleAndAuthorityForRevision(...)");
                                UserList usersWithRole = CoreData.UserManager.GetUsersByRole(dyntaxaApplicationUserContext, roleList[0].Id);
                                UserList usersToBeRemovedFromRole = usersWithRole;                                
                                bool reloadContext = UpdateRoleAndAuthorityForRevision(usersToBeRemovedFromRole, roleList, loggedInUser, selectedUsersForRevision, applicationTransactionUserContext, usersWithRole);
                                WebServiceProxy.ReferenceService.ClearCache(new ReferenceDataSource().GetClientInformation(dyntaxaApplicationUserContext));
                                WebServiceProxy.TaxonAttributeService.ClearCache(new TaxonAttributeDataSource().GetClientInformation(dyntaxaApplicationUserContext));
                                DyntaxaLogger.WriteMessage("RevisionController.Edit (POST) - efter UpdateRoleAndAuthorityForRevision(...)");
                                // Reload loggedInUser context since roles has been changed.
                                if (reloadContext)
                                {
                                    // Must clear the service cash so that roles can be reloded.
                                    // CoreData.TaxonManager.ClearCacheForUserRoles(loggedInUser);
                                    //Update our user with new roles
                                    CoreData.UserManager.UpdateUserRoles(loggedInUser.User.Id);
                                    rolesUpdated = true;
                                }
                                 revisionUsers = GetTaxonEditorsForSpecificRevision(dyntaxaApplicationUserContext, taxonRevision);
                                 int roleId2;
                                 allUsers = GetAllTaxonEditors(dyntaxaApplicationUserContext, out roleId2);

                               if (isSpeciecFactOk)
                                {
                                    // Check returning button
                                    if (buttonClicked == model.Labels.GetSelectedSave)
                                    {
                                        return RedirectToAction("Edit", new { revisionId = taxonRevision.Id });
                                    }
                                   else if (buttonClicked == model.Labels.GetSelectedInitialize)
                                   {
                                        if (selectedUsersForRevision.IsNull() || selectedUsersForRevision.Length == 0)
                                        {
                                            errorMsg = Resources.DyntaxaResource.RevisionAddNoUsersErrorInformation;
                                            ModelState.AddModelError("", errorMsg);
                                            modelManager.ReloadRevisionEditViewModel(loggedInUser, dyntaxaApplicationUserContext, taxon, taxonRevision, model, revisionUsers, allUsers);
                                        }
                                        else
                                        {
                                            return RedirectToAction("Initialize", new { revisionId = taxonRevision.Id.ToString() });
                                        }
                                    }
                                    else if (buttonClicked == model.Labels.GetSelectedFinalize)
                                    {
                                        if (selectedUsersForRevision.IsNull() || selectedUsersForRevision.Length == 0)
                                        {
                                            errorMsg = Resources.DyntaxaResource.RevisionEditNoUsersFinalizeErrorInformation;
                                            ModelState.AddModelError("", errorMsg);
                                            modelManager.ReloadRevisionEditViewModel(loggedInUser, dyntaxaApplicationUserContext, taxon, taxonRevision, model, revisionUsers, allUsers);
                                        }
                                        else
                                        {
                                            return RedirectToAction("Finalize", new { revisionId = taxonRevision.Id });
                                        }
                                    }
                                    else
                                    {
                                        return RedirectToAction("Edit", new { revisionId = taxonRevision.Id });
                                    }
                                }
                            }
                            else
                            {
                                modelManager.ReloadRevisionEditViewModel(loggedInUser, dyntaxaApplicationUserContext, taxon, taxonRevision, model, revisionUsers, allUsers);
                                 errorMsg = Resources.DyntaxaResource.RevisionEditUpdateUsersErrorText;
                                ModelState.AddModelError("", errorMsg);
                            }
                        }
                        else
                        {
                            modelManager.ReloadRevisionEditViewModel(loggedInUser, dyntaxaApplicationUserContext, taxon, taxonRevision, model, revisionUsers, allUsers);
                             errorMsg = Resources.DyntaxaResource.RevisionEditUpdateRevisionErrorText;
                            ModelState.AddModelError("", errorMsg);
                        }
                    }
                    else
                    {
                        modelManager.ReloadRevisionEditViewModel(loggedInUser, dyntaxaApplicationUserContext, taxon, taxonRevision, model, null, allUsers);
                        ModelState.AddModelError("", Resources.DyntaxaResource.RevisionAddInvalidRevisionErrorText);
                    }
                }

                // Model state not valid... and not reference error 
                else
                {
                    modelManager.ReloadRevisionEditViewModel(loggedInUser, dyntaxaApplicationUserContext, null, null, model, null, allUsers);
                    ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                }
                return View("Edit", model);
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);                
                // If we fail in creating roles, we must close the revision.
                if (revisionCreated && !rolesUpdated)
                {
                    errorMsg = Resources.DyntaxaResource.SharedRevisionNoRolesCreatedErrorInformation;
                }
                else
                {
                    errorMsg = e.Message;
                    additionalErrorMsg = e.StackTrace;
                }
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.RevisionEditActionHeaderText,
                Resources.DyntaxaResource.RevisionEditActionHeaderText,
                errorMsg, 
                model.RevisionTaxonId, 
                model.RevisionId, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// POST: Change revision status to published. Only valid for taxon revision administators.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionAdministrator, ChangeCurrentRole = false)]
        public ActionResult Finalize(string revisionId)
        {
            var finalizeSteps = new[,] { { false, false }, { false, false }, { false, false }, { false, false } };
            var revId = 0;
            var errorMsg = string.Empty;
            var additionalErrorMsg = string.Empty;
            try
            {
                // Get user and dyntaxa application user.
                var loggedInUser = GetLoggedInUser();
                var dyntaxaApplicationUserContext = GetApplicationUser();
                IUserContext applicationTransactionUser = GetApplicationTransactionUser();
                if (loggedInUser.IsNotNull() && dyntaxaApplicationUserContext.IsNotNull())
                {
                    if (revisionId.IsNotNull())
                    {
                        revId = int.Parse(revisionId);
                        if (revId == 0)
                        {
                            // revision not saved go back, should never get here though..
                            errorMsg = DyntaxaResource.RevisionFinalizeErrorInformation;
                        }
                        else
                        {
                            //Finalize revision ie set revision state to closed
                            ITaxonRevision taxonRevision = CoreData.TaxonManager.GetTaxonRevision(loggedInUser, revId);
                           
                            if (taxonRevision.IsNotNull() && taxonRevision.State.Id == (int)TaxonRevisionStateId.Ongoing)
                            {
                                finalizeSteps[0, 0] = true;
                                // Start a transaction w/ 5 min timeout.
                                int transactionTimeout = 300;
                                using (ITransaction transaction = loggedInUser.StartTransaction(transactionTimeout))
                                {
                                    CoreData.TaxonManager.CheckInTaxonRevision(loggedInUser, taxonRevision);
                                    transaction.Commit();
                                }
                                CacheManager.FireRefreshCache(loggedInUser);

                                finalizeSteps[0, 1] = true;
                                DyntaxaLogger.WriteMessage("Revision " + revId + " is checked in.");                                
                            }

                            var taxonServiceManager = new DyntaxaInternalTaxonServiceManager();

                            if (!taxonRevision.IsSpeciesFactPublished)
                            {
                                finalizeSteps[1, 0] = true;
                                
                                var revisionSpeciesFacts = taxonServiceManager.GetAllDyntaxaRevisionSpeciesFacts(loggedInUser, revId);

                                //Check if we have any changes in Swedish history or Swedish occurence
                                if (revisionSpeciesFacts.Any())
                                {
                                    //Get changed taxa
                                    var taxonIds = (from f in revisionSpeciesFacts select f.TaxonId).Distinct().ToList();
                                    var changedTaxa = CoreData.TaxonManager.GetTaxa(loggedInUser, taxonIds);

                                    //Loop throw all changed taxa
                                    foreach (var taxon in changedTaxa)
                                    {
                                        //Get a species fact model manager
                                        var speciesFactModelManager = new SpeciesFactModelManager(taxon, loggedInUser);
                                        SpeciesFactList changedSpeciesFactList = new SpeciesFactList();

                                        //Get facts changed for this taxon
                                        var specieFacts = revisionSpeciesFacts.Where(f => f.TaxonId == taxon.Id);

                                        //Lopp throw changes and update model
                                        foreach (var fact in specieFacts)
                                        {
                                            // Check if species fact was both added and removed in this revision => do nothing.
                                            if (!fact.StatusId.HasValue && !fact.SpeciesFactExists)
                                            {
                                                continue;
                                            }

                                            switch ((FactorId)fact.FactorId)
                                            {                                             
                                                case FactorId.SwedishOccurrence:
                                                    speciesFactModelManager.SwedishOccurrenceId = fact.StatusId;
                                                    speciesFactModelManager.SwedishOccurrenceQualityId = fact.QualityId;
                                                    speciesFactModelManager.SwedishOccurrenceReferenceId = fact.ReferenceId;
                                                    speciesFactModelManager.SwedishOccurrenceDescription = fact.Description;
                                                    changedSpeciesFactList.Add(speciesFactModelManager.SwedishOccurrenceSpeciesFact);
                                                    break;
                                                case FactorId.SwedishHistory:
                                                    speciesFactModelManager.SwedishHistoryId = fact.StatusId;
                                                    speciesFactModelManager.SwedishHistoryQualityId = fact.QualityId;
                                                    speciesFactModelManager.SwedishHistoryReferenceId = fact.ReferenceId;
                                                    speciesFactModelManager.SwedishHistoryDescription = fact.Description;
                                                    changedSpeciesFactList.Add(speciesFactModelManager.SwedishHistorySpeciesFact);

                                                    break;
                                            }
                                        }
                                        
                                        //Save changes to Artfakta
                                        speciesFactModelManager.UpdateDyntaxaSpeciesFacts(changedSpeciesFactList);
                                    }
                                }
                                
                                finalizeSteps[1, 1] = true;
                                finalizeSteps[2, 0] = true;
                                using (ITransaction transaction = loggedInUser.StartTransaction(30))
                                { //Save information about artfakta updated
                                    taxonServiceManager.SetRevisionSpeciesFactPublished(loggedInUser, revId);
                                    transaction.Commit();
                                }
                                finalizeSteps[2, 1] = true;
                            }

                            if (!taxonRevision.IsReferenceRelationsPublished)
                            {
                                // todo
                                // * lägg till kod för att föra över ändringar av referensrelationer till Artfakta
                                // * om allt lyckas anropa då taxonServiceManager.SetRevisionReferenceRelationsPublished
                                // * uppdatera finalizeSteps[...,...]
                            }

                            finalizeSteps[3, 0] = true;
                            
                            // First we get users that have correct role
                            var roleSearch = new RoleSearchCriteria();
                            roleSearch.Identifier = "%" + taxonRevision.Guid + "%";
                            DyntaxaLogger.WriteMessage("Calling GetRolesBySearchCritera.");
                            var roleList = CoreData.UserManager.GetRolesBySearchCriteria(dyntaxaApplicationUserContext, roleSearch);
                            if (roleList.IsNotNull() && roleList.Count == 1)
                            {
                                using (ITransaction transactionUserAdmin = applicationTransactionUser.StartTransaction())
                                {
                                    DyntaxaLogger.WriteMessage("Calling DeleteRole.");
                                    CoreData.UserManager.DeleteRole(applicationTransactionUser, roleList[0]);
                                    DyntaxaLogger.WriteMessage("Commiting transaction after DeleteRole.");
                                    transactionUserAdmin.Commit();
                                }
                            }
                            finalizeSteps[3, 1] = true;
                            DyntaxaLogger.WriteMessage("Done with Finalize...");
                            this.RedrawTree();

                            return RedirectToAction("List");
                        }
                    }
                    else
                    {
                        DyntaxaLogger.WriteMessage("Setting errormsg 1.");
                        errorMsg = DyntaxaResource.SharedRevisionNoValidRevisionErrorText;
                    }
                }
                else
                {
                    DyntaxaLogger.WriteMessage("Setting errormsg 2.");
                    errorMsg = DyntaxaResource.SharedInvalidUserContext;
                }
            }
            catch (Exception e)
            {
                if (finalizeSteps[0, 0] && !finalizeSteps[0, 1])
                {
                    DyntaxaLogger.WriteMessage("Exception - Checking in revision.");
                    errorMsg = DyntaxaResource.RevisionFinalizeCheckInErrorInformation;
                }
                else if (finalizeSteps[1, 0] && !finalizeSteps[1, 1])
                {
                    DyntaxaLogger.WriteMessage("Exception - Failed to update Artfakta.");
                    errorMsg = DyntaxaResource.RevisionFinalizeFactsUpdateErrorInformation;
                }
                else if (finalizeSteps[2, 0] && !finalizeSteps[2, 1])
                {
                    DyntaxaLogger.WriteMessage("Exception - Failed to set IsSpeciesFactPublished flag to true.");
                    errorMsg = DyntaxaResource.RevisionFinalizeFlagNotSetErrorInformation + " " + revId;
                }
                else if (finalizeSteps[3, 0] && !finalizeSteps[3, 1])
                {
                    DyntaxaLogger.WriteMessage("Exception - roles are not removed.");
                    errorMsg = DyntaxaResource.RevisionFinalizeRolesNotRemovedErrorInformation + " " + revId;
                }
                else
                {
                    DyntaxaLogger.WriteMessage("Exception - Finalize.");
                    errorMsg = e.Message;
                    additionalErrorMsg = e.StackTrace;
                }
            }
           
            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            var errorModel = errorModelManger.GetErrorViewModel(
                DyntaxaResource.RevisionFinalizeActionErrorHeaderText,
                DyntaxaResource.RevisionFinalizeMainErrorHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// If called go back to List view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // Go back to start view
            return RedirectToAction("List", "Revision");
        }

        /// <summary>
        /// Get information on a revision. Valid for all users.
        /// </summary>
        /// <param name="revisionInfoId"></param>
        /// <returns></returns>
        public ActionResult Info(string revisionInfoId)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            try
            {
                IUserContext currentUser = GetCurrentUser();
                RevisionInfoViewModel model = new RevisionInfoViewModel();
                RevisionModelManager modelManager = new RevisionModelManager();
                if (currentUser.IsNotNull())
                {
                    if (revisionInfoId != null)
                    {
                        IList<RevisionInfoItemModelHelper> revisionInfos = new List<RevisionInfoItemModelHelper>();
                        ITaxonRevision taxonRevision = CoreData.TaxonManager.GetTaxonRevision(currentUser, Int32.Parse(revisionInfoId));
                        if (taxonRevision.IsNotNull())
                        {
                            ITaxon revisionTaxon = taxonRevision.RootTaxon;
                            
                            RevisionInfoItemModelHelper infoItem = new RevisionInfoItemModelHelper();
                            infoItem = modelManager.GetRevisionInformation(currentUser, revisionTaxon, taxonRevision, infoItem);

                            infoItem.ShowRevisionEditingButton = false;
                            infoItem.EnableRevisionEditingButton = false;
                            infoItem.RevisionEditingButtonText = string.Empty;
                            infoItem.RevisionWaitingLabel = Resources.DyntaxaResource.SharedLoading;
                            infoItem.ShowRevisionInformation = true;
                            infoItem.EditingAction = "List";
                            infoItem.EditingController = "Revision";
                            
                            revisionInfos.Add(infoItem);

                            model.RevisionInfoItems = revisionInfos;
                            model.EditingAction = "List";
                            model.EditingController = "Revision";
                            model.RevisionId = taxonRevision.Id.ToString();
                            model.TaxonId = revisionTaxon.Id.ToString();
                            model.RevisionEditingHeaderText = Resources.DyntaxaResource.RevisionInfoMainHeaderText;
                            model.RevisionEditingActionHeaderText = Resources.DyntaxaResource.RevisionInfoActionHeaderText;
                            
                             // Assign view data to model...
                            ViewData.Model = model;
                            return View("RevisionInfo");
                        }
                        else
                        {
                            errorMsg = Resources.DyntaxaResource.RevisionSharedNoValidRevisionIdErrorText + taxonRevision.Id + "."; 
                        }
                    }
                    else
                    {
                        errorMsg = Resources.DyntaxaResource.SharedRevisionNoValidRevisionErrorText;
                    }
                }
                else
                {
                    errorMsg = Resources.DyntaxaResource.SharedInvalidUserContext;
                }
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }
            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.RevisionInfoMainHeaderText,
                Resources.DyntaxaResource.RevisionInfoActionHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// POST: Informationview is shown return to list..
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Info(RevisionInfoViewModel model)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            try
            {
                return RedirectToAction("List");
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }
            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.RevisionInfoMainHeaderText,
                Resources.DyntaxaResource.RevisionInfoActionHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// Perfore initalize ie change status of revision to ongoing
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionAdministrator, ChangeCurrentRole = false)]
        public ActionResult Initialize(string revisionId)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            
            try
            {
                IUserContext loggedInUser = GetLoggedInUser();
                if (loggedInUser.IsNotNull())
                {
                    if (revisionId.IsNotNull())
                    {
                        int revId = Int32.Parse(revisionId);
                        if (revId == 0)
                        {
                            errorMsg = Resources.DyntaxaResource.RevisionInitializeRevisionNotSavedErrorInformation;
                        }
                        else
                        {
                            //Initialize revision ie set revision state to onGoing
                            ITaxonRevision taxonRevision = CoreData.TaxonManager.GetTaxonRevision(loggedInUser, revId);
                            
                            // Initialize revision first
                            try
                            {
                                using (ITransaction transaction = loggedInUser.StartTransaction())
                                {
                                    CoreData.TaxonManager.CheckOutTaxonRevision(loggedInUser, taxonRevision);
                                    transaction.Commit();
                                }

                                CacheManager.FireRefreshCache(loggedInUser);
                            }
                            catch (ArgumentException argumentException)
                            {
                                DyntaxaLogger.WriteException(argumentException);
                                errorMsg = Resources.DyntaxaResource.RevisionInitializeErrorInformation + " " + revId;
                                  // Revision could not be created..
                                var errorModelCheckOutManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
                                ErrorViewModel errorCheckoutModel = errorModelCheckOutManger.GetErrorViewModel(
                                    Resources.DyntaxaResource.SharedRevisionInitializeLabel,
                                    Resources.DyntaxaResource.SharedRevisionInitializeLabel,
                                    errorMsg, 
                                    null);
                                return View("ErrorInfo", errorCheckoutModel);
                            }
                            return RedirectToAction("List", new { taxonId = taxonRevision.RootTaxon.Id });
                        }
                    }
                    else
                    {
                        errorMsg = Resources.DyntaxaResource.SharedRevisionNoValidRevisionErrorText;
                    }
                }
                else
                {
                    errorMsg = Resources.DyntaxaResource.SharedInvalidUserContext;
                }
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }
            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.SharedRevisionInitializeLabel,
                Resources.DyntaxaResource.SharedRevisionInitializeLabel,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        ///  List all avaliable revisions. Avaliable for all users.
        ///  If any paramaeter of type bool is changed according to default then all bool parameters must be set.
        /// </summary>
        /// <param name="taxonId">Taxon id as input , optional</param>
        /// <param name="showRevisionForSelectedTaxon">Indicates if revisions to be shown is related to a taxon.</param>
        /// <param name="showCreatedRevisions">If set to true created/preliminary revisions is shown.</param>
        /// <param name="showOngoingRevisions">If set to true ongoing revisions is shown.</param>
        /// <param name="showClosedRevisions">If set to true closed/published revisions is shown.</param>
        /// <returns></returns>
        public ActionResult List(string taxonId, string sort, string page)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            bool showRevisionForSelectedTaxonTemp = true;
            bool showCreatedRevisionsTemp = false;
            bool showOngoingRevisionsTemp = true;
            bool showClosedRevisionsTemp = false;

            try
            {
                ITaxon revisionTaxon = null;
                string selectedTaxon = null;
                if (taxonId.IsNull())
                {
                    selectedTaxon = this.TaxonIdentifier.Id.ToString();
                }
                else
                {
                    selectedTaxon = taxonId;
                }

                if (selectedTaxon.IsNotNull())
                {
                    TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(selectedTaxon);
                    if (searchResult.NumberOfMatches != 1)
                    {
                        return RedirectToSearch(selectedTaxon, null);
                    }

                    this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
                    revisionTaxon = searchResult.Taxon;
                    ViewBag.Taxon = revisionTaxon;  
                }

                // restore search result if we are sorting or paging
                if ((!string.IsNullOrEmpty(sort) || !string.IsNullOrEmpty(page)) && Session["RevisionList"] != null)
                {
                    return View((RevisionListViewModel)Session["RevisionList"]);
                }

                // Update settings if set eralier
                if (this.RevisionListSettings.IsNotNull() && this.RevisionListSettings.Count > 0)
                {
                    showRevisionForSelectedTaxonTemp = RevisionListSettings.Contains(Resources.DyntaxaSettings.Default.RevisionSelctionTaxonStatusCheckBoxId);

                    showCreatedRevisionsTemp = RevisionListSettings.Contains((int)TaxonRevisionStateId.Created);

                    showOngoingRevisionsTemp = RevisionListSettings.Contains((int)TaxonRevisionStateId.Ongoing);

                    showClosedRevisionsTemp = RevisionListSettings.Contains((int)TaxonRevisionStateId.Closed);
                }
                else if (this.RevisionListSettings.IsNotNull())
                {
                    showRevisionForSelectedTaxonTemp = false;
                    showCreatedRevisionsTemp = false;
                    showOngoingRevisionsTemp = false;
                    showClosedRevisionsTemp = false;
                }
                
                RevisionListViewModel model = new RevisionListViewModel();
                RevisionModelManager modelManager = new RevisionModelManager();
                // Get user.
                IUserContext currentUser = GetCurrentUser();
                IUserContext applicationUser = GetApplicationUser();
                if (currentUser.IsNotNull() && applicationUser.IsNotNull())
                {
                    model.IsViewReadonly = true;
                    // If user is dyntaxa application user then user is not allowed to edit any revisins since this user is hidden...
                    if (currentUser.User.Id != applicationUser.User.Id)
                    {
                        // Check if user is a taxon revision identifer if so enable edit revision button option. 
                        // Must later find out which revisions is ongoing or preliminary ie possible to edit..
                        if (currentUser.CurrentRoles.IsNotNull())
                        {
                            foreach (IRole role in currentUser.CurrentRoles)
                            {
                                if (role.Identifier == Resources.DyntaxaSettings.Default.TaxonRevisionAdministrator || role.Identifier == Resources.DyntaxaSettings.Default.DyntaxaTaxonEditor)
                                {
                                    model.IsViewReadonly = false;
                                    break;
                                }
                            }
                        }
                    }
                      
                    // Create all states set efult to true and add id to revisionStatesId to search for later
                    var revisionStatesId = new List<int>();
                     model.RevisionStatus = modelManager.SetRevisionStates(showCreatedRevisionsTemp, showOngoingRevisionsTemp, showClosedRevisionsTemp, out revisionStatesId);
                    // Extract revision search criteria
                    TaxonRevisionList revisions = new TaxonRevisionList();
                    ITaxonRevisionSearchCriteria taxonRevisionSearchCriteria = new TaxonRevisionSearchCriteria();
                    taxonRevisionSearchCriteria.StateIds = revisionStatesId;
                    // Get revisions depending on input, all or from selected taxon and create a helper...
                    model.RevisionSelectionItemHelper = new RevisionSelectionItemModelHelper()
                    {
                        IsChecked = false,
                        RevisionSelctionStatusId = Resources.DyntaxaSettings.Default.RevisionSelctionTaxonStatusCheckBoxId,
                    };

                    //If id is null then no taxon has been selected
                    if (selectedTaxon.IsNotNull() && revisionTaxon.IsNotNull())
                    {
                        model.TaxonScientificName = revisionTaxon.ScientificName;
                        model.TaxonCommonName = revisionTaxon.CommonName;
                        model.TaxonCategory = revisionTaxon.Category.Name;
                        model.TaxonId = revisionTaxon.Id.ToString();
                        if (revisionTaxon.PartOfConceptDefinition.IsNotNull())
                        {
                            model.TaxonDescription = revisionTaxon.PartOfConceptDefinition;
                        }
                        else
                        {
                            model.TaxonDescription = string.Empty;
                        }
                        if (selectedTaxon.IsNotNull() && showRevisionForSelectedTaxonTemp)
                        {
                            taxonRevisionSearchCriteria.TaxonIds = new List<int>() { revisionTaxon.Id };
                            model.RevisionSelectionItemHelper.IsChecked = true;
                        }
                        model.ShowTaxonNameLabelForRevisions = true;
                        // Get all revisions belonging to a taxon
                        if (taxonRevisionSearchCriteria.StateIds.Count > 0)
                        {
                            TaxonRevisionList taxonRevisions = new TaxonRevisionList();
                            if (selectedTaxon.IsNotNull() && showRevisionForSelectedTaxonTemp)
                            {
                                taxonRevisions = CoreData.TaxonManager.GetTaxonRevisions(applicationUser, revisionTaxon);
                                // Only add revistion in correct state
                                foreach (ITaxonRevision revision in taxonRevisions)
                                {
                                    if (showCreatedRevisionsTemp)
                                    {
                                       // remove Created revisions 
                                        if (revision.State.Id == (int)TaxonRevisionStateId.Created)
                                        {
                                            revisions.Add(revision);
                                        }
                                    }
                                    if (showOngoingRevisionsTemp)
                                    {
                                        // remove ongoing revisions
                                        if (revision.State.Id == (int)TaxonRevisionStateId.Ongoing)
                                        {
                                            revisions.Add(revision);
                                        }
                                    }
                                    if (showClosedRevisionsTemp)
                                    {
                                        // remove closed revision
                                        if (revision.State.Id == (int)TaxonRevisionStateId.Closed)
                                        {
                                            revisions.Add(revision);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                revisions = CoreData.TaxonManager.GetTaxonRevisions(applicationUser, taxonRevisionSearchCriteria);
                            }
                        }
                    }
                    else
                    {
                        model.TaxonId = string.Empty;
                        model.TaxonDescription = string.Empty;
                        model.TaxonScientificName = string.Empty;
                        model.TaxonCategory = string.Empty;
                        model.TaxonCategory = string.Empty;
                        taxonRevisionSearchCriteria.StateIds = revisionStatesId;
                        model.ShowTaxonNameLabelForRevisions = false;

                        // Get all revisions by set up search critera on revisions status
                        if (taxonRevisionSearchCriteria.StateIds.Count > 0)
                        {
                            revisions = CoreData.TaxonManager.GetTaxonRevisions(applicationUser, taxonRevisionSearchCriteria);
                        }
                    }

                    var revisionItems = new List<RevisionItemModel>();
                    foreach (ITaxonRevision revision in revisions)
                    {
                        var revisionItem = new RevisionItemModel();
                        revisionItem.PublishingDate = revision.ExpectedEndDate.ToShortDateString();
                        revisionItem.RevisionId = revision.Id;
                        revisionItem.StartDate = revision.ExpectedStartDate.ToShortDateString();
                        revisionItem.TaxonCategory = string.Empty;
                        revisionItem.TaxonScentificRecomendedName = string.Empty;
                        revisionItem.GUID = revision.Guid;
                        if (revision.RootTaxon.IsNotNull() && revision.RootTaxon.Id >= 0)
                        {
                            revisionItem.TaxonCategory = revision.RootTaxon.Category.Name;
                        }
                        if (revision.RootTaxon.IsNotNull() && revision.RootTaxon.Id >= 0 && revision.RootTaxon.ScientificName.IsNotEmpty())
                        {
                            revisionItem.TaxonScentificRecomendedName = revision.RootTaxon.ScientificName;
                        }
                        
                        // Check roles
                        bool isTaxonRevisionAdministrator = currentUser.CurrentRoles.IsNotNull() && currentUser.IsTaxonRevisionAdministrator();
                        // Check if user has role to edit revision

                        // Map revision states and revision to show info or edit in table...
                        revisionItem.IsRevisionEditable = false;
                        revisionItem.IsRevisionPossibleToStart = false;
                        revisionItem.IsRevisionPossibleToStop = false;
                        revisionItem.IsRevisionPossibleToDelete = false;
                        
                        if (revision.State.Id == (int)TaxonRevisionStateId.Created)
                        {
                            if (!model.IsViewReadonly && isTaxonRevisionAdministrator && currentUser.CurrentRole.IsNull())
                            {
                                revisionItem.IsRevisionEditable = true;
                                revisionItem.IsRevisionPossibleToDelete = true;
                            }
                            revisionItem.RevisionStatus = DyntaxaResource.RevisionListSelectedRevisionStatusCreatedText;
                        }
                        else if (revision.State.Id == (int)TaxonRevisionStateId.Ongoing)
                        {
                            if (!model.IsViewReadonly && isTaxonRevisionAdministrator && currentUser.CurrentRole.IsNull())
                            {
                                revisionItem.IsRevisionEditable = true;
                            }
                            if (currentUser.CurrentRole.IsNull() && currentUser.CurrentRoles.IsNotNull())
                            {
                                foreach (IRole role in currentUser.CurrentRoles)
                                {
                                    if (role.Identifier.IsNotNull() && role.Identifier.Contains(revision.Guid))
                                    {
                                        revisionItem.IsRevisionPossibleToStart = true;
                                        break;
                                    }
                                }
                            }
                            else if (currentUser.CurrentRole.IsNotNull() && currentUser.CurrentRole.Identifier.Contains(revision.Guid))
                            {
                                revisionItem.IsRevisionPossibleToStop = true;
                            }
                            revisionItem.RevisionStatus = DyntaxaResource.RevisionListSelectedRevisionStatusOngoingText;
                        }
                        else // Status closed
                        {
                            revisionItem.RevisionStatus = DyntaxaResource.RevisionListSelectedRevisionStatusClosedText;
                            revisionItem.IsRevisionEditable = !revision.IsSpeciesFactPublished;
                            //revisionItem.IsRevisionEditable = !revision.IsSpeciesFactPublished || !revision.IsReferenceRelationsPublished; // todo - använd den här raden istället för raden ovan när spara undan referensrelationer är implementerade.
                        }
                        
                        revisionItems.Add(revisionItem);
                    }
                    // All Revision items are set
                    model.Revisions = revisionItems;
                    // Assign view data to model...
                    ViewData.Model = model;
                    Session.Add("RevisionList", model);
                    ModelState.Remove("TaxonId");

                    return View("List");
                }
                else
                {
                    errorMsg = Resources.DyntaxaResource.SharedInvalidUserContext;
                }
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }
            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.RevisionListHeaderText,
                Resources.DyntaxaResource.RevisionListHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// POST, Activates a reload of List  with new parameters set. For all users.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isChecked"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult List(RevisionListViewModel model, int[] isChecked)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            
            try
            {
                 // Set common settings to be remenbered through this session
                this.RevisionListSettings = new List<int?>();
                if (isChecked.IsNotNull())
                {
                    foreach (int checkedCheckBox in isChecked)
                    {
                        if (checkedCheckBox == Resources.DyntaxaSettings.Default.RevisionSelctionTaxonStatusCheckBoxId)
                        {
                            this.RevisionListSettings.Add(Resources.DyntaxaSettings.Default.RevisionSelctionTaxonStatusCheckBoxId);
                        }
                        else if (checkedCheckBox == (int)TaxonRevisionStateId.Created)
                        {
                            this.RevisionListSettings.Add((int)TaxonRevisionStateId.Created);
                        }
                        else if (checkedCheckBox == (int)TaxonRevisionStateId.Ongoing)
                        {
                            this.RevisionListSettings.Add((int)TaxonRevisionStateId.Ongoing);
                        }
                        else if (checkedCheckBox == (int)TaxonRevisionStateId.Closed)
                        {
                            this.RevisionListSettings.Add((int)TaxonRevisionStateId.Closed);
                        }
                        else if (checkedCheckBox == Resources.DyntaxaSettings.Default.RevisionSelectAllStatusCheckBoxId &&
                                 (checkedCheckBox == (int)TaxonRevisionStateId.Created && 
                                 checkedCheckBox == (int)TaxonRevisionStateId.Ongoing && 
                                 checkedCheckBox == (int)TaxonRevisionStateId.Closed))
                        {
                            this.RevisionListSettings.Add(Resources.DyntaxaSettings.Default.RevisionSelctionTaxonStatusCheckBoxId);
                            this.RevisionListSettings.Add((int)TaxonRevisionStateId.Created);
                            this.RevisionListSettings.Add((int)TaxonRevisionStateId.Ongoing);
                            this.RevisionListSettings.Add((int)TaxonRevisionStateId.Closed);
                        }
                    }
                }
               
                // Now we reaload the view..
                return RedirectToAction("List", new { taxonId = model.TaxonId });
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }
            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.RevisionListHeaderText,
                Resources.DyntaxaResource.RevisionListHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// List revision events.
        /// </summary>
        /// <param name="revisionId"></param>
        /// <returns></returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor, ChangeCurrentRole = false)]
        public ActionResult ListEvent(string revisionId, string taxonId = null)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            try
            {
                int? revisionIdTemp = this.RevisionId;
                if (revisionId.IsNotNull())
                {
                    revisionIdTemp = Int32.Parse(revisionId);
                }
                if (taxonId.IsNotNull())
                {
                    this.TaxonIdentifier = TaxonIdTuple.CreateFromId(Int32.Parse(taxonId));
                }
                RevisionModelManager modelManager = new RevisionModelManager();

                IUserContext currentUser = GetCurrentUser();
                RevisionEventViewModel model = new RevisionEventViewModel();
                model.TaxonId = this.TaxonIdentifier.Id.ToString();
                if (currentUser.IsNotNull())
                {
                    if (revisionIdTemp != null)
                    {
                        IList<RevisionEventModelHelper> revisionEventInfos = new List<RevisionEventModelHelper>();
                        ITaxonRevision taxonRevision = CoreData.TaxonManager.GetTaxonRevision(currentUser, (int)revisionIdTemp);
                        if (taxonRevision.IsNotNull())
                        {
                            ITaxon revisionTaxon = taxonRevision.RootTaxon;
                            model.RevisionTaxonInfoViewModel = modelManager.GetRevisionInfoViewModel(revisionTaxon, taxonRevision);
                            model.RevisionId = taxonRevision.Id.ToString();
                            TaxonRevisionEventList events = CoreData.TaxonManager.GetTaxonRevisionEvents(GetLoggedInUser(), taxonRevision.Id);
                            int index = 1;
                            foreach (TaxonRevisionEvent tempEvent in events)
                            {
                                RevisionEventModelHelper infoItem = new RevisionEventModelHelper();
                                infoItem.AffectedTaxa = tempEvent.AffectedTaxa.IsNull() ? string.Empty : tempEvent.AffectedTaxa;
                                infoItem.ChangeEventType = tempEvent.Type.Description.IsNull() ? string.Empty : tempEvent.Type.Description;
                                infoItem.FormerValue = tempEvent.OldValue.IsNull() ? string.Empty : tempEvent.OldValue;
                                infoItem.NewValue = tempEvent.NewValue.IsNull() ? string.Empty : tempEvent.NewValue;
                                infoItem.RevisionEventId = tempEvent.Id.ToString();
                                infoItem.RevisionEventIndex = index;
                                index++;
                                revisionEventInfos.Add(infoItem);
                            }
                            model.RevisionEventItems = revisionEventInfos;
                            model.ExistEvents = false;
                            // Set last revision event
                            if (revisionEventInfos.Count > 0)
                            {
                                model.RevisionEventId = revisionEventInfos.Last().RevisionEventId; 
                                model.ExistEvents = true;
                            }
                         }
                        else
                        {
                            model.ErrorMessage = Resources.DyntaxaResource.RevisionSharedNoValidRevisionIdErrorText + taxonRevision.Id + ".";
                            model.RevisionTaxonInfoViewModel = modelManager.GetRevisionInfoViewModel(null, taxonRevision);
                        }
                    }
                    else
                    {
                        model.ErrorMessage = Resources.DyntaxaResource.SharedRevisionNoValidRevisionErrorText;
                        model.RevisionTaxonInfoViewModel = modelManager.GetRevisionInfoViewModel(null, null);
                    }
                }
                else
                {
                    model.ErrorMessage = Resources.DyntaxaResource.SharedInvalidUserContext;
                    model.RevisionTaxonInfoViewModel = modelManager.GetRevisionInfoViewModel(null, null);
                }

                if (model.ErrorMessage.IsNotNull())
                {
                    errorMsg = model.ErrorMessage;
                    ModelState.AddModelError(string.Empty, errorMsg);
                }
                ViewData.Model = model;

                return View("ListEvent", model);
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
                ModelState.AddModelError(string.Empty, errorMsg);
            }
            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.RevisionRevisonEventActionHeaderText,
                Resources.DyntaxaResource.RevisionRevisonEventActionHeaderText,
                errorMsg, 
                additionalErrorMsg);            
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// Called when a user wants to undo the last revision event.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor, ChangeCurrentRole = false)]
        [HttpPost]
        public ActionResult ListEvent(RevisionEventViewModel model)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            try
            {
                RevisionModelManager modelManager = new RevisionModelManager();
                if (ModelState.IsValid)
                {
                    bool isValid = ModelState.IsValid;

                    // Get user and dyntaxa application user.
                    IUserContext loggedInUser = GetLoggedInUser();

                    // Check users 
                    bool userTest = CheckUserContextValidity(loggedInUser);
                    if (!userTest)
                    {
                        isValid = false;
                    }

                    //Get revision id
                    int revId = Int32.Parse(model.RevisionId);
                    int revEventId = Int32.Parse(model.RevisionEventId);

                    // Get revision
                    ITaxonRevision taxonRevision = CoreData.TaxonManager.GetTaxonRevision(loggedInUser, revId);
                    ITaxonRevisionEvent revsionEvent = CoreData.TaxonManager.GetTaxonRevisionEvent(loggedInUser, revEventId);

                    bool revisionTest = CheckRevisionValidity(model.RevisionId, taxonRevision);
                    if (!revisionTest)
                    {
                        isValid = false;
                    }

                    if (revsionEvent.IsNull())
                    {
                        errorMsg = Resources.DyntaxaResource.RevisionInvalidRevisionEventErrorText;
                        ModelState.AddModelError("", errorMsg);
                        isValid = false;
                    }
                    if (revsionEvent.IsNotNull() && !(revsionEvent.Id == revEventId))
                    {
                        errorMsg = Resources.DyntaxaResource.RevisionAddInvalidRevisionErrorText;
                        string propName = string.Empty;
                        if (revsionEvent.IsNotNull())
                        {
                            propName = ReflectionUtility.GetPropertyName(() => revsionEvent.Id);
                        }
                        ModelState.AddModelError(propName, errorMsg);
                        isValid = false;
                    }

                    if (isValid)
                    {
                        using (ITransaction transaction = loggedInUser.StartTransaction())
                        {
                            CoreData.TaxonManager.DeleteTaxonRevisionEvent(loggedInUser, revsionEvent, taxonRevision);
                            transaction.Commit();
                        }
                         
                        // Check if taxon still is valid
                        if (this.RootTaxonId != null)
                        {
                            try
                            {
                                CoreData.TaxonManager.GetTaxon(loggedInUser, (int)this.TaxonIdentifier.Id);
                            }
                            catch (Exception exception)
                            {
                                DyntaxaLogger.WriteException(exception);
                                // Must set new taxon in tree since taxon don't exist
                                this.TaxonIdentifier = TaxonIdTuple.CreateFromId(taxonRevision.RootTaxon.Id);
                                this.RootTaxonId = this.TaxonIdentifier.Id.Value;
                            }

                            // Reload tree.
                            if (this.TaxonIdentifier != null)
                            {
                                this.RedrawTree((int)this.RootTaxonId, (int)this.TaxonIdentifier.Id);
                            }
                            else
                            {
                                this.RedrawTree((int)this.RootTaxonId);
                            }
                        }
                        else
                        {
                            this.RootTaxonId = taxonRevision.RootTaxon.Id;
                            this.RedrawTree(taxonRevision.RootTaxon.Id);
                        }

                        return RedirectToAction("ListEvent", new { revisionId = model.RevisionId });
                    }
                    else
                    {
                        model.RevisionTaxonInfoViewModel = modelManager.GetRevisionInfoViewModel(taxonRevision.RootTaxon, taxonRevision);
                    }
                }
                // Model state not valid...
                else
                {
                    ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                    model.RevisionTaxonInfoViewModel = modelManager.GetRevisionInfoViewModel(null, null);
                }

                return View("ListEvent", model);
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.RevisionRevisonEventActionHeaderText,
                Resources.DyntaxaResource.RevisionRevisonEventActionHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// Get all revisions, enable starting revisions for logged in user with matching roles. Valid only for taxon editors.
        /// If Current role already has stated a revision , no revisions will be enabled to start.
        /// </summary>
        /// <returns></returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonEditor, ChangeCurrentRole = false)]
        public ActionResult StartEditing(string revisionId = null)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            try
            {
                IUserContext loggedInUser = GetLoggedInUser();
                // Login to UserAdmin with a dyntaxa application user.
                IUserContext dyntaxaApplicationUserContext = GetApplicationUser();

                RevisionCommonInfoViewModel model = new RevisionCommonInfoViewModel();
                RevisionModelManager modelManger = new RevisionModelManager();

                if (loggedInUser.IsNotNull() && dyntaxaApplicationUserContext.IsNotNull())
                {
                    RoleList roles = CoreData.UserManager.GetRolesByUser(
                        dyntaxaApplicationUserContext, 
                        loggedInUser.User.Id,
                        Resources.DyntaxaSettings.Default.DyntaxaApplicationIdentifier);
                    TaxonRevisionList revisions = new TaxonRevisionList();

                    if (revisionId.IsNotNull())
                    {
                        ITaxonRevision taxonRevision = CoreData.TaxonManager.GetTaxonRevision(loggedInUser, Int32.Parse(revisionId));
                        revisions.Add(taxonRevision);
                        model.RevisionId = revisionId;
                    }
                    else
                    {
                        revisions = CoreData.TaxonManager.GetTaxonRevisions(
                            loggedInUser, 
                            new TaxonRevisionSearchCriteria()
                            {
                                StateIds = new List<int>() { (int)TaxonRevisionStateId.Ongoing }
                            });
                    }
                    model = modelManger.GetStartEditingViewModel(revisions, loggedInUser, dyntaxaApplicationUserContext, model, false, roles);
                    // Assign view data to model...
                    ViewData.Model = model;
                    return View("Start");
                }
                else
                {
                    errorMsg = Resources.DyntaxaResource.SharedInvalidUserContext;
                }
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }
            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.RevisionStartMainHeaderText,
                Resources.DyntaxaResource.RevisionStartEditingActionHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// Get all revisions, enable starting revisions for logged in user with matching roles. Valid only for taxon editors.
        /// If Current role already has stated a revision , no revisions will be enabled to start.
        /// </summary>
        /// <returns></returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonEditor, ChangeCurrentRole = false)]
        public ActionResult StartEditingRevision(string revisionId = null)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            try
            {
                IUserContext loggedInUser = GetLoggedInUser();
                // Login to UserAdmin with a dyntaxa application user.
                IUserContext dyntaxaApplicationUserContext = GetApplicationUser();

                RevisionCommonInfoViewModel model = new RevisionCommonInfoViewModel();
                RevisionModelManager modelManger = new RevisionModelManager();

                if (loggedInUser.IsNotNull() && dyntaxaApplicationUserContext.IsNotNull())
                {
                    RoleList roles = CoreData.UserManager.GetRolesByUser(
                        dyntaxaApplicationUserContext, 
                        loggedInUser.User.Id,
                        Resources.DyntaxaSettings.Default.DyntaxaApplicationIdentifier);
                    TaxonRevisionList revisions = new TaxonRevisionList();
                    
                    if (revisionId.IsNotNull())
                    {
                        ITaxonRevision taxonRevision = CoreData.TaxonManager.GetTaxonRevision(loggedInUser, Int32.Parse(revisionId));
                        revisions.Add(taxonRevision);
                        model.RevisionId = revisionId;
                    }
                    else
                    {
                        revisions = CoreData.TaxonManager.GetTaxonRevisions(
                            loggedInUser, 
                            new TaxonRevisionSearchCriteria()
                            {
                                StateIds = new List<int>() { (int)TaxonRevisionStateId.Ongoing }
                            });
                    }
                    model = modelManger.GetStartEditingViewModel(revisions, loggedInUser, dyntaxaApplicationUserContext, model, true, roles);
                    foreach (var infoItem in model.RevisionInfoItems)
                    {
                        infoItem.ShowRevisionEditingButton = false;
                        infoItem.EnableRevisionEditingButton = false;
                    }
                    // Assign view data to model...
                    ViewData.Model = model;
                    return View("CommonInfo");
                }
                else
                {
                    errorMsg = Resources.DyntaxaResource.SharedInvalidUserContext;
                }
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }
            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.RevisionStartMainHeaderText,
                Resources.DyntaxaResource.RevisionStartEditingActionHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// POST: Set Current role to corresponding revision for logged in taxon editor. Valid only for taxon editors.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonEditor, ChangeCurrentRole = false)]
        public ActionResult StartEditing(RevisionInfoItemModelHelper model)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null; 
            try
            {
                IUserContext loggedInUser = GetLoggedInUser();
                //IUserContext dyntaxaApplicationUserContext = GetApplicationContext();
                if (loggedInUser.IsNotNull())
                {
                    if (model.RevisionId.IsNotNull())
                    {
                        ITaxonRevision taxonRevision = CoreData.TaxonManager.GetTaxonRevision(loggedInUser, Int32.Parse(model.RevisionId));
                    
                       //RoleList roles = CoreData.UserManager.GetRolesByUser(dyntaxaApplicationUserContext, loggedInUser.User.Id, Resources.DyntaxaSettings.Default.DyntaxaApplicationIdentifier);
                        RoleList roles = loggedInUser.CurrentRoles;
                        if (taxonRevision.IsNotNull())
                        {
                            SetUserRevisionRole(taxonRevision, loggedInUser, roles);
                            return RedirectToAction("ListEvent", new { revisionId = taxonRevision.Id.ToString() });
                        }
                        else
                        {
                            errorMsg = Resources.DyntaxaResource.SharedRevisionNoValidRevisionErrorText;
                        }
                     }
                    else
                    {
                        errorMsg = Resources.DyntaxaResource.SharedRevisionNoValidRevisionErrorText;
                    }
                }
                else
                {
                    errorMsg = Resources.DyntaxaResource.SharedInvalidUserContext;
                }
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }
            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.RevisionStartMainHeaderText,
                Resources.DyntaxaResource.RevisionStartEditingActionHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// Get all revisions, enable stopping revision for logged in user with current role. Valid only for taxon editors.
        /// </summary>
        /// <returns></returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonEditor, ChangeCurrentRole = false)]
        public ActionResult StopEditing(string revisionId = null)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null; 
            try
            {
                IUserContext loggedInUser = GetLoggedInUser();
                
                RevisionCommonInfoViewModel model = new RevisionCommonInfoViewModel();
                RevisionModelManager modelManger = new RevisionModelManager();
                if (loggedInUser.IsNotNull())
                {
                    TaxonRevisionList revisions = new TaxonRevisionList();
                    if (revisionId.IsNotNull())
                    {
                        ITaxonRevision taxonRevision = CoreData.TaxonManager.GetTaxonRevision(loggedInUser, Int32.Parse(revisionId));
                        revisions.Add(taxonRevision);
                        model.RevisionId = revisionId;
                    }
                    else
                    {
                        revisions = CoreData.TaxonManager.GetTaxonRevisions(loggedInUser, new TaxonRevisionSearchCriteria() { StateIds = new List<int>() { (int)TaxonRevisionStateId.Ongoing } });
                    }
                    // Get taxon from input set to an int and create a temporary role out of taxon revision data
                    List<RevisionInfoItemModelHelper> revisionInfos = new List<RevisionInfoItemModelHelper>();
                        // Get revisions for this user
                    TaxonRevisionList userRevisions = new TaxonRevisionList();
                    foreach (ITaxonRevision revision in revisions)
                    {
                        // Check if user has role to edit the revision
                        foreach (IRole role in loggedInUser.CurrentRoles)
                        {
                            if (role.Identifier.IsNotNull())
                            {
                                if (role.Identifier.Contains(revision.Guid))
                                {
                                    userRevisions.Add(revision);
                                    break;
                                }
                            }
                        }
                    }
                    if (userRevisions.IsNotNull())
                    {
                        model = modelManger.GetStopEditingViewModel(revisionInfos, loggedInUser, model, userRevisions, false);
                        // Assign view data to model...
                        ViewData.Model = model;
                        return View("CommonInfo");
                    }
                    else
                    {
                        errorMsg = Resources.DyntaxaResource.SharedRevisionNoValidRevisionErrorText;
                    }
                }
                else
                {
                    errorMsg = Resources.DyntaxaResource.SharedInvalidUserContext;
                }
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }
            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.RevisionStopMainHeaderText,
                Resources.DyntaxaResource.RevisionStopEditingActionHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// /// POST: Set Current role to null for logged in taxon editor. Valid only for taxon editors.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonEditor, ChangeCurrentRole = false)]
        public ActionResult StopEditing(RevisionInfoItemModelHelper model)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            try
            {
                IUserContext loggedInUser = GetLoggedInUser();
                IUserContext dyntaxaUser = GetApplicationUser();
               
                if (loggedInUser.IsNotNull())
                {
                    if (model.RevisionId.IsNotNull())
                    {
                        RoleList roles = loggedInUser.CurrentRoles;
                        ITaxonRevision taxonRevision = CoreData.TaxonManager.GetTaxonRevision(loggedInUser, Int32.Parse(model.RevisionId));

                        if (taxonRevision.IsNotNull())
                        {
                            ClearCurrentUserRole(taxonRevision, loggedInUser, roles);
                            // Reload tree.
                            //this.RootTaxonId = MvcApplication.rootTaxonId;
                            //Check selecetd taxon id
                           if (this.TaxonIdentifier.Id.IsNotNull())
                           {
                               try
                               {
                                   // use application useer to verify that taxon exist in tree outside revision.
                                   ITaxon taxon = CoreData.TaxonManager.GetTaxon(dyntaxaUser, (int)this.TaxonIdentifier.Id);
                               }
                               catch (Exception exception)
                               {
                                    DyntaxaLogger.WriteException(exception);
                                    //Ops, taxon don't exist outside revision. Set Revision taxon as selected
                                    this.TaxonIdentifier = TaxonIdTuple.CreateFromId(taxonRevision.RootTaxon.Id);
                               }
                           }
                           var id = this.RootTaxonId;
                           // redraw tree if we have a root taxon, here we definitley have one!! 
                           //this.RedrawTree();
                           //this.RedrawTree(0, 0);
                           if (id != null)
                            {
                                this.RedrawTree((int)id, (int)this.TaxonIdentifier.Id);
                            }

                            return RedirectToAction("List");
                        }
                        else
                        {
                            errorMsg = Resources.DyntaxaResource.SharedRevisionNoValidRevisionErrorText;
                        }
                    }
                    else
                    {
                        errorMsg = Resources.DyntaxaResource.SharedRevisionNoValidRevisionErrorText;
                    }
                }
                else
                {
                    errorMsg = Resources.DyntaxaResource.SharedInvalidUserContext;
                }         
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }
            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.RevisionStopMainHeaderText,
                Resources.DyntaxaResource.RevisionStopEditingActionHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// Stop selcetd revision
        /// </summary>
        /// <param name="revisionId"></param>
        /// <returns></returns>
         [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonEditor, ChangeCurrentRole = false)]
        public ActionResult StopEditingRevision(string revisionId)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            try
            {
                IUserContext loggedInUser = GetLoggedInUser();

                RevisionCommonInfoViewModel model = new RevisionCommonInfoViewModel();
                RevisionModelManager modelManger = new RevisionModelManager();
                if (loggedInUser.IsNotNull())
                {
                    TaxonRevisionList revisions = new TaxonRevisionList();
                    if (revisionId.IsNotNull())
                    {
                        ITaxonRevision taxonRevision = CoreData.TaxonManager.GetTaxonRevision(loggedInUser, Int32.Parse(revisionId));
                        revisions.Add(taxonRevision);
                        model.RevisionId = revisionId;
                    }
                    else
                    {
                        revisions = CoreData.TaxonManager.GetTaxonRevisions(loggedInUser, new TaxonRevisionSearchCriteria() { StateIds = new List<int>() { (int)TaxonRevisionStateId.Ongoing } });
                    }
                    // Get taxon from input set to an int and create a temporary role out of taxon revision data
                    List<RevisionInfoItemModelHelper> revisionInfos = new List<RevisionInfoItemModelHelper>();
                    // Get revisions for this user
                    TaxonRevisionList userRevisions = new TaxonRevisionList();
                    foreach (ITaxonRevision revision in revisions)
                    {
                        // Check if user has role to edit the revision
                        foreach (IRole role in loggedInUser.CurrentRoles)
                        {
                            if (role.Identifier.IsNotNull())
                            {
                                if (role.Identifier.Contains(revision.Guid))
                                {
                                    userRevisions.Add(revision);
                                    break;
                                }
                            }
                        }
                    }
                    if (userRevisions.IsNotNull())
                    {
                        model = modelManger.GetStopEditingViewModel(revisionInfos, loggedInUser, model, userRevisions, true);
                        foreach (var infoItem in model.RevisionInfoItems)
                        {
                            infoItem.ShowRevisionEditingButton = false;
                            infoItem.EnableRevisionEditingButton = false;
                        }
                        // Assign view data to model...
                        ViewData.Model = model;
                        return View("CommonInfo");
                    }
                    else
                    {
                        errorMsg = Resources.DyntaxaResource.SharedRevisionNoValidRevisionErrorText;
                    }
                }
                else
                {
                    errorMsg = Resources.DyntaxaResource.SharedInvalidUserContext;
                }
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }
            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.RevisionStopMainHeaderText,
                Resources.DyntaxaResource.RevisionStopEditingActionHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        #region Helper methods for administrations of users....

        /// <summary>
        /// Sets user current role to selected revision role and initate revision data
        /// </summary>
        /// <param name="taxonRevision"></param>
        /// <param name="loggedInUser"></param>
        /// <param name="roles"></param>
        private void SetUserRevisionRole(ITaxonRevision taxonRevision, IUserContext loggedInUser, RoleList roles)
        {
            if (roles.IsNotNull())
            {
                // Check if user has role to edit revision
                foreach (IRole role in roles)
                {
                    if (role.Identifier.IsNotNull())
                    {
                        if (role.Identifier.Contains(taxonRevision.Guid))
                        {
                            loggedInUser.CurrentRole = role;
                            this.TaxonIdentifier = TaxonIdTuple.CreateFromId(taxonRevision.RootTaxon.Id);
                            this.RevisionId = taxonRevision.Id;
                            this.TaxonRevision = taxonRevision;
                            this.RevisionTaxonId = taxonRevision.RootTaxon.Id;
                            this.RevisionTaxonCategorySortOrder = taxonRevision.RootTaxon.Category.SortOrder;
                            // Reset all lump split rerence properties...
                            ResetAllLumpSplitData();

                            // Replace the root taxon.
                            this.RedrawTree(taxonRevision.RootTaxon.Id, taxonRevision.RootTaxon.Id);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Clears the current user role and reset revision data
        /// </summary>
        /// <param name="taxonRevision"></param>
        /// <param name="loggedInUser"></param>
        /// <param name="roles"></param>
        private void ClearCurrentUserRole(ITaxonRevision taxonRevision, IUserContext loggedInUser, RoleList roles)
        {
            if (roles.IsNotNull())
            {
                // Restore user role to taxon editor
                foreach (IRole role in roles)
                {
                    if (role.Identifier.IsNotNull())
                    {
                        if (role.Identifier.Contains(taxonRevision.Guid))
                        {
                            loggedInUser.CurrentRole = null;
                            this.RevisionId = null;
                            this.TaxonRevision = null;
                            this.RevisionTaxonId = null;
                            this.RevisionTaxonCategorySortOrder = null;
                             // Reset all lump split properties...
                            ResetAllLumpSplitData();
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Create role and authority for revision
        /// </summary>
        /// <param name="startDate">Role and Authority start date</param>
        /// <param name="selectedUsersForRevision">List of users selected for this revision</param>
        /// <param name="loggedInUser">Logged in User Context</param>
        /// <param name="applicationTransactionUserContext">Dyntaxa application user</param>
        /// <param name="endDateForRoles">Enddate for role and authority for this revision</param>
        /// <param name="taxonRevision">Selected revision</param>
        /// <param name="taxonName">Name of the taxon</param>
        /// <param name="roleCreated">Indicate if roles has been created</param>
        /// <returns>True if logged in user context is to be reloaded. (Roles has changed for logged in user)</returns>
        private bool CreateRoleAndAuthiotyForRevision(
            DateTime startDate, 
            int[] selectedUsersForRevision,
            IUserContext loggedInUser, 
            IUserContext applicationTransactionUserContext,
            DateTime endDateForRoles, 
            ITaxonRevision taxonRevision, 
            string taxonName, 
            out bool roleCreated)
        {
            bool tempRoleCreated = false;
            bool reloadContext = false;
            using (ITransaction transactionUserAdmin = applicationTransactionUserContext.StartTransaction())
            {
                // Create a role for this revision
                IRole revisionRole = CreateRevisionRole(
                    applicationTransactionUserContext, 
                    taxonRevision, 
                    taxonName, 
                    startDate,
                    endDateForRoles);
                if (revisionRole.IsNotNull())
                {
                    //Create an  authority for this revision role
                    int? applicationId = applicationTransactionUserContext.User.ApplicationId;
                    if (applicationId.IsNotNull())
                    {
                        CreateRevisionAuthority(
                            applicationTransactionUserContext, 
                            taxonRevision, 
                            taxonName, 
                            revisionRole, 
                            applicationId,
                            startDate, 
                            endDateForRoles);
                        if (selectedUsersForRevision.IsNotNull())
                        {
                            foreach (int userId in selectedUsersForRevision)
                            {
                                // Finally add user to created revision role. Authorities will automatocally be added...
                                CoreData.UserManager.AddUserToRole(applicationTransactionUserContext, revisionRole.Id, userId);
                                if (userId == loggedInUser.User.Id)
                                {
                                    reloadContext = true;
                                }
                            }
                        }
                        // Everything is done that belongs to created revision now commit db changes...
                        transactionUserAdmin.Commit();
                        tempRoleCreated = true;
                    }
                }
            }
            roleCreated = tempRoleCreated;
            return reloadContext;
        }

        /// <summary>
        /// Updets role and authority for revision ie remove or add role for deleted or added users
        /// to this revision.
        /// </summary>
        /// <param name="usersToBeRemovedFromRole">Users not to have role for this revision anymore</param>
        /// <param name="roleList">List of all useres with editor possibilities</param>
        /// <param name="loggedInUser">Logged in user</param>
        /// <param name="selectedUsersForRevision">Users selected for this revision.</param>
        /// <param name="applicationTransactionUserContext">Dyntaxa application user context</param>
        /// <param name="usersWithRole">Users that will have role for this revision.</param>
        /// <returns></returns>
        private static bool UpdateRoleAndAuthorityForRevision(
            UserList usersToBeRemovedFromRole, 
            RoleList roleList,
            IUserContext loggedInUser, 
            int[] selectedUsersForRevision,
            IUserContext applicationTransactionUserContext, 
            UserList usersWithRole)
        {
            bool reloadContext = false;
            using (ITransaction transactionUserAdmin = applicationTransactionUserContext.StartTransaction())
            {
                if (selectedUsersForRevision.IsNotNull())
                {
                    foreach (int userId in selectedUsersForRevision)
                    {
                        bool userExistForRevision = false;
                        foreach (IUser user in usersWithRole)
                        {
                            if (userId == user.Id)
                            {
                                userExistForRevision = true;
                                usersToBeRemovedFromRole.Remove(user);
                                break;
                            }
                        }
                        if (!userExistForRevision)
                        {
                            // Must add user to created revision role. Authorities will automatically be added...
                            CoreData.UserManager.AddUserToRole(applicationTransactionUserContext, roleList[0].Id, userId);
                            if (userId == loggedInUser.User.Id)
                            {
                                reloadContext = true;
                            }
                        }
                    }
                }
                if (usersToBeRemovedFromRole.Count > 0)
                {
                    foreach (IUser userToRemovedFromRole in usersToBeRemovedFromRole)
                    {
                        CoreData.UserManager.RemoveUserFromRole(
                            applicationTransactionUserContext, 
                            roleList[0].Id,
                            userToRemovedFromRole.Id);
                        if (userToRemovedFromRole.Id == loggedInUser.User.Id)
                        {
                            reloadContext = true;
                        }
                    }
                }
                transactionUserAdmin.Commit();
            }
            return reloadContext;
        }

        /// <summary>
        /// Get all users assigned ie set to editors for a specific revision
        /// </summary>
        /// <param name="dyntaxaApplicationUserContext"></param>
        /// <param name="taxonRevision"></param>
        /// <returns></returns>
        private List<RevisionUserItemModelHelper> GetTaxonEditorsForSpecificRevision(IUserContext dyntaxaApplicationUserContext, ITaxonRevision taxonRevision)
        {
            List<RevisionUserItemModelHelper> userList = new List<RevisionUserItemModelHelper>();
            try
            {
                IRoleSearchCriteria roleSearch = new RoleSearchCriteria();
                roleSearch.Identifier = "%" + taxonRevision.Guid + "%";
                RoleList roleList = CoreData.UserManager.GetRolesBySearchCriteria(dyntaxaApplicationUserContext, roleSearch);
                if (roleList.IsNotNull() && roleList.Count == 1)
                {
                    UserList users = CoreData.UserManager.GetUsersByRole(dyntaxaApplicationUserContext, roleList[0].Id);
                    
                    foreach (IUser user in users)
                    {
                        RevisionUserItemModelHelper userItem = GetRevisionUserItemHelper(dyntaxaApplicationUserContext, userList, user);
                        userList.Add(userItem);
                    }
                }
                else if (roleList.IsNotNull() && roleList.Count == 0)
                {
                    return userList;
                }
                else if (roleList.IsNotNull() && roleList.Count > 1)
                {
                    string errorMsg = Resources.DyntaxaResource.SharedRevisionToManyEditorsErrorText;
                    throw new Exception(errorMsg);
                }
                else
                {
                    //Something wrong in DB...
                    string errorMsg = Resources.DyntaxaResource.SharedRevisionNoEditorsSelectedErrorText;
                    throw new Exception(errorMsg);
                }
            }
            catch (Exception e)
            {            
                DyntaxaLogger.WriteException(e);    
                throw new Exception(e.Message, e.InnerException);
            }
            return userList;
        }

        /// <summary>
        /// Gets all users that has taxon editor role
        /// </summary>
        /// <param name="dyntaxaApplicationUserContext"></param>
        /// <param name="roleId"></param>
        /// <returns>A list of users to be used in the mover box.</returns>
        private List<RevisionUserItemModelHelper> GetAllTaxonEditors(IUserContext dyntaxaApplicationUserContext, out int roleId)
        {
            List<RevisionUserItemModelHelper> userList = new List<RevisionUserItemModelHelper>();
            int tempRoleId = 0;
            try
            {
                IRoleSearchCriteria roleSearchcriteria = new RoleSearchCriteria();
                roleSearchcriteria.Identifier = Resources.DyntaxaSettings.Default.DyntaxaTaxonEditor;
                RoleList roleList = CoreData.UserManager.GetRolesBySearchCriteria(dyntaxaApplicationUserContext, roleSearchcriteria);

                //Get id for role
                if (roleList.IsNotNull() && roleList.Count == 1)
                {
                    foreach (IRole role in roleList)
                    {
                        UserList users = CoreData.UserManager.GetUsersByRole(dyntaxaApplicationUserContext, role.Id);
                        foreach (IUser user in users)
                        {
                            RevisionUserItemModelHelper userItem = GetRevisionUserItemHelper(dyntaxaApplicationUserContext, userList, user);
                            userList.Add(userItem);
                        }
                        tempRoleId = role.Id; 
                    }
                }
                else if (roleList.IsNotNull() && roleList.Count > 1)
                {
                    throw new Exception(Resources.DyntaxaResource.SharedRevisionToManyEditorsErrorText);
                }
                else
                {
                    //Something wrong in DB...
                    throw new Exception(Resources.DyntaxaResource.SharedRevisionNoTaxonEditorRoleExist);
                }
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                throw new Exception(e.Message, e.InnerException);
            }
            roleId = tempRoleId;
            return userList;
        }

        /// <summary>
        /// Get user revision information ie get first and last names for users with
        /// taxon editor role to be displayed in a mover box. If last and first name are equal for a user
        /// the unique username will be shown inside brackets.
        /// </summary>
        /// <param name="dyntaxaApplicationUserContext"></param>
        /// <param name="userList"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private static RevisionUserItemModelHelper GetRevisionUserItemHelper(IUserContext dyntaxaApplicationUserContext, List<RevisionUserItemModelHelper> userList, IUser user)
        {
            RevisionUserItemModelHelper userItem = new RevisionUserItemModelHelper();
            try
            {
                userItem.Id = user.Id;
                userItem.UserName = user.UserName;
                AuthorityList authorites = CoreData.UserManager.GetAuthorities(
                    dyntaxaApplicationUserContext, 
                    user.Id,
                    Resources.DyntaxaSettings.Default.DyntaxaApplicationId);
                IList<String> identifers = new List<string>();
                foreach (IAuthority authority in authorites)
                {
                    identifers.Add(authority.Identifier);
                }
                //userItem.RevisionIdentifier = identifers;
                int? personId = user.PersonId;
                if (personId != null)
                {
                    IPerson person = CoreData.UserManager.GetPerson(dyntaxaApplicationUserContext, (int)personId);
                    userItem.PersonName = person.FirstName + " " + person.LastName;
                    foreach (RevisionUserItemModelHelper tempUserItem in userList)
                    {
                        if (userItem.PersonName.Equals(tempUserItem.PersonName))
                        {
                            userItem.PersonName = userItem.PersonName + " (" + user.UserName + ")";
                            tempUserItem.PersonName = tempUserItem.PersonName + " (" + tempUserItem.UserName + ")";
                        }
                    }
                }
                else
                {
                    userItem.PersonName = string.Empty;
                }
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                throw new Exception(e.Message, e.InnerException);
            }        
            
            return userItem;
        }

        /// <summary>
        /// Creates a authority for role and revision to be created
        /// </summary>
        /// <param name="dyntaxaApplicationUserContext">Applicaton user context ie Dyntaxa application user</param>
        /// <param name="taxonRevision">Revision to be created</param>
        /// <param name="taxonName">Selected scentific taxon name for revision to be created</param>
        /// <param name="revisionRole">Created role for revision to be created</param>
        /// <param name="applicationId">Application to assign authority to</param>
        /// <param name="startDate">Validation start date</param>
        /// <param name="endDate">Validation end date</param>
        private void CreateRevisionAuthority(
            IUserContext dyntaxaApplicationUserContext, 
            ITaxonRevision taxonRevision, 
            string taxonName, 
            IRole revisionRole,
            int? applicationId, 
            DateTime startDate, 
            DateTime endDate)
        {
            try
            {
                // Create text strings that is added to the authority and shown in the UserAdmin application.
                string authorityDesc = Resources.DyntaxaResource.RevisionInitializeAuthorityDescriptionText + " " + taxonName + ".";
                string authorityName = Resources.DyntaxaResource.RevisionInitializeAuthorityNameText + " " + taxonName + " (" + taxonRevision.ExpectedStartDate.ToShortDateString() + ")";
                string obligation = Resources.DyntaxaResource.RevisionInitializeAuthorityObligationText;
                IAuthority revisionAuthority = new Authority(dyntaxaApplicationUserContext)
                {
                    Name = authorityName,
                    Identifier = taxonRevision.Guid,
                    Description = authorityDesc,
                    ValidFromDate = startDate,
                    ValidToDate = endDate,
                    RoleId = revisionRole.Id,
                    Obligation = obligation,
                    ApplicationId = Convert.ToInt32(applicationId),
                    CreatePermission = true,
                    DeletePermission = true,
                    UpdatePermission = true,
                    ReadNonPublicPermission = true,
                    ReadPermission = true,
                    AuthorityType = AuthorityType.Application
                };
                CoreData.UserManager.CreateAuthority(dyntaxaApplicationUserContext, revisionAuthority);
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                throw new Exception(e.Message, e.InnerException);
            }
            return;
        }

        /// <summary>
        /// Creates a role for revision to be created
        /// </summary>
        /// <param name="dyntaxaApplicationUserContext">Applicaton user context ie Dyntaxa application user</param>
        /// <param name="taxonRevision">Revision to be created</param>
        /// <param name="taxonName">Selected secientific taxon name for revision to be created</param>
        /// <param name="startDate">Validation start date</param>
        /// <param name="endDate">Validation end date</param>
        /// <returns>Create role</returns>
        private IRole CreateRevisionRole(IUserContext dyntaxaApplicationUserContext, ITaxonRevision taxonRevision, string taxonName, DateTime startDate, DateTime endDate)
        {
            try
            {
                IRole createdRevisionRole = null;
                // Create text strings that is added to the authority and shown in the UserAdmin application.
                string desc = Resources.DyntaxaResource.RevisionInitializeRoleDescriptionText + " " + taxonName + ".";
                string name = Resources.DyntaxaResource.RevisionInitializeRoleNameText + " " + taxonName + " " + Resources.DyntaxaResource.SharedRevisionText + 
                              " " + taxonRevision.Id + " (" + taxonRevision.ExpectedStartDate.ToShortDateString() + ")";
                string shortName = Resources.DyntaxaResource.RevisionInitializeRoleShortNameText + taxonRevision.Id + "(" + taxonName.Trim() + ")";

                // Create role for this revision
                IRole revisionRole = new Role(dyntaxaApplicationUserContext)
                {
                    Name = name,
                    ShortName = shortName,
                    Description = desc,
                    ValidFromDate = startDate,
                    ValidToDate = endDate,
                    Identifier = Resources.DyntaxaSettings.Default.TaxonRevisionEditor + "_" + taxonRevision.Guid
                };
                CoreData.UserManager.CreateRole(dyntaxaApplicationUserContext, revisionRole);
                
                // Get role that is to be returned
                RoleList roles = CoreData.UserManager.GetRolesBySearchCriteria(dyntaxaApplicationUserContext, new RoleSearchCriteria() { Name = name });
                if (roles.Count == 1)
                {
                    createdRevisionRole = roles[0];
                }
                else if (roles.IsNotNull() && roles.Count > 1)
                {
                    throw new Exception(Resources.DyntaxaResource.SharedRevisionToManyEditorsErrorText);
                }
                else
                {
                    //Something wrong in DB...
                    throw new Exception(Resources.DyntaxaResource.SharedRevisionNoTaxonEditorRoleExist);
                }
                return createdRevisionRole;
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                throw new Exception(e.Message, e.InnerException);
            }
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers.Extensions;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    public class RevisionModelManager
    {
#if DEBUG
        private readonly string userAdminRoleLink = Resources.DyntaxaSettings.Default.UrlToGetUserAdminUpdateRolesLinkMoneses;
#else
        
        private readonly string userAdminRoleLink = Resources.DyntaxaSettings.Default.UrlToGetUserAdminUpdateRolesLink;
        
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggedInUser"></param>
        /// <param name="applicationUserContext"></param>
        /// <param name="taxonId"></param>
        /// <param name="taxon"></param>
        /// <param name="userList"></param>
        /// <returns></returns>
        public RevisionAddViewModel GetRevisionAddViewModel(IUserContext loggedInUser, IUserContext applicationUserContext, string taxonId, ITaxon taxon, List<RevisionUserItemModelHelper> userList)
        {
            // Create model
            RevisionAddViewModel model = new RevisionAddViewModel();
            // Set initaial values
            model.TaxonId = taxonId;
            model.ExpectedStartDate = DateTime.Now;
            model.ExpectedPublishingDate = DateTime.Now.AddYears(1);
            model.RevisionDescription = string.Empty;
            model.ShowInitalizeButton = true;
            string url = Resources.DyntaxaResource.SharedRevisionUserAdminLinkText;
            model.UserAdminLink = new LinkItem(LinkType.Url, LinkQuality.Automatic, url, userAdminRoleLink);

            // Check that all logged in user and dyntaxa application user is valid.
            if (loggedInUser.IsNotNull())
            {
                if (applicationUserContext.IsNotNull())
                {
                    // Set locale to logged in user, used so that correct language for created roles  and authorities will be set.
                    applicationUserContext.Locale = loggedInUser.Locale;

                    // Get all users that are allowed to to edit taxon
                    model.UserList = userList; //GetAllTaxonEditors(applicationUserContext, out roleId);
                    model.RevisionId = "0";
                    // Set taxon values
                    if (taxon.IsNotNull() && taxon.Id.IsNotNull())
                    {
                        model.TaxonId = taxon.Id.ToString();                      
                    }
                    else
                    {
                        model.ErrorMessage = Resources.DyntaxaResource.RevisonAddInvalidTaxonErrorText;
                        model.TaxonId = taxonId;
                    }
                }
                else
                {
                    model.ErrorMessage = Resources.DyntaxaResource.SharedInvalidApplicationUserContext;
                }
            }
            else
            {
                model.ErrorMessage = Resources.DyntaxaResource.SharedInvalidUserContext;
            }
            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggedInUser"></param>
        /// <param name="dyntaxaApplicationUserContext"></param>
        /// <param name="taxonRevision"></param>
        /// <param name="isTaxonInRevision"></param>
        /// <param name="revisionUsers"></param>
        /// <param name="allUsers"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public RevisionEditViewModel GetRevisionEditViewModel(
            IUserContext loggedInUser, 
            IUserContext dyntaxaApplicationUserContext, 
            ITaxonRevision taxonRevision, 
            ITaxon taxon, 
            bool isTaxonInRevision,
            IList<RevisionUserItemModelHelper> revisionUsers, 
            IList<RevisionUserItemModelHelper> allUsers, 
            int roleId)
        {
            RevisionEditViewModel model = new RevisionEditViewModel();
            if (loggedInUser.IsNotNull() && dyntaxaApplicationUserContext.IsNotNull())
            {
                // Set locale to logged in user.
                dyntaxaApplicationUserContext.Locale = loggedInUser.Locale;

                if (taxonRevision.IsNotNull())
                {
                    if (taxon.IsNotNull())
                    {
                        model.ExpectedStartDate = taxonRevision.ExpectedStartDate;
                        model.ExpectedPublishingDate = taxonRevision.ExpectedEndDate;
                        model.IsSpeciesFactPublished = taxonRevision.IsSpeciesFactPublished;
                        model.IsReferenceRelationsPublished = taxonRevision.IsReferenceRelationsPublished;
                        model.GUID = taxonRevision.Guid;
                        model.RevisionReferencesList = new List<int>();
                        model.NoOfRevisionReferences = 0;
                        foreach (ReferenceRelation referenceRelation in taxonRevision.GetReferences(loggedInUser))
                        {
                            model.RevisionReferencesList.Add(referenceRelation.Id);
                            model.NoOfRevisionReferences++;
                        }
                        
                        model.SelectedUserList = new List<RevisionUserItemModelHelper>();
                        model.UserList = new List<RevisionUserItemModelHelper>();
                        foreach (RevisionUserItemModelHelper user in allUsers)
                        {
                            bool userAssigedToRevision = false;
                            foreach (RevisionUserItemModelHelper revisionUser in revisionUsers)
                            {
                                if (user.Id == revisionUser.Id)
                                {
                                    //We found a alreday assigned user
                                    userAssigedToRevision = true;
                                    break;
                                }
                            }

                            if (userAssigedToRevision)
                            {
                                model.SelectedUserList.Add(user);
                            }
                            else
                            {
                                model.UserList.Add(user);
                            }
                        }
                        string url = Resources.DyntaxaResource.SharedRevisionUserAdminLinkText;
                        model.RoleId = roleId.ToString();
                        model.UserAdminLink = new LinkItem(LinkType.Url, LinkQuality.Automatic, url, userAdminRoleLink);

                        model.RevisionDescription = taxonRevision.Description;

                        // Get specices fact information
                        try
                        {
                            SpeciesFactModelManager speciesModel = new SpeciesFactModelManager(taxon, loggedInUser);
                            try
                            {
                                if (speciesModel.QualityStatus.IsNotNull())
                                {
                                    model.RevisionQualityId = speciesModel.QualityStatus.Id;
                                }
                                else if (speciesModel.QualityStatusList.IsNotNull() && speciesModel.QualityStatusList.Count > 1)
                                {
                                    model.RevisionQualityId = speciesModel.QualityStatusList.ElementAt(0).Id;
                                }
                                else
                                {
                                    model.RevisionQualityId = 0;
                                }

                                model.RevisionQualityList = new List<TaxonDropDownModelHelper>();
                                foreach (var status in speciesModel.QualityStatusList)
                                {
                                    model.RevisionQualityList.Add(new TaxonDropDownModelHelper(status.Id, status.Label));
                                }
                                model.RevisionQualityDescription = speciesModel.QualityDescription;
                            }
                            catch (Exception)
                            {
                                model.ErrorMessage = Resources.DyntaxaResource.SharedNotPossibleToReadSpeciesFactError;
                            }
                        }
                        catch (Exception)
                        {
                            model.ErrorMessage = Resources.DyntaxaResource.SharedNotPossibleToReadSpeciesFactError;
                        }

                        model.RevisionTaxonInfoViewModel = new RevisionTaxonInfoViewModel();

                        model.RevisionTaxonInfoViewModel.Id = taxonRevision.Id.ToString();
                        model.RevisionTaxonInfoViewModel.CommonName = taxon.CommonName.IsNotEmpty() ? taxon.CommonName : string.Empty;
                        model.RevisionTaxonInfoViewModel.ScientificName = taxon.ScientificName;
                        model.RevisionTaxonInfoViewModel.Category = taxon.Category.Name;
                        model.RevisionTaxonInfoViewModel.CategorySortOrder = taxon.Category.SortOrder;

                        model.RevisionTaxonInfoViewModel.RevisionText = Resources.DyntaxaResource.SharedRevisionIdLabelText;
                        model.RevisionTaxonInfoViewModel.MainHeaderText = Resources.DyntaxaResource.RevisionEditMainHeaderFullText;

                        model.RevisionId = taxonRevision.Id.ToString();
                        model.ShowFinalizeButton = false;
                        model.ShowInitalizeButton = false;
                        model.ShowDeleteButton = false;
                        ITaxon revisionTaxon = taxonRevision.RootTaxon;
                        int state = taxonRevision.State.Id;
                        if (state == (int)TaxonRevisionStateId.Created)
                        {
                            model.RevisionStatus = Resources.DyntaxaResource.RevisionListSelectedRevisionStatusCreatedText;
                            model.ShowInitalizeButton = true;
                            model.IsTaxonInRevision = revisionTaxon.IsInRevision;
                            model.ShowDeleteButton = true;
                        }
                        else if (state == (int)TaxonRevisionStateId.Ongoing)
                        {
                            model.RevisionStatus = Resources.DyntaxaResource.RevisionListSelectedRevisionStatusOngoingText;
                            model.ShowFinalizeButton = true;
                        }
                        else if (state == (int)TaxonRevisionStateId.Closed)
                        {
                            model.RevisionStatus = Resources.DyntaxaResource.RevisionListSelectedRevisionStatusClosedText;
                            model.ShowUpdateSpeciesFactButton = !taxonRevision.IsSpeciesFactPublished;
                            model.ShowUpdateReferenceRelationsButton = !taxonRevision.IsReferenceRelationsPublished;
                        }
                        model.RevisionTaxonId = revisionTaxon.Id.ToString();
                        
                        return model;
                    }
                    else
                    {
                        model.ErrorMessage = Resources.DyntaxaResource.RevisonAddInvalidTaxonErrorText;
                    }
                }
                else
                {
                    model.ErrorMessage = Resources.DyntaxaResource.RevisionSharedNoValidRevisionIdErrorText + taxonRevision.Id + ".";
                }
            }
            else
            {
                model.ErrorMessage = Resources.DyntaxaResource.SharedInvalidUserContext + " " + Resources.DyntaxaResource.SharedInvalidApplicationUserContext;
            }
            return model;
        }

        #region Helper methods for revision

        /// <summary>
        /// Set avaliable status for revisions (ie set up checkboxes for GUI)
        /// </summary>
        /// <param name="showCreatedRevisionsStatus"></param>
        /// <param name="showOngoingRevisionsStatus"></param>
        /// <param name="showClosedRevisionsStatus"></param>
        /// <param name="revisionStatesId"></param>
        /// <returns></returns>
        public IList<RevisionStatusItemModelHelper> SetRevisionStates(
            bool? showCreatedRevisionsStatus, 
            bool? showOngoingRevisionsStatus,
            bool? showClosedRevisionsStatus, 
            out List<int> revisionStatesId)
        {
            // Get all revision categories
            IList<RevisionStatusItemModelHelper> revisionStatesForModel = new List<RevisionStatusItemModelHelper>();
            List<int> revStateIds = new List<int>();
            // Create states if not null
            foreach (var val in Enum.GetValues(typeof(TaxonRevisionStateId)))
            {
                string revisionStatusName = string.Empty;
                bool isChecked = false;
                if ((int)val == (int)TaxonRevisionStateId.Created)
                {
                    revisionStatusName = Resources.DyntaxaResource.RevisionListSelectedRevisionStatusCreatedText;
                    if (showCreatedRevisionsStatus != null && (bool)showCreatedRevisionsStatus)
                    {
                        isChecked = true;
                        revStateIds.Add((int)val);
                    }
                }
                else if ((int)val == (int)TaxonRevisionStateId.Ongoing)
                {
                    revisionStatusName = Resources.DyntaxaResource.RevisionListSelectedRevisionStatusOngoingText;
                    if (showOngoingRevisionsStatus != null && (bool)showOngoingRevisionsStatus)
                    {
                        isChecked = true;
                        revStateIds.Add((int)val);
                    }
                }
                else if ((int)val == (int)TaxonRevisionStateId.Closed)
                {
                    revisionStatusName = Resources.DyntaxaResource.RevisionListSelectedRevisionStatusClosedText;
                    if (showClosedRevisionsStatus != null && (bool)showClosedRevisionsStatus)
                    {
                        isChecked = true;
                        revStateIds.Add((int)val);
                    }
                }
                revisionStatesForModel.Add(new RevisionStatusItemModelHelper()
                {
                    RevisionStatusName = revisionStatusName,
                    IsChecked = isChecked,
                    RevisionStatusId = (int)val
                });
            }
            revisionStatesId = revStateIds;
            return revisionStatesForModel;
        }

        /// <summary>
        /// Returns information extracted from revision
        /// </summary>
        /// <param name="loggedInUser"></param>
        /// <param name="revisionTaxon"></param>
        /// <param name="taxonRevision"></param>
        /// <param name="infoItem"></param>
        /// <returns></returns>
        public RevisionInfoItemModelHelper GetRevisionInformation(IUserContext loggedInUser, ITaxon revisionTaxon, ITaxonRevision taxonRevision, RevisionInfoItemModelHelper infoItem)
        {
            infoItem.ExpectedStartDate = taxonRevision.ExpectedStartDate.ToShortDateString();
            infoItem.ExpectedPublishingDate = taxonRevision.ExpectedEndDate.ToShortDateString();
            infoItem.RevisionDescription = taxonRevision.Description;
            infoItem.RevisionId = taxonRevision.Id.ToString();
            infoItem.TaxonId = revisionTaxon.Id.ToString();
            int state = taxonRevision.State.Id;
            if (state == (int)TaxonRevisionStateId.Created)
            {
                infoItem.RevisionStatus = Resources.DyntaxaResource.RevisionListSelectedRevisionStatusCreatedText;
            }
            else if (state == (int)TaxonRevisionStateId.Ongoing)
            {
                infoItem.RevisionStatus = Resources.DyntaxaResource.RevisionListSelectedRevisionStatusOngoingText;
            }
            else if (state == (int)TaxonRevisionStateId.Closed)
            {
                infoItem.RevisionStatus = Resources.DyntaxaResource.RevisionListSelectedRevisionStatusClosedText;
            }
            if (revisionTaxon.ScientificName.IsNotEmpty())
            {
                infoItem.ScientificName = revisionTaxon.ScientificName;
            }
            if (revisionTaxon.CommonName.IsNotEmpty())
            {
                infoItem.CommonName = revisionTaxon.CommonName;
            }
            infoItem.TaxonCategory = revisionTaxon.Category.Name;
            return infoItem;
        }

        /// <summary>
        /// Create revision for taxon revision
        /// </summary>
        /// <param name="userContext">Applicaton user context ie Dyntaxa application user</param>
        /// <param name="revisionTaxon">Taxon that is adressed in this revision.</param>
        /// <param name="revisionDescription"></param>
        /// <returns></returns>
        public ITaxonRevision InitRevision(IUserContext userContext, ITaxon revisionTaxon, string revisionDescription, DateTime startDate, DateTime endDate)
        {
            try
            {
                ITaxonRevisionState taxonRevisionState = new TaxonRevisionState() { Id = (int)TaxonRevisionStateId.Created, Identifier = TaxonRevisionStateId.Created.ToString() };
                List<IReferenceRelation> referenceRelations = new List<IReferenceRelation>();
                List<ITaxonRevisionEvent> revisionEvents = new List<ITaxonRevisionEvent>();
                ITaxonRevision taxonRevision = new TaxonRevision();
                taxonRevision.RootTaxon = revisionTaxon;

                taxonRevision.State = taxonRevisionState;
                taxonRevision.ExpectedEndDate = endDate;
                taxonRevision.ExpectedStartDate = startDate;
                taxonRevision.SetReferences(referenceRelations);
                taxonRevision.SetRevisionEvents(revisionEvents);
                taxonRevision.Description = revisionDescription;

                return taxonRevision;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        #endregion

        public void UpdateSpeciesFact(IUserContext loggedInUser, RevisionEditViewModel model, ITaxon taxon)
        {             
            try
            { 
                // Check logged in user
                if (loggedInUser.IsNotNull())
                {        
                    SpeciesFactModelManager speciesModel = new SpeciesFactModelManager(taxon, loggedInUser);
                       
                    // Create quality values
                    if (model.RevisionQualityId != 0)
                    {
                        speciesModel.QualityStatusId = model.RevisionQualityId;
                    }

                    if (model.RevisionQualityDescription.IsNotNull())
                    {
                        speciesModel.QualityDescription = model.RevisionQualityDescription;
                    }
                    // Now we update 
                    speciesModel.UpdateDyntaxaSpeciesFacts();
                }
            }
            catch (Exception e)
            {
                Exception ex = new Exception(Resources.DyntaxaResource.SharedNotPossibleToUpdateSpeciesFactError, e);
                throw ex;
            }       
        }

        public RevisionEditViewModel ReloadRevisionEditViewModel(
            IUserContext userContext, 
            IUserContext dyntaxaApplicationUserContext, 
            ITaxon taxon, 
            ITaxonRevision taxonRevision,
            RevisionEditViewModel model, 
            IList<RevisionUserItemModelHelper> revisionUsers, 
            IList<RevisionUserItemModelHelper> allUsers)
        {
            IUserContext loggedInUser = userContext;
            model.RevisionQualityList = new List<TaxonDropDownModelHelper>();
            model.SelectedUserList = new List<RevisionUserItemModelHelper>();
            model.UserList = new List<RevisionUserItemModelHelper>();
            model.RevisionReferencesList = new List<int>();
            model.RevisionTaxonInfoViewModel = new RevisionTaxonInfoViewModel();
            if (loggedInUser.IsNotNull() && dyntaxaApplicationUserContext.IsNotNull())
            {
                if (taxonRevision.IsNotNull())
                {
                    foreach (ReferenceRelation referenceRelation in taxonRevision.GetReferences(userContext))
                    {
                        model.RevisionReferencesList.Add(referenceRelation.Id);
                    }
                }

                foreach (RevisionUserItemModelHelper user in allUsers)
                {
                    bool userAssigedToRevision = false;
                    if (revisionUsers.IsNotNull())
                    {
                        foreach (RevisionUserItemModelHelper revisionUser in revisionUsers)
                        {
                            if (user.Id == revisionUser.Id)
                            {
                                //We found a alreday assigned user
                                userAssigedToRevision = true;
                                break;
                            }
                        }
                    }
                    if (userAssigedToRevision)
                    {
                        model.SelectedUserList.Add(user);
                    }
                    else
                    {
                        model.UserList.Add(user);
                    }
                }

                if (taxon.IsNotNull())
                {
                    SpeciesFactModelManager speciesModel = new SpeciesFactModelManager(taxon, loggedInUser);
                    foreach (var status in speciesModel.QualityStatusList)
                    {
                        model.RevisionQualityList.Add(new TaxonDropDownModelHelper(status.Id, status.Label));
                    }
                }
                if (taxon.IsNotNull() && taxonRevision.IsNotNull())
                {
                    model.RevisionTaxonInfoViewModel = new RevisionTaxonInfoViewModel();

                    model.RevisionTaxonInfoViewModel.Id = taxonRevision.Id.ToString();
                    model.RevisionTaxonInfoViewModel.CommonName = taxon.CommonName.IsNotEmpty() ? taxon.CommonName : string.Empty;
                    model.RevisionTaxonInfoViewModel.ScientificName = taxon.ScientificName;
                    model.RevisionTaxonInfoViewModel.Category = taxon.Category.Name;
                    model.RevisionTaxonInfoViewModel.CategorySortOrder = taxon.Category.SortOrder;

                    model.RevisionTaxonInfoViewModel.RevisionText = Resources.DyntaxaResource.SharedRevisionIdLabelText;
                    model.RevisionTaxonInfoViewModel.MainHeaderText = Resources.DyntaxaResource.RevisionEditMainHeaderFullText;
                }
              
                 string url = Resources.DyntaxaResource.SharedRevisionUserAdminLinkText;
                 model.UserAdminLink = new LinkItem(LinkType.Url, LinkQuality.Automatic, url, userAdminRoleLink);
            }
            else
            {
                model.ErrorMessage = Resources.DyntaxaResource.SharedInvalidUserContext + " " + Resources.DyntaxaResource.SharedInvalidApplicationUserContext;
            }
            return model;
        }

        public void InitUserListsAndLinks(RevisionEditViewModel model)
        {
            if (model.UserAdminLink.IsNull())
            {
                model.UserAdminLink = new LinkItem();
            }

            if (model.UserList.IsNull())
            {
                model.UserList = new List<RevisionUserItemModelHelper>();
            }

            if (model.SelectedUserList.IsNull())
            {
                model.SelectedUserList = new List<RevisionUserItemModelHelper>();
            }

            if (model.RevisionQualityList.IsNull())
            {
                model.RevisionQualityList = new List<TaxonDropDownModelHelper>();
            }
        }

        public void InitUserListsAndLinks(RevisionAddViewModel model)
        {
            if (model.UserAdminLink.IsNull())
            {
                model.UserAdminLink = new LinkItem();
            }

            if (model.UserList.IsNull())
            {
                model.UserList = new List<RevisionUserItemModelHelper>();
            }
        }

        /// <summary>
        /// Get view for startEditing
        /// </summary>
        /// <param name="revisions"></param>
        /// <param name="loggedInUser"></param>
        /// <param name="dyntaxaApplicationUserContext"></param>
        /// <param name="model"></param>
        /// <param name="submit"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public RevisionCommonInfoViewModel GetStartEditingViewModel(
            TaxonRevisionList revisions, 
            IUserContext loggedInUser, 
            IUserContext dyntaxaApplicationUserContext,
            RevisionCommonInfoViewModel model, 
            bool submit, 
            RoleList roles)
        {
            IList<RevisionInfoItemModelHelper> revisionInfos = new List<RevisionInfoItemModelHelper>();
             //Check if current role is set to any of the avaliable revisions
            bool alreadyEditor = false;
            if (loggedInUser.CurrentRole.IsNotNull())
            {
                foreach (ITaxonRevision revision in revisions)
                {
                    if (loggedInUser.CurrentRole.IsNotNull() && loggedInUser.CurrentRole.Identifier.Contains(revision.Guid))
                    {
                        alreadyEditor = true;
                        break;
                    }
                }
            }

            // Get revisions for this user
            TaxonRevisionList userRevisions = new TaxonRevisionList();
            foreach (ITaxonRevision revision in revisions)
            {
                // Check if user has role to edit the revision
                foreach (IRole role in roles)
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
           // bool revsionExpanded = false;
            foreach (ITaxonRevision userRevision in userRevisions)
            {
                ITaxon revisionTaxon = userRevision.RootTaxon;
                // First we check which revisions that is enabels for this user
                RevisionInfoItemModelHelper infoItem = new RevisionInfoItemModelHelper();
                RevisionModelManager modelManager = new RevisionModelManager();
                infoItem.EnableRevisionEditingButton = false;
                infoItem.SelectedRevisionForEditingText = string.Empty;
                // Do not enable any buttons if editor is already working on a revision
                if (!alreadyEditor)
                {
                    // Check if user has role to edit the revision
                    foreach (IRole role in roles)
                    {
                        if (role.Identifier.IsNotNull())
                        {
                            if (role.Identifier.Contains(userRevision.Guid))
                            {
                                infoItem.EnableRevisionEditingButton = true;
                                //if (!revsionExpanded)
                                //{
                                //    infoItem.ShowRevisionInformation = true;
                                //    revsionExpanded = true;
                                //}
                                infoItem.ShowRevisionInformation = false;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (loggedInUser.CurrentRole.IsNotNull() &&
                        loggedInUser.CurrentRole.Identifier.Contains(userRevision.Id.ToString()))
                    {
                        infoItem.SelectedRevisionForEditingText = Resources.DyntaxaResource.RevisionStartRevisionForEditingText;
                       // infoItem.ShowRevisionInformation = true;
                    }
                }

                infoItem = modelManager.GetRevisionInformation(loggedInUser, revisionTaxon, userRevision, infoItem);
                infoItem.ShowRevisionEditingButton = true;

                infoItem.RevisionEditingButtonText = Resources.DyntaxaResource.RevisionStartEditingButtonText;
                infoItem.RevisionEditingButtonText = Resources.DyntaxaResource.RevisionStartEditingButtonText;
                infoItem.RevisionId = userRevision.Id.ToString();
                infoItem.TaxonId = userRevision.RootTaxon.Id.ToString();
                infoItem.RevisionWaitingLabel = " " + Resources.DyntaxaResource.RevisionStartMainHeaderText 
                                              + " " + Resources.DyntaxaResource.SharedRevisionText + " " + Resources.DyntaxaResource.SharedRevisionIdLabel + ": "
                                              + userRevision.Id.ToString();
                infoItem.EditingAction = "StartEditing";
                infoItem.EditingController = "Revision";
                revisionInfos.Add(infoItem);
            }

            // Check that there is any data and if nothing has been set set the first to be expanded if there is only one revision.
           if (revisionInfos.IsNotNull() && revisionInfos.Count == 1)
            {
                revisionInfos.FirstOrDefault().ShowRevisionInformation = true;
            }

            model.EditingAction = "StartEditing";
            model.EditingController = "Revision";
            model.RevisionInfoItems = revisionInfos;
            model.RevisionEditingHeaderText = Resources.DyntaxaResource.RevisionStartMainHeaderText;
            model.RevisionEditingActionHeaderText = Resources.DyntaxaResource.RevisionStartEditingActionHeaderText;

            model.DialogTextPopUpText = Resources.DyntaxaResource.SharedRevisionNoValidRevisionErrorText;
            model.DialogTitlePopUpText = Resources.DyntaxaResource.RevisionStartMainHeaderText;
            model.Submit = submit;
            return model;
        }

        /// <summary>
        /// Get view for stopEditing
        /// </summary>
        /// <param name="revisionInfos"></param>
        /// <param name="loggedInUser"></param>
        /// <param name="model"></param>
        /// <param name="userRevisions"></param>
        public RevisionCommonInfoViewModel GetStopEditingViewModel(
            List<RevisionInfoItemModelHelper> revisionInfos, 
            IUserContext loggedInUser, 
            RevisionCommonInfoViewModel model,
            TaxonRevisionList userRevisions, 
            bool submit)
        {
            foreach (ITaxonRevision userRevision in userRevisions)
            {
                ITaxon revisionTaxon = userRevision.RootTaxon;
                RevisionInfoItemModelHelper infoItem = new RevisionInfoItemModelHelper();
                infoItem.FormName = "commonInfoForm";
                RevisionModelManager modelManager = new RevisionModelManager();
                infoItem = modelManager.GetRevisionInformation(loggedInUser, revisionTaxon, userRevision, infoItem);
                infoItem.ShowRevisionEditingButton = true;
                infoItem.EnableRevisionEditingButton = false;
                infoItem.ShowRevisionInformation = false;

                // Check if user has role for this revision
                if (loggedInUser.CurrentRole.IsNotNull() && loggedInUser.CurrentRole.Identifier.IsNotNull() &&
                    loggedInUser.CurrentRole.Identifier.Contains(userRevision.Guid))
                {
                    infoItem.EnableRevisionEditingButton = true;
                    infoItem.ShowRevisionInformation = true;
                    infoItem.RevisionEditingButtonText = Resources.DyntaxaResource.RevisionStopEditingButtonText;
                    infoItem.RevisionEditingButtonText = Resources.DyntaxaResource.RevisionStopEditingButtonText;
                    infoItem.RevisionId = userRevision.Id.ToString();
                    infoItem.TaxonId = userRevision.RootTaxon.Id.ToString();
                    infoItem.RevisionWaitingLabel = " " + Resources.DyntaxaResource.RevisionStopMainHeaderText
                                             + " " + Resources.DyntaxaResource.SharedRevisionText + " " + Resources.DyntaxaResource.SharedRevisionIdLabel + ": "
                                             + userRevision.Id.ToString();
                    infoItem.EditingAction = "StopEditing";
                    infoItem.EditingController = "Revision";  
                    revisionInfos.Add(infoItem);
                }
            }
            // Check that ther is any data and if nothing has been set set the first to be expanded if ther is only one revision.
            var items = revisionInfos.Where(r => r.ShowRevisionInformation);
            if (!items.Any() && revisionInfos.IsNotNull() && revisionInfos.Count == 1)
            {
                revisionInfos.FirstOrDefault().ShowRevisionInformation = true;
            }

            model.EditingAction = "StopEditing";
            model.EditingController = "Revision";  
            model.RevisionInfoItems = revisionInfos;
            model.RevisionEditingHeaderText = Resources.DyntaxaResource.RevisionStopMainHeaderText;
            model.RevisionEditingActionHeaderText = Resources.DyntaxaResource.RevisionStopEditingActionHeaderText;

            model.DialogTextPopUpText = Resources.DyntaxaResource.SharedRevisionNoValidRevisionErrorText;
            model.DialogTitlePopUpText = Resources.DyntaxaResource.RevisionStopMainHeaderText;
            model.Submit = submit;
            return model;
        }

        public RevisionTaxonInfoViewModel GetRevisionInfoViewModel(ITaxon revisionTaxon, ITaxonRevision taxonRevision)
        {
            RevisionTaxonInfoViewModel revisionTaxonInfoViewModel = new RevisionTaxonInfoViewModel();

            if (revisionTaxon.IsNotNull())
            {
                revisionTaxonInfoViewModel.CommonName = revisionTaxon.CommonName.IsNotEmpty()
                                                              ? revisionTaxon.CommonName
                                                              : string.Empty;
                revisionTaxonInfoViewModel.ScientificName = revisionTaxon.ScientificName;
                revisionTaxonInfoViewModel.Category = revisionTaxon.Category.Name;
                revisionTaxonInfoViewModel.CategorySortOrder = revisionTaxon.Category.SortOrder;
            }
            else
            {
                revisionTaxonInfoViewModel.CommonName = string.Empty;
                revisionTaxonInfoViewModel.ScientificName = string.Empty;
                revisionTaxonInfoViewModel.Category = string.Empty;
                revisionTaxonInfoViewModel.CategorySortOrder = 0;
            }
            if (taxonRevision.IsNotNull())
            {
                revisionTaxonInfoViewModel.Id = taxonRevision.Id.ToString();
            }
            else
            {
                revisionTaxonInfoViewModel.Id = "0";
            }

            revisionTaxonInfoViewModel.RevisionText = Resources.DyntaxaResource.SharedRevisionIdLabelText;
            revisionTaxonInfoViewModel.MainHeaderText = Resources.DyntaxaResource.RevisionListEventMainHeaderlText;

            return revisionTaxonInfoViewModel;
        }
    }
}

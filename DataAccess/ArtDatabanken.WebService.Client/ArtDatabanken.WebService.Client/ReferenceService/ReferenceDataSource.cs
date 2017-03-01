using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Client.UserService;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Client.ReferenceService
{
    /// <summary>
    /// This class is used to handle reference related information.
    /// </summary>
    public class ReferenceDataSource : ReferenceDataSourceBase, IReferenceDataSource
    {
        /// <summary>
        /// Create a new reference.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="reference">New reference to create.</param>
        public void CreateReference(IUserContext userContext,
                                    IReference reference)
        {
            WebReference webReference;

            CheckTransaction(userContext);
            webReference = GetReference(reference);
            WebServiceProxy.ReferenceService.CreateReference(GetClientInformation(userContext),
                                                             webReference);
        }

        /// <summary>
        /// Create a reference relation.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="referenceRelation">
        /// Information about the new reference relation.
        /// This object is updated with changes after the creation.
        /// </param>
        public virtual void CreateReferenceRelation(IUserContext userContext,
                                                    IReferenceRelation referenceRelation)
        {
            WebReferenceRelation webReferenceRelation;

            CheckTransaction(userContext);
            webReferenceRelation = WebServiceProxy.ReferenceService.CreateReferenceRelation(GetClientInformation(userContext),
                                                                                            GetReferenceRelation(referenceRelation));
            UpdateReferenceRelation(userContext, referenceRelation, webReferenceRelation);
        }

        /// <summary>
        /// Delete specified reference relation.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="referenceRelation">Reference relation to be deleted.</param>
        public virtual void DeleteReferenceRelation(IUserContext userContext,
                                                    IReferenceRelation referenceRelation)
        {
            CheckTransaction(userContext);

            WebServiceProxy.ReferenceService.DeleteReferenceRelation(GetClientInformation(userContext),
                                                                     referenceRelation.Id);
        }

        /// <summary>
        /// Convert an IReference instance into
        /// a WebReference instance.
        /// </summary>
        /// <param name="reference">An IReference instance.</param>
        /// <returns>A WebReference instance.</returns>
        private WebReference GetReference(IReference reference)
        {
            WebReference webReference;

            webReference = new WebReference();
            webReference.Id = reference.Id;
            webReference.IsModifiedDateSpecified = reference.ModifiedDate.HasValue;
            webReference.IsYearSpecified = reference.Year.HasValue;
            webReference.ModifiedBy = reference.ModifiedBy;
            if (reference.ModifiedDate.HasValue)
            {
                webReference.ModifiedDate = reference.ModifiedDate.Value;
            }

            webReference.Name = reference.Name;
            webReference.Title = reference.Title;
            if (reference.Year.HasValue)
            {
                webReference.Year = reference.Year.Value;
            }

            return webReference;
        }

        /// <summary>
        /// Convert a WebReference instance into
        /// an IReference instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webReference">A WebReference instance.</param>
        /// <returns>An IReference instance.</returns>
        private IReference GetReference(IUserContext userContext,
                                        WebReference webReference)
        {
            IReference reference;

            reference = new Reference();
            reference.DataContext = GetDataContext(userContext);
            reference.Id = webReference.Id;
            reference.ModifiedBy = webReference.ModifiedBy;
            if (webReference.IsModifiedDateSpecified)
            {
                reference.ModifiedDate = webReference.ModifiedDate;
            }

            reference.Name = webReference.Name;
            reference.Title = webReference.Title;
            if (webReference.IsYearSpecified)
            {
                reference.Year = webReference.Year;
            }

            return reference;
        }

        /// <summary> 
        /// Get specified reference relation.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="referenceRelationId">Id for a reference relation.</param>
        /// <returns>Specified reference relation.</returns>
        public virtual IReferenceRelation GetReferenceRelation(IUserContext userContext,
                                                               Int32 referenceRelationId)
        {
            WebReferenceRelation webReferenceRelation;

            CheckTransaction(userContext);
            webReferenceRelation = WebServiceProxy.ReferenceService.GetReferenceRelationById(GetClientInformation(userContext),
                                                                                             referenceRelationId);
            return GetReferenceRelation(userContext, webReferenceRelation);
        }

        /// <summary>
        /// Convert an IReferenceRelation instance
        /// to a WebReferenceRelation instance.
        /// </summary>
        /// <param name="referenceRelation">An IReferenceRelation instance.</param>
        /// <returns>A WebReferenceRelation instance.</returns>
        private WebReferenceRelation GetReferenceRelation(IReferenceRelation referenceRelation)
        {
            WebReferenceRelation webReferenceRelation;

            webReferenceRelation = null;
            if (referenceRelation.IsNotNull())
            {
                webReferenceRelation = new WebReferenceRelation();
                webReferenceRelation.Id = referenceRelation.Id;
                webReferenceRelation.ReferenceId = referenceRelation.ReferenceId;
                webReferenceRelation.RelatedObjectGuid = referenceRelation.RelatedObjectGuid;
                webReferenceRelation.TypeId = referenceRelation.Type.Id;
            }

            return webReferenceRelation;
        }

        /// <summary>
        /// Convert a WebReferenceRelation instance
        /// to a IReferenceRelation instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webReferenceRelation">A WebReferenceRelation object.</param>
        /// <returns>A IReferenceRelation instance.</returns>
        private IReferenceRelation GetReferenceRelation(IUserContext userContext,
                                                        WebReferenceRelation webReferenceRelation)
        {
            IReferenceRelation referenceRelation;

            referenceRelation = new ReferenceRelation();
            UpdateReferenceRelation(userContext,
                                    referenceRelation,
                                    webReferenceRelation);
            return referenceRelation;
        }

        /// <summary>
        /// Get reference relations that are related to specified object.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="relatedObjectGuid">GUID for the related object.</param>
        /// <returns>Reference relations that are related to specified object.</returns>
        public virtual ReferenceRelationList GetReferenceRelations(IUserContext userContext,
                                                                   String relatedObjectGuid)
        {
            List<WebReferenceRelation> webReferenceRelations;

            CheckTransaction(userContext);
            webReferenceRelations = WebServiceProxy.ReferenceService.GetReferenceRelationsByRelatedObjectGuid(GetClientInformation(userContext),
                                                                                                       relatedObjectGuid);
            return GetReferenceRelations(userContext, webReferenceRelations);
        }

        /// <summary>
        /// Convert a list of WebReferenceRelation instances
        /// to a ReferenceRelationList.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webReferenceRelations">List of WebReferenceRelation instances.</param>
        /// <returns>Reference relations.</returns>
        private ReferenceRelationList GetReferenceRelations(IUserContext userContext,
                                                            List<WebReferenceRelation> webReferenceRelations)
        {
            ReferenceRelationList referenceRelations;

            referenceRelations = new ReferenceRelationList();
            if (webReferenceRelations.IsNotEmpty())
            {
                foreach (WebReferenceRelation webReferenceRelation in webReferenceRelations)
                {
                    referenceRelations.Add(GetReferenceRelation(userContext, webReferenceRelation));
                }
            }

            return referenceRelations;
        }

        /// <summary>
        /// Convert a WebReferenceRelationType instance
        /// to a IReferenceRelationType instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webReferenceRelationType">A WebReferenceRelationType object.</param>
        /// <returns>A IReferenceRelationType instance.</returns>
        private IReferenceRelationType GetReferenceRelationType(IUserContext userContext,
                                                                WebReferenceRelationType webReferenceRelationType)
        {
            IReferenceRelationType referenceRelationType;

            referenceRelationType = new ReferenceRelationType();
            referenceRelationType.DataContext = GetDataContext(userContext);
            referenceRelationType.Description = webReferenceRelationType.Description;
            referenceRelationType.Id = webReferenceRelationType.Id;
            referenceRelationType.Identifier = webReferenceRelationType.Identifier;
            return referenceRelationType;
        }

        /// <summary>
        /// Get all reference relation types.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All reference relation types.</returns>
        public virtual ReferenceRelationTypeList GetReferenceRelationTypes(IUserContext userContext)
        {
            List<WebReferenceRelationType> webReferenceRelationTypes;

            CheckTransaction(userContext);
            webReferenceRelationTypes = WebServiceProxy.ReferenceService.GetReferenceRelationTypes(GetClientInformation(userContext));
            return GetReferenceRelationTypes(userContext, webReferenceRelationTypes);
        }

        /// <summary>
        /// Convert a list of WebReferenceRelationType instances
        /// to a ReferenceRelationTypeList.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webReferenceRelationTypes">List of WebReferenceRelationType instances.</param>
        /// <returns>Reference relation types.</returns>
        private ReferenceRelationTypeList GetReferenceRelationTypes(IUserContext userContext,
                                                                    List<WebReferenceRelationType> webReferenceRelationTypes)
        {
            ReferenceRelationTypeList referenceRelationTypes;

            referenceRelationTypes = new ReferenceRelationTypeList();
            if (webReferenceRelationTypes.IsNotEmpty())
            {
                foreach (WebReferenceRelationType webReferenceRelationType in webReferenceRelationTypes)
                {
                    referenceRelationTypes.Add(GetReferenceRelationType(userContext, webReferenceRelationType));
                }
            }

            return referenceRelationTypes;
        }

        /// <summary>
        /// Get all references.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All references.</returns>
        public ReferenceList GetReferences(IUserContext userContext)
        {
            List<WebReference> webReferences;

            CheckTransaction(userContext);
            webReferences = WebServiceProxy.ReferenceService.GetReferences(GetClientInformation(userContext));
            return GetReferences(userContext, webReferences);
        }

        /// <summary>
        /// Convert a list of WebReference instances
        /// to a ReferenceList.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webReferences">List of WebReference instances.</param>
        /// <returns>A ReferenceList.</returns>
        private ReferenceList GetReferences(IUserContext userContext,
                                            List<WebReference> webReferences)
        {
            ReferenceList references;

            references = null;
            if (webReferences.IsNotEmpty())
            {
                references = new ReferenceList();
                foreach (WebReference webReference in webReferences)
                {
                    references.Add(GetReference(userContext, webReference));
                }

                references.Sort();
            }

            return references;
        }

        /// <summary>
        /// Get specified references.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="referenceIds">Reference ids.</param>
        /// <returns>Specified references.</returns>
        public ReferenceList GetReferences(IUserContext userContext,
                                           List<Int32> referenceIds)
        {
            List<WebReference> webReferences;

            CheckTransaction(userContext);
            webReferences = WebServiceProxy.ReferenceService.GetReferencesByIds(GetClientInformation(userContext),
                                                                                referenceIds);
            return GetReferences(userContext, webReferences);
        }

        /// <summary>
        /// Get references that matches search criteria.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="searchCriteria">Reference search criteria.</param>
        /// <returns>References that matches search criteria.</returns>
        public ReferenceList GetReferences(IUserContext userContext,
                                           IReferenceSearchCriteria searchCriteria)
        {
            List<WebReference> webReferences;
            WebReferenceSearchCriteria webSearchCriteria;

            CheckTransaction(userContext);
            webSearchCriteria = GetReferenceSearchCriteria(searchCriteria);
            webReferences = WebServiceProxy.ReferenceService.GetReferencesBySearchCriteria(GetClientInformation(userContext),
                                                                                           webSearchCriteria);
            return GetReferences(userContext, webReferences);
        }

        /// <summary>
        /// Convert an IReferenceSearchCriteria instance
        /// to a WebReferenceSearchCriteria instance.
        /// </summary>
        /// <param name="searchCriteria">An IReferenceSearchCriteria instance.</param>
        /// <returns>A WebReferenceSearchCriteria instance.</returns>
        private WebReferenceSearchCriteria GetReferenceSearchCriteria(IReferenceSearchCriteria searchCriteria)
        {
            WebReferenceSearchCriteria webSearchCriteria;

            webSearchCriteria = null;
            if (searchCriteria.IsNotNull())
            {
                webSearchCriteria = new WebReferenceSearchCriteria();
                webSearchCriteria.LogicalOperator = searchCriteria.LogicalOperator;
                webSearchCriteria.NameSearchString = GetStringSearchCriteria(searchCriteria.NameSearchString);
                webSearchCriteria.TitleSearchString = GetStringSearchCriteria(searchCriteria.TitleSearchString);
                webSearchCriteria.Years = searchCriteria.Years;
            }

            return webSearchCriteria;
        }

        /// <summary>
        /// Set ReferenceService as data source
        /// in the onion data model.
        /// </summary>
        public static void SetDataSource()
        {
            ReferenceDataSource referenceDataSource;

            referenceDataSource = new ReferenceDataSource();
            CoreData.ReferenceManager.DataSource = new ReferenceDataSource();
            ((UserDataSource)(CoreData.UserManager.DataSource)).UserLoggedInEvent += referenceDataSource.Login;
            ((UserDataSource)(CoreData.UserManager.DataSource)).UserLoggedOutEvent += referenceDataSource.Logout;
        }

        /// <summary>
        /// Update existing reference.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="reference">Existing reference to update.</param>
        public void UpdateReference(IUserContext userContext,
                                    IReference reference)
        {
            WebReference webReference;

            CheckTransaction(userContext);
            webReference = GetReference(reference);
            WebServiceProxy.ReferenceService.UpdateReference(GetClientInformation(userContext),
                                                             webReference);
        }

        /// <summary>
        /// Copy data from WebReferenceRelation to IReferenceRelation.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="referenceRelation">IReferenceRelation object.</param>
        /// <param name="webReferenceRelation">WebReferenceRelation object.</param>
        private void UpdateReferenceRelation(IUserContext userContext,
                                             IReferenceRelation referenceRelation,
                                             WebReferenceRelation webReferenceRelation)
        {
            referenceRelation.DataContext = GetDataContext(userContext);
            referenceRelation.Id = webReferenceRelation.Id;
            referenceRelation.RelatedObjectGuid = webReferenceRelation.RelatedObjectGuid;
            referenceRelation.Reference = null;
            referenceRelation.ReferenceId = webReferenceRelation.ReferenceId;
            referenceRelation.Type = CoreData.ReferenceManager.GetReferenceRelationType(userContext, webReferenceRelation.TypeId);
        }
    }
}

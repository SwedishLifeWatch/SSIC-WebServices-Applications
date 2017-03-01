using System;
using System.Collections.Generic;
using System.Web.Caching;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.ReferenceService.Data
{
    /// <summary>
    /// Handle references.
    /// </summary>
    public static class ReferenceManager
    {
        /// <summary>
        /// Indicates if reference related information has been changed.
        /// </summary>
        private static Boolean _isReferenceInformationUpdated;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static ReferenceManager()
        {
            _isReferenceInformationUpdated = false;
            WebServiceContext.CommitTransactionEvent += RemoveCachedObjects;
        }

        /// <summary>
        /// Create a new reference.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="reference">New reference to create.</param>
        public static void CreateReference(WebServiceContext context,
                                           WebReference reference)
        {
            String userFullName;

            // Check access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.EditReference);
            
            // Check data.
            context.CheckTransaction();
            reference.CheckData(context);

            // Create reference.
            if (context.GetUser().Type == UserType.Person)
            {
                userFullName = WebServiceData.UserManager.GetPerson(context).GetFullName();
            }
            else
            {
                userFullName = context.GetUser().UserName;
            }

            context.GetReferenceDatabase().CreateReference(reference.Name,
                                                           reference.Year,
                                                           reference.Title,
                                                           userFullName);
        }

        /// <summary>
        /// Creates a reference relation.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="referenceRelation">Information about the reference relation that should be created.</param>
        /// <returns>The created reference relation.</returns>
        public static WebReferenceRelation CreateReferenceRelation(WebServiceContext context, WebReferenceRelation referenceRelation)
        {
            Int32 referenceRelationId;

            // Check access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.EditReference);

            // Check data.
            context.CheckTransaction();
            referenceRelation.CheckData();

            // Create reference relation.
            referenceRelationId = context.GetReferenceDatabase().CreateReferenceRelation(referenceRelation.RelatedObjectGuid,
                                                                                         referenceRelation.ReferenceId,
                                                                                         referenceRelation.TypeId);
            return GetReferenceRelationById(context, referenceRelationId);
        }

        /// <summary>
        /// Delete specified reference relation.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="referenceRelationId">The reference relation id.</param>
        public static void DeleteReferenceRelation(WebServiceContext context,
                                                   Int32 referenceRelationId)
        {
            // Check access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.EditReference);

            // Check data.
            context.CheckTransaction();

            // Delete reference relation.
            context.GetReferenceDatabase().DeleteReferenceRelation(referenceRelationId);
        }

        /// <summary>
        /// Get cached references.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Cached references.</returns>
        private static Dictionary<Int32, WebReference> GetCachedReferences(WebServiceContext context)
        {
            Dictionary<Int32, WebReference> cachedReferences;
            String cacheKey;

            if (context.IsInTransaction())
            {
                cachedReferences = new Dictionary<Int32, WebReference>();
            }
            else
            {
                cacheKey = Settings.Default.ReferenceCacheKey;
                cachedReferences = (Dictionary<Int32, WebReference>)context.GetCachedObject(cacheKey);
                if (cachedReferences.IsNull())
                {
                    // Add information to the cache.
                    cachedReferences = new Dictionary<Int32, WebReference>();
                    context.AddCachedObject(cacheKey,
                                            cachedReferences,
                                            DateTime.Now + new TimeSpan(3, 0, 0),
                                            CacheItemPriority.AboveNormal);
                }
            }

            return cachedReferences;
        }

        /// <summary>
        /// Get information about a reference relation.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="referenceRelationId">Id of reference relation record.</param>
        /// <returns>Returns reference relation or null if id doesn't match.</returns>
        public static WebReferenceRelation GetReferenceRelationById(WebServiceContext context, int referenceRelationId)
        {
            WebReferenceRelation referenceRelation;

            // Get information from database.
            using (DataReader dataReader = context.GetReferenceDatabase().GetReferenceRelationById(referenceRelationId))
            {
                if (dataReader.Read())
                {
                    referenceRelation = new WebReferenceRelation();
                    referenceRelation.LoadData(dataReader);
                }
                else
                {
                    throw new ArgumentException("Reference relation not found. Id = " + referenceRelationId);
                }
            }

            return referenceRelation;
        }

        /// <summary>
        /// Get reference relations that are related to specified object.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="relatedObjectGuid">GUID for the related object.</param>
        /// <returns>Reference relations that are related to specified object.</returns>
        public static List<WebReferenceRelation> GetReferenceRelationsByGuid(WebServiceContext context,
                                                                             String relatedObjectGuid)
        {
            List<WebReferenceRelation> referenceRelations;
            WebReferenceRelation referenceRelation;

            // Check data.
            relatedObjectGuid.CheckNotEmpty("relatedObjectGuid");

            // Get information from database.
            referenceRelations = new List<WebReferenceRelation>();
            using (DataReader dataReader = context.GetReferenceDatabase().GetReferenceRelationsByGuid(relatedObjectGuid))
            {
                while (dataReader.Read())
                {
                    referenceRelation = new WebReferenceRelation();
                    referenceRelation.LoadData(dataReader);
                    referenceRelations.Add(referenceRelation);
                }
            }

            return referenceRelations;
        }

        /// <summary>
        /// Get all reference relation types.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All reference relation types.</returns>
        public static List<WebReferenceRelationType> GetReferenceRelationTypes(WebServiceContext context)
        {
            List<WebReferenceRelationType> referenceRelationTypeList;
            WebReferenceRelationType referenceRelationType;

            // Get information from database.
            using (DataReader dataReader = context.GetReferenceDatabase().GetReferenceRelationTypes(context.Locale.Id))
            {
                referenceRelationTypeList = new List<WebReferenceRelationType>();
                while (dataReader.Read())
                {
                    referenceRelationType = new WebReferenceRelationType();
                    referenceRelationType.LoadData(dataReader);
                    referenceRelationTypeList.Add(referenceRelationType);
                }
            }

            return referenceRelationTypeList;
        }

        /// <summary>
        /// Get all references.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All references.</returns>
        public static List<WebReference> GetReferences(WebServiceContext context)
        {
            List<WebReference> references;
            WebReference reference;

            references = new List<WebReference>();
            using (DataReader dataReader = context.GetReferenceDatabase().GetReferences())
            {
                while (dataReader.Read())
                {
                    reference = new WebReference();
                    reference.LoadData(dataReader);
                    references.Add(reference);
                }
            }

            return references;
        }

        /// <summary>
        /// Get specified references.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="referenceIds">Reference ids.</param>
        /// <returns>Specified references.</returns>
        public static List<WebReference> GetReferencesByIds(WebServiceContext context,
                                                            List<Int32> referenceIds)
        {
            Dictionary<Int32, WebReference> cachedReferences,
                                            loadedReferences;
            List<Int32> notCachedReferenceIds;
            List<WebReference> references;
            WebReference reference;

            // Check data.
            referenceIds.CheckNotEmpty("referenceIds");

            // Get cached information.
            cachedReferences = GetCachedReferences(context);
            notCachedReferenceIds = new List<Int32>();
            foreach (Int32 referenceId in referenceIds)
            {
                if (!cachedReferences.ContainsKey(referenceId))
                {
                    notCachedReferenceIds.Add(referenceId);
                }
            }

            loadedReferences = new Dictionary<Int32, WebReference>();
            if (notCachedReferenceIds.IsNotEmpty())
            {
                // Get information from database.
                using (DataReader dataReader = context.GetReferenceDatabase().GetReferencesByIds(notCachedReferenceIds))
                {
                    while (dataReader.Read())
                    {
                        reference = new WebReference();
                        reference.LoadData(dataReader);
                        loadedReferences.Add(reference.Id, reference);
                    }
                }

                UpdateCachedReferences(context,
                                       cachedReferences,
                                       loadedReferences);
            }

            // Create output.
            references = new List<WebReference>();
            foreach (Int32 referenceId in referenceIds)
            {
                if (cachedReferences.ContainsKey(referenceId))
                {
                    references.Add(cachedReferences[referenceId]);
                }
                else
                {
                    if (!loadedReferences.ContainsKey(referenceId))
                    {
                        // Not existing reference requested.
                        // TODO: Remove this code when data has been fixed.
                        WebServiceData.LogManager.LogError(context, new Exception("Not existing reference requested. Reference id = " + referenceId));
                    }

                    references.Add(loadedReferences[referenceId]);
                }
            }

            return references;
        }

        /// <summary>
        /// Get references that matches search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Reference search criteria.</param>
        /// <returns>References that matches search criteria.</returns>
        public static List<WebReference> GetReferencesBySearchCriteria(WebServiceContext context,
                                                                       WebReferenceSearchCriteria searchCriteria)
        {
            List<WebReference> references;
            String whereCondition;
            WebReference reference;

            // Check data.
            searchCriteria.CheckData();

            // Get information from database.
            whereCondition = searchCriteria.GetWhereCondition();
            references = new List<WebReference>();
            using (DataReader dataReader = context.GetReferenceDatabase().GetReferencesBySearchCriteria(whereCondition))
            {
                while (dataReader.Read())
                {
                    reference = new WebReference();
                    reference.LoadData(dataReader);
                    references.Add(reference);
                }
            }

            return references;
        }

        /// <summary>
        /// Remove information objects from cache.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        private static void RemoveCachedObjects(WebServiceContext context)
        {
            String cacheKey;

            if (_isReferenceInformationUpdated)
            {
                _isReferenceInformationUpdated = false;
                cacheKey = Settings.Default.ReferenceCacheKey;
                context.RemoveCachedObject(cacheKey);
            }
        }

        /// <summary>
        /// Get cached references.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="cachedReferences">Already cached references.</param>
        /// <param name="loadedReferences">References that should be cached.</param>
        private static void UpdateCachedReferences(WebServiceContext context,
                                                   Dictionary<Int32, WebReference> cachedReferences,
                                                   Dictionary<Int32, WebReference> loadedReferences)
        {
            if (!context.IsInTransaction())
            {
                lock (cachedReferences)
                {
                    foreach (WebReference loadedReference in loadedReferences.Values)
                    {
                        cachedReferences[loadedReference.Id] = loadedReference;
                    }
                }
            }
        }

        /// <summary>
        /// Update existing reference.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="reference">Existing reference to update.</param>
        public static void UpdateReference(WebServiceContext context,
                                           WebReference reference)
        {
            String userFullName;

            // Check access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.EditReference);

            // Check data.
            context.CheckTransaction();
            reference.CheckData(context);

            // Update reference.
            if (context.GetUser().Type == UserType.Person)
            {
                userFullName = WebServiceData.UserManager.GetPerson(context).GetFullName();
            }
            else
            {
                userFullName = context.GetUser().UserName;
            }

            context.GetReferenceDatabase().UpdateReference(reference.Id,
                                                           reference.Name,
                                                           reference.Year,
                                                           reference.Title,
                                                           userFullName);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles cache of reference related information.
    /// </summary>
    public class ReferenceManagerSingleThreadCache : ReferenceManager
    {
        /// <summary>
        /// Create a ReferenceManagerSingleThreadCache instance.
        /// </summary>
        public ReferenceManagerSingleThreadCache()
        {
            ReferenceRelationTypes = new Hashtable();
            References = new Hashtable();
            CacheManager.RefreshCacheEvent += RefreshCache;
        }

        /// <summary>
        /// Reference relation types cache.
        /// </summary>
        protected Hashtable ReferenceRelationTypes { get; private set; }

        /// <summary>
        /// Reference relations cache.
        /// </summary>
        protected Hashtable References { get; private set; }

        /// <summary>
        /// Get reference relation types for specified locale.
        /// </summary>
        /// <param name="locale">Currently used locale.</param>
        /// <returns>Reference relation types for specified locale.</returns>
        protected virtual ReferenceRelationTypeList GetReferenceRelationTypes(ILocale locale)
        {
            ReferenceRelationTypeList referenceRelationTypes;

            referenceRelationTypes = null;
            if (ReferenceRelationTypes.ContainsKey(locale.ISOCode))
            {
                referenceRelationTypes = (ReferenceRelationTypeList)(ReferenceRelationTypes[locale.ISOCode]);
            }

            return referenceRelationTypes;
        }

        /// <summary>
        /// Get all reference relation types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All reference relation types.</returns>       
        public override ReferenceRelationTypeList GetReferenceRelationTypes(IUserContext userContext)
        {
            ReferenceRelationTypeList referenceRelationTypes;

            referenceRelationTypes = GetReferenceRelationTypes(userContext.Locale);
            if (referenceRelationTypes.IsNull())
            {
                referenceRelationTypes = base.GetReferenceRelationTypes(userContext);
                SetReferenceRelationTypes(referenceRelationTypes, userContext.Locale);
            }

            return referenceRelationTypes;
        }

        /// <summary>
        /// Get references for specified locale.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="locale">Currently used locale.</param>
        /// <returns>References for specified locale.</returns>
        protected virtual ReferenceList GetReferences(IUserContext userContext,
                                                      ILocale locale)
        {
            ReferenceList references;

            if (userContext.Transaction.IsNull())
            {
                if (References.ContainsKey(locale.ISOCode))
                {
                    references = (ReferenceList)(References[locale.ISOCode]);
                }
                else
                {
                    references = new ReferenceList();
                    References[locale.ISOCode] = references;
                }
            }
            else
            {
                references = new ReferenceList();
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
        public override ReferenceList GetReferences(IUserContext userContext,
                                                    List<Int32> referenceIds)
        {
            ReferenceList cachedReferences, loadedReferences, references;
            List<Int32> notCachedReferenceIds;
                
            cachedReferences = GetReferences(userContext,
                                             userContext.Locale);
            notCachedReferenceIds = new List<Int32>();
            foreach (Int32 referenceId in referenceIds)
            {
                if (!cachedReferences.Contains(referenceId))
                {
                    notCachedReferenceIds.Add(referenceId);
                }
            }

            loadedReferences = new ReferenceList();
            if (notCachedReferenceIds.IsNotEmpty())
            {
                loadedReferences = base.GetReferences(userContext, notCachedReferenceIds);
                SetReferences(userContext, loadedReferences, userContext.Locale);
            }

            // Create output.
            references = new ReferenceList();
            foreach (Int32 referenceId in referenceIds)
            {
                if (cachedReferences.Contains(referenceId))
                {
                    references.Add(cachedReferences.Get(referenceId));
                }
                else
                {
                    references.Add(loadedReferences.Get(referenceId));
                }
            }

            return references;
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        protected virtual void RefreshCache(IUserContext userContext)
        {
            ReferenceRelationTypes.Clear();
            References.Clear();
        }

        /// <summary>
        /// Set reference relation types for specified locale.
        /// </summary>
        /// <param name="referenceRelationTypes">Reference relation types.</param>
        /// <param name="locale">Currently used locale.</param>
        protected virtual void SetReferenceRelationTypes(ReferenceRelationTypeList referenceRelationTypes,
                                                         ILocale locale)
        {
            ReferenceRelationTypes[locale.ISOCode] = referenceRelationTypes;
        }

        /// <summary>
        /// Set references for specified locale.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="loadedReferences">Loaded references.</param>
        /// <param name="locale">Currently used locale.</param>
        protected virtual void SetReferences(IUserContext userContext,
                                             ReferenceList loadedReferences,
                                             ILocale locale)
        {
            ReferenceList cachedReferences;

            if (userContext.Transaction.IsNull())
            {
                cachedReferences = (ReferenceList)(References[locale.ISOCode]);
                cachedReferences.Merge(loadedReferences);
            }
        }
    }
}

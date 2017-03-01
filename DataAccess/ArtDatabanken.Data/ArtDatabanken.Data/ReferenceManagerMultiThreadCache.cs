namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles cache of reference related information.
    /// </summary>
    public class ReferenceManagerMultiThreadCache : ReferenceManagerSingleThreadCache
    {
        /// <summary>
        /// Get reference relation types for specified locale.
        /// </summary>
        /// <param name="locale">Currently used locale.</param>
        /// <returns>Reference relation types for specified locale.</returns>
        protected override ReferenceRelationTypeList GetReferenceRelationTypes(ILocale locale)
        {
            ReferenceRelationTypeList referenceRelationTypes = null;

            lock (ReferenceRelationTypes)
            {
                if (ReferenceRelationTypes.ContainsKey(locale.ISOCode))
                {
                    referenceRelationTypes = (ReferenceRelationTypeList)(ReferenceRelationTypes[locale.ISOCode]);
                }
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
        protected override ReferenceList GetReferences(IUserContext userContext,
                                                       ILocale locale)
        {
            ReferenceList references;

            if (userContext.Transaction.IsNull())
            {
                lock (References)
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
            }
            else
            {
                references = new ReferenceList();
            }

            return references;
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        protected override void RefreshCache(IUserContext userContext)
        {
            lock (ReferenceRelationTypes)
            {
                ReferenceRelationTypes.Clear();
            }

            lock (References)
            {
                References.Clear();
            }
        }

        /// <summary>
        /// Set reference relation types for specified locale.
        /// </summary>
        /// <param name="referenceRelationTypes">Reference relation types.</param>
        /// <param name="locale">Currently used locale.</param>
        protected override void SetReferenceRelationTypes(ReferenceRelationTypeList referenceRelationTypes, ILocale locale)
        {
            lock (ReferenceRelationTypes)
            {
                ReferenceRelationTypes[locale.ISOCode] = referenceRelationTypes;
            }
        }

        /// <summary>
        /// Set references for specified locale.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="loadedReferences">Loaded references.</param>
        /// <param name="locale">Currently used locale.</param>
        protected override void SetReferences(IUserContext userContext,
                                              ReferenceList loadedReferences,
                                              ILocale locale)
        {
            ReferenceList cachedReferences;

            if (userContext.Transaction.IsNull())
            {
                lock (References)
                {
                    cachedReferences = (ReferenceList)(References[locale.ISOCode]);
                    cachedReferences.Merge(loadedReferences);
                }
            }
        }
    }
}

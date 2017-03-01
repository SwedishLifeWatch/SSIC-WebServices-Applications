using System.Collections;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles cache of picture related information.
    /// </summary>
    public class PictureManagerSingleThreadCache : PictureManager
    {
        /// <summary>
        /// Create a PictureManagerSingleThreadCache instance.
        /// </summary>
        public PictureManagerSingleThreadCache()
        {
            PictureRelationDataTypes = new Hashtable();
            PictureRelationTypes = new Hashtable();
            CacheManager.RefreshCacheEvent += RefreshCache;
        }

        /// <summary>
        /// Picture relation data types cache.
        /// </summary>
        protected Hashtable PictureRelationDataTypes { get; private set; }

        /// <summary>
        /// Picture relation types cache.
        /// </summary>
        protected Hashtable PictureRelationTypes { get; private set; }

        /// <summary>
        /// Get picture relation data types for specified locale.
        /// </summary>
        /// <param name="locale">The locale.</param>
        /// <returns>Picture relation data types for specified locale.</returns>
        protected virtual PictureRelationDataTypeList GetPictureRelationDataTypes(ILocale locale)
        {
            PictureRelationDataTypeList pictureRelationDataTypes = null;

            if (PictureRelationDataTypes.ContainsKey(locale.ISOCode))
            {
                pictureRelationDataTypes = (PictureRelationDataTypeList)(PictureRelationDataTypes[locale.ISOCode]);
            }

            return pictureRelationDataTypes;
        }

        /// <summary>
        /// Get all picture relation data types.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All picture relation data types.</returns>
        public override PictureRelationDataTypeList GetPictureRelationDataTypes(IUserContext userContext)
        {
            PictureRelationDataTypeList pictureRelationDataTypes;

            pictureRelationDataTypes = GetPictureRelationDataTypes(userContext.Locale);
            if (pictureRelationDataTypes.IsNull())
            {
                pictureRelationDataTypes = base.GetPictureRelationDataTypes(userContext);
                SetPictureRelationDataTypes(pictureRelationDataTypes, userContext.Locale);
            }

            return pictureRelationDataTypes;
        }

        /// <summary>
        /// Get picture relation types for specified locale.
        /// </summary>
        /// <param name="locale">The locale.</param>
        /// <returns>Picture relation types for specified locale.</returns>
        protected virtual PictureRelationTypeList GetPictureRelationTypes(ILocale locale)
        {
            PictureRelationTypeList pictureRelationTypes = null;

            if (PictureRelationTypes.ContainsKey(locale.ISOCode))
            {
                pictureRelationTypes = (PictureRelationTypeList)(PictureRelationTypes[locale.ISOCode]);
            }

            return pictureRelationTypes;
        }

        /// <summary>
        /// Get all picture relation types.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All picture relation types.</returns>
        public override PictureRelationTypeList GetPictureRelationTypes(IUserContext userContext)
        {
            PictureRelationTypeList pictureRelationTypes;

            pictureRelationTypes = GetPictureRelationTypes(userContext.Locale);
            if (pictureRelationTypes.IsNull())
            {
                pictureRelationTypes = base.GetPictureRelationTypes(userContext);
                SetPictureRelationTypes(pictureRelationTypes, userContext.Locale);
            }

            return pictureRelationTypes;
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        protected virtual void RefreshCache(IUserContext userContext)
        {
            PictureRelationDataTypes.Clear();
            PictureRelationTypes.Clear();
        }

        /// <summary>
        /// Set picture relation data types for specified locale.
        /// </summary>
        /// <param name="pictureRelationDataTypes">Picture relation data types.</param>
        /// <param name="locale">The locale.</param>
        protected virtual void SetPictureRelationDataTypes(PictureRelationDataTypeList pictureRelationDataTypes,
                                                           ILocale locale)
        {
            PictureRelationDataTypes[locale.ISOCode] = pictureRelationDataTypes;
        }

        /// <summary>
        /// Set picture relation types for specified locale.
        /// </summary>
        /// <param name="pictureRelationTypes">Picture relation types.</param>
        /// <param name="locale">The locale.</param>
        protected virtual void SetPictureRelationTypes(PictureRelationTypeList pictureRelationTypes,
                                                       ILocale locale)
        {
            PictureRelationTypes[locale.ISOCode] = pictureRelationTypes;
        }
    }
}

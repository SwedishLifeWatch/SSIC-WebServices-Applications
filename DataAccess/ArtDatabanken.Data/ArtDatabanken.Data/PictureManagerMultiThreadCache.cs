namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles cache of picture related information.
    /// </summary>
    public class PictureManagerMultiThreadCache : PictureManagerSingleThreadCache
    {
        /// <summary>
        /// Get picture relation data types for specified locale.
        /// </summary>
        /// <param name="locale">The locale.</param>
        /// <returns>Picture relation data types for specified locale.</returns>
        protected override PictureRelationDataTypeList GetPictureRelationDataTypes(ILocale locale)
        {
            PictureRelationDataTypeList pictureRelationDataTypes = null;

            lock (PictureRelationDataTypes)
            {
                if (PictureRelationDataTypes.ContainsKey(locale.ISOCode))
                {
                    pictureRelationDataTypes = (PictureRelationDataTypeList)(PictureRelationDataTypes[locale.ISOCode]);
                }
            }

            return pictureRelationDataTypes;
        }

        /// <summary>
        /// Get picture relation types for specified locale.
        /// </summary>
        /// <param name="locale">The locale.</param>
        /// <returns>Picture relation types for specified locale.</returns>
        protected override PictureRelationTypeList GetPictureRelationTypes(ILocale locale)
        {
            PictureRelationTypeList pictureRelationTypes = null;

            lock (PictureRelationTypes)
            {
                if (PictureRelationTypes.ContainsKey(locale.ISOCode))
                {
                    pictureRelationTypes = (PictureRelationTypeList)(PictureRelationTypes[locale.ISOCode]);
                }
            }

            return pictureRelationTypes;
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        protected override void RefreshCache(IUserContext userContext)
        {
            lock (PictureRelationDataTypes)
            {
                PictureRelationDataTypes.Clear();
            }

            lock (PictureRelationTypes)
            {
                PictureRelationTypes.Clear();
            }
        }

        /// <summary>
        /// Set picture relation data types for specified locale.
        /// </summary>
        /// <param name="pictureRelationDataTypes">Picture relation data types.</param>
        /// <param name="locale">The locale.</param>
        protected override void SetPictureRelationDataTypes(PictureRelationDataTypeList pictureRelationDataTypes,
                                                            ILocale locale)
        {
            lock (PictureRelationDataTypes)
            {
                PictureRelationDataTypes[locale.ISOCode] = pictureRelationDataTypes;
            }
        }

        /// <summary>
        /// Set picture relation types for specified locale.
        /// </summary>
        /// <param name="pictureRelationTypes">Picture relation types.</param>
        /// <param name="locale">The locale.</param>
        protected override void SetPictureRelationTypes(PictureRelationTypeList pictureRelationTypes,
                                                        ILocale locale)
        {
            lock (PictureRelationTypes)
            {
                PictureRelationTypes[locale.ISOCode] = pictureRelationTypes;
            }
        }
    }
}

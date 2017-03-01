using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles cache of metadata information.
    /// </summary>
    public class MetadataManagerMultiThreadCache : MetadataManagerSingleThreadCache
    {
        /// <summary>
        /// Get all species observation field descriptions.
        /// </summary>
        /// <param name="isoCode">Language ISO code.</param>
        /// <returns>
        /// All species observation field descriptions.
        /// </returns>
        protected override SpeciesObservationFieldDescriptionList GetSpeciesObservationFieldDescriptions(String isoCode)
        {
            SpeciesObservationFieldDescriptionList speciesObservationFieldDescriptions;

            lock (this)
            {
                speciesObservationFieldDescriptions = base.GetSpeciesObservationFieldDescriptions(isoCode);                
            }

            return speciesObservationFieldDescriptions;
        }

        /// <summary>
        /// Get all picture metadata descriptions.
        /// </summary>
        /// <param name="isoCode">Language ISO code.</param>
        /// <returns>All picture metadata descriptions.</returns>
        protected override PictureMetaDataDescriptionList GetPictureMetaDataDescriptions(string isoCode)
        {
            PictureMetaDataDescriptionList pictureMetaDataDescriptions;

            lock (this)
            {
                pictureMetaDataDescriptions = base.GetPictureMetaDataDescriptions(isoCode);
            }

            return pictureMetaDataDescriptions;
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        protected override void RefreshCache(IUserContext userContext)
        {
            lock (this)
            {
                SpeciesObservationFieldDescriptions = null;
                PictureMetaDataDescriptions = null;
            }
        }

        /// <summary>
        /// Set species observation field descriptions.
        /// </summary>
        /// <param name="speciesObservationFieldDescriptions">Species observation field descriptions.</param>
        /// <param name="isoCode">Language ISO code.</param>
        protected override void SetSpeciesObservationFieldDescriptions(SpeciesObservationFieldDescriptionList speciesObservationFieldDescriptions, string isoCode)
        {
            lock (this)
            {
                base.SetSpeciesObservationFieldDescriptions(speciesObservationFieldDescriptions, isoCode);
            }
        }

        /// <summary>
        /// Set picture metadata descriptions.
        /// </summary>
        /// <param name="pictureMetaDataDescriptions">Picture metadata descriptions.</param>
        /// <param name="isoCode">Language ISO code.</param>
        protected override void SetPictureMetaDataDescriptions(PictureMetaDataDescriptionList pictureMetaDataDescriptions, String isoCode)
        {
            lock (this)
            {
                base.SetPictureMetaDataDescriptions(pictureMetaDataDescriptions, isoCode);
            }
        }
    }
}
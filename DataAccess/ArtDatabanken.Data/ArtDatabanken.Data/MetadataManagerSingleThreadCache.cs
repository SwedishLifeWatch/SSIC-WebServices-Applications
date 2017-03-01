using System;
using System.Collections;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles cache of metadata.
    /// </summary>
    public class MetadataManagerSingleThreadCache : MetadataManager
    {
        /// <summary>
        /// Create a MetadataManagerSingleThreadCache instance.
        /// </summary>
        public MetadataManagerSingleThreadCache()
        {
            SpeciesObservationFieldDescriptions = null;
            PictureMetaDataDescriptions = null;
            CacheManager.RefreshCacheEvent += RefreshCache;
        }

        /// <summary>
        /// SpeciesObservationFieldDescriptions cache.
        /// </summary>        
        protected Dictionary<string, SpeciesObservationFieldDescriptionList> SpeciesObservationFieldDescriptions 
        { get; set; }

        /// <summary>
        /// PictureMetaDataDescriptions cache.
        /// </summary>        
        protected Dictionary<string, PictureMetaDataDescriptionList> PictureMetaDataDescriptions
        { get; set; }

        /// <summary>
        /// Retrieve all Species Observation Field Descriptions.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>
        /// A Species Observation Field Description List.
        /// </returns>
        public override SpeciesObservationFieldDescriptionList GetSpeciesObservationFieldDescriptions(
            IUserContext userContext)
        {
            SpeciesObservationFieldDescriptionList speciesObservationFieldDescriptions;

            speciesObservationFieldDescriptions = GetSpeciesObservationFieldDescriptions(userContext.Locale.ISOCode);
            if (speciesObservationFieldDescriptions.IsNull())
            {
                speciesObservationFieldDescriptions = base.GetSpeciesObservationFieldDescriptions(userContext);
                SetSpeciesObservationFieldDescriptions(speciesObservationFieldDescriptions, userContext.Locale.ISOCode);
            }
            return speciesObservationFieldDescriptions;
        }

        /// <summary>
        /// Get all species observation field descriptions.
        /// </summary>
        /// <param name="isoCode">language ISO code.</param>
        /// <returns>All species observation field descriptions.</returns>
        protected virtual SpeciesObservationFieldDescriptionList GetSpeciesObservationFieldDescriptions(string isoCode)
        {
            SpeciesObservationFieldDescriptionList speciesObservationFieldDescriptions = null;
            if (SpeciesObservationFieldDescriptions != null)
            {
                SpeciesObservationFieldDescriptions.TryGetValue(isoCode, out speciesObservationFieldDescriptions);
            }

            return speciesObservationFieldDescriptions;            
        }

        /// <summary>
        /// Retrieve all picture metadata descriptions.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>Picture metadata descriptions.</returns>
        public override PictureMetaDataDescriptionList GetPictureMetaDataDescriptions(IUserContext userContext)
        {
            PictureMetaDataDescriptionList pictureMetaDataDescriptions = GetPictureMetaDataDescriptions(userContext.Locale.ISOCode);

            if (pictureMetaDataDescriptions.IsNull())
            {
                pictureMetaDataDescriptions = base.GetPictureMetaDataDescriptions(userContext);
                SetPictureMetaDataDescriptions(pictureMetaDataDescriptions, userContext.Locale.ISOCode);
            }

            return pictureMetaDataDescriptions;
        }

        /// <summary>
        /// Get all picture metadata descriptions.
        /// </summary>
        /// <param name="isoCode">language ISO code.</param>
        /// <returns>All picture metadata descriptions.</returns>
        protected virtual PictureMetaDataDescriptionList GetPictureMetaDataDescriptions(string isoCode)
        {
            PictureMetaDataDescriptionList pictureMetaDataDescriptions = null;

            if (PictureMetaDataDescriptions != null)
            {
                PictureMetaDataDescriptions.TryGetValue(isoCode, out pictureMetaDataDescriptions);
            }

            return pictureMetaDataDescriptions;
        }

        /// <summary>
        /// Set species observation field descriptions.
        /// </summary>
        /// <param name="speciesObservationFieldDescriptions">Species observation field descriptions.</param>
        /// <param name="isoCode">language ISO code</param>
        protected virtual void SetSpeciesObservationFieldDescriptions(SpeciesObservationFieldDescriptionList speciesObservationFieldDescriptions, string isoCode)
        {
            if (SpeciesObservationFieldDescriptions == null)
                SpeciesObservationFieldDescriptions = new Dictionary<string, SpeciesObservationFieldDescriptionList>();
            SpeciesObservationFieldDescriptions.Add(isoCode, speciesObservationFieldDescriptions);              
        }

        /// <summary>
        /// Set picture metadata descriptions.
        /// </summary>
        /// <param name="pictureMetaDataDescriptions">Picture metadata descriptions.</param>
        /// <param name="isoCode">language ISO code</param>
        protected virtual void SetPictureMetaDataDescriptions(PictureMetaDataDescriptionList pictureMetaDataDescriptions, string isoCode)
        {
            if (PictureMetaDataDescriptions == null)
                PictureMetaDataDescriptions = new Dictionary<string, PictureMetaDataDescriptionList>();
            PictureMetaDataDescriptions.Add(isoCode, pictureMetaDataDescriptions);
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        protected virtual void RefreshCache(IUserContext userContext)
        {
            SpeciesObservationFieldDescriptions = null;
            PictureMetaDataDescriptions = null;
        }

        /// <summary>
        /// Get cache key for taxon.
        /// </summary>
        /// <param name="taxonId">Taxon id.</param>
        /// <param name="locale">Locale.</param>
        /// <returns>Cache key for taxon.</returns>
        protected String GetTaxonCacheKey(Int32 taxonId, ILocale locale)
        {
            return taxonId.WebToString() + locale.ISOCode;
        }

    }
}

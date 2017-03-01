using System;
using System.Collections.Generic;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Manager class for Metadata handling.
    /// </summary>
    public class MetadataManager : IMetadataManager
    {
        /// <summary>
        /// This interface is used to retrieve information via PictureDataSource.
        /// </summary>
        public IPictureDataSource PictureDataSource { get; set; }

        /// <summary>
        /// This interface is used to retrieve information.
        /// </summary>
        public ISpeciesObservationDataSource SpeciesObservationDataSource { get; set; }

        /// <summary>
        /// Get information about data source.
        /// </summary>
        /// <returns>Information about data source.</returns>
        public virtual IDataSourceInformation GetDataSourceInformation()
        {
            return GetSpeciesObservationDataSourceInformation();
        }

        /// <summary>
        /// Get information about data source.
        /// </summary>
        /// <returns>Information about data source.</returns>
        public virtual IDataSourceInformation GetPictureDataSourceInformation()
        {
            return PictureDataSource.GetDataSourceInformation();
        }

        /// <summary>
        /// Get picture metadata descriptions for a specific culture.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>Picture metadata descriptions for specified culture.</returns>
        public virtual PictureMetaDataDescriptionList GetPictureMetaDataDescriptions(IUserContext userContext)
        {
            return PictureDataSource.GetPictureMetaDataDescriptions(userContext);
        }

        /// <summary>
        /// Get picture metadata descriptions for specified ids.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="pictureMetaDataDescriptionsIds">Ids of the metadadescriptions to be retrieved.</param>
        /// <returns>Picture metadata descriptions for specified ids.</returns>
        public virtual PictureMetaDataDescriptionList GetPictureMetaDataDescriptions(IUserContext userContext,
                                                                                     List<Int32> pictureMetaDataDescriptionsIds)
        {
            return PictureDataSource.GetPictureMetaDataDescriptions(userContext, pictureMetaDataDescriptionsIds);
        }

        /// <summary>
        /// Get information about species observation data source.
        /// </summary>
        /// <returns>Information about species observation data source.</returns>
        public virtual IDataSourceInformation GetSpeciesObservationDataSourceInformation()
        {
            return SpeciesObservationDataSource.GetDataSourceInformation();
        }

        /// <summary>
        /// Retrieve all Species Observation Field Descriptions.
        /// This method does not include descriptions for the classes which each field belongs to.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>A Species Observation Field Description List.</returns>
        public virtual SpeciesObservationFieldDescriptionList GetSpeciesObservationFieldDescriptions(IUserContext userContext)
        {
            return GetSpeciesObservationFieldDescriptions(userContext, false);
        }

        /// <summary>
        /// Retrieve all Species Observation Field Descriptions. 
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="includeClassDescriptions">Indication of whether or not class information should be included.</param>
        /// <returns>A Species Observation Field Description List.</returns>
        private SpeciesObservationFieldDescriptionList GetSpeciesObservationFieldDescriptions(IUserContext userContext,
                                                                                              Boolean includeClassDescriptions)
        {
            if (includeClassDescriptions)
            {
                return this.SpeciesObservationDataSource.GetSpeciesObservationFieldDescriptions(userContext);
            }
            else
            {
                SpeciesObservationFieldDescriptionList list = new SpeciesObservationFieldDescriptionList(true);
                SpeciesObservationFieldDescriptionList fullList = this.SpeciesObservationDataSource.GetSpeciesObservationFieldDescriptions(userContext);
                foreach (SpeciesObservationFieldDescription item in fullList)
                {
                    if (!item.IsClass)
                    {
                        list.Add(item);
                    }
                }
                
                return list;
            }
        }
    }
}

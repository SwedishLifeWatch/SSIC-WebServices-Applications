using System;
using System.Collections.Generic;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Interface for manager classes handling Metadata.
    /// </summary>
    public interface IMetadataManager : IManager
    {
        /// <summary>
        /// This interface is used to retrieve information via PictureDataSource.
        /// </summary>
        IPictureDataSource PictureDataSource { get; set; }

        /// <summary>
        /// This interface is used to retrieve information.
        /// </summary>
        ISpeciesObservationDataSource SpeciesObservationDataSource { get; set; }

        /// <summary>
        /// Get information about picture data source.
        /// </summary>
        /// <returns>Information about picture data source.</returns>
        IDataSourceInformation GetPictureDataSourceInformation();

        /// <summary>
        /// Get picture metadata descriptions for a specific culture.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>Picture metadata descriptions for specified culture.</returns>
        PictureMetaDataDescriptionList GetPictureMetaDataDescriptions(IUserContext userContext);

        /// <summary>
        /// Get picture metadata descriptions for specified ids.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="pictureMetaDataDescriptionsIds">Ids of the metadadescriptions to be retrieved.</param>
        /// <returns>Picture metadata descriptions for specified ids.</returns>
        PictureMetaDataDescriptionList GetPictureMetaDataDescriptions(IUserContext userContext,
                                                                      List<Int32> pictureMetaDataDescriptionsIds);

        /// <summary>
        /// Get information about species observation data source.
        /// </summary>
        /// <returns>Information about species observation data source.</returns>
        IDataSourceInformation GetSpeciesObservationDataSourceInformation();

        /// <summary>
        /// Retrieve all Species Observation Field Descriptions.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>A Species Observation Field Description List.</returns>
        SpeciesObservationFieldDescriptionList GetSpeciesObservationFieldDescriptions(IUserContext userContext);
    }
}

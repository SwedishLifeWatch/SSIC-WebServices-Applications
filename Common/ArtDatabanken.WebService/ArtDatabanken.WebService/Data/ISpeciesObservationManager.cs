using System;
using System.Collections.Generic;
using System.Data;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Definition of the SpeciesObservationManager interface.
    /// </summary>
    public interface ISpeciesObservationManager
    {
        /// <summary>
        /// Coordinate system used for species observations that 
        /// are retrieved from SwedishSpeciesObservation database.
        /// </summary>
        WebCoordinateSystem SpeciesObservationCoordinateSystem
        { get; set; }

        /// <summary>
        /// Get all county regions
        /// </summary>
        /// <param name="context"></param>
        /// <returns>All county regions</returns>
        List<WebRegion> GetCountyRegions(WebServiceContext context);

        /// <summary>
        /// Get DataTable with same definition as table
        /// DarwinCoreObservation in database.
        /// </summary>
        /// <returns>
        /// DataTable with same definition as table
        /// DarwinCoreObservation in database.
        /// </returns>
        DataTable GetDarwinCoreTable();

        /// <summary>
        /// Get all province regions
        /// </summary>
        /// <param name="context"></param>
        /// <returns>All province regions</returns>
        List<WebRegion> GetProvinceRegions(WebServiceContext context);

        /// <summary>
        /// Get specified species observation data provider.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationDataProviderId">Species observation data provider id.</param>
        /// <returns>Specified species observation data provider.</returns>
        WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context,
                                                                            Int32 speciesObservationDataProviderId);

        /// <summary>
        /// Get specified species observation data provider.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationDataProviderGuid">Species observation data provider GUID.</param>
        /// <returns>Specified species observation data provider.</returns>
        WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context,
                                                                            String speciesObservationDataProviderGuid);

        /// <summary>
        /// Get specified species observation data provider.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationDataProviderId">Species observation data provider id.</param>
        /// <returns>Specified species observation data provider.</returns>
        WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context,
                                                                            SpeciesObservationDataProviderId speciesObservationDataProviderId);

        /// <summary>
        /// Get all species observation data sources.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All species observation data sources.</returns>
        List<WebSpeciesObservationDataProvider> GetSpeciesObservationDataProviders(WebServiceContext context);

        /// <summary>
        /// Get all species observation data sources.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All species observation data sources.</returns>
        Dictionary<Int32, WebSpeciesObservationDataProvider> GetSpeciesObservationDataProvidersDictionary(WebServiceContext context);
    }
}

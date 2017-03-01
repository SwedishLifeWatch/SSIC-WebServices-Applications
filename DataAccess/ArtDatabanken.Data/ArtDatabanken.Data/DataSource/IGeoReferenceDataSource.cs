namespace ArtDatabanken.Data.DataSource
{
    /// <summary>
    /// Definition of the GeoReferenceDataSource interface.
    /// This interface is used to retrieve map related information.
    /// </summary>
    public interface IGeoReferenceDataSource
    {
        /// <summary>
        /// Get cities by name string search
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="criteria">City search criteria</param>
        /// <param name="coordinateSystem">The coordinate system used in the returned CityInformationList</param>
        /// <returns></returns>
        CityInformationList GetCitiesByNameSearchString(IUserContext userContext,
                                                        IStringSearchCriteria criteria,
                                                        ICoordinateSystem coordinateSystem);
    }
}

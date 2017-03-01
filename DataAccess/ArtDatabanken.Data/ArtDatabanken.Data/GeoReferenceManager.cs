using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles geographic information
    /// </summary>
    public class GeoReferenceManager : IGeoReferenceManager
    {
        /// <summary>
        /// This interface is used to retrieve city information from the actual data source.
        /// </summary>
        public IGeoReferenceDataSource DataSource { get; set; }

        /// <summary>
        /// Get cities by name string search
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="criteria">City search criteria</param>
        /// <param name="coordinateSystem">The coordinate system used in the returned CityInformationList</param>
        /// <returns></returns>
        public CityInformationList GetCitiesByNameSearchString(IUserContext userContext,
                                                               IStringSearchCriteria criteria,
                                                               ICoordinateSystem coordinateSystem)
        {
            return DataSource.GetCitiesByNameSearchString(userContext, criteria, coordinateSystem);
        }
    }
}

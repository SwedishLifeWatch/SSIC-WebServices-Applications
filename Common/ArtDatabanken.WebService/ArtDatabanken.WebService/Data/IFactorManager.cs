using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Database;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Interface to factor related functionality.
    /// </summary>
    public interface IFactorManager
    {
        /// <summary>
        /// Get default individual category object.
        /// </summary>
        /// <returns>Default individual category.</returns>
        WebIndividualCategory GetDefaultIndividualCategory(WebServiceContext context);


        /// <summary>
        /// A method that retrieves a period object
        /// representing the current public period.
        /// This method only handles the default period type, i.e. 
        /// The Swedish Red List.
        /// </summary>
        /// <returns>The current public period.</returns>
        WebPeriod GetCurrentPublicPeriod(WebServiceContext context);

        /// <summary>
        /// Get all factor field enumerations.
        /// </summary>
        /// <returns>All factor field enumerations.</returns>
        List<ArtDatabanken.WebService.Data.WebFactorFieldEnum> GetFactorFieldEnums(WebServiceContext context);

    }
}

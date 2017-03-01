using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Data
{
    /// <summary>
    /// Contains extension methods to a generic list of type WebDarwinCore.
    /// </summary>
    public static class ListWebDarwinCoreExtension
    {
        /// <summary>
        /// Get species observation ids.
        /// </summary>
        /// <param name="speciesObservations">Species observations.</param>
        /// <returns>Species observation ids.</returns>
        public static List<Int64> GetIds(this List<WebDarwinCore> speciesObservations)
        {
            List<Int64> speciesObservationIds;

            speciesObservationIds = null;
            if (speciesObservations.IsNotEmpty())
            {
                speciesObservationIds = new List<Int64>();
                foreach (WebDarwinCore speciesObservation in speciesObservations)
                {
                    speciesObservationIds.Add(speciesObservation.Id);
                }
            }

            return speciesObservationIds;
        }
    }
}

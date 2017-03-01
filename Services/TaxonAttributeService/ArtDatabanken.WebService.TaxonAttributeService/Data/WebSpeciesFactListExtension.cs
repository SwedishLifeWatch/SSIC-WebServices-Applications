using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.TaxonAttributeService.Data
{
    /// <summary>
    /// Contains extensions to lists of WebSpeciesFact instances.
    /// </summary>
    public static class WebSpeciesFactListExtension
    {
        /// <summary>
        /// Check data.
        /// </summary>
        /// <param name="speciesFacts">Species facts.</param>
        public static void CheckData(this List<WebSpeciesFact> speciesFacts)
        {
            if (speciesFacts.IsNotEmpty())
            {
                foreach (WebSpeciesFact speciesFact in speciesFacts)
                {
                    speciesFact.CheckData();
                }
            }
        }

        /// <summary>
        /// Get ids for all species facts.
        /// </summary>
        /// <param name="speciesFacts">Species facts.</param>
        /// <returns>Ids for all species facts.</returns>
        public static List<Int32> GetIds(this List<WebSpeciesFact> speciesFacts)
        {
            List<Int32> speciesFactIds;

            speciesFactIds = null;
            if (speciesFacts.IsNotEmpty())
            {
                speciesFactIds = new List<Int32>();
                foreach (WebSpeciesFact speciesFact in speciesFacts)
                {
                    speciesFactIds.Add(speciesFact.Id);
                }
            }

            return speciesFactIds;
        }
    }
}

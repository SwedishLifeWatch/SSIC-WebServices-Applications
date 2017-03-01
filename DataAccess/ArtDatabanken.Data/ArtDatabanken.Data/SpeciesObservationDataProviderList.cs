using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the ISpeciesObservationDataProvider interface.
    /// </summary>
    public class SpeciesObservationDataProviderList : DataId32List<ISpeciesObservationDataProvider>
    {
        /// <summary>
        /// Get GUIDs for species observation data providers.
        /// </summary>
        /// <returns>GUIDs for species observation data providers.</returns>
        public List<String> GetGuids()
        {
            List<String> guids;

            guids = null;
            if (this.IsNotEmpty())
            {
                guids = new List<String>();
                foreach (ISpeciesObservationDataProvider speciesObservationDataProvider in this)
                {
                    guids.Add(speciesObservationDataProvider.Guid);
                }
            }

            return guids;
        }
    }
}

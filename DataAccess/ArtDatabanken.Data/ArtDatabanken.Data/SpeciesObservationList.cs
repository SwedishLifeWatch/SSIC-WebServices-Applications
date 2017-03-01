using System;
using System.Collections;
using System.Linq;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the ISpeciesObservation interface.
    /// </summary>
    [Serializable]
    public class SpeciesObservationList : DataId64List<ISpeciesObservation>
    {
        /// <summary>
        /// Remove species duplicates in this SpeciesObservationList.
        /// </summary>
        public void RemoveDuplicates()
        {
            Hashtable speciesObservationIds;
            Int32 index;
            Int64 speciesObservationId;

            if (this.IsNotEmpty())
            {
                speciesObservationIds = new Hashtable(Count);
                for (index = Count - 1; index >= 0; index--)
                {
                    speciesObservationId = this[index].Id;
                    if (speciesObservationIds.Contains(speciesObservationId))
                    {
                        // Remove duplicate.
                        this.RemoveAt(index);
                    }
                    else
                    {
                        // Add id to hash table.
                        speciesObservationIds[speciesObservationId] = speciesObservationId;
                    }
                }
            }
        }
    }
}

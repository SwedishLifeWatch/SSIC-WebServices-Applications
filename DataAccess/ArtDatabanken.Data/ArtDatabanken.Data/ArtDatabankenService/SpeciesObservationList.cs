using System;
using System.Collections;
using System.Runtime.Serialization;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the SpeciesObservation class.
    /// </summary>
    [Serializable()]
    public class SpeciesObservationList : ArrayList
    {
        /// <summary>
        /// Remove species observation duplicates in this list.
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

        /// <summary>
        /// Get/set SpeciesObservation by list index.
        /// </summary>
        public new SpeciesObservation this[Int32 index]
        {
            get
            {
                return (SpeciesObservation)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}

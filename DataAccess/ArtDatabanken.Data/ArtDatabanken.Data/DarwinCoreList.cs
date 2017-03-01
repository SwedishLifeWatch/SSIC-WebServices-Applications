using System;
using System.Collections;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the IDarwinCore interface.
    /// </summary>
    [Serializable]
    public class DarwinCoreList : DataId64List<IDarwinCore> 
    {
         /// <summary>
        /// Constructor for the DarwinCoreList class.
        /// </summary>
        public DarwinCoreList()
            : this(false)
        {
        }
 
        /// <summary>
        /// Constructor for the DarwinCoreList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public DarwinCoreList(Boolean optimize)
            : base(optimize)
        {
        }

        /// <summary>
        /// Get all unique taxa in species observations.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <returns>All unique taxa in species observations.</returns>
        public TaxonList GetTaxa(IUserContext userContext)
        {
            List<Int32> taxonIds;
            TaxonList taxa;

            taxa = null;
            if (this.IsNotEmpty())
            {
                taxonIds = new List<Int32>();
                foreach (IDarwinCore speciesObservation in this)
                {
                    if (!(taxonIds.Contains(speciesObservation.Taxon.DyntaxaTaxonID)))
                    {
                        taxonIds.Add(speciesObservation.Taxon.DyntaxaTaxonID);
                    }
                }

                taxa = CoreData.TaxonManager.GetTaxa(userContext, taxonIds);
            }

            return taxa;
        }

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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the TaxonName class.
    /// </summary>
    [Serializable]
    public class TaxonNameList : DataIdList
    {
        /// <summary>
        /// Get TaxonName with specified id.
        /// </summary>
        /// <param name='taxonNameId'>Id of requested taxon name.</param>
        /// <returns>Requested taxon name.</returns>
        /// <exception cref="ArgumentException">Thrown if no taxon name has the requested id.</exception>
        public TaxonName Get(Int32 taxonNameId)
        {
            return (TaxonName)(GetById(taxonNameId));
        }

        /// <summary>
        /// Update taxa in all TaxonNames.
        /// This procedure does not add any new functionality.
        /// It is only used to improve performance.
        /// DEPRECATED
        /// Not used any more.
        /// Taxon information is sent together with
        /// TaxonName information in the dynamic data fields.
        /// </summary>
        public void UpdateTaxa()
        {
            UpdateTaxa(TaxonInformationType.Basic);
        }

        /// <summary>
        /// Get the union of two lists.
        /// All taxon names in this list whose taxon that is not
        /// contained in the taxa list is removed from this list.
        /// </summary>
        /// <param name='taxa'>The taxa list to compare with.</param>
        public override void Subset(DataIdList taxa)
        {
            Int32 index;

            if (this.IsNotEmpty())
            {
                for (index = this.Count - 1; index >= 0; index--)
                {
                    if (taxa.IsEmpty() || !taxa.Exists(((TaxonName)(this[index])).TaxonId))
                    {
                        this.RemoveAt(index);
                    }
                }
            }
        }

        /// <summary>
        /// Update taxa in all TaxonNames.
        /// This procedure does not add any new functionality.
        /// It is only used to improve performance.
        /// DEPRECATED
        /// Not used any more.
        /// Taxon information is sent together with
        /// TaxonName information in the dynamic data fields.
        /// </summary>
        /// <param name='taxonInformationType'>How much information about taxa that should be retrieved.</param>
        public void UpdateTaxa(TaxonInformationType taxonInformationType)
        {
/*
            // 
            List<Int32> taxonIds;
            TaxonList taxa;

            if (this.Count > 0)
            {
                taxonIds = new List<Int32>();
                foreach (TaxonName taxonName in this)
                {
                    if (!taxonIds.Contains(taxonName.TaxonId))
                    {
                        taxonIds.Add(taxonName.TaxonId);
                    }
                }
                taxa = TaxonManager.GetTaxa(taxonIds, taxonInformationType);
                foreach (Taxon taxon in taxa)
                {
                    foreach (TaxonName taxonName in this)
                    {
                        if (taxon.Id == taxonName.TaxonId)
                        {
                            taxonName.Taxon = taxon;
                        }
                    }
                }
            }
*/
        }

        /// <summary>
        /// Get/set TaxonName by list index.
        /// </summary>
        public new TaxonName this[Int32 index]
        {
            get
            {
                return (TaxonName)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Data
{
    /// <summary>
    /// Handle list of web taxon.
    /// </summary>
    public class WebTaxonList : ArrayList
    {
        /// <summary>
        ///  Table of taxon ids.
        /// </summary>
        private readonly Hashtable _idHashTable;
        
        /// <summary>
        /// Constructor for the WebTaxonList class.
        /// </summary>
        public WebTaxonList()
        {
            _idHashTable = new Hashtable();    
        }

        /// <summary>
        /// Add data to list.
        /// </summary>
        /// <param name='taxon'>Taxon to add.</param>
        /// <returns>The list index at which the value has been added. -1 means has not been added.</returns>
        public int Add(WebTaxon taxon)
        {
            if (taxon.IsNotNull())
            {
                int id = taxon.Id;
                if (!_idHashTable.Contains(id))
                    {
                        _idHashTable.Add(id, taxon);
                    }
                
                return base.Add(taxon);
            }

            return -1;
        }

        /// <summary>
        /// Checks if data is in the list.
        /// </summary>
        /// <param name="taxonId">Int with the ID to search for.</param>
        /// <returns>Boolean value indicating if the data exists in the list or not.</returns>
        public Boolean Exists(Int32 taxonId)
        {
            return _idHashTable.Contains(taxonId);
        }

        /// <summary>
        /// Merge a list of taxa with this list.
        /// Only taxa that are not already in the list
        /// are added to the list.
        /// </summary>
        /// <param name='taxaList'>The collection to merge.</param>
        public void Merge(List<WebTaxon> taxaList)
        {
            if (taxaList.IsNotEmpty())
            {
                foreach (WebTaxon taxon in taxaList)
                {
                    if (taxon.IsNotNull() && !Exists(taxon.Id))
                    {
                        Add(taxon);
                    }
                }
            }
        }

        /// <summary>
        /// Sort taxa based on taxon sort order.
        /// </summary>
        public override void Sort()
        {
            Sort(new TaxonComparer());
        }

        /// <summary>
        /// Handles comparing operations on taxon.
        /// </summary>
        private class TaxonComparer : IComparer
        {
            /// <summary>
            /// Compares taxon by theirs sorting order.
            /// </summary>
            /// <param name="x">A taxon.</param>
            /// <param name="y">A taxon.</param>
            /// <returns>
            /// Returns -1: taxon x has lower sorting order than taxon y.
            /// Returns 0: taxon x has equal sorting order as taxon y.
            /// Returns +1: taxon x has higher sorting order than taxon y.
            /// </returns>
            public int Compare(object x, object y)
            {
                int sortOrderX = ((WebTaxon)x).SortOrder;
                int sortOrderY = ((WebTaxon)y).SortOrder;
                if (sortOrderX < sortOrderY)
                {
                    return -1;
                }

                if (sortOrderX == sortOrderY)
                {
                    return 0;
                }
                //// if (sortOrderX > sortOrderY)
                return 1;
            }
        }
    }
}

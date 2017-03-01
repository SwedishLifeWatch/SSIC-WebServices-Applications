using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.SpeciesObservation.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Data
{
    /// <summary>
    /// Extension methods to a dictionary of type Dictionary(String, List(TaxonInformation)).
    /// </summary>
    public static class TaxonInformationListDictionaryExtension
    {
        /// <summary>
        /// Add element to taxon information list dictionary.
        /// </summary>
        /// <param name="dictionary">Add element to this dictionary.</param>
        /// <param name="key">Key to the new element.</param>
        /// <param name="value">The new element.</param>
        public static void Add(this Dictionary<String, List<TaxonInformation>> dictionary,
                               String key,
                               TaxonInformation value)
        {
            if (!(dictionary.ContainsKey(key)))
            {
                dictionary[key] = new List<TaxonInformation>();
            }

            dictionary[key].Add(value);
        }

        /// <summary>
        /// Get value from taxon information list dictionary.
        /// May be null if key is not in dictionary.
        /// First element is returned if the list only contains one element.
        /// If list contains more than one element but only one that
        /// is valid => return the valid element.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">Key of the element.</param>
        /// <returns>Value from taxon information list dictionary.</returns>
        public static TaxonInformation Get(this Dictionary<String, List<TaxonInformation>> dictionary,
                                           String key)
        {
            TaxonInformation taxonInformation;

            if (dictionary.ContainsKey(key))
            {
                taxonInformation = dictionary[key].GetSingleOrValid();
            }
            else
            {
                taxonInformation = null;
            }

            return taxonInformation;
        }

        /// <summary>
        /// Get value from taxon information list dictionary.
        /// May be null if key is not in dictionary.
        /// First element is returned if the list only contains one element.
        /// If list contains more than one element but only one that
        /// is valid => return the valid element.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">Key of the element.</param>
        /// <param name="kingdom">Kingdom.</param>
        /// <returns>Value from taxon information list dictionary.</returns>
        public static TaxonInformation GetByKingdom(this Dictionary<String, List<TaxonInformation>> dictionary,
                                                    String key,
                                                    String kingdom)
        {
            TaxonInformation taxonInformation;

            if (dictionary.ContainsKey(key))
            {
                taxonInformation = dictionary[key].GetByKingdom(kingdom).GetSingleOrValid();
            }
            else
            {
                taxonInformation = null;
            }

            return taxonInformation;
        }

        /// <summary>
        /// Get value from taxon information list dictionary.
        /// May be null if key is not in dictionary.
        /// First element is returned if the list only contains one element.
        /// If list contains more than one element but only one that
        /// is valid => return the valid element.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">Key of the element.</param>
        /// <param name="taxonRank">Taxon rank.</param>
        /// <returns>Value from taxon information list dictionary.</returns>
        public static TaxonInformation GetByTaxonRank(this Dictionary<String, List<TaxonInformation>> dictionary,
                                                      String key,
                                                      String taxonRank)
        {
            TaxonInformation taxonInformation;

            if (dictionary.ContainsKey(key))
            {
                taxonInformation = dictionary[key].GetByTaxonRank(taxonRank).GetSingleOrValid();
            }
            else
            {
                taxonInformation = null;
            }

            return taxonInformation;
        }
    }
}

using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.SpeciesObservation.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Data
{
    /// <summary>
    /// Class that contains dictionaries that can be used when scientific names from
    /// data providers are matched with scientific names from dyntaxa in order to
    /// retrieve dyntaxa taxon id.
    /// </summary>
    public class TaxonNameDictionaries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaxonNameDictionaries"/> class.
        /// </summary>
        public TaxonNameDictionaries()
        {
            Genus = new Dictionary<String, List<TaxonInformation>>();
            ScientificNames = new Dictionary<String, List<TaxonInformation>>();
            ScientificNameAndAuthor = new Dictionary<String, List<TaxonInformation>>();
        }

        /// <summary>
        /// Dictinary where genus is key.
        /// </summary>
        public Dictionary<String, List<TaxonInformation>> Genus { get; set; }

        /// <summary>
        /// Dictinary where scientific name is key.
        /// </summary>
        public Dictionary<String, List<TaxonInformation>> ScientificNames { get; set; }

        /// <summary>
        /// Dictinary where scientific name combined with author is key.
        /// </summary>
        public Dictionary<String, List<TaxonInformation>> ScientificNameAndAuthor { get; set; }
    }
}

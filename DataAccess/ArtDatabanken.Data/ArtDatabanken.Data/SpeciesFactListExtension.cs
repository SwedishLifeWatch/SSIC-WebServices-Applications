using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Extension methods for SpeciesFactList.
    /// </summary>
    public static class SpeciesFactListExtension
    {
        /// <summary>
        /// Creates a dictionary where the species facts is grouped by TaxonId and then FactorId.
        /// </summary>
        /// <param name="speciesFactList">The species fact list.</param>
        /// <returns>A dictionary where the species facts is grouped by TaxonId and then FactorId.</returns>
        public static Dictionary<int, Dictionary<FactorId, ISpeciesFact>> ToDictionaryGroupedByTaxonIdThenFactorId(this SpeciesFactList speciesFactList)
        {
            Dictionary<int, Dictionary<FactorId, ISpeciesFact>> speciesFactDictionary = new Dictionary<int, Dictionary<FactorId, ISpeciesFact>>();
            foreach (ISpeciesFact speciesFact in speciesFactList)
            {
                if (!speciesFactDictionary.ContainsKey(speciesFact.Taxon.Id))
                {
                    speciesFactDictionary.Add(speciesFact.Taxon.Id, new Dictionary<FactorId, ISpeciesFact>());
                }

                if (!speciesFactDictionary[speciesFact.Taxon.Id].ContainsKey((FactorId)speciesFact.Factor.Id))
                {
                    speciesFactDictionary[speciesFact.Taxon.Id].Add((FactorId)speciesFact.Factor.Id, speciesFact);
                }
            }

            return speciesFactDictionary;
        }
    }
}

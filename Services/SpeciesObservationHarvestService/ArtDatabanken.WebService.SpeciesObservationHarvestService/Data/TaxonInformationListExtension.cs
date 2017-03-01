using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.SpeciesObservation.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Data
{
    /// <summary>
    /// Extension methods to a list of TaxonInformation instances.
    /// </summary>
    public static class TaxonInformationListExtension
    {
        /// <summary>
        /// Get TaxonInformation instances with specified kingdom.
        /// </summary>
        /// <param name="taxonInformationList">Taxon information list.</param>
        /// <param name="kingdom">Kingdom.</param>
        /// <returns>TaxonInformation instances with specified kingdom.</returns>
        public static List<TaxonInformation> GetByKingdom(this List<TaxonInformation> taxonInformationList,
                                                          String kingdom)
        {
            List<TaxonInformation> taxonInformations;

            taxonInformations = new List<TaxonInformation>();
            if (taxonInformationList.IsNotEmpty())
            {
                foreach (TaxonInformation taxonInformation in taxonInformationList)
                {
                    if (taxonInformation.Kingdom.ToLower() == kingdom)
                    {
                        taxonInformations.Add(taxonInformation);
                    }
                }
            }

            return taxonInformations;
        }

        /// <summary>
        /// Get TaxonInformation instances with specified taxon rank.
        /// </summary>
        /// <param name="taxonInformationList">Taxon information list.</param>
        /// <param name="taxonRank">Taxon rank.</param>
        /// <returns>TaxonInformation instances with specified taxon rank.</returns>
        public static List<TaxonInformation> GetByTaxonRank(this List<TaxonInformation> taxonInformationList,
                                                            String taxonRank)
        {
            List<TaxonInformation> taxonInformations;

            taxonInformations = new List<TaxonInformation>();
            if (taxonInformationList.IsNotEmpty())
            {
                foreach (TaxonInformation taxonInformation in taxonInformationList)
                {
                    if (taxonInformation.TaxonRank.ToLower() == taxonRank)
                    {
                        taxonInformations.Add(taxonInformation);
                    }
                }
            }

            return taxonInformations;
        }

        /// <summary>
        /// Get value from taxon information list dictionary.
        /// First element is returned if the list only contains one element.
        /// If list contains more than one element but only one that
        /// is valid => return the valid element.
        /// </summary>
        /// <param name="taxonInformationList">Taxon information list.</param>
        /// <returns>
        /// Value from taxon information list dictionary.
        /// May be null if key is not in dictionary.
        /// </returns>
        public static TaxonInformation GetSingleOrValid(this List<TaxonInformation> taxonInformationList)
        {
            Int32 validCount;
            TaxonInformation taxonInformation;

            taxonInformation = null;
            if (taxonInformationList.IsNotEmpty())
            {
                if (taxonInformationList.Count == 1)
                {
                    taxonInformation = taxonInformationList[0];
                }
                else
                {
                    validCount = 0;
                    foreach (TaxonInformation taxonInformationTemp in taxonInformationList)
                    {
                        if (taxonInformationTemp.IsValid)
                        {
                            validCount++;
                            if (validCount > 1)
                            {
                                // More than one valid found.
                                taxonInformation = null;
                                break;
                            }
                            else
                            {
                                taxonInformation = taxonInformationTemp;
                            }
                        }
                    }
                }
            }

            return taxonInformation;
        }
    }
}

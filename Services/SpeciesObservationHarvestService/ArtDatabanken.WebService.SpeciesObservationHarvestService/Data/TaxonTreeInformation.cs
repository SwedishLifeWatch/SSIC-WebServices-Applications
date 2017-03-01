using System;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Data
{
    /// <summary>
    /// Container of Taxon tree information.
    /// </summary>
    public class TaxonTreeInformation
    {
        /// <summary>
        /// Contains parent taxon id.
        /// </summary>
        public Int32 ParentTaxonId;

        /// <summary>
        /// Contains child taxon id.
        /// </summary>
        public Int32 ChildTaxonId;
    }

    /// <summary>
    /// Container of extension methods for Taxon tree information.
    /// </summary>
    public static class TaxonTreeInformationExtension
    {
        /// <summary>
        /// Populate taxon tree information object with parent and child taxon id by the data reader.
        /// </summary>
        /// <param name="taxonTreeInformation">Taxon tree information.</param>
        /// <param name="dataReader">Database reader.</param>
        public static void Load(this TaxonTreeInformation taxonTreeInformation, ArtDatabanken.Database.DataReader dataReader)
        {
            taxonTreeInformation.ParentTaxonId = dataReader.GetInt32("ParentTaxonId", 0);
            taxonTreeInformation.ChildTaxonId = dataReader.GetInt32("ChildTaxonId", 0);
        }
    }
}

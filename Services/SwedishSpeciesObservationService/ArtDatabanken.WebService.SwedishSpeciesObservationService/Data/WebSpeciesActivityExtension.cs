using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SwedishSpeciesObservationService.Database;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Data
{
    /// <summary>
    /// Extension class that has methods for WebSpeciesActivity objects.
    /// </summary>
    public static class WebSpeciesActivityExtension
    {
        /// <summary>
        /// Populate species activity with content from data reader.
        /// </summary>
        /// <param name="speciesActivity">Species activity that will be populated.</param>
        /// <param name="dataReader">Data source that will populate the species activity.</param>
        public static void LoadData(this WebSpeciesActivity speciesActivity,
                                    DataReader dataReader)
        {
            speciesActivity.CategoryId = dataReader.GetInt32(SpeciesActivityData.CATEGORY_ID, 0);
            speciesActivity.Guid = dataReader.GetString(SpeciesActivityData.GUID);
            speciesActivity.Id = dataReader.GetInt32(SpeciesActivityData.ID);
            speciesActivity.Identifier = dataReader.GetString(SpeciesActivityData.IDENTIFIER);
            speciesActivity.Name = dataReader.GetString(SpeciesActivityData.NAME);
            speciesActivity.TaxonIds = new List<Int32>();
            speciesActivity.TaxonIds.Add(dataReader.GetInt32(SpeciesActivityData.TAXON_ID, (Int32)(TaxonId.Life)));
        }
    }
}

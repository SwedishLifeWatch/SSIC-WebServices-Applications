using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Data
{
    /// <summary>
    /// Handles web species observation field description mapping.
    /// </summary>
    public class WebSpeciesObservationFieldDescriptionMapping : WebSpeciesObservationFieldDescription
    {
        /// <summary>
        /// Mappings dictionary.
        /// </summary>
        public Dictionary<Int32, WebSpeciesObservationFieldMapping> MappingsDictionary { get; set; }
    }
}

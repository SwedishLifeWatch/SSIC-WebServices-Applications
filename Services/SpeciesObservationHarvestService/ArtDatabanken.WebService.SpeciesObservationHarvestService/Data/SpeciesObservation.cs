using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Data
{
    /// <summary>
    /// Handles harvest species observation.
    /// </summary>
    public class HarvestSpeciesObservation
    {
        /// <summary>
        /// Information about one species observation. 
        /// How much information that a HarvestSpeciesObservation contains
        /// depends on context, how much information that was requested
        /// and how much information that is available.
        /// </summary>
        public List<HarvestSpeciesObservationField> Fields { get; set; }

        /// <summary>
        /// Get field value from the list of harvest species observation field, finding by observation class and property ids.
        /// </summary>
        /// <param name="speciesObservationClass">Species observation class.</param>
        /// <param name="speciesObservationProperty">Species observation property.</param>
        /// <returns>Field value.</returns>
        public String GetFieldValue(WebSpeciesObservationClass speciesObservationClass, WebSpeciesObservationProperty speciesObservationProperty)
        {
            String fieldValue = null;
            foreach (HarvestSpeciesObservationField speciesObservationField in Fields)
            {
                if (speciesObservationField.Class.Id.Equals(speciesObservationClass.Id) &&
                    speciesObservationField.Property.Id.Equals(speciesObservationProperty.Id))
                {
                    fieldValue = speciesObservationField.Value;
                    break; // When field value is once found, stop searching further
                }
            }

            return fieldValue;
        }
    }
}

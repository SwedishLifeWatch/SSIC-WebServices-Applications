using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Data
{
    /// <summary>
    /// Contains extension methods to a generic list
    /// of type WebSpeciesObservationField.
    /// </summary>
    public static class ListWeb
    {
        /// <summary>
        /// Get specified species observation field.
        /// </summary>
        /// <param name="speciesObservationFields">Species observation fields.</param>
        /// <param name="speciesObservationClass">Web service request context.</param>
        /// <param name="speciesObservationProperty">Species observation search criteria.</param>
        /// <returns>Specified species observation field.</returns>
        public static WebSpeciesObservationField GetField(this List<WebSpeciesObservationField> speciesObservationFields,
                                                          String speciesObservationClass,
                                                          String speciesObservationProperty)
        {
            if (speciesObservationFields.IsNotEmpty())
            {
                foreach (WebSpeciesObservationField field in speciesObservationFields)
                {
                    if ((field.ClassIdentifier == speciesObservationClass) &&
                        (field.PropertyIdentifier == speciesObservationProperty))
                    {
                        return field;
                    }
                }
            }

            return null;
        }
    }
}

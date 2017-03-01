using System;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SpeciesObservation.Data
{
    /// <summary>
    /// Contains extension methods for class WebSpeciesObservationField.
    /// </summary>
    public static class WebSpeciesObservationFieldExtension
    {
        /// <summary>
        /// Get field name in Elasticsearch.
        /// </summary>
        /// <param name="field">Search criteria.</param>
        /// <returns>Field name in Elasticsearch.</returns>
        public static String GetFieldName(this WebSpeciesObservationField field)
        {
            if ((field.ClassIdentifier == "Project") &&
                field.PropertyIdentifier.StartsWith("ProjectParameter"))
            {
                return field.PropertyIdentifier;
            }
            else
            {
                return field.ClassIdentifier + "_" +
                       field.PropertyIdentifier;
            }
        }
    }
}

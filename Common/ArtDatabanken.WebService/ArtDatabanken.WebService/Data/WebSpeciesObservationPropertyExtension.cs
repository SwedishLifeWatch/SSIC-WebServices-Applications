using System;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to the WebSpeciesObservationProperty class.
    /// </summary>
    public static class WebSpeciesObservationPropertyExtension
    {
        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="speciesObservationProperty">Species observation property.</param>
        public static void CheckData(this WebSpeciesObservationProperty speciesObservationProperty)
        {
            if (speciesObservationProperty.IsNotNull())
            {
                speciesObservationProperty.Identifier = speciesObservationProperty.Identifier.CheckJsonInjection();
                if (speciesObservationProperty.GetProperty().IsEmpty())
                {
                    throw new ArgumentException("Species observation property has not been specified.");
                }
            }
        }

        /// <summary>
        /// Get property as string.
        /// </summary>
        /// <param name="speciesObservationProperty">Species observation property.</param>
        /// <returns>Property as string.</returns>
        public static String GetProperty(this WebSpeciesObservationProperty speciesObservationProperty)
        {
            String propertyString;

            propertyString = null;
            if (speciesObservationProperty.IsNotNull())
            {
                if (speciesObservationProperty.Id == SpeciesObservationPropertyId.None)
                {
                    propertyString = speciesObservationProperty.Identifier;
                }
                else
                {
                    propertyString = speciesObservationProperty.Id.ToString();
                }
            }
            return propertyString;
        }
    }
}

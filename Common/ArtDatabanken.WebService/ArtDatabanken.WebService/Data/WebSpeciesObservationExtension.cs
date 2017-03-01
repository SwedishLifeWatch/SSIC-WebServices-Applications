using System;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to the WebSpeciesObservation class.
    /// </summary>
    public static class WebSpeciesObservationExtension
    {
        /// <summary>
        /// Get value for a spiecesObservationField
        /// </summary>
        /// <param name="speciesObservation">Species observation.</param>
        /// <param name="speciesObservationClass">WebSpeciesObservationClass that the field belongs to.</param>
        /// <param name="speciesObservationProperty">WebSpeciesObservationProperty for the field.</param>
        /// <returns>Field value as string.</returns>
        public static String GetFieldValue(this WebSpeciesObservation speciesObservation,
                                           WebSpeciesObservationClass speciesObservationClass,
                                           WebSpeciesObservationProperty speciesObservationProperty)
        {
            String speciesObservationClassIdentifier,
                   speciesObservationPropertyIdentifier, fieldValue = null;

            speciesObservationClassIdentifier = speciesObservationClass.Id.ToString();
            speciesObservationPropertyIdentifier = speciesObservationProperty.Id.ToString();
            foreach (WebSpeciesObservationField speciesObservationField in speciesObservation.Fields)
            {
                if (speciesObservationField.ClassIdentifier.Equals(speciesObservationClassIdentifier) &&
                    speciesObservationField.PropertyIdentifier.Equals(speciesObservationPropertyIdentifier))
                {
                    fieldValue = speciesObservationField.Value;
                    continue;
                }
            }

            return fieldValue;
        }

        /// <summary>
        /// Get a spiecesObservationField
        /// </summary>
        /// <param name="speciesObservation">Species observation.</param>
        /// <param name="speciesObservationClass">WebSpeciesObservationClass that the field belongs to.</param>
        /// <param name="speciesObservationProperty">WebSpeciesObservationProperty for the field.</param>
        /// <returns>WebSpeciesObservationField.</returns>
        public static WebSpeciesObservationField GetField(this WebSpeciesObservation speciesObservation, WebSpeciesObservationClass speciesObservationClass, WebSpeciesObservationProperty speciesObservationProperty)
        {
            WebSpeciesObservationField field = null;
            String speciesObservationClassIdentifier,
                   speciesObservationPropertyIdentifier;

            speciesObservationClassIdentifier = speciesObservationClass.Id.ToString();
            speciesObservationPropertyIdentifier = speciesObservationProperty.Id.ToString();
            foreach (WebSpeciesObservationField speciesObservationField in speciesObservation.Fields)
            {
                if (speciesObservationField.ClassIdentifier.Equals(speciesObservationClassIdentifier) &&
                    speciesObservationField.PropertyIdentifier.Equals(speciesObservationPropertyIdentifier))
                {
                    field = speciesObservationField;
                    continue;
                }
            }
            return field;
        }
    }
}

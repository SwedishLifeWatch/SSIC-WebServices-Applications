using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Data
{
    /// <summary>
    /// Contains extension methods to a list of
    /// WebSpeciesObservationFieldSpecification instances.
    /// </summary>
    public static class ListWebSpeciesObservationFieldSpecificationExtension
    {
        /// <summary>
        /// Determines whether a species observation field
        /// specification is in the list.
        /// </summary>
        /// <param name='fieldSpecifications'>Species observation field specifiactions.</param>
        /// <param name='classIdentifier'>Species observation class identifier.</param>
        /// <param name='propertyIdentifier'>Species observation property identifier.</param>
        /// <returns>
        /// True if the species observation field
        /// specification is in the list.
        /// </returns>
        public static Boolean ContainsFieldSpecification(this List<WebSpeciesObservationFieldSpecification> fieldSpecifications,
                                                         String classIdentifier,
                                                         String propertyIdentifier)
        {
            if (fieldSpecifications.IsEmpty())
            {
                return false;
            }

            foreach (WebSpeciesObservationFieldSpecification tempFieldSpecification in fieldSpecifications)
            {
                if ((tempFieldSpecification.Class.GetClass() == classIdentifier) &&
                    (tempFieldSpecification.Property.GetProperty() == propertyIdentifier))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether a species observation field
        /// specification is in the list.
        /// </summary>
        /// <param name='fieldSpecifications'>Species observation field specifiactions.</param>
        /// <param name='fieldSpecification'>Species observation field specification.</param>
        /// <returns>
        /// True if the species observation field
        /// specification is in the list.
        /// </returns>
        public static Boolean ContainsFieldSpecification(this List<WebSpeciesObservationFieldSpecification> fieldSpecifications,
                                                         WebSpeciesObservationFieldSpecification fieldSpecification)
        {
            if (fieldSpecifications.IsEmpty())
            {
                return false;
            }

            foreach (WebSpeciesObservationFieldSpecification tempFieldSpecification in fieldSpecifications)
            {
                if ((tempFieldSpecification.Class.GetClass() == fieldSpecification.Class.GetClass()) &&
                    (tempFieldSpecification.Property.GetProperty() == fieldSpecification.Property.GetProperty()))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Merge a species observation field specification into the list.
        /// </summary>
        /// <param name='fieldSpecifications'>Species observation fields.</param>
        /// <param name='fieldSpecification'>Species observation field.</param>
        public static void Merge(this List<WebSpeciesObservationFieldSpecification> fieldSpecifications,
                                 WebSpeciesObservationFieldSpecification fieldSpecification)
        {
            if (!(fieldSpecifications.ContainsFieldSpecification(fieldSpecification)))
            {
                fieldSpecifications.Add(fieldSpecification);
            }
        }

        /// <summary>
        /// Merge a species observation field specifications into the list.
        /// </summary>
        /// <param name='fieldSpecifications'>Current species observation field specifications.</param>
        /// <param name='fieldSpecificationsToAdd'>Species observation field specifications to add.</param>
        public static void Merge(this List<WebSpeciesObservationFieldSpecification> fieldSpecifications,
                                 List<WebSpeciesObservationFieldSpecification> fieldSpecificationsToAdd)
        {
            if (fieldSpecificationsToAdd.IsNotEmpty())
            {
                foreach (WebSpeciesObservationFieldSpecification fieldSpecification in fieldSpecificationsToAdd)
                {
                    fieldSpecifications.Merge(fieldSpecification);
                }
            }
        }

        /// <summary>
        /// Merge a species observation field specification into the list.
        /// </summary>
        /// <param name='fieldSpecifications'>Species observation field specifiactions.</param>
        /// <param name='classId'>Species observation class.</param>
        /// <param name='propertyId'>Species observation property.</param>
        public static void Merge(this List<WebSpeciesObservationFieldSpecification> fieldSpecifications,
                                 SpeciesObservationClassId classId,
                                 SpeciesObservationPropertyId propertyId)
        {
            WebSpeciesObservationFieldSpecification fieldSpecification;

            fieldSpecification = new WebSpeciesObservationFieldSpecification();
            fieldSpecification.Class = new WebSpeciesObservationClass();
            fieldSpecification.Class.Id = classId;
            fieldSpecification.Property = new WebSpeciesObservationProperty();
            fieldSpecification.Property.Id = propertyId;
            Merge(fieldSpecifications, fieldSpecification);
        }
    }
}

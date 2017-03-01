using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Data
{
    /// <summary>
    /// Contains extension to the WebSpeciesObservation class.
    /// </summary>
    public static class WebSpeciesObservationExtension
    {
        /// <summary>
        /// Add a field to the species observation.
        /// </summary>
        /// <param name="speciesObservation">The species observation.</param>
        /// <param name="speciesObservationClassId">Species observation class id.</param>
        /// <param name="speciesObservationPropertyId">Species observation property id.</param>
        /// <param name="value">A Boolean value.</param>
        public static void AddField(this WebSpeciesObservation speciesObservation,
                                    SpeciesObservationClassId speciesObservationClassId,
                                    SpeciesObservationPropertyId speciesObservationPropertyId,
                                    Boolean value)
        {
            WebSpeciesObservationField field;

            if (speciesObservation.Fields.IsNull())
            {
                speciesObservation.Fields = new List<WebSpeciesObservationField>();
            }

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = speciesObservationClassId.ToString();
            field.PropertyIdentifier = speciesObservationPropertyId.ToString();
            field.Type = WebDataType.Boolean;
            field.Value = value.WebToString();
            speciesObservation.Fields.Add(field);
        }

        /// <summary>
        /// Add a field to the species observation.
        /// </summary>
        /// <param name="speciesObservation">The species observation.</param>
        /// <param name="speciesObservationClassId">Species observation class id.</param>
        /// <param name="speciesObservationPropertyId">Species observation property id.</param>
        /// <param name="value">A DateTime value.</param>
        public static void AddField(this WebSpeciesObservation speciesObservation,
                                    SpeciesObservationClassId speciesObservationClassId,
                                    SpeciesObservationPropertyId speciesObservationPropertyId,
                                    DateTime value)
        {
            WebSpeciesObservationField field;

            if (speciesObservation.Fields.IsNull())
            {
                speciesObservation.Fields = new List<WebSpeciesObservationField>();
            }

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = speciesObservationClassId.ToString();
            field.PropertyIdentifier = speciesObservationPropertyId.ToString();
            field.Type = WebDataType.DateTime;
            field.Value = value.WebToString();
            speciesObservation.Fields.Add(field);
        }

        /// <summary>
        /// Add a field to the species observation.
        /// </summary>
        /// <param name="speciesObservation">The species observation.</param>
        /// <param name="speciesObservationClassId">Species observation class id.</param>
        /// <param name="speciesObservationPropertyId">Species observation property id.</param>
        /// <param name="value">A Double value.</param>
        public static void AddField(this WebSpeciesObservation speciesObservation,
                                    SpeciesObservationClassId speciesObservationClassId,
                                    SpeciesObservationPropertyId speciesObservationPropertyId,
                                    Double value)
        {
            WebSpeciesObservationField field;

            if (speciesObservation.Fields.IsNull())
            {
                speciesObservation.Fields = new List<WebSpeciesObservationField>();
            }

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = speciesObservationClassId.ToString();
            field.PropertyIdentifier = speciesObservationPropertyId.ToString();
            field.Type = WebDataType.Float64;
            field.Value = value.WebToString();
            speciesObservation.Fields.Add(field);
        }

        /// <summary>
        /// Add a field to the species observation.
        /// </summary>
        /// <param name="speciesObservation">The species observation.</param>
        /// <param name="speciesObservationClassId">Species observation class id.</param>
        /// <param name="speciesObservationPropertyId">Species observation property id.</param>
        /// <param name="value">An Int32 value.</param>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static void AddField(this WebSpeciesObservation speciesObservation,
                                    SpeciesObservationClassId speciesObservationClassId,
                                    SpeciesObservationPropertyId speciesObservationPropertyId,
                                    Int32 value)
        {
            WebSpeciesObservationField field;

            if (speciesObservation.Fields.IsNull())
            {
                speciesObservation.Fields = new List<WebSpeciesObservationField>();
            }

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = speciesObservationClassId.ToString();
            field.PropertyIdentifier = speciesObservationPropertyId.ToString();
            field.Type = WebDataType.Int32;
            field.Value = value.WebToString();
            speciesObservation.Fields.Add(field);
        }

        /// <summary>
        /// Add a field to the species observation.
        /// </summary>
        /// <param name="speciesObservation">The species observation.</param>
        /// <param name="speciesObservationClassId">Species observation class id.</param>
        /// <param name="speciesObservationPropertyId">Species observation property id.</param>
        /// <param name="value">An Int64 value.</param>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static void AddField(this WebSpeciesObservation speciesObservation,
                                    SpeciesObservationClassId speciesObservationClassId,
                                    SpeciesObservationPropertyId speciesObservationPropertyId,
                                    Int64 value)
        {
            WebSpeciesObservationField field;

            if (speciesObservation.Fields.IsNull())
            {
                speciesObservation.Fields = new List<WebSpeciesObservationField>();
            }

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = speciesObservationClassId.ToString();
            field.PropertyIdentifier = speciesObservationPropertyId.ToString();
            field.Type = WebDataType.Int64;
            field.Value = value.WebToString();
            speciesObservation.Fields.Add(field);
        }

        /// <summary>
        /// Add a field to the species observation.
        /// </summary>
        /// <param name="speciesObservation">The species observation.</param>
        /// <param name="speciesObservationClassId">Species observation class id.</param>
        /// <param name="speciesObservationPropertyId">Species observation property id.</param>
        /// <param name="value">A string value.</param>
        public static void AddField(this WebSpeciesObservation speciesObservation,
                                    SpeciesObservationClassId speciesObservationClassId,
                                    SpeciesObservationPropertyId speciesObservationPropertyId,
                                    String value)
        {
            WebSpeciesObservationField field;

            if (value.IsNotEmpty())
            {
                if (speciesObservation.Fields.IsNull())
                {
                    speciesObservation.Fields = new List<WebSpeciesObservationField>();
                }

                field = new WebSpeciesObservationField();
                field.ClassIdentifier = speciesObservationClassId.ToString();
                field.PropertyIdentifier = speciesObservationPropertyId.ToString();
                field.Type = WebDataType.String;
                field.Value = value;
                speciesObservation.Fields.Add(field);
            }
        }
    }
}
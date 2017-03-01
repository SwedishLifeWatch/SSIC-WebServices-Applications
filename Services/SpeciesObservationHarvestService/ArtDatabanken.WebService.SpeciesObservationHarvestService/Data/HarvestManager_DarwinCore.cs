using System;
using System.Data;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Database;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Database;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Data
{
    /// <summary>
    /// The harvest manager.
    /// </summary>
    public partial class HarvestManager
    {
        /// <summary>
        /// The create.
        /// </summary>
        public const string CREATE = "CREATE";

        /// <summary>
        /// The update.
        /// </summary>
        public const string UPDATE = "UPDATE";

        /// <summary>
        /// The default.
        /// </summary>
        public const string DEFAULT = "DEFAULT";

        /// <summary>
        /// Add CONSERVATION data to DarwinCore row.
        /// </summary>
        /// <param name="darwinCoreRow">Data table row with Darwin core information.</param>
        /// <param name="speciesObservationField">Species observation field.</param>
        private static void AddDarwinCoreConservation(DataRow darwinCoreRow, HarvestSpeciesObservationField speciesObservationField)
        {
            switch (speciesObservationField.Property.Id)
            {
                case SpeciesObservationPropertyId.ProtectionLevel:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.ProtectionLevel)] = speciesObservationField.Value.WebParseInt32();
                    break;
            }
        }

        /// <summary>
        /// Add DARWINCORE data to DarwinCore row.
        /// </summary>
        /// <param name="darwinCoreRow">Data table row with Darwin core information.</param>
        /// <param name="speciesObservationField">Species observation field.</param>
        private static void AddDarwinCore(DataRow darwinCoreRow,
                                          HarvestSpeciesObservationField speciesObservationField)
        {
            switch (speciesObservationField.Property.Id)
            {
                case SpeciesObservationPropertyId.AccessRights:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.AccessRights)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.BasisOfRecord:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.BasisOfRecord)] = speciesObservationField.Value;
                    break;

                // case SpeciesObservationPropertyId.BibliographicCitation:
                // darwinCoreRow[(Int32)(DarwinCoreColumn.BibliographicCitation)] = speciesObservationField.Value;
                // break;
                case SpeciesObservationPropertyId.CollectionCode:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.CollectionCode)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.CollectionID:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.CollectionId)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.DataGeneralizations:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.DataGeneralizations)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.DynamicProperties:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.DynamicProperties)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.InformationWithheld:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.InformationWithheld)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.InstitutionCode:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.InstitutionCode)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.InstitutionID:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.InstitutionId)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.Language:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.Language)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.Modified:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.Modified)] = speciesObservationField.Value.WebParseDateTime();
                    break;
                case SpeciesObservationPropertyId.Owner:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.Owner)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.OwnerInstitutionCode:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.OwnerInstitutionCode)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.References:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.References)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.ReportedBy:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.ReportedBy)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.ReportedDate:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.ReportedDate)] = speciesObservationField.Value.WebParseDateTime();
                    break;
                case SpeciesObservationPropertyId.Rights:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.Rights)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.RightsHolder:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.RightsHolder)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.SpeciesObservationURL:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.SpeciesObservationUrl)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.Type:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.Type)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.ValidationStatus:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.ValidationStatus)] = speciesObservationField.Value;
                    break;
            }
        }

        /// <summary>
        /// Add EVENT data to DarwinCore row.
        /// </summary>
        /// <param name="darwinCoreRow">Data table row with Darwin core information.</param>
        /// <param name="speciesObservationField">Species observation field.</param>
        private static void AddDarwinCoreEvent(DataRow darwinCoreRow,
                                               HarvestSpeciesObservationField speciesObservationField)
        {
            switch (speciesObservationField.Property.Id)
            {
                case SpeciesObservationPropertyId.Day:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.Day)] = speciesObservationField.Value.WebParseInt32();
                    break;
                case SpeciesObservationPropertyId.End:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.End)] = speciesObservationField.Value.WebParseDateTime();
                    break;
                case SpeciesObservationPropertyId.EndDayOfYear:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.EndDayOfYear)] = speciesObservationField.Value.WebParseInt32();
                    break;
                case SpeciesObservationPropertyId.EventDate:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.EventDate)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.EventID:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.EventId)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.EventRemarks:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.EventRemarks)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.EventTime:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.EventTime)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.FieldNotes:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.FieldNotes)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.FieldNumber:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.FieldNumber)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.Habitat:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.Habitat)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.Month:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.Month)] = speciesObservationField.Value.WebParseInt32();
                    break;
                case SpeciesObservationPropertyId.SamplingEffort:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.SamplingEffort)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.SamplingProtocol:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.SamplingProtocol)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.Start:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.Start)] = speciesObservationField.Value.WebParseDateTime();
                    break;
                case SpeciesObservationPropertyId.StartDayOfYear:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.StartDayOfYear)] = speciesObservationField.Value.WebParseInt32();
                    break;
                case SpeciesObservationPropertyId.VerbatimEventDate:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.VerbatimEventDate)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.Year:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.Year)] = speciesObservationField.Value.WebParseInt32();
                    break;
            }
        }

        /// <summary>
        /// Add IDENTIFICATION data to DarwinCore row.
        /// </summary>
        /// <param name="darwinCoreRow">Data table row with Darwin core information.</param>
        /// <param name="speciesObservationField">Species observation field.</param>
        private static void AddDarwinCoreIdentification(DataRow darwinCoreRow,
                                                        HarvestSpeciesObservationField speciesObservationField)
        {
            switch (speciesObservationField.Property.Id)
            {
                case SpeciesObservationPropertyId.DateIdentified:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.DateIdentified)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.IdentificationID:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.IdentificationId)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.IdentificationQualifier:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.IdentificationQualifier)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.IdentificationReferences:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.IdentificationReferences)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.IdentificationRemarks:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.IdentificationRemarks)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.IdentificationVerificationStatus:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.IdentificationVerificationStatus)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.IdentifiedBy:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.IdentifiedBy)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.TypeStatus:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.TypeStatus)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.UncertainDetermination:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.UncertainDetermination)] = speciesObservationField.Value.WebParseBoolean();
                    break;
            }
        }

        /// <summary>
        /// Add LOCATION data to DarwinCore row.
        /// </summary>
        /// <param name="darwinCoreRow">Data table row with Darwin core information.</param>
        /// <param name="speciesObservationField">Species observation field.</param>
        private static void AddDarwinCoreLocation(DataRow darwinCoreRow,
                                                  HarvestSpeciesObservationField speciesObservationField)
        {
            switch (speciesObservationField.Property.Id)
            {
                //// case SpeciesObservationPropertyId.Continent:
                ////   darwinCoreRow[(Int32)(DarwinCoreColumn.Continent)] = speciesObservationField.Value;
                ////   break;
                case SpeciesObservationPropertyId.CoordinateM:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.CoordinateM)] = speciesObservationField.Value.WebParseDouble();
                    break;
                case SpeciesObservationPropertyId.CoordinatePrecision:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.CoordinatePrecision)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.CoordinateUncertaintyInMeters:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.CoordinateUncertaintyInMeters)] = speciesObservationField.Value.WebParseInt32();
                    break;
                case SpeciesObservationPropertyId.CoordinateX:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.CoordinateX)] = (Int32)speciesObservationField.Value.WebParseDouble();
                    break;
                case SpeciesObservationPropertyId.CoordinateY:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.CoordinateY)] = (Int32)speciesObservationField.Value.WebParseDouble();
                    break;

                case SpeciesObservationPropertyId.CoordinateX_RT90:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.CoordinateX_RT90)] = (Int32)speciesObservationField.Value.WebParseDouble();
                    break;
                case SpeciesObservationPropertyId.CoordinateY_RT90:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.CoordinateY_RT90)] = (Int32)speciesObservationField.Value.WebParseDouble();
                    break;

                case SpeciesObservationPropertyId.CoordinateX_SWEREF99:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.CoordinateX_SWEREF99)] = (Int32)speciesObservationField.Value.WebParseDouble();
                    break;
                case SpeciesObservationPropertyId.CoordinateY_SWEREF99:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.CoordinateY_SWEREF99)] = (Int32)speciesObservationField.Value.WebParseDouble();
                    break;

                case SpeciesObservationPropertyId.CoordinateX_ETRS89_LAEA:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.CoordinateX_ETRS89_LAEA)] = (Int32)speciesObservationField.Value.WebParseDouble();
                    break;
                case SpeciesObservationPropertyId.CoordinateY_ETRS89_LAEA:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.CoordinateY_ETRS89_LAEA)] = (Int32)speciesObservationField.Value.WebParseDouble();
                    break;

                case SpeciesObservationPropertyId.CoordinateZ:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.CoordinateZ)] = (Int32)speciesObservationField.Value.WebParseDouble();
                    break;
                //// case SpeciesObservationPropertyId.Country:
                ////    darwinCoreRow[(Int32)(DarwinCoreColumn.Country)] = speciesObservationField.Value;
                ////    break;
                case SpeciesObservationPropertyId.CountryCode:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.CountryCode)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.County:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.County)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.DecimalLatitude:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.DecimalLatitude)] = speciesObservationField.Value.WebParseDouble();
                    break;
                case SpeciesObservationPropertyId.DecimalLongitude:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.DecimalLongitude)] = speciesObservationField.Value.WebParseDouble();
                    break;
                case SpeciesObservationPropertyId.FootprintSpatialFit:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.FootprintSpatialFit)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.FootprintSRS:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.FootprintSrs)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.FootprintWKT:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.FootprintWkt)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.GeodeticDatum:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.GeodeticDatum)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.GeoreferencedBy:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.GeoreferencedBy)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.GeoreferencedDate:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.GeoreferencedDate)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.GeoreferenceProtocol:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.GeoreferenceProtocol)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.GeoreferenceRemarks:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.GeoreferenceRemarks)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.GeoreferenceSources:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.GeoreferenceSources)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.GeoreferenceVerificationStatus:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.GeoreferenceVerificationStatus)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.HigherGeography:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.HigherGeography)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.HigherGeographyID:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.HigherGeographyId)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.Island:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.Island)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.IslandGroup:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.IslandGroup)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.Locality:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.Locality)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.LocationAccordingTo:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.LocationAccordingTo)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.LocationId:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.LocationId)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.LocationRemarks:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.LocationRemarks)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.LocationURL:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.LocationUrl)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.MaximumDepthInMeters:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.MaximumDepthInMeters)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.MaximumDistanceAboveSurfaceInMeters:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.MaximumDistanceAboveSurfaceInMeters)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.MaximumElevationInMeters:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.MaximumElevationInMeters)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.MinimumDepthInMeters:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.MinimumDepthInMeters)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.MinimumDistanceAboveSurfaceInMeters:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.MinimumDistanceAboveSurfaceInMeters)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.MinimumElevationInMeters:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.MinimumElevationInMeters)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.Municipality:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.Municipality)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.Parish:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.Parish)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.PointRadiusSpatialFit:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.PointRadiusSpatialFit)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.StateProvince:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.StateProvince)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.VerbatimCoordinates:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.VerbatimCoordinates)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.VerbatimCoordinateSystem:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.VerbatimCoordinateSystem)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.VerbatimDepth:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.VerbatimDepth)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.VerbatimElevation:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.VerbatimElevation)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.VerbatimLatitude:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.VerbatimLatitude)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.VerbatimLocality:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.VerbatimLocality)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.VerbatimLongitude:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.VerbatimLongitude)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.VerbatimSRS:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.VerbatimSrs)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.WaterBody:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.WaterBody)] = speciesObservationField.Value;
                    break;
            }
        }

        /// <summary>
        /// Add Occurrence data to  DarwinCore row.
        /// </summary>
        /// <param name="darwinCoreRow">Data table row with Darwin core information.</param>
        /// <param name="speciesObservationField">Species observation field.</param>
        private static void AddDarwinCoreOccurrence(DataRow darwinCoreRow,
                                                    HarvestSpeciesObservationField speciesObservationField)
        {
            switch (speciesObservationField.Property.Id)
            {
                case SpeciesObservationPropertyId.ActivityId:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.ActivityId)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.AssociatedMedia:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.AssociatedMedia)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.AssociatedOccurrences:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.AssociatedOccurrences)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.AssociatedReferences:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.AssociatedReferences)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.AssociatedSequences:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.AssociatedSequences)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.AssociatedTaxa:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.AssociatedTaxa)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.Behavior:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.Behavior)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.BirdNestActivityId:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.BirdNestActivityId)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.CatalogNumber:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.CatalogNumber)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.Disposition:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.Disposition)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.EstablishmentMeans:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.EstablishmentMeans)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.IndividualCount:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.IndividualCount)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.IndividualID:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.IndividualId)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.IsNaturalOccurrence:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.IsNaturalOccurrence)] = speciesObservationField.Value.WebParseBoolean();
                    break;
                case SpeciesObservationPropertyId.IsNeverFoundObservation:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.IsNeverFoundObservation)] = speciesObservationField.Value.WebParseBoolean();
                    break;
                case SpeciesObservationPropertyId.IsNotRediscoveredObservation:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.IsNotRediscoveredObservation)] = speciesObservationField.Value.WebParseBoolean();
                    break;
                case SpeciesObservationPropertyId.IsPositiveObservation:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.IsPositiveObservation)] = speciesObservationField.Value.WebParseBoolean();
                    break;
                case SpeciesObservationPropertyId.LifeStage:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.LifeStage)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.OccurrenceID:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.OccurrenceId)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.OccurrenceRemarks:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.OccurrenceRemarks)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.OccurrenceStatus:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.OccurrenceStatus)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.OccurrenceURL:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.OccurrenceUrl)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.OtherCatalogNumbers:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.OtherCatalogNumbers)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.Preparations:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.Preparations)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.PreviousIdentifications:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.PreviousIdentifications)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.Quantity:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.Quantity)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.QuantityUnit:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.QuantityUnit)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.RecordedBy:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.RecordedBy)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.RecordNumber:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.RecordNumber)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.ReproductiveCondition:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.ReproductiveCondition)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.Sex:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.Sex)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.Substrate:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.Substrate)] = speciesObservationField.Value;
                    break;
            }
        }

        /// <summary>
        /// Add PROJECT data to DarwinCore row.
        /// </summary>
        /// <param name="darwinCoreRow">Data table row with Darwin core information.</param>
        /// <param name="speciesObservationField">Species observation field.</param>
        private static void AddDarwinCoreProject(DataRow darwinCoreRow,
                                                 HarvestSpeciesObservationField speciesObservationField)
        {
            switch (speciesObservationField.Property.Id)
            {
                case SpeciesObservationPropertyId.IsPublic:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.ProjectIsPublic)] = speciesObservationField.Value.WebParseBoolean();
                    break;
                case SpeciesObservationPropertyId.ProjectCategory:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.ProjectCategory)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.ProjectDescription:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.ProjectDescription)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.ProjectEndDate:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.ProjectEndDate)] = speciesObservationField.Value.WebParseDateTime();
                    break;
                case SpeciesObservationPropertyId.ProjectID:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.ProjectId)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.ProjectName:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.ProjectName)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.ProjectOwner:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.ProjectOwner)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.ProjectStartDate:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.ProjectStartDate)] = speciesObservationField.Value.WebParseDateTime();
                    break;
                case SpeciesObservationPropertyId.ProjectURL:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.ProjectUrl)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.SurveyMethod:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.ProjectSurveyMethod)] = speciesObservationField.Value;
                    break;
            }
        }

        /// <summary>
        /// Add TAXON data to DarwinCore row.
        /// </summary>
        /// <param name="darwinCoreRow">Data table row with Darwin core information.</param>
        /// <param name="speciesObservationField">Species observation field.</param>
        private static void AddDarwinCoreTaxon(DataRow darwinCoreRow,
                                               HarvestSpeciesObservationField speciesObservationField)
        {
            switch (speciesObservationField.Property.Id)
            {
                case SpeciesObservationPropertyId.DyntaxaTaxonID:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.DyntaxaTaxonId)] = speciesObservationField.Value.WebParseInt32();
                    break;
                case SpeciesObservationPropertyId.TaxonRemarks:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.TaxonRemarks)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.VerbatimScientificName:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.VerbatimScientificName)] = speciesObservationField.Value;
                    break;
                case SpeciesObservationPropertyId.VerbatimTaxonRank:
                    darwinCoreRow[(Int32)(DarwinCoreColumn.VerbatimTaxonRank)] = speciesObservationField.Value;
                    break;
            }
        }

        /// <summary>
        /// Add any Species Observation Field to correct Darwin Core row.
        /// </summary>
        /// <param name="darwinCoreRow">Data table row with Darwin core information.</param>
        /// <param name="speciesObservationField">Species observation field.</param>
        private static void AddSpeciesObservationField(DataRow darwinCoreRow,
                                                       HarvestSpeciesObservationField speciesObservationField)
        {
            if ((speciesObservationField.Class.Id != SpeciesObservationClassId.None) &&
                (!speciesObservationField.IsClassIndexSpecified) &&
                (speciesObservationField.Property.Id != SpeciesObservationPropertyId.None) &&
                (!speciesObservationField.IsPropertyIndexSpecified))
            {
                // Select and add which field to add
                switch (speciesObservationField.Class.Id)
                {
                    case SpeciesObservationClassId.DarwinCore:
                        AddDarwinCore(darwinCoreRow, speciesObservationField);
                        break;
                    case SpeciesObservationClassId.Conservation:
                        AddDarwinCoreConservation(darwinCoreRow, speciesObservationField);
                        break;
                    case SpeciesObservationClassId.Event:
                        AddDarwinCoreEvent(darwinCoreRow, speciesObservationField);
                        break;
                    case SpeciesObservationClassId.Identification:
                        AddDarwinCoreIdentification(darwinCoreRow, speciesObservationField);
                        break;
                    case SpeciesObservationClassId.Location:
                        AddDarwinCoreLocation(darwinCoreRow, speciesObservationField);
                        break;
                    case SpeciesObservationClassId.Occurrence:
                        AddDarwinCoreOccurrence(darwinCoreRow, speciesObservationField);
                        break;
                    case SpeciesObservationClassId.Project:
                        AddDarwinCoreProject(darwinCoreRow, speciesObservationField);
                        break;
                    case SpeciesObservationClassId.Taxon:
                        AddDarwinCoreTaxon(darwinCoreRow, speciesObservationField);
                        break;
                }
            }
        }

        /// <summary>
        /// The get darwin core table.
        /// </summary>
        /// <param name="tableType">
        /// The table type.
        /// </param>
        /// <returns>
        /// The darwin Core Table.<see cref="DataTable"/>.
        /// </returns>
        internal static DataTable GetDarwinCoreTable(string tableType)
        {
            string tableName = DarwinCoreData.TABLE_NAME;
            switch (tableType)
            {
                case CREATE:
                    tableName = DarwinCoreData.CREATE_TABLE_NAME;
                    break;
                case UPDATE:
                    tableName = DarwinCoreData.UPDATE_TABLE_NAME;
                    break;
            }

            DataTable darwinCoreTable = WebServiceData.SpeciesObservationManager.GetDarwinCoreTable();
            darwinCoreTable.TableName = tableName;
            return darwinCoreTable;
        }

        /// <summary>
        /// Get DataTable with same definition as table
        /// SpeciesObservationFieldDescription in database.
        /// </summary>
        /// <returns>
        /// DataTable with same definition as table
        /// SpeciesObservationFieldDescription in database.
        /// </returns>
        internal static DataTable GetSpeciesObservationFieldDescriptionTable()
        {
            using (var table = new DataTable(SpeciesObservationFieldDescriptionData.TABLE_NAME))
            {
                table.Columns.Add(SpeciesObservationFieldDescriptionData.ID, typeof(Int32));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.CLASS, typeof(String));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.NAME, typeof(String));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.TYPE, typeof(String));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.DEFINITION, typeof(String));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.DEFINITION_URL, typeof(String));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.DOCUMENTATION, typeof(String));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.DOCUMENTATION_URL, typeof(String));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.LABEL, typeof(String));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.SWEDISH_LABEL, typeof(String));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.GUID, typeof(String));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.UUID, typeof(String));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.SORT_ORDER, typeof(Int32));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.IMPORTANCE, typeof(Int32));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.REMARKS, typeof(String));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.IS_ACCEPTED_BY_TDWG, typeof(Boolean));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.IS_IMPLEMENTED, typeof(Boolean));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.IS_PLANNED, typeof(Boolean));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.IS_MANDATORY, typeof(Boolean));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.IS_MANDATORY_FROM_PROVIDER, typeof(Boolean));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.IS_OBTAINED_FROM_PROVIDER, typeof(Boolean));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.IS_CLASS_NAME, typeof(Boolean));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.IS_PUBLIC, typeof(Boolean));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.IS_SORTABLE, typeof(Boolean));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.IS_DARWINCORE, typeof(Boolean));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.IS_SEARCHABLEFIELD, typeof(Boolean));
                table.Columns.Add(SpeciesObservationFieldDescriptionData.PERSISTED_IN_TABLE, typeof(String));

                return table;
            }
        }

        /// <summary>
        /// Get DataTable with same definition as table
        /// SpeciesObservationFieldMapping in database.
        /// </summary>
        /// <returns>
        /// DataTable with same definition as table
        /// SpeciesObservationFieldMapping in database.
        /// </returns>
        internal static DataTable GetSpeciesObservationFieldMappingTable()
        {
            using (var table = new DataTable(SpeciesObservationFieldMappingData.TABLE_NAME))
            {
                table.Columns.Add(SpeciesObservationFieldMappingData.ID, typeof(Int32));
                table.Columns.Add(SpeciesObservationFieldMappingData.DATA_PROVIDER_ID, typeof(Int32));
                table.Columns.Add(SpeciesObservationFieldMappingData.FIELD_ID, typeof(Int32));
                table.Columns.Add(SpeciesObservationFieldMappingData.FIELD_NAME, typeof(String));
                table.Columns.Add(SpeciesObservationFieldMappingData.CLASS, typeof(String));
                table.Columns.Add(SpeciesObservationFieldMappingData.PROVIDER_FIELD_NAME, typeof(String));
                table.Columns.Add(SpeciesObservationFieldMappingData.METHOD, typeof(String));
                table.Columns.Add(SpeciesObservationFieldMappingData.DEFAULT_VALUE, typeof(String));
                table.Columns.Add(SpeciesObservationFieldMappingData.DOCUMENTATION, typeof(String));
                table.Columns.Add(SpeciesObservationFieldMappingData.IS_IMPLEMENTED, typeof(Boolean));
                table.Columns.Add(SpeciesObservationFieldMappingData.IS_PLANNED, typeof(Boolean));
                table.Columns.Add(SpeciesObservationFieldMappingData.GUID, typeof(String));
                table.Columns.Add(SpeciesObservationFieldMappingData.PROPERTY_IDENTIFIER, typeof(String));
                table.Columns.Add(SpeciesObservationFieldMappingData.PROJECT_NAME, typeof(String));
                table.Columns.Add(SpeciesObservationFieldMappingData.PROJECT_ID, typeof(Int32));

                return table;
            }
        }

        /// <summary>
        /// Get DataTable with same definition as table
        /// SpeciesObservationField in database.
        /// </summary>
        /// <param name="tableType">
        /// The table Type or default.
        /// </param>
        /// <returns>
        /// DataTable with same definition as table
        /// SpeciesObservationField in database.
        /// </returns>
        internal static DataTable GetSpeciesObservationFieldTable(string tableType = DEFAULT)
        {
            string tableName = SpeciesObservationFieldData.TABLE_NAME;
            switch (tableType)
            {
                case CREATE:
                    tableName = SpeciesObservationFieldData.CREATE_TABLE_NAME;
                    break;
                case UPDATE:
                    tableName = SpeciesObservationFieldData.UPDATE_TABLE_NAME;
                    break;
            }

            using (DataTable speciesObservationFieldTable = new DataTable(tableName))
            {
                speciesObservationFieldTable.Columns.Add(SpeciesObservationFieldData.SPECIES_OBSERVATION_ID, typeof(Int64));
                speciesObservationFieldTable.Columns.Add(SpeciesObservationFieldData.CLASS, typeof(String));
                speciesObservationFieldTable.Columns.Add(SpeciesObservationFieldData.CLASS_INDEX, typeof(Int64));
                speciesObservationFieldTable.Columns.Add(SpeciesObservationFieldData.LOCALE_ID, typeof(Int32));
                speciesObservationFieldTable.Columns.Add(SpeciesObservationFieldData.INFORMATION, typeof(String));
                speciesObservationFieldTable.Columns.Add(SpeciesObservationFieldData.PROPERTY, typeof(String));
                speciesObservationFieldTable.Columns.Add(SpeciesObservationFieldData.PROPERTY_INDEX, typeof(Int64));
                speciesObservationFieldTable.Columns.Add(SpeciesObservationFieldData.TYPE_ID, typeof(Int32));
                speciesObservationFieldTable.Columns.Add(SpeciesObservationFieldData.UNIT, typeof(String));
                speciesObservationFieldTable.Columns.Add(SpeciesObservationFieldData.VALUE, typeof(String));
                speciesObservationFieldTable.Columns.Add(SpeciesObservationFieldData.CATALOGNUMBER, typeof(String));
                speciesObservationFieldTable.Columns.Add(SpeciesObservationFieldData.DATAPROVIDERID, typeof(String));

                speciesObservationFieldTable.Columns.Add(SpeciesObservationFieldData.VALUE_DOUBLE, typeof(Double));
                speciesObservationFieldTable.Columns.Add(SpeciesObservationFieldData.VALUE_BIGINT, typeof(Int64));
                speciesObservationFieldTable.Columns.Add(SpeciesObservationFieldData.VALUE_BOOLEAN, typeof(Boolean));
                speciesObservationFieldTable.Columns.Add(SpeciesObservationFieldData.VALUE_DATETIME, typeof(DateTime));
                speciesObservationFieldTable.Columns.Add(SpeciesObservationFieldData.VALUE_STRING, typeof(String));

                return speciesObservationFieldTable;
            }
        }

        /// <summary>
        /// The get species observation error field table.
        /// </summary>
        /// <returns>
        /// The speciesObservationErrorFieldTable.<see cref="DataTable"/>.
        /// </returns>
        internal static DataTable GetSpeciesObservationErrorFieldTable()
        {
            using (DataTable speciesObservationErrorFieldTable = new DataTable(SpeciesObservationErrorFieldData.TABLE_NAME))
            {
                speciesObservationErrorFieldTable.Columns.Add(SpeciesObservationFieldData.SPECIES_OBSERVATION_ID, typeof(String));
                speciesObservationErrorFieldTable.Columns.Add(SpeciesObservationFieldData.CLASS, typeof(String));
                speciesObservationErrorFieldTable.Columns.Add(SpeciesObservationFieldData.CLASS_INDEX, typeof(String));
                speciesObservationErrorFieldTable.Columns.Add(SpeciesObservationFieldData.LOCALE_ID, typeof(String));
                speciesObservationErrorFieldTable.Columns.Add(SpeciesObservationFieldData.INFORMATION, typeof(String));
                speciesObservationErrorFieldTable.Columns.Add(SpeciesObservationFieldData.PROPERTY, typeof(String));
                speciesObservationErrorFieldTable.Columns.Add(SpeciesObservationFieldData.PROPERTY_INDEX, typeof(String));
                speciesObservationErrorFieldTable.Columns.Add(SpeciesObservationFieldData.TYPE_ID, typeof(String));
                speciesObservationErrorFieldTable.Columns.Add(SpeciesObservationFieldData.UNIT, typeof(String));
                speciesObservationErrorFieldTable.Columns.Add(SpeciesObservationFieldData.VALUE, typeof(String));
                speciesObservationErrorFieldTable.Columns.Add(SpeciesObservationErrorFieldData.DATAPROVIDERID, typeof(Int32));
                speciesObservationErrorFieldTable.Columns.Add(SpeciesObservationErrorFieldData.DATAPROVIDER, typeof(String));
                speciesObservationErrorFieldTable.Columns.Add(SpeciesObservationErrorFieldData.DESCRIPTION, typeof(String));
                speciesObservationErrorFieldTable.Columns.Add(SpeciesObservationErrorFieldData.TRANSACTIONTYPE, typeof(String));

                return speciesObservationErrorFieldTable;
            }
        }

        /// <summary>
        /// Fills a DataRow from a DataReader, NOTE: not all data types are implemented
        /// </summary>
        /// <param name="row">The DataRow</param>
        /// <param name="reader">The DataReader</param>
        private static void FillTableRow(DataRow row, DataReader reader)
        {
            foreach (DataColumn column in row.Table.Columns)
            {
                if (reader.HasColumn(column.ColumnName))
                {
                    if (column.DataType == typeof(bool))
                    {
                        SetBooleanValue(row, reader, column.ColumnName);
                    }

                    if (column.DataType == typeof(DateTime))
                    {
                        SetDatetimeValue(row, reader, column.ColumnName);
                    }

                    if (column.DataType == typeof(Int32))
                    {
                        SetInt32Value(row, reader, column.ColumnName);
                    }

                    if (column.DataType == typeof(String))
                    {
                        SetStringValue(row, reader, column.ColumnName);
                    }
                }
            }
        }

        private static void SetBooleanValue(DataRow row, DataReader reader, string columnName)
        {
            if (!reader.IsDbNull(columnName))
            {
                row[columnName] = reader.GetBoolean(columnName);
            }
        }

        private static void SetDatetimeValue(DataRow row, DataReader reader, string columnName)
        {
            if (!reader.IsDbNull(columnName))
            {
                row[columnName] = reader.GetDateTime(columnName);
            }
        }

        private static void SetInt32Value(DataRow row, DataReader reader, string columnName)
        {
            if (!reader.IsDbNull(columnName))
            {
                row[columnName] = reader.GetInt32(columnName);
            }
        }

        private static void SetStringValue(DataRow row, DataReader reader, string columnName)
        {
            if (!reader.IsDbNull(columnName))
            {
                row[columnName] = reader.GetString(columnName);
            }
        }

        /// <summary>
        /// The get deleted observation table.
        /// </summary>
        /// <returns>
        /// The <see cref="DataTable"/>.
        /// </returns>
        internal static DataTable GetDeletedObservationTable()
        {
            using (DataTable deletedObservationTable = new DataTable(DeletedObservationTableData.DELETE_TABLE_NAME))
            {
                deletedObservationTable.Columns.Add(DeletedObservationTableData.OBSERVATIONID, typeof(String));
                deletedObservationTable.Columns.Add(DeletedObservationTableData.CATALOGNUMBER, typeof(String));
                deletedObservationTable.Columns.Add(DeletedObservationTableData.DATAPROVIDERID, typeof(String));

                return deletedObservationTable;
            }
        }
    }
}
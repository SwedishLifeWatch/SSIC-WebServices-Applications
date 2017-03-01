using System;
using System.Diagnostics.CodeAnalysis;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Data
{
    /// <summary>
    /// Enum that holds column positon in for 
    /// protected species observation indication
    /// </summary>
    public enum ProtectedSpeciesObservationIndicationColumn
    {
#pragma warning disable 1591
        Id = 0,
        CoordinateX = 1,
        CoordinateY = 2,
        County = 3,
        DataProviderId = 4,
        DecimalLatitude = 5,
        DecimalLongitude = 6,
        DyntaxaTaxonId = 7,
        Locality = 8,
        Municipality = 9,
        OccurrenceId = 10,
        ProtectionLevel = 11,
        StateProvince = 12
#pragma warning restore 1591
    }

    /// <summary>
    /// Contains extension to the WebDarwinCore class.
    /// </summary>
    public static class WebDarwinCoreExtension
    {
        /// <summary>
        /// Load information about an observation into the observation.
        /// </summary>
        /// <param name="webDarwinCore">The observation.</param>
        /// <param name="dataReader">An open data reader.</param>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static void Load(this WebDarwinCore webDarwinCore, 
                                DataReader dataReader)
        {
            // Load species observation information.
            webDarwinCore.AccessRights = dataReader.GetString((Int32)(DarwinCoreColumn.AccessRights));
            webDarwinCore.BasisOfRecord = dataReader.GetString((Int32)(DarwinCoreColumn.BasisOfRecord));
            // webDarwinCore.BibliographicCitation = dataReader.GetString((Int32)(DarwinCoreColumn.BibliographicCitation)); // "Artportalen 2012, Swedish University of Agricultural Sciences, Swedish Species Information Centre [Online] (Visited 21 August 2012). Url http://www.artportalen.se";
            webDarwinCore.CollectionCode = dataReader.GetString((Int32)(DarwinCoreColumn.CollectionCode));
            webDarwinCore.CollectionID = dataReader.GetString((Int32)(DarwinCoreColumn.CollectionId));
            webDarwinCore.DataGeneralizations = dataReader.GetString((Int32)(DarwinCoreColumn.DataGeneralizations));
            webDarwinCore.DatasetID = dataReader.GetInt32((Int32)(DarwinCoreColumn.DataProviderId)).WebToString();
            webDarwinCore.DynamicProperties = dataReader.GetString((Int32)(DarwinCoreColumn.DynamicProperties));
            webDarwinCore.Id = dataReader.GetInt64((Int32)(DarwinCoreColumn.Id));
            webDarwinCore.InformationWithheld = dataReader.GetString((Int32)(DarwinCoreColumn.InformationWithheld));
            webDarwinCore.InstitutionCode = dataReader.GetString((Int32)(DarwinCoreColumn.InstitutionCode));
            webDarwinCore.InstitutionID = dataReader.GetString((Int32)(DarwinCoreColumn.InstitutionId));
            webDarwinCore.Language = dataReader.GetString((Int32)(DarwinCoreColumn.Language));
            webDarwinCore.Modified = dataReader.GetDateTime((Int32)(DarwinCoreColumn.Modified));
            webDarwinCore.References = dataReader.GetString((Int32)(DarwinCoreColumn.References));
            webDarwinCore.ReportedBy = dataReader.GetString((Int32)(DarwinCoreColumn.ReportedBy));
            webDarwinCore.ReportedDate = dataReader.GetDateTime((Int32)(DarwinCoreColumn.ReportedDate));
            webDarwinCore.Rights = dataReader.GetString((Int32)(DarwinCoreColumn.Rights));
            webDarwinCore.RightsHolder = dataReader.GetString((Int32)(DarwinCoreColumn.RightsHolder));
            webDarwinCore.SpeciesObservationURL = dataReader.GetString((Int32)(DarwinCoreColumn.SpeciesObservationUrl));
            webDarwinCore.Type = dataReader.GetString((Int32)(DarwinCoreColumn.Type));
            webDarwinCore.ValidationStatus = dataReader.GetString((Int32)(DarwinCoreColumn.ValidationStatus));

            // Load conservation information.
            webDarwinCore.Conservation = new WebDarwinCoreConservation();
            webDarwinCore.Conservation.ProtectionLevel = dataReader.GetInt32((Int32)(DarwinCoreColumn.ProtectionLevel));

            // Load event information.
            webDarwinCore.Event = new WebDarwinCoreEvent();
            webDarwinCore.Event.Day = dataReader.GetInt16((Int32)(DarwinCoreColumn.Day), 0);
            webDarwinCore.Event.End = dataReader.GetDateTime((Int32)(DarwinCoreColumn.End));
            webDarwinCore.Event.EndDayOfYear = dataReader.GetInt16((Int32)(DarwinCoreColumn.EndDayOfYear), 0);
            webDarwinCore.Event.EventDate = dataReader.GetString((Int32)(DarwinCoreColumn.EventDate));
            webDarwinCore.Event.EventID = dataReader.GetString((Int32)(DarwinCoreColumn.EventId));
            webDarwinCore.Event.EventRemarks = dataReader.GetString((Int32)(DarwinCoreColumn.EventRemarks));
            webDarwinCore.Event.EventTime = dataReader.GetString((Int32)(DarwinCoreColumn.EventTime));
            webDarwinCore.Event.FieldNotes = dataReader.GetString((Int32)(DarwinCoreColumn.FieldNotes));
            webDarwinCore.Event.FieldNumber = dataReader.GetString((Int32)(DarwinCoreColumn.FieldNumber));
            webDarwinCore.Event.Habitat = dataReader.GetString((Int32)(DarwinCoreColumn.Habitat));
            webDarwinCore.Event.Month = dataReader.GetInt16((Int32)(DarwinCoreColumn.Month), 0);
            webDarwinCore.Event.SamplingEffort = dataReader.GetString((Int32)(DarwinCoreColumn.SamplingEffort));
            webDarwinCore.Event.SamplingProtocol = dataReader.GetString((Int32)(DarwinCoreColumn.SamplingProtocol));
            webDarwinCore.Event.Start = dataReader.GetDateTime((Int32)(DarwinCoreColumn.Start));
            webDarwinCore.Event.StartDayOfYear = dataReader.GetInt16((Int32)(DarwinCoreColumn.StartDayOfYear), 0);
            webDarwinCore.Event.VerbatimEventDate = dataReader.GetString((Int32)(DarwinCoreColumn.VerbatimEventDate));
            webDarwinCore.Event.Year = dataReader.GetInt16((Int32)(DarwinCoreColumn.Year), 0);

            // Load identification information.
            webDarwinCore.Identification = new WebDarwinCoreIdentification();
            webDarwinCore.Identification.DateIdentified = dataReader.GetString((Int32)(DarwinCoreColumn.DateIdentified));
            webDarwinCore.Identification.IdentificationID = dataReader.GetString((Int32)(DarwinCoreColumn.IdentificationId));
            webDarwinCore.Identification.IdentificationQualifier = dataReader.GetString((Int32)(DarwinCoreColumn.IdentificationQualifier));
            webDarwinCore.Identification.IdentificationReferences = dataReader.GetString((Int32)(DarwinCoreColumn.IdentificationReferences));
            webDarwinCore.Identification.IdentificationRemarks = dataReader.GetString((Int32)(DarwinCoreColumn.IdentificationRemarks));
            webDarwinCore.Identification.IdentificationVerificationStatus = dataReader.GetString((Int32)(DarwinCoreColumn.IdentificationVerificationStatus));
            webDarwinCore.Identification.IdentifiedBy = dataReader.GetString((Int32)(DarwinCoreColumn.IdentifiedBy));
            webDarwinCore.Identification.TypeStatus = dataReader.GetString((Int32)(DarwinCoreColumn.TypeStatus));
            webDarwinCore.Identification.UncertainDetermination = dataReader.GetByte((Int32)(DarwinCoreColumn.UncertainDetermination), 0) == 1;

            // Load location information.
            webDarwinCore.Location = new WebDarwinCoreLocation();
            webDarwinCore.Location.Continent = "Europe"; // dataReader.GetString((Int32)(DarwinCoreColumn.Continent));
            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.CoordinateM)))
            {
                webDarwinCore.Location.CoordinateM = dataReader.GetDouble((Int32)(DarwinCoreColumn.CoordinateM)).WebToString();
            }
            webDarwinCore.Location.CoordinatePrecision = dataReader.GetString((Int32)(DarwinCoreColumn.CoordinatePrecision));
            webDarwinCore.Location.CoordinateUncertaintyInMeters = dataReader.GetInt32((Int32)(DarwinCoreColumn.CoordinateUncertaintyInMeters)).WebToString();
            webDarwinCore.Location.CoordinateX = dataReader.GetInt32((Int32)(DarwinCoreColumn.CoordinateX));
            webDarwinCore.Location.CoordinateY = dataReader.GetInt32((Int32)(DarwinCoreColumn.CoordinateY));
            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.CoordinateZ)))
            {
                webDarwinCore.Location.CoordinateZ = dataReader.GetDouble((Int32)(DarwinCoreColumn.CoordinateZ)).WebToString();
            }

            if (dataReader.GetString((Int32)(DarwinCoreColumn.CountryCode)) == "SE")
            {
                webDarwinCore.Location.Country = "Sweden"; // GetData(dataReader, DarwinCoreColumn.Country);
            }
            else
            {
                webDarwinCore.Location.Country = "Not known: " + dataReader.GetString((Int32)(DarwinCoreColumn.CountryCode));
            }

            webDarwinCore.Location.CountryCode = dataReader.GetString((Int32)(DarwinCoreColumn.CountryCode));
            webDarwinCore.Location.County = dataReader.GetString((Int32)(DarwinCoreColumn.County));
            webDarwinCore.Location.DecimalLatitude = dataReader.GetDouble((Int32)(DarwinCoreColumn.DecimalLatitude), 0);
            webDarwinCore.Location.DecimalLongitude = dataReader.GetDouble((Int32)(DarwinCoreColumn.DecimalLongitude), 0);
            webDarwinCore.Location.FootprintSRS = dataReader.GetString((Int32)(DarwinCoreColumn.FootprintSrs));
            webDarwinCore.Location.FootprintSpatialFit = dataReader.GetString((Int32)(DarwinCoreColumn.FootprintSpatialFit));
            webDarwinCore.Location.FootprintWKT = dataReader.GetString((Int32)(DarwinCoreColumn.FootprintWkt));
            webDarwinCore.Location.GeodeticDatum = dataReader.GetString((Int32)(DarwinCoreColumn.GeodeticDatum));
            webDarwinCore.Location.GeoreferenceProtocol = dataReader.GetString((Int32)(DarwinCoreColumn.GeoreferenceProtocol));
            webDarwinCore.Location.GeoreferenceRemarks = dataReader.GetString((Int32)(DarwinCoreColumn.GeoreferenceRemarks));
            webDarwinCore.Location.GeoreferenceSources = dataReader.GetString((Int32)(DarwinCoreColumn.GeoreferenceSources));
            webDarwinCore.Location.GeoreferenceVerificationStatus = dataReader.GetString((Int32)(DarwinCoreColumn.GeoreferenceVerificationStatus));
            webDarwinCore.Location.GeoreferencedBy = dataReader.GetString((Int32)(DarwinCoreColumn.GeoreferencedBy));
            webDarwinCore.Location.GeoreferencedDate = dataReader.GetString((Int32)(DarwinCoreColumn.GeoreferencedDate));
            webDarwinCore.Location.HigherGeography = dataReader.GetString((Int32)(DarwinCoreColumn.HigherGeography));
            webDarwinCore.Location.HigherGeographyID = dataReader.GetString((Int32)(DarwinCoreColumn.HigherGeographyId));
            webDarwinCore.Location.Island = dataReader.GetString((Int32)(DarwinCoreColumn.Island));
            webDarwinCore.Location.IslandGroup = dataReader.GetString((Int32)(DarwinCoreColumn.IslandGroup));
            webDarwinCore.Location.Locality = dataReader.GetString((Int32)(DarwinCoreColumn.Locality));
            webDarwinCore.Location.LocationAccordingTo = dataReader.GetString((Int32)(DarwinCoreColumn.LocationAccordingTo));
            webDarwinCore.Location.LocationId = dataReader.GetString((Int32)(DarwinCoreColumn.LocationId));
            webDarwinCore.Location.LocationRemarks = dataReader.GetString((Int32)(DarwinCoreColumn.LocationRemarks));
            webDarwinCore.Location.LocationURL = dataReader.GetString((Int32)(DarwinCoreColumn.LocationUrl));
            webDarwinCore.Location.MaximumDepthInMeters = dataReader.GetString((Int32)(DarwinCoreColumn.MaximumDepthInMeters));
            webDarwinCore.Location.MaximumDistanceAboveSurfaceInMeters = dataReader.GetString((Int32)(DarwinCoreColumn.MaximumDistanceAboveSurfaceInMeters));
            webDarwinCore.Location.MaximumElevationInMeters = dataReader.GetString((Int32)(DarwinCoreColumn.MaximumElevationInMeters));
            webDarwinCore.Location.MinimumDepthInMeters = dataReader.GetString((Int32)(DarwinCoreColumn.MinimumDepthInMeters));
            webDarwinCore.Location.MinimumDistanceAboveSurfaceInMeters = dataReader.GetString((Int32)(DarwinCoreColumn.MinimumDistanceAboveSurfaceInMeters));
            webDarwinCore.Location.MinimumElevationInMeters = dataReader.GetString((Int32)(DarwinCoreColumn.MinimumElevationInMeters));
            webDarwinCore.Location.Municipality = dataReader.GetString((Int32)(DarwinCoreColumn.Municipality));
            webDarwinCore.Location.Parish = dataReader.GetString((Int32)(DarwinCoreColumn.Parish));
            webDarwinCore.Location.PointRadiusSpatialFit = dataReader.GetString((Int32)(DarwinCoreColumn.PointRadiusSpatialFit));
            webDarwinCore.Location.StateProvince = dataReader.GetString((Int32)(DarwinCoreColumn.StateProvince));
            webDarwinCore.Location.VerbatimCoordinateSystem = dataReader.GetString((Int32)(DarwinCoreColumn.VerbatimCoordinateSystem));
            webDarwinCore.Location.VerbatimCoordinates = dataReader.GetString((Int32)(DarwinCoreColumn.VerbatimCoordinates));
            webDarwinCore.Location.VerbatimDepth = dataReader.GetString((Int32)(DarwinCoreColumn.VerbatimDepth));
            webDarwinCore.Location.VerbatimElevation = dataReader.GetString((Int32)(DarwinCoreColumn.VerbatimElevation));
            webDarwinCore.Location.VerbatimLatitude = dataReader.GetString((Int32)(DarwinCoreColumn.VerbatimLatitude));
            webDarwinCore.Location.VerbatimLocality = dataReader.GetString((Int32)(DarwinCoreColumn.VerbatimLocality));
            webDarwinCore.Location.VerbatimLongitude = dataReader.GetString((Int32)(DarwinCoreColumn.VerbatimLongitude));
            webDarwinCore.Location.VerbatimSRS = dataReader.GetString((Int32)(DarwinCoreColumn.VerbatimSrs));
            webDarwinCore.Location.WaterBody = dataReader.GetString((Int32)(DarwinCoreColumn.WaterBody));

            // Load occurrence information.
            webDarwinCore.Occurrence = new WebDarwinCoreOccurrence();
            webDarwinCore.Occurrence.AssociatedMedia = dataReader.GetString((Int32)(DarwinCoreColumn.AssociatedMedia));
            webDarwinCore.Occurrence.AssociatedOccurrences = dataReader.GetString((Int32)(DarwinCoreColumn.AssociatedOccurrences));
            webDarwinCore.Occurrence.AssociatedReferences = dataReader.GetString((Int32)(DarwinCoreColumn.AssociatedReferences));
            webDarwinCore.Occurrence.AssociatedSequences = dataReader.GetString((Int32)(DarwinCoreColumn.AssociatedSequences));
            webDarwinCore.Occurrence.AssociatedTaxa = dataReader.GetString((Int32)(DarwinCoreColumn.AssociatedTaxa));
            webDarwinCore.Occurrence.Behavior = dataReader.GetString((Int32)(DarwinCoreColumn.Behavior));
            webDarwinCore.Occurrence.CatalogNumber = dataReader.GetString((Int32)(DarwinCoreColumn.CatalogNumber));
            webDarwinCore.Occurrence.Disposition = dataReader.GetString((Int32)(DarwinCoreColumn.Disposition));
            webDarwinCore.Occurrence.EstablishmentMeans = dataReader.GetString((Int32)(DarwinCoreColumn.EstablishmentMeans));
            webDarwinCore.Occurrence.IndividualCount = dataReader.GetString((Int32)(DarwinCoreColumn.IndividualCount));
            webDarwinCore.Occurrence.IndividualID = dataReader.GetString((Int32)(DarwinCoreColumn.IndividualId));
            webDarwinCore.Occurrence.IsNaturalOccurrence = dataReader.GetByte((Int32)(DarwinCoreColumn.IsNaturalOccurrence)) == 1;
            webDarwinCore.Occurrence.IsNeverFoundObservation = dataReader.GetByte((Int32)(DarwinCoreColumn.IsNeverFoundObservation), 0) == 1;
            webDarwinCore.Occurrence.IsNotRediscoveredObservation = dataReader.GetByte((Int32)(DarwinCoreColumn.IsNotRediscoveredObservation), 0) == 1;
            webDarwinCore.Occurrence.IsPositiveObservation = dataReader.GetByte((Int32)(DarwinCoreColumn.IsPositiveObservation)) == 1;
            webDarwinCore.Occurrence.LifeStage = dataReader.GetString((Int32)(DarwinCoreColumn.LifeStage));
            webDarwinCore.Occurrence.OccurrenceID = dataReader.GetString((Int32)(DarwinCoreColumn.OccurrenceId));
            webDarwinCore.Occurrence.OccurrenceRemarks = dataReader.GetString((Int32)(DarwinCoreColumn.OccurrenceRemarks));
            webDarwinCore.Occurrence.OccurrenceStatus = dataReader.GetString((Int32)(DarwinCoreColumn.OccurrenceStatus));
            webDarwinCore.Occurrence.OccurrenceURL = dataReader.GetString((Int32)(DarwinCoreColumn.OccurrenceUrl));
            webDarwinCore.Occurrence.OtherCatalogNumbers = dataReader.GetString((Int32)(DarwinCoreColumn.OtherCatalogNumbers));
            webDarwinCore.Occurrence.Preparations = dataReader.GetString((Int32)(DarwinCoreColumn.Preparations));
            webDarwinCore.Occurrence.PreviousIdentifications = dataReader.GetString((Int32)(DarwinCoreColumn.PreviousIdentifications));
            webDarwinCore.Occurrence.Quantity = dataReader.GetString((Int32)(DarwinCoreColumn.Quantity));
            webDarwinCore.Occurrence.QuantityUnit = dataReader.GetString((Int32)(DarwinCoreColumn.QuantityUnit));
            webDarwinCore.Occurrence.RecordNumber = dataReader.GetString((Int32)(DarwinCoreColumn.RecordNumber));
            webDarwinCore.Occurrence.RecordedBy = dataReader.GetString((Int32)(DarwinCoreColumn.RecordedBy));
            webDarwinCore.Occurrence.ReproductiveCondition = dataReader.GetString((Int32)(DarwinCoreColumn.ReproductiveCondition));
            webDarwinCore.Occurrence.Sex = dataReader.GetString((Int32)(DarwinCoreColumn.Sex));
            webDarwinCore.Occurrence.Substrate = dataReader.GetString((Int32)(DarwinCoreColumn.Substrate));
            webDarwinCore.Owner = dataReader.GetString((Int32)(DarwinCoreColumn.Owner));
            webDarwinCore.OwnerInstitutionCode = dataReader.GetString((Int32)(DarwinCoreColumn.OwnerInstitutionCode));

            // Load project information.
            webDarwinCore.Project = new WebDarwinCoreProject();
            webDarwinCore.Project.IsPublic = dataReader.GetByte((Int32)(DarwinCoreColumn.ProjectIsPublic), 0) == 1;
            webDarwinCore.Project.ProjectCategory = dataReader.GetString((Int32)(DarwinCoreColumn.ProjectCategory));
            webDarwinCore.Project.ProjectDescription = dataReader.GetString((Int32)(DarwinCoreColumn.ProjectDescription));
            webDarwinCore.Project.ProjectEndDate = dataReader.GetString((Int32)(DarwinCoreColumn.ProjectEndDate));
            webDarwinCore.Project.ProjectID = dataReader.GetString((Int32)(DarwinCoreColumn.ProjectId));
            webDarwinCore.Project.ProjectName = dataReader.GetString((Int32)(DarwinCoreColumn.ProjectName));
            webDarwinCore.Project.ProjectOwner = dataReader.GetString((Int32)(DarwinCoreColumn.ProjectOwner));
            webDarwinCore.Project.ProjectStartDate = dataReader.GetString((Int32)(DarwinCoreColumn.ProjectStartDate));
            webDarwinCore.Project.ProjectURL = dataReader.GetString((Int32)(DarwinCoreColumn.ProjectUrl));
            webDarwinCore.Project.SurveyMethod = dataReader.GetString((Int32)(DarwinCoreColumn.ProjectSurveyMethod));

            // Load taxon information.
            webDarwinCore.Taxon = new WebDarwinCoreTaxon();
            webDarwinCore.Taxon.DyntaxaTaxonID = dataReader.GetInt32((Int32)(DarwinCoreColumn.DyntaxaTaxonId));
            webDarwinCore.Taxon.TaxonURL = @"https://www.dyntaxa.se/Taxon/Info/" + webDarwinCore.Taxon.DyntaxaTaxonID;
            webDarwinCore.Taxon.VerbatimScientificName = dataReader.GetString((Int32)(DarwinCoreColumn.VerbatimScientificName));
            webDarwinCore.Taxon.VerbatimTaxonRank = dataReader.GetString((Int32)(DarwinCoreColumn.VerbatimTaxonRank));
        }

        /// <summary>
        /// Load information about an observation into the observation.
        /// This method is optimized for protected species observation
        /// indication.
        /// </summary>
        /// <param name="webDarwinCore">The observation.</param>
        /// <param name="dataReader">DataReader contains speciesObservationsAccessRights data.</param>
        public static void LoadProtectedSpeciesObservationIndication(this WebDarwinCore webDarwinCore,
                                                                     DataReader dataReader)
        {
            webDarwinCore.Id = dataReader.GetInt64((Int32)(ProtectedSpeciesObservationIndicationColumn.Id));
            webDarwinCore.Conservation = new WebDarwinCoreConservation();
            webDarwinCore.Conservation.ProtectionLevel = dataReader.GetInt32((Int32)(ProtectedSpeciesObservationIndicationColumn.ProtectionLevel));
            webDarwinCore.DatasetID = dataReader.GetInt32((Int32)(ProtectedSpeciesObservationIndicationColumn.DataProviderId)).WebToString();
            webDarwinCore.Location = new WebDarwinCoreLocation();
            webDarwinCore.Location.CoordinateX = dataReader.GetInt32((Int32)(ProtectedSpeciesObservationIndicationColumn.CoordinateX));
            webDarwinCore.Location.CoordinateY = dataReader.GetInt32((Int32)(ProtectedSpeciesObservationIndicationColumn.CoordinateY));
            webDarwinCore.Location.County = dataReader.GetString((Int32)(ProtectedSpeciesObservationIndicationColumn.County));
            webDarwinCore.Location.DecimalLatitude = dataReader.GetDouble((Int32)(ProtectedSpeciesObservationIndicationColumn.DecimalLatitude), 0);
            webDarwinCore.Location.DecimalLongitude = dataReader.GetDouble((Int32)(ProtectedSpeciesObservationIndicationColumn.DecimalLongitude), 0);
            webDarwinCore.Location.Locality = dataReader.GetString((Int32)(ProtectedSpeciesObservationIndicationColumn.Locality));
            webDarwinCore.Location.Municipality = dataReader.GetString((Int32)(ProtectedSpeciesObservationIndicationColumn.Municipality));
            webDarwinCore.Location.StateProvince = dataReader.GetString((Int32)(ProtectedSpeciesObservationIndicationColumn.StateProvince));
            webDarwinCore.Occurrence = new WebDarwinCoreOccurrence();
            webDarwinCore.Occurrence.OccurrenceID = dataReader.GetString((Int32)(ProtectedSpeciesObservationIndicationColumn.OccurrenceId));
            webDarwinCore.Taxon = new WebDarwinCoreTaxon();
            webDarwinCore.Taxon.DyntaxaTaxonID = dataReader.GetInt32((Int32)(ProtectedSpeciesObservationIndicationColumn.DyntaxaTaxonId));
        }
    }
}
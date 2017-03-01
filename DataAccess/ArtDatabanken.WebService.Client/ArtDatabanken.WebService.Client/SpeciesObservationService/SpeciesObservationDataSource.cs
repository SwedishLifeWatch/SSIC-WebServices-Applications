using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Client.UserService;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Client.SpeciesObservationService
{
    /// <summary>
    /// This class is used to retrieve species observation related information.
    /// </summary>
    public class SpeciesObservationDataSource : SpeciesObservationDataSourceBase, ISpeciesObservationDataSource
    {
        /// <summary>
        /// Get all bird nest activities.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>Information about bird nest activities.</returns>
        public virtual SpeciesActivityList GetBirdNestActivities(IUserContext userContext)
        {
            List<WebSpeciesActivity> webBirdNestActivities;

            webBirdNestActivities = WebServiceProxy.SwedishSpeciesObservationService.GetBirdNestActivities(GetClientInformation(userContext));
            return GetSpeciesActivities(userContext, webBirdNestActivities);
        }

        /// <summary>
        /// Get all county regions
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <returns>Information about county regions</returns>
        public virtual RegionList GetCountyRegions(IUserContext userContext)
        {
            List<WebRegion> webRegions;

            webRegions = WebServiceProxy.SwedishSpeciesObservationService.GetCountyRegions(GetClientInformation(userContext));
            return GetRegions(userContext, webRegions);
        }

        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// This method provides paging functionality of the result.
        /// Max page size is 10000 species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Species observations are returned in a format
        /// that is compatible with Darwin Core 1.5.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in geometry search criteria
        /// and returned species observations.
        /// </param>
        /// <param name="pageSpecification">
        /// Specification of paging information when
        /// species observations are retrieved.
        /// </param>
        /// <returns>Information about requested species observations.</returns>
        public virtual DarwinCoreList GetDarwinCore(IUserContext userContext,
                                                    ISpeciesObservationSearchCriteria searchCriteria,
                                                    ICoordinateSystem coordinateSystem,
                                                    ISpeciesObservationPageSpecification pageSpecification)
        {
            List<WebDarwinCore> webSpeciesObservations;
            WebSpeciesObservationSearchCriteria webSpeciesObservationSearchCriteria;
            WebSpeciesObservationPageSpecification webPageSpecification;
            WebCoordinateSystem webCoordinateSystem;

            // Check arguments.
            coordinateSystem.CheckNotNull("coordinateSystem");
            pageSpecification.CheckNotNull("pageSpecification");
            searchCriteria.CheckNotNull("searchCriteria");

            // Get species observations from web service.
            webCoordinateSystem = GetCoordinateSystem(coordinateSystem);
            webSpeciesObservationSearchCriteria = GetSpeciesObservationSearchCriteria(searchCriteria);
            webPageSpecification = GetSpeciesObservationPageSpecification(pageSpecification);
            webSpeciesObservations = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteriaPage(GetClientInformation(userContext),
                                                                                                                        webSpeciesObservationSearchCriteria,
                                                                                                                        webCoordinateSystem,
                                                                                                                        webPageSpecification);
            return GetDarwinCore(userContext, webSpeciesObservations);
        }

        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in geometry search criteria
        /// and returned species observations.
        /// </param>
        /// <param name="sortOrder">
        /// Defines how species observations should be sorted.
        /// This parameter is optional. Random order is used
        /// if no sort order has been specified.
        /// </param>
        /// <returns>Information about species observations.</returns>
        public virtual DarwinCoreList GetDarwinCore(IUserContext userContext,
                                                    ISpeciesObservationSearchCriteria searchCriteria,
                                                    ICoordinateSystem coordinateSystem,
                                                    SpeciesObservationFieldSortOrderList sortOrder)
        {
            List<WebSpeciesObservationFieldSortOrder> webSpeciesObservationFieldSortOrders;
            WebCoordinateSystem webCoordinateSystem;
            WebDarwinCoreInformation webDarwinCoreInformation;
            WebSpeciesObservationSearchCriteria webSpeciesObservationSearchCriteria;

            // Check arguments
            coordinateSystem.CheckNotNull("coordinateSystem");
            searchCriteria.CheckNotNull("searchCriteria");

            // Get species observations from web service.
            webCoordinateSystem = GetCoordinateSystem(coordinateSystem);
            webSpeciesObservationSearchCriteria = GetSpeciesObservationSearchCriteria(searchCriteria);
            webSpeciesObservationFieldSortOrders = GetSpeciesObservationSortOrder(sortOrder);
            webDarwinCoreInformation = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreBySearchCriteria(GetClientInformation(userContext),
                                                                                                                      webSpeciesObservationSearchCriteria,
                                                                                                                      webCoordinateSystem,
                                                                                                                      webSpeciesObservationFieldSortOrders);
            return GetDarwinCore(userContext,
                                 webDarwinCoreInformation,
                                 webCoordinateSystem);
        }

        /// <summary>
        /// Get requested species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="speciesObservationIds">Id for species observations to get.</param>
        /// <param name="coordinateSystem"> Coordinate system used in returned species observations.
        /// </param>
        /// <returns>Species observations.</returns>
        public virtual DarwinCoreList GetDarwinCore(IUserContext userContext,
                                                    List<Int64> speciesObservationIds,
                                                    ICoordinateSystem coordinateSystem)
        {
            WebCoordinateSystem webCoordinateSystem;
            WebDarwinCoreInformation webDarwinCoreInformation;

            // Check arguments
            coordinateSystem.CheckNotNull("coordinateSystem");
            speciesObservationIds.CheckNotNull("speciesObservationIds");

            // Get species observations from web service.
            webCoordinateSystem = GetCoordinateSystem(coordinateSystem);
            webDarwinCoreInformation = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreByIds(GetClientInformation(userContext),
                                                                                                           speciesObservationIds,
                                                                                                           webCoordinateSystem);
            return GetDarwinCore(userContext,
                                 webDarwinCoreInformation,
                                 webCoordinateSystem);
        }

        /// <summary>
        /// Convert a list of WebDarwinCore
        /// instances to a list of IDarwinCore instances.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webSpeciesObservations">List of WebDarwinCore instances.</param>
        /// <returns>A list of IDarwinCore instances.</returns>
        private DarwinCoreList GetDarwinCore(IUserContext userContext,
                                             List<WebDarwinCore> webSpeciesObservations)
        {
            DarwinCoreList speciesObservations;

            speciesObservations = null;
            if (webSpeciesObservations.IsNotEmpty())
            {
                speciesObservations = new DarwinCoreList();
                foreach (WebDarwinCore webSpeciesObservation in webSpeciesObservations)
                {
                    speciesObservations.Add(GetDarwinCore(userContext, webSpeciesObservation));
                }
            }

            return speciesObservations;
        }

        /// <summary>
        /// Convert a WebDarwinCoreInformation instance
        /// into a list of IDarwinCore instances.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webSpeciesObservationInformation">Web species observation information.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in returned species observations.
        /// </param>
        /// <returns>A list of IDarwinCore instances.</returns>
        private DarwinCoreList GetDarwinCore(IUserContext userContext,
                                             WebDarwinCoreInformation webSpeciesObservationInformation,
                                             WebCoordinateSystem coordinateSystem)
        {
            Int32 index, speciesObservationIdsIndex;
            List<Int64> speciesObservationIds;
            DarwinCoreList speciesObservations;
            WebDarwinCoreInformation webInformation;

            if (webSpeciesObservationInformation.SpeciesObservations.IsEmpty() &&
                webSpeciesObservationInformation.SpeciesObservationIds.IsNotEmpty())
            {
                // Get species observations in parts.
                speciesObservations = new DarwinCoreList();
                speciesObservationIds = new List<Int64>();
                index = 0;
                for (speciesObservationIdsIndex = 0; speciesObservationIdsIndex < webSpeciesObservationInformation.SpeciesObservationIds.Count; speciesObservationIdsIndex++)
                {
                    speciesObservationIds.Add(webSpeciesObservationInformation.SpeciesObservationIds[speciesObservationIdsIndex]);
                    if (++index == webSpeciesObservationInformation.MaxSpeciesObservationCount)
                    {
                        // Get one part of species observations.
                        webInformation = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreByIds(GetClientInformation(userContext),
                                                                                                             speciesObservationIds,
                                                                                                             coordinateSystem);
                        speciesObservations.AddRange(GetDarwinCore(userContext,
                                                                   webInformation.SpeciesObservations));
                        index = 0;
                        speciesObservationIds = new List<Int64>();
                    }
                }

                if (speciesObservationIds.IsNotEmpty())
                {
                    webInformation = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreByIds(GetClientInformation(userContext),
                                                                                                         speciesObservationIds,
                                                                                                         coordinateSystem);
                    speciesObservations.AddRange(GetDarwinCore(userContext,
                                                               webInformation.SpeciesObservations));
                }
            }
            else
            {
                speciesObservations = GetDarwinCore(userContext,
                                                    webSpeciesObservationInformation.SpeciesObservations);
            }

            return speciesObservations;
        }

        /// <summary>
        /// Convert a WebDarwinCore instance
        /// into an IDarwinCore instances.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webDarwinCore">A WebDarwinCore instance.</param>
        /// <returns>An IDarwinCore instances.</returns>
        private IDarwinCore GetDarwinCore(IUserContext userContext,
                                          WebDarwinCore webDarwinCore)
        {
            IDarwinCore darwinCore;

            darwinCore = new DarwinCore();
            darwinCore.AccessRights = webDarwinCore.AccessRights;
            darwinCore.BasisOfRecord = webDarwinCore.BasisOfRecord;
            darwinCore.BibliographicCitation = webDarwinCore.BibliographicCitation;
            darwinCore.CollectionCode = webDarwinCore.CollectionCode;
            darwinCore.CollectionID = webDarwinCore.CollectionID;
            darwinCore.DataContext = GetDataContext(userContext);
            darwinCore.DataGeneralizations = webDarwinCore.DataGeneralizations;
            darwinCore.DatasetID = webDarwinCore.DatasetID;
            darwinCore.DatasetName = webDarwinCore.DatasetName;
            darwinCore.DynamicProperties = webDarwinCore.DynamicProperties;
            darwinCore.Id = webDarwinCore.Id;
            darwinCore.InformationWithheld = webDarwinCore.InformationWithheld;
            darwinCore.InstitutionCode = webDarwinCore.InstitutionCode;
            darwinCore.InstitutionID = webDarwinCore.InstitutionID;
            darwinCore.Language = webDarwinCore.Language;
            darwinCore.Modified = webDarwinCore.Modified;
            darwinCore.Owner = webDarwinCore.Owner;
            darwinCore.OwnerInstitutionCode = webDarwinCore.OwnerInstitutionCode;
            darwinCore.References = webDarwinCore.References;
            darwinCore.ReportedBy = webDarwinCore.ReportedBy;
#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
            darwinCore.ReportedDate = webDarwinCore.ReportedDate;
#endif
            darwinCore.Rights = webDarwinCore.Rights;
            darwinCore.RightsHolder = webDarwinCore.RightsHolder;
            darwinCore.SpeciesObservationURL = webDarwinCore.SpeciesObservationURL;
            darwinCore.Type = webDarwinCore.Type;
            darwinCore.ValidationStatus = webDarwinCore.ValidationStatus;

            if (webDarwinCore.Taxon.IsNotNull())
            {
                darwinCore.Taxon = new DarwinCoreTaxon();
                darwinCore.Taxon.AcceptedNameUsage = webDarwinCore.Taxon.AcceptedNameUsage;
                darwinCore.Taxon.AcceptedNameUsageID = webDarwinCore.Taxon.AcceptedNameUsageID;
                darwinCore.Taxon.Class = webDarwinCore.Taxon.Class;
                darwinCore.Taxon.DyntaxaTaxonID = webDarwinCore.Taxon.DyntaxaTaxonID;
                darwinCore.Taxon.Family = webDarwinCore.Taxon.Family;
                darwinCore.Taxon.Genus = webDarwinCore.Taxon.Genus;
                darwinCore.Taxon.HigherClassification = webDarwinCore.Taxon.HigherClassification;
                darwinCore.Taxon.InfraspecificEpithet = webDarwinCore.Taxon.InfraspecificEpithet;
                darwinCore.Taxon.Kingdom = webDarwinCore.Taxon.Kingdom;
                darwinCore.Taxon.NameAccordingTo = webDarwinCore.Taxon.NameAccordingTo;
                darwinCore.Taxon.NameAccordingToID = webDarwinCore.Taxon.NameAccordingToID;
                darwinCore.Taxon.NamePublishedIn = webDarwinCore.Taxon.NamePublishedIn;
                darwinCore.Taxon.NamePublishedInID = webDarwinCore.Taxon.NamePublishedInID;
                darwinCore.Taxon.NamePublishedInYear = webDarwinCore.Taxon.NamePublishedInYear;
                darwinCore.Taxon.NomenclaturalCode = webDarwinCore.Taxon.NomenclaturalCode;
                darwinCore.Taxon.NomenclaturalStatus = webDarwinCore.Taxon.NomenclaturalStatus;
                darwinCore.Taxon.Order = webDarwinCore.Taxon.Order;
                darwinCore.Taxon.OrganismGroup = webDarwinCore.Taxon.OrganismGroup;
                darwinCore.Taxon.OriginalNameUsage = webDarwinCore.Taxon.OriginalNameUsage;
                darwinCore.Taxon.OriginalNameUsageID = webDarwinCore.Taxon.OriginalNameUsageID;
                darwinCore.Taxon.ParentNameUsage = webDarwinCore.Taxon.ParentNameUsage;
                darwinCore.Taxon.ParentNameUsageID = webDarwinCore.Taxon.ParentNameUsageID;
                darwinCore.Taxon.Phylum = webDarwinCore.Taxon.Phylum;
                darwinCore.Taxon.ScientificName = webDarwinCore.Taxon.ScientificName;
                darwinCore.Taxon.ScientificNameAuthorship = webDarwinCore.Taxon.ScientificNameAuthorship;
                darwinCore.Taxon.ScientificNameID = webDarwinCore.Taxon.ScientificNameID;
                darwinCore.Taxon.SpecificEpithet = webDarwinCore.Taxon.SpecificEpithet;
                darwinCore.Taxon.Subgenus = webDarwinCore.Taxon.Subgenus;
                darwinCore.Taxon.TaxonConceptID = webDarwinCore.Taxon.TaxonConceptID;
                darwinCore.Taxon.TaxonConceptStatus = webDarwinCore.Taxon.TaxonConceptStatus;
                darwinCore.Taxon.TaxonID = webDarwinCore.Taxon.TaxonID;
                darwinCore.Taxon.TaxonRank = webDarwinCore.Taxon.TaxonRank;
                darwinCore.Taxon.TaxonRemarks = webDarwinCore.Taxon.TaxonRemarks;
                //darwinCore.Taxon.TaxonSortOrder = webDarwinCore.Taxon.TaxonSortOrder;
                darwinCore.Taxon.TaxonURL = webDarwinCore.Taxon.TaxonURL;
                darwinCore.Taxon.TaxonomicStatus = webDarwinCore.Taxon.TaxonomicStatus;
                darwinCore.Taxon.VerbatimScientificName = webDarwinCore.Taxon.VerbatimScientificName;
                darwinCore.Taxon.VerbatimTaxonRank = webDarwinCore.Taxon.VerbatimTaxonRank;
                darwinCore.Taxon.VernacularName = webDarwinCore.Taxon.VernacularName;
            }

            if (webDarwinCore.ResourceRelationship.IsNotNull())
            {
                darwinCore.ResourceRelationship = new DarwinCoreResourceRelationship();
                darwinCore.ResourceRelationship.RelatedResourceID = webDarwinCore.ResourceRelationship.RelatedResourceID;
                darwinCore.ResourceRelationship.RelationshipAccordingTo =
                    webDarwinCore.ResourceRelationship.RelationshipAccordingTo;
                darwinCore.ResourceRelationship.RelationshipEstablishedDate =
                    webDarwinCore.ResourceRelationship.RelationshipEstablishedDate;
                darwinCore.ResourceRelationship.RelationshipOfResource =
                    webDarwinCore.ResourceRelationship.RelationshipOfResource;
                darwinCore.ResourceRelationship.RelationshipRemarks = webDarwinCore.ResourceRelationship.RelationshipRemarks;
                darwinCore.ResourceRelationship.ResourceID = webDarwinCore.ResourceRelationship.ResourceID;
                darwinCore.ResourceRelationship.ResourceRelationshipID =
                    webDarwinCore.ResourceRelationship.ResourceRelationshipID;
            }

            if (webDarwinCore.Project.IsNotNull())
            {
                darwinCore.Project = new DarwinCoreProject();
                darwinCore.Project.IsPublic = webDarwinCore.Project.IsPublic;
                darwinCore.Project.ProjectCategory = webDarwinCore.Project.ProjectCategory;
                darwinCore.Project.ProjectDescription = webDarwinCore.Project.ProjectDescription;
                darwinCore.Project.ProjectEndDate = webDarwinCore.Project.ProjectEndDate;
                darwinCore.Project.ProjectID = webDarwinCore.Project.ProjectID;
                darwinCore.Project.ProjectName = webDarwinCore.Project.ProjectName;
                darwinCore.Project.ProjectOwner = webDarwinCore.Project.ProjectOwner;
                darwinCore.Project.ProjectStartDate = webDarwinCore.Project.ProjectStartDate;
                darwinCore.Project.ProjectURL = webDarwinCore.Project.ProjectURL;
                darwinCore.Project.SurveyMethod = webDarwinCore.Project.SurveyMethod;
            }

            if (webDarwinCore.Occurrence.IsNotNull())
            {
                darwinCore.Occurrence = new DarwinCoreOccurrence();
                darwinCore.Occurrence.AssociatedMedia = webDarwinCore.Occurrence.AssociatedMedia;
                darwinCore.Occurrence.AssociatedOccurrences = webDarwinCore.Occurrence.AssociatedOccurrences;
                darwinCore.Occurrence.AssociatedReferences = webDarwinCore.Occurrence.AssociatedReferences;
                darwinCore.Occurrence.AssociatedSequences = webDarwinCore.Occurrence.AssociatedSequences;
                darwinCore.Occurrence.AssociatedTaxa = webDarwinCore.Occurrence.AssociatedTaxa;
                darwinCore.Occurrence.Behavior = webDarwinCore.Occurrence.Behavior;
                darwinCore.Occurrence.CatalogNumber = webDarwinCore.Occurrence.CatalogNumber;
                darwinCore.Occurrence.Disposition = webDarwinCore.Occurrence.Disposition;
                darwinCore.Occurrence.EstablishmentMeans = webDarwinCore.Occurrence.EstablishmentMeans;
                darwinCore.Occurrence.IndividualCount = webDarwinCore.Occurrence.IndividualCount;
                darwinCore.Occurrence.IndividualID = webDarwinCore.Occurrence.IndividualID;
                darwinCore.Occurrence.IsNaturalOccurrence = webDarwinCore.Occurrence.IsNaturalOccurrence;
                darwinCore.Occurrence.IsNeverFoundObservation = webDarwinCore.Occurrence.IsNeverFoundObservation;
                darwinCore.Occurrence.IsNotRediscoveredObservation = webDarwinCore.Occurrence.IsNotRediscoveredObservation;
                darwinCore.Occurrence.IsPositiveObservation = webDarwinCore.Occurrence.IsPositiveObservation;
                darwinCore.Occurrence.LifeStage = webDarwinCore.Occurrence.LifeStage;
                darwinCore.Occurrence.OccurrenceID = webDarwinCore.Occurrence.OccurrenceID;
                darwinCore.Occurrence.OccurrenceRemarks = webDarwinCore.Occurrence.OccurrenceRemarks;
                darwinCore.Occurrence.OccurrenceStatus = webDarwinCore.Occurrence.OccurrenceStatus;
                darwinCore.Occurrence.OccurrenceURL = webDarwinCore.Occurrence.OccurrenceURL;
                darwinCore.Occurrence.OtherCatalogNumbers = webDarwinCore.Occurrence.OtherCatalogNumbers;
                darwinCore.Occurrence.Preparations = webDarwinCore.Occurrence.Preparations;
                darwinCore.Occurrence.PreviousIdentifications = webDarwinCore.Occurrence.PreviousIdentifications;
                darwinCore.Occurrence.Quantity = webDarwinCore.Occurrence.Quantity;
                darwinCore.Occurrence.QuantityUnit = webDarwinCore.Occurrence.QuantityUnit;
                darwinCore.Occurrence.RecordNumber = webDarwinCore.Occurrence.RecordNumber;
                darwinCore.Occurrence.RecordedBy = webDarwinCore.Occurrence.RecordedBy;
                darwinCore.Occurrence.ReproductiveCondition = webDarwinCore.Occurrence.ReproductiveCondition;
                darwinCore.Occurrence.Sex = webDarwinCore.Occurrence.Sex;
                darwinCore.Occurrence.Substrate = webDarwinCore.Occurrence.Substrate;
            }

            if (webDarwinCore.MeasurementOrFact.IsNotNull())
            {
                darwinCore.MeasurementOrFact = new DarwinCoreMeasurementOrFact();
                darwinCore.MeasurementOrFact.MeasurementAccuracy = webDarwinCore.MeasurementOrFact.MeasurementAccuracy;
                darwinCore.MeasurementOrFact.MeasurementDeterminedBy = webDarwinCore.MeasurementOrFact.MeasurementDeterminedBy;
                darwinCore.MeasurementOrFact.MeasurementDeterminedDate =
                    webDarwinCore.MeasurementOrFact.MeasurementDeterminedDate;
                darwinCore.MeasurementOrFact.MeasurementID = webDarwinCore.MeasurementOrFact.MeasurementID;
                darwinCore.MeasurementOrFact.MeasurementMethod = webDarwinCore.MeasurementOrFact.MeasurementMethod;
                darwinCore.MeasurementOrFact.MeasurementRemarks = webDarwinCore.MeasurementOrFact.MeasurementRemarks;
                darwinCore.MeasurementOrFact.MeasurementType = webDarwinCore.MeasurementOrFact.MeasurementType;
                darwinCore.MeasurementOrFact.MeasurementUnit = webDarwinCore.MeasurementOrFact.MeasurementUnit;
                darwinCore.MeasurementOrFact.MeasurementValue = webDarwinCore.MeasurementOrFact.MeasurementValue;
            }

            if (webDarwinCore.Identification.IsNotNull())
            {
                darwinCore.Identification = new DarwinCoreIdentification();
                darwinCore.Identification.DateIdentified = webDarwinCore.Identification.DateIdentified;
                darwinCore.Identification.IdentificationID = webDarwinCore.Identification.IdentificationID;
                darwinCore.Identification.IdentificationQualifier = webDarwinCore.Identification.IdentificationQualifier;
                darwinCore.Identification.IdentificationReferences = webDarwinCore.Identification.IdentificationReferences;
                darwinCore.Identification.IdentificationRemarks = webDarwinCore.Identification.IdentificationRemarks;
                darwinCore.Identification.IdentificationVerificationStatus =
                    webDarwinCore.Identification.IdentificationVerificationStatus;
                darwinCore.Identification.IdentifiedBy = webDarwinCore.Identification.IdentifiedBy;
                darwinCore.Identification.TypeStatus = webDarwinCore.Identification.TypeStatus;
                darwinCore.Identification.UncertainDetermination = webDarwinCore.Identification.UncertainDetermination;
            }

            if (webDarwinCore.GeologicalContext.IsNotNull())
            {
                darwinCore.GeologicalContext = new DarwinCoreGeologicalContext();
                darwinCore.GeologicalContext.Bed = webDarwinCore.GeologicalContext.Bed;
                darwinCore.GeologicalContext.EarliestAgeOrLowestStage = webDarwinCore.GeologicalContext.EarliestAgeOrLowestStage;
                darwinCore.GeologicalContext.EarliestEonOrLowestEonothem =
                    webDarwinCore.GeologicalContext.EarliestEonOrLowestEonothem;
                darwinCore.GeologicalContext.EarliestEpochOrLowestSeries =
                    webDarwinCore.GeologicalContext.EarliestEpochOrLowestSeries;
                darwinCore.GeologicalContext.EarliestEraOrLowestErathem =
                    webDarwinCore.GeologicalContext.EarliestEraOrLowestErathem;
                darwinCore.GeologicalContext.EarliestPeriodOrLowestSystem =
                    webDarwinCore.GeologicalContext.EarliestPeriodOrLowestSystem;
                darwinCore.GeologicalContext.Formation = webDarwinCore.GeologicalContext.Formation;
                darwinCore.GeologicalContext.GeologicalContextID = webDarwinCore.GeologicalContext.GeologicalContextID;
                darwinCore.GeologicalContext.Group = webDarwinCore.GeologicalContext.Group;
                darwinCore.GeologicalContext.HighestBiostratigraphicZone =
                    webDarwinCore.GeologicalContext.HighestBiostratigraphicZone;
                darwinCore.GeologicalContext.LatestAgeOrHighestStage = webDarwinCore.GeologicalContext.LatestAgeOrHighestStage;
                darwinCore.GeologicalContext.LatestEonOrHighestEonothem =
                    webDarwinCore.GeologicalContext.LatestEonOrHighestEonothem;
                darwinCore.GeologicalContext.LatestEpochOrHighestSeries =
                    webDarwinCore.GeologicalContext.LatestEpochOrHighestSeries;
                darwinCore.GeologicalContext.LatestEraOrHighestErathem =
                    webDarwinCore.GeologicalContext.LatestEraOrHighestErathem;
                darwinCore.GeologicalContext.LatestPeriodOrHighestSystem =
                    webDarwinCore.GeologicalContext.LatestPeriodOrHighestSystem;
                darwinCore.GeologicalContext.LithostratigraphicTerms = webDarwinCore.GeologicalContext.LithostratigraphicTerms;
                darwinCore.GeologicalContext.LowestBiostratigraphicZone =
                    webDarwinCore.GeologicalContext.LowestBiostratigraphicZone;
                darwinCore.GeologicalContext.Member = webDarwinCore.GeologicalContext.Member;
            }

            if (webDarwinCore.Event.IsNotNull())
            {
                darwinCore.Event = new DarwinCoreEvent();
                darwinCore.Event.Day = webDarwinCore.Event.Day;
                darwinCore.Event.End = webDarwinCore.Event.End;
                darwinCore.Event.EndDayOfYear = webDarwinCore.Event.EndDayOfYear;
                darwinCore.Event.EventDate = webDarwinCore.Event.EventDate;
                darwinCore.Event.EventID = webDarwinCore.Event.EventID;
                darwinCore.Event.EventRemarks = webDarwinCore.Event.EventRemarks;
                darwinCore.Event.EventTime = webDarwinCore.Event.EventTime;
                darwinCore.Event.FieldNotes = webDarwinCore.Event.FieldNotes;
                darwinCore.Event.FieldNumber = webDarwinCore.Event.FieldNumber;
                darwinCore.Event.Habitat = webDarwinCore.Event.Habitat;
                darwinCore.Event.Month = webDarwinCore.Event.Month;
                darwinCore.Event.SamplingEffort = webDarwinCore.Event.SamplingEffort;
                darwinCore.Event.SamplingProtocol = webDarwinCore.Event.SamplingProtocol;
                darwinCore.Event.Start = webDarwinCore.Event.Start;
                darwinCore.Event.StartDayOfYear = webDarwinCore.Event.StartDayOfYear;
                darwinCore.Event.VerbatimEventDate = webDarwinCore.Event.VerbatimEventDate;
                darwinCore.Event.Year = webDarwinCore.Event.Year;
            }

            if (webDarwinCore.Conservation.IsNotNull())
            {
                darwinCore.Conservation = new DarwinCoreConservation();
                darwinCore.Conservation.ActionPlan = webDarwinCore.Conservation.ActionPlan;
                darwinCore.Conservation.ConservationRelevant = webDarwinCore.Conservation.ConservationRelevant;
                darwinCore.Conservation.Natura2000 = webDarwinCore.Conservation.Natura2000;
                darwinCore.Conservation.ProtectedByLaw = webDarwinCore.Conservation.ProtectedByLaw;
                darwinCore.Conservation.ProtectionLevel = webDarwinCore.Conservation.ProtectionLevel;
                darwinCore.Conservation.RedlistCategory = webDarwinCore.Conservation.RedlistCategory;
                darwinCore.Conservation.SwedishImmigrationHistory = webDarwinCore.Conservation.SwedishImmigrationHistory;
                darwinCore.Conservation.SwedishOccurrence = webDarwinCore.Conservation.SwedishOccurrence;
            }

            if (webDarwinCore.Location.IsNotNull())
            {
                darwinCore.Location = new DarwinCoreLocation();
                darwinCore.Location.Continent = webDarwinCore.Location.Continent;
                darwinCore.Location.CoordinateM = webDarwinCore.Location.CoordinateM;
                darwinCore.Location.CoordinatePrecision = webDarwinCore.Location.CoordinatePrecision;
                darwinCore.Location.CoordinateSystemWkt = webDarwinCore.Location.CoordinateSystemWkt;
                darwinCore.Location.CoordinateUncertaintyInMeters = webDarwinCore.Location.CoordinateUncertaintyInMeters;
                darwinCore.Location.CoordinateX = webDarwinCore.Location.CoordinateX;
                darwinCore.Location.CoordinateY = webDarwinCore.Location.CoordinateY;
                darwinCore.Location.CoordinateZ = webDarwinCore.Location.CoordinateZ;
                darwinCore.Location.Country = webDarwinCore.Location.Country;
                darwinCore.Location.CountryCode = webDarwinCore.Location.CountryCode;
                darwinCore.Location.County = webDarwinCore.Location.County;
                darwinCore.Location.DecimalLatitude = webDarwinCore.Location.DecimalLatitude;
                darwinCore.Location.DecimalLongitude = webDarwinCore.Location.DecimalLongitude;
                darwinCore.Location.FootprintSRS = webDarwinCore.Location.FootprintSRS;
                darwinCore.Location.FootprintSpatialFit = webDarwinCore.Location.FootprintSpatialFit;
                darwinCore.Location.FootprintWKT = webDarwinCore.Location.FootprintWKT;
                darwinCore.Location.GeodeticDatum = webDarwinCore.Location.GeodeticDatum;
                darwinCore.Location.GeoreferenceProtocol = webDarwinCore.Location.GeoreferenceProtocol;
                darwinCore.Location.GeoreferenceRemarks = webDarwinCore.Location.GeoreferenceRemarks;
                darwinCore.Location.GeoreferenceSources = webDarwinCore.Location.GeoreferenceSources;
                darwinCore.Location.GeoreferenceVerificationStatus = webDarwinCore.Location.GeoreferenceVerificationStatus;
                darwinCore.Location.GeoreferencedBy = webDarwinCore.Location.GeoreferencedBy;
                darwinCore.Location.GeoreferencedDate = webDarwinCore.Location.GeoreferencedDate;
                darwinCore.Location.HigherGeography = webDarwinCore.Location.HigherGeography;
                darwinCore.Location.HigherGeographyID = webDarwinCore.Location.HigherGeographyID;
                darwinCore.Location.Island = webDarwinCore.Location.Island;
                darwinCore.Location.IslandGroup = webDarwinCore.Location.IslandGroup;
                darwinCore.Location.Locality = webDarwinCore.Location.Locality;
                darwinCore.Location.LocationAccordingTo = webDarwinCore.Location.LocationAccordingTo;
                darwinCore.Location.LocationId = webDarwinCore.Location.LocationId;
                darwinCore.Location.LocationRemarks = webDarwinCore.Location.LocationRemarks;
                darwinCore.Location.LocationURL = webDarwinCore.Location.LocationURL;
                darwinCore.Location.MaximumDepthInMeters = webDarwinCore.Location.MaximumDepthInMeters;
                darwinCore.Location.MaximumDistanceAboveSurfaceInMeters = webDarwinCore.Location.MaximumDistanceAboveSurfaceInMeters;
                darwinCore.Location.MaximumElevationInMeters = webDarwinCore.Location.MaximumElevationInMeters;
                darwinCore.Location.MinimumDepthInMeters = webDarwinCore.Location.MinimumDepthInMeters;
                darwinCore.Location.MinimumDistanceAboveSurfaceInMeters = webDarwinCore.Location.MinimumDistanceAboveSurfaceInMeters;
                darwinCore.Location.MinimumElevationInMeters = webDarwinCore.Location.MinimumElevationInMeters;
                darwinCore.Location.Municipality = webDarwinCore.Location.Municipality;
                darwinCore.Location.Parish = webDarwinCore.Location.Parish;
                darwinCore.Location.PointRadiusSpatialFit = webDarwinCore.Location.PointRadiusSpatialFit;
                darwinCore.Location.StateProvince = webDarwinCore.Location.StateProvince;
                darwinCore.Location.VerbatimCoordinateSystem = webDarwinCore.Location.VerbatimCoordinateSystem;
                darwinCore.Location.VerbatimCoordinates = webDarwinCore.Location.VerbatimCoordinates;
                darwinCore.Location.VerbatimDepth = webDarwinCore.Location.VerbatimDepth;
                darwinCore.Location.VerbatimElevation = webDarwinCore.Location.VerbatimElevation;
                darwinCore.Location.VerbatimLatitude = webDarwinCore.Location.VerbatimLatitude;
                darwinCore.Location.VerbatimLocality = webDarwinCore.Location.VerbatimLocality;
                darwinCore.Location.VerbatimLongitude = webDarwinCore.Location.VerbatimLongitude;
                darwinCore.Location.VerbatimSRS = webDarwinCore.Location.VerbatimSRS;
                darwinCore.Location.WaterBody = webDarwinCore.Location.WaterBody;
            }

            return darwinCore;
        }

        /// <summary>
        /// Get changes in species observations for a certain time space or certain changeId.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="changedFrom">Start date for change.</param>
        /// <param name="changedTo">End date for change.</param>
        /// <param name="changeId">Change Id.</param>
        /// <param name="maxReturnedChanges">Max number of returned rows.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned species observations.</param>
        /// <returns>Changed species observation.</returns>
        public virtual IDarwinCoreChange GetDarwinCoreChange(IUserContext userContext,
                                                             DateTime? changedFrom,
                                                             DateTime? changedTo,
                                                             Int64? changeId,
                                                             Int64 maxReturnedChanges,
                                                             ISpeciesObservationSearchCriteria searchCriteria,
                                                             ICoordinateSystem coordinateSystem)
        {
            DateTime changedFromDateTime, changedToDateTime;
            IDarwinCoreChange darwinCoreChange;
            Int64 tempChangeId;
            WebCoordinateSystem webCoordinateSystem;
            WebDarwinCoreChange webDarwinCoreChange;
            WebSpeciesObservationSearchCriteria webSpeciesObservationSearchCriteria;

            if (changedFrom.HasValue)
            {
                changedFromDateTime = changedFrom.Value;
            }
            else
            {
                changedFromDateTime = DateTime.Now;
            }

            if (changedTo.HasValue)
            {
                changedToDateTime = changedTo.Value;
            }
            else
            {
                changedToDateTime = DateTime.Now;
            }

            if (changeId.HasValue)
            {
                tempChangeId = changeId.Value;
            }
            else
            {
                tempChangeId = 0;
            }

            webCoordinateSystem = GetCoordinateSystem(coordinateSystem);
            webSpeciesObservationSearchCriteria = GetSpeciesObservationSearchCriteria(searchCriteria);
            webDarwinCoreChange = WebServiceProxy.SwedishSpeciesObservationService.GetDarwinCoreChange(GetClientInformation(userContext),
                                                                                                       changedFromDateTime,
                                                                                                       changedFrom.HasValue,
                                                                                                       changedToDateTime,
                                                                                                       changedTo.HasValue,
                                                                                                       tempChangeId,
                                                                                                       changeId.HasValue,
                                                                                                       maxReturnedChanges,
                                                                                                       webSpeciesObservationSearchCriteria,
                                                                                                       webCoordinateSystem);

            darwinCoreChange = new DarwinCoreChange();
            darwinCoreChange.CreatedSpeciesObservations = new DarwinCoreList();
            darwinCoreChange.UpdatedSpeciesObservations = new DarwinCoreList();
            darwinCoreChange.DeletedSpeciesObservationGuids = new List<string>();

#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
            foreach (WebDarwinCore createdSpeciesObservation in webDarwinCoreChange.CreatedSpeciesObservations)
            {
                darwinCoreChange.CreatedSpeciesObservations.Add(GetDarwinCore(userContext, createdSpeciesObservation));
            }
#endif

            foreach (WebDarwinCore updatedSpeciesObservation in webDarwinCoreChange.UpdatedSpeciesObservations)
            {
                darwinCoreChange.UpdatedSpeciesObservations.Add(GetDarwinCore(userContext, updatedSpeciesObservation));
            }

            foreach (String deletedSpeciesObservation in webDarwinCoreChange.DeletedSpeciesObservationGuids)
            {
                darwinCoreChange.DeletedSpeciesObservationGuids.Add(deletedSpeciesObservation);
            }

#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
            darwinCoreChange.MaxChangeCount = webDarwinCoreChange.MaxChangeCount;
            darwinCoreChange.IsMoreSpeciesObservationsAvailable = webDarwinCoreChange.IsMoreSpeciesObservationsAvailable;
            darwinCoreChange.MaxChangeId = webDarwinCoreChange.MaxChangeId;
#endif

            return darwinCoreChange;
        }

        /// <summary>
        /// Convert a WebDataField instance into an IDataField instance.
        /// </summary>
        /// <param name="webDataField">A WebDataField instance.</param>
        /// <returns>An IDataField instance.</returns>
        private IDataField GetDataField(WebDataField webDataField)
        {
            IDataField dataField;

            dataField = null;
            if (webDataField.IsNotNull())
            {
                dataField = new DataField();
                dataField.Information = webDataField.Information;
                dataField.Name = webDataField.Name;
                dataField.Type = GetDataType(webDataField.Type);
                dataField.Unit = webDataField.Unit;
                dataField.Value = webDataField.Value;
            }

            return dataField;
        }

        /// <summary>
        /// Convert a list of WebDataField instances to a list of IDataField instances.
        /// </summary>
        /// <param name="webDataFields">A list of WebDataField instances.</param>
        /// <returns>A list of IDataField instances.</returns>
        private DataFieldList GetDataFields(List<WebDataField> webDataFields)
        {
            DataFieldList dataFields;

            dataFields = null;
            if (webDataFields.IsNotEmpty())
            {
                dataFields = new DataFieldList();
                foreach (WebDataField webDataField in webDataFields)
                {
                    dataFields.Add(GetDataField(webDataField));
                }
            }

            return dataFields;
        }

        /// <summary>
        /// Convert a WebDataType value to a DataType value.
        /// </summary>
        /// <param name="webDataType">A WebDataType value.</param>
        /// <returns>A DataType value.</returns>
        private DataType GetDataType(WebDataType webDataType)
        {
            switch (webDataType)
            {
                case WebDataType.Boolean:
                    return DataType.Boolean;
                case WebDataType.DateTime:
                    return DataType.DateTime;
                case WebDataType.Float64:
                    return DataType.Float64;
                case WebDataType.Int32:
                    return DataType.Int32;
                case WebDataType.Int64:
                    return DataType.Int64;
                case WebDataType.String:
                    return DataType.String;
                default:
                    throw new ArgumentException("Not supported WebDataType = " + webDataType);
            }
        }

        /// <summary>
        /// Get an indication if specified geometries contains any
        /// protected species observations.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">
        /// The species observation search criteria.
        /// At least one of BoundingBox, Polygons and RegionGuids
        /// must be specified.
        /// Search criteria that may be used: Accuracy,
        /// BirdNestActivityLimit, BoundingBox, IsAccuracyConsidered,
        /// IsDisturbanceSensitivityConsidered, MinProtectionLevel,
        /// ObservationDateTime, Polygons and RegionGuids.
        /// </param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.</param>
        /// <returns>
        /// True, if specified geometries contains any
        /// protected species observations.
        /// </returns>
        public virtual Boolean GetProtectedSpeciesObservationIndication(IUserContext userContext,
                                                                        ISpeciesObservationSearchCriteria searchCriteria,
                                                                        ICoordinateSystem coordinateSystem)
        {
            return WebServiceProxy.SwedishSpeciesObservationService.GetProtectedSpeciesObservationIndication(GetClientInformation(userContext),
                                                                                                             GetSpeciesObservationSearchCriteria(searchCriteria),
                                                                                                             GetCoordinateSystem(coordinateSystem));
        }

        /// <summary>
        /// Get all province regions
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <returns>Information about province regions</returns>
        public virtual RegionList GetProvinceRegions(IUserContext userContext)
        {
            List<WebRegion> webRegions;

            webRegions = WebServiceProxy.SwedishSpeciesObservationService.GetProvinceRegions(GetClientInformation(userContext));
            return GetRegions(userContext, webRegions);
        }

        /// <summary>
        /// Get all Activities.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All Activities.</returns>       
        public virtual SpeciesActivityList GetSpeciesActivities(IUserContext userContext)
        {
            List<WebSpeciesActivity> webSpeciesActivities;

            webSpeciesActivities = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesActivities(GetClientInformation(userContext));
            return GetSpeciesActivities(userContext, webSpeciesActivities);
        }

        /// <summary>
        /// Convert a list of WebSpeciesActivity instances
        /// to a SpeciesActivityList.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webSpeciesActivities">List of WebSpeciesActivity instances.</param>
        /// <returns>Species activities.</returns>
        private SpeciesActivityList GetSpeciesActivities(IUserContext userContext,
                                                         List<WebSpeciesActivity> webSpeciesActivities)
        {
            SpeciesActivityList speciesActivities;

            speciesActivities = new SpeciesActivityList();
            if (webSpeciesActivities.IsNotEmpty())
            {
                foreach (WebSpeciesActivity webSpeciesActivity in webSpeciesActivities)
                {
                    speciesActivities.Add(GetSpeciesActivity(userContext, webSpeciesActivity));
                }
            }

            return speciesActivities;
        }

        /// <summary>
        /// Convert a WebSpeciesActivity instance
        /// to a ISpeciesActivity instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webSpeciesActivity">A WebSpeciesActivity object.</param>
        /// <returns>A ISpeciesActivity instance.</returns>
        private ISpeciesActivity GetSpeciesActivity(IUserContext userContext,
                                                    WebSpeciesActivity webSpeciesActivity)
        {
            ISpeciesActivity speciesActivity;

            speciesActivity = new SpeciesActivity();
#if NOT_YET_DEFINED
            if (webSpeciesActivity.CategoryId == 0)
            {
                speciesActivity.Category = null;
            }
            else
            {
                speciesActivity.Category = CoreData.SpeciesObservationManager.GetSpeciesActivityCategory(userContext, webSpeciesActivity.CategoryId);
            }
#else
            speciesActivity.Category = null;
#endif
            speciesActivity.DataContext = GetDataContext(userContext);
            speciesActivity.Guid = webSpeciesActivity.Guid;
            speciesActivity.Id = webSpeciesActivity.Id;
            speciesActivity.Identifier = webSpeciesActivity.Identifier;
            speciesActivity.Name = webSpeciesActivity.Name;
            speciesActivity.TaxonIds = webSpeciesActivity.TaxonIds;
            return speciesActivity;
        }

        /// <summary>
        /// Get all species activity categories.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All species activity categories.</returns>
        public virtual SpeciesActivityCategoryList GetSpeciesActivityCategories(IUserContext userContext)
        {
            SpeciesActivityCategoryList speciesActivityCategories = new SpeciesActivityCategoryList();

            List<WebSpeciesActivityCategory> listspeciesActivityCategory = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesActivityCategories(GetClientInformation(userContext));

            foreach (WebSpeciesActivityCategory speciesActivityItem in listspeciesActivityCategory)
            {
                ISpeciesActivityCategory speciesActivityCategory = new SpeciesActivityCategory();

                speciesActivityCategory.DataContext = GetDataContext(userContext);
                speciesActivityCategory.Guid = speciesActivityItem.Guid;
                speciesActivityCategory.Id = speciesActivityItem.Id;
                speciesActivityCategory.Identifier = speciesActivityItem.Identifier;
                speciesActivityCategory.Name = speciesActivityItem.Name;

                speciesActivityCategories.Add(speciesActivityCategory);
            }

            return speciesActivityCategories;
        }

        /// <summary>
        /// Convert a WebSpeciesObservation
        /// instance to a ISpeciesObservation instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webSpeciesObservation">A WebSpeciesObservation instance.</param>
        /// <returns>A ISpeciesObservation instances.</returns>
        private ISpeciesObservation GetSpeciesObservation(IUserContext userContext,
                                                          WebSpeciesObservation webSpeciesObservation)
        {
            ISpeciesObservation speciesObservation;

            speciesObservation = new SpeciesObservation();
            speciesObservation.DataContext = GetDataContext(userContext);
            if (webSpeciesObservation.IsNotNull() &&
                webSpeciesObservation.Fields.IsNotEmpty())
            {
                speciesObservation.Fields = new SpeciesObservationFieldList();
                foreach (WebSpeciesObservationField webSpeciesObservationField in webSpeciesObservation.Fields)
                {
                    speciesObservation.Fields.Add(GetSpeciesObservationField(userContext,
                                                                             webSpeciesObservationField));

                    if (Enum.IsDefined(typeof(SpeciesObservationClassId), webSpeciesObservationField.ClassIdentifier))
                    {
                        switch ((SpeciesObservationClassId)(Enum.Parse(typeof(SpeciesObservationClassId), webSpeciesObservationField.ClassIdentifier)))
                        {
                            case SpeciesObservationClassId.Conservation:
                                GetSpeciesObservationConservation(speciesObservation,
                                                                  webSpeciesObservationField);
                                break;
                            case SpeciesObservationClassId.DarwinCore:
                                GetSpeciesObservationDarwinCore(speciesObservation,
                                                                webSpeciesObservationField);
                                break;
                            case SpeciesObservationClassId.Event:
                                GetSpeciesObservationEvent(speciesObservation,
                                                           webSpeciesObservationField);
                                break;
                            case SpeciesObservationClassId.GeologicalContext:
                                GetSpeciesObservationGeologicalContext(speciesObservation,
                                                                       webSpeciesObservationField);
                                break;
                            case SpeciesObservationClassId.Identification:
                                GetSpeciesObservationIdentification(speciesObservation,
                                                                    webSpeciesObservationField);
                                break;
                            case SpeciesObservationClassId.Location:
                                GetSpeciesObservationLocation(speciesObservation,
                                                              webSpeciesObservationField);
                                break;
                            case SpeciesObservationClassId.MeasurementOrFact:
                                GetSpeciesObservationMeasurementOrFact(speciesObservation,
                                                                       webSpeciesObservationField);
                                break;
                            case SpeciesObservationClassId.Occurrence:
                                GetSpeciesObservationOccurrence(speciesObservation,
                                                                webSpeciesObservationField);
                                break;
                            case SpeciesObservationClassId.Project:
                                GetSpeciesObservationProject(speciesObservation,
                                                             webSpeciesObservationField);
                                break;
                            case SpeciesObservationClassId.ResourceRelationship:
                                GetSpeciesObservationResourceRelationship(speciesObservation,
                                                                          webSpeciesObservationField);
                                break;
                            case SpeciesObservationClassId.Taxon:
                                GetSpeciesObservationTaxon(speciesObservation,
                                                           webSpeciesObservationField);
                                break;
                        }
                    }
                }
            }

            return speciesObservation;
        }

        /// <summary>
        /// Get information about species observations that has
        /// changed.
        /// 
        /// Scope is restricted to those observations that the
        /// user has access rights to. There is no access right
        /// check on deleted species observations. This means
        /// that a client can obtain information about deleted
        /// species observations that the client has not
        /// received any create or update information about.
        /// 
        /// Max 25000 species observation changes can be
        /// retrieved in one web service call.
        /// Exactly one of the parameters changedFrom and 
        /// changeId should be specified.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="changedFrom">Start date and time for changes that should be returned.</param>
        /// <param name="changedTo">
        /// End date and time for changes that should be
        /// returned. Parameter changedTo is optional and works
        /// with either parameter changedFrom or changeId.
        /// </param>
        /// <param name="changeId">
        /// Start id for changes that should be returned.
        /// The species observation that is changed in the
        /// specified change id may be included in returned
        /// information.
        /// </param>
        /// <param name="maxReturnedChanges">
        /// Requested maximum number of changes that should
        /// be returned. This property is used by the client
        /// to avoid problems with resource limitations on
        /// the client side.
        /// Max 25000 changes are returned if property
        /// maxChanges has a higher value than 25000.
        /// </param>
        /// <param name="searchCriteria">
        /// Only species observations that matches the search 
        /// criteria are included in the returned information.
        /// This parameter is optional and may be null.
        /// There is no check on search criteria for
        /// deleted species observations. </param>
        /// <param name="coordinateSystem">Coordinate system used in returned species observations. </param>
        /// <param name="speciesObservationSpecification">
        /// Specify which subset of the data that should be
        /// returned for each species observation.
        /// All information for each species observation is
        /// returned if this parameter is null.
        /// This parameter is currently not used.
        /// </param>
        /// <returns>
        /// Information about changed species observations.
        /// </returns>
        public virtual ISpeciesObservationChange GetSpeciesObservationChange(IUserContext userContext,
                                                                             DateTime? changedFrom,
                                                                             DateTime? changedTo,
                                                                             Int64? changeId,
                                                                             Int64 maxReturnedChanges,
                                                                             ISpeciesObservationSearchCriteria searchCriteria,
                                                                             ICoordinateSystem coordinateSystem,
                                                                             ISpeciesObservationSpecification speciesObservationSpecification)
        {
            DateTime changedFromDateTime, changedToDateTime;
            Int64 changedIdInt64;
            ISpeciesObservationChange speciesObservationChange;
            WebSpeciesObservationChange webSpeciesObservationChange;

            if (changedFrom.HasValue)
            {
                changedFromDateTime = changedFrom.Value;
            }
            else
            {
                changedFromDateTime = DateTime.Now;
            }

            if (changedTo.HasValue)
            {
                changedToDateTime = changedTo.Value;
            }
            else
            {
                changedToDateTime = DateTime.Now;
            }

            if (changeId.HasValue)
            {
                changedIdInt64 = changeId.Value;
            }
            else
            {
                changedIdInt64 = 0;
            }

            webSpeciesObservationChange = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationChange(GetClientInformation(userContext),
                                                                                                                       changedFromDateTime,
                                                                                                                       changedFrom.HasValue,
                                                                                                                       changedToDateTime,
                                                                                                                       changedTo.HasValue,
                                                                                                                       changedIdInt64,
                                                                                                                       changeId.HasValue,
                                                                                                                       maxReturnedChanges,
                                                                                                                       GetSpeciesObservationSearchCriteria(searchCriteria),
                                                                                                                       GetCoordinateSystem(coordinateSystem),
                                                                                                                       GetSpeciesObservationSpecification(speciesObservationSpecification));
            speciesObservationChange = GetSpeciesObservationChange(userContext, webSpeciesObservationChange);
            return speciesObservationChange;
        }

        /// <summary>
        /// Convert a WebSpeciesObservationChange instance
        /// to a ISpeciesObservationChange instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webSpeciesObservationChange">A WebSpeciesObservationChange object.</param>
        /// <returns>An ISpeciesObservationChange instance.</returns>
        private ISpeciesObservationChange GetSpeciesObservationChange(IUserContext userContext,
                                                                      WebSpeciesObservationChange webSpeciesObservationChange)
        {
            ISpeciesObservationChange speciesObservationChange;

            speciesObservationChange = new SpeciesObservationChange();
            speciesObservationChange.CreatedSpeciesObservations = GetSpeciesObservations(userContext, webSpeciesObservationChange.CreatedSpeciesObservations);
            speciesObservationChange.DataContext = GetDataContext(userContext);
            speciesObservationChange.DeletedSpeciesObservationGuids = webSpeciesObservationChange.DeletedSpeciesObservationGuids;
            speciesObservationChange.IsMoreSpeciesObservationsAvailable = webSpeciesObservationChange.IsMoreSpeciesObservationsAvailable;
            speciesObservationChange.MaxChangeCount = webSpeciesObservationChange.MaxChangeCount;
            speciesObservationChange.MaxChangeId = webSpeciesObservationChange.MaxChangeId;
            speciesObservationChange.UpdatedSpeciesObservations = GetSpeciesObservations(userContext, webSpeciesObservationChange.UpdatedSpeciesObservations);
            return speciesObservationChange;
        }

        /// <summary>
        /// Convert a ISpeciesObservationClass instance
        /// to a WebSpeciesObservationClass instance.
        /// </summary>
        /// <param name="speciesObservationClass">A ISpeciesObservationClass instance.</param>
        /// <returns>A WebSpeciesObservationClass instance.</returns>
        private WebSpeciesObservationClass GetSpeciesObservationClass(ISpeciesObservationClass speciesObservationClass)
        {
            WebSpeciesObservationClass webSpeciesObservationClass;

            webSpeciesObservationClass = new WebSpeciesObservationClass();
            webSpeciesObservationClass.Id = speciesObservationClass.Id;
            webSpeciesObservationClass.Identifier = speciesObservationClass.Identifier;

            return webSpeciesObservationClass;
        }

        /// <summary>
        /// Get an ISpeciesObservationClass instance based
        /// on a species observation class identifier.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="speciesObservationClassIdentifier">Identifier for a species observation class.</param>
        /// <returns>An ISpeciesObservationClass instance.</returns>
        private ISpeciesObservationClass GetSpeciesObservationClass(IUserContext userContext,
                                                                    String speciesObservationClassIdentifier)
        {
            ISpeciesObservationClass speciesObservationClass;

            speciesObservationClass = new SpeciesObservationClass();
            speciesObservationClass.DataContext = GetDataContext(userContext);
            if (Enum.IsDefined(typeof(SpeciesObservationClassId), speciesObservationClassIdentifier))
            {
                speciesObservationClass.Id = (SpeciesObservationClassId)(Enum.Parse(typeof(SpeciesObservationClassId), speciesObservationClassIdentifier));
            }
            else
            {
                speciesObservationClass.Id = SpeciesObservationClassId.None;
            }

            speciesObservationClass.Identifier = speciesObservationClassIdentifier;
            return speciesObservationClass;
        }

        /// <summary>
        /// Convert a WebSpeciesObservationClass instance
        /// to a ISpeciesObservationClass instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webSpeciesObservationClass">A WebSpeciesObservationClass object.</param>
        /// <returns>An ISpeciesObservationClass instance.</returns>
        private ISpeciesObservationClass GetSpeciesObservationClass(IUserContext userContext,
                                                                    WebSpeciesObservationClass webSpeciesObservationClass)
        {
            ISpeciesObservationClass speciesObservationClass;

            speciesObservationClass = new SpeciesObservationClass();
            speciesObservationClass.DataContext = GetDataContext(userContext);
            speciesObservationClass.Id = webSpeciesObservationClass.Id;
            speciesObservationClass.Identifier = webSpeciesObservationClass.Identifier;

            return speciesObservationClass;
        }

        /// <summary>
        /// Add conservation information from species observation field
        /// to a species observation instance. Only properties defined
        /// in interface ISpeciesObservationConservation are handled.
        /// </summary>
        /// <param name="speciesObservation">Species observation.</param>
        /// <param name="webSpeciesObservationField">Species observation field.</param>
        private void GetSpeciesObservationConservation(ISpeciesObservation speciesObservation,
                                                       WebSpeciesObservationField webSpeciesObservationField)
        {
            if (speciesObservation.Conservation.IsNull())
            {
                speciesObservation.Conservation = new SpeciesObservationConservation();
            }

            if (Enum.IsDefined(typeof(SpeciesObservationPropertyId), webSpeciesObservationField.PropertyIdentifier))
            {
                switch ((SpeciesObservationPropertyId)(Enum.Parse(typeof(SpeciesObservationPropertyId), webSpeciesObservationField.PropertyIdentifier)))
                {
                    case SpeciesObservationPropertyId.ActionPlan:
                        speciesObservation.Conservation.ActionPlan = webSpeciesObservationField.Value.WebParseBoolean();
                        break;
                    case SpeciesObservationPropertyId.ConservationRelevant:
                        speciesObservation.Conservation.ConservationRelevant = webSpeciesObservationField.Value.WebParseBoolean();
                        break;
                    case SpeciesObservationPropertyId.Natura2000:
                        speciesObservation.Conservation.Natura2000 = webSpeciesObservationField.Value.WebParseBoolean();
                        break;
                    case SpeciesObservationPropertyId.ProtectedByLaw:
                        speciesObservation.Conservation.ProtectedByLaw = webSpeciesObservationField.Value.WebParseBoolean();
                        break;
                    case SpeciesObservationPropertyId.ProtectionLevel:
                        speciesObservation.Conservation.ProtectionLevel = webSpeciesObservationField.Value.WebParseInt32();
                        break;
                    case SpeciesObservationPropertyId.RedlistCategory:
                        speciesObservation.Conservation.RedlistCategory = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.SwedishImmigrationHistory:
                        speciesObservation.Conservation.SwedishImmigrationHistory = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.SwedishOccurrence:
                        speciesObservation.Conservation.SwedishOccurrence = webSpeciesObservationField.Value;
                        break;
                }
            }
        }

        /// <summary>
        /// Add darwin core information from species observation field
        /// to a species observation instance. Only properties defined
        /// in interface ISpeciesObservation are handled.
        /// </summary>
        /// <param name="speciesObservation">Species observation.</param>
        /// <param name="webSpeciesObservationField">Species observation field.</param>
        private void GetSpeciesObservationDarwinCore(ISpeciesObservation speciesObservation,
                                                     WebSpeciesObservationField webSpeciesObservationField)
        {
            if (Enum.IsDefined(typeof(SpeciesObservationPropertyId), webSpeciesObservationField.PropertyIdentifier))
            {
                switch ((SpeciesObservationPropertyId)(Enum.Parse(typeof(SpeciesObservationPropertyId), webSpeciesObservationField.PropertyIdentifier)))
                {
                    case SpeciesObservationPropertyId.AccessRights:
                        speciesObservation.AccessRights = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.BasisOfRecord:
                        speciesObservation.BasisOfRecord = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.BibliographicCitation:
                        speciesObservation.BibliographicCitation = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.CollectionCode:
                        speciesObservation.CollectionCode = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.CollectionID:
                        speciesObservation.CollectionID = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.DataGeneralizations:
                        speciesObservation.DataGeneralizations = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.DatasetID:
                        speciesObservation.DatasetID = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.DatasetName:
                        speciesObservation.DatasetName = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.DynamicProperties:
                        speciesObservation.DynamicProperties = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Id:
                        speciesObservation.Id = webSpeciesObservationField.Value.WebParseInt64();
                        break;
                    case SpeciesObservationPropertyId.InformationWithheld:
                        speciesObservation.InformationWithheld = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.InstitutionCode:
                        speciesObservation.InstitutionCode = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.InstitutionID:
                        speciesObservation.InstitutionID = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Language:
                        speciesObservation.Language = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Modified:
                        speciesObservation.Modified = webSpeciesObservationField.Value.WebParseDateTime();
                        break;
                    case SpeciesObservationPropertyId.Owner:
                        speciesObservation.Owner = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.OwnerInstitutionCode:
                        speciesObservation.OwnerInstitutionCode = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.References:
                        speciesObservation.References = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.ReportedBy:
                        speciesObservation.ReportedBy = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.ReportedDate:
                        speciesObservation.ReportedDate = webSpeciesObservationField.Value.WebParseDateTime();
                        break;
                    case SpeciesObservationPropertyId.Rights:
                        speciesObservation.Rights = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.RightsHolder:
                        speciesObservation.RightsHolder = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.SpeciesObservationURL:
                        speciesObservation.SpeciesObservationURL = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Type:
                        speciesObservation.Type = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.ValidationStatus:
                        speciesObservation.ValidationStatus = webSpeciesObservationField.Value;
                        break;
                }
            }
        }

        /// <summary>
        /// Get information about species observations data providers.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>Data providers list.</returns>
        public virtual SpeciesObservationDataProviderList GetSpeciesObservationDataProviders(IUserContext userContext)
        {
            List<WebSpeciesObservationDataProvider> webSpeciesObservationDataProviders;
            ISpeciesObservationDataProvider dataProvider;
            SpeciesObservationDataProviderList speciesObservationDataProviders;

            speciesObservationDataProviders = new SpeciesObservationDataProviderList();
            webSpeciesObservationDataProviders = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationDataProviders(GetClientInformation(userContext));

            foreach (WebSpeciesObservationDataProvider webSpeciesObservationDataSource in webSpeciesObservationDataProviders)
            {
                dataProvider = new SpeciesObservationDataProvider();
                dataProvider.ContactEmail = webSpeciesObservationDataSource.ContactEmail;
                dataProvider.ContactPerson = webSpeciesObservationDataSource.ContactPerson;
                dataProvider.DataContext = GetDataContext(userContext);
                dataProvider.Description = webSpeciesObservationDataSource.Description;
                dataProvider.Guid = webSpeciesObservationDataSource.Guid;
                dataProvider.Id = webSpeciesObservationDataSource.Id;
                dataProvider.Name = webSpeciesObservationDataSource.Name;
                dataProvider.NonPublicSpeciesObservationCount = webSpeciesObservationDataSource.NonPublicSpeciesObservationCount;
                dataProvider.Organization = webSpeciesObservationDataSource.Organization;
                dataProvider.PublicSpeciesObservationCount = webSpeciesObservationDataSource.PublicSpeciesObservationCount;
                dataProvider.SpeciesObservationCount = webSpeciesObservationDataSource.SpeciesObservationCount;
                dataProvider.Url = webSpeciesObservationDataSource.Url;
                speciesObservationDataProviders.Add(dataProvider);
            }

            return speciesObservationDataProviders;
        }

        /// <summary>
        /// Convert a WebSpeciesObservationField
        /// instance to an ISpeciesObservationField instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webSpeciesObservationField">A WebSpeciesObservationField instance.</param>
        /// <returns>An ISpeciesObservationField instances.</returns>
        private ISpeciesObservationField GetSpeciesObservationField(IUserContext userContext,
                                                                    WebSpeciesObservationField webSpeciesObservationField)
        {
            ISpeciesObservationField speciesObservationField;

            speciesObservationField = new SpeciesObservationField();
            speciesObservationField.Class = GetSpeciesObservationClass(userContext, webSpeciesObservationField.ClassIdentifier);
            speciesObservationField.DataContext = GetDataContext(userContext);
            speciesObservationField.DataFields = GetDataFields(webSpeciesObservationField.DataFields);
            speciesObservationField.Property = GetSpeciesObservationProperty(userContext, webSpeciesObservationField.PropertyIdentifier);
            speciesObservationField.Type = GetDataType(webSpeciesObservationField.Type);
            speciesObservationField.Value = webSpeciesObservationField.Value;

            return speciesObservationField;
        }

        /// <summary>
        /// Add event information from species observation field
        /// to a species observation instance. Only properties defined
        /// in interface ISpeciesObservationEvent are handled.
        /// </summary>
        /// <param name="speciesObservation">Species observation.</param>
        /// <param name="webSpeciesObservationField">Species observation field.</param>
        private void GetSpeciesObservationEvent(ISpeciesObservation speciesObservation,
                                                WebSpeciesObservationField webSpeciesObservationField)
        {
            if (speciesObservation.Event.IsNull())
            {
                speciesObservation.Event = new SpeciesObservationEvent();
            }

            if (Enum.IsDefined(typeof(SpeciesObservationPropertyId), webSpeciesObservationField.PropertyIdentifier))
            {
                switch ((SpeciesObservationPropertyId)(Enum.Parse(typeof(SpeciesObservationPropertyId), webSpeciesObservationField.PropertyIdentifier)))
                {
                    case SpeciesObservationPropertyId.Day:
                        speciesObservation.Event.Day = webSpeciesObservationField.Value.WebParseInt32();
                        break;
                    case SpeciesObservationPropertyId.End:
                        speciesObservation.Event.End = webSpeciesObservationField.Value.WebParseDateTime();
                        break;
                    case SpeciesObservationPropertyId.EndDayOfYear:
                        speciesObservation.Event.EndDayOfYear = webSpeciesObservationField.Value.WebParseInt32();
                        break;
                    case SpeciesObservationPropertyId.EventDate:
                        speciesObservation.Event.EventDate = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.EventID:
                        speciesObservation.Event.EventID = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.EventRemarks:
                        speciesObservation.Event.EventRemarks = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.EventTime:
                        speciesObservation.Event.EventTime = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.FieldNotes:
                        speciesObservation.Event.FieldNotes = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.FieldNumber:
                        speciesObservation.Event.FieldNumber = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Habitat:
                        speciesObservation.Event.Habitat = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Month:
                        speciesObservation.Event.Month = webSpeciesObservationField.Value.WebParseInt32();
                        break;
                    case SpeciesObservationPropertyId.SamplingEffort:
                        speciesObservation.Event.SamplingEffort = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.SamplingProtocol:
                        speciesObservation.Event.SamplingProtocol = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Start:
                        speciesObservation.Event.Start = webSpeciesObservationField.Value.WebParseDateTime();
                        break;
                    case SpeciesObservationPropertyId.StartDayOfYear:
                        speciesObservation.Event.StartDayOfYear = webSpeciesObservationField.Value.WebParseInt32();
                        break;
                    case SpeciesObservationPropertyId.VerbatimEventDate:
                        speciesObservation.Event.VerbatimEventDate = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Year:
                        speciesObservation.Event.Year = webSpeciesObservationField.Value.WebParseInt32();
                        break;
                }
            }
        }

        /// <summary>
        /// Add geological context information from species observation field
        /// to a species observation instance. Only properties defined
        /// in interface ISpeciesObservationGeologicalContext are handled.
        /// </summary>
        /// <param name="speciesObservation">Species observation.</param>
        /// <param name="webSpeciesObservationField">Species observation field.</param>
        private void GetSpeciesObservationGeologicalContext(ISpeciesObservation speciesObservation,
                                                            WebSpeciesObservationField webSpeciesObservationField)
        {
            if (speciesObservation.GeologicalContext.IsNull())
            {
                speciesObservation.GeologicalContext = new SpeciesObservationGeologicalContext();
            }

            if (Enum.IsDefined(typeof(SpeciesObservationPropertyId), webSpeciesObservationField.PropertyIdentifier))
            {
                switch ((SpeciesObservationPropertyId)(Enum.Parse(typeof(SpeciesObservationPropertyId), webSpeciesObservationField.PropertyIdentifier)))
                {
                    case SpeciesObservationPropertyId.Bed:
                        speciesObservation.GeologicalContext.Bed = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.EarliestAgeOrLowestStage:
                        speciesObservation.GeologicalContext.EarliestAgeOrLowestStage = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.EarliestEonOrLowestEonothem:
                        speciesObservation.GeologicalContext.EarliestEonOrLowestEonothem =
                            webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.EarliestEpochOrLowestSeries:
                        speciesObservation.GeologicalContext.EarliestEpochOrLowestSeries =
                            webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.EarliestEraOrLowestErathem:
                        speciesObservation.GeologicalContext.EarliestEraOrLowestErathem =
                            webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.EarliestPeriodOrLowestSystem:
                        speciesObservation.GeologicalContext.EarliestPeriodOrLowestSystem =
                            webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Formation:
                        speciesObservation.GeologicalContext.Formation = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.GeologicalContextID:
                        speciesObservation.GeologicalContext.GeologicalContextID = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Group:
                        speciesObservation.GeologicalContext.Group = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.HighestBiostratigraphicZone:
                        speciesObservation.GeologicalContext.HighestBiostratigraphicZone =
                            webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.LatestAgeOrHighestStage:
                        speciesObservation.GeologicalContext.LatestAgeOrHighestStage = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.LatestEonOrHighestEonothem:
                        speciesObservation.GeologicalContext.LatestEonOrHighestEonothem =
                            webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.LatestEpochOrHighestSeries:
                        speciesObservation.GeologicalContext.LatestEpochOrHighestSeries =
                            webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.LatestEraOrHighestErathem:
                        speciesObservation.GeologicalContext.LatestEraOrHighestErathem =
                            webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.LatestPeriodOrHighestSystem:
                        speciesObservation.GeologicalContext.LatestPeriodOrHighestSystem =
                            webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.LithostratigraphicTerms:
                        speciesObservation.GeologicalContext.LithostratigraphicTerms = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.LowestBiostratigraphicZone:
                        speciesObservation.GeologicalContext.LowestBiostratigraphicZone =
                            webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Member:
                        speciesObservation.GeologicalContext.Member = webSpeciesObservationField.Value;
                        break;
                }
            }
        }

        /// <summary>
        /// Add identification information from species observation field
        /// to a species observation instance. Only properties defined
        /// in interface ISpeciesObservationIdentification are handled.
        /// </summary>
        /// <param name="speciesObservation">Species observation.</param>
        /// <param name="webSpeciesObservationField">Species observation field.</param>
        private void GetSpeciesObservationIdentification(ISpeciesObservation speciesObservation,
                                                         WebSpeciesObservationField webSpeciesObservationField)
        {
            if (speciesObservation.Identification.IsNull())
            {
                speciesObservation.Identification = new SpeciesObservationIdentification();
            }

            if (Enum.IsDefined(typeof(SpeciesObservationPropertyId), webSpeciesObservationField.PropertyIdentifier))
            {
                switch ((SpeciesObservationPropertyId)(Enum.Parse(typeof(SpeciesObservationPropertyId), webSpeciesObservationField.PropertyIdentifier)))
                {
                    case SpeciesObservationPropertyId.DateIdentified:
                        speciesObservation.Identification.DateIdentified = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.IdentificationID:
                        speciesObservation.Identification.IdentificationID = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.IdentificationQualifier:
                        speciesObservation.Identification.IdentificationQualifier = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.IdentificationReferences:
                        speciesObservation.Identification.IdentificationReferences = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.IdentificationRemarks:
                        speciesObservation.Identification.IdentificationRemarks = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.IdentificationVerificationStatus:
                        speciesObservation.Identification.IdentificationVerificationStatus =
                            webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.IdentifiedBy:
                        speciesObservation.Identification.IdentifiedBy = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.TypeStatus:
                        speciesObservation.Identification.TypeStatus = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.UncertainDetermination:
                        speciesObservation.Identification.UncertainDetermination =
                            webSpeciesObservationField.Value.WebParseBoolean();
                        break;
                }
            }
        }

        /// <summary>
        /// Add location information from species observation field
        /// to a species observation instance. Only properties defined
        /// in interface ISpeciesObservationLocation are handled.
        /// </summary>
        /// <param name="speciesObservation">Species observation.</param>
        /// <param name="webSpeciesObservationField">Species observation field.</param>
        private void GetSpeciesObservationLocation(ISpeciesObservation speciesObservation,
                                                   WebSpeciesObservationField webSpeciesObservationField)
        {
            if (speciesObservation.Location.IsNull())
            {
                speciesObservation.Location = new SpeciesObservationLocation();
            }

            if (Enum.IsDefined(typeof(SpeciesObservationPropertyId), webSpeciesObservationField.PropertyIdentifier))
            {
                switch ((SpeciesObservationPropertyId)(Enum.Parse(typeof(SpeciesObservationPropertyId), webSpeciesObservationField.PropertyIdentifier)))
                {
                    case SpeciesObservationPropertyId.Continent:
                        speciesObservation.Location.Continent = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.CoordinateM:
                        speciesObservation.Location.CoordinateM = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.CoordinatePrecision:
                        speciesObservation.Location.CoordinatePrecision = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.CoordinateSystemWkt:
                        speciesObservation.Location.CoordinateSystemWkt = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.CoordinateUncertaintyInMeters:
                        speciesObservation.Location.CoordinateUncertaintyInMeters = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.CoordinateX:
                        speciesObservation.Location.CoordinateX = webSpeciesObservationField.Value.WebParseDouble();
                        break;
                    case SpeciesObservationPropertyId.CoordinateY:
                        speciesObservation.Location.CoordinateY = webSpeciesObservationField.Value.WebParseDouble();
                        break;
                    case SpeciesObservationPropertyId.CoordinateZ:
                        speciesObservation.Location.CoordinateZ = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Country:
                        speciesObservation.Location.Country = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.CountryCode:
                        speciesObservation.Location.CountryCode = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.County:
                        speciesObservation.Location.County = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.DecimalLatitude:
                        speciesObservation.Location.DecimalLatitude = webSpeciesObservationField.Value.WebParseDouble();
                        break;
                    case SpeciesObservationPropertyId.DecimalLongitude:
                        speciesObservation.Location.DecimalLongitude = webSpeciesObservationField.Value.WebParseDouble();
                        break;
                    case SpeciesObservationPropertyId.FootprintSpatialFit:
                        speciesObservation.Location.FootprintSpatialFit = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.FootprintSRS:
                        speciesObservation.Location.FootprintSRS = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.FootprintWKT:
                        speciesObservation.Location.FootprintWKT = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.GeodeticDatum:
                        speciesObservation.Location.GeodeticDatum = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.GeoreferencedBy:
                        speciesObservation.Location.GeoreferencedBy = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.GeoreferencedDate:
                        speciesObservation.Location.GeoreferencedDate = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.GeoreferenceProtocol:
                        speciesObservation.Location.GeoreferenceProtocol = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.GeoreferenceRemarks:
                        speciesObservation.Location.GeoreferenceRemarks = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.GeoreferenceSources:
                        speciesObservation.Location.GeoreferenceSources = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.GeoreferenceVerificationStatus:
                        speciesObservation.Location.GeoreferenceVerificationStatus = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.HigherGeography:
                        speciesObservation.Location.HigherGeography = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.HigherGeographyID:
                        speciesObservation.Location.HigherGeographyID = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Island:
                        speciesObservation.Location.Island = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.IslandGroup:
                        speciesObservation.Location.IslandGroup = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Locality:
                        speciesObservation.Location.Locality = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.LocationAccordingTo:
                        speciesObservation.Location.LocationAccordingTo = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.LocationId:
                        speciesObservation.Location.LocationID = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.LocationRemarks:
                        speciesObservation.Location.LocationRemarks = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.LocationURL:
                        speciesObservation.Location.LocationURL = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.MaximumDepthInMeters:
                        speciesObservation.Location.MaximumDepthInMeters = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.MaximumDistanceAboveSurfaceInMeters:
                        speciesObservation.Location.MaximumDistanceAboveSurfaceInMeters =
                            webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.MaximumElevationInMeters:
                        speciesObservation.Location.MaximumElevationInMeters = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.MinimumDepthInMeters:
                        speciesObservation.Location.MinimumDepthInMeters = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.MinimumDistanceAboveSurfaceInMeters:
                        speciesObservation.Location.MinimumDistanceAboveSurfaceInMeters =
                            webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.MinimumElevationInMeters:
                        speciesObservation.Location.MinimumElevationInMeters = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Municipality:
                        speciesObservation.Location.Municipality = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Parish:
                        speciesObservation.Location.Parish = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.PointRadiusSpatialFit:
                        speciesObservation.Location.PointRadiusSpatialFit = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.StateProvince:
                        speciesObservation.Location.StateProvince = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.VerbatimCoordinates:
                        speciesObservation.Location.VerbatimCoordinates = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.VerbatimCoordinateSystem:
                        speciesObservation.Location.VerbatimCoordinateSystem = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.VerbatimDepth:
                        speciesObservation.Location.VerbatimDepth = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.VerbatimElevation:
                        speciesObservation.Location.VerbatimElevation = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.VerbatimLatitude:
                        speciesObservation.Location.VerbatimLatitude = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.VerbatimLocality:
                        speciesObservation.Location.VerbatimLocality = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.VerbatimLongitude:
                        speciesObservation.Location.VerbatimLongitude = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.VerbatimSRS:
                        speciesObservation.Location.VerbatimSRS = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.WaterBody:
                        speciesObservation.Location.WaterBody = webSpeciesObservationField.Value;
                        break;
                }
            }
        }

        /// <summary>
        /// Add measurement or fact information from species observation field
        /// to a species observation instance. Only properties defined
        /// in interface ISpeciesObservationMeasurementOrFact are handled.
        /// </summary>
        /// <param name="speciesObservation">Species observation.</param>
        /// <param name="webSpeciesObservationField">Species observation field.</param>
        private void GetSpeciesObservationMeasurementOrFact(ISpeciesObservation speciesObservation,
                                                            WebSpeciesObservationField webSpeciesObservationField)
        {
            if (speciesObservation.MeasurementOrFact.IsNull())
            {
                speciesObservation.MeasurementOrFact = new SpeciesObservationMeasurementOrFact();
            }

            if (Enum.IsDefined(typeof(SpeciesObservationPropertyId), webSpeciesObservationField.PropertyIdentifier))
            {
                switch ((SpeciesObservationPropertyId)(Enum.Parse(typeof(SpeciesObservationPropertyId), webSpeciesObservationField.PropertyIdentifier)))
                {
                    case SpeciesObservationPropertyId.MeasurementAccuracy:
                        speciesObservation.MeasurementOrFact.MeasurementAccuracy = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.MeasurementDeterminedBy:
                        speciesObservation.MeasurementOrFact.MeasurementDeterminedBy = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.MeasurementDeterminedDate:
                        speciesObservation.MeasurementOrFact.MeasurementDeterminedDate =
                            webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.MeasurementID:
                        speciesObservation.MeasurementOrFact.MeasurementID = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.MeasurementMethod:
                        speciesObservation.MeasurementOrFact.MeasurementMethod = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.MeasurementRemarks:
                        speciesObservation.MeasurementOrFact.MeasurementRemarks = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.MeasurementType:
                        speciesObservation.MeasurementOrFact.MeasurementType = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.MeasurementUnit:
                        speciesObservation.MeasurementOrFact.MeasurementUnit = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.MeasurementValue:
                        speciesObservation.MeasurementOrFact.MeasurementValue = webSpeciesObservationField.Value;
                        break;
                }
            }
        }

        /// <summary>
        /// Add occurrence information from species observation field
        /// to a species observation instance. Only properties defined
        /// in interface ISpeciesObservationOccurrence are handled.
        /// </summary>
        /// <param name="speciesObservation">Species observation.</param>
        /// <param name="webSpeciesObservationField">Species observation field.</param>
        private void GetSpeciesObservationOccurrence(ISpeciesObservation speciesObservation,
                                                     WebSpeciesObservationField webSpeciesObservationField)
        {
            if (speciesObservation.Occurrence.IsNull())
            {
                speciesObservation.Occurrence = new SpeciesObservationOccurrence();
            }

            if (Enum.IsDefined(typeof(SpeciesObservationPropertyId), webSpeciesObservationField.PropertyIdentifier))
            {
                switch ((SpeciesObservationPropertyId)(Enum.Parse(typeof(SpeciesObservationPropertyId), webSpeciesObservationField.PropertyIdentifier)))
                {
                    case SpeciesObservationPropertyId.AssociatedMedia:
                        speciesObservation.Occurrence.AssociatedMedia = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.AssociatedOccurrences:
                        speciesObservation.Occurrence.AssociatedOccurrences = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.AssociatedReferences:
                        speciesObservation.Occurrence.AssociatedReferences = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.AssociatedSequences:
                        speciesObservation.Occurrence.AssociatedSequences = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.AssociatedTaxa:
                        speciesObservation.Occurrence.AssociatedTaxa = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Behavior:
                        speciesObservation.Occurrence.Behavior = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.CatalogNumber:
                        speciesObservation.Occurrence.CatalogNumber = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Disposition:
                        speciesObservation.Occurrence.Disposition = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.EstablishmentMeans:
                        speciesObservation.Occurrence.EstablishmentMeans = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.IndividualCount:
                        speciesObservation.Occurrence.IndividualCount = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.IndividualID:
                        speciesObservation.Occurrence.IndividualID = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.IsNaturalOccurrence:
                        speciesObservation.Occurrence.IsNaturalOccurrence =
                            webSpeciesObservationField.Value.WebParseBoolean();
                        break;
                    case SpeciesObservationPropertyId.IsNeverFoundObservation:
                        speciesObservation.Occurrence.IsNeverFoundObservation =
                            webSpeciesObservationField.Value.WebParseBoolean();
                        break;
                    case SpeciesObservationPropertyId.IsNotRediscoveredObservation:
                        speciesObservation.Occurrence.IsNotRediscoveredObservation =
                            webSpeciesObservationField.Value.WebParseBoolean();
                        break;
                    case SpeciesObservationPropertyId.IsPositiveObservation:
                        speciesObservation.Occurrence.IsPositiveObservation =
                            webSpeciesObservationField.Value.WebParseBoolean();
                        break;
                    case SpeciesObservationPropertyId.LifeStage:
                        speciesObservation.Occurrence.LifeStage = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.OccurrenceID:
                        speciesObservation.Occurrence.OccurrenceID = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.OccurrenceRemarks:
                        speciesObservation.Occurrence.OccurrenceRemarks = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.OccurrenceStatus:
                        speciesObservation.Occurrence.OccurrenceStatus = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.OccurrenceURL:
                        speciesObservation.Occurrence.OccurrenceURL = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.OtherCatalogNumbers:
                        speciesObservation.Occurrence.OtherCatalogNumbers = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Preparations:
                        speciesObservation.Occurrence.Preparations = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.PreviousIdentifications:
                        speciesObservation.Occurrence.PreviousIdentifications = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Quantity:
                        speciesObservation.Occurrence.Quantity = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.QuantityUnit:
                        speciesObservation.Occurrence.QuantityUnit = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.RecordedBy:
                        speciesObservation.Occurrence.RecordedBy = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.RecordNumber:
                        speciesObservation.Occurrence.RecordNumber = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.ReproductiveCondition:
                        speciesObservation.Occurrence.ReproductiveCondition = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Sex:
                        speciesObservation.Occurrence.Sex = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Substrate:
                        speciesObservation.Occurrence.Substrate = webSpeciesObservationField.Value;
                        break;
                }
            }
        }

        /// <summary>
        /// Add project information from species observation field
        /// to a species observation instance. Only properties defined
        /// in interface ISpeciesObservationProject are handled.
        /// </summary>
        /// <param name="speciesObservation">Species observation.</param>
        /// <param name="webSpeciesObservationField">Species observation field.</param>
        private void GetSpeciesObservationProject(ISpeciesObservation speciesObservation,
                                                  WebSpeciesObservationField webSpeciesObservationField)
        {
            ISpeciesObservationProjectParameter speciesObservationProjectParameter;
            List<WebDataField> dataFields;

            if (speciesObservation.Project.IsNull())
            {
                speciesObservation.Project = new SpeciesObservationProject();
            }

            if (Enum.IsDefined(typeof(SpeciesObservationPropertyId), webSpeciesObservationField.PropertyIdentifier))
            {
                switch ((SpeciesObservationPropertyId)(Enum.Parse(typeof(SpeciesObservationPropertyId), webSpeciesObservationField.PropertyIdentifier)))
                {
                    case SpeciesObservationPropertyId.IsPublic:
                        speciesObservation.Project.IsPublic = webSpeciesObservationField.Value.WebParseBoolean();
                        break;
                    case SpeciesObservationPropertyId.ProjectCategory:
                        speciesObservation.Project.ProjectCategory = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.ProjectDescription:
                        speciesObservation.Project.ProjectDescription = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.ProjectEndDate:
                        speciesObservation.Project.ProjectEndDate = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.ProjectID:
                        speciesObservation.Project.ProjectID = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.ProjectName:
                        speciesObservation.Project.ProjectName = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.ProjectOwner:
                        speciesObservation.Project.ProjectOwner = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.ProjectStartDate:
                        speciesObservation.Project.ProjectStartDate = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.ProjectURL:
                        speciesObservation.Project.ProjectURL = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.SurveyMethod:
                        speciesObservation.Project.SurveyMethod = webSpeciesObservationField.Value;
                        break;
                }
            }
            else
            {
                dataFields = webSpeciesObservationField.DataFields;
                if (dataFields.IsNotEmpty() &&
                    dataFields.IsDataFieldSpecified(Settings.Default.WebDataPropertyIdentifier) &&
                    dataFields.GetString(Settings.Default.WebDataPropertyIdentifier).Contains(Settings.Default.WebDataProjectParameter))
                {
                    if (speciesObservation.Project.ProjectParameters.IsNull())
                    {
                        speciesObservation.Project.ProjectParameters = new SpeciesObservationProjectParameterList();
                    }

                    speciesObservationProjectParameter = new SpeciesObservationProjectParameter();
                    if (dataFields.IsDataFieldSpecified(Settings.Default.WebDataGuid))
                    {
                        speciesObservationProjectParameter.Guid = dataFields.GetString(Settings.Default.WebDataGuid);
                    }

                    if (dataFields.IsDataFieldSpecified(Settings.Default.WebDataProjectId))
                    {
                        speciesObservationProjectParameter.ProjectId = dataFields.GetInt32(Settings.Default.WebDataProjectId);
                    }

                    if (dataFields.IsDataFieldSpecified(Settings.Default.WebDataProjectName))
                    {
                        speciesObservationProjectParameter.ProjectName = dataFields.GetString(Settings.Default.WebDataProjectName);
                    }

                    speciesObservationProjectParameter.Property = webSpeciesObservationField.PropertyIdentifier;
                    if (dataFields.IsDataFieldSpecified(Settings.Default.WebDataPropertyIdentifier))
                    {
                        speciesObservationProjectParameter.PropertyIdentifier = dataFields.GetString(Settings.Default.WebDataPropertyIdentifier);
                    }

                    speciesObservationProjectParameter.Type = GetDataType(webSpeciesObservationField.Type);
                    if (dataFields.IsDataFieldSpecified(Settings.Default.WebDataUnit))
                    {
                        speciesObservationProjectParameter.Unit = dataFields.GetString(Settings.Default.WebDataUnit);
                    }

                    speciesObservationProjectParameter.Value = webSpeciesObservationField.Value;
                    speciesObservation.Project.ProjectParameters.Add(speciesObservationProjectParameter);
                }
            }
        }

        /// <summary>
        /// Add resource relationship information from species observation field
        /// to a species observation instance. Only properties defined
        /// in interface ISpeciesObservationResourceRelationship are handled.
        /// </summary>
        /// <param name="speciesObservation">Species observation.</param>
        /// <param name="webSpeciesObservationField">Species observation field.</param>
        private void GetSpeciesObservationResourceRelationship(ISpeciesObservation speciesObservation,
                                                               WebSpeciesObservationField webSpeciesObservationField)
        {
            if (speciesObservation.ResourceRelationship.IsNull())
            {
                speciesObservation.ResourceRelationship = new SpeciesObservationResourceRelationship();
            }

            if (Enum.IsDefined(typeof(SpeciesObservationPropertyId), webSpeciesObservationField.PropertyIdentifier))
            {
                switch ((SpeciesObservationPropertyId)(Enum.Parse(typeof(SpeciesObservationPropertyId), webSpeciesObservationField.PropertyIdentifier)))
                {
                    case SpeciesObservationPropertyId.RelatedResourceID:
                        speciesObservation.ResourceRelationship.RelatedResourceID = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.RelationshipAccordingTo:
                        speciesObservation.ResourceRelationship.RelationshipAccordingTo =
                            webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.RelationshipEstablishedDate:
                        speciesObservation.ResourceRelationship.RelationshipEstablishedDate =
                            webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.RelationshipOfResource:
                        speciesObservation.ResourceRelationship.RelationshipOfResource =
                            webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.RelationshipRemarks:
                        speciesObservation.ResourceRelationship.RelationshipRemarks = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.ResourceID:
                        speciesObservation.ResourceRelationship.ResourceID = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.ResourceRelationshipID:
                        speciesObservation.ResourceRelationship.ResourceRelationshipID =
                            webSpeciesObservationField.Value;
                        break;
                }
            }
        }

        /// <summary>
        /// Get requested species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Species observations are returned in a format
        /// that is compatible with Darwin Core 1.5.
        /// Max 100000 observations with information
        /// can be retrieved in one call.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="speciesObservationIds">Id for species observations to get.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in returned species observations.
        /// </param>
        /// <param name="speciesObservationSpecification">
        /// Specify which subset of the data that should be
        /// returned for each species observation.
        /// All information for each species observation is
        /// returned if this parameter is null. 
        /// This parameter is currently not used.
        /// </param>
        /// <returns>Species observations.</returns>
        public virtual SpeciesObservationList GetSpeciesObservations(IUserContext userContext,
                                                                     List<Int64> speciesObservationIds,
                                                                     ICoordinateSystem coordinateSystem,
                                                                     ISpeciesObservationSpecification speciesObservationSpecification)
        {
            WebCoordinateSystem webCoordinateSystem;
            WebSpeciesObservationInformation webInformation;
            WebSpeciesObservationSpecification webSpeciesObservationSpecification;

            // Check arguments.
            coordinateSystem.CheckNotNull("coordinateSystem");
            speciesObservationIds.CheckNotEmpty("speciesObservationIds");

            // Get data from web service.
            webCoordinateSystem = GetCoordinateSystem(coordinateSystem);
            webSpeciesObservationSpecification = GetSpeciesObservationSpecification(speciesObservationSpecification);
            webInformation = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationsByIds(GetClientInformation(userContext),
                                                                                                          speciesObservationIds,
                                                                                                          webCoordinateSystem,
                                                                                                          webSpeciesObservationSpecification);
            return GetSpeciesObservations(userContext,
                                          webInformation,
                                          webCoordinateSystem,
                                          webSpeciesObservationSpecification);
        }

        /// <summary>
        /// Convert a WebSpeciesObservationInformation instance
        /// into a list of ISpeciesObservation instances.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webSpeciesObservationInformation">Web species observation information.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in returned species observations.
        /// </param>
        /// <param name="speciesObservationSpecification">
        /// Specify which subset of the data that should be
        /// returned for each species observation.
        /// All information for each species observation is
        /// returned if this parameter is null.
        /// This parameter is currently not used.
        /// </param>
        /// <returns>A list of ISpeciesObservation instances.</returns>
        private SpeciesObservationList GetSpeciesObservations(IUserContext userContext,
                                                              WebSpeciesObservationInformation webSpeciesObservationInformation,
                                                              WebCoordinateSystem coordinateSystem,
                                                              WebSpeciesObservationSpecification speciesObservationSpecification)
        {
            Int32 index, speciesObservationIdsIndex;
            List<Int64> speciesObservationIds;
            SpeciesObservationList speciesObservations;
            WebSpeciesObservationInformation webInformation;

            if (webSpeciesObservationInformation.SpeciesObservations.IsEmpty() &&
                webSpeciesObservationInformation.SpeciesObservationIds.IsNotEmpty())
            {
                // Get species observations in parts.
                speciesObservations = new SpeciesObservationList();
                speciesObservationIds = new List<Int64>();
                index = 0;
                for (speciesObservationIdsIndex = 0; speciesObservationIdsIndex < webSpeciesObservationInformation.SpeciesObservationIds.Count; speciesObservationIdsIndex++)
                {
                    speciesObservationIds.Add(webSpeciesObservationInformation.SpeciesObservationIds[speciesObservationIdsIndex]);
                    if (++index == webSpeciesObservationInformation.MaxSpeciesObservationCount)
                    {
                        // Get one part of species observations.
                        webInformation = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationsByIds(GetClientInformation(userContext),
                                                                                                                      speciesObservationIds,
                                                                                                                      coordinateSystem,
                                                                                                                      speciesObservationSpecification);
                        speciesObservations.AddRange(GetSpeciesObservations(userContext,
                                                                            webInformation.SpeciesObservations));
                        index = 0;
                        speciesObservationIds = new List<Int64>();
                    }
                }

                if (speciesObservationIds.IsNotEmpty())
                {
                    webInformation = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationsByIds(GetClientInformation(userContext),
                                                                                                                  speciesObservationIds,
                                                                                                                  coordinateSystem,
                                                                                                                  speciesObservationSpecification);
                    speciesObservations.AddRange(GetSpeciesObservations(userContext,
                                                                        webInformation.SpeciesObservations));
                }
            }
            else
            {
                speciesObservations = GetSpeciesObservations(userContext,
                                                             webSpeciesObservationInformation.SpeciesObservations);
            }

            return speciesObservations;
        }

        /// <summary>
        /// Translates a Web Species Observation Field Description to a Species Observation Field Description object.
        /// </summary>
        /// <param name="userContext">User Context.</param>
        /// <param name="webSpeciesObservationFieldDescription">A Web Species Observation Field Description.</param>
        /// <returns>A Species Observation Field Description.</returns>
        private ISpeciesObservationFieldDescription GetSpeciesObservationFieldDescription(IUserContext userContext,
                                                WebSpeciesObservationFieldDescription webSpeciesObservationFieldDescription)
        {
            ISpeciesObservationFieldDescription fieldDescription;

            fieldDescription = new SpeciesObservationFieldDescription();
            fieldDescription.Class = GetSpeciesObservationClass(userContext,
                                                                webSpeciesObservationFieldDescription.Class);
            fieldDescription.DataContext = GetDataContext(userContext);
            fieldDescription.Definition = webSpeciesObservationFieldDescription.Definition;
            fieldDescription.DefinitionUrl = webSpeciesObservationFieldDescription.DefinitionUrl;
            fieldDescription.Documentation = webSpeciesObservationFieldDescription.Documentation;
            fieldDescription.DocumentationUrl = webSpeciesObservationFieldDescription.DocumentationUrl;
            fieldDescription.Guid = webSpeciesObservationFieldDescription.Guid;
            fieldDescription.Id = webSpeciesObservationFieldDescription.Id;
            fieldDescription.Importance = webSpeciesObservationFieldDescription.Importance;
            fieldDescription.IsAcceptedByTdwg = webSpeciesObservationFieldDescription.IsAcceptedByTdwg;
            fieldDescription.IsClass = webSpeciesObservationFieldDescription.IsClass;
            fieldDescription.IsImplemented = webSpeciesObservationFieldDescription.IsImplemented;
            fieldDescription.IsMandatory = webSpeciesObservationFieldDescription.IsMandatory;
            fieldDescription.IsMandatoryFromProvider = webSpeciesObservationFieldDescription.IsMandatoryFromProvider;
            fieldDescription.IsObtainedFromProvider = webSpeciesObservationFieldDescription.IsObtainedFromProvider;
            fieldDescription.IsPlanned = webSpeciesObservationFieldDescription.IsPlanned;
            fieldDescription.IsSearchable = webSpeciesObservationFieldDescription.IsSearchable;
            fieldDescription.IsSortable = webSpeciesObservationFieldDescription.IsSortable;
            fieldDescription.Label = webSpeciesObservationFieldDescription.Label;
            fieldDescription.Name = webSpeciesObservationFieldDescription.Name;
            fieldDescription.Mappings = new SpeciesObservationFieldMappingList();
            if (webSpeciesObservationFieldDescription.Mappings.IsNotEmpty())
            {
                foreach (WebSpeciesObservationFieldMapping webMapping in webSpeciesObservationFieldDescription.Mappings)
                {
                    fieldDescription.Mappings.Add(GetSpeciesObservationFieldMapping(userContext, webMapping));
                }
            }

            fieldDescription.Property = GetSpeciesObservationProperty(userContext,
                                                                      webSpeciesObservationFieldDescription.Property);
            fieldDescription.Remarks = webSpeciesObservationFieldDescription.Remarks;
            fieldDescription.SortOrder = webSpeciesObservationFieldDescription.SortOrder;
            fieldDescription.Type = (DataType)webSpeciesObservationFieldDescription.Type;
            fieldDescription.Uuid = webSpeciesObservationFieldDescription.Uuid;
            return fieldDescription;
        }

        /// <summary>
        /// Retrieve all Species Observation Field Descriptions.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>A Species Observation Field Description List.</returns>
        public virtual SpeciesObservationFieldDescriptionList GetSpeciesObservationFieldDescriptions(IUserContext userContext)
        {
            List<WebSpeciesObservationFieldDescription> webFieldDescriptions;

            webFieldDescriptions = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationFieldDescriptions(GetClientInformation(userContext));
            return GetSpeciesObservationFieldDescriptions(userContext, webFieldDescriptions);
        }

        /// <summary>
        /// Translates a list of Web Species Observation Field Description objects to a Species Observation Field Description List.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webFieldDescriptions">List with Web Species Observation Field Description objects.</param>
        /// <returns>A Species Observation Field Description List.</returns>
        private SpeciesObservationFieldDescriptionList GetSpeciesObservationFieldDescriptions(IUserContext userContext,
                                                                                              List<WebSpeciesObservationFieldDescription> webFieldDescriptions)
        {
            SpeciesObservationFieldDescriptionList fieldDescriptions;

            fieldDescriptions = new SpeciesObservationFieldDescriptionList(true);
            if (webFieldDescriptions.IsNotEmpty())
            {
                foreach (WebSpeciesObservationFieldDescription webFieldDescription in webFieldDescriptions)
                {
                    fieldDescriptions.Add(GetSpeciesObservationFieldDescription(userContext, webFieldDescription));
                }
            }

            return fieldDescriptions;
        }

        /// <summary>
        /// Translates a Web Species Observation Field Mapping to a Species Observation Field Mapping object.
        /// </summary>
        /// <param name="userContext">User Context.</param>
        /// <param name="webSpeciesObservationFieldMapping">A Web Species Observation Field Mapping.</param>
        /// <returns>A Species Observation Field Mapping.</returns>
        private ISpeciesObservationFieldMapping GetSpeciesObservationFieldMapping(IUserContext userContext,
                                                WebSpeciesObservationFieldMapping webSpeciesObservationFieldMapping)
        {
            ISpeciesObservationFieldMapping fieldMapping;
            fieldMapping = new SpeciesObservationFieldMapping();
            fieldMapping.DataContext = GetDataContext(userContext);
            fieldMapping.DataProviderId = webSpeciesObservationFieldMapping.DataProviderId;
            fieldMapping.DefaultValue = webSpeciesObservationFieldMapping.DefaultValue;
            fieldMapping.Documentation = webSpeciesObservationFieldMapping.Documentation;
            fieldMapping.FieldId = webSpeciesObservationFieldMapping.FieldId;
            fieldMapping.GUID = webSpeciesObservationFieldMapping.GetGUID();
            fieldMapping.Id = webSpeciesObservationFieldMapping.Id;
            fieldMapping.IsImplemented = webSpeciesObservationFieldMapping.IsImplemented;
            fieldMapping.IsPlanned = webSpeciesObservationFieldMapping.IsPlanned;
            fieldMapping.Method = webSpeciesObservationFieldMapping.Method;
            fieldMapping.PropertyIdentifier = webSpeciesObservationFieldMapping.GetPropertyIdentifier();
            fieldMapping.ProjectId = webSpeciesObservationFieldMapping.GetProjectId();
            fieldMapping.ProjectName = webSpeciesObservationFieldMapping.GetProjectName();
            fieldMapping.ProviderFieldName = webSpeciesObservationFieldMapping.ProviderFieldName;
            return fieldMapping;
        }

        /// <summary>
        /// Convert a ISpeciesObservationFieldSpecification instance
        /// to a WebSpeciesObservationFieldSpecification instance.
        /// </summary>
        /// <param name="speciesObservationFieldSpecification">A ISpeciesObservationFieldSpecification instance.</param>
        /// <returns>A WebSpeciesObservationFieldSpecification instance.</returns>
        private WebSpeciesObservationFieldSpecification GetSpeciesObservationFieldSpecification(ISpeciesObservationFieldSpecification speciesObservationFieldSpecification)
        {
            WebSpeciesObservationFieldSpecification webSpeciesObservationFieldSpecification;

            webSpeciesObservationFieldSpecification = new WebSpeciesObservationFieldSpecification();
            webSpeciesObservationFieldSpecification.Class = GetSpeciesObservationClass(speciesObservationFieldSpecification.Class);
            webSpeciesObservationFieldSpecification.IsClassIndexSpecified = false;
            webSpeciesObservationFieldSpecification.IsPropertyIndexSpecified = false;
            webSpeciesObservationFieldSpecification.Property = GetSpeciesObservationProperty(speciesObservationFieldSpecification.Property);

            return webSpeciesObservationFieldSpecification;
        }

        /// <summary>
        /// Convert a list of ISpeciesObservationFieldSpecification
        /// instances to a list of WebSpeciesObservationFieldSpecification.
        /// </summary>
        /// <param name="speciesObservationFieldSpecifications">List of ISpeciesObservationFieldSpecification instances.</param>
        /// <returns>Species observation field specifications.</returns>
        private List<WebSpeciesObservationFieldSpecification> GetSpeciesObservationFieldSpecifications(SpeciesObservationFieldSpecificationList speciesObservationFieldSpecifications)
        {
            List<WebSpeciesObservationFieldSpecification> webSpeciesObservationFieldSpecifications;

            webSpeciesObservationFieldSpecifications = null;
            if (speciesObservationFieldSpecifications.IsNotEmpty())
            {
                webSpeciesObservationFieldSpecifications = new List<WebSpeciesObservationFieldSpecification>();
                foreach (ISpeciesObservationFieldSpecification speciesObservationFieldSpecification in speciesObservationFieldSpecifications)
                {
                    webSpeciesObservationFieldSpecifications.Add(GetSpeciesObservationFieldSpecification(speciesObservationFieldSpecification));
                }
            }

            return webSpeciesObservationFieldSpecifications;
        }

        /// <summary>
        /// Convert a WebSpeciesObservationProperty instance
        /// to a ISpeciesObservationProperty instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webSpeciesObservationProperty">A WebSpeciesObservationProperty object.</param>
        /// <returns>An ISpeciesObservationProperty instance.</returns>
        private ISpeciesObservationProperty GetSpeciesObservationProperty(IUserContext userContext,
                                                                          WebSpeciesObservationProperty webSpeciesObservationProperty)
        {
            ISpeciesObservationProperty speciesObservationProperty;

            speciesObservationProperty = null;
            if (webSpeciesObservationProperty.IsNotNull())
            {
                speciesObservationProperty = new SpeciesObservationProperty();
                speciesObservationProperty.DataContext = GetDataContext(userContext);
                speciesObservationProperty.Id = webSpeciesObservationProperty.Id;
                speciesObservationProperty.Identifier = webSpeciesObservationProperty.Identifier;
            }

            return speciesObservationProperty;
        }

        /// <summary>
        /// Convert a WebSpeciesObservationProperty instance
        /// to a ISpeciesObservationProperty instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="speciesObservationPropertyIdentifier">Identifier for a species observation property.</param>
        /// <returns>An ISpeciesObservationProperty instance.</returns>
        private ISpeciesObservationProperty GetSpeciesObservationProperty(IUserContext userContext,
                                                                          String speciesObservationPropertyIdentifier)
        {
            ISpeciesObservationProperty speciesObservationProperty;

            speciesObservationProperty = new SpeciesObservationProperty();
            speciesObservationProperty.DataContext = GetDataContext(userContext);
            if (Enum.IsDefined(typeof(SpeciesObservationPropertyId), speciesObservationPropertyIdentifier))
            {
                speciesObservationProperty.Id = (SpeciesObservationPropertyId)(Enum.Parse(typeof(SpeciesObservationPropertyId), speciesObservationPropertyIdentifier));
            }
            else
            {
                speciesObservationProperty.Id = SpeciesObservationPropertyId.None;
            }

            speciesObservationProperty.Identifier = speciesObservationPropertyIdentifier;

            return speciesObservationProperty;
        }

        /// <summary>
        /// Convert a ISpeciesObservationProperty instance
        /// to a WebSpeciesObservationProperty instance.
        /// </summary>
        /// <param name="speciesObservationProperty">A ISpeciesObservationProperty instance.</param>
        /// <returns>A WebSpeciesObservationProperty instance.</returns>
        private WebSpeciesObservationProperty GetSpeciesObservationProperty(ISpeciesObservationProperty speciesObservationProperty)
        {
            WebSpeciesObservationProperty webSpeciesObservationProperty;

            webSpeciesObservationProperty = new WebSpeciesObservationProperty();
            webSpeciesObservationProperty.Id = speciesObservationProperty.Id;
            webSpeciesObservationProperty.Identifier = speciesObservationProperty.Identifier;

            return webSpeciesObservationProperty;
        }

        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// This method provides paging functionality of the result.
        /// Max page size is 10000 species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in geometry search criteria
        /// and returned species observations.
        /// </param>
        /// <param name="pageSpecification">
        /// SpecificationId of paging information when
        /// species observations are retrieved.
        /// </param>
        /// <param name="speciesObservationSpecification">
        /// Specify which subset of the data that should be
        /// returned for each species observation.
        /// All information for each species observation is
        /// returned if this parameter is null. 
        /// This parameter is currently not used.
        /// </param>
        /// <returns>Information about requested species observations.</returns>
        public virtual SpeciesObservationList GetSpeciesObservations(IUserContext userContext,
                                                                     ISpeciesObservationSearchCriteria searchCriteria,
                                                                     ICoordinateSystem coordinateSystem,
                                                                     ISpeciesObservationPageSpecification pageSpecification,
                                                                     ISpeciesObservationSpecification speciesObservationSpecification)
        {
            List<WebSpeciesObservation> webSpeciesObservations;

            webSpeciesObservations = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationsBySearchCriteriaPage(GetClientInformation(userContext),
                                                                                                                                 GetSpeciesObservationSearchCriteria(searchCriteria),
                                                                                                                                 GetCoordinateSystem(coordinateSystem),
                                                                                                                                 GetSpeciesObservationPageSpecification(pageSpecification),
                                                                                                                                 GetSpeciesObservationSpecification(speciesObservationSpecification));
            return GetSpeciesObservations(userContext, webSpeciesObservations);
        }

        /// <summary>
        /// Convert a list of WebSpeciesObservation
        /// instances to a list of ISpeciesObservation instances.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webSpeciesObservations">List of WebSpeciesObservation instances.</param>
        /// <returns>A list of ISpeciesObservation instances.</returns>
        private SpeciesObservationList GetSpeciesObservations(IUserContext userContext,
                                                              List<WebSpeciesObservation> webSpeciesObservations)
        {
            SpeciesObservationList speciesObservations = new SpeciesObservationList();
            
            if (webSpeciesObservations.IsNotEmpty())
            {
                foreach (WebSpeciesObservation webSpeciesObservation in webSpeciesObservations)
                {
                    speciesObservations.Add(GetSpeciesObservation(userContext, webSpeciesObservation));
                }
            }

            return speciesObservations;
        }

        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// Max 100000 observations with information
        /// can be retrieved in one call.
        /// Max 1000000 observation ids can be retrieved in one call.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria
        /// and returned species observations. </param>
        /// <param name="speciesObservationSpecification">
        /// Specify which subset of the data that should be
        /// returned for each species observation.
        /// All information for each species observation is
        /// returned if this parameter is null. 
        /// </param>
        /// <param name="sortOrder">
        /// Defines how species observations should be sorted.
        /// This parameter is optional. Random order is used
        /// if no sort order has been specified.
        /// This parameter is currently not used.
        /// </param>
        /// <returns>Information about requested species observations.</returns>
        public virtual SpeciesObservationList GetSpeciesObservations(IUserContext userContext,
                                                                     ISpeciesObservationSearchCriteria searchCriteria,
                                                                     ICoordinateSystem coordinateSystem,
                                                                     ISpeciesObservationSpecification speciesObservationSpecification,
                                                                     SpeciesObservationFieldSortOrderList sortOrder)
        {
            WebCoordinateSystem webCoordinateSystem;
            WebSpeciesObservationInformation webInformation;
            WebSpeciesObservationSpecification webSpeciesObservationSpecification;

            // Get data from web service.
            webCoordinateSystem = GetCoordinateSystem(coordinateSystem);
            webSpeciesObservationSpecification = GetSpeciesObservationSpecification(speciesObservationSpecification);
            webInformation = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationsBySearchCriteria(GetClientInformation(userContext),
                                                                                                                     GetSpeciesObservationSearchCriteria(searchCriteria),
                                                                                                                     webCoordinateSystem,
                                                                                                                     webSpeciesObservationSpecification,
                                                                                                                     GetSpeciesObservationSortOrder(sortOrder));
            return GetSpeciesObservations(userContext,
                                          webInformation,
                                          webCoordinateSystem,
                                          webSpeciesObservationSpecification);
        }

        /// <summary>
        /// Get number of species observations that matches the search criteria.
        /// Scope is restricted to those observations that the user has access rights to.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.</param>
        /// <returns>Number of species observations.</returns>
        public Int64 GetSpeciesObservationCount(IUserContext userContext,
                                                ISpeciesObservationSearchCriteria searchCriteria,
                                                ICoordinateSystem coordinateSystem)
        {
            WebSpeciesObservationSearchCriteria webSpeciesObservationSearchCriteria;
            WebCoordinateSystem webCoordinateSystem;

            // Check arguments
            searchCriteria.CheckNotNull("searchCriteria");

            webCoordinateSystem = GetCoordinateSystem(coordinateSystem);

            // Convert incoming search criteria format to web search criteria format. 
            webSpeciesObservationSearchCriteria = GetSpeciesObservationSearchCriteria(searchCriteria);

            Int64 noOfObservations = WebServiceProxy.SwedishSpeciesObservationService.GetSpeciesObservationCountBySearchCriteria(GetClientInformation(userContext), webSpeciesObservationSearchCriteria, webCoordinateSystem);

            return noOfObservations;
        }

        /// <summary>
        /// Convert a ISpeciesObservationSpecification instance
        /// to a WebSpeciesObservationSpecification instance.
        /// </summary>
        /// <param name="speciesObservationSpecification">A ISpeciesObservationSpecification object.</param>
        /// <returns>A WebSpeciesObservationSpecification instance.</returns>
        private WebSpeciesObservationSpecification GetSpeciesObservationSpecification(ISpeciesObservationSpecification speciesObservationSpecification)
        {
            WebSpeciesObservationSpecification webSpeciesObservationSpecification;

            webSpeciesObservationSpecification = null;
            if (speciesObservationSpecification.IsNotNull())
            {
                webSpeciesObservationSpecification = new WebSpeciesObservationSpecification();
                webSpeciesObservationSpecification.Fields = GetSpeciesObservationFieldSpecifications(speciesObservationSpecification.Fields);
                webSpeciesObservationSpecification.Specification = speciesObservationSpecification.Specification;
            }

            return webSpeciesObservationSpecification;
        }

        /// <summary>
        /// Add taxon information from species observation field
        /// to a species observation instance. Only properties defined
        /// in interface ISpeciesObservationTaxon are handled.
        /// </summary>
        /// <param name="speciesObservation">Species observation.</param>
        /// <param name="webSpeciesObservationField">Species observation field.</param>
        private void GetSpeciesObservationTaxon(ISpeciesObservation speciesObservation,
                                                WebSpeciesObservationField webSpeciesObservationField)
        {
            if (speciesObservation.Taxon.IsNull())
            {
                speciesObservation.Taxon = new SpeciesObservationTaxon();
            }

            if (Enum.IsDefined(typeof(SpeciesObservationPropertyId), webSpeciesObservationField.PropertyIdentifier))
            {
                switch ((SpeciesObservationPropertyId)(Enum.Parse(typeof(SpeciesObservationPropertyId), webSpeciesObservationField.PropertyIdentifier)))
                {
                    case SpeciesObservationPropertyId.AcceptedNameUsage:
                        speciesObservation.Taxon.AcceptedNameUsage = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.AcceptedNameUsageID:
                        speciesObservation.Taxon.AcceptedNameUsageID = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Class:
                        speciesObservation.Taxon.Class = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.DyntaxaTaxonID:
                        speciesObservation.Taxon.DyntaxaTaxonID = webSpeciesObservationField.Value.WebParseInt32();
                        break;
                    case SpeciesObservationPropertyId.Family:
                        speciesObservation.Taxon.Family = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Genus:
                        speciesObservation.Taxon.Genus = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.HigherClassification:
                        speciesObservation.Taxon.HigherClassification = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.InfraspecificEpithet:
                        speciesObservation.Taxon.InfraspecificEpithet = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Kingdom:
                        speciesObservation.Taxon.Kingdom = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.NameAccordingTo:
                        speciesObservation.Taxon.NameAccordingTo = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.NameAccordingToID:
                        speciesObservation.Taxon.NameAccordingToID = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.NamePublishedIn:
                        speciesObservation.Taxon.NamePublishedIn = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.NamePublishedInID:
                        speciesObservation.Taxon.NamePublishedInID = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.NamePublishedInYear:
                        speciesObservation.Taxon.NamePublishedInYear = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.NomenclaturalCode:
                        speciesObservation.Taxon.NomenclaturalCode = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.NomenclaturalStatus:
                        speciesObservation.Taxon.NomenclaturalStatus = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Order:
                        speciesObservation.Taxon.Order = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.OrganismGroup:
                        speciesObservation.Taxon.OrganismGroup = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.OriginalNameUsage:
                        speciesObservation.Taxon.OriginalNameUsage = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.OriginalNameUsageID:
                        speciesObservation.Taxon.OriginalNameUsageID = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.ParentNameUsage:
                        speciesObservation.Taxon.ParentNameUsage = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.ParentNameUsageID:
                        speciesObservation.Taxon.ParentNameUsageID = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Phylum:
                        speciesObservation.Taxon.Phylum = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.ScientificName:
                        speciesObservation.Taxon.ScientificName = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.ScientificNameAuthorship:
                        speciesObservation.Taxon.ScientificNameAuthorship = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.ScientificNameID:
                        speciesObservation.Taxon.ScientificNameID = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.SpecificEpithet:
                        speciesObservation.Taxon.SpecificEpithet = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.Subgenus:
                        speciesObservation.Taxon.Subgenus = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.TaxonConceptID:
                        speciesObservation.Taxon.TaxonConceptID = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.TaxonConceptStatus:
                        speciesObservation.Taxon.TaxonConceptStatus = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.TaxonID:
                        speciesObservation.Taxon.TaxonID = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.TaxonomicStatus:
                        speciesObservation.Taxon.TaxonomicStatus = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.TaxonRank:
                        speciesObservation.Taxon.TaxonRank = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.TaxonRemarks:
                        speciesObservation.Taxon.TaxonRemarks = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.TaxonSortOrder:
                        speciesObservation.Taxon.TaxonSortOrder = webSpeciesObservationField.Value.WebParseInt32();
                        break;
                    case SpeciesObservationPropertyId.TaxonURL:
                        speciesObservation.Taxon.TaxonURL = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.VerbatimScientificName:
                        speciesObservation.Taxon.VerbatimScientificName = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.VerbatimTaxonRank:
                        speciesObservation.Taxon.VerbatimTaxonRank = webSpeciesObservationField.Value;
                        break;
                    case SpeciesObservationPropertyId.VernacularName:
                        speciesObservation.Taxon.VernacularName = webSpeciesObservationField.Value;
                        break;
                }
            }
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="userName">User name.</param>
        /// <param name="password">The password.</param>
        /// <param name="applicationIdentifier">
        /// Application identifier.
        /// User authorities for this application is included in
        /// the user context.
        /// </param>
        /// <param name="isActivationRequired">
        /// Flag that indicates if user must be activated
        /// for login to succeed.
        /// </param>
        private void Login(IUserContext userContext,
                           String userName,
                           String password,
                           String applicationIdentifier,
                           Boolean isActivationRequired)
        {
            WebLoginResponse loginResponse;

            loginResponse = WebServiceProxy.SwedishSpeciesObservationService.Login(userName,
                                                               password,
                                                               applicationIdentifier,
                                                               isActivationRequired);
            if (loginResponse.IsNotNull())
            {
                SetToken(userContext, loginResponse.Token);
            }
        }

        /// <summary>
        /// Logout user.
        /// </summary>
        /// <param name="userContext">User context.</param>
        private void Logout(IUserContext userContext)
        {
            WebServiceProxy.SwedishSpeciesObservationService.Logout(GetClientInformation(userContext));
            SetToken(userContext, null);
        }

        /// <summary>
        /// Set web service SwedishSpeciesObservationService as
        /// data source in the onion data model.
        /// </summary>
        public static void SetDataSource()
        {
            SpeciesObservationDataSource speciesObservationDataSource;

            speciesObservationDataSource = new SpeciesObservationDataSource();
            CoreData.SpeciesObservationManager.DataSource = speciesObservationDataSource;
            CoreData.MetadataManager.SpeciesObservationDataSource = speciesObservationDataSource;
            ((UserDataSource)(CoreData.UserManager.DataSource)).UserLoggedInEvent += speciesObservationDataSource.Login;
            ((UserDataSource)(CoreData.UserManager.DataSource)).UserLoggedOutEvent += speciesObservationDataSource.Logout;
        }
    }
}

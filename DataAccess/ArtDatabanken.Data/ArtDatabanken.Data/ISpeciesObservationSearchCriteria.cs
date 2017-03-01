using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface holds parameters used to
    /// search for species observations.
    /// </summary>
    public interface ISpeciesObservationSearchCriteria
    {
        /// <summary>
        /// Requested minimum accuracy of the coordinates.
        /// Species observations with bad accurray (ie higher accuracy value )
        /// will not be included in the search result.
        /// Unit is meters.
        /// </summary>
        Double? Accuracy { get; set; }

        /// <summary>
        /// Limit returned observations based on bird nest activity level.
        /// Only bird observations in Artportalen are affected
        /// by this search criteria.
        /// Only bird observations with the specified bird nest
        /// activity level or stronger may be returned.
        /// Observation of other organism groups (not birds) are
        /// not affected by this search criteria. 
        /// Use method GetBirdNestActivities() in SpeciesObservationManager
        /// to retrieve currently used bird nest activities.
        /// Property BirdNestActivityLimit should be set to 
        /// one of the bird nest activities.
        /// </summary>
        ISpeciesActivity BirdNestActivityLimit { get; set; }

        /// <summary>
        /// Limit returned observations to specified bounding box.
        /// Only two-dimensional bounding boxes (rectangles) are handled
        /// in SwedishSpeciesObservationService.
        /// </summary>
        IBoundingBox BoundingBox { get; set; }

        /// <summary>
        /// Search observations based on change date and time.
        /// </summary>
        IDateTimeSearchCriteria ChangeDateTime { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// List of data sources to use in the search.
        /// All data sources are used if no specific
        /// data sources are provided.
        /// </summary>
        List<String> DataSourceGuids { get; set; }

        /// <summary>
        /// Limit search based on values for any data that is
        /// related to species observations.
        /// </summary>
        SpeciesObservationFieldSearchCriteriaList FieldSearchCriteria { get; set; }

        /// <summary>
        /// Specify how field search criteria
        /// should be logically combined when species
        /// observations are searched.
        /// </summary>
        LogicalOperator FieldLogicalOperator { get; set; }

        /// <summary>
        /// This property indicates whether to search
        /// for never found observations.
        /// "Never found observations" is an observation that says
        /// that the specified species was not found in a location
        /// deemed appropriate for the species.
        /// </summary>
        Boolean IncludeNeverFoundObservations { get; set; }

        /// <summary>
        /// This property indicates whether to search
        /// for not rediscovered observations.
        /// "Not rediscovered observations" is an observation that says
        /// that the specified species was not found in a location
        /// where it has previously been observed.
        /// </summary>
        Boolean IncludeNotRediscoveredObservations { get; set; }

        /// <summary>
        /// This property indicates whether to search
        /// for positive observations.
        /// "Positive observations" are normal observations indicating
        /// that a species has been seen at a specified location.
        /// </summary>
        Boolean IncludePositiveObservations { get; set; }

        /// <summary>
        /// Search observations based on taxa that is
        /// readlisted in any of the specified red list categories.
        /// If property IncludeRedListCategories is not empty
        /// all taxa that is redlisted in any of the specified
        /// red list categories are added to property TaxonIds
        /// by the web service.
        /// </summary>
        List<RedListCategory> IncludeRedListCategories { get; set; }

        /// <summary>
        /// Search observations based on readlisted taxa.
        /// If property IncludeRedlistedTaxa is set to true
        /// all redlisted taxa is added to property TaxonIds
        /// by the web service.
        /// </summary>
        Boolean IncludeRedlistedTaxa { get; set; }

        /// <summary>
        /// Indicates if species observations that are outside
        /// geography area (bounding box, polygons or regions)
        /// but close enough when accuracy of observation are
        /// considered should be included in the result.
        /// </summary>
        Boolean IsAccuracyConsidered { get; set; }

        /// <summary>
        /// Indicates if species observations that are outside
        /// geography area (bounding box, polygons or regions) but
        /// close enough when disturbance sensitivity of species are
        /// considered should be included in the result.
        /// </summary>
        Boolean IsDisturbanceSensitivityConsidered { get; set; }

        /// <summary>
        /// Restrict search based on if a positive observation
        /// is natural or not.
        /// Property IsIsNaturalOccurrenceSpecified indicates if
        /// property IsNaturalOccurrence should be used or not.
        /// </summary>
        Boolean? IsNaturalOccurrence { get; set; }

        /// <summary>
        /// String search criteria to match with locality names.
        /// All localities are used if
        /// LocalityNameSearchString is empty.
        /// </summary>
        IStringSearchCriteria LocalityNameSearchString { get; set; }

        /// <summary>
        /// Only observations that has a protection level that
        /// is equal to or lower than the value of this property
        /// are included in the result.
        /// 1 is the lowest possible value.
        /// </summary>
        Int32? MaxProtectionLevel { get; set; }

        /// <summary>
        /// Only observations that has a protection level that
        /// is equal to or higher than the value of this property
        /// are included in the result.
        /// 1 is the lowest possible value.
        /// </summary>
        Int32? MinProtectionLevel { get; set; }

        /// <summary>
        /// Search observations based on observation date and time.
        /// If ObservationDateTime.PartOfYear has a DateTimeInterval
        /// value then the first value in PartOfYear is used and
        /// search is limited to part of year with no limit on years.
        /// If ObservationDateTime.PartOfYear is empty then 
        /// ObservationDateTime.Begin and ObservationDateTime.End
        /// is used to limit species observation search.
        /// </summary>
        IDateTimeSearchCriteria ObservationDateTime { get; set; }

        /// <summary>
        /// Search observations based on observers.
        /// Observer id corresponds to person id in the
        /// user service.
        /// Observations that are not related to persons
        /// defined in user service will not match this
        /// search criteria.
        /// This property is not used in the current version of
        /// SwedishSpeciesObservationService.
        /// </summary>
        List<Int32> ObserverIds { get; set; }

        /// <summary>
        /// String search criteria to match with observer names.
        /// </summary>
        IStringSearchCriteria ObserverSearchString { get; set; }

        /// <summary>
        /// Search observations that are inside specified polygons.
        /// </summary>
        List<IPolygon> Polygons { get; set; }

        /// <summary>
        /// Get observations related to specified projects
        /// (projects corresponds to "syfte" in Artportalen 1).
        /// This property is not used in the current version of
        /// SwedishSpeciesObservationService.
        /// </summary>
        List<String> ProjectGuids { get; set; }

        /// <summary>
        /// Limit search to specified regions.
        /// Regions are defined by GeoReferenceService.
        /// </summary>
        List<String> RegionGuids { get; set; }

        /// <summary>
        /// Specify how regions, polygons or bounding box
        /// should be logically combined when species
        /// observations are searched.
        /// The logical operator OR are used between spatial geometries.
        /// This property is not used in the current version of
        /// SwedishSpeciesObservationService.
        /// </summary>
        LogicalOperator RegionLogicalOperator { get; set; }

        /// <summary>
        /// Specifies how to search when having county or province 
        /// Regions in the search criteria. ByCoordinate is the 
        /// default type and uses the precalculated Id's of the 
        /// observation, the Id is calculated using coordinates.
        /// ByName works in the same way as ByCoordinates but the 
        /// Id's are calculated by matching of region names (primary), 
        /// secondary by coordinates.
        /// </summary>
        CountyProvinceRegionSearchType CountyProvinceRegionSearchType { get; set; }

        /// <summary>
        /// Search observations based on reported date and time.
        /// Not all functionality in WebDateTimeSearchCriteria
        /// are implemented.
        /// Current restrictions:
        /// Only Days are handled if Accuracy has been specified.
        /// </summary>
        IDateTimeSearchCriteria ReportedDateTime { get; set; }

        /// <summary>
        /// Limit returned observations based on species activities.
        /// Use method GetSpeciesActivities() in web service
        /// SwedishSpeciesObservationService to retrieve
        /// currently used species activities.
        /// This property is currently not used.
        /// </summary>
        List<Int32> SpeciesActivityIds { get; set; }

        /// <summary>
        /// List of taxa to use in the search.
        /// Ids for taxon are defined in Dyntaxa.
        /// </summary>
        List<Int32> TaxonIds { get; set; }

        /// <summary>
        /// Return only observations with specified validation status.
        /// This property is not used in the current version of
        /// SwedishSpeciesObservationService.
        /// </summary>
        List<Int32> ValidationStatusIds { get; set; }

        /// <summary>
        /// Add region GUID to search criteria.
        /// </summary>
        /// <param name="regionGuid">Region GUID.</param>
        void AddRegionGuid(String regionGuid);

        /// <summary>
        /// Add taxon id to search criteria.
        /// </summary>
        /// <param name="taxonId">Taxon id.</param>
        void AddTaxon(Int32 taxonId);

        /// <summary>
        /// Set observation date time search criteria.
        /// </summary>
        /// <param name="begin">Start of observation.</param>
        /// <param name="end">End of observation.</param>
        /// <param name="compareOperator">
        /// How species observations should be compared with the search criteria.
        /// This value should be either Excluding or Including.
        /// </param>
        void SetObservationDataTime(DateTime begin,
                                    DateTime end,
                                    CompareOperator compareOperator);
    }
}

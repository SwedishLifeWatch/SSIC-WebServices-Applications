using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class holds parameters used to
    /// search for species observations.
    /// Field search criteria logical operator is used as an WebDataField.
    /// </summary>
    [DataContract]
    public class WebSpeciesObservationSearchCriteria : WebData
    {
        /// <summary>
        /// Requested minimum accuracy of the coordinates.
        /// Species observations with bad accurray (ie higher accuracy value )
        /// will not be included in the search result.
        /// Unit is meters. Use values ​​equal to or greater than zero.
        /// Parameter IsAccuracySpecified must be set to true if
        /// property Accuracy should be used.
        /// </summary>
        [DataMember]
        public Double Accuracy
        { get; set; }

#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
        /// <summary>
        /// Limit returned observations based on bird nest activity level.
        /// Only bird observations in Artportalen are affected
        /// by this search criteria.
        /// Only bird observations with the specified bird nest
        /// activity level or stronger may be returned.
        /// Observation of other organism groups (not birds) are
        /// not affected by this search criteria. 
        /// Use method GetBirdNestActivities() in web service
        /// SwedishSpeciesObservationService to retrieve
        /// currently used bird nest activities.
        /// Property BirdNestActivityLimit should be set to 
        /// the Id value of the selected bird nest activity.
        /// Set property IsBirdNestActivityLimitSpecified to true
        /// if this property is used.
        /// </summary>
        [DataMember]
        public Int32 BirdNestActivityLimit
        { get; set; }
#endif

        /// <summary>
        /// Limit returned observations to specified bounding box.
        /// Currently only two-dimensional bounding boxes can be used.
        /// </summary>
        [DataMember]
        public WebBoundingBox BoundingBox
        { get; set; }

        /// <summary>
        /// Search observations based on date and time
        /// when the observation was last changed.
        /// Not all functionality in WebDateTimeSearchCriteria
        /// are implemented.
        /// Current restrictions:
        /// Only Days are handled if Accuracy has been specified.
        /// </summary>
        [DataMember]
        public WebDateTimeSearchCriteria ChangeDateTime
        { get; set; }

        /// <summary>
        /// List of data sources to use in the search.
        /// All data sources are used if no specific
        /// data sources are provided.
        /// </summary>
        [DataMember]
#if SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
        public List<String> DataSourceGuids
        { get; set; }
#else
        public List<String> DataProviderGuids
        { get; set; }
#endif

        /// <summary>
        /// Limit search based on values for any data that is
        /// related to species observations.
        /// </summary>
        [DataMember]
        public List<WebSpeciesObservationFieldSearchCriteria> FieldSearchCriteria
        { get; set; }

        /// <summary>
        /// This property indicates whether to search
        /// for never found observations.
        /// "Never found observations" is an observation that says
        /// that the specified species was not found in a location
        /// deemed appropriate for the species.
        /// </summary>
        [DataMember]
        public Boolean IncludeNeverFoundObservations
        { get; set; }

        /// <summary>
        /// This property indicates whether to search
        /// for not rediscovered observations.
        /// "Not rediscovered observations" is an observation that says
        /// that the specified species was not found in a location
        /// where it has previously been observed.
        /// </summary>
        [DataMember]
        public Boolean IncludeNotRediscoveredObservations
        { get; set; }

        /// <summary>
        /// This property indicates whether to search
        /// for positive observations.
        /// "Positive observations" are normal observations indicating
        /// that a species has been seen at a specified location.
        /// </summary>
        [DataMember]
        public Boolean IncludePositiveObservations
        { get; set; }

        /// <summary>
        /// Search observations based on taxa that is
        /// readlisted in any of the specified red list categories.
        /// If property IncludeRedListCategories is not empty
        /// all taxa that is redlisted in any of the specified
        /// red list categories are added to property TaxonIds
        /// by the web service.
        /// Provided red list categories must be among the values
        /// DD, RE, CR, EN, VU or NT in the enum RedListCategory.
        /// The other values in enum RedListCategory are not valid
        /// search criterias.
        /// </summary>
        [DataMember]
        public List<RedListCategory> IncludeRedListCategories
        { get; set; }

        /// <summary>
        /// Search observations based on readlisted taxa.
        /// If property IncludeRedlistedTaxa is set to true
        /// all redlisted taxa is added to property TaxonIds
        /// by the web service.
        /// </summary>
        [DataMember]
        public Boolean IncludeRedlistedTaxa
        { get; set; }

        /// <summary>
        /// Indicates if species observations that are outside
        /// geographic area (e.g. bounding box, polygons or regions)
        /// but close enough when accuracy of observation are
        /// considered should be included in the result.
        /// This property has no impact on returned information
        /// if no geographic area is involved in the search.
        /// </summary>
        [DataMember]
        public Boolean IsAccuracyConsidered
        { get; set; }

        /// <summary>
        /// Indicates if property Accuracy has been set.
        /// </summary>
        [DataMember]
        public Boolean IsAccuracySpecified
        { get; set; }

#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
        /// <summary>
        /// Indicates if property BirdNestActivityLimit has been set.
        /// </summary>
        [DataMember]
        public Boolean IsBirdNestActivityLimitSpecified
        { get; set; }
#endif

        /// <summary>
        /// Indicates if species observations that are outside
        /// geography area (bounding box, polygons or regions) but
        /// close enough when disturbance sensitivity of species are
        /// considered should be included in the result.
        /// </summary>
        [DataMember]
        public Boolean IsDisturbanceSensitivityConsidered
        { get; set; }

#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
        /// <summary>
        /// Indicates if property IsNaturalOccurrence
        /// should be used or not.
        /// </summary>
        [DataMember]
        public Boolean IsIsNaturalOccurrenceSpecified
        { get; set; }
#endif

        /// <summary>
        /// Indicates if property MaxProtectionLevel has been set.
        /// </summary>
        [DataMember]
        public Boolean IsMaxProtectionLevelSpecified
        { get; set; }

        /// <summary>
        /// Indicates if property MinProtectionLevel has been set.
        /// </summary>
        [DataMember]
        public Boolean IsMinProtectionLevelSpecified
        { get; set; }

#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
        /// <summary>
        /// Restrict search based on if a positive observation
        /// is natural or not.
        /// Property IsIsNaturalOccurrenceSpecified indicates if
        /// property IsNaturalOccurrence should be used or not.
        /// </summary>
        [DataMember]
        public Boolean IsNaturalOccurrence
        { get; set; }
#endif

        /// <summary>
        /// String search criteria to match with locality names.
        /// Exactly one string compare operator must be specified.
        /// </summary>
        [DataMember]
        public WebStringSearchCriteria LocalityNameSearchString
        { get; set; }

        /// <summary>
        /// Only observations that has a protection level that
        /// is equal to or lower than the value of this property
        /// are included in the result.
        /// 1 is the lowest possible value.
        /// </summary>
        [DataMember]
        public Int32 MaxProtectionLevel
        { get; set; }

        /// <summary>
        /// Only observations that has a protection level that
        /// is equal to or higher than the value of this property
        /// are included in the result.
        /// 1 is the lowest possible value.
        /// </summary>
        [DataMember]
        public Int32 MinProtectionLevel
        { get; set; }

        /// <summary>
        /// Search observations based on observation date and time.
        /// Not all functionality in WebDateTimeSearchCriteria
        /// are implemented.
        /// Current restrictions:
        /// Only Days are handled if Accuracy has been specified.
        /// </summary>
        [DataMember]
        public WebDateTimeSearchCriteria ObservationDateTime
        { get; set; }

        /// <summary>
        /// Search observations based on observers.
        /// Observer id corresponds to person id in the
        /// user service.
        /// Observations that are not related to persons
        /// defined in user service will not match this
        /// search criteria.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public List<Int32> ObserverIds
        { get; set; }

        /// <summary>
        /// String search criteria to match with observer names.
        /// Exactly one string compare operator must be specified.
        /// </summary>
        [DataMember]
        public WebStringSearchCriteria ObserverSearchString
        { get; set; }

        /// <summary>
        /// Search observations that are inside specified polygons.
        /// </summary>
        [DataMember]
        public List<WebPolygon> Polygons
        { get; set; }

        /// <summary>
        /// Get observations related to specified projects
        /// (projects corresponds to "syfte" in Artportalen 1).
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public List<String> ProjectGuids
        { get; set; }

        /// <summary>
        /// Limit search to specified regions.
        /// Regions are defined by GeoReferenceService.
        /// </summary>
        [DataMember]
        public List<String> RegionGuids
        { get; set; }

        /// <summary>
        /// Specify how regions, polygons or bounding box
        /// should be logically combined when species
        /// observations are searched.
        /// The logical operator OR are used between spatial geometries.
        /// This property is currently not used.
        /// </summary>
        public LogicalOperator RegionLogicalOperator
        { get; set; }

#if SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
        /// <summary>
        /// Search observations based on registration date and time.
        /// Not all functionality in WebDateTimeSearchCriteria
        /// are implemented.
        /// Current restrictions:
        /// Only Days are handled if Accuracy has been specified.
        /// </summary>
        [DataMember]
        public WebDateTimeSearchCriteria RegistrationDateTime
        { get; set; }
#else
        /// <summary>
        /// Search observations based on reported date and time.
        /// Not all functionality in WebDateTimeSearchCriteria
        /// are implemented.
        /// Current restrictions:
        /// Only Days are handled if Accuracy has been specified.
        /// </summary>
        [DataMember]
        public WebDateTimeSearchCriteria ReportedDateTime
        { get; set; }
#endif

        /// <summary>
        /// Limit returned observations based on species activities.
        /// Use method GetSpeciesActivities() in web service
        /// SwedishSpeciesObservationService to retrieve
        /// currently used species activities.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public List<Int32> SpeciesActivityIds
        { get; set; }

        /// <summary>
        /// Limit search to taxa with specified ids.
        /// Ids for taxon are defined in
        /// web application Dyntaxa (www.Dyntaxa.se).
        /// Species observations made on sub taxa to specified taxa
        /// are also included in returned species observations.
        /// </summary>
        [DataMember]
        public List<Int32> TaxonIds
        { get; set; }

        /// <summary>
        /// Return only observations with specified validation status.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public List<Int32> ValidationStatusIds
        { get; set; }
    }
}

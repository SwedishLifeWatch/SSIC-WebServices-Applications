using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a species fact search criteria.
    /// </summary>
    [DataContract]
    public class WebSpeciesFactSearchCriteria : WebData
    {
        /// <summary>
        /// Search for species facts that belongs to factors
        /// that is of one of these factor data types.
        /// </summary>
        [DataMember]
        public List<Int32> FactorDataTypeIds { get; set; }

        /// <summary>
        /// Search for species facts that belongs to specified factors.
        /// </summary>
        [DataMember]
        public List<Int32> FactorIds { get; set; }

        /// <summary>
        /// Logical operator to use between field search criteria
        /// if more than one field search criteria are specified.
        /// Possible logical operator values are AND or OR.
        /// </summary>
        [DataMember]
        public LogicalOperator FieldLogicalOperator { get; set; }

        /// <summary>
        /// Search criteria for fields in species facts.
        /// </summary>
        [DataMember]
        public List<WebSpeciesFactFieldSearchCriteria> FieldSearchCriteria { get; set; }

        /// <summary>
        /// Search for species facts that are related to specified
        /// hosts. Host ids are taxon ids according to Dyntaxa.
        /// </summary>
        [DataMember]
        public List<Int32> HostIds { get; set; }

        /// <summary>
        /// Specify if species facts related to not valid hosts
        /// should be included or not. Valid hosts are always
        /// included in the species fact search.
        /// </summary>
        [DataMember]
        public Boolean IncludeNotValidHosts { get; set; }

        /// <summary>
        /// Specify if species facts related to not valid taxa
        /// should be included or not. Valid taxa are always
        /// included in the species fact search.
        /// </summary>
        [DataMember]
        public Boolean IncludeNotValidTaxa { get; set; }

        /// <summary>
        /// Search for species facts that belongs to specified 
        /// individual categories.
        /// </summary>
        [DataMember]
        public List<Int32> IndividualCategoryIds { get; set; }

        /// <summary>
        /// Property IsIsPeriodicSpecified indicates if property
        /// IsPeriodic has a value or not.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Boolean IsIsPeriodicSpecified { get; set; }

        /// <summary>
        /// Property IsIsPublicSpecified indicates if property
        /// IsPublic has a value or not.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Boolean IsIsPublicSpecified { get; set; }

        /// <summary>
        /// Property IsIsTaxonomicSpecified indicates if property
        /// IsTaxonomic has a value or not.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Boolean IsIsTaxonomicSpecified { get; set; }

        /// <summary>
        /// Restrict search to periodic or not periodic species facts.
        /// Property IsIsPeriodicSpecified indicates if this property
        /// has a value or not.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Boolean IsPeriodic { get; set; }

        /// <summary>
        /// Restrict search to public or not public species facts.
        /// Property IsIsPublicSpecified indicates if this property
        /// has a value or not.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Boolean IsPublic { get; set; }

        /// <summary>
        /// Restrict search to taxonomic or not taxonomic species facts.
        /// Property IsIsTaxonomicSpecified indicates if this property
        /// has a value or not.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Boolean IsTaxonomic { get; set; }

        /// <summary>
        /// Search for species facts that belongs to specified periods.
        /// </summary>
        [DataMember]
        public List<Int32> PeriodIds { get; set; }

        /// <summary>
        /// Search for species facts that has
        /// one of the specified quality values.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public List<Int32> QualityIds { get; set; }

        /// <summary>
        /// Search for species facts that belongs to specified references.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public List<Int32> ReferenceIds { get; set; }

        /// <summary>
        /// Search for species facts that belongs to specified taxa.
        /// Taxon ids are according to Dyntaxa.
        /// </summary>
        [DataMember]
        public List<Int32> TaxonIds { get; set; }

        /// <summary>
        /// Search species facts based on update date and time.
        /// Not all functionality in WebDateTimeSearchCriteria
        /// are implemented.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public WebDateTimeSearchCriteria UpdateDateSearchCriteria { get; set; }

        /// <summary>
        /// String search criteria to match with names of persons
        /// that has modified species facts.
        /// Exactly one string compare operator must be specified.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public WebStringSearchCriteria UpdatePersonSearchCriteria { get; set; }
    }
}

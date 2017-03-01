using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class holds factor filter information.
    /// </summary>
    [DataContract]
    public class WebFactorSearchCriteria : WebData
    {
        /// <summary>
        /// Search for factors that is of one of
        /// these factor data types.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public List<Int32> FactorDataTypeIds { get; set; }

        /// <summary>
        /// Search for factors that is of one of
        /// these factor origins.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public List<Int32> FactorOriginIds { get; set; }

        /// <summary>
        /// Indication whether or not the name search string may be a factor id.
        /// </summary>
        [DataMember]
        public Boolean IsIdInNameSearchString { get; set; }

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
        /// Restrict search to periodic or not periodic factors.
        /// Property IsIsPeriodicSpecified indicates if this property
        /// has a value or not.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Boolean IsPeriodic { get; set; }

        /// <summary>
        /// Restrict search to public or not public factors.
        /// Property IsIsPublicSpecified indicates if this property
        /// has a value or not.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Boolean IsPublic { get; set; }

        /// <summary>
        /// Restrict search to taxonomic or not taxonomic factors.
        /// Property IsIsTaxonomicSpecified indicates if this property
        /// has a value or not.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Boolean IsTaxonomic { get; set; }

        /// <summary>
        /// The factor name search criteria.
        /// </summary>
        [DataMember]
        public WebStringSearchCriteria NameSearchString { get; set; }

        /// <summary>
        /// Scope for factors that is returned.
        /// </summary>
        [DataMember]
        public FactorSearchScope RestrictReturnToScope { get; set; }

        /// <summary>
        /// Limit factor search (not factor return) to factors.
        /// </summary>
        [DataMember]
        public List<Int32> RestrictSearchToFactorIds { get; set; }

        /// <summary>
        /// Scope for factors that is searched.
        /// </summary>
        [DataMember]
        public FactorSearchScope RestrictSearchToScope { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Alternative restrictions of return data
    /// </summary>
    [DataContract]
    public enum FactorSearchScope
    {
        /// <summary>
        /// No special scope.
        /// </summary>
        [EnumMember]
        NoScope,
        /// <summary>
        /// Include child factors.
        /// </summary>
        [EnumMember]
        AllChildFactors,
        /// <summary>
        /// Include parent factors.
        /// </summary>
        [EnumMember]
        AllParentFactors,
        /// <summary>
        /// Include nearest child factors.
        /// </summary>
        [EnumMember]
        NearestChildFactors,
        /// <summary>
        /// Include nearest parent factors.
        /// </summary>
        [EnumMember]
        NearestParentFactors,
        /// <summary>
        /// Include child leaf factors.
        /// </summary>
        [EnumMember]
        LeafFactors
    }

    /// <summary>
    /// This class holds factor filter information
    /// </summary>
    [DataContract]
    public class WebFactorSearchCriteria : WebData
    {
        /// <summary>
        /// Create a WebFactorSearchCriteria instance.
        /// </summary>
        public WebFactorSearchCriteria()
            : base()
        {
            NameSearchString = null;
            NameSearchMethod = SearchStringComparisonMethod.Like;
            IsIdInNameSearchString = false;
            RestrictSearchToFactorIds = null;
            RestrictSearchToScope = FactorSearchScope.NoScope;
            RestrictReturnToScope = FactorSearchScope.NoScope;
        }

        /// <summary>
        /// Indication whether or not the name search string may be a factor id.
        /// </summary>
        [DataMember]
        public Boolean IsIdInNameSearchString
        { get; set; }

        /// <summary>
        /// Name search method.
        /// </summary>
        [DataMember]
        public SearchStringComparisonMethod NameSearchMethod
        { get; set; }

        /// <summary>
        /// The factor name search string.
        /// </summary>
        [DataMember]
        public String NameSearchString
        { get; set; }

        /// <summary>
        /// Scope for factors that is returned.
        /// </summary>
        [DataMember]
        public FactorSearchScope RestrictReturnToScope
        { get; set; }

        /// <summary>
        /// Limit factor search (not factor return) to factors.
        /// </summary>
        [DataMember]
        public List<Int32> RestrictSearchToFactorIds
        { get; set; }

        /// <summary>
        /// Scope for factors that is searched.
        /// </summary>
        [DataMember]
        public FactorSearchScope RestrictSearchToScope
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        public override void CheckData()
        {
            base.CheckData();
            NameSearchString = NameSearchString.CheckSqlInjection();
        }
    }
}

using System.Runtime.Serialization;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Localization;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Enums
{
    /// <summary>
    /// The different type of Row delimiters
    /// </summary>
    public enum RowDelimiter
    {
        /// <summary>ReturnLinefeed</summary>
        [LocalizedEnum("SharedRowDelimiterReturnLinefeed", NameResourceType = typeof(Resources.Resource))]
        ReturnLinefeed = 1,

        /// <summary>Semicolon</summary>
        [LocalizedEnum("SharedRowDelimiterSemicolon", NameResourceType = typeof(Resources.Resource))]
        Semicolon = 2,

        /// <summary>Comma</summary>
        [LocalizedEnum("SharedRowDelimiterTab", NameResourceType = typeof(Resources.Resource))]
        Tab = 3,

        /// <summary>VerticalBar</summary>
        [LocalizedEnum("SharedRowDelimiterVerticalBar", NameResourceType = typeof(Resources.Resource))]
        VerticalBar = 4
    }

    /// <summary>
    /// Definition of how search string is compared to
    /// stored string data in Dyntaxa.
    /// </summary>
    [DataContract]
    public enum SearchStringCompareOperator
    {
        /// <summary>
        /// Search for strings that begins with the specified
        /// search string. Wild chard is added to the search
        /// string (at the end) before the search begins.
        /// </summary>
        [EnumMember]
        BeginsWith,

        /// <summary>
        /// Search for strings that contains the specified
        /// search string. Wild chards are added to the search string
        /// (at the beginning and end) before the search begins.
        /// </summary>
        [EnumMember]
        Contains,

        /// <summary>
        /// Search for strings that ends with the specified
        /// search string. Wild chard is added to the search
        /// string (at the beginning) before the search begins.
        /// </summary>
        [EnumMember]
        EndsWith,

        /// <summary>
        /// Search for strings that are equal to the specified
        /// search string.
        /// </summary>
        [EnumMember]
        Equal,

        /// <summary>
        /// Search for strings that similar the specified
        /// search string.
        /// No wild chards are added to the search string.
        /// </summary>
        [EnumMember]
        Like,

        /// <summary>
        /// Search in sequence Equal, BeginsWith, Contains
        /// search string.
        /// Checks result after each step in the sequence and returns if one (1) or more record(s) are found.
        /// </summary>
        [EnumMember]
        Iterative
    }

    /// <summary>
    /// Taxon change status
    /// </summary>
    public enum TaxonChangeStatus
    {
        Splitted = -4,
        Lumped = -2,
        Deleted = -1,
        Unchanged = 0,
        Lumping = 2,
        Splitting = 4
    }

    /// <summary>
    /// Taxon validity
    /// </summary>
    public enum TaxonValidity
    {
        Valid,
        NotValid,
        NonTaxonomic
    }

    /// <summary>
    /// A classification of taxon quality,
    /// which summarize, in general terms, the quality of 
    /// the taxonomic information of current taxon,
    /// its child taxa and positioning in the hierarchical system.
    /// </summary>
    public enum QualityStatus
    {
        /// <summary>
        /// (0) dålig, det är känt att kvaliteten på data är felaktig eller data saknas (t.ex. ofullständigt inlagda taxa);
        /// </summary>
        Bad,

        /// <summary>
        /// (1) osäker, d.v.s. ej kvalitetssäkrad (gäller exempelvis data som importerats tidigt i Dyntaxas historia och som därefter inte expertgranskats); 
        /// </summary>
        Uncertain,

        /// <summary>
        /// (2) kvalitetssäkrad
        /// </summary>
        Good
    }
}

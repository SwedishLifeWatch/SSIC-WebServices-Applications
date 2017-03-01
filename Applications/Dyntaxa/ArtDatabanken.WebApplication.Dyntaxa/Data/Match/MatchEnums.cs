using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public enum MatchTaxonInputType
    {
        /// <summary>ExcelFile</summary>
        [LocalizedEnum("MatchTaxonInputTypeExcelFile", NameResourceType = typeof(Resources.DyntaxaResource))]
        ExcelFile = 1,

        /// <summary>ClipboardText</summary>
        [LocalizedEnum("MatchTaxonInputTypeClipboardText", NameResourceType = typeof(Resources.DyntaxaResource))]
        ClipboardText = 2
    }

    public enum MatchColumnContentAlternative
    {
        /// <summary>First column includes only name or taxonid.</summary>        
        [LocalizedEnum("MatchColumnContentAlternativeWithoutAuthor", NameResourceType = typeof(Resources.DyntaxaResource))]
        WithoutAuthor = 1,

        /// <summary>First column includes both name and author.</summary>
        [LocalizedEnum("MatchColumnContentAlternativeNameAndAuthorCombined", NameResourceType = typeof(Resources.DyntaxaResource))]
        NameAndAuthorCombined = 2,

        /// <summary>First column includes only names and second column includes author</summary>
        [LocalizedEnum("MatchColumnContentAlternativeNameAndAuthorInSeparateColumns", NameResourceType = typeof(Resources.DyntaxaResource))]
        NameAndAuthorInSeparateColumns = 3
    }

    public enum MatchTaxonRowDelimiter
    {
        /// <summary>ReturnLinefeed</summary>
        [LocalizedEnum("MatchTaxonRowDelimiterReturnLinefeed", NameResourceType = typeof(Resources.DyntaxaResource))]
        ReturnLinefeed = 1,

        /// <summary>Semicolon</summary>
        [LocalizedEnum("MatchTaxonRowDelimiterSemicolon", NameResourceType = typeof(Resources.DyntaxaResource))]
        Semicolon = 2,

        /// <summary>Comma</summary>
        [LocalizedEnum("MatchTaxonRowDelimiterTab", NameResourceType = typeof(Resources.DyntaxaResource))]
        Tab = 3,

        /// <summary>VerticalBar</summary>
        [LocalizedEnum("MatchTaxonRowDelimiterVerticalBar", NameResourceType = typeof(Resources.DyntaxaResource))]
        VerticalBar = 4
    }

    public enum MatchTaxonColumnDelimiter
    {
        /// <summary>Tab</summary>
        [LocalizedEnum("MatchTaxonColumnDelimiterTab", NameResourceType = typeof(Resources.DyntaxaResource))]
        Tab = 1,

        /// <summary>Semicolon</summary>
        [LocalizedEnum("MatchTaxonColumnDelimiterSemicolon", NameResourceType = typeof(Resources.DyntaxaResource))]
        Semicolon = 2,

        /// <summary>VerticalBar</summary>
        [LocalizedEnum("MatchTaxonColumnDelimiterVerticalBar", NameResourceType = typeof(Resources.DyntaxaResource))]
        VerticalBar = 3
    }

    public enum MatchTaxonToType
    {
        /// <summary>ScientificName</summary>
        [LocalizedEnum("MatchTaxonToTypeTaxonName", NameResourceType = typeof(Resources.DyntaxaResource))]
        TaxonName = 1,

        /// <summary>TaxonId</summary>
        [LocalizedEnum("MatchTaxonToTypeTaxonId", NameResourceType = typeof(Resources.DyntaxaResource))]
        TaxonId = 2,
        [LocalizedEnum("MatchTaxonToTypeTaxonNameAndAuthor", NameResourceType = typeof(Resources.DyntaxaResource))]
        TaxonNameAndAuthor = 3        
    }

    public enum MatchStatus
    {
        [LocalizedEnum("MatchStatusUndone", NameResourceType = typeof(Resources.DyntaxaResource))]
        Undone,
        [LocalizedEnum("MatchStatusExact", NameResourceType = typeof(Resources.DyntaxaResource))]
        Exact,
        [LocalizedEnum("MatchStatusNeedsManualSelection", NameResourceType = typeof(Resources.DyntaxaResource))]
        NeedsManualSelection,
        [LocalizedEnum("MatchStatusManualSelection", NameResourceType = typeof(Resources.DyntaxaResource))]
        ManualSelection,
        [LocalizedEnum("MatchStatusFuzzy", NameResourceType = typeof(Resources.DyntaxaResource))]
        Fuzzy,
        [LocalizedEnum("MatchStatusNoMatch", NameResourceType = typeof(Resources.DyntaxaResource))]
        NoMatch
    }    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Reference;
using Resources;
using System.Web.Mvc;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Match
{
    public class MatchSettingsViewModel
    {
        public MatchSettingsViewModel()
        {
            this.LabelForProvidedText = Resources.DyntaxaResource.MatchOptionsOutputProvidedTextLabel;

            _searchOptions = new TaxonSearchOptions();
            _searchOptions.DefaultNameCompareOperator = DyntaxaStringCompareOperator.Equal;
            _searchOptions.NameCompareOperator = DyntaxaStringCompareOperator.Equal;
            _searchOptions.HideAuthorTextbox = true;
        }

        private readonly TaxonSearchOptions _searchOptions = null;
        public TaxonSearchOptions SearchOptions
        {
            get { return _searchOptions; }
        }

        #region BasicProperties

        /// <summary>
        /// Id of taxon.
        /// </summary>
        public string TaxonId { get; set; }

        /// <summary>
        /// Recommended Scientific Name of the taxon.
        /// </summary>
        public string ScientificName { get; set; }

        /// <summary>
        /// Author of the recommended scientific name of the taxon.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Recommended common name of the taxon.
        /// </summary>
        public string CommonName { get; set; }

        /// <summary>
        /// Taxon category the taxon, e.g. Species, Genus or Family.
        /// </summary>
        public string TaxonCategory { get; set; }

        /// <summary>
        /// Type of input format to match
        /// </summary>
        public MatchTaxonInputType MatchInputType { get; set; }

        /// <summary>
        /// String including auto-generated filename
        /// </summary>
        [LocalizedDisplayName("MatchOptionsFileName", NameResourceType = typeof(Resources.DyntaxaResource))]
        public string FileName { get; set; }

        /// <summary>
        /// String pasted into textarea
        /// </summary>
        [LocalizedDisplayName("MatchOptionsClipboard", NameResourceType = typeof(Resources.DyntaxaResource))]
        [AllowHtml]
        public string ClipBoard { get; set; }

        /// <summary>
        /// Boolean to indicate whether first row in is Column names
        /// </summary>
        [LocalizedDisplayName("MatchOptionsIsFirstRowColumnNameLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public bool IsFirstRowColumnName { get; set; }

        /// <summary>
        /// Column content alternatives.
        /// </summary>
        [LocalizedDisplayName("MatchOptionsInputColumnContentLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public MatchColumnContentAlternative ColumnContentAlternative { get; set; }

        /// <summary>
        /// Type of the Column delimiter
        /// </summary>
        [LocalizedDisplayName("MatchOptionsInputRowDelimeterLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public MatchTaxonRowDelimiter RowDelimiter { get; set; }

        /// <summary>
        /// Type of the Column delimiter
        /// </summary>
        [LocalizedDisplayName("MatchOptionsInputColumnDelimeterLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public MatchTaxonColumnDelimiter ColumnDelimiter { get; set; }

        ///// <summary>
        ///// Gets or sets a value indicating whether the author name is part of taxon name.
        ///// </summary>        
        //[LocalizedDisplayName("MatchOptionsInputAuthorNamePartOfTaxonName", NameResourceType = typeof(Resources.DyntaxaResource))]
        //public bool AuthorNameIsPartOfTaxonName { get; set; }

        /// <summary>
        /// Value indicating what list should match to
        /// </summary>
        [LocalizedDisplayName("MatchOptionsMatchToTypeLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public MatchTaxonToType MatchToType { get; set; }

        /// <summary>
        /// Label for the Limit to taxon option property.
        /// </summary>
        public string LimitToTaxonLabel { get; set; }

        /// <summary>
        /// Option setting for limit match to certain parent taxon.
        /// </summary>
        [LocalizedDisplayName("MatchOptionsLimitToTaxonLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public bool LimitToTaxon { get; set; }

        /// <summary>
        /// Name or identifier that represents a taxon which match should be limited to
        /// Is same taxon as reprecented by InputId if provided.
        /// </summary>
        public int? LimitToParentTaxonId { get; set; }
        #endregion

        #region Field labels

        /// <summary>
        /// Label for the output column representing provided information.
        /// </summary>
        public string LabelForProvidedText { get; set; }

        #endregion

        #region Output options

        /// <summary>
        /// Boolean to indicate if a column with taxon category should be included in output.
        /// </summary>
        [LocalizedDisplayName("MatchOptionsOutputTaxonCetegoryLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public bool OutputTaxonCategory { get; set; }

        /// <summary>
        /// Boolean to indicate if a column with OutputTaxonId column should be included in output.
        /// </summary>
        [LocalizedDisplayName("MatchOptionsOutputTaxonIdLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public bool OutputTaxonId { get; set; }

        /// <summary>
        /// Boolean to indicate if a column with GUID column should be included in output.
        /// </summary>
        [LocalizedDisplayName("MatchOptionsOutputGUIDLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public bool OutputGUID { get; set; }

        /// <summary>
        /// Boolean to indicate if a column with recommended scientific name column should be included in output.
        /// </summary>
        [LocalizedDisplayName("MatchOptionsOutputScientificNameLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public bool OutputScientificName { get; set; }

        /// <summary>
        /// Boolean to indicate if a column with scientific Synonyms column should be included in output.
        /// </summary>
        [LocalizedDisplayName("MatchOptionsOutputScientificSynonymsLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public bool OutputScientificSynonyms { get; set; }

        /// <summary>
        /// Boolean to indicate if columns representing different parent taxon categories should be included in output.
        /// </summary>
        [LocalizedDisplayName("MatchOptionsOutputParentTaxaLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public bool OutputParentTaxa { get; set; }

        /// <summary>
        /// Boolean to indicate if a column with author of recommended scientific name column should be included in output.
        /// </summary>
        [LocalizedDisplayName("MatchOptionsOutputAuthorLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public bool OutputAuthor { get; set; }

        /// <summary>
        /// Boolean to indicate if a column with recommended common name should be included in output.
        /// </summary>
        [LocalizedDisplayName("MatchOptionsOutputCommonNameLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public bool OutputCommonName { get; set; }

        [LocalizedDisplayName("ExportStraightColumnRecommendedGUID", NameResourceType = typeof(Resources.DyntaxaResource))]
        public Boolean OutputRecommendedGUID { get; set; }

        [LocalizedDisplayName("ExportStraightSwedishOccurrence", NameResourceType = typeof(Resources.DyntaxaResource))]
        public Boolean OutputSwedishOccurrence { get; set; }

        #endregion   

        public ModelLabels Labels
        {
            get { return _labels; }
        }
        private readonly ModelLabels _labels = new ModelLabels();

        /// <summary>
        /// Localized labels
        /// </summary>
        public class ModelLabels
        {            
            public string FilterLabel { get { return Resources.DyntaxaResource.MatchOptionsFilter; } }
            public string OutputLabel { get { return Resources.DyntaxaResource.MatchOptionsOutput; } }
            public string Input { get { return Resources.DyntaxaResource.MatchOptionsInput; } }
            public string ClipboardLabel { get { return Resources.DyntaxaResource.MatchOptionsClipboard; } }
            public string MatchToTypeLabel { get { return Resources.DyntaxaResource.MatchOptionsMatchToType; } }
            public string SearchOptionsLabel { get { return Resources.DyntaxaResource.MatchOptionsSearchOptions; } }
            public string TitleLabel { get { return DyntaxaResource.MatchOptionsTitle; } }

            /// <summary>
            /// Label for the output column representing match status.
            /// </summary>
            public string MatchStatusLabel
            {
                get { return Resources.DyntaxaResource.MatchOptionsOutputMatchStatusLabel; }
            }

            /// <summary>
            /// Label for the output column representing ambiguities.
            /// </summary>
            public string AmbiguitiesLabel
            {
                get { return Resources.DyntaxaResource.MatchOptionsOutputAmbiguitiesLabel; }
            }

            /// <summary>
            /// Label for the output column representing taxon id.
            /// </summary>
            public string TaxonIdLabel
            {
                get { return Resources.DyntaxaResource.MatchOptionsOutputTaxonIdLabel; }
            }

            /// <summary>
            /// Label for the output column representing recommended scientific name.
            /// </summary>
            public string ScientificNameLabel
            {
                get { return Resources.DyntaxaResource.MatchOptionsOutputScientificNameLabel; }
            }

            /// <summary>
            /// Label for the output column representing author.
            /// </summary>
            public string AuthorLabel
            {
                get { return Resources.DyntaxaResource.MatchOptionsOutputAuthorLabel; }
            }

            /// <summary>
            /// Label for the output column representing common name.
            /// </summary>
            public string CommonNameLabel
            {
                get { return Resources.DyntaxaResource.MatchOptionsOutputCommonNameLabel; }
            }

            /// <summary>
            /// Label for the output column representing GUID.
            /// </summary>
            public string GuidLabel
            {
                get { return Resources.DyntaxaResource.MatchOptionsOutputGUIDLabel; }
            }

            /// <summary>
            /// Label for the output column representing taxon category.
            /// </summary>
            public string TaxonCategoryLabel
            {
                get { return Resources.DyntaxaResource.MatchOptionsOutputTaxonCetegoryLabel; }
            }

            /// <summary>
            /// Label for the output column representing scientific Synonyms.
            /// </summary>
            public string ScientificSynonymsLabel
            {
                get { return Resources.DyntaxaResource.MatchOptionsOutputScientificSynonymsLabel; }
            }

            /// <summary>
            /// Label for the output column representing taxon parents.
            /// </summary>
            public string ParentTaxaLabel
            {
                get { return Resources.DyntaxaResource.MatchOptionsOutputParentTaxaLabel; }
            }

            public string PasteTextLabel
            {
                get { return Resources.DyntaxaResource.MatchOptionsPasteText; }
            }

            public string ExcelFileLabel
            {
                get { return Resources.DyntaxaResource.MatchOptionsExcelFile; }
            }

            public string SaveAsExcelLabel
            {
                get { return Resources.DyntaxaResource.MatchOptionsSaveAsExcel; }
            }            

            public string ViewResultOnScreenLabel
            {
                get { return Resources.DyntaxaResource.MatchOptionsViewResultScreen; }
            }            

            public string AmbiguitiesTitleLabel
            {
                get { return Resources.DyntaxaResource.MatchOptionsAmbiguitiesTitle; }
            }            

            public string PickCorrectMatchLabel
            {
                get { return Resources.DyntaxaResource.MatchOptionsPickCorrectMatchFor; }
            }            

            public string ResultLabel
            {
                get { return Resources.DyntaxaResource.MatchOptionsResult; }
            }     

            public string ProvidedTextLabel
            {
                get { return Resources.DyntaxaResource.MatchOptionsOutputProvidedTextLabel; }
            }     
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ArtDatabanken.Data;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    public class TaxonSwedishOccuranceEditViewModel : TaxonSwedishOccuranceBaseViewModel 
    {
        private readonly ModelLabels _labels = new ModelLabels();
        
        /// <summary>
        /// Get abd sets the taxon Id
        /// </summary>
        public string TaxonId { get; set; }

        /// <summary>
        /// gets and sets taxon Guid
        /// </summary>
        public string TaxonGuid { get; set; }

        /// <summary>
        /// Indicates if it is possible to Species fact ie if Taxon has sortorder higher and  included for "släkte".
        /// </summary>
        public bool EnableSpeciesFact { get; set; }

        /// <summary>
        /// Indicates Species fact not possible to read.
        /// </summary>
        public bool SpeciesFactError { get; set; }

        /// <summary>
        /// Get and sets the internal error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Sets the id of failing taxon
        /// </summary>
        public string TaxonErrorId { get; set; }

        #region Labels

        /// <summary>
        /// All localized labels
        /// </summary>
        public ModelLabels Labels
        {
            get { return _labels; }
        }

        /// <summary>
        /// Localized labels used in edit revision view
        /// </summary>
        public class ModelLabels
        {
            public string SaveButtonText
            {
                get { return Resources.DyntaxaResource.SharedSaveButtonText; }
            }

            public string CancelButtonText
            {
                get { return Resources.DyntaxaResource.SharedCancelButtonText; }
            }

            public string EditTaxonLabel
            {
                get { return Resources.DyntaxaResource.TaxonEditHeaderLabel; }
            }

            public string SwedishHistoryLabel
            {
                get { return Resources.DyntaxaResource.TaxonEditSwedishImmigrationHistoryLabel; }
            }
            public string SwedishOccurrenceLabel
            {
                get { return Resources.DyntaxaResource.TaxonEditSwedishOccurrenceLabel; }
            }

            public string TaxonEditQualityHeaderText
            {
                get { return Resources.DyntaxaResource.TaxonEditQualityHeaderText; }
            }

            public string SpeciesFactErrorText
            {
                get { return Resources.DyntaxaResource.SharedSpeciesFactError; }
            }

            public string SavingLabel
            {
                get { return Resources.DyntaxaResource.SharedSaving; }
            } 
        }

#endregion

    }
}
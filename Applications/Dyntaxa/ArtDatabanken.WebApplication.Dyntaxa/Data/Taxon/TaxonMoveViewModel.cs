using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;
using Resources;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// View model for /Taxon/Move/    
    /// </summary>
    public class TaxonMoveViewModel
    {        
        private readonly ModelLabels _labels = new ModelLabels();

        [Required(ErrorMessageResourceName = "TaxonMoveNoOldParentTaxonSelectedError", ErrorMessageResourceType = typeof(Resources.DyntaxaResource))]
         public int OldParentTaxonId { get; set; }

        [Required(ErrorMessageResourceName = "TaxonMoveNoChildTaxaSelectedError", ErrorMessageResourceType = typeof(Resources.DyntaxaResource))]
        public List<int> SelectedChildTaxaToMove { get; set; }

        [Required(ErrorMessageResourceName = "TaxonMoveNoNewParentTaxonSelectedError", ErrorMessageResourceType = typeof(Resources.DyntaxaResource))]
        public int? NewParentTaxon { get; set; }  

        public int TaxonId { get; set; }

        public List<TaxonCategoryViewModel> TaxonCategories { get; set; }

        /// <summary>
        /// Indicates if selected taxon has children.
        /// </summary>
        public bool HasChildren { get; set; }

        /// <summary>
        /// All child taxons
        /// </summary>
        public List<RelatedTaxonViewModel> ChildTaxa { get; set; }
        
        public List<RelatedTaxonViewModel> AvailableParents { get; set; }

        /// <summary>
        /// Indicates if it is ok to  move
        /// </summary>
        public bool IsOkToMove { get; set; }

        /// <summary>
        /// Error information on what went wrong when trying to move taxa.
        /// </summary>
        public string MoveErrorText { get; set; }

        /// <summary>
        /// All localized labels
        /// </summary>
        public ModelLabels Labels
        {
            get { return _labels; }
        }

        /// <summary>
        /// Localized labels used in Taxon/Move view
        /// </summary>
        public class ModelLabels
        {
            public string SelectChildrenToMoveLabel { get { return DyntaxaResource.TaxonMoveSelectChildrenToMove; } }
            public string TitleLabel { get { return DyntaxaResource.TaxonMoveTitle; } }
            public string SelectNewParentLabel { get { return DyntaxaResource.TaxonMoveSelectNewParent; } }
            public string AvailableParentsLabel { get { return DyntaxaResource.TaxonMoveAvailableParents; } }
            public string SelectedParentLabel { get { return DyntaxaResource.TaxonMoveSelectedParent; } }
            public string MoveSelectedTaxaLabel { get { return DyntaxaResource.TaxonMoveMoveSelectedTaxa; } }

            public object TaxonMoveNoChildrenErrorLabel { get { return DyntaxaResource.TaxonMoveNoChildrenErrorText; } }

            public string ConfirmText { get { return DyntaxaResource.SharedOkButtonText; } }
        }
    }
}

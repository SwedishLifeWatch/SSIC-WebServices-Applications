using System.Collections.Generic;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;
using Resources;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    public class RevisionCommonInfoViewModel 
    {
        private readonly RevisionCommonInfoViewModelLabels labels = new RevisionCommonInfoViewModelLabels();

        /// <summary>
        /// Get the internal taxon object.
        /// </summary>
        public string TaxonId { get; set; }

        /// <summary>
        /// Get the revision object.
        /// </summary>
        public string RevisionId { get; set; }
        
        /// <summary>
         /// Text to show on header for edidting data..
         /// </summary>
         public string RevisionEditingHeaderText { get; set; }

         /// <summary>
         /// Text to show on action header for edidting data..
         /// </summary> 
         public string RevisionEditingActionHeaderText { get; set; }

        /// <summary>
        /// A list of information for revisions
        /// </summary>
        public IList<RevisionInfoItemModelHelper> RevisionInfoItems { get; set; }

       /// <summary>
       /// Text information in pop up dialog
       /// </summary>
        public string DialogTextPopUpText { get; set; }
       
        /// <summary>
        /// Text information in popup dialog title
        /// </summary>
        public string DialogTitlePopUpText { get; set; }

        /// <summary>
        /// Get and sets the internal error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// All localized labels
        /// </summary>
        public RevisionCommonInfoViewModelLabels Labels
        {
            get { return labels; }
        }

        /// <summary>
        /// St if for is to be submitted at once
        /// </summary>
        public bool Submit { get; set; } 

        /////// <summary>
        /////// Get the internal taxon object.
        /////// </summary>
        //public new string TaxonId
        //{
        //    get { return base.TaxonId; }
        //    set { base.TaxonId = value; }
        //}

        /////// <summary>
        /////// Revision identifier
        /////// </summary>
        //public new string RevisionId
        //{
        //    get { return base.RevisionId; }
        //    set { base.RevisionId = value; }
        //}

        /// <summary>
        /// Which action to perform when pushing editing button
        /// </summary>
        public string EditingAction { get; set; }

        /// <summary>
        /// Which controller to perform when pushing editing button
        /// </summary>
        public string EditingController { get; set; }
    }

    /// <summary>
    /// Localized labels used in Taxon/Info view
    /// </summary>
    public class RevisionCommonInfoViewModelLabels
    {
        public string CancelTextPopUpText
        {
            get { return DyntaxaResource.SharedCancelButtonText; }
        }

         public string ConfirmTextPopUpText
        {
            get { return DyntaxaResource.SharedOkButtonText; }
        }

        public string CancelText
        {
            get { return Resources.DyntaxaResource.SharedCancelButtonText; }
        }

        public string RevisionIdText
        {
            get { return " " + Resources.DyntaxaResource.SharedRevisionText + " " + Resources.DyntaxaResource.SharedRevisionIdLabel + ": "; }
        }

        public string ReturnToListText
        {
            get { return Resources.DyntaxaResource.SharedReturnToListText; }
        }
    }
}

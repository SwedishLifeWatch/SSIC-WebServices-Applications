using System.Collections.Generic;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    public class RevisionInfoViewModel
    {
        private readonly InfoRevisionViewModelLabels labels = new InfoRevisionViewModelLabels();    

        /// <summary>
        /// Get the internal taxon object.
        /// </summary>
        public string TaxonId { get; set; }
      
        /// <summary>
        /// Get the revision object.
        /// </summary>
        [LocalizedDisplayName("SharedRevisionIdLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
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
        /// Get and sets the internal error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Which action to perform when pushing editing button
        /// </summary>
        public string EditingAction { get; set; }

        /// <summary>
        /// Which controller to perform when pushing editing button
        /// </summary>
        public string EditingController { get; set; }

        /// <summary>
        /// All localized labels
        /// </summary>
        public InfoRevisionViewModelLabels Labels
        {
            get { return labels; }
        }

        /// <summary>
        /// Localized labels used in Taxon/Info view
        /// </summary>
        public class InfoRevisionViewModelLabels
        {
            public string CancelText
            {
                get { return Resources.DyntaxaResource.SharedCancelButtonText; }
            }

            public string ReturnToListText
            {
                get { return Resources.DyntaxaResource.SharedReturnToListText; }
            }
        }
    }
}

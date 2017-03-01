using System;
using System.Collections.Generic;
using ArtDatabanken.Data;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    public class RevisionEventViewModel
    {
        private readonly RevisionEventViewModelLabels labels = new RevisionEventViewModelLabels();

        /// <summary>
        /// Get the internal taxon object.
        /// </summary>
        public string TaxonId { get; set; }

        /// <summary>
        /// Get the revision object.
        /// </summary>
        public string RevisionId { get; set; }

        /// <summary>
        /// Get the selecetd revision event.
        /// </summary>
        public string RevisionEventId { get; set; }

        /// <summary>
        /// A list of information for revisions
        /// </summary>
        public IList<RevisionEventModelHelper> RevisionEventItems { get; set; }

        /// <summary>
        /// Internal error message
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Indicates if revisionEvents exist
        /// </summary>
        public bool ExistEvents { get; set; }

        /// <summary>
        /// All localized labels
        /// </summary>
        public RevisionEventViewModelLabels Labels
        {
            get { return labels; }
        }

        /// <summary>
        /// Holds information on selected taxon...
        /// </summary>
        public RevisionTaxonInfoViewModel RevisionTaxonInfoViewModel { get; set; }

        /// <summary>
        /// Localized labels used in Taxon Event controller view
        /// </summary>
        public class RevisionEventViewModelLabels
        {
            public string PageTitle
            {
                get { return Resources.DyntaxaResource.RevisionListEventPageTitleText; }
            }
            public string TitleLabel
            {
                get { return Resources.DyntaxaResource.RevisionListEventTitleLabelText; }
            }

            public string EventListLabel
            {
                get
                {
                    return Resources.DyntaxaResource.RevisionListEventListTitleLabelText;
                }
            }

            public string EventListCountLabel
            {
                get
                {
                    return Resources.DyntaxaResource.RevisionListEventListCountLabelText;
                }
            }

            public string GetUndo
            {
                get { return "GetUndo"; }
            }

            public string ReturnToListText
            {
                get { return Resources.DyntaxaResource.SharedReturnToListText; }
            }

            public string CancelText
            {
                get { return Resources.DyntaxaResource.SharedCancelButtonText; }
            }

            public string ConfirmText
            {
                get { return Resources.DyntaxaResource.SharedOkButtonText; }
            }

            public string UndoText
            {
                get { return Resources.DyntaxaResource.RevisionListEventUndoTypeText; }
            }
            public string PopUpTitle
            {
                get { return Resources.DyntaxaResource.SharedDeleteButtonText + " " + Resources.DyntaxaResource.RevisionListEventPageTitleText; }
            }

            public string UndoTextPopUp
            {
                get { return Resources.DyntaxaResource.RevisionListEventUndoTypeText + "?"; }
            }

            public string StepText
            {
                get { return Resources.DyntaxaResource.RevisionListEventStepTypeText; }
            }

            public string ChangeTypeText
            {
                get { return Resources.DyntaxaResource.RevisionListEventChangeTypeText; }
            }

            public string AffectedTaxaText
            {
                get { return Resources.DyntaxaResource.RevisionListEventAffectedTaxaValueText; }
            }

            public string NewValueText
            {
                get { return Resources.DyntaxaResource.RevisionListEventNewValueText; }
            }

            public string FormerValueText
            {
                get { return Resources.DyntaxaResource.RevisionListEventOldValueText; }
            }

            public string RevisionListNoRevisionEventsAvailableLabel
            {
                get { return Resources.DyntaxaResource.RevisionListEventNoRevisionEventsAvailableText; }
            }
        }
    }
}

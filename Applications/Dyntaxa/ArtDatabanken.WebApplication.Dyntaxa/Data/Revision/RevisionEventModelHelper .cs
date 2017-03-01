using System;
using System.Collections.Generic;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    public class RevisionEventModelHelper
    {
        private readonly RevisionEventModelHelperLabels labels = new RevisionEventModelHelperLabels();

        /// <summary>
        /// Get and sets the internal revision object.
        /// </summary>
        public string RevisionEventId { get; set; }

        /// <summary>
        /// Get and sets the internal revision object index.
        /// </summary>
        public int RevisionEventIndex { get; set; }

        /// <summary>
        /// Get and sets change event type string
        /// </summary>
        public string ChangeEventType { get; set; }

        /// <summary>
        /// Get and sets new taxa event value information string.
        /// </summary> 
        public string NewValue { get; set; }

        /// <summary>
        /// Get and sets former taxa event value information string.
        /// </summary> 
        public string FormerValue { get; set; }

        /// <summary>
        /// Get and sets affected taxa event information string.
        /// </summary> 
        public string AffectedTaxa { get; set; }

        /// <summary>
        /// All localized labels
        /// </summary>
        public RevisionEventModelHelperLabels Labels
        {
            get { return labels; }
        }

        /// <summary>
        /// Localized labels used in EventController view
        /// </summary>
        public class RevisionEventModelHelperLabels
        {
            public string CancelText
            {
                get { return Resources.DyntaxaResource.SharedCancelButtonText; }
            }
        }
    }
}

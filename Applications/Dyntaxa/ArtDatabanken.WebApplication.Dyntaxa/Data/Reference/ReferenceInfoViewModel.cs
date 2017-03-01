using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resources;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Reference
{
    public class ReferenceInfoViewModel
    {
        public string Guid { get; set; }
        public int? TaxonId { get; set; }
        public List<ReferenceViewModel> References { get; set; }

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
            public string TitleLabel { get { return DyntaxaResource.ReferenceAddTitle; } }
            public string SearchReferencesLabel { get { return DyntaxaResource.ReferenceAddSearchReferences; } }
            public string AddButtonLabel { get { return DyntaxaResource.ReferenceAddButtonAdd; } }

            public string ColumnTitleType { get { return DyntaxaResource.ReferenceAddType; } }
            public string ColumnTitleId { get { return DyntaxaResource.ReferenceAddId; } }
            public string ColumnTitleName { get { return DyntaxaResource.ReferenceAddName; } }
            public string ColumnTitleYear { get { return DyntaxaResource.ReferenceAddYear; } }
            public string ColumnTitleText { get { return DyntaxaResource.ReferenceAddText; } }
            public string ColumnTitleUsage { get { return DyntaxaResource.ReferenceAddUsage; } }
            public string ColumnTitleUsageTypeId { get { return DyntaxaResource.ReferenceAddUsageTypeId; } }

            public string SearchLabel { get { return DyntaxaResource.ReferenceAddSearchLabel; } }
            public string NumberOfFilteredElementsLabel { get { return DyntaxaResource.ReferenceAddNumberOfFilteredElementsLabel; } }
            public string NoDataAvailableLabel { get { return DyntaxaResource.ReferenceAddNoDataAvailableLabel; } }
            public string FilteringLabel { get { return DyntaxaResource.ReferenceAddFilteringLabel; } }
            public string NoRecordsLabel { get { return DyntaxaResource.ReferenceAddNoRecordsLabel; } }
            public string ReferencesLabel { get { return DyntaxaResource.ReferenceAddReferences; } }
        }
    }
}

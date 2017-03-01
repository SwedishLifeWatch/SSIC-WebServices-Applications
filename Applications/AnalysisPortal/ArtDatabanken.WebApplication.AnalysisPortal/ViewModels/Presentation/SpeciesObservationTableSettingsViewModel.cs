using System.Collections.Generic;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Presentation
{
    public class TableTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TableTypeViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public TableTypeViewModel()
        {
        }
    }

    public class SpeciesObservationTableSettingsViewModel
    {
        public List<TableTypeViewModel> SystemDefinedTables { get; set; }
        public List<TableTypeViewModel> UserDefinedTables { get; set; }
        public bool UseUserDefinedTableType { get; set; }
        public int SelectedTableId { get; set; }
        public bool UseLabelAsColumnHeader { get; set; }

        public ModelLabels Labels
        {
            get { return _labels; }
        }

        public bool IsSettingsDefault { get; set; }

        private readonly ModelLabels _labels = new ModelLabels();

        /// <summary>
        /// Localized labels
        /// </summary>
        public class ModelLabels
        {
            public string Title = Resources.Resource.PresentationTableSettings;
            public string InfoText = Resources.Resource.PresentationTableInfoLabel;
            public string PreviewLabel = Resources.Resource.PresentationTablePreviewLabel;
            public string UpdateColumnsButton = Resources.Resource.PresentationTableSelectColumnsButtonText;
            public string UpdateTableTypeButton = Resources.Resource.PresentationTableSelectTypeButtonText;
            public string TableType = Resources.Resource.PresentationTableTypeSelectedLabel;
            public string ColumnType = Resources.Resource.PresentationTableColumnSelectedLabel;
        }
    }
}

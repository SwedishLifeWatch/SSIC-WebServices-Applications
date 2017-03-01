using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.Table;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation
{
    /// <summary>
    /// This class stores view settings
    /// </summary>
    [DataContract]
    public sealed class PresentationTableSetting : SettingBase
    {
        private ObservableCollection<PresentationTableType> _selectedTableTypes;

        /// <summary>
        /// Gets or sets whether PresentationViewSetting is active or not.
        /// </summary>        
        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public SpeciesObservationTableSetting SpeciesObservationTable { get; set; }

        [DataMember]
        public SpeciesObservationTaxonTableSetting SpeciesObservationTaxonTable { get; set; }

        [DataMember]
        public ObservableCollection<PresentationTableType> SelectedTableTypes
        {
            get
            {
                if (_selectedTableTypes.IsNull())
                {
                    _selectedTableTypes = new ObservableCollection<PresentationTableType>();
                }
                return _selectedTableTypes;
            }

            set
            {
                _selectedTableTypes = value;
            }
        }

        /// <summary>
        /// Determines whether any settings has been done.
        /// </summary>                
        public override bool HasSettings
        {
            get
            {
                return false;                
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationTableSetting"/> class.
        /// </summary>
        public PresentationTableSetting()
        {            
            IsActive = true;
            ResetSettings();
        }

        public void ResetSettings()
        {
            SelectedTableTypes.Clear();
            SelectedTableTypes.Add(PresentationTableType.SpeciesObservationTable);
        }

        public override bool IsSettingsDefault()
        {
            if (SelectedTableTypes.Count == 1 && SelectedTableTypes[0] == PresentationTableType.SpeciesObservationTable)
            {
                return true;
            }
            return false;
        }
    }
}

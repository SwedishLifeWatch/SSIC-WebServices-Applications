using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.Table;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Presentation;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Presentation
{
    /// <summary>
    /// This class is a view manager for handling data sources operations using the MySettings object.
    /// </summary>
    public class SpeciesObservationTableSettingsViewManager : ViewManagerBase
    {
        /// <summary>
        /// Gets the table setting that exists in MySettings.
        /// </summary>
        public PresentationTableSetting TableSetting
        {
            get { return MySettings.Presentation.Table; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProvidersViewManager"/> class.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="mySettings">The MySettings object.</param>
        public SpeciesObservationTableSettingsViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        public void UpdateTableSettings(SpeciesObservationTableSettingsViewModel tableSettingsModel)
        {
            TableSetting.SpeciesObservationTable.UseUserDefinedTableType = tableSettingsModel.UseUserDefinedTableType;
            TableSetting.SpeciesObservationTable.SelectedTableId = tableSettingsModel.SelectedTableId;
            TableSetting.SpeciesObservationTable.UseLabelAsColumnHeader = tableSettingsModel.UseLabelAsColumnHeader;
        }

        public SpeciesObservationTableSettingsViewModel CreateSpeciesObservationTableSettingsViewModel()
        {
            var model = new SpeciesObservationTableSettingsViewModel();            
            model.UseUserDefinedTableType = TableSetting.SpeciesObservationTable.UseUserDefinedTableType;
            model.SelectedTableId = TableSetting.SpeciesObservationTable.SelectedTableId;
            model.UseLabelAsColumnHeader = TableSetting.SpeciesObservationTable.UseLabelAsColumnHeader;
            model.SystemDefinedTables = TableSetting.SpeciesObservationTable.SystemDefinedTables;            
            model.UserDefinedTables = new List<TableTypeViewModel>();
            for (int i = 0; i < TableSetting.SpeciesObservationTable.UserDefinedTables.Count; i++)
            {
                UserDefinedTable table = TableSetting.SpeciesObservationTable.UserDefinedTables[i];
                model.UserDefinedTables.Add(new TableTypeViewModel(i, table.Title));
            }
            model.IsSettingsDefault = TableSetting.SpeciesObservationTable.IsSettingsDefault();
            return model;
        }
    }
}

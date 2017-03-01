using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.Table;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Presentation;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Presentation
{
    /// <summary>
    /// This class is a view manager for handling data sources operations using the MySettings object.
    /// </summary>
    public class TableSettingsViewManager : ViewManagerBase
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
        public TableSettingsViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        public PresentationTableViewModel CreatePresentationTableViewModel()
        {
            var model = new PresentationTableViewModel();            
            model.SelectedTableTypes = TableSetting.SelectedTableTypes.Select(tableType => (int)tableType).ToList();
            model.Tables = new List<PresentationTableTypeViewModel>();

            var speciesObservationTable = new PresentationTableTypeViewModel();
            speciesObservationTable.Title = Resource.PresentationSpeciesObservationTable;
            speciesObservationTable.Id = (int)PresentationTableType.SpeciesObservationTable;
            speciesObservationTable.HasSettings = true;
            speciesObservationTable.PageInfo = PageInfoManager.GetPageInfo("Result", "SpeciesObservationTable");
            speciesObservationTable.IsSelected = TableSetting.SelectedTableTypes.Any(id => id == PresentationTableType.SpeciesObservationTable);
            model.Tables.Add(speciesObservationTable);

            var gridStatisticsTable = new PresentationTableTypeViewModel();
            gridStatisticsTable.Title = Resource.PresentationGridStatisticsTable;
            gridStatisticsTable.Id = (int)PresentationTableType.GridStatisticsTable;
            gridStatisticsTable.HasSettings = false;
            gridStatisticsTable.PageInfo = null; // no settings in presentation
            bool viewGridStatistics = TableSetting.SelectedTableTypes.Any(id => id == PresentationTableType.GridStatisticsTable);
            gridStatisticsTable.IsSelected = false || viewGridStatistics;
            model.Tables.Add(gridStatisticsTable);
            model.IsSettingsDefault = TableSetting.IsSettingsDefault();

            model.SelectedSpeciesObservationTableName = TableSetting.SpeciesObservationTable.GetSelectedObservationTableName();
            //var gridTaxonStatisticsTable = new PresentationTableTypeViewModel();
            //gridTaxonStatisticsTable.Title = Resource.PresentationGridTaxonStatisticsTable;
            //gridTaxonStatisticsTable.Id = (int)PresentationTableType.GridTaxonStatisticsTable;
            //gridTaxonStatisticsTable.PageInfo = null; // no settings in presentation
            //gridTaxonStatisticsTable.IsSelected = TableSetting.SelectedTableTypes.Any(id => id == PresentationTableType.GridTaxonStatisticsTable);
            //model.Tables.Add(gridTaxonStatisticsTable);

            return model;
        }

        public void UpdateTableSettings(PresentationTableViewModel tableSettings)
        {
            List<PresentationTableType> selectedTableTypes = new List<PresentationTableType>();
            foreach (int tableTypeId in tableSettings.SelectedTableTypes)
            {
                try
                {
                    selectedTableTypes.Add((PresentationTableType)tableTypeId);
                }
                catch
                {                    
                }
            }
            TableSetting.SelectedTableTypes = new ObservableCollection<PresentationTableType>(selectedTableTypes);
        }
    }
}

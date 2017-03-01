using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Calculation
{
    /// <summary>
    /// This class contains settings summary for data provider settings.
    /// </summary>
    public class GridStatisticsSettingSummary : MySettingsSummaryItemBase
    {
        public GridStatisticsSettingSummary()
        {
            SupportDeactivation = false;
        }

        private GridStatisticsSetting GridStatisticsSetting
        {
            get { return SessionHandler.MySettings.Calculation.GridStatistics; }
        }

        public override string Title
        {
            get
            {
                return Resources.Resource.StateButtonCalculationGridStatistics;                
            }
        }

        public override PageInfo PageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Calculation", "GridStatistics");
            }
        }

        public override bool HasSettingsSummary
        {
            get { return true; }
        }

        public GridStatisticsViewModel GetSettingsSummaryModel(IUserContext userContext)
        {
            var viewManager = new GridStatisticsViewManager(userContext, SessionHandler.MySettings);
            GridStatisticsViewModel model = viewManager.CreateGridStatisticsViewModel();
            return model;
        }
        
        public override int? SettingsSummaryWidth
        {
            get { return 350; }
        }        

        public override bool IsActive
        {
            get { return GridStatisticsSetting.IsActive; }
            set { GridStatisticsSetting.IsActive = value; }
        }

        public override bool HasSettings
        {
            get { return GridStatisticsSetting.HasSettings; }
        }

        public override MySettingsSummaryItemIdentifier Identifier
        {
            get { return MySettingsSummaryItemIdentifier.CalculationGridStatistics; }
        }

        // Såhär skulle man kunna generera olika summary settings beroende på vilken resultatsida
        // man är inne på, för att visa vilka inställningar som påverkar just det resultatet.
        //public ImprovedMySettingsSummaryItemBase2 GetExtensiveRepresentation(ResultType? resultType)
        //{
        //    if (resultType.Value == ResultType.SpeciesObservationGridTable)
        //    {
                
        //    }
        //}

        //public ImprovedMySettingsSummaryItemBase2 GetExtensiveRepresentation()
        //{
        //    GridStatisticsViewModel gridStatisticsViewModel = GetSettingsSummaryModel(null);
        //    MySettingsSummaryHierarchical hierarchical = new MySettingsSummaryHierarchical();

        //    // gridPropertiesGroup
        //    MySettingsSummaryHierarchicalGroup gridPropertiesGroup = new MySettingsSummaryHierarchicalGroup();
        //    gridPropertiesGroup.Title = Resource.GridStatisticsParameters;
        //    gridPropertiesGroup.Items = new List<string>();
        //    gridPropertiesGroup.Items.Add(string.Format("{0}: {1}", Resource.GridStatisticsCoordinateSystem, gridStatisticsViewModel.GetSelectedCoordinateSystemName()));
        //    gridPropertiesGroup.Items.Add(string.Format("{0}: {1}", Resource.GridStatisticsGridSize, gridStatisticsViewModel.GetGridSizeFormatted()));
        //    MySettingsSummaryList gridPropertiesGroupList = new MySettingsSummaryList();
        //    gridPropertiesGroupList.Add(string.Format("{0}: {1}", Resource.GridStatisticsCoordinateSystem, gridStatisticsViewModel.GetSelectedCoordinateSystemName()));
        //    gridPropertiesGroupList.Add(string.Format("{0}: {1}", Resource.GridStatisticsGridSize, gridStatisticsViewModel.GetGridSizeFormatted()));
        //    gridPropertiesGroup.Items2 = gridPropertiesGroupList;            
        //    gridPropertiesGroupList.AffectedResultTypes = GetGridResultTypes();
        //    hierarchical.Groups.Add(gridPropertiesGroup);

        //    // grid cells
        //    MySettingsSummaryHierarchicalGroup gridCellsGroup = new MySettingsSummaryHierarchicalGroup();
        //    gridCellsGroup.Title = Resource.GridStatisticsGridCells;
        //    gridCellsGroup.Items = new List<string>();
        //    gridCellsGroup.Items.Add(string.Format("{0}: {1}", Resource.GridStatisticsGenerateAllGridCells, gridStatisticsViewModel.GenerateAllGridCells ? Resource.SharedDialogButtonTextYes : Resource.SharedDialogButtonTextNo));            
        //    MySettingsSummaryList gridCellsGroupList = new MySettingsSummaryList();
        //    gridCellsGroupList.Add(string.Format("{0}: {1}", Resource.GridStatisticsGenerateAllGridCells, gridStatisticsViewModel.GenerateAllGridCells ? Resource.SharedDialogButtonTextYes : Resource.SharedDialogButtonTextNo));
        //    gridCellsGroupList.AffectedResultTypes = GetGridResultTypes();
        //    gridCellsGroup.Items2 = gridCellsGroupList;
        //    hierarchical.Groups.Add(gridCellsGroup);

        //    // calculations
        //    MySettingsSummaryHierarchicalGroup calculationsGroup = new MySettingsSummaryHierarchicalGroup();
        //    calculationsGroup.Title = Resource.GridStatisticsCalculations;
        //    calculationsGroup.Items = new List<string>();
        //    foreach (string str in gridStatisticsViewModel.GetCalculateStrings())
        //    {
        //        calculationsGroup.Items.Add(str);
        //    }                    
        //    MySettingsSummaryList calculationsGroupList = new MySettingsSummaryList();
        //    foreach (string str in gridStatisticsViewModel.GetCalculateStrings())
        //    {
        //        calculationsGroupList.Add(str);
        //    }
        //    calculationsGroupList.AffectedResultTypes = GetGridResultTypes();
        //    calculationsGroup.Items2 = calculationsGroupList;
        //    hierarchical.Groups.Add(calculationsGroup);

        //    // environmental data
        //    MySettingsSummaryHierarchicalGroup environmentalDataGroup = new MySettingsSummaryHierarchicalGroup();
        //    environmentalDataGroup.Title = Resource.GridStatisticsEnvironmentalData;
        //    environmentalDataGroup.Items = new List<string>();
        //    environmentalDataGroup.Items.Add(string.Format("{0}: {1}", Resource.SharedLayer, gridStatisticsViewModel.GetSelectedWfsLayerName()));
        //    environmentalDataGroup.Items.Add(string.Format("{0}: {1}", Resource.SharedCalculate, gridStatisticsViewModel.GetWfsCalculationModeText()));
        //    MySettingsSummaryList environmentalDataGroupList = new MySettingsSummaryList();
        //    environmentalDataGroupList.Add(string.Format("{0}: {1}", Resource.SharedLayer, gridStatisticsViewModel.GetSelectedWfsLayerName()));
        //    environmentalDataGroupList.Add(string.Format("{0}: {1}", Resource.SharedCalculate, gridStatisticsViewModel.GetWfsCalculationModeText()));
        //    environmentalDataGroupList.AffectedResultTypes = new List<ResultType>
        //    {
        //        ResultType.CombinedGridStatisticsTable,
        //        ResultType.WfsStatisticsGridMap
        //    };
        //    environmentalDataGroup.Items2 = environmentalDataGroupList;
        //    hierarchical.Groups.Add(environmentalDataGroup);

        //    return hierarchical;
        //}

        //private static List<ResultType> GetGridResultTypes()
        //{
        //    var gridResultTypes = new List<ResultType>
        //    {
        //        ResultType.CombinedGridStatisticsTable,
        //        ResultType.SpeciesObservationGridMap,
        //        ResultType.SpeciesObservationGridTable,
        //        ResultType.SpeciesRichnessGridMap,
        //        ResultType.SpeciesRichnessGridTable,
        //        ResultType.WfsStatisticsGridMap
        //    };
        //    return gridResultTypes;
        //}

        //public bool SettingAffectsTheResult(ResultType resultType)
        //{
        //    return true; // taxa filter can affect all type of results.            
        //}
    }
}

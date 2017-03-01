using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Improved.Calculation
{
    /// <summary>
    /// This class contains settings summary for data provider settings.
    /// </summary>
    public class ImprovedGridStatisticsSettingSummary : ImprovedMySettingsSummaryItemBase
    {
        public ImprovedGridStatisticsSettingSummary(IUserContext userContext, MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        private GridStatisticsSetting GridStatisticsSetting
        {
            get { return SessionHandler.MySettings.Calculation.GridStatistics; }
        }

        public override MySettingsSummaryItemIdentifierModel IdentifierModel
        {
            get
            {
                List<MySettingsSummaryItemSubIdentifier> subIdentifiers = new List<MySettingsSummaryItemSubIdentifier>();
                subIdentifiers.Add(MySettingsSummaryItemSubIdentifier.CalculationGridStatisticsSubGrid);
                subIdentifiers.Add(MySettingsSummaryItemSubIdentifier.CalculationGridStatisticsSubCalculation);
                subIdentifiers.Add(MySettingsSummaryItemSubIdentifier.CalculationGridStatisticsSubEnvironment);
                return new MySettingsSummaryItemIdentifierModel(MySettingsSummaryItemIdentifier.CalculationGridStatistics, subIdentifiers);
            }
        }

        public override MySettingsSummaryItemIdentifier Identifier
        {
            get { return MySettingsSummaryItemIdentifier.CalculationGridStatistics; }
        }

        public override List<MySettingsSummaryItemSubIdentifier> SubIdentifiers
        {
            get
            {
                List<MySettingsSummaryItemSubIdentifier> subIdentifiers = new List<MySettingsSummaryItemSubIdentifier>();
                subIdentifiers.Add(MySettingsSummaryItemSubIdentifier.CalculationGridStatisticsSubGrid);
                subIdentifiers.Add(MySettingsSummaryItemSubIdentifier.CalculationGridStatisticsSubCalculation);
                subIdentifiers.Add(MySettingsSummaryItemSubIdentifier.CalculationGridStatisticsSubEnvironment);
                return subIdentifiers;
            }
        }

        public override List<MySettingsSummaryItemIdentifier> SubIdentifiers2
        {
            get
            {
                List<MySettingsSummaryItemIdentifier> subIdentifiers = new List<MySettingsSummaryItemIdentifier>();
                subIdentifiers.Add(MySettingsSummaryItemIdentifier.CalculationGridStatisticsSubGrid);
                subIdentifiers.Add(MySettingsSummaryItemIdentifier.CalculationGridStatisticsSubCalculation);
                subIdentifiers.Add(MySettingsSummaryItemIdentifier.CalculationGridStatisticsSubEnvironment);
                return subIdentifiers;
            }
        }

        public static List<ResultType> GetAffectedResultTypes()
        {
            // Borde troligtvis istället returnera en lista med alla resultattyper 
            // som anger true/false för om inställningen påverkar resultatet.
            Dictionary<ResultType, bool> dicAffectedResultTypes = new Dictionary<ResultType, bool>();
            // Då kan man skriva ett enhetstest som automatiskt testar ifall man har uppdaterat och lagt till
            // nya inställningar och resultattyper???
            // men... eftersom vi kör GetAllResultTypesExcept(...) så funkar inte det så bra.

            return MySettingsSummaryResultTypeManager.GetAllResultTypesExcept(
                ResultType.SpeciesObservationMap, 
                ResultType.SpeciesObservationTable, 
                ResultType.TimeSeriesOnSpeciesObservationAbundanceIndexHistogram);
        }

        ////public Dictionary<MySettingsSummaryItemIdentifier, List<ResultType>> GetAffectedResultTypeDictionarySub()
        ////{
            
        ////}

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
        }

        public override bool HasSettings
        {
            get { return GridStatisticsSetting.HasSettings; }
        }
    }
}

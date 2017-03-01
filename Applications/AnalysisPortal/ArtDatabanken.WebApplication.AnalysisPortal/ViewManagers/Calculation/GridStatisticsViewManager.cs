using System;
using System.Collections.Generic;
using System.Globalization;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Calculation
{
    /// <summary>
    /// This class is a view manager Grid statistics
    /// </summary>
    public class GridStatisticsViewManager : ViewManagerBase
    {
        /// <summary>
        /// Gets the grid statistics setting that exists in MySettings.
        /// </summary>
        public GridStatisticsSetting GridStatisticsSetting
        {
            get { return MySettings.Calculation.GridStatistics; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridStatisticsViewManager"/> class.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="mySettings">The MySettings object.</param>
        public GridStatisticsViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        /// <summary>
        /// Updates the grid statistics in MySettings.
        /// </summary>        
        public void UpdateGridStatistics(GridStatisticsViewModel model)
        {
            GridStatisticsSetting.GridSize = model.GridSize;
            GridStatisticsSetting.GenerateAllGridCells = model.GenerateAllGridCells;
            GridStatisticsSetting.CalculateNumberOfObservations = model.CalculateNumberOfObservations;
            GridStatisticsSetting.CalculateNumberOfTaxa = model.CalculateNumberOfTaxa;
            GridStatisticsSetting.CoordinateSystemId = model.CoordinateSystemId;
            GridStatisticsSetting.WfsGridStatisticsLayerId = model.WfsGridStatisticsLayerId;
            GridStatisticsSetting.WfsGridStatisticsCalculationModeId = (int)model.WfsGridStatisticsCalculationMode;
        }

        /// <summary>
        /// Creates a view model for the "~/Calculation/GridStatistics" view
        /// </summary>
        public GridStatisticsViewModel CreateGridStatisticsViewModel()
        {            
            var model = new GridStatisticsViewModel();
            model.GenerateAllGridCells = GridStatisticsSetting.GenerateAllGridCells;
            model.CalculateNumberOfObservations = GridStatisticsSetting.CalculateNumberOfObservations;
            model.GridSize = GridStatisticsSetting.GridSize;
            model.CoordinateSystemId = GridStatisticsSetting.CoordinateSystemId;
            model.CalculateNumberOfTaxa = GridStatisticsSetting.CalculateNumberOfTaxa;

            model.CoordinateSystems = new List<CoordinateSystemViewModel>();
            model.CoordinateSystems.Add(new CoordinateSystemViewModel((int)GridCoordinateSystem.SWEREF99_TM, "SWEREF 99", GridStatisticsSetting.CoordinateSystemId.GetValueOrDefault(-100) == (int)GridCoordinateSystem.SWEREF99_TM));
            model.CoordinateSystems.Add(new CoordinateSystemViewModel((int)GridCoordinateSystem.Rt90_25_gon_v, "RT 90", GridStatisticsSetting.CoordinateSystemId.GetValueOrDefault(-100) == (int)GridCoordinateSystem.Rt90_25_gon_v));
            
            // WFS Grid statistics
            WfsGridStatisticsCalculationMode wfsGridStatisticsCalculationMode;
            if (Enum.TryParse(GridStatisticsSetting.WfsGridStatisticsCalculationModeId.ToString(CultureInfo.InvariantCulture), out wfsGridStatisticsCalculationMode))
            {
                model.WfsGridStatisticsCalculationMode = wfsGridStatisticsCalculationMode;
            }
            else
            {
                model.WfsGridStatisticsCalculationMode = WfsGridStatisticsCalculationMode.Count;
            }
            model.WfsGridStatisticsLayerId = GridStatisticsSetting.WfsGridStatisticsLayerId;

            var wfsViewManager = new WfsLayersViewManager(UserContext, MySettings);
            model.WfsLayers = wfsViewManager.CreateWfsLayersList();

            model.IsSettingsDefault = GridStatisticsSetting.IsSettingsDefault();
            return model;
        }

        /// <summary>
        /// Creates a view model for the "~/Result/ResultSpeciesObservationGridTable" view
        /// </summary>        
        public ResultSpeciesObservationGridTableViewModel CreateResultSpeciesObservationGridTableViewModel()
        {
            var model = new ResultSpeciesObservationGridTableViewModel();
            if (GridStatisticsSetting.CoordinateSystemId != null)
            {
                GridCoordinateSystem coordinateSystem = (GridCoordinateSystem)GridStatisticsSetting.CoordinateSystemId;
                switch (coordinateSystem)
                {
                    //case GridCoordinateSystem.RT90:
                    case GridCoordinateSystem.Rt90_25_gon_v:
                        model.OriginalCoordinateSystemName = "RT 90";
                        break;
                    case GridCoordinateSystem.SWEREF99_TM:
                        model.OriginalCoordinateSystemName = "SWEREF 99";
                        break;
                    default:
                        model.OriginalCoordinateSystemName = "";
                        break;
                }
            }
            
            model.CoordinateSystemName = MySettings.Presentation.Map.PresentationCoordinateSystemId.GetCoordinateSystemName();
            model.CentreCoordinateName = Resources.Resource.GridStatisticsCentreCoordinate;
            return model;
        }

        /// <summary>
        /// Creates a view model for the "~/Result/ResultTaxaGridTable" view
        /// </summary>        
        public ResultTaxonGridTableViewModel CreateResultTaxaGridTableViewModel()
        {
            var model = new ResultTaxonGridTableViewModel();
            if (GridStatisticsSetting.CoordinateSystemId != null)
            {
                GridCoordinateSystem coordinateSystem = (GridCoordinateSystem)GridStatisticsSetting.CoordinateSystemId;
                switch (coordinateSystem)
                {
                    //case GridCoordinateSystem.RT90:
                    case GridCoordinateSystem.Rt90_25_gon_v:
                        model.OriginalCoordinateSystemName = "RT 90";
                        break;
                    case GridCoordinateSystem.SWEREF99_TM:
                        model.OriginalCoordinateSystemName = "SWEREF 99";
                        break;
                    default:
                        model.OriginalCoordinateSystemName = "";
                        break;
                }
            }

            model.CoordinateSystemName = MySettings.Presentation.Map.PresentationCoordinateSystemId.GetCoordinateSystemName();
            model.CentreCoordinateName = Resources.Resource.GridStatisticsCentreCoordinate;
            return model;
        }
    }
}

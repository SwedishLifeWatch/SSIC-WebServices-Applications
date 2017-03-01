using System;
using System.Collections.Generic;
using System.Globalization;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Result
{
    public class WfsGridStatisticsViewManager : ViewManagerBase
    {
        public WfsGridStatisticsViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        /// <summary>
        /// Gets the grid statistics setting that exists in MySettings.
        /// </summary>
        public GridStatisticsSetting GridStatisticsSetting
        {
            get { return MySettings.Calculation.GridStatistics; }
        }

        public WfsGridStatisticsMapViewModel CreateViewModel()
        {
            WfsGridStatisticsMapViewModel model = new WfsGridStatisticsMapViewModel();
            model.GridSize = GridStatisticsSetting.GridSize;
            //model.CoordinateSystemId = GridStatisticsSetting.CoordinateSystemId;
            model.CoordinateSystems = new List<CoordinateSystemViewModel>();
            model.CoordinateSystems.Add(new CoordinateSystemViewModel((int)GridCoordinateSystem.Rt90_25_gon_v, "RT 90", GridStatisticsSetting.CoordinateSystemId.GetValueOrDefault(-100) == (int)GridCoordinateSystem.Rt90_25_gon_v));
            model.CoordinateSystems.Add(new CoordinateSystemViewModel((int)GridCoordinateSystem.SWEREF99_TM, "SWEREF 99", GridStatisticsSetting.CoordinateSystemId.GetValueOrDefault(-100) == (int)GridCoordinateSystem.SWEREF99_TM));

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

            model.AddSpartialFilterLayer = MySettings.Filter.Spatial.IsActive && MySettings.Filter.Spatial.HasSettings;

            return model;
        }

        //public ViewTableViewModel CreateWfsGridStatisticsViewModel()
        //{

        //    String featuresUrl;
        //    CoordinateSystem coordinateSystem;

        //    FeatureStatisticsSummary featureStatisticsSummary;
        //    GridSpecification gridSpecification;

        //    gridSpecification = new GridSpecification();
        //    coordinateSystem = new CoordinateSystem();
        //    featureStatisticsSummary = new FeatureStatisticsSummary();
        //    featureStatisticsSummary.FeatureType = FeatureType.Multipolygon;
        //    featureStatisticsSummary.BoundingBox = new BoundingBox();
        //    featureStatisticsSummary.BoundingBox.Max = new Point();
        //    featureStatisticsSummary.BoundingBox.Min = new Point();

        //    gridSpecification.BoundingBox = new BoundingBox();
        //    gridSpecification.BoundingBox.Max = new Point();
        //    gridSpecification.BoundingBox.Min = new Point();

        //    //This bounding box is parsed and extracted from the url?
        //    featureStatisticsSummary.BoundingBox.Max.Y = 1500001;
        //    featureStatisticsSummary.BoundingBox.Max.X = 6900001;
        //    featureStatisticsSummary.BoundingBox.Min.Y = 1499001;
        //    featureStatisticsSummary.BoundingBox.Min.X = 6875163;

        //    gridSpecification.BoundingBox.Max.Y = 1489104;
        //    gridSpecification.BoundingBox.Max.X = 6858363;
        //    gridSpecification.BoundingBox.Min.Y = 1400000;
        //    gridSpecification.BoundingBox.Min.X = 6800000;

        //    gridSpecification.GridCellSize = 100000;
        //    gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
        //    gridSpecification.IsGridCellSizeSpecified = true;
        //    gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
        //    coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;

        //    featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&version=1.1.0&typeName=SLW:MapOfSwedishCounties&srsName=EPSG:3021";

        //    List<IGridCellFeatureStatistics> list = CoreData.AnalysisManager.GetGridFeatureStatistics(UserContext, featureStatisticsSummary, featuresUrl, gridSpecification, coordinateSystem);
        //    int x = 8;
        //    return null;
        //    //List<IGridCellFeatureStatistics> gridCellFeatureStatistics = CoreData.AnalysisManager.GetGridCellFeatureStatistics(GetUserContext(), featureStatisticsSummary, featuresUrl,
        //    //                                                                                          gridSpecification, coordinateSystem);
        //    //System.Diagnostics.Debug.Assert();
        //    //Assert.IsTrue(gridCellFeatureStatistics.Count > 0);
        //    //Assert.IsTrue(gridCellFeatureStatistics.Count.Equals(4));
        //    //Assert.IsTrue(gridCellFeatureStatistics[0].GridCellBoundingBox.LinearRings[0].Points[0].Y.Equals(1400000));
        //    //Assert.IsTrue(gridCellFeatureStatistics[3].GridCellBoundingBox.LinearRings[0].Points[2].X.Equals(7000000));             

        //    //ViewTableViewModel model = new ViewTableViewModel();
        //    //PagedSpeciesObservationTableResultCalculator resultCalculator = new PagedSpeciesObservationTableResultCalculator(UserContext, MySettings);
        //    //model.ComplexityEstimate = resultCalculator.GetQueryComplexityEstimate();
        //    ////model.ComplexityEstimate = QueryComplexityManager.GetQueryComplexityEstimate(ResultType.SpeciesObservationTable, UserContext, MySettings);

        //    //List<ISpeciesObservationFieldDescription> fields = PresentationTableSetting.SpeciesObservationTable.GetTableFields(UserContext);
        //    //var tableFields = new List<ViewTableField>();
        //    //foreach (ISpeciesObservationFieldDescription field in fields)
        //    //{
        //    //    ViewTableField viewTableField = new ViewTableField(field.Label, field.Name.FirstLetterToUpper());
        //    //    tableFields.Add(viewTableField);
        //    //}
        //    //model.TableFields = tableFields;
        //    //return model;
        //}
    }
}

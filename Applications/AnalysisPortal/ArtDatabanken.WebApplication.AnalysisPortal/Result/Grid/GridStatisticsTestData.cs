using System;
using System.Collections.Generic;
using ArtDatabanken.GIS.CoordinateSystems;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid
{
    public static class GridStatisticsTestData
    {
        private static readonly Random _random = new Random();

        public static SpeciesObservationGridResult CreateGridCellTestDataUsingWgs84(int nrRows, int nrColumns, int gridSize)
        {
            var result = new SpeciesObservationGridResult();

            //var start = Wgs84Util.GoogleMercatorToWGS84(1716164.1588198, 8507342.0363544);

            //double[] upperLeftStartCoordinate = {15.569241, 58.3898709}; // Linköping
            double[] leftUpperStartCoordinate = { 15.416564941405483, 60.479793034510955 }; // Borlänge
            //double[] upperLeftStartCoordinate = start;

            //double[] currentSouthWestCoordinate;
            //double[] currentNorthEastCoordinate;

            result.GridCellCoordinateSystem = "GoogleMercator";
            result.GridCellCoordinateSystemId = 3;
            result.GridCellSize = gridSize;
            result.Cells = new List<SpeciesObservationGridCellResult>();

            for (int i = 0; i < nrRows; i++)
            {
                for (int j = 0; j < nrColumns; j++)
                {
                    double[] currentWestNorthCoordinate = Wgs84Util.TranslateCoordinate(leftUpperStartCoordinate, j * gridSize, -i * gridSize);
                    double[] currentEastSouthCoordinate = Wgs84Util.TranslateCoordinate(currentWestNorthCoordinate, gridSize, -gridSize);
                    double[] currentCentreCoordinate = Wgs84Util.TranslateCoordinate(currentWestNorthCoordinate, gridSize / 2.0, -gridSize / 2.0);

                    double[] mercatorCurrentWestNorthCoordinate = Wgs84Util.WGS84ToGoogleMercator(currentWestNorthCoordinate);
                    double[] mercatorCurrentEastSouthCoordinate = Wgs84Util.WGS84ToGoogleMercator(currentEastSouthCoordinate);
                    double[] mercatorCurrentCentreCoordinate = Wgs84Util.WGS84ToGoogleMercator(currentCentreCoordinate);

                    var gridCell = new SpeciesObservationGridCellResult();
                    gridCell.CentreCoordinateX = mercatorCurrentCentreCoordinate[0];
                    gridCell.CentreCoordinateY = mercatorCurrentCentreCoordinate[1];
                    gridCell.CentreCoordinate = mercatorCurrentCentreCoordinate;

                    gridCell.BoundingBox[0] = new[] { mercatorCurrentWestNorthCoordinate[0], mercatorCurrentEastSouthCoordinate[1] };
                    gridCell.BoundingBox[2] = new[] { mercatorCurrentEastSouthCoordinate[0], mercatorCurrentWestNorthCoordinate[1] };

                    gridCell.ObservationCount = _random.Next(0, 200);
                    result.Cells.Add(gridCell);
                }
            }

            return result;
        }

        public static SpeciesObservationGridResult CreateGridCellTestDataUsingGoogleMercator(int nrRows, int nrColumns, int gridSize)
        {
            var result = new SpeciesObservationGridResult();

            //double[] upperLeftStartCoordinate = {15.569241, 58.3898709}; // Linköping
            //double[] leftUpperStartCoordinate = { 15.416564941405483, 60.479793034510955 }; // Borlänge

            double[] centerStartCoordinate = { 15.416564941405483, 60.479793034510955 }; // Borlänge
            double[] mercatorCenterStartCoordinate = Wgs84Util.WGS84ToGoogleMercator(centerStartCoordinate);

            //double[] currentSouthWestCoordinate;
            //double[] currentNorthEastCoordinate;

            result.GridCellCoordinateSystem = "GoogleMercator";
            result.GridCellCoordinateSystemId = 3;
            result.GridCellSize = gridSize;
            result.Cells = new List<SpeciesObservationGridCellResult>();

            for (int i = 0; i < nrRows; i++)
            {
                for (int j = 0; j < nrColumns; j++)
                {
                    double[] mercatorCenterCoordinate = new double[] { mercatorCenterStartCoordinate[0] + (j * gridSize), mercatorCenterStartCoordinate[1] - (i * gridSize) };
                    double halfGridSize = gridSize / 2.0;

                    //double[] currentWestNorthCoordinate = Wgs84Util.TranslateCoordinate(leftUpperStartCoordinate, j * gridSize, -i * gridSize);
                    //double[] currentEastSouthCoordinate = Wgs84Util.TranslateCoordinate(currentWestNorthCoordinate, gridSize, -gridSize);
                    //double[] currentCentreCoordinate = Wgs84Util.TranslateCoordinate(currentWestNorthCoordinate, gridSize / 2.0, -gridSize / 2.0);

                    //double[] mercatorCurrentWestNorthCoordinate = Wgs84Util.WGS84ToGoogleMercator(currentWestNorthCoordinate);
                    //double[] mercatorCurrentEastSouthCoordinate = Wgs84Util.WGS84ToGoogleMercator(currentEastSouthCoordinate);
                    //double[] mercatorCurrentCentreCoordinate = Wgs84Util.WGS84ToGoogleMercator(currentCentreCoordinate);

                    var gridCell = new SpeciesObservationGridCellResult();
                    gridCell.CentreCoordinateX = mercatorCenterCoordinate[0];
                    gridCell.CentreCoordinateY = mercatorCenterCoordinate[1];
                    gridCell.CentreCoordinate = mercatorCenterCoordinate;

                    gridCell.BoundingBox[0] = new[] { mercatorCenterCoordinate[0] - halfGridSize, mercatorCenterCoordinate[1] - halfGridSize };
                    gridCell.BoundingBox[2] = new[] { mercatorCenterCoordinate[0] + halfGridSize, mercatorCenterCoordinate[1] + halfGridSize };

                    gridCell.ObservationCount = _random.Next(0, 200);
                    result.Cells.Add(gridCell);
                }
            }

            return result;
        }
    }
}

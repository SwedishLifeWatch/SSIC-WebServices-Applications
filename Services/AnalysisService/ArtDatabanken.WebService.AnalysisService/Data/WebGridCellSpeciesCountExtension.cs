using System;
using System.Collections.Generic;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.AnalysisService.Database;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.AnalysisService.Data
{
    /// <summary>
    /// Contains extension to the WebGridCellSpeciesCount class.
    /// </summary>
    public static class WebGridCellSpeciesCountExtension
    {
        /// <summary>
        /// Load data into the WebGridCellSpeciesCount instance.
        /// </summary>
        /// <param name="gridCellObservationCount"> Information on species counts.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebGridCellSpeciesCount gridCellObservationCount,
                                    DataReader dataReader)
        {
            if (gridCellObservationCount != null && dataReader != null)
            {
                // Returning int 32, no problem for gridcells.
                gridCellObservationCount.SpeciesObservationCount = dataReader.GetInt32(ObservationGridCellSearchCriteriaData.SPECIES_OBSERVATION_COUNT);
                gridCellObservationCount.SpeciesCount = dataReader.GetInt32(ObservationGridCellSearchCriteriaData.SPECIES_COUNT);
                gridCellObservationCount.Size = dataReader.GetInt32(ObservationGridCellSearchCriteriaData.GRID_CELL_SIZE);

                // Calculate grid points 
                double centreCoordinateX = Convert.ToDouble(dataReader.GetInt32(ObservationGridCellSearchCriteriaData.GRID_CELL_COORDINATE_X));
                double centreCoordinateY = Convert.ToDouble(dataReader.GetInt32(ObservationGridCellSearchCriteriaData.GRID_CELL_COORDINATE_Y));
                //TODO Create method:
                double halfGridSize = gridCellObservationCount.Size / 2.0;

                double upperCoordinateY = centreCoordinateY + halfGridSize;
                double upperCoordinateX = centreCoordinateX + halfGridSize;
                double lowerCoordinateY = centreCoordinateY - halfGridSize;
                double lowerCoordinateX = centreCoordinateX - halfGridSize;

                // Create Point and BoundingBox
                WebPoint centrePoint = new WebPoint(centreCoordinateX, centreCoordinateY);

                WebPoint pointMax = new WebPoint(upperCoordinateX, upperCoordinateY);
                WebPoint pointMin = new WebPoint(lowerCoordinateX, lowerCoordinateY);

                gridCellObservationCount.CentreCoordinate = centrePoint;
                gridCellObservationCount.OrginalCentreCoordinate = centrePoint;
                //gridCellObservationCount.GridCellBoundingBox = new WebBoundingBox() { Max = pointMax, Min = pointMin };
                gridCellObservationCount.BoundingBox = new WebPolygon();
                gridCellObservationCount.BoundingBox.LinearRings = new List<WebLinearRing>();
                gridCellObservationCount.BoundingBox.LinearRings.Add(new WebLinearRing());
                gridCellObservationCount.BoundingBox.LinearRings[0].Points = new List<WebPoint>();
                gridCellObservationCount.BoundingBox.LinearRings[0].Points.Add(new WebPoint());
                gridCellObservationCount.BoundingBox.LinearRings[0].Points.Add(new WebPoint());
                gridCellObservationCount.BoundingBox.LinearRings[0].Points.Add(new WebPoint());
                gridCellObservationCount.BoundingBox.LinearRings[0].Points.Add(new WebPoint());
                gridCellObservationCount.BoundingBox.LinearRings[0].Points.Add(new WebPoint());
                //Create the linear ring that is the "bounding polygon".
                gridCellObservationCount.BoundingBox.LinearRings[0].Points[0].X = pointMin.X;
                gridCellObservationCount.BoundingBox.LinearRings[0].Points[0].Y = pointMin.Y;
                gridCellObservationCount.BoundingBox.LinearRings[0].Points[1].X = pointMin.X;
                gridCellObservationCount.BoundingBox.LinearRings[0].Points[1].Y = pointMax.Y;
                gridCellObservationCount.BoundingBox.LinearRings[0].Points[2].X = pointMax.X;
                gridCellObservationCount.BoundingBox.LinearRings[0].Points[2].Y = pointMax.Y;
                gridCellObservationCount.BoundingBox.LinearRings[0].Points[3].X = pointMax.X;
                gridCellObservationCount.BoundingBox.LinearRings[0].Points[3].Y = pointMin.Y;
                gridCellObservationCount.BoundingBox.LinearRings[0].Points[4].X = pointMin.X;
                gridCellObservationCount.BoundingBox.LinearRings[0].Points[4].Y = pointMin.Y;

                gridCellObservationCount.OrginalBoundingBox = new WebBoundingBox { Max = pointMax, Min = pointMin };
            }

        }

        /// <summary>
        /// Load data into the WebGridCellSpeciesCount instance.
        /// </summary>
        /// <param name="gridCellObservationCount"> Information on species counts.</param>
        /// <param name="uniqueValue">A DocumentUniqueValue with coordinate, count and sum information</param>
        /// <param name="gridSpecification">The grid specification: bounding box, grid cell size, etc.</param>        
        /// <param name="coordinateSystem"></param>        
        public static void LoadData(this WebGridCellSpeciesCount gridCellObservationCount,
                                         DocumentUniqueValue uniqueValue,
                                         WebGridSpecification gridSpecification,
                                         WebCoordinateSystem coordinateSystem)
        {
            if (gridCellObservationCount != null)
            {
                gridCellObservationCount.SpeciesCount = Convert.ToInt32(uniqueValue.Count);
                LoadData(gridCellObservationCount, new KeyValuePair<string, long>(uniqueValue.Key, uniqueValue.DocumentCount), gridSpecification, coordinateSystem);
            }
        }

        /// <summary>
        /// Load data into the WebGridCellSpeciesCount instance.
        /// </summary>
        /// <param name="gridCellObservationCount"> Information on species counts.</param>
        /// <param name="uniqueValue">A KeyValuePair with coordinate and count information</param>
        /// <param name="gridSpecification">The grid specification: bounding box, grid cell size, etc.</param>        
        /// <param name="coordinateSystem"></param>        
        public static void LoadData(this WebGridCellSpeciesCount gridCellObservationCount,
                                         KeyValuePair<string, long> uniqueValue,
                                         WebGridSpecification gridSpecification,
                                         WebCoordinateSystem coordinateSystem)
        {
            if (gridCellObservationCount != null)
            {
                // Returning int 32, no problem for gridcells.
                gridCellObservationCount.SpeciesObservationCount = uniqueValue.Value;
                gridCellObservationCount.GridCoordinateSystem = gridSpecification.GridCoordinateSystem;
                gridCellObservationCount.CoordinateSystem = coordinateSystem;
                gridCellObservationCount.Size = gridSpecification.GridCellSize;

                // Calculate grid points 
                double centreCoordinateX = uniqueValue.Key.Split(':')[0].WebParseDouble();
                double centreCoordinateY = uniqueValue.Key.Split(':')[1].WebParseDouble();
                //TODO Create method:
                double halfGridSize = gridCellObservationCount.Size / 2.0;

                double upperCoordinateY = centreCoordinateY + halfGridSize;
                double upperCoordinateX = centreCoordinateX + halfGridSize;
                double lowerCoordinateY = centreCoordinateY - halfGridSize;
                double lowerCoordinateX = centreCoordinateX - halfGridSize;

                // Create Point and BoundingBox
                WebPoint centrePoint = new WebPoint(centreCoordinateX, centreCoordinateY);

                WebPoint pointMax = new WebPoint(upperCoordinateX, upperCoordinateY);
                WebPoint pointMin = new WebPoint(lowerCoordinateX, lowerCoordinateY);

                gridCellObservationCount.CentreCoordinate = centrePoint;
                gridCellObservationCount.OrginalCentreCoordinate = centrePoint;
                //gridCellObservationCount.GridCellBoundingBox = new WebBoundingBox() { Max = pointMax, Min = pointMin };
                gridCellObservationCount.BoundingBox = new WebPolygon();
                gridCellObservationCount.BoundingBox.LinearRings = new List<WebLinearRing>();
                gridCellObservationCount.BoundingBox.LinearRings.Add(new WebLinearRing());
                gridCellObservationCount.BoundingBox.LinearRings[0].Points = new List<WebPoint>();
                gridCellObservationCount.BoundingBox.LinearRings[0].Points.Add(new WebPoint());
                gridCellObservationCount.BoundingBox.LinearRings[0].Points.Add(new WebPoint());
                gridCellObservationCount.BoundingBox.LinearRings[0].Points.Add(new WebPoint());
                gridCellObservationCount.BoundingBox.LinearRings[0].Points.Add(new WebPoint());
                gridCellObservationCount.BoundingBox.LinearRings[0].Points.Add(new WebPoint());
                //Create the linear ring that is the "bounding polygon".
                gridCellObservationCount.BoundingBox.LinearRings[0].Points[0].X = pointMin.X;
                gridCellObservationCount.BoundingBox.LinearRings[0].Points[0].Y = pointMin.Y;
                gridCellObservationCount.BoundingBox.LinearRings[0].Points[1].X = pointMin.X;
                gridCellObservationCount.BoundingBox.LinearRings[0].Points[1].Y = pointMax.Y;
                gridCellObservationCount.BoundingBox.LinearRings[0].Points[2].X = pointMax.X;
                gridCellObservationCount.BoundingBox.LinearRings[0].Points[2].Y = pointMax.Y;
                gridCellObservationCount.BoundingBox.LinearRings[0].Points[3].X = pointMax.X;
                gridCellObservationCount.BoundingBox.LinearRings[0].Points[3].Y = pointMin.Y;
                gridCellObservationCount.BoundingBox.LinearRings[0].Points[4].X = pointMin.X;
                gridCellObservationCount.BoundingBox.LinearRings[0].Points[4].Y = pointMin.Y;

                gridCellObservationCount.OrginalBoundingBox = new WebBoundingBox { Max = pointMax, Min = pointMin };
            }

        }

        /// <summary>
        /// Check if grid specifications is correct.
        /// </summary>
        /// <param name="gridSpecification"> Information on grid specifications.</param>
        /// <param name="speciesBoundingBox">Bounding box for species observation.</param>
        public static void CheckGridSpecificationsForSpeciesCount(this WebGridSpecification gridSpecification, WebBoundingBox speciesBoundingBox)
        {

            if (gridSpecification.IsNotNull() && gridSpecification.BoundingBox.IsNotNull())
            {
                if (speciesBoundingBox.IsNotNull())
                {
                    throw new ArgumentException("WebGridSpecifications: Properties WebGridSpecifications.BoundingBox and WebSpeciesObservatioSearchCriteria.BoundingBox have value set, only one BoundigBox can be set.");
                }
            }

        }

    }
}

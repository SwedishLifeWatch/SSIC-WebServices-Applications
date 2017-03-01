using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.GIS.SwedenExtent;
using ArtDatabanken.WebService.Data;
using Microsoft.SqlServer.Types;

namespace ArtDatabanken.WebService.AnalysisService.Data
{    
    /// <summary>
    /// Manager class for creation of sql geometric objects and conversion of GeoJson points
    /// to SqlGeometry points.
    /// </summary>
    public class GeometryManager :IGeometryManager
    {
        /// <summary>
        /// Converts a GeoJson point to the microsoft sql geometry point format.
        /// </summary>
        /// <param name="geoJsonPoints">A list of points in the GeoJSON format.</param>
        /// <param name="srid">The Spatial Reference System Identifier (SRID) is a unique value used 
        /// to unambiguously identify projected,  unprojected, and local spatial coordinate system. 
        /// The srid should be the same as for incoming geoJsonPoints.</param>
        /// <returns>A list of points in the sql geometry format.</returns>
        public static List<SqlGeometry> ConvertGeoJsonPointToSqlGeometry(List<ArtDatabanken.GIS.GeoJSON.Net.Geometry.Point> geoJsonPoints, int srid)
        {
            SqlGeometryBuilder geomBuilder;
            List<SqlGeometry> pointList;
            pointList = new List<SqlGeometry>();

            if (geoJsonPoints != null)
            {
                foreach (ArtDatabanken.GIS.GeoJSON.Net.Geometry.Point point in geoJsonPoints)
                {                    
                    geomBuilder = new SqlGeometryBuilder();

                    // Setting the srid to the same as for the geoJsonPoints
                    geomBuilder.SetSrid(srid);
                    geomBuilder.BeginGeometry(OpenGisGeometryType.Point);
                    geomBuilder.BeginFigure(point.Coordinates.Longitude, point.Coordinates.Latitude);
                    geomBuilder.EndFigure();
                    geomBuilder.EndGeometry();
                    pointList.Add(geomBuilder.ConstructedGeometry.MakeValid());
                }
            }
            return pointList;
        }

        /// <summary>
        /// Creates a polygon from a list of points using the same SRID the points are using. 
        /// The method expects the first and the last point to be the same and will return error
        /// if they are not.
        /// </summary>
        /// <param name="points">A list of SqlGeometry points.</param>
        /// <returns>A plygon in .NET sqlserver geometry type</returns>
        public static SqlGeometry CreatePolygon(List<SqlGeometry> points)
        {
            bool firstIsPassed = false;
            SqlGeometryBuilder geometryBuilder;
            SqlGeometry result = null;
            int srid;

            geometryBuilder = new SqlGeometryBuilder();
            if (points != null)
            {
                srid = (int)points[0].STSrid;

                //Check start and end points
                int numberOfPoints = points.Count();
                double stx1 = points[0].STX.Value;
                double stxLast = points[numberOfPoints - 1].STX.Value;
                double sty1 = points[0].STY.Value;
                double styLast = points[numberOfPoints - 1].STY.Value;

                //Make sure the points are valid for polygon
                if ((!stx1.Equals(stxLast)) && (!sty1.Equals(styLast)))
                {
                    throw new Exception("First and last point in point list are not the same.");
                }
                geometryBuilder.SetSrid(srid);
                geometryBuilder.BeginGeometry(OpenGisGeometryType.Polygon);
                foreach (SqlGeometry point in points)
                {
                    if (firstIsPassed == true)
                    {
                        geometryBuilder.AddLine((double)point.STX, (double)point.STY);
                    }
                    else if (firstIsPassed == false)
                    {
                        geometryBuilder.BeginFigure((double)point.STX, (double)point.STY);
                        firstIsPassed = true;
                    }
                }
                geometryBuilder.EndFigure();
                geometryBuilder.EndGeometry();
                result = geometryBuilder.ConstructedGeometry.MakeValid();                
            }
            return result;
        }

        /// <summary>
        /// Creates a polygon from a list of points using the same SRID the points are using. 
        /// The method expects the first and the last point to be the same and will return error
        /// if they are not.
        /// </summary>
        /// <param name="points">A list of SqlGeometry points.</param>
        /// <returns>A plygon in .NET sqlserver geometry type</returns>
        public static SqlGeometry CreateLineString(List<SqlGeometry> points)
        {
            bool isFirst = false;
            SqlGeometryBuilder geometryBuilder;
            SqlGeometry result = null;
            int srid;

            geometryBuilder = new SqlGeometryBuilder();
            if (points != null)
            {
                srid = (int)points[0].STSrid;

               // int numberOfPoints = points.Count();

                geometryBuilder.SetSrid(srid);
                geometryBuilder.BeginGeometry(OpenGisGeometryType.LineString);
                foreach (SqlGeometry point in points)
                {
                    if (!isFirst)
                    {
                        geometryBuilder.BeginFigure((double)point.STX, (double)point.STY);
                        isFirst = true;
                    }
                    else
                    {
                        geometryBuilder.AddLine((double)point.STX, (double)point.STY);
                    }
                }

                geometryBuilder.EndFigure();
                geometryBuilder.EndGeometry();
                result = geometryBuilder.ConstructedGeometry.MakeValid();
            }

            return result;

        }

        /// <summary>
        /// Converts a list of geometries to a multi-geometry.      
        /// </summary>
        /// <param name="geometryList">A list of geometries.</param>
        /// <returns>A multi geometry in sql geometry type.</returns>
        public static SqlGeometry CreateMultiGeometry(IEnumerable<SqlGeometry> geometryList )
        {
            SqlGeometry result, polygon;
            result = new SqlGeometry();
            result = null;
            if (geometryList != null)
            {
                foreach (SqlGeometry g in geometryList)
                {
                    if (result != null)
                    {
                        polygon = result.STDifference(g);
                        if (polygon.STArea() != result.STArea())
                        {
                            result = result.STDifference(g);
                        }
                        else
                        {
                            result = result.STUnion(g);
                        }
                    }

                    if (result == null)
                    {
                        result = g;
                    }

                    //result = result == null ? g : result.STUnion(g); //Harder to debug...
                }
            }
            
            return result;   
        }

        ///// <summary>
        ///// Not yet implemented. Check a list of sql geometries for errors:
        ///// </summary>
        ///// <param name="checkList">A list of Sql Geometries to be checked.</param>
        ///// <returns>Returns true if geometry checks out allright.</returns>
        //public static bool CheckGeometry(List<SqlGeometry> checkList)
        //{
        //    //Todo:
        //    foreach (SqlGeometry sqlGeometry in checkList)
        //    {
        //        //OpenGisGeometryType.Point():
        //               //Check if first point in list == last
        //               //Check if x and y are in the correct order

        //        //OpenGisGeometryType.Polygon():
        //               //Check for gaps

        //        // All 
        //               //Check if within Sweden
       
        //    }
        //    return false;
        //}


        /// <summary>        
        /// This method will take a list of Sql Geometries, convert them to WebMultiPolygons
        /// and reproject them from the current coordinat system to the target coordinat system.
        /// </summary>
        /// <param name="toCoordinateSystem">The target coordinate system.</param>
        /// <param name="fromCoordinateSystem">The current coordinate system.</param>
        /// <param name="sqlGeometryToBeConvertedList">The slit of Sql Geometries that are to be converted.</param>
        public static List<WebMultiPolygon> ReProjectMultiPolygon(WebCoordinateSystem toCoordinateSystem,
                                                               WebCoordinateSystem fromCoordinateSystem,
                                                               List<SqlGeometry> sqlGeometryToBeConvertedList)
        {
            List<WebMultiPolygon> webMultiPolygonListToBeConverted;
            WebMultiPolygon webMultiPolygonToBeConverted;
            int sridInsqlGeometryToBeConvertedList = 0;
            int sridInfromCoordinateSystem = 0;
            

            webMultiPolygonListToBeConverted = new List<WebMultiPolygon>();
            webMultiPolygonToBeConverted = new WebMultiPolygon();
            if (sqlGeometryToBeConvertedList != null && sqlGeometryToBeConvertedList.Count > 0)
            {
                sridInsqlGeometryToBeConvertedList = (int)sqlGeometryToBeConvertedList[0].STSrid;


                sridInfromCoordinateSystem = GetSridFromWebCoordinateSystem(fromCoordinateSystem);

                if (!sridInfromCoordinateSystem.Equals(sridInsqlGeometryToBeConvertedList))
                {
                    throw new Exception("There is a mismatch between coordinate systems in Sql Geometry list and fromCoordinateSystem.");
                }

                if (toCoordinateSystem.GetWkt().ToUpper() != fromCoordinateSystem.GetWkt().ToUpper())
                {
                    // Todo: Konvertera sqlGeometryToBeConvertedList till WebMultiPolygon
                    foreach (SqlGeometry geom in sqlGeometryToBeConvertedList)
                    {
                        webMultiPolygonToBeConverted = geom.GetMultiPolygon();
                        webMultiPolygonListToBeConverted.Add(webMultiPolygonToBeConverted);

                        // i++;
                    }

                    // Convert coordinates if needed 
                    List<WebMultiPolygon> toGeometryList =
                        WebServiceData.CoordinateConversionManager.GetConvertedMultiPolygons(
                            webMultiPolygonListToBeConverted, fromCoordinateSystem, toCoordinateSystem);

                    return toGeometryList;
                }
            }
            return null;
        }

        /// <summary>        
        /// This method will take a list of Sql Geometries, convert them to WebMultiPolygons
        /// and reproject them from the current coordinat system to the target coordinat system.
        /// </summary>
        /// <param name="toCoordinateSystem">The target coordinate system.</param>
        /// <param name="fromCoordinateSystem">The current coordinate system.</param>
        /// <param name="sqlGeometryToBeConvertedList">The slit of Sql Geometries that are to be converted.</param>
        public static List<WebPolygon> ReProjectPolygon(WebCoordinateSystem toCoordinateSystem,
                                                               WebCoordinateSystem fromCoordinateSystem,
                                                               List<SqlGeometry> sqlGeometryToBeConvertedList)
        {
            List<WebPolygon> webPolygonListToBeConverted;
            WebPolygon webPolygonToBeConverted;
            int sridInsqlGeometryToBeConvertedList = 0;
            int sridInfromCoordinateSystem = 0;


            webPolygonListToBeConverted = new List<WebPolygon>();
            webPolygonToBeConverted = new WebPolygon();
            if (sqlGeometryToBeConvertedList.IsNotEmpty())
            {
                sridInsqlGeometryToBeConvertedList = (int)sqlGeometryToBeConvertedList[0].STSrid;
                sridInfromCoordinateSystem = GetSridFromWebCoordinateSystem(fromCoordinateSystem);

                if (!sridInfromCoordinateSystem.Equals(sridInsqlGeometryToBeConvertedList))
                {
                    throw new Exception("There is a mismatch between coordinate systems in Sql Geometry list and fromCoordinateSystem.");
                }


                if (toCoordinateSystem.GetWkt().ToUpper() != fromCoordinateSystem.GetWkt().ToUpper())
                {
                    //Todo: Konvertera sqlGeometryToBeConvertedList till WebMultiPolygon
                    foreach (SqlGeometry geom in sqlGeometryToBeConvertedList)
                    {
                        webPolygonToBeConverted = geom.GetPolygon();
                        webPolygonListToBeConverted.Add(webPolygonToBeConverted);

                    }
                    // Convert coordinates if needed 
                    List<WebPolygon> toGeometryList =
                        WebServiceData.CoordinateConversionManager.GetConvertedPolygons(
                            webPolygonListToBeConverted, fromCoordinateSystem, toCoordinateSystem);

                    return toGeometryList;
                }
            }
            return null;
        }


        /// <summary>
        /// Merges the features bounding box with the grid bounding box.
        /// </summary>
        /// <param name="featuresBoundingBox">The features bounding box.</param>
        /// <param name="gridBoundingBox">The grid bounding box.</param>
        /// <returns>A minimum bounding box.</returns>
        public static WebBoundingBox GetFeatureStatisticsMergedBoundingBox(WebBoundingBox featuresBoundingBox,
                                                                            WebBoundingBox gridBoundingBox)
        {
            WebBoundingBox boundingBox = null;

            if (featuresBoundingBox == null & gridBoundingBox == null)
                return null;
            if (featuresBoundingBox == null)
            {
                boundingBox = new WebBoundingBox();
                boundingBox.Min = new WebPoint(gridBoundingBox.Min.X, gridBoundingBox.Min.Y);
                boundingBox.Max = new WebPoint(gridBoundingBox.Max.X, gridBoundingBox.Max.Y);
                return boundingBox;
            }
            if (gridBoundingBox == null)
            {
                
                boundingBox = new WebBoundingBox();
                boundingBox.Min = new WebPoint(featuresBoundingBox.Min.X, featuresBoundingBox.Min.Y);
                boundingBox.Max = new WebPoint(featuresBoundingBox.Max.X, featuresBoundingBox.Max.Y);
                return boundingBox;
            }

            boundingBox = new WebBoundingBox();
            boundingBox.Min = new WebPoint();
            boundingBox.Max = new WebPoint();

            if (featuresBoundingBox.Min.X < gridBoundingBox.Min.X)
                boundingBox.Min.X = gridBoundingBox.Min.X;
            else
                boundingBox.Min.X = featuresBoundingBox.Min.X;

            if (featuresBoundingBox.Min.Y < gridBoundingBox.Min.Y)
                boundingBox.Min.Y = gridBoundingBox.Min.Y;
            else
                boundingBox.Min.Y = featuresBoundingBox.Min.Y;

            if (gridBoundingBox.Max.X < featuresBoundingBox.Max.X)
                boundingBox.Max.X = gridBoundingBox.Max.X;
            else
                boundingBox.Max.X = featuresBoundingBox.Max.X;

            if (gridBoundingBox.Max.Y < featuresBoundingBox.Max.Y)
                boundingBox.Max.Y = gridBoundingBox.Max.Y;
            else
                boundingBox.Max.Y = featuresBoundingBox.Max.Y;

            return boundingBox;
        }

        /// <summary>
        /// Returns a list of gridcells with measurements and centre points. The grid cells may
        /// exceed the bounding box boundaries.
        /// </summary>
        /// <param name="featureList">A list of geometries to measure on.</param>
        /// <param name="gridSpecification">.</param>
        /// <param name="srid"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public static List<WebGridCellFeatureStatistics> GetFeaturesBasedOnGridCells(List<SqlGeometry> featureList, WebGridSpecification gridSpecification, int srid)
        {
            List<WebGridCellFeatureStatistics> webGridCellFeatureStatisticsList;
            List<SqlGeometry> gridCellList;
            List<WebPoint> centrePointList;
            SqlGeometry newFeature;

            if (featureList.IsNotNull() && featureList.Count.Equals(0)) 
                throw new Exception("There are no features in the featureList.");
           
            webGridCellFeatureStatisticsList = new List<WebGridCellFeatureStatistics>();
             if (gridSpecification.IsNull())
                return webGridCellFeatureStatisticsList;
                

            WebBoundingBox gridBoundingBox = gridSpecification.BoundingBox;
            WebBoundingBox featuresBoundingBox = GetBoundingBoxFromSqlGeometryList(featureList);

            var mergedBoundingBox = GetFeatureStatisticsMergedBoundingBox(featuresBoundingBox, gridBoundingBox);
            if (mergedBoundingBox != null)
                gridSpecification.BoundingBox = mergedBoundingBox; // change bounding box            
            gridCellList = CreateSqlGeometryGrid(gridSpecification, srid, out centrePointList);
            gridSpecification.BoundingBox = gridBoundingBox; // reset bounding box
            WebCoordinateSystem gridCoordinateSystem = gridSpecification.GridCoordinateSystem.ToWebCoordinateSystem();
            int maxGridCellArea = gridSpecification.GridCellSize * gridSpecification.GridCellSize;

            // Pick one grid cell at a time and step through all the features. 
            // If they overlap, add the grid cell and the data to the resulting list.
            for (int i = 0; i < gridCellList.Count; i++)
            {
                SqlGeometry gridCell = gridCellList[i];
                WebGridCellFeatureStatistics webGridCellFeatureStatistics = null;

                foreach (SqlGeometry feature in featureList)
                {
                    if (feature == null)
                        continue;


                    

                        // *     ____                --------                            ____
                        //  ____/     |             |       |                         __/    |
                        // < ___      |             |       |           =>(intersect) |_     |
                        //      |_   _|   feature   |_______| gridcell                  |____|
                        //        |_|
                        newFeature = feature.STIntersection(gridCell).MakeValid(); //Create a new feature from overlay with the geometry feature and the gridcell                                                                
                        if (!(newFeature.STNumGeometries() == 0)) //If there is intersection  
                        {
                            if (webGridCellFeatureStatistics == null)
                            {
                                webGridCellFeatureStatistics = new WebGridCellFeatureStatistics();

                                webGridCellFeatureStatistics.CoordinateSystem = gridCoordinateSystem;
                                webGridCellFeatureStatistics.BoundingBox = CreateGridCellBoundingPolygonFromSqlGeometry(gridCell);
                                webGridCellFeatureStatistics.CentreCoordinate = centrePointList[i];
                                webGridCellFeatureStatistics.Size = gridSpecification.GridCellSize;
                                webGridCellFeatureStatistics.GridCoordinateSystem = gridSpecification.GridCoordinateSystem;
                                webGridCellFeatureStatistics.OrginalBoundingBox = CreateGridCellBoundingBoxFromSqlGeometry(gridCell);
                                webGridCellFeatureStatistics.OrginalCentreCoordinate = CreateCentreCoordinateFromBoundingBox(webGridCellFeatureStatistics.OrginalBoundingBox);
                                if (feature.GetGeometryType().ToString().Equals("MultiPolygon"))
                                    webGridCellFeatureStatistics.FeatureType = FeatureType.Multipolygon;
                                if (feature.GetGeometryType().ToString().Equals("Polygon"))
                                    webGridCellFeatureStatistics.FeatureType = FeatureType.Polygon;
                                if (feature.GetGeometryType().ToString().Equals("MultiLineString"))
                                    webGridCellFeatureStatistics.FeatureType = FeatureType.Multiline;
                                if (feature.GetGeometryType().ToString().Equals("LineString"))
                                    webGridCellFeatureStatistics.FeatureType = FeatureType.Line;
                                if (feature.GetGeometryType().ToString().Equals("MultiPoint"))
                                    webGridCellFeatureStatistics.FeatureType = FeatureType.Point;
                                if (feature.GetGeometryType().ToString().Equals("Point"))
                                    webGridCellFeatureStatistics.FeatureType = FeatureType.Point;

                                webGridCellFeatureStatisticsList.Add(webGridCellFeatureStatistics);

                                // Create line for lines and Polygones
                                if (feature.GetGeometryType().ToString().Equals("MultiPolygon") || feature.GetGeometryType().ToString().Equals("Polygon")
                                || feature.GetGeometryType().ToString().Equals("MultiLineString") || feature.GetGeometryType().ToString().Equals("LineString"))
                                {
                                    // Old incorrect value
                                    // webGridCellFeatureStatistics.FeatureLength += newFeature.STLength().Value;
                                    // For debug only 
                                    // double test = newFeature.STLength().Value;

                                    // Here we get the length of the newFeaure without intersection length.
                                    // First we get boundaries for feature, newFeature and gridcell
                                    SqlGeometry featureBoundary = feature.STBoundary();
                                    SqlGeometry newfeatureBoundary = newFeature.STBoundary();
                                    // SqlGeometry gridBoundary = gridCell.STBoundary();

                                    // *     ____                       ____
                                    //    __/    |                 ___/     |                               
                                    //    |_     |                <___      |            => (difference)   |
                                    //      |____|   newfeature       |_   _| feature                          __
                                    //                                   |_|                                           
                                    // Get the difference between newfeature and feature ie intersection with the gridcell
                                    SqlGeometry intersectionObjectsWithGridCell = newfeatureBoundary.STDifference(featureBoundary);

                                    // *           ____                                                               ____
                                    //          __/    |                                                           __/    |  
                                    //          |_     |                    |               => (difference)         _     |    
                                    //            |____| newfeature            __                                    |_  _| 
                                    //                                                                      
                                    // Then we remove intersectionObjects from newFeature and we get the resulting objects.
                                    SqlGeometry newFeatueWithoutIntersection = newfeatureBoundary.STDifference(intersectionObjectsWithGridCell);

                                    // And now we have the correct lenght of newFeature.
                                    double newFeatueWithoutIntersectionLine = newFeatueWithoutIntersection.STLength().Value;
                                    webGridCellFeatureStatistics.FeatureLength += newFeatueWithoutIntersectionLine;
                                }
                            }
                            else
                            {
                                // Create add line length to existing line used for lines and polygones
                                if (feature.GetGeometryType().ToString().Equals("MultiPolygon") || feature.GetGeometryType().ToString().Equals("Polygon")
                                   || feature.GetGeometryType().ToString().Equals("MultiLineString") || feature.GetGeometryType().ToString().Equals("LineString"))
                                {
                                    webGridCellFeatureStatistics.FeatureLength += newFeature.STLength().Value;
                                }
                            }
                            // Calculate area only for polygons
                            if (feature.GetGeometryType().ToString().Equals("MultiPolygon") || feature.GetGeometryType().ToString().Equals("Polygon"))
                            {
                                webGridCellFeatureStatistics.FeatureArea += newFeature.STArea().Value;
                                if (webGridCellFeatureStatistics.FeatureArea > maxGridCellArea)
                                {
                                    webGridCellFeatureStatistics.FeatureArea = maxGridCellArea;
                                }
                            }

                            if (feature.GetGeometryType().ToString().Equals("MultiPoint") || feature.GetGeometryType().ToString().Equals("Point"))
                            {
                                
                                webGridCellFeatureStatistics.FeatureCount += (int)newFeature.STNumGeometries();
                            }
                            else
                            {
                                webGridCellFeatureStatistics.FeatureCount += 1;
                            }
                            
                            // webGridCellFeatureStatistics.FeatureCount += newFeature.STNumGeometries().Value;                                                
                        
                    }
                }
            
            }
            return webGridCellFeatureStatisticsList;
        }


        /// <summary>
        /// Creates the centre coordinate from bounding box.
        /// </summary>
        /// <param name="boundingBox">The bounding box.</param>        
        private static WebPoint CreateCentreCoordinateFromBoundingBox(WebBoundingBox boundingBox)
        {
            WebPoint point = new WebPoint(
                boundingBox.Min.X + ((boundingBox.Max.X - boundingBox.Min.X) / 2.0),
                boundingBox.Min.Y + ((boundingBox.Max.Y - boundingBox.Min.Y) / 2.0));
            return point;
        }


        /// <summary>
        /// Gets the bounding box of all geometries in a list of SqlGeometry objects.
        /// </summary>
        /// <param name="featureList">The feature list.</param>
        private static WebBoundingBox GetBoundingBoxFromSqlGeometryList(List<SqlGeometry> featureList)
        {
            SqlGeometry feature;
            if (featureList.IsEmpty())
                return null;

            feature = featureList[0];
            feature = feature.STEnvelope();
            WebBoundingBox boundingBox = new WebBoundingBox();
            boundingBox.Min = new WebPoint(feature.STStartPoint().STX.Value, feature.STStartPoint().STY.Value);
            boundingBox.Max = new WebPoint(feature.STPointN(3).STX.Value, feature.STPointN(3).STY.Value);

            for (int i = 1; i < featureList.Count; i++)
            {
                feature = featureList[i];
                feature = feature.STEnvelope();
                if (feature.STStartPoint().STX.Value < boundingBox.Min.X)
                    boundingBox.Min.X = feature.STStartPoint().STX.Value;
                if (feature.STStartPoint().STY.Value < boundingBox.Min.Y)
                    boundingBox.Min.Y = feature.STStartPoint().STY.Value;

                if (feature.STPointN(3).STX.Value > boundingBox.Max.X)
                    boundingBox.Max.X = feature.STPointN(3).STX.Value;
                if (feature.STPointN(3).STY.Value > boundingBox.Max.Y)
                    boundingBox.Max.Y = feature.STPointN(3).STY.Value;
            }

            return boundingBox;
        }

        /// <summary>
        /// Creates the grid cell bounding polygon from an SQL geometry.
        /// </summary>
        /// <param name="gridCell">The grid cell.</param>        
        private static WebPolygon CreateGridCellBoundingPolygonFromSqlGeometry(SqlGeometry gridCell)
        {
            WebPolygon gridCellBoundingPolygon;
            //Map the gridCell coordinates to the BoundingBox polygon property in the returning object
            gridCellBoundingPolygon = new WebPolygon();
            gridCellBoundingPolygon.LinearRings = new List<WebLinearRing>();
            gridCellBoundingPolygon.LinearRings.Add(new WebLinearRing());
            gridCellBoundingPolygon.LinearRings[0].Points = new List<WebPoint>();
            gridCellBoundingPolygon.LinearRings[0].Points.Add(new WebPoint());
            gridCellBoundingPolygon.LinearRings[0].Points.Add(new WebPoint());
            gridCellBoundingPolygon.LinearRings[0].Points.Add(new WebPoint());
            gridCellBoundingPolygon.LinearRings[0].Points.Add(new WebPoint());
            gridCellBoundingPolygon.LinearRings[0].Points.Add(new WebPoint());

            gridCellBoundingPolygon.LinearRings[0].Points[0].X = Convert.ToDouble(gridCell.STStartPoint().STX.Value);
            gridCellBoundingPolygon.LinearRings[0].Points[0].Y = Convert.ToDouble(gridCell.STStartPoint().STY.Value);
            gridCellBoundingPolygon.LinearRings[0].Points[1].X = Convert.ToDouble(gridCell.STStartPoint().STX.Value);
            gridCellBoundingPolygon.LinearRings[0].Points[1].Y = Convert.ToDouble(gridCell.STPointN(3).STY.Value);
            gridCellBoundingPolygon.LinearRings[0].Points[2].X = Convert.ToDouble(gridCell.STPointN(3).STX.Value);
            gridCellBoundingPolygon.LinearRings[0].Points[2].Y = Convert.ToDouble(gridCell.STPointN(3).STY.Value);
            gridCellBoundingPolygon.LinearRings[0].Points[3].X = Convert.ToDouble(gridCell.STPointN(3).STX.Value);
            gridCellBoundingPolygon.LinearRings[0].Points[3].Y = Convert.ToDouble(gridCell.STStartPoint().STY.Value);
            gridCellBoundingPolygon.LinearRings[0].Points[4].X = Convert.ToDouble(gridCell.STStartPoint().STX.Value);
            gridCellBoundingPolygon.LinearRings[0].Points[4].Y = Convert.ToDouble(gridCell.STStartPoint().STY.Value);
            return gridCellBoundingPolygon;
        }

        /// <summary>
        /// Creates the grid cell bounding polygon from an SQL geometry.
        /// </summary>
        /// <param name="gridCell">The grid cell.</param>        
        private static WebBoundingBox CreateGridCellBoundingBoxFromSqlGeometry(SqlGeometry gridCell)
        {
            WebBoundingBox gridCellBoundingBox;
            //Map the gridCell coordinates to the BoundingBox polygon property in the returning object
            gridCellBoundingBox = new WebBoundingBox();
            gridCellBoundingBox.Min = new WebPoint();
            gridCellBoundingBox.Max = new WebPoint();
            gridCellBoundingBox.Min.X = Convert.ToDouble(gridCell.STStartPoint().STX.Value);
            gridCellBoundingBox.Min.Y = Convert.ToDouble(gridCell.STStartPoint().STY.Value);
            gridCellBoundingBox.Max.X = Convert.ToDouble(gridCell.STPointN(3).STX.Value);
            gridCellBoundingBox.Max.Y = Convert.ToDouble(gridCell.STPointN(3).STY.Value);            
            return gridCellBoundingBox;
        }


        /// <summary>
        /// Creates a SQL geometry grid based on a grid specification.
        /// </summary>
        /// <param name="gridSpecification">The grid specification.</param>
        /// <param name="srid">The srid that is used.</param>
        /// <param name="centrePointList">The centre point list.</param>        
        public static List<SqlGeometry>  CreateSqlGeometryGrid(WebGridSpecification gridSpecification,  int srid, out List<WebPoint> centrePointList)
        {            
            SqlGeometry newCell;
            WebPoint centrePoint;
            SqlGeometryBuilder geomBuilder;
            List<SqlGeometry> gridCellList = null;
            Double xMin, yMin, xMax, yMax;
            Int32 cellSize;

            centrePointList = new List<WebPoint>();
            if (gridSpecification.IsNotNull() && centrePointList.IsNotNull() && gridSpecification.GridCellSize.IsNotNull())
            {
                cellSize = gridSpecification.GridCellSize;

                // Adjust grid into even integer intervals.
                // this means that the first grid cells may exceed the boundary box.
                xMin = Math.Floor(gridSpecification.BoundingBox.Min.X / cellSize) * cellSize;
                xMax = Math.Ceiling(gridSpecification.BoundingBox.Max.X / cellSize) * cellSize;
                yMin = Math.Floor(gridSpecification.BoundingBox.Min.Y / cellSize) * cellSize;
                yMax = Math.Ceiling(gridSpecification.BoundingBox.Max.Y / cellSize) * cellSize;

                //Todo: This should be done in earlier checkdata 
                if (xMin >= xMax) throw new Exception(string.Format("The bounding box is defect. xMin={0}, xMax={1}", xMin, xMax));
                if (yMin >= yMax) throw new Exception(string.Format("The bounding box is defect.yMin={0}, yMax={1}", yMin, yMax));
                double count = ((xMax - xMin) / cellSize) * ((yMax - yMin) / cellSize);
                if (count > 200000)
                    throw new Exception("Too many grid cells. Use larger cell size or smaller bounding box.");

                //Create a gridcell polygon list and a corresponding centre point list
                gridCellList = new List<SqlGeometry>();
                while (xMin < xMax) //The last grid cell may exceed the boundary box
                {
                    while (yMin < yMax) //The last grid cell may exceed the boundary box
                    {
                        //Build new list of cells 
                        geomBuilder = new SqlGeometryBuilder();
                        geomBuilder.SetSrid(srid);
                        geomBuilder.BeginGeometry(OpenGisGeometryType.Polygon);
                        geomBuilder.BeginFigure(xMin, yMin);
                        geomBuilder.AddLine(xMin + cellSize, yMin);
                        geomBuilder.AddLine(xMin + cellSize, yMin + cellSize);
                        geomBuilder.AddLine(xMin, yMin + cellSize);
                        geomBuilder.AddLine(xMin, yMin);
                        geomBuilder.EndFigure();
                        geomBuilder.EndGeometry();
                        newCell = geomBuilder.ConstructedGeometry;
                        gridCellList.Add(newCell);

                        //Create a list of centre points
                        centrePoint = new WebPoint();
                        centrePoint.X = Math.Floor(xMin / cellSize) * cellSize + cellSize * 0.5;
                        centrePoint.Y = Math.Floor(yMin / cellSize) * cellSize + cellSize * 0.5;
                        if (centrePointList.IsNotNull())
                            centrePointList.Add(centrePoint);

                        yMin = yMin + cellSize;
                    }
                    xMin = xMin + cellSize;
                    yMin = Math.Floor(gridSpecification.BoundingBox.Min.Y / cellSize) * cellSize;
                }
            }

            return gridCellList;
        }





        /// <summary>
        /// Returns the corresponding srid for every defined coordinate system term. 
        /// </summary>
        /// <param name="coordinateSystem">The enum term for the coordinate system.</param>
        /// <returns></returns>
        public static int GetSridFromGridCoordinateSystem(GridCoordinateSystem coordinateSystem)
        {
            if (coordinateSystem.ToString().Equals("RT90"))
            {return 3021;} //4124;}
            else if (coordinateSystem.ToString().Equals("GoogleMercator"))
            //{return 900913;}
            {return 3857;} //900913;}
            else if (coordinateSystem.ToString().Equals("Rt90_25_gon_v"))
            {return 3021;}
            else if (coordinateSystem.ToString().Equals("SWEREF99_TM"))
            {return 3006;}
            else if (coordinateSystem.ToString().Equals("SWEREF99"))
            {return 4378;}
            else if (coordinateSystem.ToString().Equals("WGS84"))
            {return 4326;}
            else if (coordinateSystem.ToString().Equals("UnKnown"))
            {
                throw new Exception("No coordinate system was supplied.");
            }
            else
            {
                throw new Exception("No coordinate system was supplied.");
            }
        }

      

        /// <summary>
        /// Returns the corresponding srid for every defined coordinate system term. 
        /// </summary>
        /// <param name="coordinateSystem">The enum term for the coordinate system.</param>
        /// <returns></returns>
        public static int GetSridFromWebCoordinateSystem(WebCoordinateSystem coordinateSystem)
        {
            if (coordinateSystem.IsNotNull() && coordinateSystem.Id.ToString().Equals("RT90"))
            {return 3021;} //4124;}
            else if (coordinateSystem.IsNotNull() && coordinateSystem.Id.ToString().Equals("GoogleMercator"))
            {return 3857;} //900913;}            
            else if (coordinateSystem.IsNotNull() && coordinateSystem.Id.ToString().Equals("Rt90_25_gon_v"))
            {return 3021;}
            else if (coordinateSystem.IsNotNull() && coordinateSystem.Id.ToString().Equals("SWEREF99_TM"))
            {return 3006;}
            else if (coordinateSystem.IsNotNull() && coordinateSystem.Id.ToString().Equals("SWEREF99"))
            {return 4378;}
            else if (coordinateSystem.IsNotNull() && coordinateSystem.Id.ToString().Equals("WGS84"))
            {return 4326;}
            else if (coordinateSystem.IsNotNull() && coordinateSystem.Id.ToString().Equals("None"))
            {
                throw new Exception("No coordinate system was supplied.");
            }
            else
            {
                throw new Exception("No coordinate system was supplied.");
            }
        }

        /// <summary>
        /// Returns the corresponding srid for every defined coordinate system term. 
        /// </summary>
        /// <param name="srid">the number of the coordinatesysteem as string.</param>
        /// <returns></returns>
        public static WebCoordinateSystem GetWebCoordinateSystemFromSrid(string srid)
        {
            WebCoordinateSystem webCoordinateSystem;
            webCoordinateSystem = new WebCoordinateSystem();
            if (srid.IsNull())
                return webCoordinateSystem;
            if (srid.Equals("3021")) { webCoordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;} else
            if (srid.Equals("3857")){webCoordinateSystem.Id = CoordinateSystemId.GoogleMercator;} else
            if (srid.Equals("900913")) {webCoordinateSystem.Id = CoordinateSystemId.GoogleMercator;} else
            if (srid.Equals("3006")) { webCoordinateSystem.Id = CoordinateSystemId.SWEREF99_TM;} else
            if (srid.Equals("4378")) {webCoordinateSystem.Id = CoordinateSystemId.SWEREF99;} else
            if (srid.Equals("4326")) { webCoordinateSystem.Id = CoordinateSystemId.WGS84;} else
            {
               throw new Exception("Coordinate system nor supported.");
            }
            return webCoordinateSystem;
        }

        /// <summary>
        /// Returns the corresponding srid for every defined coordinate system term. 
        /// </summary>
        /// <param name="srid">the number of the coordinatesysteem as string.</param>
        /// <returns></returns>
        public static GridCoordinateSystem GetGridCoordinateSystemFromSrid(string srid)
        {
            GridCoordinateSystem gridCoordinateSystem;
            gridCoordinateSystem = new GridCoordinateSystem();
            if (srid.IsNull())
                return gridCoordinateSystem;
            if (srid.Equals("3021")) { gridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v; }
            else
                if (srid.Equals("3857")) { gridCoordinateSystem = GridCoordinateSystem.GoogleMercator; }
                else
                    if (srid.Equals("900913")) { gridCoordinateSystem = GridCoordinateSystem.GoogleMercator; }
                    else
                        if (srid.Equals("3006")) { gridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM; }
                        else
                            //if (srid.Equals("4378")) { gridCoordinateSystem = GridCoordinateSystem.SWEREF99; }
                            //else
                                //if (srid.Equals("4326")) { gridCoordinateSystem = GridCoordinateSystem.WGS84; }
                                //else
                                {
                                    throw new Exception("Coordinate system nor supported.");
                                }
            return gridCoordinateSystem;
        }


        /// <summary>
        /// Gets the sweden extent bounding box polygon.
        /// </summary>
        /// <param name="coordinateSystem">The coordinate system.</param>
        public static WebPolygon GetSwedenExtentBoundingBoxPolygon(WebCoordinateSystem coordinateSystem)
        {
            WebPolygon boundingBox = new WebPolygon();

            // Sweden extent in SWEREF99_TM            
            WebBoundingBox swedenBoundingBox = SwedenExtentCoordinates.GetSwedenExtentWebBoundingBoxSweref99();

            if (coordinateSystem.IsNotNull() && coordinateSystem.Id.IsNotNull() &&
                (coordinateSystem.Id != CoordinateSystemId.SWEREF99_TM))
            {
                WebCoordinateSystem sweref99CoordinateSystem = new WebCoordinateSystem();
                sweref99CoordinateSystem.Id = CoordinateSystemId.SWEREF99_TM;
                boundingBox = WebServiceData.CoordinateConversionManager.GetConvertedBoundingBox(
                    swedenBoundingBox, sweref99CoordinateSystem, coordinateSystem);
            }
            else
            {
                boundingBox = swedenBoundingBox.GetPolygon();
            }

            return boundingBox;
        }

        /// <summary>
        /// Gets the sweden extent bounding box.
        /// </summary>
        /// <param name="coordinateSystem">The coordinate system.</param>
        public static WebBoundingBox GetSwedenExtentBoundingBox(WebCoordinateSystem coordinateSystem)
        {
            WebPolygon boundingBoxPolygon = GetSwedenExtentBoundingBoxPolygon(coordinateSystem);
            return boundingBoxPolygon.GetBoundingBox();
        }

        public static SqlGeometry CreateMultiPointGeometry(List<ArtDatabanken.GIS.GeoJSON.Net.Geometry.Point> geoJsonPoints, int srid)
        {
            if (geoJsonPoints == null)
            {
                return null;
            }
            SqlGeometryBuilder geomBuilder;
         
            geomBuilder = new SqlGeometryBuilder();
            geomBuilder.SetSrid(srid);            
            geomBuilder.BeginGeometry(OpenGisGeometryType.MultiPoint);

            foreach (ArtDatabanken.GIS.GeoJSON.Net.Geometry.Point point in geoJsonPoints)
            {                
                // Setting the srid to the same as for the geoJsonPoints
                //geomBuilder.SetSrid(srid);
                geomBuilder.BeginGeometry(OpenGisGeometryType.Point);
                geomBuilder.BeginFigure(point.Coordinates.Longitude, point.Coordinates.Latitude);
                geomBuilder.EndFigure();
                geomBuilder.EndGeometry();
                //pointList.Add(geomBuilder.ConstructedGeometry.MakeValid());
            }
            geomBuilder.EndGeometry();
            SqlGeometry geometry = geomBuilder.ConstructedGeometry.MakeValid();
            return geometry;            
        }

        /// <summary>
        /// Convert list of WebGridCellSpeciesObservationCount to multi polygon
        /// </summary>
        /// <param name="gridCells"></param>
        /// <returns></returns>
        public static SqlGeometry GridCellsToMultiPolygon(List<WebGridCellSpeciesObservationCount> gridCells)
        {
            if (gridCells == null || !gridCells.Any())
            {
                return null;
            }

            var gb = new SqlGeometryBuilder();

            // Set the Spatial Reference ID
            gb.SetSrid(gridCells.First().GridCoordinateSystem.Srid());

            // Start the collection
            gb.BeginGeometry(OpenGisGeometryType.MultiPolygon);

            foreach (var gridCell in gridCells)
            {
                var boundingBox = gridCell.OrginalBoundingBox;

                if (boundingBox == null)
                {
                    continue;
                }

                // Define a new polygon
                gb.BeginGeometry(OpenGisGeometryType.Polygon);

                gb.BeginFigure(boundingBox.Min.X, boundingBox.Min.Y);
                gb.AddLine(boundingBox.Min.X, boundingBox.Max.Y);
                gb.AddLine(boundingBox.Max.X, boundingBox.Max.Y);
                gb.AddLine(boundingBox.Max.X, boundingBox.Min.Y);
                gb.AddLine(boundingBox.Min.X, boundingBox.Min.Y);

                // End the polygon
                gb.EndFigure();
                gb.EndGeometry();
            }

            // End the multipolygon
            gb.EndGeometry();

            // Return as a valid SqlGeometry instance
            return gb.ConstructedGeometry.MakeValid();
        }

        public static SqlGeometry GridCellsCenterPointsToPolygon(List<WebGridCellSpeciesObservationCount> gridCells)
        {
            if (gridCells == null || !gridCells.Any())
            {
                return null;
            }

            var gb = new SqlGeometryBuilder();

            // Set the Spatial Reference ID
            gb.SetSrid(gridCells.First().GridCoordinateSystem.Srid());

            // Start the collection
            gb.BeginGeometry(OpenGisGeometryType.MultiPoint);

            foreach (var gridCell in gridCells)
            {
                gb.BeginGeometry(OpenGisGeometryType.Point);
                gb.BeginFigure(gridCell.OrginalCentreCoordinate.X, gridCell.OrginalCentreCoordinate.Y);
                gb.EndFigure();
                gb.EndGeometry();
            }

            // End the multipolygon
            gb.EndGeometry();

            // Return as a valid SqlGeometry instance
            return gb.ConstructedGeometry.MakeValid();
        }
    }
}
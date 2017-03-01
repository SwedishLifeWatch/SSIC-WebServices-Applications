using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.GIS.SwedenExtent
{
    /// <summary>
    /// This class contains data for getting the Sweden extent.
    /// </summary>
    public static class SwedenExtentCoordinates
    {
        /// <summary>
        /// Gets the sweden extent bounding box in SWEREF99_TM coordinates.        
        /// </summary>
        /// <returns>
        /// The sweden extent bounding box in SWEREF99_TM coordinates.
        /// </returns>
        public static BoundingBox GetSwedenExtentBoundingBoxSweref99()
        {
            BoundingBox boundingBox;
            boundingBox = new BoundingBox();
            boundingBox.Max = new Point();
            boundingBox.Min = new Point();
            boundingBox.Max.X = 969927.0414948005;
            boundingBox.Max.Y = 7737973.607602615;
            boundingBox.Min.X = 195816.17626999575;
            boundingBox.Min.Y = 6029444.002008331;
            return boundingBox;
        }

        /// <summary>
        /// Gets the sweden extent bounding box in SWEREF99_TM coordinates.        
        /// </summary>
        /// <returns>
        /// The sweden extent bounding box in SWEREF99_TM coordinates.
        /// </returns>
        public static WebBoundingBox GetSwedenExtentWebBoundingBoxSweref99()
        {
            WebBoundingBox boundingBox;
            boundingBox = new WebBoundingBox();
            boundingBox.Max = new WebPoint();
            boundingBox.Min = new WebPoint();
            boundingBox.Max.X = 969927.0414948005;
            boundingBox.Max.Y = 7737973.607602615;
            boundingBox.Min.X = 195816.17626999575;
            boundingBox.Min.Y = 6029444.002008331;
            return boundingBox;
        }
    }
}

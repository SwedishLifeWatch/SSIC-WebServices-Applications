using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.GIS.WFS
{
    /// <summary>
    /// TBD ?!?
    /// </summary>
    public class WfsBoundingBox
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public double MinX { get; private set; }
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public double MinY { get; private set; }
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public double MaxX { get; private set; }
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public double MaxY { get; private set; }
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string SrsName { get; private set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public WfsBoundingBox(double minX, double minY, double maxX, double maxY, string srsName)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
            SrsName = srsName;
        }

    }
}

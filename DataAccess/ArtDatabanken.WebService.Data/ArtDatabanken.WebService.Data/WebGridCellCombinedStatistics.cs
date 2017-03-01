using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains statistics about a grid cell
    /// </summary>
    [DataContract]
    public class WebGridCellCombinedStatistics : WebData
    {
        /// <summary>
        /// Bounding box for the grid cell in calculated
        /// coordinate system, ie GridCoordinateSystem.
        /// </summary>
        [DataMember]
        public WebBoundingBox OriginalBoundingBox { get; set; }

        /// <summary>
        /// Contains information about features in grid cells.
        /// </summary>
        [DataMember]
        public WebGridCellFeatureStatistics FeatureStatistics { get; set; }

        ///// <summary>
        ///// Contains information on counting number of species observations.
        ///// </summary>
        //[DataMember]
        //public WebGridCellSpeciesObservationCount SpeciesObservationCount { get; set; }

        /// <summary>
        /// Contains information on counting number of species and species observations.
        /// </summary>
        [DataMember]
        public WebGridCellSpeciesCount SpeciesCount { get; set; }
    }
}

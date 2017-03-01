using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about what statistics are requested
    /// from a web feature service and wich spatial feature type that is to 
    /// be measured. This is indata for GridCellFeatureStatistics();
    /// </summary>
    public class FeatureStatisticsSummary 
    {
        ///// <summary>
        /////  Number of features that is requested.
        ///// </summary>
        //public bool FeatureCount { get; set; }

        ///// <summary>
        /////  Length of feature that is requested.
        ///// </summary>
        //public bool FeatureLength { get; set; }

        ///// <summary>
        /////  Area of feature that is requested.
        ///// </summary>
        //public bool FeatureArea { get; set; }

        /// <summary>
        ///  Type of feature that is to be measured.
        /// </summary>
        public FeatureType FeatureType { get; set; }

        /// <summary>
        /// Property holding bounding box information from hand written url in GUI.
        /// Only two-dimensional bounding boxes (rectangles) are handled
        /// in the first version of AnalysisService.
        /// </summary>
        public BoundingBox BoundingBox { get; set; }

        ///// <summary>
        /////  Type of data that are to be used for 
        /////  filering/statistical calculations.
        ///// </summary>
        //public String CalculationProperty { get; set; }

        ///// <summary>
        /////  Type of data filering. Type medelvärde, median osv
        ///// </summary>
        //public CalculationType CalulationType { get; set; }


    }

}

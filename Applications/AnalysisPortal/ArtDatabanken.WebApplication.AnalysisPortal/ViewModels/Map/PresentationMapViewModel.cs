using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Map
{
    /// <summary>
    /// View model for ~/Format/Map.
    /// </summary>
    public class PresentationMapViewModel
    {
        /// <summary>
        /// Gets or sets the presentation coordinate systems.
        /// </summary>        
        public List<CoordinateSystemViewModel> PresentationCoordinateSystems { get; set; }

        /// <summary>
        /// Gets or sets the presentation coordinate system identifier.
        /// </summary>        
        public CoordinateSystemId PresentationCoordinateSystemId { get; set; }

        /// <summary>
        /// Gets or sets the download coordinate systems.
        /// </summary>        
        public List<CoordinateSystemViewModel> DownloadCoordinateSystems { get; set; }

        /// <summary>
        /// Gets or sets the download coordinate system identifier.
        /// </summary>        
        public CoordinateSystemId DownloadCoordinateSystemId { get; set; }

        /// <summary>
        /// Gets or sets the grid maps coordinate systems.
        /// </summary>        
        public List<CoordinateSystemViewModel> GridMapsCoordinateSystems { get; set; }

        /// <summary>
        /// Gets or sets the grid maps coordinate system identifier.
        /// </summary>        
        public CoordinateSystemId GridMapsCoordinateSystemId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the map settings are default.
        /// </summary>        
        public bool IsSettingsDefault { get; set; }
    }
}

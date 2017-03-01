using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Spatial
{
    /// <summary>
    /// This class is a ViewModel for the Filter/Locality view.
    /// </summary>
    public class LocalityViewModel
    {
        /// <summary>
        /// Gets or sets the name of the locality.
        /// </summary>
        /// <value>
        /// The name of the locality.
        /// </value>
        public string LocalityName { get; set; }

        /// <summary>
        /// Gets or sets the compare operator.
        /// </summary>
        /// <value>
        /// The compare operator.
        /// </value>
        public StringCompareOperator CompareOperator { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the locality settings default.
        /// </summary>
        /// <value>
        /// <c>true</c> if the locality settings is default; otherwise, <c>false</c>.
        /// </value>
        public bool IsSettingsDefault { get; set; }
    }
}

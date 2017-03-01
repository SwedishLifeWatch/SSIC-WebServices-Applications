using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter;

namespace AnalysisPortal.Helpers.WebAPI
{
    /// <summary>
    /// A class to represent the Json object sent to the WebAPI.
    /// This class handles the Filter property of the MySettings object.
    /// </summary>
    public class MySettingsWebAPI
    {
        /// <summary>
        /// Describes if a full update is required.
        /// </summary>
        public bool? FullUpdate { get; set; }

        /// <summary>
        /// The Filter property of the MySettings object.
        /// </summary>
        public FilterSettings Filter { get; set; }
    }
}
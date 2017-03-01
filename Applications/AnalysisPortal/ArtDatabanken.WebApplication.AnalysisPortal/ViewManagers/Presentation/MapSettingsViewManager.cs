using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.General;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Map;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Presentation;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Presentation
{
    /// <summary>
    /// The map settings view manager.
    /// </summary>
    public class MapSettingsViewManager : ViewManagerBase
    {
        /// <summary>
        /// Gets the map settings.
        /// </summary>        
        public PresentationMapSetting MapSettings
        {
            get { return MySettings.Presentation.Map; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapSettingsViewManager"/> class.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="mySettings">The MySettings object.</param>
        public MapSettingsViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        /// <summary>
        /// Creates the presentation map view model.
        /// </summary>
        /// <returns>A view model.</returns>
        public PresentationMapViewModel CreatePresentationMapViewModel()
        {
            PresentationMapViewModel model = new PresentationMapViewModel();
            model.PresentationCoordinateSystems = CoordinateSystemHelper.GetPresentationMapCoordinateSystems();
            model.DownloadCoordinateSystems = CoordinateSystemHelper.GetDownloadMapCoordinateSystems();
            model.GridMapsCoordinateSystems = CoordinateSystemHelper.GetGridMapsCoordinateSystems();
            model.IsSettingsDefault = MapSettings.IsSettingsDefault() && SessionHandler.MySettings.Calculation.GridStatistics.IsCoordinateSystemSettingsDefault();
            return model;
        }

        /// <summary>
        /// Updates the map settings.
        /// </summary>
        /// <param name="model">The data.</param>
        public void UpdateMapSettings(PresentationMapViewModel model)
        {
            MapSettings.PresentationCoordinateSystemId = model.PresentationCoordinateSystemId;
            MapSettings.DownloadCoordinateSystemId = model.DownloadCoordinateSystemId;
            MySettings.Calculation.GridStatistics.CoordinateSystemId = (int)model.GridMapsCoordinateSystemId;
        }

        /// <summary>
        /// Updates the current coordinate system.
        /// </summary>
        /// <param name="coordinateSystemId">The coordinate system identifier.</param>
        public void UpdateCurrentCoordinateSystem(CoordinateSystemId coordinateSystemId)
        {
            MapSettings.PresentationCoordinateSystemId = coordinateSystemId;            
        }
    }
}

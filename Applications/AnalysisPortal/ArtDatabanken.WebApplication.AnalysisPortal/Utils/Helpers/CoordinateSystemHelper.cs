using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers
{
    /// <summary>
    /// Coordinate system helper functions.
    /// </summary>
    public static class CoordinateSystemHelper
    {
        /// <summary>
        /// Gets the presentation coordinate systems.
        /// </summary>
        /// <returns>A list with available presentatione coordinate systems.</returns>
        public static List<CoordinateSystemViewModel> GetCoordinateSystems()
        {
            return GetCoordinateSystems(null);
        }

        /// <summary>
        /// Gets the presentation coordinate systems.
        /// </summary>
        /// <returns>A list with available presentation coordinate systems.</returns>
        public static List<CoordinateSystemViewModel> GetPresentationMapCoordinateSystems()
        {
            return GetCoordinateSystems(SessionHandler.MySettings.Presentation.Map.PresentationCoordinateSystemId);
        }

        /// <summary>
        /// Gets the download coordinate systems.
        /// </summary>
        /// <returns>A list with available download coordinate systems.</returns>
        public static List<CoordinateSystemViewModel> GetDownloadMapCoordinateSystems()
        {
            return GetCoordinateSystems(SessionHandler.MySettings.Presentation.Map.DownloadCoordinateSystemId);
        }

        /// <summary>
        /// Gets the grid maps coordinate systems.
        /// </summary>
        /// <returns>A list with available grid maps coordinate systems.</returns>
        public static List<CoordinateSystemViewModel> GetGridMapsCoordinateSystems()
        {
            CoordinateSystemId coordinateSystemId = CoordinateSystemId.SWEREF99_TM;
            if (SessionHandler.MySettings.Calculation.GridStatistics.CoordinateSystemId.HasValue)
            {
                try
                {
                    coordinateSystemId =
                        (CoordinateSystemId)SessionHandler.MySettings.Calculation.GridStatistics.CoordinateSystemId.Value;
                }
                catch (Exception)
                {
                }
            }

            return GetGridMapsCoordinateSystems(coordinateSystemId);
        }

        /// <summary>
        /// Gets the presentation coordinate systems.
        /// </summary>
        /// <param name="selectedCoordinateSystemId">The selected coordinate system identifier.</param>
        /// <returns>A list with available presentatione coordinate systems.</returns>
        public static List<CoordinateSystemViewModel> GetCoordinateSystems(CoordinateSystemId? selectedCoordinateSystemId)
        {            
            var coordinateSystems = new List<CoordinateSystemViewModel>();
            coordinateSystems.Add(new CoordinateSystemViewModel((int)CoordinateSystemId.GoogleMercator, "Google Mercator", false));
            coordinateSystems.Add(new CoordinateSystemViewModel((int)CoordinateSystemId.SWEREF99_TM, "SWEREF 99", false));
            coordinateSystems.Add(new CoordinateSystemViewModel((int)CoordinateSystemId.Rt90_25_gon_v, "RT 90", false));
            coordinateSystems.Add(new CoordinateSystemViewModel((int)CoordinateSystemId.WGS84, "WGS 84", false));
            if (selectedCoordinateSystemId.HasValue)
            {
                foreach (CoordinateSystemViewModel coordinateSystem in coordinateSystems)
                {
                    // Set one of the coordinate systems selected.
                    coordinateSystem.Selected = coordinateSystem.Id == (int)selectedCoordinateSystemId.Value;
                }
            }

            return coordinateSystems;
        }

        /// <summary>
        /// Gets the presentation coordinate systems.
        /// </summary>
        /// <param name="selectedCoordinateSystemId">The selected coordinate system identifier.</param>
        /// <returns>A list with available presentatione coordinate systems.</returns>
        public static List<CoordinateSystemViewModel> GetGridMapsCoordinateSystems(CoordinateSystemId? selectedCoordinateSystemId)
        {
            var coordinateSystems = new List<CoordinateSystemViewModel>();            
            coordinateSystems.Add(new CoordinateSystemViewModel((int)CoordinateSystemId.SWEREF99_TM, "SWEREF 99", false));
            coordinateSystems.Add(new CoordinateSystemViewModel((int)CoordinateSystemId.Rt90_25_gon_v, "RT 90", false));            
            if (selectedCoordinateSystemId.HasValue)
            {
                foreach (CoordinateSystemViewModel coordinateSystem in coordinateSystems)
                {
                    // Set one of the coordinate systems selected.
                    coordinateSystem.Selected = coordinateSystem.Id == (int)selectedCoordinateSystemId.Value;
                }
            }

            return coordinateSystems;
        }

        /// <summary>
        /// Gets the coordinate system from grid coordinate system.
        /// </summary>
        /// <param name="gridCoordinateSystem">The grid coordinate system.</param>
        /// <returns>The coordinate system that equals the <paramref name="gridCoordinateSystem"/>.</returns>
        public static CoordinateSystem GetCoordinateSystemFromGridCoordinateSystem(GridCoordinateSystem gridCoordinateSystem)
        {
            switch (gridCoordinateSystem)
            {
                case GridCoordinateSystem.GoogleMercator:
                    return new CoordinateSystem(CoordinateSystemId.GoogleMercator);
                case GridCoordinateSystem.Rt90_25_gon_v:
                    return new CoordinateSystem(CoordinateSystemId.Rt90_25_gon_v);
                case GridCoordinateSystem.SWEREF99_TM:
                    return new CoordinateSystem(CoordinateSystemId.SWEREF99_TM);
                default:
                    return new CoordinateSystem(CoordinateSystemId.None);
            }
        }

        /// <summary>
        /// Gets the name of the coordinate system.
        /// </summary>
        /// <param name="coordinateSystemId">The coordinate system identifier.</param>
        /// <returns>The name of the coordinate system.</returns>
        public static string GetCoordinateSystemName(this CoordinateSystemId coordinateSystemId)
        {
            switch (coordinateSystemId)
            {
                case CoordinateSystemId.GoogleMercator:
                    return "Google Mercator";
                case CoordinateSystemId.SWEREF99:
                    return "SWEREF 99";
                case CoordinateSystemId.SWEREF99_TM:
                    return "SWEREF 99";
                case CoordinateSystemId.WGS84:
                    return "WGS 84";
                case CoordinateSystemId.Rt90_25_gon_v:
                    return "RT 90";
                case CoordinateSystemId.None:
                    return "None";
                default:
                    throw new ArgumentException(string.Format("CoordinateSystemId: {0} doesn't exist.", coordinateSystemId));
            }
        }
    }
}

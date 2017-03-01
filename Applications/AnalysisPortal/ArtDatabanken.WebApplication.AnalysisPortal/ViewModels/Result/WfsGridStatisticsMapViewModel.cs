using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.Map;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result
{
    public class WfsGridStatisticsMapViewModel
    {
        public int? GridSize { get; set; }
        //public int? CoordinateSystemId { get; set; }
        public List<CoordinateSystemViewModel> CoordinateSystems { get; set; }
        public WfsGridStatisticsCalculationMode WfsGridStatisticsCalculationMode { get; set; }
        public int? WfsGridStatisticsLayerId { get; set; }
        public List<WfsLayerViewModel> WfsLayers { get; set; }
        public bool AddSpartialFilterLayer { get; set; }
    }
}

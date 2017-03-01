using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.Map;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation
{
    public class CoordinateSystemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }

        public CoordinateSystemViewModel(int id, string name, bool selected)
        {
            Id = id;
            Name = name;
            Selected = selected;
        }        
    }

    public class GridStatisticsViewModel
    {
        public List<CoordinateSystemViewModel> CoordinateSystems { get; set; }
        public int? GridSize { get; set; }
        public bool CalculateNumberOfObservations { get; set; }
        public int? CoordinateSystemId { get; set; }
        public bool CalculateNumberOfTaxa { get; set; }

        //public int WfsGridStatisticsCalculationMode { get; set; }
        //public int? WfsGridStatisticsLayerId { get; set; }
        public WfsGridStatisticsCalculationMode WfsGridStatisticsCalculationMode { get; set; }
        public int? WfsGridStatisticsLayerId { get; set; }
        public List<WfsLayerViewModel> WfsLayers { get; set; }

        public bool IsSettingsDefault { get; set; }
        public bool GenerateAllGridCells { get; set; }

        public string GetSelectedCoordinateSystemName()
        {            
            if (CoordinateSystemId.HasValue)
            {
                CoordinateSystemViewModel coordinateSystem = CoordinateSystems.FirstOrDefault(m => m.Id == CoordinateSystemId.Value);
                if (coordinateSystem != null)
                {
                    return coordinateSystem.Name;
                }

                return "";
            }
            return "";
        }

        public string GetGridSizeFormatted()
        {
            if (GridSize.HasValue)
            {
                return string.Format("{0:N0}m", GridSize.Value);
            }
            return "";
        }

        public List<string> GetCalculateStrings()
        {
            var strings = new List<string>();
            if (CalculateNumberOfObservations)
            {
                strings.Add(Resources.Resource.GridStatisticsNumberOfObservations);
            }
            if (CalculateNumberOfTaxa)
            {
                strings.Add(Resources.Resource.GridStatisticsNumberOfTaxa);
            }

            return strings;
        }

        public string GetWfsCalculationModeText()
        {
            switch (WfsGridStatisticsCalculationMode)
            {
                case WfsGridStatisticsCalculationMode.Count:
                    return Resource.GridStatisticsCalculationModeCount;
                case WfsGridStatisticsCalculationMode.Area:
                    return Resource.GridStatisticsCalculationModeArea;
                case WfsGridStatisticsCalculationMode.Length:
                    return Resource.GridStatisticsCalculationModeLength;                
                default:
                    throw new ArgumentException(this.WfsGridStatisticsCalculationMode + " doesn't exist");
            }
        }

        public string GetSelectedWfsLayerName()
        {
            if (!WfsGridStatisticsLayerId.HasValue)
            {
                return "-";
            }

            WfsLayerViewModel selectedLayer = WfsLayers.FirstOrDefault(layer => layer.Id == WfsGridStatisticsLayerId.Value);
            return selectedLayer != null ? selectedLayer.Name : "-";
        }
    }
}

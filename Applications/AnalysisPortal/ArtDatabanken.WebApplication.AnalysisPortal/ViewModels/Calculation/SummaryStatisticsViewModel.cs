using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.Map;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation
{
    /// <summary>
    /// ViewModel handling summary statistics.
    /// </summary>
    public class SummaryStatisticsViewModel 
    {
        /// <summary>
        /// Is set to true if calculations of number of observations is to be performed 
        /// on selected settings. 
        /// </summary>
        public bool CalculateNumberOfObservationsfromObsData { get; set; }

        /// <summary>
        /// Is set to true if calculations of number of species is to be performed 
        /// on selected settings. 
        /// </summary>
        public bool CalculateNumberOfSpeciesfromObsData { get; set; }
      
        /// <summary>
        /// Get what to calculate on. TODO more calculations is to be added here.
        /// </summary>
        /// <returns>Returns text to be shown.</returns>
        public List<string> GetCalculateStrings()
        {
            var strings = new List<string>();
            if (CalculateNumberOfObservationsfromObsData)
            {
                strings.Add(Resources.Resource.SummaryStatisticsNumberOfObservations);
            }

            if (CalculateNumberOfSpeciesfromObsData)
            {
                strings.Add(Resources.Resource.SummaryStatisticsNumberOfSpecies);
            }

            return strings;
        }

        /// <summary>
        /// Handling labels in view.
        /// </summary>
        public ModelLabels Labels
        {
            get { return _labels; }
        }

        /// <summary>
        /// To check if default settings are used.
        /// </summary>
        public bool IsSettingsDefault { get; set; }

        // From GridStatisticsViewModel

        /// <summary>
        /// Layer id of WfsGridStatistics.
        /// </summary>
        public int? WfsGridStatisticsLayerId { get; set; }

        /// <summary>
        /// Wfs layers.
        /// </summary>
        public List<WfsLayerViewModel> WfsLayers { get; set; }

        /// <summary>
        /// Labels to be handled in view.
        /// </summary>
        private readonly ModelLabels _labels = new ModelLabels();

        /// <summary>
        /// Localized labels class.
        /// </summary>
        public class ModelLabels
        {
            /// <summary>
            /// Summary statistics title.
            /// </summary>
            public readonly string Title = Resource.SummaryStatisticsTitle;

            /// <summary>
            /// Calculate number of observations.
            /// </summary>
            public readonly string CalculateNumberOfObservations = Resource.SummaryStatisticsCalculateNumberOfObservations;

            /// <summary>
            /// Number of observations.
            /// </summary>
            public readonly string NumberOfObservations = Resource.SummaryStatisticsNumberOfObservations;

            /// <summary>
            /// Calculate number of species.
            /// </summary>
            public readonly string CalculateNumberOfSpecies = Resource.SummaryStatisticsCalculateNumberOfSpecies;

            /// <summary>
            /// Number of species.
            /// </summary>
            public readonly string NumberOfSpecies = Resource.SummaryStatisticsNumberOfSpecies;
        }

        /// <summary>
        /// Get the selected Wfs layer name.
        /// </summary>
        /// <returns>The name to be shown.</returns>
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
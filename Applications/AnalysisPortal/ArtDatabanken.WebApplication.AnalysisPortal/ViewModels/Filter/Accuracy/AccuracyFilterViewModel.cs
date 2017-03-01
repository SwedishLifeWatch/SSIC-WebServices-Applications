using System;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Accuracy
{
    /// <summary>
    /// This class is a view model for the accuracy filter page
    /// </summary>
    public class AccuracyFilterViewModel
    {
        public int MaxCoordinateAccuracy { get; set; }
        
        public bool Inclusive { get; set; }

        //public bool Exclusive { get; set; }

        public bool IsCoordinateAccuracyActive { get; set; }

        /// <summary>
        /// Gets the model labels.
        /// </summary>
        public ModelLabels Labels
        {
            get
            {
                if (_labels == null)
                {
                    _labels = new ModelLabels();
                }

                return _labels;
            }
        }

        public bool IsSettingsDefault { get; set; }

        private ModelLabels _labels;

        /// <summary>
        /// Localized labels
        /// </summary>
        public class ModelLabels
        {
            public string Title { get { return Resources.Resource.FilterAccuracyTitle; } }
            public string Inclusive { get { return Resources.Resource.FilterAccuracyInclusive; } }
            public string Exclusive { get { return Resources.Resource.FilterAccuracyExclusive; } }
            public string MaxCoordinateAccuracy { get { return Resources.Resource.FilterAccuracyMaxCoordinateAccuracy; } }
            public string InclusiveHint { get { return Resources.Resource.FilterAccuracyInclusiveHint; } }
            public string ExclusiveHint { get { return Resources.Resource.FilterAccuracyExclusiveHint; } }
            public string CoordinateAccuracyIsActive { get { return Resources.Resource.FilterCoordinateAccuracyIsActive; } }
        }
    }
}
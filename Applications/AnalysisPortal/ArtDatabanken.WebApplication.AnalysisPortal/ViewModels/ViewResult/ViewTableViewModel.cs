using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.QueryComplexity;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.ViewResult
{
    public class ViewTableViewModel
    {
        public List<ViewTableField> TableFields { get; set; }
        //public List<ObservationViewModel> Observations { get; set; }

        public ModelLabels Labels
        {
            get { return _labels; }
        }

        public QueryComplexityEstimate ComplexityEstimate { get; set; }

        private readonly ModelLabels _labels = new ModelLabels();

        /// <summary>
        /// Localized labels
        /// </summary>
        public class ModelLabels
        {
            public readonly string TitleLabel = Resources.Resource.PresentationTableTitleLabel;
        }
    }
    }

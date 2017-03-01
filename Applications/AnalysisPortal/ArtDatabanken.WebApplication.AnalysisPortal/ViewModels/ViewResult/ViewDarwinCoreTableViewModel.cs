using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Observations;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.ViewResult
{
    public class ViewDarwinCoreTableViewModel
    {
        public List<SpeciesObservationViewModel> Observations { get; set; }

        public ModelLabels Labels
        {
            get { return _labels; }
        }
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

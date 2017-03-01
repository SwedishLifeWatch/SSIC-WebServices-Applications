using System;
using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.QueryComplexity;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result
{
    public class ResultTimeSeriesOnSpeciesObservationCountsViewModel
    {
        public string OriginalCoordinateSystemName { get; set; }
        public string CoordinateSystemName { get; set; }
        public string CentreCoordinateName { get; set; }

        public QueryComplexityEstimate ComplexityEstimate { get; set; }
        public int NoOfTaxa { get; set; }
        public int NoOfObservations { get; set; }

        public string NoOfTaxaAsString { get { return Convert.ToString(NoOfTaxa); } }

        public List<TaxonViewModel> SelectedTaxa { get; set; }
    }
}

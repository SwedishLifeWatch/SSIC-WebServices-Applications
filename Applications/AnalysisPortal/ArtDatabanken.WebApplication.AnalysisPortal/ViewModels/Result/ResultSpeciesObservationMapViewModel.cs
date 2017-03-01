using System;
using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.QueryComplexity;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result
{
    public class ResultSpeciesObservationMapViewModel
    {
        public QueryComplexityEstimate ComplexityEstimate { get; set; }
        public List<CategoryTaxaViewModel> CategoryTaxaList { get; set; }
        public string SelectedTaxaDescription { get; set; }
        public bool AddSpartialFilterLayer { get; set; }
    }
}

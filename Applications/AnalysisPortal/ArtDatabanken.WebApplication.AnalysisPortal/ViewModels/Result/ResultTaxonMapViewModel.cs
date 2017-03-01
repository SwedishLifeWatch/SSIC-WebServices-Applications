using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.QueryComplexity;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result
{
    public class ResultTaxonMapViewModel
    {
        public QueryComplexityEstimate ComplexityEstimate { get; set; }
    }
}

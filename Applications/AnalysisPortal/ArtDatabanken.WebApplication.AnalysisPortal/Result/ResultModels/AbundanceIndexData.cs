using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultModels
{
    /// <summary>
    /// This class holds abundance index data for a single time step and taxon
    /// </summary>
    public class AbundanceIndexData
    {
        public int TaxonId { get; set; }
        public string TimeStep { get; set; }
        public double? AbundanceIndex { get; set; }
        public int Count { get; set; }
        public int TotalCount { get; set; }
    }
}

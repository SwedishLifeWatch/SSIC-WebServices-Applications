using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid
{    
    /// <summary>
    /// Species observation count per polygon and taxon result model.
    /// </summary>
    public class TaxonSpecificSpeciesObservationCountPerPolygonResult
    {        
        /// <summary>
        /// Gets or sets the taxa.
        /// </summary>        
        public List<TaxonViewModel> Taxa { get; set; }

        /// <summary>
        /// Gets or sets the species observation count per polygon and taxon data.
        /// </summary>        
        public Dictionary<string, Dictionary<int, long>> SpeciesObservationCountPerPolygon { get; set; }
    }
}
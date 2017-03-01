using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid
{    
    /// <summary>
    /// Multiple species observation grid result model.
    /// </summary>
    public class TaxonSpecificGridSpeciesObservationCountResult
    {        
        /// <summary>
        /// Gets or sets the taxa.
        /// </summary>        
        public List<TaxonViewModel> Taxa { get; set; }

        /// <summary>
        /// Gets or sets the grid cells.
        /// </summary>        
        public Dictionary<IGridCellBase, Dictionary<int, IGridCellSpeciesObservationCount>> GridCells { get; set; }
    }
}
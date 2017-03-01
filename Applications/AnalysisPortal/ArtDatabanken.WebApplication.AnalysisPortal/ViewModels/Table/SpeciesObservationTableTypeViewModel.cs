using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Table
{
    /// <summary>
    /// Species observation table type view model.
    /// </summary>
    public class SpeciesObservationTableTypeViewModel
    {
        /// <summary>
        /// Id.
        /// </summary>
        public Int32? Id { get; set; }

        /// <summary>
        /// Title.
        /// </summary>
        public String Title { get; set; }

        /// <summary>
        /// Field Ids.
        /// </summary>
        public List<Int32> FieldIds { get; set; }
    }
}
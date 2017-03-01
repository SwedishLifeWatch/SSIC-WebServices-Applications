using System;
using System.Collections.Generic;
using System.Text;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This base class containes general properties describing a time step.
    /// It is used as a base for classes holding time step specific summary statistics of different kind. 
    /// </summary>
    public class TimeStepBase : ITimeStepBase
    {
        /// <summary>
        /// Unique identification this time step.
        /// Mandatory ie always required.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Name of the time step.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// The start date of the time step.
        /// This property may have missing value as it may not be relevant for the specified time step type.
        /// </summary>
        public DateTime? Date { get; set; }
        
        /// <summary>
        /// This property defines the temporal extent of the time step and its periodicity.
        /// </summary>
        public Periodicity Periodicity { get; set; }
    }
}

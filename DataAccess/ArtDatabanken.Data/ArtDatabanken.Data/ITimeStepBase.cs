using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface defines general properties describing a time step.
    /// It is used as an interface for the base class holding time step specific summary statistics of different kind. 
    /// </summary>
    public interface ITimeStepBase : IDataId32
    {
        /// <summary>
        /// Name of the time step.
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// The start date of the time step.
        /// This property may have missing value as it may not be relevant for the specified time step type.
        /// </summary>
        DateTime? Date { get; set; }

        /// <summary>
        /// This property defines the temporal extent of the time step and its periodicity.
        /// </summary>
        Periodicity Periodicity { get; set; }
    }
}

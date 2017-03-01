using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the ITimeStepSpeciesObservationCount interface.
    /// This list represents a time serie of species observation counts.
    /// </summary>
    public class TimeStepSpeciesObservationCountList : DataId32List<ITimeStepSpeciesObservationCount>
    {
        /// <summary>
        /// Constructor for the TimeStepSpeciesObservationCountList class.
        /// </summary>
        public TimeStepSpeciesObservationCountList()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the TimeStepSpeciesObservationCountList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public TimeStepSpeciesObservationCountList(Boolean optimize)
            : base(optimize)
        {
        }
    }
}

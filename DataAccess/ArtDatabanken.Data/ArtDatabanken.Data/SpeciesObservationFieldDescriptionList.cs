using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the ISpeciesObservationFieldDescription interface.
    /// </summary>
    [Serializable]
    public class SpeciesObservationFieldDescriptionList : DataId32List<ISpeciesObservationFieldDescription>
    {
        /// <summary>
        /// Constructor for the SpeciesObservationFieldDescriptionList class.
        /// </summary>
        public SpeciesObservationFieldDescriptionList()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the SpeciesObservationFieldDescriptionList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public SpeciesObservationFieldDescriptionList(Boolean optimize)
            : base(optimize)
        {
        }
    }
}


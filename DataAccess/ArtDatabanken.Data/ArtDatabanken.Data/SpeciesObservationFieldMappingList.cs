using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the ISpeciesObservationFieldMapping interface.
    /// </summary>
    [Serializable]
    public class SpeciesObservationFieldMappingList : DataId32List<ISpeciesObservationFieldMapping>
    {
        /// <summary>
        /// Constructor for the SpeciesObservationFieldMappingList class.
        /// </summary>
        public SpeciesObservationFieldMappingList()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the SpeciesObservationFieldMappingList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public SpeciesObservationFieldMappingList(Boolean optimize)
            : base(optimize)
        {
        }
    }
}



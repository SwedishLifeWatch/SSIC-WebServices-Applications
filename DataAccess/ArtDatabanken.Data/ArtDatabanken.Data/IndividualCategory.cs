using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents an individual category.
    /// </summary>
    public class IndividualCategory : IIndividualCategory
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Definition for this individual category.
        /// </summary>
        public String Definition { get; set; }

        /// <summary>
        /// Id for this individual category.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Name for this individual category.
        /// </summary>
        public String Name { get; set; }
    }
}
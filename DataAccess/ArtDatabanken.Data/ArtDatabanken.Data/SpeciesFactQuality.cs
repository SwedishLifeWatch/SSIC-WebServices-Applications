using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents a species fact quality.
    /// </summary>
    public class SpeciesFactQuality : ISpeciesFactQuality
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Definition for this species fact quality.
        /// </summary>
        public String Definition { get; set; }

        /// <summary>
        /// Id for this species fact quality.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Name for this species fact quality.
        /// </summary>
        public String Name { get; set; }
    }
}
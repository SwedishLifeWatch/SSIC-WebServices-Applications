using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a factor origin.
    /// </summary>
    public class FactorOrigin : IFactorOrigin
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Definition for this factor origin.
        /// </summary>
        public String Definition { get; set; }

        /// <summary>
        /// Id for this factor origin.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Name for this factor origin.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Sort order for this factor origin.
        /// </summary>
        public Int32 SortOrder { get; set; }
    }
}

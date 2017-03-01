using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// A specific reference.
    /// </summary>
    public class Reference : IReference
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Unique identification of a reference.
        /// Mandatory i.e. always required.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Name of the person that made the last
        /// update of this reference.
        /// </summary>
        public String ModifiedBy { get; set; }

        /// <summary>
        /// Date when the reference was last updated.
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Name of the reference.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Title for the reference.
        /// </summary>
        public String Title { get; set; }

        /// <summary>
        /// Reference year.
        /// </summary>
        public Int32? Year { get; set; }
    }
}

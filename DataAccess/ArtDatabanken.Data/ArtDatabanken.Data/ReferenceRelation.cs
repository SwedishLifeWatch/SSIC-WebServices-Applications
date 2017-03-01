using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class with information about a reference relation.
    /// </summary>
    public class ReferenceRelation : IReferenceRelation
    {
        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Id of the reference relation
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Reference that the related object is associated to.
        /// This property must be set by the application before
        /// the value is retrieved.
        /// </summary>
        public IReference Reference { get; set; }

        /// <summary>
        /// Id of the reference that the related object is associated to.
        /// </summary>
        public Int32 ReferenceId { get; set; }

        /// <summary>
        /// GUID for the object that the reference is related to.
        /// </summary>
        public String RelatedObjectGuid { get; set; }

        /// <summary>
        /// Type of reference relation.
        /// </summary>
        public IReferenceRelationType Type { get; set; }
    }
}

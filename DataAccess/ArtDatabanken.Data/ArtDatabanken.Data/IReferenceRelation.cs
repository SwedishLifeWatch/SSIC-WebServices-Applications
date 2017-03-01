using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Handles a relation between a reference and another object,
    /// for example Taxon or TaxonName.
    /// </summary>
    public interface IReferenceRelation : IDataId32
    {
        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Reference that the related object is associated to.
        /// This property must be set by the application before
        /// the value is retrieved.
        /// </summary>
        IReference Reference { get; set; }

        /// <summary>
        /// Id of the reference that the related object is associated to.
        /// </summary>
        Int32 ReferenceId { get; set; }

        /// <summary>
        /// Guid for the object that the reference is related to.
        /// </summary>
        String RelatedObjectGuid { get; set; }

        /// <summary>
        /// Type of reference relation.
        /// </summary>
        IReferenceRelationType Type { get; set; }
    }
}

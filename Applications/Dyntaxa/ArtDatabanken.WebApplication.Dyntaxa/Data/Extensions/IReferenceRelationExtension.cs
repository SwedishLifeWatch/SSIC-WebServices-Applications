using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions
{
    /// <summary>
    /// Contains extension methods to the IReferenceRelation interface.
    /// </summary>
    public static class IReferenceRelationExtension
    {
        /// <summary>
        /// Get the reference that belongs to this reference relations.
        /// </summary>
        /// <param name="referenceRelation">The reference relation.</param>
        /// <returns>The reference that belongs to this reference relations.</returns>
        public static IReference GetReference(this IReferenceRelation referenceRelation, IUserContext userContext)
        {            
            IReference reference;

            if (referenceRelation.Reference.IsNull())
            {
                reference = CoreData.ReferenceManager.GetReference(userContext, referenceRelation.ReferenceId);
                referenceRelation.Reference = reference;
                //referenceRelation.Reference = new ArtDatabanken.Data.Reference();
                //referenceRelation.Reference.Id = reference.Id;
                //referenceRelation.Reference.Name = reference.Name;
                //referenceRelation.Reference.Title = reference.Title;
                //referenceRelation.Reference.Year = reference.Year;                
            }

            return referenceRelation.Reference;
        }
    }
}

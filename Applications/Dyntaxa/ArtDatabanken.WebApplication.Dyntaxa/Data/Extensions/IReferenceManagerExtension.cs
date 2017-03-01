using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions
{
    /// <summary>
    /// Contains extension methods to the IReferenceManager interface.
    /// </summary>
    public static class IReferenceManagerExtension
    {
        /// <summary>
        /// Creates and deletes reference relations.
        /// </summary>
        /// <param name="referenceManager">A reference manager instance.</param>
        /// <param name="userContext">User context</param>
        /// <param name="referenceRelationsToCreate">List of Reference relation objects to be created.</param>
        /// <param name="referenceRelationsToDelete">List of Reference relation objects to be deleted.</param>
        public static void CreateDeleteReferenceRelations(
            this IReferenceManager referenceManager,
            IUserContext userContext,
            ReferenceRelationList referenceRelationsToCreate,
            ReferenceRelationList referenceRelationsToDelete)
        {
            referenceManager.CreateReferenceRelations(userContext, referenceRelationsToCreate);
            referenceManager.DeleteReferenceRelations(userContext, referenceRelationsToDelete);
        }
    }
}

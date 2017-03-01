using System;
using System.Collections.Generic;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Definition of the IReferenceManager interface.
    /// This interface is used to handle reference related information.
    /// </summary>
    public interface IReferenceManager : IManager
    {
        /// <summary>
        /// This interface is used to retrieve or update
        /// reference information from the actual data source.
        /// </summary>
        IReferenceDataSource DataSource { get; set; }

        /// <summary>
        /// Create a new reference.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="reference">New reference to create.</param>
        void CreateReference(IUserContext userContext,
                             IReference reference);

        /// <summary>
        /// Create a reference relation.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="referenceRelation">
        /// Information about the new reference relation.
        /// This object is updated with changes after the creation.
        /// </param>
        void CreateReferenceRelation(IUserContext userContext,
                                     IReferenceRelation referenceRelation);

        /// <summary>
        /// Create reference relations.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="referenceRelations">
        /// Information about the new reference relations.
        /// These objects are updated with changes after the creation.
        /// </param>
        void CreateReferenceRelations(IUserContext userContext,
                                      ReferenceRelationList referenceRelations);

        /// <summary>
        /// Delete a reference relation.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="referenceRelation">Reference relation to be deleted.</param>
        void DeleteReferenceRelation(IUserContext userContext,
                                     IReferenceRelation referenceRelation);

        /// <summary>
        /// Delete reference relations.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="referenceRelations">Reference relations that should be deleted.</param>
        void DeleteReferenceRelations(IUserContext userContext,
                                      ReferenceRelationList referenceRelations);

        /// <summary>
        /// Get specified reference.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="referenceId">Reference id.</param>
        /// <returns>Specified reference.</returns>
        IReference GetReference(IUserContext userContext,
                                Int32 referenceId);

        /// <summary> 
        /// Get specified reference relation.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="referenceRelationId">Id for a reference relation.</param>
        /// <returns>Specified reference relation.</returns>
        IReferenceRelation GetReferenceRelation(IUserContext userContext,
                                                Int32 referenceRelationId);

        /// <summary>
        /// Get reference relations that are related to specified object.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="relatedObjectGuid">GUID for the related object.</param>
        /// <returns>Reference relations that are related to specified object.</returns>
        ReferenceRelationList GetReferenceRelations(IUserContext userContext,
                                                    String relatedObjectGuid);

        /// <summary>
        /// Get specified reference relation type.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="referenceRelationTypeId">Reference relation type id.</param>
        /// <returns>Specified reference relation type.</returns>
        IReferenceRelationType GetReferenceRelationType(IUserContext userContext,
                                                        Int32 referenceRelationTypeId);

        /// <summary>
        /// Get specified reference relation type.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="referenceRelationTypeId">Reference relation type id.</param>
        /// <returns>Specified reference relation type.</returns>
        IReferenceRelationType GetReferenceRelationType(IUserContext userContext,
                                                        ReferenceRelationTypeId referenceRelationTypeId);

        /// <summary>
        /// Get all reference relation types.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All reference relation types.</returns>
        ReferenceRelationTypeList GetReferenceRelationTypes(IUserContext userContext);

        /// <summary>
        /// Get all references.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All references.</returns>
        ReferenceList GetReferences(IUserContext userContext);

        /// <summary>
        /// Get specified references.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="referenceIds">Reference ids.</param>
        /// <returns>Specified references.</returns>
        ReferenceList GetReferences(IUserContext userContext,
                                    List<Int32> referenceIds);

        /// <summary>
        /// Get references that matches search criteria.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="searchCriteria">Reference search criteria.</param>
        /// <returns>References that matches search criteria.</returns>
        ReferenceList GetReferences(IUserContext userContext,
                                    IReferenceSearchCriteria searchCriteria);

        /// <summary>
        /// Update existing reference.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="reference">Existing reference to update.</param>
        void UpdateReference(IUserContext userContext,
                             IReference reference);
    }
}

using System;
using System.Collections.Generic;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Definition of the IReferenceManager interface.
    /// This interface is used to handle reference related information.
    /// </summary>
    public class ReferenceManager : IReferenceManager
    {
        /// <summary>
        /// This interface is used to retrieve or update
        /// reference information from the actual data source.
        /// </summary>
        public IReferenceDataSource DataSource { get; set; }

        /// <summary>
        /// Create a new reference.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="reference">New reference to create.</param>
        public virtual void CreateReference(IUserContext userContext,
                                            IReference reference)
        {
            DataSource.CreateReference(userContext, reference);
        }

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
        public virtual void CreateReferenceRelation(IUserContext userContext,
                                                    IReferenceRelation referenceRelation)
        {
            DataSource.CreateReferenceRelation(userContext, referenceRelation);
        }

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
        public virtual void CreateReferenceRelations(IUserContext userContext,
                                                     ReferenceRelationList referenceRelations)
        {
            if (referenceRelations.IsNotEmpty())
            {
                foreach (IReferenceRelation referenceRelation in referenceRelations)
                {
                    CreateReferenceRelation(userContext, referenceRelation);
                }
            }
        }

        /// <summary>
        /// Delete a reference relation.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="referenceRelation">Reference relation to be deleted.</param>
        public virtual void DeleteReferenceRelation(IUserContext userContext,
                                                    IReferenceRelation referenceRelation)
        {
            DataSource.DeleteReferenceRelation(userContext,
                                                    referenceRelation);
        }

        /// <summary>
        /// Delete reference relations.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="referenceRelations">Reference relations that should be deleted.</param>
        public virtual void DeleteReferenceRelations(IUserContext userContext,
                                                     ReferenceRelationList referenceRelations)
        {
            if (referenceRelations.IsNotEmpty())
            {
                foreach (IReferenceRelation referenceRelation in referenceRelations)
                {
                    DeleteReferenceRelation(userContext, referenceRelation);
                }
            }
        }

        /// <summary>
        /// Get information about data source.
        /// </summary>
        /// <returns>Information about data source.</returns>
        public virtual IDataSourceInformation GetDataSourceInformation()
        {
            return DataSource.GetDataSourceInformation();
        }

        /// <summary>
        /// Get specified reference.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="referenceId">Reference id.</param>
        /// <returns>Specified reference.</returns>
        public virtual IReference GetReference(IUserContext userContext,
                                               Int32 referenceId)
        {
            List<Int32> referenceIds;
            ReferenceList references;

            referenceIds = new List<Int32>();
            referenceIds.Add(referenceId);
            references = GetReferences(userContext, referenceIds);
            if (references.IsEmpty())
            {
                return null;
            }
            else
            {
                return references[0];
            }
        }

        /// <summary> 
        /// Get specified reference relation.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="referenceRelationId">Id for a reference relation.</param>
        /// <returns>Specified reference relation.</returns>
        public virtual IReferenceRelation GetReferenceRelation(IUserContext userContext,
                                                               Int32 referenceRelationId)
        {
            return DataSource.GetReferenceRelation(userContext,
                                                        referenceRelationId);
        }

        /// <summary>
        /// Get reference relations that are related to specified object.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="relatedObjectGuid">GUID for the related object.</param>
        /// <returns>Reference relations that are related to specified object.</returns>
        public virtual ReferenceRelationList GetReferenceRelations(IUserContext userContext,
                                                                   String relatedObjectGuid)
        {
            // Check parameters.
            relatedObjectGuid.CheckNotEmpty("relatedObjectGuid");

            // Get data.
            return DataSource.GetReferenceRelations(userContext, relatedObjectGuid);
        }

        /// <summary>
        /// Get specified reference relation type.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="referenceRelationTypeId">Reference relation type id.</param>
        /// <returns>Specified reference relation type.</returns>
        public virtual IReferenceRelationType GetReferenceRelationType(IUserContext userContext,
                                                                       Int32 referenceRelationTypeId)
        {
            return GetReferenceRelationTypes(userContext).Get(referenceRelationTypeId);
        }

        /// <summary>
        /// Get specified reference relation type.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="referenceRelationTypeId">Reference relation type id.</param>
        /// <returns>Specified reference relation type.</returns>
        public virtual IReferenceRelationType GetReferenceRelationType(IUserContext userContext,
                                                                       ReferenceRelationTypeId referenceRelationTypeId)
        {
            return GetReferenceRelationTypes(userContext).Get((Int32)referenceRelationTypeId);
        }

        /// <summary>
        /// Get all reference relation types.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All reference relation types.</returns>
        public virtual ReferenceRelationTypeList GetReferenceRelationTypes(IUserContext userContext)
        {
            return DataSource.GetReferenceRelationTypes(userContext);
        }

        /// <summary>
        /// Get all references.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All references.</returns>
        public virtual ReferenceList GetReferences(IUserContext userContext)
        {
            return DataSource.GetReferences(userContext);
        }

        /// <summary>
        /// Get specified references.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="referenceIds">Reference ids.</param>
        /// <returns>Specified references.</returns>
        public virtual ReferenceList GetReferences(IUserContext userContext,
                                                   List<Int32> referenceIds)
        {
            return DataSource.GetReferences(userContext, referenceIds);
        }

        /// <summary>
        /// Get references that matches search criteria.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="searchCriteria">Reference search criteria.</param>
        /// <returns>References that matches search criteria.</returns>
        public virtual ReferenceList GetReferences(IUserContext userContext,
                                                   IReferenceSearchCriteria searchCriteria)
        {
            return DataSource.GetReferences(userContext, searchCriteria);
        }

        /// <summary>
        /// Update existing reference.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="reference">Existing reference to update.</param>
        public virtual void UpdateReference(IUserContext userContext,
                                            IReference reference)
        {
            DataSource.UpdateReference(userContext, reference);
        }
    }
}

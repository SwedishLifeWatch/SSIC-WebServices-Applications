using System;
using System.Collections.Generic;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Interface that handles picture related information.
    /// </summary>
    public class PictureManager : IPictureManager
    {
        /// <summary>
        /// This property is used to retrieve or update picture information.
        /// </summary>
        public IPictureDataSource DataSource { get; set; }

        /// <summary>
        /// Creates a new picture filename.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="picture">Picture to save to disk.</param>
        /// <param name="filename">Filename of the picture filename.</param>
        /// <param name="lastModified">The date when the picture was last modified.</param>
        /// <param name="versionId">Version number of picture.</param>
        /// <param name="updatedBy">User who last updated the picture.</param>
        /// <param name="createMetaData">List of metadata to create.</param>
        /// <returns>Response message containing user context created pictureId and number of inserted rows.</returns>
        public virtual IPictureResponse CreatePictureFilename(IUserContext userContext,
                                                              String picture,
                                                              String filename,
                                                              DateTime? lastModified,
                                                              Int64 versionId,
                                                              String updatedBy,
                                                              PictureMetaDataList createMetaData)
        {
            return DataSource.CreatePictureFilename(userContext,
                                                    picture,
                                                    filename,
                                                    lastModified,
                                                    versionId,
                                                    updatedBy,
                                                    createMetaData);
        }

        /// <summary>
        /// Update pictures.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="updatePictures">List of pictures to update.</param>
        /// <param name="updatedBy">User who last updated the picture.</param>
        /// <returns>Number of updated records.</returns>
        public virtual Int32 UpdatePictures(IUserContext userContext,
                                            PictureList updatePictures,
                                            String updatedBy)
        {
            return DataSource.UpdatePictures(userContext,
                                             updatePictures,
                                             updatedBy);
        }

        /// <summary>
        /// Deletes a picture filename.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="pictureId">Id of the picture to delete.</param>
        /// <param name="filename">Filename of the picture filename.</param>
        /// <param name="pictureStringId">PictureStringId for the picture filename.</param>
        /// <returns>Number of deleted rows.</returns>
        public virtual Int32 DeletePictureFilename(IUserContext userContext,
                                                   Int64? pictureId,
                                                   String filename,
                                                   Int64 pictureStringId)
        {
            return DataSource.DeletePictureFilename(userContext,
                                                    pictureId,
                                                    filename,
                                                    pictureStringId);
        }

        /// <summary>
        /// Create picture metadata
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="pictureId">Id of the picture whose metadata should be created.</param>
        /// <param name="createMetaData">List of metadata to create.</param>
        public virtual void CreatePictureMetaData(IUserContext userContext,
                                                  Int64 pictureId,
                                                  PictureMetaDataList createMetaData)
        {
            DataSource.CreatePictureMetaData(userContext,
                                             pictureId,
                                             createMetaData);
        }

        /// <summary>
        /// Update picture metadata
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="pictureId">Id of the picture whose metadata should be updated.</param>
        /// <param name="updatedBy">User who last updated the picture.</param>
        /// <param name="updateMetaData">List of metadata to update.</param>
        /// <returns>Number of updated records.</returns>
        public virtual Int32 UpdatePictureMetaData(IUserContext userContext,
                                                   Int64 pictureId,
                                                   String updatedBy,
                                                   PictureMetaDataList updateMetaData)
        {
            return DataSource.UpdatePictureMetaData(userContext,
                                                    pictureId,
                                                    updatedBy,
                                                    updateMetaData);
        }

        /// <summary>
        /// Delete picture metadata
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="pictureId">Id of the picture whose metadata should be deleted.</param>
        /// <param name="deleteMetaData">List of metadata to delete.</param>
        /// <returns>Number of deleted records.</returns>
        public virtual Int32 DeletePictureMetaData(IUserContext userContext,
                                                   Int64 pictureId,
                                                   PictureMetaDataList deleteMetaData)
        {
            return DataSource.DeletePictureMetaData(userContext,
                                                    pictureId,
                                                    deleteMetaData);
        }

        /// <summary>
        /// Create picture relations.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="createPictureRelations">List of picture relations to create.</param>
        public virtual void CreatePictureRelations(IUserContext userContext,
                                                   PictureRelationList createPictureRelations)
        {
            DataSource.CreatePictureRelations(userContext,
                                              createPictureRelations);
        }

        /// <summary>
        /// Update picture relations.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="updatePictureRelations">List of picture relations to update.</param>
        /// <returns>Number of updated records.</returns>
        public virtual Int32 UpdatePictureRelations(IUserContext userContext,
                                                    PictureRelationList updatePictureRelations)
        {
            return DataSource.UpdatePictureRelations(userContext,
                                                     updatePictureRelations);
        }

        /// <summary>
        /// Delete picture relations.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="pictureRelationIds">List of ids of picture relations to delete.</param>
        /// <returns>Number of deleted records.</returns>
        public virtual Int32 DeletePictureRelations(IUserContext userContext,
                                                    List<Int64> pictureRelationIds)
        {
            return DataSource.DeletePictureRelations(userContext,
                                                     pictureRelationIds);
        }

        /// <summary>
        /// Get picture with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="pictureId">Picture id.</param>
        /// <param name="height">Specifies a particular height.</param>
        /// <param name="width">Specifies a particular width.</param>
        /// <param name="requestedSize">Requested size of retrieved.</param>
        /// <param name="isRequestedSizeSpecified">Indicates if requested size has been specified.</param>
        /// <param name="requestedFormat">Requested format of returned picture.</param>
        /// <returns>Specified picture.</returns>
        public virtual IPicture GetPicture(IUserContext userContext,
                                           Int64 pictureId,
                                           Int32? height,
                                           Int32? width,
                                           Int64 requestedSize,
                                           Boolean isRequestedSizeSpecified,
                                           String requestedFormat)
        {
            return DataSource.GetPicture(userContext,
                                             pictureId,
                                             height,
                                             width,
                                             requestedSize,
                                             isRequestedSizeSpecified,
                                             requestedFormat);
        }

        /// <summary>
        /// Get picture with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="pictureStringId">Unique id for picture.</param>
        /// <returns>Specified picture.</returns>
        public virtual IPicture GetPictureByPictureStringId(IUserContext userContext, Int64 pictureStringId)
        {
            return DataSource.GetPictureByPictureStringId(userContext, pictureStringId);
        }

        /// <summary>
        /// Get picture by picture filename.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="pictureFilename">Picture filename.</param>
        /// <returns>Specified picture.</returns>
        public virtual IPicture GetPictureByPictureFilename(IUserContext userContext, String pictureFilename)
        {
            return DataSource.GetPictureByPictureFilename(userContext, pictureFilename);
        }

        /// <summary>
        /// Get information about picture with specified id.
        /// This information contains the picture, meta data
        /// and picture relations.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="pictureId">Picture id.</param>
        /// <param name="height">Specifies a particular height.</param>
        /// <param name="width">Specifies a particular width.</param>
        /// <param name="metaDataIds">Desired metadata to be returned along with the picture.</param>
        /// <returns>Information about picture with specified id.</returns>
        public virtual IPictureInformation GetPictureInformation(IUserContext userContext,
                                                                 Int64 pictureId,
                                                                 Int32? height,
                                                                 Int32? width,
                                                                 List<Int32> metaDataIds)
        {
            return DataSource.GetPictureInformation(userContext,
                                                    pictureId,
                                                    height,
                                                    width,
                                                    metaDataIds);
        }

        /// <summary>
        /// Get meta data about picture with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="pictureId">Picture id.</param>
        /// <param name="metaDataIds">Desired meta data to be returned along with the picture.</param>
        /// <returns>Meta data about picture with specified id.</returns>
        public virtual PictureMetaDataList GetPictureMetaData(IUserContext userContext,
                                                              Int64 pictureId,
                                                              List<Int32> metaDataIds)
        {
            return DataSource.GetPictureMetaData(userContext,
                                                 pictureId,
                                                 metaDataIds);
        }

        /// <summary>
        /// Get metadata about all recommended pictures selected by picture relation type id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="pictureRelationType">Type of picture to get metadata for. </param>
        /// <param name="pictureMetaDataDescriptionsIds">Ids of the metadadescriptions to be retrieved.</param>
        /// <returns>Picture metadata descriptions for specified ids and pictureId.</returns>
        public PictureMetaDataInformationList GetAllRecommendedPicturesMetaData(IUserContext userContext,
                                                                                IPictureRelationType pictureRelationType,
                                                                                List<Int32> pictureMetaDataDescriptionsIds)
        {
            return DataSource.GetAllRecommendedPicturesMetaData(userContext,
                                                                pictureRelationType,
                                                                pictureMetaDataDescriptionsIds);
        }

        /// <summary>
        /// Get picture relation data type with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="pictureRelationDataTypeId">Picture relation data type id.</param>
        /// <returns>Picture relation data type with specified id.</returns>
        public virtual IPictureRelationDataType GetPictureRelationDataType(IUserContext userContext,
                                                                           Int32 pictureRelationDataTypeId)
        {
            return GetPictureRelationDataTypes(userContext).Get(pictureRelationDataTypeId);
        }

        /// <summary>
        /// Get all picture relation data types.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All picture relation data types.</returns>
        public virtual PictureRelationDataTypeList GetPictureRelationDataTypes(IUserContext userContext)
        {
            return DataSource.GetPictureRelationDataTypes(userContext);
        }

        /// <summary>
        /// Get picture relations related to specified factor.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factor">Factor that may be related to pictures.</param>
        /// <returns>Picture relations related to specified factor.</returns>
        public virtual PictureRelationList GetPictureRelations(IUserContext userContext,
                                                               IFactor factor)
        {
            var pictureRelationType = GetPictureRelationType(userContext,
                PictureRelationTypeIdentifier.Factor);
            return DataSource.GetPictureRelations(userContext,
                                                  factor.Id.ToString(),
                                                  pictureRelationType);
        }

        /// <summary>
        /// Get picture relations related to specified picture.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="pictureId">Id of specified picture.</param>
        /// <returns>Picture relations related to specified picture.</returns>
        public virtual PictureRelationList GetPictureRelations(IUserContext userContext,
                                                                          Int64 pictureId)
        {
            return DataSource.GetPictureRelations(userContext, pictureId);
        }

        /// <summary>
        /// Get picture relations related to specified species fact.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="speciesFact">Species fact that may be related to pictures.</param>
        /// <returns>Picture relations related to specified species fact.</returns>
        public virtual PictureRelationList GetPictureRelations(IUserContext userContext,
                                                               ISpeciesFact speciesFact)
        {
            IPictureRelationType pictureRelationType;

            pictureRelationType = GetPictureRelationType(userContext,
                                                         PictureRelationTypeIdentifier.SpeciesFact);
            return DataSource.GetPictureRelations(userContext,
                                                  speciesFact.Identifier,
                                                  pictureRelationType);
        }

        /// <summary>
        /// Get picture relations related to specified taxon.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="taxon">Taxon that may be related to pictures.</param>
        /// <param name="pictureRelationType">Type of picture relations that are requested.</param>
        /// <returns>Picture relations related to specified taxon.</returns>
        public virtual PictureRelationList GetPictureRelations(IUserContext userContext,
                                                               ITaxon taxon,
                                                               IPictureRelationType pictureRelationType)
        {
            return DataSource.GetPictureRelations(userContext,
                                                  taxon.Id.ToString(),
                                                  pictureRelationType);
        }

        /// <summary>
        /// Get requested picture relation type.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="pictureRelationTypeId">
        /// Id for requested picture relation type.
        /// </param>
        /// <returns>Requested picture relation type.</returns>
        public virtual IPictureRelationType GetPictureRelationType(IUserContext userContext,
                                                                   Int32 pictureRelationTypeId)
        {
            return GetPictureRelationTypes(userContext).Get(pictureRelationTypeId);
        }

        /// <summary>
        /// Get requested picture relation type.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="pictureRelationTypeIdentifier">
        /// Identifier for requested picture relation type.
        /// </param>
        /// <returns>Requested picture relation type.</returns>
        public virtual IPictureRelationType GetPictureRelationType(IUserContext userContext,
                                                                   PictureRelationTypeIdentifier pictureRelationTypeIdentifier)
        {
            return GetPictureRelationTypes(userContext).Get(pictureRelationTypeIdentifier);
        }

        /// <summary>
        /// Get all picture relation types.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All picture relation types.</returns>
        public virtual PictureRelationTypeList GetPictureRelationTypes(IUserContext userContext)
        {
            return DataSource.GetPictureRelationTypes(userContext);
        }

        /// <summary>
        /// Get pictures with specified ids.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="pictureIds">Picture ids.</param>
        /// <param name="height">Specifies a particular height.</param>
        /// <param name="width">Specifies a particular width.</param>
        /// <param name="requestedSize">Requested size of retrieved.</param>
        /// <param name="isRequestedSizeSpecified">Indicates if requested size has been specified.</param>
        /// <param name="requestedFormat">Requested format of returned picture.</param>
        /// <returns>Specified pictures.</returns>
        public virtual PictureList GetPictures(IUserContext userContext,
                                               List<Int64> pictureIds,
                                               Int32? height,
                                               Int32? width,
                                               Int64 requestedSize,
                                               Boolean isRequestedSizeSpecified,
                                               String requestedFormat)
        {
            return DataSource.GetPictures(userContext,
                                               pictureIds,
                                               height,
                                               width,
                                               requestedSize,
                                               isRequestedSizeSpecified,
                                               requestedFormat);
        }

        /// <summary>
        /// Get information about pictures that matches search criteria.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="searchCriteria">Pictures search criteria.</param>
        /// <returns>Pictures that matches search criteria.</returns>
        public virtual PictureList GetPictures(IUserContext userContext,
                                               IPicturesSearchCriteria searchCriteria)
        {
            return DataSource.GetPictures(userContext, searchCriteria);
        }
        
        /// <summary>
        /// Get information about pictures that match search criteria.
        /// This information contains the picture, metadata
        /// and picture relations.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>        
        /// <param name="searchCriteria">Pictures search criteria.</param>
        /// <param name="height">Specifies a particular height.</param>
        /// <param name="width">Specifies a particular width.</param>
        /// <param name="requestedSize">Requested size of retrieved.</param>        
        /// <param name="requestedFormat">Requested format of returned picture.</param>
        /// <param name="metaDataIds">Desired metadata to be returned along with the picture.</param>
        /// <returns>Information about pictures that matches search criteria.</returns>
        public List<IPictureInformation> GetPicturesInformation(
            IUserContext userContext, 
            IPicturesSearchCriteria searchCriteria,
            Int32? height,
            Int32? width,
            Int64? requestedSize,            
            String requestedFormat,
            List<Int32> metaDataIds)
        {
            return DataSource.GetPicturesInformation(userContext,
                searchCriteria,
                height,
                width,
                requestedSize,
                requestedFormat,
                metaDataIds);
        }

        /// <summary>
        /// Get recommended picture ids related to corresponding object guids.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="objectGuids">Guids for objects that may be related to a picture.</param>
        /// <param name="pictureRelationType">Picture relation of this type.</param>
        /// <returns>Picture guids related to specified object.</returns>
        public PictureGuidList GetRecommendedPictureIdsByObjectGuid(IUserContext userContext,
                                                                    List<string> objectGuids,
                                                                    IPictureRelationType pictureRelationType)
        {
            PictureGuidList pictureGuidList;
            pictureGuidList = DataSource.GetRecommendedPictureIdsByObjectGuid(userContext,
                                                                             objectGuids,
                                                                             pictureRelationType);
            return pictureGuidList;
        }


        /// <summary>
        /// Gets all recommended picutres from picture relation type.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="pictureRelationType">Picture relation of this type.</param>
        /// <returns>Picture guids related to specified object.</returns>
        public PictureGuidList GetAllRecommendedPictureIds(IUserContext userContext,
                                                                   IPictureRelationType pictureRelationType)
        {
            PictureGuidList pictureGuidList;
            pictureGuidList = DataSource.GetAllRecommendedPictureIds(userContext, pictureRelationType);
            return  pictureGuidList;
        }
    }
}

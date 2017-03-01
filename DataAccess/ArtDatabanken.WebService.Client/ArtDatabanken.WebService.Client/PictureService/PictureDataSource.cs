using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Client.UserService;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Client.PictureService
{
    /// <summary>
    /// This class is used to handle picture related information.
    /// </summary>
    public class PictureDataSource : PictureDataSourceBase, IPictureDataSource
    {
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
            List<WebPictureMetaData> webCreatePictureMetaData;
                
            CheckTransaction(userContext);
            webCreatePictureMetaData = GetWebPictureMetaData(createMetaData);

             // Create picture filename, and, eventually, metadata.
            WebPictureResponse response = WebServiceProxy.PictureService.CreatePictureFilename(GetClientInformation(userContext),
                                                                                               picture,
                                                                                               filename,
                                                                                               lastModified.GetValueOrDefault(),
                                                                                               lastModified.HasValue,
                                                                                               versionId,
                                                                                               updatedBy,
                                                                                               webCreatePictureMetaData);


            return GetPictureResponse(userContext, response);
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
            List<WebPicture> webUpdatePictures;
            
            CheckTransaction(userContext);

            webUpdatePictures = GetWebPictures(updatePictures);

            return WebServiceProxy.PictureService.UpdatePictures(GetClientInformation(userContext),
                                                                 webUpdatePictures,
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
            CheckTransaction(userContext);
            return WebServiceProxy.PictureService.DeletePictureFilename(GetClientInformation(userContext),
                                                                        pictureId,
                                                                        filename,
                                                                        pictureStringId);
        }

        /// <summary>
        /// Create picture metadata.
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
            List<WebPictureMetaData> webCreatePictureMetaData;

            CheckTransaction(userContext);
            webCreatePictureMetaData = GetWebPictureMetaData(createMetaData);

            // Create picture metadata.
            WebServiceProxy.PictureService.CreatePictureMetaData(GetClientInformation(userContext), pictureId, webCreatePictureMetaData);
        }

        /// <summary>
        /// Update picture metadata.
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
            List<WebPictureMetaData> webUpdatePictureMetaData;

            CheckTransaction(userContext);
            webUpdatePictureMetaData = GetWebPictureMetaData(updateMetaData);

            // Update picture metadata.
            return WebServiceProxy.PictureService.UpdatePictureMetaData(GetClientInformation(userContext),
                                                                        pictureId,
                                                                        updatedBy,
                                                                        webUpdatePictureMetaData);
        }

        /// <summary>
        /// Delete picture metadata.
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
            List<WebPictureMetaData> webDeletePictureMetaData;

            CheckTransaction(userContext);
            webDeletePictureMetaData = GetWebPictureMetaData(deleteMetaData);

            // Delete picture metadata.
            return WebServiceProxy.PictureService.DeletePictureMetaData(GetClientInformation(userContext), pictureId, webDeletePictureMetaData);
        }


        /// <summary>
        /// Convert a list of WebPictureGuid instances
        /// to a PictureGuidList.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webPictureGuids">List of WebPictureGuid instances.</param>
        /// <returns>Picture relations.</returns>
        private PictureGuidList GetPictureGuids(IUserContext userContext,
                                                List<WebPictureGuid> webPictureGuids)
        {
            PictureGuidList pictureGuids = null;
            if (webPictureGuids.IsNotEmpty())
            {
                pictureGuids = new PictureGuidList();
                foreach (WebPictureGuid webPictureGuid in webPictureGuids)
                {
                    pictureGuids.Add(GetPictureGuid(userContext, webPictureGuid));
                }
            }

            return pictureGuids;
        }

        /// <summary>
        /// Create picture relations.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="createPictureRelations">List of picture relations to create.</param>
        public virtual void CreatePictureRelations(IUserContext userContext, PictureRelationList createPictureRelations)
        {
            List<WebPictureRelation> webCreatePictureRelations;

            CheckTransaction(userContext);
            webCreatePictureRelations = GetWebPictureRelations(createPictureRelations);

            // Create picture relations.
            WebServiceProxy.PictureService.CreatePictureRelations(GetClientInformation(userContext), webCreatePictureRelations);
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
            List<WebPictureRelation> webUpdatePictureRelations;

            CheckTransaction(userContext);
            webUpdatePictureRelations = GetWebPictureRelations(updatePictureRelations);

            // Update picture relations.
            return WebServiceProxy.PictureService.UpdatePictureRelations(GetClientInformation(userContext),
                                                                         webUpdatePictureRelations);
        }

        /// <summary>
        /// Delete picture relations.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="pictureRelationIds">List of picture relations to delete.</param>
        /// <returns>Number of deleted records.</returns>
        public virtual Int32 DeletePictureRelations(IUserContext userContext,
                                                    List<Int64> pictureRelationIds)
        {
            CheckTransaction(userContext);

            // Delete picture relations.
            return WebServiceProxy.PictureService.DeletePictureRelations(GetClientInformation(userContext),
                                                                         pictureRelationIds);
        }

        /// <summary>
        /// Convert a WebPictureRelation instance into
        /// an IPictureRelation instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webPictureRelation">A WebPictureRelation instance.</param>
        /// <returns>An IPictureRelation instance.</returns>
        private IPictureRelation GetPictureRelation(IUserContext userContext,
                                                    WebPictureRelation webPictureRelation)
        {
            IPictureRelation pictureRelation;

            pictureRelation = new PictureRelation();
            pictureRelation.DataContext = GetDataContext(userContext);
            pictureRelation.Id = webPictureRelation.Id;
            pictureRelation.IsRecommended = webPictureRelation.IsRecommended;
            pictureRelation.ObjectGuid = webPictureRelation.ObjectGuid;
            pictureRelation.PictureId = webPictureRelation.PictureId;
            pictureRelation.SortOrder = webPictureRelation.SortOrder;
            pictureRelation.Type = CoreData.PictureManager.GetPictureRelationType(userContext,
                                                                                  webPictureRelation.TypeId);
            return pictureRelation;
        }

        /// <summary>
        /// Convert a WebPictureGuid instance into
        /// an IPictureGuid instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webPictureGuid">A WebPictureGuid instance.</param>
        /// <returns>An IPictureGuid instance.</returns>
        private IPictureGuid GetPictureGuid(IUserContext userContext,
                                            WebPictureGuid webPictureGuid)
        {
            IPictureGuid pictureGuid;

            pictureGuid = new PictureGuid();
            pictureGuid.DataContext = GetDataContext(userContext);
            pictureGuid.Id = webPictureGuid.PictureId;
            pictureGuid.ObjectGuid = webPictureGuid.ObjectGuid;
            return pictureGuid;
        }

        /// <summary>
        /// Convert a WebPictureRelationDataType instance into
        /// an IPictureRelationDataType instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webPictureRelationDataType">A WebPictureRelationDataType instance.</param>
        /// <returns>An IPictureRelationDataType instance.</returns>
        private IPictureRelationDataType GetPictureRelationDataType(IUserContext userContext,
                                                                    WebPictureRelationDataType webPictureRelationDataType)
        {
            IPictureRelationDataType pictureRelationDataType;

            pictureRelationDataType = new PictureRelationDataType();
            pictureRelationDataType.DataContext = GetDataContext(userContext);
            pictureRelationDataType.Description = webPictureRelationDataType.Description;
            pictureRelationDataType.Id = webPictureRelationDataType.Id;
            pictureRelationDataType.Identifier = webPictureRelationDataType.Identifier;
            pictureRelationDataType.Name = webPictureRelationDataType.Name;
            return pictureRelationDataType;
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
            List<WebPictureRelationDataType> webPictureRelationDataTypes;

            CheckTransaction(userContext);
            webPictureRelationDataTypes = WebServiceProxy.PictureService.GetPictureRelationDataTypes(GetClientInformation(userContext));
            return GetPictureRelationDataTypes(userContext, webPictureRelationDataTypes);
        }

        /// <summary>
        /// Convert a list of WebPictureRelationDataType instances
        /// to a PictureRelationDataTypeList.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webPictureRelationDataTypes">List of WebPictureRelationDataType instances.</param>
        /// <returns>Picture relation data types.</returns>
        private PictureRelationDataTypeList GetPictureRelationDataTypes(IUserContext userContext,
                                                                        List<WebPictureRelationDataType> webPictureRelationDataTypes)
        {
            PictureRelationDataTypeList pictureRelationDataTypes;

            pictureRelationDataTypes = null;
            if (webPictureRelationDataTypes.IsNotEmpty())
            {
                pictureRelationDataTypes = new PictureRelationDataTypeList();
                foreach (WebPictureRelationDataType webPictureRelationDataType in webPictureRelationDataTypes)
                {
                    pictureRelationDataTypes.Add(GetPictureRelationDataType(userContext, webPictureRelationDataType));
                }
            }

            return pictureRelationDataTypes;
        }

        /// <summary>
        /// Get picture relations related to specified object.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="objectGuid">Guid for an object that may be related to pictures.</param>
        /// <param name="pictureRelationType">Picture relation is of this type.</param>
        /// <returns>Picture relations related to specified object.</returns>
        public virtual PictureRelationList GetPictureRelations(IUserContext userContext,
                                                               String objectGuid,
                                                               IPictureRelationType pictureRelationType)
        {
            List<WebPictureRelation> webPictureRelations;

            CheckTransaction(userContext);
            webPictureRelations = WebServiceProxy.PictureService.GetPictureRelationsByObjectGuid(GetClientInformation(userContext),
                                                                                                 objectGuid,
                                                                                                 pictureRelationType.Id);
            return GetPictureRelations(userContext, webPictureRelations);
        }

        /// <summary>
        /// Convert a list of WebPictureRelation instances
        /// to a PictureRelationList.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webPictureRelations">List of WebPictureRelation instances.</param>
        /// <returns>Picture relations.</returns>
        private PictureRelationList GetPictureRelations(IUserContext userContext,
                                                        List<WebPictureRelation> webPictureRelations)
        {
            PictureRelationList pictureRelations;

            pictureRelations = null;
            if (webPictureRelations.IsNotEmpty())
            {
                pictureRelations = new PictureRelationList();
                foreach (WebPictureRelation webPictureRelation in webPictureRelations)
                {
                    pictureRelations.Add(GetPictureRelation(userContext, webPictureRelation));
                }
            }

            return pictureRelations;
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
            List<WebPictureRelation> webPictureRelations;

            CheckTransaction(userContext);
            webPictureRelations = WebServiceProxy.PictureService.GetPictureRelationsByPictureId(GetClientInformation(userContext),
                                                                                                pictureId);
            return GetPictureRelations(userContext, webPictureRelations);
        }

        /// <summary>
        /// Convert a WebPictureRelationType instance into
        /// an IPictureRelationType instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webPictureRelationType">A WebPictureRelationType instance.</param>
        /// <returns>An IPictureRelationType instance.</returns>
        private IPictureRelationType GetPictureRelationType(IUserContext userContext,
                                                            WebPictureRelationType webPictureRelationType)
        {
            IPictureRelationType pictureRelationType;

            pictureRelationType = new PictureRelationType();
            pictureRelationType.DataContext = GetDataContext(userContext);
            pictureRelationType.DataType = CoreData.PictureManager.GetPictureRelationDataType(userContext, webPictureRelationType.DataTypeId);
            pictureRelationType.Description = webPictureRelationType.Description;
            pictureRelationType.Id = webPictureRelationType.Id;
            pictureRelationType.Identifier = webPictureRelationType.Identifier;
            pictureRelationType.Name = webPictureRelationType.Name;
            return pictureRelationType;
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
            List<WebPictureRelationType> webPictureRelationTypes;

            CheckTransaction(userContext);
            webPictureRelationTypes = WebServiceProxy.PictureService.GetPictureRelationTypes(GetClientInformation(userContext));
            return GetPictureRelationTypes(userContext, webPictureRelationTypes);
        }

        /// <summary>
        /// Convert a list of WebPictureRelationType instances
        /// to a PictureRelationTypeList.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webPictureRelationTypes">List of WebPictureRelationType instances.</param>
        /// <returns>Picture relation types.</returns>
        private PictureRelationTypeList GetPictureRelationTypes(IUserContext userContext,
                                                                List<WebPictureRelationType> webPictureRelationTypes)
        {
            PictureRelationTypeList pictureRelationTypes;

            pictureRelationTypes = null;
            if (webPictureRelationTypes.IsNotEmpty())
            {
                pictureRelationTypes = new PictureRelationTypeList();
                foreach (WebPictureRelationType webPictureRelationType in webPictureRelationTypes)
                {
                    pictureRelationTypes.Add(GetPictureRelationType(userContext, webPictureRelationType));
                }
            }

            return pictureRelationTypes;
        }

        /// <summary>
        /// Converts a WebPicture instance into
        /// an IPicture instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webPicture">A WebPicture instance.</param>
        /// <returns>An IPicture instance.</returns>
        private IPicture GetPicture(IUserContext userContext,
                                    WebPicture webPicture)
        {
            IPicture picture = new Picture
            {
                DataContext = GetDataContext(userContext),
                Format = webPicture.Format,
                Guid = webPicture.Guid,
                Id = webPicture.Id,
                Image = webPicture.Image,
                IsArchived = webPicture.IsArchived,
                IsPublic = webPicture.IsPublic,
                LastModified = webPicture.LastModified,
                LastUpdated = webPicture.LastUpdated,
                OriginalSize = webPicture.OriginalSize,
                PictureStringId = webPicture.PictureStringId,
                Size = webPicture.Size,
                Taxon = null,
                UpdatedBy = webPicture.UpdatedBy,
                VersionId = webPicture.VersionId
            };

            if (webPicture.IsTaxonIdSpecified)
            {
                picture.Taxon = CoreData.TaxonManager.GetTaxon(userContext,
                                                               webPicture.TaxonId);
            }

            return picture;
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
            WebPicture webPicture = WebServiceProxy.PictureService.GetPictureById(GetClientInformation(userContext),
                                                                                  pictureId,
                                                                                  height,
                                                                                  width,
                                                                                  requestedSize,
                                                                                  isRequestedSizeSpecified,
                                                                                  requestedFormat);

            return GetPicture(userContext, webPicture);
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
            WebPicture webPicture = WebServiceProxy.PictureService.GetPictureByPictureStringId(GetClientInformation(userContext), pictureStringId);
            if (webPicture.IsNotNull() && webPicture.Format.IsNotNull())
            {
                return GetPicture(userContext, webPicture);
            }

            return null;
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
            WebPicture webPicture = WebServiceProxy.PictureService.GetPictureByPictureFilename(GetClientInformation(userContext), pictureFilename);
            if (webPicture.IsNotNull() && webPicture.Format.IsNotNull())
            {
                return GetPicture(userContext, webPicture);
            }

            return null;
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
            List<WebPicture> webPictures = WebServiceProxy.PictureService.GetPicturesByIds(GetClientInformation(userContext),
                                                                                           pictureIds,
                                                                                           height,
                                                                                           width,
                                                                                           requestedSize,
                                                                                           isRequestedSizeSpecified,
                                                                                           requestedFormat);

            return GetPictures(userContext, webPictures);
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
            List<WebPicture> webPictures;
            WebPicturesSearchCriteria webSearchCriteria;

            CheckTransaction(userContext);
            webSearchCriteria = GetPicturesSearchCriteria(searchCriteria);
            webPictures = WebServiceProxy.PictureService.GetPictureBySearchCriteria(GetClientInformation(userContext), webSearchCriteria);
            return GetPictures(userContext, webPictures);
        }

        /// <summary>
        /// Converts a WebPictureResponse instance into
        /// an IPictureResponse instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webPictureResponse">A WebPicture instance.</param>
        /// <returns>An IPictureResponse instance.</returns>
        private IPictureResponse GetPictureResponse(IUserContext userContext,
                                                    WebPictureResponse webPictureResponse)
        {
            IPictureResponse pictureResponse = new PictureResponse();
            pictureResponse.DataContext = GetDataContext(userContext);
            pictureResponse.AffectedRows = Convert.ToInt32(webPictureResponse.AffectedRows);
            pictureResponse.Id = webPictureResponse.Id;

            return pictureResponse;
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
            List<WebPictureInformation> webPictureInformations;
            WebPicturesSearchCriteria webSearchCriteria;

            CheckTransaction(userContext);
            webSearchCriteria = GetPicturesSearchCriteria(searchCriteria);            

            webPictureInformations = WebServiceProxy.PictureService.GetPicturesInformationBySearchCriteria(
                GetClientInformation(userContext),
                webSearchCriteria,
                height,
                width,
                requestedSize,
                requestedFormat,
                metaDataIds);
            
            return GetPictureInformations(userContext, webPictureInformations);
        }

        /// <summary>
        /// Convert a list of WebPicture instances
        /// to a PictureList.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webPictures">List of WebPicture instances.</param>
        /// <returns>A list of IPicture instances.</returns>
        private PictureList GetPictures(IUserContext userContext,
                                        List<WebPicture> webPictures)
        {
            PictureList pictures = null;

            if (webPictures.IsNotEmpty())
            {
                pictures = new PictureList();
                foreach (WebPicture webPicture in webPictures)
                {
                    pictures.Add(GetPicture(userContext, webPicture));
                }
            }

            return pictures;
        }

        /// <summary>
        /// Convert a WebPictureMetaData instance into
        /// an IPictureMetaData instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webPictureMetaData">A WebPictureMetaData instance.</param>
        /// <returns>An IPictureMEtaDataInstance.</returns>
        private IPictureMetaData GetPictureMetaData(IUserContext userContext,
                                                    WebPictureMetaData webPictureMetaData)
        {
            IPictureMetaData pictureMetaData = new PictureMetaData
                                                   {
                                                       DataContext = GetDataContext(userContext),
                                                       Id = webPictureMetaData.HasPictureMetaDataId ? webPictureMetaData.PictureMetaDataId : -1,
                                                       Name = webPictureMetaData.Name,
                                                       Value = webPictureMetaData.Value
                                                   };

            return pictureMetaData;
        }

        /// <summary>
        /// Get meta data about picture with specified id.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="pictureId">Picture id.</param>
        /// <param name="metaDataIds">Desired metadata to be returned along with the picture.</param>
        /// <returns>Meta data about picture with specified id.</returns>
        public virtual PictureMetaDataList GetPictureMetaData(IUserContext userContext,
                                                              Int64 pictureId,
                                                              List<Int32> metaDataIds)
        {
            List<WebPictureMetaData> webPictureMetaDataAttributes = WebServiceProxy.PictureService.GetPictureMetaDataById(GetClientInformation(userContext),
                                                                                                                          pictureId,
                                                                                                                          metaDataIds);

            return GetPictureMetaDataAttributes(userContext, webPictureMetaDataAttributes);
        }

        /// <summary>
        /// Convert a list of WebPictureMetaData instances
        /// to a PictureMetaDataList.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webPictureMetaDataAttributes">List of WebPictureMetaData instances.</param>
        /// <returns>Picture meta data.</returns>
        private PictureMetaDataList GetPictureMetaDataAttributes(IUserContext userContext,
                                                                 List<WebPictureMetaData> webPictureMetaDataAttributes)
        {
            PictureMetaDataList pictureMetaDataAttributes = null;

            if (webPictureMetaDataAttributes.IsNotEmpty())
            {
                pictureMetaDataAttributes = new PictureMetaDataList();
                foreach (WebPictureMetaData webPictureMetaData in webPictureMetaDataAttributes)
                {
                    pictureMetaDataAttributes.Add(GetPictureMetaData(userContext, webPictureMetaData));
                }
            }

            return pictureMetaDataAttributes;
        }

        /// <summary>
        /// Converts a list of WebPictureInformation instances
        /// into a list of IPictureInformation instances.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webPictureInformations">A list of WebPictureInformation instances.</param>
        /// <returns>A list of IPictureInformation instances.</returns>
        private List<IPictureInformation> GetPictureInformations(IUserContext userContext,
                                                                 IEnumerable<WebPictureInformation> webPictureInformations)
        {
            return webPictureInformations.Select(webPictureInformation => this.GetPictureInformation(userContext, webPictureInformation)).ToList();
        }

        /// <summary>
        /// Converts a WebPictureInformation instance
        /// into an IPictureInformation instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webPictureInformation">A WebPictureInformation instance.</param>
        /// <returns>An IPictureInformation instance.</returns>
        private IPictureInformation GetPictureInformation(IUserContext userContext,
                                                          WebPictureInformation webPictureInformation)
        {
            IPictureInformation pictureInformation = new PictureInformation
                                                         {
                                                             DataContext = GetDataContext(userContext),
                                                             Id = webPictureInformation.Id,
                                                             MetaData = GetPictureMetaDataAttributes(userContext,
                                                                                                     webPictureInformation.MetaData),
                                                             Picture = GetPicture(userContext,
                                                                                  webPictureInformation.Picture),
                                                             Relations = GetPictureRelations(userContext,
                                                                                             webPictureInformation.Relations)
                                                         };

            return pictureInformation;
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
            WebPictureInformation webPictureInformation = WebServiceProxy.PictureService.GetPictureInformationById(GetClientInformation(userContext),
                                                                                                                   pictureId,
                                                                                                                   height,
                                                                                                                   width,
                                                                                                                   metaDataIds);

            return GetPictureInformation(userContext, webPictureInformation);
        }

        /// <summary>
        /// Get picture metadata descriptions for a specific culture.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>Picture metadata descriptions for specified culture.</returns>
        public virtual PictureMetaDataDescriptionList GetPictureMetaDataDescriptions(IUserContext userContext)
        {
            List<WebPictureMetaDataDescription> webPictureMetaDataDescriptions;

            CheckTransaction(userContext);
            webPictureMetaDataDescriptions = WebServiceProxy.PictureService.GetPictureMetaDataDescriptions(GetClientInformation(userContext));
            return GetPictureMetaDataDescriptions(userContext, webPictureMetaDataDescriptions);
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
        public virtual PictureMetaDataInformationList GetAllRecommendedPicturesMetaData(IUserContext userContext,
                                                                                       IPictureRelationType pictureRelationType,
                                                                                       List<Int32> pictureMetaDataDescriptionsIds)
        {
            List<WebPictureMetaDataInformation> webPictureMetaDataInformations;

            this.CheckTransaction(userContext);
            webPictureMetaDataInformations = WebServiceProxy.PictureService.GetAllRecommendedPicturesMetaData(this.GetClientInformation(userContext),
                                                                                                              pictureRelationType.Id,
                                                                                                              pictureMetaDataDescriptionsIds);
            return this.GetPictureMetaDataInformations(userContext,  webPictureMetaDataInformations);
        }
      
        /// <summary>
        /// Converts a WebPictureMetaDataDescription instance
        /// into an IPictureMetaDataDescription instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webPictureMetaDataDescription">A WebPictureMetaDataDescription instance.</param>
        /// <returns>An IPictureMetaDataDescription instance.</returns>
        private IPictureMetaDataDescription GetPictureMetaDataDescription(IUserContext userContext, WebPictureMetaDataDescription webPictureMetaDataDescription)
        {
            IPictureMetaDataDescription pictureMetaDataDescription = new PictureMetaDataDescription
                                                                         {
                                                                             DataContext = GetDataContext(userContext),
                                                                             Description = webPictureMetaDataDescription.Description,
                                                                             Exif = webPictureMetaDataDescription.Exif,
                                                                             Id = webPictureMetaDataDescription.Id,
                                                                             Name = webPictureMetaDataDescription.Name
                                                                         };

            return pictureMetaDataDescription;
        }

        /// <summary>
        /// Convert a list of WebPictureMetaDataDescription instances
        /// to a PictureMetaDataDescriptionList.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webPictureMetaDataDescriptions">List of WebPictureMetaDataDescription instances.</param>
        /// <returns>Picture metadata descriptions.</returns>
        private PictureMetaDataDescriptionList GetPictureMetaDataDescriptions(IUserContext userContext,
                                                                              List<WebPictureMetaDataDescription> webPictureMetaDataDescriptions)
        {
            PictureMetaDataDescriptionList pictureMetaDataDescriptions = null;

            if (webPictureMetaDataDescriptions.IsNotEmpty())
            {
                pictureMetaDataDescriptions = new PictureMetaDataDescriptionList();
                foreach (WebPictureMetaDataDescription webPictureMetaDataDescription in webPictureMetaDataDescriptions)
                {
                    pictureMetaDataDescriptions.Add(GetPictureMetaDataDescription(userContext, webPictureMetaDataDescription));
                }
            }

            return pictureMetaDataDescriptions;
        }

        /// <summary>
        /// Converts a WebPictureMetaDataInformation instance
        /// into an IPictureMetaDataInformation instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webPictureMetaDataInformation">A WebPictureMetaDataInformation instance.</param>
        /// <returns>An IPictureMetaDataInformation instance.</returns>
        private IPictureMetaDataInformation GetPictureMetaDataInformation(IUserContext userContext, WebPictureMetaDataInformation webPictureMetaDataInformation)
        {
            IPictureMetaDataInformation pictureMetaDataInformation = new PictureMetaDataInformation
            {
                DataContext = this.GetDataContext(userContext),
                Id = webPictureMetaDataInformation.PictureId,
                PictureMetaDataList = new List<IPictureMetaData>()
            };

            foreach (WebPictureMetaData webPictureMetaData in webPictureMetaDataInformation.PictureMetaDataList)
            {
                pictureMetaDataInformation.PictureMetaDataList.Add(this.GetPictureMetaData(userContext, webPictureMetaData));
            }
          
            return pictureMetaDataInformation;
        }

        /// <summary>
        /// Convert a list of WebPictureMetaDataInformation instances
        /// to a PictureMetaDataInformationList.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webPictureMetaDataInformations">List of WebPictureMetaDataInformation instances.</param>
        /// <returns>Picture metadata informatons.</returns>
        private PictureMetaDataInformationList GetPictureMetaDataInformations(IUserContext userContext,
                                                                              List<WebPictureMetaDataInformation> webPictureMetaDataInformations)
        {
            PictureMetaDataInformationList pictureMetaDataInformations = new PictureMetaDataInformationList();
            if (webPictureMetaDataInformations.IsNotEmpty())
            {
               
                foreach (WebPictureMetaDataInformation webPictureMetaDataInformation in webPictureMetaDataInformations)
                {
                    IPictureMetaDataInformation metaDataInformation = this.GetPictureMetaDataInformation(userContext, webPictureMetaDataInformation);
                    pictureMetaDataInformations.Add(metaDataInformation);
                }
            }
            return pictureMetaDataInformations;
        }
        /// <summary>
        /// Get picture metadata descriptions for specified ids.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="pictureMetaDataDescriptionsIds">Ids of the metadadescriptions to be retrieved.</param>
        /// <returns>Picture metadata descriptions for specified ids.</returns>
        public virtual PictureMetaDataDescriptionList GetPictureMetaDataDescriptions(IUserContext userContext,
                                                                                     List<Int32> pictureMetaDataDescriptionsIds)
        {
            List<WebPictureMetaDataDescription> webPictureMetaDataDescriptions;

            CheckTransaction(userContext);
            webPictureMetaDataDescriptions = WebServiceProxy.PictureService.GetPictureMetaDataDescriptionsByIds(GetClientInformation(userContext),
                                                                                                                pictureMetaDataDescriptionsIds);
            return GetPictureMetaDataDescriptions(userContext, webPictureMetaDataDescriptions);
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
        public virtual PictureGuidList GetRecommendedPictureIdsByObjectGuid(IUserContext userContext,
                                                               List<string> objectGuids,
                                                               IPictureRelationType pictureRelationType)
        {
            List<WebPictureGuid> webPictureGuid;

            CheckTransaction(userContext);
            webPictureGuid = WebServiceProxy.PictureService.GetRecommendedPictureIdsByObjectGuid(GetClientInformation(userContext),
                                                                                                 objectGuids,
                                                                                                 pictureRelationType.Id);
            return GetPictureGuids(userContext, webPictureGuid);
        }


        /// <summary>
        /// Gets all recommended picutres from picture relation type.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="pictureRelationType">Picture relation of this type.</param>
        /// <returns>Picture guids related to specified object.</returns>
        public virtual PictureGuidList GetAllRecommendedPictureIds(IUserContext userContext,
                                                                   IPictureRelationType pictureRelationType)
        {
            List<WebPictureGuid> webPictureGuid;

            CheckTransaction(userContext);
            webPictureGuid = WebServiceProxy.PictureService.GetAllRecommendedPictureIds(GetClientInformation(userContext),
                                                                                        pictureRelationType.Id);
            return GetPictureGuids(userContext, webPictureGuid);
        }

        /// <summary>
        /// Converts an IPicture instance to a WebPicture instance.
        /// </summary>
        /// <param name="picture">An IPicture instance.</param>
        /// <returns>A WebPicture instance.</returns>
        private WebPicture GetWebPicture(IPicture picture)
        {
            WebPicture webPicture = new WebPicture
                                        {
                                            Id = picture.Id,
                                            IsPublic = picture.IsPublic,
                                            IsArchived = picture.IsArchived,
                                            LastUpdated = picture.LastUpdated,
                                            UpdatedBy = picture.UpdatedBy
                                        };

            webPicture.IsTaxonIdSpecified = picture.Taxon.IsNotNull();
            if (webPicture.IsTaxonIdSpecified)
            {
                webPicture.TaxonId = picture.Taxon.Id;
            }

            return webPicture;
        }

        /// <summary>
        /// Converts a PictureList instance to a list of WebPicture instances.
        /// </summary>
        /// <param name="metaData">List of PictureMetaData instances.</param>
        /// <returns>List of WebPictureMetaData instances.</returns>
        private List<WebPicture> GetWebPictures(PictureList metaData)
        {
            List<WebPicture> webPictureMetaData = null;

            if (metaData.IsNotEmpty())
            {
                webPictureMetaData = metaData.Select(GetWebPicture).ToList();
            }

            return webPictureMetaData;
        }

        /// <summary>
        /// Converts an IPictureMetaData instance to a WebPictureMetaData instance.
        /// </summary>
        /// <param name="metaData">An IPictureMetaData instance.</param>
        /// <returns>A WebPictureMetaData instance.</returns>
        private WebPictureMetaData GetWebPictureMetaData(IPictureMetaData metaData)
        {
            WebPictureMetaData webPictureMetaData = new WebPictureMetaData
                                                        {
                                                            Name = metaData.Name,
                                                            HasPictureMetaDataId = metaData.Id > 0,
                                                            PictureMetaDataId = metaData.Id,
                                                            Value = metaData.Value
                                                        };

            return webPictureMetaData;
        }

        /// <summary>
        /// Converts a PictureMetaDataList instance to a list of WebPictureMetaData instances.
        /// </summary>
        /// <param name="metaData">List of PictureMetaData instances.</param>
        /// <returns>List of WebPictureMetaData instances.</returns>
        private List<WebPictureMetaData> GetWebPictureMetaData(PictureMetaDataList metaData)
        {
            List<WebPictureMetaData> webPictureMetaData = null;

            if (metaData.IsNotEmpty())
            {
                webPictureMetaData = metaData.Select(GetWebPictureMetaData).ToList();
            }

            return webPictureMetaData;
        }

        /// <summary>
        /// Converts an IPictureRelation instance to a WebPictureRelation instance.
        /// </summary>
        /// <param name="pictureRelation">An IPictureRealtion instance.</param>
        /// <returns>A WebPictureRelation instance.</returns>
        private WebPictureRelation GetWebPictureRelation(IPictureRelation pictureRelation)
        {
            WebPictureRelation webPictureRelation = new WebPictureRelation
                                                        {
                                                            Id = pictureRelation.Id,
                                                            IsRecommended = pictureRelation.IsRecommended,
                                                            ObjectGuid = pictureRelation.ObjectGuid,
                                                            PictureId = pictureRelation.PictureId,
                                                            SortOrder = pictureRelation.SortOrder,
                                                            TypeId = pictureRelation.Type.Id
                                                        };

            return webPictureRelation;
        }

        /// <summary>
        /// Converts a PictureRelationList instance to a list of WebPictureRelation instances.
        /// </summary>
        /// <param name="pictureRelations">List of PictureRelation instances.</param>
        /// <returns>List of WebPictureRelation instances.</returns>
        private List<WebPictureRelation> GetWebPictureRelations(PictureRelationList pictureRelations)
        {
            List<WebPictureRelation> webPictureRelations = null;

            if (pictureRelations.IsNotEmpty())
            {
                webPictureRelations = pictureRelations.Select(GetWebPictureRelation).ToList();
            }

            return webPictureRelations;
        }

        /// <summary>
        /// Set PictureService as data source
        /// in the onion data model.
        /// </summary>
        public static void SetDataSource()
        {
            PictureDataSource pictureDataSource;

            pictureDataSource = new PictureDataSource();
            CoreData.PictureManager.DataSource = new PictureDataSource();
            CoreData.MetadataManager.PictureDataSource = new PictureDataSource();
            ((UserDataSource)(CoreData.UserManager.DataSource)).UserLoggedInEvent += pictureDataSource.Login;
            ((UserDataSource)(CoreData.UserManager.DataSource)).UserLoggedOutEvent += pictureDataSource.Logout;
        }
    }
}
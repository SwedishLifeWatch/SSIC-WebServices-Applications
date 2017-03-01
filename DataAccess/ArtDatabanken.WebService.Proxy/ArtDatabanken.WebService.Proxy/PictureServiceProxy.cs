using System;
using System.Collections.Generic;
using System.ServiceModel;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy.PictureService;

namespace ArtDatabanken.WebService.Proxy
{
    /// <summary>
    /// Class that manages taxon attribute service requests.
    /// </summary>
    public class PictureServiceProxy : WebServiceProxyBase, ITransactionProxy, IWebService
    {
        /// <summary>
        /// Create a PictureServiceProxy instance.
        /// </summary>
        public PictureServiceProxy()
            : this(null)
        {
        }

        /// <summary>
        /// Create a PictureServiceProxy instance.
        /// </summary>
        /// <param name="webServiceAddress">
        /// Address to web service without internet protocol (http or https)
        /// and binding protocol (SOAP 1.1, SOAP 1.2 or binary).
        /// For example Picture.ArtDatabankenSOA.se/PictureService.svc.
        /// </param>
        public PictureServiceProxy(String webServiceAddress)
        {
            WebServiceAddress = webServiceAddress;
            switch (Configuration.InstallationType)
            {
                case InstallationType.ArtportalenTest:
                case InstallationType.ServerTest:
                    WebServiceComputer = WebServiceComputer.Moneses;
                    break;

                case InstallationType.LocalTest:
                    WebServiceComputer = WebServiceComputer.LocalTest;
                    break;

                case InstallationType.Production:
                    WebServiceComputer = WebServiceComputer.ArtDatabankenSoa;
                    break;

                case InstallationType.SpeciesFactTest:
                    WebServiceComputer = WebServiceComputer.SpeciesFactTest;
                    break;

                case InstallationType.SystemTest:
                    WebServiceComputer = WebServiceComputer.SystemTest;
                    break;

                case InstallationType.TwoBlueberriesTest:
                    WebServiceComputer = WebServiceComputer.TwoBlueberriesTest;
                    break;

                default:
                    throw new ApplicationException("Not handled installation type " + Configuration.InstallationType);
            }
        }

        /// <summary>
        /// Address to web service without internet protocol (http or https)
        /// and binding protocol (SOAP 1.1, SOAP 1.2 or binary).
        /// For example Picture.ArtDatabankenSOA.se/PictureService.svc.
        /// </summary>
        public String WebServiceAddress { get; set; }

        /// <summary>
        /// Clear data cache in web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        public void ClearCache(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.ClearCache(clientInformation);
            }
        }

        /// <summary>
        /// Close a web service client.
        /// </summary>
        /// <param name="client">Web service client.</param>
        protected override void CloseClient(Object client)
        {
            try
            {
                ((ClientBase<IPictureService>)client).Close();
            }
            catch (Exception)
            {
                try
                {
                    ((ClientBase<IPictureService>)client).Abort();
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch (Exception)
                {
                    // We are only interested in releasing resources.
                }
            }
        }

        /// <summary>
        /// Commit a transaction.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        public void CommitTransaction(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.CommitTransaction(clientInformation);
            }
        }

        /// <summary>
        /// Create a web service client.
        /// </summary>
        /// <returns>A web service client.</returns>
        protected override Object CreateClient()
        {
            PictureServiceClient client;

            client = new PictureServiceClient(GetBinding(),
                                              GetEndpointAddress());

            // Increase data size for all methods that  
            // sends or receives a lot of data.
            IncreaseDataSize("CreatePictureFilename", client.Endpoint);
            IncreaseDataSize("GetPictureById", client.Endpoint);
            IncreaseDataSize("GetPictureByPictureFilename", client.Endpoint);
            IncreaseDataSize("GetPictureByPictureStringId", client.Endpoint);
            IncreaseDataSize("GetPictureBySearchCriteria", client.Endpoint);
            IncreaseDataSize("GetPictureInformationById", client.Endpoint);
            IncreaseDataSize("GetPicturesByIds", client.Endpoint);
            IncreaseDataSize("GetPicturesInformationBySearchCriteria", client.Endpoint);
            IncreaseDataSize("UpdatePictures", client.Endpoint);

            return client;
        }

        /// <summary>
        /// Delete trace information from the web service log.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        public void DeleteTrace(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.DeleteTrace(clientInformation);
            }
        }

        /// <summary>
        /// Get entries from the web service log.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="type">Get log entries of this type.</param>
        /// <param name="userName">Get log entries for this user. May be empty.</param>
        /// <param name="rowCount">Maximum number of log entries to get.</param>
        /// <returns> Requested web log entries.</returns>
        public List<WebLogRow> GetLog(WebClientInformation clientInformation,
                                      LogType type,
                                      String userName,
                                      Int32 rowCount)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetLog(clientInformation, type, userName, rowCount);
            }
        }

        /// <summary>
        /// Creates a new picture filename.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="picture">Picture to save to disk.</param>
        /// <param name="filename">Filename of the picture filename.</param>
        /// <param name="lastModified">The date when the picture was last modified.</param>
        /// <param name="hasLastModified">If lastModified has been specified or not.</param>
        /// <param name="versionId">Version number of picture.</param>
        /// <param name="updatedBy">User who last updated the picture.</param>
        /// <param name="metaData">List of metadata to create.</param>
        /// <returns>Information on what picture has been changed and number of affected rows.</returns>
        public WebPictureResponse CreatePictureFilename(WebClientInformation clientInformation,
                                                        String picture,
                                                        String filename,
                                                        DateTime lastModified,
                                                        Boolean hasLastModified,
                                                        Int64 versionId,
                                                        String updatedBy,
                                                        List<WebPictureMetaData> metaData)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.CreatePictureFilename(clientInformation,
                                                           picture,
                                                           filename,
                                                           lastModified,
                                                           hasLastModified,
                                                           versionId,
                                                           updatedBy,
                                                           metaData);
            }
        }

        /// <summary>
        /// Update pictures.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="pictures">List of pictures to update.</param>
        /// <param name="updatedBy">User who last updated the picture.</param>
        /// <returns>Number of updated records.</returns>
        public Int32 UpdatePictures(WebClientInformation clientInformation,
                                    List<WebPicture> pictures,
                                    String updatedBy)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.UpdatePictures(clientInformation,
                                                    pictures,
                                                    updatedBy);
            }
        }

        /// <summary>
        /// Deletes a picture filename.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="pictureId">Id of the picture to delete.</param>
        /// <param name="filename">Filename of the picture filename.</param>
        /// <param name="pictureStringId">PictureStringId for the picture filename.</param>
        /// <returns>Number of deleted rows.</returns>
        public Int32 DeletePictureFilename(WebClientInformation clientInformation,
                                          Int64? pictureId,
                                          String filename,
                                          Int64 pictureStringId)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.DeletePictureFilename(clientInformation,
                                                           pictureId.GetValueOrDefault(),
                                                           pictureId.HasValue,
                                                           filename,
                                                           pictureStringId,
                                                           pictureStringId != -1);
            }
        }

        /// <summary>
        /// Create picture metadata.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="pictureId">Id of the picture whose metadata should be created.</param>
        /// <param name="metaData">List of metadata to create.</param>
        public void CreatePictureMetaData(WebClientInformation clientInformation,
                                          Int64 pictureId,
                                          List<WebPictureMetaData> metaData)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                client.Client.CreatePictureMetaData(clientInformation,
                                                    pictureId,
                                                    metaData);
            }
        }

        /// <summary>
        /// Update picture metadata.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="pictureId">Id of the picture whose metadata should be updated.</param>
        /// <param name="updatedBy">User who last updated the picture.</param>
        /// <param name="metaData">List of metadata to update.</param>
        /// <returns>Number of updated records.</returns>
        public Int32 UpdatePictureMetaData(WebClientInformation clientInformation,
                                           Int64 pictureId,
                                           String updatedBy,
                                           List<WebPictureMetaData> metaData)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.UpdatePictureMetaData(clientInformation,
                                                           pictureId,
                                                           updatedBy,
                                                           metaData);
            }
        }

        /// <summary>
        /// Delete picture metadata.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="pictureId">Id of the picture whose metadata should be deleted.</param>
        /// <param name="metaData">List of metadata to delete.</param>
        /// <returns>Number of deleted records.</returns>
        public Int32 DeletePictureMetaData(WebClientInformation clientInformation,
                                           Int64 pictureId,
                                           List<WebPictureMetaData> metaData)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.DeletePictureMetaData(clientInformation,
                                                           pictureId,
                                                           metaData);
            }
        }

        /// <summary>
        /// Create picture relations.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="pictureRelations">List of picture relation to create.</param>
        public void CreatePictureRelations(WebClientInformation clientInformation,
                                           List<WebPictureRelation> pictureRelations)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                client.Client.CreatePictureRelations(clientInformation,
                                                     pictureRelations);
            }
        }

        /// <summary>
        /// Update picture relations.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="pictureRelations">List of picture relations to update.</param>
        /// <returns>Number of updated records.</returns>
        public Int32 UpdatePictureRelations(WebClientInformation clientInformation,
                                            List<WebPictureRelation> pictureRelations)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.UpdatePictureRelations(clientInformation,
                                                            pictureRelations);
            }
        }

        /// <summary>
        /// Delete picture relations.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="pictureRelationIds">List of ids of picture relations to delete.</param>
        /// <returns>Number of deleted records.</returns>
        public Int32 DeletePictureRelations(WebClientInformation clientInformation,
                                            List<Int64> pictureRelationIds)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.DeletePictureRelations(clientInformation,
                                                            pictureRelationIds);
            }
        }

        /// <summary>
        /// Get picture with specified id.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="pictureId">Picture id.</param>
        /// <param name="height">Specifies a particular height.</param>
        /// <param name="width">Specifies a particular width.</param>
        /// <param name="requestedSize">Requested size of retrieved.</param>
        /// <param name="isRequestedSizeSpecified">Indicates if requested size has been specified.</param>
        /// <param name="requestedFormat">Requested format of returned picture.</param>
        /// <returns>Specified picture.</returns>
        public WebPicture GetPictureById(WebClientInformation clientInformation,
                                         Int64 pictureId,
                                         Int32? height,
                                         Int32? width,
                                         Int64 requestedSize,
                                         Boolean isRequestedSizeSpecified,
                                         String requestedFormat)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetPictureById(clientInformation,
                                                    pictureId,
                                                    height.GetValueOrDefault(),
                                                    height.HasValue,
                                                    width.GetValueOrDefault(),
                                                    width.HasValue,
                                                    requestedSize,
                                                    isRequestedSizeSpecified,
                                                    requestedFormat);
            }
        }

        /// <summary>
        /// Get picture with specified id.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="pictureStringId">Unique id for picture.</param>
        /// <returns>Specified picture.</returns>
        public WebPicture GetPictureByPictureStringId(WebClientInformation clientInformation, Int64 pictureStringId)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetPictureByPictureStringId(clientInformation, pictureStringId);
            }
        }

        /// <summary>
        /// Get picture by picture filename.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="pictureFilename">Picture filename.</param>
        /// <returns>Specified picture.</returns>
        public WebPicture GetPictureByPictureFilename(WebClientInformation clientInformation, String pictureFilename)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetPictureByPictureFilename(clientInformation, pictureFilename);
            }
        }


        /// <summary>
        /// Get information about picture with specified id.
        /// This information contains the picture, meta data
        /// and picture relations.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="pictureId">Picture id.</param>
        /// <param name="height">Specifies a particular height.</param>
        /// <param name="width">Specifies a particular width.</param>
        /// <param name="metaDataIds">Desired metadata to be returned along with the picture.</param>
        /// <returns>Information about picture with specified id.</returns>
        public WebPictureInformation GetPictureInformationById(WebClientInformation clientInformation,
                                                               Int64 pictureId,
                                                               Int32? height,
                                                               Int32? width,
                                                               List<Int32> metaDataIds)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetPictureInformationById(clientInformation,
                                                               pictureId,
                                                               height.GetValueOrDefault(),
                                                               height.HasValue,
                                                               width.GetValueOrDefault(),
                                                               width.HasValue,
                                                               metaDataIds);
            }
        }

        /// <summary>
        /// Gets recommended pictures ids and corresponding GUID from guid and picture relation type.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="objectGuids">List of objectGuids that is to check if recommended picture exist.</param>
        /// <param name="pictureRelationTypeId">Type of picture to look for. 
        /// This must be set in order to get correct picture for correct application.</param>
        /// <returns>A list of picture web picture guid ie guid and corresponding picture id.</returns>
        public List<WebPictureGuid> GetRecommendedPictureIdsByObjectGuid(WebClientInformation clientInformation,
                                                                               List<String> objectGuids,
                                                                               Int32 pictureRelationTypeId)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetRecommendedPictureIdsByObjectGuid(clientInformation,
                                                               objectGuids,
                                                               pictureRelationTypeId);
            }
        }

        /// <summary>
        /// Gets all recommended picutres from picture relation type.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
       /// <param name="pictureRelationTypeId">Type of picture to look for. 
        /// This must be set in order to get correct picture for correct application.</param>
        /// <returns>A list of picture web picture guid ie guid and corresponding picture id.</returns>
        public List<WebPictureGuid> GetAllRecommendedPictureIds(WebClientInformation clientInformation,
                                                                               Int32 pictureRelationTypeId)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetAllRecommendedPictureIds(clientInformation, pictureRelationTypeId);
            }
        }

        /// <summary>
        /// Get information about pictures that match search criteria.
        /// This information contains the picture, metadata
        /// and picture relations.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>        
        /// <param name="searchCriteria">Pictures search criteria.</param>
        /// <param name="height">Specifies a particular height.</param>
        /// <param name="width">Specifies a particular width.</param>
        /// <param name="requestedSize">Requested size of retrieved.</param>        
        /// <param name="requestedFormat">Requested format of returned picture.</param>
        /// <param name="metaDataIds">Desired metadata to be returned along with the picture.</param>
        /// <returns>Information about pictures that matches search criteria.</returns>
        public List<WebPictureInformation> GetPicturesInformationBySearchCriteria(
            WebClientInformation clientInformation,
            WebPicturesSearchCriteria searchCriteria,
            Int32? height,            
            Int32? width,            
            Int64? requestedSize,            
            String requestedFormat,
            List<Int32> metaDataIds)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetPicturesInformationBySearchCriteria(clientInformation,
                                                               searchCriteria,
                                                               height.GetValueOrDefault(),
                                                               height.HasValue,
                                                               width.GetValueOrDefault(),
                                                               width.HasValue,
                                                               requestedSize.GetValueOrDefault(),
                                                               requestedSize.HasValue,
                                                               requestedFormat,
                                                               metaDataIds);
            }
        }

        /// <summary>
        /// Get meta data about picture with specified id.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="pictureId">Picture id.</param>
        /// <param name="metaDataIds">Desired metadata to be returned along with the picture.</param>
        /// <returns>Meta data about picture with specified id.</returns>
        public List<WebPictureMetaData> GetPictureMetaDataById(WebClientInformation clientInformation,
                                                               Int64 pictureId,
                                                               List<Int32> metaDataIds)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetPictureMetaDataById(clientInformation,
                                                            pictureId,
                                                            metaDataIds);
            }
        }

        /// <summary>
        /// Get picture metadata descriptions for a specific culture.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Picture metadata descriptions for specified culture.</returns>
        public List<WebPictureMetaDataDescription> GetPictureMetaDataDescriptions(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetPictureMetaDataDescriptions(clientInformation);
            }
        }

        /// <summary>
        /// Get picture metadata descriptions for specified ids.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="pictureMetaDataDescriptionsIds">Ids of the metadadescriptions to be retrieved.</param>
        /// <returns>Picture metadata descriptions for specified ids.</returns>
        public List<WebPictureMetaDataDescription> GetPictureMetaDataDescriptionsByIds(WebClientInformation clientInformation,
                                                                                       List<Int32> pictureMetaDataDescriptionsIds)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetPictureMetaDataDescriptionsByIds(clientInformation,
                                                                         pictureMetaDataDescriptionsIds);
            }
        }

        /// <summary>
        /// Get metadata about all recommended pictures selected by picture relation type id.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="pictureRelationTypeId">Type of picture to look for. </param>
        /// <param name="metaDataIds">Desired metadata to be returned along with the picture.</param>
        /// <returns>Metadata about all recommended pictures by picture id.</returns>
        public List<WebPictureMetaDataInformation> GetAllRecommendedPicturesMetaData(WebClientInformation clientInformation, Int32 pictureRelationTypeId, List<Int32> metaDataIds)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetAllRecommendedPicturesMetaData(clientInformation, pictureRelationTypeId, metaDataIds);
            }
        }

        /// <summary>
        /// Get all picture relation data types.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All picture relation data types.</returns>
        public List<WebPictureRelationDataType> GetPictureRelationDataTypes(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetPictureRelationDataTypes(clientInformation);
            }
        }

        /// <summary>
        /// Get picture relations related to specified object.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="objectGuid">Guid for an object that may be related to pictures.</param>
        /// <param name="pictureRelationTypeId">Picture relation is of this type.</param>
        /// <returns>Picture relations related to specified object.</returns>
        public List<WebPictureRelation> GetPictureRelationsByObjectGuid(WebClientInformation clientInformation,
                                                                        String objectGuid,
                                                                        Int32 pictureRelationTypeId)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetPictureRelationsByObjectGuid(clientInformation,
                                                                     objectGuid,
                                                                     pictureRelationTypeId);
            }
        }

        /// <summary>
        /// Get picture relations related to specified picture.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="pictureId">Id of specified picture.</param>
        /// <returns>Picture relations related to specified picture.</returns>
        public List<WebPictureRelation> GetPictureRelationsByPictureId(WebClientInformation clientInformation,
                                                                       Int64 pictureId)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetPictureRelationsByPictureId(clientInformation,
                                                                    pictureId);
            }
        }

        /// <summary>
        /// Get all picture relation types.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All picture relation types.</returns>
        public List<WebPictureRelationType> GetPictureRelationTypes(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetPictureRelationTypes(clientInformation);
            }
        }

        /// <summary>
        /// Get pictures with specified ids.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="pictureIds">Picture ids.</param>
        /// <param name="height">Specifies a particular height.</param>
        /// <param name="width">Specifies a particular width.</param>
        /// <param name="requestedSize">Requested size of retrieved.</param>
        /// <param name="isRequestedSizeSpecified">Indicates if requested size has been specified.</param>
        /// <param name="requestedFormat">Requested format of returned picture.</param>
        /// <returns>Specified pictures.</returns>
        public List<WebPicture> GetPicturesByIds(WebClientInformation clientInformation,
                                                 List<Int64> pictureIds,
                                                 Int32? height,
                                                 Int32? width,
                                                 Int64 requestedSize,
                                                 Boolean isRequestedSizeSpecified,
                                                 String requestedFormat)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetPicturesByIds(clientInformation,
                                                      pictureIds,
                                                      height.GetValueOrDefault(),
                                                      height.HasValue,
                                                      width.GetValueOrDefault(),
                                                      width.HasValue,
                                                      requestedSize,
                                                      isRequestedSizeSpecified,
                                                      requestedFormat);
            }
        }

        /// <summary>
        /// Get information about pictures that match search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Picture search criteria.</param>
        /// <returns>Pictures that matches search criteria.</returns>
        public List<WebPicture> GetPictureBySearchCriteria(WebClientInformation clientInformation,
                                                            WebPicturesSearchCriteria searchCriteria)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetPictureBySearchCriteria(clientInformation, searchCriteria);
            }
        }

        /// <summary>
        /// Get status for this web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Status for this web service.</returns>       
        public List<WebResourceStatus> GetStatus(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetStatus(clientInformation);
            }
        }

        /// <summary>
        /// Get address of currently used web service.
        /// </summary>
        /// <returns>Address of currently used web service.</returns>
        protected override String GetWebServiceAddress()
        {
            if (WebServiceAddress.IsEmpty())
            {
                if (Configuration.InstallationType != InstallationType.LocalTest)
                {
                    WebServiceAddress = WebServiceProxy.UserService.GetSoaWebServiceAddress(ApplicationIdentifier.PictureService);
                }

                if (WebServiceAddress.IsEmpty())
                {
                    switch (WebServiceComputer)
                    {
                        case WebServiceComputer.ArtDatabankenSoa:
                            WebServiceAddress = Settings.Default.PictureServiceArtDatabankenSoaAddress;
                            break;
                        case WebServiceComputer.LocalTest:
                            WebServiceAddress = Settings.Default.PictureServiceLocalAddress;
                            break;
                        case WebServiceComputer.Moneses:
                            WebServiceAddress = Settings.Default.PictureServiceMonesesAddress;
                            break;
                        default:
                            throw new Exception("Not handled computer in web service " + GetWebServiceName() + " " + WebServiceComputer);
                    }
                }
            }

            return WebServiceAddress;
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">The password.</param>
        /// <param name="applicationIdentifier">
        /// Application identifier.
        /// User authorities for this application is included in
        /// the user context.
        /// </param>
        /// <param name="isActivationRequired">
        /// Flag that indicates if user must be activated
        /// for login to succeed.
        /// </param>
        /// <returns>Web login response or null if login failed.</returns>
        public WebLoginResponse Login(String userName,
                                      String password,
                                      String applicationIdentifier,
                                      Boolean isActivationRequired)
        {
            WebLoginResponse loginResponse;

            WebServiceProxy.UserService.LoadSoaWebServiceAddresses(userName,
                                                                   password,
                                                                   applicationIdentifier,
                                                                   isActivationRequired);
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                loginResponse = client.Client.Login(userName, password, applicationIdentifier, isActivationRequired);
            }

            return loginResponse;
        }

        /// <summary>
        /// Logout user from web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        public void Logout(WebClientInformation clientInformation)
        {
            try
            {
                using (ClientProxy client = new ClientProxy(this, 1))
                {
                    client.Client.Logout(clientInformation);
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // No need to handle errors.
                // Logout is only used to relase
                // resources in the web service.
            }
        }

        /// <summary>
        /// Check if the web service is up and running.
        /// </summary>
        /// <returns>
        /// True = OK.
        /// False = Some kind of problem.
        /// </returns>       
        public Boolean Ping()
        {
            try
            {
                using (ClientProxy client = new ClientProxy(this, 0, 10))
                {
                    return client.Client.Ping();
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Rollback a transaction.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        public void RollbackTransaction(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.RollbackTransaction(clientInformation);
            }
        }

        /// <summary>
        /// Start trace usage of web service for specified user.
        /// If no user is specified then all usage of web service
        /// is traced.
        /// Note: Tracing has negative impact on web service performance.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="userName">User name.</param>
        public void StartTrace(WebClientInformation clientInformation,
                               String userName)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.StartTrace(clientInformation, userName);
            }
        }

        /// <summary>
        /// Start a transaction.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="timeout">
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </param>
        public void StartTransaction(WebClientInformation clientInformation,
                                     Int32 timeout)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.StartTransaction(clientInformation,
                                               timeout);
            }
        }

        /// <summary>
        /// Stop tracing usage of web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        public void StopTrace(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.StopTrace(clientInformation);
            }
        }

        /// <summary>
        /// Private class that encapsulate handling
        /// of web service connections.
        /// </summary>
        private class ClientProxy : IDisposable
        {
            private readonly Int32 _operationTimeout;
            private PictureServiceClient _client;
            private readonly PictureServiceProxy _webService;

            /// <summary>
            /// Constructor for the ClientProxy class.
            /// Allocates an instance of the real web service client.
            /// </summary>
            /// <param name="webService">Web service proxy.</param>
            /// <param name="operationTimeoutMinutes">Operation timeout to set in the client. Unit is minutes.</param>
            /// <param name="operationTimeoutSeconds">Operation timeout to set in the client. Unit is seconds.</param>
            public ClientProxy(PictureServiceProxy webService,
                               Int32 operationTimeoutMinutes,
                               Int32 operationTimeoutSeconds = 0)
            {
                _operationTimeout = (operationTimeoutMinutes * 60) + operationTimeoutSeconds;
                _webService = webService;
                _client = (PictureServiceClient)(_webService.PopClient(_operationTimeout));
                _webService.SetTimeout(Client.Endpoint.Binding, _operationTimeout);
            }

            /// <summary>
            /// Get the real web service client.
            /// </summary>
            public PictureServiceClient Client
            {
                get { return _client; }
            }

            /// <summary>
            /// Implementation of the IDisposable interface.
            /// Recycle the client instance.
            /// </summary>
            public void Dispose()
            {
                if ((_client.State != CommunicationState.Opened) ||
                    (!_webService.PushClient(_client, _operationTimeout)))
                {
                    // Client is not in state open or
                    // was not added to the client pool.
                    // Release resources.
                    _client.Close();
                }

                _client = null;
            }
        }
    }
}

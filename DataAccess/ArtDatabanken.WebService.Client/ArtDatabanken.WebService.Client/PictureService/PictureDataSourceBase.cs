using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Client.PictureService
{
    using System.Linq;

    /// <summary>
    /// Base class for all picture service data sources.
    /// </summary>
    public class PictureDataSourceBase : DataSourceBase, ITransactionService
    {
        /// <summary>
        /// Information about this data source that is included in all
        /// data that comes from picture service.
        /// </summary>
        private static IDataSourceInformation _dataSourceInformation;

        /// <summary>
        /// Check if a transaction should be created.
        /// </summary>
        /// <param name="userContext">User context.</param>
        protected void CheckTransaction(IUserContext userContext)
        {
            CheckTransaction(userContext, this);
        }

        /// <summary>
        /// Commit a transaction.
        /// </summary>
        /// <param name="userContext">User context.</param>
        public void CommitTransaction(IUserContext userContext)
        {
            WebServiceProxy.PictureService.CommitTransaction(GetClientInformation(userContext));
        }

        /// <summary>
        /// Get information about data source.
        /// </summary>
        /// <returns>Information about data source.</returns>
        public override IDataSourceInformation GetDataSourceInformation()
        {
            if (_dataSourceInformation.IsNull())
            {
                _dataSourceInformation = new DataSourceInformation(WebServiceProxy.PictureService.GetWebServiceName(),
                                                                   WebServiceProxy.PictureService.GetEndpointAddress().ToString(),
                                                                   DataSourceType.WebService);
            }

            return _dataSourceInformation;
        }

        /// <summary>
        /// Convert a IPicturesSearchCriteria instance
        /// to a WebPicturesSearchCriteria instance.
        /// </summary>
        /// <param name="searchCriteria">An IPicturesSearchCriteria object.</param>
        /// <returns>A WebPicturesSearchCriteria object.</returns>
        protected WebPicturesSearchCriteria GetPicturesSearchCriteria(IPicturesSearchCriteria searchCriteria)
        {
            WebPicturesSearchCriteria webSearchCriteria;

            webSearchCriteria = new WebPicturesSearchCriteria();
            if (searchCriteria.IsNotNull())
            {
                // TODO: only a single factor and a single taxon are considered, at present
                if (searchCriteria.Factors.IsNotEmpty() && searchCriteria.Taxa.IsNotEmpty())
                {
                    Int32 taxonId = searchCriteria.Taxa[0].Id;
                    Int32 individualCategoryId = 0;
                    Int32 factorId = searchCriteria.Factors[0].Id;
                    Int32 hostId = 0;
                    Int32 periodId = 0;

                    if (webSearchCriteria.SpeciesFactIdentifiers.IsNull())
                    {
                        webSearchCriteria.SpeciesFactIdentifiers = new List<String>();
                    }

                    webSearchCriteria.SpeciesFactIdentifiers.Add(CoreData.SpeciesFactManager.GetSpeciesFactIdentifier(taxonId, individualCategoryId, factorId, false, hostId, false, periodId));
                }
                else
                {
                    if (searchCriteria.Factors.IsNotEmpty())
                    {
                        webSearchCriteria.FactorIds = searchCriteria.Factors.GetIds();
                    }

                    if (searchCriteria.Taxa.IsNotEmpty())
                    {
                        webSearchCriteria.TaxonIds = searchCriteria.Taxa.GetIds();
                    }
                }

                webSearchCriteria.MetaData = GetPictureMetaData(searchCriteria.MetaData);
            }

            return webSearchCriteria;
        }

        /// <summary>
        /// Convert an IPictureMetaData instance
        /// to a WebPictureMetaData instance.
        /// </summary>
        /// <param name="pictureMetaData">An IPictureMetaData instance.</param>
        /// <returns>A WebPictureMetaData instance.</returns>
        private WebPictureMetaData GetPictureMetaData(IPictureMetaData pictureMetaData)
        {
            WebPictureMetaData webPictureMetaData = new WebPictureMetaData
                                                        {
                                                            PictureMetaDataId = pictureMetaData.Id,
                                                            HasPictureMetaDataId = pictureMetaData.Id > 0,
                                                            Name = pictureMetaData.Name,
                                                            Value = pictureMetaData.Value
                                                        };

            return webPictureMetaData;
        }

        /// <summary>
        /// Convert a list of IPictureMetaData instances
        /// to a list of WebPictureMetaData instances.
        /// </summary>
        /// <param name="pictureMetaDataList">List of IPictureMetaDataList instances.</param>
        /// <returns>A list of WebPictureMetaData instances.</returns>
        private List<WebPictureMetaData> GetPictureMetaData(PictureMetaDataList pictureMetaDataList)
        {
            List<WebPictureMetaData> webPictureMetaDataList;

            webPictureMetaDataList = null;
            if (pictureMetaDataList.IsNotEmpty())
            {
                webPictureMetaDataList = pictureMetaDataList.Select(GetPictureMetaData).ToList();
            }

            return webPictureMetaDataList;
        }

        /// <summary>
        /// Get web service name.
        /// </summary>
        /// <returns>Web service name.</returns>
        protected override String GetWebServiceName()
        {
            return WebServiceProxy.PictureService.GetWebServiceName();
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
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
        protected void Login(IUserContext userContext,
                            String userName,
                            String password,
                            String applicationIdentifier,
                            Boolean isActivationRequired)
        {
            WebLoginResponse loginResponse;

            loginResponse = WebServiceProxy.PictureService.Login(userName,
                                                                 password,
                                                                 applicationIdentifier,
                                                                 isActivationRequired);
            if (loginResponse.IsNotNull())
            {
                SetToken(userContext, loginResponse.Token);
            }
        }

        /// <summary>
        /// Logout user.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        protected void Logout(IUserContext userContext)
        {
            WebServiceProxy.PictureService.Logout(GetClientInformation(userContext));
            SetToken(userContext, null);
        }

        /// <summary>
        /// Rollback a transaction.
        /// </summary>
        /// <param name="userContext">User context.</param>
        public void RollbackTransaction(IUserContext userContext)
        {
            WebServiceProxy.PictureService.RollbackTransaction(GetClientInformation(userContext));
        }

        /// <summary>
        /// Start a transaction.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="timeout">
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </param>
        public void StartTransaction(IUserContext userContext, Int32 timeout)
        {
            WebServiceProxy.PictureService.StartTransaction(GetClientInformation(userContext), timeout);
        }
    }
}

using System;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.UserService;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Client.TaxonAttributeService
{
    /// <summary>
    /// Base class for all taxon attribute service data sources.
    /// </summary>
    public class TaxonAttributeDataSource : DataSourceBase, ITransactionService 
    {
        /// <summary>
        /// Information about this data source that is included in all
        /// data that comes from taxon attribute service.
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
            WebServiceProxy.TaxonAttributeService.CommitTransaction(GetClientInformation(userContext));
        }

        /// <summary>
        /// Get information about data source.
        /// </summary>
        /// <returns>Information about data source.</returns>
        public override IDataSourceInformation GetDataSourceInformation()
        {
            if (_dataSourceInformation.IsNull())
            {
                _dataSourceInformation = new DataSourceInformation(WebServiceProxy.TaxonAttributeService.GetWebServiceName(),
                                                                   WebServiceProxy.TaxonAttributeService.GetEndpointAddress().ToString(),
                                                                   DataSourceType.WebService);
            }

            return _dataSourceInformation;
        }

        /// <summary>
        /// Get web service name.
        /// </summary>
        /// <returns>Web service name.</returns>
        protected override String GetWebServiceName()
        {
            return WebServiceProxy.TaxonAttributeService.GetWebServiceName();
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
        private void Login(IUserContext userContext,
                           String userName,
                           String password,
                           String applicationIdentifier,
                           Boolean isActivationRequired)
        {
            WebLoginResponse loginResponse;

            loginResponse = WebServiceProxy.TaxonAttributeService.Login(userName,
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
        private void Logout(IUserContext userContext)
        {
            WebServiceProxy.TaxonAttributeService.Logout(GetClientInformation(userContext));
            SetToken(userContext, null);
        }

        /// <summary>
        /// Rollback a transaction.
        /// </summary>
        /// <param name="userContext">User context.</param>
        public void RollbackTransaction(IUserContext userContext)
        {
            WebServiceProxy.TaxonAttributeService.RollbackTransaction(GetClientInformation(userContext));
        }

        /// <summary>
        /// Set TaxonAttributeService as data source
        /// in the onion data model.
        /// </summary>
        public static void SetDataSource()
        {
            TaxonAttributeDataSource taxonAttributeDataSource;

            taxonAttributeDataSource = new TaxonAttributeDataSource();
            CoreData.FactorManager.DataSource = new FactorDataSource();
            CoreData.SpeciesFactManager.DataSource = new SpeciesFactDataSource();
            ((UserDataSource)(CoreData.UserManager.DataSource)).UserLoggedInEvent += taxonAttributeDataSource.Login;
            ((UserDataSource)(CoreData.UserManager.DataSource)).UserLoggedOutEvent += taxonAttributeDataSource.Logout;
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
            WebServiceProxy.TaxonAttributeService.StartTransaction(GetClientInformation(userContext), timeout);
        }
    }
}

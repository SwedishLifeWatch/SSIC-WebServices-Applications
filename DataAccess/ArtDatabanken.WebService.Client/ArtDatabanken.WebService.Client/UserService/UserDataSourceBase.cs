using System;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Client.UserService
{
    /// <summary>
    /// Base class for all user service data sources.
    /// </summary>
    public class UserDataSourceBase : DataSourceBase, ITransactionService 
    {
        private static IDataSourceInformation _dataSourceInformation = null;

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
            WebServiceProxy.UserService.CommitTransaction(GetClientInformation(userContext));
        }

        /// <summary>
        /// Get country from web country.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webCountry">Web country.</param>
        /// <returns>Country.</returns>
        protected ICountry GetCountry(IUserContext userContext,
                                      WebCountry webCountry)
        {
            return new Country(webCountry.Id,
                               webCountry.ISOAlpha2Code,
                               webCountry.ISOName,
                               webCountry.Name,
                               webCountry.NativeName,
                               webCountry.PhoneNumberPrefix,
                               GetDataContext(userContext));
        }

        /// <summary>
        /// Get web country from country.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="country">country.</param>
        /// <returns>Web country.</returns>
        protected WebCountry GetCountry(IUserContext userContext,
                                        ICountry country)
        {
            WebCountry webCountry;

            webCountry = new WebCountry();
            webCountry.Id = country.Id;
            webCountry.ISOAlpha2Code = country.ISOCode;
            webCountry.ISOName = country.ISOName;
            webCountry.Name = country.Name;
            webCountry.NativeName = country.NativeName;
            webCountry.PhoneNumberPrefix = country.PhoneNumberPrefix;
            return webCountry;
        }

        /// <summary>
        /// Get information about data source.
        /// </summary>
        /// <returns>Information about data source.</returns>
        public override IDataSourceInformation GetDataSourceInformation()
        {
            if (_dataSourceInformation.IsNull())
            {
                _dataSourceInformation = new DataSourceInformation(WebServiceProxy.UserService.GetWebServiceName(),
                                                                   WebServiceProxy.UserService.GetEndpointAddress().ToString(),
                                                                   DataSourceType.WebService);
            }
            return _dataSourceInformation;
        }

        /// <summary>
        /// Get web service name.
        /// </summary>
        protected override String GetWebServiceName()
        {
            return WebServiceProxy.UserService.GetWebServiceName();
        }

        /// <summary>
        /// Rollback a transaction.
        /// </summary>
        /// <param name="userContext">User context.</param>
        public void RollbackTransaction(IUserContext userContext)
        {
            WebServiceProxy.UserService.RollbackTransaction(GetClientInformation(userContext));
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
            WebServiceProxy.UserService.StartTransaction(GetClientInformation(userContext), timeout);
        }
    }
}

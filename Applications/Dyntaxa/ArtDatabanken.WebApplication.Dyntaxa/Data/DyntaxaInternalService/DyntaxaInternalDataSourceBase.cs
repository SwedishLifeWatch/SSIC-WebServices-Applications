using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.DyntaxaInternalService
{
    /// <summary>
    /// Base class for all taxon service data sources.
    /// </summary>
    public class DyntaxaInternalDataSourceBase : DataSourceBase, ITransactionService
    {
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
            WebServiceProxy.TaxonService.CommitTransaction(GetClientInformation(userContext));
            //WebServiceProxy.DyntaxaInternalService.CommitTransaction(GetClientInformation(userContext));
        }

        /// <summary>
        /// Get information about data source.
        /// </summary>
        /// <returns>Information about data source.</returns>
        public override IDataSourceInformation GetDataSourceInformation()
        {
            if (_dataSourceInformation.IsNull())
            {
                _dataSourceInformation = new DataSourceInformation(
                    WebServiceProxy.TaxonService.GetWebServiceName(),
                    WebServiceProxy.TaxonService.GetEndpointAddress().ToString(),
                    DataSourceType.WebService);
                //_dataSourceInformation = new DataSourceInformation(WebServiceProxy.DyntaxaInternalService.GetWebServiceName(),
                //                                                   WebServiceProxy.DyntaxaInternalService.GetEndpointAddress().ToString(),
                //                                                   DataSourceType.WebService);
            }
            return _dataSourceInformation;
        }

        /// <summary>
        /// Get web service name.
        /// </summary>
        protected override String GetWebServiceName()
        {
            return WebServiceProxy.TaxonService.GetWebServiceName();
            //return WebServiceProxy.DyntaxaInternalService.GetWebServiceName();            
        }

        /// <summary>
        /// Rollback a transaction.
        /// </summary>
        /// <param name="userContext">User context.</param>
        public void RollbackTransaction(IUserContext userContext)
        {
            WebServiceProxy.TaxonService.RollbackTransaction(GetClientInformation(userContext));            
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
            WebServiceProxy.TaxonService.StartTransaction(GetClientInformation(userContext), timeout);
        }
    }
}
using System;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Client.AnalysisService
{
    /// <summary>
    /// Base class for all species observation service data sources.
    /// </summary>
    public class AnalysisDataSourceBase : DataSourceBase
    {
        private static IDataSourceInformation _dataSourceInformation;

        
        /// <summary>
        /// Get information about data source.
        /// </summary>
        /// <returns>Information about data source.</returns>
        public override IDataSourceInformation GetDataSourceInformation()
        {
            if (_dataSourceInformation.IsNull())
            {
               _dataSourceInformation = new DataSourceInformation(WebServiceProxy.AnalysisService.GetWebServiceName(),
                                                                  WebServiceProxy.AnalysisService.GetEndpointAddress().ToString(),
                                                                  DataSourceType.WebService);
            }
            return _dataSourceInformation;
        }


        /// <summary>
        /// Get web service name.
        /// </summary>
        protected override string GetWebServiceName()
        {
            return WebServiceProxy.AnalysisService.GetWebServiceName();
        }

    }
}

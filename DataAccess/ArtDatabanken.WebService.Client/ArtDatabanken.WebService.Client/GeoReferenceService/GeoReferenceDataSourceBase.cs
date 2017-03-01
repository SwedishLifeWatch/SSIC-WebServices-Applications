using System;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Client.GeoReferenceService
{
    /// <summary>
    /// Base class for all geo reference service data sources.
    /// </summary>
    public class GeoReferenceDataSourceBase : DataSourceBase
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
                _dataSourceInformation = new DataSourceInformation(WebServiceProxy.GeoReferenceService.GetWebServiceName(),
                                                                   WebServiceProxy.GeoReferenceService.GetEndpointAddress().ToString(),
                                                                   DataSourceType.WebService);
            }
            return _dataSourceInformation;
        }

        /// <summary>
        /// Get web service name.
        /// </summary>
        protected override String GetWebServiceName()
        {
            return WebServiceProxy.GeoReferenceService.GetWebServiceName();
        }
    }
}

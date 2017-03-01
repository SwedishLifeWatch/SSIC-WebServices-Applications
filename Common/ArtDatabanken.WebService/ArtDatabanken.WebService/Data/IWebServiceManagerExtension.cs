using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension methods to the IWebServiceManager interface.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class IWebServiceManagerExtension
    {
        /// <summary>
        /// Get status for GeoReferenceService.
        /// </summary>
        /// <param name="webServiceManager">Web service manager.</param>
        /// <param name='status'>Status for GeoReferenceService is saved in this object.</param>
        public static void GetGeoReferenceServiceStatus(this IWebServiceManager webServiceManager,
                                                        Dictionary<Int32, List<WebResourceStatus>> status)
        {
            Boolean ping;
            WebResourceStatus resourceStatus;

            ping = WebServiceProxy.GeoReferenceService.Ping();
            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = Settings.Default.ResourceAccessTypeReadSwedish;
            resourceStatus.Address = WebServiceProxy.GeoReferenceService.GetWebAddress();
            resourceStatus.Name = ApplicationIdentifier.GeoReferenceService.ToString();
            resourceStatus.ResourceType = WebServiceBase.GetResourceType(ResourceTypeIdentifier.WebService,
                                                                         (Int32)(LocaleId.sv_SE));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.sv_SE)].Add(resourceStatus);

            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = Settings.Default.ResourceAccessTypeReadEnglish;
            resourceStatus.Address = WebServiceProxy.GeoReferenceService.GetWebAddress();
            resourceStatus.Name = ApplicationIdentifier.GeoReferenceService.ToString();
            resourceStatus.ResourceType = WebServiceBase.GetResourceType(ResourceTypeIdentifier.WebService,
                                                                         (Int32)(LocaleId.en_GB));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.en_GB)].Add(resourceStatus);
        }

        /// <summary>
        /// Get status for TaxonService.
        /// </summary>
        /// <param name="webServiceManager">Web service manager.</param>
        /// <param name='status'>Status for TaxonService is saved in this object.</param>
        public static void GetTaxonServiceStatus(this IWebServiceManager webServiceManager,
                                                 Dictionary<Int32, List<WebResourceStatus>> status)
        {
            Boolean ping;
            WebResourceStatus resourceStatus;

            ping = WebServiceProxy.TaxonService.Ping();
            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = Settings.Default.ResourceAccessTypeReadSwedish;
            resourceStatus.Address = WebServiceProxy.TaxonService.GetWebAddress();
            resourceStatus.Name = ApplicationIdentifier.TaxonService.ToString();
            resourceStatus.ResourceType = WebServiceBase.GetResourceType(ResourceTypeIdentifier.WebService,
                                                                         (Int32)(LocaleId.sv_SE));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.sv_SE)].Add(resourceStatus);

            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = Settings.Default.ResourceAccessTypeReadEnglish;
            resourceStatus.Address = WebServiceProxy.TaxonService.GetWebAddress();
            resourceStatus.Name = ApplicationIdentifier.TaxonService.ToString();
            resourceStatus.ResourceType = WebServiceBase.GetResourceType(ResourceTypeIdentifier.WebService,
                                                                         (Int32)(LocaleId.en_GB));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.en_GB)].Add(resourceStatus);
        }

        /// <summary>
        /// Get status for TaxonAttributeService.
        /// </summary>
        /// <param name="webServiceManager">Web service manager.</param>
        /// <param name='status'>Status for TaxonAttributeService is saved in this object.</param>
        public static void GetTaxonAttributeServiceStatus(this IWebServiceManager webServiceManager,
                                                          Dictionary<Int32, List<WebResourceStatus>> status)
        {
            Boolean ping;
            WebResourceStatus resourceStatus;

            ping = WebServiceProxy.TaxonAttributeService.Ping();
            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = Settings.Default.ResourceAccessTypeReadSwedish;
            resourceStatus.Address = WebServiceProxy.TaxonAttributeService.GetWebAddress();
            resourceStatus.Name = ApplicationIdentifier.TaxonAttributeService.ToString();
            resourceStatus.ResourceType = WebServiceBase.GetResourceType(ResourceTypeIdentifier.WebService,
                                                                         (Int32)(LocaleId.sv_SE));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.sv_SE)].Add(resourceStatus);

            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = Settings.Default.ResourceAccessTypeReadEnglish;
            resourceStatus.Address = WebServiceProxy.TaxonAttributeService.GetWebAddress();
            resourceStatus.Name = ApplicationIdentifier.TaxonAttributeService.ToString();
            resourceStatus.ResourceType = WebServiceBase.GetResourceType(ResourceTypeIdentifier.WebService,
                                                                         (Int32)(LocaleId.en_GB));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.en_GB)].Add(resourceStatus);
        }

        /// <summary>
        /// Get status for UserService.
        /// </summary>
        /// <param name="webServiceManager">Web service manager.</param>
        /// <param name='status'>Status for user service is saved in this object.</param>
        public static void GetUserServiceStatus(this IWebServiceManager webServiceManager,
                                                Dictionary<Int32, List<WebResourceStatus>> status)
        {
            Boolean ping;
            WebResourceStatus resourceStatus;

            ping = WebServiceProxy.UserService.Ping();
            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = Settings.Default.ResourceAccessTypeReadSwedish;
            resourceStatus.Address = WebServiceProxy.UserService.GetWebAddress();
            resourceStatus.Name = ApplicationIdentifier.UserService.ToString();
            resourceStatus.ResourceType = WebServiceBase.GetResourceType(ResourceTypeIdentifier.WebService,
                                                                         (Int32)(LocaleId.sv_SE));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.sv_SE)].Add(resourceStatus);

            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = Settings.Default.ResourceAccessTypeReadEnglish;
            resourceStatus.Address = WebServiceProxy.UserService.GetWebAddress();
            resourceStatus.Name = ApplicationIdentifier.UserService.ToString();
            resourceStatus.ResourceType = WebServiceBase.GetResourceType(ResourceTypeIdentifier.WebService,
                                                                         (Int32)(LocaleId.en_GB));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.en_GB)].Add(resourceStatus);
        }

        /// <summary>
        /// Test if status for UserService is ok.
        /// </summary>
        /// <param name="webServiceManager">Web service manager.</param>
        /// <param name='status'>Status for UserService is contained in this object.</param>
        /// <returns>True if status for UserService is ok.</returns>       
        public static Boolean IsUserServiceStatusOk(this IWebServiceManager webServiceManager,
                                                    Dictionary<Int32, List<WebResourceStatus>> status)
        {
            List<WebResourceStatus> resourceStatuses;

            resourceStatuses = status[(Int32)(LocaleId.en_GB)];
            if (resourceStatuses.IsNotEmpty())
            {
                foreach (WebResourceStatus resourceStatus in resourceStatuses)
                {
                    if (resourceStatus.Name == ApplicationIdentifier.UserService.ToString())
                    {
                        return resourceStatus.Status;
                    }
                }
            }

            return false;
        }
    }
}

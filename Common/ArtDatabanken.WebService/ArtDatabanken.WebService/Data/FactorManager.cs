using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// FactorManager.
    /// </summary>
    public class FactorManager : ManagerBase, IFactorManager
    {
        /// <summary>
        /// GetDefaultIndividualCategory.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public WebIndividualCategory GetDefaultIndividualCategory(WebServiceContext context)
        {
            WebClientInformation clientInformation;

            clientInformation = GetClientInformation(context, WebServiceId.TaxonAttributeService);

            List<WebIndividualCategory> individualCategories = WebServiceProxy.TaxonAttributeService.GetIndividualCategories(clientInformation);

            foreach (var individualCategory in individualCategories)
            {
                if (individualCategory.Id == (Int32)IndividualCategoryId.Default)
                {
                    return individualCategory;
                }
            }

            return null;
        }

        /// <summary>
        /// GetCurrentPublicPeriod.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public WebPeriod GetCurrentPublicPeriod(WebServiceContext context)
        {
            WebClientInformation clientInformation;

            clientInformation = GetClientInformation(context, WebServiceId.TaxonAttributeService);

            List<WebPeriod> periods = WebServiceProxy.TaxonAttributeService.GetPeriods(clientInformation);

            WebPeriod currentPublicPeriod;

            currentPublicPeriod = null;
            foreach (WebPeriod period in periods)
            {
                if (period.StopUpdates < DateTime.Today && period.TypeId == (Int32)PeriodTypeId.SwedishRedList)
                {
                    if (currentPublicPeriod.IsNull() ||
                        // ReSharper disable once PossibleNullReferenceException
                        (currentPublicPeriod.StopUpdates < period.StopUpdates))
                    {
                        currentPublicPeriod = period;
                    }
                }
            }

            return currentPublicPeriod;
        }

        /// <summary>
        /// GetFactorFieldEnums.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<ArtDatabanken.WebService.Data.WebFactorFieldEnum> GetFactorFieldEnums(WebServiceContext context)
        {
            WebClientInformation clientInformation;

            clientInformation = GetClientInformation(context, WebServiceId.TaxonAttributeService);

            return WebServiceProxy.TaxonAttributeService.GetFactorFieldEnums(clientInformation);
        }
    }
}

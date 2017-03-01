using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.Log;
using ArtDatabanken.WebService;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonAttributeService.Data;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Wcf;
using FactorManager = ArtDatabanken.WebService.TaxonAttributeService.Data.FactorManager;
using SpeciesFactManager = ArtDatabanken.WebService.TaxonAttributeService.Data.SpeciesFactManager;

namespace TaxonAttributeService
{
    /// <summary>
    /// Implementation of the taxon attribute web service.
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class TaxonAttributeService : WebServiceBase, ITaxonAttributeService
    {
        /// <summary>
        /// Static constructor.
        /// </summary>
        static TaxonAttributeService()
        {
            WebServiceData.ApplicationManager = new ApplicationManager();
            WebServiceData.AuthorizationManager = new AuthorizationManager();
            WebServiceData.CoordinateConversionManager = new CoordinateConversionManager();
            WebServiceData.DatabaseManager = new DatabaseManager();
            WebServiceData.LogManager = new LogManager();
            WebServiceData.RegionManager = new RegionManager();
            WebServiceData.UserManager = new UserManager();
            WebServiceData.WebServiceManager = new WebServiceManager();
            WebServiceData.SpeciesActivityManager = new SpeciesActivityManager();
            WebServiceData.SpeciesObservationManager = new SpeciesObservationManager();
            WebServiceData.TaxonManager = new TaxonManager();
        }

        /// <summary>
        /// Create species facts.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="createSpeciesFacts">New species facts to create.</param>
        public void CreateSpeciesFacts(WebClientInformation clientInformation,
                                       List<WebSpeciesFact> createSpeciesFacts)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    SpeciesFactManager.CreateSpeciesFacts(context, createSpeciesFacts);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Delete species facts.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="deleteSpeciesFacts">Existing species facts to delete.</param>
        public void DeleteSpeciesFacts(WebClientInformation clientInformation,
                                       List<WebSpeciesFact> deleteSpeciesFacts)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    SpeciesFactManager.DeleteSpeciesFacts(context, deleteSpeciesFacts);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about all factor data types.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Factor data types.</returns>
        public List<WebFactorDataType> GetFactorDataTypes(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return FactorManager.GetFactorDataTypes(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about all factor field enumerations.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Factor field enumerations.</returns>
        public List<WebFactorFieldEnum> GetFactorFieldEnums(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return FactorManager.GetFactorFieldEnums(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about all factor field types.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Factor field types.</returns>
        public List<WebFactorFieldType> GetFactorFieldTypes(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return FactorManager.GetFactorFieldTypes(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all factor origins.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All factor origins.</returns>
        public List<WebFactorOrigin> GetFactorOrigins(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return FactorManager.GetFactorOrigins(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about all factors.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All factors.</returns>
        public List<WebFactor> GetFactors(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return FactorManager.GetFactors(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about factors that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">The factor search criteria.</param>
        /// <returns>Factors that matches the search criteria.</returns>
        public List<WebFactor> GetFactorsBySearchCriteria(WebClientInformation clientInformation,
                                                          WebFactorSearchCriteria searchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return FactorManager.GetFactorsBySearchCriteria(context, searchCriteria);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about all factor trees.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Factor trees.</returns>
        public List<WebFactorTreeNode> GetFactorTrees(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return FactorManager.GetFactorTrees(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about factor trees that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Factor tree information.</param>
        /// <returns>Factor trees.</returns>
        public List<WebFactorTreeNode> GetFactorTreesBySearchCriteria(WebClientInformation clientInformation,
                                                                      WebFactorTreeSearchCriteria searchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return FactorManager.GetFactorTreesBySearchCriteria(context, searchCriteria);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about all factor update modes.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Factor update modes.</returns>
        public List<WebFactorUpdateMode> GetFactorUpdateModes(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return FactorManager.GetFactorUpdateModes(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about all individual categories.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All individual categories.</returns>
        public List<WebIndividualCategory> GetIndividualCategories(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return FactorManager.GetIndividualCategories(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about all periods.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All periods.</returns>
        public List<WebPeriod> GetPeriods(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return FactorManager.GetPeriods(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about all period types.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Period types.</returns>
        public List<WebPeriodType> GetPeriodTypes(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return FactorManager.GetPeriodTypes(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about all species fact qualities.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All species fact qualities.</returns>
        public List<WebSpeciesFactQuality> GetSpeciesFactQualities(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return SpeciesFactManager.GetSpeciesFactQualities(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get species facts with specified identifiers.
        /// Only existing species facts are returned,
        /// e.g. species fact identifiers that does not
        /// match existing species fact does not affect
        /// the returned species facts.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="speciesFactIdentifiers">
        /// Species facts identifiers. E.g. WebSpeciesFacts
        /// instances where id for requested combination of
        /// factor, host, individual category, period and taxon
        /// has been set.
        /// Host id is only used together with taxonomic factors.
        /// Period id is only used together with periodic factors.
        /// </param>
        /// <returns>
        /// Existing species facts among the
        /// requested species facts.
        /// </returns>
        public List<WebSpeciesFact> GetSpeciesFactsByIdentifiers(WebClientInformation clientInformation,
                                                                 List<WebSpeciesFact> speciesFactIdentifiers)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return SpeciesFactManager.GetSpeciesFactsByIdentifiers(context, speciesFactIdentifiers);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about all species facts.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="speciesFactIds">Ids for species facts to get information about.</param>
        /// <returns>Species facts.</returns>
        public List<WebSpeciesFact> GetSpeciesFactsByIds(WebClientInformation clientInformation, List<int> speciesFactIds)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return SpeciesFactManager.GetSpeciesFactsByIds(context, speciesFactIds);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about species facts that matches search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Species facts that matches search criteria.</returns>
        public List<WebSpeciesFact> GetSpeciesFactsBySearchCriteria(WebClientInformation clientInformation,
                                                                    WebSpeciesFactSearchCriteria searchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return SpeciesFactManager.GetSpeciesFactsBySearchCriteria(context,
                                                                              searchCriteria);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get taxa that matches fact search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxa that matches fact search criteria.</returns>
        public List<WebTaxon> GetTaxaBySearchCriteria(WebClientInformation clientInformation, WebSpeciesFactSearchCriteria searchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return SpeciesFactManager.GetTaxaBySearchCriteria(context, searchCriteria);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get taxon count of the taxa that matches fact search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxon count of the taxa that matches fact search criteria.</returns>
        public Int32 GetTaxaCountBySearchCriteria(WebClientInformation clientInformation, WebSpeciesFactSearchCriteria searchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return SpeciesFactManager.GetTaxaCountBySearchCriteria(context, searchCriteria);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get web service context.
        /// This method is used to add Application Insights telemetry data from the request.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Web service context.</returns>
        private WebServiceContext GetWebServiceContext(WebClientInformation clientInformation)
        {
            RequestTelemetry telemetry;
            WebServiceContext context;
            WebUser user;

            try
            {
                context = new WebServiceContext(clientInformation);
                try
                {
                    if (context.IsNotNull() && (Configuration.InstallationType == InstallationType.Production))
                    {
                        telemetry = OperationContext.Current.GetRequestTelemetry();
                        if (telemetry.IsNotNull())
                        {
                            if (context.ClientToken.IsNotNull())
                            {
                                telemetry.Properties[TelemetryProperty.ApplicationIdentifier.ToString()] = context.ClientToken.ApplicationIdentifier;
                                telemetry.Properties[TelemetryProperty.ClientIpAddress.ToString()] = context.ClientToken.ClientIpAddress;
                                telemetry.Properties[TelemetryProperty.LoginDateTime.ToString()] = context.ClientToken.CreatedDate.WebToString();
                            }

                            user = context.GetUser();
                            if (user.IsNotNull())
                            {
                                telemetry.Properties[TelemetryProperty.UserId.ToString()] = user.Id.WebToString();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // Do nothing. We don't want calls to fail because of logging problems.
                }
            }
            catch (ApplicationException)
            {
                LogClientToken(clientInformation);
                throw;
            }
            catch (ArgumentException)
            {
                LogClientToken(clientInformation);
                throw;
            }

            return context;
        }

        /// <summary>
        /// Add information about client to Application Insights.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        private void LogClientToken(WebClientInformation clientInformation)
        {
            RequestTelemetry telemetry;
            WebClientToken clientToken;
            WebUser user;

            try
            {
                if (Configuration.InstallationType == InstallationType.Production)
                {
                    clientToken = new WebClientToken(clientInformation.Token, WebServiceData.WebServiceManager.Key);
                    if (clientToken.IsNotNull())
                    {
                        telemetry = OperationContext.Current.GetRequestTelemetry();
                        if (telemetry.IsNotNull())
                        {
                            telemetry.Properties[TelemetryProperty.ApplicationIdentifier.ToString()] = clientToken.ApplicationIdentifier;
                            telemetry.Properties[TelemetryProperty.ClientIpAddress.ToString()] = clientToken.ClientIpAddress;
                            telemetry.Properties[TelemetryProperty.LoginDateTime.ToString()] = clientToken.CreatedDate.WebToString();

                            user = WebServiceData.UserManager.GetUser(clientToken.UserName);
                            if (user.IsNotNull())
                            {
                                telemetry.Properties[TelemetryProperty.UserId.ToString()] = user.Id.WebToString();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Do nothing. We don't want calls to fail because of logging problems.
            }
        }

        /// <summary>
        /// Update species facts.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="updateSpeciesFacts">Existing species facts to update.</param>
        public void UpdateSpeciesFacts(WebClientInformation clientInformation,
                                       List<WebSpeciesFact> updateSpeciesFacts)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    SpeciesFactManager.UpdateSpeciesFacts(context,
                                                          updateSpeciesFacts);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }
    }
}

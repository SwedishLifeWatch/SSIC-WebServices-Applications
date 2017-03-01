using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.Log;
using ArtDatabanken.WebService;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Wcf;
using ApplicationManager = ArtDatabanken.WebService.Data.ApplicationManager;
using DatabaseManager = ArtDatabanken.WebService.SpeciesObservationHarvestService.Data.DatabaseManager;
using MetadataManager = ArtDatabanken.WebService.Data.MetadataManager;
using RegionManager = ArtDatabanken.WebService.Data.RegionManager;
using SpeciesObservationManager = ArtDatabanken.WebService.Data.SpeciesObservationManager;
using TaxonManager = ArtDatabanken.WebService.Data.TaxonManager;
using UserManager = ArtDatabanken.WebService.Data.UserManager;

namespace SpeciesObservationHarvestService
{
    /// <summary>
    /// Implementation of the web service SpeciesObservationHarvestService.
    /// This web service is used to harvest information about 
    /// species observations made in Sweden and store these
    /// species observations in Swedish Life Watch database.
    /// Data sources for the moment:
    /// Species Gateway (www.artportalen.se).
    /// Artdatabanken internal observation database.
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SpeciesObservationHarvestService : WebServiceBase, ISpeciesObservationHarvestService
    {
        /// <summary>
        /// Static constructor.
        /// </summary>
        static SpeciesObservationHarvestService()
        {
            ////Configuration.InstallationType = InstallationType.Production;
            ////WebServiceProxy.UserService.WebServiceAddress = "User.ArtDatabankenSOA.se/UserService.svc";
            ////WebServiceClient.Endpoint = WebServiceClient.EndpointId.ArtDatabankenSoaFastEndpoint;

            WebServiceData.ApplicationManager = new ApplicationManager();
            WebServiceData.AuthorizationManager = new AuthorizationManager();
            WebServiceData.CoordinateConversionManager = new CoordinateConversionManager();
            WebServiceData.DatabaseManager = new DatabaseManager();
            WebServiceData.FactorManager = new ArtDatabanken.WebService.Data.FactorManager();
            WebServiceData.LogManager = new ArtDatabanken.WebService.SpeciesObservationHarvestService.Data.LogManager();
            WebServiceData.MetadataManager = new MetadataManager();
            WebServiceData.TaxonManager = new TaxonManager();
            WebServiceData.RegionManager = new RegionManager();
            WebServiceData.SpeciesFactManager = new ArtDatabanken.WebService.Data.SpeciesFactManager();
            WebServiceData.SpeciesObservationManager = new SpeciesObservationManager();
            WebServiceData.UserManager = new UserManager();
            WebServiceData.WebServiceManager = new WebServiceManager();

            WebSpeciesObservationServiceData.SpeciesObservationManager = new ArtDatabanken.WebService.SpeciesObservation.Data.SpeciesObservationManager();
            WebSpeciesObservationServiceData.TaxonManager = new ArtDatabanken.WebService.SpeciesObservation.Data.TaxonManager();

            ArtDatabanken.Data.ArtDatabankenService.UserManager.Login(WebServiceData.WebServiceManager.Name,
                                                                      WebServiceData.WebServiceManager.Password,
                                                                      ApplicationIdentifier.ArtDatabankenSOA.ToString(),
                                                                      false);
        }

        /// <summary>
        /// Update database with changes in species observation.
        /// Loop over each connector and read all changes.
        /// Add data to create, update or delete in data tables.
        /// Write Data tables to temp tables in database.
        /// Update ID, TaxonId and convert Points to wgs85 if needed.
        /// Call DB procedures to Copy/Update/Delete from temp tables to production tables.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="changedFrom">Start date for changes.</param>
        /// <param name="changedTo">End date for changes.</param>
        /// <param name="dataProviderIds">Update species observations for these data providers.</param>
        public void UpdateSpeciesObservations(WebClientInformation clientInformation,
                                              DateTime changedFrom,
                                              DateTime changedTo,
                                              List<Int32> dataProviderIds)
        {
            using (var context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    HarvestManager.UpdateSpeciesObservations(context, changedFrom, changedTo, dataProviderIds, false);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Update database with changes in species observation.
        /// Loop over each connector and read all changes. 
        /// Add data to create, update or delete in data tables.
        /// Write Data tables to temp tables in database.
        /// Update ID, TaxonId and convert Points to wgs85 if needed.
        /// Call DB procedures to Copy/Update/Delete from temp tables to production tables.
        /// Used as asynchronous service operation.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="changedFrom">Start date for changes.</param>
        /// <param name="changedTo">End date for changes.</param>
        /// <param name="dataProviderIds">Update species observations for these data providers.</param>
        /// <param name="isChangedDatesSpecified">Notification if start and end dates are specified or not.</param>
        public void StartSpeciesObservationUpdate(
            WebClientInformation clientInformation,
            DateTime changedFrom,
            DateTime changedTo,
            List<int> dataProviderIds,
            bool isChangedDatesSpecified)
        {
            using (var context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    HarvestManager.StartSpeciesObservationUpdate(context, changedFrom, changedTo, dataProviderIds, isChangedDatesSpecified);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Let harvest job thread stop.
        /// Used as asynchronous service operation.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        public void StopSpeciesObservationUpdate(WebClientInformation clientInformation)
        {
            using (var context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    HarvestManager.StopSpeciesObservationUpdate(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Let harvest job thread stop and save current harvest setup.
        /// Used as asynchronous service operation.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        public void PauseSpeciesObservationUpdate(WebClientInformation clientInformation)
        {
            using (var context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    HarvestManager.PauseSpeciesObservationUpdate(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Let harvest job thread continue by current harvest setup.
        /// Used as synchronous service operation.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        public void ContinueSpeciesObservationUpdate(WebClientInformation clientInformation)
        {
            using (var context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    HarvestManager.ContinueSpeciesObservationUpdate(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get progress status about the current observation update process.
        /// Used as synchronous service operation.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>Progress status object.</returns>
        public WebSpeciesObservationHarvestStatus GetSpeciesObservationUpdateStatus(WebClientInformation clientInformation)
        {
            using (var context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return HarvestManager.GetSpeciesObservationUpdateStatus(context);
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
    }
}

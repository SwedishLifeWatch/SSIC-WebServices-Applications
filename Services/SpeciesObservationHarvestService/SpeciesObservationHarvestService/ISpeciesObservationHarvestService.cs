using System;
using System.Collections.Generic;
using System.ServiceModel;
using ArtDatabanken.WebService.Data;
using Microsoft.ApplicationInsights.Wcf;

namespace SpeciesObservationHarvestService
{
    /// <summary>
    /// Interface to the web service SpeciesObservationHarvestService.
    /// This web service is used to harvest information about 
    /// species observations made in Sweden and store these
    /// species observations in Swedish Life Watch database.
    /// </summary>
    [ServiceContract(Namespace = "urn:WebServices.ArtDatabanken.slu.se",
                     SessionMode = SessionMode.NotAllowed)]
    public interface ISpeciesObservationHarvestService
    {
        /// <summary>
        /// Clear data cache in web service.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        [OperationContract]
        void ClearCache(WebClientInformation clientInformation);

        /// <summary>
        /// Delete trace information from the web service log.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        [OperationContract]
        void DeleteTrace(WebClientInformation clientInformation);

        /// <summary>
        /// Get entries from the web service log
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="type">Get log entries of this type.</param>
        /// <param name="userName">Get log entries for this user. May be empty.</param>
        /// <param name="rowCount">Maximum number of log entries to get.</param>
        /// <returns> Requested web log entries.</returns>
        [OperationContract]
        List<WebLogRow> GetLog(WebClientInformation clientInformation,
                               LogType type,
                               String userName,
                               Int32 rowCount);

        /// <summary>
        /// Get status for this web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Status for this web service.</returns>       
        [OperationContract]
        List<WebResourceStatus> GetStatus(WebClientInformation clientInformation);

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">The password.</param>
        /// <param name="applicationIdentifier">
        /// Identifier of the application that the user uses.
        /// </param>
        /// <param name="isActivationRequired">
        /// Flag that indicates that the user account must
        /// be activated before login can succeed.
        /// </param>
        /// <returns>
        /// Token and user roles for the specified application
        /// or null if the login failed.
        /// </returns>       
        [OperationContract]
        WebLoginResponse Login(String userName,
                               String password,
                               String applicationIdentifier,
                               Boolean isActivationRequired);

        /// <summary>
        /// Logout user. Release resources.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        [OperationContract]
        void Logout(WebClientInformation clientInformation);

        /// <summary>
        /// Check if the web service is up and running.
        /// </summary>
        /// <returns>
        /// True = OK.
        /// False = Some kind of problem.
        /// </returns>       
        [OperationContract]
        Boolean Ping();

        /// <summary>
        /// Start trace usage of web service for specified user.
        /// If no user is specified then all usage of web service
        /// is traced.
        /// Note: Tracing has negative impact on web service performance.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="userName">User name.</param>
        [OperationContract]
        void StartTrace(WebClientInformation clientInformation,
                        String userName);

        /// <summary>
        /// Stop tracing usage of web service.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        [OperationContract]
        void StopTrace(WebClientInformation clientInformation);

        /// <summary>
        /// Update database with changes in species observation.
        /// Loop over each connector and read all changes.
        /// Add data to create, update or delete in data tables.
        /// Write Data tables to temp tables in database.
        /// Update ID, TaxonId and convert Points to wgs85 if needed.
        /// Call DB procedures to Copy/Update/Delete from temp tables to production tables.
        /// Used as synchronous service operation.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="changedFrom">Start date for changes.</param>
        /// <param name="changedTo">End date for changes.</param>
        /// <param name="dataProviderIds">Update species observations for these data providers.</param>
        [OperationContract]
        [OperationTelemetry]
        void UpdateSpeciesObservations(WebClientInformation clientInformation,
                                       DateTime changedFrom,
                                       DateTime changedTo,
                                       List<Int32> dataProviderIds);

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
        [OperationContract]
        [OperationTelemetry]
        void StartSpeciesObservationUpdate(WebClientInformation clientInformation,
                                           DateTime changedFrom,
                                           DateTime changedTo,
                                           List<Int32> dataProviderIds,
                                           Boolean isChangedDatesSpecified);

        /// <summary>
        /// Let harvest job thread stop.
        /// Used as asynchronous service operation.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        [OperationContract]
        [OperationTelemetry]
        void StopSpeciesObservationUpdate(WebClientInformation clientInformation);

        /// <summary>
        /// Let harvest job thread stop and save current harvest setup.
        /// Used as asynchronous service operation.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        [OperationContract]
        [OperationTelemetry]
        void PauseSpeciesObservationUpdate(WebClientInformation clientInformation);

        /// <summary>
        /// Let harvest job thread continue by current harvest setup.
        /// Used as synchronous service operation.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        [OperationContract]
        [OperationTelemetry]
        void ContinueSpeciesObservationUpdate(WebClientInformation clientInformation);

        /// <summary>
        /// Get progress status about the current observation update process.
        /// Used as synchronous service operation.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>Progress status object.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebSpeciesObservationHarvestStatus GetSpeciesObservationUpdateStatus(WebClientInformation clientInformation);
    }
}

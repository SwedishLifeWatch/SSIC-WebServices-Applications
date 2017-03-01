using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService
{
    /// <summary>
    /// Base class to all web services in ArtDatabankenSOA.
    /// </summary>
    public class WebServiceBase
    {
        private Boolean _ping;
        private DateTime _lastStatusUpdate;
        private Dictionary<Int32, List<WebResourceStatus>> _statuses;
        private readonly Object _statusesLockObject;
 
        /// <summary>
        /// Constructor.
        /// </summary>
        public WebServiceBase()
        {
            // Force update on next status request.
            _lastStatusUpdate = new DateTime(2000, 1, 1);
            _statusesLockObject = new Object();
            _ping = false;
            _statuses = null;
        }

        /// <summary>
        /// Latest resource statuses.
        /// </summary>
        protected Dictionary<Int32, List<WebResourceStatus>> Statuses
        {
            get
            {
                lock (_statusesLockObject)
                {
                    return _statuses;
                }
            }
            set
            {
                lock (_statusesLockObject)
                {
                    _statuses = value;
                }
            }
        }

        /// <summary>
        /// Clear data cache in web service.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        public virtual void ClearCache(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    context.ClearCache();
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Commit a transaction.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        public virtual void CommitTransaction(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    context.CommitTransaction();
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Delete trace information from the web service log.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        public virtual void DeleteTrace(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    WebServiceData.LogManager.DeleteTrace(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get entries from the web service log
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="type">Get log entries of this type.</param>
        /// <param name="userName">Get log entries for this user. May be empty.</param>
        /// <param name="rowCount">Maximum number of log entries to get.</param>
        /// <returns> Requested web log entries.</returns>
        public virtual List<WebLogRow> GetLog(WebClientInformation clientInformation, LogType type, String userName, Int32 rowCount)
        {
            List<WebLogRow> logEntries;

            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    logEntries = WebServiceData.LogManager.GetLog(context, type, userName, rowCount);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }

            return logEntries;
        }

        /// <summary>
        /// Get specified resource type.
        /// </summary>
        /// <param name="resourceTypeIdentifier">Resource type identifier.</param>
        /// <param name="localeId">Locale id.</param>
        /// <returns>Resource type.</returns>       
        public static WebResourceType GetResourceType(ResourceTypeIdentifier resourceTypeIdentifier,
                                                      Int32 localeId)
        {
            WebResourceType resourceType;

            resourceType = new WebResourceType();
            resourceType.Id = (Int32)(resourceTypeIdentifier);
            resourceType.Identifier = resourceTypeIdentifier.ToString();
            switch (localeId)
            {
                case ((Int32)LocaleId.sv_SE):
                    switch (resourceTypeIdentifier)
                    {
                        case ResourceTypeIdentifier.Database:
                            resourceType.Name = Settings.Default.ResourceTypeDatabaseSwedishName;
                            break;
                        case ResourceTypeIdentifier.WebService:
                            resourceType.Name = Settings.Default.ResourceTypeWebServiceSwedishName;
                            break;
                        default:
                            throw new ArgumentException("Not supported resource type = " + resourceTypeIdentifier);
                    }
                    break;
                default:
                    // English is default and also returned if not
                    // supported language is requested.
                    switch (resourceTypeIdentifier)
                    {
                        case ResourceTypeIdentifier.Database:
                            resourceType.Name = Settings.Default.ResourceTypeDatabaseEnglishName;
                            break;
                        case ResourceTypeIdentifier.WebService:
                            resourceType.Name = Settings.Default.ResourceTypeWebServiceEnglishName;
                            break;
                        default:
                            throw new ArgumentException("Not supported resource type = " + resourceTypeIdentifier);
                    }
                    break;
            }

            return resourceType;
        }

        /// <summary>
        /// Get status for this web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Status for this web service.</returns>       
        public virtual List<WebResourceStatus> GetStatus(WebClientInformation clientInformation)
        {
            Dictionary<Int32, List<WebResourceStatus>> statuses;

            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    UpdateStatus();
                    statuses = Statuses;
                    if (statuses.ContainsKey(context.Locale.Id))
                    {
                        return statuses[context.Locale.Id];
                    }
                    else
                    {
                        return statuses[Settings.Default.DefaultLocaleId];
                    }
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Check if it is time to update status.
        /// </summary>
        /// <returns>True, if it is time to update status.</returns>       
        private Boolean IsTimeToUpdateStatus()
        {
            DateTime compareDateTime;

            if (_ping)
            {
                compareDateTime = DateTime.Now - new TimeSpan(0,
                                                              Settings.Default.OkStatusUpdateIntervalMinutes,
                                                              Settings.Default.OkStatusUpdateIntervalSeconds);
            }
            else
            {
                compareDateTime = DateTime.Now - new TimeSpan(0,
                                                              Settings.Default.ErrorStatusUpdateIntervalMinutes,
                                                              Settings.Default.ErrorStatusUpdateIntervalSeconds);
            }

            return (_lastStatusUpdate < compareDateTime);
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
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
        public virtual WebLoginResponse Login(String userName,
                                              String password,
                                              String applicationIdentifier,
                                              Boolean isActivationRequired)
        {
            using (WebServiceContext context = new WebServiceContext(userName, applicationIdentifier))
            {
                try
                {
                    return WebServiceData.UserManager.Login(context,
                                                            userName,
                                                            password,
                                                            applicationIdentifier,
                                                            isActivationRequired);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Logout user. Release resources.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        public virtual void Logout(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    WebServiceData.UserManager.Logout(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Check if the web service is up and running.
        /// </summary>
        /// <returns>
        /// True = OK.
        /// False = Some kind of problem.
        /// </returns>       
        public virtual Boolean Ping()
        {
            UpdateStatus();
            return _ping;
        }

        /// <summary>
        /// Rollback a transaction.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        public virtual void RollbackTransaction(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    context.RollbackTransaction();
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Start trace usage of web service for specified user.
        /// If no user is specified then all usage of web service
        /// is traced.
        /// Note: Tracing has negativ impact on web service performance.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="userName">User name.</param>
        public virtual void StartTrace(WebClientInformation clientInformation, String userName)
        {
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    context.StartTrace(userName);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Start a transaction.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="timeout">
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </param>
        public virtual void StartTransaction(WebClientInformation clientInformation,
                                             Int32 timeout)
        {
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    context.StartTransaction(timeout);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Stop tracing usage of web service.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        public virtual void StopTrace(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    context.StopTrace();
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Update status for this web service.
        /// </summary>
        private void UpdateStatus()
        {
            Dictionary<Int32, List<WebResourceStatus>> statuses;
            WebResourceStatus resourceStatus;

            if (IsTimeToUpdateStatus())
            {
                lock (this)
                {
                    // This double check of last status update time
                    // is necessary since things may have changed
                    // during possible lock of this thread.
                    if (IsTimeToUpdateStatus())
                    {
                        try
                        {
                            statuses = WebServiceData.WebServiceManager.GetStatus();
                        }
                        catch (Exception exception)
                        {
                            statuses = new Dictionary<Int32, List<WebResourceStatus>>();
                            statuses[(Int32)(LocaleId.en_GB)] = new List<WebResourceStatus>();
                            statuses[(Int32)(LocaleId.sv_SE)] = new List<WebResourceStatus>();

                            resourceStatus = new WebResourceStatus();
                            resourceStatus.AccessType = Settings.Default.ResourceAccessTypeReadSwedish;
                            resourceStatus.Address = null;
                            resourceStatus.Information = Settings.Default.WebServiceCommunicationFailureSwedish + " " +
                                                         Settings.Default.ErrorMessageSwedish + " = " + exception.Message;
                            resourceStatus.Name = WebServiceData.WebServiceManager.Name;
                            resourceStatus.ResourceType = GetResourceType(ResourceTypeIdentifier.WebService,
                                                                          (Int32)(LocaleId.sv_SE));
                            resourceStatus.Status = false;
                            resourceStatus.Time = DateTime.Now;
                            statuses[(Int32)(LocaleId.sv_SE)].Add(resourceStatus);

                            resourceStatus = new WebResourceStatus();
                            resourceStatus.AccessType = Settings.Default.ResourceAccessTypeReadEnglish;
                            resourceStatus.Address = null;
                            resourceStatus.Information = Settings.Default.WebServiceCommunicationFailureEnglish + " " +
                                                         Settings.Default.ErrorMessageEnglish + " = " + exception.Message;
                            resourceStatus.Name = WebServiceData.WebServiceManager.Name;
                            resourceStatus.ResourceType = GetResourceType(ResourceTypeIdentifier.WebService,
                                                                          (Int32)(LocaleId.en_GB));
                            resourceStatus.Status = false;
                            resourceStatus.Time = DateTime.Now;
                            statuses[(Int32)(LocaleId.en_GB)].Add(resourceStatus);
                        }
                        _ping = true;
                        if (statuses.IsNotEmpty())
                        {
                            foreach (WebResourceStatus tempResourceStatus in statuses[(Int32)(LocaleId.en_GB)])
                            {
                                if (!tempResourceStatus.Status)
                                {
                                    _ping = false;
                                    break;
                                }
                            }
                        }
                        _lastStatusUpdate = DateTime.Now;
                        Statuses = statuses;
                    }
                }
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Web.Caching;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Delegate for event handling of changes
    /// in application related information.
    /// </summary>
    public delegate void ApplicationInformationChangeEventHandler();

    /// <summary>
    /// Manager of application information.
    /// </summary>
    public class ApplicationManager
    {
        /// <summary>
        /// Indicates if any application related
        /// information has been changed.
        /// </summary>
        private static Boolean _isUpdated;

        /// <summary>
        /// Event handling of changes
        /// in application related information.
        /// </summary>
        public static event ApplicationInformationChangeEventHandler ApplicationInformationChangeEvent;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static ApplicationManager()
        {
            _isUpdated = false;
            WebServiceContext.CommitTransactionEvent += RemoveCachedObjects;
        }

        /// <summary>
        /// Adds an authority data type to an application.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="authorityDataTypeId">AuthorityDataType Id.</param>
        /// <param name="applicationId">Application Id.</param>
        public static void AddAuthorityDataTypeToApplication(WebServiceContext context,
                                                             Int32 authorityDataTypeId,
                                                             Int32 applicationId)
        {
            // Check access rights.
            AuthorizationManager.CheckSuperAdministrator(context);

            context.CheckTransaction();
            context.GetUserDatabase().AddAuthorityDataTypeToApplication(authorityDataTypeId, applicationId);
            FireApplicationInformationChangeEvent();
        }

        /// <summary>
        /// Creates a new application.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="application">Object representing the Application.</param>
        /// <returns>WebApplication object with the created Application.</returns>
        public static WebApplication CreateApplication(WebServiceContext context, WebApplication application)
        {
            Int32 applicationId;

            // Check access rights.
            AuthorizationManager.CheckSuperAdministrator(context);

            // Check arguments.
            context.CheckTransaction();
            application.CheckNotNull("application");
            application.CheckData();

            Int32? administrationRoleId, contactPersonId;
            administrationRoleId = null;
            contactPersonId = null;
            if (application.IsAdministrationRoleIdSpecified)
            {
                administrationRoleId = application.AdministrationRoleId;
            }

            if (application.IsContactPersonIdSpecified)
            {
                contactPersonId = application.ContactPersonId;
            }

            applicationId = context.GetUserDatabase().CreateApplication(application.Identifier,
                                                                        application.Name,
                                                                        application.ShortName,
                                                                        application.URL,
                                                                        application.Description, 
                                                                        context.Locale.Id,
                                                                        contactPersonId,
                                                                        administrationRoleId,
                                                                        context.GetUser().Id,
                                                                        application.ValidFromDate,
                                                                        application.ValidToDate);
            FireApplicationInformationChangeEvent();
            return GetApplicationById(context, applicationId);
        }

        /// <summary>
        /// Creates a new application action.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="applicationAction">Object representing the ApplicationAction.</param>
        /// <returns>WebApplication object with the created ApplicationAction.</returns>
        public static WebApplicationAction CreateApplicationAction(WebServiceContext context, WebApplicationAction applicationAction)
        {
            // Check access rights.
            AuthorizationManager.CheckSuperAdministrator(context);

            Int32 applicationActionId;

            // Check arguments.
            context.CheckTransaction();
            applicationAction.CheckNotNull("applicationAction");
            applicationAction.CheckData();

            Int32? administrationRoleId;
            administrationRoleId = null;
            if (applicationAction.IsAdministrationRoleIdSpecified)
            {
                administrationRoleId = applicationAction.AdministrationRoleId;
            }

            applicationActionId = context.GetUserDatabase().CreateApplicationAction(applicationAction.ApplicationId,
                                                                                    applicationAction.Name,
                                                                                    applicationAction.Identifier,
                                                                                    applicationAction.Description,
                                                                                    context.Locale.Id,
                                                                                    administrationRoleId,
                                                                                    context.GetUser().Id,
                                                                                    applicationAction.ValidFromDate,
                                                                                    applicationAction.ValidToDate);
            FireApplicationInformationChangeEvent();
            return GetApplicationAction(context, applicationActionId);
        }

        /// <summary>
        /// Create a new application version.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="applicationVersion">Object representing the ApplicationVersion.</param>
        /// <returns>The new application version.</returns>
        public static WebApplicationVersion CreateApplicationVersion(WebServiceContext context, WebApplicationVersion applicationVersion)
        {
            // Check access rights.
            AuthorizationManager.CheckSuperAdministrator(context);

            Int32 applicationVersionId;

            // Check arguments.
            context.CheckTransaction();
            applicationVersion.CheckNotNull("applicationVersion");
            applicationVersion.CheckData();

            applicationVersionId = context.GetUserDatabase().CreateApplicationVersion(applicationVersion.ApplicationId,
                                                                                      applicationVersion.Version,
                                                                                      applicationVersion.IsRecommended,
                                                                                      applicationVersion.IsValid,
                                                                                      applicationVersion.Description,
                                                                                      context.Locale.Id,
                                                                                      context.GetUser().Id,
                                                                                      applicationVersion.ValidFromDate,
                                                                                      applicationVersion.ValidToDate);
            FireApplicationInformationChangeEvent();
            return GetApplicationVersion(context, applicationVersionId);
        }

        /// <summary>
        /// Delete specified application.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="application">Application that should be delete.</param>
        public static void DeleteApplication(WebServiceContext context, WebApplication application)
        {
            // Check access rights.
            AuthorizationManager.CheckSuperAdministrator(context);

            context.CheckTransaction();
            context.GetUserDatabase().DeleteApplication(application.Id, context.GetUser().Id);
            FireApplicationInformationChangeEvent();
        }

        /// <summary>
        /// Fire application information change event.
        /// </summary>
        public static void FireApplicationInformationChangeEvent()
        {
            _isUpdated = true;
            if (ApplicationInformationChangeEvent.IsNotNull())
            {
                ApplicationInformationChangeEvent();
            }
        }

        /// <summary>
        /// Get specified application action.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="applicationActionId">Application action id.</param>
        /// <returns>Specified application action.</returns>
        public static WebApplicationAction GetApplicationAction(WebServiceContext context,
                                                                Int32 applicationActionId)
        {
            Hashtable applicationActions;
            WebApplicationAction applicationAction;

            applicationActions = GetApplicationActions(context);

            if (applicationActions.IsNull())
            {
                // Get data from database.
                using (DataReader dataReader = context.GetUserDatabase().GetApplicationAction(applicationActionId, context.Locale.Id))
                {
                    if (dataReader.Read())
                    {
                        applicationAction = new WebApplicationAction();
                        applicationAction.LoadData(dataReader);
                    }
                    else
                    {
                        throw new ArgumentException("ApplicationAction not found. ApplicationActionId = " + applicationActionId);
                    }
                }
            }
            else
            {
                if (applicationActions.ContainsKey(applicationActionId))
                {
                    applicationAction = (WebApplicationAction)(applicationActions[applicationActionId]);
                }
                else
                {
                    throw new ArgumentException("ApplicationAction not found. ApplicationActionId = " + applicationActionId);
                }
            }

            return applicationAction;
        }

        /// <summary>
        /// Get cache key for one applications actions.
        /// </summary>
        /// <param name="applicationId">Application id.</param>
        /// <returns>Cache key for one applications actions.</returns>
        private static String GetApplicationActionCacheKey(Int32 applicationId)
        {
            return Settings.Default.ApplicationCacheKey + ":" + applicationId;
        }

        /// <summary>
        /// Get all application actions for specified locale.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All application actions for specified locale.</returns>
        private static Hashtable GetApplicationActions(WebServiceContext context)
        {
            ConcurrentDictionary<Int32, Hashtable> allApplicationActions;
            Hashtable applicationActions;
            List<WebApplicationAction> oneApplicationsActions;
            String cacheKey;
            WebApplicationAction applicationAction;

#if OLD_WEB_SERVICE_ADDRESS
            applicationActions = null;
#else
            if (context.IsInTransaction())
            {
                // Do not use cached information.
                // User must get information from database.
                applicationActions = null;
            }
            else
            {
                // Get cached information.
                cacheKey = Settings.Default.ApplicationActionCacheKey;
                allApplicationActions = (ConcurrentDictionary<Int32, Hashtable>)context.GetCachedObject(cacheKey);

                if (allApplicationActions.IsNull())
                {
                    // Add information to ASP.NET cache.
                    allApplicationActions = new ConcurrentDictionary<Int32, Hashtable>();
                    context.AddCachedObject(cacheKey,
                                            allApplicationActions,
                                            DateTime.Now + new TimeSpan(24, 0, 0),
                                            CacheItemPriority.Normal);
                }

                if (allApplicationActions.ContainsKey(context.Locale.Id))
                {
                    applicationActions = allApplicationActions[context.Locale.Id];
                }
                else
                {
                    applicationActions = Hashtable.Synchronized(new Hashtable());
                    allApplicationActions[context.Locale.Id] = applicationActions;

                    // Get information from database.
                    using (DataReader dataReader = context.GetUserDatabase().GetApplicationActions(null, context.Locale.Id))
                    {
                        while (dataReader.Read())
                        {
                            applicationAction = new WebApplicationAction();
                            applicationAction.LoadData(dataReader);

                            // Add information to Hashtable cache.
                            cacheKey = GetApplicationActionCacheKey(applicationAction.ApplicationId);
                            if (applicationActions.ContainsKey(cacheKey))
                            {
                                oneApplicationsActions = (List<WebApplicationAction>)(applicationActions[cacheKey]);
                            }
                            else
                            {
                                oneApplicationsActions = new List<WebApplicationAction>();
                                applicationActions[cacheKey] = oneApplicationsActions;
                            }

                            oneApplicationsActions.Add(applicationAction);
                            applicationActions[applicationAction.Id] = applicationAction;
                        }
                    }
                }
            }
#endif
            return applicationActions;
        }

        /// <summary>
        /// Get application actions for an application.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="applicationId">Application id.</param>
        /// <returns>Application actions for an application.</returns>
        public static List<WebApplicationAction> GetApplicationActionsByApplicationId(WebServiceContext context, Int32 applicationId)
        {
            Hashtable applicationActions;
            List<WebApplicationAction> oneApplicationsActions;
            String cacheKey;
            WebApplicationAction applicationAction;

            oneApplicationsActions = new List<WebApplicationAction>();
            applicationActions = GetApplicationActions(context);

            if (applicationActions.IsNull())
            {
                // Get data from database.
                using (DataReader dataReader = context.GetUserDatabase().GetApplicationActions(applicationId, context.Locale.Id))
                {
                    while (dataReader.Read())
                    {
                        applicationAction = new WebApplicationAction();
                        applicationAction.LoadData(dataReader);
                        oneApplicationsActions.Add(applicationAction);
                    }
                }
            }
            else
            {
                cacheKey = GetApplicationActionCacheKey(applicationId);
                if (applicationActions.ContainsKey(cacheKey))
                {
                    oneApplicationsActions = (List<WebApplicationAction>)(applicationActions[cacheKey]);
                }
            }

            return oneApplicationsActions;
        }

        /// <summary>
        /// Get specified application actions for an application.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="applicationActionGuids">Application action GUID values.</param>
        /// <returns>Specified application actions for an application.</returns>
        public static List<WebApplicationAction> GetApplicationActionsByGuids(WebServiceContext context,
                                                                              List<String> applicationActionGuids)
        {
            List<Int32> applicationActionIds;

            applicationActionIds = new List<Int32>();
            if (applicationActionGuids.IsNotEmpty())
            {
                foreach (String applicationActionGuid in applicationActionGuids)
                {
                    applicationActionIds.Add(Int32.Parse(applicationActionGuid));
                }
            }

            return GetApplicationActionsByIds(context, applicationActionIds);
        }

        /// <summary>
        /// Get specified application actions.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="applicationActionIds">Application action ids.</param>
        /// <returns>Specified application actions.</returns>
        public static List<WebApplicationAction> GetApplicationActionsByIds(WebServiceContext context,
                                                                            List<Int32> applicationActionIds)
        {
            Hashtable applicationActions;
            List<WebApplicationAction> requestedApplicationActions;
            WebApplicationAction applicationAction;

            requestedApplicationActions = new List<WebApplicationAction>();
            applicationActions = GetApplicationActions(context);

            if (applicationActions.IsNull())
            {
                // Get data from database.
                using (DataReader dataReader = context.GetUserDatabase().GetApplicationActionsByIds(applicationActionIds,
                                                                                                    context.Locale.Id))
                {
                    while (dataReader.Read())
                    {
                        applicationAction = new WebApplicationAction();
                        applicationAction.LoadData(dataReader);
                        requestedApplicationActions.Add(applicationAction);
                    }
                }
            }
            else
            {
                if (applicationActionIds.IsNotEmpty())
                {
                    foreach (Int32 applicationActionId in applicationActionIds)
                    {
                        if (applicationActions.ContainsKey(applicationActionId))
                        {
                            requestedApplicationActions.Add((WebApplicationAction)(applicationActions[applicationActionId]));
                        }
                        else
                        {
                            throw new ArgumentException("ApplicationAction not found. ApplicationActionId = " + applicationActionId);
                        }
                    }
                }
            }

            return requestedApplicationActions;
        }

        /// <summary>
        /// Get specified application.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="applicationId">Application id.</param>
        /// <returns>Specified application.</returns>
        public static WebApplication GetApplicationById(WebServiceContext context, Int32 applicationId)
        {
            Hashtable applications;
            WebApplication application;

            application = null;
            applications = GetApplicationsFromCache(context);

            if (applications.IsNull())
            {
                // Get information from database.
                using (DataReader dataReader = context.GetUserDatabase().GetApplicationById(applicationId, context.Locale.Id))
                {
                    if (dataReader.Read())
                    {
                        application = new WebApplication();
                        application.LoadData(dataReader);
                    }
                }
            }
            else
            {
                // Get information from cache.
                if (applications.ContainsKey(applicationId))
                {
                    application = (WebApplication)(applications[applicationId]);
                }
            }

            if (application.IsNull())
            {
                throw new ArgumentException("Application not found. ApplicationId = " + applicationId);
            }

            return application;
        }

        /// <summary>
        /// Get application with specified identifier.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="applicationIdentifier">Application identifier.</param>
        /// <returns>Application with specified identifier.</returns>
        public static WebApplication GetApplicationByIdentifier(WebServiceContext context,
                                                                String applicationIdentifier)
        {
            Hashtable applications;
            WebApplication application;

            // Check arguments.
            applicationIdentifier.CheckNotEmpty("applicationIdentifier");
            applicationIdentifier = applicationIdentifier.CheckInjection();

            application = null;
            applications = GetApplicationsFromCache(context);

            if (applications.IsNull() ||
                !(applications.ContainsKey(applicationIdentifier)))
            {
                // Get information from database.
                using (DataReader dataReader = context.GetUserDatabase().GetApplicationByIdentifier(applicationIdentifier, context.Locale.Id))
                {
                    if (dataReader.Read())
                    {
                        application = new WebApplication();
                        application.LoadData(dataReader);
                    }
                }
            }
            else
            {
                // Get information from cache.
                if (applications.ContainsKey(applicationIdentifier))
                {
                    application = (WebApplication)(applications[applicationIdentifier]);
                }
            }

            return application;
        }

        /// <summary>
        /// Get information about all applications.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Information about all applications.</returns>
        public static List<WebApplication> GetApplications(WebServiceContext context)
        {
            Hashtable applicationsCached;
            List<WebApplication> applications;
            WebApplication application;

            applications = new List<WebApplication>();
            applicationsCached = GetApplicationsFromCache(context);

            if (applicationsCached.IsNull())
            {
                // Get information from database.
                using (DataReader dataReader = context.GetUserDatabase().GetApplications(context.Locale.Id))
                {
                    while (dataReader.Read())
                    {
                        application = new WebApplication();
                        application.LoadData(dataReader);
                        applications.Add(application);
                    }
                }
            }
            else
            {
                // Get information from cache.
                if (applicationsCached.ContainsKey(Settings.Default.ApplicationCacheKey))
                {
                    applications = (List<WebApplication>)(applicationsCached[Settings.Default.ApplicationCacheKey]);
                }
            }

            return applications;
        }

        /// <summary>
        /// Get all application for specified locale.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All application for specified locale.</returns>
        private static Hashtable GetApplicationsFromCache(WebServiceContext context)
        {
            ConcurrentDictionary<Int32, Hashtable> allCachedApplications;
            Hashtable applications;
            List<WebApplication> allApplications;
            String cacheKey;
            WebApplication application;

#if OLD_WEB_SERVICE_ADDRESS
            applications = null;
#else
            if (context.IsInTransaction())
            {
                // Do not use cached information.
                // User must get information from database.
                applications = null;
            }
            else
            {
                // Get cached information.
                cacheKey = Settings.Default.ApplicationCacheKey;
                allCachedApplications = (ConcurrentDictionary<Int32, Hashtable>)context.GetCachedObject(cacheKey);

                if (allCachedApplications.IsNull())
                {
                    // Add information to ASP.NET cache.
                    allCachedApplications = new ConcurrentDictionary<Int32, Hashtable>();
                    context.AddCachedObject(cacheKey,
                                            allCachedApplications,
                                            DateTime.Now + new TimeSpan(24, 0, 0),
                                            CacheItemPriority.Normal);
                }

                if (allCachedApplications.ContainsKey(context.Locale.Id))
                {
                    applications = allCachedApplications[context.Locale.Id];
                }
                else
                {
                    applications = Hashtable.Synchronized(new Hashtable());
                    allApplications = new List<WebApplication>();
                    cacheKey = Settings.Default.ApplicationCacheKey;
                    applications[cacheKey] = allApplications;

                    // Get information from database.
                    using (DataReader dataReader = context.GetUserDatabase().GetApplications(context.Locale.Id))
                    {
                        while (dataReader.Read())
                        {
                            application = new WebApplication();
                            application.LoadData(dataReader);
                            allApplications.Add(application);
                            applications[application.Id] = application;
                            applications[application.Identifier] = application;
                        }
                    }

                    allCachedApplications[context.Locale.Id] = applications;
                }
            }
#endif
            return applications;
        }

        /// <summary>
        /// Get information about all web services that constitute the SOA.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>information about all applications web services that constitute the SOA.</returns>
        public static List<WebApplication> GetApplicationsInSoa(WebServiceContext context)
        {
            ConcurrentDictionary<Int32, List<WebApplication>> webServicesCached;
            Int32 localeId;
            List<WebApplication> applications, webServices;
            String cacheKey;

            // Get cached information.
            webServices = null;
            if (context.Locale.IsNull())
            {
                localeId = context.GetDefaultLocale().Id;
            }
            else
            {
                localeId = context.Locale.Id;
            }

            cacheKey = Settings.Default.ApplicationsInSoaCacheKey;
            webServicesCached = (ConcurrentDictionary<Int32, List<WebApplication>>)context.GetCachedObject(cacheKey);
            if (webServicesCached.IsNull())
            {
                // Add information to cache.
                webServicesCached = new ConcurrentDictionary<Int32, List<WebApplication>>();
                context.AddCachedObject(cacheKey, webServicesCached, DateTime.Now + new TimeSpan(12, 0, 0), CacheItemPriority.AboveNormal);
            }
            else
            {
                if (webServicesCached.ContainsKey(localeId))
                {
                    webServices = webServicesCached[localeId];
                }
            }

            if (webServices.IsNull())
            {
                // Get all applications.
                applications = GetApplications(context);
                webServices = new List<WebApplication>();
                foreach (WebApplication application in applications)
                {
                    if ((application.Identifier == ApplicationIdentifier.AnalysisService.ToString()) ||
                        (application.Identifier == ApplicationIdentifier.ArtDatabankenService.ToString()) ||
                        (application.Identifier == ApplicationIdentifier.GeoReferenceService.ToString()) ||
                        (application.Identifier == ApplicationIdentifier.PictureService.ToString()) ||
                        (application.Identifier == ApplicationIdentifier.ReferenceService.ToString()) ||
                        (application.Identifier == ApplicationIdentifier.SpeciesObservationHarvestService.ToString()) ||
                        (application.Identifier == ApplicationIdentifier.SwedishSpeciesObservationService.ToString()) ||
                        (application.Identifier == ApplicationIdentifier.SwedishSpeciesObservationSOAPService.ToString()) ||
                        (application.Identifier == ApplicationIdentifier.TaxonService.ToString()) ||
                        (application.Identifier == ApplicationIdentifier.TaxonAttributeService.ToString()) ||
                        (application.Identifier == ApplicationIdentifier.UserService.ToString()))
                    {
                        webServices.Add(application);
                    }
                }

                // Add information to cache.
                webServicesCached[localeId] = webServices;
            }

            return webServices;
        }

        /// <summary>
        /// Get list of ApplicationVersions for an Application
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="applicationVersionId">Application version id.</param>
        /// <returns>Returns applicationversion or null if 
        ///          applicationversionid doesn't match.</returns>
        public static WebApplicationVersion GetApplicationVersion(WebServiceContext context, Int32 applicationVersionId)
        {
            Hashtable applicationVersions;
            WebApplicationVersion applicationVersion;

            applicationVersions = GetApplicationVersions(context);

            if (applicationVersions.IsNull())
            {
                // Get data from database.
                using (DataReader dataReader = context.GetUserDatabase().GetApplicationVersion(applicationVersionId, context.Locale.Id))
                {
                    if (dataReader.Read())
                    {
                        applicationVersion = new WebApplicationVersion();
                        applicationVersion.LoadData(dataReader);
                    }
                    else
                    {
                        throw new ArgumentException("ApplicationVersion not found. ApplicationVersionId = " + applicationVersionId);
                    }
                }
            }
            else
            {
                if (applicationVersions.ContainsKey(applicationVersionId))
                {
                    applicationVersion = (WebApplicationVersion)(applicationVersions[applicationVersionId]);
                }
                else
                {
                    throw new ArgumentException("ApplicationVersion not found. ApplicationVersionId = " + applicationVersionId);
                }
            }

            return applicationVersion;
        }

        /// <summary>
        /// Get cache key for one applications versions.
        /// </summary>
        /// <param name="applicationId">Application id.</param>
        /// <returns>Cache key for one applications versions.</returns>
        private static String GetApplicationVersionCacheKey(Int32 applicationId)
        {
            return Settings.Default.ApplicationCacheKey + ":" + applicationId;
        }

        /// <summary>
        /// Get all application versions for specified locale.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All application versions for specified locale.</returns>
        private static Hashtable GetApplicationVersions(WebServiceContext context)
        {
            ConcurrentDictionary<Int32, Hashtable> allApplicationVersions;
            Hashtable applicationVersions;
            List<WebApplicationVersion> oneApplicationsVersions;
            String cacheKey;
            WebApplicationVersion applicationVersion;

#if OLD_WEB_SERVICE_ADDRESS
            applicationVersions = null;
#else
            if (context.IsInTransaction())
            {
                // Do not use cached information.
                // User must get information from database.
                applicationVersions = null;
            }
            else
            {
                // Get cached information.
                cacheKey = Settings.Default.ApplicationVersionCacheKey;
                allApplicationVersions = (ConcurrentDictionary<Int32, Hashtable>)context.GetCachedObject(cacheKey);

                if (allApplicationVersions.IsNull())
                {
                    // Add information to ASP.NET cache.
                    allApplicationVersions = new ConcurrentDictionary<Int32, Hashtable>();
                    context.AddCachedObject(cacheKey,
                                            allApplicationVersions,
                                            DateTime.Now + new TimeSpan(24, 0, 0),
                                            CacheItemPriority.Normal);
                }

                if (allApplicationVersions.ContainsKey(context.Locale.Id))
                {
                    applicationVersions = allApplicationVersions[context.Locale.Id];
                }
                else
                {
                    applicationVersions = Hashtable.Synchronized(new Hashtable());
                    allApplicationVersions[context.Locale.Id] = applicationVersions;

                    // Get information from database.
                    using (DataReader dataReader = context.GetUserDatabase().GetApplicationVersionList(null, context.Locale.Id))
                    {
                        while (dataReader.Read())
                        {
                            applicationVersion = new WebApplicationVersion();
                            applicationVersion.LoadData(dataReader);

                            // Add information to Hashtable cache.
                            cacheKey = GetApplicationVersionCacheKey(applicationVersion.ApplicationId);
                            if (applicationVersions.ContainsKey(cacheKey))
                            {
                                oneApplicationsVersions = (List<WebApplicationVersion>)(applicationVersions[cacheKey]);
                            }
                            else
                            {
                                oneApplicationsVersions = new List<WebApplicationVersion>();
                                applicationVersions[cacheKey] = oneApplicationsVersions;
                            }
                            oneApplicationsVersions.Add(applicationVersion);
                            applicationVersions[applicationVersion.Id] = applicationVersion;
                        }
                    }
                }
            }
#endif
            return applicationVersions;
        }

        /// <summary>
        /// Get list of ApplicationVersions for an Application
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="applicationId">Application id.</param>
        /// <returns>Returns list of applicationversions or null if 
        ///          applicationid doesn't match.</returns>
        public static List<WebApplicationVersion> GetApplicationVersionsByApplicationId(WebServiceContext context,
                                                                                        Int32 applicationId)
        {
            Hashtable applicationVersions;
            List<WebApplicationVersion> oneApplicationsVersions;
            String cacheKey;
            WebApplicationVersion applicationVersion;

            oneApplicationsVersions = new List<WebApplicationVersion>();
            applicationVersions = GetApplicationVersions(context);

            if (applicationVersions.IsNull())
            {
                // Get data from database.
                using (DataReader dataReader = context.GetUserDatabase().GetApplicationVersionList(applicationId, context.Locale.Id))
                {
                    while (dataReader.Read())
                    {
                        applicationVersion = new WebApplicationVersion();
                        applicationVersion.LoadData(dataReader);
                        oneApplicationsVersions.Add(applicationVersion);
                    }
                }
            }
            else
            {
                cacheKey = GetApplicationVersionCacheKey(applicationId);
                if (applicationVersions.ContainsKey(cacheKey))
                {
                    oneApplicationsVersions = (List<WebApplicationVersion>)(applicationVersions[cacheKey]);
                }
            }

            return oneApplicationsVersions;
        }

        /// <summary>
        /// Get list of all Authorities Types for specific application id.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="applictionId"></param>
        /// <returns>A list of Web Authority Data Type.</returns>
        public static List<WebAuthorityDataType> GetAuthorityDataTypesByApplicationId(WebServiceContext context,
                                                                                      Int32 applictionId)
        {
            ConcurrentDictionary<Int32, List<WebAuthorityDataType>> applicationAuthorityDataTypes;
            Int32 tempApplicationId;
            List<WebAuthorityDataType> authorityDataTypes;
            String cacheKey;
            WebAuthorityDataType authorityDataType;

            authorityDataTypes = null;

#if !OLD_WEB_SERVICE_ADDRESS
            if (!context.IsInTransaction())
            {
                // Get cached information.
                cacheKey = Settings.Default.ApplicationAuthorityDataTypeCacheKey;
                applicationAuthorityDataTypes = (ConcurrentDictionary<Int32, List<WebAuthorityDataType>>)context.GetCachedObject(cacheKey);

                if (applicationAuthorityDataTypes.IsNull())
                {
                    applicationAuthorityDataTypes = new ConcurrentDictionary<Int32, List<WebAuthorityDataType>>();

                    // Get data from database.
                    using (DataReader dataReader = context.GetUserDatabase().GetAuthorityDataTypesByApplicationId(null))
                    {
                        while (dataReader.Read())
                        {
                            authorityDataType = new WebAuthorityDataType();
                            authorityDataType.LoadData(dataReader);
                            tempApplicationId = dataReader.GetInt32(AuthorityData.APPLICATION_ID);
                            if (applicationAuthorityDataTypes.ContainsKey(tempApplicationId))
                            {
                                authorityDataTypes = applicationAuthorityDataTypes[tempApplicationId];
                            }
                            else
                            {
                                authorityDataTypes = new List<WebAuthorityDataType>();
                                applicationAuthorityDataTypes[tempApplicationId] = authorityDataTypes;
                            }
                            authorityDataTypes.Add(authorityDataType);
                        }
                    }

                    // Add information to ASP.NET cache.
                    context.AddCachedObject(cacheKey,
                                            applicationAuthorityDataTypes,
                                            DateTime.Now + new TimeSpan(24, 0, 0),
                                            CacheItemPriority.Normal);
                }

                if (applicationAuthorityDataTypes.ContainsKey(applictionId))
                {
                    authorityDataTypes = applicationAuthorityDataTypes[applictionId];
                }
                else
                {
                    // Application does not have any authority
                    // data types related to it.
                    authorityDataTypes = new List<WebAuthorityDataType>();
                }
            }
#endif
            if (authorityDataTypes.IsNull())
            {
                // Get data from database.
                using (DataReader dataReader = context.GetUserDatabase().GetAuthorityDataTypesByApplicationId(applictionId))
                {
                    authorityDataTypes = new List<WebAuthorityDataType>();
                    while (dataReader.Read())
                    {
                        authorityDataType = new WebAuthorityDataType();
                        authorityDataType.LoadData(dataReader);
                        authorityDataTypes.Add(authorityDataType);
                    }
                }
            }

            return authorityDataTypes;
        }

        /// <summary>
        /// Check if specific applicationversion is valid.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="applicationIdentifier">Application Identifier.</param>
        /// <param name="version">Version to check if vaild.</param>
        /// <returns>Returns applicationversion object with valid version if version is valid
        ///          applicationversion object with IsValid-flag set to false if version is invalid.
        /// </returns>
        public static WebApplicationVersion IsApplicationVersionValid(WebServiceContext context, String applicationIdentifier, String version)
        {
            DateTime today;
            List<WebApplicationVersion> applicationVersions;
            WebApplication application;
            WebApplicationVersion applicationVersion;

            // Check arguments.
            applicationIdentifier.CheckNotEmpty("applicationIdentifier");
            applicationIdentifier = applicationIdentifier.CheckInjection();
            version.CheckNotEmpty("version");
            version = version.CheckInjection();

            applicationVersion = null;
            if (context.IsInTransaction())
            {
                // Get data from database.
                using (DataReader dataReader = context.GetUserDatabase().IsApplicationVersionValid(applicationIdentifier, version, context.Locale.Id))
                {
                    if (dataReader.Read())
                    {
                        // Valid application version.
                        applicationVersion = new WebApplicationVersion();
                        applicationVersion.LoadData(dataReader);
                    }
                }
            }
            else
            {
                application = GetApplicationByIdentifier(context,
                                                         applicationIdentifier);

                if (application.IsNotNull())
                {
                    applicationVersions = GetApplicationVersionsByApplicationId(context,
                                                                                application.Id);
                    today = DateTime.Now;
                    foreach (WebApplicationVersion webApplicationVersion in applicationVersions)
                    {
                        if (webApplicationVersion.IsValid &&
                            (webApplicationVersion.Version == version) &&
                            (webApplicationVersion.ValidFromDate < today) &&
                            (webApplicationVersion.ValidToDate > today))
                        {
                            // Valid application version.
                            applicationVersion = webApplicationVersion;
                        }
                    }
                }
            }

            if (applicationVersion.IsNull())
            {
                // Not a valid application version.
                applicationVersion = new WebApplicationVersion();
                applicationVersion.Version = version;
                applicationVersion.IsValid = false;
                applicationVersion.IsRecommended = false;
                applicationVersion.Id = 0;
                applicationVersion.Description = String.Empty;
                applicationVersion.ValidFromDate = DateTime.MinValue;
                applicationVersion.ValidToDate = DateTime.MinValue;
            }

            return applicationVersion;
        }

        /// <summary>
        /// Adds an authority data type to an application
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="authorityDataTypeId">AuthorityDataType Id.</param>
        /// <param name="applicationId">Application Id.</param>
        /// <returns>void</returns>
        public static void RemoveAuthorityDataTypeFromApplication(WebServiceContext context,
                                                                  Int32 authorityDataTypeId,
                                                                  Int32 applicationId)
        {
            // Check access rights.
            AuthorizationManager.CheckSuperAdministrator(context);

            context.CheckTransaction();
            context.GetUserDatabase().RemoveAuthorityDataTypeFromApplication(authorityDataTypeId, applicationId);
            FireApplicationInformationChangeEvent();
        }

        /// <summary>
        /// Remove cached application information objects from cache.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        private static void RemoveCachedObjects(WebServiceContext context)
        {
            String cacheKey;

            if (_isUpdated)
            {
                _isUpdated = false;
                cacheKey = Settings.Default.ApplicationVersionCacheKey;
                context.RemoveCachedObject(cacheKey);
                cacheKey = Settings.Default.ApplicationAuthorityDataTypeCacheKey;
                context.RemoveCachedObject(cacheKey);
                cacheKey = Settings.Default.ApplicationActionCacheKey;
                context.RemoveCachedObject(cacheKey);
                cacheKey = Settings.Default.ApplicationCacheKey;
                context.RemoveCachedObject(cacheKey);
                cacheKey = Settings.Default.ApplicationsInSoaCacheKey;
                context.RemoveCachedObject(cacheKey);
            }
        }

        /// <summary>
        /// Updates an application
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="application">Object representing the Application.</param>
        /// <returns>WebApplication object with the updated application.</returns>
        public static WebApplication UpdateApplication(WebServiceContext context, WebApplication application)
        {
            // Check access rights.
            AuthorizationManager.CheckSuperAdministrator(context);

            Int32? administrationRoleId, contactPersonId;
            administrationRoleId = null;
            contactPersonId = null;
            
            // Check arguments.
            context.CheckTransaction();
            application.CheckData();

            if (application.IsAdministrationRoleIdSpecified)
            {
                administrationRoleId = application.AdministrationRoleId;
            }

            if (application.IsContactPersonIdSpecified)
            {
                contactPersonId = application.ContactPersonId;
            }

            context.GetUserDatabase().UpdateApplication(
                    application.Id, application.Identifier, application.Name, 
                    application.ShortName, application.URL, application.Description, context.Locale.Id,
                    contactPersonId, administrationRoleId,
                    context.GetUser().Id, application.ValidFromDate, application.ValidToDate);
            FireApplicationInformationChangeEvent();
            return GetApplicationById(context, application.Id);
        }

        /// <summary>
        /// Updates an applicationaction
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="applicationAction">Object representing the ApplicationAction.</param>
        /// <returns>WebApplicationAction object with the updated applicationaction.</returns>
        public static WebApplicationAction UpdateApplicationAction(WebServiceContext context, WebApplicationAction applicationAction)
        {
            // Check access rights.
            AuthorizationManager.CheckSuperAdministrator(context);

            Int32? administrationRoleId;
            administrationRoleId = null;

            // Check arguments.
            context.CheckTransaction();
            applicationAction.CheckData();

            if (applicationAction.IsAdministrationRoleIdSpecified)
            {
                administrationRoleId = applicationAction.AdministrationRoleId;
            }

            context.GetUserDatabase().UpdateApplicationAction(
                applicationAction.Id, applicationAction.Identifier, applicationAction.Name,
                administrationRoleId, applicationAction.Description,
                context.Locale.Id, context.GetUser().Id, 
                applicationAction.ValidFromDate, applicationAction.ValidToDate);
            FireApplicationInformationChangeEvent();
            return GetApplicationAction(context, applicationAction.Id);
        }

        /// <summary>
        /// Updates an applicationversion
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="applicationVersion">Object representing the ApplicationVersion.</param>
        /// <returns>WebApplicationVersion object with the updated applicationversion.</returns>
        public static WebApplicationVersion UpdateApplicationVersion(WebServiceContext context,
                                                                     WebApplicationVersion applicationVersion)
        {
            // Check access rights.
            AuthorizationManager.CheckSuperAdministrator(context);

            // Check arguments
            context.CheckTransaction();
            applicationVersion.CheckData();

            context.GetUserDatabase().UpdateApplicationVersion(
                applicationVersion.Id, applicationVersion.Version, applicationVersion.IsRecommended,
                applicationVersion.IsValid, applicationVersion.Description,
                context.Locale.Id, context.GetUser().Id, applicationVersion.ValidFromDate, applicationVersion.ValidToDate);
            FireApplicationInformationChangeEvent();
            return GetApplicationVersion(context, applicationVersion.Id);
        }
    }
}

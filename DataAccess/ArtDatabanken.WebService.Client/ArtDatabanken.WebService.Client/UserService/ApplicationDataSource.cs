using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Client.UserService
{
    /// <summary>
    /// This class is used to retrieve or update
    /// application related information.
    /// </summary>
    public class ApplicationDataSource : UserDataSourceBase, IApplicationDataSource 
    {

        /// <summary>
        /// Adds an authority data type to an application
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="authorityDataTypeId">AuthorityDataType Id.</param>
        /// <param name="applicationId">Application Id.</param>
        /// <returns>void</returns>
        public void AddAuthorityDataTypeToApplication(IUserContext userContext,
                                                      Int32 authorityDataTypeId,
                                                      Int32 applicationId)
        {
            CheckTransaction(userContext);
            WebServiceProxy.UserService.AddAuthorityDataTypeToApplication(GetClientInformation(userContext),
                                                                          authorityDataTypeId,
                                                                          applicationId);
        }

        /// <summary>
        /// Create new application.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="application">
        /// Information about the new application.
        /// This object is updated with information 
        /// about the created application.
        /// </param>
        public void CreateApplication(IUserContext userContext,
                                      IApplication application)
        {
            WebApplication webApplication;

            CheckTransaction(userContext);
            webApplication = WebServiceProxy.UserService.CreateApplication(GetClientInformation(userContext),
                                                                  GetApplication(userContext, application));
            UpdateApplication(userContext, application, webApplication);
        }

        /// <summary>
        /// Create new application action.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationAction">ApplicationAction
        /// Information about the new application Action.
        /// This object is updated with information 
        /// about the created application action.
        /// </param>
        public void CreateApplicationAction(IUserContext userContext,
                                            IApplicationAction applicationAction)
        {
            WebApplicationAction webApplicationAction;

            CheckTransaction(userContext);
            webApplicationAction = WebServiceProxy.UserService.CreateApplicationAction(GetClientInformation(userContext),
                                                                              GetApplicationAction(userContext, applicationAction));
            UpdateApplicationAction(userContext, applicationAction, webApplicationAction);
        }

        /// <summary>
        /// Create new application version.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationVersion">
        /// Information about the new application version.
        /// This object is updated with information 
        /// about the created application version.
        /// </param>
        public void CreateApplicationVersion(IUserContext userContext,
                                             IApplicationVersion applicationVersion)
        {
            WebApplicationVersion webApplicationVersion;

            CheckTransaction(userContext);
            webApplicationVersion = WebServiceProxy.UserService.CreateApplicationVersion(GetClientInformation(userContext),
                                                                                GetApplicationVersion(userContext, applicationVersion));
            UpdateApplicationVersion(userContext, applicationVersion, webApplicationVersion);
        }

        /// <summary>
        /// Delete an application.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="application">Delete this application.</param>
        public void DeleteApplication(IUserContext userContext,
                                      IApplication application)
        {
            CheckTransaction(userContext);
            WebServiceProxy.UserService.DeleteApplication(GetClientInformation(userContext),
                                                 GetApplication(userContext, application));
        }

        /// <summary>
        /// Get application by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationId">Application id.</param>
        /// <returns>Requested application.</returns>       
        public IApplication GetApplication(IUserContext userContext,
                                           Int32 applicationId)
        {
            WebApplication webApplication;

            CheckTransaction(userContext);
            webApplication = WebServiceProxy.UserService.GetApplication(GetClientInformation(userContext), applicationId);
            return GetApplication(userContext, webApplication);
        }

        /// <summary>
        /// Get application from web application.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webApplication">A web application.</param>
        /// <returns>Requested application.</returns>
        public IApplication GetApplication(IUserContext userContext,
                                           WebApplication webApplication)
        {
            IApplication application;

            application = new Application(userContext);
            UpdateApplication(userContext, application, webApplication);
            return application;
        }

        /// <summary>
        /// Get WebApplication from Application.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="application">Application.</param>
        /// <returns>WebApplication.</returns>
        public WebApplication GetApplication(IUserContext userContext,
                                             IApplication application)
        {
            WebApplication webApplication;
            webApplication = new WebApplication();

            if (application.AdministrationRoleId.HasValue)
            {
                webApplication.AdministrationRoleId = application.AdministrationRoleId.Value;
            }
            webApplication.Identifier = application.Identifier;
            if (application.ContactPersonId.HasValue)
            {
                webApplication.ContactPersonId = application.ContactPersonId.Value;
            }
            webApplication.CreatedBy = application.UpdateInformation.CreatedBy;
            webApplication.CreatedDate = application.UpdateInformation.CreatedDate;
            webApplication.Description = application.Description;
            webApplication.GUID = application.GUID;
            webApplication.Id = application.Id;
            webApplication.IsAdministrationRoleIdSpecified = application.AdministrationRoleId.HasValue;
            webApplication.IsContactPersonIdSpecified = application.ContactPersonId.HasValue;
            webApplication.ModifiedBy = application.UpdateInformation.ModifiedBy;
            webApplication.ModifiedDate = application.UpdateInformation.ModifiedDate;
            webApplication.Name = application.Name;
            webApplication.ShortName = application.ShortName;
            webApplication.URL = application.URL;
            webApplication.ValidFromDate = application.ValidFromDate;
            webApplication.ValidToDate = application.ValidToDate;

            return webApplication;
        }

        /// <summary>
        /// GetApplications
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>
        /// Returns list of applications or null if no applications are found.
        /// </returns>
        public ApplicationList GetApplications(IUserContext userContext)
        {
            ApplicationList applicationList;
            List<WebApplication> webApplicationList;

            CheckTransaction(userContext);
            webApplicationList = WebServiceProxy.UserService.GetApplications(GetClientInformation(userContext));
            applicationList = new ApplicationList();
            foreach (WebApplication webApplication in webApplicationList)
            {
                applicationList.Add(GetApplication(userContext, webApplication));
            }
            return applicationList;
        }


        /// <summary>
        /// Get ApplicationVersion from WebApplicationVersion.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webApplicationVersion">webApplicationVersion.</param>
        /// <returns>Requested application version.</returns>
        public IApplicationVersion GetApplicationVersion(IUserContext userContext,
                                                         WebApplicationVersion webApplicationVersion)
        {
            IApplicationVersion applicationVersion;

            applicationVersion = new ApplicationVersion(userContext);
            UpdateApplicationVersion(userContext, applicationVersion, webApplicationVersion);
            return applicationVersion;
        }


        /// <summary>
        /// Get applicationversion by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationVersionId">ApplicationVersion id.</param>
        /// <returns>Requested applicationVersion.</returns>       
        public IApplicationVersion GetApplicationVersion(IUserContext userContext,
                                                         Int32 applicationVersionId)
        {
            WebApplicationVersion webApplicationVersion;

            CheckTransaction(userContext);
            webApplicationVersion = WebServiceProxy.UserService.GetApplicationVersion(GetClientInformation(userContext),
                                                                             applicationVersionId);
            return GetApplicationVersion(userContext, webApplicationVersion);
        }

        /// <summary>
        /// Get WebApplicationVersion from ApplicationVersion.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationVersion">ApplicationVersion.</param>
        /// <returns>WebApplicationVersion.</returns>
        public WebApplicationVersion GetApplicationVersion(IUserContext userContext,
                                                           IApplicationVersion applicationVersion)
        {
            WebApplicationVersion webApplicationVersion;
            webApplicationVersion = new WebApplicationVersion();

            webApplicationVersion.ApplicationId = applicationVersion.ApplicationId;
            webApplicationVersion.CreatedBy = applicationVersion.UpdateInformation.CreatedBy;
            webApplicationVersion.CreatedDate = applicationVersion.UpdateInformation.CreatedDate;
            webApplicationVersion.Description = applicationVersion.Description;
            webApplicationVersion.Id = applicationVersion.Id;
            webApplicationVersion.IsRecommended = applicationVersion.IsRecommended;
            webApplicationVersion.IsValid = applicationVersion.IsValid;
            webApplicationVersion.ModifiedBy = applicationVersion.UpdateInformation.ModifiedBy;
            webApplicationVersion.ModifiedDate = applicationVersion.UpdateInformation.ModifiedDate;
            webApplicationVersion.ValidFromDate = applicationVersion.ValidFromDate;
            webApplicationVersion.ValidToDate = applicationVersion.ValidToDate;
            webApplicationVersion.Version = applicationVersion.Version;

            return webApplicationVersion;
        }

        /// <summary>
        /// GetApplicationVersionList
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationId">Application id.</param>
        /// <returns>
        /// Returns list of application versions or null if no application versions are found.
        /// </returns>
        public ApplicationVersionList GetApplicationVersionList(IUserContext userContext,
                                                                Int32 applicationId)
        {
            ApplicationVersionList applicationVersionList;
            List<WebApplicationVersion> webApplicationVersionList;

            CheckTransaction(userContext);
            webApplicationVersionList = WebServiceProxy.UserService.GetApplicationVersions(GetClientInformation(userContext), applicationId);
            applicationVersionList = new ApplicationVersionList();
            foreach (WebApplicationVersion webApplicationVersion in webApplicationVersionList)
            {
                applicationVersionList.Add(GetApplicationVersion(userContext, webApplicationVersion));
            }
            return applicationVersionList;
        }

        /// <summary>
        /// Get applicationversion by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationActionId">ApplicationAction id.</param>
        /// <returns>Requested applicationAction.</returns>       
        public IApplicationAction GetApplicationAction(IUserContext userContext,
                                                       Int32 applicationActionId)
        {
            WebApplicationAction webApplicationAction;

            CheckTransaction(userContext);
            webApplicationAction = WebServiceProxy.UserService.GetApplicationAction(GetClientInformation(userContext),
                                                                           applicationActionId);
            return GetApplicationAction(userContext, webApplicationAction);
        }


        /// <summary>
        /// Get WebApplicationAction from ApplicationAction.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationAction">ApplicationAction.</param>
        /// <returns>WebApplicationAction.</returns>
        public WebApplicationAction GetApplicationAction(IUserContext userContext,
                                                         IApplicationAction applicationAction)
        {
            WebApplicationAction webApplicationAction;
            webApplicationAction = new WebApplicationAction();

            webApplicationAction.ApplicationId = applicationAction.ApplicationId;
            webApplicationAction.Identifier = applicationAction.Identifier;
            if (applicationAction.AdministrationRoleId.HasValue)
            {
                webApplicationAction.AdministrationRoleId = applicationAction.AdministrationRoleId.Value;
            }
            webApplicationAction.CreatedBy = applicationAction.UpdateInformation.CreatedBy;
            webApplicationAction.CreatedDate = applicationAction.UpdateInformation.CreatedDate;
            webApplicationAction.Description = applicationAction.Description;
            webApplicationAction.GUID = applicationAction.GUID;
            webApplicationAction.Id = applicationAction.Id;
            webApplicationAction.IsAdministrationRoleIdSpecified = applicationAction.AdministrationRoleId.HasValue;
            webApplicationAction.ModifiedBy = applicationAction.UpdateInformation.ModifiedBy;
            webApplicationAction.ModifiedDate = applicationAction.UpdateInformation.ModifiedDate;
            webApplicationAction.Name = applicationAction.Name;
            webApplicationAction.ValidFromDate = applicationAction.ValidFromDate;
            webApplicationAction.ValidToDate = applicationAction.ValidToDate;

            return webApplicationAction;
        }

        /// <summary>
        /// Get ApplicationAction from WebApplicationAction.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webApplicationAction">webApplicationAction.</param>
        /// <returns>Requested application version.</returns>
        public IApplicationAction GetApplicationAction(IUserContext userContext,
                                                       WebApplicationAction webApplicationAction)
        {
            IApplicationAction applicationAction;

            applicationAction = new ApplicationAction(userContext);
            UpdateApplicationAction(userContext, applicationAction, webApplicationAction);
            return applicationAction;
        }

        /// <summary>
        /// Get all application actions for an application.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationId">Application id.</param>
        /// <returns>
        /// Returns list of application actions or null if no application actions are found.
        /// </returns>
        public ApplicationActionList GetApplicationActions(IUserContext userContext,
                                                           Int32 applicationId)
        {
            List<WebApplicationAction> webApplicationActionList;

            CheckTransaction(userContext);
            webApplicationActionList = WebServiceProxy.UserService.GetApplicationActions(GetClientInformation(userContext), applicationId);
            return GetApplicationActions(userContext, webApplicationActionList);
        }

        /// <summary>
        /// Get list of application action objects.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationActionGUIDs">List of application action GUIDs</param>
        /// <returns>
        /// Returns list of application actions or null if no application actions are found.
        /// </returns>
        public ApplicationActionList GetApplicationActionsByGUIDs(IUserContext userContext,
                                                                  List<String> applicationActionGUIDs)
        {
            List<WebApplicationAction> webApplicationActionList;

            CheckTransaction(userContext);
            webApplicationActionList = WebServiceProxy.UserService.GetApplicationActionsByGUIDs(GetClientInformation(userContext), applicationActionGUIDs);
            return GetApplicationActions(userContext, webApplicationActionList);
        }

        /// <summary>
        /// Get list of application action objects.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationActionIds">List of application action id</param>
        /// <returns>
        /// Returns list of application actions or null if no application actions are found.
        /// </returns>
        public ApplicationActionList GetApplicationActionsByIds(IUserContext userContext,
                                                                List<Int32> applicationActionIds)
        {
            List<WebApplicationAction> webApplicationActionList;

            CheckTransaction(userContext);
            webApplicationActionList = WebServiceProxy.UserService.GetApplicationActionsByIds(GetClientInformation(userContext), applicationActionIds);
            return GetApplicationActions(userContext, webApplicationActionList);
        }

        /// <summary>
        /// Get ApplicationActionList from list of WebApplicationActions
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webApplicationActionList">List of WebApplicationActions.</param>
        /// <returns>ApplicationActionList.</returns>
        private ApplicationActionList GetApplicationActions(IUserContext userContext,
                                                            List<WebApplicationAction> webApplicationActionList)
        {
            ApplicationActionList applicationActionList;

            CheckTransaction(userContext);
            applicationActionList = new ApplicationActionList();
            if (webApplicationActionList.IsNotEmpty())
            {
                foreach (WebApplicationAction webApplicationAction in webApplicationActionList)
                {
                    applicationActionList.Add(GetApplicationAction(userContext, webApplicationAction));
                }
            }
            return applicationActionList;
        }

        /// <summary>
        /// Test if application version is valid.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationIdentifier">Application identity.</param>
        /// <param name="version">Version to check if valid or not</param>
        /// <returns>
        /// Information about requested applicationversion.
        ///</returns>     
        public IApplicationVersion IsApplicationVersionValid(IUserContext userContext,
                                                             String applicationIdentifier,
                                                             String version)
        {
            WebApplicationVersion webApplicationVersion;
            ApplicationVersion applicationVersion;

            CheckTransaction(userContext);
            applicationVersion = new ApplicationVersion(userContext);
            webApplicationVersion = WebServiceProxy.UserService.IsApplicationVersionValid(GetClientInformation(userContext),
                                                                applicationIdentifier,
                                                                version);
            UpdateApplicationVersion(userContext, applicationVersion, webApplicationVersion);
            return applicationVersion;
        }

        /// <summary>
        /// Removes an authority data type from an application
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="authorityDataTypeId">AuthorityDataType Id.</param>
        /// <param name="applicationId">Application Id.</param>
        /// <returns>void</returns>
        public void RemoveAuthorityDataTypeFromApplication(IUserContext userContext,
                                                           Int32 authorityDataTypeId,
                                                           Int32 applicationId)
        {
            CheckTransaction(userContext);
            WebServiceProxy.UserService.RemoveAuthorityDataTypeFromApplication(GetClientInformation(userContext),
                                                                                authorityDataTypeId,
                                                                                applicationId);
        }
        
        /// <summary>
        /// Update application.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="application">
        /// Information about the updated application.
        /// This object is updated with information 
        /// about the updated application.
        /// </param>
        public void UpdateApplication(IUserContext userContext, IApplication application)
        {
            WebApplication webApplication;

            CheckTransaction(userContext);
            webApplication = WebServiceProxy.UserService.UpdateApplication(GetClientInformation(userContext),
                                                                  GetApplication(userContext, application));
            UpdateApplication(userContext, application, webApplication);
        }

        /// <summary>
        /// Update application object.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="application">Application.</param>
        /// <param name="webApplication">Web application.</param>
        private void UpdateApplication(IUserContext userContext,
                                        IApplication application,
                                        WebApplication webApplication)
        {
            if (webApplication.IsAdministrationRoleIdSpecified)
            {
                application.AdministrationRoleId = webApplication.AdministrationRoleId;
            }
            else
            {
                application.AdministrationRoleId = null;
            }
            application.Identifier = webApplication.Identifier;
            application.ContactPersonId = webApplication.ContactPersonId;
            application.UpdateInformation.CreatedBy = webApplication.CreatedBy;
            application.UpdateInformation.CreatedDate = webApplication.CreatedDate;
            application.DataContext = GetDataContext(userContext);
            application.Description = webApplication.Description;
            application.GUID = webApplication.GUID;
            application.Id = webApplication.Id;
            application.UpdateInformation.ModifiedBy = webApplication.ModifiedBy;
            application.UpdateInformation.ModifiedDate = webApplication.ModifiedDate;
            application.Name = webApplication.Name;
            application.ShortName = webApplication.ShortName;
            application.URL = webApplication.URL;
            application.ValidFromDate = webApplication.ValidFromDate;
            application.ValidToDate = webApplication.ValidToDate;
        }


        /// <summary>
        /// Update ApplicationVersion.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationVersion">
        /// Information about the updated applicationVersion.
        /// This object is updated with information 
        /// about the updated applicationVersion.
        /// </param>
        public void UpdateApplicationVersion(IUserContext userContext, IApplicationVersion applicationVersion)
        {
            WebApplicationVersion webApplicationVersion;

            CheckTransaction(userContext);
            webApplicationVersion = WebServiceProxy.UserService.UpdateApplicationVersion(GetClientInformation(userContext),
                                                                                GetApplicationVersion(userContext, applicationVersion));
            UpdateApplicationVersion(userContext, applicationVersion, webApplicationVersion);
        }

        /// <summary>
        /// Update applicationVersion object.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationVersion">ApplicationVersion.</param>
        /// <param name="webApplicationVersion">Web application version.</param>
        private void UpdateApplicationVersion(IUserContext userContext,
                                              IApplicationVersion applicationVersion,
                                              WebApplicationVersion webApplicationVersion)
        {
            applicationVersion.ApplicationId = webApplicationVersion.ApplicationId;
            applicationVersion.UpdateInformation.CreatedBy = webApplicationVersion.CreatedBy;
            applicationVersion.UpdateInformation.CreatedDate = webApplicationVersion.CreatedDate;
            applicationVersion.DataContext = GetDataContext(userContext);
            applicationVersion.Description = webApplicationVersion.Description;
            applicationVersion.Id = webApplicationVersion.Id;
            applicationVersion.IsRecommended = webApplicationVersion.IsRecommended;
            applicationVersion.IsValid = webApplicationVersion.IsValid;
            applicationVersion.UpdateInformation.ModifiedBy = webApplicationVersion.ModifiedBy;
            applicationVersion.UpdateInformation.ModifiedDate = webApplicationVersion.ModifiedDate;
            applicationVersion.ValidFromDate = webApplicationVersion.ValidFromDate;
            applicationVersion.ValidToDate = webApplicationVersion.ValidToDate;
            applicationVersion.Version = webApplicationVersion.Version;
        }

        /// <summary>
        /// Update applicationAction object.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationAction">ApplicationAction.</param>
        /// <param name="webApplicationAction">Web application version.</param>
        private void UpdateApplicationAction(IUserContext userContext,
                                             IApplicationAction applicationAction,
                                             WebApplicationAction webApplicationAction)
        {
            applicationAction.Identifier = webApplicationAction.Identifier;
            if (webApplicationAction.IsAdministrationRoleIdSpecified)
            {
                applicationAction.AdministrationRoleId = webApplicationAction.AdministrationRoleId;
            }
            else
            {
                applicationAction.AdministrationRoleId = null;
            }
            applicationAction.ApplicationId = webApplicationAction.ApplicationId;
            applicationAction.UpdateInformation.CreatedBy = webApplicationAction.CreatedBy;
            applicationAction.UpdateInformation.CreatedDate = webApplicationAction.CreatedDate;
            applicationAction.DataContext = GetDataContext(userContext);
            applicationAction.Description = webApplicationAction.Description;
            applicationAction.GUID = webApplicationAction.GUID;
            applicationAction.Id = webApplicationAction.Id;
            applicationAction.Name = webApplicationAction.Name;
            applicationAction.UpdateInformation.ModifiedBy = webApplicationAction.ModifiedBy;
            applicationAction.UpdateInformation.ModifiedDate = webApplicationAction.ModifiedDate;
            applicationAction.ValidFromDate = webApplicationAction.ValidFromDate;
            applicationAction.ValidToDate = webApplicationAction.ValidToDate;
        }


        /// <summary>
        /// Update ApplicationAction.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationAction">
        /// Information about the updated applicationAction.
        /// This object is updated with information 
        /// about the updated applicationAction.
        /// </param>
        public void UpdateApplicationAction(IUserContext userContext, IApplicationAction applicationAction)
        {
            WebApplicationAction webApplicationAction;

            CheckTransaction(userContext);
            webApplicationAction = WebServiceProxy.UserService.UpdateApplicationAction(GetClientInformation(userContext),
                                                                              GetApplicationAction(userContext, applicationAction));
            UpdateApplicationAction(userContext, applicationAction, webApplicationAction);
        }
    }
}

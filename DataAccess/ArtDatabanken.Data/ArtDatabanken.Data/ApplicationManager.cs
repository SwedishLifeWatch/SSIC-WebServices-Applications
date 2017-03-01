using System;
using System.Collections.Generic;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles application related information.
    /// </summary>
    public class ApplicationManager : IApplicationManager
    {
        /// <summary>
        /// This property is used to retrieve or update information.
        /// </summary>
        public IApplicationDataSource DataSource { get; set; }

        /// <summary>
        /// Adds an authority data type to an application.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="authorityDataTypeId">AuthorityDataType Id.</param>
        /// <param name="applicationId">Application Id.</param>       
        public virtual void AddAuthorityDataTypeToApplication(IUserContext userContext,
                                                              Int32 authorityDataTypeId,
                                                              Int32 applicationId)
        {
            DataSource.AddAuthorityDataTypeToApplication(userContext, authorityDataTypeId, applicationId);
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
        public virtual void CreateApplication(IUserContext userContext,
                                              IApplication application)
        {
            DataSource.CreateApplication(userContext, application);
        }

        /// <summary>
        /// Create new application action.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationAction">
        /// Information about the new application action.
        /// This object is updated with information 
        /// about the created application action.
        /// </param>
        public virtual void CreateApplicationAction(IUserContext userContext,
                                                     IApplicationAction applicationAction)
        {
            DataSource.CreateApplicationAction(userContext, applicationAction);
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
        public virtual void CreateApplicationVersion(IUserContext userContext,
                                                     IApplicationVersion applicationVersion)
        {
            DataSource.CreateApplicationVersion(userContext, applicationVersion);
        }

        /// <summary>
        /// Delete a application.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="application">Delete this application.</param>
        public virtual void DeleteApplication(IUserContext userContext,
                                              IApplication application)
        {
            DataSource.DeleteApplication(userContext, application);
        }

        /// <summary>
        /// Removes an authority data type from an application.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="authorityDataTypeId">AuthorityDataType Id.</param>
        /// <param name="applicationId">Application Id.</param>     
        public virtual void RemoveAuthorityDataTypeFromApplication(IUserContext userContext,
                                                                   Int32 authorityDataTypeId,
                                                                   Int32 applicationId)
        {
            DataSource.RemoveAuthorityDataTypeFromApplication(userContext, authorityDataTypeId, applicationId);
        }

        /// <summary>
        /// Get information about data source.
        /// </summary>
        /// <returns>Information about data source.</returns>
        public virtual IDataSourceInformation GetDataSourceInformation()
        {
            return DataSource.GetDataSourceInformation();
        }

        /// <summary>
        /// Get application by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationId">Application id.</param>
        /// <returns>Requested application.</returns>       
        public virtual IApplication GetApplication(IUserContext userContext,
                                                   Int32 applicationId)
        {
            return DataSource.GetApplication(userContext, applicationId);
        }

        /// <summary>
        /// Get application by identifier.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationIdentifier">Application identifier.</param>
        /// <returns>Requested application.</returns>       
        public virtual IApplication GetApplication(IUserContext userContext,
                                                   ApplicationIdentifier applicationIdentifier)
        {
            foreach (IApplication application in GetApplications(userContext))
            {
                if (application.Identifier == applicationIdentifier.ToString())
                {
                    return application;
                }
            }

            return null;
        }

        /// <summary>
        /// Get ApplicationAction by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationActionId">ApplicationAction id.</param>
        /// <returns>Requested ApplicationAction.</returns>       
        public virtual IApplicationAction GetApplicationAction(IUserContext userContext,
                                                               Int32 applicationActionId)
        {
            return DataSource.GetApplicationAction(userContext, applicationActionId);
        }

        /// <summary>
        /// Get all application actions for an application.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationId">Application id.</param>
        /// <returns>
        /// Returns list of application actions or null if no application actions are found.
        /// </returns>
        public virtual ApplicationActionList GetApplicationActions(IUserContext userContext,
                                                                   Int32 applicationId)
        {
            return DataSource.GetApplicationActions(userContext, applicationId);
        }

        /// <summary>
        /// Get list of application action objects.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationActionGuids">List of application action GUIDs.</param>
        /// <returns>
        /// Returns list of application actions or null if no application actions are found.
        /// </returns>
        public virtual ApplicationActionList GetApplicationActionsByGUIDs(IUserContext userContext,
                                                                          List<String> applicationActionGuids)
        {
            return DataSource.GetApplicationActionsByGUIDs(userContext, applicationActionGuids);
        }

        /// <summary>
        /// Get list of application action objects.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationActionIds">List of application action id.</param>
        /// <returns>
        /// Returns list of application actions or null if no application actions are found.
        /// </returns>
        public virtual ApplicationActionList GetApplicationActionsByIds(IUserContext userContext,
                                                                        List<Int32> applicationActionIds)
        {
            return DataSource.GetApplicationActionsByIds(userContext, applicationActionIds);
        }

        /// <summary>
        /// Get Applications.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>
        /// Returns list of applications or null if no applications are found.
        /// </returns>
        public virtual ApplicationList GetApplications(IUserContext userContext)
        {
            return DataSource.GetApplications(userContext);
        }

        /// <summary>
        /// Get ApplicationVersion by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationVersionId">ApplicationVersion id.</param>
        /// <returns>Requested ApplicationVersion.</returns>       
        public virtual IApplicationVersion GetApplicationVersion(IUserContext userContext,
                                                                 Int32 applicationVersionId)
        {
            return DataSource.GetApplicationVersion(userContext, applicationVersionId);
        }

        /// <summary>
        /// Get specified application version.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationIdentifier">Application identifier.</param>
        /// <param name="applicationVersion">Application version.</param>
        /// <returns>Specified application version or null if version does not exists.</returns>
        public virtual IApplicationVersion GetApplicationVersion(IUserContext userContext,
                                                                 String applicationIdentifier,
                                                                 String applicationVersion)
        {
            ApplicationList applications;
            ApplicationVersionList applicationVersions;

            applications = GetApplications(userContext);
            foreach (IApplication application in applications)
            {
                if (application.Identifier.ToLower() == applicationIdentifier.ToLower())
                {
                    applicationVersions = GetApplicationVersions(userContext, application.Id);
                    if (applicationVersions.IsNotEmpty())
                    {
                        foreach (IApplicationVersion tempApplicationVersion in applicationVersions)
                        {
                            if (tempApplicationVersion.Version.ToLower() == applicationVersion.ToLower())
                            {
                                return tempApplicationVersion;
                            }
                        }
                    }

                    break;
                }
            }

            return null;
        }

        /// <summary>
        /// Get ApplicationVersionList.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationId">Application id.</param>
        /// <returns>
        /// Returns list of application versions or null if no application versions are found.
        /// </returns>
        public virtual ApplicationVersionList GetApplicationVersions(IUserContext userContext,
                                                                     Int32 applicationId)
        {
            return DataSource.GetApplicationVersionList(userContext, applicationId);
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
        public virtual void UpdateApplication(IUserContext userContext,
                                               IApplication application)
        {
            DataSource.UpdateApplication(userContext, application);
        }

        /// <summary>
        /// Update ApplicationAction.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationAction">
        /// Information about the updated ApplicationAction.
        /// This object is updated with information 
        /// about the updated ApplicationAction.
        /// </param>
        public virtual void UpdateApplicationAction(IUserContext userContext,
                                                    IApplicationAction applicationAction)
        {
            DataSource.UpdateApplicationAction(userContext, applicationAction);
        }

        /// <summary>
        /// Update ApplicationVersion.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationVersion">
        /// Information about the updated ApplicationVersion.
        /// This object is updated with information 
        /// about the updated ApplicationVersion.
        /// </param>
        public virtual void UpdateApplicationVersion(IUserContext userContext,
                                                     IApplicationVersion applicationVersion)
        {
            DataSource.UpdateApplicationVersion(userContext, applicationVersion);
        }
    }
}
using System;
using System.Collections.Generic;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Definition of the ApplicationManager interface.
    /// </summary>
    public interface IApplicationManager : IManager
    {
        /// <summary>
        /// This interface is used to retrieve information.
        /// </summary>
        IApplicationDataSource DataSource
        { get; set; }

        /// <summary>
        /// Adds an authority data type to an application
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="authorityDataTypeId">AuthorityDataType Id.</param>
        /// <param name="applicationId">Application Id.</param>
        
        void AddAuthorityDataTypeToApplication(IUserContext userContext,
                                               Int32 authorityDataTypeId,
                                               Int32 applicationId);

        /// <summary>
        /// Create new application.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="application">
        /// Information about the new application.
        /// This object is updated with information 
        /// about the created application.
        /// </param>
        void CreateApplication(IUserContext userContext,
                               IApplication application);

        /// <summary>
        /// Create new application action.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationAction">
        /// Information about the new application action.
        /// This object is updated with information 
        /// about the created application action.
        /// </param>
        void CreateApplicationAction(IUserContext userContext,
                                     IApplicationAction applicationAction);
        
        /// <summary>
        /// Create new application version.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationVersion">
        /// Information about the new application version.
        /// This object is updated with information 
        /// about the created application version.
        /// </param>
        void CreateApplicationVersion(IUserContext userContext,
                                      IApplicationVersion applicationVersion);

        /// <summary>
        /// Delete a application.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="application">Delete this application.</param>
        void DeleteApplication(IUserContext userContext, 
                               IApplication application);

        /// <summary>
        /// Get application by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationId">Application id.</param>
        /// <returns>Requested application.</returns>       
        IApplication GetApplication(IUserContext userContext,
                                    Int32 applicationId);

        /// <summary>
        /// Get application by identifier.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationIdentifier">Application identifier.</param>
        /// <returns>Requested application.</returns>       
        IApplication GetApplication(IUserContext userContext,
                                    ApplicationIdentifier applicationIdentifier);

        /// <summary>
        /// Get applicationAction by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationActionId">ApplicationAction id.</param>
        /// <returns>Requested applicationAction.</returns>       
        IApplicationAction GetApplicationAction(IUserContext userContext,
                                                Int32 applicationActionId);

        /// <summary>
        /// Get application actions
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationId">Application id.</param>
        /// <returns>
        /// Returns list of application actions or null if no application actions are found.
        /// </returns>
        ApplicationActionList GetApplicationActions(IUserContext userContext,
                                                    Int32 applicationId);

        /// <summary>
        /// Get list of application action objects.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationActionGuids">List of application action GUIDs</param>
        /// <returns>
        /// Returns list of application actions or null if no application actions are found.
        /// </returns>
        ApplicationActionList GetApplicationActionsByGUIDs(IUserContext userContext,
                                                           List<String> applicationActionGuids);

        /// <summary>
        /// Get list of application action objects.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationActionIds">List of application action id</param>
        /// <returns>
        /// Returns list of application actions or null if no application actions are found.
        /// </returns>
        ApplicationActionList GetApplicationActionsByIds(IUserContext userContext,
                                                         List<Int32> applicationActionIds);

        /// <summary>
        /// Get applications.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>
        /// Returns list of applications or null if no applications are found.
        /// </returns>
        ApplicationList GetApplications(IUserContext userContext);

        /// <summary>
        /// Get applicationVersion by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationVersionId">ApplicationVersion id.</param>
        /// <returns>Requested applicationVersion.</returns>       
        IApplicationVersion GetApplicationVersion(IUserContext userContext,
                                                  Int32 applicationVersionId);

        /// <summary>
        /// Get specified application version.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationIdentifier">Application identifier.</param>
        /// <param name="applicationVersion">Application version.</param>
        /// <returns>Specified application version or null if version does not exists.</returns>
        IApplicationVersion GetApplicationVersion(IUserContext userContext,
                                                  String applicationIdentifier,
                                                  String applicationVersion);

        /// <summary>
        /// Get application versions.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationId">Application id.</param>
        /// <returns>
        /// Returns list of application versions or null if no application versions are found.
        /// </returns>
        ApplicationVersionList GetApplicationVersions(IUserContext userContext,
                                                      Int32 applicationId);

        /// <summary>
        /// Removes an authority data type from an application
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="authorityDataTypeId">AuthorityDataType Id.</param>
        /// <param name="applicationId">Application Id.</param>
        
        void RemoveAuthorityDataTypeFromApplication(IUserContext userContext,
                                                    Int32 authorityDataTypeId,
                                                    Int32 applicationId);
        
        /// <summary>
        /// Update application.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="application">
        /// Information about the updated application.
        /// This object is updated with information 
        /// about the updated application.
        /// </param>
        void UpdateApplication(IUserContext userContext, 
                               IApplication application);

        /// <summary>
        /// Update ApplicationVersion.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationVersion">
        /// Information about the updated Application Version.
        /// This object is updated with information 
        /// about the updated ApplicationVersion.
        /// </param>
        void UpdateApplicationVersion(IUserContext userContext,
                                      IApplicationVersion applicationVersion);

        /// <summary>
        /// Update ApplicationAction.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationAction">
        /// Information about the updated Application Action.
        /// This object is updated with information 
        /// about the updated Application Action.
        /// </param>
        void UpdateApplicationAction(IUserContext userContext,
                                      IApplicationAction applicationAction);

    }
}

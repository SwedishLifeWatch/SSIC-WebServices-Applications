using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.DataSource
{
    /// <summary>
    /// Definition of the AppliactionDataSource interface.
    /// This interface is used to retrieve application related information.
    /// </summary>
    public interface IApplicationDataSource : IDataSource
    {

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
        /// Get applicationaction by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationActionId">ApplicationAction id.</param>
        /// <returns>Requested ApplicationAction.</returns>       
        IApplicationAction GetApplicationAction(IUserContext userContext,
                                                Int32 applicationActionId);

        /// <summary>
        /// Get all application actions for an application.
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
        /// <param name="applicationActionGUIDs">List of application action GUIDs</param>
        /// <returns>
        /// Returns list of application actions or null if no application actions are found.
        /// </returns>
        ApplicationActionList GetApplicationActionsByGUIDs(IUserContext userContext,
                                                           List<String> applicationActionGUIDs);

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
        /// GetApplications
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>
        /// Returns list of applications or null if no applications are found.
        /// </returns>
        ApplicationList GetApplications(IUserContext userContext);


        /// <summary>
        /// Get applicationversion by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationVersionId">ApplicationVersion id.</param>
        /// <returns>Requested applicationVersion.</returns>       
        IApplicationVersion GetApplicationVersion(IUserContext userContext,
                                                  Int32 applicationVersionId);

        /// <summary>
        /// GetApplicationVersionList
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationId">Application id.</param>
        /// <returns>
        /// Returns list of application versions or null if no application versions are found.
        /// </returns>
        ApplicationVersionList GetApplicationVersionList(IUserContext userContext,
                                                         Int32 applicationId);


        /// <summary>
        /// Test if application version is valid.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationIdentifier">Application identity.</param>
        /// <param name="version">Version to check if valid or not</param>
        /// <returns>
        /// Information about requested applicationversion.
        /// </returns>     
        IApplicationVersion IsApplicationVersionValid(IUserContext userContext,
                                                      String applicationIdentifier,
                                                      String version);

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
        /// Update ApplicationAction.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationAction">
        /// Information about the updated applicationAction.
        /// This object is updated with information 
        /// about the updated ApplicationAction.
        /// </param>
        void UpdateApplicationAction(IUserContext userContext,
                                      IApplicationAction applicationAction);

        /// <summary>
        /// Update ApplicationVersion.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationVersion">
        /// Information about the updated applicationVersion.
        /// This object is updated with information 
        /// about the updated applicationVersion.
        /// </param>
        void UpdateApplicationVersion(IUserContext userContext,
                                      IApplicationVersion applicationVersion);


    }
}

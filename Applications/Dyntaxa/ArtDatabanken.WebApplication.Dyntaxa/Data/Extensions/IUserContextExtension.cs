using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Helpers.Extensions
{
    /// <summary>
    /// Extension methods for IUserContext
    /// </summary>
    public static class IUserContextExtension
    {
        private const string WebserviceadministratorIdentifier = "WebServiceAdministrator";

        /// <summary>
        /// Checks if the user is Authenticated
        /// The user is Authenticated if it's not Application user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsAuthenticated(this IUserContext user)
        {
            if (user == null)
            {
                return false;
            }

            return user.User.UserName != Resources.DyntaxaSettings.Default.DyntaxaApplicationUserName;
        }

        /// <summary>
        /// Checks if the user is TaxonRevisionAdministrator
        /// </summary>
        /// <param name="user">the user</param>
        /// <returns></returns>
        public static bool IsTaxonRevisionAdministrator(this IUserContext user)
        {
            return IsAuthenticated(user) && AuthorizationManager.IsTaxonRevisionAdministrator(user);
        }

        public static bool IsWebServiceAdministrator(this IUserContext user)
        {
            return AuthorizationManager.HasAuthority(user, WebserviceadministratorIdentifier);
        }

        /// <summary>
        /// Checks if the user is TaxonEditor
        /// </summary>
        /// <param name="user">the user</param>
        /// <returns></returns>
        public static bool IsTaxonEditor(this IUserContext user)
        {
            return IsAuthenticated(user) && AuthorizationManager.IsTaxonEditor(user);
        }

        /// <summary>
        /// Checks if the user has authority to edit species facts and reference relations.
        /// </summary>
        /// <param name="user">the user</param>
        /// <returns></returns>
        public static bool HasSpeciesFactAuthority(this IUserContext user)
        {
            return IsAuthenticated(user) && AuthorizationManager.HasSpeciesFactAuthority(user);
        }

        /// <summary>
        /// Checks if the user has authority to read species facts. (Only public data)
        /// </summary>
        /// <param name="user">the user</param>
        /// <returns></returns>
        public static bool HasSpeciesFactFactorReadAuthority(this IUserContext user)
        {
            return IsAuthenticated(user); //&& AuthorizationManager.HasSpeciesFactFactorAuthority(user);
        }

        /// <summary>
        /// Checks if the user is TaxonRevisionEditor
        /// </summary>
        /// <param name="user">the user</param>
        /// <param name="taxonRevision">the revision</param>
        /// <returns></returns>
        public static bool IsTaxonRevisionEditor(this IUserContext user, ITaxonRevision taxonRevision)
        {
            return IsAuthenticated(user) && AuthorizationManager.IsTaxonRevisionEditor(user, taxonRevision);
        }

        /// <summary>
        /// Checks if the user has EditReference authority.
        /// </summary>
        /// <param name="userContext">The user context.</param>        
        /// <returns>
        /// <c>true</c> if the user has EditReference authority; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasEditReferenceAuthority(this IUserContext userContext)
        {
            return AuthorizationManager.HasEditReferenceAuthority(userContext);
        }

        /// <summary>
        /// Checks if the user is TaxonRevisionEditor
        /// </summary>
        /// <param name="user">the user</param>
        /// <param name="revisionGuid">the revision GUID</param>
        /// <returns></returns>
        public static bool IsTaxonRevisionEditor(this IUserContext user, string revisionGuid)
        {
            return IsAuthenticated(user) && AuthorizationManager.IsTaxonRevisionEditor(user, revisionGuid);
        }
        
        /// <summary>
        /// Sets the current user role to TaxonRevisionAdministrator
        /// </summary>
        /// <param name="user">the user</param>
        /// <returns>true if the role existed, otherwise false</returns>
        public static bool SetCurrentUserRoleToTaxonRevisionAdministrator(this IUserContext user)
        {
            return AuthorizationManager.SetUserRoleToTaxonRevisionAdministrator(user);
        }

        /// <summary>
        /// Sets the current user role to TaxonRevisionEditor
        /// </summary>
        /// <param name="user">the user</param>
        /// <param name="revisionGuid">the revision GUID</param>
        /// <returns>true if the role existed, otherwise false</returns>        
        public static bool SetCurrentUserRoleToTaxonRevisionEditor(this IUserContext user, string revisionGuid)
        {
            return AuthorizationManager.SetUserRoleToTaxonRevisionEditor(user, revisionGuid);
        }

        public static bool SetCurrentUserRoleToTaxonRevisionEditor(this IUserContext user, int revisionId)
        {
            try
            {            
                ITaxonRevision taxonRevision = CoreData.TaxonManager.GetTaxonRevision(user, revisionId);
                return AuthorizationManager.SetUserRoleToTaxonRevisionEditor(user, taxonRevision.Guid);
            }
            catch (Exception ex)
            {
                DyntaxaLogger.WriteException(ex);
                return false;                
            }
        }

        /// <summary>
        /// Sets the current user role to TaxonEditor
        /// </summary>
        /// <param name="user">the user</param>        
        /// <returns>true if the role existed, otherwise false</returns>
        public static bool SetCurrentUserRoleToTaxonEditor(this IUserContext user)
        {
            return AuthorizationManager.SetUserRoleToTaxonEditor(user);
        }
    }
}

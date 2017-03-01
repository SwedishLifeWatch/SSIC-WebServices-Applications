using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.Authorization;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions
{
    /// <summary>
    /// Extension methods for IUserContext.
    /// </summary>
    public static class IUserContextExtension
    {
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

            return user.User.UserName != Resources.AppSettings.Default.ApplicationUserName;            
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
        /// Checks if the user has authority to edit species facts and reference relations, temporary check during 
        /// development. Should later be relace by HasSpeciesFactAuthority(IUserContext userContext).
        /// </summary>
        /// <param name="user">the user</param>
        /// <returns></returns>
        public static bool HasSpeciesFactFactorAuthority(this IUserContext user)
        {
            return IsAuthenticated(user) && AuthorizationManager.HasSpeciesFactFactorAuthority(user);
        }

        /// <summary>
        /// Determines whether the current role is the private person role..
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>
        ///   <c>true</c> if the current role is private person; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCurrentRolePrivatePerson(this IUserContext user)
        {
            if (user == null || user.CurrentRole == null)
            {
                return false;
            }            
             
#if DEBUG
            string privatePersonRoleGuid = Resources.AppSettings.Default.PrivatePersonRoleGuidDebug;
#else
            string privatePersonRoleGuid = Resources.AppSettings.Default.PrivatePersonRoleGuidRelease;
#endif
            return user.CurrentRole.GUID == privatePersonRoleGuid;
        }
    }
}

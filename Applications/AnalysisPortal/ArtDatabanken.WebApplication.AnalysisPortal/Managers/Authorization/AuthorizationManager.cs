using System;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers.Authorization
{
    /// <summary>
    /// Manager class that handles all authorization in Dyntaxa Web application.
    /// It is used by Dyntaxa Authorize attribute.
    /// </summary>
    public class AuthorizationManager
    {
        public AuthorizationManager()
        {
            IUserContext userContext = SessionHandler.UserContext;
            this.UserContext = userContext;
        }

        public IUserContext UserContext { get; set; }

        public bool HasAuthority(string identifier)
        {
            if (this.UserContext.IsNull())
            {
                return false;
            }
            else
            {
                return this.UserContext.CurrentRoles.AuthorityExists(identifier);
            }
        }

        public bool ActionIsPermitted(string identifier)
        {
            if (this.UserContext.IsNull())
            {
                return false;
            }
            else
            {
                return this.UserContext.CurrentRoles.ApplicationActionExists(this.UserContext, identifier);
            }
        }

        #region HasSpeciesFactPermission

        /// <summary>
        /// Checks whether or not the user has permissions to edit, create and delete Species facts and Reference relations.
        /// </summary>
        /// <param name="userContext">User Context.</param>
        /// <returns>True or false</returns>
        public static bool HasSpeciesFactAuthority(IUserContext userContext)
        {
            if (userContext.IsNotNull())
            {
                foreach (IRole role in userContext.CurrentRoles)
                {
                    foreach (IAuthority authority in role.Authorities)
                    {
                        if (authority.Identifier == "SpeciesFact" && authority.UpdatePermission)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Checks whether or not the user has permissions to edit, create and delete Species facts and Reference relations.
        /// </summary>
        /// <returns>True or false</returns>
        public bool HasSpeciesFactAuthority()
        {
            if (this.UserContext.IsNotNull())
            {
                foreach (IRole role in this.UserContext.CurrentRoles)
                {
                    foreach (IAuthority authority in role.Authorities)
                    {
                        if (authority.Identifier == "SpeciesFact" && authority.UpdatePermission)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        #endregion

        #region SetRole
        public bool SetUserRole(int index)
        {
            return SetUserRole(this.UserContext, index);
        }

        public static bool SetUserRole(IUserContext user, int index)
        {
            if (index >= user.CurrentRoles.Count)
            {
                return false;
            }

            user.CurrentRole = user.CurrentRoles[index];
            return true;
        }

        #endregion

        /// <summary>
        /// Checks if the user has a specific role
        /// </summary>
        /// <param name="user">the user</param>
        /// <param name="identifier">the role to search for</param>
        /// <param name="index">index of the role found</param>
        /// <returns></returns>
        private static bool RoleExists(IUserContext user, String identifier, out int index)
        {
            for (int i = 0; i < user.CurrentRoles.Count; i++)
            {
                IRole role = user.CurrentRoles[i];
                if (role.Identifier == identifier)
                {
                    index = i;
                    return true;
                }
            }
            index = -1;
            return false;
        }

        #region HasSpeciesFactFactorPermission

        /// <summary>
        /// Checks whether or not the user has permissions to view and export Species facts, temporary check during 
        /// development. Should later be relace by HasSpeciesFactAuthority(IUserContext userContext)
        /// </summary>
        /// <param name="userContext">User Context.</param>
        /// <returns>True or false</returns>
        public static bool HasSpeciesFactFactorAuthority(IUserContext userContext)
        {
            if (userContext.IsNotNull())
            {
                foreach (IRole role in userContext.CurrentRoles)
                {
                    foreach (IAuthority authority in role.Authorities)
                    {
                        if (authority.Identifier == "ArtfaktaIDyntaxa" && authority.UpdatePermission)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Checks whether or not the user has permissions to view and export Species fact, temporary check during 
        /// development. Should late be relace by HasSpeciesFactAuthority().
        /// </summary>
        /// <returns>True or false</returns>
        public bool HasSpeciesFactFactorAuthority()
        {
            if (this.UserContext.IsNotNull())
            {
                foreach (IRole role in this.UserContext.CurrentRoles)
                {
                    foreach (IAuthority authority in role.Authorities)
                    {
                        if (authority.Identifier == "ArtfaktaIDyntaxa" && authority.UpdatePermission)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        #endregion

    }
}

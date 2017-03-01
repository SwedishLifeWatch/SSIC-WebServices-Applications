using System;
using System.Web;
using ArtDatabanken.Data;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Manager class that handles all authorization in Dyntaxa Web application.
    /// It is used by Dyntaxa Authorize attribute.
    /// </summary>
    public class AuthorizationManager
    {
        public AuthorizationManager(HttpSessionStateBase session)
        {
            IUserContext userContext = session["userContext"] as IUserContext;
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

        public static bool HasAuthority(IUserContext userContext, string identifier)
        {
            if (userContext.IsNull())
            {
                return false;
            }
            else
            {
                return userContext.CurrentRoles.AuthorityExists(identifier);
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

#region IsTaxonRevisionAdministrator
        
        public bool IsTaxonRevisionAdministrator()
        {
            return IsTaxonRevisionAdministrator(this.UserContext);            
        }

        public bool IsTaxonRevisionAdministrator(out int index)
        {
            return IsTaxonRevisionAdministrator(this.UserContext, out index);
        }

        public static bool IsTaxonRevisionAdministrator(IUserContext user)
        {
            int index;
            return user != null && RoleExists(user, Resources.DyntaxaSettings.Default.TaxonRevisionAdministrator, out index);
        }

        /// <summary>
        /// Checks if the user is TaxonRevisionAdministrator
        /// </summary>
        /// <param name="user">the user</param>
        /// <param name="index">index of the role found</param>
        /// <returns></returns>
        public static bool IsTaxonRevisionAdministrator(IUserContext user, out int index)
        {
            index = -1;
            return user != null && RoleExists(user, Resources.DyntaxaSettings.Default.TaxonRevisionAdministrator, out index);
        }

#endregion

#region IsTaxonEditor
        public bool IsTaxonEditor()
        {
            return IsTaxonEditor(this.UserContext);
        }

        public bool IsTaxonEditor(out int index)
        {
            return IsTaxonEditor(this.UserContext, out index);
        }

        public static bool IsTaxonEditor(IUserContext user)
        {
            int index;
            return user != null && RoleExists(user, Resources.DyntaxaSettings.Default.DyntaxaTaxonEditor, out index);
        }

        /// <summary>
        /// Checks if the user is TaxonEditor
        /// </summary>
        /// <param name="user">the user</param>
        /// <param name="index">index of the role found</param>
        /// <returns></returns>
        public static bool IsTaxonEditor(IUserContext user, out int index)
        {
            index = -1;
            return user != null && RoleExists(user, Resources.DyntaxaSettings.Default.DyntaxaTaxonEditor, out index);
        }
#endregion

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

        /// <summary>
        /// Checks whether or not the user has permissions to edit, create and delete Species facts and Reference relations.
        /// </summary>
        /// <returns>True or false</returns>
        public bool HasEVASpeciesFactAuthority()
        {
            if (this.UserContext.IsNotNull())
            {
                foreach (IRole role in this.UserContext.CurrentRoles)
                {
                    foreach (IAuthority authority in role.Authorities)
                    {
                        if (authority.Identifier == "EditSpeciesFacts" && authority.UpdatePermission)
                        {
                            return true;
                        }                        
                    }
                }
            }

            return false;
        }
        #endregion

#region IsTaxonRevisionEditor
        public bool IsTaxonRevisionEditor(ITaxonRevision taxonRevision)
        {
            return IsTaxonRevisionEditor(this.UserContext, taxonRevision);
        }

        public bool IsTaxonRevisionEditor(ITaxonRevision taxonRevision, out int index)
        {
            return IsTaxonRevisionEditor(this.UserContext, taxonRevision, out index);
        }

        public bool IsTaxonRevisionEditor(string revisionGuid)
        {
            return IsTaxonRevisionEditor(this.UserContext, revisionGuid);
        }

        public bool IsTaxonRevisionEditor(string revisionGuid, out int index)
        {
            return IsTaxonRevisionEditor(this.UserContext, revisionGuid, out index);
        }

        public static bool IsTaxonRevisionEditor(IUserContext user, ITaxonRevision taxonRevision)
        {
            try
            {                
                return IsTaxonRevisionEditor(user, taxonRevision.Guid);
            }
            catch (Exception)
            {
                return false;                
            }            
        }

        public static bool IsTaxonRevisionEditor(IUserContext user, ITaxonRevision taxonRevision, out int index)
        {
            index = -1;
            try
            {                
                return IsTaxonRevisionEditor(user, taxonRevision.Guid, out index);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsTaxonRevisionEditor(IUserContext user, string revisionGuid)
        {
            int index;
            return IsTaxonRevisionEditor(user, revisionGuid, out index);     
        }

        /// <summary>
        /// Checks if the user is TaxonRevisionEditor
        /// </summary>
        /// <param name="user">the user</param>
        /// <param name="revisionGuid">the revision guid</param>
        /// <param name="index">index of the role found</param>
        /// <returns></returns>
        public static bool IsTaxonRevisionEditor(IUserContext user, string revisionGuid, out int index)
        {
            index = -1;
            if (user.IsNull())
            {
                return false;
            }

            for (int i = 0; i < user.CurrentRoles.Count; i++)
            {
                IRole role = user.CurrentRoles[i];
                string roleIdentifier = Resources.DyntaxaSettings.Default.TaxonRevisionEditor;
                if (role.Identifier != null && role.Identifier.Length > roleIdentifier.Length)
                {
                    if (role.Identifier.Substring(0, roleIdentifier.Length) == roleIdentifier)
                    {
                        string roleRevisionGuid = role.Identifier.Substring(roleIdentifier.Length + 1);
                        if (revisionGuid == roleRevisionGuid)
                        {
                            index = i;
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

        public static bool SetUserRoleToTaxonRevisionAdministrator(IUserContext user)
        {
            int index;
            if (IsTaxonRevisionAdministrator(user, out index))
            {
                SetUserRole(user, index);
                return true;
            }
            return false;
        }

        public static bool SetUserRoleToTaxonRevisionEditor(IUserContext user, string revisionGuid)
        {
            int index;
            if (IsTaxonRevisionEditor(user, revisionGuid, out index))
            {
                SetUserRole(user, index);
                return true;
            }
            return false;
        }

        public static bool SetUserRoleToTaxonEditor(IUserContext user)
        {
            int index;
            if (IsTaxonEditor(user, out index))
            {
                SetUserRole(user, index);
                return true;
            }
            return false;
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

        /// <summary>
        /// Checks if the user has EditReference authority.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the user has EditReference authority; otherwise, <c>false</c>.
        /// </returns>
        public bool HasEditReferenceAuthority()
        {
            return HasAuthority("EditReference");
        }

        /// <summary>
        /// Checks if the user has EditReference authority.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>
        /// <c>true</c> if the user has EditReference authority; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasEditReferenceAuthority(IUserContext userContext)
        {
            return HasAuthority(userContext, "EditReference");
        }
    }
}
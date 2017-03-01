using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the IRole interface.
    /// </summary>
    [Serializable]
    public class RoleList : DataId32List<IRole>
    {
        /// <summary>
        /// Check whether or not any of the roles is accociated with a certain action identifier.
        /// </summary>
        /// <returns></returns>
        public Boolean ApplicationActionExists(IUserContext userContext,
                                               String identifier)
        {
            if (this.IsNotEmpty())
            {
                foreach (IRole role in this)
                {
                    if (role.Authorities.IsNotEmpty())
                    {
                        foreach (IAuthority authority in role.Authorities)
                        {
                            if (authority.ActionGUIDs.IsNotEmpty())
                            {
                                List<Int32> actionIds = new List<Int32>();
                                foreach (String actionId in authority.ActionGUIDs)
                                {
                                    actionIds.Add(Int32.Parse(actionId));
                                }
                                ApplicationActionList actions = CoreData.ApplicationManager.GetApplicationActionsByIds(userContext, actionIds);
                                foreach (IApplicationAction action in actions)
                                {
                                    if (action.Identifier == identifier)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Check whether or not any of the roles is accociated with a certain authority identifier.
        /// </summary>
        /// <returns></returns>
        public bool AuthorityExists(String identifier)
        {
            if (this.IsNotEmpty())
            {
                foreach (IRole role in this)
                {
                    if (role.Authorities.IsNotEmpty())
                    {
                        foreach (IAuthority authority in role.Authorities)
                        {
                            if (authority.Identifier == identifier)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}


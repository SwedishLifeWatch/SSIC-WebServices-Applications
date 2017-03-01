using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This enum is only used in role search and
    /// the definition of the role types are defined
    /// in the web service UserService.
    /// There is no member Type in the class Role.
    /// </summary>
    [DataContract]
    public enum RoleType
    {
        /// <summary>
        /// User administration role.
        /// </summary>
        [EnumMember]
        AdministrationRole,
        /// <summary>
        /// Basic.
        /// </summary>
        [EnumMember]
        Basic,
        /// <summary>
        /// Role related to handling of species observation.
        /// </summary>
        [EnumMember]
        SpeciesObservationRole,
        /// <summary>
        /// Role is not related to any specific purpose.
        /// </summary>
        [EnumMember]
        Unspecified,
        /// <summary>
        /// Role that allows users to administrate user groups.
        /// </summary>
        [EnumMember]
        UserGroupAdministrationRole
    }
}

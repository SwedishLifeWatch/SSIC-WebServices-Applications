using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about a role
    /// </summary>
    public interface IRole : IDataId32
    {
        /// <summary>
        /// AdministrationRoleId
        /// </summary>
        Int32? AdministrationRoleId
        { get; set; }

        /// <summary>
        /// Authorities
        /// </summary>
        AuthorityList Authorities
        { get; set; }

        /// <summary>
        /// Get data context.
        /// </summary>
        IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        String Description
        { get; set; }

        /// <summary>
        /// GUID (Globally Unique Identifier) for this object.
        /// It is a LSID, which is unique for each version of the record holding the information included in this object. 
        /// It is updated automatically by database each time information is saved.
        /// </summary>
        String GUID
        { get; set; }

        /// <summary>
        /// Identifier for this role.
        /// The same identifier can be used for more than a role. 
        /// </summary>
        String Identifier
        { get; set; }

        /// <summary>
        /// Is activation of role membership required for this role.
        /// </summary>
        Boolean IsActivationRequired
        { get; set; }

        /// <summary>
        /// Indicates if this role is used as UserAdministrationRole
        /// </summary>
        Boolean IsUserAdministrationRole
        { get; set; }

        /// <summary>
        /// Message type of this role.
        /// </summary>
        IMessageType MessageType
        { get; set; }

        /// <summary>
        /// Name of role
        /// </summary>
        String Name
        { get; set; }

        /// <summary>
        /// OrganizationId that this role relates to. 
        /// </summary>
        Int32? OrganizationId
        { get; set; }

        /// <summary>
        /// Role shortname
        /// </summary>
        String ShortName
        { get; set; }

        /// <summary>
        /// Information about create/update of role.
        /// </summary>
        IUpdateInformation UpdateInformation
        { get; }

        /// <summary>
        /// UserAdministrationRoleId
        /// </summary>
        Int32? UserAdministrationRoleId
        { get; set; }

        /// <summary>
        /// Date role is valid from. Not Null. Is set to date created by default.
        /// </summary>
        DateTime ValidFromDate
        { get; set; }

        /// <summary>
        /// Date role is valid to. Not Null. Is set to date created + 100 years by default.
        /// </summary>
        DateTime ValidToDate
        { get; set; }

    }
}

using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about an authority
    /// </summary>
    public interface IAuthority : IDataId32
    {
        /// <summary>
        /// List of Application actions GUID.
        /// </summary>
        List<String> ActionGUIDs
        { get; set; }

        /// <summary>
        /// AdministrationRoleId
        /// </summary>
        Int32? AdministrationRoleId
        { get; set; }

        /// <summary>
        /// ApplicationId
        /// </summary>
        Int32 ApplicationId
        { get; set; }

        /// <summary>
        /// AuthorityDataType 
        /// </summary>
        AuthorityDataType AuthorityDataType
        { get; set; }

        /// <summary>
        /// AuthorityType indicates type of Authority i.e Application or AuthorityDataType.
        /// </summary>
        AuthorityType AuthorityType
        { get; set; }

        /// <summary>
        /// Identifier. Mandatory. Max length 50.
        /// This identifier is set by the application administrator. It does not need to be unique. Instead it could optionally be used as an identfier for a sertain type of authorties.
        /// It is normally used as an identifier for the authority object within the fysical application the object is assigned to.
        /// </summary>
        String Identifier
        { get; set; }

        /// <summary>
        /// CreatePermission
        /// </summary>
        Boolean CreatePermission
        { get; set; }

        /// <summary>
        /// Get data context.
        /// </summary>
        IDataContext DataContext
        { get; set; }

        /// <summary>
        /// DeletePermission
        /// </summary>
        Boolean DeletePermission
        { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        String Description
        { get; set; }

        /// <summary>
        /// List of Attribute Factor GUIDs
        /// </summary>
        List<String> FactorGUIDs
        { get; set; }

        /// <summary>
        /// GUID (Globally Unique Identifier) for this object.
        /// It is a LSID, which is unique for each version of the record holding the information included in this object. 
        /// It is updated automatically by database each time information is saved.
        /// </summary>
        String GUID
        { get; set; }

        /// <summary>
        /// List of Attribute locality GUIDs
        /// </summary>
        List<String> LocalityGUIDs
        { get; set; }

        /// <summary>
        /// Max species observation protection level that a
        /// user with this autority has read access rights to.
        /// </summary>
        Int32 MaxProtectionLevel
        { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        String Name
        { get; set; }

        /// <summary>
        /// Obligation
        /// </summary>
        String Obligation
        { get; set; }

        /// <summary>
        /// List of Attribute Project GUIDs
        /// </summary>
        List<String> ProjectGUIDs
        { get; set; }

        /// <summary>
        /// ReadPermission
        /// </summary>
        Boolean ReadPermission
        { get; set; }

        /// <summary>
        /// List of Attribute region GUIDs
        /// </summary>
        List<String> RegionGUIDs
        { get; set; }

        /// <summary>
        /// Role id.
        /// </summary>
        Int32 RoleId
        { get; set; }

        /// <summary>
        /// Read non public data permission.
        /// </summary>
        Boolean ReadNonPublicPermission
        { get; set; }

        /// <summary>
        /// List of Attribute taxon GUIDs.
        /// </summary>
        List<String> TaxonGUIDs
        { get; set; }

        /// <summary>
        /// Update permission.
        /// </summary>
        Boolean UpdatePermission
        { get; set; }

        /// <summary>
        /// Information about create/update of authority.
        /// </summary>
        IUpdateInformation UpdateInformation
        { get; }

        /// <summary>
        /// Date organization is valid from. Not Null.
        /// Is set to date created by default.
        /// </summary>
        DateTime ValidFromDate
        { get; set; }

        /// <summary>
        /// Date organization is valid to. Not Null.
        /// Is set to date created + 100 years by default.
        /// </summary>
        DateTime ValidToDate
        { get; set; }

        /// <summary>
        /// Get list of application action objects.
        /// May be null if no application action object exists.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationActionGuids">List of application action GUIDs</param>
        /// <returns>ApplicationActionList.</returns>
        ApplicationActionList GetApplicationActionsByGUIDs(IUserContext userContext, List<String> applicationActionGuids);

        /// <summary>
        /// Get list of application action objects.
        /// May be null if no application action object exists.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationActionIdList">List of application action id</param>
        /// <returns>Person.</returns>
        ApplicationActionList GetApplicationActionsByIdList(IUserContext userContext,
                                                            List<Int32> applicationActionIdList);

    }
}

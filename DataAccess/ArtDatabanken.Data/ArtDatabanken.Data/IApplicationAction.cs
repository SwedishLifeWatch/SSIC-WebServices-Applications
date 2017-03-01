using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about an organization
    /// </summary>
    public interface IApplicationAction : IDataId32
    {

        /// <summary>
        /// Identifier. Mandatory. Max length 200.
        /// This identifier is set by the application administrator.
        /// It is normally used as an identifier for the application action object within the fysical application the object represents.
        /// </summary>
        String Identifier
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
        /// Name of application action
        /// </summary>
        String Name
        { get; set; }

        /// <summary>
        /// Date application is valid from. Not Null. Is set to date created by default.
        /// </summary>
        DateTime ValidFromDate
        { get; set; }

        /// <summary>
        /// Date application is valid to. Not Null. Is set to date created + 100 years by default.
        /// </summary>
        DateTime ValidToDate
        { get; set; }

         /// <summary>
        /// Get data context.
        /// </summary>
        IDataContext DataContext
        { get; set; }

         /// <summary>
        /// Information about create/update of organization.
        /// </summary>
        IUpdateInformation UpdateInformation
        { get; }

    }
}

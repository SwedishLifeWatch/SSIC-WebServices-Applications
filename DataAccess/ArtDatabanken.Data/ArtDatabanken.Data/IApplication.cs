using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about an organization
    /// </summary>
    public interface IApplication : IDataId32
    {
        /// <summary>
        /// Administration role id. 
        /// Optional. It is set by administrators in order to enable delegation of the administration of this object.
        /// </summary>
        Int32? AdministrationRoleId
        { get; set; }

        /// <summary>
        /// Identifier. Mandatory. Max length 50.
        /// This identifier is set by the application administrator.
        /// It is normally used as an identifier for the application object within the fysical application the object represents.
        /// </summary>
        String Identifier
        { get; set; }

        /// <summary>
        /// Id of the the person object that is corresponding the current contact person of the application. Optional.
        /// </summary>
        Int32? ContactPersonId
        { get; set; }

        /// <summary>
        /// Application description. Optional. Max length 3000.
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
        /// Name of application. This is the full name of the application on current language. Mandatory. Max length 50.
        /// </summary>
        String Name
        { get; set; }

        /// <summary>
        /// ShortName of application. This is the short name of the application on current language. Max length 50.
        /// </summary>
        String ShortName
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
        /// Information about create/update of the application object.
        /// </summary>
        IUpdateInformation UpdateInformation
        { get; }

        /// <summary>
        /// Application URL. Optional. Max length 200.
        /// </summary>
        String URL
        { get; set; }
    }
}

using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    ///  This class contains information about a application.
    /// </summary>
    public class Application : IApplication
    {
        /// <summary>
        /// Create an Application instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        public Application(IUserContext userContext)
        {
            // Set default values.
            Identifier = null;
            AdministrationRoleId = null;
            ContactPersonId = Int32.MinValue;
            DataContext = new DataContext(userContext);
            Description = null;
            GUID = Settings.Default.ApplicationGUIDTemplate;
            Id = Int32.MinValue;
            Name = null;
            ShortName = null;
            UpdateInformation = new UpdateInformation();
            ValidFromDate = DateTime.Now;
            ValidToDate = ValidFromDate + new TimeSpan(Settings.Default.ValidToDateYearIncrement * 365, 0, 0, 0);
            URL = null;
        }

        /// <summary>
        /// Administration role id. 
        /// Optional. It is set by administrators in order to enable delegation of the administration of this object.
        /// </summary>
        public Int32? AdministrationRoleId
        { get; set; }

        /// <summary>
        /// Identifier. Mandatory. Max length 50.
        /// This identifier is set by the application administrator.
        /// It is normally used as an identifier for the application object within the fysical application the object represents.
        /// </summary>
        public String Identifier
        { get; set; }

        /// <summary>
        /// Id of the the person object that is corresponding the current contact person of the application. Optional.
        /// </summary>
        public Int32? ContactPersonId
        { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Application description. Optional. Max length 3000.
        /// </summary>
        public String Description
        { get; set; }

        /// <summary>
        /// GUID (Globally Unique Identifier) for this object.
        /// It is a LSID, which is unique for each version of the record holding the information included in this object. 
        /// It is updated automatically by database each time information is saved.
        /// </summary>
        public String GUID
        { get; set; }

        /// <summary>
        /// Id for this application
        /// </summary>
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Is administration role id specified.
        /// </summary>
        public Boolean IsAdministrationRoleIdSpecified
        { get; set; }

        /// <summary>
        /// Name of application. This is the full name of the application on current language. Mandatory. Max length 50.
        /// </summary>
        public String Name
        { get; set; }


        /// <summary>
        /// ShortName of application. This is the short name of the application on current language. Max length 50.
        /// </summary>
        public String ShortName
        { get; set; }

        /// <summary>
        /// Date application is valid from. Not Null. Is set to date created by default.
        /// </summary>
        public DateTime ValidFromDate
        { get; set; }

        /// <summary>
        /// Date application is valid to. Not Null. Is set to date created + 100 years by default.
        /// </summary>
        public DateTime ValidToDate
        { get; set; }

        /// <summary>
        /// Information about create/update of the application object.
        /// </summary>
        public IUpdateInformation UpdateInformation { get; private set; }

        /// <summary>
        /// Application URL. Optional. Max length 200.
        /// </summary>
        public String URL
        { get; set; }
    }
}

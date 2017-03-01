using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    ///  This class contains information about a application version.
    /// </summary>
    public class ApplicationAction : IApplicationAction
    {
        /// <summary>
        /// Create an ApplicationAction instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        public ApplicationAction(IUserContext userContext)
        {
            // Set default values.
            Identifier = null;
            AdministrationRoleId = null;
            ApplicationId = Int32.MinValue; 
            DataContext = new DataContext(userContext);
            Description = null;
            Id = Int32.MinValue;
            GUID = Settings.Default.ApplicationActionGUIDTemplate;
            Name = null;
            UpdateInformation = new UpdateInformation();
            ValidFromDate = DateTime.Now;
            ValidToDate = ValidFromDate + new TimeSpan(Settings.Default.ValidToDateYearIncrement * 365, 0, 0, 0);
        }

        /// <summary>
        /// Identifier. Mandatory. Max length 200.
        /// This identifier is set by the application administrator.
        /// It is normally used as an identifier for the application action object within the fysical application the object represents.
        /// </summary>
        public String Identifier
        { get; set; }

        /// <summary>
        /// Administration role id. 
        /// Optional. It is set by super administrators in order to enable delegation of the administration of this object.
        /// </summary>
        public Int32? AdministrationRoleId
        { get; set; }

        /// <summary>
        /// ApplicationId. 
        /// </summary>
        public Int32 ApplicationId
        { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; set; }

        /// <summary>
        /// ApplicationAction description. Optional. Max length 3000.
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
        /// Id for this application version
        /// </summary>
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public String Name
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
        /// Information about create/update of application.
        /// </summary>
        public IUpdateInformation UpdateInformation { get; private set; }
    }
}

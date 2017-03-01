using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    ///  This class contains information about a application version.
    /// </summary>
    public class ApplicationVersion : IApplicationVersion
    {
        /// <summary>
        /// Create an ApplicationVersion instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        public ApplicationVersion(IUserContext userContext)
        {
            // Set default values.
            ApplicationId = Int32.MinValue; 
            DataContext = new DataContext(userContext);
            Description = null;
            Id = Int32.MinValue;
            IsRecommended = false;
            IsValid = false;
            UpdateInformation = new UpdateInformation();
            ValidFromDate = DateTime.Now;
            ValidToDate = ValidFromDate + new TimeSpan(Settings.Default.ValidToDateYearIncrement * 365, 0, 0, 0);
            Version = null;
        }

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
        /// ApplicationVersion description. Optional. Max length 3000.
        /// </summary>
        public String Description
        { get; set; }

        /// <summary>
        /// Id for this application version
        /// </summary>
        public Int32 Id
        { get; set; }

        /// <summary>
        /// IsRecommended
        /// </summary>
        public Boolean IsRecommended
        { get; set; }

        /// <summary>
        /// IsValid
        /// </summary>
        public Boolean IsValid
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
        /// Version
        /// </summary>
        public String Version
        { get; set; }

        /// <summary>
        /// Information about create/update of application.
        /// </summary>
        public IUpdateInformation UpdateInformation { get; private set; }
    }
}

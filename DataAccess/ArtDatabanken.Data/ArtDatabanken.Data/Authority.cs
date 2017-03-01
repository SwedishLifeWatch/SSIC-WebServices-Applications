using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    ///  This class contains information about a authority.
    /// </summary>
    [Serializable]
    public class Authority : IAuthority
    {

        /// <summary>
        /// Create an Authority instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        public Authority(IUserContext userContext)
        {
            // Set default values.
            ActionGUIDs = new List<String>();
            AdministrationRoleId = null;
            ApplicationId = Int32.MinValue;
            Identifier = null;
            CreatePermission = false;
            DataContext = new DataContext(userContext);
            DeletePermission = false;
            Description = null;
            GUID = Settings.Default.AuthorityGUIDTemplate;
            FactorGUIDs = new List<String>();
            Id = Int32.MinValue;
            LocalityGUIDs = new List<String>();
            MaxProtectionLevel = 0;
            Name = null;
            Obligation = null;
            ProjectGUIDs = new List<String>();
            ReadPermission = false;
            RegionGUIDs = new List<String>();
            RoleId = Int32.MinValue;
            ReadNonPublicPermission = false;
            TaxonGUIDs = new List<String>();
            UpdateInformation = new UpdateInformation();
            UpdatePermission = false;
            ValidFromDate = DateTime.Now;
            ValidToDate = ValidFromDate + new TimeSpan(Settings.Default.ValidToDateYearIncrement * 365, 0, 0, 0);
        }

        /// <summary>
        /// Application Actions
        /// </summary>
        public List<String> ActionGUIDs
        { get; set; }

        /// <summary>
        /// Administration role id. 
        /// Optional. It is set by super administrators in order to enable delegation of the administration of this object.
        /// </summary>
        public Int32? AdministrationRoleId
        { get; set; }

        /// <summary>
        /// ApplicationId
        /// </summary>
        public Int32 ApplicationId
        { get; set; }

        /// <summary>
        /// AuthorityDataType 
        /// </summary>
        public AuthorityDataType AuthorityDataType
        { get; set; }

        /// <summary>
        /// AuthorityType indicates type of Authority i.e Application or AuthorityDataType.
        /// </summary>
        public AuthorityType AuthorityType
        { get; set; }


        /// <summary>
        /// Identifier. Mandatory. Max length 50.
        /// This identifier is set by the application administrator. It does not need to be unique. Instead it could optionally be used as an identfier for a certain type of authorties.
        /// It is normally used as an identifier for the authority object within the physical application the object is assigned to.
        /// </summary>
        public String Identifier
        { get; set; }

        /// <summary>
        /// CreatePermission
        /// </summary>
        public Boolean CreatePermission
        { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; set; }

        /// <summary>
        /// DeletePermission
        /// </summary>
        public Boolean DeletePermission
        { get; set; }

        /// <summary>
        /// Authority description. Optional. Max length 3000.
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
        /// Attribute Factors
        /// </summary>
        public List<String> FactorGUIDs
        { get; set; }

        /// <summary>
        /// Id for this authority
        /// </summary>
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Attribute Locality
        /// </summary>
        public List<String> LocalityGUIDs
        { get; set; }

        /// <summary>
        /// MaxProtectionLevel
        /// </summary>
        public Int32 MaxProtectionLevel
        { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public String Name
        { get; set; }

        /// <summary>
        /// Obligation
        /// </summary>
        public String Obligation
        { get; set; }

        /// <summary>
        /// Attribute Project
        /// </summary>
        public List<String> ProjectGUIDs
        { get; set; }

        /// <summary>
        /// ReadPermission
        /// </summary>
        public Boolean ReadPermission
        { get; set; }

        /// <summary>
        /// Attribute Regions
        /// </summary>
        public List<String> RegionGUIDs
        { get; set; }

        /// <summary>
        /// RoleId
        /// </summary>
        public Int32 RoleId
        { get; set; }

        /// <summary>
        /// Read non public data permission
        /// </summary>
        public Boolean ReadNonPublicPermission
        { get; set; }

        /// <summary>
        /// Attribute Taxa
        /// </summary>
        public List<String> TaxonGUIDs
        { get; set; }

        /// <summary>
        /// UpdatePermission
        /// </summary>
        public Boolean UpdatePermission
        { get; set; }


        /// <summary>
        /// Information about create/update of authority.
        /// </summary>
        public IUpdateInformation UpdateInformation { get; private set; }

        /// <summary>
        /// Date authority is valid from. Not Null. Is set to date created by default.
        /// </summary>
        public DateTime ValidFromDate
        { get; set; }

        /// <summary>
        /// Date authority is valid to. Not Null. Is set to date created + 100 years by default.
        /// </summary>
        public DateTime ValidToDate
        { get; set; }

        /// <summary>
        /// Get list of application action objects.
        /// May be null if no application action object exists.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationActionGuids">List of application action GUIDs</param>
        /// <returns>ApplicationActionList.</returns>
        public ApplicationActionList GetApplicationActionsByGUIDs(IUserContext userContext, List<String> applicationActionGuids)
        {
            ApplicationActionList applicationActions = new ApplicationActionList();
            if (applicationActionGuids.IsNotEmpty())
            {
                // Get application action list
                applicationActions = CoreData.ApplicationManager.GetApplicationActionsByGUIDs(userContext, applicationActionGuids);
            }

            return applicationActions;
        }

        /// <summary>
        /// Get list of application action objects.
        /// May be null if no application action object exists.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationActionIdList">List of application action id</param>
        /// <returns>ApplicationActionList.</returns>
        public ApplicationActionList GetApplicationActionsByIdList(IUserContext userContext, List<Int32> applicationActionIdList)
        {
            ApplicationActionList _applicationActionList;
            _applicationActionList = new ApplicationActionList();
            if (applicationActionIdList.IsNotEmpty())
            {
                // Get application action list
                _applicationActionList = CoreData.ApplicationManager.GetApplicationActionsByIds(userContext, applicationActionIdList);
            }
            return _applicationActionList;
        }
    }
}

using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    ///  This class contains information about a role.
    /// </summary>
    [Serializable()]
    public class Role : IRole
    {

        /// <summary>
        /// Create a Role instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        public Role(IUserContext userContext)
        {
            // Set default values.
            AdministrationRoleId = null;
            Authorities = new AuthorityList();
            DataContext = new DataContext(userContext);
            Description = null;
            GUID = Settings.Default.RoleGUIDTemplate;
            Id = Int32.MinValue;
            Identifier = null;
            IsActivationRequired = false;
            IsUserAdministrationRole = false;
            // this.MessageType = CoreData.UserManager.GetMessageType(userContext, MessageTypeId.NoMail);
            this.MessageType = new MessageType();
            Name = null;
            OrganizationId = null;
            ShortName = null;
            UpdateInformation = new UpdateInformation();
            UserAdministrationRoleId = null;
            ValidFromDate = DateTime.Now;
            ValidToDate = ValidFromDate + new TimeSpan(Settings.Default.ValidToDateYearIncrement * 365, 0, 0, 0);
        }

        /// <summary>
        /// Administration role id. 
        /// Optional. It is set by super administrators in order to enable delegation of the administration of this object.
        /// </summary>
        public Int32? AdministrationRoleId
        { get; set; }

        /// <summary>
        /// Authorities
        /// </summary>
        public AuthorityList Authorities
        { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Role description. Optional. Max length 3000.
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
        /// Id for this role
        /// </summary>
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Identifier for this role.
        /// The same identifier can be used for more than a role. 
        /// </summary>
        public String Identifier
        { get; set; }

        /// <summary>
        /// Is activation of role membership required for this role.
        /// </summary>
        public Boolean IsActivationRequired
        { get; set; }

        /// <summary>
        /// Indicates if this role is used as UserAdministrationRole
        /// </summary>
        public Boolean IsUserAdministrationRole
        { get; set; }

        /// <summary>
        /// Message type of this role.
        /// </summary>
        public IMessageType MessageType
        { get; set; }

        /// <summary>
        /// Name of role
        /// </summary>
        public String Name
        { get; set; }

        /// <summary>
        /// OrganizationId that this role relates to 
        /// </summary>
        public Int32? OrganizationId
        { get; set; }

        /// <summary>
        /// ShortName of role
        /// </summary>
        public String ShortName
        { get; set; }

        /// <summary>
        /// Date role is valid from. Not Null. Is set to date created by default.
        /// </summary>
        public DateTime ValidFromDate
        { get; set; }

        /// <summary>
        /// Date role is valid to. Not Null. Is set to date created + 100 years by default.
        /// </summary>
        public DateTime ValidToDate
        { get; set; }

        /// <summary>
        /// UserAdministrationRoleId
        /// </summary>
        public Int32? UserAdministrationRoleId
        { get; set; }

        /// <summary>
        /// Information about create/update of role
        /// </summary>
        public IUpdateInformation UpdateInformation
        { get; private set; }
    }
}

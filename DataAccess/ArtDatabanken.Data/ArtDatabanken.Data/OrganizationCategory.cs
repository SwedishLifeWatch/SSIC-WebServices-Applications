using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about an Organization type.
    /// </summary>
    public class OrganizationCategory : IOrganizationCategory
    {
        private String _name;

        /// <summary>
        /// Create an OrganizationCategory instance.
        /// <param name='userContext'>User context.</param>
        /// </summary>
        public OrganizationCategory(IUserContext userContext)
        {
            DataContext = new DataContext(userContext);

            // Set data.
            Id = Int32.MinValue;
            Name = @"OrganizationCategoryName";
            Description = null;
            DescriptionStringId = Int32.MinValue;
            AdministrationRoleId = Int32.MinValue;
            UpdateInformation = new UpdateInformation();
        }


        /// <summary>
        /// Create an OrganizationCategory instance.
        /// </summary>
        /// <param name='id'>Id for this Organization type.</param>
        /// <param name='name'>Name.</param>
        /// <param name="description">Description</param>
        /// <param name='descriptionStringId'>String id for the Description property.</param>
        /// <param name="administrationRoleId">AdministrationRoleId</param>
        /// <param name="updateInformation">UpdateInformation object</param>
        /// <param name='dataContext'>Data context.</param>
        public OrganizationCategory(Int32 id,
                           String name,
                           String description,
                           Int32 descriptionStringId,
                           Int32 administrationRoleId,
                           UpdateInformation updateInformation,
                           IDataContext dataContext)
        {
            // Check data.
            dataContext.CheckNotNull("dataContext");

            // Set data.
            DataContext = dataContext;
            Id = id;
            Name = name;
            Description = description;
            DescriptionStringId = descriptionStringId;
            AdministrationRoleId = administrationRoleId;
            UpdateInformation = updateInformation;
        }

        /// <summary>
        /// Administration role id. 
        /// Optional. It is set by super administrators in order to enable delegation of the administration of this object.
        /// </summary>
        public Int32? AdministrationRoleId
        { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Id for this Organization type.
        /// </summary>
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                value.CheckNotEmpty("_name");
                _name = value;
            }
        }

        /// <summary>
        /// Description.
        /// </summary>
        public String Description
        { get; set; }

        /// <summary>
        /// String id for the Description property.
        /// </summary>
        public Int32 DescriptionStringId
        { get; private set; }

        /// <summary>
        /// Information about create/update of organization.
        /// </summary>
        public IUpdateInformation UpdateInformation
        { get; private set; }
    }
}

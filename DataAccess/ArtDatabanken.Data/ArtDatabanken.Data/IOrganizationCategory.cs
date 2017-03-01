using System;

namespace ArtDatabanken.Data
{

    /// <summary>
    /// This interface handles information about an Organization type.
    /// </summary>
    public interface IOrganizationCategory: IDataId32
    {
        /// <summary>
        /// Get data context.
        /// </summary>
        IDataContext DataContext
        { get; set; }

        /// <summary>
        /// AdministrationRoleId
        /// </summary>
        Int32? AdministrationRoleId
        { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        String Name
        { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        String Description
        { get; set; }

        /// <summary>
        /// Get string id for the Description property.
        /// </summary>
        Int32 DescriptionStringId
        { get; }

        /// <summary>
        /// Information about create/update of OrganizationCategory.
        /// </summary>
        IUpdateInformation UpdateInformation
        { get; }
    }
}

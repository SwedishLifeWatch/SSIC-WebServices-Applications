using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Localization;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Account
{
    public class UserRoleModel
    {
        /// <summary>
        /// List of user roles
        /// </summary>
        public IList<UserRoleDropDownModelHelper> UserRoles { get; set; }

        /// <summary>
        /// Url indication which view to return to
        /// </summary>
        public string ReturnUrl { get; set; }
        
        /// <summary>
        /// Selected user role index in dropdownlist
        /// </summary>
        [Required]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "AccountChangeUserRoleUserRoleText")]
        [LocalizedDisplayName("AccountChangeUserRoleUserRole", NameResourceType = typeof(Resources.Resource))]
        public int UserRoleIndex { get; set; }

        /// <summary>
        /// Description for selected role
        /// </summary>
        [LocalizedDisplayName("AccountChangeUserRoleUserRoleDescription", NameResourceType = typeof(Resources.Resource))]
        public string UserRoleDescription { get; set; }

        /// <summary>
        /// Role name for selected user
        /// </summary>
        public string UserRoleName { get; set; }
 
        /// <summary>
        /// Model labels
        /// </summary>
        private readonly ModelLabels labels = new ModelLabels();
        
        /// <summary>
         /// All localized labels
         /// </summary>
         public ModelLabels Labels
         {
             get { return labels; }
         }

         [Required]
         [LocalizedDisplayName("AccountChangeUserRoleUserRoleId", NameResourceType = typeof(Resources.Resource))]
         public int UserRoleId { get; set; }

        /// <summary>
         /// Localized labels class used in Changeuserrole View
         /// </summary>
         public class ModelLabels
         {
             public string Title
             {
                 get { return Resources.Resource.AccountChangeUserRoleUserRoleTiltle; }
             }
             public string Update
             {
                 get { return Resources.Resource.SharedUpdateButtonText; }
             }
             public string Cancel
             {
                 get { return Resources.Resource.SharedCancelButtonText; }
             }
             public string CurrentRole
             {
                 get { return Resources.Resource.AccountChangeUserRoleUserRoleName; }
             }
             public string UserAdminLink
             {
                 get { return Resources.Resource.AccountUserAdminLinkText; }
             }
             public string UserAdminRoleLink
             {
                 get { return Resources.Resource.AccountUserAdminRoleLinkText; }
             }
         }
    }
}

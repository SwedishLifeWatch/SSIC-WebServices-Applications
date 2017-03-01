using System.ComponentModel.DataAnnotations;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Localization;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Account
{
    public class LogInModel
    {
        [Required]
        [LocalizedDisplayName("SharedUserNameLabel", NameResourceType = typeof(Resources.Resource))]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("SharedPasswordLabel", NameResourceType = typeof(Resources.Resource))]
        public string Password { get; set; }
    }
}

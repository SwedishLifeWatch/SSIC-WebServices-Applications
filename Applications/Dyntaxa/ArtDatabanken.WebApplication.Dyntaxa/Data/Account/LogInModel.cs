using System.ComponentModel.DataAnnotations;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    public class LogInModel : BaseViewModel
    {
        [Required]
        [LocalizedDisplayName("SharedUserNameLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("SharedPasswordLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public string Password { get; set; }
    }
}
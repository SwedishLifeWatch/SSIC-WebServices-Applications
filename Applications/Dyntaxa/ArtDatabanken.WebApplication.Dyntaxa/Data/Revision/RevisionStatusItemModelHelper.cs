// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Holds information on revision categories if selected, name and id.
    /// </summary>
    public class RevisionStatusItemModelHelper
    {
        /// <summary>
        /// Indicated if category is selected
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// Holds revision category/status name
        /// </summary>
        public string RevisionStatusName { get; set; }

        /// <summary>
        /// Holds revision category/status id.
        /// </summary>
        public int RevisionStatusId { get; set; }
    }
}
namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Account
{
    /// <summary>
    /// Helper class for UserRoleModel when selecting a user role from a dropdown list
    /// </summary>
    public class UserRoleDropDownModelHelper
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }

        public UserRoleDropDownModelHelper(int id, string text, string description, int index)
        {
            Id = id;
            Text = text;
            Description = description;
            Index = index;
        }
    }
}

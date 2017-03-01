using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Debug
{
    /// <summary>
    /// This class acts as a view model for showing Session variables i Debug mode.
    /// </summary>
    public class DebugSessionVariablesViewModel
    {
        public string Language { get; set; }

        public static DebugSessionVariablesViewModel Create()
        {
            var model = new DebugSessionVariablesViewModel();
            model.Language = SessionHandler.Language ?? "null";
            return model;
        }
    }
}

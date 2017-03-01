namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result
{
    public class FilterSettingsStatus
    {
        public bool IsOK { get; set; }

        public string Msg { get; set; }

        public static FilterSettingsStatus CreateValidStatus()
        {
            var model = new FilterSettingsStatus();
            model.IsOK = true;
            return model;
        }

        public static FilterSettingsStatus CreateInvalidStatus(string msg)
        {
            var model = new FilterSettingsStatus();
            model.IsOK = false;
            model.Msg = msg;
            return model;
        }
    }
}

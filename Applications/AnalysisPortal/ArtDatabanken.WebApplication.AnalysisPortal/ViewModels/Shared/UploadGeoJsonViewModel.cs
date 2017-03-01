using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Shared
{
    public class UploadGeoJsonViewModel
    {
        public UploadGeoJsonViewModel(string uploadUrl, string successUrl)
        {
            CoordinateSystems = new CoordinateSystemViewModel[]
            {
                new CoordinateSystemViewModel((int)CoordinateSystemId.GoogleMercator, "Google Mercator", false),
                new CoordinateSystemViewModel((int)CoordinateSystemId.Rt90_25_gon_v, "RT 90", false),
                new CoordinateSystemViewModel((int)CoordinateSystemId.SWEREF99_TM, "SWEREF 99", false),
                new CoordinateSystemViewModel((int)CoordinateSystemId.WGS84, "WGS 84", false)
            };

            SuccessUrl = successUrl;
            UploadUrl = uploadUrl;
        }
        public CoordinateSystemViewModel[] CoordinateSystems { get; set; }
        public string UploadUrl { get; set; }
        public string SuccessUrl { get; set; }

        public string FileNameRegEx { get; set; }

        public string FileFormatDescription { get; set; }
    }
}

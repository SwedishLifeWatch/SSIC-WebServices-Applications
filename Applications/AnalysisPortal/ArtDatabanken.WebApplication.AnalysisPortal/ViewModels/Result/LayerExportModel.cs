using System.Web.Mvc;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result
{
    public class LayerExportModel
    {
        public enum FileExportFormat
        {
            GeoJson = 0,
            GeoTiff,
            Shape
        }
        public int? LayerId { get; set; }
        public string Attribute { get; set; }
        public DataType DataType { get; set; }
        public SelectListItem[] DataTypes { get; set; }
        public FileExportFormat ExportFormat { get; set; }
        public SelectListItem[] ExportFormats { get; set; }
        public byte RazterSize { get; set; }
        public int PixelsHeight { get; set; }
        public int PixelsWidth { get; set; }
        public int PixelSize { get; set; }
        public bool PreSelectMode { get; set; }
        public int? AlphaValue { get; set; }
        public bool UseCenterPoint { get; set; }
    }
}

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result
{
    public class MapExportModel
    {
        public class Layer
        {
            public int? AlphaValue { get; set; }
            public string Attribute { get; set; }
            public string GeoJson { get; set; }
            public int Id { get; set; }
            public bool IsPointLayer { get; set; }
            public LegendItem[] Legends { get; set; }
            public string Name { get; set; }
            public byte Occupancy { get; set; }
            public bool? UseCenterPoint { get; set; }
            public int Zindex { get; set; }  
        }

        public class LegendItem
        {
            public string Name { get; set; }
            public string Color { get; set; }
            public int MinValue { get; set; }
            public int MaxValue { get; set; }
        }

        public class Extent
        {
            public double Bottom { get; set; }
            public double Left { get; set; }
            public double Right { get; set; }
            public double Top { get; set; }
        }

        public int Dpi { get; set; }
        public int Scale { get; set; }
        public Layer[] Layers { get; set; }
        public Extent MapExtent { get; set; }
    }
}

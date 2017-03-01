namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid
{
    /// <summary>
    /// This class is the base class for grid cell results
    /// </summary>
    public class GridCellResultBase
    {
        public double CentreCoordinateX { get; set; }

        public double CentreCoordinateY { get; set; }

        public double OriginalCentreCoordinateX { get; set; }

        public double OriginalCentreCoordinateY { get; set; }

        public double[] CentreCoordinate { get; set; }

        public double[][] BoundingBox { get; set; }
    }
}
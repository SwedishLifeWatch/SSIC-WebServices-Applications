namespace ArtDatabanken.Data
{
    /// <summary>
    /// A set of 2, 4, 6 or 8 numbers indicating the upper and
    /// lower bounds of an interval (1D), rectangle (2D),
    /// parallelpiped (3D), or hypercube along each axis of a
    /// given coordinate reference system.
    /// </summary>
    public interface IBoundingBox
    {
        /// <summary>
        /// Get data context.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Max values of the bounding box.
        /// </summary>
        IPoint Max { get; set; }

        /// <summary>
        /// Min values of the bounding box.
        /// </summary>
        IPoint Min { get; set; }
    }
}

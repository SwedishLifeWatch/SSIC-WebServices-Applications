using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// A set of 2, 4, 6 or 8 numbers indicating the upper and
    /// lower bounds of an interval (1D), rectangle (2D),
    /// parallelpiped (3D), or hypercube along each axis of a
    /// given coordinate reference system.
    /// </summary>
    [Serializable]
    public class BoundingBox : IBoundingBox
    {
        /// <summary>
        /// Create an BoundingBox instance.
        /// </summary>
        public BoundingBox()
        {
        }

        /// <summary>
        /// Create an BoundingBox instance.
        /// </summary>
        /// <param name="max">Max coordinates.</param>
        /// <param name="min">Min coordinates.</param>
        /// <param name="dataContext">Data context.</param>
        public BoundingBox(IPoint max,
                           IPoint min,
                           IDataContext dataContext)
        {
            // Check data.
            dataContext.CheckNotNull("dataContext");

            // Set data.
            DataContext = dataContext;
            Max = max;
            Min = min;
        }

        /// <summary>
        /// Get data context.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Max values of the bounding box.
        /// </summary>
        public IPoint Max { get; set; }

        /// <summary>
        /// Min values of the bounding box.
        /// </summary>
        public IPoint Min { get; set; }
    }
}

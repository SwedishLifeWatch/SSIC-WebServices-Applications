using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    ///  This class represents an region type,
    ///  for example "Political boundary" or "Validate regions".
    /// </summary>
    [Serializable()]
    public class RegionType : IRegionType
    {
        /// <summary>
        /// Create a RegionType instance.
        /// </summary>
        /// <param name='id'>Id for this region type.</param>
        /// <param name='name'>Region type name.</param>
        /// <param name='dataContext'>Data context.</param>
        public RegionType(Int32 id, String name, IDataContext dataContext)
        {
            // Check data.
            dataContext.CheckNotNull("dataContext");

            // Set data.
            DataContext = dataContext;
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; private set; }

        /// <summary>
        /// Id for this region type.
        /// </summary>
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Region type name.
        /// </summary>
        public String Name
        { get; private set; }
    }
}

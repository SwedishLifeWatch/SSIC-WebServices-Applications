using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about a region.
    /// A region can for example represent a county or a municipality.
    /// </summary>
    [Serializable()]
    public class Region : IRegion
    {
        /// <summary>
        /// Create a Region instance.
        /// </summary>
        /// <param name='id'>Id for this region.</param>
        /// <param name="categoryId">Category id for this region.</param>
        /// <param name="guid">Globally unique identifier.</param>
        /// <param name='name'>Region type name.</param>
        /// <param name="nativeId">Native id according to source specified in region category.</param>
        /// <param name="shortName">Short name of region.</param>
        /// <param name="sortOrder">Order when sorting regions</param>
        /// <param name='dataContext'>Data context.</param>
        public Region(Int32 id, Int32 categoryId, String guid, String name, String nativeId,
                      String shortName, Int32 sortOrder, IDataContext dataContext)
        {
            // Check data.
            dataContext.CheckNotNull("dataContext");

            // Set data.
            DataContext = dataContext;
            CategoryId = categoryId;
            GUID = guid;
            Id = id;
            Name = name;
            NativeId = nativeId;
            // Polygons = multiPolygon;
            ShortName = shortName;
            SortOrder = sortOrder;
        }

        /// <summary>
        /// Create an empty Region instance
        /// </summary>
        /// <param name='dataContext'>Data context.</param>
        public Region (IDataContext dataContext) :
                        this(Int32.MinValue,
                        Int32.MinValue,
                        null,
                        null,
                        null,
                        null,
                        Int32.MinValue,
                        dataContext)
        { }

        /// <summary>
        /// Region category id.
        /// </summary>
        public Int32 CategoryId
        { get; private set; }

        /// <summary>
        /// Region category id.
        /// </summary>
        public string CategoryName
        { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; private set; }

        /// <summary>
        /// Globally unique identifier (GUID) implemented according to 
        /// the Life Science Identifier (LSID) resolution protocol.
        /// Handling of this property is not implemented in current
        /// web services.
        /// </summary>
        public String GUID
        { get; private set; }

        /// <summary>
        /// Region id.
        /// </summary>
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Name of the region.
        /// </summary>
        public String Name
        { get; private set; }

        /// <summary>
        /// Native id according to source specified in region category.
        /// </summary>
        public String NativeId
        { get; private set; }

        /// <summary>
        /// A short version of the region name.
        /// </summary>
        public String ShortName
        { get; private set; }

        /// <summary>
        /// Used to sort regions within specified category.
        /// Handling of this property is not implemented in current
        /// web services.
        /// </summary>
        public Int32 SortOrder
        { get; private set; }
    }
}

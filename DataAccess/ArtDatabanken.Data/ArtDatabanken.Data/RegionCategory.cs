using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about an region category.
    /// Examples of region categories are county and municipality.
    /// Most region categories are related to a specific country
    /// </summary>
    [Serializable()]
    public class RegionCategory : IRegionCategory
    {
        /// <summary>
        /// Region Categories enum
        /// The values are the same as in the database.
        /// </summary>
        public enum RegionCategories
        {
            /// <summary>
            /// Municipality
            /// </summary>
            Municipality = 1,

            /// <summary>
            /// Parish
            /// </summary>
            Parish = 11,

            /// <summary>
            /// Province
            /// </summary>
            Province = 16,

            /// <summary>
            /// County
            /// </summary>
            County = 21,
        }

        /// <summary>
        /// Create a RegionCategory instance.
        /// </summary>
        /// <param name='id'>Id for this region type.</param>
        /// <param name="countryIsoCode">Country iso code that this region belongs to</param>
        /// <param name="guid">Globally unique identifier (GUID) implemented according to the Life Science Identifier (LSID) resolution protocol.</param>
        /// <param name="level">Level of the region category</param>
        /// <param name='name'>Region category name.</param>
        /// <param name="nativeSourceId">Used source when native ids are assigned to regions.</param>
        /// <param name="sortOrder">Order when categories are sorted</param>
        /// <param name="typeId">Region type id</param>
        /// <param name='dataContext'>Data context.</param>
        public RegionCategory(Int32 id, Int32? countryIsoCode, String guid, Int32? level, String name,
                              String nativeSourceId, Int32 sortOrder, Int32 typeId, IDataContext dataContext)
        {
            // Check data.
            dataContext.CheckNotNull("dataContext");

            // Set data.
            DataContext = dataContext;
            CountryIsoCode = countryIsoCode;
            GUID = guid;
            Id = id; 
            Level = level;
            Name = name;
            NativeIdSource = nativeSourceId;
            SortOrder = sortOrder;
            TypeId = typeId;
        }

        /// <summary>
        /// Country iso code as specified in standard ISO-3166.
        /// Not all region categories has a country iso code.
        /// </summary>
        public Int32? CountryIsoCode
        { get; private set; }

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
        /// Id for this region category.
        /// </summary>
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Specifies level of the region category according to
        /// some hierarchical order.
        /// Not all region categories has a level value.
        /// </summary>
        public Int32? Level
        { get; private set; }

        /// <summary>
        /// Region category name.
        /// </summary>
        public String Name
        { get; private set; }

        /// <summary>
        /// Used source when native ids are assigned to regions.
        /// An example of source is Statistics Sweden (SCB) that
        /// defines ids for counties, municipalities, etc.
        /// </summary>
        public String NativeIdSource
        { get; private set; }

        /// <summary>
        /// Used to sort region categories.
        /// </summary>
         public Int32 SortOrder
        { get; private set; }

        /// <summary>
        /// Region type id.
        /// </summary>
         public Int32 TypeId
        { get; private set; }
    }
}

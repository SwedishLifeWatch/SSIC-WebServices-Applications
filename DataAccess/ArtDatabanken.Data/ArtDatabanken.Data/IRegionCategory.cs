using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about an region category.
    /// Examples of region categories are county and municipality.
    /// Most region categories are related to a specific country
    /// </summary>
    public interface IRegionCategory : IDataId32
    {
        /// <summary>
        /// Country iso code as specified in standard ISO-3166.
        /// Not all region categories has a country iso code.
        /// </summary>
        Int32? CountryIsoCode
        { get; }

        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext
        { get; }

        /// <summary>
        /// Globally unique identifier (GUID) implemented according to 
        /// the Life Science Identifier (LSID) resolution protocol.
        /// Handling of this property is not implemented in current
        /// web services.
        /// </summary>
        String GUID
        { get; }

        /// <summary>
        /// Specifies level of the region category according to
        /// some hierarchical order.
        /// Not all region categories has a level value.
        /// </summary>
        Int32? Level
        { get; }

        /// <summary>
        /// Region category name.
        /// </summary>
        String Name
        { get; }

        /// <summary>
        /// Used source when native ids are assigned to regions.
        /// An example of source is Statistics Sweden (SCB) that
        /// defines ids for counties, municipalities, etc.
        /// </summary>
        String NativeIdSource
        { get; }

        /// <summary>
        /// Used to sort region categories.
        /// </summary>
        Int32 SortOrder
        { get; }

        /// <summary>
        /// Region type id.
        /// </summary>
        Int32 TypeId
        { get; }
    }
}

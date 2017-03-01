using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface contains information about a region.
    /// A region can for example represent a county or a municipality.
    /// </summary>
    public interface IRegion : IDataId32
    {
        /// <summary>
        /// Region category id.
        /// </summary>
        Int32 CategoryId
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
        { get;  }

        /// <summary>
        /// Name of the region.
        /// </summary>
        String Name
        { get; }

        /// <summary>
        /// Category Name of the region.
        /// </summary>
        String CategoryName
        { get; }

        /// <summary>
        /// Native id according to source specified in region category.
        /// </summary>
        String NativeId
        { get; }

        /// <summary>
        /// A short version of the region name.
        /// </summary>
        String ShortName
        { get; }

        /// <summary>
        /// Used to sort regions within specified category.
        /// Handling of this property is not implemented in current
        /// web services.
        /// </summary>
        Int32 SortOrder
        { get; }
    }
}

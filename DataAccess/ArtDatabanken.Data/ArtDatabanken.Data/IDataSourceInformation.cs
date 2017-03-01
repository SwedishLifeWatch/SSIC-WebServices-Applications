using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Definition of data source types.
    /// </summary>
    public enum DataSourceType
    {
        /// <summary>Database.</summary>
        Database,
        /// <summary>File.</summary>
        File,
        /// <summary>Onion.</summary>
        Onion,
        /// <summary>Web service.</summary>
        WebService
    }

    /// <summary>
    /// Information about the data source for a data object.
    /// </summary>
    public interface IDataSourceInformation
    {
        /// <summary>
        /// Get address to the data source.
        /// </summary>
        String Address
        { get; }

        /// <summary>
        /// Get the data source name.
        /// </summary>
        String Name
        { get; }

        /// <summary>
        /// Get type of data source.
        /// </summary>
        DataSourceType Type
        { get; }
    }
}

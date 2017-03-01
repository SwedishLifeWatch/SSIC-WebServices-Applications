using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Definition of interface that all managers must implement.
    /// </summary>
    public interface IManager
    {
        /// <summary>
        /// Get information about data source.
        /// </summary>
        /// <returns>Information about data source.</returns>
        IDataSourceInformation GetDataSourceInformation();
    }
}

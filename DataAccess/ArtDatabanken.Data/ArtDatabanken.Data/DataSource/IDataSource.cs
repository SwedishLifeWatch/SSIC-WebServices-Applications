namespace ArtDatabanken.Data.DataSource
{
    /// <summary>
    /// Definition of interface that all data source managers
    /// must implement.
    /// </summary>
    public interface IDataSource
    {
        /// <summary>
        /// Get information about data source.
        /// </summary>
        /// <returns>Information about data source.</returns>
        IDataSourceInformation GetDataSourceInformation();
    }
}

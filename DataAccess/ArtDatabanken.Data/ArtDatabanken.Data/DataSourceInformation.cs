using System;
using System.Reflection;
using ArtDatabanken;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about the data source for a data object.
    /// </summary>
    [Serializable()]
    public class DataSourceInformation : IDataSourceInformation
    {
        /// <summary>
        /// Create a DataSourceInformation instance that is used
        /// for data that has not yet been stored anywhere.
        /// </summary>
        public DataSourceInformation()
        {
            // Set data.
            Address = Assembly.GetExecutingAssembly().GetApplicationName() + " " +
                      Assembly.GetExecutingAssembly().GetApplicationVersion();
            Name = Settings.Default.OnionDataSourceName;
            Type = DataSourceType.Onion;
        }

        /// <summary>
        /// Create a DataSource instance.
        /// </summary>
        /// <param name='name'>Data source name.</param>
        /// <param name='address'>Address to the data source.</param>
        /// <param name='type'>Type of data source.</param>
        public DataSourceInformation(String name,
                                     String address,
                                     DataSourceType type)
        {
            // Check data.
            address.CheckNotEmpty("address");
            name.CheckNotEmpty("name");

            // Set data.
            Address = address;
            Name = name;
            Type = type;
        }

        /// <summary>
        /// Get address to the data source.
        /// </summary>
        public String Address
        { get; private set; }

        /// <summary>
        /// Get data source name.
        /// </summary>
        public String Name
        { get; private set; }

        /// <summary>
        /// Get type of data source.
        /// </summary>
        public DataSourceType Type
        { get; private set; }
    }
}

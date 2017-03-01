using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Meta information about a data object.
    /// </summary>
    [Serializable()]
    public class DataContext : IDataContext
    {
        private IDataSourceInformation _dataSource;

        /// <summary>
        /// Create an empty DataContext.
        /// </summary>
        /// <param name="userContext">User context.</param>
        public DataContext(IUserContext userContext)
            : this(new DataSourceInformation(), userContext.Locale)
        {
        }

        /// <summary>
        /// Create a DataContext instance.
        /// </summary>
        /// <param name='dataSource'>Data source.</param>
        /// <param name='locale'>Locale.</param>
        public DataContext(IDataSourceInformation dataSource, ILocale locale)
        {
            // Set data
            DataSource = dataSource;
            IsChanged = false;
            Locale = locale;
        }

        /// <summary>
        /// Information about the data source.
        /// </summary>
        public IDataSourceInformation DataSource
        {
            get
            {
                return _dataSource;
            }
            set
            {
                value.CheckNotNull("_dataSource");
                _dataSource = value;
            }
        }

        /// <summary>
        /// Indicates if data in the related data object has been changed.
        /// </summary>
        public Boolean IsChanged
        { get; set; }

        /// <summary>
        /// The related data object was retrieved with this locale set.
        /// Can be null if the data handling is independent of locale.
        /// </summary>
        public ILocale Locale
        { get; private set; }
    }
}

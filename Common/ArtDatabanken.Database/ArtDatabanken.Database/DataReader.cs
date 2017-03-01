using System;
using System.Collections;
using System.Data.SqlClient;
using System.IO;
using Microsoft.SqlServer.Types;

namespace ArtDatabanken.Database
{
    /// <summary>
    /// Decorator class for the SqlDataReader class.
    /// DataReader makes it easier to retrieve values.
    ///
    /// Make sure to close the data reader after usage.
    /// The data reader can be closed by a call to
    /// Close() or Dispose().
    /// It is also possible to use implicit closing
    /// with the help of the "using" directive.
    /// <example> Example pseudo code:
    /// <code>
    ///     using (DataReader dataReader = DataServer.GetDataReader())
    ///     {
    ///         // Use data reader to read information from database.
    ///     }
    /// </code>
    /// </example>
    /// </summary>
    public class DataReader : IDisposable
    {
        private Boolean _isRowRead;
        private Boolean[] _readColumns;
        private Hashtable _columnInformation;
        private Int32 _unreadColumnIndex;
        private SqlDataReader _dataReader;

        /// <summary>
        /// Constructor for the DataReader class.
        /// </summary>
        /// <param name='dataReader'>An open SqlDataReader object.</param>
        public DataReader(SqlDataReader dataReader)
        {
            _dataReader = dataReader;
            ColumnNamePrefix = String.Empty;
            _isRowRead = false;
            _readColumns = null;
            _unreadColumnIndex = -1;
            GetColumnInformation();
        }

        /// <summary>
        /// Handle column name prefix.
        /// The column name prefix is added to all 
        /// column names when retrieving information
        /// in this DataReader instance.
        /// It is used if information about multiple
        /// objects are returned in the same row.
        /// <example>
        /// If ColumnNamePrefix has the value "Database" and
        /// columnName has the value "Id" then the search
        /// column name would be "DatabaseId".
        /// </example>
        /// </summary>
        public String ColumnNamePrefix { get; set; }

        /// <summary>
        /// Close the data reader.
        /// </summary>
        public void Close()
        {
            if (_dataReader.IsNotNull())
            {
                if (IsOpen())
                {
                    _dataReader.Close();
                }

                _dataReader = null;
            }
        }

        /// <summary>
        /// Implementation of the IDisposable interface.
        /// Close the data reader.
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        /// <summary>
        /// Get a Boolean value.
        /// </summary>
        /// <param name="columnIndex">Index of the column where the value is located.</param>
        /// <returns>The Boolean value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the index "columnIndex"</exception>
        public Boolean GetBoolean(Int32 columnIndex)
        {
            return _dataReader.GetBoolean(columnIndex);
        }

        /// <summary>
        /// Get a Boolean value.
        /// </summary>
        /// <param name="columnIndex">Index of the column where the value is located.</param>
        /// <returns>The Boolean value.</returns>
        /// <param name="defaultValue">Value to use if the database value is null.</param>
        /// <exception cref="ArgumentException">Thrown if no column has the index "columnIndex"</exception>
        public Boolean GetBoolean(Int32 columnIndex, Boolean defaultValue)
        {
            if (_dataReader.IsDBNull(columnIndex))
            {
                return defaultValue;
            }
            else
            {
                return _dataReader.GetBoolean(columnIndex);
            }
        }

        /// <summary>
        /// Get a Boolean value.
        /// </summary>
        /// <param name="columnName">Name of the column where the value is located.</param>
        /// <returns>The Boolean value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public Boolean GetBoolean(String columnName)
        {
            return _dataReader.GetBoolean(GetColumnIndex(columnName, true));
        }

        /// <summary>
        /// Get a Boolean value.
        /// </summary>
        /// <param name="columnName">Name of the column where the value is located.</param>
        /// <param name="defaultValue">Value to use if the database value is null.</param>
        /// <returns>The Boolean value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public Boolean GetBoolean(String columnName, Boolean defaultValue)
        {
            return GetBoolean(GetColumnIndex(columnName, true), defaultValue);
        }

        /// <summary>
        /// Get an Byte value.
        /// </summary>
        /// <param name="columnIndex">Index of the column where the value is located.</param>
        /// <returns>The Byte value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the index "columnIndex"</exception>
        public Byte GetByte(Int32 columnIndex)
        {
            return _dataReader.GetByte(columnIndex);
        }

        /// <summary>
        /// Get an Byte value.
        /// </summary>
        /// <param name="columnIndex">Index of the column where the value is located.</param>
        /// <param name="defaultValue">Value to use if the database value is null.</param>
        /// <returns>The Byte value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the index "columnIndex"</exception>
        public Byte GetByte(Int32 columnIndex, Byte defaultValue)
        {
            if (_dataReader.IsDBNull(columnIndex))
            {
                return defaultValue;
            }
            else
            {
                return _dataReader.GetByte(columnIndex);
            }
        }

        /// <summary>
        /// Get an Byte value.
        /// </summary>
        /// <param name="columnName">Name of the column where the value is located.</param>
        /// <returns>The Byte value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public Byte GetByte(String columnName)
        {
            return _dataReader.GetByte(GetColumnIndex(columnName, true));
        }

        /// <summary>
        /// Get an Byte value.
        /// </summary>
        /// <param name="columnName">Name of the column where the value is located.</param>
        /// <param name="defaultValue">Value to use if the database value is null.</param>
        /// <returns>The Byte value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public Byte GetByte(String columnName, Byte defaultValue)
        {
            return GetByte(GetColumnIndex(columnName, true), defaultValue);
        }

        /// <summary>
        /// Get index for a column based on its name.
        /// </summary>
        /// <param name="columnName">Name of the column to get index for.</param>
        /// <returns>The column index.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        private Int32 GetColumnIndex(String columnName)
        {
            columnName = ColumnNamePrefix + columnName;
            if (_columnInformation.ContainsKey(columnName))
            {
                return (Int32)(_columnInformation[columnName]);
            }
            else
            {
                throw new ArgumentException("No column with name " + columnName);
            }
        }

        /// <summary>
        /// Get index for a column based on its name.
        /// </summary>
        /// <param name="columnName">Name of the column to get index for.</param>
        /// <param name="isReadingData">Indicates if data is read from the column.</param>
        /// <returns>The column index.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        private Int32 GetColumnIndex(String columnName,
                                     Boolean isReadingData)
        {
            Int32 columnIndex;

            columnIndex = GetColumnIndex(columnName);
            if (isReadingData)
            {
                _readColumns[columnIndex] = true;
            }

            return columnIndex;
        }

        /// <summary>
        /// Create mapping between column index and column name.
        /// </summary>
        private void GetColumnInformation()
        {
            Int32 columnIndex;

            _columnInformation = new Hashtable();
            for (columnIndex = 0; columnIndex < _dataReader.FieldCount; columnIndex++)
            {
                _columnInformation.Add(_dataReader.GetName(columnIndex), columnIndex);
            }
        }

        /// <summary>
        /// Get a DateTime value.
        /// </summary>
        /// <param name="columnIndex">Index of the column where the value is located.</param>
        /// <returns>The DateTime value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the index "columnIndex"</exception>
        public DateTime GetDateTime(Int32 columnIndex)
        {
            return _dataReader.GetDateTime(columnIndex);
        }

        /// <summary>
        /// Get a DateTime value.
        /// </summary>
        /// <param name="columnIndex">Index of the column where the value is located.</param>
        /// <param name="defaultValue">Value to use if the database value is null.</param>
        /// <returns>The DateTime value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the index "columnIndex"</exception>
        public DateTime GetDateTime(Int32 columnIndex, DateTime defaultValue)
        {
            if (_dataReader.IsDBNull(columnIndex))
            {
                return defaultValue;
            }
            else
            {
                return _dataReader.GetDateTime(columnIndex);
            }
        }

        /// <summary>
        /// Get a DateTime value.
        /// </summary>
        /// <param name="columnName">Name of the column where the value is located.</param>
        /// <returns>The DateTime value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public DateTime GetDateTime(String columnName)
        {
            return _dataReader.GetDateTime(GetColumnIndex(columnName, true));
        }

        /// <summary>
        /// Get a DateTime value.
        /// </summary>
        /// <param name="columnName">Name of the column where the value is located.</param>
        /// <param name="defaultValue">Value to use if the database value is null.</param>
        /// <returns>The DateTime value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public DateTime GetDateTime(String columnName, DateTime defaultValue)
        {
            return GetDateTime(GetColumnIndex(columnName, true), defaultValue);
        }

        /// <summary>
        /// Get a Double value.
        /// </summary>
        /// <param name="columnIndex">Index of the column where the value is located.</param>
        /// <returns>The Double value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public Double GetDouble(Int32 columnIndex)
        {
            return _dataReader.GetDouble(columnIndex);
        }

        /// <summary>
        /// Get a Double value.
        /// </summary>
        /// <param name="columnIndex">Index of the column where the value is located.</param>
        /// <param name="defaultValue">Value to use if the database value is null.</param>
        /// <returns>The Double value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the index "columnIndex"</exception>
        public Double GetDouble(Int32 columnIndex, Double defaultValue)
        {
            if (_dataReader.IsDBNull(columnIndex))
            {
                return defaultValue;
            }
            else
            {
                return _dataReader.GetDouble(columnIndex);
            }
        }

        /// <summary>
        /// Get a Double value.
        /// </summary>
        /// <param name="columnName">Name of the column where the value is located.</param>
        /// <returns>The Double value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public Double GetDouble(String columnName)
        {
            return _dataReader.GetDouble(GetColumnIndex(columnName, true));
        }

        /// <summary>
        /// Get a Double value.
        /// </summary>
        /// <param name="columnName">Name of the column where the value is located.</param>
        /// <param name="defaultValue">Value to use if the database value is null.</param>
        /// <returns>The Double value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public Double GetDouble(String columnName, Double defaultValue)
        {
            return GetDouble(GetColumnIndex(columnName, true), defaultValue);
        }

        /// <summary>
        /// Get an Int16 value.
        /// </summary>
        /// <param name="columnIndex">Index of the column where the value is located.</param>
        /// <returns>The Int16 value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public Int16 GetInt16(Int32 columnIndex)
        {
            return _dataReader.GetInt16(columnIndex);
        }

        /// <summary>
        /// Get a Int16 value.
        /// </summary>
        /// <param name="columnIndex">Index of the column where the value is located.</param>
        /// <param name="defaultValue">Value to use if the database value is null.</param>
        /// <returns>The Int16 value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the index "columnIndex"</exception>
        public Int16 GetInt16(Int32 columnIndex, Int16 defaultValue)
        {
            if (_dataReader.IsDBNull(columnIndex))
            {
                return defaultValue;
            }
            else
            {
                return _dataReader.GetInt16(columnIndex);
            }
        }

        /// <summary>
        /// Get an Int16 value.
        /// </summary>
        /// <param name="columnName">Name of the column where the value is located.</param>
        /// <returns>The Int16 value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public Int16 GetInt16(String columnName)
        {
            return _dataReader.GetInt16(GetColumnIndex(columnName, true));
        }

        /// <summary>
        /// Get an Int16 value.
        /// </summary>
        /// <param name="columnName">Name of the column where the value is located.</param>
        /// <param name="defaultValue">Value to use if the database value is null.</param>
        /// <returns>The Int16 value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public Int16 GetInt16(String columnName, Int16 defaultValue)
        {
            return GetInt16(GetColumnIndex(columnName, true), defaultValue);
        }

        /// <summary>
        /// Get an Int32 value.
        /// </summary>
        /// <param name="columnIndex">Index of the column where the value is located.</param>
        /// <returns>The Int32 value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the index "columnIndex"</exception>
        public Int32 GetInt32(Int32 columnIndex)
        {
            return _dataReader.GetInt32(columnIndex);
        }

        /// <summary>
        /// Get an Int32 value.
        /// </summary>
        /// <param name="columnIndex">Index of the column where the value is located.</param>
        /// <param name="defaultValue">Value to use if the database value is null.</param>
        /// <returns>The Int32 value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the index "columnIndex"</exception>
        public Int32 GetInt32(Int32 columnIndex, Int32 defaultValue)
        {
            if (_dataReader.IsDBNull(columnIndex))
            {
                return defaultValue;
            }
            else
            {
                return _dataReader.GetInt32(columnIndex);
            }
        }

        /// <summary>
        /// Get an Int32 value.
        /// </summary>
        /// <param name="columnName">Name of the column where the value is located.</param>
        /// <returns>The Int32 value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public Int32 GetInt32(String columnName)
        {
            return _dataReader.GetInt32(GetColumnIndex(columnName, true));
        }

        /// <summary>
        /// Get an Int32 value.
        /// </summary>
        /// <param name="columnName">Name of the column where the value is located.</param>
        /// <param name="defaultValue">Value to use if the database value is null.</param>
        /// <returns>The Int32 value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public Int32 GetInt32(String columnName, Int32 defaultValue)
        {
            return GetInt32(GetColumnIndex(columnName, true), defaultValue);
        }

        /// <summary>
        /// Get an Int32 value or null if the value is null in database.
        /// </summary>
        /// <param name="columnName">Name of the column where the value is located.</param>
        /// <returns>The Int32 value or null.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public Int32? GetNullableInt32(String columnName)
        {
            if (IsNotDbNull(columnName))
            {
                return GetInt32(columnName);
            }
            return null;
        }


        /// <summary>
        /// Get an Int64 value.
        /// </summary>
        /// <param name="columnIndex">Index of the column where the value is located.</param>
        /// <returns>The Int64 value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the index "columnIndex"</exception>
        public Int64 GetInt64(Int32 columnIndex)
        {
            return _dataReader.GetInt64(columnIndex);
        }

        /// <summary>
        /// Get an Int64 value.
        /// </summary>
        /// <param name="columnIndex">Index of the column where the value is located.</param>
        /// <returns>The Int64 value.</returns>
        /// <param name="defaultValue">Value to use if the database value is null.</param>
        /// <exception cref="ArgumentException">Thrown if no column has the index "columnIndex"</exception>
        public Int64 GetInt64(Int32 columnIndex, Int64 defaultValue)
        {
            if (_dataReader.IsDBNull(columnIndex))
            {
                return defaultValue;
            }
            else
            {
                return _dataReader.GetInt64(columnIndex);
            }
        }

        /// <summary>
        /// Get an Int64 value.
        /// </summary>
        /// <param name="columnName">Name of the column where the value is located.</param>
        /// <returns>The Int64 value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public Int64 GetInt64(String columnName)
        {
            return _dataReader.GetInt64(GetColumnIndex(columnName, true));
        }

        /// <summary>
        /// Get an Int64 value.
        /// </summary>
        /// <param name="columnName">Name of the column where the value is located.</param>
        /// <param name="defaultValue">Value to use if the database value is null.</param>
        /// <returns>The Int64 value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public Int64 GetInt64(String columnName, Int64 defaultValue)
        {
            return GetInt64(GetColumnIndex(columnName, true), defaultValue);
        }

        /// <summary>
        /// Get a Geometry value.
        /// </summary>
        /// <param name="columnIndex">Index of the column where the value is located.</param>
        /// <returns>The Geometry value.</returns>
        public SqlGeometry GetSqlGeometry(Int32 columnIndex)
        {
            if (_dataReader.IsDBNull(columnIndex))
            {
                return null;
            }
            else
            {
                SqlGeometry g = new SqlGeometry();
                g.Read(new BinaryReader(_dataReader.GetSqlBytes(columnIndex).Stream));
                return g;
            }
        }

        /// <summary>
        /// Get a Geometry value.
        /// </summary>
        /// <param name="columnName">Name of the column where the value is located.</param>
        /// <returns>The Geometry value.</returns>
        public SqlGeometry GetSqlGeometry(String columnName)
        {
            return GetSqlGeometry(GetColumnIndex(columnName, true));
        }

        /// <summary>
        /// Get a String value.
        /// </summary>
        /// <param name="columnIndex">Index of the column where the value is located.</param>
        /// <returns>The String value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public String GetString(Int32 columnIndex)
        {
            if (_dataReader.IsDBNull(columnIndex))
            {
                return null;
            }
            else
            {
                return _dataReader.GetString(columnIndex);
            }
        }

        /// <summary>
        /// Get a String value.
        /// </summary>
        /// <param name="columnIndex">Index of the column where the value is located.</param>
        /// <param name="defaultValue">Value to use if the database value is null.</param>
        /// <returns>The String value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public String GetString(Int32 columnIndex, String defaultValue)
        {
            if (_dataReader.IsDBNull(columnIndex))
            {
                return defaultValue;
            }
            else
            {
                return _dataReader.GetString(columnIndex);
            }
        }

        /// <summary>
        /// Get a String value.
        /// </summary>
        /// <param name="columnName">Name of the column where the value is located.</param>
        /// <returns>The String value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public String GetString(String columnName)
        {
            return GetString(GetColumnIndex(columnName, true));
        }

        /// <summary>
        /// Get a String value.
        /// </summary>
        /// <param name="columnName">Name of the column where the value is located.</param>
        /// <param name="defaultValue">Value to use if the database value is null.</param>
        /// <returns>The String value.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public String GetString(String columnName, String defaultValue)
        {
            return GetString(GetColumnIndex(columnName, true), defaultValue);
        }

        /// <summary>
        /// Get the name of the current unread column.
        /// </summary>
        /// <returns>Column name.</returns>
        public String GetUnreadColumnName()
        {
            return _dataReader.GetName(_unreadColumnIndex);
        }

        /// <summary>
        /// Get the data type of the current unread column.
        /// </summary>
        /// <returns>Data type.</returns>
        public Type GetUnreadColumnType()
        {
            return _dataReader.GetFieldType(_unreadColumnIndex);
        }

        /// <summary>
        /// Test if the result set has a column with the specified name.
        /// </summary>
        /// <param name="columnName">Name of the column where the value is located.</param>
        /// <returns>True if result set has a column with the specified name.</returns>
        public Boolean HasColumn(String columnName)
        {
            return _columnInformation.ContainsKey(ColumnNamePrefix + columnName);
        }

        /// <summary>
        /// Test if value from the datebase is null.
        /// </summary>
        /// <param name="columIndex">Index of the column where the value is located.</param>
        /// <returns>True if value is null.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public Boolean IsDbNull(Int32 columIndex)
        {
            return _dataReader.IsDBNull(columIndex);
        }

        /// <summary>
        /// Test if value from the datebase is null.
        /// </summary>
        /// <param name="columnName">Name of the column where the value is located.</param>
        /// <returns>True if value is null.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public Boolean IsDbNull(String columnName)
        {
            return _dataReader.IsDBNull(GetColumnIndex(columnName));
        }

        /// <summary>
        /// Test if value from the datebase is not null.
        /// </summary>
        /// <param name="columnIndex">Index of the column where the value is located.</param>
        /// <returns>True if value is not null.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public Boolean IsNotDbNull(Int32 columnIndex)
        {
            return !_dataReader.IsDBNull(columnIndex);
        }
        /// <summary>
        /// Test if value from the datebase is not null.
        /// </summary>
        /// <param name="columnName">Name of the column where the value is located.</param>
        /// <returns>True if value is not null.</returns>
        /// <exception cref="ArgumentException">Thrown if no column has the name "columnName"</exception>
        public Boolean IsNotDbNull(String columnName)
        {
            return !_dataReader.IsDBNull(GetColumnIndex(columnName));
        }

        /// <summary>
        /// Test if data reader is open.
        /// </summary>
        /// <returns>True if data reader is open.</returns>
        private Boolean IsOpen()
        {
            return (_dataReader.IsNotNull() && !_dataReader.IsClosed);
        }

        /// <summary>
        /// Move the data reader to the next result set.
        /// </summary>
        /// <returns>True if there is another result set.</returns>
        public Boolean NextResultSet()
        {
            Boolean hasAnotherResultSet;

            hasAnotherResultSet = false;
            _isRowRead = false;
            _readColumns = null;
            if (IsOpen())
            {
                hasAnotherResultSet = _dataReader.NextResult();
                if (hasAnotherResultSet)
                {
                    GetColumnInformation();
                }
            }
            return hasAnotherResultSet;
        }

        /// <summary>
        /// Move the data reader to the next column that has not been read.
        /// </summary>
        /// <returns>True if there is another unread column.</returns>
        public Boolean NextUnreadColumn()
        {
            Boolean hasUnreadColumn;
            Int32 columnIndex;

            hasUnreadColumn = false;
            _unreadColumnIndex = -1;

            // Get the next unread column.
            if (IsOpen() && _isRowRead)
            {
                for (columnIndex = 0; columnIndex < _dataReader.FieldCount; columnIndex++)
                {
                    if (!_readColumns[columnIndex])
                    {
                        _unreadColumnIndex = columnIndex;
                        hasUnreadColumn = true;
                        _readColumns[columnIndex] = true;
                        break;
                    }
                }
            }
            return hasUnreadColumn;
        }

        /// <summary>
        /// Move the data reader to the next row.
        /// </summary>
        /// <returns>True if there is another row.</returns>
        public Boolean Read()
        {
            Int32 columnIndex;

            _isRowRead = false;
            if (IsOpen())
            {
                _isRowRead = _dataReader.Read();
                if (_isRowRead)
                {
                    _readColumns = new Boolean[_dataReader.FieldCount];
                    for (columnIndex = 0; columnIndex < _dataReader.FieldCount; columnIndex++)
                    {
                        _readColumns[columnIndex] = false;
                    }
                }
            }

            return _isRowRead;
        }
    }
}

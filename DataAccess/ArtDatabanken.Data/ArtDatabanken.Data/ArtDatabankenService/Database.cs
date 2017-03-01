using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  This class handles information about a database.
    /// </summary>
    [Serializable()]
    public class Database : DataId
    {
        private String _longName;
        private String _shortName;

        /// <summary>
        /// Create a Database instance.
        /// </summary>
        /// <param name='id'>Id for database.</param>
        /// <param name='longName'>Long name for database.</param>
        /// <param name='shortName'>Short name for database.</param>
        public Database(Int32 id, String longName, String shortName)
            : base(id)
        {
            _longName = longName;
            _shortName = shortName;
        }

        /// <summary>
        /// Get long name for this taxon type.
        /// </summary>
        public String LongName
        {
            get { return _longName; }
        }

        /// <summary>
        /// Get short name for this taxon type.
        /// </summary>
        public String ShortName
        {
            get { return _shortName; }
        }
    }
}

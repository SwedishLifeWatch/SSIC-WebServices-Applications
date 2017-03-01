using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Database
{
    /// <summary>
    /// Used for grouping a unique set of field values, combined to a key, and their individual count and also the document count.
    /// </summary>
    public class DocumentUniqueValue
    {
        /// <summary>
        /// Combined unique key
        /// </summary>
        public string Key;

        /// <summary>
        /// Value count for the unique key
        /// </summary>
        public long Count;

        /// <summary>
        /// Document count for the unique key
        /// </summary>
        public long DocumentCount;
    }
}

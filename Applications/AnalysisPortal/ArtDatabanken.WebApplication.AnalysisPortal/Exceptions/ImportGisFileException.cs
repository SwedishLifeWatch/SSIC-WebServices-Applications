using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Exceptions
{
    /// <summary>
    /// Import GIS file exception.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class ImportGisFileException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportGisFileException"/> class.
        /// </summary>
        public ImportGisFileException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportGisFileException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ImportGisFileException(string message)
        : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportGisFileException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The inner exception.</param>
        public ImportGisFileException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
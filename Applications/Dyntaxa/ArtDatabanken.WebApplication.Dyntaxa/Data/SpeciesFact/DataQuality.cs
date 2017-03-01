using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// Specifies possible values of the Data Quality.
    /// </summary>
    public enum DataQuality
    {
        /// <summary>
        /// Data quality not evaluated.
        /// </summary>
        NotEvaluated = 474,

        /// <summary>
        /// Low data quality.
        /// </summary>
        Low = 475,

        /// <summary>
        /// Acceptable data quality.
        /// </summary>
        Acceptable = 476,

        /// <summary>
        /// High data quality.
        /// </summary>
        High = 477
    }
}

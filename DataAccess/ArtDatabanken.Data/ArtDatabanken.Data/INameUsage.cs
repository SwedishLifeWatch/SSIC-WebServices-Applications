// -----------------------------------------------------------------------
// <copyright file="INameUsage.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ArtDatabanken.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface INameUsage : IDataId
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets NameString.
        /// </summary>
        string NameString { get; set; }

        /// <summary>
        /// Gets or sets DescriptionString.
        /// </summary>       
        string DescriptionString { get; set; }
    }
}

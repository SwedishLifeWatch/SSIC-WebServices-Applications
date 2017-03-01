// -----------------------------------------------------------------------
// <copyright file="NameUsage.cs" company="Microsoft">
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
    public class NameUsage : INameUsage
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary      
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets NameString.
        /// </summary>
        public string NameString { get; set; }

        /// <summary>
        /// Gets or sets DescriptionString.
        /// </summary>
        public string DescriptionString { get; set; }
    }
}

// -----------------------------------------------------------------------
// <copyright file="IReferenceRealtion.cs" company="Microsoft">
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
    public interface IReferenceRealtion
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets RelatedObjectGuid.
        /// </summary>
        string RelatedObjectGuid { get; set; }

        /// <summary>
        /// Gets or sets ReferenceId.
        /// </summary>
        int ReferenceId { get; set; }
    }
}

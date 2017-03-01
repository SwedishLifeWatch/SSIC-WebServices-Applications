using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Reference
{
    /// <summary>
    /// Specifies how references should be assigned.
    /// </summary>
    public enum ReferenceApplyMode
    {
        /// <summary>
        /// Only the selected taxon reference is affected.
        /// </summary>
        OnlySelected,

        /// <summary>
        /// The selected taxon reference is affected, and the references are added to all underlying taxa.
        /// </summary>
        AddToUnderlyingTaxa,

        /// <summary>
        /// The selected taxon reference is affected, and the references are replaced for all underlying taxa.
        /// </summary>
        ReplaceUnderlyingTaxa,

        /// <summary>
        /// The selected taxon reference is affected, and the source references are replaced for all underlying taxa.
        /// </summary>
        ReplaceOnlySourceInUnderlyingTaxa
    }
}

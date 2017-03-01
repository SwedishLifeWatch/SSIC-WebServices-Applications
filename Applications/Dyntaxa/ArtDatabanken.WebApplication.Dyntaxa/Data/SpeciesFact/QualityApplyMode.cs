using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// Specifies how the update should be performed.
    /// </summary>
    public enum QualityApplyMode
    {
        /// <summary>
        /// Only the selected taxon's quality declaration is affected.
        /// </summary>
        OnlySelected,

        /// <summary>
        /// The selected taxon's quality declaration is affected, and the quality declaration are added to all underlying taxa.
        /// </summary>
        AddToAllUnderlyingTaxa,

        /// <summary>
        /// The selected taxon's quality declaration is affected, and the quality declaration are added to all underlying taxa, except the ones that already have a quality declaration.
        /// </summary>
        AddToUnderlyingTaxaExceptWhereAlreadyDeclared,

        /// <summary>
        /// The selected taxon's quality declaration is affected, and the quality declaration are added to all underlying taxa, except the ones that already have a quality declaration of a higher degree.
        /// </summary>
        AddToUnderlyingTaxaExceptWhereAlreadyDeclaredHigher
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the TaxonTreeNode class.
    /// </summary>
    public class TaxonTreeNodeList : DataIdList
    {
        /// <summary>
        /// Get Taxon with specified id.
        /// </summary>
        /// <param name='taxonId'>Id of taxon belonging to requested taxon tree node.</param>
        /// <returns>Requested taxon tree node.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public TaxonTreeNode Get(Int32 taxonId)
        {
            return (TaxonTreeNode)(GetById(taxonId));
        }

        /// <summary>
        /// Get/set TaxonTreeNode by list index.
        /// </summary>
        public new TaxonTreeNode this[Int32 index]
        {
            get
            {
                return (TaxonTreeNode)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}

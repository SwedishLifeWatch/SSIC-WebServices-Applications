using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the TaxonType class.
    /// </summary>
    [Serializable]
    public class TaxonTypeList : DataIdList
    {
        /// <summary>
        /// Constructor for the TaxonTypeList class.
        /// </summary>
        public TaxonTypeList()
            : this(true)
        {
        }

        /// <summary>
        /// Constructor for the TaxonTypeList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public TaxonTypeList(Boolean optimize)
            : base(optimize)
        {
        }

        /// <summary>
        /// Get TaxonType with specified id.
        /// </summary>
        /// <param name='taxonTypeId'>Id of requested taxon type.</param>
        /// <returns>Requested taxon type.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public TaxonType Get(Int32 taxonTypeId)
        {
            return (TaxonType)(GetById(taxonTypeId));
        }

        /// <summary>
        /// Get/set TaxonType by list index.
        /// </summary>
        public new TaxonType this[Int32 index]
        {
            get
            {
                return (TaxonType)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}

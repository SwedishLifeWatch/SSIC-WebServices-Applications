using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the TaxonNameType class.
    /// </summary>
    [Serializable]
    public class TaxonNameTypeList : DataIdList
    {
        /// <summary>
        /// Constructor for the TaxonNameTypeList class.
        /// </summary>
        public TaxonNameTypeList()
            : this(true)
        {
        }

        /// <summary>
        /// Constructor for the TaxonNameTypeList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public TaxonNameTypeList(Boolean optimize)
            : base(optimize)
        {
        }

        /// <summary>
        /// Get TaxonNameType with specified id.
        /// </summary>
        /// <param name='taxonNameTypeId'>Id of requested taxon name type.</param>
        /// <returns>Requested taxon name type.</returns>
        /// <exception cref="ArgumentException">Thrown if no taxon name type has the requested id.</exception>
        public TaxonNameType Get(Int32 taxonNameTypeId)
        {
            return (TaxonNameType)(GetById(taxonNameTypeId));
        }

        /// <summary>
        /// Get/set TaxonNameType by list index.
        /// </summary>
        public new TaxonNameType this[Int32 index]
        {
            get
            {
                return (TaxonNameType)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}

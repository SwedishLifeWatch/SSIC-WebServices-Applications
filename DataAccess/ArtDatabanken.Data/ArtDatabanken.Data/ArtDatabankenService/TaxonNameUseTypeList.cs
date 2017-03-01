using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the TaxonNameUseType class.
    /// </summary>
    [Serializable]
    public class TaxonNameUseTypeList : DataIdList
    {
        /// <summary>
        /// Constructor for the TaxonNameUseTypeList class.
        /// </summary>
        public TaxonNameUseTypeList()
            : this(true)
        {
        }

        /// <summary>
        /// Constructor for the TaxonNameUseTypeList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public TaxonNameUseTypeList(Boolean optimize)
            : base(optimize)
        {
        }

        /// <summary>
        /// Get TaxonNameUseType with specified id.
        /// </summary>
        /// <param name='taxonNameUseTypeId'>Id of requested taxon name use type.</param>
        /// <returns>Requested taxon name use type.</returns>
        /// <exception cref="ArgumentException">Thrown if no taxon name use type has the requested id.</exception>
        public TaxonNameUseType Get(Int32 taxonNameUseTypeId)
        {
            return (TaxonNameUseType)(GetById(taxonNameUseTypeId));
        }

        /// <summary>
        /// Get/set TaxonNameUseType by list index.
        /// </summary>
        public new TaxonNameUseType this[Int32 index]
        {
            get
            {
                return (TaxonNameUseType)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}

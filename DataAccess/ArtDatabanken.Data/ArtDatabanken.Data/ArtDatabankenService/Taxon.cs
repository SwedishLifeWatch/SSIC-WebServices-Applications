using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  Enum that contains taxon ids.
    /// </summary>
    public enum TaxonId
    {
        /// <summary>Dummy = 0</summary>
        Dummy = 0
    }

    /// <summary>
    ///  This class represents a taxon.
    /// </summary>
    [Serializable]
    public class Taxon : DataSortOrder, IListableItem
    {
        private String _author;
        private String _commonName;
        private String _scientificName;
        private TaxonInformationType _taxonInformationType;
        private TaxonNameList _taxonNames;
        private TaxonType _taxonType;

        /// <summary>
        /// Create a Taxon instance.
        /// </summary>
        /// <param name='id'>Id for taxon type.</param>
        /// <param name='taxonTypeId'>Id for type of taxon.</param>
        /// <param name='sortOrder'>Sort order among taxa.</param>
        /// <param name="taxonInformationType">Type of taxon information in this object.</param>
        /// <param name="scientificName">Scientific name for this taxon.</param>
        /// <param name="author">Author of the scientific name for this taxon.</param>
        /// <param name="commonName">Common name for this taxon.</param>
        public Taxon(Int32 id,
                     Int32 taxonTypeId,
                     Int32 sortOrder,
                     TaxonInformationType taxonInformationType,
                     String scientificName,
                     String author,
                     String commonName)
            : base(id, sortOrder)
        {
            _taxonType = TaxonManager.GetTaxonType(taxonTypeId);
            _taxonInformationType = taxonInformationType;
            _scientificName = scientificName;
            _author = author;
            _commonName = commonName;
            _taxonNames = null;
        }

        /// <summary>
        /// Get author of the scientific name for this taxon.
        /// </summary>
        public String Author
        {
            get { return _author; }
        }

        /// <summary>
        /// Get common name for this taxon.
        /// </summary>
        public String CommonName
        {
            get { return _commonName; }
            //set { _commonName = value; }
        }

        /// <summary>
        /// Get scientific name for this taxon.
        /// </summary>
        public String ScientificName
        {
            get { return _scientificName; }
            //set { _scientificName = value; }
        }

        /// <summary>
        /// Get scientific name and author for this taxon.
        /// </summary>
        public String ScientificNameAndAuthor
        {
            get
            {
                String scientificNameAndAuthor;

                scientificNameAndAuthor = _scientificName + " " + _author;
                return scientificNameAndAuthor.Trim();
            }
        }

        /// <summary>
        /// Get all taxon names.
        /// </summary>
        public TaxonNameList TaxonNames
        {
            get
            {
                if (_taxonNames.IsNull())
                {
                    _taxonNames = TaxonManager.GetTaxonNames(Id);
                }
                return _taxonNames;
            }
        }

        /// <summary>
        /// Get type of taxon.
        /// </summary>
        public TaxonType TaxonType
        {
            get { return _taxonType; }
        }

        /// <summary>
        /// Get type of taxon information in this object.
        /// </summary>
        public TaxonInformationType TaxonInformationType
        {
            get { return _taxonInformationType; }
        }

        #region IListableItem Members
        /// <summary>
        /// A string usable as a display name
        /// </summary>
        public string Label
        {
            get
            {
                StringBuilder str = new StringBuilder();
                str.Append(_scientificName);
                if ((_author != null) && (_author.Length > 0))
                {
                    str.Append(" " + _author);
                }

                if ((_commonName != null) && (_commonName.Length > 0))
                {
                    str.Append(", " + _commonName);
                }
                return str.ToString();

            }
        }

        #endregion
    }
}

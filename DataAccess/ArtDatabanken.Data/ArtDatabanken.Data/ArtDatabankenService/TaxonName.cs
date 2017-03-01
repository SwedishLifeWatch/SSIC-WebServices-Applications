using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  This class represents a taxon name.
    /// </summary>
    [Serializable]
    public class TaxonName : DataId, IListableItem
    {
        private const String DUMMY_TAXON_NAME = "DUMMY";

        private Boolean _isRecommended;
        private Int32 _taxonId;
        private String _author;
        private String _name;
        private Taxon _taxon;
        private TaxonNameType _taxonNameType;
        private TaxonNameUseType _taxonNameUseType;

        /// <summary>
        /// Create a TaxonName instance.
        /// </summary>
        /// <param name='id'>Id for taxon name.</param>
        /// <param name='taxonId'>Id for the taxon that this name belongs to.</param>
        /// <param name='taxonNameTypeId'>Id for type of name.</param>
        /// <param name='taxonNameUseTypeId'>Id for use of name.</param>
        /// <param name='name'>Name for taxon name.</param>
        /// <param name='author'>Author of this taxon name.</param>
        /// <param name='isRecommended'>Information about if this is the recommended name.</param>
        public TaxonName(Int32 id,
                         Int32 taxonId,
                         Int32 taxonNameTypeId,
                         Int32 taxonNameUseTypeId,
                         String name,
                         String author,
                         Boolean isRecommended)
            : base(id)
        {
            _taxonId = taxonId;
            _taxonNameType = TaxonManager.GetTaxonNameType(taxonNameTypeId);
            _taxonNameUseType = TaxonManager.GetTaxonNameUseType(taxonNameUseTypeId);
            _name = name;
            _author = author;
            _taxon = null;
            _isRecommended = isRecommended;
        }

        /// <summary>
        /// Get author of this taxon name.
        /// </summary>
        public String Author
        {
            get { return _author; }
        }

        /// <summary>
        /// Test if this name is the recommended name for the taxon
        /// in combination with taxon name type.
        /// </summary>
        public Boolean IsRecommended
        {
            get { return _isRecommended; }
        }

        /// <summary>
        /// Get name for this taxon name.
        /// </summary>
        public String Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Get taxon that this name belongs to.
        /// </summary>
        public Taxon Taxon
        {
            get
            {
                if (_taxon.IsNull())
                {
                    _taxon = TaxonManager.GetTaxon(_taxonId, TaxonInformationType.Basic);
                }
                return _taxon;
            }
            set
            {
                if (_taxon.IsNull() && (_taxonId == value.Id))
                {
                    _taxon = value;
                }
            }
        }

        /// <summary>
        /// Get id for the taxon that this name belongs to.
        /// </summary>
        public Int32 TaxonId
        {
            get { return _taxonId; }
        }

        /// <summary>
        /// Get type of name.
        /// </summary>
        public TaxonNameType TaxonNameType
        {
            get { return _taxonNameType; }
        }

        /// <summary>
        /// Get use of name.
        /// </summary>
        public TaxonNameUseType TaxonNameUseType
        {
            get { return _taxonNameUseType; }
        }

        #region IListableItem Members

        /// <summary>
        /// A string usable as a display name
        /// </summary>
        public string Label
        {
            get { return _name; }
        }

        #endregion

        /// <summary>
        /// Test if a taxon name is a dummy.
        /// </summary>
        /// <param name="taxonName">Taxon name to test.</param>
        /// <returns>True if the taxon name is a dummy.</returns>
        public static Boolean IsDummyTaxonName(String taxonName)
        {
            return (taxonName.IsNotEmpty() &&
                    (taxonName.Length >= DUMMY_TAXON_NAME.Length) &&
                    (taxonName.Substring(0, DUMMY_TAXON_NAME.Length).ToUpper() == DUMMY_TAXON_NAME));
        }
    }
}

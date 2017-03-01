using System;
using System.Collections.Generic;
using System.Text;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class holds taxa information that is shown
    /// in the PrintObs applikation.
    /// </summary>
    [Serializable]
    public class TaxonPrintObs : Taxon
    {
        /// <summary>
        /// Name of the phylum data field in WebTaxon.
        /// </summary>
        public const String PHYLUM_DATA_FIELD = "Phylum";
        /// <summary>
        /// Name of the class data field in WebTaxon.
        /// </summary>
        public const String CLASS_DATA_FIELD = "Class";
        /// <summary>
        /// Name of the order data field in WebTaxon.
        /// </summary>
        public const String ORDER_DATA_FIELD = "Order";
        /// <summary>
        /// Name of the class family field in WebTaxon.
        /// </summary>
        public const String FAMILY_DATA_FIELD = "Family";

        private String _className;
        private String _familyName;
        private String _orderName;
        private String _phylumName;

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
        /// <param name="phylumName">Phylum name for this taxon.</param>
        /// <param name="className">Class name for this taxon.</param>
        /// <param name="orderName">Order name for this taxon.</param>
        /// <param name="familyName">Family name for this taxon.</param>
        public TaxonPrintObs(Int32 id,
                             Int32 taxonTypeId,
                             Int32 sortOrder,
                             TaxonInformationType taxonInformationType,
                             String scientificName,
                             String author,
                             String commonName,
                             String phylumName,
                             String className,
                             String orderName,
                             String familyName)
            : base(id,
                   taxonTypeId,
                   sortOrder,
                   taxonInformationType,
                   scientificName,
                   author,
                   commonName)
        {
            _phylumName = phylumName;
            _className = className;
            _orderName = orderName;
            _familyName = familyName;
        }

        /// <summary>
        /// Get name for the class that this taxon belongs to.
        /// </summary>
        public String ClassName
        {
            get { return _className; }
        }

        /// <summary>
        /// Get name for the family that this taxon belongs to.
        /// </summary>
        public String FamilyName
        {
            get { return _familyName; }
        }

        /// <summary>
        /// Get name for the order that this taxon belongs to.
        /// </summary>
        public String OrderName
        {
            get { return _orderName; }
        }

        /// <summary>
        /// Get name for the phylum that this taxon belongs to.
        /// </summary>
        public String PhylumName
        {
            get { return _phylumName; }
        }
    }
}

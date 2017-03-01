using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [DataContract]
    public class WebTaxonProperties : WebData
    {
        /// <summary>
        /// Gets or sets AlertStatus.
        /// </summary>
        [DataMember]
        public Int32 AlertStatusId { get; set; }

        /// <summary>
        /// Gets or sets ChangedInTaxonRevisionEvent.
        /// </summary>
        [DataMember]
        public WebTaxonRevisionEvent ChangedInTaxonRevisionEvent { get; set; }

        /// <summary>
        /// Full concept definition for a taxon. 
        /// Not required ie could be null.
        /// </summary>
        [DataMember]
        public String ConceptDefinition { get; set; }

        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Gets or sets IsPublished.
        /// </summary>
        [DataMember]
        public Boolean IsPublished { get; set; }

        /// <summary>
        /// Gets or sets IsValid
        /// </summary>
        [DataMember]
        public Boolean IsValid { get; set; }

        /// <summary>
        /// Gets or sets ModifiedBy.
        /// </summary>
        [DataMember]
        public WebUser ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets ModifiedDate.
        /// </summary>
        [DataMember]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Part of concept definition for a taxon. 
        /// Not required ie could be null.
        /// </summary>
        [DataMember]
        public String PartOfConceptDefinition { get; set; }

        /// <summary>
        /// Gets or sets ReplacedInTaxonRevisionEvent.
        /// </summary>
        [DataMember]
        public WebTaxonRevisionEvent ReplacedInTaxonRevisionEvent { get; set; }

        /// <summary>
        /// Gets or sets Taxon.
        /// </summary>
        [DataMember]
        public WebTaxon Taxon { get; set; }

        /// <summary>
        /// Gets or sets TaxonCategory.
        /// </summary>
        [DataMember]
        public WebTaxonCategory TaxonCategory { get; set; }

        /// <summary>
        /// Gets or sets ValidFromDate.
        /// </summary>
        [DataMember]
        public DateTime ValidFromDate { get; set; }

        /// <summary>
        /// Gets or sets ValidToDate.
        /// </summary>
        [DataMember]
        public DateTime ValidToDate { get; set; }
    }
}

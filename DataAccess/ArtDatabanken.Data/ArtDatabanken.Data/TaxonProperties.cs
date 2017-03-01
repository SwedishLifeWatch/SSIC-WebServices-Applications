using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TaxonProperties : ITaxonProperties
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Gets or sets Taxon.
        /// </summary>
        public ITaxon Taxon { get; set; }

        /// <summary>
        /// Gets or sets TaxonCategory.
        /// </summary>
        public ITaxonCategory TaxonCategory { get; set; }

        /// <summary>
        /// Part of concept definition for a taxon. 
        /// Not required ie could be null.
        /// </summary>
        public string PartOfConceptDefinition
        { get; set; }

        /// <summary>
        /// Full concept definition for a taxon. 
        /// Not required ie could be null.
        /// </summary>
        public string ConceptDefinition
        { get; set; }

        /// <summary>
        /// Gets or sets AlertStatus.
        /// </summary>
        public TaxonAlertStatusId AlertStatus
        { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Gets or sets ValidFromDate.
        /// </summary>
        public DateTime ValidFromDate { get; set; }

        /// <summary>
        /// Gets or sets ValidToDate.
        /// </summary>
        public DateTime ValidToDate { get; set; }

        /// <summary>
        /// Gets or sets IsValid.
        /// </summary>
        public Boolean IsValid { get; set; }

        /// <summary>
        /// Gets or sets ModifiedDate.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets ModifiedBy.
        /// </summary>
        public IUser ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets PersonName.
        /// </summary>
        public String ModifiedByPerson { get; set; }

        /// <summary>
        /// Gets or sets RevisionEvent.
        /// </summary>
        public ITaxonRevisionEvent ChangedInTaxonRevisionEvent { get; set; }

        /// <summary>
        /// Gets or sets ChangedInRevisionEvent.
        /// </summary>
        public ITaxonRevisionEvent ReplacedInTaxonRevisionEvent { get; set; }

        /// <summary>
        /// Gets or sets IsPublished.
        /// </summary>
        public Boolean IsPublished { get; set; }

        /// <summary>
        /// Indicates that a specie is a microspecies
        /// </summary>
        public Boolean IsMicrospecies { get; set; }
    }
}

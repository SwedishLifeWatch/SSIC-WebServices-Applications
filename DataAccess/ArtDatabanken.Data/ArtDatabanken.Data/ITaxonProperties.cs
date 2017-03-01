using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITaxonProperties : IDataId32
    {
        /// <summary>
        /// Gets or sets AlertStatus.
        /// </summary>
        TaxonAlertStatusId AlertStatus { get; set; }

        /// <summary>
        /// Gets or sets RevisionEvent.
        /// </summary>
        ITaxonRevisionEvent ChangedInTaxonRevisionEvent { get; set; }

        /// <summary>
        /// Full concept definition for a taxon. 
        /// Not required ie could be null.
        /// </summary>
        String ConceptDefinition { get; set; }

        /// <summary>
        /// Data context with meta information about this object.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Gets or sets IsPublished.
        /// </summary>
        Boolean IsPublished { get; set; }

        /// <summary>
        /// Gets or sets IsValid.
        /// </summary>
        Boolean IsValid { get; set; }

        /// <summary>
        /// Gets or sets ModifiedBy.
        /// </summary>
        IUser ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets PersonName.
        /// </summary>
        String ModifiedByPerson { get; set; }

        /// <summary>
        /// Gets or sets ModifiedDate.
        /// </summary>
        DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Part of concept definition for a taxon. 
        /// Not required ie could be null.
        /// </summary>
        String PartOfConceptDefinition { get; set; }

        /// <summary>
        /// Gets or sets ChangedInRevisionEvent.
        /// </summary>
        ITaxonRevisionEvent ReplacedInTaxonRevisionEvent { get; set; }

        /// <summary>
        /// Gets or sets Taxon.
        /// </summary>
        ITaxon Taxon { get; set; }

        /// <summary>
        /// Gets or sets TaxonCategory.
        /// </summary>
        ITaxonCategory TaxonCategory { get; set; }

        /// <summary>
        /// Gets or sets ValidFromDate.
        /// </summary>
        DateTime ValidFromDate { get; set; }

        /// <summary>
        /// Gets or sets ValidToDate.
        /// </summary>
        DateTime ValidToDate { get; set; }

        /// <summary>
        /// Indicates that a specie is a microspecies
        /// </summary>
        Boolean IsMicrospecies { get; set; }
    }
}

using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Each taxon relation connects a parent taxon with a child taxon.
    /// Current taxon tree is defined by all currently
    /// valid taxon relations.
    /// </summary>
    public class TaxonRelation : Trackable, ITaxonRelation
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Parent taxon in this taxon relation.
        /// </summary>
        public ITaxon ParentTaxon { get; set; }

        /// <summary>
        /// Child taxon in this taxon relation.
        /// </summary>
        public ITaxon ChildTaxon { get; set; }

        /// <summary>
        /// Gets or sets IsMainRelation.
        /// </summary>
        public Boolean IsMainRelation { get; set; }

        /// <summary>
        /// Gets or sets ValidFromDate.
        /// </summary>
        public DateTime ValidFromDate { get; set; }

        /// <summary>
        /// Gets or sets ValidToDate.
        /// </summary>
        public DateTime ValidToDate { get; set; }

        /// <summary>
        /// Gets or sets LumpSplitEvent.
        /// </summary>
        public Int32? ChangedInTaxonRevisionEventId { get; set; }

        /// <summary>
        /// Gets or sets ChangedInRevisionEvent.
        /// </summary>
        public Int32? ReplacedInTaxonRevisionEventId { get; set; }

        /// <summary>
        /// Gets or sets IsPublished.
        /// </summary>
        public Boolean IsPublished { get; set; }

        /// <summary>
        /// Gets or sets SortOrder.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets PersonName.
        /// </summary>
        public string ModifiedByPerson { get; set; }

        /// <summary>
        /// Gets or sets CreatedBy.
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets CreatedDate.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Gets or sets ModifiedBy.
        /// </summary>
        public Int32 ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets ModifiedDate.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets GUID.
        /// </summary>
        public String Guid { get; set; }


        /// <summary>
        /// Determines whether the specified taxon relation
        /// is equal to the current taxon relation.
        /// </summary>
        /// <param name="other">Taxon relation object.</param>
        /// <returns>
        /// True if the specified taxon relation is equal to
        /// the current taxon relation, otherwise false.
        /// </returns>
        public bool Equals(TaxonRelation other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id == Id;
        }

        /// <summary>
        /// Determines whether the specified taxon relation
        /// is equal to the current taxon.
        /// </summary>
        /// <param name="obj">Taxon object.</param>
        /// <returns>
        /// True if the specified taxon relation is equal to
        /// the current taxon, otherwise false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(TaxonRelation)) return false;
            return Equals((TaxonRelation) obj);
        }

        /// <summary>
        /// Returns the hash code for this taxon relation.
        /// </summary>
        /// <returns>
        /// A Int32 containing the hash code for this taxon relation.
        /// </returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if(ParentTaxon != null && ChildTaxon != null)
            {
                return string.Format("{0}[{1}] => {2}[{3}] TaxonTreeId: {4}, MainRelation: {5}, Valid: {6}", ParentTaxon.ScientificName, ParentTaxon.Id, ChildTaxon.ScientificName, ChildTaxon.Id, Id, IsMainRelation ? "Yes" : "No", DateTime.Now > ValidFromDate && DateTime.Now < ValidToDate ? "Yes" : "No");
            }
            return base.ToString();
        }
    }
}

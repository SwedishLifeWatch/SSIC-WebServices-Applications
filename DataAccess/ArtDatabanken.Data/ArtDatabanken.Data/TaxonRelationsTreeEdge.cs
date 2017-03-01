using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Taxon relation tree edge.
    /// </summary>
    public class TaxonRelationsTreeEdge : ITaxonRelationsTreeEdge
    {
        /// <summary>
        /// Creates a new taxon relation tree edge.
        /// </summary>
        /// <param name="parent">The parent node.</param>
        /// <param name="child">The child node.</param>
        /// <param name="taxonRelation">The taxon relation.</param>
        public TaxonRelationsTreeEdge(ITaxonRelationsTreeNode parent, ITaxonRelationsTreeNode child, ITaxonRelation taxonRelation)
        {
            Parent = parent;
            Child = child;
            TaxonRelation = taxonRelation;
        }

        /// <summary>
        /// Parent node.
        /// </summary>
        public ITaxonRelationsTreeNode Parent { get; set; }
        
        /// <summary>
        /// Child node.
        /// </summary>
        public ITaxonRelationsTreeNode Child { get; set; }

        /// <summary>
        /// Taxon relation.
        /// </summary>
        public ITaxonRelation TaxonRelation { get; set; }        

        /// <summary>
        /// Gets a value indicating whether this relation is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this relation is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid
        {
            get
            {
                DateTime today = DateTime.Now;
                return TaxonRelation.ValidFromDate <= today && today <= TaxonRelation.ValidToDate;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this relation is main relation.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this relation is main relation; otherwise, <c>false</c>.
        /// </value>
        public bool IsMain
        {
            get { return TaxonRelation.IsMainRelation; }
        }

        /// <summary>
        /// Gets a value indicating whether this relation is published.
        /// </summary>
        /// <value>
        /// <c>true</c> if this relation is published; otherwise, <c>false</c>.
        /// </value>
        public bool IsPublished
        {
            get { return TaxonRelation.IsPublished; }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                "{0} => {1}, IsMain: {2}, IsValid: {3}",
                Parent,
                Child,
                IsMain,
                IsValid);
        }

        /// <summary>
        /// Determines whether the specified <see cref="TaxonRelationsTreeEdge" />, is equal to this instance.        
        /// </summary>
        /// <param name="other">The <see cref="TaxonRelationsTreeEdge" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="TaxonRelationsTreeEdge" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        protected bool Equals(ITaxonRelationsTreeEdge other)
        {
            return Equals(TaxonRelation.Id, other.TaxonRelation.Id);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((ITaxonRelationsTreeEdge)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return TaxonRelation != null ? TaxonRelation.Id.GetHashCode() : 0;
        }
    }
}
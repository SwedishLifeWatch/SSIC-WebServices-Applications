using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Taxon
{
    /// <summary>
    /// This class is a view model for a search result item
    /// </summary>
    public class TaxonSearchResultItemViewModel
    {
        public int TaxonId { get; set; }
        public string ScientificName { get; set; }
        public string CommonName { get; set; }
        public string SearchMatchName { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public string NameCategory { get; set; }
        public TaxonAlertStatusId TaxonStatus { get; set; }

        /// <summary>
        /// Creates a search result item from an ITaxonName.
        /// </summary>
        /// <param name="taxonName">Taxon name object.</param>
        /// <returns></returns>
        public static TaxonSearchResultItemViewModel CreateFromTaxonName(ITaxonName taxonName)
        {
            var model = new TaxonSearchResultItemViewModel();
            ITaxon taxon = taxonName.Taxon;
            model.NameCategory = taxonName.Category.Name;
            model.Author = taxon.Author.IsNotEmpty() ? taxon.Author : "";
            model.TaxonId = taxonName.Taxon.Id;
            model.SearchMatchName = taxonName.Name;
            model.ScientificName = taxon.ScientificName.IsNotEmpty() ? taxon.ScientificName : "";
            model.CommonName = taxon.CommonName.IsNotEmpty() ? taxon.CommonName : "";
            model.Category = taxon.Category != null ? taxon.Category.Name : "";
            model.TaxonStatus = (TaxonAlertStatusId)taxon.AlertStatus.Id;
            return model;
        }

        /// <summary>
        /// Creates a search result item from a Taxon object.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        /// <returns></returns>
        public static TaxonSearchResultItemViewModel CreateFromTaxon(ITaxon taxon)
        {
            var model = new TaxonSearchResultItemViewModel();
            model.NameCategory = "TaxonId";
            model.Author = taxon.Author.IsNotEmpty() ? taxon.Author : "";
            model.TaxonId = taxon.Id;
            model.SearchMatchName = taxon.Id.ToString();
            model.ScientificName = taxon.ScientificName.IsNotEmpty() ? taxon.ScientificName : "";
            model.CommonName = taxon.CommonName.IsNotEmpty() ? taxon.CommonName : "";
            model.Category = taxon.Category != null ? taxon.Category.Name : "";
            model.TaxonStatus = (TaxonAlertStatusId)taxon.AlertStatus.Id;
            return model;
        }

        protected bool Equals(TaxonSearchResultItemViewModel other)
        {
            return TaxonId == other.TaxonId && string.Equals(ScientificName, other.ScientificName) && string.Equals(CommonName, other.CommonName) && string.Equals(SearchMatchName, other.SearchMatchName) && string.Equals(Category, other.Category) && string.Equals(Author, other.Author) && string.Equals(NameCategory, other.NameCategory) && TaxonStatus == other.TaxonStatus;
        }

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

            return Equals((TaxonSearchResultItemViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = TaxonId;
                hashCode = (hashCode * 397) ^ (ScientificName != null ? ScientificName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CommonName != null ? CommonName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (SearchMatchName != null ? SearchMatchName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Category != null ? Category.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Author != null ? Author.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (NameCategory != null ? NameCategory.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)TaxonStatus;
                return hashCode;
            }
        }

        public static bool operator ==(TaxonSearchResultItemViewModel left, TaxonSearchResultItemViewModel right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TaxonSearchResultItemViewModel left, TaxonSearchResultItemViewModel right)
        {
            return !Equals(left, right);
        }
    }
}

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// This class represent host data
    /// </summary>
    public class DyntaxaHost
    {
        string label = string.Empty;
        string id = string.Empty;
        string commonName = string.Empty;
        string scientificName = string.Empty;
        int factorId = 0;
        int individualCategory = 0;
        string individualCategoryName = string.Empty;

        public DyntaxaHost(string id, string label, string scientificName, string commonName, int factorId = 0)
        {
            this.id = id;
            this.label = label;
            this.scientificName = scientificName;
            this.commonName = commonName;
            this.factorId = factorId;
        }

        public string Label
        {
            get
            {
                return label;
            }
        }

        public string Id
        {
            get
            {
                return id;
            }
        }

        public string CommonName
        {
            get
            {
                return commonName;
            }
        }
        public string ScientificName
        {
            get
            {
                return scientificName;
            }
        }

        public string IndividualCategoryName
        {
            get
            {
                return individualCategoryName;
            }
            set
            {
                individualCategoryName = value;
            }
        }

        public int IndividualCategory
        {
            get
            {
                return individualCategory;
            }
            set
            {
                individualCategory = value;
            }
        }

        public int FactorId
        {
            get
            {
                return factorId;
            }
        }
    }
}
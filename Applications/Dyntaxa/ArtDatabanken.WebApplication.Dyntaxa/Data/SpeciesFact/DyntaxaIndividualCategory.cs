namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// This class represent individual categoy data.
    /// </summary>
    public class DyntaxaIndividualCategory
    {
        string label = string.Empty;
        int id = 0;
        string factorValue = string.Empty;

        public DyntaxaIndividualCategory(int id, string label, string factorValue)
        {
            this.id = id;
            this.label = label;
            this.factorValue = factorValue;
        }

        public string Label
        {
            get
            {
                return label;
            }
            set
            {
                label = value;
            }
        }

        public string FactorValue
        {
            get
            {
                return factorValue;
            }
            set
            {
                factorValue = value;
            }
        }

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
    }
}
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// This class represent period data
    /// </summary>
    public class DyntaxaPeriod
    {
        string label = string.Empty;
        string id = string.Empty;

        public DyntaxaPeriod(string id, string label)
        {
            this.id = id;
            this.label = label;
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

        public string Id
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
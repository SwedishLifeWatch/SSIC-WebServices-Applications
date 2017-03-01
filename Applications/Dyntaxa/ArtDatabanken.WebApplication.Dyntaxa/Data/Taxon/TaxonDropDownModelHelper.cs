namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public class TaxonDropDownModelHelper
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public TaxonDropDownModelHelper(int id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}
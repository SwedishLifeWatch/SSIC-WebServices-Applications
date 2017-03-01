namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public class SpeciesFactDropDownModelHelper
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Selectable { get; set; }
        public bool IsSubHeader { get; set; }

        public SpeciesFactDropDownModelHelper(int id, string text, bool selectable = true, bool isSubHeader = false)
        {
            Id = id;
            Text = text;
            Selectable = selectable;
            IsSubHeader = isSubHeader;
        }
    }
}
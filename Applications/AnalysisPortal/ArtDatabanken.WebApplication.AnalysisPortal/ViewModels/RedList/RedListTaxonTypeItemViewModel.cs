namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    public class RedListTaxonTypeItemViewModel
    {
        /// <summary>
        /// RedList Taxon Type id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// RedList Taxon Type custom class definition value.
        /// </summary>
        public string ClassDefinitionTextValue { get; set; }

        /// <summary>
        /// RedList Taxon Type name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if RedList Taxon Type should be used in search criteria.
        /// </summary>
        public bool Selected { get; set; }
    }
}

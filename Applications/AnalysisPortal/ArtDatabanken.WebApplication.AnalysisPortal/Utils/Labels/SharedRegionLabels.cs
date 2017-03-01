namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Labels
{
    public sealed class SharedRegionLabels
    {
        private static readonly SharedRegionLabels instance = new SharedRegionLabels();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static SharedRegionLabels()
        {
        }

        private SharedRegionLabels()
        {
        }

        public static SharedRegionLabels Instance
        {
            get
            {
                return instance;
            }
        }

        public string Municipality { get { return Resources.Resource.SharedRegionCategoryMunicipality; } }
        public string Province { get { return Resources.Resource.SharedRegionCategoryProvince; } }
        public string County { get { return Resources.Resource.SharedRegionCategoryCounty; } }
        public string GroupOfProvences { get { return Resources.Resource.SharedRegionCategoryGroupOfProvinces; } }
    }
}
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Table;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Presentation
{
    public class PresentationTableTypeViewModel
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public PageInfo PageInfo { get; set; }
        public bool IsSelected { get; set; }
        public bool HasSettings { get; set; }
    }

    public class PresentationTableViewModel
    {
        public List<int> SelectedTableTypes { get; set; }
        public List<PresentationTableTypeViewModel> Tables { get; set; }

        public bool IsSettingsDefault { get; set; }
        public string SelectedSpeciesObservationTableName { get; set; }
        //public SpeciesObservationTableTypeViewModel SelectedSpeciesObservationTable { get; set; }
        public IEnumerable<PresentationTableTypeViewModel> GetSelectedTables()
        {
            return Tables.Where(table => table.IsSelected);
        }
    }
}

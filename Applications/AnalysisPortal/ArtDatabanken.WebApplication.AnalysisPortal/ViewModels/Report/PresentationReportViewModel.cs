using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Report
{
    public class PresentationReportTypeViewModel
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public PageInfo PageInfo { get; set; }
        public bool IsSelected { get; set; }
    }

    public class PresentationReportViewModel
    {
        public List<int> SelectedReportTypes { get; set; }
        public List<PresentationReportTypeViewModel> Reports { get; set; }

        public IEnumerable<PresentationReportTypeViewModel> GetSelectedReports()
        {
            return Reports.Where(report => report.IsSelected);
        }

        /// <summary>
        /// Gets the model labels.
        /// </summary>
        public ModelLabels Labels
        {
            get
            {
                if (_labels == null)
                {
                    _labels = new ModelLabels();
                }

                return _labels;
            }
        }
        private ModelLabels _labels;

        /// <summary>
        /// Localized labels
        /// </summary>
        public class ModelLabels
        {
            public string TitleLabel { get { return Resources.Resource.FilterTaxonFromIdsTitle; } }
            public string SearchLabel { get { return Resources.Resource.SharedSearch; } }
        }
    }
}

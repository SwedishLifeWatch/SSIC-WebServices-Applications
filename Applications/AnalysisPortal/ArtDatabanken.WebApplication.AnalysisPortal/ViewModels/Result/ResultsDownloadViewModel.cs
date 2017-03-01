using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Diagrams;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Maps;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Reports;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Tables;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result
{
    /// <summary>
    /// This class is a View model for showing many results
    /// </summary>
    public class ResultsDownloadViewModel
    {
        private readonly List<ResultGroupBase> _resultGroups = new List<ResultGroupBase>();

        // private readonly MapsResultGroup _mapsResultGroup;
        private readonly TablesResultGroup _tablesResultGroup;
        private readonly ReportsResultGroup _reportsResultGroup;
        private readonly ResultGroupType? _expandedResultGroup;

        // private DiagramsResultGroup _diagramsResultGroup;        

        public IEnumerable<ResultGroupBase> GetActiveResultGroups()
        {
            return _resultGroups.Where(resultGroup => resultGroup.HasActiveItems());
        }

        public List<ResultGroupBase> ResultGroups
        {
            get { return _resultGroups; }
        }        

        // public MapsResultGroup MapResultGroup
        // {
        //    get { return _mapsResultGroup; }
        // }

        public TablesResultGroup TablesResultGroup
        {
            get { return _tablesResultGroup; }
        }

        public ReportsResultGroup ReportsResultGroup
        {
            get { return _reportsResultGroup; }
        }

        //public DiagramsResultGroup DiagramsResultGroup
        //{
        //    get { return _diagramsResultGroup; }
        //}

        public ResultGroupType? ExpandedResultGroup
        {
            get { return _expandedResultGroup; }
        }
        
        public ResultsDownloadViewModel()
            : this(null)
        {
        }

        public ResultsDownloadViewModel(ResultGroupType? expandedResultGroup)
        {
            _expandedResultGroup = expandedResultGroup;

            // _mapsResultGroup = new MapsResultGroup();
            _tablesResultGroup = new TablesResultGroup();
            
            // _diagramsResultGroup = new DiagramsResultGroup();
            _reportsResultGroup = new ReportsResultGroup();            
            
            // _resultGroups.Add(_mapsResultGroup);
            _resultGroups.Add(_tablesResultGroup);
            
            // _resultGroups.Add(_diagramsResultGroup);
            _resultGroups.Add(_reportsResultGroup);

            if (expandedResultGroup.HasValue)
            {
                foreach (var resultGroupBase in ResultGroups)
                {
                    resultGroupBase.IsExpanded = resultGroupBase.ResultGroupType == expandedResultGroup.Value;
                }
            }           
        }

        public bool IsPageActive(PageInfo pageInfo)
        {
            foreach (ResultGroupBase resultGroup in ResultGroups)
            {
                foreach (ResultViewBase item in resultGroup.GetActiveItems())
                {
                    if (item.StaticPageInfo.Action == pageInfo.Action && item.StaticPageInfo.Controller == pageInfo.Controller)
                    {
                        return true;
                    }                    
                }
            }

            return false;
        }
    }   
}

//using System.Collections.Generic;
//using System.Linq;
//using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
//using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews;
//using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Maps;
//using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Reports;
//using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Tables;

//namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result
//{
//    /// <summary>
//    /// This class is a View model for showing many results
//    /// </summary>
//    public class ResultGridTableViewModel
//    {
//        private readonly List<ResultGroupBase> _resultGridGroups = new List<ResultGroupBase>();

//        public IEnumerable<ResultGroupBase> GetActiveResultGroups()
//        {
//            return _resultGridGroups.Where(resultGroup => resultGroup.HasActiveItems());
//        }

//        public List<ResultGroupBase> ResultGroups
//        {
//            get { return _resultGridGroups; }
//        }        

//        public ResultGridTableViewModel()
//        {
//            _resultGridGroups.Add(new GridTablesResultGroup());
            
//        }

//        public bool IsPageActive(PageInfo pageInfo)
//        {
//            foreach (ResultGroupBase resultGroup in ResultGroups)
//            {
//                foreach (ResultViewBase item in resultGroup.GetActiveItems())
//                {
//                    if (item.PageInfo.Action == pageInfo.Action && item.PageInfo.Controller == pageInfo.Controller)
//                    {
//                        return true;
//                    }                    
//                }
//            }
//            return false;
//        }
//    }   
//}

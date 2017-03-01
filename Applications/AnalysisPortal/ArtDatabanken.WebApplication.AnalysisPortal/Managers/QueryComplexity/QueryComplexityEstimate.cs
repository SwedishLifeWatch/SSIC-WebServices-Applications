using System;
using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers.QueryComplexity
{
    /// <summary>
    /// This class represents a complexity estimation of a query.    
    /// </summary>
    public class QueryComplexityEstimate
    {
        private readonly QueryComplexityDescription _complexityDescription = new QueryComplexityDescription();

        /// <summary>
        /// An enum value that describes if the query is estimated to be
        /// slow, medium or fast.
        /// </summary>        
        public QueryComplexityExecutionTime QueryComplexityExecutionTime { get; set; }        

        /// <summary>
        /// Gets a description with information about why
        /// the query is slow/fast.
        /// </summary>        
        public QueryComplexityDescription ComplexityDescription
        {
            get { return _complexityDescription; }
        }

        public TimeSpan EstimatedProcessTime { get; set; }
        public long EstimatedDataSize { get; set; }
    }

    /// <summary>
    /// This class describes why a query is estimated to be
    /// slow/medium/fast to execute. 
    /// It also stores suggestions to more appropriate result views
    /// which are faster to execute.
    /// </summary>
    public class QueryComplexityDescription
    {
        private List<ResultViewBase> _suggestedResultViews;
        public string Title { get; set; }
        public string Text { get; set; }
        public List<ResultViewBase> SuggestedResultViews
        {
            get
            {
                if (_suggestedResultViews == null)
                {
                    _suggestedResultViews = new List<ResultViewBase>();
                }

                return _suggestedResultViews;
            }            
        }

        public QueryComplexityDescription()
        {
            Title = Resources.Resource.QueryComplexityTitle;
            Text = Resources.Resource.QueryComplexityText;
        }
    }
}
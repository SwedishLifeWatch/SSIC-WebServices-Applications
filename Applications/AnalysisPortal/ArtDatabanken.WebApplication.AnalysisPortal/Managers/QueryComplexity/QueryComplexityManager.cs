//using ArtDatabanken.Data;
//using ArtDatabanken.WebApplication.AnalysisPortal.Result;
//using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation;

//namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers.QueryComplexity
//{

//    /// <summary>
//    /// This class is used to get a complexity estimation of a query.
//    /// If the query is slow we can warn the user and suggest a more 
//    /// appropriate result view.
//    /// </summary>
//    public static class QueryComplexityManager
//    {

//        /// <summary>
//        /// Gets a query complexity estimate.
//        /// </summary>
//        /// <param name="resultType">The result type.</param>
//        /// <param name="userContext">The user context.</param>
//        /// <param name="mySettings">The MySettings object.</param>
//        /// <returns></returns>
//        public static QueryComplexityEstimate GetQueryComplexityEstimate(ResultType resultType, IUserContext userContext, MySettings.MySettings mySettings)
//        {
//            IResultCalculator resultCalculator = ResultCalculationFactory.CreateResultCalculator(resultType, userContext, mySettings);
//            return resultCalculator.GetQueryComplexityEstimate();
//        }
//    }    
//}
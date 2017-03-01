using System;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation
{
    /// <summary>
    /// This class is the base class for all result calculator classes.
    /// </summary>    
    public abstract class ResultCalculatorBase
    {
        /// <summary>
        /// The user context used in calculations.
        /// </summary>        
        public IUserContext UserContext { get; set; }

        /// <summary>
        /// The settings to use when calculating the result.
        /// </summary>
        public MySettings.MySettings MySettings { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultCalculatorBase"/> class.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="mySettings">The settings to use when calculating the result.</param>
        protected ResultCalculatorBase(IUserContext userContext, MySettings.MySettings mySettings)
        {
            UserContext = userContext;
            MySettings = mySettings;
        }

        public CalculatedDataItem<T> GetCacheCalculatedDataItem<T>(CalculatedDataItemType calculatedDataItemType)
        {
            return CalculatedDataItemCacheManager.GetCalculatedDataItemEx<T>(calculatedDataItemType, MySettings);
        }

        public CalculatedDataItem<T> GetCacheCalculatedDataItem<T>(CalculatedDataItemType calculatedDataItemType, string localeISOCode)
        {
            return CalculatedDataItemCacheManager.GetCalculatedDataItemEx<T>(calculatedDataItemType, MySettings, localeISOCode);
        }

        protected bool TryGetCachedCalculatedResult<T>(
            CalculatedDataItemType calculatedDataItemType,
            out T result)
        {
            CalculatedDataItem<T> calculatedDataItem;

            MySettings.ResultCacheNeedsRefresh = false; // this works because we clear the cache when a setting is changed in MySettings. Perhaps change later...
            calculatedDataItem = GetCacheCalculatedDataItem<T>(calculatedDataItemType);
            if (calculatedDataItem.HasFreshData)
            {
                result = calculatedDataItem.Data;
                return true;
            }

            result = default(T);
            return false;
        }

        protected bool TryGetCachedCalculatedResult<T>(
            CalculatedDataItemType calculatedDataItemType,
            string localeISOCode,
            out T result)
        {
            CalculatedDataItem<T> calculatedDataItem;

            MySettings.ResultCacheNeedsRefresh = false; // this works because we clear the cache when a setting is changed in MySettings. Perhaps change later...
            calculatedDataItem = GetCacheCalculatedDataItem<T>(calculatedDataItemType, localeISOCode);
            if (calculatedDataItem.HasFreshData)
            {
                result = calculatedDataItem.Data;
                return true;
            }

            result = default(T);
            return false;
        }

        protected bool TryGetPrecalculatedResult<T>(
            CalculatedDataItemType calculatedDataItemType,
            string localeISOCode,
            out T result)
        {
            CalculatedDataItem<T> calculatedDataItem = DefaultResultsManager.GetCalculatedDataItem<T>(calculatedDataItemType, localeISOCode);
            if (calculatedDataItem.HasData)
            {
                result = calculatedDataItem.Data;
                return true;
            }

            result = default(T);
            return false;
        }

        protected bool TryGetPrecalculatedResult<T>(
            CalculatedDataItemType calculatedDataItemType,
            out T result)
        {
            CalculatedDataItem<T> calculatedDataItem = DefaultResultsManager.GetCalculatedDataItem<T>(calculatedDataItemType);

            if (calculatedDataItem.HasData)
            {
                result = calculatedDataItem.Data;
                return true;
            }

            result = default(T);
            return false;
        }

        protected void AddResultToCache<T>(
            CalculatedDataItemType calculatedDataItemType,
            T result)
        {
            GetCacheCalculatedDataItem<T>(calculatedDataItemType).Data = result;
        }

        protected void AddResultToCache<T>(
            CalculatedDataItemType calculatedDataItemType,
            string localeISOCode,
            T result)
        {
            GetCacheCalculatedDataItem<T>(calculatedDataItemType, localeISOCode).Data = result;
        }
    }
}
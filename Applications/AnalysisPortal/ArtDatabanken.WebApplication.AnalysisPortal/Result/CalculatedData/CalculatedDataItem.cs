using System;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData
{
    /// <summary>
    /// This class holds data and calculation status about a result.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    public class CalculatedDataItem<T>
    {
        private T _data;
        private DateTime _lastUpdated;        

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public T Data
        {
            get
            {
                return _data;
            }

            set
            {
                _data = value;
                _lastUpdated = DateTime.Now;
            }
        }        

        /// <summary>
        /// Gets the the creation time of this object.
        /// </summary>
        public DateTime LastUpdated
        {
            get { return _lastUpdated; }
        }

        /// <summary>
        /// Returns true if the result cache item has data.
        /// </summary>
        public bool HasData
        {
            get { return _data != null; }
        }

        /// <summary>
        /// Returns true if the the result cache item has data
        /// that is up to date.
        /// </summary>
        public bool HasFreshData
        {
            get { return Data != null && !NeedsRefresh; }
        }
        
        /// <summary>
        /// Gets a value indicating whether the data needs a refresh.
        /// Ie, some information in the Filter has been changed
        /// Todo - For now this is never true, because when we change
        /// a setting in MySettings the cache is cleared. Perhaps change this later...
        /// </summary>        
        public bool NeedsRefresh
        {
            get
            {                
                return SessionHandler.MySettings.ResultCacheNeedsRefresh;                
            }
        }

        /// <summary>
        /// Gets or sets the total count. Used by paging.
        /// </summary>        
        public long? TotalCount { get; set; }        

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatedDataItem{T}"/> class.
        /// </summary>
        public CalculatedDataItem()
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatedDataItem{T}"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public CalculatedDataItem(T data)
        {
            _data = data;
            _lastUpdated = DateTime.Now;
        }   
        
        ///// <summary>
        ///// Set calculation finished state.
        ///// </summary>
        ///// <param name="data">The calculated data.</param>
        //public void SetData(T data)
        //{            
        //    Data = data;            
        //}        
    }
}

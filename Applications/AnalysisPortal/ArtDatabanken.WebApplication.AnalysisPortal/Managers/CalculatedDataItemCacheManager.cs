using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
using ArtDatabanken.WebApplication.AnalysisPortal.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Observations;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers
{
    /// <summary>
    /// This class is used to get/set result data from/to session cache.
    /// </summary>
    public static class CalculatedDataItemCacheManager
    {
        /// <summary>
        /// Gets the result cache object.
        /// </summary>        
        /// <remarks>
        /// Right now, only only one ResultCache object is used. 
        /// Later, we could introduce a ResultCache object for each 
        /// MySettings state we used when we calculated the results.
        /// </remarks>
        private static CalculatedDataItemCollection CalculatedDataItemCollection
        {
            get { return SessionHandler.Results; }
        }

        /// <summary>
        /// Gets a result base item.
        /// </summary>
        /// <typeparam name="T">The result base data type.</typeparam>
        /// <param name="calculatedDataItemType">Type of the result base (enum).</param>
        /// <param name="mySettings">The MySettings object.</param>
        /// <param name="localeIsoCode">The locale ISO code.</param>
        /// <returns>
        /// Returns a CalculatedDataItem if the CalculatedDataItemType was found in session cache.
        /// Otherwise a NullCalculatedDataItem object is returned.
        /// </returns>
        public static CalculatedDataItem<T> GetCalculatedDataItem<T>(CalculatedDataItemType calculatedDataItemType, MySettings.MySettings mySettings, string localeIsoCode)
        {
            return CalculatedDataItemCollection.GetCalculatedDataItem<T>(calculatedDataItemType, localeIsoCode);
        }                

        /// <summary>
        /// Gets grid cell observations result base.
        /// </summary>        
        public static CalculatedDataItem<IList<IGridCellSpeciesObservationCount>> GetGridCellObservations(
            MySettings.MySettings mySettings,
            string localeIsoCode)
        {
            return GetCalculatedDataItem<IList<IGridCellSpeciesObservationCount>>(
                CalculatedDataItemType.GridCellObservations,
                mySettings, 
                localeIsoCode);
        }
       
        /// <summary>
        /// Get grid taxa result base.
        /// </summary>        
        public static CalculatedDataItem<IList<IGridCellSpeciesCount>> GetGridCellTaxa(
            MySettings.MySettings mySettings, string localeIsoCode)
        {
            return GetCalculatedDataItem<IList<IGridCellSpeciesCount>>(
                CalculatedDataItemType.GridCellTaxa,
                mySettings, 
                localeIsoCode);
        }

        /// <summary>
        /// Get species observation result base.
        /// </summary>        
        public static CalculatedDataItem<SpeciesObservationsData> GetSpeciesObservationData(
            MySettings.MySettings mySettings, string localeIsoCode)
        {
            return GetCalculatedDataItem<SpeciesObservationsData>(CalculatedDataItemType.SpeciesObservationData, mySettings, localeIsoCode);            
        }

        public static CalculatedDataItem<T> GetCalculatedDataItemEx<T>(CalculatedDataItemType calculatedDataItemType, MySettings.MySettings mySettings)
        {            
            return CalculatedDataItemCollection.GetCalculatedDataItem<T>(calculatedDataItemType);
        }

        public static CalculatedDataItem<T> GetCalculatedDataItemEx<T>(CalculatedDataItemType calculatedDataItemType, MySettings.MySettings mySettings, string localeISOCode)
        {
            return CalculatedDataItemCollection.GetCalculatedDataItem<T>(calculatedDataItemType, localeISOCode);
        }
    }
}

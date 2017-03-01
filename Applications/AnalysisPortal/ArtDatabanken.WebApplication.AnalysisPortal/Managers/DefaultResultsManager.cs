using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers
{
    public static class DefaultResultsManager
    {
        private static readonly CalculatedDataItemCollection CalculatedDataItemCollection = new CalculatedDataItemCollection();
       
        /// <summary>
        /// Clears the result base items.
        /// </summary>
        public static void Clear()
        {
            CalculatedDataItemCollection.Clear();
        }

        public static CalculatedDataItem<SpeciesObservationGridResult> GetGridCellObservations()
        {
            return CalculatedDataItemCollection.GetCalculatedDataItem<SpeciesObservationGridResult>(CalculatedDataItemType.GridCellObservations);
        }

        public static CalculatedDataItem<SpeciesObservationGridResult> AddGridCellObservations(SpeciesObservationGridResult data)
        {
            return CalculatedDataItemCollection.AddCalculatedDataItem(CalculatedDataItemType.GridCellObservations, data);
        }
            
        public static void ClearGridCellObservations()
        {
            CalculatedDataItemCollection.ClearCalculatedDataItem(CalculatedDataItemType.GridCellObservations);
        }

        public static CalculatedDataItem<List<KeyValuePair<string, string>>> AddSummaryStatistics(List<KeyValuePair<string, string>> data, string localeIsoCode)
        {
            return CalculatedDataItemCollection.AddCalculatedDataItem(CalculatedDataItemType.SummaryStatistics, localeIsoCode, data);            
        }

        public static CalculatedDataItem<List<KeyValuePair<string, string>>> GetSummaryStatistics(string localeIsoCode)
        {
            return CalculatedDataItemCollection.GetCalculatedDataItem<List<KeyValuePair<string, string>>>(CalculatedDataItemType.SummaryStatistics, localeIsoCode);
        }

        public static void ClearSummaryStatistics()
        {
            CalculatedDataItemCollection.ClearCalculatedDataItem(CalculatedDataItemType.SummaryStatistics);
        }

        public static CalculatedDataItem<T> GetCalculatedDataItem<T>(CalculatedDataItemType calculatedDataItemType, string localeIsoCode)
        {
            return CalculatedDataItemCollection.GetCalculatedDataItem<T>(calculatedDataItemType, localeIsoCode);
        }

        public static CalculatedDataItem<T> GetCalculatedDataItem<T>(CalculatedDataItemType calculatedDataItemType)
        {
            return CalculatedDataItemCollection.GetCalculatedDataItem<T>(calculatedDataItemType, null);
        }

        public static CalculatedDataItem<TaxonGridResult> AddGridCellTaxa(TaxonGridResult data)
        {
            return CalculatedDataItemCollection.AddCalculatedDataItem(CalculatedDataItemType.GridCellTaxa, data);            
        }

        public static CalculatedDataItem<TaxonGridResult> GetGridCellTaxa()
        {
            return CalculatedDataItemCollection.GetCalculatedDataItem<TaxonGridResult>(CalculatedDataItemType.GridCellTaxa);
        }
    }
}

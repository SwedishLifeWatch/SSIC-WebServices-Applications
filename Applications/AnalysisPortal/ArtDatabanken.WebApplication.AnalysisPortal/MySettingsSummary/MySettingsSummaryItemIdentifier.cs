namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary
{
    /// <summary>
    /// Identifiers of all MySettingsSummaryItemBase implementations
    /// </summary>
    public enum MySettingsSummaryItemIdentifier
    {
        DataProviders,
        DataMapLayers,
        DataEnvironmentalData,
        FilterPolygon,
        FilterRegion,
        FilterLocality,
        FilterTaxa,
        FilterTemporal,
        FilterAccuracy,
        FilterOccurrence,
        FilterField,        
        CalculationGridStatistics,
        CalculationGridStatisticsSubGrid,
        CalculationGridStatisticsSubCalculation,
        CalculationGridStatisticsSubEnvironment,
        CalculationSummaryStatistics,
        CalculationTimeSeries,
        PresentationMap,
        PresentationTable,
        PresentationReport,
        PresentationFileFormat
    }

    public enum MySettingsSummaryItemSubIdentifier
    {
        CalculationGridStatisticsSubGrid,
        CalculationGridStatisticsSubCalculation,
        CalculationGridStatisticsSubEnvironment
        ////, PresentationMapSubCoordinateSystem,
        ////  PresentationMapSubMapType
    }

    ////public enum MySettingsSummaryCalculationGridStatisticsItemIdentifier
    ////{
    ////    Grid,
    ////    Calculation,
    ////    Environment
    ////}
}

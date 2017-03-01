namespace ArtDatabanken.WebApplication.AnalysisPortal.Result
{
    /// <summary>
    /// The different types of results that can be calculated in AnalysisPortal.
    /// </summary>
    public enum ResultType
    {
        SpeciesObservationMap,
        SpeciesObservationTable,
        SpeciesObservationTaxonTable,
        SpeciesObservationTaxonWithObservationCountTable,
        TimeSeriesOnSpeciesObservationCountsHistogram,
        TimeSeriesOnSpeciesObservationAbundanceIndexHistogram,
        TimeSeriesOnSpeciesObservationCountsTable,
        SpeciesObservationGridMap,
        SpeciesObservationGridTable,
        SpeciesRichnessGridTable,
        SpeciesRichnessGridMap,
        SummaryStatisticsReport,
        SummaryStatisticsPerPolygon,
        SettingsSummary,
        WfsStatisticsGridMap,
        CombinedGridStatisticsTable,
        SpeciesObservationProvenanceReport,
        SpeciesObservationHeatMap
    }
}
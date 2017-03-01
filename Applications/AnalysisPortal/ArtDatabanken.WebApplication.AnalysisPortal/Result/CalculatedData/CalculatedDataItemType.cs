namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData
{
    /// <summary>
    /// The different types of base results.
    /// </summary>
    /// <remarks>
    /// A base result can be the result source for many ResultTypes.
    /// I.e. Two or more results uses the same base result data but formats it in different ways.
    /// E.g. CalculatedDataItemType.SpeciesObservationData is the result source        
    /// for both ResultType.SpeciesObservationMap & ResultType.SpeciesObservationTable
    /// </remarks>
    public enum CalculatedDataItemType
    {
        None,
        SpeciesObservationData,
        GridCellObservations,
        GridCellTaxa,        
        SummaryStatistics,
        SummaryStatisticsPerPolygon,
        SpeciesObservationDiagramData,
        SpeciesObservationAbundanceIndexDiagramData,
        SpeciesObservationTaxa,
        SpeciesObservationTaxaWithSpeciesObservationCount,
        PagedSpeciesObservation,
        WfsGridCellStatistics,
        CombinedGridCellStatistics,
        SpeciesObservationProvenance
    }
}

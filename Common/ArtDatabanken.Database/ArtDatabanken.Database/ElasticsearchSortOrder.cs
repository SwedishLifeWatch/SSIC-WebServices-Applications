namespace ArtDatabanken.Database
{
    /// <summary>
    /// Establish which type of order to use when assembling an aggregate string
    /// </summary>
    public enum ElasticsearchSortOrder
    {
        /// <summary>The sort part of the aggregation string is omitted (the Elasticsearch default is document count descending)</summary>
        None,

        /// <summary>Ascending sort order</summary>
        Ascending,

        /// <summary>Descending sort order</summary>
        Descending
    }
}

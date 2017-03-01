using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Database;

namespace ArtDatabanken.WebService.TaxonService.Data
{
    /// <summary>
    /// Extension methods to the class WebTaxonRevisionEventType.
    /// </summary>
    public static class WebTaxonRevisionEventTypeExtension
    {
        /// <summary>
        /// Load data into the WebTaxonRevisionEventType instance.
        /// </summary>
        /// <param name='revisionEventType'>Revision event type.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebTaxonRevisionEventType revisionEventType,
                                    DataReader dataReader)
        {
            revisionEventType.Id = dataReader.GetInt32(RevisionData.ID);
            revisionEventType.Description = dataReader.GetString(RevisionData.EVENT_TYPE_DESCRIPTION);
            revisionEventType.Identifier = dataReader.GetString(RevisionData.IDENTIFIER);
        }

        /// <summary>
        /// Check the data in current object.
        /// </summary>
        /// <param name='revisionEventType'>The taxon revision event type.</param>
        public static void CheckData(this WebTaxonRevisionEventType revisionEventType)
        {
            if (!revisionEventType.IsDataChecked)
            {
                revisionEventType.CheckStrings();
                revisionEventType.IsDataChecked = true;
            }
        }
    }
}

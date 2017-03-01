using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Database;

namespace ArtDatabanken.WebService.TaxonService.Data
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class WebRevisionEventExtension
    {

        /// <summary>
        /// </summary>
        /// <param name="revisionEvent">
        /// The revision event.
        /// </param>
        /// <param name="dataReader">
        /// The data reader.
        /// </param>
        public static void LoadData(this WebTaxonRevisionEvent revisionEvent, DataReader dataReader)
        {
            revisionEvent.Id = dataReader.GetInt32(RevisionEventData.ID);
            revisionEvent.RevisionId = dataReader.GetInt32(RevisionEventData.REVISIONID);
            revisionEvent.CreatedBy = dataReader.GetInt32(RevisionEventData.CREATEDBYID);
            revisionEvent.CreatedDate = dataReader.GetDateTime(RevisionEventData.CREATEDDATE);
            revisionEvent.TypeId = dataReader.GetInt32(RevisionEventData.EVENTTYPEID);
            revisionEvent.AffectedTaxa = dataReader.GetString(RevisionEventData.DESCRIPTION_AFFECTEDTAXA);
            revisionEvent.NewValue = dataReader.GetString(RevisionEventData.DESCRIPTION_NEWVALUE);
            revisionEvent.OldValue = dataReader.GetString(RevisionEventData.DESCRIPTION_OLDVALUE);
        }

        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='revisionEvent'>The revision event.</param>
        public static void CheckData(this WebTaxonRevisionEvent revisionEvent)
        {
            if (!revisionEvent.IsDataChecked)
            {
                revisionEvent.CheckStrings();
                revisionEvent.IsDataChecked = true;
            }
        }
    }
}

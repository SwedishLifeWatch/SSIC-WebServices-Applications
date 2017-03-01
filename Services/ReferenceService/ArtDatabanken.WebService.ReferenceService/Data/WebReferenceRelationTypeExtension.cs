using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.ReferenceService.Database;

namespace ArtDatabanken.WebService.ReferenceService.Data
{
    /// <summary>
    /// Extensions for class WebReferenceRelationType.
    /// </summary>
    public static class WebReferenceRelationTypeExtension
    {
        /// <summary>
        /// Load data into the WebReferenceRelationType instance.
        /// </summary>
        /// <param name='referenceRelationType'>WebReferenceRelationType object.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebReferenceRelationType referenceRelationType,
                                    DataReader dataReader)
        {
            referenceRelationType.Id = dataReader.GetInt32(ReferenceRelationData.ID);
            referenceRelationType.Identifier = dataReader.GetString(ReferenceRelationData.IDENTIFIER);
            referenceRelationType.Description = dataReader.GetString(ReferenceRelationData.DESCRIPTION);
        }

        /// <summary>
        /// Check the data in current object.
        /// </summary>
        /// <param name='referenceRelationType'>The WebReferenceRelationType object to be checked.</param>
        public static void CheckData(this WebReferenceRelationType referenceRelationType)
        {
            if (!referenceRelationType.IsDataChecked)
            {
                referenceRelationType.CheckStrings();
                referenceRelationType.IsDataChecked = true;
            }
        }
    }
}

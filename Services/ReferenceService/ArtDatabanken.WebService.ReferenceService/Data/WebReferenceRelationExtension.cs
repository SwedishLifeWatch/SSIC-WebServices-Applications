using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.ReferenceService.Database;

namespace ArtDatabanken.WebService.ReferenceService.Data
{
    /// <summary>
    /// Extension method to class WebReferenceRelation.
    /// </summary>
    public static class WebReferenceRelationExtension
    {
        /// <summary>
        /// Load data into the WebReferenceRelation instance.
        /// </summary>
        /// <param name='referenceRelation'>Reference relation.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebReferenceRelation referenceRelation, DataReader dataReader)
        {
            referenceRelation.Id = dataReader.GetInt32(ReferenceRelationData.ID);
            referenceRelation.RelatedObjectGuid = dataReader.GetString(ReferenceRelationData.RELATEDOBJECTGUID);
            referenceRelation.ReferenceId = dataReader.GetInt32(ReferenceRelationData.REFERENCEID);
            referenceRelation.TypeId = dataReader.GetInt32(ReferenceRelationData.TYPE);
        }

        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='referenceRelation'>ReferenceRelation</param>
        public static void CheckData(this WebReferenceRelation referenceRelation)
        {
            if (!referenceRelation.IsDataChecked)
            {
                referenceRelation.CheckStrings();
                referenceRelation.IsDataChecked = true;
            }
        }
    }
}

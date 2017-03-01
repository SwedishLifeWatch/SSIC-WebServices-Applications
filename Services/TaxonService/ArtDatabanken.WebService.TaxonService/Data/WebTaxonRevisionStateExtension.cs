using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Database;

namespace ArtDatabanken.WebService.TaxonService.Data
{
    /// <summary>
    /// Extension methods to the class WebTaxonRevisionState.
    /// </summary>
    public static class WebTaxonRevisionStateExtension
    {
        /// <summary>
        /// Load data into the WebTaxonRevisionState instance.
        /// </summary>
        /// <param name='revisionState'>Revision state.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebTaxonRevisionState revisionState,
                                    DataReader dataReader)
        {
            revisionState.Id = dataReader.GetInt32(RevisionData.ID);
            revisionState.Description = dataReader.GetString(RevisionData.DESCRIPTIONSTRING);
            revisionState.Identifier = dataReader.GetString(RevisionData.IDENTIFIER);
        }

        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='revisionState'>The revision state.</param>
        public static void CheckData(this WebTaxonRevisionState revisionState)
        {
            if (!revisionState.IsDataChecked)
            {
                revisionState.CheckStrings();
                revisionState.IsDataChecked = true;
            }
        }
    }
}

using System.Collections.Generic;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Database;

namespace ArtDatabanken.WebService.TaxonService.Data
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class WebRevisionExtension
    {
        /// <summary>
        /// Set IsSpeciesFactPublished
        /// </summary>
        /// <param name="revision"></param>
        /// <param name="value"></param>
        private static void SetSpeciesFactPublished(this WebTaxonRevision revision, bool value)
        {
            if (revision.DataFields.IsNull())
            {
                revision.DataFields = new List<WebDataField>();
            }

            revision.DataFields.Add(new WebDataField
                                    {
                                        Name = "IsSpeciesFactPublished",
                                        Type = WebDataType.Boolean,
                                        Value = value.ToString()
                                    });
        }

        /// <summary>
        /// Set IsReferenceRelationsPublished
        /// </summary>
        /// <param name="revision"></param>
        /// <param name="value"></param>
        private static void SetReferenceRelationsPublished(this WebTaxonRevision revision, bool value)
        {
            if (revision.DataFields.IsNull())
            {
                revision.DataFields = new List<WebDataField>();
            }

            revision.DataFields.Add(new WebDataField
                                    {
                                        Name = "IsReferenceRelationsPublished",
                                        Type = WebDataType.Boolean,
                                        Value = value.ToString()
                                    });
        }
        
        /// <summary>
        /// Load data into the WebRevision instance.
        /// </summary>
        /// <param name='revision'>Revision.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebTaxonRevision revision, DataReader dataReader)
        {
            revision.Id = dataReader.GetInt32(RevisionData.ID);
            revision.RootTaxon = new WebTaxon { Id = dataReader.GetInt32(RevisionData.TAXONID) };
            revision.Description = dataReader.GetString(RevisionData.DESCRIPTIONSTRING);
            revision.ExpectedStartDate = dataReader.GetDateTime(RevisionData.EXPECTEDSTARTDATE);
            revision.ExpectedEndDate = dataReader.GetDateTime(RevisionData.EXPECTEDENDDATE);
            revision.StateId = dataReader.GetInt32(RevisionData.REVISIONSTATE);
            revision.Guid = dataReader.GetString(RevisionData.GUID);
            revision.CreatedBy = dataReader.GetInt32(RevisionData.CREATEDBY);
            revision.CreatedDate = dataReader.GetDateTime(RevisionData.CREATEDDATE);
            revision.ModifiedBy = dataReader.GetInt32(RevisionData.MODIFIEDBY);
            revision.ModifiedDate = dataReader.GetDateTime(RevisionData.MODIFIEDDATE);
            revision.SetSpeciesFactPublished(dataReader.GetBoolean(RevisionData.ISSPECIESFACTPUBLISHED));
            if (dataReader.HasColumn(RevisionData.ISREFERENCERELATIONSPUBLISHED))
            {
                revision.SetReferenceRelationsPublished(dataReader.GetBoolean(RevisionData.ISREFERENCERELATIONSPUBLISHED));
            }
        }

        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='revision'>The revision.</param>
        public static void CheckData(this WebTaxonRevision revision)
        {
            if (!revision.IsDataChecked)
            {
                revision.CheckStrings();
                revision.IsDataChecked = true;
            }
        }
    }
}

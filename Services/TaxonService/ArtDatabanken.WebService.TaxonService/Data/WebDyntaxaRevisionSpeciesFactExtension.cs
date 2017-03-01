using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Database;

namespace ArtDatabanken.WebService.TaxonService.Data
{
    /// <summary>
    /// Contains extension to the WebDyntaxaRevisionSpeciesFact class.
    /// </summary>
    public static class WebDyntaxaRevisionSpeciesFactExtension
    {
        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name="dyntaxaSpeciesFact">Dyntaxa revision species fact.</param>
        public static void CheckData(this WebDyntaxaRevisionSpeciesFact dyntaxaSpeciesFact)
        {
            if (!dyntaxaSpeciesFact.IsDataChecked)
            {
                dyntaxaSpeciesFact.CheckStrings();
                dyntaxaSpeciesFact.IsDataChecked = true;
            }
        }

        /// <summary>
        /// Load data into the WebDyntaxaRevisionSpeciesFact instance.
        /// </summary>        
        /// <param name="dyntaxaSpeciesFact">Dyntaxa revision species fact.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebDyntaxaRevisionSpeciesFact dyntaxaSpeciesFact, DataReader dataReader)
        {
            dyntaxaSpeciesFact.Id = dataReader.GetInt32(TaxonCommon.ID);
            dyntaxaSpeciesFact.FactorId = dataReader.GetInt32(DyntaxaRevisionSpeciesFact.FACTOR_ID);
            dyntaxaSpeciesFact.TaxonId = dataReader.GetInt32(TaxonCommon.TAXON_ID);
            dyntaxaSpeciesFact.RevisionId = dataReader.GetInt32(TaxonCommon.REVISON_ID);
            dyntaxaSpeciesFact.StatusId = dataReader.GetNullableInt32(DyntaxaRevisionSpeciesFact.STATUS_ID);
            dyntaxaSpeciesFact.QualityId = dataReader.GetNullableInt32(DyntaxaRevisionSpeciesFact.QUALITY_ID);
            dyntaxaSpeciesFact.ReferenceId = dataReader.GetNullableInt32(DyntaxaRevisionSpeciesFact.REFERENCE_ID);
            dyntaxaSpeciesFact.Description = dataReader.GetString(DyntaxaRevisionSpeciesFact.DESCRIPTION);
            dyntaxaSpeciesFact.IsPublished = dataReader.GetBoolean(TaxonCommon.IS_PUBLISHED);
            dyntaxaSpeciesFact.ModifiedBy = dataReader.GetInt32(TaxonCommon.MODIFIED_BY);
            dyntaxaSpeciesFact.ModifiedDate = dataReader.GetDateTime(TaxonCommon.MODIFIED_DATE);
            dyntaxaSpeciesFact.CreatedBy = dataReader.GetInt32(TaxonCommon.CREATED_BY);
            dyntaxaSpeciesFact.CreatedDate = dataReader.GetDateTime(TaxonCommon.CREATED_DATE);
            dyntaxaSpeciesFact.IsRevisionEventIdSpecified = dataReader.IsNotDbNull(TaxonCommon.REVISON_EVENT_ID);
            if (dyntaxaSpeciesFact.IsRevisionEventIdSpecified)
            {
                dyntaxaSpeciesFact.RevisionEventId = dataReader.GetInt32(TaxonCommon.REVISON_EVENT_ID);
            }
            dyntaxaSpeciesFact.IsChangedInRevisionEventIdSpecified = dataReader.IsNotDbNull(TaxonCommon.CHANGED_IN_REVISON_EVENT_ID);
            if (dyntaxaSpeciesFact.IsChangedInRevisionEventIdSpecified)
            {                
                dyntaxaSpeciesFact.ChangedInRevisionEventId = dataReader.GetInt32(TaxonCommon.CHANGED_IN_REVISON_EVENT_ID);
            }
            dyntaxaSpeciesFact.SpeciesFactExists = dataReader.GetBoolean(DyntaxaRevisionSpeciesFact.SPECIESFACT_EXISTS);            
            dyntaxaSpeciesFact.OriginalStatusId = dataReader.GetNullableInt32(DyntaxaRevisionSpeciesFact.ORIGINAL_STATUS_ID);            
            dyntaxaSpeciesFact.OriginalQualityId = dataReader.GetNullableInt32(DyntaxaRevisionSpeciesFact.ORIGINAL_QUALITY_ID);            
            dyntaxaSpeciesFact.OriginalReferenceId = dataReader.GetNullableInt32(DyntaxaRevisionSpeciesFact.ORIGINAL_REFERENCE_ID);
                        
            dyntaxaSpeciesFact.OriginalDescription = dataReader.GetString(DyntaxaRevisionSpeciesFact.ORIGINAL_DESCRIPTION);
        }
    }
}
using System;
using System.Collections.Generic;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Database;

namespace ArtDatabanken.WebService.TaxonService.Data
{
    /// <summary>
    /// Contains extension to the WebDyntaxaReferenceRelation class.
    /// </summary>
    public static class WebDyntaxaReferenceRelationExtension
    {
        /// <summary>
        /// Load data into the WebDyntaxaReferenceRelation instance.
        /// </summary>        
        /// <param name="dyntaxaReferenceRelation">Dyntaxa reference relation.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebDyntaxaRevisionReferenceRelation dyntaxaReferenceRelation, DataReader dataReader)
        {                                                                                 
            dyntaxaReferenceRelation.Id = dataReader.GetInt32(TaxonCommon.ID);            
            dyntaxaReferenceRelation.RevisionId = dataReader.GetInt32(TaxonCommon.REVISON_ID);
            dyntaxaReferenceRelation.Action = dataReader.GetString(DyntaxaRevisionReferenceRelation.ACTION);
            dyntaxaReferenceRelation.RelatedObjectGUID = dataReader.GetString(DyntaxaRevisionReferenceRelation.RELATED_OBJECT_GUID);            
            dyntaxaReferenceRelation.ReferenceId = dataReader.GetInt32(DyntaxaRevisionReferenceRelation.REFERENCE_ID);
            dyntaxaReferenceRelation.ReferenceType = dataReader.GetInt32(DyntaxaRevisionReferenceRelation.REFERENCE_TYPE);
            dyntaxaReferenceRelation.OldReferenceType = dataReader.GetNullableInt32(DyntaxaRevisionReferenceRelation.OLD_REFERENCE_TYPE);
            dyntaxaReferenceRelation.ReferenceRelationId = dataReader.GetNullableInt32(DyntaxaRevisionReferenceRelation.REFERENCE_RELATION_ID);

            dyntaxaReferenceRelation.IsPublished = dataReader.GetBoolean(TaxonCommon.IS_PUBLISHED);
            dyntaxaReferenceRelation.ModifiedBy = dataReader.GetInt32(TaxonCommon.MODIFIED_BY);
            dyntaxaReferenceRelation.ModifiedDate = dataReader.GetDateTime(TaxonCommon.MODIFIED_DATE);
            dyntaxaReferenceRelation.CreatedBy = dataReader.GetInt32(TaxonCommon.CREATED_BY);
            dyntaxaReferenceRelation.CreatedDate = dataReader.GetDateTime(TaxonCommon.CREATED_DATE);
            dyntaxaReferenceRelation.IsRevisionEventIdSpecified = dataReader.IsNotDbNull(TaxonCommon.REVISON_EVENT_ID);
            if (dyntaxaReferenceRelation.IsRevisionEventIdSpecified)
            {
                dyntaxaReferenceRelation.RevisionEventId = dataReader.GetInt32(TaxonCommon.REVISON_EVENT_ID);
            }
            dyntaxaReferenceRelation.IsChangedInRevisionEventIdSpecified = dataReader.IsNotDbNull(TaxonCommon.CHANGED_IN_REVISON_EVENT_ID);
            if (dyntaxaReferenceRelation.IsChangedInRevisionEventIdSpecified)
            {                
                dyntaxaReferenceRelation.ChangedInRevisionEventId = dataReader.GetInt32(TaxonCommon.CHANGED_IN_REVISON_EVENT_ID);
            }            
        }
    }
}
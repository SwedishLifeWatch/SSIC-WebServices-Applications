using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;


namespace ArtDatabanken.WebService.TaxonService.Test.TestFactories
{
    public class WebDyntaxaReferenceRelationTestFactory
    {        
        /// <summary>
        /// Creates a WebDyntaxaRevisionReferenceRelation out of predefined data
        /// </summary>
        /// <returns>WebDyntaxaRevisionReferenceRelation</returns>
        public static WebDyntaxaRevisionReferenceRelation Create(
            int revisionId, 
            string relatedObjectGUID, 
            string action)
        {
            WebDyntaxaRevisionReferenceRelation refReferenceRelation = new WebDyntaxaRevisionReferenceRelation();
            refReferenceRelation.RelatedObjectGUID = relatedObjectGUID;
            refReferenceRelation.RevisionId = revisionId;
            refReferenceRelation.Action = action;
            refReferenceRelation.ReferenceType = 1;
            refReferenceRelation.OldReferenceType = null;
            refReferenceRelation.ReferenceRelationId = 1;
            refReferenceRelation.ReferenceId = 1;
            refReferenceRelation.CreatedBy = Settings.Default.TestUserId;
            refReferenceRelation.CreatedDate = DateTime.Now;
            refReferenceRelation.IsRevisionEventIdSpecified = true;
            refReferenceRelation.RevisionEventId = 1;
            refReferenceRelation.IsChangedInRevisionEventIdSpecified = true;
            refReferenceRelation.ChangedInRevisionEventId = 1;
            refReferenceRelation.IsPublished = false;

            return refReferenceRelation;
        }

        /// <summary>
        /// Creates a WebDyntaxaRevisionReferenceRelation out of predefined data
        /// </summary>
        /// <returns>WebDyntaxaRevisionReferenceRelation</returns>
        public static WebDyntaxaRevisionReferenceRelation Create(
            int revisionId, 
            int revisionEventId,
            string relatedObjectGUID,
            int referenceId,
            int typeId, 
            string action)
        {
            WebDyntaxaRevisionReferenceRelation refReferenceRelation = new WebDyntaxaRevisionReferenceRelation();
            refReferenceRelation.RelatedObjectGUID = relatedObjectGUID;
            refReferenceRelation.RevisionId = revisionId;
            refReferenceRelation.RevisionEventId = revisionEventId;
            refReferenceRelation.ReferenceId = referenceId;
            refReferenceRelation.ReferenceType = typeId;
            refReferenceRelation.Action = action;

            refReferenceRelation.OldReferenceType = null;
            refReferenceRelation.ReferenceRelationId = 1;            
            refReferenceRelation.CreatedBy = Settings.Default.TestUserId;
            refReferenceRelation.CreatedDate = DateTime.Now;
            refReferenceRelation.IsRevisionEventIdSpecified = true;
            
            refReferenceRelation.IsChangedInRevisionEventIdSpecified = true;
            refReferenceRelation.ChangedInRevisionEventId = 1;
            refReferenceRelation.IsPublished = false;

            return refReferenceRelation;
        }


        public static WebDyntaxaRevisionReferenceRelation CreateModifyAction(
            int revisionId,
            int revisionEventId,
            string relatedObjectGUID,
            int referenceId,
            int typeId,
            int oldTypeId)
        {
            WebDyntaxaRevisionReferenceRelation refReferenceRelation = new WebDyntaxaRevisionReferenceRelation();
            refReferenceRelation.RelatedObjectGUID = relatedObjectGUID;
            refReferenceRelation.RevisionId = revisionId;
            refReferenceRelation.RevisionEventId = revisionEventId;
            refReferenceRelation.ReferenceId = referenceId;
            refReferenceRelation.ReferenceType = typeId;
            refReferenceRelation.OldReferenceType = oldTypeId;
            refReferenceRelation.Action = "Modify";
            
            refReferenceRelation.ReferenceRelationId = 1;
            refReferenceRelation.CreatedBy = Settings.Default.TestUserId;
            refReferenceRelation.CreatedDate = DateTime.Now;
            refReferenceRelation.IsRevisionEventIdSpecified = true;

            refReferenceRelation.IsChangedInRevisionEventIdSpecified = true;
            refReferenceRelation.ChangedInRevisionEventId = 1;
            refReferenceRelation.IsPublished = false;

            return refReferenceRelation;
        }

        public static WebDyntaxaRevisionReferenceRelation CreateAddAction(
            int revisionId,
            int revisionEventId,
            string relatedObjectGUID,
            int referenceId,
            int typeId)
        {
            WebDyntaxaRevisionReferenceRelation refReferenceRelation = new WebDyntaxaRevisionReferenceRelation();
            refReferenceRelation.RelatedObjectGUID = relatedObjectGUID;
            refReferenceRelation.RevisionId = revisionId;
            refReferenceRelation.RevisionEventId = revisionEventId;
            refReferenceRelation.ReferenceId = referenceId;
            refReferenceRelation.ReferenceType = typeId;
            refReferenceRelation.OldReferenceType = null;
            refReferenceRelation.Action = "Add";

            refReferenceRelation.ReferenceRelationId = 1;
            refReferenceRelation.CreatedBy = Settings.Default.TestUserId;
            refReferenceRelation.CreatedDate = DateTime.Now;
            refReferenceRelation.IsRevisionEventIdSpecified = true;

            refReferenceRelation.IsChangedInRevisionEventIdSpecified = true;
            refReferenceRelation.ChangedInRevisionEventId = 1;
            refReferenceRelation.IsPublished = false;

            return refReferenceRelation;
        }

        public static WebDyntaxaRevisionReferenceRelation CreateDeleteAction(
                    int revisionId,
                    int revisionEventId,
                    string relatedObjectGUID,
                    int referenceId,
                    int typeId,
                    int oldTypeId)
        {
            WebDyntaxaRevisionReferenceRelation refReferenceRelation = new WebDyntaxaRevisionReferenceRelation();
            refReferenceRelation.RelatedObjectGUID = relatedObjectGUID;
            refReferenceRelation.RevisionId = revisionId;
            refReferenceRelation.RevisionEventId = revisionEventId;
            refReferenceRelation.ReferenceId = referenceId;
            refReferenceRelation.ReferenceType = typeId;
            refReferenceRelation.OldReferenceType = oldTypeId;
            refReferenceRelation.Action = "Delete";

            refReferenceRelation.ReferenceRelationId = 1;
            refReferenceRelation.CreatedBy = Settings.Default.TestUserId;
            refReferenceRelation.CreatedDate = DateTime.Now;
            refReferenceRelation.IsRevisionEventIdSpecified = true;

            refReferenceRelation.IsChangedInRevisionEventIdSpecified = true;
            refReferenceRelation.ChangedInRevisionEventId = 1;
            refReferenceRelation.IsPublished = false;

            return refReferenceRelation;
        }

    }
}

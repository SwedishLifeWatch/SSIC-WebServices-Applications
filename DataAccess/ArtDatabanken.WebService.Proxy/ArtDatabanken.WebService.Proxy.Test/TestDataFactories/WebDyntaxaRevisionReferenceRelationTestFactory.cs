using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Proxy.Test.TestDataFactories
{
    public class WebDyntaxaRevisionReferenceRelationTestFactory
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
    }
}
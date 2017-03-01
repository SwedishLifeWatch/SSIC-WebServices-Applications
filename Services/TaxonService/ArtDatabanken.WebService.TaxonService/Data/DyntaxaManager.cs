using System;
using System.Collections.Generic;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.TaxonService.Data
{
    /// <summary>
    /// Dyntaxa Manager used for handling revision items with
    /// connection to other services/databases.
    /// </summary>
    public class DyntaxaManager
    {
        /// <summary>
        /// Get Dyntaxa Revision Species Fact item.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="factorId">Factor id.</param>
        /// <param name="taxonId">Taxon id.</param>
        /// <param name="taxonRevisionId">Revision id.</param>
        /// <returns>
        /// A WebDyntaxaRevisionSpeciesFact if any revision steps have been 
        /// made for specified (factorId,taxonId,revisionId); otherwise null.
        /// </returns>
        public static WebDyntaxaRevisionSpeciesFact GetDyntaxaRevisionSpeciesFact(WebServiceContext context, Int32 factorId, Int32 taxonId, Int32 taxonRevisionId)
        {
            WebDyntaxaRevisionSpeciesFact dyntaxaSpeciesFact;

            // Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetDyntaxaRevisionSpeciesFact(factorId, taxonId, taxonRevisionId))
            {
                if (dataReader.Read())
                {
                    dyntaxaSpeciesFact = new WebDyntaxaRevisionSpeciesFact();
                    dyntaxaSpeciesFact.LoadData(dataReader);                    
                }
                else
                {
                    return null;
                }
            }

            return dyntaxaSpeciesFact;
        }

        /// <summary>
        /// Get all Dyntaxa Revision Species Fact items.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="taxonRevisionId"></param>
        /// <returns></returns>
        public static List<WebDyntaxaRevisionSpeciesFact> GetAllDyntaxaRevisionSpeciesFacts(WebServiceContext context, int taxonRevisionId)
        {
            List<WebDyntaxaRevisionSpeciesFact> dyntaxaSpeciesFacts = new List<WebDyntaxaRevisionSpeciesFact>();

            // Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetAllDyntaxaRevisionSpeciesFacts(taxonRevisionId))
            {
                while (dataReader.Read())
                {
                    var dyntaxaSpeciesFact = new WebDyntaxaRevisionSpeciesFact();
                    dyntaxaSpeciesFact.LoadData(dataReader);

                    dyntaxaSpeciesFacts.Add(dyntaxaSpeciesFact);
                }
            }

            return dyntaxaSpeciesFacts;
        }

        /// <summary>
        /// Creates the dyntaxa revision species fact.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="dyntaxaRevisionSpeciesFact">The dyntaxa revision species fact.</param>
        /// <returns></returns>        
        public static WebDyntaxaRevisionSpeciesFact CreateDyntaxaRevisionSpeciesFact(WebServiceContext context, WebDyntaxaRevisionSpeciesFact dyntaxaRevisionSpeciesFact)
        {
            // Check authority - AuthorityIdentifier.DyntaxaTaxonEditation
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.DyntaxaTaxonEditation);            
            if (!WebServiceData.AuthorizationManager.IsIdentiferInCurrentRole(context, Settings.Default.TaxonRevisionGuidPrefix))
            {
                throw new ApplicationException("User:" + context.GetUser().UserName + " is not editor of revision in current role.");
            }

            // Check arguments.
            context.CheckTransaction();
            dyntaxaRevisionSpeciesFact.CheckNotNull("dyntaxaRevisionSpeciesFact");
            dyntaxaRevisionSpeciesFact.CheckData();
            //var userId = WebServiceData.UserManager.GetUser(context).Id;

            var id = context.GetTaxonDatabase().CreateDyntaxaRevisionSpeciesFact(
                dyntaxaRevisionSpeciesFact.FactorId,
                dyntaxaRevisionSpeciesFact.TaxonId,
                dyntaxaRevisionSpeciesFact.RevisionId,
                dyntaxaRevisionSpeciesFact.StatusId,
                dyntaxaRevisionSpeciesFact.QualityId,
                dyntaxaRevisionSpeciesFact.Description,
                dyntaxaRevisionSpeciesFact.ReferenceId,
                dyntaxaRevisionSpeciesFact.CreatedDate,
                dyntaxaRevisionSpeciesFact.CreatedBy,
                dyntaxaRevisionSpeciesFact.RevisionEventId,
                dyntaxaRevisionSpeciesFact.SpeciesFactExists,
                dyntaxaRevisionSpeciesFact.OriginalStatusId,
                dyntaxaRevisionSpeciesFact.OriginalQualityId,
                dyntaxaRevisionSpeciesFact.OriginalReferenceId,
                dyntaxaRevisionSpeciesFact.OriginalDescription);

            WebDyntaxaRevisionSpeciesFact newDyntaxaRevisionSpeciesFact = GetDyntaxaRevisionSpeciesFactById(context, id);
            return newDyntaxaRevisionSpeciesFact;           
        }

        /// <summary>
        /// Gets the dyntaxa revision species fact by identifier.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public static WebDyntaxaRevisionSpeciesFact GetDyntaxaRevisionSpeciesFactById(WebServiceContext context, int id)
        {
            WebDyntaxaRevisionSpeciesFact dyntaxaSpeciesFact;

            // Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetDyntaxaRevisionSpeciesFactById(id))
            {
                if (dataReader.Read())
                {
                    dyntaxaSpeciesFact = new WebDyntaxaRevisionSpeciesFact();
                    dyntaxaSpeciesFact.LoadData(dataReader);
                }
                else
                {
                    return null;
                }
            }

            return dyntaxaSpeciesFact;
        }

        /// <summary>
        /// Creates a new complete Revision event, i.e. all revision event data is set.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="revisionEvent">Revision event object.</param>
        /// <returns>The created revision event object.</returns>
        public static WebTaxonRevisionEvent CreateCompleteRevisionEvent(WebServiceContext context, WebTaxonRevisionEvent revisionEvent)
        {
            // Check authority - AuthorityIdentifier.DyntaxaTaxonEditation
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.DyntaxaTaxonEditation);

            context.CheckTransaction();
            revisionEvent.CheckData();
            var revisionId = context.GetTaxonDatabase().CreateCompleteRevisionEvent(revisionEvent.RevisionId, revisionEvent.TypeId, revisionEvent.CreatedBy,
                revisionEvent.CreatedDate, revisionEvent.AffectedTaxa, revisionEvent.NewValue, revisionEvent.OldValue);

            return TaxonManager.GetRevisionEventById(context, revisionId);
        }

        /// <summary>
        /// Set revision species fact published flag to true
        /// </summary>
        /// <param name="context"></param>
        /// <param name="revisionId"></param>
        /// <returns></returns>
        public static bool SetRevisionSpeciesFactPublished(WebServiceContext context, int revisionId)
        {
            return context.GetTaxonDatabase().SetRevisionSpeciesFactPublished(revisionId);
        }

        /// <summary>
        /// Set revision reference relation published flag to true
        /// </summary>
        /// <param name="context"></param>
        /// <param name="revisionId"></param>
        /// <returns></returns>
        public static bool SetRevisionReferenceRelationPublished(WebServiceContext context, int revisionId)
        {
            return context.GetTaxonDatabase().SetRevisionReferenceRelationPublished(revisionId);
        }

        /// <summary>
        /// Creates a dyntaxa revision reference relation..
        /// </summary>
        /// <param name="context">Web service request context.</param>        
        /// <param name="dyntaxaRevisionReferenceRelation">The dyntaxa revision reference relation.</param>
        /// <returns></returns>        
        public static WebDyntaxaRevisionReferenceRelation CreateDyntaxaRevisionReferenceRelation(
            WebServiceContext context, 
            WebDyntaxaRevisionReferenceRelation dyntaxaRevisionReferenceRelation)
        {
            // Check authority - AuthorityIdentifier.DyntaxaTaxonEditation
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.DyntaxaTaxonEditation);
            if (!WebServiceData.AuthorizationManager.IsIdentiferInCurrentRole(context, Settings.Default.TaxonRevisionGuidPrefix))
            {
                throw new ApplicationException("User:" + context.GetUser().UserName + " is not editor of revision in current role.");
            }

            // Check arguments.
            context.CheckTransaction();            

            var id = context.GetTaxonDatabase().CreateDyntaxaRevisionReferenceRelation(
                dyntaxaRevisionReferenceRelation.RevisionId,
                dyntaxaRevisionReferenceRelation.Action,
                dyntaxaRevisionReferenceRelation.RelatedObjectGUID,
                dyntaxaRevisionReferenceRelation.ReferenceId,
                dyntaxaRevisionReferenceRelation.ReferenceType,
                dyntaxaRevisionReferenceRelation.OldReferenceType,
                dyntaxaRevisionReferenceRelation.ReferenceRelationId,
                dyntaxaRevisionReferenceRelation.CreatedDate,
                dyntaxaRevisionReferenceRelation.CreatedBy,
                dyntaxaRevisionReferenceRelation.RevisionEventId);

            WebDyntaxaRevisionReferenceRelation newDyntaxaReferenceRelation = GetDyntaxaRevisionReferenceRelationById(context, id);
            return newDyntaxaReferenceRelation;
        }

        /// <summary>
        /// Gets the dyntaxa revision reference relation by identifier.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public static WebDyntaxaRevisionReferenceRelation GetDyntaxaRevisionReferenceRelationById(
            WebServiceContext context, 
            int id)
        {
            WebDyntaxaRevisionReferenceRelation dyntaxaReferenceRelation;

            // Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetDyntaxaRevisionReferenceRelationById(id))
            {
                if (dataReader.Read())
                {
                    dyntaxaReferenceRelation = new WebDyntaxaRevisionReferenceRelation();
                    dyntaxaReferenceRelation.LoadData(dataReader);
                }
                else
                {
                    return null;
                }
            }

            return dyntaxaReferenceRelation;
        }

        /// <summary>
        /// Get all Dyntaxa Revision Reference relation items.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="revisionId"></param>
        /// <returns></returns>
        public static List<WebDyntaxaRevisionReferenceRelation> GetAllDyntaxaRevisionReferenceRelations(
            WebServiceContext context, 
            int revisionId)
        {
            List<WebDyntaxaRevisionReferenceRelation> referenceRelations = new List<WebDyntaxaRevisionReferenceRelation>();

            // Get information from database.
            using (DataReader dataReader = context.GetTaxonDatabase().GetAllDyntaxaRevisionReferenceRelations(revisionId))
            {
                while (dataReader.Read())
                {
                    var referenceRelation = new WebDyntaxaRevisionReferenceRelation();
                    referenceRelation.LoadData(dataReader);
                    referenceRelations.Add(referenceRelation);
                }
            }

            return referenceRelations;
        }

        /// <summary>
        /// Get dyntaxa revision reference relation item(s).
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="revisionId">The revision identifier.</param>
        /// <param name="relatedObjectGUID">The related object unique identifier.</param>
        /// <returns>
        /// A List of WebDyntaxaRevisionReferenceRelation if any revision steps have been 
        /// made for specified (revisionId,relatedObjectGUID); otherwise null.
        /// </returns>        
        public static List<WebDyntaxaRevisionReferenceRelation> GetDyntaxaRevisionReferenceRelation(
            WebServiceContext context, 
            int revisionId, 
            string relatedObjectGUID)
        {
            List<WebDyntaxaRevisionReferenceRelation> referenceRelations = new List<WebDyntaxaRevisionReferenceRelation>();            

            // Get information from database.
            relatedObjectGUID.CheckNotEmpty("relatedObjectGUID");
            relatedObjectGUID = relatedObjectGUID.CheckInjection();
            using (DataReader dataReader = context.GetTaxonDatabase().GetDyntaxaRevisionReferenceRelation(revisionId, relatedObjectGUID))
            {
                while (dataReader.Read())
                {
                    var referenceRelation = new WebDyntaxaRevisionReferenceRelation();
                    referenceRelation.LoadData(dataReader);
                    referenceRelations.Add(referenceRelation);
                }
            }

            return referenceRelations;
        }                
    }
}
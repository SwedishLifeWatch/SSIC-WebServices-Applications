using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.DyntaxaInternalService
{
    /// <summary>
    /// This class is used to handle Species fact revision data.
    /// </summary>
    public class DyntaxaInternalDataSource : DyntaxaInternalDataSourceBase, IDyntaxaInternalDataSource
    {
        private const string ReferenceEditActionStringAdd = "Add";
        private const string ReferenceEditActionStringDelete = "Delete";
        private const string ReferenceEditActionStringModify = "Modify";

        /// <summary>
        /// Creates a complete revision event.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonRevisionEvent">The taxon revision event.</param>
        /// <returns>The created revision event.</returns>
        public virtual TaxonRevisionEvent CreateCompleteRevisionEvent(IUserContext userContext, TaxonRevisionEvent taxonRevisionEvent)
        {
            WebTaxonRevisionEvent webRevisionEvent;

            CheckTransaction(userContext);
            webRevisionEvent = WebServiceProxy.DyntaxaInternalService.CreateCompleteRevisionEvent(
                GetClientInformation(userContext),
                GetTaxonRevisionEvent(taxonRevisionEvent));
            UpdateTaxonRevisionEvent(userContext, taxonRevisionEvent, webRevisionEvent);
            return taxonRevisionEvent;
        }

        /// <summary>
        /// Creates the dyntaxa revision species fact.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="speciesFact">The species fact.</param>
        /// <returns>The created Dyntaxa revision species fact.</returns>
        public virtual DyntaxaRevisionSpeciesFact CreateDyntaxaRevisionSpeciesFact(
            IUserContext userContext,
            DyntaxaRevisionSpeciesFact speciesFact)
        {
            WebDyntaxaRevisionSpeciesFact webSpeciesFact;
            CheckTransaction(userContext);

            webSpeciesFact =
                WebServiceProxy.DyntaxaInternalService.CreateDyntaxaRevisionSpeciesFact(
                    GetClientInformation(userContext), GetSpeciesFact(speciesFact));
            UpdateDyntaxaRevisionSpeciesFact(userContext, speciesFact, webSpeciesFact);

            return speciesFact;
        }

        /// <summary>
        /// Gets the dyntaxa revision species fact for specific factor, taxon, revision.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="factorId">The factor identifier.</param>
        /// <param name="taxonId">The taxon identifier.</param>
        /// <param name="revisionId">The revision identifier.</param>
        /// <returns>DyntaxaRevisionSpeciesFact or null if not found.</returns>
        public virtual DyntaxaRevisionSpeciesFact GetDyntaxaRevisionSpeciesFact(IUserContext userContext, Int32 factorId, Int32 taxonId, Int32 revisionId)
        {
            WebDyntaxaRevisionSpeciesFact dyntaxaRevisionSpeciesFact;

            CheckTransaction(userContext);
            dyntaxaRevisionSpeciesFact = WebServiceProxy.DyntaxaInternalService.GetDyntaxaRevisionSpeciesFact(GetClientInformation(userContext), factorId, taxonId, revisionId);
            return GetDyntaxaRevisionSpeciesFact(userContext, dyntaxaRevisionSpeciesFact);
        }

        public IList<IDyntaxaRevisionReferenceRelation> GetDyntaxaRevisionReferenceRelation(
            IUserContext userContext, 
            int revisionId, 
            string relatedObjectGUID)
        {
            CheckTransaction(userContext);
            var webDyntaxaRevisionReferenceRelation = WebServiceProxy.DyntaxaInternalService.GetDyntaxaRevisionReferenceRelation(
                GetClientInformation(userContext), 
                revisionId,
                relatedObjectGUID);

            return GetDyntaxaRevisionReferenceRelations(userContext, webDyntaxaRevisionReferenceRelation);
        }

        private IList<IDyntaxaRevisionReferenceRelation> GetDyntaxaRevisionReferenceRelations(
            IUserContext userContext, 
            IList<WebDyntaxaRevisionReferenceRelation> webDyntaxaRevisionReferenceRelations)
        {
            IList<IDyntaxaRevisionReferenceRelation> list = new List<IDyntaxaRevisionReferenceRelation>();
            foreach (var webDyntaxaRevisionReferenceRelation in webDyntaxaRevisionReferenceRelations)
            {
                IDyntaxaRevisionReferenceRelation referenceRelation = GetDyntaxaRevisionReferenceRelation(
                    userContext,
                    webDyntaxaRevisionReferenceRelation);
                list.Add(referenceRelation);
            }

            return list;
        }

        private IDyntaxaRevisionReferenceRelation GetDyntaxaRevisionReferenceRelation(
            IUserContext userContext, 
            WebDyntaxaRevisionReferenceRelation webDyntaxaRevisionReferenceRelation)
        {
            // Vi kanske borde skapa en Factory istället?
            // DyntaxaRevisionReferenceRelation.Create(...)            
            DyntaxaRevisionReferenceRelation referenceRelation = null;

            if (webDyntaxaRevisionReferenceRelation.IsNotNull())
            {
                referenceRelation = new DyntaxaRevisionReferenceRelation();
                UpdateDyntaxaRevisionReferenceRelation(userContext, referenceRelation, webDyntaxaRevisionReferenceRelation);
            }

            return referenceRelation;
        }

        private void UpdateDyntaxaRevisionReferenceRelation(IUserContext userContext, DyntaxaRevisionReferenceRelation referenceRelation, WebDyntaxaRevisionReferenceRelation webReferenceRelation)
        {
            if (webReferenceRelation.IsNull())
            {
                return;
            }
            referenceRelation.Id = webReferenceRelation.Id;
            referenceRelation.CreatedBy = webReferenceRelation.CreatedBy;
            referenceRelation.CreatedDate = webReferenceRelation.CreatedDate;
            referenceRelation.DataContext = GetDataContext(userContext);
            
            if (webReferenceRelation.IsChangedInRevisionEventIdSpecified)
            {
                referenceRelation.ChangedInRevisionEventId = webReferenceRelation.ChangedInRevisionEventId;
            }
            else
            {
                referenceRelation.ChangedInRevisionEventId = null;
            }

            referenceRelation.RevisionEventId = webReferenceRelation.RevisionEventId;
            referenceRelation.IsPublished = webReferenceRelation.IsPublished;
            referenceRelation.ModifiedBy = webReferenceRelation.ModifiedBy;
            referenceRelation.ModifiedDate = webReferenceRelation.ModifiedDate;            
            referenceRelation.RevisionId = webReferenceRelation.RevisionId;
            referenceRelation.OldReferenceType = webReferenceRelation.OldReferenceType;
            referenceRelation.RelatedObjectGUID = webReferenceRelation.RelatedObjectGUID;
            referenceRelation.ReferenceType = webReferenceRelation.ReferenceType;
            referenceRelation.ReferenceRelationId = webReferenceRelation.ReferenceRelationId;
            referenceRelation.ReferenceId = webReferenceRelation.ReferenceId;

            switch (webReferenceRelation.Action)
            {
                case ReferenceEditActionStringAdd:
                    referenceRelation.Action = ReferenceRelationEditAction.Add;
                    break;
                case ReferenceEditActionStringDelete:
                    referenceRelation.Action = ReferenceRelationEditAction.Delete;
                    break;
                case ReferenceEditActionStringModify:
                    referenceRelation.Action = ReferenceRelationEditAction.Modify;
                    break;
                default:
                    referenceRelation.Action = ReferenceRelationEditAction.Unknown;
                    break;                    
            }            
        }

        /// <summary>
        /// Converts a WebDyntaxaRevisionSpeciesFact to a DyntaxaRevisionSpeciesFact.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="webDyntaxaRevisionSpeciesFact">The web dyntaxa revision species fact.</param>
        /// <returns>A converted DyntaxaRevisionSpeciesFact.</returns>
        private DyntaxaRevisionSpeciesFact GetDyntaxaRevisionSpeciesFact(IUserContext userContext, WebDyntaxaRevisionSpeciesFact webDyntaxaRevisionSpeciesFact)
        {
            DyntaxaRevisionSpeciesFact speciesFact = null;

            if (webDyntaxaRevisionSpeciesFact.IsNotNull())
            {
                speciesFact = new DyntaxaRevisionSpeciesFact();
                UpdateDyntaxaRevisionSpeciesFact(userContext, speciesFact, webDyntaxaRevisionSpeciesFact);
            }

            return speciesFact;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="webDyntaxaRevisionSpeciesFacts"></param>
        /// <returns></returns>
        private List<DyntaxaRevisionSpeciesFact> GetDyntaxaRevisionSpeciesFacts(IUserContext userContext, List<WebDyntaxaRevisionSpeciesFact> webDyntaxaRevisionSpeciesFacts)
        {
            List<DyntaxaRevisionSpeciesFact> speciesFacts = new List<DyntaxaRevisionSpeciesFact>();
            if (webDyntaxaRevisionSpeciesFacts.IsNotNull() && webDyntaxaRevisionSpeciesFacts.Any())
            {
                foreach (var webDyntaxaRevisionSpeciesFact in webDyntaxaRevisionSpeciesFacts)
                {
                    speciesFacts.Add(GetDyntaxaRevisionSpeciesFact(userContext, webDyntaxaRevisionSpeciesFact));
                }
            }

            return speciesFacts;
        }      

        public virtual List<DyntaxaRevisionSpeciesFact> GetAllDyntaxaRevisionSpeciesFacts(IUserContext userContext, Int32 revisionId)
        {
            CheckTransaction(userContext);
            var dyntaxaRevisionSpeciesFacts = WebServiceProxy.DyntaxaInternalService.GetAllDyntaxaRevisionSpeciesFacts(GetClientInformation(userContext), revisionId);
            return GetDyntaxaRevisionSpeciesFacts(userContext, dyntaxaRevisionSpeciesFacts);
        }

        public virtual bool SetRevisionSpeciesFactPublished(IUserContext userContext, Int32 revisionId)
        {
            CheckTransaction(userContext);
            return WebServiceProxy.DyntaxaInternalService.SetRevisionSpeciesFactPublished(GetClientInformation(userContext), revisionId);
        }

        /// <summary>
        /// Updates the dyntaxa revision species fact.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="speciesFact">The species fact.</param>
        /// <param name="webSpeciesFact">The web species fact.</param>
        private void UpdateDyntaxaRevisionSpeciesFact(IUserContext userContext, DyntaxaRevisionSpeciesFact speciesFact, WebDyntaxaRevisionSpeciesFact webSpeciesFact)
        {
            if (webSpeciesFact.IsNull())
            {
                return;
            }

            speciesFact.CreatedBy = webSpeciesFact.CreatedBy;
            speciesFact.CreatedDate = webSpeciesFact.CreatedDate;
            speciesFact.DataContext = GetDataContext(userContext);
            speciesFact.Description = webSpeciesFact.Description;
            speciesFact.FactorId = webSpeciesFact.FactorId;
            speciesFact.Id = webSpeciesFact.Id;
            if (webSpeciesFact.IsChangedInRevisionEventIdSpecified)
            {
                speciesFact.ChangedInRevisionEventId = webSpeciesFact.ChangedInRevisionEventId;
            }
            else
            {
                speciesFact.ChangedInRevisionEventId = null;
            }

            if (webSpeciesFact.IsRevisionEventIdSpecified)
            {
                speciesFact.RevisionEventId = webSpeciesFact.RevisionEventId;
            }
            else
            {
                speciesFact.RevisionEventId = null;
            }

            speciesFact.IsPublished = webSpeciesFact.IsPublished;
            speciesFact.ModifiedBy = webSpeciesFact.ModifiedBy;
            speciesFact.ModifiedDate = webSpeciesFact.ModifiedDate;
            speciesFact.QualityId = webSpeciesFact.QualityId;
            speciesFact.ReferenceId = webSpeciesFact.ReferenceId;
            speciesFact.StatusId = webSpeciesFact.StatusId;
            speciesFact.TaxonId = webSpeciesFact.TaxonId;
            speciesFact.SpeciesFactExists = webSpeciesFact.SpeciesFactExists;
            speciesFact.OriginalStatusId = webSpeciesFact.OriginalStatusId;
            speciesFact.OriginalQualityId = webSpeciesFact.OriginalQualityId;
            speciesFact.OriginalReferenceId = webSpeciesFact.OriginalReferenceId;
            speciesFact.OriginalDescription = webSpeciesFact.OriginalDescription;
        }

        /// <summary>
        /// Copies data from webobject to domainobject.
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <param name="taxonRevisionEvent">
        /// The revision event.
        /// </param>
        /// <param name="webRevisionEvent">
        /// The web revision event.
        /// </param>
        private void UpdateTaxonRevisionEvent(
            IUserContext userContext,
            ITaxonRevisionEvent taxonRevisionEvent,
            WebTaxonRevisionEvent webRevisionEvent)
        {
            taxonRevisionEvent.AffectedTaxa = webRevisionEvent.AffectedTaxa;
            taxonRevisionEvent.CreatedBy = webRevisionEvent.CreatedBy;
            taxonRevisionEvent.CreatedDate = webRevisionEvent.CreatedDate;
            taxonRevisionEvent.DataContext = GetDataContext(userContext);
            taxonRevisionEvent.Id = webRevisionEvent.Id;
            taxonRevisionEvent.NewValue = webRevisionEvent.NewValue;
            taxonRevisionEvent.OldValue = webRevisionEvent.OldValue;
            taxonRevisionEvent.RevisionId = webRevisionEvent.RevisionId;
            taxonRevisionEvent.Type = CoreData.TaxonManager.GetTaxonRevisionEventType(userContext, webRevisionEvent.TypeId);
        }

        /// <summary>
        /// Convert a ITaxonRevisionEvent instance
        /// to a WebTaxonRevisionEvent instance.
        /// </summary>
        /// <param name="taxonRevisionEvent">A ITaxonRevisionEvent object.</param>
        /// <returns>A WebTaxonRevisionEvent instance.</returns>
        private WebTaxonRevisionEvent GetTaxonRevisionEvent(ITaxonRevisionEvent taxonRevisionEvent)
        {
            WebTaxonRevisionEvent webTaxonRevisionEvent;

            webTaxonRevisionEvent = null;
            if (taxonRevisionEvent.IsNotNull())
            {
                webTaxonRevisionEvent = new WebTaxonRevisionEvent();
                webTaxonRevisionEvent.AffectedTaxa = taxonRevisionEvent.AffectedTaxa;
                webTaxonRevisionEvent.CreatedBy = taxonRevisionEvent.CreatedBy;
                webTaxonRevisionEvent.CreatedDate = taxonRevisionEvent.CreatedDate;
                webTaxonRevisionEvent.Id = taxonRevisionEvent.Id;
                webTaxonRevisionEvent.NewValue = taxonRevisionEvent.NewValue;
                webTaxonRevisionEvent.OldValue = taxonRevisionEvent.OldValue;
                webTaxonRevisionEvent.RevisionId = taxonRevisionEvent.RevisionId;
                webTaxonRevisionEvent.TypeId = taxonRevisionEvent.Type.Id;
            }

            return webTaxonRevisionEvent;
        }

        /// <summary>
        /// Convert a DyntaxaRevisionSpeciesFact instance to a WebDyntaxaRevisionSpeciesFact instance.
        /// </summary>
        /// <param name="speciesFact">A DyntaxaRevisionSpeciesFact object.</param>
        /// <returns>A WebDyntaxaRevisionSpeciesFact object.</returns>
        private WebDyntaxaRevisionSpeciesFact GetSpeciesFact(DyntaxaRevisionSpeciesFact speciesFact)
        {
            WebDyntaxaRevisionSpeciesFact webSpeciesFact;

            webSpeciesFact = new WebDyntaxaRevisionSpeciesFact();
            if (speciesFact.ChangedInRevisionEventId.HasValue)
            {
                webSpeciesFact.ChangedInRevisionEventId = speciesFact.ChangedInRevisionEventId.Value;
                webSpeciesFact.IsChangedInRevisionEventIdSpecified = true;
            }
            else
            {
                webSpeciesFact.IsChangedInRevisionEventIdSpecified = false;
            }

            if (speciesFact.RevisionEventId.HasValue)
            {
                webSpeciesFact.RevisionEventId = speciesFact.RevisionEventId.Value;
                webSpeciesFact.IsRevisionEventIdSpecified = true;
            }
            else
            {
                webSpeciesFact.IsRevisionEventIdSpecified = false;
            }

            webSpeciesFact.CreatedBy = speciesFact.CreatedBy;
            webSpeciesFact.CreatedDate = speciesFact.CreatedDate;
            webSpeciesFact.Description = speciesFact.Description;
            webSpeciesFact.FactorId = speciesFact.FactorId;
            webSpeciesFact.Id = speciesFact.Id;
            webSpeciesFact.IsPublished = speciesFact.IsPublished;
            webSpeciesFact.ModifiedBy = speciesFact.ModifiedBy;
            webSpeciesFact.ModifiedDate = speciesFact.ModifiedDate;
            webSpeciesFact.QualityId = speciesFact.QualityId;
            webSpeciesFact.ReferenceId = speciesFact.ReferenceId;
            webSpeciesFact.StatusId = speciesFact.StatusId;
            webSpeciesFact.TaxonId = speciesFact.TaxonId;
            webSpeciesFact.RevisionId = speciesFact.RevisionId;
            webSpeciesFact.SpeciesFactExists = speciesFact.SpeciesFactExists;
            webSpeciesFact.OriginalStatusId = speciesFact.OriginalStatusId;
            webSpeciesFact.OriginalQualityId = speciesFact.OriginalQualityId;
            webSpeciesFact.OriginalReferenceId = speciesFact.OriginalReferenceId;
            webSpeciesFact.OriginalDescription = speciesFact.OriginalDescription;

            return webSpeciesFact;           
        }
    }
}

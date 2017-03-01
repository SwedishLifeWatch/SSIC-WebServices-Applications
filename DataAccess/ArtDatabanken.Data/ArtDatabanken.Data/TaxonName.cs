using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents a taxon name.
    /// </summary>
    public class TaxonName : UpdateInformation, ITaxonName
    {
        private List<IReferenceRelation> _references;
        private ITaxonRevisionEvent _replacedInRevisionEvent;
        private ITaxonRevisionEvent _changedInRevisionEvent;

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// IsRecommended
        /// </summary>
        public Boolean IsRecommended { get; set; }

        /// <summary>
        /// IsOkForObsSystems
        /// </summary>
        public Boolean IsOkForSpeciesObservation { get; set; }
        
        /// <summary>
        /// IsUnique
        /// </summary>
        public Boolean IsUnique { get; set; }

        /// <summary>
        /// Gets or sets IsPublished
        /// </summary>
        public Boolean IsPublished { get; set; }

        /// <summary>
        /// Gets or sets IsOriginalName
        /// </summary>
        public Boolean IsOriginalName { get; set; }

        /// <summary>
        /// Id for this taxon name.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Name for this taxon name.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets GUID.
        /// </summary>
        public String Guid { get; set; }

        /// <summary>
        /// Author of this taxon name.
        /// </summary>
        public String Author { get; set; }

        /// <summary>
        /// Gets or sets NameCategory.
        /// </summary>
        public ITaxonNameCategory Category { get; set; }

        /// <summary>
        /// Status of this taxon name.
        /// </summary>
        public ITaxonNameStatus Status { get; set; }

        /// <summary>
        /// Name usage of this taxon name.
        /// </summary>
        public ITaxonNameUsage NameUsage { get; set; }

        /// <summary>
        /// PersonName
        /// </summary>
        public String ModifiedByPerson { get; set; }

        /// <summary>
        /// Date user is valid from. Not Null. Is set to date created by default.
        /// Mandatory ie always required.
        /// </summary>
        public DateTime ValidFromDate { get; set; }

        /// <summary>
        /// Date user is valid to. Not Null. Is set to date created + 100 years by default.
        /// Mandatory ie always required.
        /// </summary>
        public DateTime ValidToDate { get; set; }

        /// <summary>
        /// Taxon that this name belongs to.
        /// </summary>
        public ITaxon Taxon { get; set; }

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Gets or sets RevisionEventId
        /// </summary>
        public Int32? ChangedInTaxonRevisionEventId { get; set; }

        /// <summary>
        /// Version of this taxon name.
        /// </summary>
        public Int32 Version { get; set; }

        /// <summary>
        /// Gets References.
        /// </summary>
        public List<IReferenceRelation> GetReferences(IUserContext userContext)
        {            
            if (_references.IsNull() && Guid.IsNotNull())
            {
                _references = new List<IReferenceRelation>();
                _references.AddRange(CoreData.ReferenceManager.GetReferenceRelations(userContext, Guid));
            }
            return _references;                        
        }

        /// <summary>
        /// Sets References.
        /// </summary>
        public void SetReferences(List<IReferenceRelation> references)
        {
            _references = references;
        }

        /// <summary>
        /// Gets RevisionEvent.
        /// </summary>
        public ITaxonRevisionEvent GetChangedInRevisionEvent(IUserContext userContext)
        {            
            if (_changedInRevisionEvent.IsNull() &&
                ChangedInTaxonRevisionEventId.HasValue)
            {
                _changedInRevisionEvent = CoreData.TaxonManager.GetTaxonRevisionEvent(userContext,
                                                                                     ChangedInTaxonRevisionEventId.Value);
            }
            return _changedInRevisionEvent;            
        }

        /// <summary>
        /// Sets RevisionEvent.
        /// </summary>
        public void SetChangedInRevisionEvent(ITaxonRevisionEvent changedInRevisionEvent)
        {
            _changedInRevisionEvent = changedInRevisionEvent;
            if (_changedInRevisionEvent.IsNull())
            {
                ChangedInTaxonRevisionEventId = null;
            }
            else
            {
                ChangedInTaxonRevisionEventId = _changedInRevisionEvent.Id;
            }
        }

        /// <summary>
        /// Gets or sets ChangedInRevisionEventId
        /// </summary>
        public Int32? ReplacedInTaxonRevisionEventId { get; set; }

        /// <summary>
        /// Gets ChangedInRevisionEvent.
        /// </summary>
        public ITaxonRevisionEvent GetReplacedInRevisionEvent(IUserContext userContext)
        {            
            if (_replacedInRevisionEvent.IsNull() &&
                ReplacedInTaxonRevisionEventId.HasValue)
            {
                _replacedInRevisionEvent = CoreData.TaxonManager.GetTaxonRevisionEvent(userContext,
                                                                                      ReplacedInTaxonRevisionEventId.Value);
            }
            return _replacedInRevisionEvent;
        }

        /// <summary>
        /// Sets ChangedInRevisionEvent.
        /// </summary>
        public void SetReplacedInRevisionEvent(ITaxonRevisionEvent replacedInRevisionEvent)
        {
            _replacedInRevisionEvent = replacedInRevisionEvent;
            if (_replacedInRevisionEvent.IsNull())
            {
                ReplacedInTaxonRevisionEventId = null;
            }
            else
            {
                ReplacedInTaxonRevisionEventId = _replacedInRevisionEvent.Id;
            }
        }

        /// <summary>
        /// Get a copy of this TaxonName.
        /// </summary>
        /// <returns>A copy of this TaxonName.</returns>
        public TaxonName Clone(IUserContext userContext)
        {
            TaxonName clone = (TaxonName)(MemberwiseClone());
            clone.SetChangedInRevisionEvent(GetChangedInRevisionEvent(userContext));
            clone.Taxon = Taxon;
            return clone;
        }
    }
}





using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Interface for TaxonName
    /// </summary>
    public interface ITaxonName : IUpdateInformation, IDataId32
    {
        /// <summary>
        /// Author of this taxon name.
        /// </summary>
        String Author { get; set; }

        /// <summary>
        /// Gets or sets NameCategory.
        /// </summary>
        ITaxonNameCategory Category { get; set; }

        /// <summary>
        /// Gets or sets RevisionEventId
        /// </summary>
        Int32? ChangedInTaxonRevisionEventId { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Gets or sets GUID.
        /// </summary>
        String Guid { get; set; }

        /// <summary>
        /// IsOkForSpeciesObservation
        /// </summary>
        Boolean IsOkForSpeciesObservation { get; set; }

        /// <summary>
        /// Gets or sets IsOriginalName
        /// </summary>
        Boolean IsOriginalName { get; set; }

        /// <summary>
        /// Gets or sets IsPublished
        /// </summary>
        Boolean IsPublished { get; set; }

        /// <summary>
        /// IsRecommended
        /// </summary>
        Boolean IsRecommended { get; set; }

        /// <summary>
        /// IsUnique
        /// </summary>
        Boolean IsUnique { get; set; }

        /// <summary>
        /// Last modified by the person with this name.
        /// </summary>
        String ModifiedByPerson { get; set; }        

        /// <summary>
        /// Name for this taxon name.
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Name usage of this taxon name.
        /// </summary>
        ITaxonNameUsage NameUsage { get; set; }

        /// <summary>
        /// Gets or sets ChangedInRevisionEventId
        /// </summary>
        Int32? ReplacedInTaxonRevisionEventId { get; set; }

        /// <summary>
        /// Status of this taxon name.
        /// </summary>
        ITaxonNameStatus Status { get; set; }

        /// <summary>
        /// Taxon name belongs to this taxon.
        /// </summary>
        ITaxon Taxon { get; set; }

        /// <summary>
        /// Version of this taxon name.
        /// </summary>
        Int32 Version { get; set; }

        /// <summary>
        /// Date user is valid from. Not Null. Is set to date created by default.
        /// Mandatory ie always required.
        /// </summary>
        DateTime ValidFromDate { get; set; }

        /// <summary>
        /// Date user is valid to. Not Null. Is set to date created + 100 years by default.
        /// Mandatory ie always required.
        /// </summary>
        DateTime ValidToDate { get; set; }

        /// <summary>
        /// Gets RevisionEvent.
        /// </summary>
        ITaxonRevisionEvent GetChangedInRevisionEvent(IUserContext userContext);

        /// <summary>
        /// Gets References.
        /// </summary>
        List<IReferenceRelation> GetReferences(IUserContext userContext);

        /// <summary>
        /// Gets ChangedInRevisionEvent.
        /// </summary>
        ITaxonRevisionEvent GetReplacedInRevisionEvent(IUserContext userContext);

        /// <summary>
        /// Sets RevisionEvent.
        /// </summary>
        void SetChangedInRevisionEvent(ITaxonRevisionEvent changedInRevisionEvent);

        /// <summary>
        /// Sets References.
        /// </summary>
        void SetReferences(List<IReferenceRelation> references);

        /// <summary>
        /// Sets ChangedInRevisionEvent.
        /// </summary>
        void SetReplacedInRevisionEvent(ITaxonRevisionEvent replacedInRevisionEvent);
    }
}

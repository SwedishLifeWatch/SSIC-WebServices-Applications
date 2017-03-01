using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents a Dyntaxa revision species fact.
    /// </summary>
    public class DyntaxaRevisionSpeciesFact
    {
        /// <summary>
        /// Unique id for this dyntaxa revision species fact.
        /// </summary>        
        public Int32 Id { get; set; }

        /// <summary>
        /// Factor Id for this species fact.
        /// </summary>
        public Int32 FactorId { get; set; }

        /// <summary>
        /// Taxon id for this species fact.
        /// </summary>
        public Int32 TaxonId { get; set; }

        /// <summary>
        /// Revision id.
        /// </summary>        
        public Int32 RevisionId { get; set; }

        /// <summary>
        /// StatusId for this species fact.        
        /// </summary>
        public Int32? StatusId { get; set; }      

        /// <summary>
        /// Quality id for this species fact.
        /// </summary>
        public Int32? QualityId { get; set; }

        /// <summary>
        /// Reference id for this species fact.
        /// </summary>
        public Int32? ReferenceId { get; set; }

        /// <summary>
        /// Description for this species fact.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Indicates whether the species fact exists in Artfakta database or not.
        /// </summary>
        public Boolean SpeciesFactExists { get; set; }

        /// <summary>
        /// Original StatusId for this species fact.
        /// Will be null if SpeciesFactExists==false.
        /// </summary>
        public Int32? OriginalStatusId { get; set; }

        /// <summary>
        /// Original Quality id for this species fact.
        /// Will be null if SpeciesFactExists==false.
        /// </summary>
        public Int32? OriginalQualityId { get; set; }

        /// <summary>
        /// Original Reference id for this species fact.
        /// Will be null if SpeciesFactExists==false.
        /// </summary>
        public Int32? OriginalReferenceId { get; set; }

        /// <summary>
        /// Original Description for this species fact.
        /// Will be null if SpeciesFactExists==false.
        /// </summary>
        public String OriginalDescription { get; set; }

        /// <summary>
        /// Last modified by the user with this id.        
        /// </summary>        
        public Int32 ModifiedBy { get; set; }

        /// <summary>
        /// Last modified at this date.        
        /// </summary>        
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Id of user that created the item.        
        /// </summary>         
        public Int32 CreatedBy { get; set; }

        /// <summary>
        /// Date and time when the item was created.        
        /// </summary>        
        public DateTime CreatedDate { get; set; }        

        /// <summary>
        /// Revision event id.
        /// </summary>        
        public Int32? RevisionEventId { get; set; }

        /// <summary>
        /// This species fact are part of a revision if a
        /// revision event id is specified.        
        /// </summary>        
        public Int32? ChangedInRevisionEventId { get; set; }        

        /// <summary>
        /// Indicates if this species fact has been published.
        /// </summary>
        public Boolean IsPublished { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext { get; set; }
    }
}





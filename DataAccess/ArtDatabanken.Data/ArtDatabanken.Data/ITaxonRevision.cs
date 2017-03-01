using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information with information related to a taxon revision.
    /// </summary>
    public interface ITaxonRevision : IUpdateInformation, IDataId32
    {
        /// <summary>
        /// Data context with meta information about this object.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Description of the revision.
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Expected taxon revision end date.
        /// </summary>
        DateTime ExpectedEndDate { get; set; }

        /// <summary>
        /// Expected taxon revision start date.
        /// </summary>
        DateTime ExpectedStartDate { get; set; }

        /// <summary>
        /// True if species facts are published to Artfakta
        /// </summary>
        bool IsSpeciesFactPublished { get; set; }

        /// <summary>
        /// True if reference relations are published to Artfakta
        /// </summary>
        bool IsReferenceRelationsPublished { get; set; }

        /// <summary>
        /// GUID (Globally Unique Identifier) for this object.
        /// It is a LSID, which is unique for each version of the record holding the information included in this object. 
        /// It is updated automatically by database each time information is saved.
        /// </summary>
        String Guid { get; set; }

        /// <summary>
        /// Root taxon for this revision.
        /// The root taxon defines the scoop of the revision.
        /// This must not be the same taxon as the
        /// root taxon for the whole taxon tree.
        /// </summary>
        ITaxon RootTaxon { get; set; }

        /// <summary>
        /// Current state of the revision.
        /// </summary>
        ITaxonRevisionState State { get; set; }        

        /// <summary>
        /// Get references.
        /// </summary>
        /// <param name="userContext"> The user context.</param>
        /// <returns>References.</returns>
        List<IReferenceRelation> GetReferences(IUserContext userContext);

        /// <summary>
        /// Set references.
        /// </summary>
        /// <param name="references">References.</param>
        void SetReferences(List<IReferenceRelation> references);

        /// <summary>
        /// Get revision events.
        /// </summary>
        /// <param name="userContext"> The user context.</param>
        /// <returns>Revision events.</returns>
        List<ITaxonRevisionEvent> GetRevisionEvents(IUserContext userContext);

        /// <summary>
        /// Set revision events.
        /// </summary>
        /// <param name="revisionEvents">Revision events.</param>
        void SetRevisionEvents(List<ITaxonRevisionEvent> revisionEvents);
    }
}

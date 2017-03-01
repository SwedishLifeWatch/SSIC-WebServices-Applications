using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class with information related to a taxon revision.
    /// </summary>
    public class TaxonRevision : UpdateInformation, ITaxonRevision
    {
        /// <summary>
        /// Data context with meta information about this object.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Id for the taxon revision.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Taxon that belongs to this revision.
        /// </summary>
        public ITaxon RootTaxon { get; set; }

        /// <summary>
        /// Description of the revision.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Expected taxon revision start date.
        /// </summary>
        public DateTime ExpectedStartDate { get; set; }

        /// <summary>
        /// Expected taxon revision end date.
        /// </summary>
        public DateTime ExpectedEndDate { get; set; }

        /// <summary>
        /// Current state of the revision.
        /// </summary>
        public ITaxonRevisionState State { get; set; }

        /// <summary>
        /// True if species facts are published to Artfakta
        /// </summary>
        public bool IsSpeciesFactPublished { get; set; }

        /// <summary>
        /// True if reference relations are published to Artfakta
        /// </summary>
        public bool IsReferenceRelationsPublished { get; set; }

        /// <summary>
        /// GUID (Globally Unique Identifier) for this object.
        /// It is a LSID, which is unique for each version of the record holding the information included in this object. 
        /// It is updated automatically by database each time information is saved.
        /// </summary>
        public String Guid { get; set; }

        /// <summary>
        /// References used in this taxon revision.
        /// </summary>
        private List<IReferenceRelation> _references;

        /// <summary>
        /// Get references.
        /// </summary>
        /// <param name="userContext"> The user context.</param>
        /// <returns>References.</returns>
        public List<IReferenceRelation> GetReferences(IUserContext userContext)
        {
            if (_references.IsNull())
            {
                _references = new List<IReferenceRelation>();
                _references.AddRange(CoreData.ReferenceManager.GetReferenceRelations(userContext, Guid));
            }
            return _references;
        }

        /// <summary>
        /// Set references.
        /// </summary>
        /// <param name="references">References.</param>
        public void SetReferences(List<IReferenceRelation> references)
        {
            _references = references;
        }


        private List<ITaxonRevisionEvent> _revisionEvents;

        /// <summary>
        /// Get revision events.
        /// </summary>
        /// <param name="userContext"> The user context.</param>
        /// <returns>Revision events.</returns>
        public List<ITaxonRevisionEvent> GetRevisionEvents(IUserContext userContext)
        {            
            if (_revisionEvents == null)
            {
                var dataIdList = CoreData.TaxonManager.GetTaxonRevisionEvents(userContext, Id);
                _revisionEvents = new List<ITaxonRevisionEvent>();

                foreach (var revEvent in dataIdList)
                {
                    _revisionEvents.Add((ITaxonRevisionEvent)revEvent);
                }

                _revisionEvents = _revisionEvents.OrderBy(x => x.Id).ToList();
            }

            return _revisionEvents;            
        }

        /// <summary>
        /// Set revision events.
        /// </summary>
        /// <param name="revisionEvents">Revision events.</param>
        public void SetRevisionEvents(List<ITaxonRevisionEvent> revisionEvents)
        {
            _revisionEvents = revisionEvents;
        }
    }
}

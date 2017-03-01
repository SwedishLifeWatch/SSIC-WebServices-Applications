using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.DyntaxaInternalService
{
    /// <summary>
    /// Manager for handling species fact revision data.
    /// </summary>
    public class DyntaxaInternalTaxonServiceManager
    {
        /// <summary>
        /// This property is used to retrieve or update taxon information.
        /// </summary>
        public DyntaxaInternalDataSource DataSource { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DyntaxaInternalTaxonServiceManager"/> class.
        /// </summary>
        public DyntaxaInternalTaxonServiceManager()
        {
            DataSource = new DyntaxaInternalDataSource();
        }

        /// <summary>
        /// Gets the dyntaxa revision species fact.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="factorId">The factor identifier.</param>
        /// <param name="taxonId">The taxon identifier.</param>
        /// <param name="revisionId">The revision identifier.</param>
        /// <returns>A DyntaxaRevisionSpeciesFact if found; otherwise null.</returns>
        public DyntaxaRevisionSpeciesFact GetDyntaxaRevisionSpeciesFact(IUserContext userContext, Int32 factorId, Int32 taxonId, Int32 revisionId)
        {
            return DataSource.GetDyntaxaRevisionSpeciesFact(userContext, factorId, taxonId, revisionId);
        }

        /// <summary>
        /// Creates the dyntaxa revision species fact.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="speciesFact">The species fact.</param>
        /// <returns>The created DyntaxaRevisionSpeciesFact.</returns>
        public DyntaxaRevisionSpeciesFact CreateDyntaxaRevisionSpeciesFact(IUserContext userContext, DyntaxaRevisionSpeciesFact speciesFact)
        {
            return DataSource.CreateDyntaxaRevisionSpeciesFact(userContext, speciesFact);            
        }

        /// <summary>
        /// Gets all dyntaxa revision species facts.
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="revisionId"></param>
        /// <returns></returns>
        public List<DyntaxaRevisionSpeciesFact> GetAllDyntaxaRevisionSpeciesFacts(IUserContext userContext, Int32 revisionId)
        {
            return DataSource.GetAllDyntaxaRevisionSpeciesFacts(userContext, revisionId);
        }

        /// <summary>
        /// Creates the complete revision event.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonRevisionEvent">The taxon revision event.</param>
        /// <returns>The created revision event.</returns>
        public TaxonRevisionEvent CreateCompleteRevisionEvent(IUserContext userContext, TaxonRevisionEvent taxonRevisionEvent)
        {
            return DataSource.CreateCompleteRevisionEvent(userContext, taxonRevisionEvent);            
        }

        public bool SetRevisionSpeciesFactPublished(IUserContext userContext, Int32 revisionId)
        {
            return DataSource.SetRevisionSpeciesFactPublished(userContext, revisionId);
        }
    }
}
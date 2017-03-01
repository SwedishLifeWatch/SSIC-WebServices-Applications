using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a pictures search criteria.
    /// </summary>
    public interface IPicturesSearchCriteria
    {
        /// <summary>
        /// Search for pictures that belong to specified factors.
        /// </summary>
        FactorList Factors { get; set; }

        /// <summary>
        /// Search for pictures that belong to specified taxa.
        /// </summary>
        TaxonList Taxa { get; set; }

        /// <summary>
        /// Search for pictures that belong to specified species fact identifiers.
        /// </summary>
        List<String> SpeciesFactIdentifiers { get; set; }
        
        /// <summary>
        /// Search for pictures that contain the specified metadata (id and/or value).
        /// </summary>
        PictureMetaDataList MetaData { get; set; }

        /// <summary>
        /// Add factor to search criteria.
        /// </summary>
        /// <param name="factor">The factor.</param>
        void Add(IFactor factor);

        /// <summary>
        /// Add taxon to search criteria.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        void Add(ITaxon taxon);

        /// <summary>
        /// Add species fact identifier to search criteria.
        /// </summary>
        /// <param name="speciesFactIdentifier">The species fact identifier.</param>
        void Add(String speciesFactIdentifier);

        /// <summary>
        /// Add metadata to search criteria.
        /// </summary>
        /// <param name="metaData">The metadata.</param>
        void Add(IPictureMetaData metaData);
    }
}
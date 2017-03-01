using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents a pictures search criteria.
    /// </summary>
    public class PicturesSearchCriteria : IPicturesSearchCriteria
    {
        /// <summary>
        /// Search for pictures that belong to specified factors.
        /// </summary>
        public FactorList Factors { get; set; }

        /// <summary>
        /// Search for pictures that belong to specified taxa.
        /// </summary>
        public TaxonList Taxa { get; set; }

        /// <summary>
        /// Search for pictures that belong to specified species fact identifiers.
        /// </summary>
        public List<String> SpeciesFactIdentifiers { get; set; }

        /// <summary>
        /// Search for pictures that contain the specified metadata (id and/or value).
        /// </summary>
        public PictureMetaDataList MetaData { get; set; }

        /// <summary>
        /// Add factor to search criteria.
        /// </summary>
        /// <param name="factor">The factor.</param>
        public void Add(IFactor factor)
        {
            if (Factors.IsNull())
            {
                Factors = new FactorList();
            }

            Factors.Add(factor);
        }

        /// <summary>
        /// Add taxon to search criteria.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        public void Add(ITaxon taxon)
        {
            if (Taxa.IsNull())
            {
                Taxa = new TaxonList();
            }

            Taxa.Add(taxon);
        }

        /// <summary>
        /// Add species fact identifier to search criteria.
        /// </summary>
        /// <param name="speciesFactIdentifier">The species fact identifier.</param>
        public void Add(String speciesFactIdentifier)
        {
            if (SpeciesFactIdentifiers.IsNull())
            {
                SpeciesFactIdentifiers = new List<String>();
            }

            SpeciesFactIdentifiers.Add(speciesFactIdentifier);
        }

        /// <summary>
        /// Add metadata to search criteria.
        /// </summary>
        /// <param name="metaData">The metadata.</param>
        public void Add(IPictureMetaData metaData)
        {
            if (MetaData.IsNull())
            {
                MetaData = new PictureMetaDataList();
            }

            MetaData.Add(metaData);
        }
    }
}
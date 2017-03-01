using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a species fact search criteria.
    /// </summary>
    public interface ISpeciesFactSearchCriteria
    {
        /// <summary>
        /// Search for species facts that belongs to factors
        /// that is of one of these factor data types.
        /// </summary>
        FactorDataTypeList FactorDataTypes { get; set; }

        /// <summary>
        /// Search for species facts that belongs to specified factors.
        /// </summary>
        FactorList Factors { get; set; }

        /// <summary>
        /// Logical operator to use between field search criteria
        /// if more than one field search criteria are specified.
        /// Possible logical operator values are AND or OR.
        /// </summary>
        LogicalOperator FieldLogicalOperator { get; set; }

        /// <summary>
        /// Search criteria for fields in species facts.
        /// Only parts of the possible functionality is implemented.
        /// </summary>
        SpeciesFactFieldSearchCriteriaList FieldSearchCriteria { get; set; }

        /// <summary>
        /// Search for species facts that are related to specified hosts.
        /// </summary>
        TaxonList Hosts { get; set; }

        /// <summary>
        /// Specify if species facts related to not valid hosts
        /// should be included or not. Valid hosts are always
        /// included in the species fact search.
        /// </summary>
        Boolean IncludeNotValidHosts { get; set; }

        /// <summary>
        /// Specify if species facts related to not valid taxa
        /// should be included or not. Valid taxa are always
        /// included in the species fact search.
        /// </summary>
        Boolean IncludeNotValidTaxa { get; set; }

        /// <summary>
        /// Search for species facts that belongs to specified 
        /// individual categories.
        /// </summary>
        IndividualCategoryList IndividualCategories { get; set; }

        /// <summary>
        /// Search for species facts that belongs to specified periods.
        /// </summary>
        PeriodList Periods { get; set; }

        ReferenceList References { get; set; }

        /// <summary>
        /// Search for species facts that belongs to specified taxa.
        /// </summary>
        TaxonList Taxa { get; set; }

        /// <summary>
        /// Add factor to search criteria.
        /// </summary>
        /// <param name="factor">The factor.</param>
         void Add(IFactor factor);

         /// <summary>
         /// Add individual category to search criteria.
         /// </summary>
         /// <param name="individualCategory">The individual category.</param>
         void Add(IIndividualCategory individualCategory);

         /// <summary>
         /// Add period to search criteria.
         /// </summary>
         /// <param name="period">The period.</param>
         void Add(IPeriod period);

         /// <summary>
         /// Add species fact field search criteria to search criteria.
         /// </summary>
         /// <param name="fieldSearchCriteria">The field search criteria.</param>
         void Add(ISpeciesFactFieldSearchCriteria fieldSearchCriteria);

        /// <summary>
         /// Add taxon to search criteria.
         /// </summary>
         /// <param name="taxon">The taxon.</param>
         void AddTaxon(ITaxon taxon);
    }
}

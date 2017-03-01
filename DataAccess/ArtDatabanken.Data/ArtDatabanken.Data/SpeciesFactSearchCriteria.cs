using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents a species fact search criteria.
    /// </summary>
    public class SpeciesFactSearchCriteria : ISpeciesFactSearchCriteria
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SpeciesFactSearchCriteria()
        {
            FactorDataTypes = new FactorDataTypeList();
            Factors = new FactorList();
            FieldSearchCriteria = new SpeciesFactFieldSearchCriteriaList();
            Hosts = new TaxonList();
            IndividualCategories = new IndividualCategoryList();
            Periods = new PeriodList();
            References = new ReferenceList();
            Taxa = new TaxonList();
        }

        /// <summary>
        /// Search for species facts that belongs to factors
        /// that is of one of these factor data types.
        /// </summary>
        public FactorDataTypeList FactorDataTypes { get; set; }

        /// <summary>
        /// Search for species facts that belongs to specified factors.
        /// </summary>
        public FactorList Factors { get; set; }

        /// <summary>
        /// Logical operator to use between field search criteria
        /// if more than one field search criteria are specified.
        /// Possible logical operator values are AND or OR.
        /// </summary>
        public LogicalOperator FieldLogicalOperator { get; set; }

        /// <summary>
        /// Search criteria for fields in species facts.
        /// Only parts of the possible functionality is implemented.
        /// </summary>
        public SpeciesFactFieldSearchCriteriaList FieldSearchCriteria { get; set; }

        /// <summary>
        /// Search for species facts that are related to specified hosts.
        /// </summary>
        public TaxonList Hosts { get; set; }

        /// <summary>
        /// Specify if species facts related to not valid hosts
        /// should be included or not. Valid hosts are always
        /// included in the species fact search.
        /// </summary>
        public Boolean IncludeNotValidHosts { get; set; }

        /// <summary>
        /// Specify if species facts related to not valid taxa
        /// should be included or not. Valid taxa are always
        /// included in the species fact search.
        /// </summary>
        public Boolean IncludeNotValidTaxa { get; set; }

        /// <summary>
        /// Search for species facts that belongs to specified 
        /// individual categories.
        /// </summary>
        public IndividualCategoryList IndividualCategories { get; set; }

        /// <summary>
        /// Search for species facts that belongs to specified periods.
        /// </summary>
        public PeriodList Periods { get; set; }

        public ReferenceList References { get; set; }

        /// <summary>
        /// Search for species facts that belongs to specified taxa.
        /// </summary>
        public TaxonList Taxa { get; set; }

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
        /// Add individual category to search criteria.
        /// </summary>
        /// <param name="individualCategory">The individual category.</param>
        public void Add(IIndividualCategory individualCategory)
        {
            if (IndividualCategories.IsNull())
            {
                IndividualCategories = new IndividualCategoryList();
            }

            IndividualCategories.Add(individualCategory);
        }

        /// <summary>
        /// Add period to search criteria.
        /// </summary>
        /// <param name="period">The period.</param>
        public void Add(IPeriod period)
        {
            if (Periods.IsNull())
            {
                Periods = new PeriodList();
            }

            Periods.Add(period);
        }

        /// <summary>
        /// Add species fact field search criteria to search criteria.
        /// </summary>
        /// <param name="fieldSearchCriteria">The field search criteria.</param>
        public void Add(ISpeciesFactFieldSearchCriteria fieldSearchCriteria)
        {
            if (FieldSearchCriteria.IsNull())
            {
                FieldSearchCriteria = new SpeciesFactFieldSearchCriteriaList();
            }

            FieldSearchCriteria.Add(fieldSearchCriteria);
        }

        /// <summary>
        /// Add taxon to search criteria.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        public virtual void AddTaxon(ITaxon taxon)
        {
            if (Taxa.IsNull())
            {
                Taxa = new TaxonList();
            }

            Taxa.Add(taxon);
        }
    }
}

using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface holds selections that are used when
    /// a species fact data set is created or updated.
    /// </summary>
    public interface ISpeciesFactDataSetSelection : ICloneable
    {
        /// <summary>
        /// List of factors selected by user.
        /// This list is never set to null.
        /// </summary>
        FactorList Factors { get; set; }

        /// <summary>
        /// Indication of whether or not the species fact data set
        /// selection holds any selection of factors.
        /// </summary>
        Boolean HasFactors { get;}

        /// <summary>
        /// Indication of whether or not the species fact data set
        /// selection holds any selection of hosts.
        /// </summary>
        Boolean HasHosts { get;}

        /// <summary>
        /// Indication of whether or not the species fact data set
        /// selection holds any selection of individual categories.
        /// </summary>
        Boolean HasIndividualCategories { get;}

        /// <summary>
        /// Indication of whether or not the species fact data set
        /// selection holds any selection of periods.
        /// </summary>
        Boolean HasPeriods { get;}
       
        /// <summary>
        /// Indication of whether or not the species fact data set
        /// selection holds any selection of references.
        /// </summary>
        Boolean HasReferences { get;}
        
        /// <summary>
        /// Indication of whether or not the species fact data set
        /// selection holds any selection of taxa.
        /// </summary>
        Boolean HasTaxa { get;}

        /// <summary>
        /// List of host taxa selected by user.
        /// This list is never set to null.
        /// </summary>
        TaxonList Hosts { get; set; }

        /// <summary>
        /// List of individual categories selected by user.
        /// This list is never set to null.
        /// </summary>
        IndividualCategoryList IndividualCategories { get; set; }

        /// <summary>
        /// List of periods selected by user.
        /// This list is never set to null.
        /// </summary>
        PeriodList Periods { get; set; }

        /// <summary>
        /// List of periods selected by user.
        /// This list is never set to null.
        /// </summary>
        ReferenceList References { get; set; }

        /// <summary>
        /// List of taxa selected by user.
        /// This list is never set to null.
        /// </summary>
        TaxonList Taxa { get; set; }
    }
}

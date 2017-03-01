using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class holds selections that are used when
    /// a species fact data set is created or updated.
    /// </summary>
    public class SpeciesFactDataSetSelection : ISpeciesFactDataSetSelection
    {
        private FactorList _factors;
        private TaxonList _hosts;
        private IndividualCategoryList _individualCategories;
        private PeriodList _periods;
        private ReferenceList _references;
        private TaxonList _taxa;

        /// <summary>
        /// Create a species fact data set selection instance.
        /// </summary>
        public SpeciesFactDataSetSelection()
        {
            _factors = new FactorList(true);
            _hosts = new TaxonList(true);
            _individualCategories = new IndividualCategoryList(true);
            _periods = new PeriodList(true);
            _references = new ReferenceList(true);
            _taxa = new TaxonList(true);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            SpeciesFactDataSetSelection selection;

            selection = new SpeciesFactDataSetSelection();
            selection.Factors = new FactorList(true);
            selection.Factors.Merge(Factors);
            selection.Hosts = new TaxonList(true);
            selection.Hosts.Merge(Hosts);
            selection.IndividualCategories = new IndividualCategoryList(true);
            selection.IndividualCategories.Merge(IndividualCategories);
            selection.Periods = new PeriodList(true);
            selection.Periods.Merge(Periods);
            selection.References = new ReferenceList(true);
            selection.References.Merge(References);
            selection.Taxa = new TaxonList(true);
            selection.Taxa.Merge(Taxa);

            return selection;
        }
        
        /// <summary>
        /// List of factors selected by user.
        /// This list is never set to null.
        /// </summary>
        public FactorList Factors
        {
            get { return _factors; }
            set
            {
                _factors.Clear();
                _factors.Merge(value);
            }
        }

        /// <summary>
        /// Indication of whether or not the species fact data set
        /// selection holds any selection of factors.
        /// </summary>
        public Boolean HasFactors
        {
            get { return _factors.IsNotEmpty(); }
        }

        /// <summary>
        /// Indication of whether or not the species fact data set
        /// selection holds any selection of hosts.
        /// </summary>
        public Boolean HasHosts
        {
            get { return _hosts.IsNotEmpty(); }
        }

        /// <summary>
        /// Indication of whether or not the species fact data set
        /// selection holds any selection of individual categories.
        /// </summary>
        public Boolean HasIndividualCategories
        {
            get { return _individualCategories.IsNotEmpty(); }
        }

        /// <summary>
        /// Indication of whether or not the species fact data set
        /// selection holds any selection of periods.
        /// </summary>
        public Boolean HasPeriods
        {
            get { return _periods.IsNotEmpty(); }
        }
        /// <summary>
        /// Indication of whether or not the species fact data set
        /// selection holds any selection of references.
        /// </summary>
        public Boolean HasReferences
        {
            get { return _references.IsNotEmpty(); }
        }
        /// <summary>
        /// Indication of whether or not the species fact data set
        /// selection holds any selection of taxa.
        /// </summary>
        public Boolean HasTaxa
        {
            get { return _taxa.IsNotEmpty(); }
        }

        /// <summary>
        /// List of host taxa selected by user.
        /// This list is never set to null.
        /// </summary>
        public TaxonList Hosts
        {
            get { return _hosts; }
            set
            {
                _hosts.Clear();
                _hosts.Merge(value);
            }
        }

        /// <summary>
        /// List of individual categories selected by user.
        /// This list is never set to null.
        /// </summary>
        public IndividualCategoryList IndividualCategories
        {
            get { return _individualCategories; }
            set
            {
                _individualCategories.Clear();
                _individualCategories.Merge(value);
            }
        }

        /// <summary>
        /// List of periods selected by user.
        /// This list is never set to null.
        /// </summary>
        public PeriodList Periods
        {
            get { return _periods; }
            set
            {
                _periods.Clear();
                _periods.Merge(value);
            }
        }


        /// <summary>
        /// List of references selected by user.
        /// This list is never set to null.
        /// </summary>
        public ReferenceList References
        {
            get { return _references; }
            set
            {
                _references.Clear();
                _references.Merge(value);
            }
        }

        /// <summary>
        /// List of taxa selected by user.
        /// This list is never set to null.
        /// </summary>
        public TaxonList Taxa
        {
            get { return _taxa; }
            set
            {
                _taxa.Clear();
                _taxa.Merge(value);
            }
        }
    }
}

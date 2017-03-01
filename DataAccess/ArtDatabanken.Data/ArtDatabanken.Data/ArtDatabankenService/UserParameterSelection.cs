using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class holds all parameter lists selected by a user.
    /// </summary>
    [Serializable]
    public class UserParameterSelection
    {
        private FactorList _factors;
        private TaxonList _hosts;
        private IndividualCategoryList _individualCategories;
        private PeriodList _periods;
        private ReferenceList _references;
        private TaxonList _taxa;

        /// <summary>
        /// Create a user parameter selection instance.
        /// </summary>
        public UserParameterSelection()
        {
            _factors = new FactorList(true);
            _hosts = new TaxonList(true);
            _individualCategories = new IndividualCategoryList(true);
            _periods = new PeriodList(true);
            _references = new ReferenceList(true);
            _taxa = new TaxonList(true);
        }
        
        /// <summary>
        /// Private constructor. Makes a clone of a UserParameterSelection.
        /// </summary>
        /// <param name="userParameterSelection">UserParameterSelection to clone</param>
        private UserParameterSelection(UserParameterSelection userParameterSelection)
        {
            _factors = new FactorList(true);
            _factors.AddRange(userParameterSelection.Factors);
            _hosts = new TaxonList(true);
            _hosts.AddRange(userParameterSelection.Hosts);
            _individualCategories = new IndividualCategoryList(true);
            _individualCategories.AddRange(userParameterSelection.IndividualCategories);
            _periods = new PeriodList(true);
            _periods.AddRange(userParameterSelection.Periods);
            _references = new ReferenceList(true);
            _references.AddRange(userParameterSelection.References);
            _taxa = new TaxonList(true);
            _taxa.AddRange(userParameterSelection.Taxa);
        }

        /// <summary>
        /// List of factors selected by user
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
        /// Indication of whether or not the user parameter selection holds any selection of factors
        /// </summary>
        public Boolean HasFactors
        {
            get { return _factors.IsNotEmpty(); }
        }

        /// <summary>
        /// Indication of whether or not the user parameter selection holds any selection of hosts
        /// </summary>
        public Boolean HasHosts
        {
            get { return _hosts.IsNotEmpty(); }
        }

        /// <summary>
        /// Indication of whether or not the user parameter selection holds any selection of individual categories
        /// </summary>
        public Boolean HasIndividualCategories
        {
            get { return _individualCategories.IsNotEmpty(); }
        }

        /// <summary>
        /// Indication of whether or not the user parameter selection holds any selection of periods
        /// </summary>
        public Boolean HasPeriods
        {
            get { return _periods.IsNotEmpty(); }
        }

        /// <summary>
        /// Indication of whether or not the user parameter selection holds any selection of references
        /// </summary>
        public Boolean HasReferences
        {
            get { return _references.IsNotEmpty(); }
        }

        /// <summary>
        /// Indication of whether or not the user parameter selection holds any selection of taxa
        /// </summary>
        public Boolean HasTaxa
        {
            get { return _taxa.IsNotEmpty(); }
        }

        /// <summary>
        /// List of host taxa selected by user
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
        /// List of periods selected by user
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
        /// List of references selected by user
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
        /// <summary>
        /// Creates a clone of the UserParameterSelection.
        /// </summary>
        /// <returns>The clone of the UserParameterSelection.</returns>
        public UserParameterSelection Clone()
        {
            return new UserParameterSelection(this);
        }
    }
}

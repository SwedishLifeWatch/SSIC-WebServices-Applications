using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class holds all parameter lists selected by a user in combination with all corresponding species fact items
    /// </summary>
    [Serializable]
    public class UserDataSet
    {
        private UserParameterSelection _parameters;
        private SpeciesFactList _speciesFacts;

        /// <summary>
        /// Create a user data set instance.
        /// </summary>
        public UserDataSet()
        {
            _parameters = new UserParameterSelection();
            _speciesFacts = new SpeciesFactList();
        }

        /// <summary>
        /// The sets of parameters selected by user.
        /// Parameters is never set to null.
        /// </summary>
        public UserParameterSelection Parameters
        {
            get { return _parameters; }
            set
            {
                if (value.IsNull())
                {
                    _parameters = new UserParameterSelection();
                }
                else
                {
                    _parameters = value.Clone();
                }
            }
        }

        /// <summary>
        /// List of species facts corresponding to all combination of taxa, individual categories, factors, hosts and periods listed in the parameters in the user data set.
        /// This list is never set to null.
        /// </summary>
        public SpeciesFactList SpeciesFacts
        {
            get { return _speciesFacts; }
            set
            {
                if (value.IsNull())
                {
                    _speciesFacts = new SpeciesFactList();
                }
                else
                {
                    _speciesFacts = value;
                }
            }
        }

        /// <summary>
        /// List of taxa in the user data set.
        /// This list is never set to null.
        /// </summary>
        public TaxonList Taxa
        {
            get { return _parameters.Taxa; }
            set { _parameters.Taxa = value; }
        }

        /// <summary>
        /// List of individual categories in the user data set.
        /// This list is never set to null.
        /// </summary>
        public IndividualCategoryList IndividualCategories
        {
            get { return _parameters.IndividualCategories; }
            set { _parameters.IndividualCategories = value; }
        }

        /// <summary>
        /// List of factors in the user data set.
        /// This list is never set to null.
        /// </summary>
        public FactorList Factors
        {
            get { return _parameters.Factors; }
            set { _parameters.Factors = value; }
        }

        /// <summary>
        /// List of hosts in the user data set.
        /// This list is never set to null.
        /// </summary>
        public TaxonList Hosts
        {
            get
            {
                _parameters.Hosts.Sort();
                return _parameters.Hosts;
            }
            set { _parameters.Hosts = value; }
        }

        /// <summary>
        /// List of periods in the user data set.
        /// This list is never set to null.
        /// </summary>
        public PeriodList Periods
        {
            get { return _parameters.Periods; }
            set { _parameters.Periods = value; }
        }

        /// <summary>
        /// List of references in the user data set.
        /// This list is never set to null.
        /// </summary>
        public ReferenceList References
        {
            get { return _parameters.References; }
            set { _parameters.References = value; }
        }

        /// <summary>
        /// Indication of whether or not the user dataset contains any taxa
        /// </summary>
        public Boolean HasTaxa
        {
            get { return _parameters.HasTaxa; }
        }

        /// <summary>
        /// Indication of whether or not the user dataset contains any individual categories
        /// </summary>
        public Boolean HasIndividualCategories
        {
            get { return _parameters.HasIndividualCategories; }
        }

        /// <summary>
        /// Indication of whether or not the user dataset contains any factors
        /// </summary>
        public Boolean HasFactors
        {
            get { return _parameters.HasFactors; }
        }

        /// <summary>
        /// Indication of whether or not the user dataset contains any hosts
        /// </summary>
        public Boolean HasHosts
        {
            get { return _parameters.HasHosts; }
        }

        /// <summary>
        /// Indication of whether or not the user dataset contains any periods
        /// </summary>
        public Boolean HasPeriods
        {
            get { return _parameters.HasPeriods; }
        }

        /// <summary>
        /// Indication of whether or not the user dataset contains any periods
        /// </summary>
        public Boolean HasReferences
        {
            get { return _parameters.HasReferences; }
        }

        /// <summary>
        /// Indication of whether or not the user dataset contains any species facts
        /// </summary>
        public Boolean HasSpeciesFacts
        {
            get { return _speciesFacts.IsNotEmpty(); }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter
{
    /// <summary>
    /// This class stores filtered field settings.
    /// </summary>
    [DataContract]
    public sealed class FieldSetting : SettingBase
    {
        private ObservableCollection<SpeciesObservationFieldSearchCriteria> _filteredfieldExpressions;

        private bool _isActive;

        /// <summary>
        /// Gets or sets whether field setting is active or not.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is checked; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsActive
        {
            get
            {
                return _isActive;
            }

            set
            {
                _isActive = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        /// <summary>
        /// The filtered fields.
        /// </summary>
        [DataMember]
        public ObservableCollection<SpeciesObservationFieldSearchCriteria> FieldFilterExpressions
        {
            get
            {
                if (_filteredfieldExpressions.IsNull())
                {
                    _filteredfieldExpressions = new ObservableCollection<SpeciesObservationFieldSearchCriteria>();
                    _filteredfieldExpressions.CollectionChanged += (sender, args) => { ResultCacheNeedsRefresh = true; };
                }
                return _filteredfieldExpressions;
            }
            set
            {
                _filteredfieldExpressions = value;
                ResultCacheNeedsRefresh = true;

                if (_filteredfieldExpressions.IsNotNull())
                {
                    _filteredfieldExpressions.CollectionChanged += (sender, args) => { ResultCacheNeedsRefresh = true; };
                }
            }
        }

        /// <summary>
        /// The logical operator that combines the fields as an expression.
        /// </summary>
        [DataMember]
        public LogicalOperator FieldLogicalOperator
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates whether this instance has values set or not (ie. FieldIds)
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has values; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSettings
        {
            get { return !FieldFilterExpressions.IsEmpty(); }
        }

        /// <summary>
        /// Adds field search criteria to the search criteria.
        /// </summary>
        /// <param name="speciesObservationFieldSearchCriteria">The field search criteria.</param>
        public void AddFieldFilterExpressions(SpeciesObservationFieldSearchCriteria speciesObservationFieldSearchCriteria)
        {
            int countBefore = FieldFilterExpressions.Count;
            if (speciesObservationFieldSearchCriteria.IsNull())
            {
                return;
            }

            if (!IsActive && !HasSettings)
            {
                IsActive = true;
            }

            var newList = new List<SpeciesObservationFieldSearchCriteria>(FieldFilterExpressions);
            newList.Add(speciesObservationFieldSearchCriteria);
            IEnumerable<SpeciesObservationFieldSearchCriteria> distinctList = newList.Distinct();
            var distinctCollection = new ObservableCollection<SpeciesObservationFieldSearchCriteria>();

            foreach (var filter in distinctList)
            {
                distinctCollection.Add(filter);
            }
            FieldFilterExpressions = distinctCollection;

            int countAfter = FieldFilterExpressions.Count;
            
            if (!IsActive && (countAfter > countBefore))
            {
                IsActive = true;
            }
        }

        public override bool IsSettingsDefault()
        {
            return FieldFilterExpressions.Count == 0;
        }

        public void ResetSettings()
        {
            FieldFilterExpressions.Clear();
            FieldLogicalOperator = LogicalOperator.And;
        }
    }
}

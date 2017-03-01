using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using ArtDatabanken.Data;
using WmsCapabilites_1_3_0;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter
{
    /// <summary>
    /// This class stores filtered taxa settings
    /// </summary>
    [DataContract]
    public sealed class TaxaSetting : SettingBase
    {
        private ObservableCollection<int> _taxonIds;
        private bool _isActive;

        /// <summary>
        /// Gets or sets whether TaxaSetting is active or not.
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
        /// The filtered taxon ids
        /// </summary>
        [DataMember]
        public ObservableCollection<int> TaxonIds
        {
            get
            {
                if (_taxonIds.IsNull())
                {
                    _taxonIds = new ObservableCollection<int>();
                    _taxonIds.CollectionChanged += (sender, args) => { ResultCacheNeedsRefresh = true; };
                }
                return _taxonIds;
            }
            set
            {                
                _taxonIds = value;                
                ResultCacheNeedsRefresh = true;

                if (_taxonIds.IsNotNull())
                {
                    _taxonIds.CollectionChanged += (sender, args) => { ResultCacheNeedsRefresh = true; };
                }
            }
        }

        /// <summary>
        /// Indicates whether this instance has values set or not (ie. TaxonIds)
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has values; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSettings
        {
            get { return !TaxonIds.IsEmpty(); }
        }

        /// <summary>
        /// Gets the number of selected taxa.
        /// </summary>
        public int NumberOfSelectedTaxa
        {
            get
            {                
                if (TaxonIds.IsNull())
                {
                    return 0;
                }

                return TaxonIds.Count;
            }
        }

        /// <summary>
        /// Adds taxon id to the TaxonIds list.
        /// </summary>
        /// <param name="taxonId">The taxon id.</param>
        public void AddTaxonId(int taxonId)
        {
            int countBefore = TaxonIds.Count;            
            TaxonIds.Add(taxonId);
            IEnumerable<int> distinctList = TaxonIds.Distinct();
            var distinctCollection = new ObservableCollection<int>();
            foreach (int id in distinctList)
            {
                distinctCollection.Add(id);
            }
            TaxonIds = distinctCollection;
            int countAfter = TaxonIds.Count;
            if (!IsActive && (countAfter > countBefore))
            {
                IsActive = true;
            }
        }

        /// <summary>
        /// Adds taxon ids to the TaxonIds list.
        /// </summary>
        /// <param name="taxonIds">The taxon ids.</param>
        public void AddTaxonIds(IEnumerable<int> taxonIds)
        {
            int countBefore = TaxonIds.Count;
            if (taxonIds.IsNull())
            {
                return;
            }

            if (!IsActive && !HasSettings)
            {
                IsActive = true;
            }

            var newList = new List<int>(TaxonIds);
            newList.AddRange(taxonIds);
            IEnumerable<int> distinctList = newList.Distinct();
            var distinctCollection = new ObservableCollection<int>();
            foreach (int id in distinctList)
            {
                distinctCollection.Add(id);
            }
            TaxonIds = distinctCollection;
            int countAfter = TaxonIds.Count;
            if (!IsActive && (countAfter > countBefore))
            {
                IsActive = true;
            }
        }

        /// <summary>
        /// Removes the taxon id from the TaxonIds list.
        /// </summary>
        /// <param name="taxonId">The taxon id.</param>
        public void RemoveTaxonId(int taxonId)
        {
            if (TaxonIds.IsNull())
            {
                return;
            }

            TaxonIds.Remove(taxonId);            
        }

        /// <summary>
        /// Removes the taxon ids from the TaxonIds list.
        /// </summary>
        /// <param name="taxonIds">The taxon ids.</param>
        public void RemoveTaxonIds(int[] taxonIds)
        {
            if (TaxonIds.IsNull())
            {
                return;
            }

            foreach (int taxonId in taxonIds)
            {
                TaxonIds.Remove(taxonId);
            }
        }

        public override bool IsSettingsDefault()
        {
            return TaxonIds.Count == 0;
        }

        public void ResetSettings()
        {
            TaxonIds.Clear();
        }
    }
}

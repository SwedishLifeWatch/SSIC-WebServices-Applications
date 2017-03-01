using System;
using System.Collections;
using System.Collections.Generic;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
#if IS_DATA_QUERY_IMPLEMENTED
    /// <summary>
    /// List class for the DataIdentifier class.
    /// </summary>
    public class DataIdentifierList : ArrayList
    {
        /// <summary>
        /// Add a data identifier to the list.
        /// </summary>
        /// <param name='dataIdentifier'>A data identifier.</param>
        private void Add(DataIdentifier dataIdentifier)
        {
            base.Add(dataIdentifier);
        }

        /// <summary>
        /// Add a factor to the data identifier list.
        /// </summary>
        /// <param name='factor'>A factor.</param>
        public void Add(Factor factor)
        {
            if (factor.IsNotNull())
            {
                Add(new DataIdentifier(factor));
            }
        }

        /// <summary>
        /// Add a factor field enum value to the data identifier list.
        /// </summary>
        /// <param name='factorFieldEnumValue'>A factor field enum value.</param>
        public void Add(FactorFieldEnumValue factorFieldEnumValue)
        {
            if (factorFieldEnumValue.IsNotNull())
            {
                Add(new DataIdentifier(factorFieldEnumValue));
            }
        }

        /// <summary>
        /// Add a individual category to the data identifier list.
        /// </summary>
        /// <param name='individualCategory'>An individual category.</param>
        public void Add(IndividualCategory individualCategory)
        {
            if (individualCategory.IsNotNull())
            {
                Add(new DataIdentifier(individualCategory));
            }
        }

        /// <summary>
        /// Add a period to the data identifier list.
        /// </summary>
        /// <param name='period'>A period.</param>
        public void Add(Period period)
        {
            if (period.IsNotNull())
            {
                Add(new DataIdentifier(period));
            }
        }

        /// <summary>
        /// Add a reference to the data identifier list.
        /// </summary>
        /// <param name='reference'>A reference.</param>
        public void Add(Reference reference)
        {
            if (reference.IsNotNull())
            {
                Add(new DataIdentifier(reference));
            }
        }

        /// <summary>
        /// Add a taxon to the data identifier list.
        /// Data type is added to distinguish between taxon and host.
        /// Default is taxon.
        /// </summary>
        /// <param name='taxon'>A taxon.</param>
        public void Add(Taxon taxon)
        {
            this.Add(taxon, DataTypeId.Taxon);
        }

        /// <summary>
        /// Add a taxon to the data identifier list.
        /// Data type is needed to distinguish between taxon and host.
        /// Default is taxon.
        /// </summary>
        /// <param name='taxon'>A taxon.</param>
        /// <param name='dataType'>
        /// Data type is needed to distinguish between taxon and host.
        /// Default is taxon.
        /// </param>
        public void Add(Taxon taxon, DataTypeId dataType)
        {
            if (taxon.IsNotNull())
            {
                Add(new DataIdentifier(taxon, dataType));
            }
        }

        /// <summary>
        /// Hide base class implementation of Add.
        /// This list class only supports specific data types.
        /// Generic data types are not supported.
        /// </summary>
        /// <param name='value'>A value.</param>
        public override int Add(object value)
        {
            throw new NotSupportedException("DataIdentifierList does not support generic data.");
        }

        /// <summary>
        /// Add factors to the data identifier list.
        /// </summary>
        /// <param name='factors'>Factors.</param>
        public void AddRange(FactorList factors)
        {
            if (factors.IsNotEmpty())
            {
                foreach (Factor factor in factors)
                {
                    Add(factor);
                }
            }
        }

        /// <summary>
        /// Add factor field enum values to the data identifier list.
        /// </summary>
        /// <param name='factorFieldEnumValues'>Factor field enum values.</param>
        public void AddRange(FactorFieldEnumValueList factorFieldEnumValues)
        {
            if (factorFieldEnumValues.IsNotEmpty())
            {
                foreach (FactorFieldEnumValue factorFieldEnumValue in factorFieldEnumValues)
                {
                    Add(factorFieldEnumValue);
                }
            }
        }

        /// <summary>
        /// Add individual categories to the data identifier list.
        /// </summary>
        /// <param name='individualCategories'>Individual categories.</param>
        public void AddRange(IndividualCategoryList individualCategories)
        {
            if (individualCategories.IsNotEmpty())
            {
                foreach (IndividualCategory individualCategory in individualCategories)
                {
                    Add(individualCategory);
                }
            }
        }

        /// <summary>
        /// Add periods to the data identifier list.
        /// </summary>
        /// <param name='periods'>Periods.</param>
        public void AddRange(PeriodList periods)
        {
            if (periods.IsNotEmpty())
            {
                foreach (Period period in periods)
                {
                    Add(period);
                }
            }
        }

        /// <summary>
        /// Add references to the data identifier list.
        /// </summary>
        /// <param name='references'>References.</param>
        public void AddRange(ReferenceList references)
        {
            if (references.IsNotEmpty())
            {
                foreach (Reference reference in references)
                {
                    Add(reference);
                }
            }
        }

        /// <summary>
        /// Add taxa to the data identifier list.
        /// Data type is added to distinguish between taxon and host.
        /// Default is taxon.
        /// </summary>
        /// <param name='taxa'>Taxa.</param>
        public void AddRange(TaxonList taxa)
        {
            if (taxa.IsNotEmpty())
            {
                foreach (Taxon taxon in taxa)
                {
                    Add(taxon);
                }
            }
        }

        /// <summary>
        /// Add taxa to the data identifier list.
        /// Data type is needed to distinguish between taxon and host.
        /// Default is taxon.
        /// </summary>
        /// <param name='taxa'>Taxa.</param>
        /// <param name='dataType'>
        /// Data type is needed to distinguish between taxon and host.
        /// Default is taxon.
        /// </param>
        public void AddRange(TaxonList taxa, DataTypeId dataType)
        {
            if (taxa.IsNotEmpty())
            {
                foreach (Taxon taxon in taxa)
                {
                    Add(taxon, dataType);
                }
            }
        }

        /// <summary>
        /// Add user parameter selection to the data identifier list.
        /// </summary>
        /// <param name='userParameterSelection'>A user parameter selection.</param>
        public void AddRange(UserParameterSelection userParameterSelection)
        {
            if (userParameterSelection.IsNotNull())
            {
                AddRange(userParameterSelection.Factors);
                AddRange(userParameterSelection.Hosts, DataTypeId.Host);
                AddRange(userParameterSelection.IndividualCategories);
                AddRange(userParameterSelection.Periods);
                AddRange(userParameterSelection.References);
                AddRange(userParameterSelection.Taxa);
            }
        }

        /// <summary>
        /// Hide base class implementation of AddRange.
        /// This list class only supports specific data types.
        /// Generic data types are not supported.
        /// </summary>
        /// <param name='c'>A collection.</param>
        public override void AddRange(ICollection c)
        {
            throw new NotSupportedException("DataIdentifierList does not support generic data.");
        }

        /// <summary>
        /// Get/set DataIdentifier by list index.
        /// </summary>
        public new DataIdentifier this[Int32 index]
        {
            get
            {
                return (DataIdentifier)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
#endif
}

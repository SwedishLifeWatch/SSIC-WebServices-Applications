using System;
using System.Collections.Generic;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
#if IS_DATA_QUERY_IMPLEMENTED
    /// <summary>
    /// Identifies one instance of data.
    /// This class is used in data query handling.
    /// String is used to contain the data id, instead of
    /// Int32, since string is more generic and can handle 
    /// more cases.
    /// </summary>
    public class DataIdentifier : ArtDatabankenBase
    {
        private DataTypeId _dataType = DataTypeId.NoDataType;
        private Object _data;
        private String _identifier;

        /// <summary>
        /// Create a DataIdentifier instance with information
        /// from a factor.
        /// </summary>
        /// <param name='factor'>A factor.</param>
        public DataIdentifier(Factor factor)
        {
            _data = factor;
            _dataType = DataTypeId.Factor;
            _identifier = factor.Id.ToString();
        }

        /// <summary>
        /// Create a DataIdentifier instance with information from a
        /// factor field enum value.
        /// </summary>
        /// <param name='factorFieldEnumValue'>A factor field enum value.</param>
        public DataIdentifier(FactorFieldEnumValue factorFieldEnumValue)
        {
            _data = factorFieldEnumValue;
            _dataType = DataTypeId.FactorFieldEnumValue;
            _identifier = factorFieldEnumValue.Id.ToString();
        }

        /// <summary>
        /// Create a DataIdentifier instance with information from a
        /// individual category.
        /// </summary>
        /// <param name='individualCategory'>An individual category.</param>
        public DataIdentifier(IndividualCategory individualCategory)
        {
            _data = individualCategory;
            _dataType = DataTypeId.IndividualCategory;
            _identifier = individualCategory.Id.ToString();
        }

        /// <summary>
        /// Create a DataIdentifier instance with information
        /// from a period.
        /// </summary>
        /// <param name='period'>A period.</param>
        public DataIdentifier(Period period)
        {
            _data = period;
            _dataType = DataTypeId.Period;
            _identifier = period.Id.ToString();
        }

        /// <summary>
        /// Create a DataIdentifier instance with information
        /// from a reference.
        /// </summary>
        /// <param name='reference'>A reference.</param>
        public DataIdentifier(Reference reference)
        {
            _data = reference;
            _dataType = DataTypeId.Reference;
            _identifier = reference.Id.ToString();
        }

        /// <summary>
        /// Create a DataIdentifier instance with information from
        /// a taxon.
        /// Data type is added to distinguish between taxon and host.
        /// Default is taxon.
        /// </summary>
        /// <param name='taxon'>A taxon.</param>
        public DataIdentifier(Taxon taxon)
            : this (taxon, DataTypeId.Taxon)
        {
        }

        /// <summary>
        /// Create a DataIdentifier instance with information from
        /// a taxon.
        /// </summary>
        /// <param name='taxon'>A taxon.</param>
        /// <param name='dataType'>
        /// Data type is needed to distinguish between taxon and host.
        /// Default is taxon.
        /// </param>
        public DataIdentifier(Taxon taxon, DataTypeId dataType)
        {
            _data = taxon;
            _dataType = dataType;
            _identifier = taxon.Id.ToString();
        }

        /// <summary>
        /// Get data type for this identifier.
        /// </summary>
        public DataTypeId DataType
        {
            get { return _dataType; }
        }

        /// <summary>
        /// Get identifier for the data.
        /// String is used to contain the data id, instead of Int32,
        /// since string is more generic and can handle more cases.
        /// </summary>
        public String Identifier
        {
            get { return _identifier; }
        }
    }
#endif
}

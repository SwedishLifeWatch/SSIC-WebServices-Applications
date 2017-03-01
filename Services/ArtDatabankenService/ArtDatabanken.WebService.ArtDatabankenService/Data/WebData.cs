using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Id for data types that are used in the web service interface.
    /// </summary>
    [DataContract]
    public enum DataTypeId
    {
        /// <summary>
        /// Factor.
        /// </summary>
        [EnumMember]
        Factor,
        /// <summary>
        /// FactorFieldEnumValue, one value of many in a FactorFieldEnum.
        /// </summary>
        [EnumMember]
        FactorFieldEnumValue,
        /// <summary>
        /// Host, a taxon that is a host for another taxon.
        /// </summary>
        [EnumMember]
        Host,
        /// <summary>
        /// IndividualCategory.
        /// </summary>
        [EnumMember]
        IndividualCategory,
        /// <summary>
        /// NoDataType.
        /// </summary>
        [EnumMember]
        NoDataType,
        /// <summary>
        /// Period.
        /// </summary>
        [EnumMember]
        Period,
        /// <summary>
        /// Reference.
        /// </summary>
        [EnumMember]
        Reference,
        /// <summary>
        /// SpeciesFact.
        /// </summary>
        [EnumMember]
        SpeciesFact,
        /// <summary>
        /// SpeciesObservation.
        /// </summary>
        [EnumMember]
        SpeciesObservation,
        /// <summary>
        /// Taxon.
        /// </summary>
        [EnumMember]
        Taxon
    }

    /// <summary>
    /// Alternative name search methods
    /// </summary>
    [DataContract]
    public enum SearchStringComparisonMethod
    {
        /// <summary>
        /// Search for strings that contains the specified
        /// search string. Wild chards are added to the search string
        /// (at the beginning and end) before the search begins.
        /// </summary>
        [EnumMember]
        Contains,

        /// <summary>
        /// Match search string exact.
        /// </summary>
        [EnumMember]
        Exact,

        /// <summary>
        /// Try exact match and then all possible matches
        /// if no exact match was found.
        /// </summary>
        [EnumMember]
        ExactOrAll,

        /// <summary>
        /// Try different match methods until match is found.
        /// </summary>
        [EnumMember]
        Iterative,

        /// <summary>
        /// Match similary search strings.
        /// </summary>
        [EnumMember]
        Like
    }

    /// <summary>
    /// Contains support for dynamic adding of data to WebXXX 
    /// data classes without breaking the web service interface.
    /// </summary>
    [DataContract]
    public class WebData
    {
        /// <summary>
        /// Constant that defines an id value for 
        /// data that has no id.
        /// </summary>
        public const Int32 NO_ID = Int32.MinValue;

        /// <summary>
        /// Create a WebData instance.
        /// </summary>
        public WebData()
        {
            DataFields = null;
        }

        /// <summary>Data fields.</summary>
        [DataMember]
        public List<WebDataField> DataFields
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        public virtual void CheckData()
        {
            if (DataFields.IsNotEmpty())
            {
                foreach (WebDataField dataField in DataFields)
                {
                    dataField.CheckData();
                }
            }
        }

        /// <summary>
        /// Get Boolean value from data field with the specified name.
        /// </summary>
        /// <param name='name'>Name of data field.</param>
        /// <returns>The Boolean value.</returns>
        /// <exception cref="ArgumentException">Thrown if no data field with the specified name was found.</exception>
        protected Boolean GetDataBoolean(String name)
        {
            WebDataField dataField;

            dataField = GetDataField(name);
            if (dataField.IsNotNull() &&
                (dataField.Type == WebDataType.Boolean))
            {
                return dataField.Value.WebParseBoolean();
            }
            throw new ArgumentNullException("No data field with name and type " + name + " was found.");
        }

        /// <summary>
        /// Get a data field with the specified name.
        /// </summary>
        /// <param name='name'>Name of data field to get.</param>
        /// <returns>The requested data field.</returns>
        protected WebDataField GetDataField(String name)
        {
            if (DataFields.IsNotEmpty())
            {
                foreach (WebDataField dataField in DataFields)
                {
                    if (dataField.Name == name)
                    {
                        return dataField;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Test if this object has a data field with the specified name.
        /// </summary>
        /// <param name='name'>Name of data field to search for.</param>
        /// <returns>True of data field was found.</returns>
        protected Boolean HasDataField(String name)
        {
            return GetDataField(name).IsNotNull();
        }

        /// <summary>
        /// Load all unread columns as WebDataFields into this WebData.
        /// </summary>
        /// <param name='dataReader'>An open DataReader.</param>
        public void LoadData(DataReader dataReader)
        {
            if (dataReader.NextUnreadColumn())
            {
                DataFields = new List<WebDataField>();
                do
                {
                    DataFields.Add(new WebDataField(dataReader));
                }
                while (dataReader.NextUnreadColumn());
            }
        }

        /// <summary>
        /// Set Boolean value in data field with the specified name.
        /// </summary>
        /// <param name='name'>Name of data field.</param>
        /// <param name='value'>Value to set in data field.</param>
        /// <exception cref="ArgumentException">Thrown if no data field with the specified name was found.</exception>
        protected void SetDataBoolean(String name, Boolean value)
        {
            WebDataField dataField;

            dataField = GetDataField(name);
            if (dataField.IsNotNull() &&
                (dataField.Type == WebDataType.Boolean))
            {
                dataField.Value = value.WebToString();
            }
            else
            {
                throw new ArgumentNullException("No data field with name and type " + name + " was found.");
            }
        }
    }
}

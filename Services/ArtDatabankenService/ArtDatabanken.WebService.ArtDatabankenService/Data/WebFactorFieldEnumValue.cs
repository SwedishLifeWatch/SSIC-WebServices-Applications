using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    ///  This class represents a factor field enum value.
    /// </summary>
    [DataContract]
    public class WebFactorFieldEnumValue : WebData
    {
        /// <summary>
        /// Create a WebFactorFieldEnumValue instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebFactorFieldEnumValue(DataReader dataReader)
        {
            Id = dataReader.GetInt32(FactorFieldEnumValueData.ID);
            FactorFieldEnumId = dataReader.GetInt32(FactorFieldEnumValueData.FACTOR_FIELD_ENUM_ID);
            KeyText = dataReader.GetString(FactorFieldEnumValueData.KEY_TEXT);
            IsKeyIntegerSpecified = dataReader.IsNotDBNull(FactorFieldEnumValueData.KEY_INT);
            if (IsKeyIntegerSpecified)
            {
                KeyInteger = dataReader.GetInt32(FactorFieldEnumValueData.KEY_INT);
            }
            else
            {
                KeyInteger = -999;
            }
            SortOrder = dataReader.GetInt32(FactorFieldEnumValueData.SORT_ORDER);
            Label = dataReader.GetString(FactorFieldEnumValueData.LABEL);
            Information = dataReader.GetString(FactorFieldEnumValueData.INFOMATION);
            ShouldBeSaved = dataReader.GetBoolean(FactorFieldEnumValueData.SHOULD_BE_SAVED);
            base.LoadData(dataReader);
        }

        /// <summary>
        /// Id for the factor field enum which this factor field enum value belongs to.
        /// </summary>
        [DataMember]
        public Int32 FactorFieldEnumId
        { get; set; }

        /// <summary>
        /// Id for this factor field enum value.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Information and definition text for this field enum value.
        /// </summary>
        [DataMember]
        public String Information
        { get; set; }

        /// <summary>
        /// Indication about whether or not this field enum has a integer key.
        /// </summary>
        [DataMember]
        public Boolean IsKeyIntegerSpecified
        { get; set; }

        /// <summary>
        /// Key Integer Value for this field enum value.
        /// </summary>
        [DataMember]
        public Int32 KeyInteger
        { get; set; }

        /// <summary>
        /// Key Text Value for this field enum value.
        /// </summary>
        [DataMember]
        public String KeyText
        { get; set; }

        /// <summary>
        /// Label Value for this field enum value.
        /// </summary>
        [DataMember]
        public String Label
        { get; set; }

        /// <summary>
        /// Indication about whether or not this field enum value should be saved to Database.
        /// </summary>
        [DataMember]
        public Boolean ShouldBeSaved
        { get; set; }

        /// <summary>
        /// SortOrder for this field enum value.
        /// </summary>
        [DataMember]
        public Int32 SortOrder
        { get; set; }
    }
}

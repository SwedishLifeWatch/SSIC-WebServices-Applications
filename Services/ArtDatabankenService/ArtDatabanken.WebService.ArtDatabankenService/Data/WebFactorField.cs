using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    ///  This class represents a factor field.
    /// </summary>
    [DataContract]
    public class WebFactorField : WebData
    {
        /// <summary>
        /// Create a WebFactorField instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebFactorField(DataReader dataReader)
        {
            Id = dataReader.GetInt32(FactorFieldData.ID);
            FactorDataTypeId = dataReader.GetInt32(FactorFieldData.FACTOR_DATA_TYPE_ID);
            DatabaseFieldName = dataReader.GetString(FactorFieldData.DATABASE_FIELD_NAME);
            Label = dataReader.GetString(FactorFieldData.LABEL);
            Information = dataReader.GetString(FactorFieldData.INFORMATION);
            IsMain = dataReader.GetBoolean(FactorFieldData.IS_MAIN);
            IsSubstantial = dataReader.GetBoolean(FactorFieldData.IS_SUBSTANTIAL);
            TypeId = dataReader.GetInt32(FactorFieldData.FACTOR_FIELD_TYPE_ID);
            Size = dataReader.GetInt32(FactorFieldData.SIZE, -1);
            FactorFieldEnumId = dataReader.GetInt32(FactorFieldData.FACTOR_FIELD_ENUM_ID, -1);
            UnitLabel = dataReader.GetString(FactorFieldData.UNIT_LABEL);
            base.LoadData(dataReader);
        }

        /// <summary>
        /// Name for this factor field in database.
        /// </summary>
        [DataMember]
        public String DatabaseFieldName
        { get; set; }

        /// <summary>
        /// Get data type for this factor field.
        /// </summary>
        public WebDataType DataType
        {
            get
            {
                return WebFactorFieldType.GetDataType(TypeId);
            }
        }

        /// <summary>
        /// Id for this factor data type.
        /// </summary>
        [DataMember]
        public Int32 FactorDataTypeId
        { get; set; }

        /// <summary>
        /// Factor field enum id for this factor field.
        /// </summary>
        [DataMember]
        public Int32 FactorFieldEnumId
        { get; set; }

        /// <summary>
        /// Id for this factor field.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Information for this factor field.
        /// </summary>
        [DataMember]
        public String Information
        { get; set; }

        /// <summary>
        /// Test if this factor field is of type enum.
        /// </summary>
        public Boolean IsEnumField
        {
            get
            {
                return FactorFieldEnumId >= 0;
            }
        }

        /// <summary>
        /// Indicator of whether or not this factor field is the main field.
        /// </summary>
        [DataMember]
        public Boolean IsMain
        { get; set; }

        /// <summary>
        /// Indicator of whether or not this factor field is a substantial field.
        /// </summary>
        [DataMember]
        public Boolean IsSubstantial
        { get; set; }

        /// <summary>
        /// Label for this factor field.
        /// </summary>
        [DataMember]
        public String Label
        { get; set; }

        /// <summary>
        /// Maximum length of this factor field if it is of type String.
        /// </summary>
        [DataMember]
        public Int32 Size
        { get; set; }

        /// <summary>
        /// Type id for this factor field.
        /// </summary>
        [DataMember]
        public Int32 TypeId
        { get; set; }

        /// <summary>
        /// Unit label for this factor field.
        /// </summary>
        [DataMember]
        public String UnitLabel
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        public override void CheckData()
        {
            base.CheckData();
            DatabaseFieldName = DatabaseFieldName.CheckSqlInjection();
            Information = Information.CheckSqlInjection();
            Label = Label.CheckSqlInjection();
            UnitLabel = UnitLabel.CheckSqlInjection();
        }
    }
}

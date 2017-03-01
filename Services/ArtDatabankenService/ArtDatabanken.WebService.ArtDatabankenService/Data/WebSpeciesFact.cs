using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Holds information about how user selected species facts should be used.
    /// </summary>
    public enum UserSelectedSpeciesFactUsage
    {
        /// <summary>
        /// SpeciesFact is used as input to stored procedures.
        /// </summary>
        Input,
        /// <summary>
        /// SpeciesFact is used when producing output from stored procedures.
        /// </summary>
        Output
    }

    /// <summary>
    ///  This class represents a species fact.
    /// </summary>
    [DataContract]
    public class WebSpeciesFact : WebData
    {
        /// <summary>
        /// Create a WebSpeciesFact instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebSpeciesFact(DataReader dataReader)
        {
            Id = dataReader.GetInt32(SpeciesFactData.ID);
            TaxonId = dataReader.GetInt32(SpeciesFactData.TAXON_ID);
            IndividualCategoryId = dataReader.GetInt32(SpeciesFactData.INDIVIDUAL_CATEGORY_ID);
            FactorId = dataReader.GetInt32(SpeciesFactData.FACTOR_ID);
            if (dataReader.IsDBNull(SpeciesFactData.PERIOD_ID))
            {
                PeriodId = -1;
                IsPeriodSpecified = false;
            }
            else
            {
                PeriodId = dataReader.GetInt32(SpeciesFactData.PERIOD_ID);
                IsPeriodSpecified = true;
            }
            if (dataReader.IsDBNull(SpeciesFactData.HOST_ID))
            {
                HostId = -1;
                IsHostSpecified = false;
            }
            else
            {
                HostId = dataReader.GetInt32(SpeciesFactData.HOST_ID);
                IsHostSpecified = HostId > 0;
            }
           
            FieldValue1 = dataReader.GetDouble(SpeciesFactData.FIELD_VALUE_1, -99);
            FieldValue2 = dataReader.GetDouble(SpeciesFactData.FIELD_VALUE_2, -99);
            FieldValue3 = dataReader.GetDouble(SpeciesFactData.FIELD_VALUE_3, -99);

            FieldValue4 = dataReader.GetString(SpeciesFactData.FIELD_VALUE_4);
            FieldValue5 = dataReader.GetString(SpeciesFactData.FIELD_VALUE_5);

            IsFieldValue1Specified = dataReader.IsNotDBNull(SpeciesFactData.FIELD_VALUE_1);
            IsFieldValue2Specified = dataReader.IsNotDBNull(SpeciesFactData.FIELD_VALUE_2);
            IsFieldValue3Specified = dataReader.IsNotDBNull(SpeciesFactData.FIELD_VALUE_3);
            IsFieldValue4Specified = dataReader.IsNotDBNull(SpeciesFactData.FIELD_VALUE_4);
            IsFieldValue5Specified = dataReader.IsNotDBNull(SpeciesFactData.FIELD_VALUE_5);

            QualityId = dataReader.GetInt32(SpeciesFactData.QUALITY_ID, -1);
            ReferenceId = dataReader.GetInt32(SpeciesFactData.REFERENCE_ID, -1);
            UpdateUserFullName = dataReader.GetString(SpeciesFactData.UPDATE_USER_FULL_NAME);
            if (dataReader.IsDBNull(SpeciesFactData.UPDATE_DATE))
            {
                UpdateDate = new DateTime(2000, 1, 1);
            }
            else
            {
                UpdateDate = dataReader.GetDateTime(SpeciesFactData.UPDATE_DATE);
            }
            base.LoadData(dataReader);
        }

        /// <summary>
        /// Factor Id for this species fact.
        /// </summary>
        [DataMember]
        public Int32 FactorId
        { get; set; }

        /// <summary>
        /// Field value 1 of this species fact.
        /// </summary>
        [DataMember]
        public Double FieldValue1
        { get; set; }

        /// <summary>
        /// Field value 2 of this species fact.
        /// </summary>
        [DataMember]
        public Double FieldValue2
        { get; set; }

        /// <summary>
        /// Field value 3 of this species fact.
        /// </summary>
        [DataMember]
        public Double FieldValue3
        { get; set; }

        /// <summary>
        /// Field value 4 of this species fact.
        /// </summary>
        [DataMember]
        public String FieldValue4
        { get; set; }

        /// <summary>
        /// Field value 5 of this species fact.
        /// </summary>
        [DataMember]
        public String FieldValue5
        { get; set; }

        /// <summary>
        /// Host Id for this species fact.
        /// </summary>
        [DataMember]
        public Int32 HostId
        { get; set; }

        /// <summary>
        /// Id for this species fact.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Individual Category Id for this species fact.
        /// </summary>
        [DataMember]
        public Int32 IndividualCategoryId
        { get; set; }

        /// <summary>
        /// Indication about whether or not field 1 has a Value for this species fact.
        /// </summary>
        [DataMember]
        public Boolean IsFieldValue1Specified
        { get; set; }

        /// <summary>
        /// Indication about whether or not field 2 has a Value for this species fact.
        /// </summary>
        [DataMember]
        public Boolean IsFieldValue2Specified
        { get; set; }

        /// <summary>
        /// Indication about whether or not field 3 has a Value for this species fact.
        /// </summary>
        [DataMember]
        public Boolean IsFieldValue3Specified
        { get; set; }

        /// <summary>
        /// Indication about whether or not field 4 has a Value for this species fact.
        /// </summary>
        [DataMember]
        public Boolean IsFieldValue4Specified
        { get; set; }

        /// <summary>
        /// Indication about whether or not field 5 has a Value for this species fact.
        /// </summary>
        [DataMember]
        public Boolean IsFieldValue5Specified
        { get; set; }

        /// <summary>
        /// Indication about whether or not this species fact has a host value.
        /// </summary>
        [DataMember]
        public Boolean IsHostSpecified
        { get; set; }

        /// <summary>
        /// Indication about whether or not this species fact has a period value.
        /// </summary>
        [DataMember]
        public Boolean IsPeriodSpecified
        { get; set; }

        /// <summary>
        /// Period Id for this species fact.
        /// </summary>
        [DataMember]
        public Int32 PeriodId
        { get; set; }

        /// <summary>
        /// Quality Id for this species fact.
        /// </summary>
        [DataMember]
        public Int32 QualityId
        { get; set; }

        /// <summary>
        /// Reference Id for this species fact.
        /// </summary>
        [DataMember]
        public Int32 ReferenceId
        { get; set; }

        /// <summary>
        /// Taxon Id for this species fact.
        /// </summary>
        [DataMember]
        public Int32 TaxonId
        { get; set; }

        /// <summary>
        /// Update user date for this species fact.
        /// </summary>
        [DataMember]
        public DateTime UpdateDate
        { get; set; }

        /// <summary>
        /// Update user Name for this species fact.
        /// </summary>
        [DataMember]
        public String UpdateUserFullName
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public void CheckData(WebServiceContext context)
        {
            FieldValue4 = FieldValue4.CheckSqlInjection();
            FieldValue4.CheckLength(GetField4MaxLength(context));
            FieldValue5 = FieldValue5.CheckSqlInjection();
            FieldValue5.CheckLength(GetField5MaxLength(context));
            UpdateUserFullName = UpdateUserFullName.CheckSqlInjection();
            UpdateUserFullName.CheckLength(GetUpdateUserFullNameMaxLength(context));
        }

        /// <summary>
        /// Get max string length for field 4.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Max string length for field 4.</returns>
        public static Int32 GetField4MaxLength(WebServiceContext context)
        {
            return DataServer.GetColumnLength(context, DataServer.DatabaseId.SpeciesFact, SpeciesFactData.TABLE_NAME, SpeciesFactData.FIELD_VALUE_4_COLUMN);
        }

        /// <summary>
        /// Get max string length for field 5.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Max string length for field 5.</returns>
        public static Int32 GetField5MaxLength(WebServiceContext context)
        {
            return DataServer.GetColumnLength(context, DataServer.DatabaseId.SpeciesFact, SpeciesFactData.TABLE_NAME, SpeciesFactData.FIELD_VALUE_5_COLUMN);
        }

        /// <summary>
        /// Get max string length for update user full name.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Max string length for update user full name.</returns>
        public static Int32 GetUpdateUserFullNameMaxLength(WebServiceContext context)
        {
            return DataServer.GetColumnLength(context, DataServer.DatabaseId.SpeciesFact, SpeciesFactData.TABLE_NAME, SpeciesFactData.UPDATE_USER_FULL_NAME_COLUMN);
        }
    }
}

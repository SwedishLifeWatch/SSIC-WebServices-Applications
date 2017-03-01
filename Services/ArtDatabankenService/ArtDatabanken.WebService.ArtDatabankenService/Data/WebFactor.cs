using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Holds information about how user selected factors should be used.
    /// </summary>
    public enum UserSelectedFactorUsage
    {
        /// <summary>
        /// Factor is used as input to stored procedures.
        /// </summary>
        Input,
        /// <summary>
        /// Factor is used when producing output from stored procedures.
        /// </summary>
        Output
    }

    /// <summary>
    ///  This class represents a factor data type.
    /// </summary>
    [DataContract]
    public class WebFactor : WebData
    {
        /// <summary>
        /// Create a WebFactor instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebFactor(DataReader dataReader)
        {
            Id = dataReader.GetInt32(FactorData.ID);
            Name = dataReader.GetString(FactorData.NAME);
            Label = dataReader.GetString(FactorData.LABEL);
            Information = dataReader.GetString(FactorData.INFORMATION);
            SortOrder = dataReader.GetInt32(FactorData.SORT_ORDER, -1);
            HostLabel = dataReader.GetString(FactorData.HOST_LABEL);
            IsTaxonomic = HostLabel.IsNotNull();
            DefaultHostParentTaxonId = dataReader.GetInt32(FactorData.DEFAULT_HOST_PARENT_TAXON_ID, 0);
            FactorUpdateModeId = dataReader.GetInt32(FactorData.FACTOR_UPDATE_MODE_ID, -1);
            FactorDataTypeId = dataReader.GetInt32(FactorData.FACTOR_DATA_TYPE_ID, -1);
            FactorOriginId = dataReader.GetInt32(FactorData.FACTOR_ORIGIN_ID, -1);
            IsPublic = dataReader.GetBoolean(FactorData.IS_PUBLIC, false);
            IsPeriodic = dataReader.GetBoolean(FactorData.IS_PERIODIC, false);
            IsLeaf = dataReader.GetBoolean(FactorData.IS_LEAF, false);
            base.LoadData(dataReader);
        }

        /// <summary>
        /// Taxon id for parent taxon for all potential hosts associated with this factor.
        /// </summary>
        [DataMember]
        public Int32 DefaultHostParentTaxonId
        { get; set; }

        /// <summary>
        /// Id of the Factor data type of this factor.
        /// </summary>
        [DataMember]
        public Int32 FactorDataTypeId
        { get; set; }

        /// <summary>
        /// Id of the Factor origin of this factor.
        /// </summary>
        [DataMember]
        public Int32 FactorOriginId
        { get; set; }

        /// <summary>
        /// Id of the Factor update mode of this factor.
        /// </summary>
        [DataMember]
        public Int32 FactorUpdateModeId
        { get; set; }

        /// <summary>
        /// HostLabel for this factor.
        /// </summary>
        [DataMember]
        public String HostLabel
        { get; set; }

        /// <summary>
        /// Id for this factor.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Information about this factor.
        /// </summary>
        [DataMember]
        public String Information
        { get; set; }

        /// <summary>
        /// Indicates if this factor is a leaf in the factor tree.
        /// </summary>
        [DataMember]
        public Boolean IsLeaf
        { get; set; }

        /// <summary>
        /// Indication about whether or not this factor is periodic.
        /// </summary>
        [DataMember]
        public Boolean IsPeriodic
        { get; set; }

        /// <summary>
        /// Indication about whether or not this factor should be available for public use.
        /// </summary>
        [DataMember]
        public Boolean IsPublic
        { get; set; }

        /// <summary>
        /// Indication about whether or not this factor can be associated with a host taxon.
        /// </summary>
        [DataMember]
        public Boolean IsTaxonomic
        { get; set; }

        /// <summary>
        /// Label for this factor.
        /// </summary>
        [DataMember]
        public String Label
        { get; set; }

        /// <summary>
        /// Name for this factor.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Sort order for this factor.
        /// </summary>
        [DataMember]
        public Int32 SortOrder
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        public override void CheckData()
        {
            base.CheckData();
            HostLabel = HostLabel.CheckSqlInjection();
            Information = Information.CheckSqlInjection();
            Label = Label.CheckSqlInjection();
            Name = Name.CheckSqlInjection();
        }
    }
}

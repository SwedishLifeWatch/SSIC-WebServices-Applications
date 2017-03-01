using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Holds information about how user selected individual categories should be used.
    /// </summary>
    public enum UserSelectedIndividualCategoryUsage
    {
        /// <summary>
        /// Individual category is used as input to stored procedures.
        /// </summary>
        Input,
        /// <summary>
        /// Individual category is used when producing output from stored procedures.
        /// </summary>
        Output
    }

    /// <summary>
    /// This class represents a individual category.
    /// </summary>
    [DataContract]
    public class WebIndividualCategory : WebData
    {
        /// <summary>
        /// Create an instance of individual category.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebIndividualCategory(DataReader dataReader)
        {
            Id = dataReader.GetInt32(IndividualCategoryData.ID);
            Name = dataReader.GetString(IndividualCategoryData.NAME);
            Definition = dataReader.GetString(IndividualCategoryData.DEFINITION);
            SortOrder = dataReader.GetInt32(IndividualCategoryData.ID);
        }

        /// <summary>
        /// Description for this individual category.
        /// </summary>
        [DataMember]
        public String Definition
        { get; set; }

        /// <summary>
        /// Id for this individual category.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Name for this individual category.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Sort order for this individual category type.
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
            Definition = Definition.CheckSqlInjection();
            Name = Name.CheckSqlInjection();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Id for operators that are used in data conditions.
    /// Data conditions are used in DataQuery.
    /// </summary>
    [DataContract]
    public enum DataConditionOperatorId
    {
        /// <summary>
        /// Equal.
        /// </summary>
        [EnumMember]
        Equal,
        /// <summary>
        /// Greater.
        /// </summary>
        [EnumMember]
        Greater,
        /// <summary>
        /// GreaterOrEqual.
        /// </summary>
        [EnumMember]
        GreaterOrEqual,
        /// <summary>
        /// IsNull.
        /// </summary>
        [EnumMember]
        IsNull,
        /// <summary>
        /// Less.
        /// </summary>
        [EnumMember]
        Less,
        /// <summary>
        /// LessOrEqual.
        /// </summary>
        [EnumMember]
        LessOrEqual,
        /// <summary>
        /// Like.
        /// </summary>
        [EnumMember]
        Like,
        /// <summary>
        /// NoOperator.
        /// </summary>
        [EnumMember]
        NoOperator,
        /// <summary>
        /// NotEqual.
        /// </summary>
        [EnumMember]
        NotEqual
    }

    /// <summary>
    /// Contains data query information used to retrieve
    /// data from the web service.
    /// Exactly one of the properties DataCondition and DataConversion
    /// should be set in a WebDataQuery object.
    /// Property DataLimitation is optional in both cases.
    /// </summary>
    [DataContract]
    public class WebDataQuery : WebData
    {
        /// <summary>
        /// Create a WebDataQuery instance.
        /// </summary>
        public WebDataQuery()
        {
            DataCondition = null;
            DataConversion = null;
            DataLimitation = null;
        }

        /// <summary>Condition on the data that is returned.</summary>
        [DataMember]
        public WebDataCondition DataCondition
        { get; set;}

        /// <summary>
        /// Type conversion of the returned data.
        /// E.g. returning all distinct taxa for a
        /// set of SpeciesObservations</summary>
        [DataMember]
        public WebDataConversion DataConversion
        { get; set; }

        /// <summary>
        /// Limitations on which data the DataQuery encompass.
        /// E.g. only return SpeciesObservations for taxa
        /// that are specified in the limitation.
        /// </summary>
        [DataMember]
        public WebDataLimitation DataLimitation
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        public override void CheckData()
        {
            base.CheckData();
            if (DataConversion.IsNotNull())
            {
                throw new ApplicationException("Not yet implemented");
            }
            if (DataLimitation.IsNotNull())
            {
                throw new ApplicationException("Not yet implemented");
            }

            if (DataCondition.IsNull())
            {
                DataConversion.CheckNotNull("DataConversion");
            }
            else
            {
                DataConversion.CheckNull("DataConversion");
            }

            if (DataCondition.IsNotNull())
            {
                DataCondition.CheckData();
            }
            if (DataConversion.IsNotNull())
            {
                DataConversion.CheckData();
            }
            if (DataLimitation.IsNotNull())
            {
                DataLimitation.CheckData();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Id for operators that are used in data conditions.
    /// Data conditions are used in WebDataQuery.
    /// </summary>
    [DataContract]
    public enum DataLogicConditionOperatorId
    {
        /// <summary>
        /// And.
        /// </summary>
        [EnumMember]
        And,
        /// <summary>
        /// Not.
        /// </summary>
        [EnumMember]
        Not,
        /// <summary>
        /// Or.
        /// </summary>
        [EnumMember]
        Or
    }

    /// <summary>
    /// Contains a logic condition on data that is returned.
    /// This class is used in WebDataQuery handling.
    /// </summary>
    [DataContract]
    public class WebDataLogicCondition : WebData
    {
        /// <summary>
        /// Create a WebDataLogicCondition instance.
        /// </summary>
        public WebDataLogicCondition()
        {
            DataQueries = null;
            Operator = DataLogicConditionOperatorId.And;
        }

        /// <summary>
        /// Data queries that this logic condition operates on.
        /// All queries must have the same resulting data type.
        /// </summary>
        [DataMember]
        public List<WebDataQuery> DataQueries
        { get; set; }

        /// <summary>
        /// Logic operator for this condition.
        /// </summary>
        [DataMember]
        public DataLogicConditionOperatorId Operator
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        public override void CheckData()
        {
            base.CheckData();
            DataQueries.CheckNotEmpty("DataQueries");
            foreach (WebDataQuery dataQuery in DataQueries)
            {
                dataQuery.CheckData();
            }
            if (Operator == DataLogicConditionOperatorId.Not)
            {
                if (DataQueries.Count != 1)
                {
                    throw new ApplicationException("The condition 'Not' requires exactly one sub query!");
                }
            }
        }
    }
}

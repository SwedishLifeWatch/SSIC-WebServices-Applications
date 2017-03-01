using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Definition of logical operators that can be used anywhere.
    /// </summary>
    [DataContract]
    public enum LogicalOperator
    {
        /// <summary>And.</summary>
        [EnumMember]
        And,
        /// <summary>Not.</summary>
        [EnumMember]
        Not,
        /// <summary>Or.</summary>
        [EnumMember]
        Or
    }
}

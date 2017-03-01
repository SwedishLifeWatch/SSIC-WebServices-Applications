using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.GIS.WFS.Filter.Formula
{
    /// <summary>
    /// TBD ?!?
    /// </summary>
    public enum WFSLogicalOperator
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        And,

        /// <summary>
        /// TBD ?!?
        /// </summary>
        Or,

        /// <summary>
        /// TBD ?!?
        /// </summary>
        Not
    }

    /// <summary>
    /// TBD ?!?
    /// </summary>
    public enum WFSBinaryLogicalOperator
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        And,

        /// <summary>
        /// TBD ?!?
        /// </summary>
        Or
    }

    /// <summary>
    /// TBD ?!?
    /// </summary>
    public enum WFSUnaryLogicalOperator
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        Not
    }

    /// <summary>
    /// TBD ?!?
    /// </summary>
    public enum WFSComparisionOperator
    {
        /// <summary>
        /// Greater than
        /// </summary>
        GreaterThan,
        /// <summary>
        /// Less tha
        /// </summary>
        LessThan,
        /// <summary>
        /// Greater or equal to
        /// </summary>
        GreaterOrEqualTo,
        /// <summary>
        /// Less or equal to
        /// </summary>
        LessOrEqualTo,
        /// <summary>
        /// Not equal to
        /// </summary>
        NotEqualTo,
        /// <summary>
        /// EqualTo
        /// </summary>
        EqualTo,
        /// <summary>
        /// Like
        /// </summary>
        Like,
        /// <summary>
        /// Is null
        /// </summary>
        IsNull
    }

    /// <summary>
    /// TBD ?!?
    /// </summary>
    public enum WFSBinaryComparisionOperator
    {
        /// <summary>
        /// Greater than
        /// </summary>
        GreaterThan,
        /// <summary>
        /// Less than
        /// </summary>
        LessThan,
        /// <summary>
        /// Greater or equal to
        /// </summary>
        GreaterOrEqualTo,
        /// <summary>
        /// Less or equal to
        /// </summary>
        LessOrEqualTo,
        /// <summary>
        /// Different
        /// </summary>
        NotEqualTo,
        /// <summary>
        /// EqualTo
        /// </summary>
        EqualTo,
        /// <summary>
        /// Like
        /// </summary>
        Like       
    }

    /// <summary>
    /// TBD ?!?
    /// </summary>
    public enum WFSUnaryComparisionOperator
    {      
        /// <summary>
        /// Is null
        /// </summary>
        IsNull
    }

    /// <summary>
    /// TBD ?!?
    /// </summary>
    public enum WFSSpatialOperator
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        Contains,
        /// <summary>
        /// TBD ?!?
        /// </summary>
        CrossesWith,
        /// <summary>
        /// TBD ?!?
        /// </summary>
        Within,
        /// <summary>
        /// TBD ?!?
        /// </summary>
        Disjoint,
        /// <summary>
        /// TBD ?!?
        /// </summary>
        EqualThan,
        /// <summary>
        /// TBD ?!?
        /// </summary>
        InsideBbox,
        /// <summary>
        /// TBD ?!?
        /// </summary>
        IntersectsWith,
        /// <summary>
        /// TBD ?!?
        /// </summary>
        Overlaps,
        /// <summary>
        /// TBD ?!?
        /// </summary>
        Touches
    }

    /// <summary>
    /// TBD ?!?
    /// </summary>
    public enum WFSOperationType
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        BinaryLogicalOperation,
        /// <summary>
        /// TBD ?!?
        /// </summary>
        UnaryLogicalOperation,
        /// <summary>
        /// TBD ?!?
        /// </summary>
        BinaryComparisionOperation,
        /// <summary>
        /// TBD ?!?
        /// </summary>
        UnaryComparisionOperation,
        /// <summary>
        /// TBD ?!?
        /// </summary>
        SpatialOperation
    }
}

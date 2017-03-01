using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.GIS.WFS.Filter.Formula
{


    /// <summary>
    /// This class is an abstract base class for all comparision operation classes.
    /// </summary>
    public abstract class ComparisionOperation : FormulaOperation
    {
    }


    /// <summary>
    /// This class represents a binary comparision operation
    /// </summary>
    public class BinaryComparisionOperation : ComparisionOperation
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public FieldValue LeftOperand { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public FixedValue RightOperand { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public WFSBinaryComparisionOperator OperationType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryComparisionOperation"/> class.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <param name="operationType">The binary operation type.</param>
        public BinaryComparisionOperation(FieldValue leftOperand, FixedValue rightOperand, WFSBinaryComparisionOperator operationType)
        {
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
            OperationType = operationType;
        }

        /// <summary>
        /// Returns a WFS filter representation that can be used in a WFS request.
        /// </summary>
        /// <remarks>
        /// Create representation like this:
        /// <![CDATA[
        /// <PropertyIsLessThanOrEqualTo><PropertyName>POPULATION</PropertyName>
        /// <Literal>499</Literal></PropertyIsLessThanOrEqualTo>
        /// ]]>
        /// </remarks>
        public override string WFSRepresentation()
        {
            string str;
            if (OperationType == WFSBinaryComparisionOperator.Like)
            {
                
                const string strOperationType = "PropertyIsLike";
                string strLeftOperand = LeftOperand.WFSRepresentation();
                string strRightOperand = RightOperand.WFSRepresentation();
                str = "<" + strOperationType + " wildcard='*' singleChar='.' escape='!'>" + strLeftOperand + strRightOperand + "</" + strOperationType + ">";
                return str;
            }
            else
            {
                string strOperationType = GetWFSOperationTypeString();
                string strLeftOperand = LeftOperand.WFSRepresentation();
                string strRightOperand = RightOperand.WFSRepresentation();
                str = "<" + strOperationType + ">" + strLeftOperand + strRightOperand + "</" + strOperationType + ">";
                return str;
            }
        }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public override string WfsXmlRepresentation()
        {
            string str;
            if (OperationType == WFSBinaryComparisionOperator.Like)
            {

                const string strOperationType = "PropertyIsLike";
                string strLeftOperand = LeftOperand.WFSRepresentation();
                string strRightOperand = RightOperand.WFSRepresentation();
                str = "<" + strOperationType + " wildcard='*' singleChar='.' escape='!'>" + strLeftOperand + strRightOperand + "</" + strOperationType + ">";
                return str;
            }
            else
            {                
                string strOperationType = GetWFSOperationTypeString();
                string strLeftOperand = LeftOperand.WfsXmlRepresentation();
                string strRightOperand = RightOperand.WfsXmlRepresentation();

                str = "<ogc:" + strOperationType + " matchCase=\"true\">" + strLeftOperand + strRightOperand + "</ogc:" + strOperationType + ">";
                //<ogc:PropertyIsEqualTo matchCase="true"><ogc:PropertyName>LänSKOD</ogc:PropertyName><ogc:Literal>5</ogc:Literal></ogc:PropertyIsEqualTo>                
                return str;
            }
        }


        /// <summary>
        /// Gets the WFS operation type string.
        /// </summary>
        /// <returns></returns>
        private string GetWFSOperationTypeString()
        {
            switch (OperationType)
            {
                case WFSBinaryComparisionOperator.GreaterThan:
                    return "PropertyIsGreaterThan";
                case WFSBinaryComparisionOperator.LessThan:
                    return "PropertyIsLessThan";
                case WFSBinaryComparisionOperator.GreaterOrEqualTo:
                    return "PropertyIsGreaterThanOrEqualTo";
                case WFSBinaryComparisionOperator.LessOrEqualTo:
                    return "PropertyIsLessThanOrEqualTo";
                case WFSBinaryComparisionOperator.NotEqualTo:
                    return "PropertyIsNotEqualTo";
                case WFSBinaryComparisionOperator.EqualTo:
                    return "PropertyIsEqualTo";
                case WFSBinaryComparisionOperator.Like:
                    return "PropertyIsLike";
                default:
                    throw new Exception(OperationType + " is not yet supported");
            }
        }


        /// <summary>
        /// Gets the formula operation type string.
        /// </summary>
        /// <returns></returns>
        private string GetFormulaOperationTypeString()
        {
            switch (OperationType)
            {
                case WFSBinaryComparisionOperator.GreaterThan:
                    return ">";
                case WFSBinaryComparisionOperator.LessThan:
                    return "<";
                case WFSBinaryComparisionOperator.GreaterOrEqualTo:
                    return ">=";
                case WFSBinaryComparisionOperator.LessOrEqualTo:
                    return "<=";
                case WFSBinaryComparisionOperator.NotEqualTo:
                    return "<>";
                case WFSBinaryComparisionOperator.EqualTo:
                    return "=";
                case WFSBinaryComparisionOperator.Like:
                    return "like";

                default:
                    throw new Exception(OperationType + " is not yet supported");
            }
        }

        /// <summary>
        /// Returns a string representation of the formula
        /// </summary>
        public override string FormulaRepresentation()
        {
            string strOperationType = GetFormulaOperationTypeString();
            string strLeftOperand = LeftOperand.FormulaRepresentation();
            string strRightOperand = RightOperand.FormulaRepresentation();
            string str = strLeftOperand + " " + strOperationType + " " + strRightOperand;
            return str;
        }


        /// <summary>
        /// Gets the operation type enum.
        /// </summary>
        public override WFSOperationType OperationTypeEnum
        {
            get { return WFSOperationType.BinaryComparisionOperation;  }
        }
    }


    /// <summary>
    /// This class represents a unary comparision operation
    /// For the moment there exist one such operation: "IsNull"
    /// </summary>
    public class UnaryComparisionOperation : ComparisionOperation
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public FieldValue Operand { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public WFSUnaryComparisionOperator OperationType { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="UnaryComparisionOperation"/> class.
        /// </summary>
        /// <param name="operand">The operand.</param>
        /// <param name="operationType">Operation type.</param>
        public UnaryComparisionOperation(FieldValue operand, WFSUnaryComparisionOperator operationType)
        {
            Operand = operand;
            OperationType = operationType;
        }

        /// <summary>
        /// Returns a WFS filter representation that can be used in a WFS request.
        /// </summary>
        /// <remarks>
        /// Create representation like this:
        /// <![CDATA[ 
        /// <PropertyIsNull>
        ///        <ogc:PropertyName>myns:COLA/myns:NAME</ogc:PropertyName>
        /// </PropertyIsNull>
        ///  ]]>
        /// </remarks>
        public override string WFSRepresentation()
        {            
            if (OperationType == WFSUnaryComparisionOperator.IsNull)
            {
                const string strOperationType = "PropertyIsNull"; // ska det vara "NullCheck" i 1.1.0?
                string strOperand = Operand.WFSRepresentation();
                string str = "<" + strOperationType + ">" + strOperand + "</" + strOperationType + ">";
                return str;
            }
            throw new Exception(OperationType + " is not yet supported");
        }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public override string WfsXmlRepresentation()
        {
            if (OperationType == WFSUnaryComparisionOperator.IsNull)
            {
                const string strOperationType = "PropertyIsNull"; // ska det vara "NullCheck" i 1.1.0?
                string strOperand = Operand.WFSRepresentation();
                string str = "<" + strOperationType + ">" + strOperand + "</" + strOperationType + ">";
                return str;
            }
            throw new Exception(OperationType + " is not yet supported");
        }

        /// <summary>
        /// Returns a string representation of the formula
        /// </summary>
        public override string FormulaRepresentation()
        {
            if (OperationType == WFSUnaryComparisionOperator.IsNull)
            {                
                string strOperand = Operand.FormulaRepresentation();
                string str = "IsNull(" + strOperand + ")";
                return str;
            }
            throw new Exception(OperationType + " is not yet supported");
        }

        /// <summary>
        /// Gets the operation type enum.
        /// </summary>
        public override WFSOperationType OperationTypeEnum
        {
            get { return WFSOperationType.UnaryComparisionOperation; }
        }
    }
}

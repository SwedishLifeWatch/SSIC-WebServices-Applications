using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.GIS.WFS.Filter.Formula
{

    /// <summary>
    /// This class is an abstract base class for logical operations
    /// </summary>
    public abstract class LogicalOperation : FormulaOperation
    {
    }


    /// <summary>
    /// This class represents a binary logical operation (And and Or)
    /// </summary>
    public class BinaryLogicalOperation : LogicalOperation
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public FormulaOperation LeftOperand { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public FormulaOperation RightOperand { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public WFSBinaryLogicalOperator OperationType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryLogicalOperation"/> class.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <param name="operationType">The operation type.</param>
        public BinaryLogicalOperation(FormulaOperation leftOperand, FormulaOperation rightOperand, WFSBinaryLogicalOperator operationType)
        {
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
            OperationType = operationType;
        }

        /// <summary>
        /// Returns a WFS filter representation that can be used in a WFS request.
        /// </summary>
        public override string WFSRepresentation()
        {
            string strOperationType = GetWFSOperationTypeString();
            string strLeftOperand = LeftOperand.WFSRepresentation();
            string strRightOperand = RightOperand.WFSRepresentation();
            string str = "<" + strOperationType + ">" + strLeftOperand + strRightOperand + "</" + strOperationType + ">";
            return str;
        }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public override string WfsXmlRepresentation()
        {
            string strOperationType = GetWFSOperationTypeString();
            string strLeftOperand = LeftOperand.WfsXmlRepresentation();
            string strRightOperand = RightOperand.WfsXmlRepresentation();
            string str = "<ogc:" + strOperationType + ">" + strLeftOperand + strRightOperand + "</ogc:" + strOperationType + ">";
            return str;
        }

        /// <summary>
        /// Gets the WFS operation type string.
        /// </summary>
        /// <returns></returns>
        private string GetWFSOperationTypeString()
        {
            switch (OperationType)
            {
                case WFSBinaryLogicalOperator.And:
                    return "And";
                case WFSBinaryLogicalOperator.Or:
                    return "Or";                

                default:
                    throw new Exception(OperationType + " is not yet supported");
            }
        }


        /// <summary>
        /// Returns a string representation of the formula
        /// </summary>
        public override string FormulaRepresentation()
        {
            string strOperationType = GetWFSOperationTypeString();
            string strLeftOperand = LeftOperand.FormulaRepresentation();
            string strRightOperand = RightOperand.FormulaRepresentation();
            string str = "(" + strLeftOperand + " " + strOperationType + " " + strRightOperand + ")";
            return str;
        }

        /// <summary>
        /// Gets the operation type enum.
        /// </summary>
        public override WFSOperationType OperationTypeEnum
        {
            get { return WFSOperationType.BinaryLogicalOperation; }
        }
    }


    /// <summary>
    /// This class represents a unary logical operation (Not)
    /// </summary>
    public class UnaryLogicalOperation : LogicalOperation
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public FormulaOperation Operand { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public WFSUnaryLogicalOperator OperationType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnaryLogicalOperation"/> class.
        /// </summary>
        /// <param name="operand">The operand.</param>
        /// <param name="operationType">The operation type.</param>
        public UnaryLogicalOperation(FormulaOperation operand, WFSUnaryLogicalOperator operationType)
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
        /// <Not><PropertyIsEqualTo><PropertyName>POPULATION</PropertyName>
        /// <Literal>0</Literal></PropertyIsEqualTo></Not>
        ///  ]]>
        /// </remarks>
        public override string WFSRepresentation()
        {
            if (OperationType == WFSUnaryLogicalOperator.Not)
            {                
                const string strOperationType = "Not";
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
            if (OperationType == WFSUnaryLogicalOperator.Not)
            {                
                const string strOperationType = "Not";
                string strOperand = Operand.WfsXmlRepresentation();
                string str = "<ogc:" + strOperationType + ">" + strOperand + "</ogc:" + strOperationType + ">";
                return str;
            }
            throw new Exception(OperationType + " is not yet supported");            
        }

        /// <summary>
        /// Returns a string representation of the formula
        /// </summary>
        public override string FormulaRepresentation()
        {
            if (OperationType == WFSUnaryLogicalOperator.Not)
            {
                string strOperand = Operand.FormulaRepresentation();
                string str = "Not(" + strOperand + ")";
                return str;
            }
            throw new Exception(OperationType + " is not yet supported");
        }

        /// <summary>
        /// Gets the operation type enum.
        /// </summary>
        public override WFSOperationType OperationTypeEnum
        {
            get { return WFSOperationType.UnaryLogicalOperation; }
        }
    }

}

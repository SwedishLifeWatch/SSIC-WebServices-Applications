using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.GIS.WFS.Filter.Formula
{

    /// <summary>
    /// This class is a representation of a WFS filter
    /// This class can be compiled using SharpKit C# to JavaScript compiler in Ext Js mode http://sharpkit.net/
    /// </summary>
    /// <remarks>
    /// 1. Create a new project and add the files that will be converted to JavaScript. 
    /// 2. Add the following line to AssemblyInfo.cs:
    ///  [assembly: JsType(Mode = JsMode.ExtJs, Filename = "~/AnalysisPortal.WFS.Formula.js", AutomaticPropertiesAsFields = true, OmitCasts = true)]
    /// 3. Compile. The JavaScript file is created.
    /// 4. Change the namespace in the JavaScript file to "AnalysisPortal.WFS.Formula"
    /// 5. Copy the file to the AnalysisPortal project.     
    /// </remarks>
    public class WFSFilter
    {

        /// <summary>
        /// Gets or sets the formula.
        /// </summary>
        public FormulaOperation Formula { get; set; }


        /// <summary>
        /// Indicates whether you can add a logical operation or not.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can add a logical operation; otherwise, <c>false</c>.
        /// </value>
        public bool CanAddLogicalOperation
        {
            get
            {
                return Formula != null;
            }
        }

        /// <summary>
        /// Indicates whether you can add a comparision operation.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can add a comparision operation; otherwise, <c>false</c>.
        /// </value>
        public bool CanAddComparisionOperation
        {
            get { return Formula == null; }
        }

        /// <summary>
        /// Indicates whether you can remove the last operation.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can undo; otherwise, <c>false</c>.
        /// </value>
        public bool CanUndo
        {
            get { return Formula != null; }
        }

        /// <summary>
        /// Returns a WFS filter representation that can be used in a WFS request.
        /// </summary>
        public string WFSRepresentation()
        {
            if (Formula == null)
                return "";
            string strFormula = Formula.WFSRepresentation();
            string str = "<Filter>" + strFormula + "</Filter>";
            return str;
        }

        /// <summary>
        /// Returns a WFS filter representation that can be used in a WFS HTTP POST request.
        /// </summary>
        /// <returns></returns>
        public string WfsXmlRepresentation()
        {
            if (Formula == null)
                return "";
            string strFormula = Formula.WfsXmlRepresentation();
            string str = "<ogc:Filter xmlns:ogc=\"http://www.opengis.net/ogc\">" + strFormula + "</ogc:Filter>";
            return str;
        }

        /// <summary>
        /// Returns a string representation of the formula
        /// </summary>
        public string FormulaRepresentation()
        {
            if (Formula == null)
                return "";
            string strFormula = Formula.FormulaRepresentation();            
            return strFormula;
        }


        /// <summary>
        /// Removes the last operation from the formula.
        /// </summary>
        public void Undo()
        {
            if (Formula == null)
                return;
            
            if (Formula.OperationTypeEnum == WFSOperationType.BinaryLogicalOperation)
            {
                var binaryLogicalOperation = (BinaryLogicalOperation) Formula;
                Formula = binaryLogicalOperation.LeftOperand;
            }
            else if (Formula.OperationTypeEnum == WFSOperationType.UnaryLogicalOperation)
            {
                var unaryLogicalOperation = (UnaryLogicalOperation)Formula;
                Formula = unaryLogicalOperation.Operand;
            }
            else
            {
                Formula = null;
            }
        }


        /// <summary>
        /// Adds a logical operation to the formula.
        /// </summary>
        /// <param name="logicalOperator">The logical operator.</param>
        /// <param name="operation">The comparision operation.</param>
        public void AddLogicalOperation(WFSLogicalOperator logicalOperator, ComparisionOperation operation)
        {
            switch (logicalOperator)
            {
                case WFSLogicalOperator.And:
                    Formula = new BinaryLogicalOperation(Formula, operation, WFSBinaryLogicalOperator.And);
                    break;
                case WFSLogicalOperator.Or:
                    Formula = new BinaryLogicalOperation(Formula, operation, WFSBinaryLogicalOperator.Or);
                    break;
                case WFSLogicalOperator.Not:
                    Formula = new UnaryLogicalOperation(Formula, WFSUnaryLogicalOperator.Not);
                    break;
            }
        }


        /// <summary>
        /// Adds a binary logical operation to the formula.
        /// </summary>
        /// <param name="logicalOperator">The logical operator.</param>
        /// <param name="operation">The comparision operation.</param>
        public void AddBinaryLogicalOperation(WFSBinaryLogicalOperator logicalOperator, ComparisionOperation operation)
        {
            switch (logicalOperator)
            {
                case WFSBinaryLogicalOperator.And:
                    Formula = new BinaryLogicalOperation(Formula, operation, WFSBinaryLogicalOperator.And);
                    break;
                case WFSBinaryLogicalOperator.Or:
                    Formula = new BinaryLogicalOperation(Formula, operation, WFSBinaryLogicalOperator.Or);
                    break;                
            }
        }


        /// <summary>
        /// Adds a unary logical operation to the formula.
        /// </summary>
        /// <param name="logicalOperator">The logical operator.</param>
        public void AddUnaryLogicalOperation(WFSUnaryLogicalOperator logicalOperator)
        {
            switch (logicalOperator)
            {                
                case WFSUnaryLogicalOperator.Not:
                    Formula = new UnaryLogicalOperation(Formula, WFSUnaryLogicalOperator.Not);
                    break;
            }
        }


        /// <summary>
        /// Adds a comparision operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        public void AddComparisionOperation(ComparisionOperation operation)
        {
            Formula = operation;
        }

    }
}

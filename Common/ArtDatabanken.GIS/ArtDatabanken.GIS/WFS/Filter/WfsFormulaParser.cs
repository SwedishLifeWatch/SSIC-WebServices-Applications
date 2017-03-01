using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using ArtDatabanken.GIS.WFS.Filter.Formula;

namespace ArtDatabanken.GIS.WFS.Filter
{

    /// <summary>
    /// This class is used to parse a WFS formula string
    /// For example a formula like this: <PropertyIsGreaterThanOrEqualTo><PropertyName>LänSBOKSTA</PropertyName><Literal>25</Literal></PropertyIsGreaterThanOrEqualTo>
    /// </summary>
    public class WfsFormulaParser
    {


        /// <summary>
        /// Parses the specified WFS formula.
        /// </summary>
        /// <param name="wfsFormula">The WFS formula.</param>
        /// <returns>A FormulaOperation object representing the formula.</returns>
        public FormulaOperation Parse(string wfsFormula)
        {
            XmlReader reader = XmlReader.Create(new StringReader(wfsFormula));
            if (!reader.Read())
                return null;

            FormulaOperation formulaOperation = ParseItem(reader) as FormulaOperation;
            reader.Close();
            return formulaOperation;            
        }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public object ParseItem(XmlReader reader)
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    if (reader.Name.ToLower() == "filter")
                    {
                        reader.Read();
                        return ParseItem(reader);
                    }
                    if (reader.Name.ToLower() == "propertyisequalto")
                    {
                        reader.Read();
                        FieldValue fieldValue = (FieldValue) ParseItem(reader);
                        reader.Read();
                        FixedValue fixedValue = (FixedValue) ParseItem(reader);
                        reader.Read();                        
                        return new BinaryComparisionOperation(fieldValue, fixedValue, WFSBinaryComparisionOperator.EqualTo);
                    }
                    if (reader.Name.ToLower() == "propertyisgreaterthan")
                    {
                        reader.Read();
                        FieldValue fieldValue = (FieldValue)ParseItem(reader);
                        reader.Read();
                        FixedValue fixedValue = (FixedValue)ParseItem(reader);
                        reader.Read();
                        return new BinaryComparisionOperation(fieldValue, fixedValue, WFSBinaryComparisionOperator.GreaterThan);
                    }
                    if (reader.Name.ToLower() == "propertyisgreaterthanorequalto")
                    {
                        reader.Read();
                        FieldValue fieldValue = (FieldValue)ParseItem(reader);
                        reader.Read();
                        FixedValue fixedValue = (FixedValue)ParseItem(reader);
                        reader.Read();
                        return new BinaryComparisionOperation(fieldValue, fixedValue, WFSBinaryComparisionOperator.GreaterOrEqualTo);
                    }
                    if (reader.Name.ToLower() == "propertyislessthanorequalto")
                    {
                        reader.Read();
                        FieldValue fieldValue = (FieldValue)ParseItem(reader);
                        reader.Read();
                        FixedValue fixedValue = (FixedValue)ParseItem(reader);
                        reader.Read();
                        return new BinaryComparisionOperation(fieldValue, fixedValue, WFSBinaryComparisionOperator.LessOrEqualTo);
                    }
                    if (reader.Name.ToLower() == "propertyislessthan")
                    {
                        reader.Read();
                        FieldValue fieldValue = (FieldValue)ParseItem(reader);
                        reader.Read();
                        FixedValue fixedValue = (FixedValue)ParseItem(reader);
                        reader.Read();
                        return new BinaryComparisionOperation(fieldValue, fixedValue, WFSBinaryComparisionOperator.LessThan);
                    }
                    if (reader.Name.ToLower() == "propertyislike")
                    {
                        reader.Read();
                        FieldValue fieldValue = (FieldValue)ParseItem(reader);
                        reader.Read();
                        FixedValue fixedValue = (FixedValue)ParseItem(reader);
                        reader.Read();
                        return new BinaryComparisionOperation(fieldValue, fixedValue, WFSBinaryComparisionOperator.Like);
                    }
                    if (reader.Name.ToLower() == "propertyisnotequalto")
                    {
                        reader.Read();
                        FieldValue fieldValue = (FieldValue)ParseItem(reader);
                        reader.Read();
                        FixedValue fixedValue = (FixedValue)ParseItem(reader);
                        reader.Read();
                        return new BinaryComparisionOperation(fieldValue, fixedValue, WFSBinaryComparisionOperator.NotEqualTo);
                    }
                    if (reader.Name.ToLower() == "propertyisnull")
                    {
                        reader.Read();
                        FieldValue fieldValue = (FieldValue)ParseItem(reader);
                        reader.Read();                        
                        return new UnaryComparisionOperation(fieldValue,  WFSUnaryComparisionOperator.IsNull);
                    }
                    if (reader.Name.ToLower() == "or")
                    {
                        reader.Read();
                        FormulaOperation leftOperand = (FormulaOperation)ParseItem(reader);
                        reader.Read();
                        FormulaOperation rightOperand = (FormulaOperation)ParseItem(reader);
                        reader.Read();
                        return new BinaryLogicalOperation(leftOperand, rightOperand, WFSBinaryLogicalOperator.Or);
                    }
                    if (reader.Name.ToLower() == "and")
                    {
                        reader.Read();
                        FormulaOperation leftOperand = (FormulaOperation)ParseItem(reader);
                        reader.Read();
                        FormulaOperation rightOperand = (FormulaOperation)ParseItem(reader);
                        reader.Read();
                        return new BinaryLogicalOperation(leftOperand, rightOperand, WFSBinaryLogicalOperator.And);
                    }
                    if (reader.Name.ToLower() == "not")
                    {
                        reader.Read();
                        FormulaOperation operand = (FormulaOperation)ParseItem(reader);
                        reader.Read();                        
                        return new UnaryLogicalOperation(operand, WFSUnaryLogicalOperator.Not);
                    } 
                    if (reader.Name.ToLower() == "propertyname") // leaf
                    {
                        reader.Read();
                        FieldValue fieldValue = new FieldValue(reader.Value);
                        reader.Read();
                        return fieldValue;
                    }
                    if (reader.Name.ToLower() == "literal") // leaf
                    {
                        reader.Read();
                        ConstantValue constantValue = new ConstantValue(reader.Value);
                        reader.Read();
                        return constantValue;
                    }
                    break;                
            }
            reader.Read();
            return null;
        }

    }
}

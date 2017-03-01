using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.GIS.WFS.Filter.Formula
{


    /// <summary>
    /// This class represent a constant or a field
    /// </summary>
    public abstract class FixedValue
    {
        /// <summary>
        /// Returns a WFS filter representation that can be used in a WFS request.
        /// </summary>
        public abstract string WFSRepresentation();

        /// <summary>
        /// Returns a WFS filter representation that can be used in a WFS HTTP POST request.
        /// </summary>
        /// <returns></returns>
        public abstract string WfsXmlRepresentation();

        /// <summary>
        /// Returns a string representation of the formula
        /// </summary>
        public abstract string FormulaRepresentation();        
    }


    /// <summary>
    /// This class represents a constant value
    /// </summary>
    public class ConstantValue : FixedValue
    {

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantValue"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ConstantValue(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Returns a WFS filter representation that can be used in a WFS request.
        /// </summary>
        public override string WFSRepresentation()
        {
            string str = "<Literal>" + Value + "</Literal>";
            return str;
        }

        /// <summary>
        /// Returns a WFS filter representation that can be used in a WFS HTTP POST request.
        /// </summary>
        /// <returns></returns>
        public override string WfsXmlRepresentation()
        {
            string str = "<ogc:Literal>" + Value + "</ogc:Literal>";            
            return str;
        }

        /// <summary>
        /// Returns a string representation of the formula
        /// </summary>
        public override string FormulaRepresentation()
        {
            return Value;
        }
    }


    /// <summary>
    /// This class represents a field value (property value)
    /// </summary>
    public class FieldValue : FixedValue
    {

        /// <summary>
        /// Gets or sets the field name.
        /// </summary>        
        public string FieldName { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="FieldValue"/> class.
        /// </summary>
        /// <param name="fieldName">The field name.</param>
        public FieldValue(string fieldName)
        {
            FieldName = fieldName;
        }

        /// <summary>
        /// Returns a WFS filter representation that can be used in a WFS request.
        /// </summary>
        public override string WFSRepresentation()
        {
            string str = "<PropertyName>" + FieldName + "</PropertyName>";
            return str;
        }

        /// <summary>
        /// Returns a WFS filter representation that can be used in a WFS HTTP POST request.
        /// </summary>
        /// <returns></returns>
        public override string WfsXmlRepresentation()
        {            
            string str = "<ogc:PropertyName>" + FieldName + "</ogc:PropertyName>";
            return str;
        }

        /// <summary>
        /// Returns a string representation of the formula
        /// </summary>
        public override string FormulaRepresentation()
        {
            return "'" + FieldName + "'";
        }
    }

}

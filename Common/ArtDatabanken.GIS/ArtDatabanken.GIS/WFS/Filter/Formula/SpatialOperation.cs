using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ArtDatabanken.GIS.WFS.Filter.Formula
{


    /// <summary>
    /// This class represents a spatial operation.
    /// </summary>
    public class SpatialOperation : FormulaOperation
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public SpatialFieldValue LeftOperand { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public SpatialValue RightOperand { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public WFSSpatialOperator OperationType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpatialOperation"/> class.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <param name="operationType">The operation type.</param>
        public SpatialOperation(SpatialFieldValue leftOperand, SpatialValue rightOperand, WFSSpatialOperator operationType)
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
        /// <Intersects><PropertyName>Geometry</PropertyName>
        ///  <gml:Polygon><gml:outerBoundaryIs><gml:LinearRing>
        ///  <gml:coordinates>135.5329,34.6624 135.4921,34.8153 135.3673,34.7815
        ///  135.3800,34.6216 135.5361,34.6210 135.5329,34.6624</gml:coordinates>
        ///  </gml:LinearRing></gml:outerBoundaryIs></gml:Polygon></Intersects>
        /// ]]>
        /// </remarks>
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
                case WFSSpatialOperator.Contains:
                    return "Contains";
                case WFSSpatialOperator.CrossesWith:
                    return "Crosses";
                case WFSSpatialOperator.Within:
                    return "Within";
                case WFSSpatialOperator.Disjoint:
                    return "Disjoint";
                case WFSSpatialOperator.EqualThan:
                    return "Equals";
                case WFSSpatialOperator.InsideBbox:
                    return "BBOX";
                case WFSSpatialOperator.IntersectsWith:
                    return "Intersects";
                case WFSSpatialOperator.Overlaps:
                    return "Overlaps";
                case WFSSpatialOperator.Touches:
                    return "Touches";

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
            string str = strLeftOperand + " " + strOperationType + " " + strRightOperand;
            return str;
        }


        /// <summary>
        /// Gets the operation type enum.
        /// </summary>
        public override WFSOperationType OperationTypeEnum
        {
            get { return WFSOperationType.SpatialOperation; }
        }
    }





    /// <summary>
    /// This class in an abstract base class for spatial values
    /// </summary>
    public abstract class SpatialValue
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
    /// TBD ?!?
    /// </summary>
    public class SpatialBoundingBox : SpatialValue
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public double MinX { get; private set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public double MinY { get; private set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public double MaxX { get; private set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public double MaxY { get; private set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string SrsName { get; private set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public SpatialBoundingBox(double minX, double minY, double maxX, double maxY, string srsName)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
            SrsName = srsName;
        }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public override string WFSRepresentation()
        {
            return "";
        }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public override string WfsXmlRepresentation()
        {
            string strTemplate = "<gml:Envelope xmlns:gml=\"http://www.opengis.net/gml\" srsName=\"{0}\"><gml:lowerCorner>{1} {2}</gml:lowerCorner><gml:upperCorner>{3} {4}</gml:upperCorner></gml:Envelope>";
            string str = string.Format(strTemplate, SrsName, MinX.ToString(CultureInfo.InvariantCulture), MinY.ToString(CultureInfo.InvariantCulture), MaxX.ToString(CultureInfo.InvariantCulture), MaxY.ToString(CultureInfo.InvariantCulture));
            return str;
        }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public override string FormulaRepresentation()
        {
            string strTemplate = "BBox={0},{1} {2},{3}";
            string str = string.Format(strTemplate, MinX.ToString(CultureInfo.InvariantCulture), MinY.ToString(CultureInfo.InvariantCulture), MaxX.ToString(CultureInfo.InvariantCulture), MaxY.ToString(CultureInfo.InvariantCulture));
            return str;
        }
    }

    /// <summary>
    /// This class represents a spatial constant value, ie. GML
    /// </summary>
    public class SpatialConstantValue : SpatialValue
    {
        /// <summary>
        /// Gets or sets the GML string.
        /// </summary>
        public string Gml { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="SpatialConstantValue"/> class.
        /// </summary>
        /// <param name="gml">The GML.</param>
        public SpatialConstantValue(string gml)
        {
            Gml = gml;
        }

        /// <summary>
        /// Returns a WFS filter representation that can be used in a WFS request.
        /// </summary>
        public override string WFSRepresentation()
        {
            return Gml;
        }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public override string WfsXmlRepresentation()
        {
            return Gml;
        }

        /// <summary>
        /// Returns a string representation of the formula
        /// </summary>
        public override string FormulaRepresentation()
        {
            return Gml;
        }
    }


    /// <summary>
    /// This class represents a spatial field (property)
    /// </summary>
    public class SpatialFieldValue : SpatialValue
    {

        /// <summary>
        /// Gets or sets the field name.
        /// </summary>        
        public string FieldName { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="SpatialFieldValue"/> class.
        /// </summary>
        /// <param name="fieldName">The field name.</param>
        public SpatialFieldValue(string fieldName)
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
        /// TBD ?!?
        /// </summary>
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

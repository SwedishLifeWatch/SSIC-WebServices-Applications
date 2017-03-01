using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.GIS.WFS.Filter.Formula
{


    /// <summary>
    /// This class is an abstract base class for all WFS formulas
    /// </summary>
    public abstract class FormulaOperation
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public FormulaOperation()
        {
        }

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

        /// <summary>
        /// Gets the operation type enum.
        /// </summary>
        public abstract WFSOperationType OperationTypeEnum { get; }
        

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  Enum that contains factor field type ids.
    /// </summary>
    public enum FactorFieldDataTypeId
    {
        /// <summary>
        /// Boolean data type.
        /// </summary>
        Boolean = 0,
        /// <summary>
        /// Enum data type.
        /// </summary>
        Enum = 1,
        /// <summary>
        /// String data type.
        /// </summary>
        String = 2,
        /// <summary>
        /// Int32 data type.
        /// </summary>
        Int32 = 3,
        /// <summary>
        /// Double data type.
        /// </summary>
        Double = 4
    }


    /// <summary>
    /// This class represents a factor field type.
    /// </summary>
    [Serializable]
    public class FactorFieldType : DataSortOrder
    {
        private String _name;
        private String _definition;

        /// <summary>
        /// Create a FactorFieldType instance.
        /// </summary>
        /// <param name="id">Id of the factor field type</param>
        /// <param name="sortOrder">Sorting order of the factor field type</param>
        /// <param name="name">Name of the factor field type</param>
        /// <param name="definition">Definition of the factor field type</param>
        public FactorFieldType(
            Int32 id,
            Int32 sortOrder,
            String name,
            String definition)
            : base(id, sortOrder)
        {
            _name = name;
            _definition = definition;
        }

        /// <summary>
        /// Get name of this factor field type object.
        /// </summary>
        public String Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Get definition of this factor field type object.
        /// </summary>
        public String Definition
        {
            get { return _definition; }
        }

        /// <summary>
        /// Get data type of this factor field type object.
        /// </summary>
        public FactorFieldDataTypeId DataType
        {
            get
            {
                return (FactorFieldDataTypeId)(Enum.Parse(typeof(FactorFieldDataTypeId), Id.ToString()));
            }
        }
    }
}

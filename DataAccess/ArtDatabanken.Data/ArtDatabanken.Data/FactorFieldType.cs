using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents a factor field type.
    /// </summary>
    public class FactorFieldType : IFactorFieldType
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

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

        /// <summary>
        /// Definition for this factor field type.
        /// </summary>
        public String Definition { get; set; }

        /// <summary>
        /// Id for this factor field type.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Name for this factor field type.
        /// </summary>
        public String Name { get; set; }
    }
}

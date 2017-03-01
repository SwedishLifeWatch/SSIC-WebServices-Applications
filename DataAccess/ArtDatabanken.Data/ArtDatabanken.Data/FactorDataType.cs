using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents a factor data type.
    /// </summary>
    public class FactorDataType : IFactorDataType
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Definition for this factor data type.
        /// </summary>
        public String Definition { get; set; }

        /// <summary>
        /// Get field 1 of this factor data type.
        /// </summary>
        public IFactorField Field1
        {
            get { return FieldArray[0]; }
        }

        /// <summary>
        /// Get field 2 of this factor data type.
        /// </summary>
        public IFactorField Field2
        {
            get { return FieldArray[1]; }
        }

        /// <summary>
        /// Get field 3 of this factor data type.
        /// </summary>
        public IFactorField Field3
        {
            get { return FieldArray[2]; }
        }

        /// <summary>
        /// Get field 4 of this factor data type.
        /// </summary>
        public IFactorField Field4
        {
            get { return FieldArray[3]; }
        }

        /// <summary>
        /// Get field 5 of this factor data type.
        /// </summary>
        public IFactorField Field5
        {
            get { return FieldArray[4]; }
        }

        /// <summary>
        /// Fields this factor data type.
        /// </summary>
        public FactorFieldList Fields { get; set; }

        /// <summary>
        /// An ordered list of factor fields for this factor data type. May contain empty slots.
        /// </summary>
        public IFactorField[] FieldArray { get; set; }

        /// <summary>
        /// Id for this factor data type.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Get the main field of this factor data type.
        /// </summary>
        public IFactorField MainField
        {
            get
            {
                foreach (IFactorField field in Fields)
                {
                    if (field.IsMain)
                    {
                        return field;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Name for this factor data type.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Get all substantial fields of this factor data type.
        /// </summary>
        public FactorFieldList SubstantialFields
        {
            get
            {
                FactorFieldList fields;

                fields = new FactorFieldList();
                foreach (IFactorField field in Fields)
                {
                    if (field.IsSubstantial)
                    {
                        fields.Add(field);
                    }
                }

                return fields;
            }
        }
    }
}
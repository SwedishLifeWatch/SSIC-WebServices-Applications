using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the IPictureRelationDataType interface.
    /// </summary>
    [Serializable]
    public class PictureRelationDataTypeList : DataId32List<IPictureRelationDataType>
    {
        /// <summary>
        /// Returns the first picture relation data type in the list which has
        /// an identifier corresponding to the parameter identifier.
        /// </summary>
        /// <param name="identifier">
        /// Identifier for requested picture relation data type.
        /// </param>
        /// <returns>Picture relation data type with the correct identifier.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if no picture relation data type has the requested identifier.
        /// </exception>
        public IPictureRelationDataType Get(PictureRelationDataTypeIdentifier identifier)
        {
            IPictureRelationDataType pictureRelationDataType;

            pictureRelationDataType = null;
            if (this.IsNotEmpty())
            {
                foreach (IPictureRelationDataType tempPictureRelationDataType in this)
                {
                    if (tempPictureRelationDataType.Identifier == identifier.ToString())
                    {
                        pictureRelationDataType = tempPictureRelationDataType;
                    }
                }
            }

            if (pictureRelationDataType.IsNull())
            {
                // Picture relation data type not found.
                throw new ArgumentException("No picture relation data type with identifier = " + identifier);
            }

            return pictureRelationDataType;
        }
    }
}

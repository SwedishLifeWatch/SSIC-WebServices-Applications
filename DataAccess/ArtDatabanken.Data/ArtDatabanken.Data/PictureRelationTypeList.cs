using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the IPictureRelationType interface.
    /// </summary>
    [Serializable]
    public class PictureRelationTypeList : DataId32List<IPictureRelationType>
    {
        /// <summary>
        /// Returns the first picture relation type in the list which has
        /// an identifier corresponding to the parameter identifier.
        /// </summary>
        /// <param name="identifier">
        /// Identifier for requested picture relation type.
        /// </param>
        /// <returns>Picture relation type with the correct identifier.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if no picture relation type has the requested identifier.
        /// </exception>
        public IPictureRelationType Get(PictureRelationTypeIdentifier identifier)
        {
            IPictureRelationType pictureRelationType;

            pictureRelationType = null;
            if (this.IsNotEmpty())
            {
                foreach (IPictureRelationType tempPictureRelationType in this)
                {
                    if (tempPictureRelationType.Identifier == identifier.ToString())
                    {
                        pictureRelationType = tempPictureRelationType;
                    }
                }
            }
 
            if (pictureRelationType.IsNull())
            {
                // Picture relation type not found.
                throw new ArgumentException("No picture relation type with identifier = " + identifier);
            }

            return pictureRelationType;
        }
    }
}

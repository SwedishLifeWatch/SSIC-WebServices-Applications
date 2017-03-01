using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the IPictureLifeCycleStatusType interface.
    /// </summary>
    [Serializable]
    public class PictureLifeCycleStatusTypeList : DataId32List<IPictureLifeCycleStatusType>
    {
        /// <summary>
        /// Returns the first picture life cycle status type in the list which has
        /// an identifier corresponding to the parameter identifier.
        /// </summary>
        /// <param name="identifier">
        /// Identifier for requested picture life cycle status type.
        /// </param>
        /// <returns>Picture life cycle status type with the correct identifier.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if no picture life cycle status type has the requested identifier.
        /// </exception>
        public IPictureLifeCycleStatusType Get(PictureLifeCycleStatusIdentifier identifier)
        {
            IPictureLifeCycleStatusType pictureLifeCycleStatusType;

            pictureLifeCycleStatusType = null;
            if (this.IsNotEmpty())
            {
                foreach (IPictureLifeCycleStatusType tempPictureLifeCycleStatusType in this)
                {
                    if (tempPictureLifeCycleStatusType.Identifier == identifier.ToString())
                    {
                        pictureLifeCycleStatusType = tempPictureLifeCycleStatusType;
                    }
                }
            }
 
            if (pictureLifeCycleStatusType.IsNull())
            {
                // Picture life cycle status type not found.
                throw new ArgumentException("No picture life status cycle type with identifier = " + identifier);
            }

            return pictureLifeCycleStatusType;
        }
    }
}

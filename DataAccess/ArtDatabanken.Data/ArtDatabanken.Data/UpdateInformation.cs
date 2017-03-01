using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    ///  This class contains information
    ///  about create/update of an object.
    /// </summary>
    [Serializable()]
    public class UpdateInformation : IUpdateInformation
    {
        /// <summary>
        /// Id of user that created the object. Not null. Must be set first time the object is saved.
        /// </summary>
        public Int32 CreatedBy
        { get; set; }

        /// <summary>
        /// Date and time when the object was created. Not null. Is set by the database.
        /// </summary>
        public DateTime CreatedDate
        { get; set; }

        /// <summary>
        /// Id of user that modified the object. Not null. Must be set each time the object is saved.
        /// </summary>
        public Int32 ModifiedBy
        { get; set; }

        /// <summary>
        /// Date and time when the object was last modified. Not null. Is set by the database.
        /// </summary>
        public DateTime ModifiedDate
        { get; set; }
    }
}

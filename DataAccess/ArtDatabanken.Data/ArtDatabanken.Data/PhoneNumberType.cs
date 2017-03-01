using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about a phone number type.
    /// </summary>
    [Serializable()]
    public class PhoneNumberType : IPhoneNumberType
    {
        /// <summary>
        /// Create a phone number type instance.
        /// </summary>
        /// <param name='id'>Id for this phone number type.</param>
        /// <param name='name'>Name.</param>
        /// <param name='nameStringId'>String id for the Name property.</param>
        /// <param name='dataContext'>Data context.</param>
        public PhoneNumberType(Int32 id,
                               String name,
                               Int32 nameStringId,
                               IDataContext dataContext)
        {
            // Check data.
            dataContext.CheckNotNull("dataContext");

            // Set data.
            DataContext = dataContext;
            Id = id;
            Name = name;
            NameStringId = nameStringId;
        }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; private set; }

        /// <summary>
        /// Id for this phone number type.
        /// </summary>
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public String Name
        { get; set; }

        /// <summary>
        /// StringId for the Name property.
        /// </summary>
        public Int32 NameStringId
        { get; private set; }
    }
}

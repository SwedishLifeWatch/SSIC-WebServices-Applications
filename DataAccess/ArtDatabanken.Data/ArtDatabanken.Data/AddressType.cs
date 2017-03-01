using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about an address type.
    /// </summary>
    [Serializable]
    public class AddressType : IAddressType
    {
        private String _name;

        /// <summary>
        /// Create an AddressType instance.
        /// </summary>
        /// <param name='id'>Id for this address type.</param>
        /// <param name='name'>Name.</param>
        /// <param name='nameStringId'>String id for the Name property.</param>
        /// <param name='dataContext'>Data context.</param>
        public AddressType(Int32 id,
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
        public IDataContext DataContext { get; private set; }

        /// <summary>
        /// Id for this address type.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                value.CheckNotEmpty("_name");
                _name = value;
            }
        }

        /// <summary>
        /// String id for the Name property.
        /// </summary>
        public Int32 NameStringId { get; private set; }
    }
}

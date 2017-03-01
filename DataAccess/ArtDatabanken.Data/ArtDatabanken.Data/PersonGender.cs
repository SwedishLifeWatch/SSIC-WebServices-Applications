using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about a person gender.
    /// </summary>
    [Serializable()]
    public class PersonGender : IPersonGender
    {
        private String _name;

        /// <summary>
        /// Create an Person Gender instance.
        /// </summary>
        /// <param name='id'>Id for this person gender.</param>
        /// <param name='name'>Name.</param>
        /// <param name='nameStringId'>String id for the Name property.</param>
        /// <param name='dataContext'>Data context.</param>
        public PersonGender(Int32 id,
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
        /// Id for this address type.
        /// </summary>
        public Int32 Id
        { get; set; }

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
        public Int32 NameStringId
        { get; private set; }
    }
}

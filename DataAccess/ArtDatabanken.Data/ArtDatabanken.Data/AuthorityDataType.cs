using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about an authority data type.
    /// </summary>
    [Serializable()]
    public class AuthorityDataType : IAuthorityDataType
    {
        private String _identifier;

        /// <summary>
        /// Create an AuthorityDataType instance.
        /// </summary>
        /// <param name='id'>Id for this address type.</param>
        /// <param name='identifier'>identifier.</param>
        /// <param name='dataContext'>Data context.</param>
        public AuthorityDataType(Int32 id,
                                 String identifier,
                                 IDataContext dataContext)
        {
            // Check data.
            dataContext.CheckNotNull("dataContext");

            // Set data.
            DataContext = dataContext;
            Id = id;
            Identifier = identifier;
        }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; private set; }

        /// <summary>
        /// Id for this authorty data type.
        /// </summary>
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Identifier.
        /// </summary>
        public String Identifier
        {
            get
            {
                return _identifier;
            }
            set
            {
                value.CheckNotEmpty("_identifier");
                _identifier = value;
            }
        }
    }
}

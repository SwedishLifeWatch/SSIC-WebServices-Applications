using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    ///  This class handles information related to a country.
    /// </summary>
    [Serializable()]
    public class Country : ICountry
    {
        /// <summary>
        /// Create a Country instance.
        /// </summary>
        /// <param name='id'>Id for this country.</param>
        /// <param name='ISOCode'>ISO code for this country.</param>
        /// <param name='ISOName'>ISO name of the country.</param>
        /// <param name='name'>English name of the country.</param>
        /// <param name='nativeName'>Native name or names of the country.</param>
        /// <param name='phoneNumberPrefix'>Phone number prefix.</param>
        /// <param name='dataContext'>Data context.</param>
        public Country(Int32 id,
                       String ISOCode,
                       String ISOName,
                       String name,
                       String nativeName,
                       Int32 phoneNumberPrefix,
                       IDataContext dataContext)
        {
            // Check data.
            dataContext.CheckNotNull("dataContext");

            // Set data.
            DataContext = dataContext;
            Id = id;
            this.ISOCode = ISOCode;
            this.ISOName = ISOName;
            Name = name;
            NativeName = nativeName;
            PhoneNumberPrefix = phoneNumberPrefix;
        }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; private set; }

        /// <summary>
        /// Id for this country.
        /// </summary>
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Two character code representing the country
        /// according to ISO-3166. Not Null. Is read only.
        /// </summary>
        public String ISOCode
        { get; private set; }

        /// <summary>
        /// ISO name of the country. Not Null. Is read only.
        /// </summary>
        public String ISOName
        { get; private set; }

        /// <summary>
        /// English name of the country. Not Null. Is read only.
        /// </summary>
        public String Name
        { get; private set; }

        /// <summary>
        /// Native name or names of the country. Not Null. Is read only.
        /// </summary>
        public String NativeName
        { get; private set; }

        /// <summary>
        /// Phone number prefix. Not Null. Is read only.
        /// </summary>
        public Int32 PhoneNumberPrefix
        { get; private set; }
    }
}

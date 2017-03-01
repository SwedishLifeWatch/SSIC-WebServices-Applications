using System;
using System.Globalization;

namespace ArtDatabanken.Data
{
    /// <summary>
    ///  This class handles information about a locale.
    /// </summary>
    [Serializable]
    public class Locale : ILocale
    {
        private CultureInfo _cultureInfo;

        /// <summary>
        /// Create a Locale instance.
        /// </summary>
        /// <param name='id'>Id for this locale.</param>
        /// <param name='ISOCode'>ISO code for this locale.</param>
        /// <param name='name'>English name of the locale.</param>
        /// <param name='nativeName'>Native name or names of the locale.</param>
        /// <param name='dataContext'>Data context.</param>
        public Locale(Int32 id,
                      String ISOCode,
                      String name,
                      String nativeName,
                      IDataContext dataContext)
        {
            // Check data.
            dataContext.CheckNotNull("dataContext");

            // Set data.
            _cultureInfo = null;
            DataContext = dataContext;
            Id = id;
            this.ISOCode = ISOCode;
            Name = name;
            NativeName = nativeName;
        }

        /// <summary>
        /// Get culture info which correspond to this locale.
        /// </summary>
        public CultureInfo CultureInfo
        {
            get
            {
                if (_cultureInfo.IsNull())
                {
                    _cultureInfo = new CultureInfo(ISOCode);
                }
                return _cultureInfo;
            }
        }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; private set; }

        /// <summary>
        /// Id for this locale.
        /// </summary>
        public Int32 Id
        { get; set; }

        /// <summary>
        /// ISO code for this locale.
        /// This code is a combination of "ISO 639-1" (language code)
        /// and "ISO 3166-1 alpha-2" (country code).
        /// E.g. en-GB and sv-SE.
        /// </summary>
        public String ISOCode
        { get; private set; }

        /// <summary>
        /// English name of the locale.
        /// </summary>
        public String Name
        { get; private set; }

        /// <summary>
        /// Native name or names of the locale.
        /// </summary>
        public String NativeName
        { get; private set; }
    }
}

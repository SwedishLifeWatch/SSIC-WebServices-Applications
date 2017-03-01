using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the ILocale interface.
    /// </summary>
    [Serializable]
    public class LocaleList : DataId32List<ILocale>
    {
        /// <summary>
        /// Get locale with specified ISO code.
        /// This code is a combination of "ISO 639-1" (language code)
        /// and "ISO 3166-1 alpha-2" (country code).
        /// E.g. en-GB and sv-SE.
        /// Language codes are matched to first locale of the same language.
        /// For example "en" may be matched to "en-GB".
        /// </summary>
        /// <param name='isoCode'>ISO code of locale.</param>
        /// <returns>Requested locale.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested ISOCode.</exception>
        public ILocale Get(String isoCode)
        {
            ILocale locale;

            locale = null;

            if (isoCode.IsNotEmpty())
            {
                // Get exact match.
                foreach (ILocale tempLocale in this)
                {
                    if (tempLocale.ISOCode.ToLower() == isoCode.ToLower())
                    {
                        locale = tempLocale;
                        break;
                    }
                }

                // Try to match language code.
                if (locale.IsNull())
                {
                    foreach (ILocale tempLocale in this)
                    {
                        if (tempLocale.ISOCode.ToLower().StartsWith(isoCode.ToLower()))
                        {
                            locale = tempLocale;
                            break;
                        }
                    }
                }
            }

            return locale;
        }
    }
}

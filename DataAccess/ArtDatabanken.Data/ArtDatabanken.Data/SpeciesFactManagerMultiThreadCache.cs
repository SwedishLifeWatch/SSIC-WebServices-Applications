namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles cache of species fact information.
    /// </summary>
    public class SpeciesFactManagerMultiThreadCache : SpeciesFactManagerSingleThreadCache
    {
        /// <summary>
        /// Get species fact qualities for specified locale.
        /// </summary>
        /// <param name="locale">Currently used locale.</param>
        /// <returns>Species fact qualities for specified locale.</returns>
        protected override SpeciesFactQualityList GetSpeciesFactQualities(ILocale locale)
        {
            SpeciesFactQualityList speciesFactQualities = null;

            lock (SpeciesFactQualities)
            {
                if (SpeciesFactQualities.ContainsKey(locale.ISOCode))
                {
                    speciesFactQualities = (SpeciesFactQualityList)(SpeciesFactQualities[locale.ISOCode]);
                }
            }

            return speciesFactQualities;
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        protected override void RefreshCache(IUserContext userContext)
        {
            lock (SpeciesFactQualities)
            {
                SpeciesFactQualities.Clear();
            }
        }

        /// <summary>
        /// Set species fact qualities for specified locale.
        /// </summary>
        /// <param name="speciesFactQualities">Individual categories.</param>
        /// <param name="locale">Currently used locale.</param>
        protected override void SetSpeciesFactQualities(SpeciesFactQualityList speciesFactQualities,
                                                        ILocale locale)
        {
            lock (SpeciesFactQualities)
            {
                SpeciesFactQualities[locale.ISOCode] = speciesFactQualities;
            }
        }
    }
}
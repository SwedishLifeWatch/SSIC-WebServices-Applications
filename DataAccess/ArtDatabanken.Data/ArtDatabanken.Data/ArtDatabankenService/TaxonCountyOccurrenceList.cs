using System;
using System.Collections;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the TaxonCountyOccurrence class.
    /// </summary>
    [Serializable()]
    public class TaxonCountyOccurrenceList : ArrayList
    {
        /// <summary>
        /// Get county occurrence for specified county.
        /// </summary>
        /// <param name='countyIdentifier'>County identifier.</param>
        /// <exception cref="ArgumentException">Thrown if no data related to specified county exists.</exception>
        /// <returns>Requested county occurrence.</returns>
        public TaxonCountyOccurrence GetByCountyIdentifier(String countyIdentifier)
        {
            countyIdentifier = countyIdentifier.ToLower();
            foreach (TaxonCountyOccurrence countyOccurrence in this)
            {
                if (countyOccurrence.County.Identifier.ToLower() == countyIdentifier)
                {
                    // County occurrence found. Return it.
                    return countyOccurrence;
                }
            }

            // No data found for specified county identifier.
            throw new ArgumentException("No county occurrence data for county identifier " + countyIdentifier + "!");
        }

        /// <summary>
        /// Get/set TaxonCountyOccurrence by list index.
        /// </summary>
        public new TaxonCountyOccurrence this[Int32 index]
        {
            get
            {
                return (TaxonCountyOccurrence)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}

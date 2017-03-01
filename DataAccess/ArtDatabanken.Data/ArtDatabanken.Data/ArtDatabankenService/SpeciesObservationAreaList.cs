using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the SpeciesObservationArea class.
    /// </summary>
    [Serializable()]
    public class SpeciesObservationAreaList : ArrayList
    {
        /// <summary>
        /// Get/set SpeciesObservationArea by list index.
        /// </summary>
        public new SpeciesObservationArea this[Int32 index]
        {
            get
            {
                return (SpeciesObservationArea)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }

        /// <summary>
        /// Show all information about species observation areas as string.
        /// </summary>
        public override string ToString()
        {
            String text = String.Empty;

            foreach (SpeciesObservationArea speciesObservationArea in this)
            {
                if (text.Length > 0)
                {
                    text += Environment.NewLine + speciesObservationArea;
                }
                else
                {
                    text = speciesObservationArea.ToString();
                }
            }
            return text;
        }
    }
}

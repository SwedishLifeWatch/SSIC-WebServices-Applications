using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the City class.
    /// </summary>
    [Serializable()]
    public class CityList : ArrayList
    {
        /// <summary>
        /// Get/set City by list index.
        /// </summary>
        public new City this[Int32 index]
        {
            get
            {
                return (City)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}

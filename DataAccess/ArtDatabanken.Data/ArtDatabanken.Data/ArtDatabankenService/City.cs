using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  This class handles information about a city.
    /// </summary>
    [Serializable()]
    public class City
    {
        private Int32 _xCoordinate;
        private Int32 _yCoordinate;
        private String _county;
        private String _municipality;
        private String _name;
        private String _parish;

        /// <summary>
        /// Create a city instance.
        /// </summary>
        /// <param name='name'>Name for city.</param>
        /// <param name='county'>County in which the city is located.</param>
        /// <param name='municipality'>Municipality in which the city is located.</param>
        /// <param name='parish'>Parish in which the city is located.</param>
        /// <param name='xCoordinate'>XCoordinate where the city is located.</param>
        /// <param name='yCoordinate'>YCoordinate where the city is located.</param>
        public City(String name,
                    String county,
                    String municipality,
                    String parish,
                    Int32 xCoordinate,
                    Int32 yCoordinate)
        {
            _county = county;
            _municipality = municipality;
            _name = name;
            _parish = parish;
            _xCoordinate = xCoordinate;
            _yCoordinate = yCoordinate;
        }

        /// <summary>
        /// Get county in which the city is located.
        /// </summary>
        public String County
        {
            get { return _county; }
        }

        /// <summary>
        /// Get municipality in which the city is located.
        /// </summary>
        public String Municipality
        {
            get { return _municipality; }
        }

        /// <summary>
        /// Get name for this city.
        /// </summary>
        public String Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Get parish in which the city is located.
        /// </summary>
        public String Parish
        {
            get { return _parish; }
        }

        /// <summary>
        /// Get xCoordinate where the city is located.
        /// </summary>
        public Int32 XCoordinate
        {
            get { return _xCoordinate; }
        }

        /// <summary>
        /// Get yCoordinate where the city is located.
        /// </summary>
        public Int32 YCoordinate
        {
            get { return _yCoordinate; }
        }
    }
}

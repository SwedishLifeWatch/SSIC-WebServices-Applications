using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information related to a city
    /// </summary>
    public class CityInformation : ICityInformation
    {
        /// <summary>
        /// Gets or sets the coordinate of this CityInformation
        /// </summary>
        public IPoint Coordinate { get; set; }

        /// <summary>
        /// Gets or sets the name of the County
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// Gets or sets the DataContext of this CityInformation
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Gets or sets the name of the Municpality
        /// </summary>
        public string Municipality { get; set; }

        /// <summary>
        /// Gets or sets the name of the city
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the Parish
        /// </summary>
        public string Parish { get; set; }
    }
}

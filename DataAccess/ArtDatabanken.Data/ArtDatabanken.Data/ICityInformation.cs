using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data
{
    public interface ICityInformation
    {
        /// <summary>
        /// Get data context.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Coordinate where the city is located.
        /// </summary>
        IPoint Coordinate { get; set; }

        /// <summary>
        /// Gets or sets the County name
        /// </summary>
        string County { get; set; }

        /// <summary>
        /// Gets or sets the Municipality name
        /// </summary>
         string Municipality { get; set; }

        /// <summary>
        /// Gets or sets the name of the City
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the Parish name
        /// </summary>
        string Parish { get; set; }
    }
}

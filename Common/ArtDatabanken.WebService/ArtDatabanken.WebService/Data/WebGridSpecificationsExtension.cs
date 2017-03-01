using System;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to the WebGridSpecifications class.
    /// </summary>
    public static class WebGridSpecificationsExstension
    {
        /// <summary>
        /// Check if grid specifications is correct.
        /// </summary>
        /// <param name="gridSpecification"> Information on grid specifications.</param>
        public static void CheckData(this WebGridSpecification gridSpecification)
        //, WebBoundingBox speciesObservationBoundingBox )
        {
            if (gridSpecification.IsNotNull())
            {
                if (gridSpecification.GridCoordinateSystem.IsNull())
                {
                    throw new ArgumentException(
                        "WebGridSpecifications: Property GridCellCoordinateSystem cant be null ie value must be set.");
                }
            }
            //if(gridSpecifications.IsNotNull() && gridSpecifications.BoundingBox.IsNotNull())
            //{
            //    if(speciesObservationBoundingBox.IsNotNull())
            //    {
            //        throw new ArgumentException("WebGridSpecifications: Properties WebGridSpecifications.BoundingBox and WebSpeciesObservatioSearchCriteria.BoundingBox have value set, only one BoundigBox can be set.");
            //    }
            //}

        }

        public static WebCoordinateSystem GetWebCoordinateSystem(this WebGridSpecification gridSpecification)
        {
            WebCoordinateSystem gridCellCoordinateSystemAsWebCoordinateSystem = null;
            foreach (CoordinateSystemId coordinateSystemId in Enum.GetValues(typeof(CoordinateSystemId)))
            {
                if (coordinateSystemId.ToString().Equals(gridSpecification.GridCoordinateSystem.ToString()))
                {
                    gridCellCoordinateSystemAsWebCoordinateSystem = new WebCoordinateSystem {Id = coordinateSystemId};
                    break;
                }
            }

            if (gridCellCoordinateSystemAsWebCoordinateSystem.IsNull())
            {
                throw new ArgumentException(string.Format("GridCellCoordinateSystem don't match any existing CoordinateSystem. {0} doesn't exist in CoordinateSystem as enum value.", gridSpecification.GridCoordinateSystem));
            }

            return gridCellCoordinateSystemAsWebCoordinateSystem;
        }
    }
}

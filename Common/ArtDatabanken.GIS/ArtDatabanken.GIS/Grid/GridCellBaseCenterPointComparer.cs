using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.GIS.Grid
{
    /// <summary>
    /// This class is used for equality comparision between objects that
    /// implements the IGridCellBase interface.
    /// Two objects implementing the IGridCellBase are considered equal if
    /// their property GridCellCentreCoordinate are equal.
    /// </summary>
    public class GridCellBaseCenterPointComparer : IEqualityComparer<IGridCellBase>
    {
        /// <summary>
        /// Determines whether gridCell1.OrginalGridCellCentreCoordinate is equal to 
        /// gridCell2.OrginalGridCellCentreCoordinate.
        /// </summary>            
        /// <param name="gridCell1">The first grid cell.</param>
        /// <param name="gridCell2">The second grid cell.</param>
        /// <returns>
        ///   <c>true</c> if gridCell1.OrginalGridCellCentreCoordinate is equal to gridCell2.OrginalGridCellCentreCoordinate ; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(IGridCellBase gridCell1, IGridCellBase gridCell2)
        {
            if (ReferenceEquals(null, gridCell1.OrginalGridCellCentreCoordinate) &&
                ReferenceEquals(null, gridCell2.OrginalGridCellCentreCoordinate))
            {
                return true;
            }

            if (ReferenceEquals(null, gridCell1.OrginalGridCellCentreCoordinate) ||
                ReferenceEquals(null, gridCell2.OrginalGridCellCentreCoordinate))
            {
                return false;
            }

            if (ReferenceEquals(gridCell1.OrginalGridCellCentreCoordinate, gridCell2.OrginalGridCellCentreCoordinate))
            {
                return true;
            }

            return gridCell1.OrginalGridCellCentreCoordinate.X.Equals(gridCell2.OrginalGridCellCentreCoordinate.X) && gridCell1.OrginalGridCellCentreCoordinate.Y.Equals(gridCell2.OrginalGridCellCentreCoordinate.Y);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="gridCell">
        /// The grid Cell.
        /// </param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public int GetHashCode(IGridCellBase gridCell)
        {
            unchecked
            {
                return (gridCell.OrginalGridCellCentreCoordinate.X.GetHashCode() * 397) ^ gridCell.OrginalGridCellCentreCoordinate.Y.GetHashCode();
            }
        }
    }

}

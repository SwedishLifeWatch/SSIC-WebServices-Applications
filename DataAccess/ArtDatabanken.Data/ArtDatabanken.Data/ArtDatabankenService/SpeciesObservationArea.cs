using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  This class contains area information about
    ///  species observations for one taxon. That is
    ///  AOO/EOO (Area of Occupancy/Extent of Occurrence)
    ///  which is used in the redlist work.
    /// </summary>
    [Serializable()]
    public class SpeciesObservationArea
    {
        private const String SEPARATOR = "\t";

        private Int32 _gridSquareMaxDistance;
        private Int32 _gridSquareSize;
        private Int32 _speciesObservationCount;
        private Int64 _areaOfOccupancy;
        private Int64 _extentOfOccurrence;
        private Taxon _taxon;

        /// <summary>
        /// Create a SpeciesObservationArea instance.
        /// </summary>
        /// <param name='taxon'>Taxon for which the area calculations has been made.</param>
        /// <param name='speciesObservationCount'>Number of species observation used in the calculation.</param>
        /// <param name='gridSquareSize'>Size of grid square used in the calculation.</param>
        /// <param name='gridSquareMaxDistance'>Max distance between grid squares.</param>
        /// <param name='areaOfOccupancy'>Area of Occupancy (AOO) value.</param>
        /// <param name='extentOfOccurrence'>Extent of Occurrence (EOO) value.</param>
        public SpeciesObservationArea(Taxon taxon,
                                      Int32 speciesObservationCount,
                                      Int32 gridSquareSize,
                                      Int32 gridSquareMaxDistance,
                                      Int64 areaOfOccupancy,
                                      Int64 extentOfOccurrence)
        {
            _taxon = taxon;
            _speciesObservationCount = speciesObservationCount;
            _gridSquareSize = gridSquareSize;
            _gridSquareMaxDistance = gridSquareMaxDistance;
            _areaOfOccupancy = areaOfOccupancy;
            _extentOfOccurrence = extentOfOccurrence;
        }

        /// <summary>
        /// Get area of Occupancy (AOO) value.
        /// Unit is square meter.
        /// </summary>
        public Int64 AreaOfOccupancy
        {
            get { return _areaOfOccupancy; }
        }

        /// <summary>
        /// Get extent of Occurrence (EOO) value.
        /// Unit is square meter.
        /// </summary>
        public Int64 ExtentOfOccurrence
        {
            get { return _extentOfOccurrence; }
        }

        /// <summary>
        /// Get max distance between grid squares.
        /// Unit is meter.
        /// </summary>
        public Int32 GridSquareMaxDistance
        {
            get { return _gridSquareMaxDistance; }
        }

        /// <summary>
        /// Get size of grid square used in the calculation.
        /// The size is stated as the length of one side in the square.
        /// Unit is meter.
        /// </summary>
        public Int32 GridSquareSize
        {
            get { return _gridSquareSize; }
        }

        /// <summary>
        /// Get number of species observation used in the calculation.
        /// </summary>
        public Int32 SpeciesObservationCount
        {
            get { return _speciesObservationCount; }
        }

        /// <summary>
        /// Get taxon for which the area calculations has been made.
        /// </summary>
        public Taxon Taxon
        {
            get { return _taxon; }
        }

        /// <summary>
        /// Show all information about species observation area as string.
        /// </summary>
        public override string ToString()
        {
            return "TaxonId" + SEPARATOR + Taxon.Id + SEPARATOR +
                   "SpeciesObservationCount" + SEPARATOR + SpeciesObservationCount + SEPARATOR +
                   "GridSquareSize" + SEPARATOR + GridSquareSize + SEPARATOR +
                   "GridSquareMaxDistance" + SEPARATOR + GridSquareMaxDistance + SEPARATOR +
                   "AreaOfOccupancy" + SEPARATOR + (((Double)AreaOfOccupancy) / 1000000) + SEPARATOR +
                   "ExtentOfOccurrence" + SEPARATOR + (((Double)ExtentOfOccurrence) / 1000000);
        }
    }
}

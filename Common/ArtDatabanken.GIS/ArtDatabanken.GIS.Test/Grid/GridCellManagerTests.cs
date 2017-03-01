using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.GIS.Grid;
using ArtDatabanken.GIS.SwedenExtent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.GIS.Test.Grid
{
    [TestClass]
    public class GridCellManagerTests
    {
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GenerateGridCellsAndCalculateNumberOfGridCells_SwedenExtent_GridCellsCountAndCalculatedNumberOfGridCellsShouldBeEqual()
        {
            GridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCellSize = 10000;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.BoundingBox = SwedenExtentManager.GetSwedenExtentBoundingBox(new CoordinateSystem(CoordinateSystemId.SWEREF99_TM));
            CoordinateSystem displayCoordinateSystem = new CoordinateSystem(CoordinateSystemId.GoogleMercator);            
            GridCellManager gridCellManager = new GridCellManager();

            List<GridCellBase> gridCells = gridCellManager.GenerateGrid(gridSpecification, displayCoordinateSystem);
            long calculatedNumberOfGridCells = gridCellManager.CalculateNumberOfGridCells(gridSpecification);

            Assert.AreEqual(gridCells.Count, calculatedNumberOfGridCells);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void GenerateGridCellsAndCalculateNumberOfGridCells_CustomBoundingBox_GridCellsCountAndCalculatedNumberOfGridCellsShouldBeEqual()
        {
            GridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCellSize = 25000;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.GoogleMercator;
            gridSpecification.BoundingBox = new BoundingBox();
            gridSpecification.BoundingBox.Min = new Point(0, 0);
            gridSpecification.BoundingBox.Max = new Point(100000, 50000);
            CoordinateSystem displayCoordinateSystem = new CoordinateSystem(CoordinateSystemId.GoogleMercator);
            GridCellManager gridCellManager = new GridCellManager();

            List<GridCellBase> gridCells = gridCellManager.GenerateGrid(gridSpecification, displayCoordinateSystem);
            long calculatedNumberOfGridCells = gridCellManager.CalculateNumberOfGridCells(gridSpecification);

            Assert.AreEqual(gridCells.Count, calculatedNumberOfGridCells);
        }

        [TestMethod, ExpectedException(typeof (ArgumentException), "GridCellSize must be greater than 0.")]
        [TestCategory("UnitTestApp")]
        public void GenerateGrid_GridCellSizeIsZero_ArgumentExceptionIsThrown()
        {
            GridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCellSize = 0;
            GridCellManager gridCellManager = new GridCellManager();
            gridCellManager.GenerateGrid(gridSpecification, new CoordinateSystem(CoordinateSystemId.GoogleMercator));            
        }

        [TestMethod, ExpectedException(typeof(ArgumentException), "GridCellSize must be greater than 0.")]
        [TestCategory("UnitTestApp")]
        public void CalculateNumberOfGridCells_GridCellSizeIsZero_ArgumentExceptionIsThrown()
        {
            GridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCellSize = 0;
            GridCellManager gridCellManager = new GridCellManager();
            gridCellManager.CalculateNumberOfGridCells(gridSpecification);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void CalculateNumberOfGridCells_BoundingBoxIsNotSpecified_SwedenExtentIsUsedAsBoundingBox()
        {
            GridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCellSize = 10000;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            GridCellManager gridCellManager = new GridCellManager();
            long calculatedNumberOfGridCells = gridCellManager.CalculateNumberOfGridCells(gridSpecification);

            gridSpecification.BoundingBox = SwedenExtentManager.GetSwedenExtentBoundingBox(new CoordinateSystem(CoordinateSystemId.SWEREF99_TM));
            long calculatedNumberOfGridCellsWithBbox = gridCellManager.CalculateNumberOfGridCells(gridSpecification);

            Assert.AreEqual(calculatedNumberOfGridCellsWithBbox, calculatedNumberOfGridCells);            
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetMissingGridCells_SwedenExtentAndEightGridCells_ReturnSwedenExtentGridCellsExceptEightGridCells()
        {
            List<GridCellBase> swedenExtentGridCells = CreateGridCells(10000, SwedenExtentManager.GetSwedenExtentBoundingBox(new CoordinateSystem(CoordinateSystemId.SWEREF99_TM)));
            BoundingBox boundingBox = new BoundingBox
            {
                Min = new Point(400000, 7000000),
                Max = new Point(400000 + 20000, 7000000 + 40000) 
            };
            List<GridCellBase> eightGridCellsInsideSwedenExtent = CreateGridCells(10000, boundingBox);            
            GridCellManager gridCellManager = new GridCellManager();
            List<IGridCellBase> missingGridCells = gridCellManager.GetMissingGridCells(swedenExtentGridCells.Cast<IGridCellBase>().ToList(), eightGridCellsInsideSwedenExtent.Cast<IGridCellBase>().ToList());

            Assert.AreEqual(swedenExtentGridCells.Count, missingGridCells.Count + eightGridCellsInsideSwedenExtent.Count);            
        }


        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetMissingGridCells_SwedenExtentAndFiveThousandGridCells_ReturnSwedenExtentGridCellsExceptFiveThousandGridCells()
        {
            List<GridCellBase> swedenExtentGridCells = CreateGridCells(10000, SwedenExtentManager.GetSwedenExtentBoundingBox(new CoordinateSystem(CoordinateSystemId.SWEREF99_TM)));
            BoundingBox boundingBox = new BoundingBox
            {
                Min = new Point(300000, 6100000),
                Max = new Point(300000 + (50 * 10000), 6100000 + (100 * 10000))
            };
            List<GridCellBase> fiveThousandGridCellsInsideSwedenExtent = CreateGridCells(10000, boundingBox);
            GridCellManager gridCellManager = new GridCellManager();
            List<IGridCellBase> missingGridCells = gridCellManager.GetMissingGridCells(swedenExtentGridCells.Cast<IGridCellBase>().ToList(), fiveThousandGridCellsInsideSwedenExtent.Cast<IGridCellBase>().ToList());
            
            Assert.AreEqual(swedenExtentGridCells.Count, missingGridCells.Count + fiveThousandGridCellsInsideSwedenExtent.Count);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetMissingGridCells_SwedenExtentAndEightGridCellsOutSideSwedenExtent_ReturnSwedenExtentGridCells()
        {
            List<GridCellBase> swedenExtentGridCells = CreateGridCells(10000, SwedenExtentManager.GetSwedenExtentBoundingBox(new CoordinateSystem(CoordinateSystemId.SWEREF99_TM)));
            BoundingBox boundingBox = new BoundingBox
            {
                Min = new Point(1400000, 7000000), // X value is outside sweden extent.
                Max = new Point(1400000 + 20000, 7000000 + 40000) // X value is outside sweden extent.
            };
            List<GridCellBase> eightGridCellsInsideSwedenExtent = CreateGridCells(10000, boundingBox);
            GridCellManager gridCellManager = new GridCellManager();
            List<IGridCellBase> missingGridCells = gridCellManager.GetMissingGridCells(swedenExtentGridCells.Cast<IGridCellBase>().ToList(), eightGridCellsInsideSwedenExtent.Cast<IGridCellBase>().ToList());

            Assert.AreEqual(swedenExtentGridCells.Count, missingGridCells.Count);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetMissingGridCells_SwedenExtentMinusSwedenExtent_ReturnZeroGridCells()
        {
            List<GridCellBase> swedenExtentGridCells = CreateGridCells(10000, SwedenExtentManager.GetSwedenExtentBoundingBox(new CoordinateSystem(CoordinateSystemId.SWEREF99_TM)));                        
            GridCellManager gridCellManager = new GridCellManager();
            List<IGridCellBase> missingGridCells = gridCellManager.GetMissingGridCells(swedenExtentGridCells.Cast<IGridCellBase>().ToList(), swedenExtentGridCells.Cast<IGridCellBase>().ToList());
            
            Assert.AreEqual(0, missingGridCells.Count);
        }

        private List<GridCellBase> CreateGridCells(int gridCellSize, BoundingBox boundingBox, GridCoordinateSystem gridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM, CoordinateSystemId displayCoordinateSystemId = CoordinateSystemId.GoogleMercator)
        {
            GridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCellSize = gridCellSize;
            gridSpecification.GridCoordinateSystem = gridCoordinateSystem;
            gridSpecification.BoundingBox = boundingBox;            
            CoordinateSystem displayCoordinateSystem = new CoordinateSystem(displayCoordinateSystemId);
            GridCellManager gridCellManager = new GridCellManager();
            List<GridCellBase> gridCells = gridCellManager.GenerateGrid(gridSpecification, displayCoordinateSystem);
            long calculatedNumberOfGridCells = gridCellManager.CalculateNumberOfGridCells(gridSpecification);
            Assert.AreEqual(gridCells.Count, calculatedNumberOfGridCells);
            return gridCells;
        }
    }
}

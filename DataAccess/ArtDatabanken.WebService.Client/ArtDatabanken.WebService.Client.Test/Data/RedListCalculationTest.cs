using System;
using ArtDatabanken.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class RedListCalculationTest : TestBase
    {
        private RedListCalculation _redListCalculation;

        public RedListCalculationTest()
        {
            _redListCalculation = null;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void AreaOfOccupancy()
        {
            Double areaOfOccupancy;

            // Test value.
            GetRedListCalculation(true);
            for (areaOfOccupancy = RedListCalculation.AREA_OF_OCCUPANCY_MIN; areaOfOccupancy <= Settings.Default.AreaOfOccupancyTestMax; areaOfOccupancy += Settings.Default.AreaOfOccupancyTestIncrease)
            {
                GetRedListCalculation().AreaOfOccupancy = areaOfOccupancy;
                Assert.AreEqual(areaOfOccupancy, GetRedListCalculation().AreaOfOccupancy);
            }

            // Test category.
            for (areaOfOccupancy = RedListCalculation.AREA_OF_OCCUPANCY_MIN; areaOfOccupancy <= Settings.Default.AreaOfOccupancyTestMax; areaOfOccupancy += Settings.Default.AreaOfOccupancyTestIncrease)
            {
                GetRedListCalculation(true).AreaOfOccupancy = areaOfOccupancy;
                GetRedListCalculation().NumberOfLocations = RedListCalculation.NUMBER_OF_LOCATIONS_MIN + 1;
                GetRedListCalculation().ContinuingDecline = GetRedListCalculation().ContinuingDeclineMax;
                GetRedListCalculation().IsCriteriaBB1Fulfilled = true;
                if (areaOfOccupancy <= RedListCalculation.CRITERIA_AREA_OF_OCCUPANCY_RE_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.RE);
                    continue;
                }

                if (areaOfOccupancy < RedListCalculation.CRITERIA_B2_CR_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.CR, GetRedListCalculation().Category);
                    continue;
                }

                if (areaOfOccupancy < RedListCalculation.CRITERIA_B2_EN_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.EN, GetRedListCalculation().Category);
                    continue;
                }

                if (areaOfOccupancy < RedListCalculation.CRITERIA_B2_VU_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.VU, GetRedListCalculation().Category);
                    continue;
                }

                if (areaOfOccupancy < RedListCalculation.CRITERIA_B2_NT_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
                    continue;
                }

                Assert.AreEqual(RedListCategory.LC, GetRedListCalculation().Category);
            }

            // Test criteria.
            GetRedListCalculation(true).NumberOfLocations = RedListCalculation.NUMBER_OF_LOCATIONS_MIN + 1;
            GetRedListCalculation().ContinuingDecline = GetRedListCalculation().ContinuingDeclineMax;
            GetRedListCalculation().IsCriteriaBB1Fulfilled = true;
            GetRedListCalculation().AreaOfOccupancy = RedListCalculation.AREA_OF_OCCUPANCY_MIN + 1;
            Assert.AreEqual("B2ab(i)", GetRedListCalculation().Criteria);
            GetRedListCalculation().AreaOfOccupancy = RedListCalculation.AREA_OF_OCCUPANCY_MAX;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void AreaOfOccupancyToSmallValueError()
        {
            GetRedListCalculation().AreaOfOccupancy = -1;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void AreaOfOccupancyMax()
        {
            Assert.IsTrue(RedListCalculation.AREA_OF_OCCUPANCY_MIN < RedListCalculation.AREA_OF_OCCUPANCY_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_B2_CR_LIMIT <= RedListCalculation.AREA_OF_OCCUPANCY_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_B2_EN_LIMIT <= RedListCalculation.AREA_OF_OCCUPANCY_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_B2_VU_LIMIT <= RedListCalculation.AREA_OF_OCCUPANCY_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_B2_NT_LIMIT <= RedListCalculation.AREA_OF_OCCUPANCY_MAX);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void AreaOfOccupancyMin()
        {
            Assert.IsTrue(RedListCalculation.AREA_OF_OCCUPANCY_MIN < RedListCalculation.AREA_OF_OCCUPANCY_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_B2_CR_LIMIT >= RedListCalculation.AREA_OF_OCCUPANCY_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_B2_EN_LIMIT >= RedListCalculation.AREA_OF_OCCUPANCY_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_B2_VU_LIMIT >= RedListCalculation.AREA_OF_OCCUPANCY_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_B2_NT_LIMIT >= RedListCalculation.AREA_OF_OCCUPANCY_MIN);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void Category()
        {
            // Test RE conditions.
            GetRedListCalculation(true).AreaOfOccupancy = RedListCalculation.AREA_OF_OCCUPANCY_MIN;
            Assert.AreEqual(RedListCategory.RE, GetRedListCalculation().Category);
            GetRedListCalculation(true).ExtentOfOccurrence = RedListCalculation.EXTENT_OF_OCCURRENCE_MIN;
            Assert.AreEqual(RedListCategory.RE, GetRedListCalculation().Category);
            GetRedListCalculation(true).NumberOfLocations = RedListCalculation.NUMBER_OF_LOCATIONS_MIN;
            Assert.AreEqual(RedListCategory.RE, GetRedListCalculation().Category);
            GetRedListCalculation(true).PopulationSize = RedListCalculation.POPULATION_SIZE_MIN;
            Assert.AreEqual(RedListCategory.RE, GetRedListCalculation().Category);

            // Test combinations of a criteria.
            CategoryTestA(RedListCalculation.POPULATION_REDUCTION_A1_MAX,
                          RedListCalculation.CRITERIA_A1_CR_LIMIT,
                          RedListCalculation.CRITERIA_A2_CR_LIMIT - 1,
                          RedListCalculation.CRITERIA_A2_EN_LIMIT,
                          RedListCalculation.CRITERIA_A3_CR_LIMIT - 1,
                          RedListCalculation.CRITERIA_A3_EN_LIMIT,
                          RedListCalculation.CRITERIA_A4_CR_LIMIT - 1,
                          RedListCalculation.CRITERIA_A4_EN_LIMIT,
                          RedListCategory.CR);
            CategoryTestA(RedListCalculation.CRITERIA_A1_CR_LIMIT - 1,
                          RedListCalculation.CRITERIA_A1_EN_LIMIT,
                          RedListCalculation.POPULATION_REDUCTION_A2_MAX,
                          RedListCalculation.CRITERIA_A2_CR_LIMIT,
                          RedListCalculation.CRITERIA_A3_CR_LIMIT - 1,
                          RedListCalculation.CRITERIA_A3_EN_LIMIT,
                          RedListCalculation.CRITERIA_A4_CR_LIMIT - 1,
                          RedListCalculation.CRITERIA_A4_EN_LIMIT,
                          RedListCategory.CR);
            CategoryTestA(RedListCalculation.CRITERIA_A1_CR_LIMIT - 1,
                          RedListCalculation.CRITERIA_A1_EN_LIMIT,
                          RedListCalculation.CRITERIA_A2_CR_LIMIT - 1,
                          RedListCalculation.CRITERIA_A2_EN_LIMIT,
                          RedListCalculation.POPULATION_REDUCTION_A3_MAX,
                          RedListCalculation.CRITERIA_A3_CR_LIMIT,
                          RedListCalculation.CRITERIA_A4_CR_LIMIT - 1,
                          RedListCalculation.CRITERIA_A4_EN_LIMIT,
                          RedListCategory.CR);
            CategoryTestA(RedListCalculation.CRITERIA_A1_CR_LIMIT - 1,
                          RedListCalculation.CRITERIA_A1_EN_LIMIT,
                          RedListCalculation.CRITERIA_A2_CR_LIMIT - 1,
                          RedListCalculation.CRITERIA_A2_EN_LIMIT,
                          RedListCalculation.CRITERIA_A3_CR_LIMIT - 1,
                          RedListCalculation.CRITERIA_A3_EN_LIMIT,
                          RedListCalculation.POPULATION_REDUCTION_A4_MAX,
                          RedListCalculation.CRITERIA_A4_CR_LIMIT,
                          RedListCategory.CR);

            CategoryTestA(RedListCalculation.CRITERIA_A1_CR_LIMIT - 1,
                          RedListCalculation.CRITERIA_A1_EN_LIMIT,
                          RedListCalculation.CRITERIA_A2_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_A2_VU_LIMIT,
                          RedListCalculation.CRITERIA_A3_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_A3_VU_LIMIT,
                          RedListCalculation.CRITERIA_A4_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_A4_VU_LIMIT,
                          RedListCategory.EN);
            CategoryTestA(RedListCalculation.CRITERIA_A1_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_A1_VU_LIMIT,
                          RedListCalculation.CRITERIA_A2_CR_LIMIT - 1,
                          RedListCalculation.CRITERIA_A2_EN_LIMIT,
                          RedListCalculation.CRITERIA_A3_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_A3_VU_LIMIT,
                          RedListCalculation.CRITERIA_A4_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_A4_VU_LIMIT,
                          RedListCategory.EN);
            CategoryTestA(RedListCalculation.CRITERIA_A1_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_A1_VU_LIMIT,
                          RedListCalculation.CRITERIA_A2_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_A2_VU_LIMIT,
                          RedListCalculation.CRITERIA_A3_CR_LIMIT - 1,
                          RedListCalculation.CRITERIA_A3_EN_LIMIT,
                          RedListCalculation.CRITERIA_A4_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_A4_VU_LIMIT,
                          RedListCategory.EN);
            CategoryTestA(RedListCalculation.CRITERIA_A1_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_A1_VU_LIMIT,
                          RedListCalculation.CRITERIA_A2_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_A2_VU_LIMIT,
                          RedListCalculation.CRITERIA_A3_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_A3_VU_LIMIT,
                          RedListCalculation.CRITERIA_A4_CR_LIMIT - 1,
                          RedListCalculation.CRITERIA_A4_EN_LIMIT,
                          RedListCategory.EN);

            CategoryTestA(RedListCalculation.CRITERIA_A1_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_A1_VU_LIMIT,
                          RedListCalculation.CRITERIA_A2_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_A2_NT_LIMIT,
                          RedListCalculation.CRITERIA_A3_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_A3_NT_LIMIT,
                          RedListCalculation.CRITERIA_A4_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_A4_NT_LIMIT,
                          RedListCategory.VU);
            CategoryTestA(RedListCalculation.CRITERIA_A1_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_A1_NT_LIMIT,
                          RedListCalculation.CRITERIA_A2_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_A2_VU_LIMIT,
                          RedListCalculation.CRITERIA_A3_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_A3_NT_LIMIT,
                          RedListCalculation.CRITERIA_A4_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_A4_NT_LIMIT,
                          RedListCategory.VU);
            CategoryTestA(RedListCalculation.CRITERIA_A1_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_A1_NT_LIMIT,
                          RedListCalculation.CRITERIA_A2_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_A2_NT_LIMIT,
                          RedListCalculation.CRITERIA_A3_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_A3_VU_LIMIT,
                          RedListCalculation.CRITERIA_A4_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_A4_NT_LIMIT,
                          RedListCategory.VU);
            CategoryTestA(RedListCalculation.CRITERIA_A1_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_A1_NT_LIMIT,
                          RedListCalculation.CRITERIA_A2_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_A2_NT_LIMIT,
                          RedListCalculation.CRITERIA_A3_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_A3_NT_LIMIT,
                          RedListCalculation.CRITERIA_A4_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_A4_VU_LIMIT,
                          RedListCategory.VU);

            CategoryTestA(RedListCalculation.CRITERIA_A1_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_A1_NT_LIMIT,
                          RedListCalculation.CRITERIA_A2_NT_LIMIT - 1,
                          Settings.Default.PopulationReductionA2TestMin,
                          RedListCalculation.CRITERIA_A3_NT_LIMIT - 1,
                          Settings.Default.PopulationReductionA3TestMin,
                          RedListCalculation.CRITERIA_A4_NT_LIMIT - 1,
                          Settings.Default.PopulationReductionA4TestMin,
                          RedListCategory.NT);
            CategoryTestA(RedListCalculation.CRITERIA_A1_NT_LIMIT - 1,
                          Settings.Default.PopulationReductionA1TestMin,
                          RedListCalculation.CRITERIA_A2_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_A2_NT_LIMIT,
                          RedListCalculation.CRITERIA_A3_NT_LIMIT - 1,
                          Settings.Default.PopulationReductionA3TestMin,
                          RedListCalculation.CRITERIA_A4_NT_LIMIT - 1,
                          Settings.Default.PopulationReductionA4TestMin,
                          RedListCategory.NT);
            CategoryTestA(RedListCalculation.CRITERIA_A1_NT_LIMIT - 1,
                          Settings.Default.PopulationReductionA1TestMin,
                          RedListCalculation.CRITERIA_A2_NT_LIMIT - 1,
                          Settings.Default.PopulationReductionA2TestMin,
                          RedListCalculation.CRITERIA_A3_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_A3_NT_LIMIT,
                          RedListCalculation.CRITERIA_A4_NT_LIMIT - 1,
                          Settings.Default.PopulationReductionA4TestMin,
                          RedListCategory.NT);
            CategoryTestA(RedListCalculation.CRITERIA_A1_NT_LIMIT - 1,
                          Settings.Default.PopulationReductionA1TestMin,
                          RedListCalculation.CRITERIA_A2_NT_LIMIT - 1,
                          Settings.Default.PopulationReductionA2TestMin,
                          RedListCalculation.CRITERIA_A3_NT_LIMIT - 1,
                          Settings.Default.PopulationReductionA3TestMin,
                          RedListCalculation.CRITERIA_A4_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_A4_NT_LIMIT,
                          RedListCategory.NT);

            CategoryTestB(RedListCalculation.AREA_OF_OCCUPANCY_MIN + 1,
                          RedListCalculation.CRITERIA_B1_CR_LIMIT - 1,
                          RedListCalculation.CRITERIA_B2_CR_LIMIT,
                          RedListCalculation.CRITERIA_B2_EN_LIMIT,
                          6,
                          4,
                          RedListCategory.CR);
            CategoryTestB(RedListCalculation.CRITERIA_B1_CR_LIMIT,
                          RedListCalculation.CRITERIA_B1_EN_LIMIT,
                          RedListCalculation.EXTENT_OF_OCCURRENCE_MIN + 1,
                          RedListCalculation.CRITERIA_B2_CR_LIMIT - 1,
                          6,
                          4,
                          RedListCategory.CR);

            CategoryTestB(RedListCalculation.AREA_OF_OCCUPANCY_MIN + 1,
                          RedListCalculation.CRITERIA_B1_CR_LIMIT - 1,
                          RedListCalculation.CRITERIA_B2_CR_LIMIT,
                          RedListCalculation.CRITERIA_B2_EN_LIMIT,
                          3,
                          3,
                          RedListCategory.EN);
            CategoryTestB(RedListCalculation.CRITERIA_B1_CR_LIMIT,
                          RedListCalculation.CRITERIA_B1_EN_LIMIT,
                          RedListCalculation.EXTENT_OF_OCCURRENCE_MIN + 1,
                          RedListCalculation.CRITERIA_B2_CR_LIMIT - 1,
                          3,
                          3,
                          RedListCategory.EN);
            CategoryTestB(RedListCalculation.CRITERIA_B1_CR_LIMIT,
                          RedListCalculation.CRITERIA_B1_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_B2_EN_LIMIT,
                          RedListCalculation.CRITERIA_B2_VU_LIMIT,
                          6,
                          4,
                          RedListCategory.EN);
            CategoryTestB(RedListCalculation.CRITERIA_B1_EN_LIMIT,
                          RedListCalculation.CRITERIA_B1_VU_LIMIT,
                          RedListCalculation.CRITERIA_B2_CR_LIMIT,
                          RedListCalculation.CRITERIA_B2_EN_LIMIT - 1,
                          6,
                          4,
                          RedListCategory.EN);

            CategoryTestB(RedListCalculation.CRITERIA_B1_CR_LIMIT,
                          RedListCalculation.CRITERIA_B1_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_B2_EN_LIMIT,
                          RedListCalculation.CRITERIA_B2_VU_LIMIT,
                          3,
                          3,
                          RedListCategory.VU);
            CategoryTestB(RedListCalculation.CRITERIA_B1_EN_LIMIT,
                          RedListCalculation.CRITERIA_B1_VU_LIMIT,
                          RedListCalculation.CRITERIA_B2_CR_LIMIT,
                          RedListCalculation.CRITERIA_B2_EN_LIMIT - 1,
                          3,
                          3,
                          RedListCategory.VU);
            CategoryTestB(RedListCalculation.CRITERIA_B1_EN_LIMIT,
                          RedListCalculation.CRITERIA_B1_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_B2_VU_LIMIT,
                          RedListCalculation.CRITERIA_B2_NT_LIMIT,
                          6,
                          4,
                          RedListCategory.VU);
            CategoryTestB(RedListCalculation.CRITERIA_B1_VU_LIMIT,
                          RedListCalculation.CRITERIA_B1_NT_LIMIT,
                          RedListCalculation.CRITERIA_B2_EN_LIMIT,
                          RedListCalculation.CRITERIA_B2_VU_LIMIT - 1,
                          6,
                          4,
                          RedListCategory.VU);

            CategoryTestB(RedListCalculation.CRITERIA_B1_EN_LIMIT,
                          RedListCalculation.CRITERIA_B1_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_B2_VU_LIMIT,
                          RedListCalculation.CRITERIA_B2_NT_LIMIT,
                          3,
                          3,
                          RedListCategory.NT);
            CategoryTestB(RedListCalculation.CRITERIA_B1_VU_LIMIT,
                          RedListCalculation.CRITERIA_B1_NT_LIMIT,
                          RedListCalculation.CRITERIA_B2_EN_LIMIT,
                          RedListCalculation.CRITERIA_B2_VU_LIMIT - 1,
                          3,
                          3,
                          RedListCategory.NT);
            CategoryTestB(RedListCalculation.AREA_OF_OCCUPANCY_MIN + 1,
                          RedListCalculation.CRITERIA_B1_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_B2_VU_LIMIT,
                          RedListCalculation.CRITERIA_B2_NT_LIMIT,
                          2,
                          2,
                          RedListCategory.NT);
            CategoryTestB(RedListCalculation.CRITERIA_B1_VU_LIMIT,
                          RedListCalculation.CRITERIA_B1_NT_LIMIT,
                          RedListCalculation.EXTENT_OF_OCCURRENCE_MIN + 1,
                          RedListCalculation.CRITERIA_B2_EN_LIMIT - 1,
                          2,
                          2,
                          RedListCategory.NT);
            CategoryTestB(RedListCalculation.CRITERIA_B1_VU_LIMIT,
                          RedListCalculation.CRITERIA_B1_NT_LIMIT - 1,
                          RedListCalculation.CRITERIA_B2_NT_LIMIT,
                          RedListCalculation.CRITERIA_B2_NT_LIMIT,
                          6,
                          4,
                          RedListCategory.NT);
            CategoryTestB(RedListCalculation.CRITERIA_B1_NT_LIMIT,
                          RedListCalculation.CRITERIA_B1_NT_LIMIT,
                          RedListCalculation.CRITERIA_B2_VU_LIMIT,
                          RedListCalculation.CRITERIA_B2_NT_LIMIT - 1,
                          6,
                          4,
                          RedListCategory.NT);

            CategoryTestB(RedListCalculation.CRITERIA_B1_EN_LIMIT,
                          Settings.Default.AreaOfOccupancyTestMax,
                          RedListCalculation.CRITERIA_B2_NT_LIMIT,
                          RedListCalculation.CRITERIA_B2_NT_LIMIT,
                          2,
                          0,
                          RedListCategory.LC);
            CategoryTestB(RedListCalculation.CRITERIA_B1_NT_LIMIT,
                          RedListCalculation.CRITERIA_B1_NT_LIMIT,
                          RedListCalculation.CRITERIA_B2_EN_LIMIT,
                          Settings.Default.ExtentOfOccurrenceTestMax,
                          2,
                          0,
                          RedListCategory.LC);
            CategoryTestB(RedListCalculation.CRITERIA_B1_NT_LIMIT,
                          Settings.Default.AreaOfOccupancyTestMax,
                          RedListCalculation.CRITERIA_B2_NT_LIMIT,
                          RedListCalculation.CRITERIA_B2_NT_LIMIT,
                          6,
                          3,
                          RedListCategory.LC);
            CategoryTestB(RedListCalculation.CRITERIA_B1_NT_LIMIT,
                          RedListCalculation.CRITERIA_B1_NT_LIMIT,
                          RedListCalculation.CRITERIA_B2_NT_LIMIT,
                          Settings.Default.ExtentOfOccurrenceTestMax,
                          6,
                          3,
                          RedListCategory.LC);

            CategoryTestC(RedListCalculation.POPULATION_SIZE_MIN + 1,
                          RedListCalculation.CRITERIA_C_CR_LIMIT - 1,
                          RedListCalculation.CRITERIA_C1_CR_LIMIT,
                          GetRedListCalculation().ContinuingDeclineMax,
                          RedListCalculation.CRITERIA_C2A1_CR_LIMIT + 1,
                          RedListCalculation.CRITERIA_C2A1_EN_LIMIT,
                          Settings.Default.MaxProportionLocalPopulationTestMin,
                          RedListCalculation.CRITERIA_C2A2_CR_LIMIT - 1,
                          RedListCategory.CR);
            CategoryTestC(RedListCalculation.POPULATION_SIZE_MIN + 1,
                          RedListCalculation.CRITERIA_C_CR_LIMIT - 1,
                          RedListCalculation.CRITERIA_C1_EN_LIMIT,
                          RedListCalculation.CRITERIA_C1_CR_LIMIT - 1,
                          RedListCalculation.MAX_SIZE_LOCAL_POPULATION_MIN,
                          RedListCalculation.CRITERIA_C2A1_CR_LIMIT,
                          Settings.Default.MaxProportionLocalPopulationTestMin,
                          RedListCalculation.CRITERIA_C2A2_CR_LIMIT - 1,
                          RedListCategory.CR);
            CategoryTestC(RedListCalculation.POPULATION_SIZE_MIN + 1,
                          RedListCalculation.CRITERIA_C_CR_LIMIT - 1,
                          RedListCalculation.CRITERIA_C1_EN_LIMIT,
                          RedListCalculation.CRITERIA_C1_CR_LIMIT - 1,
                          RedListCalculation.CRITERIA_C2A1_CR_LIMIT + 1,
                          RedListCalculation.CRITERIA_C2A1_EN_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_CR_LIMIT,
                          RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MAX,
                          RedListCategory.CR);

            CategoryTestC(RedListCalculation.CRITERIA_C_CR_LIMIT,
                          RedListCalculation.CRITERIA_C_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_C1_EN_LIMIT,
                          RedListCalculation.CRITERIA_C1_CR_LIMIT - 1,
                          RedListCalculation.CRITERIA_C2A1_EN_LIMIT + 1,
                          RedListCalculation.CRITERIA_C2A1_VU_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_CR_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_EN_LIMIT - 1,
                          RedListCategory.EN);
            CategoryTestC(RedListCalculation.CRITERIA_C_CR_LIMIT,
                          RedListCalculation.CRITERIA_C_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_C1_VU_LIMIT,
                          RedListCalculation.CRITERIA_C1_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_C2A1_CR_LIMIT + 1,
                          RedListCalculation.CRITERIA_C2A1_EN_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_CR_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_EN_LIMIT - 1,
                          RedListCategory.EN);
            CategoryTestC(RedListCalculation.CRITERIA_C_CR_LIMIT,
                          RedListCalculation.CRITERIA_C_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_C1_VU_LIMIT,
                          RedListCalculation.CRITERIA_C1_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_C2A1_EN_LIMIT + 1,
                          RedListCalculation.CRITERIA_C2A1_VU_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_EN_LIMIT,
                          RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MAX,
                          RedListCategory.EN);

            CategoryTestC(RedListCalculation.CRITERIA_C_EN_LIMIT,
                          RedListCalculation.CRITERIA_C_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_C1_VU_LIMIT,
                          RedListCalculation.CRITERIA_C1_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_C2A1_VU_LIMIT + 1,
                          RedListCalculation.CRITERIA_C2A1_NT_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_CR_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_NT_LIMIT - 1,
                          RedListCategory.VU);
            CategoryTestC(RedListCalculation.CRITERIA_C_EN_LIMIT,
                          RedListCalculation.CRITERIA_C_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_C1_NT_LIMIT,
                          RedListCalculation.CRITERIA_C1_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_C2A1_EN_LIMIT + 1,
                          RedListCalculation.CRITERIA_C2A1_VU_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_CR_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_NT_LIMIT - 1,
                          RedListCategory.VU);
            CategoryTestC(RedListCalculation.CRITERIA_C_EN_LIMIT,
                          RedListCalculation.CRITERIA_C_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_C1_NT_LIMIT,
                          RedListCalculation.CRITERIA_C1_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_C2A1_VU_LIMIT + 1,
                          RedListCalculation.CRITERIA_C2A1_NT_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_VU_LIMIT,
                          RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MAX,
                          RedListCategory.VU);

            CategoryTestC(RedListCalculation.CRITERIA_C_VU_LIMIT,
                          RedListCalculation.CRITERIA_C_NT_LIMIT - 1,
                          RedListCalculation.CRITERIA_C1_VU_LIMIT,
                          RedListCalculation.CRITERIA_C1_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_C2A1_VU_LIMIT + 1,
                          RedListCalculation.CRITERIA_C2A1_NT_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_CR_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_NT_LIMIT - 1,
                          RedListCategory.NT);
            CategoryTestC(RedListCalculation.CRITERIA_C_VU_LIMIT,
                          RedListCalculation.CRITERIA_C_NT_LIMIT - 1,
                          RedListCalculation.CRITERIA_C1_NT_LIMIT,
                          RedListCalculation.CRITERIA_C1_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_C2A1_EN_LIMIT + 1,
                          RedListCalculation.CRITERIA_C2A1_VU_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_CR_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_NT_LIMIT - 1,
                          RedListCategory.NT);
            CategoryTestC(RedListCalculation.CRITERIA_C_VU_LIMIT,
                          RedListCalculation.CRITERIA_C_NT_LIMIT - 1,
                          RedListCalculation.CRITERIA_C1_NT_LIMIT,
                          RedListCalculation.CRITERIA_C1_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_C2A1_VU_LIMIT + 1,
                          RedListCalculation.CRITERIA_C2A1_NT_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_VU_LIMIT,
                          RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MAX,
                          RedListCategory.NT);

            CategoryTestC(RedListCalculation.CRITERIA_C_EN_LIMIT,
                          RedListCalculation.CRITERIA_C_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_C1_NT_LIMIT,
                          RedListCalculation.CRITERIA_C1_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_C2A1_NT_LIMIT + 1,
                          Settings.Default.MaxSizeLocalPopulationTestMax,
                          RedListCalculation.CRITERIA_C2A2_CR_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_NT_LIMIT - 1,
                          RedListCategory.NT);
            CategoryTestC(RedListCalculation.CRITERIA_C_EN_LIMIT,
                          RedListCalculation.CRITERIA_C_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_C2_LIMIT,
                          RedListCalculation.CRITERIA_C1_NT_LIMIT - 1,
                          RedListCalculation.CRITERIA_C2A1_VU_LIMIT + 1,
                          RedListCalculation.CRITERIA_C2A1_NT_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_CR_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_NT_LIMIT - 1,
                          RedListCategory.NT);

            CategoryTestC(RedListCalculation.CRITERIA_C_NT_LIMIT,
                          Settings.Default.PopulationSizeTestMax,
                          RedListCalculation.CRITERIA_C1_VU_LIMIT,
                          RedListCalculation.CRITERIA_C1_EN_LIMIT - 1,
                          RedListCalculation.CRITERIA_C2A1_VU_LIMIT + 1,
                          RedListCalculation.CRITERIA_C2A1_NT_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_CR_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_NT_LIMIT - 1,
                          RedListCategory.LC);
            CategoryTestC(RedListCalculation.CRITERIA_C_NT_LIMIT,
                          Settings.Default.PopulationSizeTestMax,
                          RedListCalculation.CRITERIA_C1_NT_LIMIT,
                          RedListCalculation.CRITERIA_C1_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_C2A1_EN_LIMIT + 1,
                          RedListCalculation.CRITERIA_C2A1_VU_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_CR_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_NT_LIMIT - 1,
                          RedListCategory.LC);
            CategoryTestC(RedListCalculation.CRITERIA_C_NT_LIMIT,
                          Settings.Default.PopulationSizeTestMax,
                          RedListCalculation.CRITERIA_C1_NT_LIMIT,
                          RedListCalculation.CRITERIA_C1_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_C2A1_VU_LIMIT + 1,
                          RedListCalculation.CRITERIA_C2A1_NT_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_VU_LIMIT,
                          RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MAX,
                          RedListCategory.LC);

            CategoryTestC(RedListCalculation.CRITERIA_C_VU_LIMIT,
                          RedListCalculation.CRITERIA_C_NT_LIMIT - 1,
                          RedListCalculation.CRITERIA_C1_NT_LIMIT,
                          RedListCalculation.CRITERIA_C1_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_C2A1_NT_LIMIT + 1,
                          Settings.Default.MaxSizeLocalPopulationTestMax,
                          RedListCalculation.CRITERIA_C2A2_CR_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_NT_LIMIT - 1,
                          RedListCategory.LC);
            CategoryTestC(RedListCalculation.CRITERIA_C_VU_LIMIT,
                          RedListCalculation.CRITERIA_C_NT_LIMIT - 1,
                          RedListCalculation.CRITERIA_C2_LIMIT,
                          RedListCalculation.CRITERIA_C1_NT_LIMIT - 1,
                          RedListCalculation.CRITERIA_C2A1_VU_LIMIT + 1,
                          RedListCalculation.CRITERIA_C2A1_NT_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_CR_LIMIT,
                          RedListCalculation.CRITERIA_C2A2_NT_LIMIT - 1,
                          RedListCategory.LC);

            CategoryTestD(RedListCalculation.POPULATION_SIZE_MIN + 1,
                          RedListCalculation.CRITERIA_D_CR_LIMIT - 1,
                          GetRedListCalculation().VeryRestrictedAreaMin,
                          GetRedListCalculation().VeryRestrictedAreaMax,
                          RedListCategory.CR);
            CategoryTestD(RedListCalculation.CRITERIA_D_CR_LIMIT,
                          RedListCalculation.CRITERIA_D_EN_LIMIT - 1,
                          GetRedListCalculation().VeryRestrictedAreaMin,
                          GetRedListCalculation().VeryRestrictedAreaMax,
                          RedListCategory.EN);
            CategoryTestD(RedListCalculation.CRITERIA_D_EN_LIMIT,
                          RedListCalculation.CRITERIA_D1_VU_LIMIT - 1,
                          RedListCalculation.CRITERIA_D2_NT_LIMIT,
                          GetRedListCalculation().VeryRestrictedAreaMax,
                          RedListCategory.VU);
            CategoryTestD(RedListCalculation.CRITERIA_D1_VU_LIMIT,
                          RedListCalculation.CRITERIA_D1_NT_LIMIT - 1,
                          GetRedListCalculation().VeryRestrictedAreaMin,
                          RedListCalculation.CRITERIA_D2_VU_LIMIT,
                          RedListCategory.VU);
            CategoryTestD(RedListCalculation.CRITERIA_D1_VU_LIMIT,
                          RedListCalculation.CRITERIA_D1_NT_LIMIT - 1,
                          RedListCalculation.CRITERIA_D2_NT_LIMIT + 1,
                          GetRedListCalculation().VeryRestrictedAreaMax,
                          RedListCategory.NT);
            CategoryTestD(RedListCalculation.CRITERIA_D1_NT_LIMIT,
                          Settings.Default.PopulationSizeTestMax,
                          RedListCalculation.CRITERIA_D2_VU_LIMIT + 1,
                          RedListCalculation.CRITERIA_D2_NT_LIMIT,
                          RedListCategory.NT);
            CategoryTestD(RedListCalculation.CRITERIA_D1_NT_LIMIT,
                          Settings.Default.PopulationSizeTestMax,
                          RedListCalculation.CRITERIA_D2_NT_LIMIT + 1,
                          GetRedListCalculation().VeryRestrictedAreaMax,
                          RedListCategory.LC);

            CategoryTestE(GetRedListCalculation().ProbabilityOfExtinctionMin,
                          RedListCalculation.CRITERIA_E_CR_LIMIT,
                          RedListCategory.CR);
            CategoryTestE(RedListCalculation.CRITERIA_E_CR_LIMIT + 1,
                          RedListCalculation.CRITERIA_E_EN_LIMIT,
                          RedListCategory.EN);
            CategoryTestE(RedListCalculation.CRITERIA_E_EN_LIMIT + 1,
                          RedListCalculation.CRITERIA_E_VU_LIMIT,
                          RedListCategory.VU);
            CategoryTestE(RedListCalculation.CRITERIA_E_VU_LIMIT + 1,
                          RedListCalculation.CRITERIA_E_NT_LIMIT,
                          RedListCategory.NT);
            CategoryTestE(RedListCalculation.CRITERIA_E_NT_LIMIT + 1,
                          GetRedListCalculation().ProbabilityOfExtinctionMax,
                          RedListCategory.LC);
        }

        private void CategoryTestA(Double populationReductionA1TestMax,
                                   Double populationReductionA1TestMin,
                                   Double populationReductionA2TestMax,
                                   Double populationReductionA2TestMin,
                                   Double populationReductionA3TestMax,
                                   Double populationReductionA3TestMin,
                                   Double populationReductionA4TestMax,
                                   Double populationReductionA4TestMin,
                                   RedListCategory category)
        {
            Double populationReductionA1, populationReductionA2,
                   populationReductionA3, populationReductionA4;

            GetRedListCalculation(true).IsCriteriaA1AFulfilled = true;
            GetRedListCalculation().IsCriteriaA2AFulfilled = true;
            GetRedListCalculation().IsCriteriaA3BFulfilled = true;
            GetRedListCalculation().IsCriteriaA4AFulfilled = true;
            for (populationReductionA1 = populationReductionA1TestMin; populationReductionA1 <= populationReductionA1TestMax; populationReductionA1++)
            {
                GetRedListCalculation().PopulationReductionA1 = populationReductionA1;
                for (populationReductionA2 = populationReductionA2TestMin; populationReductionA2 <= populationReductionA2TestMax; populationReductionA2++)
                {
                    GetRedListCalculation().PopulationReductionA2 = populationReductionA2;
                    for (populationReductionA3 = populationReductionA3TestMin; populationReductionA3 <= populationReductionA3TestMax; populationReductionA3++)
                    {
                        GetRedListCalculation().PopulationReductionA3 = populationReductionA3;
                        for (populationReductionA4 = populationReductionA4TestMin; populationReductionA4 <= populationReductionA4TestMax; populationReductionA4++)
                        {
                            GetRedListCalculation().PopulationReductionA4 = populationReductionA4;
                            Assert.AreEqual(category, GetRedListCalculation().Category);
                        }
                    }
                }
            }
        }

        private void CategoryTestB(Double areaOfOccupancyMin,
                                   Double areaOfOccupancyMax,
                                   Double extentOfOccurrenceMin,
                                   Double extentOfOccurrenceMax,
                                   Int32 criteriaBCountMax,
                                   Int32 criteriaBCountMin,
                                   RedListCategory category)
        {
            Double areaOfOccupancy;
            Double extentOfOccurrence;
            Int32 criteriaBCount;

            for (areaOfOccupancy = areaOfOccupancyMin; areaOfOccupancy <= areaOfOccupancyMax; areaOfOccupancy += Settings.Default.AreaOfOccupancyTestIncrease)
            {
                for (extentOfOccurrence = extentOfOccurrenceMin; extentOfOccurrence <= extentOfOccurrenceMax; extentOfOccurrence += Settings.Default.ExtentOfOccurrenceTestIncrease)
                {
                    for (criteriaBCount = criteriaBCountMin; criteriaBCount <= criteriaBCountMax; criteriaBCount++)
                    {
                        GetRedListCalculation(true).AreaOfOccupancy = areaOfOccupancy;
                        GetRedListCalculation().ExtentOfOccurrence = extentOfOccurrence;
                        GetRedListCalculation().IsCriteriaBB1Fulfilled = true;
                        GetRedListCalculation().IsCriteriaBC1Fulfilled = true;
                        if (criteriaBCount >= 1)
                        {
                            GetRedListCalculation().SeverlyFragmented = RedListCalculation.CRITERIA_BA_1_LIMIT;
                        }

                        if (criteriaBCount >= 2)
                        {
                            GetRedListCalculation().SeverlyFragmented = RedListCalculation.CRITERIA_BA_2_LIMIT;
                        }

                        if (criteriaBCount >= 3)
                        {
                            GetRedListCalculation().ContinuingDecline = RedListCalculation.CRITERIA_BB_1_LIMIT;
                        }

                        if (criteriaBCount >= 4)
                        {
                            GetRedListCalculation().ContinuingDecline = RedListCalculation.CRITERIA_BB_2_LIMIT;
                        }

                        if (criteriaBCount >= 5)
                        {
                            GetRedListCalculation().ExtremeFluctuations = RedListCalculation.CRITERIA_BC_1_LIMIT;
                        }

                        if (criteriaBCount >= 6)
                        {
                            GetRedListCalculation().ExtremeFluctuations = RedListCalculation.CRITERIA_BC_2_LIMIT;
                        }

                        if (GetRedListCalculation().Category == RedListCategory.RE)
                        {
                            Assert.AreEqual(category, GetRedListCalculation().Category);
                        }
                    }
                }
            }
        }

        private void CategoryTestC(Int64 populationSizeMin,
                                   Int64 populationSizeMax,
                                   Int32 continuingDeclineMin,
                                   Int32 continuingDeclineMax,
                                   Int64 maxSizeLocalPopulationMin,
                                   Int64 maxSizeLocalPopulationMax,
                                   Double maxProportionLocalPopulationMin,
                                   Double maxProportionLocalPopulationMax,
                                   RedListCategory category)
        {
            Double maxProportionLocalPopulation;
            Int32 continuingDecline;
            Int64 maxSizeLocalPopulation;
            Int64 populationSize;

            for (populationSize = populationSizeMin; populationSize <= populationSizeMax; populationSize += Settings.Default.PopulationSizeTestIncrease)
            {
                for (continuingDecline = continuingDeclineMin; continuingDecline <= continuingDeclineMax; continuingDecline++)
                {
                    for (maxSizeLocalPopulation = maxSizeLocalPopulationMin; maxSizeLocalPopulation <= maxSizeLocalPopulationMax; maxSizeLocalPopulation += Settings.Default.MaxSizeLocalPopulationTestIncrease)
                    {
                        for (maxProportionLocalPopulation = maxProportionLocalPopulationMin; maxProportionLocalPopulation <= maxProportionLocalPopulationMax; maxProportionLocalPopulation += 1)
                        {
                            GetRedListCalculation(true).PopulationSize = populationSize;
                            GetRedListCalculation().ContinuingDecline = continuingDecline;
                            GetRedListCalculation().MaxSizeLocalPopulation = maxSizeLocalPopulation;
                            GetRedListCalculation().MaxProportionLocalPopulation = maxProportionLocalPopulation;
                            Assert.AreEqual(category, GetRedListCalculation().Category);
                        }
                    }
                }
            }
        }

        private void CategoryTestD(Int64 populationSizeMin,
                                   Int64 populationSizeMax,
                                   Int32 veryRestrictedAreaMin,
                                   Int32 veryRestrictedAreaMax,
                                   RedListCategory category)
        {
            Int32 veryRestrictedArea;
            Int64 populationSize;

            for (populationSize = populationSizeMin; populationSize <= populationSizeMax; populationSize += Settings.Default.PopulationSizeTestIncrease)
            {
                for (veryRestrictedArea = veryRestrictedAreaMin; veryRestrictedArea <= veryRestrictedAreaMax; veryRestrictedArea++)
                {
                    GetRedListCalculation(true).PopulationSize = populationSize;
                    GetRedListCalculation().VeryRestrictedArea = veryRestrictedArea;
                    Assert.AreEqual(category, GetRedListCalculation().Category);
                }
            }
        }

        private void CategoryTestE(Int32 probabilityOfExtinctionMin,
                                   Int32 probabilityOfExtinctionMax,
                                   RedListCategory category)
        {
            Int32 probabilityOfExtinction;

            for (probabilityOfExtinction = probabilityOfExtinctionMin; probabilityOfExtinction <= probabilityOfExtinctionMax; probabilityOfExtinction++)
            {
                GetRedListCalculation(true).ProbabilityOfExtinction = probabilityOfExtinction;
                Assert.AreEqual(category, GetRedListCalculation().Category);
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void Constructor()
        {
            Assert.IsNotNull(GetRedListCalculation(true));
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void ContinuingDecline()
        {
            Int32 continuingDecline;

            // Test value.
            GetRedListCalculation(true);
            for (continuingDecline = GetRedListCalculation().ContinuingDeclineMin; continuingDecline <= GetRedListCalculation().ContinuingDeclineMax; continuingDecline++)
            {
                GetRedListCalculation().ContinuingDecline = continuingDecline;
                Assert.AreEqual(continuingDecline, GetRedListCalculation().ContinuingDecline);
            }

            // Test category.
            for (continuingDecline = GetRedListCalculation().ContinuingDeclineMin; continuingDecline <= GetRedListCalculation().ContinuingDeclineMax; continuingDecline++)
            {
                GetRedListCalculation(true).ContinuingDecline = continuingDecline;
                GetRedListCalculation().AreaOfOccupancy = RedListCalculation.AREA_OF_OCCUPANCY_MIN + 1;
                GetRedListCalculation().NumberOfLocations = RedListCalculation.NUMBER_OF_LOCATIONS_MIN + 1;
                GetRedListCalculation().IsCriteriaBB1Fulfilled = true;
                if (continuingDecline >= RedListCalculation.CRITERIA_BB_2_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.CR, GetRedListCalculation().Category);
                    continue;
                }

                if (continuingDecline >= RedListCalculation.CRITERIA_BB_1_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.EN, GetRedListCalculation().Category);
                    continue;
                }

                Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
            }

            // Test criteria.
            GetRedListCalculation(true).AreaOfOccupancy = RedListCalculation.AREA_OF_OCCUPANCY_MIN + 1;
            GetRedListCalculation().ExtentOfOccurrence = RedListCalculation.EXTENT_OF_OCCURRENCE_MIN + 1;
            GetRedListCalculation().IsCriteriaBB1Fulfilled = true;
            GetRedListCalculation().PopulationSize = RedListCalculation.CRITERIA_C_NT_LIMIT - 1;
            GetRedListCalculation().MaxSizeLocalPopulation = RedListCalculation.MAX_SIZE_LOCAL_POPULATION_MIN;
            GetRedListCalculation().ContinuingDecline = GetRedListCalculation().ContinuingDeclineMin;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().ContinuingDecline = GetRedListCalculation().ContinuingDeclineMax;
            Assert.AreEqual("B1b(i)+2b(i); C1+2a(i)", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void ContinuingDeclineToLargeValueError()
        {
            GetRedListCalculation().ContinuingDecline = 1000;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void ContinuingDeclineToSmallValueError()
        {
            GetRedListCalculation().ContinuingDecline = -1000;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void ContinuingDeclineMax()
        {
            Assert.IsTrue(GetRedListCalculation().ContinuingDeclineMin < GetRedListCalculation().ContinuingDeclineMax);
            Assert.IsTrue(RedListCalculation.CRITERIA_BB_2_LIMIT <= GetRedListCalculation().ContinuingDeclineMax);
            Assert.IsTrue(RedListCalculation.CRITERIA_BB_1_LIMIT <= GetRedListCalculation().ContinuingDeclineMax);
            Assert.IsTrue(RedListCalculation.CRITERIA_C2_LIMIT <= GetRedListCalculation().ContinuingDeclineMax);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void ContinuingDeclineMin()
        {
            Assert.IsTrue(GetRedListCalculation().ContinuingDeclineMin < GetRedListCalculation().ContinuingDeclineMax);
            Assert.IsTrue(RedListCalculation.CRITERIA_BB_2_LIMIT >= GetRedListCalculation().ContinuingDeclineMin);
            Assert.IsTrue(RedListCalculation.CRITERIA_BB_1_LIMIT >= GetRedListCalculation().ContinuingDeclineMin);
            Assert.IsTrue(RedListCalculation.CRITERIA_C2_LIMIT >= GetRedListCalculation().ContinuingDeclineMin);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void Criteria()
        {
            GetRedListCalculation(true).PopulationReductionA2 = 90;
            GetRedListCalculation().IsCriteriaA2AFulfilled = true;
            GetRedListCalculation().IsCriteriaA2CFulfilled = true;
            Assert.AreEqual("A2ac", GetRedListCalculation().Criteria);

            GetRedListCalculation(true).PopulationReductionA2 = 90;
            GetRedListCalculation().IsCriteriaA2BFulfilled = true;
            GetRedListCalculation().IsCriteriaA2CFulfilled = true;
            GetRedListCalculation().IsCriteriaA2EFulfilled = true;
            Assert.AreEqual("A2bce", GetRedListCalculation().Criteria);

            GetRedListCalculation(true).PopulationReductionA2 = 90;
            GetRedListCalculation().IsCriteriaA2BFulfilled = true;
            GetRedListCalculation().IsCriteriaA2DFulfilled = true;
            GetRedListCalculation().IsCriteriaA2EFulfilled = true;
            GetRedListCalculation().PopulationReductionA3 = 90;
            GetRedListCalculation().IsCriteriaA3BFulfilled = true;
            GetRedListCalculation().IsCriteriaA3DFulfilled = true;
            GetRedListCalculation().IsCriteriaA3EFulfilled = true;
            GetRedListCalculation().PopulationReductionA4 = 90;
            GetRedListCalculation().IsCriteriaA4BFulfilled = true;
            GetRedListCalculation().IsCriteriaA4DFulfilled = true;
            GetRedListCalculation().IsCriteriaA4EFulfilled = true;
            Assert.AreEqual("A2bde+3bde+4bde", GetRedListCalculation().Criteria);

            GetRedListCalculation(true).PopulationReductionA2 = 90;
            GetRedListCalculation().IsCriteriaA2CFulfilled = true;
            GetRedListCalculation().PopulationReductionA3 = 90;
            GetRedListCalculation().IsCriteriaA3CFulfilled = true;
            GetRedListCalculation().PopulationReductionA4 = 90;
            GetRedListCalculation().IsCriteriaA4CFulfilled = true;
            Assert.AreEqual("A2c+3c+4c", GetRedListCalculation().Criteria);

            GetRedListCalculation(true).PopulationReductionA1 = 90;
            GetRedListCalculation().IsCriteriaA1AFulfilled = true;
            GetRedListCalculation().IsCriteriaA1BFulfilled = true;
            GetRedListCalculation().IsCriteriaA1CFulfilled = true;
            GetRedListCalculation().IsCriteriaA1DFulfilled = true;
            GetRedListCalculation().PopulationReductionA2 = 90;
            GetRedListCalculation().IsCriteriaA2BFulfilled = true;
            GetRedListCalculation().IsCriteriaA2CFulfilled = true;
            GetRedListCalculation().IsCriteriaA2DFulfilled = true;
            Assert.AreEqual("A1abcd+2bcd", GetRedListCalculation().Criteria);

            GetRedListCalculation(true).ExtentOfOccurrence = 90;
            GetRedListCalculation().NumberOfLocations = 1;
            GetRedListCalculation().ContinuingDecline = GetRedListCalculation().ContinuingDeclineMax;
            GetRedListCalculation().IsCriteriaBB2Fulfilled = true;
            GetRedListCalculation().IsCriteriaBB3Fulfilled = true;
            GetRedListCalculation().AreaOfOccupancy = 5;
            Assert.AreEqual("B1ab(ii,iii)+2ab(ii,iii)", GetRedListCalculation().Criteria);

            GetRedListCalculation(true).PopulationReductionA2 = 90;
            GetRedListCalculation().IsCriteriaA2CFulfilled = true;
            GetRedListCalculation().PopulationReductionA3 = 90;
            GetRedListCalculation().IsCriteriaA3CFulfilled = true;
            GetRedListCalculation().PopulationReductionA4 = 90;
            GetRedListCalculation().IsCriteriaA4CFulfilled = true;
            GetRedListCalculation().AreaOfOccupancy = 8;
            GetRedListCalculation().NumberOfLocations = 1;
            GetRedListCalculation().ContinuingDecline = GetRedListCalculation().ContinuingDeclineMax;
            GetRedListCalculation().IsCriteriaBB2Fulfilled = true;
            GetRedListCalculation().IsCriteriaBB3Fulfilled = true;
            GetRedListCalculation().IsCriteriaBB4Fulfilled = true;
            Assert.AreEqual("A2c+3c+4c; B2ab(ii,iii,iv)", GetRedListCalculation().Criteria);

            GetRedListCalculation(true).ExtentOfOccurrence = 90;
            GetRedListCalculation().NumberOfLocations = 1;
            GetRedListCalculation().IsCriteriaBB2Fulfilled = true;
            GetRedListCalculation().IsCriteriaBB3Fulfilled = true;
            GetRedListCalculation().AreaOfOccupancy = 9;
            GetRedListCalculation().PopulationSize = 200;
            GetRedListCalculation().ContinuingDecline = 2;
            GetRedListCalculation().MaxSizeLocalPopulation = 40;
            Assert.AreEqual("B1ab(ii,iii)+2ab(ii,iii); C2a(i)", GetRedListCalculation().Criteria);

            GetRedListCalculation(true).PopulationSize = 49;
            Assert.AreEqual("D", GetRedListCalculation().Criteria);

            GetRedListCalculation(true).PopulationSize = 490;
            Assert.AreEqual("D1", GetRedListCalculation().Criteria);

            GetRedListCalculation(true).VeryRestrictedArea = 1;
            Assert.AreEqual("D2", GetRedListCalculation().Criteria);

            GetRedListCalculation(true).ProbabilityOfExtinction = 2;
            Assert.AreEqual("E", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void ExtentOfOccurrence()
        {
            Double extentOfOccurrence;

            // Test value.
            GetRedListCalculation(true);
            for (extentOfOccurrence = RedListCalculation.EXTENT_OF_OCCURRENCE_MIN; extentOfOccurrence <= Settings.Default.ExtentOfOccurrenceTestMax; extentOfOccurrence += Settings.Default.ExtentOfOccurrenceTestIncrease)
            {
                GetRedListCalculation().ExtentOfOccurrence = extentOfOccurrence;
                Assert.AreEqual(extentOfOccurrence, GetRedListCalculation().ExtentOfOccurrence);
            }

            // Test category.
            for (extentOfOccurrence = RedListCalculation.EXTENT_OF_OCCURRENCE_MIN; extentOfOccurrence <= Settings.Default.ExtentOfOccurrenceTestMax; extentOfOccurrence += Settings.Default.ExtentOfOccurrenceTestIncrease)
            {
                GetRedListCalculation(true).ExtentOfOccurrence = extentOfOccurrence;
                GetRedListCalculation().NumberOfLocations = RedListCalculation.NUMBER_OF_LOCATIONS_MIN + 1;
                GetRedListCalculation().ContinuingDecline = GetRedListCalculation().ContinuingDeclineMax;
                GetRedListCalculation().IsCriteriaBB1Fulfilled = true;
                if (extentOfOccurrence <= RedListCalculation.CRITERIA_EXTENT_OF_OCCURRENCE_RE_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.RE);
                    continue;
                }

                if (extentOfOccurrence < RedListCalculation.CRITERIA_B1_CR_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.CR, GetRedListCalculation().Category);
                    continue;
                }

                if (extentOfOccurrence < RedListCalculation.CRITERIA_B1_EN_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.EN, GetRedListCalculation().Category);
                    continue;
                }

                if (extentOfOccurrence < RedListCalculation.CRITERIA_B1_VU_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.VU, GetRedListCalculation().Category);
                    continue;
                }

                if (extentOfOccurrence < RedListCalculation.CRITERIA_B1_NT_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
                    continue;
                }

                Assert.AreEqual(RedListCategory.LC, GetRedListCalculation().Category);
            }

            // Test criteria.
            GetRedListCalculation(true).NumberOfLocations = RedListCalculation.NUMBER_OF_LOCATIONS_MIN + 1;
            GetRedListCalculation().ExtentOfOccurrence = RedListCalculation.EXTENT_OF_OCCURRENCE_MIN + 1;
            Assert.AreEqual("B1a", GetRedListCalculation().Criteria);
            GetRedListCalculation().ExtentOfOccurrence = RedListCalculation.EXTENT_OF_OCCURRENCE_MAX;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void ExtentOfOccurrenceToSmallValueError()
        {
            GetRedListCalculation().ExtentOfOccurrence = -1;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void ExtentOfOccurrenceMax()
        {
            Assert.IsTrue(RedListCalculation.EXTENT_OF_OCCURRENCE_MIN < RedListCalculation.EXTENT_OF_OCCURRENCE_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_B1_CR_LIMIT <= RedListCalculation.EXTENT_OF_OCCURRENCE_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_B1_EN_LIMIT <= RedListCalculation.EXTENT_OF_OCCURRENCE_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_B1_VU_LIMIT <= RedListCalculation.EXTENT_OF_OCCURRENCE_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_B1_NT_LIMIT <= RedListCalculation.EXTENT_OF_OCCURRENCE_MAX);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void ExtentOfOccurrenceMin()
        {
            Assert.IsTrue(RedListCalculation.EXTENT_OF_OCCURRENCE_MIN < RedListCalculation.EXTENT_OF_OCCURRENCE_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_B1_CR_LIMIT >= RedListCalculation.EXTENT_OF_OCCURRENCE_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_B1_EN_LIMIT >= RedListCalculation.EXTENT_OF_OCCURRENCE_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_B1_VU_LIMIT >= RedListCalculation.EXTENT_OF_OCCURRENCE_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_B1_NT_LIMIT >= RedListCalculation.EXTENT_OF_OCCURRENCE_MIN);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void ExtremeFluctuations()
        {
            Int32 extremeFluctuations;

            // Test value.
            GetRedListCalculation(true);
            for (extremeFluctuations = GetRedListCalculation().ExtremeFluctuationsMin; extremeFluctuations <= GetRedListCalculation().ExtremeFluctuationsMax; extremeFluctuations++)
            {
                GetRedListCalculation().ExtremeFluctuations = extremeFluctuations;
                Assert.AreEqual(extremeFluctuations, GetRedListCalculation().ExtremeFluctuations);
            }

            // Test category.
            for (extremeFluctuations = GetRedListCalculation().ExtremeFluctuationsMin; extremeFluctuations <= GetRedListCalculation().ExtremeFluctuationsMax; extremeFluctuations++)
            {
                GetRedListCalculation(true).ExtremeFluctuations = extremeFluctuations;
                GetRedListCalculation().AreaOfOccupancy = RedListCalculation.AREA_OF_OCCUPANCY_MIN + 1;
                GetRedListCalculation().NumberOfLocations = RedListCalculation.NUMBER_OF_LOCATIONS_MIN + 1;
                GetRedListCalculation().IsCriteriaBC1Fulfilled = true;
                if (extremeFluctuations >= RedListCalculation.CRITERIA_BC_2_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.CR, GetRedListCalculation().Category);
                    continue;
                }

                if (extremeFluctuations >= RedListCalculation.CRITERIA_BC_1_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.EN, GetRedListCalculation().Category);
                    continue;
                }

                Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
            }

            // Test criteria.
            GetRedListCalculation(true).AreaOfOccupancy = RedListCalculation.AREA_OF_OCCUPANCY_MIN + 1;
            GetRedListCalculation().ExtentOfOccurrence = RedListCalculation.EXTENT_OF_OCCURRENCE_MIN + 1;
            GetRedListCalculation().IsCriteriaBC1Fulfilled = true;
            GetRedListCalculation().PopulationSize = RedListCalculation.CRITERIA_C_NT_LIMIT - 1;
            GetRedListCalculation().ContinuingDecline = RedListCalculation.CRITERIA_C2_LIMIT;
            GetRedListCalculation().ExtremeFluctuations = GetRedListCalculation().ExtremeFluctuationsMin;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().ExtremeFluctuations = GetRedListCalculation().ExtremeFluctuationsMax;
            Assert.AreEqual("B1c(i)+2c(i); C2b", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void ExtremeFluctuationsToLargeValueError()
        {
            GetRedListCalculation().ExtremeFluctuations = 1000;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void ExtremeFluctuationsToSmallValueError()
        {
            GetRedListCalculation().ExtremeFluctuations = -1000;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void ExtremeFluctuationsMax()
        {
            Assert.IsTrue(GetRedListCalculation().ExtremeFluctuationsMin < GetRedListCalculation().ExtremeFluctuationsMax);
            Assert.IsTrue(RedListCalculation.CRITERIA_BC_2_LIMIT <= GetRedListCalculation().ExtremeFluctuationsMax);
            Assert.IsTrue(RedListCalculation.CRITERIA_BC_1_LIMIT <= GetRedListCalculation().ExtremeFluctuationsMax);
            Assert.IsTrue(RedListCalculation.CRITERIA_C2B_LIMIT <= GetRedListCalculation().ExtremeFluctuationsMax);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void ExtremeFluctuationsMin()
        {
            Assert.IsTrue(GetRedListCalculation().ExtremeFluctuationsMin < GetRedListCalculation().ExtremeFluctuationsMax);
            Assert.IsTrue(RedListCalculation.CRITERIA_BC_2_LIMIT >= GetRedListCalculation().ExtremeFluctuationsMin);
            Assert.IsTrue(RedListCalculation.CRITERIA_BC_1_LIMIT >= GetRedListCalculation().ExtremeFluctuationsMin);
            Assert.IsTrue(RedListCalculation.CRITERIA_C2B_LIMIT >= GetRedListCalculation().ExtremeFluctuationsMin);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetCriteriaBCount()
        {
            Int32 criteriaBCount;
            Int32 continuingDecline;
            Int32 extremeFluctuations;
            Int64 numberOfLocations;
            Int32 severlyFragmented;

            for (severlyFragmented = GetRedListCalculation().SeverlyFragmentedMin; severlyFragmented <= GetRedListCalculation().SeverlyFragmentedMax; severlyFragmented++)
            {
                for (continuingDecline = GetRedListCalculation().ContinuingDeclineMin; continuingDecline <= GetRedListCalculation().ContinuingDeclineMax; continuingDecline++)
                {
                    for (extremeFluctuations = GetRedListCalculation().ExtremeFluctuationsMin; extremeFluctuations <= GetRedListCalculation().ExtremeFluctuationsMax; extremeFluctuations++)
                    {
                        for (numberOfLocations = RedListCalculation.NUMBER_OF_LOCATIONS_MIN; numberOfLocations <= Settings.Default.NumberOfLoactionsTestMax; numberOfLocations++)
                        {
                            criteriaBCount = 0;
                            if ((severlyFragmented >= RedListCalculation.CRITERIA_BA_2_LIMIT) ||
                                (numberOfLocations < RedListCalculation.CRITERIA_BA_NT_LIMIT))
                            {
                                criteriaBCount += 2;
                            }
                            else if (severlyFragmented >= RedListCalculation.CRITERIA_BA_1_LIMIT)
                            {
                                criteriaBCount += 1;
                            }

                            if (continuingDecline >= RedListCalculation.CRITERIA_BB_2_LIMIT)
                            {
                                criteriaBCount += 2;
                            }
                            else if (continuingDecline >= RedListCalculation.CRITERIA_BB_1_LIMIT)
                            {
                                criteriaBCount += 1;
                            }

                            if (extremeFluctuations >= RedListCalculation.CRITERIA_BC_2_LIMIT)
                            {
                                criteriaBCount += 2;
                            }
                            else if (extremeFluctuations >= RedListCalculation.CRITERIA_BC_1_LIMIT)
                            {
                                criteriaBCount += 1;
                            }

                            GetRedListCalculation(true).SeverlyFragmented = severlyFragmented;
                            GetRedListCalculation().NumberOfLocations = numberOfLocations;
                            GetRedListCalculation().ContinuingDecline = continuingDecline;
                            GetRedListCalculation().ExtremeFluctuations = extremeFluctuations;
                            GetRedListCalculation().IsCriteriaBB1Fulfilled = true;
                            GetRedListCalculation().IsCriteriaBC1Fulfilled = true;
                            Assert.AreEqual(criteriaBCount, GetRedListCalculation().GetCriteriaBCount(RedListCategory.NT));
                        }
                    }
                }
            }
        }

        private RedListCalculation GetRedListCalculation(Boolean refresh = false)
        {
            if (_redListCalculation.IsNull() || refresh)
            {
                _redListCalculation = new RedListCalculation(GetUserContext());
                _redListCalculation.IsEvaluationStatusSet = true;
            }

            return _redListCalculation;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void HasAreaOfOccupancy()
        {
            Assert.IsFalse(GetRedListCalculation(true).HasAreaOfOccupancy);
            GetRedListCalculation().AreaOfOccupancy = RedListCalculation.AREA_OF_OCCUPANCY_MAX;
            Assert.IsTrue(GetRedListCalculation().HasAreaOfOccupancy);
            GetRedListCalculation().HasAreaOfOccupancy = false;
            Assert.IsFalse(GetRedListCalculation().HasAreaOfOccupancy);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void HasAreaOfOccupancyTrueArgumentError()
        {
            GetRedListCalculation().HasAreaOfOccupancy = true;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void HasContinuingDecline()
        {
            Assert.IsFalse(GetRedListCalculation(true).HasContinuingDecline);
            GetRedListCalculation().ContinuingDecline = GetRedListCalculation().ContinuingDeclineMax;
            Assert.IsTrue(GetRedListCalculation().HasContinuingDecline);
            GetRedListCalculation().HasContinuingDecline = false;
            Assert.IsFalse(GetRedListCalculation().HasContinuingDecline);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void HasContinuingDeclineTrueArgumentError()
        {
            GetRedListCalculation().HasContinuingDecline = true;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void HasExtentOfOccurrence()
        {
            Assert.IsFalse(GetRedListCalculation(true).HasExtentOfOccurrence);
            GetRedListCalculation().ExtentOfOccurrence = RedListCalculation.EXTENT_OF_OCCURRENCE_MAX;
            Assert.IsTrue(GetRedListCalculation().HasExtentOfOccurrence);
            GetRedListCalculation().HasExtentOfOccurrence = false;
            Assert.IsFalse(GetRedListCalculation().HasExtentOfOccurrence);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void HasExtentOfOccurrenceTrueArgumentError()
        {
            GetRedListCalculation().HasExtentOfOccurrence = true;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void HasExtremeFluctuations()
        {
            Assert.IsFalse(GetRedListCalculation(true).HasExtremeFluctuations);
            GetRedListCalculation().ExtremeFluctuations = GetRedListCalculation().ExtremeFluctuationsMax;
            Assert.IsTrue(GetRedListCalculation().HasExtremeFluctuations);
            GetRedListCalculation().HasExtremeFluctuations = false;
            Assert.IsFalse(GetRedListCalculation().HasExtremeFluctuations);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void HasExtremeFluctuationsTrueArgumentError()
        {
            GetRedListCalculation().HasExtremeFluctuations = true;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void HasMaxProportionLocalPopulation()
        {
            Assert.IsFalse(GetRedListCalculation(true).HasMaxProportionLocalPopulation);
            GetRedListCalculation().MaxProportionLocalPopulation = RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MAX;
            Assert.IsTrue(GetRedListCalculation().HasMaxProportionLocalPopulation);
            GetRedListCalculation().HasMaxProportionLocalPopulation = false;
            Assert.IsFalse(GetRedListCalculation().HasMaxProportionLocalPopulation);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void HasMaxProportionLocalPopulationTrueArgumentError()
        {
            GetRedListCalculation().HasMaxProportionLocalPopulation = true;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void HasMaxSizeLocalPopulation()
        {
            Assert.IsFalse(GetRedListCalculation(true).HasMaxSizeLocalPopulation);
            GetRedListCalculation().MaxSizeLocalPopulation = RedListCalculation.MAX_SIZE_LOCAL_POPULATION_MAX;
            Assert.IsTrue(GetRedListCalculation().HasMaxSizeLocalPopulation);
            GetRedListCalculation().HasMaxSizeLocalPopulation = false;
            Assert.IsFalse(GetRedListCalculation().HasMaxSizeLocalPopulation);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void HasMaxSizeLocalPopulationTrueArgumentError()
        {
            GetRedListCalculation().HasMaxSizeLocalPopulation = true;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void HasNumberOfLocations()
        {
            Assert.IsFalse(GetRedListCalculation(true).HasNumberOfLocations);
            GetRedListCalculation().NumberOfLocations = RedListCalculation.NUMBER_OF_LOCATIONS_MAX;
            Assert.IsTrue(GetRedListCalculation().HasNumberOfLocations);
            GetRedListCalculation().HasNumberOfLocations = false;
            Assert.IsFalse(GetRedListCalculation().HasNumberOfLocations);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void HasNumberOfLocationsTrueArgumentError()
        {
            GetRedListCalculation().HasNumberOfLocations = true;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void HasPopulationReductionA1()
        {
            Assert.IsFalse(GetRedListCalculation(true).HasPopulationReductionA1);
            GetRedListCalculation().PopulationReductionA1 = RedListCalculation.POPULATION_REDUCTION_A1_MAX;
            Assert.IsTrue(GetRedListCalculation().HasPopulationReductionA1);
            GetRedListCalculation().HasPopulationReductionA1 = false;
            Assert.IsFalse(GetRedListCalculation().HasPopulationReductionA1);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void HasPopulationReductionA1TrueArgumentError()
        {
            GetRedListCalculation().HasPopulationReductionA1 = true;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void HasPopulationReductionA2()
        {
            Assert.IsFalse(GetRedListCalculation(true).HasPopulationReductionA2);
            GetRedListCalculation().PopulationReductionA2 = RedListCalculation.POPULATION_REDUCTION_A2_MAX;
            Assert.IsTrue(GetRedListCalculation().HasPopulationReductionA2);
            GetRedListCalculation().HasPopulationReductionA2 = false;
            Assert.IsFalse(GetRedListCalculation().HasPopulationReductionA2);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void HasPopulationReductionA2TrueArgumentError()
        {
            GetRedListCalculation().HasPopulationReductionA2 = true;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void HasPopulationReductionA3()
        {
            Assert.IsFalse(GetRedListCalculation(true).HasPopulationReductionA3);
            GetRedListCalculation().PopulationReductionA3 = RedListCalculation.POPULATION_REDUCTION_A3_MAX;
            Assert.IsTrue(GetRedListCalculation().HasPopulationReductionA3);
            GetRedListCalculation().HasPopulationReductionA3 = false;
            Assert.IsFalse(GetRedListCalculation().HasPopulationReductionA3);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void HasPopulationReductionA3TrueArgumentError()
        {
            GetRedListCalculation().HasPopulationReductionA3 = true;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void HasPopulationReductionA4()
        {
            Assert.IsFalse(GetRedListCalculation(true).HasPopulationReductionA4);
            GetRedListCalculation().PopulationReductionA4 = RedListCalculation.POPULATION_REDUCTION_A4_MAX;
            Assert.IsTrue(GetRedListCalculation().HasPopulationReductionA4);
            GetRedListCalculation().HasPopulationReductionA4 = false;
            Assert.IsFalse(GetRedListCalculation().HasPopulationReductionA4);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void HasPopulationReductionA4TrueArgumentError()
        {
            GetRedListCalculation().HasPopulationReductionA4 = true;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void HasPopulationSize()
        {
            Assert.IsFalse(GetRedListCalculation(true).HasPopulationSize);
            GetRedListCalculation().PopulationSize = RedListCalculation.POPULATION_SIZE_MAX;
            Assert.IsTrue(GetRedListCalculation().HasPopulationSize);
            GetRedListCalculation().HasPopulationSize = false;
            Assert.IsFalse(GetRedListCalculation().HasPopulationSize);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void HasPopulationSizeTrueArgumentError()
        {
            GetRedListCalculation().HasPopulationSize = true;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void HasProbabilityOfExtinction()
        {
            Assert.IsFalse(GetRedListCalculation(true).HasProbabilityOfExtinction);
            GetRedListCalculation().ProbabilityOfExtinction = GetRedListCalculation().ProbabilityOfExtinctionMax;
            Assert.IsTrue(GetRedListCalculation().HasProbabilityOfExtinction);
            GetRedListCalculation().HasProbabilityOfExtinction = false;
            Assert.IsFalse(GetRedListCalculation().HasProbabilityOfExtinction);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void HasProbabilityOfExtinctionTrueArgumentError()
        {
            GetRedListCalculation().HasProbabilityOfExtinction = true;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void HasSeverlyFragmented()
        {
            Assert.IsFalse(GetRedListCalculation(true).HasSeverlyFragmented);
            GetRedListCalculation().SeverlyFragmented = GetRedListCalculation().SeverlyFragmentedMax;
            Assert.IsTrue(GetRedListCalculation().HasSeverlyFragmented);
            GetRedListCalculation().HasSeverlyFragmented = false;
            Assert.IsFalse(GetRedListCalculation().HasSeverlyFragmented);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void HasSeverlyFragmentedTrueArgumentError()
        {
            GetRedListCalculation().HasSeverlyFragmented = true;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void HasVeryRestrictedArea()
        {
            Assert.IsFalse(GetRedListCalculation(true).HasVeryRestrictedArea);
            GetRedListCalculation().VeryRestrictedArea = GetRedListCalculation().VeryRestrictedAreaMax;
            Assert.IsTrue(GetRedListCalculation().HasVeryRestrictedArea);
            GetRedListCalculation().HasVeryRestrictedArea = false;
            Assert.IsFalse(GetRedListCalculation().HasVeryRestrictedArea);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void HasVeryRestrictedAreaTrueArgumentError()
        {
            GetRedListCalculation().HasVeryRestrictedArea = true;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaA1AFulfilled()
        {
            Double populationReduction;

            // Test value.
            GetRedListCalculation(true).IsCriteriaA1AFulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaA1AFulfilled);
            GetRedListCalculation().IsCriteriaA1AFulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaA1AFulfilled);

            // Test category.
            GetRedListCalculation(true);
            for (populationReduction = Settings.Default.PopulationReductionA1TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A1_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA1 = populationReduction;
                GetRedListCalculation().IsCriteriaA1AFulfilled = false;
                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
                GetRedListCalculation().IsCriteriaA1AFulfilled = true;
                if (populationReduction >= RedListCalculation.CRITERIA_A1_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A1_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A1_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A1_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).PopulationReductionA1 = RedListCalculation.POPULATION_REDUCTION_A1_MAX;
            GetRedListCalculation().IsCriteriaA1AFulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaA1AFulfilled = true;
            Assert.AreEqual("A1a", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaA1BFulfilled()
        {
            Double populationReduction;

            // Test value.
            GetRedListCalculation(true).IsCriteriaA1BFulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaA1BFulfilled);
            GetRedListCalculation().IsCriteriaA1BFulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaA1BFulfilled);

            // Test category.
            GetRedListCalculation(true);
            for (populationReduction = Settings.Default.PopulationReductionA1TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A1_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA1 = populationReduction;
                GetRedListCalculation().IsCriteriaA1BFulfilled = false;
                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
                GetRedListCalculation().IsCriteriaA1BFulfilled = true;
                if (populationReduction >= RedListCalculation.CRITERIA_A1_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A1_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A1_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A1_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).PopulationReductionA1 = RedListCalculation.POPULATION_REDUCTION_A1_MAX;
            GetRedListCalculation().IsCriteriaA1BFulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaA1BFulfilled = true;
            Assert.AreEqual("A1b", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaA1CFulfilled()
        {
            Double populationReduction;

            // Test value.
            GetRedListCalculation(true).IsCriteriaA1CFulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaA1CFulfilled);
            GetRedListCalculation().IsCriteriaA1CFulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaA1CFulfilled);

            // Test category.
            GetRedListCalculation(true);
            for (populationReduction = Settings.Default.PopulationReductionA1TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A1_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA1 = populationReduction;
                GetRedListCalculation().IsCriteriaA1CFulfilled = false;
                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
                GetRedListCalculation().IsCriteriaA1CFulfilled = true;
                if (populationReduction >= RedListCalculation.CRITERIA_A1_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A1_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A1_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A1_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).PopulationReductionA1 = RedListCalculation.POPULATION_REDUCTION_A1_MAX;
            GetRedListCalculation().IsCriteriaA1CFulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaA1CFulfilled = true;
            Assert.AreEqual("A1c", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaA1DFulfilled()
        {
            Double populationReduction;

            // Test value.
            GetRedListCalculation(true).IsCriteriaA1DFulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaA1DFulfilled);
            GetRedListCalculation().IsCriteriaA1DFulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaA1DFulfilled);

            // Test category.
            GetRedListCalculation(true);
            for (populationReduction = Settings.Default.PopulationReductionA1TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A1_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA1 = populationReduction;
                GetRedListCalculation().IsCriteriaA1DFulfilled = false;
                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
                GetRedListCalculation().IsCriteriaA1DFulfilled = true;
                if (populationReduction >= RedListCalculation.CRITERIA_A1_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A1_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A1_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A1_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).PopulationReductionA1 = RedListCalculation.POPULATION_REDUCTION_A1_MAX;
            GetRedListCalculation().IsCriteriaA1DFulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaA1DFulfilled = true;
            Assert.AreEqual("A1d", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaA1EFulfilled()
        {
            Double populationReduction;

            // Test value.
            GetRedListCalculation(true).IsCriteriaA1EFulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaA1EFulfilled);
            GetRedListCalculation().IsCriteriaA1EFulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaA1EFulfilled);

            // Test category.
            GetRedListCalculation(true);
            for (populationReduction = Settings.Default.PopulationReductionA1TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A1_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA1 = populationReduction;
                GetRedListCalculation().IsCriteriaA1EFulfilled = false;
                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
                GetRedListCalculation().IsCriteriaA1EFulfilled = true;
                if (populationReduction >= RedListCalculation.CRITERIA_A1_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A1_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A1_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A1_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).PopulationReductionA1 = RedListCalculation.POPULATION_REDUCTION_A1_MAX;
            GetRedListCalculation().IsCriteriaA1EFulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaA1EFulfilled = true;
            Assert.AreEqual("A1e", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaA2AFulfilled()
        {
            Double populationReduction;

            // Test value.
            GetRedListCalculation(true).IsCriteriaA2AFulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaA2AFulfilled);
            GetRedListCalculation().IsCriteriaA2AFulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaA2AFulfilled);

            // Test category.
            GetRedListCalculation(true);
            for (populationReduction = Settings.Default.PopulationReductionA1TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A2_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA2 = populationReduction;
                GetRedListCalculation().IsCriteriaA2AFulfilled = false;
                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
                GetRedListCalculation().IsCriteriaA2AFulfilled = true;
                if (populationReduction >= RedListCalculation.CRITERIA_A2_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A2_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A2_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A2_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).PopulationReductionA2 = RedListCalculation.POPULATION_REDUCTION_A2_MAX;
            GetRedListCalculation().IsCriteriaA2AFulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaA2AFulfilled = true;
            Assert.AreEqual("A2a", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaA2BFulfilled()
        {
            Double populationReduction;

            // Test value.
            GetRedListCalculation(true).IsCriteriaA2BFulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaA2BFulfilled);
            GetRedListCalculation().IsCriteriaA2BFulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaA2BFulfilled);

            // Test category.
            GetRedListCalculation(true);
            for (populationReduction = Settings.Default.PopulationReductionA2TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A2_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA2 = populationReduction;
                GetRedListCalculation().IsCriteriaA2BFulfilled = false;
                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
                GetRedListCalculation().IsCriteriaA2BFulfilled = true;
                if (populationReduction >= RedListCalculation.CRITERIA_A2_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A2_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A2_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A2_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).PopulationReductionA2 = RedListCalculation.POPULATION_REDUCTION_A2_MAX;
            GetRedListCalculation().IsCriteriaA2BFulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaA2BFulfilled = true;
            Assert.AreEqual("A2b", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaA2CFulfilled()
        {
            Double populationReduction;

            // Test value.
            GetRedListCalculation(true).IsCriteriaA2CFulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaA2CFulfilled);
            GetRedListCalculation().IsCriteriaA2CFulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaA2CFulfilled);

            // Test category.
            GetRedListCalculation(true);
            for (populationReduction = Settings.Default.PopulationReductionA2TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A2_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA2 = populationReduction;
                GetRedListCalculation().IsCriteriaA2CFulfilled = false;
                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
                GetRedListCalculation().IsCriteriaA2CFulfilled = true;
                if (populationReduction >= RedListCalculation.CRITERIA_A2_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A2_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A2_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A2_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).PopulationReductionA2 = RedListCalculation.POPULATION_REDUCTION_A2_MAX;
            GetRedListCalculation().IsCriteriaA2CFulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaA2CFulfilled = true;
            Assert.AreEqual("A2c", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaA2DFulfilled()
        {
            Double populationReduction;

            // Test value.
            GetRedListCalculation(true).IsCriteriaA2DFulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaA2DFulfilled);
            GetRedListCalculation().IsCriteriaA2DFulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaA2DFulfilled);

            // Test category.
            GetRedListCalculation(true);
            for (populationReduction = Settings.Default.PopulationReductionA2TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A2_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA2 = populationReduction;
                GetRedListCalculation().IsCriteriaA2DFulfilled = false;
                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
                GetRedListCalculation().IsCriteriaA2DFulfilled = true;
                if (populationReduction >= RedListCalculation.CRITERIA_A2_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A2_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A2_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A2_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).PopulationReductionA2 = RedListCalculation.POPULATION_REDUCTION_A2_MAX;
            GetRedListCalculation().IsCriteriaA2DFulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaA2DFulfilled = true;
            Assert.AreEqual("A2d", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaA2EFulfilled()
        {
            Double populationReduction;

            // Test value.
            GetRedListCalculation(true).IsCriteriaA2EFulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaA2EFulfilled);
            GetRedListCalculation().IsCriteriaA2EFulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaA2EFulfilled);

            // Test category.
            GetRedListCalculation(true);
            for (populationReduction = Settings.Default.PopulationReductionA2TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A2_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA2 = populationReduction;
                GetRedListCalculation().IsCriteriaA2EFulfilled = false;
                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
                GetRedListCalculation().IsCriteriaA2EFulfilled = true;
                if (populationReduction >= RedListCalculation.CRITERIA_A2_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A2_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A2_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A2_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).PopulationReductionA2 = RedListCalculation.POPULATION_REDUCTION_A2_MAX;
            GetRedListCalculation().IsCriteriaA2EFulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaA2EFulfilled = true;
            Assert.AreEqual("A2e", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaA3BFulfilled()
        {
            Double populationReduction;

            // Test value.
            GetRedListCalculation(true).IsCriteriaA3BFulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaA3BFulfilled);
            GetRedListCalculation().IsCriteriaA3BFulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaA3BFulfilled);

            // Test category.
            GetRedListCalculation(true);
            for (populationReduction = Settings.Default.PopulationReductionA3TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A3_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA3 = populationReduction;
                GetRedListCalculation().IsCriteriaA3BFulfilled = false;
                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
                GetRedListCalculation().IsCriteriaA3BFulfilled = true;
                if (populationReduction >= RedListCalculation.CRITERIA_A3_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A3_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A3_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A3_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).PopulationReductionA3 = RedListCalculation.POPULATION_REDUCTION_A3_MAX;
            GetRedListCalculation().IsCriteriaA3BFulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaA3BFulfilled = true;
            Assert.AreEqual("A3b", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaA3CFulfilled()
        {
            Double populationReduction;

            // Test value.
            GetRedListCalculation(true).IsCriteriaA3CFulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaA3CFulfilled);
            GetRedListCalculation().IsCriteriaA3CFulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaA3CFulfilled);

            // Test category.
            GetRedListCalculation(true);
            for (populationReduction = Settings.Default.PopulationReductionA3TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A3_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA3 = populationReduction;
                GetRedListCalculation().IsCriteriaA3CFulfilled = false;
                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
                GetRedListCalculation().IsCriteriaA3CFulfilled = true;
                if (populationReduction >= RedListCalculation.CRITERIA_A3_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A3_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A3_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A3_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).PopulationReductionA3 = RedListCalculation.POPULATION_REDUCTION_A3_MAX;
            GetRedListCalculation().IsCriteriaA3CFulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaA3CFulfilled = true;
            Assert.AreEqual("A3c", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaA3DFulfilled()
        {
            Double populationReduction;

            // Test value.
            GetRedListCalculation(true).IsCriteriaA3DFulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaA3DFulfilled);
            GetRedListCalculation().IsCriteriaA3DFulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaA3DFulfilled);

            // Test category.
            GetRedListCalculation(true);
            for (populationReduction = Settings.Default.PopulationReductionA3TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A3_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA3 = populationReduction;
                GetRedListCalculation().IsCriteriaA3DFulfilled = false;
                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
                GetRedListCalculation().IsCriteriaA3DFulfilled = true;
                if (populationReduction >= RedListCalculation.CRITERIA_A3_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A3_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A3_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A3_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).PopulationReductionA3 = RedListCalculation.POPULATION_REDUCTION_A3_MAX;
            GetRedListCalculation().IsCriteriaA3DFulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaA3DFulfilled = true;
            Assert.AreEqual("A3d", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaA3EFulfilled()
        {
            Double populationReduction;

            // Test value.
            GetRedListCalculation(true).IsCriteriaA3EFulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaA3EFulfilled);
            GetRedListCalculation().IsCriteriaA3EFulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaA3EFulfilled);

            // Test category.
            GetRedListCalculation(true);
            for (populationReduction = Settings.Default.PopulationReductionA3TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A3_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA3 = populationReduction;
                GetRedListCalculation().IsCriteriaA3EFulfilled = false;
                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
                GetRedListCalculation().IsCriteriaA3EFulfilled = true;
                if (populationReduction >= RedListCalculation.CRITERIA_A3_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A3_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A3_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A3_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).PopulationReductionA3 = RedListCalculation.POPULATION_REDUCTION_A3_MAX;
            GetRedListCalculation().IsCriteriaA3EFulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaA3EFulfilled = true;
            Assert.AreEqual("A3e", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaA4AFulfilled()
        {
            Double populationReduction;

            // Test value.
            GetRedListCalculation(true).IsCriteriaA4AFulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaA4AFulfilled);
            GetRedListCalculation().IsCriteriaA4AFulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaA4AFulfilled);

            // Test category.
            GetRedListCalculation(true);
            for (populationReduction = Settings.Default.PopulationReductionA4TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A4_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA4 = populationReduction;
                GetRedListCalculation().IsCriteriaA4AFulfilled = false;
                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
                GetRedListCalculation().IsCriteriaA4AFulfilled = true;
                if (populationReduction >= RedListCalculation.CRITERIA_A4_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A4_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A4_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A4_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).PopulationReductionA4 = RedListCalculation.POPULATION_REDUCTION_A4_MAX;
            GetRedListCalculation().IsCriteriaA4AFulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaA4AFulfilled = true;
            Assert.AreEqual("A4a", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaA4BFulfilled()
        {
            Double populationReduction;

            // Test value.
            GetRedListCalculation(true).IsCriteriaA4BFulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaA4BFulfilled);
            GetRedListCalculation().IsCriteriaA4BFulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaA4BFulfilled);

            // Test category.
            GetRedListCalculation(true);
            for (populationReduction = Settings.Default.PopulationReductionA4TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A4_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA4 = populationReduction;
                GetRedListCalculation().IsCriteriaA4BFulfilled = false;
                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
                GetRedListCalculation().IsCriteriaA4BFulfilled = true;
                if (populationReduction >= RedListCalculation.CRITERIA_A4_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A4_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A4_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A4_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).PopulationReductionA4 = RedListCalculation.POPULATION_REDUCTION_A4_MAX;
            GetRedListCalculation().IsCriteriaA4BFulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaA4BFulfilled = true;
            Assert.AreEqual("A4b", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaA4CFulfilled()
        {
            Double populationReduction;

            // Test value.
            GetRedListCalculation(true).IsCriteriaA4CFulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaA4CFulfilled);
            GetRedListCalculation().IsCriteriaA4CFulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaA4CFulfilled);

            // Test category.
            GetRedListCalculation(true);
            for (populationReduction = Settings.Default.PopulationReductionA4TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A4_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA4 = populationReduction;
                GetRedListCalculation().IsCriteriaA4CFulfilled = false;
                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
                GetRedListCalculation().IsCriteriaA4CFulfilled = true;
                if (populationReduction >= RedListCalculation.CRITERIA_A4_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A4_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A4_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A4_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).PopulationReductionA4 = RedListCalculation.POPULATION_REDUCTION_A4_MAX;
            GetRedListCalculation().IsCriteriaA4CFulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaA4CFulfilled = true;
            Assert.AreEqual("A4c", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaA4DFulfilled()
        {
            Double populationReduction;

            // Test value.
            GetRedListCalculation(true).IsCriteriaA4DFulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaA4DFulfilled);
            GetRedListCalculation().IsCriteriaA4DFulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaA4DFulfilled);

            // Test category.
            GetRedListCalculation(true);
            for (populationReduction = Settings.Default.PopulationReductionA4TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A4_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA4 = populationReduction;
                GetRedListCalculation().IsCriteriaA4DFulfilled = false;
                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
                GetRedListCalculation().IsCriteriaA4DFulfilled = true;
                if (populationReduction >= RedListCalculation.CRITERIA_A4_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A4_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A4_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A4_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).PopulationReductionA4 = RedListCalculation.POPULATION_REDUCTION_A4_MAX;
            GetRedListCalculation().IsCriteriaA4DFulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaA4DFulfilled = true;
            Assert.AreEqual("A4d", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaA4EFulfilled()
        {
            Double populationReduction;

            // Test value.
            GetRedListCalculation(true).IsCriteriaA4EFulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaA4EFulfilled);
            GetRedListCalculation().IsCriteriaA4EFulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaA4EFulfilled);

            // Test category.
            GetRedListCalculation(true);
            for (populationReduction = Settings.Default.PopulationReductionA4TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A4_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA4 = populationReduction;
                GetRedListCalculation().IsCriteriaA4EFulfilled = false;
                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
                GetRedListCalculation().IsCriteriaA4EFulfilled = true;
                if (populationReduction >= RedListCalculation.CRITERIA_A4_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A4_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A4_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A4_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).PopulationReductionA4 = RedListCalculation.POPULATION_REDUCTION_A4_MAX;
            GetRedListCalculation().IsCriteriaA4EFulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaA4EFulfilled = true;
            Assert.AreEqual("A4e", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaBb1Fulfilled()
        {
            Double areaOfOccupancy;

            // Test value.
            GetRedListCalculation(true).IsCriteriaBB1Fulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaBB1Fulfilled);
            GetRedListCalculation().IsCriteriaBB1Fulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaBB1Fulfilled);

            // Test category.
            for (areaOfOccupancy = RedListCalculation.AREA_OF_OCCUPANCY_MIN + 1; areaOfOccupancy <= Settings.Default.AreaOfOccupancyTestMax; areaOfOccupancy += Settings.Default.AreaOfOccupancyTestIncrease)
            {
                GetRedListCalculation(true).ContinuingDecline = GetRedListCalculation().ContinuingDeclineMax;
                GetRedListCalculation().AreaOfOccupancy = areaOfOccupancy;
                GetRedListCalculation().IsCriteriaBB1Fulfilled = false;
                Assert.AreEqual(RedListCategory.LC, GetRedListCalculation().Category);
                GetRedListCalculation().IsCriteriaBB1Fulfilled = true;
                if (areaOfOccupancy < RedListCalculation.CRITERIA_B2_CR_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
                    continue;
                }

                if (areaOfOccupancy < RedListCalculation.CRITERIA_B2_EN_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
                    continue;
                }

                Assert.AreEqual(RedListCategory.LC, GetRedListCalculation().Category);
            }

            // Test criteria.
            GetRedListCalculation(true).ExtentOfOccurrence = RedListCalculation.EXTENT_OF_OCCURRENCE_MIN + 1;
            GetRedListCalculation().ContinuingDecline = GetRedListCalculation().ContinuingDeclineMax;
            GetRedListCalculation().IsCriteriaBB1Fulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaBB1Fulfilled = true;
            Assert.AreEqual("B1b(i)", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaBb2Fulfilled()
        {
            Double areaOfOccupancy;

            // Test value.
            GetRedListCalculation(true).IsCriteriaBB2Fulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaBB2Fulfilled);
            GetRedListCalculation().IsCriteriaBB2Fulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaBB2Fulfilled);

            // Test category.
            for (areaOfOccupancy = RedListCalculation.AREA_OF_OCCUPANCY_MIN + 1; areaOfOccupancy <= Settings.Default.AreaOfOccupancyTestMax; areaOfOccupancy += Settings.Default.AreaOfOccupancyTestIncrease)
            {
                GetRedListCalculation(true).ContinuingDecline = GetRedListCalculation().ContinuingDeclineMax;
                GetRedListCalculation().AreaOfOccupancy = areaOfOccupancy;
                GetRedListCalculation().IsCriteriaBB2Fulfilled = false;
                Assert.AreEqual(RedListCategory.LC, GetRedListCalculation().Category);
                GetRedListCalculation().IsCriteriaBB2Fulfilled = true;
                if (areaOfOccupancy < RedListCalculation.CRITERIA_B2_CR_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
                    continue;
                }

                if (areaOfOccupancy < RedListCalculation.CRITERIA_B2_EN_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
                    continue;
                }

                Assert.AreEqual(RedListCategory.LC, GetRedListCalculation().Category);
            }

            // Test criteria.
            GetRedListCalculation(true).ExtentOfOccurrence = RedListCalculation.EXTENT_OF_OCCURRENCE_MIN + 1;
            GetRedListCalculation().ContinuingDecline = GetRedListCalculation().ContinuingDeclineMax;
            GetRedListCalculation().IsCriteriaBB2Fulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaBB2Fulfilled = true;
            Assert.AreEqual("B1b(ii)", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaBb3Fulfilled()
        {
            Double areaOfOccupancy;

            // Test value.
            GetRedListCalculation(true).IsCriteriaBB3Fulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaBB3Fulfilled);
            GetRedListCalculation().IsCriteriaBB3Fulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaBB3Fulfilled);

            // Test category.
            for (areaOfOccupancy = RedListCalculation.AREA_OF_OCCUPANCY_MIN + 1; areaOfOccupancy <= Settings.Default.AreaOfOccupancyTestMax; areaOfOccupancy += Settings.Default.AreaOfOccupancyTestIncrease)
            {
                GetRedListCalculation(true).ContinuingDecline = GetRedListCalculation().ContinuingDeclineMax;
                GetRedListCalculation().AreaOfOccupancy = areaOfOccupancy;
                GetRedListCalculation().IsCriteriaBB3Fulfilled = false;
                Assert.AreEqual(RedListCategory.LC, GetRedListCalculation().Category);
                GetRedListCalculation().IsCriteriaBB3Fulfilled = true;
                if (areaOfOccupancy < RedListCalculation.CRITERIA_B2_CR_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
                    continue;
                }

                if (areaOfOccupancy < RedListCalculation.CRITERIA_B2_EN_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
                    continue;
                }

                Assert.AreEqual(RedListCategory.LC, GetRedListCalculation().Category);
            }

            // Test criteria.
            GetRedListCalculation(true).ExtentOfOccurrence = RedListCalculation.EXTENT_OF_OCCURRENCE_MIN + 1;
            GetRedListCalculation().ContinuingDecline = GetRedListCalculation().ContinuingDeclineMax;
            GetRedListCalculation().IsCriteriaBB3Fulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaBB3Fulfilled = true;
            Assert.AreEqual("B1b(iii)", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaBb4Fulfilled()
        {
            Double areaOfOccupancy;

            // Test value.
            GetRedListCalculation(true).IsCriteriaBB4Fulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaBB4Fulfilled);
            GetRedListCalculation().IsCriteriaBB4Fulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaBB4Fulfilled);

            // Test category.
            for (areaOfOccupancy = RedListCalculation.AREA_OF_OCCUPANCY_MIN + 1; areaOfOccupancy <= Settings.Default.AreaOfOccupancyTestMax; areaOfOccupancy += Settings.Default.AreaOfOccupancyTestIncrease)
            {
                GetRedListCalculation(true).ContinuingDecline = GetRedListCalculation().ContinuingDeclineMax;
                GetRedListCalculation().AreaOfOccupancy = areaOfOccupancy;
                GetRedListCalculation().IsCriteriaBB4Fulfilled = false;
                Assert.AreEqual(RedListCategory.LC, GetRedListCalculation().Category);
                GetRedListCalculation().IsCriteriaBB4Fulfilled = true;
                if (areaOfOccupancy < RedListCalculation.CRITERIA_B2_CR_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
                    continue;
                }

                if (areaOfOccupancy < RedListCalculation.CRITERIA_B2_EN_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
                    continue;
                }

                Assert.AreEqual(RedListCategory.LC, GetRedListCalculation().Category);
            }

            // Test criteria.
            GetRedListCalculation(true).ExtentOfOccurrence = RedListCalculation.EXTENT_OF_OCCURRENCE_MIN + 1;
            GetRedListCalculation().ContinuingDecline = GetRedListCalculation().ContinuingDeclineMax;
            GetRedListCalculation().IsCriteriaBB4Fulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaBB4Fulfilled = true;
            Assert.AreEqual("B1b(iv)", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaBb5Fulfilled()
        {
            Double areaOfOccupancy;

            // Test value.
            GetRedListCalculation(true).IsCriteriaBB5Fulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaBB5Fulfilled);
            GetRedListCalculation().IsCriteriaBB5Fulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaBB5Fulfilled);

            // Test category.
            for (areaOfOccupancy = RedListCalculation.AREA_OF_OCCUPANCY_MIN + 1; areaOfOccupancy <= Settings.Default.AreaOfOccupancyTestMax; areaOfOccupancy += Settings.Default.AreaOfOccupancyTestIncrease)
            {
                GetRedListCalculation(true).ContinuingDecline = GetRedListCalculation().ContinuingDeclineMax;
                GetRedListCalculation().AreaOfOccupancy = areaOfOccupancy;
                GetRedListCalculation().IsCriteriaBB5Fulfilled = false;
                Assert.AreEqual(RedListCategory.LC, GetRedListCalculation().Category);
                GetRedListCalculation().IsCriteriaBB5Fulfilled = true;
                if (areaOfOccupancy < RedListCalculation.CRITERIA_B2_CR_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
                    continue;
                }

                if (areaOfOccupancy < RedListCalculation.CRITERIA_B2_EN_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
                    continue;
                }

                Assert.AreEqual(RedListCategory.LC, GetRedListCalculation().Category);
            }

            // Test criteria.
            GetRedListCalculation(true).ExtentOfOccurrence = RedListCalculation.EXTENT_OF_OCCURRENCE_MIN + 1;
            GetRedListCalculation().ContinuingDecline = GetRedListCalculation().ContinuingDeclineMax;
            GetRedListCalculation().IsCriteriaBB5Fulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaBB5Fulfilled = true;
            Assert.AreEqual("B1b(v)", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaBc1Fulfilled()
        {
            Double areaOfOccupancy;

            // Test value.
            GetRedListCalculation(true).IsCriteriaBC1Fulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaBC1Fulfilled);
            GetRedListCalculation().IsCriteriaBC1Fulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaBC1Fulfilled);

            // Test category.
            for (areaOfOccupancy = RedListCalculation.AREA_OF_OCCUPANCY_MIN + 1; areaOfOccupancy <= Settings.Default.AreaOfOccupancyTestMax; areaOfOccupancy += Settings.Default.AreaOfOccupancyTestIncrease)
            {
                GetRedListCalculation(true).ExtremeFluctuations = GetRedListCalculation().ExtremeFluctuationsMax;
                GetRedListCalculation().AreaOfOccupancy = areaOfOccupancy;
                GetRedListCalculation().IsCriteriaBC1Fulfilled = false;
                Assert.AreEqual(RedListCategory.LC, GetRedListCalculation().Category);
                GetRedListCalculation().IsCriteriaBC1Fulfilled = true;
                if (areaOfOccupancy < RedListCalculation.CRITERIA_B2_CR_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
                    continue;
                }

                if (areaOfOccupancy < RedListCalculation.CRITERIA_B2_EN_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
                    continue;
                }

                Assert.AreEqual(RedListCategory.LC, GetRedListCalculation().Category);
            }

            // Test criteria.
            GetRedListCalculation(true).ExtentOfOccurrence = RedListCalculation.EXTENT_OF_OCCURRENCE_MIN + 1;
            GetRedListCalculation().ExtremeFluctuations = GetRedListCalculation().ExtremeFluctuationsMax;
            GetRedListCalculation().IsCriteriaBC1Fulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaBC1Fulfilled = true;
            Assert.AreEqual("B1c(i)", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaBc2Fulfilled()
        {
            Double areaOfOccupancy;

            // Test value.
            GetRedListCalculation(true).IsCriteriaBC2Fulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaBC2Fulfilled);
            GetRedListCalculation().IsCriteriaBC2Fulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaBC2Fulfilled);

            // Test category.
            for (areaOfOccupancy = RedListCalculation.AREA_OF_OCCUPANCY_MIN + 1; areaOfOccupancy <= Settings.Default.AreaOfOccupancyTestMax; areaOfOccupancy += Settings.Default.AreaOfOccupancyTestIncrease)
            {
                GetRedListCalculation(true).ExtremeFluctuations = GetRedListCalculation().ExtremeFluctuationsMax;
                GetRedListCalculation().AreaOfOccupancy = areaOfOccupancy;
                GetRedListCalculation().IsCriteriaBC2Fulfilled = false;
                Assert.AreEqual(RedListCategory.LC, GetRedListCalculation().Category);
                GetRedListCalculation().IsCriteriaBC2Fulfilled = true;
                if (areaOfOccupancy < RedListCalculation.CRITERIA_B2_CR_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
                    continue;
                }

                if (areaOfOccupancy < RedListCalculation.CRITERIA_B2_EN_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
                    continue;
                }

                Assert.AreEqual(RedListCategory.LC, GetRedListCalculation().Category);
            }

            // Test criteria.
            GetRedListCalculation(true).ExtentOfOccurrence = RedListCalculation.EXTENT_OF_OCCURRENCE_MIN + 1;
            GetRedListCalculation().ExtremeFluctuations = GetRedListCalculation().ExtremeFluctuationsMax;
            GetRedListCalculation().IsCriteriaBC2Fulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaBC2Fulfilled = true;
            Assert.AreEqual("B1c(ii)", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaBc3Fulfilled()
        {
            Double areaOfOccupancy;

            // Test value.
            GetRedListCalculation(true).IsCriteriaBC3Fulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaBC3Fulfilled);
            GetRedListCalculation().IsCriteriaBC3Fulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaBC3Fulfilled);

            // Test category.
            for (areaOfOccupancy = RedListCalculation.AREA_OF_OCCUPANCY_MIN + 1; areaOfOccupancy <= Settings.Default.AreaOfOccupancyTestMax; areaOfOccupancy += Settings.Default.AreaOfOccupancyTestIncrease)
            {
                GetRedListCalculation(true).ExtremeFluctuations = GetRedListCalculation().ExtremeFluctuationsMax;
                GetRedListCalculation().AreaOfOccupancy = areaOfOccupancy;
                GetRedListCalculation().IsCriteriaBC3Fulfilled = false;
                Assert.AreEqual(RedListCategory.LC, GetRedListCalculation().Category);
                GetRedListCalculation().IsCriteriaBC3Fulfilled = true;
                if (areaOfOccupancy < RedListCalculation.CRITERIA_B2_CR_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
                    continue;
                }

                if (areaOfOccupancy < RedListCalculation.CRITERIA_B2_EN_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
                    continue;
                }

                Assert.AreEqual(RedListCategory.LC, GetRedListCalculation().Category);
            }

            // Test criteria.
            GetRedListCalculation(true).ExtentOfOccurrence = RedListCalculation.EXTENT_OF_OCCURRENCE_MIN + 1;
            GetRedListCalculation().ExtremeFluctuations = GetRedListCalculation().ExtremeFluctuationsMax;
            GetRedListCalculation().IsCriteriaBC3Fulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaBC3Fulfilled = true;
            Assert.AreEqual("B1c(iii)", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaBc4Fulfilled()
        {
            Double areaOfOccupancy;

            // Test value.
            GetRedListCalculation(true).IsCriteriaBC4Fulfilled = false;
            Assert.IsFalse(GetRedListCalculation().IsCriteriaBC4Fulfilled);
            GetRedListCalculation().IsCriteriaBC4Fulfilled = true;
            Assert.IsTrue(GetRedListCalculation().IsCriteriaBC4Fulfilled);

            // Test category.
            for (areaOfOccupancy = RedListCalculation.AREA_OF_OCCUPANCY_MIN + 1; areaOfOccupancy <= Settings.Default.AreaOfOccupancyTestMax; areaOfOccupancy += Settings.Default.AreaOfOccupancyTestIncrease)
            {
                GetRedListCalculation(true).ExtremeFluctuations = GetRedListCalculation().ExtremeFluctuationsMax;
                GetRedListCalculation().AreaOfOccupancy = areaOfOccupancy;
                GetRedListCalculation().IsCriteriaBC4Fulfilled = false;
                Assert.AreEqual(RedListCategory.LC, GetRedListCalculation().Category);
                GetRedListCalculation().IsCriteriaBC4Fulfilled = true;
                if (areaOfOccupancy < RedListCalculation.CRITERIA_B2_CR_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
                    continue;
                }

                if (areaOfOccupancy < RedListCalculation.CRITERIA_B2_EN_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
                    continue;
                }

                Assert.AreEqual(RedListCategory.LC, GetRedListCalculation().Category);
            }

            // Test criteria.
            GetRedListCalculation(true).ExtentOfOccurrence = RedListCalculation.EXTENT_OF_OCCURRENCE_MIN + 1;
            GetRedListCalculation().ExtremeFluctuations = GetRedListCalculation().ExtremeFluctuationsMax;
            GetRedListCalculation().IsCriteriaBC4Fulfilled = false;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaBC4Fulfilled = true;
            Assert.AreEqual("B1c(iv)", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void IsCriteriaCalculated()
        {
            // Test value.
            GetRedListCalculation(true).IsCriteriaCalculated = true;
            Assert.AreEqual(true, GetRedListCalculation().IsCriteriaCalculated);
            GetRedListCalculation().IsCriteriaCalculated = false;
            Assert.AreEqual(false, GetRedListCalculation().IsCriteriaCalculated);

            // Test criteria.
            GetRedListCalculation(true).IsCriteriaCalculated = false;
            GetRedListCalculation().ExtentOfOccurrence = RedListCalculation.EXTENT_OF_OCCURRENCE_MIN + 1;
            GetRedListCalculation().ExtremeFluctuations = GetRedListCalculation().ExtremeFluctuationsMax;
            GetRedListCalculation().IsCriteriaBC4Fulfilled = true;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().IsCriteriaCalculated = true;
            Assert.AreEqual("B1c(iv)", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void MaxProportionLocalPopulation()
        {
            Double maxProportionLocalPopulation;

            // Test value.
            GetRedListCalculation(true);
            for (maxProportionLocalPopulation = RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MIN; maxProportionLocalPopulation <= RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MAX; maxProportionLocalPopulation += 1)
            {
                GetRedListCalculation().MaxProportionLocalPopulation = maxProportionLocalPopulation;
                Assert.AreEqual(maxProportionLocalPopulation, GetRedListCalculation().MaxProportionLocalPopulation);
            }

            // Test category.
            GetRedListCalculation(true).ContinuingDecline = RedListCalculation.CRITERIA_C2_LIMIT;
            for (maxProportionLocalPopulation = RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MIN; maxProportionLocalPopulation <= RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MAX; maxProportionLocalPopulation += 1)
            {
                GetRedListCalculation().MaxProportionLocalPopulation = maxProportionLocalPopulation;
                if (maxProportionLocalPopulation >= RedListCalculation.CRITERIA_C2A2_CR_LIMIT)
                {
                    GetRedListCalculation().PopulationSize = RedListCalculation.CRITERIA_C_CR_LIMIT - 1;
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (maxProportionLocalPopulation >= RedListCalculation.CRITERIA_C2A2_EN_LIMIT)
                {
                    GetRedListCalculation().PopulationSize = RedListCalculation.CRITERIA_C_EN_LIMIT - 1;
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (maxProportionLocalPopulation >= RedListCalculation.CRITERIA_C2A2_VU_LIMIT)
                {
                    GetRedListCalculation().PopulationSize = RedListCalculation.CRITERIA_C_VU_LIMIT - 1;
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (maxProportionLocalPopulation >= RedListCalculation.CRITERIA_C2A2_NT_LIMIT)
                {
                    GetRedListCalculation().PopulationSize = RedListCalculation.CRITERIA_C_VU_LIMIT - 1;
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).PopulationSize = RedListCalculation.CRITERIA_C_VU_LIMIT;
            GetRedListCalculation().ContinuingDecline = RedListCalculation.CRITERIA_C2_LIMIT;
            GetRedListCalculation().MaxProportionLocalPopulation = RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MIN;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().MaxProportionLocalPopulation = RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MAX;
            Assert.AreEqual("C2a(ii)", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void MaxProportionLocalPopulationToLargeValueError()
        {
            GetRedListCalculation().MaxProportionLocalPopulation = 101;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void MaxProportionLocalPopulationToSmallValueError()
        {
            GetRedListCalculation().MaxProportionLocalPopulation = -1;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void MaxProportionLocalPopulationMax()
        {
            Assert.IsTrue(RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MIN < RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_C2A2_CR_LIMIT <= RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_C2A2_EN_LIMIT <= RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_C2A2_VU_LIMIT <= RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_C2A2_NT_LIMIT <= RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MAX);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void MaxProportionLocalPopulationMin()
        {
            Assert.IsTrue(RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MIN < RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_C2A2_CR_LIMIT >= RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_C2A2_EN_LIMIT >= RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_C2A2_VU_LIMIT >= RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_C2A2_NT_LIMIT >= RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MIN);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void MaxSizeLocalPopulation()
        {
            Int64 maxSizeLocalPopulation;

            // Test value.
            GetRedListCalculation(true);
            for (maxSizeLocalPopulation = RedListCalculation.MAX_SIZE_LOCAL_POPULATION_MIN; maxSizeLocalPopulation <= Settings.Default.MaxSizeLocalPopulationTestMax; maxSizeLocalPopulation += Settings.Default.MaxSizeLocalPopulationTestIncrease)
            {
                GetRedListCalculation().MaxSizeLocalPopulation = maxSizeLocalPopulation;
                Assert.AreEqual(maxSizeLocalPopulation, GetRedListCalculation().MaxSizeLocalPopulation);
            }

            // Test category.
            GetRedListCalculation(true).ContinuingDecline = RedListCalculation.CRITERIA_C2_LIMIT;
            for (maxSizeLocalPopulation = RedListCalculation.MAX_SIZE_LOCAL_POPULATION_MIN; maxSizeLocalPopulation <= Settings.Default.MaxSizeLocalPopulationTestMax; maxSizeLocalPopulation += Settings.Default.MaxSizeLocalPopulationTestIncrease)
            {
                GetRedListCalculation().MaxSizeLocalPopulation = maxSizeLocalPopulation;
                if (maxSizeLocalPopulation <= RedListCalculation.CRITERIA_C2A1_CR_LIMIT)
                {
                    GetRedListCalculation().PopulationSize = RedListCalculation.CRITERIA_C_CR_LIMIT - 1;
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (maxSizeLocalPopulation <= RedListCalculation.CRITERIA_C2A1_EN_LIMIT)
                {
                    GetRedListCalculation().PopulationSize = RedListCalculation.CRITERIA_C_EN_LIMIT - 1;
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (maxSizeLocalPopulation <= RedListCalculation.CRITERIA_C2A1_VU_LIMIT)
                {
                    GetRedListCalculation().PopulationSize = RedListCalculation.CRITERIA_C_VU_LIMIT - 1;
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (maxSizeLocalPopulation <= RedListCalculation.CRITERIA_C2A1_NT_LIMIT)
                {
                    GetRedListCalculation().PopulationSize = RedListCalculation.CRITERIA_C_VU_LIMIT - 1;
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).PopulationSize = RedListCalculation.CRITERIA_C_VU_LIMIT;
            GetRedListCalculation().ContinuingDecline = RedListCalculation.CRITERIA_C2_LIMIT;
            GetRedListCalculation().MaxSizeLocalPopulation = RedListCalculation.MAX_SIZE_LOCAL_POPULATION_MAX;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().MaxSizeLocalPopulation = RedListCalculation.MAX_SIZE_LOCAL_POPULATION_MIN;
            Assert.AreEqual("C2a(i)", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void MaxSizeLocalPopulationToSmallValueError()
        {
            GetRedListCalculation().MaxSizeLocalPopulation = -1;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void MaxSizeLocalPopulationMax()
        {
            Assert.IsTrue(RedListCalculation.MAX_SIZE_LOCAL_POPULATION_MIN < RedListCalculation.MAX_SIZE_LOCAL_POPULATION_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_C2A1_CR_LIMIT <= RedListCalculation.MAX_SIZE_LOCAL_POPULATION_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_C2A1_EN_LIMIT <= RedListCalculation.MAX_SIZE_LOCAL_POPULATION_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_C2A1_VU_LIMIT <= RedListCalculation.MAX_SIZE_LOCAL_POPULATION_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_C2A1_NT_LIMIT <= RedListCalculation.MAX_SIZE_LOCAL_POPULATION_MAX);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void MaxSizeLocalPopulationMin()
        {
            Assert.IsTrue(RedListCalculation.MAX_SIZE_LOCAL_POPULATION_MIN < RedListCalculation.MAX_SIZE_LOCAL_POPULATION_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_C2A1_CR_LIMIT >= RedListCalculation.MAX_SIZE_LOCAL_POPULATION_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_C2A1_EN_LIMIT >= RedListCalculation.MAX_SIZE_LOCAL_POPULATION_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_C2A1_VU_LIMIT >= RedListCalculation.MAX_SIZE_LOCAL_POPULATION_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_C2A1_NT_LIMIT >= RedListCalculation.MAX_SIZE_LOCAL_POPULATION_MIN);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void NumberOfLocations()
        {
            Int64 numberOfLocations;

            // Test value.
            GetRedListCalculation(true);
            for (numberOfLocations = RedListCalculation.NUMBER_OF_LOCATIONS_MIN; numberOfLocations <= Settings.Default.NumberOfLoactionsTestMax; numberOfLocations++)
            {
                GetRedListCalculation().NumberOfLocations = numberOfLocations;
                Assert.AreEqual(numberOfLocations, GetRedListCalculation().NumberOfLocations);
            }

            // Test category.
            GetRedListCalculation(true);
            GetRedListCalculation().ContinuingDecline = GetRedListCalculation().ContinuingDeclineMax;
            GetRedListCalculation().IsCriteriaBB1Fulfilled = true;
            for (numberOfLocations = RedListCalculation.NUMBER_OF_LOCATIONS_MIN; numberOfLocations <= Settings.Default.NumberOfLoactionsTestMax; numberOfLocations++)
            {
                GetRedListCalculation().NumberOfLocations = numberOfLocations;
                if (numberOfLocations <= RedListCalculation.CRITERIA_NUMBER_OF_LOCATIONS_RE_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.RE);
                    continue;
                }

                if (numberOfLocations <= RedListCalculation.CRITERIA_BA_CR_LIMIT)
                {
                    GetRedListCalculation().ExtentOfOccurrence = RedListCalculation.CRITERIA_B1_CR_LIMIT - 1;
                    Assert.AreEqual(RedListCategory.CR, GetRedListCalculation().Category);
                    continue;
                }

                if (numberOfLocations < RedListCalculation.CRITERIA_BA_EN_LIMIT)
                {
                    GetRedListCalculation().ExtentOfOccurrence = RedListCalculation.CRITERIA_B1_EN_LIMIT - 1;
                    Assert.AreEqual(RedListCategory.EN, GetRedListCalculation().Category);
                    continue;
                }

                if (numberOfLocations < RedListCalculation.CRITERIA_BA_VU_LIMIT)
                {
                    GetRedListCalculation().ExtentOfOccurrence = RedListCalculation.CRITERIA_B1_VU_LIMIT - 1;
                    Assert.AreEqual(RedListCategory.VU, GetRedListCalculation().Category);
                    continue;
                }

                if (numberOfLocations < RedListCalculation.CRITERIA_BA_NT_LIMIT)
                {
                    GetRedListCalculation().ExtentOfOccurrence = RedListCalculation.CRITERIA_B1_NT_LIMIT - 1;
                    Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
                }
            }

            // Test problem with Category when CriteriaBCount is 2
            // and ExtentOfOccurrence < RedListCalculation.CRITERIA_BA_EN_LIMIT.
            GetRedListCalculation(true).ExtentOfOccurrence = RedListCalculation.CRITERIA_B1_EN_LIMIT - 1;
            GetRedListCalculation().NumberOfLocations = RedListCalculation.CRITERIA_BA_EN_LIMIT - 1;
            Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
            GetRedListCalculation().NumberOfLocations = RedListCalculation.CRITERIA_BA_EN_LIMIT;
            Assert.AreEqual(RedListCategory.LC, GetRedListCalculation().Category);
            GetRedListCalculation(true).AreaOfOccupancy = RedListCalculation.CRITERIA_B2_EN_LIMIT - 1;
            GetRedListCalculation().NumberOfLocations = RedListCalculation.CRITERIA_BA_EN_LIMIT - 1;
            Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
            GetRedListCalculation().NumberOfLocations = RedListCalculation.CRITERIA_BA_EN_LIMIT;
            Assert.AreEqual(RedListCategory.LC, GetRedListCalculation().Category);

            // Test criteria.
            GetRedListCalculation(true).ExtentOfOccurrence = RedListCalculation.CRITERIA_B1_CR_LIMIT;
            GetRedListCalculation().IsCriteriaBB1Fulfilled = true;
            GetRedListCalculation().NumberOfLocations = RedListCalculation.NUMBER_OF_LOCATIONS_MAX;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().NumberOfLocations = RedListCalculation.NUMBER_OF_LOCATIONS_MIN + 1;
            Assert.AreEqual("B1a", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void NumberOfLocationsToSmallValueError()
        {
            GetRedListCalculation().NumberOfLocations = -1;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void NumberOfLocationsMax()
        {
            Assert.IsTrue(RedListCalculation.NUMBER_OF_LOCATIONS_MIN < RedListCalculation.NUMBER_OF_LOCATIONS_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_BA_CR_LIMIT <= RedListCalculation.NUMBER_OF_LOCATIONS_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_BA_EN_LIMIT <= RedListCalculation.NUMBER_OF_LOCATIONS_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_BA_VU_LIMIT <= RedListCalculation.NUMBER_OF_LOCATIONS_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_BA_NT_LIMIT <= RedListCalculation.NUMBER_OF_LOCATIONS_MAX);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void NumberOfLocationsMin()
        {
            Assert.IsTrue(RedListCalculation.NUMBER_OF_LOCATIONS_MIN < RedListCalculation.NUMBER_OF_LOCATIONS_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_BA_CR_LIMIT >= RedListCalculation.NUMBER_OF_LOCATIONS_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_BA_EN_LIMIT >= RedListCalculation.NUMBER_OF_LOCATIONS_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_BA_VU_LIMIT >= RedListCalculation.NUMBER_OF_LOCATIONS_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_BA_NT_LIMIT >= RedListCalculation.NUMBER_OF_LOCATIONS_MIN);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void PopulationReductionA1()
        {
            Double populationReduction;

            // Test value.
            GetRedListCalculation(true);
            for (populationReduction = Settings.Default.PopulationReductionA1TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A1_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA1 = populationReduction;
                Assert.AreEqual(populationReduction, GetRedListCalculation().PopulationReductionA1);
            }

            // Test category.
            GetRedListCalculation(true).IsCriteriaA1AFulfilled = true;
            for (populationReduction = Settings.Default.PopulationReductionA1TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A1_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA1 = populationReduction;
                if (populationReduction >= RedListCalculation.CRITERIA_A1_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A1_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A1_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A1_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).IsCriteriaA1AFulfilled = true;
            GetRedListCalculation().PopulationReductionA1 = RedListCalculation.POPULATION_REDUCTION_A1_MIN;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().PopulationReductionA1 = RedListCalculation.POPULATION_REDUCTION_A1_MAX;
            Assert.AreEqual("A1a", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void PopulationReductionA1ToLargeValueError()
        {
            GetRedListCalculation().PopulationReductionA1 = 101;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void PopulationReductionA1Max()
        {
            Assert.IsTrue(RedListCalculation.POPULATION_REDUCTION_A1_MIN < RedListCalculation.POPULATION_REDUCTION_A1_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_A1_CR_LIMIT <= RedListCalculation.POPULATION_REDUCTION_A1_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_A1_EN_LIMIT <= RedListCalculation.POPULATION_REDUCTION_A1_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_A1_VU_LIMIT <= RedListCalculation.POPULATION_REDUCTION_A1_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_A1_NT_LIMIT <= RedListCalculation.POPULATION_REDUCTION_A1_MAX);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void PopulationReductionA1Min()
        {
            Assert.IsTrue(RedListCalculation.POPULATION_REDUCTION_A1_MIN < RedListCalculation.POPULATION_REDUCTION_A1_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_A1_CR_LIMIT >= RedListCalculation.POPULATION_REDUCTION_A1_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_A1_EN_LIMIT >= RedListCalculation.POPULATION_REDUCTION_A1_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_A1_VU_LIMIT >= RedListCalculation.POPULATION_REDUCTION_A1_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_A1_NT_LIMIT >= RedListCalculation.POPULATION_REDUCTION_A1_MIN);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void PopulationReductionA2()
        {
            Double populationReduction;

            // Test value.
            GetRedListCalculation(true);
            for (populationReduction = Settings.Default.PopulationReductionA2TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A2_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA2 = populationReduction;
                Assert.AreEqual(populationReduction, GetRedListCalculation().PopulationReductionA2);
            }

            // Test category.
            GetRedListCalculation(true).IsCriteriaA2AFulfilled = true;
            for (populationReduction = Settings.Default.PopulationReductionA2TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A2_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA2 = populationReduction;
                if (populationReduction >= RedListCalculation.CRITERIA_A2_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A2_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A2_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A2_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).IsCriteriaA2AFulfilled = true;
            GetRedListCalculation().PopulationReductionA2 = RedListCalculation.POPULATION_REDUCTION_A2_MIN;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().PopulationReductionA2 = RedListCalculation.POPULATION_REDUCTION_A2_MAX;
            Assert.AreEqual("A2a", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void PopulationReductionA2ToLargeValueError()
        {
            GetRedListCalculation().PopulationReductionA2 = 101;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void PopulationReductionA2Max()
        {
            Assert.IsTrue(RedListCalculation.POPULATION_REDUCTION_A2_MIN < RedListCalculation.POPULATION_REDUCTION_A2_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_A2_CR_LIMIT <= RedListCalculation.POPULATION_REDUCTION_A2_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_A2_EN_LIMIT <= RedListCalculation.POPULATION_REDUCTION_A2_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_A2_VU_LIMIT <= RedListCalculation.POPULATION_REDUCTION_A2_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_A2_NT_LIMIT <= RedListCalculation.POPULATION_REDUCTION_A2_MAX);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void PopulationReductionA2Min()
        {
            Assert.IsTrue(RedListCalculation.POPULATION_REDUCTION_A2_MIN < RedListCalculation.POPULATION_REDUCTION_A2_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_A2_CR_LIMIT >= RedListCalculation.POPULATION_REDUCTION_A2_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_A2_EN_LIMIT >= RedListCalculation.POPULATION_REDUCTION_A2_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_A2_VU_LIMIT >= RedListCalculation.POPULATION_REDUCTION_A2_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_A2_NT_LIMIT >= RedListCalculation.POPULATION_REDUCTION_A2_MIN);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void PopulationReductionA3()
        {
            Double populationReduction;

            // Test value.
            GetRedListCalculation(true);
            for (populationReduction = Settings.Default.PopulationReductionA3TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A3_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA3 = populationReduction;
                Assert.AreEqual(populationReduction, GetRedListCalculation().PopulationReductionA3);
            }

            // Test category.
            GetRedListCalculation(true).IsCriteriaA3BFulfilled = true;
            for (populationReduction = Settings.Default.PopulationReductionA3TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A3_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA3 = populationReduction;
                if (populationReduction >= RedListCalculation.CRITERIA_A3_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A3_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A3_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A3_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).IsCriteriaA3BFulfilled = true;
            GetRedListCalculation().PopulationReductionA3 = RedListCalculation.POPULATION_REDUCTION_A3_MIN;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().PopulationReductionA3 = RedListCalculation.POPULATION_REDUCTION_A3_MAX;
            Assert.AreEqual("A3b", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void PopulationReductionA3ToLargeValueError()
        {
            GetRedListCalculation().PopulationReductionA3 = 101;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void PopulationReductionA3Max()
        {
            Assert.IsTrue(RedListCalculation.POPULATION_REDUCTION_A3_MIN < RedListCalculation.POPULATION_REDUCTION_A3_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_A3_CR_LIMIT <= RedListCalculation.POPULATION_REDUCTION_A3_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_A3_EN_LIMIT <= RedListCalculation.POPULATION_REDUCTION_A3_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_A3_VU_LIMIT <= RedListCalculation.POPULATION_REDUCTION_A3_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_A3_NT_LIMIT <= RedListCalculation.POPULATION_REDUCTION_A3_MAX);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void PopulationReductionA3Min()
        {
            Assert.IsTrue(RedListCalculation.POPULATION_REDUCTION_A3_MIN < RedListCalculation.POPULATION_REDUCTION_A3_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_A3_CR_LIMIT >= RedListCalculation.POPULATION_REDUCTION_A3_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_A3_EN_LIMIT >= RedListCalculation.POPULATION_REDUCTION_A3_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_A3_VU_LIMIT >= RedListCalculation.POPULATION_REDUCTION_A3_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_A3_NT_LIMIT >= RedListCalculation.POPULATION_REDUCTION_A3_MIN);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void PopulationReductionA4()
        {
            Double populationReduction;

            // Test value.
            GetRedListCalculation(true);
            for (populationReduction = Settings.Default.PopulationReductionA4TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A4_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA4 = populationReduction;
                Assert.AreEqual(populationReduction, GetRedListCalculation().PopulationReductionA4);
            }

            // Test category.
            GetRedListCalculation(true).IsCriteriaA4AFulfilled = true;
            for (populationReduction = Settings.Default.PopulationReductionA4TestMin; populationReduction <= RedListCalculation.POPULATION_REDUCTION_A4_MAX; populationReduction++)
            {
                GetRedListCalculation().PopulationReductionA4 = populationReduction;
                if (populationReduction >= RedListCalculation.CRITERIA_A4_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A4_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A4_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationReduction >= RedListCalculation.CRITERIA_A4_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria.
            GetRedListCalculation(true).IsCriteriaA4AFulfilled = true;
            GetRedListCalculation().PopulationReductionA4 = RedListCalculation.POPULATION_REDUCTION_A4_MIN;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().PopulationReductionA4 = RedListCalculation.POPULATION_REDUCTION_A4_MAX;
            Assert.AreEqual("A4a", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void PopulationReductionA4ToLargeValueError()
        {
            GetRedListCalculation().PopulationReductionA4 = 101;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void PopulationReductionA4Max()
        {
            Assert.IsTrue(RedListCalculation.POPULATION_REDUCTION_A4_MIN < RedListCalculation.POPULATION_REDUCTION_A4_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_A4_CR_LIMIT <= RedListCalculation.POPULATION_REDUCTION_A4_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_A4_EN_LIMIT <= RedListCalculation.POPULATION_REDUCTION_A4_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_A4_VU_LIMIT <= RedListCalculation.POPULATION_REDUCTION_A4_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_A4_NT_LIMIT <= RedListCalculation.POPULATION_REDUCTION_A4_MAX);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void PopulationReductionA4Min()
        {
            Assert.IsTrue(RedListCalculation.POPULATION_REDUCTION_A4_MIN < RedListCalculation.POPULATION_REDUCTION_A4_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_A4_CR_LIMIT >= RedListCalculation.POPULATION_REDUCTION_A4_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_A4_EN_LIMIT >= RedListCalculation.POPULATION_REDUCTION_A4_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_A4_VU_LIMIT >= RedListCalculation.POPULATION_REDUCTION_A4_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_A4_NT_LIMIT >= RedListCalculation.POPULATION_REDUCTION_A4_MIN);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void PopulationSize()
        {
            Int64 populationSize;

            // Test value.
            GetRedListCalculation(true);
            for (populationSize = RedListCalculation.POPULATION_SIZE_MIN; populationSize <= Settings.Default.PopulationSizeTestMax; populationSize += Settings.Default.PopulationSizeTestIncrease)
            {
                GetRedListCalculation().PopulationSize = populationSize;
                Assert.AreEqual(populationSize, GetRedListCalculation().PopulationSize);
            }

            // Test category.
            for (populationSize = RedListCalculation.POPULATION_SIZE_MIN; populationSize <= Settings.Default.PopulationSizeTestMax; populationSize += Settings.Default.PopulationSizeTestIncrease)
            {
                GetRedListCalculation(true).PopulationSize = populationSize;

                // Test criteria D.
                if (populationSize <= RedListCalculation.CRITERIA_POPULATION_SIZE_RE_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.RE);
                    continue;
                }
                else if (populationSize < RedListCalculation.CRITERIA_D_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                }
                else if (populationSize < RedListCalculation.CRITERIA_D_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                }
                else if (populationSize < RedListCalculation.CRITERIA_D1_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                }
                else if (populationSize < RedListCalculation.CRITERIA_D1_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                }
                else
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
                }

                // Test criteria C.
                GetRedListCalculation().ContinuingDecline = RedListCalculation.CRITERIA_C2_LIMIT;
                GetRedListCalculation().ExtremeFluctuations = GetRedListCalculation().ExtremeFluctuationsMax;
                if (populationSize < RedListCalculation.CRITERIA_C_CR_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.CR);
                    continue;
                }

                if (populationSize < RedListCalculation.CRITERIA_C_EN_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.EN);
                    continue;
                }

                if (populationSize < RedListCalculation.CRITERIA_C_VU_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.VU);
                    continue;
                }

                if (populationSize < RedListCalculation.CRITERIA_C_NT_LIMIT)
                {
                    Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.NT);
                    continue;
                }

                Assert.AreEqual(GetRedListCalculation().Category, RedListCategory.LC);
            }

            // Test criteria D.
            GetRedListCalculation(true);
            GetRedListCalculation().PopulationSize = RedListCalculation.POPULATION_SIZE_MAX;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().PopulationSize = RedListCalculation.CRITERIA_D_CR_LIMIT;
            Assert.AreEqual("D", GetRedListCalculation().Criteria);

            // Test criteria D.
            GetRedListCalculation(true);
            GetRedListCalculation().ContinuingDecline = RedListCalculation.CRITERIA_C2_LIMIT;
            GetRedListCalculation().ExtremeFluctuations = GetRedListCalculation().ExtremeFluctuationsMax;
            GetRedListCalculation().PopulationSize = RedListCalculation.POPULATION_SIZE_MAX;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().PopulationSize = RedListCalculation.CRITERIA_C_VU_LIMIT;
            Assert.AreEqual("C2b", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void PopulationSizeToSmallValueError()
        {
            GetRedListCalculation().PopulationSize = -1;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void PopulationSizeMax()
        {
            Assert.IsTrue(RedListCalculation.POPULATION_SIZE_MIN < RedListCalculation.POPULATION_SIZE_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_C_CR_LIMIT <= RedListCalculation.POPULATION_SIZE_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_C_EN_LIMIT <= RedListCalculation.POPULATION_SIZE_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_C_VU_LIMIT <= RedListCalculation.POPULATION_SIZE_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_C_NT_LIMIT <= RedListCalculation.POPULATION_SIZE_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_D_CR_LIMIT <= RedListCalculation.POPULATION_SIZE_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_D_EN_LIMIT <= RedListCalculation.POPULATION_SIZE_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_D1_VU_LIMIT <= RedListCalculation.POPULATION_SIZE_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_D1_NT_LIMIT <= RedListCalculation.POPULATION_SIZE_MAX);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void PopulationSizeMin()
        {
            Assert.IsTrue(RedListCalculation.POPULATION_SIZE_MIN < RedListCalculation.POPULATION_SIZE_MAX);
            Assert.IsTrue(RedListCalculation.CRITERIA_C_CR_LIMIT >= RedListCalculation.POPULATION_SIZE_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_C_EN_LIMIT >= RedListCalculation.POPULATION_SIZE_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_C_VU_LIMIT >= RedListCalculation.POPULATION_SIZE_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_C_NT_LIMIT >= RedListCalculation.POPULATION_SIZE_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_D_CR_LIMIT >= RedListCalculation.POPULATION_SIZE_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_D_EN_LIMIT >= RedListCalculation.POPULATION_SIZE_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_D1_VU_LIMIT >= RedListCalculation.POPULATION_SIZE_MIN);
            Assert.IsTrue(RedListCalculation.CRITERIA_D1_NT_LIMIT >= RedListCalculation.POPULATION_SIZE_MIN);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void SeverlyFragmented()
        {
            Int32 severlyFragmented;

            // Test value.
            GetRedListCalculation(true);
            for (severlyFragmented = GetRedListCalculation().SeverlyFragmentedMin; severlyFragmented <= GetRedListCalculation().SeverlyFragmentedMax; severlyFragmented++)
            {
                GetRedListCalculation().SeverlyFragmented = severlyFragmented;
                Assert.AreEqual(severlyFragmented, GetRedListCalculation().SeverlyFragmented);
            }

            // Test category.
            GetRedListCalculation(true).AreaOfOccupancy = RedListCalculation.AREA_OF_OCCUPANCY_MIN + 1;
            GetRedListCalculation().IsCriteriaBB1Fulfilled = true;
            GetRedListCalculation().ContinuingDecline = GetRedListCalculation().ContinuingDeclineMax;
            for (severlyFragmented = GetRedListCalculation().SeverlyFragmentedMin; severlyFragmented <= GetRedListCalculation().SeverlyFragmentedMax; severlyFragmented++)
            {
                GetRedListCalculation().SeverlyFragmented = severlyFragmented;
                if (severlyFragmented >= RedListCalculation.CRITERIA_BA_2_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.CR, GetRedListCalculation().Category);
                    continue;
                }

                if (severlyFragmented >= RedListCalculation.CRITERIA_BA_1_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.EN, GetRedListCalculation().Category);
                    continue;
                }

                Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
            }

            // Test criteria.
            GetRedListCalculation(true).AreaOfOccupancy = RedListCalculation.AREA_OF_OCCUPANCY_MIN + 1;
            GetRedListCalculation().SeverlyFragmented = GetRedListCalculation().SeverlyFragmentedMin;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().SeverlyFragmented = GetRedListCalculation().SeverlyFragmentedMax;
            Assert.AreEqual("B2a", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void SeverlyFragmentedToLargeValueError()
        {
            GetRedListCalculation().SeverlyFragmented = 1000;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void SeverlyFragmentedToSmallValueError()
        {
            GetRedListCalculation().SeverlyFragmented = -1000;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void SeverlyFragmentedMax()
        {
            Assert.IsTrue(GetRedListCalculation().SeverlyFragmentedMin < GetRedListCalculation().SeverlyFragmentedMax);
            Assert.IsTrue(RedListCalculation.CRITERIA_BA_2_LIMIT <= GetRedListCalculation().SeverlyFragmentedMax);
            Assert.IsTrue(RedListCalculation.CRITERIA_BA_1_LIMIT <= GetRedListCalculation().SeverlyFragmentedMax);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void SeverlyFragmentedMin()
        {
            Assert.IsTrue(GetRedListCalculation().SeverlyFragmentedMin < GetRedListCalculation().SeverlyFragmentedMax);
            Assert.IsTrue(RedListCalculation.CRITERIA_BA_2_LIMIT >= GetRedListCalculation().SeverlyFragmentedMin);
            Assert.IsTrue(RedListCalculation.CRITERIA_BA_1_LIMIT >= GetRedListCalculation().SeverlyFragmentedMin);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void VeryRestrictedArea()
        {
            Int32 veryRestrictedArea;

            // Test value.
            GetRedListCalculation(true);
            for (veryRestrictedArea = GetRedListCalculation().VeryRestrictedAreaMin; veryRestrictedArea <= GetRedListCalculation().VeryRestrictedAreaMax; veryRestrictedArea++)
            {
                GetRedListCalculation().VeryRestrictedArea = veryRestrictedArea;
                Assert.AreEqual(veryRestrictedArea, GetRedListCalculation().VeryRestrictedArea);
            }

            // Test category.
            GetRedListCalculation(true);
            for (veryRestrictedArea = GetRedListCalculation().VeryRestrictedAreaMin; veryRestrictedArea <= GetRedListCalculation().VeryRestrictedAreaMax; veryRestrictedArea++)
            {
                GetRedListCalculation().VeryRestrictedArea = veryRestrictedArea;
                if (veryRestrictedArea <= RedListCalculation.CRITERIA_D2_VU_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.VU, GetRedListCalculation().Category);
                    continue;
                }

                if (veryRestrictedArea <= RedListCalculation.CRITERIA_D2_NT_LIMIT)
                {
                    Assert.AreEqual(RedListCategory.NT, GetRedListCalculation().Category);
                    continue;
                }

                Assert.AreEqual(RedListCategory.LC, GetRedListCalculation().Category);
            }

            // Test criteria.
            GetRedListCalculation(true).VeryRestrictedArea = GetRedListCalculation().VeryRestrictedAreaMax;
            Assert.IsTrue(GetRedListCalculation().Criteria.IsEmpty());
            GetRedListCalculation().VeryRestrictedArea = GetRedListCalculation().VeryRestrictedAreaMin;
            Assert.AreEqual("D2", GetRedListCalculation().Criteria);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void VeryRestrictedAreaToLargeValueError()
        {
            GetRedListCalculation().VeryRestrictedArea = 1000;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void VeryRestrictedAreaToSmallValueError()
        {
            GetRedListCalculation().VeryRestrictedArea = -1000;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void VeryRestrictedAreaMax()
        {
            Assert.IsTrue(GetRedListCalculation().VeryRestrictedAreaMin < GetRedListCalculation().VeryRestrictedAreaMax);
            Assert.IsTrue(RedListCalculation.CRITERIA_D2_VU_LIMIT <= GetRedListCalculation().VeryRestrictedAreaMax);
            Assert.IsTrue(RedListCalculation.CRITERIA_D2_NT_LIMIT <= GetRedListCalculation().VeryRestrictedAreaMax);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void VeryRestrictedAreaMin()
        {
            Assert.IsTrue(GetRedListCalculation().VeryRestrictedAreaMin < GetRedListCalculation().VeryRestrictedAreaMax);
            Assert.IsTrue(RedListCalculation.CRITERIA_D2_VU_LIMIT >= GetRedListCalculation().VeryRestrictedAreaMin);
            Assert.IsTrue(RedListCalculation.CRITERIA_D2_NT_LIMIT >= GetRedListCalculation().VeryRestrictedAreaMin);
        }
    }
}

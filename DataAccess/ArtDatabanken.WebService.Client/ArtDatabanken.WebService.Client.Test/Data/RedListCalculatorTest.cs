using System;
using ArtDatabanken.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class RedListCalculatorTest : TestBase
    {
        private const Int64 POPULATION_SIZE_TEST_MAX = 2500;
        private const Int64 POPULATION_SIZE_TEST_INCREASE = 50;
        private const Int32 SWEDISH_OCCURRENCE_PROBABLY_REGIONAL_EXTINCT = 7;
        private const Int32 SWEDISH_OCCURRENCE_REGIONAL_EXTINCT_OCCASIONALLY_OCCURRING = 8;
        private const Int32 SWEDISH_OCCURRENCE_REGIONAL_EXTINCT = 9;

        private RedListCalculator _redListCalculator;

        public RedListCalculatorTest()
        {
            _redListCalculator = null;
        }

        [TestMethod]
        public void Category()
        {
            Int64 populationSizeMin, populationSizeMax;

            GetRedListCalculator(true);
            for (populationSizeMin = 0; populationSizeMin < POPULATION_SIZE_TEST_MAX; populationSizeMin += POPULATION_SIZE_TEST_INCREASE)
            {
                for (populationSizeMax = populationSizeMin; populationSizeMax < POPULATION_SIZE_TEST_MAX; populationSizeMax += POPULATION_SIZE_TEST_INCREASE)
                {
                    GetRedListCalculator().SetPopulationSize(true,
                                                             populationSizeMin,
                                                             false,
                                                             0,
                                                             true,
                                                             populationSizeMax,
                                                             null);
                    if ((GetRedListCalculator().CategoryBestCaseGraded == RedListCategory.LC) &&
                         ((GetRedListCalculator().CategoryWorstCaseGraded == RedListCategory.CR) ||
                          (GetRedListCalculator().CategoryWorstCaseGraded == RedListCategory.RE)))
                    {
                        Assert.AreEqual(RedListCategory.DD, GetRedListCalculator().Category);
                    }
                    else
                    {
                        Assert.AreNotEqual(RedListCategory.DD, GetRedListCalculator().Category);
                    }
                }
            }
        }

        private RedListCalculator GetRedListCalculator(Boolean refresh = false)
        {
            if (_redListCalculator.IsNull() || refresh)
            {
                _redListCalculator = new RedListCalculator(GetUserContext(), null, null);
            }

            return _redListCalculator;
        }

        [TestMethod]
        public void IsConservationDependent()
        {
            // Test value.
            GetRedListCalculator(true).IsConservationDependent = false;
            Assert.IsFalse(GetRedListCalculator().IsConservationDependent);
            GetRedListCalculator().IsConservationDependent = true;
            Assert.IsTrue(GetRedListCalculator().IsConservationDependent);

            // Test category.
            GetRedListCalculator(true).IsConservationDependent = false;
            GetRedListCalculator().IsEvaluationStatusSet = true;
            Assert.AreEqual(RedListCategory.LC, GetRedListCalculator().Category);
            GetRedListCalculator().IsConservationDependent = true;
            Assert.AreEqual(RedListCategory.NT, GetRedListCalculator().Category);

            // Test criteria.
            GetRedListCalculator(true).IsConservationDependent = false;
            Assert.IsTrue(GetRedListCalculator().Criteria.IsEmpty());
            GetRedListCalculator().IsConservationDependent = true;
            Assert.IsTrue(GetRedListCalculator().Criteria.IsEmpty());
        }

        [TestMethod]
        public void IsRegionalExtinct()
        {
            Int32 swedishOccurrence;

            GetRedListCalculator(true);
            for (swedishOccurrence = SWEDISH_OCCURRENCE_PROBABLY_REGIONAL_EXTINCT;
                 swedishOccurrence <= SWEDISH_OCCURRENCE_REGIONAL_EXTINCT;
                 swedishOccurrence++)
            {
                GetRedListCalculator().SetSwedishOccurrence(swedishOccurrence);
                if ((SWEDISH_OCCURRENCE_REGIONAL_EXTINCT_OCCASIONALLY_OCCURRING <= swedishOccurrence) &&
                    (swedishOccurrence <= SWEDISH_OCCURRENCE_REGIONAL_EXTINCT))
                {
                    Assert.IsTrue(GetRedListCalculator().IsRegionalExtinct);
                }
                else
                {
                    Assert.IsFalse(GetRedListCalculator().IsRegionalExtinct);
                }
            }
        }
    }
}

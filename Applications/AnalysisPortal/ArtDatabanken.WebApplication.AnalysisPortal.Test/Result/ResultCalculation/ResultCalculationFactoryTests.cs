//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Threading;
//using AnalysisPortal.Tests;
//using ArtDatabanken.Data;
//using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
//using ArtDatabanken.WebApplication.AnalysisPortal.Managers.Authorization;
//using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataModels;
//using ArtDatabanken.WebApplication.AnalysisPortal.Result;
//using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation;
//using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
//using ArtDatabanken.WebApplication.AnalysisPortal.Test.TestModels;
//using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
//using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Account;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Rhino.Mocks;

//namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.Managers
//{
//    [TestClass]
//    public class ResultCalculationFactoryTests : TestBase
//    {
        
//        [TestMethod]
//        public void CreateResultCalculator_SpeciesObservationMapResultType_ReturnSpeciesObservationMapResultCalculator()
//        {
//            IResultCalculator resultCalculator;
//            SpeciesObservationMapResultCalculator speciesObservationMapResultCalculator;
            
//            resultCalculator = ResultCalculationFactory.CreateResultCalculator(ResultType.SpeciesObservationMap, SessionHandler.UserContext, SessionHandler.MySettings);
//            speciesObservationMapResultCalculator = (SpeciesObservationMapResultCalculator)resultCalculator;
//            Assert.IsNotNull(speciesObservationMapResultCalculator);            
//        }

//        [TestMethod]
//        public void CreateResultCalculator_AllResultTypes_ReturnResultCalculatorForEveryResultType()
//        {
//            IResultCalculator resultCalculator;
            
//            foreach (ResultType resultType in Enum.GetValues(typeof(ResultType)))
//            {
//                resultCalculator = ResultCalculationFactory.CreateResultCalculator(resultType, SessionHandler.UserContext, SessionHandler.MySettings);
//                Assert.IsNotNull(resultCalculator);
//            }            
//        }
      

//    }
//}

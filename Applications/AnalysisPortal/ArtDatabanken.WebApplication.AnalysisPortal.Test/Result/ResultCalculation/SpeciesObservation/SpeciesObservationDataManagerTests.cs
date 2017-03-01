using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.Data.DataSource.Fakes;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.Test.TestModels;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Details;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Observations;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.ViewResult;
using ArtDatabanken.WebService.Client.SpeciesObservationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.Result.ResultCalculation.SpeciesObservation
{
    [TestClass]
    public class SpeciesObservationDataManagerTests : TestBase
    {
        private const string projectParameterIdentifer1 = "MyPropIdentifier1";
        //[TestMethod]
        ////[TestCategory("NightlyTestApp")]
        //public void GetObservationDetailFields_ObservationWithProjectParameters_ProjectIsPopulatedInModel()
        //{
        //    // Arrange
        //    LoginApplicationUser();            
        //    SpeciesObservationDataManager speciesObservationDataManager = new SpeciesObservationDataManager(SessionHandler.UserContext, SessionHandler.MySettings);
        //    //SessionHandler.MySettings.Filter.Taxa.AddTaxonId(5);
            
        //    // Act
        //    var observationDetailFieldViewModels = speciesObservationDataManager.GetObservationDetailFields(150);

        //    // Assert
        //    Assert.IsNotNull(observationDetailFieldViewModels);
        //}

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void GetSpeciesObservationViewModel_ObservationWithProjectParameters_ProjectIsPopulatedInModel()
        {
            // Constants
            const string projectName = "My project";
            const int observationId = 150;
            const string projectParameterName1 = "Degrees";
            const string projectParameterValue1 = "25";
            const string projectParamterPropertyIdentifier1 = "MyPropIdentifier1";

            // Arrange
            LoginApplicationUser();
            SpeciesObservationDataManager speciesObservationDataManager = new SpeciesObservationDataManager(SessionHandler.UserContext, SessionHandler.MySettings);

            // Arrange - Mock
            CoreData.SpeciesObservationManager.DataSource =
                new StubISpeciesObservationDataSource()
                {
                    GetSpeciesObservationsIUserContextListOfInt64ICoordinateSystemISpeciesObservationSpecification =
                        (userContext, speciesObservationIds, coordinateSystem, speciesObservationSpecification) =>
                        {
                            SpeciesObservationList speciesObservationList = new SpeciesObservationList();
                            ISpeciesObservation speciesObservation = new ArtDatabanken.Data.SpeciesObservation();
                            speciesObservation.Id = observationId;
                            
                            speciesObservation.Project = new SpeciesObservationProject
                            {
                                ProjectName = projectName,
                                ProjectParameters = new SpeciesObservationProjectParameterList
                                {
                                    new SpeciesObservationProjectParameter
                                    {
                                        PropertyIdentifier = projectParamterPropertyIdentifier1,
                                        ProjectName = projectName,
                                        Property = projectParameterName1,
                                        Value = projectParameterValue1
                                    }
                                }
                            };

                            speciesObservationList.Add(speciesObservation);                            
                            return speciesObservationList;
                        }
                };

            // Act
            var speciesObservationViewModel = speciesObservationDataManager.GetSpeciesObservationViewModel(observationId);

            // Assert
            Assert.AreEqual(1, speciesObservationViewModel.Projects.Count);
            Assert.AreEqual(projectName, speciesObservationViewModel.Projects[0].Name);
            Assert.AreEqual(projectParameterName1, speciesObservationViewModel.Projects[0].ProjectParameters[projectParamterPropertyIdentifier1].Name);
            Assert.AreEqual(projectParameterValue1, speciesObservationViewModel.Projects[0].ProjectParameters[projectParamterPropertyIdentifier1].Value);
        }

        [TestMethod]
        public void GetObservationsListDictionary_WhenOneProjectParameter_ThenProjectAndProjectParameterArePairedTogether()
        {
            // Constants
            const string projectName = "My project";
            const int observationId = 150;
            const string projectParameterName1 = "Degrees";
            const string projectParameterValue1 = "25";

            //Arrange
            LoginApplicationUser();
            SpeciesObservationDataManager speciesObservationDataManager = new SpeciesObservationDataManager(
                SessionHandler.UserContext, 
                SessionHandler.MySettings);
            List<SpeciesObservationViewModel> obsResultList = new List<SpeciesObservationViewModel>();
            IEnumerable<ViewTableField> tableFields = null;
            // Arrange - Mock
            CoreData.SpeciesObservationManager.DataSource =
                new StubISpeciesObservationDataSource()
                {
                    GetSpeciesObservationsIUserContextListOfInt64ICoordinateSystemISpeciesObservationSpecification =
                        (userContext, speciesObservationIds, coordinateSystem, speciesObservationSpecification) =>
                        {
                            SpeciesObservationList speciesObservationList = new SpeciesObservationList();
                            ISpeciesObservation speciesObservation = new ArtDatabanken.Data.SpeciesObservation();
                            speciesObservation.Id = observationId;
                            speciesObservation.Project = new SpeciesObservationProject
                            {
                                ProjectName = projectName,
                                ProjectParameters = new SpeciesObservationProjectParameterList
                                {
                                    new SpeciesObservationProjectParameter
                                    {
                                        ProjectName = projectName,
                                        Property = projectParameterName1,
                                        Value = projectParameterValue1
                                    }
                                }
                            };

                            speciesObservationList.Add(speciesObservation);
                            return speciesObservationList;
                        }
                };

            var viewManager = new SpeciesObservationTableViewManager(
                SessionHandler.UserContext,
                SessionHandler.MySettings);
            ViewTableViewModel viewModel = viewManager.CreateViewTableViewModel(
                SessionHandler.MySettings.Presentation.Table.SpeciesObservationTable.SpeciesObservationTableColumnsSetId);
            tableFields = viewModel.TableFields;

            obsResultList.Add(new SpeciesObservationViewModel
            {
                Id = observationId.ToString(),
                Projects = new List<ProjectViewModel>
                {
                    new ProjectViewModel()
                    {
                        Name = projectName,
                        ProjectParameters = new Dictionary<string, ProjectParameterObservationDetailFieldViewModel>
                        {
                            {projectParameterIdentifer1, new ProjectParameterObservationDetailFieldViewModel()
                                {
                                    PropertyIdentifier = projectParameterIdentifer1,
                                    Name = projectParameterName1,
                                    Label = projectParameterName1,
                                    Value = projectParameterValue1,                                
                                }
                            }
                        }
                    }
                }
            });

            //Act
            List<Dictionary<ViewTableField, string>> result = speciesObservationDataManager.GetObservationsListDictionary(
                obsResultList,
                tableFields);


            //Assert
            bool foundProjectParameter = false;
            foreach (KeyValuePair<ViewTableField, string> pair in result.First())
            {
                string headerTitle = string.Format("[{0}].[{1}]", projectName, projectParameterName1);
                if (pair.Key.Title == headerTitle  && pair.Key.DataField == headerTitle && pair.Value == projectParameterValue1)
                {
                    foundProjectParameter = true;
                }
            }
            Assert.IsTrue(foundProjectParameter, "Project parameter not found");            
        }
    }

    //public static class MyAssertExtensions
    //{
    //    public static void AreEquivalent(this Assert ast,
    //        Dictionary<string, int> propsExpected,
    //        Dictionary<string, int> propsActual)
    //    {
    //        Assert.AreEqual(propsExpected.Count, propsActual.Count);
    //        foreach (var key in propsExpected.Keys)
    //        {
    //            Assert.IsNotNull(props[key]);
    //            Assert.AreEqual(propsExpected[key], props[key]);
    //        }
    //    }
    //}
}

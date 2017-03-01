using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.QueryComplexity;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.ViewResult;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Result
{
    // Class for handling view table data
    public class SpeciesObservationTaxonTableViewManager : ViewManagerBase
    {
        // TODO DELETe: Used for customize table
        //public PresentationTableSetting PresentationTableSetting
        //{
        //    get { return MySettings.Presentation.Table; }
        //}

        public SpeciesObservationTaxonTableViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        /// <summary>
        /// Returns the view model of the table data. 
        /// </summary>
        /// <returns></returns>
        /// TODO needed?
        public ViewTableViewModel GetViewTableViewModel()
        {
            ViewTableViewModel model = new ViewTableViewModel();
            return model;
        }

        public ViewTableViewModel CreateViewTableViewModel()
        {            
            ViewTableViewModel model = new ViewTableViewModel();
            SpeciesObservationTaxonTableResultCalculator resultCalculator = new SpeciesObservationTaxonTableResultCalculator(UserContext, MySettings);
            //model.ComplexityEstimate = resultCalculator.GetQueryComplexityEstimate();
            //model.ComplexityEstimate = QueryComplexityManager.GetQueryComplexityEstimate(ResultType.SpeciesObservationTaxonTable, UserContext, MySettings);

            List<ISpeciesObservationFieldDescription> fields = MySettings.Presentation.Table.SpeciesObservationTable.GetTableFields(UserContext);
            var tableFields = new List<ViewTableField>();
            foreach (ISpeciesObservationFieldDescription field in fields)
            {
                ViewTableField viewTableField = new ViewTableField(field.Label, field.Name.FirstLetterToUpper());
                tableFields.Add(viewTableField);
            }
            model.TableFields = tableFields;
            return model;
        }
    }
}

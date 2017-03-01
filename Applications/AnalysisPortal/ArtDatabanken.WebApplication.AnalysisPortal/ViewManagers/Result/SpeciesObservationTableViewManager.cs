using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.QueryComplexity;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.Table;
using ArtDatabanken.WebApplication.AnalysisPortal.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.ViewResult;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Result
{
    // Class for handling view table data
    public class SpeciesObservationTableViewManager : ViewManagerBase
    {
        public PresentationTableSetting PresentationTableSetting
        {
            get { return MySettings.Presentation.Table; }
        }

        public SpeciesObservationTableViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        /// <summary>
        /// Returns the view model of the table data. 
        /// </summary>
        /// <returns></returns>
        public ViewTableViewModel GetViewTableViewModel()
        {
            ViewTableViewModel model = new ViewTableViewModel();
            List<ISpeciesObservationFieldDescription> fields = PresentationTableSetting.SpeciesObservationTable.GetTableFields(UserContext);
            var tableFields = new List<ViewTableField>();
            foreach (ISpeciesObservationFieldDescription field in fields)
            {
                ViewTableField viewTableField = new ViewTableField(field.Label, field.Name);
                tableFields.Add(viewTableField);
            }
            model.TableFields = tableFields;
            return model;
        }

        /// <summary>
        /// Returns the view model of the Darwin Core table data.
        /// </summary>
        /// <returns></returns>
        public ViewDarwinCoreTableViewModel GetViewDarwinCoreTableViewModel()
        {
            ViewDarwinCoreTableViewModel model = new ViewDarwinCoreTableViewModel();
            return model;
        }

        public ViewTableViewModel CreateViewTableViewModel()
        {            
            ViewTableViewModel model = new ViewTableViewModel();
            PagedSpeciesObservationResultCalculator resultCalculator = new PagedSpeciesObservationResultCalculator(UserContext, MySettings);
            model.ComplexityEstimate = resultCalculator.GetQueryComplexityEstimate();            

            List<ISpeciesObservationFieldDescription> fields = PresentationTableSetting.SpeciesObservationTable.GetTableFields(UserContext);
            var tableFields = new List<ViewTableField>();
            foreach (ISpeciesObservationFieldDescription field in fields)
            {
                ViewTableField viewTableField = new ViewTableField(field.Label, field.Name.FirstLetterToUpper());
                tableFields.Add(viewTableField);
            }
            model.TableFields = tableFields;
            return model;
        }

        public ViewTableViewModel CreateViewTableViewModel(SpeciesObservationTableColumnsSetId speciesObservationTableColumnsSetId)
        {
            ViewTableViewModel model = new ViewTableViewModel();
            PagedSpeciesObservationResultCalculator resultCalculator = new PagedSpeciesObservationResultCalculator(UserContext, MySettings);
            model.ComplexityEstimate = resultCalculator.GetQueryComplexityEstimate();
            List<ISpeciesObservationFieldDescription> fields = 
                PresentationTableSetting.SpeciesObservationTable.GetTableFields(
                    UserContext,
                    speciesObservationTableColumnsSetId.TableId,
                    speciesObservationTableColumnsSetId.UseUserDefinedTableType);

            var tableFields = new List<ViewTableField>();
            foreach (ISpeciesObservationFieldDescription field in fields)
            {
                ViewTableField viewTableField = new ViewTableField(field.Label, field.Name.FirstLetterToUpper());
                tableFields.Add(viewTableField);
            }
            model.TableFields = tableFields;
            return model;
        }

        public ViewTableViewModel CreateViewTableViewModelByImportance(int? importance)
        {
            ViewTableViewModel model = new ViewTableViewModel();
            List<ISpeciesObservationFieldDescription> fields;
            if (importance.HasValue)
            {
                fields = PresentationTableSetting.SpeciesObservationTable.GetTableFieldsByImportance(UserContext, importance.Value);
            }
            else
            {
                fields = PresentationTableSetting.SpeciesObservationTable.GetTableFields(UserContext);
            }

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

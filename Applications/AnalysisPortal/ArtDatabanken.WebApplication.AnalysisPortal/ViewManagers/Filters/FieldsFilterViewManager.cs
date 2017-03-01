using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Table;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Field;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Fields;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Filters
{
    /// <summary>
    /// This class is a manager class for Filtering Field.
    /// </summary>
    public class FieldsFilterViewManager : ViewManagerBase
    {
        public FieldSetting FieldSetting
        {
            get { return MySettings.Filter.Field; }
        }

        public FieldsFilterViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        /// <summary>
        /// Creates an FieldsFromSearchViewModel.
        /// </summary>
        /// <returns></returns>
        public FieldViewModel CreateFieldViewModel()
        {
            var model = new FieldViewModel();

            model.FieldDescriptionTypes.FieldDescriptionTypes = GetSearchableFields();
            
            model.IsSettingsDefault = FieldSetting.IsSettingsDefault();

            return model;
        }

        /// <summary>
        /// Get only searchable fields.
        /// </summary>
        /// <returns>Dictionary that contains class relations to lists of ISpeciesObservationFieldDescription.</returns>
        private Dictionary<String, List<ISpeciesObservationFieldDescription>> GetSearchableFields()
        {
            IUserContext userContext = CoreData.UserManager.GetCurrentUser();
            var viewManager = new SpeciesObservationFieldDescriptionViewManager(userContext, SessionHandler.MySettings);
            SpeciesObservationFieldDescriptionsViewModel viewModel = viewManager.CreateSpeciesObservationFieldDescriptionsViewModel();
            var fieldDescriptions = viewModel.SearchableFieldDescriptionsByClass;

            return fieldDescriptions;
        }

        /// <summary>
        /// Add field search criteria to MySettings.
        /// </summary>
        /// <param name="speciesObservationFieldSearchCriteria"></param>
        public void AddFieldFilterExpressions(SpeciesObservationFieldSearchCriteria speciesObservationFieldSearchCriteria)
        {
            FieldSetting.AddFieldFilterExpressions(speciesObservationFieldSearchCriteria);
        }

        /// <summary>
        /// Set logical combine operator for field search criterias in MySettings.
        /// </summary>
        /// <param name="logicalOperator"></param>
        public void SetFieldFilterLogicalOperator(LogicalOperator logicalOperator)
        {
            FieldSetting.FieldLogicalOperator = logicalOperator;
        }

        /// <summary>
        /// Get list of field descriptions to be displayed in drop down list in web page.
        /// </summary>
        /// <param name="className">Name of class to filter the list.</param>
        /// <returns>List of field list item view models.</returns>
        public List<FieldListItemViewModel> GetFieldsViewModelByClassName(String className)
        {
            SpeciesObservationFieldDescriptionList fieldDescriptions;
            fieldDescriptions = SpeciesObservationFieldDescriptionViewManager.GetAllFieldsExceptProjectParameters(UserContext);
            List<FieldListItemViewModel> fieldViewList = new List<FieldListItemViewModel>();
            FieldListItemViewModel fieldView;

            var fields = fieldDescriptions.Where(f => f.IsSearchable);

            foreach (var field in fields)
            {
                // If class name is empty then get all fields
                if (String.IsNullOrWhiteSpace(className) || (field.Class.GetName().Equals(className) && !field.IsClass))
                {
                    fieldView = new FieldListItemViewModel { Id = field.Id, Name = field.Name };
                    fieldViewList.Add(fieldView);
                }
            }

            return fieldViewList;
        }

        /// <summary>
        /// Get field filter expression from session.
        /// </summary>
        /// <returns></returns>
        public String GetFieldFilterExpression()
        {
            return Utils.Converters.FieldFilterExpressionConverter.GetFieldFilterExpression(MySettings.Filter.Field.FieldFilterExpressions, MySettings.Filter.Field.FieldLogicalOperator);
        }

        /// <summary>
        /// Get field filter logical operator from session.
        /// </summary>
        /// <returns></returns>
        public String GetFieldFilterLogicalOperatorAsString()
        {
            return MySettings.Filter.Field.FieldLogicalOperator.ToString().ToUpper();
        }

        /// <summary>
        /// Get dictionary that contains field (key) associated to class (value).
        /// </summary>
        /// <returns>Dictionary of fields associated to classes.</returns>
        public Dictionary<int, string> GetFieldClassDictionary()
        {
            Dictionary<int, string> mapping = new Dictionary<int, string>();
            Dictionary<string, List<ISpeciesObservationFieldDescription>> dic = GetSearchableFields();
            foreach (var pair in dic)
            {
                foreach (var field in pair.Value)
                {
                    mapping.Add(field.Id, pair.Key);
                }
            }
            return mapping;
        }

        public DataType GetTypeByFieldAndClass(Int32 fieldId, String className)
        {
            Dictionary<string, List<ISpeciesObservationFieldDescription>> dic = GetSearchableFields();
            
            foreach (var pair in dic)
            {
                if (pair.Key == className)
                {
                    foreach (var field in pair.Value)
                    {
                        if (field.Id == fieldId)
                        {
                            return field.Type;
                        }
                    }
                }
            }

            throw new ArgumentException("Field data type is not defined.");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Table;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.Table;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Details;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Presentation;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Presentation
{
    /// <summary>
    /// This class is a view manager for GetSpeciesObservationFieldDescriptions
    /// </summary>
    public class SpeciesObservationFieldDescriptionViewManager : ViewManagerBase
    {        
        public PresentationTableSetting TableSettings
        {
            get { return MySettings.Presentation.Table; }
        }

        public SpeciesObservationFieldDescriptionViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        /// <summary>
        /// Gets all project parameters fields.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>List with all project parameter fields.</returns>
        public static SpeciesObservationFieldDescriptionList GetProjectParametersFields(IUserContext userContext)
        {
            SpeciesObservationFieldDescriptionList fields = CoreData.MetadataManager.GetSpeciesObservationFieldDescriptions(userContext);
            SpeciesObservationFieldDescriptionList allProjectParameterFields = new SpeciesObservationFieldDescriptionList();
            foreach (var field in fields)
            {
                if (IsFieldProjectParameter(field))
                {
                    allProjectParameterFields.Add(field);
                }
            }

            return allProjectParameterFields;
        }

        /// <summary>
        /// Gets all fields except project parameters.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>All fields except project parameters.</returns>
        public static SpeciesObservationFieldDescriptionList GetAllFieldsExceptProjectParameters(IUserContext userContext)
        {
            SpeciesObservationFieldDescriptionList fields = CoreData.MetadataManager.GetSpeciesObservationFieldDescriptions(userContext);
            SpeciesObservationFieldDescriptionList allFieldsExceptProjectParameters = new SpeciesObservationFieldDescriptionList();
            foreach (var field in fields)
            {
                if (!IsFieldProjectParameter(field))
                {
                    allFieldsExceptProjectParameters.Add(field);
                }                
            }

            return allFieldsExceptProjectParameters;
        }

        /// <summary>
        /// Determines whether the specified field is project parameter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>
        ///   <c>true</c> if the field is project parameter; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsFieldProjectParameter(ISpeciesObservationFieldDescription field)
        {
            if (field.Class.Id != SpeciesObservationClassId.Project)
            {
                return false;
            }

            if (field.Mappings.Any())
            {
                foreach (var mapping in field.Mappings)
                {
                    if (mapping.ProjectId.HasValue)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Creates a SpeciesObservationFieldDescriptionsViewModel.
        /// </summary>        
        public SpeciesObservationFieldDescriptionsViewModel CreateSpeciesObservationFieldDescriptionsViewModel()
        {            
            SpeciesObservationFieldDescriptionsViewModel model = new SpeciesObservationFieldDescriptionsViewModel();
            model.AllFieldDescriptions = new List<ISpeciesObservationFieldDescription>();
            SpeciesObservationFieldDescriptionList fields;
            SpeciesObservationFieldDescriptionList projectParameterFields;
            List<ISpeciesObservationFieldDescription> allFieldsSorted;
            fields = GetAllFieldsExceptProjectParameters(UserContext);
            projectParameterFields = GetProjectParametersFields(UserContext);
            allFieldsSorted = (List<ISpeciesObservationFieldDescription>)fields.GetGenericList();
            allFieldsSorted = allFieldsSorted.OrderBy(x => x.SortOrder).ToList();
            model.AllFieldDescriptions = allFieldsSorted;
            model.ProjectsDictionary = GroupProjectParametersByProjectDictionary(projectParameterFields);
            return model;
        }

        /// <summary>
        /// Groups the project parameters by project dictionary.
        /// </summary>
        /// <param name="projectParameterFields">The project parameter fields.</param>
        /// <returns>Dictionary with projects grouped by project id.</returns>
        private Dictionary<int, ProjectViewModel> GroupProjectParametersByProjectDictionary(
            SpeciesObservationFieldDescriptionList projectParameterFields)
        {
            Dictionary<int, ProjectViewModel> projectsDictionary = new Dictionary<int, ProjectViewModel>();

            if (projectParameterFields == null)
            {
                return null;
            }

            var projectParametersDictionary = new Dictionary<int, List<Tuple<ISpeciesObservationFieldDescription, ISpeciesObservationFieldMapping>>>();
            var projectSet = new HashSet<ProjectViewModel>();
            foreach (var projectParameter in projectParameterFields)
            {
                foreach (ISpeciesObservationFieldMapping mapping in projectParameter.Mappings)
                {
                    if (mapping.ProjectId.HasValue)
                    {
                        if (!projectParametersDictionary.ContainsKey(mapping.ProjectId.Value))
                        {
                            projectParametersDictionary.Add(mapping.ProjectId.Value, new List<Tuple<ISpeciesObservationFieldDescription, ISpeciesObservationFieldMapping>>());
                            ProjectViewModel project = new ProjectViewModel
                            {
                                Name = mapping.ProjectName,
                                Id = mapping.ProjectId.Value
                            };
                            projectSet.Add(project);
                        }
                        projectParametersDictionary[mapping.ProjectId.Value].Add(
                            new Tuple<ISpeciesObservationFieldDescription, ISpeciesObservationFieldMapping>(projectParameter, mapping));
                    }
                }
            }

            foreach (var projectViewModel in projectSet)
            {
                var projectParameterDescriptions = projectParametersDictionary[projectViewModel.Id];
                projectViewModel.ProjectParameters = new Dictionary<string, ProjectParameterObservationDetailFieldViewModel>(projectParameterDescriptions.Count);
                foreach (var fieldDescription in projectParameterDescriptions)
                {
                    var projectParameter = ProjectParameterObservationDetailFieldViewModel.Create(
                        fieldDescription.Item1, //ISpeciesObservationFieldDescription
                        fieldDescription.Item2); // ISpeciesObservationFieldMapping

                    projectViewModel.ProjectParameters.Add(projectParameter.PropertyIdentifier, projectParameter);
                }

                // Order project parameters by its label
                projectViewModel.ProjectParameters =
                    projectViewModel.ProjectParameters.OrderBy(x => x.Value.Label).ToDictionary(x => x.Key, x => x.Value);                

                projectsDictionary.Add(projectViewModel.Id, projectViewModel);
            }

            return projectsDictionary;
        }

        public void CreateNewSpeciesObservationTableType(SpeciesObservationTableTypeViewModel tableType)
        {
            UserDefinedTable userDefinedTable = new UserDefinedTable();
            userDefinedTable.Title = tableType.Title;
            userDefinedTable.FieldIds = tableType.FieldIds;
            TableSettings.SpeciesObservationTable.UserDefinedTables.Add(userDefinedTable);
            TableSettings.SpeciesObservationTable.UseUserDefinedTableType = true;
            TableSettings.SpeciesObservationTable.SelectedTableId = TableSettings.SpeciesObservationTable.UserDefinedTables.Count - 1;
        }

        /// <summary>
        /// Gets a table field description.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        public TableFieldDescriptionViewModel GetTableFieldDescription(int fieldId)
        {
            SpeciesObservationFieldDescriptionList fields = CoreData.MetadataManager.GetSpeciesObservationFieldDescriptions(UserContext);
            return (from ISpeciesObservationFieldDescription field in fields 
                    where field.Id == fieldId 
                    select TableFieldDescriptionViewModel.CreateFromSpeciesObservationFieldDescription(field)).FirstOrDefault();
        }

        /// <summary>
        /// Gets the table fields for a specific table.
        /// </summary>
        /// <param name="tableId">The table id.</param>
        /// <param name="useUserDefinedTable">if true the tableId refers to a user defined table.</param>        
        public List<TableFieldDescriptionViewModel> GetTableFields(int tableId, bool useUserDefinedTable)
        {            
            List<ISpeciesObservationFieldDescription> tableFields = TableSettings.SpeciesObservationTable.GetTableFields(UserContext, tableId, useUserDefinedTable);
            List<TableFieldDescriptionViewModel> list = new List<TableFieldDescriptionViewModel>();            
            foreach (ISpeciesObservationFieldDescription fieldDescription in tableFields)
            {                
                list.Add(TableFieldDescriptionViewModel.CreateFromSpeciesObservationFieldDescription(fieldDescription));                
            }
            return list;
        }

        public List<TableFieldDescriptionViewModel> GetAllSelectableTableFields()
        {
            const int LeastImportance = 5;
            List<ISpeciesObservationFieldDescription> tableFields = TableSettings.SpeciesObservationTable.GetTableFieldsByImportance(UserContext, LeastImportance);
            List<TableFieldDescriptionViewModel> list = new List<TableFieldDescriptionViewModel>();
            foreach (ISpeciesObservationFieldDescription fieldDescription in tableFields)
            {
                list.Add(TableFieldDescriptionViewModel.CreateFromSpeciesObservationFieldDescription(fieldDescription));
            }

            return list;
        }

        public void CreateNewSpeciesObservationTableType(TableTypeViewModel tableType)
        {
            throw new NotImplementedException();
        }

        public void DeleteUserDefinedTableType(int id)
        {
            TableSettings.SpeciesObservationTable.UserDefinedTables.RemoveAt(id);

            // Set new selected Id
            if (TableSettings.SpeciesObservationTable.UseUserDefinedTableType)
            {
                if (TableSettings.SpeciesObservationTable.UserDefinedTables.Count == 0)
                {
                    TableSettings.SpeciesObservationTable.SelectedTableId = 0;
                    TableSettings.SpeciesObservationTable.UseUserDefinedTableType = false;
                }
                else if (TableSettings.SpeciesObservationTable.SelectedTableId >= TableSettings.SpeciesObservationTable.UserDefinedTables.Count)
                {
                    TableSettings.SpeciesObservationTable.SelectedTableId--;
                }
            }
        }

        public void EditUserDefinedTableType(int id, SpeciesObservationTableTypeViewModel table)
        {
            TableSettings.SpeciesObservationTable.UserDefinedTables[id].Title = table.Title;
            TableSettings.SpeciesObservationTable.UserDefinedTables[id].FieldIds = table.FieldIds;
            TableSettings.SpeciesObservationTable.SelectedTableId = id;
        }

        public SpeciesObservationTableTypeViewModel GetUserDefinedTable(int id)
        {
            SpeciesObservationTableTypeViewModel table = new SpeciesObservationTableTypeViewModel();
            table.Id = id;
            table.Title = TableSettings.SpeciesObservationTable.UserDefinedTables[id].Title;
            table.FieldIds = TableSettings.SpeciesObservationTable.UserDefinedTables[id].FieldIds;

            return table;
        }
    }
}

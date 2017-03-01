using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Details;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Observations;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Table;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.ViewResult;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation
{
    public class SpeciesObservationDataManager
    {
        protected IUserContext userContext;
        protected MySettings.MySettings mySettings;

        public SpeciesObservationDataManager(IUserContext userContext, MySettings.MySettings mySettings)
        {
            this.userContext = userContext;
            this.mySettings = mySettings;
        }

        public List<ObservationDetailFieldViewModel> GetObservationDetailFields(int observationId)
        {
            SpeciesObservationViewModel speciesObservationViewModel = GetSpeciesObservationViewModel(observationId);
            if (speciesObservationViewModel != null)
            {
                return GetObservationDetailFields(speciesObservationViewModel);
            }

            return null;
        }

        public Dictionary<string, string> GetObservationData(int observationId)
        {
            SpeciesObservationViewModel speciesObservationViewModel = GetSpeciesObservationViewModel(observationId);
            if (speciesObservationViewModel != null)
            {
                return GetObservationData(speciesObservationViewModel);
            }

            return null;
        }

        public Dictionary<string, string> GetObservationData(int observationId, int? importance, bool? showDwcTitle, bool? hideEmptyFields)
        {
            SpeciesObservationViewModel speciesObservationViewModel = GetSpeciesObservationViewModel(observationId);

            if (speciesObservationViewModel != null)
            {
                var viewManager = new SpeciesObservationTableViewManager(userContext, mySettings);
                ViewTableViewModel viewModel = viewManager.CreateViewTableViewModelByImportance(importance);
                bool useLabelAsKey = mySettings.Presentation.Table.SpeciesObservationTable.UseLabelAsColumnHeader;
                if (showDwcTitle.HasValue)
                {
                    useLabelAsKey = !showDwcTitle.Value;
                }

                Dictionary<string, string> dicResult = GetObservationsKeyValueDictionary(speciesObservationViewModel, viewModel.TableFields, useLabelAsKey, !hideEmptyFields.GetValueOrDefault(false));
                
                return dicResult;
                //return GetObservationData(speciesObservationViewModel);
            }
            return null;
        }

        /// <summary>
        /// Gets the species observation view model.
        /// </summary>
        /// <param name="observationId">The observation id.</param>
        /// <returns>A species observation view model.</returns>
        public SpeciesObservationViewModel GetSpeciesObservationViewModel(int observationId)
        {
            var displayCoordinateSystem = mySettings.Presentation.Map.DisplayCoordinateSystem;
            List<long> speciesObservationIds = new List<long> { observationId };
            SpeciesObservationList speciesObservationList = CoreData.SpeciesObservationManager.GetSpeciesObservations(userContext, speciesObservationIds, displayCoordinateSystem);
            SpeciesObservationFieldDescriptionViewManager fieldDescriptionViewManager = new SpeciesObservationFieldDescriptionViewManager(userContext, mySettings);
            SpeciesObservationFieldDescriptionsViewModel fieldDescriptionsViewModel = fieldDescriptionViewManager.CreateSpeciesObservationFieldDescriptionsViewModel();
            if (speciesObservationList.Count > 0)
            {
                ISpeciesObservation speciesObservation = speciesObservationList[0];
                SpeciesObservationViewModel speciesObservationViewModel =
                    SpeciesObservationViewModel.CreateFromSpeciesObservation(speciesObservation, fieldDescriptionsViewModel);
                return speciesObservationViewModel;
            }

            return null;
        }

        /// <summary>
        /// Gets the observation detail fields.
        /// </summary>
        /// <param name="speciesObservationViewModel">The species observation view model.</param>
        /// <returns>A list of observation columns</returns>
        public List<ObservationDetailFieldViewModel> GetObservationDetailFields(
            SpeciesObservationViewModel speciesObservationViewModel)
        {
            List<ISpeciesObservationFieldDescription> speciesObservationFieldDescriptions = GetAllTableFields();
            List<ObservationDetailFieldViewModel> fields = new List<ObservationDetailFieldViewModel>(speciesObservationFieldDescriptions.Count);
            Dictionary<string, ObservationDetailFieldViewModel> dicFields = new Dictionary<string, ObservationDetailFieldViewModel>(speciesObservationFieldDescriptions.Count);

            foreach (ISpeciesObservationFieldDescription fieldDescription in speciesObservationFieldDescriptions)
            {
                ObservationDetailFieldViewModel field = ObservationDetailFieldViewModel.CreateFromSpeciesObservationFieldDescription(fieldDescription);
                // Remove duplicates. Todo change this.
                if (!dicFields.ContainsKey(field.Name.ToLower()))
                {
                    dicFields.Add(field.Name.ToLower(), field);
                    fields.Add(field);
                }                
            }

            Dictionary<string, string> observationValues = GetAllFieldsObservationsKeyValueDictionary(speciesObservationViewModel);            

            foreach (KeyValuePair<string, string> keyValuePair in observationValues)
            {
                ObservationDetailFieldViewModel field;
                if (dicFields.TryGetValue(keyValuePair.Key.ToLower(), out field))
                {
                    field.Value = keyValuePair.Value;
                }
            }
            return fields;
        }        

        private List<ISpeciesObservationFieldDescription> GetAllTableFields()
        {
            SpeciesObservationFieldDescriptionList fields;
            var fieldList = new List<ISpeciesObservationFieldDescription>();            
            fields = SpeciesObservationFieldDescriptionViewManager.GetAllFieldsExceptProjectParameters(userContext);
            foreach (ISpeciesObservationFieldDescription field in fields)
            {
                // class name fields are just headers
                if (field.IsClass)
                {
                    continue;
                }

                fieldList.Add(field);
            }                
            
            fieldList = fieldList.OrderBy(x => x.SortOrder).ToList();
            return fieldList;
        }

        private Dictionary<string, string> GetObservationData(
          SpeciesObservationViewModel speciesObservationViewModel)
        {
            if (speciesObservationViewModel == null)
            {
                return null;
            }

            var viewManager = new SpeciesObservationTableViewManager(userContext, mySettings);
            ViewTableViewModel viewModel = viewManager.CreateViewTableViewModel();
            bool useLabelAsKey = mySettings.Presentation.Table.SpeciesObservationTable.UseLabelAsColumnHeader;
            Dictionary<string, string> dicResult = GetObservationsKeyValueDictionary(speciesObservationViewModel, viewModel.TableFields, useLabelAsKey);
            return dicResult;
        }

        /// <summary>
        /// Converts a list with observations result to a dictionary
        /// where only the properties that is in the table fields list is used.        
        /// </summary>
        /// <param name="obsResultList">The obsservations.</param>
        /// <param name="tableFields">The table fields.</param>        
        public List<Dictionary<string, string>> GetObservationsDataList(IEnumerable<SpeciesObservationViewModel> obsResultList, IEnumerable<ViewTableField> tableFields)
        {
            var result = new List<Dictionary<string, string>>();
            List<PropertyInfo> properties = GetObservationViewModelPropertyInfos(tableFields);

            foreach (SpeciesObservationViewModel observation in obsResultList)
            {
                var dic = new Dictionary<string, string>();
                foreach (PropertyInfo propertyInfo in properties)
                {
                    object propValue = propertyInfo.GetValue(observation, null);
                    dic.Add(propertyInfo.Name, propValue == null ? "" : propValue.ToString());
                }
                if (!dic.ContainsKey("ObservationId"))
                {
                    dic.Add("ObservationId", observation.ObservationId);
                }

                if (observation.Projects != null)
                {
                    foreach (var projectViewModel in observation.Projects)
                    {
                        if (projectViewModel.ProjectParameters != null)
                        {
                            foreach (var projectParameter in projectViewModel.ProjectParameters.Values)
                            {
                                string headerTitle = string.Format("[{0}].[{1}]", projectViewModel.Name, projectParameter.Name);                                
                                dic.Add(headerTitle, projectParameter.Value);
                            }
                        }
                    }
                }

                result.Add(dic);
            }
            return result;
        }

        /// <summary>
        /// Converts a list with observations result to a dictionary
        /// where only the properties that is in the table fields list is used.        
        /// </summary>
        /// <param name="obsResultList">The observations.</param>
        /// <param name="tableFields">The table fields.</param>        
        public List<Dictionary<ViewTableField, string>> GetObservationsListDictionary(
            IEnumerable<SpeciesObservationViewModel> obsResultList, 
            IEnumerable<ViewTableField> tableFields)
        {
            var result = new List<Dictionary<ViewTableField, string>>();
            List<PropertyInfo> properties = GetObservationViewModelPropertyInfos(tableFields);
            Dictionary<string, ViewTableField> tableFieldsDictionary = new Dictionary<string, ViewTableField>();
            //SpeciesObservationFieldDescriptionViewManager fieldDescriptionViewManager = new SpeciesObservationFieldDescriptionViewManager(userContext, mySettings);
            //var fieldDescriptionsViewModel = fieldDescriptionViewManager.CreateSpeciesObservationFieldDescriptionsViewModel();
            foreach (var viewTableField in tableFields)
            {
                tableFieldsDictionary.Add(viewTableField.DataField, viewTableField);
            }
            bool addObservationId = false;
            ViewTableField observationViewTableField = null;
            if (tableFields.All(x => x.DataField != "ObservationId"))
            {
                addObservationId = true;
                observationViewTableField = new ViewTableField("ObservationId", "ObservationId");
            }

            foreach (SpeciesObservationViewModel observation in obsResultList)
            {
                var dic = new Dictionary<ViewTableField, string>();
                foreach (PropertyInfo propertyInfo in properties)
                {
                    object propValue = propertyInfo.GetValue(observation, null);
                    dic.Add(tableFieldsDictionary[propertyInfo.Name], propValue == null ? "" : propValue.ToString());
                }
                if (addObservationId)
                {                    
                    dic.Add(observationViewTableField, observation.ObservationId);
                }
                
                if (observation.Projects != null)
                {
                    foreach (var projectViewModel in observation.Projects)
                    {
                        if (projectViewModel.ProjectParameters != null)
                        {
                            foreach (var projectParameter in projectViewModel.ProjectParameters.Values)
                            {
                                string headerTitle = string.Format("[{0}].[{1}]", projectViewModel.Name, projectParameter.Name);
                                ViewTableField viewTableField = new ViewTableField(headerTitle, headerTitle);
                                dic.Add(viewTableField, projectParameter.Value);
                            }
                        }
                    }
                }

                result.Add(dic);
            }

            return result;
        }

        private Dictionary<string, string> GetAllFieldsObservationsKeyValueDictionary(
            SpeciesObservationViewModel observation)
        {
            List<PropertyInfo> properties = typeof(SpeciesObservationViewModel).GetProperties().ToList();
            var dic = new Dictionary<string, string>();
            foreach (PropertyInfo propertyInfo in properties)
            {
                object propValue = propertyInfo.GetValue(observation, null);
                string strKey = propertyInfo.Name;

                dic.Add(strKey, propValue == null ? "" : propValue.ToString());
            }
            return dic;
        }        

        private Dictionary<string, string> GetObservationsKeyValueDictionary(
            SpeciesObservationViewModel observation,
            List<ViewTableField> tableFields,
            bool useLabelAsKey,
            bool addEmptyFields = true)
        {
            Dictionary<string, ViewTableField> dicTableFields = new Dictionary<string, ViewTableField>();

            foreach (var tableField in tableFields)
            {
                if (!dicTableFields.ContainsKey(tableField.DataField))
                {
                    dicTableFields.Add(tableField.DataField, tableField);
                }
            }
            
            List<PropertyInfo> properties = GetObservationViewModelPropertyInfos(tableFields);
            var dic = new Dictionary<string, string>();
            foreach (PropertyInfo propertyInfo in properties)
            {
                object propValue = propertyInfo.GetValue(observation, null);
                string strKey = propertyInfo.Name;
                if (useLabelAsKey)
                {
                    ViewTableField tableField;
                    if (dicTableFields.TryGetValue(strKey, out tableField))
                    {
                        strKey = tableField.Title;
                    }
                }
                string val = propValue == null ? "" : propValue.ToString();
                if (addEmptyFields || !string.IsNullOrEmpty(val))
                {
                    if (!dic.ContainsKey(strKey))
                    {
                        dic.Add(strKey, val);
                    }
                }
            }
            return dic;
        }

        private List<PropertyInfo> GetObservationViewModelPropertyInfos(IEnumerable<ViewTableField> tableFields)
        {
            PropertyInfo[] modelPropInfos = typeof(SpeciesObservationViewModel).GetProperties();
            Dictionary<string, PropertyInfo> dicModelPropInfos = modelPropInfos.ToDictionary(propertyInfo => propertyInfo.Name);
            List<PropertyInfo> properties = GetFilteredPropertyInfos(dicModelPropInfos, tableFields);
            return properties;
        }

        /// <summary>
        /// Returns a list with PropertyInfos. Only the one that exist in
        /// the table fields list is used.
        /// </summary>
        /// <param name="dicProperties">A dictionary with PropertyInfo where PropertyInfo.Name is the key.</param>
        /// <param name="tableFields">The table fields.</param>
        /// <returns></returns>
        private List<PropertyInfo> GetFilteredPropertyInfos(Dictionary<string, PropertyInfo> dicProperties, IEnumerable<ViewTableField> tableFields)
        {
            var list = new List<PropertyInfo>();
            foreach (ViewTableField tableField in tableFields)
            {
                PropertyInfo propertyInfo;
                if (dicProperties.TryGetValue(tableField.DataField, out propertyInfo))
                {
                    list.Add(propertyInfo);
                }
            }
            return list;
        }

        /// <summary>
        /// Removes new lines chars in value string.
        /// </summary>
        /// <param name="fields">The fields.</param>
        public void RemoveNewLinesInValueString<T>(IEnumerable<T> fields) 
            where T : ObservationDetailFieldViewModel
        {
            if (fields == null)
            {
                return;
            }

            foreach (var field in fields)
            {
                if (field.Value != null)
                {
                    try
                    {
                        field.Value = RemoveNewLinesInString(field.Value);
                        //field.Value = Regex.Replace(field.Value, @"\s+", " ");                            
                    }
                    catch (Exception ex)
                    {
                        // ignored
                    }
                }
            }
        }

        /// <summary>
        /// Removes the new lines in string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>String where new line chars is removed.</returns>
        private string RemoveNewLinesInString(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            var sb = new StringBuilder(str.Length);

            foreach (char i in str)
            {
                if (i != '\n' && i != '\r' && i != '\t')
                {
                    sb.Append(i);
                }
            }

            return sb.ToString();
        }

    }
}
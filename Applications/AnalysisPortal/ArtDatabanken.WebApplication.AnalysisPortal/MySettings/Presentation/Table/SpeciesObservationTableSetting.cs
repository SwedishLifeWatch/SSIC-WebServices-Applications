using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Presentation;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.Table
{
    /// <summary>
    /// This class stores view settings
    /// </summary>
    [DataContract]
    public sealed class SpeciesObservationTableSetting : SettingBase
    {
        private List<UserDefinedTable> _userDefinedTables;

        /// <summary>
        /// Gets or sets whether PresentationViewSetting is active or not.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is checked; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsActive { get; set; }
        
        [DataMember]
        public bool UseUserDefinedTableType { get; set; }

        [DataMember]
        public int SelectedTableId { get; set; }

        [DataMember]
        public List<UserDefinedTable> UserDefinedTables
        {
            get
            {
                if (_userDefinedTables.IsNull())
                {
                    _userDefinedTables = new List<UserDefinedTable>();
                }
                return _userDefinedTables;
            }

            set
            {
                _userDefinedTables = value;
            }
        }

        [DataMember]
        public bool UseLabelAsColumnHeader { get; set; }

        /// <summary>
        /// Determines whether any settings has been done.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has settings; otherwise, <c>false</c>.
        ///   </returns>
        public override bool HasSettings
        {
            get { return false; }
        }

        public SpeciesObservationTableColumnsSetId SpeciesObservationTableColumnsSetId
        {
            get
            {
                return new SpeciesObservationTableColumnsSetId(UseUserDefinedTableType, SelectedTableId);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationTableSetting"/> class.
        /// </summary>
        public SpeciesObservationTableSetting()
        {            
            ResetSettings();
#if DEBUG
            //CreateSampleUserDefinedTables();
            //UseUserDefinedTableType = true;
            //SelectedTableId = 0;
#endif
        }

        public List<ISpeciesObservationFieldDescription> GetTableFields(IUserContext userContext)
        {
            return GetTableFields(userContext, SelectedTableId, UseUserDefinedTableType);            
        }

        public List<ISpeciesObservationFieldDescription> GetTableFields(IUserContext userContext, int tableId, bool useUserDefinedTable)
        {            
            if (useUserDefinedTable && tableId < UserDefinedTables.Count)
            {
                return GetTableFieldsByIds(userContext, UserDefinedTables[tableId].FieldIds);
            }
            return GetTableFieldsByImportance(userContext, tableId + 1);            
        }

        private List<ISpeciesObservationFieldDescription> GetTableFieldsByIds(IUserContext userContext, List<int> fieldIds)
        {
            var fieldList = new List<ISpeciesObservationFieldDescription>();
            SpeciesObservationFieldDescriptionList fields = CoreData.MetadataManager.GetSpeciesObservationFieldDescriptions(userContext);
            
            // create hash set for faster search in foreach loop below.
            var fieldIdsSet = new HashSet<int>(); 
            foreach (int fieldId in fieldIds)
            {
                fieldIdsSet.Add(fieldId);
            }

            foreach (ISpeciesObservationFieldDescription field in fields)
            {
                if (fieldIdsSet.Contains(field.Id))
                {
                    fieldList.Add(field);
                }
            }
            fieldList = fieldList.OrderBy(x => x.SortOrder).ToList();
            return fieldList;
        }

        /// <summary>
        /// Gets all table field ids that is lower or equal to importance.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="importance">The importance.</param>        
        public List<ISpeciesObservationFieldDescription> GetTableFieldsByImportance(IUserContext userContext, int importance)
        {
            var fieldList = new List<ISpeciesObservationFieldDescription>();
            SpeciesObservationFieldDescriptionList fields;
            fields = SpeciesObservationFieldDescriptionViewManager.GetAllFieldsExceptProjectParameters(userContext);
            foreach (ISpeciesObservationFieldDescription field in fields)
            {
                // class name fields are just headers
                if (field.IsClass)
                {
                    continue;
                }

                if (field.Importance <= importance)
                {
                    fieldList.Add(field);
                }
            }
            fieldList = fieldList.OrderBy(x => x.SortOrder).ToList();
            return fieldList;
        }

        private void CreateSampleUserDefinedTables()
        {
            UserDefinedTable table1 = new UserDefinedTable();
            table1.FieldIds = new List<int> { 2, 3, 49 };
            table1.Title = Resources.Resource.TableUserDefinedTitleMini;
            UserDefinedTables.Add(table1);

            UserDefinedTable table2 = new UserDefinedTable();
            table2.FieldIds = new List<int> { 2, 3, 4, 5, 6, 7, 8, 49 };
            table2.Title = Resources.Resource.TableUserDefinedTitleMedium;
            UserDefinedTables.Add(table2);

            //SelectedTableId = 1;
            //UseUserDefinedTableType = true;
            //UseLabelAsColumnHeader = false;
        }

        public void ResetSettings()
        {
            UseLabelAsColumnHeader = true;
            SelectedTableId = 0;
            UseUserDefinedTableType = false;
            UserDefinedTables = new List<UserDefinedTable>();
        }

        public override bool IsSettingsDefault()
        {
            if (UseLabelAsColumnHeader == true
                && SelectedTableId == 0
                && UseUserDefinedTableType == false
                && UserDefinedTables.Count == 0)
            {
                return true;
            }
            return false;
        }

        public string GetSelectedObservationTableName()
        {
            if (UseUserDefinedTableType)
            {
                return UserDefinedTables[SelectedTableId].Title;
            }
            switch (SelectedTableId)
            {
                case 0:
                    return Resources.Resource.TableTypeByImportance1;
                case 1:
                    return Resources.Resource.TableTypeByImportance2;
                case 2:
                    return Resources.Resource.TableTypeByImportance3;
                case 3:
                    return Resources.Resource.TableTypeByImportance4;
                case 4:
                    return Resources.Resource.TableTypeByImportance5;
                default:
                    return "-";
            }
        }

        public List<TableTypeViewModel> SystemDefinedTables
        {
            get
            {
                if (_systemDefinedTables == null)
                {
                    _systemDefinedTables = new List<TableTypeViewModel>
                    {
                        new TableTypeViewModel(0, Resources.Resource.TableTypeByImportance1),
                        new TableTypeViewModel(1, Resources.Resource.TableTypeByImportance2),
                        new TableTypeViewModel(2, Resources.Resource.TableTypeByImportance3),
                        new TableTypeViewModel(3, Resources.Resource.TableTypeByImportance4),
                        new TableTypeViewModel(4, Resources.Resource.TableTypeByImportance5)
                    };
                }

                return _systemDefinedTables;                
            }
        }
        private List<TableTypeViewModel> _systemDefinedTables = null;
    }
}
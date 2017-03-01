using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Details;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Table
{
    public interface ISpeciesObservationFieldDescriptionsViewModel
    {
        /// <summary>
        /// All field descriptions sorted by sort order
        /// </summary>        
        List<ISpeciesObservationFieldDescription> AllFieldDescriptions { get; set; }

        /// <summary>
        /// Gets the field descriptions by project name.
        /// </summary>        
        Dictionary<string, List<ISpeciesObservationFieldDescription>> FieldDescriptionsByProjectName { get; }

        /// <summary>
        /// Dictionary where key=importance, value=list of field descriptions with corresponding importance.
        /// </summary>        
        Dictionary<int, List<ISpeciesObservationFieldDescription>> FieldDescriptionsByImportance { get; }

        /// <summary>
        /// Dictionary where key=class, value=list of field descriptions with corresponding class.
        /// </summary>        
        Dictionary<string, List<ISpeciesObservationFieldDescription>> FieldDescriptionsByClass { get; }

        /// <summary>
        /// Dictionary where key=class, value=list of field descriptions with corresponding class.
        /// List is limited to searchable fields.
        /// </summary>        
        Dictionary<string, List<ISpeciesObservationFieldDescription>> SearchableFieldDescriptionsByClass { get; }

        /// <summary>
        /// All projects grouped by project id.
        /// </summary>        
        Dictionary<int, ProjectViewModel> ProjectsDictionary { get; set; }

            /// <summary>
        /// Gets the field descriptions according to the criteria.
        /// </summary>
        /// <param name="importance">Filter by importance.</param>
        /// <param name="className">Filter by class.</param>
        /// <param name="isImplemented">Filter by implemented.</param>
        /// <param name="isMandatory">Filter by mandatory.</param>
        /// <param name="isPlanned">Filter by planned.</param>        
        List<ISpeciesObservationFieldDescription> GetFieldDescriptions(
            int? importance, 
            string className,
            bool? isImplemented, 
            bool? isMandatory,
            bool? isPlanned);
    }

    /// <summary>
    /// This class is a view model for SpeciesObservationFieldDescriptions.
    /// </summary>
    public class SpeciesObservationFieldDescriptionsViewModel : ISpeciesObservationFieldDescriptionsViewModel
    {
        /// <summary>
        /// All field descriptions sorted by sort order
        /// </summary>        
        public List<ISpeciesObservationFieldDescription> AllFieldDescriptions { get; set; }

        /// <summary>
        /// Gets the field descriptions by project name.
        /// </summary>        
        public Dictionary<string, List<ISpeciesObservationFieldDescription>> FieldDescriptionsByProjectName
        {
            get
            {
                if (_fieldDescriptionsByProjectName == null)
                {
                    _fieldDescriptionsByProjectName = new Dictionary<string, List<ISpeciesObservationFieldDescription>>();
                    foreach (ISpeciesObservationFieldDescription fieldDescription in AllFieldDescriptions)
                    {
                        if (fieldDescription.Mappings.Any() && !string.IsNullOrEmpty(fieldDescription.Mappings[0].ProjectName))
                        {
                            if (!_fieldDescriptionsByProjectName.ContainsKey(fieldDescription.Mappings[0].ProjectName))
                            {
                                _fieldDescriptionsByProjectName.Add(fieldDescription.Mappings[0].ProjectName, new List<ISpeciesObservationFieldDescription>());
                            }                            

                            _fieldDescriptionsByProjectName[fieldDescription.Mappings[0].ProjectName].Add(fieldDescription);
                        }                        
                    }
                }
                return _fieldDescriptionsByProjectName;
            }
        }
        private Dictionary<string, List<ISpeciesObservationFieldDescription>> _fieldDescriptionsByProjectName;

        /// <summary>
        /// Dictionary where key=importance, value=list of field descriptions with corresponding importance.
        /// </summary>        
        public Dictionary<int, List<ISpeciesObservationFieldDescription>> FieldDescriptionsByImportance
        {
            get
            {
                if (_fieldDescriptionsByImportance == null)
                {
                    _fieldDescriptionsByImportance = new Dictionary<int, List<ISpeciesObservationFieldDescription>>();
                    foreach (ISpeciesObservationFieldDescription fieldDescription in AllFieldDescriptions)
                    {
                        if (!_fieldDescriptionsByImportance.ContainsKey(fieldDescription.Importance))
                        {
                            _fieldDescriptionsByImportance.Add(fieldDescription.Importance, new List<ISpeciesObservationFieldDescription>());
                        }

                        _fieldDescriptionsByImportance[fieldDescription.Importance].Add(fieldDescription);
                    }
                }
                return _fieldDescriptionsByImportance;
            }
        }
        private Dictionary<int, List<ISpeciesObservationFieldDescription>> _fieldDescriptionsByImportance;

        /// <summary>
        /// Dictionary where key=class, value=list of field descriptions with corresponding class.
        /// </summary>        
        public Dictionary<string, List<ISpeciesObservationFieldDescription>> FieldDescriptionsByClass
        {
            get
            {
                if (_fieldDescriptionsByClass == null)
                {
                    _fieldDescriptionsByClass = new Dictionary<string, List<ISpeciesObservationFieldDescription>>();
                    foreach (ISpeciesObservationFieldDescription fieldDescription in AllFieldDescriptions)
                    {
                        if (!_fieldDescriptionsByClass.ContainsKey(fieldDescription.Class.GetName()))
                        {
                            _fieldDescriptionsByClass.Add(fieldDescription.Class.GetName(), new List<ISpeciesObservationFieldDescription>());
                        }

                        _fieldDescriptionsByClass[fieldDescription.Class.GetName()].Add(fieldDescription);
                    }
                }
                return _fieldDescriptionsByClass;
            }
        }
        private Dictionary<string, List<ISpeciesObservationFieldDescription>> _fieldDescriptionsByClass;

        /// <summary>
        /// Dictionary where key=class, value=list of field descriptions with corresponding class.
        /// List is limited to searchable fields.
        /// </summary>        
        public Dictionary<string, List<ISpeciesObservationFieldDescription>> SearchableFieldDescriptionsByClass
        {
            get
            {
                if (_searchableFieldDescriptionsByClass == null)
                {
                    _searchableFieldDescriptionsByClass = new Dictionary<string, List<ISpeciesObservationFieldDescription>>();
                    foreach (ISpeciesObservationFieldDescription fieldDescription in AllFieldDescriptions)
                    {
                        if (fieldDescription.IsSearchable)
                        {
                            if (!_searchableFieldDescriptionsByClass.ContainsKey(fieldDescription.Class.GetName()))
                            {
                                _searchableFieldDescriptionsByClass.Add(
                                    fieldDescription.Class.GetName(),
                                    new List<ISpeciesObservationFieldDescription>());
                            }

                            _searchableFieldDescriptionsByClass[fieldDescription.Class.GetName()].Add(fieldDescription);
                        }
                    }
                }
                return _searchableFieldDescriptionsByClass;
            }
        }
        
        public Dictionary<int, ProjectViewModel> ProjectsDictionary { get; set; }

        private Dictionary<string, List<ISpeciesObservationFieldDescription>> _searchableFieldDescriptionsByClass;

        /// <summary>
        /// Gets the field descriptions according to the criteria.
        /// </summary>
        /// <param name="importance">Filter by importance.</param>
        /// <param name="className">Filter by class.</param>
        /// <param name="isImplemented">Filter by implemented.</param>
        /// <param name="isMandatory">Filter by mandatory.</param>
        /// <param name="isPlanned">Filter by planned.</param>        
        public List<ISpeciesObservationFieldDescription> GetFieldDescriptions(
            int? importance, 
            string className,
            bool? isImplemented, 
            bool? isMandatory,
            bool? isPlanned)
        {
            List<ISpeciesObservationFieldDescription> list = new List<ISpeciesObservationFieldDescription>();
            foreach (ISpeciesObservationFieldDescription description in AllFieldDescriptions)
            {
                if (importance.HasValue && importance.Value != description.Importance)
                {
                    continue;
                }

                if (className != null && className != description.Class.GetName())
                {
                    continue;
                }

                if (isImplemented.HasValue && isImplemented.Value != description.IsImplemented)
                {
                    continue;
                }

                if (isMandatory.HasValue && isMandatory.Value != description.IsMandatory)
                {
                    continue;
                }

                if (isPlanned.HasValue && isPlanned.Value != description.IsPlanned)
                {
                    continue;
                }

                list.Add(description);
            }
            return list;
        }
    }
}
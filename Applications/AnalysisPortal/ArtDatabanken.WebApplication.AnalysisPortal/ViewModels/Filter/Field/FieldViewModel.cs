using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Field
{
    public class FieldViewModel
    {
        private ModelLabels _labels;
        private SpeciesObservationFieldDescriptionType _fieldDescriptionTypes;

        /// <summary>
        /// Get the model labels.
        /// </summary>
        public ModelLabels Labels
        {
            get
            {
                if (_labels == null)
                {
                    _labels = new ModelLabels();
                }

                return _labels;
            }
        }

        public bool IsSettingsDefault { get; set; }

        /// <summary>
        /// Localized labels.
        /// </summary>
        public class ModelLabels
        {
            public string TitleLabel { get { return Resources.Resource.FilterFieldsTitle; } }
            public string FilterLable { get { return Resources.Resource.FilterFieldsHeader; } }
            public string ClassLabel { get { return Resources.Resource.FilterFieldsClass; } }
            public string AllClassesLabel { get { return Resources.Resource.FilterFieldsAllClassesSelection; } }
            public string LeftOperandLabel { get { return Resources.Resource.FilterFieldsPropertyLabel; } }
            public string OperatorLabel { get { return Resources.Resource.FilterFieldsCompareOperatorLabel; } }
            public string RightOperandLabel { get { return Resources.Resource.FilterFieldsGivenValueLabel; } }
            public string AndOperationLabel { get { return Resources.Resource.FilterFieldsAndProperty; } }
            public string OrOperationLabel { get { return Resources.Resource.FilterFieldsOrProperty; } }

            public string BeginsWithOperatorLabel { get { return Resources.Resource.StringCompareOperatorBeginsWith; } }
            public string ContainsOperatorLabel { get { return Resources.Resource.StringCompareOperatorContains; } }
            public string EndsWithOperatorLabel { get { return Resources.Resource.StringCompareOperatorEndsWith; } }
            public string EqualToOperatorLabel { get { return Resources.Resource.StringCompareOperatorEqual; } }
            public string GreaterLabel { get { return Resources.Resource.FilterFieldsCompareOperatorGreater; } }
            public string GreaterOrEqualLabel { get { return Resources.Resource.FilterFieldsCompareOperatorGreaterOrEqual; } }
            public string LessLabel { get { return Resources.Resource.FilterFieldsCompareOperatorLess; } }
            public string LessOrEqualLabel { get { return Resources.Resource.FilterFieldsCompareOperatorLessOrEqual; } }
            public string LikeOperatorLabel { get { return Resources.Resource.StringCompareOperatorLike; } }
            public string NotEqualToOperatorLabel { get { return Resources.Resource.StringCompareOperatorNotEqual; } }
        }

        /// <summary>
        /// Get the model SpeciesObservationFields.
        /// </summary>
        public SpeciesObservationFieldDescriptionType FieldDescriptionTypes
        {
            get
            {
                if (_fieldDescriptionTypes == null)
                {
                    _fieldDescriptionTypes = new SpeciesObservationFieldDescriptionType();
                }

                return _fieldDescriptionTypes;
            }
            set
            {
                _fieldDescriptionTypes = new SpeciesObservationFieldDescriptionType();
            }
        }

        /// <summary>
        /// Contains field description data.
        /// </summary>
        public class SpeciesObservationFieldDescriptionType
        {
            public Dictionary<String, List<ISpeciesObservationFieldDescription>> FieldDescriptionTypes { get; set; }

            public List<String> GetClasses()
            {
                if (FieldDescriptionTypes != null)
                {
                    return FieldDescriptionTypes.Keys.ToList();
                }

                return null;
            }
        }
    }
}
